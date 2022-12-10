using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace PCWPF.Models
{
    public class FileInformation
    {
        private string _name;
        private double _size;
        private string _parsedSize;
        private string _date;
        private string _permission;
        private bool _isFile;
        private bool _isFolder;
        private bool _isDrive;
        private IconChar _icon;
        private string _path;

        public string Name { get => _name; set => _name = value; }
        public double Size
        {
            get => _size;
            set
            {
                _size = value;
                ParseFileSize(value);
            }
        }
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
        public bool IsDrive { 
            get => _isDrive;
            set
            {
                _isDrive = value;
                if (value)
                {
                    Name = _name.Substring(0,2);
                    Icon = IconChar.HardDrive;
                }
            }
        }
        public IconChar Icon { get => _icon; set => _icon = value; }
        public string Path { get => _path; set => _path = value; }
        public string ParsedSize { get => _parsedSize; set => _parsedSize = value; }

        private void ParseFileSize(double size)
        {
            double outSize = size;
            string sizeType = "B";
            if (outSize > 1000)
            {
                outSize = outSize / 1024f;
                sizeType = "KB";
            }
            if (outSize > 1000)
            {
                outSize = outSize / 1024f;
                sizeType = "MB";
            }
            if (outSize > 1000)
            {
                outSize = outSize / 1024f;
                sizeType = "GB";
            }
            double fract = outSize - (int)outSize;
            ParsedSize = fract == 0 ? $"{outSize} {sizeType}" : $"{outSize:0.0} {sizeType}";
        }
    }
}
