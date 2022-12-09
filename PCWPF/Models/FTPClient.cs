using PCWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        #endregion
    }
}
