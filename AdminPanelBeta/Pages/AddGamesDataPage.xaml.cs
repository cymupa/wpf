using AdminPanelBeta.ConnectHttp;
using Microsoft.Win32;
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
    /// Логика взаимодействия для AddGamesDataPage.xaml
    /// </summary>
    public partial class AddGamesDataPage : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private string imagePath; // Объявляем переменную здесь
        public AddGamesDataPage()
        {
            InitializeComponent();
        }
        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            // Открываем диалоговое окно для выбора файла изображения
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем путь к выбранному файлу
                imagePath = openFileDialog.FileName;

                // Здесь можно использовать путь к файлу для отображения изображения или других операций
            }
        }

        private async void AddGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, есть ли токен доступа
                string token = Properties.Settings.Default.Token;
                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Отсутствует токен доступа. Пожалуйста, авторизуйтесь.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверяем, было ли введено название игры
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите название игры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем объект с данными новой игры
                var newGame = new
                {
                    name = NameTextBox.Text,
                    description = DescriptionTextBox.Text,
                    photo = imagePath // Путь к выбранному изображению
                };

                // Создаем контент для POST-запроса
                var content = new StringContent(JsonConvert.SerializeObject(newGame), Encoding.UTF8, "application/json");

                // Формируем URL для отправки запроса
                string url = $"{APIConfig.BaseUrl}/games";

                // Добавляем токен в заголовок запроса
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Отправляем POST-запрос на сервер для добавления новой игры
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Проверяем ответ сервера
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody != null)
                {
                    MessageBox.Show("Игра успешно добавлена.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении игры: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExitPage(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
