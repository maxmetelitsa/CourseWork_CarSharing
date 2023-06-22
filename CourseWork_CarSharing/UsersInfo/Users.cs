using CourseWork_CarSharing.SQL_Manager;
using DocumentFormat.OpenXml.ExtendedProperties;
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
        public List<User> users;
        public Users(SQLiteManager manager)
        {
            users = new List<User>(); // Инициализация списка пользователей
            GetAllUsers(manager);
        }
        public bool AddUser(SQLiteManager manager, string name, string surname, string email, string password)
        {
            manager.OpenConnection();

            string insertQuery = " INSERT INTO Users (Name, Surname, Email, Password, PassportNumber, IdentificationNumber, LicenseSeries, LicenseNumber) VALUES (@Name, @Surname, @Email, @Password, @PassportNumber, @IdentificationNumber, @LicenseSeries, @LicenseNumber);";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@PassportNumber", null);
                command.Parameters.AddWithValue("@IdentificationNumber", null);
                command.Parameters.AddWithValue("@LicenseSeries", null);
                command.Parameters.AddWithValue("@LicenseNumber", null);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    users.Add(new User(name, surname, email, password));
                    manager.CloseConnection();
                    return true;
                }
                else
                {
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
       
        public int ValidateUser(SQLiteManager manager, string email, string password)
        {
            manager.OpenConnection();

            string selectQuery = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Int64 id = Convert.ToInt64(reader["ID"].ToString());
                        string nameFromDB = reader["Name"].ToString();
                        string surnameFromDB = reader["Surname"].ToString();
                        string emailFromDB = reader["Email"].ToString();
                        string passwordFromDB = reader["Password"].ToString();
                        string passportNumberFromDB = reader["PassportNumber"].ToString();
                        string identificationNumberFromDB = reader["IdentificationNumber"].ToString();
                        string licenseSeriesFromDB = reader["LicenseSeries"].ToString();
                        string licenseNumberFromDB = reader["LicenseNumber"].ToString();

                        CurrentUser currentUser = new CurrentUser((int)id,nameFromDB, surnameFromDB, emailFromDB, passwordFromDB,
                            passportNumberFromDB, identificationNumberFromDB, licenseSeriesFromDB, licenseNumberFromDB);
                        CurrentUserManager.CurrentUser = currentUser;

                        manager.CloseConnection();
                        GetAllUsers(manager);

                        return 1;
                    }
                }
            }

            if (email == "admin@icloud.com" && password == "1234")
            {
                CurrentUser currentUser = new CurrentUser("Admin", "Manager", email, password);
                CurrentUserManager.CurrentUser = currentUser;
                manager.CloseConnection();
                return 2;
            }

            manager.CloseConnection();

            return 0;
        }
    }
}
