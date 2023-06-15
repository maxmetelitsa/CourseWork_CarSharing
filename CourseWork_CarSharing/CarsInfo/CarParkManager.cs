using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    public class CarParkManager
    {
        public SQLiteManager manager;
        public Cars carsList;

        public CarParkManager()
        {
            manager = new SQLiteManager();
            carsList = new Cars();
        }

        public void UpdateAllCars()
        {
            manager.OpenConnection();

            foreach (Car car in carsList.cars)
            {
                string updateQuery = $"UPDATE Cars SET Name = @Name, FuelType = @FuelType, TransmissionType = @TransmissionType, CarType = @CarType, Brand = @Brand, Colour = @Colour, YearOfManufacture = @YearOfManufacture, Number = @Number, ImageID = @ImageID WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, manager.Connection))
                {
                    command.Parameters.AddWithValue("@Name", car.Name);
                    command.Parameters.AddWithValue("@FuelType", (int)car.FuelType);
                    command.Parameters.AddWithValue("@TransmissionType", (int)car.TransmissionType);
                    command.Parameters.AddWithValue("@CarType", (int)car.CarType);
                    command.Parameters.AddWithValue("@Brand", (int)car.Brand);
                    command.Parameters.AddWithValue("@Colour", car.Colour);
                    command.Parameters.AddWithValue("@YearOfManufacture", car.YearOfManufacture);
                    command.Parameters.AddWithValue("@Number", car.Number);
                    command.Parameters.AddWithValue("@ImageID", car.ImageID);
                    command.Parameters.AddWithValue("@ID", car.ID);

                    command.ExecuteNonQuery();
                }
            }

            manager.CloseConnection();
        }

        public void GetAllCars()
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
                        CarType carType = (CarType)reader.GetInt32(reader.GetOrdinal("CarType"));
                        Brand brand = (Brand)reader.GetInt32(reader.GetOrdinal("Brand"));
                        string colour = reader.GetString(reader.GetOrdinal("Colour"));
                        int yearOfManufacture = reader.GetInt32(reader.GetOrdinal("YearOfManufacture"));
                        string number = reader.GetString(reader.GetOrdinal("Number"));
                        int imageID = reader.GetInt32(reader.GetOrdinal("ImageID"));

                        carsList.cars.Add(new Car(name, fuelType, transmission, carType, brand, colour, yearOfManufacture, number, imageID));
                    }
                }
            }

            manager.CloseConnection();
        }

        public bool AddCar(string name, Fuel fuelType, Transmission transmission, CarType carType, Brand brand, string colour, int yearOfManufacture, string number, int imageID)
        {
            manager.OpenConnection();

            foreach (Car car in carsList.cars)
            {
                if (car.Name == name && car.FuelType == fuelType && car.TransmissionType == transmission && car.CarType == carType  && car.Brand == brand && car.Colour == colour && car.YearOfManufacture == yearOfManufacture)
                {
                    manager.CloseConnection();
                    return false;
                }
            }

            string insertQuery = "INSERT INTO Cars (Name, FuelType, TransmissionType, CarType, Brand, Colour, YearOfManufacture, Number, ImageID) VALUES (@Name, @FuelType, @TransmissionType, @CarType, @Brand, @Colour, @YearOfManufacture, @Number, @ImageID)";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@FuelType", (int)fuelType);
                command.Parameters.AddWithValue("@TransmissionType", (int)transmission);
                command.Parameters.AddWithValue("@CarType", (int)carType);
                command.Parameters.AddWithValue("@Brand", (int)brand);
                command.Parameters.AddWithValue("@Colour", colour);
                command.Parameters.AddWithValue("@YearOfManufacture", yearOfManufacture);
                command.Parameters.AddWithValue("@Amount", number);
                command.Parameters.AddWithValue("@ImageID", imageID);

                command.ExecuteNonQuery();
            }

            manager.CloseConnection();

            carsList.cars.Add(new Car(name, fuelType, transmission, carType, brand, colour, yearOfManufacture, number, imageID));

            return true;
        }
        public void RemoveCar(Car car)
        {
            manager.OpenConnection();

            string deleteQuery = "DELETE FROM Cars WHERE Name = @Name AND FuelType = @FuelType AND TransmissionType = @TransmissionType AND CarType = @CarType AND Brand = @Brand AND Colour = @Colour AND YearOfManufacture = @YearOfManufacture AND Number = @Number AND ImageID = @ImageID";

            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Name", car.Name);
                command.Parameters.AddWithValue("@FuelType", (int)car.FuelType);
                command.Parameters.AddWithValue("@TransmissionType", (int)car.TransmissionType);
                command.Parameters.AddWithValue("@CarType", (int)car.CarType);
                command.Parameters.AddWithValue("@Brand", (int)car.Brand);
                command.Parameters.AddWithValue("@Colour", car.Colour);
                command.Parameters.AddWithValue("@YearOfManufacture", car.YearOfManufacture);
                command.Parameters.AddWithValue("@Number", car.Number);
                command.Parameters.AddWithValue("@ImageID", car.ImageID);

                command.ExecuteNonQuery();
            }

            manager.CloseConnection();
            //carsList.cars.Remove(car);
        }

        public bool ValidateDataCar(string name, Fuel fuelType, Transmission transmission, CarType carType, Brand brand, string colour, int yearOfManufacture, string number, int imageID)
        {
            if (string.IsNullOrEmpty(name))
            {
                // Поле name пустое или содержит только пробельные символы
                return false;
            }

            // Проверка на корректность поля fuelType
            if (!Enum.IsDefined(typeof(Fuel), fuelType))
            {
                // Значение поля fuelType не является допустимым значением перечисления Fuel
                return false;
            }

            // Проверка на корректность поля transmission
            if (!Enum.IsDefined(typeof(Transmission), transmission))
            {
                // Значение поля transmission не является допустимым значением перечисления Transmission
                return false;
            }

            if (!Enum.IsDefined(typeof(CarType), carType))
            {
                // Значение поля transmission не является допустимым значением перечисления Transmission
                return false;
            }

            if (!Enum.IsDefined(typeof(Brand), brand))
            {
                // Значение поля transmission не является допустимым значением перечисления Transmission
                return false;
            }

            // Проверка на корректность поля colour
            if (string.IsNullOrEmpty(colour))
            {
                // Поле colour пустое или содержит только пробельные символы
                return false;
            }

            // Проверка на корректность поля yearOfManufacture
            if (yearOfManufacture < 1900 || yearOfManufacture > DateTime.Now.Year)
            {
                // Значение поля yearOfManufacture находится в недопустимом диапазоне
                return false;
            }

            // Проверка на корректность поля amount
            if (string.IsNullOrEmpty(number))
            {
                // Поле name пустое или содержит только пробельные символы
                return false;
            }

            if (imageID <= 0)
            {
                // Значение поля amount не положительное число
                return false;
            }

            return true;
        }

        public void ClearCarsTable()
        {
            manager.OpenConnection();

            string deleteQuery = "DELETE FROM Cars";

            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, manager.Connection))
            {
                command.ExecuteNonQuery();
            }

            manager.CloseConnection();

            carsList.cars.Clear();
        }
    }
}
