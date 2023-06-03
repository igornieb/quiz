using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using lab5.Models;
using System.Data.SQLite;
using lab5.ViewModels.BaseClass;
using System.Xml.Linq;

namespace lab5.ViewModels
{
    public class SelectQuizViewModel : BaseViewModel
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
        private int selectedIndex { get; set; }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
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
    }
}
