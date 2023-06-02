using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CourseWork_CarSharing.SQL_Manager;

namespace CourseWork_CarSharing.Functions
{
    public class Functions
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static void ReadUsers(SQLiteManager manager, Label TextBoxNameNotify)
        {
            manager.OpenConnection();

            string selectQuery = "SELECT * FROM Users;";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, manager.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        string surname = reader["Surname"].ToString();
                        string email = reader["Email"].ToString();
                        string password = reader["Password"].ToString();
                        TextBoxNameNotify.Content = $"Name: {name}, Surname: {surname}, Email: {email}, Password: {password}";
                    }
                }
            }

            manager.CloseConnection();
        }
    }
}