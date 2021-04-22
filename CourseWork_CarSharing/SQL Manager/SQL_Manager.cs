using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CourseWork_CarSharing.SQL_Manager
{
    public class SQLiteManager
    {
        private SQLiteConnection connection;

        public SQLiteManager()
        {
            string connectionString = "Data Source='C:\\DataBases\\CarHouse';Version=3;";
            connection = new SQLiteConnection(connectionString);
        }
        public SQLiteConnection Connection
        {
            get { return connection; }
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public void ExecuteNonQuery(string query)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public int GetRowCount(string tableName)
        {
            connection.Open();

            using (SQLiteCommand command = new SQLiteCommand())
            {
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(*) FROM {tableName}";

                int rowCount = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
                return rowCount;
            }
        }

    }

}