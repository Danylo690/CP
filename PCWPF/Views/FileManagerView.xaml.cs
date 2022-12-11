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
        private string copyFrom;
        public FileManagerView()
        {
            InitializeComponent();
            filesToCopy = new List<FileInformation>();
        }

        private void DataGridContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridContent.SelectedItem != null)
            {
                PathTextBox.Text = (DataGridContent.SelectedItem as FileInformation).Path;
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
                if (!fileToCopy.IsDrive)
                {
                    copyFrom = (DataContext as FileManagerViewModel).FileManager.GetFileManagerMode() + ">";
                    if (copyFrom == "PC>")
                    {
                        PasteBtn.IsEnabled = false;
                    }
                    if (copyFrom == "Android>")
                    {
                        PasteBtn.IsEnabled = false;
                    }
                    filesToCopy.Add(fileToCopy);
                }
            }
        }

        private void DataGridContent_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((DataContext as FileManagerViewModel) != null)
            {
                (DataContext as FileManagerViewModel).FilesToCopy = filesToCopy;
                (DataContext as FileManagerViewModel).CopyFrom = copyFrom;
                PasteBtn.IsEnabled = false;
                if (copyFrom == "PC>")
                {
                    if ((DataContext as FileManagerViewModel).FileManager.GetFileManagerMode() == "Android")
                    {
                        PasteBtn.IsEnabled = true;
                    }
                }
                if (copyFrom == "Android>")
                {
                    if ((DataContext as FileManagerViewModel).FileManager.GetFileManagerMode() == "PC")
                    {
                        PasteBtn.IsEnabled = true;
                    }
                }
            }
        }
    }
}
