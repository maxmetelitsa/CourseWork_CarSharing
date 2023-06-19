using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CourseWork_CarSharing.Main;
using static CourseWork_CarSharing.Functions.Functions;
using CourseWork_CarSharing.SQL_Manager;
using System.Windows.Documents;
using CourseWork_CarSharing.UsersInfo;
using CourseWork_CarSharing.Admin;
using CourseWork_CarSharing.Profile;

namespace CourseWork_CarSharing.Authorization
{
    /// <summary>
    /// Логика взаимодействия для SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        SQLiteManager manager = new SQLiteManager();
        Users usersList;
        public SignInWindow()
        {
            usersList = new Users(manager); 
            InitializeComponent();
        }
        private bool isMaximize = false;

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
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxSession.IsChecked == true)
            {
                /*
                 * Создание Сессии
                */
                if (isCorrectAuthorization() == true)
                {
                    if (usersList.ValidateUser(manager, TextBoxEmail.Text, TextBoxPassword.Text) == 1)
                    {
                        ProfileWindow window = new ProfileWindow();
                        window.Show();
                        this.Close();

                    }
                    else if (usersList.ValidateUser(manager, TextBoxEmail.Text, TextBoxPassword.Text) == 2)
                    {
                        AdminWindow window = new AdminWindow();
                        window.Show();
                        this.Close();
                    }
                    else
                    {
                        TextBoxPasswordNotify.Foreground = new SolidColorBrush(Colors.Red);
                        TextBoxPasswordNotify.Content = "Неверный email или пароль";
                    }
                }
            }
            else
            {
                if (isCorrectAuthorization() == true)
                {
                    if (usersList.ValidateUser(manager, TextBoxEmail.Text, TextBoxPassword.Text) == 1)
                    {
                        WelcomeWindow window = new WelcomeWindow();
                        window.Show();
                        this.Close();
                    }
                    else if (usersList.ValidateUser(manager, TextBoxEmail.Text, TextBoxPassword.Text) == 2)
                    {
                        AdminWindow window = new AdminWindow();
                        window.Show();
                        this.Close();
                    }
                    else
                    {
                        TextBoxPasswordNotify.Foreground = new SolidColorBrush(Colors.Red);
                        TextBoxPasswordNotify.Content = "Неверный email или пароль";
                    }
                }
            }
        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow window = new SignUpWindow();
            window.Show();
            this.Close();
        }
        bool flag = true;
        string clear = "";
        private bool isCorrectAuthorization()
        {
            string email = TextBoxEmail.Text;
            string password = TextBoxPassword.Text;

            if (string.IsNullOrEmpty(email))
            {
                TextBoxEmailNotify.Foreground = new SolidColorBrush(Colors.Red);
                TextBoxEmailNotify.Content = "Поле должно быть заполнено";
                flag = false;
            }
            else
            {
                if (!IsValidEmail(email))
                {
                    TextBoxEmailNotify.Foreground = new SolidColorBrush(Colors.Red);
                    TextBoxEmailNotify.Content = "Некорректный формат email";
                    flag = false;
                }
                else
                {
                    TextBoxEmailNotify.Content = clear;
                }

            }

            if (string.IsNullOrEmpty(password))
            {
                TextBoxPasswordNotify.Foreground = new SolidColorBrush(Colors.Red);
                TextBoxPasswordNotify.Content = "Поле должно быть заполнено";
                flag = false;
            }
            else
            {
                TextBoxPasswordNotify.Content = clear;
            }
            return flag;
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
    }
}