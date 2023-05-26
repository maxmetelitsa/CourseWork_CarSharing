using CourseWork_CarSharing.SQL_Manager;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static CourseWork_CarSharing.Functions.Functions;

namespace CourseWork_CarSharing.Authorization
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindowNew.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        SQLiteManager manager = new SQLiteManager("C:\\DataBases\\Stellar");
        public SignUpWindow()
        {
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

        private void txtBlockPasswordRepeat_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow window = new SignInWindow();
            window.Show();
            this.Close();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxSession.IsChecked == true)
            {
                /*
                 * Создание Сессии
                */
                if (isCorrectRegistratition() == true)
                {
                    if (addUser(manager, TextBoxName.Text, TextBoxSurname.Text, TextBoxEmail.Text, TextBoxPassword.Text) == true)
                    {
                        TextBoxNameNotify.Content = "Done";
                        ReadUsers(manager, TextBoxSurnameNotify);
                    }
                    else
                    {
                        TextBoxNameNotify.Content = "Daun";
                        ReadUsers(manager, TextBoxSurnameNotify);
                    }
                }
            }
            else
            {
                if (addUser(manager, TextBoxName.Text, TextBoxSurname.Text, TextBoxEmail.Text, TextBoxPassword.Text) == true)
                {
                    TextBoxNameNotify.Content = "Done";
                }
                else
                {
                    TextBoxNameNotify.Content = "Daun";
                }
            }

        }

        private bool isCorrectRegistratition()
        {
            string firstName = TextBoxName.Text;
            string lastName = TextBoxSurname.Text;
            string email = TextBoxEmail.Text;
            string password = TextBoxPassword.Text;
            string confirmPassword = TextBoxPasswordRepeat.Text;
            bool flag = true;
            string clear = "";

            if (string.IsNullOrEmpty(firstName))
            {
                TextBoxNameNotify.Foreground = new SolidColorBrush(Colors.Red);
                TextBoxNameNotify.Content = "Поле должно быть заполнено";
                flag = false;
            }
            else
            {
                TextBoxNameNotify.Content = clear;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                TextBoxSurnameNotify.Foreground = new SolidColorBrush(Colors.Red);
                TextBoxSurnameNotify.Content = "Поле должно быть заполнено";
                flag = false;
            }
            else
            {
                TextBoxSurnameNotify.Content = clear;
            }

            if (string.IsNullOrEmpty(email))
            {
                TextBoxEmailNotify.Foreground = new SolidColorBrush(Colors.Red);
                TextBoxEmailNotify.Content = "Поле должно быть заполнено";
                flag = false;
            }
            else
            {
                TextBoxEmailNotify.Content = clear;
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

            if (string.IsNullOrEmpty(confirmPassword))
            {
                TextBoxPasswordRepeatNotify.Foreground = new SolidColorBrush(Colors.Red);
                TextBoxPasswordRepeatNotify.Content = "Поле должно быть заполнено";
                flag = false;
            }
            else
            {
                TextBoxPasswordRepeatNotify.Content = clear;
            }
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

            if (password != confirmPassword)
            {
                TextBoxPasswordRepeatNotify.Foreground = new SolidColorBrush(Colors.Red);
                TextBoxPasswordRepeatNotify.Content = "Пароли не свопадают";
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