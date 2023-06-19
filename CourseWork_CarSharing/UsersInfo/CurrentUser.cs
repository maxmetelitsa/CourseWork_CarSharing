using CourseWork_CarSharing.SQL_Manager;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork_CarSharing.UsersInfo
{
    public class CurrentUser
    {
        public int id;
        string name;
        string surname;
        string email;
        string password;
        string passportNumber;
        string identificationNumber;
        string licenseSeries;
        string licenseNumber;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string PassportNumber
        {
            get { return passportNumber; }
            set { passportNumber = value; }
        }
        public string IdentificationNumber
        {
            get { return identificationNumber; }
            set { identificationNumber = value; }
        }
        public string LicenseSeries
        {
            get { return licenseSeries; }
            set { licenseSeries = value; }
        }
        public string LicenseNumber
        {
            get { return licenseNumber; }
            set { licenseNumber = value; }
        }

        public CurrentUser(int id, string name, string surname, string email, string password, string passportNumber, string identificationNumber, string licenseSeries, string licenseNumber)
        {
            ID = id;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            PassportNumber = passportNumber;
            IdentificationNumber = identificationNumber;
            LicenseSeries = licenseSeries;
            LicenseNumber = licenseNumber;
        }
        public CurrentUser(string name, string surname, string email, string password)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
        }
        public void GetUser(SQLiteManager manager)
        {
            manager.OpenConnection();

            CurrentUser currentUser = CurrentUserManager.CurrentUser;
            int userID = currentUser.ID; // Предполагается, что у класса CurrentUser есть свойство ID

            string selectQuery = $"SELECT * FROM Users WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@ID", userID);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nameFromDB = reader["Name"].ToString();
                        string surnameFromDB = reader["Surname"].ToString();
                        string emailFromDB = reader["Email"].ToString();
                        string passwordFromDB = reader["Password"].ToString();
                        string passportNumberFromDB = reader["PassportNumber"].ToString();
                        string identificationNumberFromDB = reader["IdentificationNumber"].ToString();
                        string licenseSeriesFromDB = reader["LicenseSeries"].ToString();
                        string licenseNumberFromDB = reader["LicenseNumber"].ToString();

                        currentUser.Name = nameFromDB;
                        currentUser.Surname = surnameFromDB;
                        currentUser.Email = emailFromDB;
                        currentUser.Password = passwordFromDB;
                        currentUser.PassportNumber = passportNumberFromDB;
                        currentUser.IdentificationNumber = identificationNumberFromDB;
                        currentUser.LicenseSeries = licenseSeriesFromDB;
                        currentUser.LicenseNumber = licenseNumberFromDB;
                    }
                }
            }

            manager.CloseConnection();
        }

    }
}
