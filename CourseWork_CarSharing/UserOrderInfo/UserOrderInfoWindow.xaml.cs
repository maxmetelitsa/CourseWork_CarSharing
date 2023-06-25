using CourseWork_CarSharing.About;
using CourseWork_CarSharing.Authorization;
using CourseWork_CarSharing.CarPark;
using CourseWork_CarSharing.Profile;
using CourseWork_CarSharing.Rent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Word = Microsoft.Office.Interop.Word;

namespace CourseWork_CarSharing.UserOrderInfo
{
    public partial class UserOrderInfoWindow : Window
    {
        string documentPath;
        bool isMaximize = false;
        public UserOrderInfoWindow(int id)
        {
            InitializeComponent();
            documentPath = $"C:\\Rentals\\rental_order4.docx";
            LoadWordPadDocument();
        }
        private void LoadWordPadDocument()
        {
            if (File.Exists(documentPath))
            {
                try
                {
                    string rtfContent = File.ReadAllText(documentPath);
                    rtfContent = RemoveInvalidRtfCharacters(rtfContent);

                    if (IsValidRtfContent(rtfContent))
                    {
                        FlowDocument flowDocument = new FlowDocument();
                        using (MemoryStream rtfMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(rtfContent)))
                        {
                            TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                            textRange.Load(rtfMemoryStream, DataFormats.Rtf);
                        }

                        FlowDocumentReader.Document = flowDocument;
                    }
                    else
                    {
                        MessageBox.Show("Документ содержит некорректное содержимое RTF.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Ошибка при загрузке документа WordPad: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string RemoveInvalidRtfCharacters(string rtfContent)
        {
            // Удаление недопустимых символов RTF
            StringBuilder cleanedContent = new StringBuilder();
            foreach (char c in rtfContent)
            {
                if (IsValidRtfCharacter(c))
                {
                    cleanedContent.Append(c);
                }
            }

            return cleanedContent.ToString();
        }

        private bool IsValidRtfContent(string rtfContent)
        {
            // Проверка на корректность содержимого RTF
            try
            {
                using (MemoryStream rtfMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(rtfContent)))
                {
                    TextRange textRange = new TextRange(new FlowDocument().ContentStart, new FlowDocument().ContentEnd);
                    textRange.Load(rtfMemoryStream, DataFormats.Rtf);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidRtfCharacter(char c)
        {
            // Проверка, является ли символ допустимым для RTF
            return c == '\t' || c == '\n' || c == '\r' || (c >= ' ' && c <= 127);
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
            ProfileWindow window = new ProfileWindow();
            window.Show();
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
            RentWindow window = new RentWindow();
            window.Show();
            this.Close();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow window = new ProfileWindow();
            window.Show();
;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
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
