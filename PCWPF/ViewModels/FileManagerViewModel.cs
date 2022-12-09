using FontAwesome.Sharp;
using PCWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCWPF.ViewModels
{
    public class FileManagerViewModel : ViewModelBase
    {
        private List<FileInformation> _files= new List<FileInformation>();

        public FileManagerViewModel()
        {
            Files = (List<FileInformation>)Thread.GetData(Thread.GetNamedDataSlot("Files"));
        }

        public List<FileInformation> Files { 
            get => _files;
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }
    }
}
