using CourseWork_CarSharing.Enums;
using CourseWork_CarSharing.SQL_Manager;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows.Media;
using CourseWork_CarSharing.CarPark;
using System.Xml.Linq;
using System;

namespace CourseWork_CarSharing.CarsInfo
{
    public class Cars
    {
        public List<Car> cars;

        public Cars()
        {
            cars = new List<Car>(); 
        }
    }
}
