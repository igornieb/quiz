using lab5.Models;
using lab5.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab5
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_AddQuiz(object sender, RoutedEventArgs e)
        {
            Main.Content = new ListQuizesView();
        }

        private void Button_AddQuest(object sender, RoutedEventArgs e)
        {
            Main.Content = new ListQuestionsView();
        }
    }
}
