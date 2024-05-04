using AdminPanelBeta.ConnectHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace AdminPanelBeta.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddUserDataPage.xaml
    /// </summary>

        public partial class AddUserDataPage : Window
        {
            private readonly HttpClient _httpClient = new HttpClient();
            public AddUserDataPage()
            {
                InitializeComponent();
            }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект с данными нового пользователя
                var newUser = new
                {
                    surname = SurnameTextBox.Text,
                    name = NameTextBox.Text,
                    birth = BirthDatePicker.SelectedDate?.ToString("yyyy-MM-dd"),
                    tel = TelTextBox.Text,
                    password = PassTextBox.Text
                };

                // Проверяем обязательные поля
                if (string.IsNullOrWhiteSpace(newUser.surname) || string.IsNullOrWhiteSpace(newUser.name) || string.IsNullOrWhiteSpace(newUser.tel))
                {
                    MessageBox.Show("Пожалуйста, заполните обязательные поля: Фамилия, Имя и Телефон.");
                    return;
                }

                // Создаем контент для POST-запроса
                var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

                // Формируем URL для отправки запроса
                string url = $"{APIConfig.BaseUrl}/registration";

                // Отправляем POST-запрос на сервер для регистрации нового пользователя
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Проверяем ответ сервера
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody != null)
                {
                    MessageBox.Show("Пользователь успешно добавлен.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitPage(object sender, RoutedEventArgs e)
            {
                this.Close();
            }
        }
    }