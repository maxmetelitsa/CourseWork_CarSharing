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
using CourseWork_CarSharing.CarsInfo;
using CourseWork_CarSharing.UsersInfo;
using System.Data.SQLite;
using DocumentFormat.OpenXml.Vml;
using CourseWork_CarSharing.Enums;
using CourseWork_CarSharing.OrdersInfo;
using CourseWork_CarSharing.CompanyInfo;
using DocumentFormat.OpenXml.ExtendedProperties;
using CourseWork_CarSharing.Authorization;
using CourseWork_CarSharing.UserOrders;

    
namespace CourseWork_CarSharing.Rent
{
    /// <summary>
    /// Логика взаимодействия для TestMainWindow.xaml
    /// </summary>
    public partial class RentCarWindow : Window
    {
        private CarParkManager carParkManager;
        private OrdersManager ordersManager;
        private UserOrderManager userOrdersManager;
        private bool isMaximize = false;
        Car car;
        CurrentUser currentUserData = CurrentUserManager.CurrentUser;
        static int ordersCounter = 0;

        public Car Car
        {
            get { return car; }
            set { car = value; }
        }
        public RentCarWindow(Car car)
        {
            InitializeComponent();
            Car = car;
            carParkManager = new CarParkManager();
            ordersManager = new OrdersManager();
            userOrdersManager = new UserOrderManager();
            ShowCar(rentalCar);
            ShowInfo();
        }
        private void ShowInfo()
        {

            SetReadAbleFields();

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
            carButton.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            carButton.BorderThickness = new Thickness(2);
            carButton.Height = 355;
            carButton.Width = 512;
            carButton.Background = new SolidColorBrush(Color.FromRgb(33, 33, 33));

            StackPanel carPanel = new StackPanel();
            carPanel.Orientation = Orientation.Horizontal;
            carPanel.Height = 355;
            carPanel.Width = 512;

            TextBlock carInfo = new TextBlock();
            carInfo.Text = $"Название: {car.Name}\nМарка: {car.Brand}\nКласс: {car.CarType}\nТопливо: {car.FuelType}\nКоробка: {car.TransmissionType}\nЦвет: {car.Colour}\nГод выпуска: {car.YearOfManufacture}\nНомер: {car.Number}\nСтоимость/День: {car.HourPrice} р";
            carInfo.Foreground = Brushes.White;
            carInfo.FontSize = 14;
            carInfo.TextAlignment = TextAlignment.Left;
            carInfo.Margin = new Thickness(30, 55, 10, 10);

            string imagePath = @"C:\Лабораторные работы C#\CourseWork_CarSharing\CourseWork_CarSharing\Images\pic" + Car.ImageID + ".jpg";
            Image image = new Image();
            image.Height = 355;
            image.Width = 250;
            image.Margin = new Thickness(20, 0, 0, 0);
            image.HorizontalAlignment = HorizontalAlignment.Left;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;

            carPanel.Children.Add(image);

            carPanel.Children.Add(carInfo);

            carButton.Content = carPanel;

            carObject.Children.Add(carButton);
        }
        public void GetAmountOfOrders()
        {
            ordersManager.manager.OpenConnection();

            string selectQuery = "SELECT COUNT(*) FROM [Order]";
            int rowCount = 0;

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, ordersManager.manager.Connection))
            {
                rowCount = Convert.ToInt32(command.ExecuteScalar());
            }

            ordersManager.manager.CloseConnection();

            ordersCounter = rowCount;
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
                GetAmountOfOrders();
                string companyRentals = $"C:\\Rentals\\rental_order{++ordersCounter}.docx";
                company.GenerateRentalDocument(companyRentals, Car,name, surname, email, passportNumber, identificationNumber, licenseSeries, licenseNumber, startDate, endDate, days, total);
                string personRentals = $"C:\\Лабораторные работы C#\\CourseWork_CarSharing\\CourseWork_CarSharing\\UserRentals\\user_order{ordersCounter}.docx";
                company.GenerateRentalDocument(personRentals, Car, name, surname, email, passportNumber, identificationNumber, licenseSeries, licenseNumber, startDate, endDate, days, total);

                string directoryCompanyPath = System.IO.Path.GetDirectoryName(companyRentals);
                System.IO.DirectoryInfo directoryInfo1 = new System.IO.DirectoryInfo(directoryCompanyPath);

                string directoryuserPath = System.IO.Path.GetDirectoryName(personRentals);
                System.IO.DirectoryInfo directoryInfo2 = new System.IO.DirectoryInfo(directoryuserPath);

                // Обновить содержимое каталога
                directoryInfo1.Refresh();
                directoryInfo2.Refresh();

                ordersManager.AddOrder(currentUserData.ID, Car.ID, startDate, endDate, total);
                userOrdersManager.userOrdersList.AddUserOrder(--ordersCounter, currentUserData.ID, ++ordersCounter);
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

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowPrice();
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowPrice();
        }
        private void ShowPrice()
        {
            int maxRentalDays = 31, minRentalDays = 1;
            if (StartDatePicker.SelectedDate != null && EndDatePicker.SelectedDate != null)
            {
                DateTime selectedStartDate = StartDatePicker.SelectedDate.GetValueOrDefault();
                DateTime selectedEndDate = EndDatePicker.SelectedDate.GetValueOrDefault();

                int days = selectedEndDate.DayOfYear - selectedStartDate.DayOfYear;
                if (days >= maxRentalDays)
                {
                    MessageBox.Show($"Количество дней аренды не должно превышать {maxRentalDays}");
                    TotalTextBox.Text = null;
                    DaysTextBox.Text = null;

                }
                else if (days < minRentalDays)
                {
                    MessageBox.Show($"Минимальное количество дней для аренды - {minRentalDays}");
                    TotalTextBox.Text = null;
                    DaysTextBox.Text = null;
                }
                else
                {
                    double total = days * Car.HourPrice;
                    TotalTextBox.Text = total.ToString();
                    DaysTextBox.Text = days.ToString();
                }
            }
            else
            {
                TotalTextBox.Text = null;
                DaysTextBox.Text = null;
            }
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
            if ((int)DateTime.Parse(StartDatePicker.Text).DayOfYear < (int)DateTime.Now.DayOfYear - 1)
            {
                // Обработка неверного формата даты
                MessageBox.Show($"Неверное значение начальной даты");
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
            ProfileWindow window = new ProfileWindow();
            window.Show();
            this.Close();
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
    }
}