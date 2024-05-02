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
using System.Net.Http;
using System.Net;

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
        }
        private async Task<bool> CheckApiConnectionAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync("http://evseev-dv.tepk-it.ru/api");
                response.EnsureSuccessStatusCode();
                Console.WriteLine("Подключение к API успешно!");
                return true;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка подключения к API: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем подключение к API перед попыткой входа
            bool isConnected = await CheckApiConnectionAsync();
            if (!isConnected)
            {
                // Если подключение не удалось, прерываем процесс входа
                return;
            }

            string tel = TelTextBox.Text;
            string pass = PasswordTextBox.Password;
            if (string.IsNullOrWhiteSpace(tel) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }
            var credentials = new { phone = tel, password = pass };

            // Сериализуем объект в JSON
            string json = JsonConvert.SerializeObject(credentials);

            // Используем HttpClient для выполнения POST запроса к API
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(APIConfig.APIurl + "/auth/login",
                     new StringContent(json, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                // Парсим ответ в объект
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Неверный телефон или пароль."); // Выводим сообщение об ошибке, если аутентификация не удалась
                    return;
                }

                string userRole = Properties.Settings.Default.Role;
                if (userRole != "admin" && userRole != "manager")
                {
                    MessageBox.Show("Доступ запрещен. У вас нет прав доступа для этого приложения."); // Выводим сообщение об ошибке, если у пользователя нет прав администратора или менеджера
                    return;
                }
            }
        }

        private void ExitPage(object sender, MouseButtonEventArgs e)
        {
            var welcomewin = new WelcomeWin();
            welcomewin.Show();
            this.Close();
        }
    }
}