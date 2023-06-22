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
using CourseWork_CarSharing.Rent;
using CourseWork_CarSharing.Profile;
using CourseWork_CarSharing.About;
using CourseWork_CarSharing.CarPark;
using CourseWork_CarSharing.Authorization;

namespace CourseWork_CarSharing.Admin
{
    public partial class AdminCarParkWindow : Window
    {
        private CarParkManager carParkManager;
        private bool isMaximize = false;

        public AdminCarParkWindow()
        {
            InitializeComponent();

            carParkManager = new CarParkManager();

            carParkManager.GetAllCars();

            ShowCars(carGrid, carParkManager);
        }


        private void CarButton_Click(object sender, RoutedEventArgs e)
        {
            Button carButton = (Button)sender;
        }
        public void ShowCars(WrapPanel carGrid, CarParkManager carParkManager)
        {
            carGrid = FindName("carGrid") as WrapPanel;
            foreach (Car car in carParkManager.carsList.cars)
            {
                Button carButton = new Button();
                carButton.Click += CarButton_Click;
                carButton.Style = (Style)Application.Current.Resources["NoHoverButtonStyle"];
                carButton.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                carButton.BorderThickness = new Thickness(5);
                carButton.Height = 350;
                carButton.Width = 255;
                carButton.Background = new SolidColorBrush(Color.FromRgb(33, 33, 33));

                StackPanel carPanel = new StackPanel();
                carPanel.Orientation = Orientation.Vertical;
                carPanel.Height = 350;

                string imagePath = @"C:\Лабораторные работы C#\CourseWork_CarSharing\CourseWork_CarSharing\Images\pic" + car.ImageID + ".jpg";
                Image image = new Image();
                image.Height = 180;
                image.Width = 160;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();

                image.Source = bitmap;


                TextBlock carInfo = new TextBlock();
                carInfo.Text = $"Name: {car.Name}\nBrand: {car.Brand}\nCarType: {car.CarType}\nFuelType: {car.FuelType}\nTransmissionType: {car.TransmissionType}\nColour: {car.Colour}\nYearOfManufacture: {car.YearOfManufacture}\nNumber: {car.Number}\nPrice/Day: {car.HourPrice}  $";
                carInfo.Foreground = Brushes.White;
                carInfo.FontSize = 12;
                carInfo.TextAlignment = TextAlignment.Left;
                carInfo.Margin = new Thickness(10, 0, 10, 10);
                //carInfo.Height = 300;

                carPanel.Children.Add(image);
                carPanel.Children.Add(carInfo);

                carButton.Content = carPanel;

                carGrid.Children.Add(carButton);
            }
        }

        public void ShowSearchedCars(WrapPanel carGrid, List<Car> cars)
        {
            carGrid = FindName("carGrid") as WrapPanel;
            foreach (Car car in cars)
            {
                Button carButton = new Button();
                carButton.Click += CarButton_Click;
                carButton.Style = (Style)Application.Current.Resources["NoHoverButtonStyle"];
                carButton.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                carButton.BorderThickness = new Thickness(5);
                carButton.Height = 350;
                carButton.Width = 255;
                carButton.Background = new SolidColorBrush(Color.FromRgb(33, 33, 33));

                StackPanel carPanel = new StackPanel();
                carPanel.Orientation = Orientation.Vertical;
                carPanel.Height = 350;

                string imagePath = @"C:\Лабораторные работы C#\CourseWork_CarSharing\CourseWork_CarSharing\Images\pic" + car.ImageID + ".jpg";
                Image image = new Image();
                image.Height = 180;
                image.Width = 160;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();

                image.Source = bitmap;


                TextBlock carInfo = new TextBlock();
                carInfo.Text = $"Name: {car.Name}\nBrand: {car.Brand}\nCarType: {car.CarType}\nFuelType: {car.FuelType}\nTransmissionType: {car.TransmissionType}\nColour: {car.Colour}\nYearOfManufacture: {car.YearOfManufacture}\nNumber: {car.Number}\nPrice/Day: {car.HourPrice}  $";
                carInfo.Foreground = Brushes.White;
                carInfo.FontSize = 12;
                carInfo.TextAlignment = TextAlignment.Left;
                carInfo.Margin = new Thickness(10, 0, 10, 10);

                carPanel.Children.Add(image);
                carPanel.Children.Add(carInfo);

                carButton.Content = carPanel;
                carButton.Margin = new Thickness(17, 0, 0, 0);

                carGrid.Children.Add(carButton);
            }
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
            Functions.Functions.TerminateProcess("CourseWork_CarSharing");
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;

            // Очистка текущего отображения списка машин
            carGrid.Children.Clear();

            // Получение списка машин, соответствующих поисковому запросу
            List<Car> searchedCars = carParkManager.carsList.cars.Where(car => car.Name.ToLower().Contains(searchText.ToLower())).ToList();

            // Отображение найденных машин
            ShowSearchedCars(carGrid, searchedCars);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text;

            // Очистка текущего отображения списка машин
            carGrid.Children.Clear();

            // Получение списка машин, соответствующих поисковому запросу
            List<Car> searchedCars = carParkManager.carsList.cars.Where(car => car.Name.ToLower().Contains(searchText.ToLower())).ToList();

            // Отображение найденных машин
            ShowSearchedCars(carGrid, searchedCars);
        }
        private void CarParkButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RentButton_Click(object sender, RoutedEventArgs e)
        {
            AdminRentWindow window = new AdminRentWindow();
            window.Show();
            this.Close();
        }

        private void CarParkEditingButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow();
            window.Show();
            this.Hide();
        }

        private void RentalTrackingButton_Click(object sender, RoutedEventArgs e)
        {
            AdminRentalTracking window = new AdminRentalTracking();
            window.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow window = new SignInWindow();
            window.Show();
            this.Hide();
        }
    }
}
