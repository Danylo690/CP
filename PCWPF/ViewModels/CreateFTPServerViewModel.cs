using PCWPF.Views;
using System;
using System.Net;
using System.Windows.Input;

namespace PCWPF.ViewModels
{
    public class CreateFTPServerViewModel : ViewModelBase
    {
        #region Fields
        private string _ipAddress;
        private string _port;
        private string _errorMessage;
        private bool _isViewVisible = true;
        #endregion

        #region Properties
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                OnPropertyChanged(nameof(IpAddress));
            }
        }
        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }
        #endregion

        #region Commands
        public ICommand CreateCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructors
        public CreateFTPServerViewModel()
        {
            CreateCommand = new ViewModelCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
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

        private bool CanExecuteCreateCommand(object obj)
        {
            try
            {
                bool validData;
                IPAddress outAddress;
                if (string.IsNullOrEmpty(IpAddress) ||
                    !IPAddress.TryParse(IpAddress, out outAddress) ||
                    Port.Length < 4)
                {
                    validData = false;
                }
                else
                {
                    validData = true;
                }
                return validData;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ExecuteCreateCommand(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
