using AdminPanelBeta.ConnectHttp;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminPanelBeta.Pages
{
    public partial class AddTournamentsData : Window
    {
        private readonly HttpClient _httpClient = new HttpClient(); // Добавляем поле _httpClient

        public AddTournamentsData()
        {
            InitializeComponent();
            StartDateTextBox.LostFocus += StartDateTextBox_LostFocus;
            EndDateTextBox.LostFocus += EndDateTextBox_LostFocus;
        }

        private async void AddTournament_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем токен из настроек
                string token = Properties.Settings.Default.Token;

                // Создаем объект с данными нового турнира
                var newTournament = new
                {
                    name = NameTextBox.Text,
                    description = DescriptionTextBox.Text,
                    start = StartDateTextBox.SelectedDate?.ToString("yyyy-MM-dd"),
                    end = EndDateTextBox.SelectedDate?.ToString("yyyy-MM-dd"),
                    payment = PaymentTextBox.Text
                };

                // Проверяем обязательные поля
                if (string.IsNullOrWhiteSpace(newTournament.name) || string.IsNullOrWhiteSpace(newTournament.description))
                {
                    MessageBox.Show("Пожалуйста, заполните обязательные поля: Название и Описание.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем контент для POST-запроса
                var content = new StringContent(JsonConvert.SerializeObject(newTournament), Encoding.UTF8, "application/json");

                // Формируем URL для отправки запроса
                string url = $"{APIConfig.BaseUrl}/tournaments";

                HttpResponseMessage response;

                // Добавляем токен в заголовок запроса
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Отправляем POST-запрос на сервер для добавления нового турнира
                response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Проверяем ответ сервера
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody != null)
                {
                    MessageBox.Show("Турнир успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении турнира: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartDateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (StartDateTextBox.SelectedDate != null)
            {
                StartDateTextBox.Text = StartDateTextBox.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
        }

        private void EndDateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EndDateTextBox.SelectedDate != null)
            {
                EndDateTextBox.Text = EndDateTextBox.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
        }

        private void ExitPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
