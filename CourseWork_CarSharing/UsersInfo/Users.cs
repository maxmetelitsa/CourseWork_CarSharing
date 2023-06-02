using CourseWork_CarSharing.SQL_Manager;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CourseWork_CarSharing.UsersInfo
{
    public class Users
    {
        public List <User> users;
        public Users(SQLiteManager manager)
        {
            users = new List<User>(); // Инициализация списка пользователей
            GetAllUsers(manager);
        }
        public bool AddUser(SQLiteManager manager, string name, string surname, string email, string password)
        {
            manager.OpenConnection();

            // Используйте параметры в SQL-запросе, чтобы избежать проблем с безопасностью и экранированием символов
            //foreach (User user in users)
            //{
            //    if (user.Email == email)
            //    {
            //        return false;
            //    }
            //}

            string insertQuery = " INSERT INTO Users (Name, Surname, Email, Password) VALUES (@Name, @Surname, @Email, @Password);";

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
                    users.Add(new User(name, surname, email, password));
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

        public void UpdateAllUsers(SQLiteManager manager)
        {
            manager.OpenConnection();

            foreach (User user in users)
            {
                string updateQuery = $"UPDATE Users SET Name = @Name, Surname = @Surname, Email = @Email, Password = @Password WHERE ID = @{user.ID}";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, manager.Connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Surname", user.Surname);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    command.ExecuteNonQuery();
                }
            }

            manager.CloseConnection();
        }

        public void GetAllUsers(SQLiteManager manager)
        {
            manager.OpenConnection();

            string selectQuery = "SELECT * FROM Users";

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
                        users.Add(new User(name, surname, email, password));
                    }
                }
            }

            manager.CloseConnection();
        }
        public bool ValidateUser(string email, string password)
        {
            foreach (User user in users)
            {
                if (user.Email == email && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
