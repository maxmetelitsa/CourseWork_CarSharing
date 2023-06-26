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
using CourseWork_CarSharing.Authorization;
using CourseWork_CarSharing.CarsInfo;
using CourseWork_CarSharing.OrdersInfo;
using CourseWork_CarSharing.UserOrders;
using System.Diagnostics;

namespace CourseWork_CarSharing.Profile
{
    /// <summary>
    /// Логика взаимодействия для TestMainWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private bool isMaximize = false;
        public SQLiteManager manager;
        public OrdersManager ordersManager;
        public UserOrderManager userOrdersManager;
        public ProfileWindow()
        {
            manager = new SQLiteManager();
            ordersManager = new OrdersManager();
            userOrdersManager = new UserOrderManager();
            CurrentUser currentUserData = CurrentUserManager.CurrentUser;
            userOrdersManager.userOrdersList.GetUserOrders(currentUserData.ID);
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

            ShowUserOrders();
            manager.CloseConnection();
        }
        public void ShowUserOrders()
        {
            RentalsListBox.ItemsSource = userOrdersManager.userOrdersList.currentUserOrders;

            var stackPanel = FindDescendantByName<StackPanel>(RentalsListBox, "BookingStackPanel");
            var bookingTextBlock = FindDescendantByName<TextBlock>(stackPanel, "BookingTextBlock");

            if (bookingTextBlock != null)
            {
                string bookingText = "Бронирование";

                foreach (UserOrder userOrder in userOrdersManager.userOrdersList.currentUserOrders)
                {
                    bookingText = $"Бронирование {userOrder.ID}";
                }

                bookingTextBlock.Text = bookingText;
            }
        }



        public static T FindDescendantByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            if (parent == null)
                return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T namedChild && (namedChild as FrameworkElement)?.Name == name)
                {
                    foundChild = namedChild;
                    break;
                }

                foundChild = FindDescendantByName<T>(child, name);
                if (foundChild != null)
                    break;
            }

            return foundChild;
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


                if (ValidateUserFields(name, surname, email, password))
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
            PasswordTextBox.Text = null;
            PasswordRepeatTextBox.Text = null;
            PassportNumberTextBox.Text = null;
            IdentificationTextBox.Text = null;
            LicenseSeriesTextBox.Text = null;
            LicenseNumberTextBox.Text = null;
        }
        public bool ValidateUserFields(string name, string surname, string email, string password)
        {

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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
            NameTextBox.IsReadOnly = true;
            SurnameTextBox.IsReadOnly = true;
            EmailTextBox.IsReadOnly = true;
            PasswordTextBox.IsReadOnly = false;
            PasswordRepeatTextBox.IsReadOnly = false;
            PassportNumberTextBox.IsReadOnly = false;
            IdentificationTextBox.IsReadOnly = false;
            LicenseSeriesTextBox.IsReadOnly = false;
            LicenseNumberTextBox.IsReadOnly = false;
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            SetWriteAbleFields();
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
            Functions.Functions.TerminateProcess("CourseWork_CarSharing");
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
            //AboutWindow window = new AboutWindow();
            //window.Show();
            //this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow window = new SignInWindow();
            window.Show();
            this.Hide();
        }

        private void RentalsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = RentalsListBox.SelectedItem as UserOrder;

            if (selectedItem != null)
            {
                string filePath = $"C:\\Лабораторные работы C#\\CourseWork_CarSharing\\CourseWork_CarSharing\\UserRentals\\user_order{selectedItem.ID}.docx";

                try
                {
                    // Получить информацию о каталоге
                    string directoryPath = System.IO.Path.GetDirectoryName(filePath);
                    System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(directoryPath);

                    // Обновить содержимое каталога
                    directoryInfo.Refresh();

                    // Открыть документ
                    Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при открытии файла: " + ex.Message);
                }
            }
        }

    }
}