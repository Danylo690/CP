using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PCWPF.Models
{
    public class PCFileManager
    {
        private List<FileInformation> _files;

        public List<FileInformation> Files { get => _files; set => _files = value; }

        public List<FileInformation> List(string path)
        {
            List<FileInformation> files = new List<FileInformation>();
            if (path == "")
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    string name = drive.Name;
                    double size = drive.TotalSize;

                    FileInformation file = new FileInformation();
                    file.Name = name;
                    file.Path = name;
                    file.IsDrive = true;
                    file.Size = size;
                    files.Add(file);
                }
            }
            else
            {
                List<string> directoriesName = Directory.GetDirectories(path).ToList();
                foreach (string directoryName in directoriesName)
                {
                    FileInformation file = CreateFileInformation(directoryName, true, false);
                    if (file != null)
                    {
                        files.Add(file);
                    }
                }
                List<string> filesName = Directory.GetFiles(path).ToList();
                foreach (string fileName in filesName)
                {
                    FileInformation file = CreateFileInformation(fileName, false, true);
                    if (file != null)
                    {
                        files.Add(file);
                    }
                }
            }
            return files;
        }

        private FileInformation CreateFileInformation(string path, bool isDir, bool isFile)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Attributes.ToString().IndexOf("Hidden") == -1)
            {
                string name = fileInfo.Name;
                string dirPath = path;
                string date = fileInfo.CreationTimeUtc.ToString();
                string permissions = fileInfo.Attributes.ToString();

                FileInformation file = new FileInformation();
                file.Name = name;
                file.Path = dirPath;
                file.IsFolder = isDir;
                file.IsFile = isFile;
                file.Date = date;
                file.Permission = fileInfo.IsReadOnly ? "Read only" : "Read Write";
                file.Size = isFile ? fileInfo.Length : 0;
                return file;
            }
            return null;
        }
    }
}
