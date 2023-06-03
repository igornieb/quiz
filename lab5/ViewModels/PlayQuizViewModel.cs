using lab5.Models;
using lab5.ViewModels.BaseClass;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace lab5.ViewModels
{
    public class PlayQuizViewModel : BaseViewModel
    {
        private Quiz quiz;
        private ObservableCollection<Question> questionList;
        private int numberOfQuestion = -1;
        private DispatcherTimer timer;
        private TimeSpan timeRemaining;
        private QuizAtempt quizAtempt;
        public string TimeRemainingFormatted => $"{timeRemaining:mm\\:ss}";

        private string textToShow;
        public string TextToShow
        {
            get { return textToShow; }
            set
            {
                textToShow = value;
                OnPropertyChanged(nameof(TextToShow));
            }
        }

        public ObservableCollection<Question> QuestionList
        {
            set
            {
                questionList = value;
                OnPropertyChanged(nameof(QuestionList));
            }
            get
            {
                return GetQuestionsListFilter(Quiz.Id);
            }
        }

        private Question question;
        public Question Question { get { return question; } set { question = value; OnPropertyChanged(nameof(Question)); } }

        private string buttonText = "START";
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        private ObservableCollection<Question> GetQuestionsListFilter(long QuizId)
        {
            ObservableCollection<Question> list = new ObservableCollection<Question>();
            var db = DataAccess.GetDb();
            var command = db.CreateCommand();
            command.CommandText = $"SELECT * FROM 'question' where idquiz={QuizId}";
            SQLiteDataReader r = command.ExecuteReader();
            while (r.Read())
            {
                long id = (long)r["id"];
                Question question = new Question { Id = (long)r["id"], IdQuiz = (long)r["idQuiz"], Quest = (string)r["question"], A = Question.decrypt((string)r["a"], id), B = Question.decrypt((string)r["b"], id), C = Question.decrypt((string)r["c"], id), D = Question.decrypt((string)r["d"], id), Answear = Question.decrypt((string)r["answear"], id) };
                list.Add(question);
            }
            db.Close();
            return list;
        }

        public Models.Quiz Quiz
        {
            get { return quiz; }
            set
            {
                quiz = value;
                OnPropertyChanged(nameof(Quiz));
            }
        }




        private string selectedOption;
        public string SelectedOption
        {
            get { return selectedOption; }
            set
            {
                selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }

        private bool isAnswerCorrect;
        public bool IsAnswerCorrect
        {
            get { return isAnswerCorrect; }
            set
            {
                isAnswerCorrect = value;
                OnPropertyChanged(nameof(IsAnswerCorrect));
            }
        }

        public ICommand CheckAnswerCommand { get; private set; }

        public PlayQuizViewModel()
        {
            CheckAnswerCommand = new RelayCommand(CheckAnswer, CanCheckAnswer);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            quizAtempt = new QuizAtempt { Score = 0, PlayerName = "ASD" };
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timeRemaining = timeRemaining.Add(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(TimeRemainingFormatted));
        }

        private bool CanCheckAnswer(object parameter)
        {
            // Add your logic to determine if the answer can be checked (e.g., if an option is selected)
            return isAChecked || isBChecked || isCChecked || isDChecked;
        }

        private void CheckAnswer(object parameter)
        {
            // Compare the selected option with the correct answer
            IsAnswerCorrect = GetAns() == Question.Answear;
            quizAtempt.Score = IsAnswerCorrect ? quizAtempt.Score + 1 : quizAtempt.Score;

        }


        private ICommand nextQuestion;
        public ICommand NextQuestion
        {
            get
            {
                return nextQuestion ?? (nextQuestion = new RelayCommand(
                    (p) =>
                    {
                        try
                        {
                            if (numberOfQuestion >= 0)
                                CheckAnswer(null);
                            numberOfQuestion++;
                            Question = QuestionList.ElementAtOrDefault(numberOfQuestion);
                            ButtonText = numberOfQuestion >= QuestionList.Count - 1 ? "FINISH" : "NEXT";
                            if (numberOfQuestion == 0)
                            {
                                timer.Start();
                            }
                            zero_ans();
                            if (numberOfQuestion == QuestionList.Count)
                            {
                                timer.Stop();
                                TextToShow = $"Score: {quizAtempt.Score}/{QuestionList.Count}\t\tTime: {TimeRemainingFormatted}";
                            }
                            else
                            {
                                TextToShow = Question.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    },
                    p => CanNextQuestion(p))
                );
            }
        }

        private void zero_ans()
        {
            IsAChecked = false;
            IsBChecked = false;
            IsCChecked = false;
            IsDChecked = false;
        }

        private bool CanNextQuestion(object parameter)
        {
            return numberOfQuestion < QuestionList.Count;
        }

        private bool isAChecked, isBChecked, isCChecked, isDChecked;
        public bool IsAChecked
        {
            get
            {
                return isAChecked;
            }
            set
            {
                if (value != isAChecked)
                {
                    isAChecked = value;
                    OnPropertyChanged(nameof(isAChecked));
                }
            }

        }
        public bool IsBChecked
        {
            get
            {
                return isBChecked;
            }
            set
            {
                if (value != isBChecked)
                {
                    isBChecked = value;
                    OnPropertyChanged(nameof(isBChecked));
                }
            }
        }
        public bool IsCChecked
        {
            get
            {
                return isCChecked;
            }
            set
            {
                if (value != isCChecked)
                {
                    isCChecked = value;
                    OnPropertyChanged(nameof(isCChecked));
                }
            }
        }
        public bool IsDChecked
        {
            get
            {
                return isDChecked;
            }
            set
            {
                if (value != isDChecked)
                {
                    isDChecked = value;
                    OnPropertyChanged(nameof(isDChecked));
                }
            }

        }
        private string GetAns()
        {
            if (IsAChecked)
            {
                return "a";
            }
            if (IsBChecked)
            {
                return "b";
            }
            if (IsCChecked)
            {
                return "c";
            }
            if (IsDChecked)
                return "d";
            return "z";
        }
    }
}
