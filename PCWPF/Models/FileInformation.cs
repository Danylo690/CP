using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PCWPF.Models
{
    public class FileInformation
    {
        private string _name;
        private int _size;
        private string _date;
        private string _permission;
        private bool _isFile;
        private bool _isFolder;
        private IconChar _icon;
        private string _path;
        public string Name { get => _name; set => _name = value; }
        public int Size { get => _size; set => _size = value; }
        public string Date { get => _date; set => _date = value; }
        public string Permission { get => _permission; set => _permission = value; }
        public bool IsFile { 
            get => _isFile;
            set
            {
                _isFile = value;
                if (value)
                {
                    Icon = IconChar.File;
                }
            }
        }
        public bool IsFolder { 
            get => _isFolder;
            set
            {
                _isFolder = value;
                if (value)
                {
                    Icon = IconChar.Folder;
                }
            }
        }
        public IconChar Icon { get => _icon; set => _icon = value; }
        public string Path { get => _path; set => _path = value; }
    }
}
