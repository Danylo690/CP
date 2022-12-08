using FontAwesome.Sharp;
using System;
using System.Windows.Input;

namespace PCWPF.ViewModels
{
    public class FTPClientFileManagerViewModel : ViewModelBase
    {
        #region Fields
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
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
        #endregion

        #region Commands
        public ICommand ShowPCFilesViewCommand { get; }
        public ICommand ShowAndroidFilesViewCommand { get; }
        public ICommand ShowMessageHistoryViewCommand { get; }
        public ICommand ShowInformationViewCommand { get; }
        #endregion

        #region Constructors
        public FTPClientFileManagerViewModel()
        {
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
            Caption = "Android Files";
            Icon = IconChar.MobileScreen;
        }

        private void ExecuteShowPCFilesViewCommand(object obj)
        {
            Caption = "PC Files";
            Icon = IconChar.Laptop;
        }
        #endregion
    }
}
