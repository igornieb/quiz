using System.Data.SQLite;
using System;

namespace lab5.Models
{
    public static class DataAccess
    {
        private static SQLiteConnection connection;
        public static SQLiteConnection GetDb()
        {
            string directory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName+"\\db_test.db";
            connection = new SQLiteConnection($"Data Source={directory}");
            connection.Open();
            return connection;
        }
    }
}
