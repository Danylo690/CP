using FontAwesome.Sharp;
using PCWPF.Models;
using PCWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PCWPF.ViewModels
{
    public class FileManagerViewModel : ViewModelBase
    {
        private List<FileInformation> _files= new List<FileInformation>();
        private List<string> pathHistory;
        private int currentPathIndex;
        private FTPClient client;
        private PCFileManager pcFileManager;
        private string managerMode;

        public FileManagerViewModel()
        {
            Files = (List<FileInformation>)Thread.GetData(Thread.GetNamedDataSlot("Files"));
            PathHistory = new List<string>();
            PathHistory.Add("");
            CurrentPathIndex = 0;
            Client = (FTPClient)Thread.GetData(Thread.GetNamedDataSlot("Client"));
            PcFileManager = (PCFileManager)Thread.GetData(Thread.GetNamedDataSlot("PCFileManager"));
            managerMode = (string)Thread.GetData(Thread.GetNamedDataSlot("ManagerMode"));
        }

        public List<FileInformation> Files { 
            get => _files;
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }
        public List<string> PathHistory { get => pathHistory; set => pathHistory = value; }
        public int CurrentPathIndex { get => currentPathIndex; set => currentPathIndex = value; }
        public FTPClient Client { get => client; set => client = value; }
        public PCFileManager PcFileManager { get => pcFileManager; set => pcFileManager = value; }
        public string ManagerMode { get => managerMode; set => managerMode = value; }
    }
}
