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
using System.Xml.Linq;
using CourseWork_CarSharing.CarPark;
using CourseWork_CarSharing.Rent;
using CourseWork_CarSharing.Profile;
using CourseWork_CarSharing.About;
using CourseWork_CarSharing.CarsInfo;

namespace CourseWork_CarSharing.Rent
{
    /// <summary>
    /// Логика взаимодействия для TestMainWindow.xaml
    /// </summary>
    public partial class RentCarWindow : Window
    {
        private CarParkManager carParkManager;
        private bool isMaximize = false;
        Car car;

        public Car Car
        {
            get { return car; }
            set { car = value; }
        }
        public RentCarWindow(Car car)
        {
            InitializeComponent();

            Car = car;

            carParkManager = new CarParkManager();
            ShowCar(rentalCar);

        }
        public void ShowCar(WrapPanel carObject)
        {
            carObject = FindName("rentalCar") as WrapPanel;

            Button carButton = new Button();
            carButton.Tag = Car;
            carButton.Style = (Style)Application.Current.Resources["NoHoverButtonStyle"];
            carButton.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            carButton.BorderThickness = new Thickness(5);
            carButton.Height = 590;
            carButton.Width = 600;
            carButton.Background = new SolidColorBrush(Color.FromRgb(33, 33, 33));

            StackPanel carPanel = new StackPanel();
            carPanel.Orientation = Orientation.Vertical;
            carPanel.Height = 590;
            carPanel.Width = 600;

            string imagePath = @"C:\Лабораторные работы C#\CourseWork_CarSharing\CourseWork_CarSharing\Images\pic" + Car.ImageID + ".jpg";
            Image image = new Image();
            image.Height = 320;
            image.Width = 300;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;

            TextBlock carInfo = new TextBlock();
            carInfo.Text = $"Name: {Car.Name}\nBrand: {Car.Brand}\nCarType: {Car.CarType}\nFuelType: {Car.FuelType}\nTransmissionType: {Car.TransmissionType}\nColour: {Car.Colour}\nYearOfManufacture: {Car.YearOfManufacture}\nNumber: {Car.Number}\nPrice/Hour: {Car.HourPrice} $";
            carInfo.Foreground = Brushes.White;
            carInfo.FontSize = 16;
            carInfo.TextAlignment = TextAlignment.Left;
            carInfo.Margin = new Thickness(10, 0, 10, 10);

            carPanel.Children.Add(image);
            carPanel.Children.Add(carInfo);

            carButton.Content = carPanel;

            carObject.Children.Add(carButton);
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

        private void CarParkButton_Click(object sender, RoutedEventArgs e)
        {
            CarParkWindow window = new CarParkWindow();
            window.Show();
            this.Close();
        }

        private void RentButton_Click(object sender, RoutedEventArgs e)
        {
            RentWindow window = new RentWindow();
            window.Show();
            this.Close();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow window = new ProfileWindow();
            window.Show();
            this.Close();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
            this.Close();
        }
    }
}