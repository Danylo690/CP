using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PCWPF.Models
{
    public class PCFileManager : IFileManager
    {
        private List<FileInformation> _files;

        public List<FileInformation> Files { get => _files; set => _files = value; }

        public string GetFileManagerMode()
        {
            return "PC";
        }

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

        public void Mkdir(string path, string name = "")
        {
            Directory.CreateDirectory(path + "\\" + name);
        }

        public void Rename(FileInformation renamedFile, string newName)
        {
            try
            {
                int index = renamedFile.Path.LastIndexOf("\\");
                string newPath = "";
                if (index != -1)
                {
                    newPath = renamedFile.Path.Substring(0, index + 1) + newName;
                }
                if (renamedFile.IsFolder)
                {
                    Directory.Move(renamedFile.Path, newPath);
                }
                if (renamedFile.IsFile)
                {
                    System.IO.File.Move(renamedFile.Path, newPath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Delete(List<FileInformation> deletedFiles)
        {
            foreach (FileInformation deletedFile in deletedFiles)
            {
                if (deletedFile.IsFolder)
                {
                    Directory.Delete(deletedFile.Path, true);
                }
                if (deletedFile.IsFile)
                {
                    File.Delete(deletedFile.Path);
                }
            }
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
