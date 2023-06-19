using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using CourseWork_CarSharing.CarPark;
using CourseWork_CarSharing.Rent;
using CourseWork_CarSharing.Profile;
using CourseWork_CarSharing.About;
using CourseWork_CarSharing.UsersInfo;
using CourseWork_CarSharing.Enums;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Data.SQLite;
using CourseWork_CarSharing.SQL_Manager;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CourseWork_CarSharing.Profile
{
    /// <summary>
    /// Логика взаимодействия для TestMainWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private bool isMaximize = false;
        public SQLiteManager manager;
        public ProfileWindow()
        {
            manager = new SQLiteManager();
            InitializeComponent();
            ShowInfo();
        }
        private void ShowInfo()
        {
            SetReadAbleFields();

            CurrentUser currentUserData = CurrentUserManager.CurrentUser;

            manager.OpenConnection();

            string selectQuery = $"SELECT * FROM Users WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@ID", currentUserData.ID);

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

                        currentUserData.Name = nameFromDB;
                        currentUserData.Surname = surnameFromDB;
                        currentUserData.Email = emailFromDB;
                        currentUserData.Password = passwordFromDB;
                        currentUserData.PassportNumber = passportNumberFromDB;
                        currentUserData.IdentificationNumber = identificationNumberFromDB;
                        currentUserData.LicenseSeries = licenseSeriesFromDB;
                        currentUserData.LicenseNumber = licenseNumberFromDB;

                        NameTextBox.Text = nameFromDB;
                        SurnameTextBox.Text = surnameFromDB;
                        EmailTextBox.Text = emailFromDB;
                        PasswordTextBox.Text = passwordFromDB;
                        PasswordRepeatTextBox.Text = passwordFromDB;
                        PassportNumberTextBox.Text = identificationNumberFromDB;
                        IdentificationTextBox.Text = licenseSeriesFromDB;
                        LicenseSeriesTextBox.Text = licenseSeriesFromDB;
                        LicenseNumberTextBox.Text = licenseNumberFromDB;
                    }
                }
            }
            manager.CloseConnection();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1024;
                    this.Height = 720;

                    isMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    this.Width = 1024;
                    this.Height = 720;

                    isMaximize = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void WindowHide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void WindowOpenFull_Click(object sender, RoutedEventArgs e)
        {
            if (isMaximize)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1024;
                this.Height = 720;

                isMaximize = false;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.Width = 1024;
                this.Height = 720;

                isMaximize = true;
            }
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CarParkButton_Click(object sender, RoutedEventArgs e)
        {
            CarParkWindow window = new CarParkWindow();
            window.Show();
            this.Close();
        }

        private void RentButton_Click(object sender, RoutedEventArgs e)
        {
            RentWindow window = new RentWindow();
            window.Show();
            this.Close();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
            this.Close();
        }
        private void SetReadAbleFields()
        {
            NameTextBox.IsReadOnly = true;
            SurnameTextBox.IsReadOnly = true;
            EmailTextBox.IsReadOnly = true;
            PasswordTextBox.IsReadOnly = true;
            PasswordRepeatTextBox.IsReadOnly = true;
            PassportNumberTextBox.IsReadOnly = true;
            IdentificationTextBox.IsReadOnly = true;
            LicenseSeriesTextBox.IsReadOnly = true;
            LicenseNumberTextBox.IsReadOnly = true;
        }
        private void SetWriteAbleFields()
        {
            NameTextBox.IsReadOnly = false;
            SurnameTextBox.IsReadOnly = false;
            EmailTextBox.IsReadOnly = true;
            PasswordTextBox.IsReadOnly = false;
            PasswordRepeatTextBox.IsReadOnly = false;
            PassportNumberTextBox.IsReadOnly = false;
            IdentificationTextBox.IsReadOnly = false;
            LicenseSeriesTextBox.IsReadOnly = false;
            LicenseNumberTextBox.IsReadOnly = false;
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            SetWriteAbleFields();
        }

        public void UpdateUserInfo(string name, string surname, string email, string password, string passportNumber, string identificationNumber, string licenseSeries, string licenseNumber)
        {
            if (PasswordTextBox.Text != PasswordRepeatTextBox.Text)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            else
            {

                CurrentUser currentUserData = CurrentUserManager.CurrentUser;


                if (ValidateUserFields(name, surname, email, password, passportNumber, identificationNumber, licenseSeries, licenseNumber))
                {
                    manager.OpenConnection();

                    string updateQuery = "UPDATE Users SET Name = @Name, Surname = @Surname, Email = @Email, Password = @Password, PassportNumber = @PassportNumber, IdentificationNumber = @IdentificationNumber, LicenseSeries = @LicenseSeries, LicenseNumber = @LicenseNumber WHERE ID = @ID";

                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, manager.Connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Surname", surname);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@PassportNumber", passportNumber);
                        command.Parameters.AddWithValue("@IdentificationNumber", identificationNumber);
                        command.Parameters.AddWithValue("@LicenseSeries", licenseSeries);
                        command.Parameters.AddWithValue("@LicenseNumber", licenseNumber);
                        command.Parameters.AddWithValue("@ID", currentUserData.ID);

                        command.ExecuteNonQuery();
                    }

                    manager.CloseConnection();

                    // Update the CurrentUser object with the new values
                    currentUserData.Name = name;
                    currentUserData.Surname = surname;
                    currentUserData.Email = email;
                    currentUserData.Password = password;
                    currentUserData.PassportNumber = passportNumber;
                    currentUserData.IdentificationNumber = identificationNumber;
                    currentUserData.LicenseSeries = licenseSeries;
                    currentUserData.LicenseNumber = licenseNumber;
                }
            }
        }

        private void SaveChangesProfileButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            string passportNumber = PassportNumberTextBox.Text;
            string identificationNumber = IdentificationTextBox.Text;
            string licenseSeries = LicenseSeriesTextBox.Text;
            string licenseNumber = LicenseNumberTextBox.Text;

            UpdateUserInfo(name, surname, email, password, passportNumber, identificationNumber, licenseSeries, licenseNumber);
            ShowInfo();
        }

        private void ClearFields()
        {
            NameTextBox.Text = null;
            SurnameTextBox.Text = null;
            EmailTextBox.Text = null;
            PasswordTextBox.Text = null;
            PasswordRepeatTextBox.Text = null;
            PassportNumberTextBox.Text = null;
            IdentificationTextBox.Text = null;
            LicenseSeriesTextBox.Text = null;
            LicenseNumberTextBox.Text = null;
        }
        public bool ValidateUserFields(string name, string surname, string email, string password, string passportNumber, string identificationNumber, string licenseSeries, string licenseNumber)
        {

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passportNumber) ||
                string.IsNullOrEmpty(identificationNumber) || string.IsNullOrEmpty(licenseSeries) ||
                string.IsNullOrEmpty(licenseNumber))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
    }
}