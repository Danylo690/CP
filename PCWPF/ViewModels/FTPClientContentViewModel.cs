using FontAwesome.Sharp;
using PCWPF.Models;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace PCWPF.ViewModels
{
    public class FTPClientContentViewModel : ViewModelBase
    {
        #region Fields
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        private bool _isViewVisible;
        private FTPClient client;
        private List<FileInformation> files;
        private PCFileManager pcFileManager;
        private List<Log> logs;
        #endregion

        #region Properties
        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        public bool IsViewVisible { 
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }
        #endregion

        #region Commands
        public ICommand ShowPCFilesViewCommand { get; }
        public ICommand ShowAndroidFilesViewCommand { get; }
        public ICommand ShowMessageHistoryViewCommand { get; }
        public ICommand ShowInformationViewCommand { get; }
        #endregion

        #region Constructors
        public FTPClientContentViewModel()
        {
            client = (FTPClient)Thread.GetData(Thread.GetNamedDataSlot("Client"));
            pcFileManager = new PCFileManager();
            logs = new List<Log>();

            //Commands
            ShowPCFilesViewCommand = new ViewModelCommand(ExecuteShowPCFilesViewCommand);
            ShowAndroidFilesViewCommand = new ViewModelCommand(ExecuteShowAndroidFilesViewCommand);
            ShowMessageHistoryViewCommand = new ViewModelCommand(ExecuteShowMessageHistoryViewCommand);
            ShowInformationViewCommand = new ViewModelCommand(ExecuteShowInformationViewCommand);
        }

        private void ExecuteShowInformationViewCommand(object obj)
        {
            Caption = "Information";
            Icon = IconChar.CircleQuestion;
        }

        private void ExecuteShowMessageHistoryViewCommand(object obj)
        {
            CurrentChildView = new MessageHistoryViewModel()
            {
                Logs = logs
            };
            Caption = "Message History";
            Icon = IconChar.BookOpen;
        }

        private void ExecuteShowAndroidFilesViewCommand(object obj)
        {
            Thread.SetData(Thread.GetNamedDataSlot("Logs"), logs);
            CurrentChildView = new FileManagerViewModel(client);
            Caption = "Android Files";
            Icon = IconChar.MobileScreen;
        }

        private void ExecuteShowPCFilesViewCommand(object obj)
        {
            Thread.SetData(Thread.GetNamedDataSlot("Logs"), logs);
            Thread.SetData(Thread.GetNamedDataSlot("PCFileManager"), pcFileManager);
            CurrentChildView = new FileManagerViewModel(pcFileManager);
            Caption = "PC Files";
            Icon = IconChar.Laptop;
        }
        #endregion
    }
}
