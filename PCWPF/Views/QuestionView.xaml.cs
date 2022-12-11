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
    /// Interaction logic for QuestionView.xaml
    /// </summary>
    public partial class QuestionView : Window
    {
        public QuestionView()
        {
            InitializeComponent();
        }

        public string QuestionTitle
        {
            get { return (string)GetValue(QuestionTitleProperty); }
            set { SetValue(QuestionTitleProperty, value); }
        }

        public static readonly DependencyProperty QuestionTitleProperty =
            DependencyProperty.Register("QuestionTitle", typeof(string), typeof(QuestionView));

        public string Question
        {
            get { return (string)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register("Question", typeof(string), typeof(QuestionView));

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as FileManagerViewModel).IsDelete = false;
            Close();
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as FileManagerViewModel).IsDelete = true;
            Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as FileManagerViewModel).IsDelete = false;
            Close();
        }
    }
}
