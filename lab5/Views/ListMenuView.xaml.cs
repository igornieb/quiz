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

namespace lab5.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ListMenuView.xaml
    /// </summary>
    public partial class ListMenuView : Page
    {
        public ListMenuView()
        {
            InitializeComponent();
        }

        private void Button_AddQuiz(object sender, RoutedEventArgs e)
        {
            var x = new ListQuizesView();
            Content = x;
        }

        private void Button_AddQuest(object sender, RoutedEventArgs e)
        {
            var x = new ListQuestionsView();
            Content = x;
        }
    }
}
