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
            cars = new List<Car>(); 
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
