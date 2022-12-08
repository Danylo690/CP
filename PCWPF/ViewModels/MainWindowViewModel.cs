using PCWPF.Views;
using System.Windows.Input;

namespace PCWPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private bool _isViewVisible = true;
        #endregion

        #region Properties
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
        public ICommand ShowConnectFTPServerViewCommand { get; }
        public ICommand ShowCreateFTPServerViewCommand { get; }
        #endregion

        #region Constructor
        public MainWindowViewModel() 
        {
            //Commands
            ShowConnectFTPServerViewCommand = new ViewModelCommand(ExecuteShowConnectFTPServerViewCommand);
            ShowCreateFTPServerViewCommand = new ViewModelCommand(ExecuteShowCreateFTPServerViewCommand);
        }

        private void ExecuteShowCreateFTPServerViewCommand(object obj)
        {
            var createWindow = new CreateFTPServerView();
            createWindow.Show();
            createWindow.IsVisibleChanged += (s, ev) =>
            {
                if (createWindow.IsVisible == false && createWindow.IsLoaded)
                {
                    createWindow.Close();
                }
            };
            IsViewVisible = false;
        }

        private void ExecuteShowConnectFTPServerViewCommand(object obj)
        {
            var connectWindow = new ConnectFTPServerView();
            connectWindow.Show();
            connectWindow.IsVisibleChanged += (s, ev) =>
            {
                if (connectWindow.IsVisible == false && connectWindow.IsLoaded)
                {
                    connectWindow.Close();
                }
            };
            IsViewVisible = false;
        }
        #endregion
    }
}
