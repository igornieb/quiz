using lab5.Models;
using lab5.ViewModels.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace lab5.ViewModels
{
    public class ListQuestionViewModel : BaseViewModel
    {
        private int FindQuizInList(long QuizId)
        {
            var quizes = GetQuizList();
            for (int i=0; i< quizes.Count(); i++)
            {
                if (quizes.ElementAt(i).Id == QuizId)
                {
                    return i;
                }
            }
            return 0;
        }

        private ObservableCollection<Question> GetQuestionsList()
        {
            ObservableCollection<Question> list = new ObservableCollection<Question>();
            var db = DataAccess.GetDb();
            var command = db.CreateCommand();
            command.CommandText = "SELECT * FROM 'question'";
            SQLiteDataReader r = command.ExecuteReader();
            while (r.Read())
            {
                Question question = new Question { Id = (long)r["id"], IdQuiz = (long)r["idQuiz"],Quest = (string)r["question"], A = (string)r["a"], B = (string)r["b"], C = (string)r["c"], D = (string)r["d"], Answear = (string)r["answear"] };
                list.Add(question);
            }
            db.Close();
            return list;
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
                Question question = new Question { Id = (long)r["id"], IdQuiz = (long)r["idQuiz"], Quest = (string)r["question"], A = (string)r["a"], B = (string)r["b"], C = (string)r["c"], D = (string)r["d"], Answear = (string)r["answear"] };
                list.Add(question);
            }
            db.Close();
            return list;
        }

        private ObservableCollection<Quiz> GetQuizList()
        {
            ObservableCollection<Quiz> list = new ObservableCollection<Quiz>();
            var db = DataAccess.GetDb();
            var command = db.CreateCommand();
            command.CommandText = "SELECT * FROM 'quiz'";
            SQLiteDataReader r = command.ExecuteReader();
            while (r.Read())
            {

                Quiz quiz = new Quiz { Id = (long)r["id"], Name = (string)r["name"], Description = (string)r["description"] };
                list.Add(quiz);
            }
            return list;
        }

        private void SetAns(string ans)
        {
            if(ans=="a")
            {
                IsAChecked = true;
            }
            if (ans == "b")
            {
                IsBChecked = true;
            }
            if (ans == "c")
            {
                IsCChecked = true;
            }
            if (ans == "d")
            {
                IsDChecked = true;
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
            return "d";
        }

        private int questionIndex, quizIndex, quizQuestionIndex;
        private string ansA, ansB, ansC, ansD, quest;
        private ObservableCollection<Question> questionList;

        private bool isAChecked, isBChecked, isCChecked, isDChecked;
        public bool IsAChecked
        {
            get
            {
                return isAChecked;
            }
            set
            {
                if(value!=isAChecked)
                {
                    isAChecked = value;
                    isBChecked = isCChecked = isDChecked = !value;
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
                    isAChecked = isCChecked = isDChecked = !value;
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
                    isAChecked = isBChecked = isDChecked = !value;
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
                    isAChecked = isCChecked = isBChecked = !value;
                    OnPropertyChanged(nameof(isDChecked));
                }
            }

        }
        public string Quest
        {
            get { return quest; }
            set
            {
                quest = value;
                OnPropertyChanged(nameof(Quest));
            }
        }
        public string AnsA
        {
            get { return ansA; }
            set 
            { 
                ansA = value;
                OnPropertyChanged(nameof(ansA));
            }
        }
        public string AnsB
        {
            get { return ansB; }
            set
            {
                ansB = value;
                OnPropertyChanged(nameof(ansB));
            }
        }
        public string AnsC
        {
            get { return ansC; }
            set
            {
                ansC = value;
                OnPropertyChanged(nameof(ansC));
            }
        }
        public string AnsD
        {
            get { return ansD; }
            set
            {
                ansD = value;
                OnPropertyChanged(nameof(ansD));
            }
        }

        public int QuestionIndex
        {
            get { return questionIndex; }
            set
            {
            questionIndex = value;
            OnPropertyChanged(nameof(questionIndex));
                if (value >= 0 && value < QuestionList.Count)
                {
                    Quest = QuestionList.ElementAt(value).Quest;
                    AnsA = QuestionList.ElementAt(value).A;
                    AnsB = QuestionList.ElementAt(value).B;
                    AnsC = QuestionList.ElementAt(value).C;
                    AnsD = QuestionList.ElementAt(value).D;
                    SetAns(QuestionList.ElementAt(value).Answear.ToLower());
                    QuizIndex = FindQuizInList(QuestionList.ElementAt(value).IdQuiz);
                }
            }
        }

        public int QuizIndex
        {
            get { return quizIndex; }
            set
            {
                if (value >= 0)
                {
                    quizIndex = value;
                    OnPropertyChanged(nameof(quizIndex));
                }
            }
        }

        public int QuizQuestionIndex
        {
            get { return quizQuestionIndex; }
            set
            {
                quizQuestionIndex = value;
                QuizIndex = value;
                OnPropertyChanged(nameof(quizQuestionIndex));
                OnPropertyChanged(nameof(QuestionList));

            }
        }
        public ObservableCollection<Quiz> QuizList
        {
            set
            {
                OnPropertyChanged(nameof(QuizList));
            }
            get { return GetQuizList(); }
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
                return GetQuestionsListFilter(QuizList[QuizQuestionIndex].Id);          
            }
        }

        private ICommand createQuestion;
        private ICommand updateQuestion;
        private ICommand deleteQuestion;
        public ICommand CreateQuestion
        {
            get
            {
                return createQuestion ?? (createQuestion = new RelayCommand(
                    (p) =>
                    {
                        try
                        {
                            Question question = new Question { IdQuiz = QuizList.ElementAt(QuizIndex).Id, Quest = Quest, A=AnsA,B=AnsB,C=AnsC,D=AnsC,Answear=GetAns().ToLower()};
                            var db = DataAccess.GetDb();
                            string query = $"INSERT INTO 'question' (idQuiz, question, a, b, c, d, answear) values ('{question.IdQuiz}', '{question.Quest}', '{question.A}', '{question.B}', '{question.C}', '{question.D}', '{question.Answear[0]}')";
                            var command = new SQLiteCommand(query, db);
                            command.ExecuteNonQuery();
                            db.Close();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        QuestionList = GetQuestionsList();
                        Quest = AnsA = AnsB = AnsC = AnsD = "";
                    }
                    ,
                    p => true)
                    );
            }
        }
        public ICommand UpdateQuestion
        {
            get
            {
                return updateQuestion ?? (updateQuestion = new RelayCommand(
                (p) =>
                {
                    if (QuizIndex >= 0 && QuizIndex < QuizList.Count && QuestionIndex>=0)
                    {
                        try
                        {
                            long id = QuestionList[QuestionIndex].Id;
                            var db = DataAccess.GetDb();
                            string query = $"update 'question' set 'question'='{Quest}','idQuiz'={QuizList[QuizIndex].Id}, 'a'='{AnsA}','b'='{AnsB}','c'='{AnsC}','d'='{AnsD}','answear'='{GetAns()[0]}' where id={id}";
                            var command = new SQLiteCommand(query, db);
                            command.ExecuteNonQuery();
                            db.Close();
                            QuestionList = GetQuestionsList();
                            Quest = AnsA = AnsB = AnsC = AnsD = "";
                            QuestionIndex = -1;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
                ,
                p => true)
                );

            }
        }
        public ICommand DeleteQuestion
        {
            get
            {
                return deleteQuestion ?? (deleteQuestion = new RelayCommand(
                (p) =>
                {
                    if (QuizIndex >= 0 && QuizIndex < QuizList.Count)
                    {
                        try
                        {
                            long id = QuestionList[QuestionIndex].Id;
                            var db = DataAccess.GetDb();
                            string query = $"Delete from 'question' where id={id}";
                            var command = new SQLiteCommand(query, db);
                            command.ExecuteNonQuery();
                            db.Close();
                            QuestionList = GetQuestionsList();
                            Quest = AnsA = AnsB = AnsC = AnsD = "";
                            QuestionIndex = -1;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
                ,
                p => true)
                );
            }
        }

    }
}
