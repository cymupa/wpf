using AdminPanelBeta.ConnectHttp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdminPanelBeta.Pages
{
    public partial class AutorizationWin : Window
    {
        private readonly HttpClient _client;

        public AutorizationWin()
        {
            InitializeComponent();
            _client = new HttpClient();
            Loaded += AutorizationWin_Loaded;
        }

        private void AutorizationWin_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем значения по умолчанию
            TelTextBox.Text = "000333000";
            PasswordTextBox.Password = "00000000";
        }

        public async void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            string tel = TelTextBox.Text;
            string pass = PasswordTextBox.Password;

            if (string.IsNullOrWhiteSpace(tel) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var credentials = new { tel = tel, password = pass };
            string json = JsonConvert.SerializeObject(credentials);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(APIConfig.BaseUrl + "/authorization",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();

                // Проверяем, успешно ли прошла аутентификация
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Неверный телефон или пароль.");
                    return;
                }

                // Получаем токен пользователя
                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                string token = responseObject.token;

                // Сохраняем токен в настройках приложения
                Properties.Settings.Default.Token = token;
                Properties.Settings.Default.Save();

                // Отправляем запрос к эндпоинту /me для получения информации о пользователе
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage meResponse = await client.GetAsync(APIConfig.BaseUrl + "/me");

                string meResponseBody = await meResponse.Content.ReadAsStringAsync();
                JArray meResponseArray = JArray.Parse(meResponseBody);

                // Предполагаем, что массив содержит только один объект пользователя
                if (meResponseArray.Count > 0)
                {
                    // Получаем данные пользователя из первого объекта массива
                    dynamic meResponseObject = meResponseArray[0];
                    string name = meResponseObject.name;
                    string surname = meResponseObject.surname;
                    int role_id = meResponseObject.role_id;

                    // Сохраняем данные пользователя в настройках приложения
                    Properties.Settings.Default.Name = name;
                    Properties.Settings.Default.Surname = surname;
                    Properties.Settings.Default.Role = role_id.ToString();
                    Properties.Settings.Default.Save();

                    // Проверяем айди ролей
                    if (role_id == 2 || role_id == 3)
                    {
                        // Открываем окно меню
                        var menuwin = new MenuWin();
                        menuwin.Show();
                    }
                    else
                    {
                        // Пользователь не имеет прав доступа
                        MessageBox.Show("У вас нет доступа к этому приложению.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось получить информацию о пользователе.");
                    return;
                }

                // Закрываем текущее окно
                this.Close();
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
