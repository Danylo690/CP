using PCWPF.ViewModels;
using PCWPF.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace PCWPF.Models
{
    public class FTPClient : ViewModelBase, IFileManager
    {
        #region Fields
        private IPAddress _ipAddress;
        private int _port;
        private FtpWebRequest ftpRequest;
        private FtpWebResponse ftpResponse;
        private TcpClient client;
        private double _progress;
        private string _progressBarFileName;
        private bool _canProgressClose;
        private List<Log> logs;
        #endregion

        #region Properties
        public IPAddress IpAddress { get => _ipAddress; set => _ipAddress = value; }
        public int Port { get => _port; set => _port = value; }
        public double Progress { 
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public string ProgressBarFileName { 
            get => _progressBarFileName;
            set
            {
                _progressBarFileName = value;
                OnPropertyChanged(nameof(ProgressBarFileName));
            }
        }

        public bool CanProgressClose
        {
            get => _canProgressClose;
            set
            {
                _canProgressClose = value;
                OnPropertyChanged(nameof(CanProgressClose));
            }
        }
        #endregion

        #region Constructors
        public FTPClient()
        {
            client = new TcpClient();
        }
        #endregion

        #region Methods
        public bool Connect(string ip, string port)
        {
            try
            {
                bool isConnected;
                IPAddress outIpAddress;
                int outPort;
                if (string.IsNullOrEmpty(ip) ||
                    string.IsNullOrEmpty(port) ||
                    port.Length < 4)
                {
                    isConnected = false;
                }
                else
                {
                    IPAddress.TryParse(ip, out outIpAddress);
                    int.TryParse(port, out outPort);
                    IpAddress= outIpAddress;
                    Port = outPort;
                    client.Connect(IpAddress, Port);
                    isConnected = client.Connected;
                }
                return isConnected;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<FileInformation> List(string path)
        {
            var watch = new Stopwatch();
            Application.Current.Dispatcher.Invoke(() => {
                logs = (List<Log>)Thread.GetData(Thread.GetNamedDataSlot("Logs"));
            });
            string fileName = path.LastIndexOf('/') != -1 ? path.Substring((path.LastIndexOf('/') + 1), path.Length - path.LastIndexOf('/') - 1) : "Starter page";
            watch.Start();
            try
            {
                string address = $"ftp://{IpAddress}:{Port}{path}";
                ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                string content = "";

                StreamReader sr = new StreamReader(ftpResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                content = sr.ReadToEnd();
                
                List<FileInformation> files = new List<FileInformation>();
                string[] filesStrings = content.Split('\r','\n');
                filesStrings = filesStrings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                foreach (string fileString in filesStrings)
                {
                    List<string> fileInfoStrings = fileString.Split(' ').ToList();
                    fileInfoStrings = fileInfoStrings.Where(x => !string.IsNullOrEmpty(x)).ToList();

                    string permission = fileInfoStrings[0];
                    int size;
                    int.TryParse(fileInfoStrings[4], out size);
                    string date = String.Join(" ", fileInfoStrings.GetRange(5, 3).ToArray());
                    string name = String.Join(" ", fileInfoStrings.GetRange(8, fileInfoStrings.Count - 8).ToArray());
                    bool isFolder = permission[0] == 'd';
                    bool isFile = !isFolder;

                    FileInformation file = new FileInformation();
                    file.Permission = permission;
                    file.Name = name;
                    file.Size = size;
                    file.Date = date;
                    file.IsFile = isFile;
                    file.IsFolder = isFolder;
                    file.Path = $"{path}/{name}";
                    files.Add(file);
                }

                watch.Stop();
                logs.Insert(0,new Log()
                {
                    CommandName = $"Get list of <{fileName}>",
                    CommandResult = $"(SUCCESS)",
                    CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                });

                sr.Close();
                ftpResponse.Close();
                return files;
            }
            catch (Exception ex)
            {
                watch.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Get list of <{fileName}>",
                    CommandResult = $"(ERROR) {ex.Message}",
                    CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                });
                return null;
            } 
        }

        public void Download(List<FileInformation> filesToDownload, string path)
        {
            var watch2 = new Stopwatch();
            watch2.Start();
            Application.Current.Dispatcher.Invoke(() => {
                logs = (List<Log>)Thread.GetData(Thread.GetNamedDataSlot("Logs"));
            });
            string fileName = "";
            try
            {
                foreach (FileInformation fileToDownload in filesToDownload)
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    fileName = fileToDownload.Name;
                    Progress = 0;
                    if (fileToDownload.IsFolder)
                    {
                        ProgressBarFileName = $"Go to {fileToDownload.Name}";
                        string newFolderPath = $"{path}\\{fileToDownload.Name}";
                        newFolderPath = IsFolderExist(newFolderPath);
                        UpdateUI(50);
                        List<FileInformation> filesInFolder = List($"{fileToDownload.Path}");
                        UpdateUI(100);
                        Download(filesInFolder, newFolderPath);
                        continue;
                    }
                    ProgressBarFileName = $"{fileToDownload.Name} is downloading";
                    string address = $"ftp://{IpAddress}:{Port}{fileToDownload.Path}";
                    ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    FileStream file = new FileStream($"{path}\\{fileToDownload.Name}", FileMode.Create, FileAccess.ReadWrite);
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    Stream responseStream = ftpResponse.GetResponseStream();
                    byte[] buffer = new byte[1024];
                    int size;
                    double progressAdder = 100f / (fileToDownload.Size / 1024f);
                    while ((size = responseStream.Read(buffer, 0, 1024)) > 0)
                    {
                        file.Write(buffer, 0, size);
                        UpdateUI(Progress + progressAdder);
                    }
                    ftpResponse.Close();
                    file.Close();
                    responseStream.Close();
                    UpdateUI(100);

                    watch.Stop();
                    logs.Insert(0, new Log()
                    {
                        CommandName = $"Downloading <{fileName}>",
                        CommandResult = $"(SUCCESS)",
                        CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                    });
                }
            }
            catch (Exception ex)
            {
                watch2.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Downloading <{fileName}>",
                    CommandResult = $"(ERROR) {ex.Message}",
                    CommandTime = $"{(watch2.ElapsedMilliseconds / 1000f):0.00 s}"
                });
            }
        }

        public void Upload(List<FileInformation> filesToUpload, string path)
        {
            var watch2 = new Stopwatch();
            watch2.Start();
            Application.Current.Dispatcher.Invoke(() => {
                logs = (List<Log>)Thread.GetData(Thread.GetNamedDataSlot("Logs"));
            });
            string fileName = "";
            try
            {
                PCFileManager pCFileManager = (PCFileManager)Thread.GetData(Thread.GetNamedDataSlot("PCFileManager"));
                foreach (FileInformation fileToUpload in filesToUpload)
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    fileName = fileToUpload.Name;
                    UpdateUI(0);
                    if (fileToUpload.IsFolder)
                    {
                        ProgressBarFileName = $"Go to {fileToUpload.Name}";
                        string newFolderPath = $"{path}/{fileToUpload.Name}";
                        newFolderPath = IsAndroidFolderExist(newFolderPath);
                        UpdateUI(50);
                        List<FileInformation> filesInFolder = pCFileManager.List($"{fileToUpload.Path}");
                        UpdateUI(100);
                        Upload(filesInFolder, newFolderPath);
                        continue;
                    }
                    ProgressBarFileName = $"{fileToUpload.Name} is uploading";
                    UpdateUI(33);
                    FileStream file = new FileStream($"{fileToUpload.Path}", FileMode.Open, FileAccess.ReadWrite);
                    string address = $"ftp://{IpAddress}:{Port}{path}/{fileToUpload.Name}";
                    ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                    ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                    Stream writer = ftpRequest.GetRequestStream();
                    byte[] buffer = new byte[file.Length];
                    file.Read(buffer, 0, buffer.Length);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Progress = 66;
                    });
                    writer.Write(buffer, 0, buffer.Length);

                    file.Close();
                    writer.Close();
                    UpdateUI(100);

                    watch.Stop();
                    logs.Insert(0, new Log()
                    {
                        CommandName = $"Uploading <{fileName}>",
                        CommandResult = $"(SUCCESS)",
                        CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                    });
                }
                UpdateUI(100);
            }
            catch (Exception ex)
            {
                watch2.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Uploading <{fileName}>",
                    CommandResult = $"(ERROR) {ex.Message}",
                    CommandTime = $"{(watch2.ElapsedMilliseconds / 1000f):0.00 s}"
                });
            }
        }

        public string IsFolderExist(string newFolderPath, string oldFolderPath = "", int i = 2)
        {
            if (oldFolderPath == "")
            {
                oldFolderPath = newFolderPath;
            }
            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
                return newFolderPath;
            }
            else
            {
                return IsFolderExist(oldFolderPath + i, oldFolderPath, ++i);
            }
        }

        public string IsAndroidFolderExist(string newFolderPath, string oldFolderPath = "", int i = 2)
        {
            if (oldFolderPath == "")
            {
                oldFolderPath = newFolderPath;
            }
            List<FileInformation> filesInDir = List(newFolderPath);
            if (filesInDir == null)
            {
                Mkdir(newFolderPath);
                return newFolderPath;
            }
            else
            {
                return IsAndroidFolderExist(oldFolderPath + i, oldFolderPath, ++i);
            }
        }

        public void Mkdir(string path, string name = "")
        {
            var watch = new Stopwatch();
            Application.Current.Dispatcher.Invoke(() => {
                logs = (List<Log>)Thread.GetData(Thread.GetNamedDataSlot("Logs"));
            });
            string fileName = name == "" ? path.Substring((path.LastIndexOf('/') + 1), path.Length - path.LastIndexOf('/') - 1) : name;
            watch.Start();
            try
            {
                name = name == "" ? name : $"/{name}";
                string address = $"ftp://{IpAddress}:{Port}{path}{name}";
                ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();

                watch.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Create folder <{fileName}>",
                    CommandResult = $"(SUCCESS)",
                    CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                });
            }
            catch (Exception ex)
            {
                watch.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Create folder <{fileName}>",
                    CommandResult = $"(ERROR) {ex.Message}",
                    CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                });
            }
        }

        public string GetFileManagerMode()
        {
            return "Android";
        }

        public void Rename(FileInformation renamedFile, string newName)
        {
            var watch = new Stopwatch();
            Application.Current.Dispatcher.Invoke(() => {
                logs = (List<Log>)Thread.GetData(Thread.GetNamedDataSlot("Logs"));
            });
            string fileName = renamedFile.Name;
            watch.Start();
            try
            {
                string address = $"ftp://{IpAddress}:{Port}{renamedFile.Path}";
                ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpRequest.RenameTo = newName;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();

                watch.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Rename file/folder <{fileName}>",
                    CommandResult = $"(SUCCESS)",
                    CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                });
            }
            catch (Exception ex)
            {
                watch.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Rename file/folder <{fileName}>",
                    CommandResult = $"(ERROR) {ex.Message}",
                    CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                });
            }
        }

        public void Delete(List<FileInformation> deletedFiles)
        {
            var watch = new Stopwatch();
            Application.Current.Dispatcher.Invoke(() => {
                logs = (List<Log>)Thread.GetData(Thread.GetNamedDataSlot("Logs"));
            });
            string fileName = "";
            watch.Start();
            try
            {
                foreach (FileInformation deletedFile in deletedFiles)
                {
                    fileName = deletedFile.Name;
                    if (deletedFile.IsFolder)
                    {
                        List<FileInformation> filesInFolder = List($"{deletedFile.Path}");
                        Delete(filesInFolder);
                        string address = $"ftp://{IpAddress}:{Port}{deletedFile.Path}";
                        ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                        ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                    }
                    if (deletedFile.IsFile)
                    {
                        string address = $"ftp://{IpAddress}:{Port}{deletedFile.Path}";
                        ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                        ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                    }
                    ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ftpResponse.Close();

                    watch.Stop();
                    logs.Insert(0, new Log()
                    {
                        CommandName = $"Delete file/folder <{fileName}>",
                        CommandResult = $"(SUCCESS)",
                        CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                    });
                }
            }
            catch (Exception ex)
            {
                watch.Stop();
                logs.Insert(0, new Log()
                {
                    CommandName = $"Delete file/folder <{fileName}>",
                    CommandResult = $"(ERROR) {ex.Message}",
                    CommandTime = $"{(watch.ElapsedMilliseconds / 1000f):0.00 s}"
                });
            }
        }

        private void UpdateUI(double progress)
        {
            Application.Current.Dispatcher.Invoke(() => {
                Progress = progress;
            });
        }
        #endregion
    }
}
