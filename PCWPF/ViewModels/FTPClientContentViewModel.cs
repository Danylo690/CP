using FontAwesome.Sharp;
using PCWPF.Models;
using PCWPF.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
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
            Caption = "Message History";
            Icon = IconChar.BookOpen;
        }

        private void ExecuteShowAndroidFilesViewCommand(object obj)
        {
            files = client.List("");
            Thread.SetData(Thread.GetNamedDataSlot("Files"), files);
            Thread.SetData(Thread.GetNamedDataSlot("ManagerMode"), "Android");
            CurrentChildView = new FileManagerViewModel();
            Caption = "Android Files";
            Icon = IconChar.MobileScreen;
        }

        private void ExecuteShowPCFilesViewCommand(object obj)
        {
            files = pcFileManager.List("");
            Thread.SetData(Thread.GetNamedDataSlot("Files"), files);
            Thread.SetData(Thread.GetNamedDataSlot("PCFileManager"), pcFileManager);
            Thread.SetData(Thread.GetNamedDataSlot("ManagerMode"), "PC");
            CurrentChildView = new FileManagerViewModel();
            Caption = "PC Files";
            Icon = IconChar.Laptop;
        }
        #endregion
    }
}
