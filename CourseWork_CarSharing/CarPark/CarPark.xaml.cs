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
using CourseWork_CarSharing.CarsInfo;
using CourseWork_CarSharing.SQL_Manager;
using CourseWork_CarSharing.UsersInfo;

namespace CourseWork_CarSharing.CarPark
{
    /// <summary>
    /// Логика взаимодействия для TestMainWindow.xaml
    /// </summary>
    public partial class CarPark : Window
    {
        SQLiteManager manager = new SQLiteManager();
        Cars carsList;
        private bool isMaximize = false;
        public CarPark()
        {
            carsList = new Cars(manager);
            InitializeComponent();
            carsList.Show
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
        private void ShowCars()
        {
            foreach(Car car in carsList.cars)
            {
                TextBlock carInfo = new TextBlock();
                carInfo.Text = $"Марка: {car.Name}, Модель: {car.Colour}";
                carInfo.Foreground = Brushes.White;
                carGrid.Children.Add(carInfo);
            }
        }
        private void AddCarToGrid(Car car)
        {
            // Создаем новый элемент TextBlock для отображения информации об автомобиле
            TextBlock carInfo = new TextBlock();
            carInfo.Text = $"Марка: {car.Name}, Модель: {car.Colour}";
            carInfo.Foreground = Brushes.White;

            // Добавляем элемент в сетку
            carGrid.Children.Add(carInfo);
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