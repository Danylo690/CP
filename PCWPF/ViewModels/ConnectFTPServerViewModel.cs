using System;
using System.Windows.Input;
using System.Net;
using PCWPF.Views;

namespace PCWPF.ViewModels
{
    public class ConnectFTPServerViewModel : ViewModelBase
    {
        #region Fields
        private string _ipAddress;
        private string _port;
        private string _errorMessage;
        private bool _isViewVisible = true;
        #endregion

        #region Properties
        public string IpAddress {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                OnPropertyChanged(nameof(IpAddress));
            }
        }
        public string Port {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }
        public string ErrorMessage {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
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
        public ICommand ConnectCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructors
        public ConnectFTPServerViewModel()
        {
            ConnectCommand = new ViewModelCommand(ExecuteConnectCommand, CanExecuteConnectCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
         }

        private void ExecuteCancelCommand(object obj)
        {
            var mainView = new MainWindowView();
            mainView.Show();
            mainView.IsVisibleChanged += (s, ev) =>
            {
                if (mainView.IsVisible == false && mainView.IsLoaded)
                {
                    mainView.Close();
                }
            };
            IsViewVisible = false;
        }

        private bool CanExecuteConnectCommand(object obj)
        {
            return true;
            //try
            //{
            //    bool validData;
            //    IPAddress outAddress;
            //    if (string.IsNullOrEmpty(IpAddress) ||
            //        !IPAddress.TryParse(IpAddress, out outAddress) ||
            //        Port.Length < 4)
            //    {
            //        validData = false;
            //    }
            //    else
            //    {
            //        validData = true;
            //    }
            //    return validData;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        private void ExecuteConnectCommand(object obj)
        {
            var fileManager = new FTPClientFileManagerView();
            fileManager.Show();
            fileManager.IsVisibleChanged += (s, ev) =>
            {
                if (fileManager.IsVisible == false && fileManager.IsLoaded)
                {
                    fileManager.Close();
                }
            };
            IsViewVisible = false;
        }
        #endregion
    }
}
