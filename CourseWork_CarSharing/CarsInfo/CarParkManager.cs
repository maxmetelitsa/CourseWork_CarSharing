using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
                string updateQuery = $"UPDATE Cars SET Name = @Name, FuelType = @FuelType, TransmissionType = @TransmissionType, Colour = @Colour, YearOfManufacture = @YearOfManufacture, Amount = @Amount, ImageID = @ImageID WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, manager.Connection))
                {
                    command.Parameters.AddWithValue("@Name", car.Name);
                    command.Parameters.AddWithValue("@FuelType", (int)car.FuelType);
                    command.Parameters.AddWithValue("@TransmissionType", (int)car.TransmissionType);
                    command.Parameters.AddWithValue("@Colour", car.Colour);
                    command.Parameters.AddWithValue("@YearOfManufacture", car.YearOfManufacture);
                    command.Parameters.AddWithValue("@Amount", car.Amount);
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
                        string colour = reader.GetString(reader.GetOrdinal("Colour"));
                        int yearOfManufacture = reader.GetInt32(reader.GetOrdinal("YearOfManufacture"));
                        int amount = reader.GetInt32(reader.GetOrdinal("Amount"));
                        int imageID = reader.GetInt32(reader.GetOrdinal("ImageID"));

                        carsList.cars.Add(new Car(name, fuelType, transmission, colour, yearOfManufacture, amount, imageID));
                    }
                }
            }

            manager.CloseConnection();
        }

        public bool AddCar(string name, Fuel fuelType, Transmission transmission, string colour, int yearOfManufacture, int amount, int imageID)
        {
            manager.OpenConnection();

            foreach (Car car in carsList.cars)
            {
                if (car.Name == name && car.FuelType == fuelType && car.TransmissionType == transmission && car.Colour == colour && car.YearOfManufacture == yearOfManufacture)
                {
                    manager.CloseConnection();
                    return false;
                }
            }

            string insertQuery = "INSERT INTO Cars (Name, FuelType, TransmissionType, Colour, YearOfManufacture, Amount, ImageID) VALUES (@Name, @FuelType, @TransmissionType, @Colour, @YearOfManufacture, @Amount, @ImageID)";

            using (SQLiteCommand command = new SQLiteCommand(insertQuery, manager.Connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@FuelType", (int)fuelType);
                command.Parameters.AddWithValue("@TransmissionType", (int)transmission);
                command.Parameters.AddWithValue("@Colour", colour);
                command.Parameters.AddWithValue("@YearOfManufacture", yearOfManufacture);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@ImageID", imageID);

                command.ExecuteNonQuery();
            }

            manager.CloseConnection();

            carsList.cars.Add(new Car(name, fuelType, transmission, colour, yearOfManufacture, amount, imageID));

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
