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
using System.Xml.Linq;
using CourseWork_CarSharing.About;
using CourseWork_CarSharing.Authorization;
using CourseWork_CarSharing.CarPark;
using CourseWork_CarSharing.CarsInfo;
using CourseWork_CarSharing.Enums;
using CourseWork_CarSharing.Profile;
using CourseWork_CarSharing.SQL_Manager;
using CourseWork_CarSharing.UsersInfo;

namespace CourseWork_CarSharing.Rent
{
    public partial class RentWindow : Window
    {
        private CarParkManager carParkManager;
        private bool isMaximize = false;
        public RentWindow()
        {
            InitializeComponent();

            carParkManager = new CarParkManager();

            carParkManager.GetAllCars();


            ShowCars(carGrid, carParkManager);

            List<CarType> carTypes = new List<CarType> { CarType.Economy, CarType.Business, CarType.Coupe, CarType.Limousine };
            ClassComboBox.ItemsSource = carTypes;
            List<Brand> brands = new List<Brand> { Brand.Audi, Brand.Bentley, Brand.BMW, Brand.Chevrolet, Brand.Ford, Brand.Honda, Brand.Hyundai, Brand.Jaguar, Brand.Kia, Brand.Lamborghini, Brand.Lexus, Brand.Mazda, Brand.MercedesBenz, Brand.Nissan, Brand.Porsche, Brand.Subaru, Brand.Tesla, Brand.Toyota, Brand.Volkswagen };
            BrandComboBox.ItemsSource = brands;
            List<string> sortBy = new List<string> { "Дешевые сначала", "Дорогие сначала" };
            PriceComboBox.ItemsSource = sortBy;
            ClassComboBox.SelectedIndex = 0;
            BrandComboBox.SelectedIndex = 0;
            PriceComboBox.SelectedIndex = 0;

            BrandComboBox.SelectedValue = null;
            ClassComboBox.SelectedValue = null;
            PriceComboBox.SelectedValue = null;
        }


        private void CarButton_Click(object sender, RoutedEventArgs e)
        {
            Button carButton = (Button)sender;
            Car car = (Car)carButton.Tag;
            RentCarWindow window = new RentCarWindow(car);
            window.Show();
            this.Hide();
        }
        public void ShowCars(WrapPanel carGrid, CarParkManager carParkManager)
        {
            carGrid = FindName("carGrid") as WrapPanel;

            foreach (Car car in carParkManager.availableCarsList.cars)
            {
                Button carButton = new Button();
                carButton.Tag = car;
                carButton.Click += CarButton_Click;
                carButton.Style = (Style)Application.Current.Resources["NoHoverButtonStyle"];
                carButton.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                carButton.BorderThickness = new Thickness(5);
                carButton.Height = 350;
                carButton.Width = 240;
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
                carInfo.Text = $"Название: {car.Name}\nМарка: {car.Brand}\nКласс: {car.CarType}\nТопливо: {car.FuelType}\nКоробка: {car.TransmissionType}\nЦвет: {car.Colour}\nГод выпуска: {car.YearOfManufacture}\nНомер: {car.Number}\nСтоимость/День: {car.HourPrice} р";
                carInfo.Foreground = Brushes.White;
                carInfo.FontSize = 12;
                carInfo.TextAlignment = TextAlignment.Left;
                carInfo.Margin = new Thickness(10, 0, 10, 10);

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
                carButton.Tag = car;
                carButton.Click += CarButton_Click;
                carButton.Style = (Style)Application.Current.Resources["NoHoverButtonStyle"];
                carButton.BorderBrush = new SolidColorBrush(Color.FromRgb(51, 51, 51));
                carButton.BorderThickness = new Thickness(5);
                carButton.Height = 350;
                carButton.Width = 240;
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
                carInfo.Text = $"Название: {car.Name}\nМарка: {car.Brand}\nКласс: {car.CarType}\nТопливо: {car.FuelType}\nКоробка: {car.TransmissionType}\nЦвет: {car.Colour}\nГод выпуска: {car.YearOfManufacture}\nНомер: {car.Number}\nСтоимость/День: {car.HourPrice} р";
                carInfo.Foreground = Brushes.White;
                carInfo.FontSize = 12;
                carInfo.TextAlignment = TextAlignment.Left;
                carInfo.Margin = new Thickness(10, 0, 10, 10);

                carPanel.Children.Add(image);
                carPanel.Children.Add(carInfo);

                carButton.Content = carPanel;

                carGrid.Children.Add(carButton);
            }
        }
        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ClearChanges();
        }
        private void ApplyChanges()
        {
            Brand selectedBrand;
            CarType selectedClass;
            string selectedPrice;

            bool isBrandSelected = Enum.TryParse<Brand>(BrandComboBox.SelectedValue?.ToString(), out selectedBrand);
            bool isCarTypeSelected = Enum.TryParse<CarType>(ClassComboBox.SelectedValue?.ToString(), out selectedClass);
            bool isPriceSelected = PriceComboBox.SelectedValue != null && !string.IsNullOrEmpty(PriceComboBox.SelectedValue.ToString());

            selectedBrand = isBrandSelected ? selectedBrand : Brand.None;
            selectedClass = isCarTypeSelected ? selectedClass : CarType.None;
            selectedPrice = PriceComboBox.SelectedValue?.ToString();

            carGrid.Children.Clear();

            List<Car> searchedCars = carParkManager.availableCarsList.cars;

            if (isBrandSelected)
            {
                searchedCars = searchedCars.Where(car => car.Brand == selectedBrand).ToList();
            }

            if (isCarTypeSelected)
            {
                searchedCars = searchedCars.Where(car => car.CarType == selectedClass).ToList();
            }

            if (isPriceSelected)
            {
                if (selectedPrice == "Дешевые сначала")
                {
                    searchedCars = searchedCars.OrderBy(car => car.HourPrice).ToList();
                }
                else
                {
                    searchedCars = searchedCars.OrderByDescending(car => car.HourPrice).ToList();
                }
            }

            ShowSearchedCars(carGrid, searchedCars);
        }



        private void ClearChanges()
        {
            BrandComboBox.SelectedValue = null;
            ClassComboBox.SelectedValue = null;
            PriceComboBox.SelectedValue = null;
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
            ProfileWindow window = new ProfileWindow();
            window.Show();
            this.Close();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            //AboutWindow window = new AboutWindow();
            //window.Show();
            //this.Close();
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
            "Tesla",
            "Porsche",
            "Hyundai",
            "Chevrolet",
            "Nissan",
            "Jaguar",
            "Lexus",
            "Subaru",
            "Mazda",
            "Kia",
            "Bentley",
            "Lamborghini"
            };
            BrandComboBox.ItemsSource = brandTypes;
            ApplyChanges();
        }

        private void ClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> carTypes = new List<string> { "Economy",
            "Business",
            "Coupe",
            "Limousine"};
            ClassComboBox.ItemsSource = carTypes;
            ApplyChanges();
        }

        private void PriceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> sortBy = new List<string> { "Дешевые сначала", "Дорогие сначала" };
            PriceComboBox.ItemsSource = sortBy;
            ApplyChanges();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow window = new SignInWindow();
            window.Show();
            this.Hide();
        }
    }
}
