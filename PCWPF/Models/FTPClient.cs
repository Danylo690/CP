using PCWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace PCWPF.Models
{
    public class FTPClient
    {
        #region Fields
        private IPAddress _ipAddress;
        private int _port;
        private FtpWebRequest ftpRequest;
        private FtpWebResponse ftpResponse;
        private TcpClient client;
        #endregion

        #region Properties
        public IPAddress IpAddress { get => _ipAddress; set => _ipAddress = value; }
        public int Port { get => _port; set => _port = value; }
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
            try
            {
                string address = $"ftp://{IpAddress}:{Port}{path}";
                ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                string content = "";

                StreamReader sr = new StreamReader(ftpResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                content = sr.ReadToEnd();
                sr.Close();
                ftpResponse.Close();
                
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
                    ;

                    FileInformation file = new FileInformation();
                    file.Permission = permission;
                    file.Name = name;
                    file.Size = size;
                    file.Date = date;
                    file.IsFile = isFile;
                    file.IsFolder = isFolder;
                    file.Path = $"{path}/";
                    files.Add(file);
                }
                return files;
            }
            catch (Exception ex)
            {
                return null;
            } 
        }

        public void Download(List<FileInformation> filesToDownload, string path)
        {
            foreach (FileInformation fileToDownload in filesToDownload)
            {
                if (fileToDownload.IsFolder)
                {
                    string newFolderPath = $"{path}\\{fileToDownload.Name}";
                    newFolderPath = IsFolderExist(newFolderPath);
                    List<FileInformation> filesInFolder = List($"{fileToDownload.Path}/{fileToDownload.Name}");
                    Download(filesInFolder, newFolderPath);
                    continue;
                }
                string address = $"ftp://{IpAddress}:{Port}{fileToDownload.Path}{fileToDownload.Name}";
                ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                FileStream file = new FileStream($"{path}\\{fileToDownload.Name}", FileMode.Create, FileAccess.ReadWrite);
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                Stream responseStream = ftpResponse.GetResponseStream();
                byte[] buffer = new byte[1024];
                int size = 0;
                while ((size = responseStream.Read(buffer, 0, 1024)) > 0)
                {
                    file.Write(buffer, 0, size);
                }
                ftpResponse.Close();
                file.Close();
                responseStream.Close();
            }
        }

        public void Upload(List<FileInformation> filesToUpload, string path)
        {
            PCFileManager pCFileManager = (PCFileManager)Thread.GetData(Thread.GetNamedDataSlot("PCFileManager"));
            foreach (FileInformation fileToUpload in filesToUpload)
            {
                if (fileToUpload.IsFolder)
                {
                    string newFolderPath = $"{path}/{fileToUpload.Name}";
                    newFolderPath = IsAndroidFolderExist(newFolderPath);
                    List<FileInformation> filesInFolder = pCFileManager.List($"{fileToUpload.Path}");
                    Upload(filesInFolder, newFolderPath);
                    continue;
                }
                FileStream file = new FileStream($"{fileToUpload.Path}", FileMode.Open, FileAccess.ReadWrite);
                string address = $"ftp://{IpAddress}:{Port}{path}/{fileToUpload.Name}";
                ftpRequest = (FtpWebRequest)WebRequest.Create(address);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                Stream writer = ftpRequest.GetRequestStream();
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);

                writer.Write(buffer, 0, buffer.Length);

                file.Close();
                writer.Close();
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

        public void Mkdir(string path)
        {
            string address = $"ftp://{IpAddress}:{Port}{path}";
            ftpRequest = (FtpWebRequest)WebRequest.Create(address);
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
        #endregion
    }
}
