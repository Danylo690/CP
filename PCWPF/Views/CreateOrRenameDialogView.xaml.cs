using PCWPF.Models;
using PCWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PCWPF.Views
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class CreateOrRenameDialogView : Window
    {
        public CreateOrRenameDialogView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CreateOrRenameDialogView));

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as FileManagerViewModel).NewFolderName = "";
            Close();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
