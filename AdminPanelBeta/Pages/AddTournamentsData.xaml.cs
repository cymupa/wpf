using System;
using AdminPanelBeta.ConnectHttp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
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
    public partial class AddTournamentsData : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public AddTournamentsData()
        {
            InitializeComponent();
        }

        private async void AddTournament_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string token = Properties.Settings.Default.Token;

                // Создаем объект с данными нового турнира
                var newTournament = new
                {
                    Name = NameTextBox.Text,
                    StartDate = StartDatePicker.SelectedDate?.ToString("yyyy-MM-dd"),
                    EndDate = EndDatePicker.SelectedDate?.ToString("yyyy-MM-dd"),
                    Contribution = ContributionTextBox.Text,
                    Description = DescriptionTextBox.Text
                };

                // Проверяем обязательные поля
                if (string.IsNullOrWhiteSpace(newTournament.Name) ||
                    newTournament.StartDate == null ||
                    newTournament.EndDate == null ||
                    string.IsNullOrWhiteSpace(newTournament.Contribution))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля: Название, Начальная дата, Конечная дата и Взнос.",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем контент для POST-запроса
                var content = new StringContent(JsonConvert.SerializeObject(newTournament), Encoding.UTF8, "application/json");

                // Формируем URL для отправки запроса
                string url = $"{APIConfig.BaseUrl}/tournaments";

                // Добавляем токен доступа к заголовкам запроса
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Отправляем POST-запрос на сервер для добавления нового турнира
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Проверяем ответ сервера
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody != null)
                {
                    MessageBox.Show("Турнир успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении турнира: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}