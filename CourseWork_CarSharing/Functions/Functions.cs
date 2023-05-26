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
        public static bool addUser(SQLiteManager manager, string name, string surname, string email, string password)
        {
            manager.OpenConnection();

            // Используйте параметры в SQL-запросе, чтобы избежать проблем с безопасностью и экранированием символов
            string insertQuery = "INSERT INTO Users (Name, Surname, Email, Password) VALUES (@Name, @Surname, @Email, @Password);";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Данные успешно внесены
                    manager.CloseConnection();
                    return true;
                }
                else
                {
                    // Не удалось внести данные
                    manager.CloseConnection();
                    return false;
                }
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

        public static bool ValidateUser(SQLiteManager manager, string email, string password)
        {
            manager.OpenConnection();

            string selectQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password;";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                int count = Convert.ToInt32(command.ExecuteScalar());

                manager.CloseConnection();

                return count > 0;
            }
        }
    }
}