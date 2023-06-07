using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using CourseWork_CarSharing.CarsInfo;
using CourseWork_CarSharing.Enums;
using CourseWork_CarSharing.SQL_Manager;
using CourseWork_CarSharing.UsersInfo;

namespace CourseWork_CarSharing.CarPark
{
    /// <summary>
    /// Логика взаимодействия для CarParkWindow.xaml
    /// </summary>
    public partial class CarParkWindow : Window
    {
        private SQLiteManager manager;
        private Cars carsList;
        private bool isMaximized = false;

        public CarParkWindow()
        {
            InitializeComponent();

            manager = new SQLiteManager();
            carsList = new Cars(manager);

            //AddCar(manager,"Porsche", Fuel.Electricity, Transmission.Automatic, "black", 2022, 2);
            //AddCar(manager,"BMW", Fuel.Electricity, Transmission.Automatic, "black", 2020, 4);
            //AddCar(manager,"Mercedes", Fuel.Electricity, Transmission.Automatic, "white", 2020, 2);
            //AddCar(manager,"Audi", Fuel.Petrol, Transmission.Manual, "black", 2014, 5);

            GetAllCars(manager);

            ShowCars(carGrid, carsList.cars);
            //UpdateAllCars(manager);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        public void AddCarToGrid(string name, Fuel fuelType, Transmission transmission, string colour, int yearOfManufacture, int amount)
        {
            Car car = new Car(name, fuelType, transmission, colour, yearOfManufacture, amount);
            carsList.cars.Add(car);

            TextBlock carInfo = new TextBlock();
            carInfo.Text = $"Марка: {car.Name}, Модель: {car.Colour}";
            carInfo.Foreground = Brushes.White;

            //carGrid.Children.Add(carInfo);
        }
        public void UpdateAllCars(SQLiteManager manager)
        {
            manager.OpenConnection();

            foreach (Car car in carsList.cars)
            {
                string updateQuery = $"UPDATE Cars SET Name = @Name, FuelType = @FuelType, TransmissionType = @TransmissionType, Colour = @Colour, YearOfManufacture = @YearOfManufacture, Amount = @Amount WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, manager.Connection))
                {
                    command.Parameters.AddWithValue("@Name", car.Name);
                    command.Parameters.AddWithValue("@FuelType", (int)car.FuelType);
                    command.Parameters.AddWithValue("@TransmissionType", (int)car.TransmissionType);
                    command.Parameters.AddWithValue("@Colour", car.Colour);
                    command.Parameters.AddWithValue("@YearOfManufacture", car.YearOfManufacture);
                    command.Parameters.AddWithValue("@Amount", car.Amount);
                    command.Parameters.AddWithValue("@ID", car.ID);

                    command.ExecuteNonQuery();
                }
            }

            manager.CloseConnection();
        }
        public void GetAllCars(SQLiteManager manager)
        {
            manager.OpenConnection();

            string selectQuery = "SELECT * FROM Cars";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, manager.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(reader.GetOrdinal("Name"));
                        Fuel fuelType = (Fuel)reader.GetInt32(reader.GetOrdinal("FuelType"));
                        Transmission transmission = (Transmission)reader.GetInt32(reader.GetOrdinal("TransmissionType"));
                        string colour = reader.GetString(reader.GetOrdinal("Colour"));
                        int yearOfManufacture = reader.GetInt32(reader.GetOrdinal("YearOfManufacture"));
                        int amount = reader.GetInt32(reader.GetOrdinal("Amount"));
                        carsList.cars.Add(new Car(name, fuelType, transmission, colour, yearOfManufacture, amount));
                    }
                }
            }

            manager.CloseConnection();
        }
        public bool AddCar(SQLiteManager manager, string name, Fuel fuelType, Transmission transmission, string colour, int yearOfManufacture, int amount)
        {
            manager.OpenConnection();

            // Use parameters in the SQL query to avoid security and character escaping issues

            foreach (Car car in carsList.cars)
            {
                if (car.Name == name && car.FuelType == fuelType && car.TransmissionType == transmission && car.Colour == colour && car.YearOfManufacture == yearOfManufacture)
                {
                    manager.CloseConnection();
                    return false;
                }
            }

            string insertQuery = "INSERT INTO Cars (Name, FuelType, TransmissionType, Colour, YearOfManufacture, Amount) VALUES (@Name, @FuelType, @TransmissionType, @Colour, @YearOfManufacture, @Amount);";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@FuelType", (int)fuelType);
                command.Parameters.AddWithValue("@TransmissionType", (int)transmission);
                command.Parameters.AddWithValue("@Colour", colour);
                command.Parameters.AddWithValue("@YearOfManufacture", yearOfManufacture);
                command.Parameters.AddWithValue("@Amount", amount);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Data successfully added
                    carsList.cars.Add(new Car(name, fuelType, transmission, colour, yearOfManufacture, amount));
                    manager.CloseConnection();
                    return true;
                }
                else
                {
                    // Failed to add data
                    manager.CloseConnection();
                    return false;
                }
            }
        }

        private void CarButton_Click(object sender, RoutedEventArgs e)
        {
            Button carButton = (Button)sender;
        }

        public void ShowCars(WrapPanel carGrid, List<Car> cars)
        {
            foreach (Car car in cars)
            {
                Button carButton = new Button();
                carButton.Click += CarButton_Click;

                TextBlock carInfo = new TextBlock();
                carInfo.Text = $"Name: {car.Name}\nFuelType: {car.FuelType}\nTransmissionType: {car.TransmissionType}\nColour: {car.Colour}\nYearOfManufacture: {car.YearOfManufacture}\nAmount: {car.Amount}";
                carInfo.Foreground = Brushes.White;

                carButton.Content = carInfo;

                carGrid.Children.Add(carButton);
            }
        }

        private void WindowHide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void WindowOpenFull_Click(object sender, RoutedEventArgs e)
        {
            if (isMaximized)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1024;
                this.Height = 720;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.Width = 1920;
                this.Height = 1080;
            }

            isMaximized = !isMaximized;
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1024;
                    this.Height = 720;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    this.Width = 1920;
                    this.Height = 1080;
                }

                isMaximized = !isMaximized;
            }
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Rent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CarPark1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
