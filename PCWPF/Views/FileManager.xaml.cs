using PCWPF.Models;
using PCWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCWPF.Views
{
    /// <summary>
    /// Interaction logic for PCFileManager.xaml
    /// </summary>
    public partial class FileManager : UserControl
    {
        private List<string> pathHistory;
        private int currentPathIndex;
        public FileManager()
        {
            InitializeComponent();
            pathHistory = new List<string>();
            pathHistory.Add("");
            currentPathIndex = 0;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileInformation selectedFile = DataGridContent.SelectedItem as FileInformation;
            if (selectedFile != null && selectedFile.IsFolder)
            {
                FTPClient client = (FTPClient)Thread.GetData(Thread.GetNamedDataSlot("Client"));
                string path = $"{selectedFile.Path}{selectedFile.Name}";
                (DataContext as FileManagerViewModel).Files = client.List(path);
                PathTextBox.Text = path + "/";
                if (currentPathIndex == pathHistory.Count - 1)
                {
                    pathHistory.Add(path);
                }
                else
                {
                    pathHistory.RemoveRange(currentPathIndex + 1, pathHistory.Count - (currentPathIndex + 1));
                    pathHistory.Add(path);
                    MoveNextFolder.IsEnabled = false;
                }
                currentPathIndex++;
                MovePrevFolder.IsEnabled = true;
            }
        }

        private void DataGridContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridContent.SelectedItem != null)
            {
                PathTextBox.Text = (DataGridContent.SelectedItem as FileInformation).Path;
            }
        }

        private void MovePrevFolder_Click(object sender, RoutedEventArgs e)
        {
            FTPClient client = (FTPClient)Thread.GetData(Thread.GetNamedDataSlot("Client"));
            string path = pathHistory[currentPathIndex - 1];
            (DataContext as FileManagerViewModel).Files = client.List(path);
            PathTextBox.Text = path + "/";
            currentPathIndex --;
            MoveNextFolder.IsEnabled = true;
            if (currentPathIndex == 0)
            {
                MovePrevFolder.IsEnabled = false;
            }
        }

        private void MoveNextFolder_Click(object sender, RoutedEventArgs e)
        {
            FTPClient client = (FTPClient)Thread.GetData(Thread.GetNamedDataSlot("Client"));
            string path = pathHistory[currentPathIndex + 1];
            (DataContext as FileManagerViewModel).Files = client.List(path);
            PathTextBox.Text = path + "/";
            currentPathIndex ++;
            MovePrevFolder.IsEnabled = true;
            if (currentPathIndex == pathHistory.Count - 1)
            {
                MoveNextFolder.IsEnabled = false;
            }
        }
    }
}
