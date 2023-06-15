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
using System.Security.AccessControl;

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
            List<Fuel> fuelTypes = new List<Fuel> { Fuel.Petrol, Fuel.Diesel, Fuel.Gas, Fuel.Electricity, Fuel.Hybrid};
            FuelTypeComboBox.ItemsSource = fuelTypes;
            List<Transmission> transmissionTypes = new List<Transmission> { Transmission.Automatic, Transmission.Manual};
            TransmissionTypeComboBox.ItemsSource = transmissionTypes;
            List<CarType> carTypes = new List<CarType> { CarType.Economy,
            CarType.Business,
            CarType.SUV,
            CarType.CargoMinibus,
            CarType.Coupe,
            CarType.Limousine,
            CarType.Minibus,
            CarType.Motorcycle };
            CarTypeComboBox.ItemsSource = carTypes;
            List<Brand> brands = new List<Brand> {
            Brand.Toyota,
            Brand.Honda,
            Brand.Ford,
            Brand.BMW,
            Brand.MercedesBenz,
            Brand.Audi,
            Brand.Volkswagen,
            Brand.Tesla
            };
            BrandComboBox.ItemsSource = brands;
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
                    this.Height = 777;

                    isMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    this.Width = 1024;
                    this.Height = 777;

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
                this.Height = 777;

                isMaximize = false;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.Width = 1024;
                this.Height = 777;

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
            string number = NumberTextBox.Text;
            Fuel selectedFuel;
            Transmission selectedTransmission;
            CarType selectedCarType;
            Brand selectedBrand;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(number) 
                && Enum.TryParse<Fuel>(FuelTypeComboBox.SelectedValue?.ToString(), out selectedFuel) &&Enum.TryParse<Transmission>(TransmissionTypeComboBox.SelectedValue?.ToString(), out selectedTransmission)
                && Enum.TryParse<CarType>(FuelTypeComboBox.SelectedValue?.ToString(), out selectedCarType) && Enum.TryParse<Brand>(TransmissionTypeComboBox.SelectedValue?.ToString(), out selectedBrand))
            {
                string colour = ColourTextBox.Text;
                int yearOfManufacture;
                int imageID;

                if (int.TryParse(YearOfManufactureTextBox.Text, out yearOfManufacture) && int.TryParse(ImageIDTextBox.Text, out imageID))
                {
                    if (carParkManager.ValidateDataCar(name, selectedFuel, selectedTransmission, selectedCarType, selectedBrand, colour, yearOfManufacture, number, imageID))
                    {
                        carParkManager.AddCar(name, selectedFuel, selectedTransmission, selectedCarType, selectedBrand, colour, yearOfManufacture, number, imageID);
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
                    Int64 carTypeValue = (Int64)rowView["CarType"];
                    Int64 brand = (Int64)rowView["Brand"];
                    string colour = rowView["Colour"] as string;
                    Int64 yearOfManufacture = (Int64)rowView["YearOfManufacture"];
                    string number = rowView["Number"] as string;
                    Int64 imageID = (Int64)rowView["ImageID"];

                    // Преобразуем целочисленное значение fuelValue в перечисление Fuel
                    Fuel selectedFuel = (Fuel)fuelValue;

                    // Преобразуем целочисленное значение transmissionValue в перечисление Transmission
                    Transmission selectedTransmission = (Transmission)transmissionValue;

                    CarType selectedCarType = (CarType)carTypeValue;

                    Brand selectedBrand = (Brand)brand;

                    // Создаем объект Car
                    Car selectedCar = new Car(name, selectedFuel, selectedTransmission, selectedCarType, selectedBrand, colour, (int)yearOfManufacture, number, (int)imageID);

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
            if (carsGrid.SelectedItem != null)
            {
                var rowView = carsGrid.SelectedItem as DataRowView;
                if (rowView != null)
                {
                    // Извлекаем значения столбцов из DataRowView
                    string name = rowView["Name"] as string;
                    Int64 fuelValue = (Int64)rowView["FuelType"];
                    Int64 transmissionValue = (Int64)rowView["TransmissionType"];
                    Int64 carTypeValue = (Int64)rowView["CarType"];
                    Int64 brand = (Int64)rowView["Brand"];
                    string colour = rowView["Colour"] as string;
                    Int64 yearOfManufacture = (Int64)rowView["YearOfManufacture"];
                    Int64 amount = (Int64)rowView["Amount"];
                    Int64 imageID = (Int64)rowView["ImageID"];

                    Fuel selectedFuel = (Fuel)fuelValue;

                    Transmission selectedTransmission = (Transmission)transmissionValue;

                    CarType selectedCarType = (CarType)carTypeValue;

                    Brand selectedBrand = (Brand)brand;

                    NameTextBox.Text = name;

                    FuelTypeComboBox.SelectedItem = selectedFuel.ToString();
                    TransmissionTypeComboBox.SelectedItem = selectedTransmission.ToString();

                    CarTypeComboBox.SelectedItem = selectedCarType.ToString();
                    BrandComboBox.SelectedItem = selectedBrand.ToString();

                    FuelTypeComboBox.UpdateLayout();
                    TransmissionTypeComboBox.UpdateLayout();

                    CarTypeComboBox.UpdateLayout();
                    BrandComboBox.UpdateLayout();

                    ColourTextBox.Text = colour;
                    YearOfManufactureTextBox.Text = yearOfManufacture.ToString();
                    NumberTextBox.Text = amount.ToString();
                    ImageIDTextBox.Text = imageID.ToString();
                }
            }
            else
            {
                ClearFields();
            }
        }


        private void CarSaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (carsGrid.SelectedItem == null)
            {
                MessageBox.Show("No item selected in the DataGrid.");
                return;
            }

            if (!ValidateFields())
            {
                return;
            }

            Int64 ID = 0;
            var rowView = carsGrid.SelectedItem as DataRowView;
            if (rowView != null)
            {
                ID = (Int64)rowView["ID"];
            }
            string colour = ColourTextBox.Text;
            string name = NameTextBox.Text;
            Fuel selectedFuel;
            Transmission selectedTransmission;
            CarType selectedCarType;
            Brand brand;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(colour) 
                && Enum.TryParse<Fuel>(FuelTypeComboBox.SelectedValue?.ToString(), out selectedFuel) && Enum.TryParse<Transmission>(TransmissionTypeComboBox.SelectedValue?.ToString(), out selectedTransmission)
                && Enum.TryParse<CarType>(FuelTypeComboBox.SelectedValue?.ToString(), out selectedCarType) && Enum.TryParse<Brand>(TransmissionTypeComboBox.SelectedValue?.ToString(), out brand)
)
            {
                int yearOfManufacture;
                int amount;
                int imageID;

                if (int.TryParse(YearOfManufactureTextBox.Text, out yearOfManufacture) &&
                    int.TryParse(NumberTextBox.Text, out amount) &&
                    int.TryParse(ImageIDTextBox.Text, out imageID))
                {
                    string updateQuery = "UPDATE Cars SET Name = @Name, FuelType = @FuelType, TransmissionType = @TransmissionType, CarType = @CarType, Brand = @Brand, Colour = @Colour, YearOfManufacture = @YearOfManufacture, Amount = @Amount, ImageID = @ImageID WHERE ID = @ID";

                    carParkManager.manager.OpenConnection();

                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, carParkManager.manager.Connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@FuelType", (int)selectedFuel);
                        command.Parameters.AddWithValue("@TransmissionType", (int)selectedTransmission);
                        command.Parameters.AddWithValue("@CarType", (int)selectedCarType);
                        command.Parameters.AddWithValue("@Brand", (int)brand);
                        command.Parameters.AddWithValue("@Colour", colour);
                        command.Parameters.AddWithValue("@YearOfManufacture", yearOfManufacture);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@ImageID", imageID);
                        command.Parameters.AddWithValue("@ID", ID);

                        command.ExecuteNonQuery();
                    }

                    carParkManager.manager.CloseConnection();
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
            ClearFields();
            ShowCarsInDataGrid(carsGrid, carParkManager);
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
        private void CarTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> carTypes = new List<string> { "Economy",
            "Business",
            "SUV",
            "CargoMinibus",
            "Coupe",
            "Limousine",
            "Minibus",
            "Motorcycle"};
            CarTypeComboBox.ItemsSource = carTypes;
        }
        private void BrandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> brandTypes = new List<string> { "Toyota",
            "Honda",
            "Ford",
            "BMW",
            "MercedesBenz",
            "Audi",
            "Volkswagen",
            "Tesla" };
            BrandComboBox.ItemsSource = brandTypes;
        }
        private bool ValidateFields()
        {
            string name = NameTextBox.Text;
            Fuel selectedFuel;
            Transmission selectedTransmission;
            string colour = ColourTextBox.Text;
            int yearOfManufacture;
            int amount;
            int imageID;

            bool isValid = true;

            // Validate name, fuel type, and transmission type
            if (string.IsNullOrWhiteSpace(name) ||
                !Enum.TryParse<Fuel>(FuelTypeComboBox.SelectedValue?.ToString(), out selectedFuel) ||
                !Enum.TryParse<Transmission>(TransmissionTypeComboBox.SelectedValue?.ToString(), out selectedTransmission))
            {
                NameNotify.Foreground = new SolidColorBrush(Colors.Red);
                NameNotify.Content = "Поле должно быть заполнено";
                FuelTypeNotify.Foreground = new SolidColorBrush(Colors.Red);
                FuelTypeNotify.Content = "Поле должно быть заполнено";
                TransmissionTypeNotify.Foreground = new SolidColorBrush(Colors.Red);
                TransmissionTypeNotify.Content = "Поле должно быть заполнено";
                CarTypeNotify.Foreground = new SolidColorBrush(Colors.Red);
                CarTypeNotify.Content = "Поле должно быть заполнено";
                BrandNotify.Foreground = new SolidColorBrush(Colors.Red);
                BrandNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                NameNotify.Content = "";
                FuelTypeNotify.Content = "";
                TransmissionTypeNotify.Content = "";
                CarTypeNotify.Content = "";
                BrandNotify.Content = "";
            }

            // Validate numeric values
            if (!int.TryParse(YearOfManufactureTextBox.Text, out yearOfManufacture) ||
                !int.TryParse(NumberTextBox.Text, out amount) ||
                !int.TryParse(ImageIDTextBox.Text, out imageID) || colour == "")
            {
                ColourNotify.Foreground = new SolidColorBrush(Colors.Red);
                ColourNotify.Content = "Поле должно быть заполнено";
                YearOfManufactureNotify.Foreground = new SolidColorBrush(Colors.Red);
                YearOfManufactureNotify.Content = "Поле должно быть заполнено";
                NumberNotify.Foreground = new SolidColorBrush(Colors.Red);
                NumberNotify.Content = "Поле должно быть заполнено";
                ImageNotify.Foreground = new SolidColorBrush(Colors.Red);
                ImageNotify.Content = "Поле должно быть заполнено";
                isValid = false;
            }
            else
            {
                ColourNotify.Content = "";
                YearOfManufactureNotify.Content = "";
                NumberNotify.Content = "";
                ImageNotify.Content = "";
            }

            return isValid;
        }
        private void ClearFields()
        {
                NameTextBox.Text = "";
                NameNotify.Content = "";
                FuelTypeComboBox.SelectedItem = null;
                FuelTypeNotify.Content = "";
                TransmissionTypeComboBox.SelectedItem = null;
                TransmissionTypeNotify.Content = "";
                CarTypeComboBox.SelectedItem = null;
                CarTypeNotify.Content = "";
                BrandComboBox.SelectedItem = null;
                BrandNotify.Content = "";
                ColourTextBox.Text = "";
                ColourNotify.Content = "";
                YearOfManufactureTextBox.Text = null;
                YearOfManufactureNotify.Content = "";
                NumberTextBox.Text = null;
                NumberNotify.Content = "";
                ImageIDTextBox.Text = null;
                ImageNotify.Content = "";
        }

        private void CarClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void CarClearTableButton_Click(object sender, RoutedEventArgs e)
        {
            carParkManager.ClearCarsTable();
            ShowCarsInDataGrid(carsGrid,carParkManager);
        }
    }
}
