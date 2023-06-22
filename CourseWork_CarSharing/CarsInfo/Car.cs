using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CourseWork_CarSharing.Enums;
using CourseWork_CarSharing.UsersInfo;

namespace CourseWork_CarSharing.CarsInfo
{
    public class Car
    {
        private static int lastID = 0;
        public int ID { get; set; }
        string name;
        Fuel fuelType;
        Transmission transmissionType;
        CarType carType;
        Brand brand;
        string colour;
        int yearOFManufacture;
        string number;
        int imageId;
        double hourPrice;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Fuel FuelType
        {
            get { return fuelType; }
            set { fuelType = value; }
        }
        public Transmission TransmissionType
        {
            get { return transmissionType; }
            set { transmissionType = value; }
        }
        public CarType CarType
        {
            get { return carType; }
            set { carType = value; }
        }
        public Brand Brand
        {
            get { return brand; }
            set { brand = value; }
        }
        public string Colour
        {
            get { return colour; }
            set { colour = value; }
        }
        public int YearOfManufacture
        {
            get { return yearOFManufacture; }
            set { yearOFManufacture = value; }
        }
        public string Number
        {
            get { return number; }
            set { number = value; }
        }
        public int ImageID
        {
            get { return imageId; }
            set { imageId = value; }
        }
        public double HourPrice
        {
            get { return hourPrice; }
            set { hourPrice = value; }
        }
        public Car(string name, Fuel fuelType, Transmission transmissionType, CarType carType, Brand brand, string colour, int yearOfManufacture, string number, int imageId, double hourPrice)
        {
            ID = ++lastID;
            Name = name;
            FuelType = fuelType;
            TransmissionType = transmissionType;
            CarType = carType;
            Brand = brand;
            Colour = colour;
            YearOfManufacture = yearOfManufacture;
            Number = number;
            ImageID = imageId;
            HourPrice = hourPrice;
        }
        public static List<Car> CreateListOfUsers()
        {
            List<Car> users = new List<Car>();
            return users;
        }
        public void AddCar(List<Car> cars, Car user)
        {
            if (user != null)
            {
                cars.Add(user);
            }
        }

        public void RemoveCar(List<Car> cars, Car user)
        {
            if (user != null)
            {
                cars.Remove(user);
            }
        }

        public Car GetCarByID(List<Car> cars, int id)
        {
            return cars.Find(u => u.ID == id);
        }

        public List<Car> GetCarsByName(List<Car> cars, string name)
        {
            return cars.FindAll(u => u.Name == name);
        }

        public List<Car> GetCarsByFuelType(List<Car> cars, Fuel fuelType)
        {
            return cars.FindAll(u => u.FuelType == fuelType);
        }

        public List<Car> GetCarsByTransmissionType(List<Car> cars, Transmission transmissionType)
        {
            return cars.FindAll(u => u.TransmissionType == transmissionType);
        }

        public List<Car> GetCarsByColour(List<Car> cars, string colour)
        {
            return cars.FindAll(u => u.Colour == colour);
        }
        public List<Car> GetCarsByYearOfManufacture(List<Car> cars, int yearOfManufacture)
        {
            return cars.FindAll(u => u.YearOfManufacture == yearOfManufacture);
        }
    }
}
