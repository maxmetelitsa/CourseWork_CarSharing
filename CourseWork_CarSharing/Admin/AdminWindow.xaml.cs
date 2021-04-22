using CourseWork_CarSharing.CarPark;
using CourseWork_CarSharing.CarsInfo;
using CourseWork_CarSharing.Enums;
using CourseWork_CarSharing.SQL_Manager;
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
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;

namespace CourseWork_CarSharing.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private CarParkManager carParkManager;
        private bool isMaximize = false;
        public AdminWindow()
        {
            InitializeComponent();

            carParkManager = new CarParkManager();

            carParkManager.GetAllCars();
            ShowCarsInDataGrid(carsGrid, carParkManager);
            List<string> fuelTypes = new List<string> { "Petrol", "Diesel", "Gas", "Electricity" };
            FuelTypeComboBox.ItemsSource = fuelTypes;
            List<string> transmissionTypes = new List<string> { "Automatic", "Manual" };
            TransmissionTypeComboBox.ItemsSource = transmissionTypes;

        }

        public void ShowCarsInDataGrid(DataGrid carsGrid, CarParkManager carParkManager)
        {
            carParkManager.manager.OpenConnection();

            string selectQuery = "SELECT * FROM Cars";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, carParkManager.manager.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    carsGrid.ItemsSource = dataTable.DefaultView;
                }
            }

            carParkManager.manager.CloseConnection();
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

        private void NewsButton_Click(object sender, RoutedEventArgs e)
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

        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CarAddButton_Click(object sender, RoutedEventArgs e)
        {

            string name = NameTextBox.Text;
            Fuel selectedFuel;
            Transmission selectedTransmission;

            if (!string.IsNullOrWhiteSpace(name) &&
                Enum.TryParse<Fuel>(FuelTypeComboBox.SelectedValue?.ToString(), out selectedFuel) &&
                Enum.TryParse<Transmission>(TransmissionTypeComboBox.SelectedValue?.ToString(), out selectedTransmission))
            {
                string colour = ColourTextBox.Text;
                int yearOfManufacture;
                int amount;
                int imageID;

                if (int.TryParse(YearOfManufactureTextBox.Text, out yearOfManufacture) &&
                    int.TryParse(AmountTextBox.Text, out amount) &&
                    int.TryParse(ImageIDTextBox.Text, out imageID))
                {
                    if (carParkManager.ValidateDataCar(name, selectedFuel, selectedTransmission, colour, yearOfManufacture, amount, imageID))
                    {
                        carParkManager.AddCar(name, selectedFuel, selectedTransmission, colour, yearOfManufacture, amount, imageID);
                        ShowCarsInDataGrid(carsGrid, carParkManager);
                    }
                }
                else
                {
                    // Обработка ошибок при некорректном вводе числовых значений
                    MessageBox.Show("Invalid numeric values entered.");
                }
            }
            else
            {
                // Обработка ошибок при некорректном выборе типов топлива или трансмиссии или пустом значении имени
                MessageBox.Show("Invalid fuel type or transmission selected, or empty name field.");
            }

        }


        private void CarDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли выбранный элемент в DataGrid
            if (carsGrid.SelectedItem != null)
            {
                // Получаем выбранный объект машины из DataGrid
                var rowView = carsGrid.SelectedItem as DataRowView;
                if (rowView != null)
                {
                    // Извлекаем значения столбцов из DataRowView
                    string name = rowView["Name"] as string;
                    Int64 fuelValue = (Int64)rowView["FuelType"];
                    Int64 transmissionValue = (Int64)rowView["TransmissionType"];
                    string colour = rowView["Colour"] as string;
                    Int64 yearOfManufacture = (Int64)rowView["YearOfManufacture"];
                    Int64 amount = (Int64)rowView["Amount"];
                    Int64 imageID = (Int64)rowView["ImageID"];

                    // Преобразуем целочисленное значение fuelValue в перечисление Fuel
                    Fuel selectedFuel = (Fuel)fuelValue;

                    // Преобразуем целочисленное значение transmissionValue в перечисление Transmission
                    Transmission selectedTransmission = (Transmission)transmissionValue;

                    // Создаем объект Car
                    Car selectedCar = new Car(name, selectedFuel, selectedTransmission, colour, (int)yearOfManufacture, (int)amount, (int)imageID);

                    // Удаляем машину из списка carParkManager
                    carParkManager.RemoveCar(selectedCar);

                    // Перезагружаем DataGrid для обновления отображения
                    ShowCarsInDataGrid(carsGrid, carParkManager);
                }
            }
            else
            {
                // Если не выбрана ни одна машина, выводим сообщение
                MessageBox.Show("No car selected.");
            }
        }



        private void CarEditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CarSaveChangesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FuelTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> fuelTypes = new List<string> { "Petrol", "Diesel", "Gas", "Electricity" };
            FuelTypeComboBox.ItemsSource = fuelTypes;
        }

        private void TransmissionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> transmissionTypes = new List<string> { "Automatic", "Manual" };
            TransmissionTypeComboBox.ItemsSource = transmissionTypes;
        }

    }
}
