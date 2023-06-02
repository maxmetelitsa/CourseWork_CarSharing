using CourseWork_CarSharing.Enums;
using CourseWork_CarSharing.SQL_Manager;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows.Media;
using CourseWork_CarSharing.CarPark;
using System.Xml.Linq;

namespace CourseWork_CarSharing.CarsInfo
{
    public class Cars
    {
        public List<Car> cars;

        public Cars(SQLiteManager manager)
        {
            cars = new List<Car>(); // Initialize the list of cars
            GetAllCars(manager);
        }

        public bool AddCar(SQLiteManager manager, string name, Fuel fuelType, Transmission transmission, string colour, int yearOfManufacture, int amount)
        {
            manager.OpenConnection();

            // Use parameters in the SQL query to avoid security and character escaping issues

            foreach (Car car in cars)
            {
                if (car.Name == name && car.FuelType == fuelType && car.TransmissionType == transmission && car.Colour == colour && car.YearOfManufacture == yearOfManufacture)
                {
                    return false;
                }
            }

            string insertQuery = "INSERT INTO Cars (Name, FuelType, TransmissionType, Colour, YearOfManufacture, Amount) VALUES (@Name, @FuelType, @TransmissionType, @Colour, @YearOfManufacture, @Amount);";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@FuelType", fuelType);
                command.Parameters.AddWithValue("@TransmissionType", transmission);
                command.Parameters.AddWithValue("@Colour", colour);
                command.Parameters.AddWithValue("@YearOfManufacture", yearOfManufacture);
                command.Parameters.AddWithValue("@Amount", amount);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Data successfully added
                    cars.Add(new Car(name, fuelType, transmission, colour, yearOfManufacture, amount));
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

        public void UpdateAllCars(SQLiteManager manager)
        {
            manager.OpenConnection();

            foreach (Car car in cars)
            {
                string updateQuery = $"UPDATE Cars SET Name = @Name, FuelType = @FuelType, TransmissionType = @TransmissionType, Colour = @Colour, YearOfManufacture = @YearOfManufacture, Amount = @Amount WHERE ID = @{car.ID}";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, manager.Connection))
                {
                    command.Parameters.AddWithValue("@Name", car.Name);
                    command.Parameters.AddWithValue("@FuelType", car.FuelType);
                    command.Parameters.AddWithValue("@TransmissionType", car.TransmissionType);
                    command.Parameters.AddWithValue("@Colour", car.Colour);
                    command.Parameters.AddWithValue("@YearOfManufacture", car.YearOfManufacture);
                    command.Parameters.AddWithValue("@Amount", car.Amount);

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
                        string name = reader["Name"].ToString();
                        Fuel fuelType = (Fuel)reader["FuelType"];
                        Transmission transmission = (Transmission)reader["TransmissionType"];
                        string colour = reader["Colour"].ToString();
                        int yearOfManufacture = (int)reader["YearOfManufacture"];
                        int amount = (int)reader["Amount"];
                        cars.Add(new Car(name, fuelType, transmission, colour, yearOfManufacture, amount));
                    }
                }
            }

            manager.CloseConnection();
        }
        public static void ShowCars(Grid carGrid, List<Car> cars)
        {
            foreach (Car car in cars)
            {
                TextBlock carInfo = new TextBlock();
                carInfo.Text = $"Name: {car.Name}\nFuelType: {car.FuelType}\nTransmissionType: {car.TransmissionType}\nColour: {car.Colour}\nYearOfManufacture: {car.YearOfManufacture}\nAmount: {car.Amount}";
                carInfo.Foreground = Brushes.White;
                carGrid.Children.Add(carInfo);
            }
        }
        private void AddCarToGrid(Grid carGrid, List<Car> cars, string name, Fuel fuelType, Transmission transmission, string colour, int yearOfManufacture, int amount)
        {
            // Создаем новый элемент TextBlock для отображения информации об автомобиле
            TextBlock carInfo = new TextBlock();
            carInfo.Text = $"Name: {name}\nFuelType: {fuelType}\nTransmissionType: {transmission}\nColour: {colour}\nYearOfManufacture: {yearOfManufacture}\nAmount: {amount}";
            cars.Add(new Car(name, fuelType, transmission, colour, yearOfManufacture, amount));
            carInfo.Foreground = Brushes.White;

            // Добавляем элемент в сетку
            carGrid.Children.Add(carInfo);
        }

        public bool ValidateCar(string name, string colour)
        {
            foreach (Car car in cars)
            {
                if (car.Name == name && car.Colour == colour)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
