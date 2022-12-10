using PCWPF.Models;
using PCWPF.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PCWPF.Views
{
    /// <summary>
    /// Interaction logic for PCFileManager.xaml
    /// </summary>
    public partial class FileManagerView : UserControl
    {
        private List<FileInformation> filesToCopy;
        private FileManagerViewModel dataContext;
        private string copyFrom;
        private string pasteTo;
        public FileManagerView()
        {
            InitializeComponent();
            filesToCopy = new List<FileInformation>();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            switch (dataContext.ManagerMode)
            {
                case "PC":
                    {
                        ListPC();
                        break;
                    }
                case "Android":
                    {
                        ListAndroid();
                        break;
                    }
            }
        }

        private void ListAndroid()
        {
            FileInformation selectedFile = DataGridContent.SelectedItem as FileInformation;
            if (selectedFile != null && selectedFile.IsFolder)
            {
                string path = $"{selectedFile.Path}{selectedFile.Name}";
                dataContext.Files = dataContext.Client.List(path);
                PathTextBox.Text = path + "/";
                if (dataContext.CurrentPathIndex == dataContext.PathHistory.Count - 1)
                {
                    dataContext.PathHistory.Add(path);
                }
                else
                {
                    dataContext.PathHistory.RemoveRange(dataContext.CurrentPathIndex + 1, dataContext.PathHistory.Count - (dataContext.CurrentPathIndex + 1));
                    dataContext.PathHistory.Add(path);
                    MoveNextFolder.IsEnabled = false;
                }
                dataContext.CurrentPathIndex++;
                MovePrevFolder.IsEnabled = true;
            }
        }

        private void ListPC()
        {
            FileInformation selectedFile = DataGridContent.SelectedItem as FileInformation;
            if (selectedFile != null && (selectedFile.IsFolder || selectedFile.IsDrive))
            {
                string path = $@"{selectedFile.Path}";
                dataContext.Files = dataContext.PcFileManager.List(path);
                PathTextBox.Text = path;
                if (dataContext.CurrentPathIndex == dataContext.PathHistory.Count - 1)
                {
                    dataContext.PathHistory.Add(path);
                }
                else
                {
                    dataContext.PathHistory.RemoveRange(dataContext.CurrentPathIndex + 1, dataContext.PathHistory.Count - (dataContext.CurrentPathIndex + 1));
                    dataContext.PathHistory.Add(path);
                    MoveNextFolder.IsEnabled = false;
                }
                dataContext.CurrentPathIndex++;
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
            string path = dataContext.PathHistory[dataContext.CurrentPathIndex - 1];
            switch (dataContext.ManagerMode)
            {
                case "PC":
                    {
                        (DataContext as FileManagerViewModel).Files = dataContext.PcFileManager.List(path);
                        PathTextBox.Text = path;
                        break;
                    }
                case "Android":
                    {
                        (DataContext as FileManagerViewModel).Files = dataContext.Client.List(path);
                        PathTextBox.Text = path + "/";
                        break;
                    }
            }
            dataContext.CurrentPathIndex --;
            MoveNextFolder.IsEnabled = true;
            if (dataContext.CurrentPathIndex == 0)
            {
                MovePrevFolder.IsEnabled = false;
            }
        }

        private void MoveNextFolder_Click(object sender, RoutedEventArgs e)
        {
            string path = dataContext.PathHistory[dataContext.CurrentPathIndex + 1];
            switch (dataContext.ManagerMode)
            {
                case "PC":
                    {
                        dataContext.Files = dataContext.PcFileManager.List(path);
                        PathTextBox.Text = path;
                        break;
                    }
                case "Android":
                    {
                        dataContext.Files = dataContext.Client.List(path);
                        PathTextBox.Text = path + "/";
                        break;
                    }
            }
            dataContext.CurrentPathIndex ++;
            MovePrevFolder.IsEnabled = true;
            if (dataContext.CurrentPathIndex == dataContext.PathHistory.Count - 1)
            {
                MoveNextFolder.IsEnabled = false;
            }
        }

        private void DataGridContent_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            dataContext = (DataContext as FileManagerViewModel);
            if (dataContext.PathHistory.Count == 1)
            {
                MovePrevFolder.IsEnabled = false;
                MoveNextFolder.IsEnabled = false;
                PathTextBox.Text = "";
            }
        }

        private void DataGridContent_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridContent.UnselectAll();
        }

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            filesToCopy.Clear();
            foreach (FileInformation fileToCopy in DataGridContent.SelectedItems)
            {
                copyFrom = dataContext.ManagerMode + ">";
                filesToCopy.Add(fileToCopy);
            }
            return;
        }

        private void PasteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (filesToCopy.Count > 0)
            {
                pasteTo = dataContext.ManagerMode;
                switch (copyFrom + pasteTo)
                {
                    case "PC>PC":
                        {
                            break;
                        }
                    case "PC>Android":
                        {
                            string path = dataContext.PathHistory[dataContext.CurrentPathIndex];
                            dataContext.Client.Upload(filesToCopy, path);
                            dataContext.Files = dataContext.Client.List(path);
                            break;
                        }
                    case "Android>PC":
                        {
                            string path = dataContext.PathHistory[dataContext.CurrentPathIndex];
                            if (path != "")
                            {
                                dataContext.Client.Download(filesToCopy, path);
                                dataContext.Files = dataContext.PcFileManager.List(path);
                            }
                            break;
                        }
                    case "Android>Android":
                        {
                            break;
                        }
                }
            }
        }
    }
}
