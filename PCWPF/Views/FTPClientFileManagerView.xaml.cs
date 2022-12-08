using System.Windows;
using System.Windows.Input;

namespace PCWPF.Views
{
    /// <summary>
    /// Interaction logic for FTPClientFileManagerView.xaml
    /// </summary>
    public partial class FTPClientFileManagerView : Window
    {
        public FTPClientFileManagerView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
