using AdminPanelBeta.ConnectHttp;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System;

namespace AdminPanelBeta.Pages
{
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
                    name = NameTextBox.Text,
                    surname = SurnameTextBox.Text,
                    patronymic = PatronymicTextBox.Text,
                    birth = BirthDatePicker.SelectedDate?.ToString("yyyy-MM-dd"),
                    tel = TelTextBox.Text,
                    address = AddressTextBox.Text,
                    password = PassTextBox.Text
                };

                // Проверяем обязательные поля
                if (string.IsNullOrWhiteSpace(newUser.name) || string.IsNullOrWhiteSpace(newUser.surname) || string.IsNullOrWhiteSpace(newUser.tel))
                {
                    MessageBox.Show("Пожалуйста, заполните обязательные поля: Фамилия, Имя и Телефон.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Пользователь успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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