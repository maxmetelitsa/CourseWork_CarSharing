﻿using System;
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
using CourseWork_CarSharing.CarsInfo;
using CourseWork_CarSharing.Functions;
using DocumentFormat.OpenXml.ExtendedProperties;
using CourseWork_CarSharing.UsersInfo;
using System.Data.SQLite;
using CourseWork_CarSharing.OrdersInfo;
using CourseWork_CarSharing.Authorization;

namespace CourseWork_CarSharing.Admin
{
    /// <summary>
    /// Логика взаимодействия для TestMainWindow.xaml
    /// </summary>
    public partial class AdminRentCarWindow : Window
    {
        private CarParkManager carParkManager;
        private OrdersManager ordersManager;
        private bool isMaximize = false;
        Car car;
        CurrentUser currentUserData = CurrentUserManager.CurrentUser;
        static int ordersCounter = 0;

        public Car Car
        {
            get { return car; }
            set { car = value; }
        }
        public AdminRentCarWindow(Car car)
        {
            Car = car;
            carParkManager = new CarParkManager();
            ordersManager = new OrdersManager();
            ShowCar(rentalCar);
            ShowInfo();
        }
        private void ShowInfo()
        {

            SetReadAbleFields();

            CurrentUser currentUserData = CurrentUserManager.CurrentUser;

            carParkManager.manager.OpenConnection();

            string selectQuery = $"SELECT * FROM Users WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, carParkManager.manager.Connection))
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
                        PassportNumberTextBox.Text = identificationNumberFromDB;
                        IdentificationTextBox.Text = licenseSeriesFromDB;
                        LicenseSeriesTextBox.Text = licenseSeriesFromDB;
                        LicenseNumberTextBox.Text = licenseNumberFromDB;
                    }
                }
            }
            carParkManager.manager.CloseConnection();
        }
        public void ShowCar(WrapPanel carObject)
        {
            carObject = FindName("rentalCar") as WrapPanel;

            Button carButton = new Button();
            carButton.Tag = Car;
            //carButton.Style = (Style)System.Windows.Application.Current.Resources["NoHoverButtonStyle"];
            carButton.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            carButton.BorderThickness = new Thickness(5);
            carButton.Height = 590;
            carButton.Width = 512;
            carButton.Background = new SolidColorBrush(Color.FromRgb(33, 33, 33));

            StackPanel carPanel = new StackPanel();
            carPanel.Orientation = Orientation.Vertical;
            carPanel.Height = 590;
            carPanel.Width = 512;

            string imagePath = @"C:\Лабораторные работы C#\CourseWork_CarSharing\CourseWork_CarSharing\Images\pic" + Car.ImageID + ".jpg";
            Image image = new Image();
            image.Height = 320;
            image.Width = 512;
            image.Margin = new Thickness(0, 30, 0, 0);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;

            TextBlock carInfo = new TextBlock();
            carInfo.Text = $"Name: {Car.Name}\nBrand: {Car.Brand}\nCarType: {Car.CarType}\nFuelType: {Car.FuelType}\nTransmissionType: {Car.TransmissionType}\nColour: {Car.Colour}\nYearOfManufacture: {Car.YearOfManufacture}\nNumber: {Car.Number}\nPrice/Day: {Car.HourPrice} $";
            carInfo.Foreground = Brushes.White;
            carInfo.FontSize = 16;
            carInfo.TextAlignment = TextAlignment.Left;
            carInfo.Margin = new Thickness(98, 10, 10, 10);

            carPanel.Children.Add(image);
            carPanel.Children.Add(carInfo);

            carButton.Content = carPanel;

            carObject.Children.Add(carButton);
        }


        private void SetReadAbleFields()
        {
            NameTextBox.IsReadOnly = true;
            SurnameTextBox.IsReadOnly = true;
            EmailTextBox.IsReadOnly = true;
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
            PassportNumberTextBox.IsReadOnly = false;
            IdentificationTextBox.IsReadOnly = false;
            LicenseSeriesTextBox.IsReadOnly = false;
            LicenseNumberTextBox.IsReadOnly = false;
        }
        private bool ValidateFields()
        {
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string email = EmailTextBox.Text;
            string passportNumber = PassportNumberTextBox.Text;
            string identificationNumber = IdentificationTextBox.Text;
            string licenseSeries = LicenseSeriesTextBox.Text;
            string licenseNumber = LicenseNumberTextBox.Text;
            DateTime startDate;
            DateTime endDate;
            int days;
            double total;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(name))
            {
                NameNotify.Foreground = new SolidColorBrush(Colors.Red);
                NameNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                NameNotify.Content = "";
            }

            if (string.IsNullOrWhiteSpace(surname))
            {
                SurnameNotify.Foreground = new SolidColorBrush(Colors.Red);
                SurnameNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                SurnameNotify.Content = "";
            }

            if (string.IsNullOrWhiteSpace(passportNumber))
            {
                PassportNumberNotify.Foreground = new SolidColorBrush(Colors.Red);
                PassportNumberNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                PassportNumberNotify.Content = "";
            }

            if (string.IsNullOrWhiteSpace(identificationNumber))
            {
                IdentificationNotify.Foreground = new SolidColorBrush(Colors.Red);
                IdentificationNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                IdentificationNotify.Content = "";
            }

            if (string.IsNullOrWhiteSpace(licenseSeries))
            {
                LicenseSeriesNotify.Foreground = new SolidColorBrush(Colors.Red);
                LicenseSeriesNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                LicenseSeriesNotify.Content = "";
            }

            if (string.IsNullOrWhiteSpace(licenseNumber))
            {
                LicenseNumberNotify.Foreground = new SolidColorBrush(Colors.Red);
                LicenseNumberNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                LicenseNumberNotify.Content = "";
            }

            if (!DateTime.TryParse(StartDatePicker.Text, out startDate))
            {
                // Обработка неверного формата даты
                isValid = false;
            }

            if (!DateTime.TryParse(EndDatePicker.Text, out endDate))
            {
                // Обработка неверного формата даты
                isValid = false;
            }

            if (!int.TryParse(DaysTextBox.Text, out days))
            {
                // Обработка неверного формата числа дней
                isValid = false;
            }

            if (!double.TryParse(TotalTextBox.Text, out total))
            {
                // Обработка неверного формата общей стоимости
                isValid = false;
            }

            return isValid;
        }
        public void GetAmountOfOrders()
        {
            ordersManager.manager.OpenConnection();
            string selectQuery = "SELECT * FROM [Order]";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, ordersManager.manager.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordersCounter++;
                    }
                }
            }

            ordersManager.manager.CloseConnection();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields() == true)
            {
                CompanyInfo.Company company = new CompanyInfo.Company("Car House", "в Минске 220036, Беларусь, Минск, ул. Карла Либкнехта 129",
                        "+375 44 547 22 52 ", "carhouse_minsk@info.by", "Альфа-Банк", "BY51ALPHA301840GHB70010290000", "BY51ALPHA2873197VTB40020130000", "153001550", "790110246");
                string name = NameTextBox.Text;
                string surname = SurnameTextBox.Text;
                string email = EmailTextBox.Text;
                string passportNumber = PassportNumberTextBox.Text;
                string identificationNumber = IdentificationTextBox.Text;
                string licenseSeries = LicenseSeriesTextBox.Text;
                string licenseNumber = LicenseNumberTextBox.Text;
                DateTime startDate = DateTime.Parse(StartDatePicker.Text);
                DateTime endDate = DateTime.Parse(EndDatePicker.Text);
                int days = int.Parse(DaysTextBox.Text);
                double total = double.Parse(TotalTextBox.Text);
                ordersManager.AddOrder(currentUserData.ID, Car.ID, startDate, endDate, total);
                GetAmountOfOrders();
                string path = $"C:\\Rentals\\rental_order{++ordersCounter}.docx";
                company.GenerateRentalDocument(path, Car, name, surname, email, passportNumber, identificationNumber, licenseSeries, licenseNumber, startDate, endDate, days, total);
                MessageBox.Show("Автомобиль успешно забронирован");
                ProfileWindow window = new ProfileWindow();
                window.Show();
                this.Hide();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SetWriteAbleFields();
        }

        //bar
        private void CarParkButton_Click(object sender, RoutedEventArgs e)
        {
            AdminCarParkWindow window = new AdminCarParkWindow();
            window.Show();
            this.Close();
        }

        private void RentButton_Click(object sender, RoutedEventArgs e)
        {
            AdminRentWindow window = new AdminRentWindow();
            window.Show();
            this.Close();
        }

        private void CarParkEditingButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow();
            window.Show();
            this.Hide();
        }

        private void RentalTrackingButton_Click(object sender, RoutedEventArgs e)
        {
            AdminRentalTracking window = new AdminRentalTracking();
            window.Show();
            this.Close();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow window = new SignInWindow();
            window.Show();
            this.Hide();
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

    }
}