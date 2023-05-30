using lab5.Models;
using lab5.ViewModels.BaseClass;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows.Input;

namespace lab5.ViewModels
{
    public class ListQuizViewModel : BaseViewModel
    {
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

        private string name { get; set; }
        private string description { get; set; }
        private int selectedIndex { get; set; }


        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                //set name and description
                if(value>=0)
                {
                    Name = QuizList[selectedIndex].Name;
                    Description = QuizList[selectedIndex].Description;
                }
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

        private ICommand deleteQuiz;
        private ICommand createQuiz;
        private ICommand updateQuiz;

        public ICommand CreateQuiz
        {
            get
            {
                return createQuiz ?? (createQuiz = new RelayCommand(
                    (p) =>
                    {
                        Quiz quiz = new Quiz();
                        quiz.Name = Name;
                        quiz.Description = Description;
                        try
                        {
                            var db = DataAccess.GetDb();
                            string query = $"INSERT INTO 'quiz' (name, description) values ('{quiz.Name}', '{quiz.Description}')";
                            var command = new SQLiteCommand(query, db);
                            command.ExecuteNonQuery();
                            db.Close();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        Name = "";
                        Description = "";
                        QuizList = GetQuizList();
                    }
                    ,
                    p => true)
                    );
            }
        }

        public ICommand UpdateQuiz
        {
            get
            {
                return updateQuiz ?? (updateQuiz = new RelayCommand(
                    (p) =>
                    {
                        if (SelectedIndex >= 0 && SelectedIndex < QuizList.Count)
                        {
                            try
                            {
                                long id = QuizList[SelectedIndex].Id;
                                var db = DataAccess.GetDb();
                                string query = $"update 'quiz' set 'name'='{Name}','description'='{Description}' where id={id}";
                                var command = new SQLiteCommand(query, db);
                                command.ExecuteNonQuery();
                                db.Close();
                                QuizList = GetQuizList();
                                Name = "";
                                Description = "";
                                SelectedIndex = -1;
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

        public ICommand DeleteQuiz
        {
            get
            {
                // jesli nie jest określone polecenie to tworzymy je i zwracamy poprozez 
                //pomocniczy typ RelayCommand
                return deleteQuiz ?? (deleteQuiz = new RelayCommand(
                    //co wykonuje polecenie
                    (p) =>
                    {
                        if (SelectedIndex>0 && SelectedIndex<QuizList.Count)
                        {
                            //get id from list
                            try
                            {
                                long id = QuizList[SelectedIndex].Id;
                                var db = DataAccess.GetDb();
                                string query = $"Delete from 'quiz' where id={id}";
                                var command = new SQLiteCommand(query, db);
                                command.ExecuteNonQuery();
                                db.Close();
                                QuizList = GetQuizList();
                                Name = "";
                                Description = "";
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                    }
                    ,
                    //warunek kiedy może je wykonać
                    p => true)
                    );
            }
        }
    }
}
