using System;
using System.Windows;
using PCWPF.Views;

namespace PCWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, EventArgs e)
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
        }
    }
}
