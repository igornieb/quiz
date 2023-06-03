using lab5.Models;
using lab5.ViewModels;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Navigation;

namespace lab5.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SelectQuizView.xaml
    /// </summary>
    public partial class SelectQuizView : Page
    {
        public SelectQuizView()
        {
            InitializeComponent();
        }
        private void Button_Quizuj(object sender, RoutedEventArgs e)
        {
            Quiz quiz = (Quiz)QuizList.SelectedItem;
            if (quiz != null)
            {
                PlayQuizViewModel playViewModel = new PlayQuizViewModel();
                playViewModel.Quiz = quiz;
                Play playPage = new Play();
                playPage.DataContext = playViewModel;
                NavigationService.Navigate(playPage);
            }
        }
    }
}
