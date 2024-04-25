using AdminPanelBeta.ConnectHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static AdminPanelBeta.ConnectHttp.APIConfig;

namespace AdminPanelBeta.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWin.xaml
    /// </summary>
    public partial class AutorizationWin : Window
    {
        public AutorizationWin()
        {
            InitializeComponent();
            LoginTextBox.Text = "123456789"; // Номер телефона администратора
            PasswordTextBox.Password = "Москва"; // Город администратора
        }
        private void ExitPage(object sender, MouseButtonEventArgs e)
        {
            var welcomewin = new WelcomeWin();
            welcomewin.Show();
            this.Close();
        }
        private async void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = LoginTextBox.Text;
            string city = PasswordTextBox.Password;

            User user = FindUser(phoneNumber, city);
            if (user != null)
            {
                // Создаем новое окно MenuWin
                MenuWin menuWindow = new MenuWin();

                // Устанавливаем видимость кнопок навигации в зависимости от роли пользователя
                if (user.Role == "Администратор")
                {
                    menuWindow.NewsButton.Visibility = Visibility.Visible;
                    menuWindow.BroadcastsButton.Visibility = Visibility.Collapsed;
                    menuWindow.TournamentsButton.Visibility = Visibility.Visible;
                    menuWindow.GamesButton.Visibility = Visibility.Visible;
                    menuWindow.EmployeesButton.Visibility = Visibility.Visible;
                    menuWindow.StatisticsButton.Visibility = Visibility.Visible;
                }
                else if (user.Role == "Менеджер")
                {
                    menuWindow.NewsButton.Visibility = Visibility.Visible;
                    menuWindow.BroadcastsButton.Visibility = Visibility.Visible;
                    menuWindow.TournamentsButton.Visibility = Visibility.Collapsed;
                    menuWindow.GamesButton.Visibility = Visibility.Collapsed;
                    menuWindow.EmployeesButton.Visibility = Visibility.Collapsed;
                    menuWindow.StatisticsButton.Visibility = Visibility.Collapsed;
                }

                // Передаем имя пользователя и его роль в новое окно
                menuWindow.TextBlockFIOUser.Text = user.name; // Имя пользователя (в данном случае номер телефона)
                menuWindow.TextBlockPositionUser.Text = user.Role; // Роль пользователя

                // Показываем новое окно
                menuWindow.Show();

                // Закрываем текущее окно авторизации
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный номер телефона или город!", "Ошибка авторизации");
            }
        }
        private User FindUser(string phoneNumber, string city)
        {
            List<User> users = new List<User>
    {
                new User { phone = "123456789", City = "Москва", name = "Иванов Иван", Role = "Администратор" },
                new User { phone = "987654321", City = "Санкт-Петербург", name = "Петров Петр", Role = "Менеджер" }
    };

            // Ищем пользователя в списке
            return users.FirstOrDefault(u => u.phone == phoneNumber && u.City == city);
        }
    }
}