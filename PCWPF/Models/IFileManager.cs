using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCWPF.Models
{
    public interface IFileManager
    {
        List<FileInformation> List(string path);
        string GetFileManagerMode();
        void Mkdir(string spath, string name = "");
        void Rename(FileInformation renamedFile, string newName);
        void Delete(List<FileInformation> deletedFiles);
    }
}
