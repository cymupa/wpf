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
        private readonly HttpClient _client;
        public AutorizationWin()
        {
            InitializeComponent();
            _client = new HttpClient();
            Loaded += AutorizationWin_Loaded;
        }

        // Метод, вызываемый при загрузке окна
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
                // Парсим ответ в объект
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                MessageBox.Show(responseBody);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Неверный телефон или пароль.");
                    return;
                }
                    // Сохраняем пользователя в настройках приложения
                    Properties.Settings.Default.Token = responseObject.token;
                    Properties.Settings.Default.Save();
                    // Открываем окно меню
                    var menuwin = new MenuWin();
                    menuwin.Show();
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