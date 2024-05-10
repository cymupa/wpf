using AdminPanelBeta.ConnectHttp;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System;

namespace AdminPanelBeta.Pages
{
    public partial class AddGamesDataPage : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private string _photoPath;

        public AddGamesDataPage()
        {
            InitializeComponent();
        }

        private async void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, выбрано ли изображение
                if (string.IsNullOrWhiteSpace(_photoPath))
                {
                    MessageBox.Show("Пожалуйста, выберите изображение для игры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Конвертируем изображение в формат base64
                string base64Image = ConvertImageToBase64(_photoPath);

                var newGame = new
                {
                    name = NameTextBox.Text,
                    description = DescriptionTextBox.Text,
                    photo = base64Image // Передаем изображение в формате base64
                };

                // Сериализуем объект данных игры в JSON
                string jsonData = JsonConvert.SerializeObject(newGame);

                // Получаем токен из настроек или иного источника
                string token = Properties.Settings.Default.Token;

                // Проверяем, что токен не пустой
                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Отсутствует токен доступа. Пожалуйста, войдите в систему.");
                    return;
                }

                // Добавляем токен в заголовок запроса
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Формируем URL для отправки запроса
                string url = $"{APIConfig.BaseUrl}/games";

                // Отправляем JSON-данные на сервер
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Проверяем ответ сервера
                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody != null)
                {
                    MessageBox.Show("Игра успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении игры: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Открытие диалогового окна для выбора файла изображения
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    // Отображение выбранного изображения
                    DisplaySelectedImage(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выборе изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplaySelectedImage(string imagePath)
        {
            try
            {
                // Загрузка выбранного изображения в Image
                img_1.Source = new BitmapImage(new Uri(imagePath));
                // Сохраняем путь к выбранному изображению
                _photoPath = imagePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Удаляем изображение и отображаем базовую фотографию
            img_1.Source = new BitmapImage(new Uri("/Images/no_photo.png", UriKind.Relative));
            // Сбрасываем путь к выбранному изображению
            _photoPath = null;
        }

        private void ExitPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Метод для конвертации изображения в формат base64
        private string ConvertImageToBase64(string imagePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64Image = Convert.ToBase64String(imageBytes);
                return base64Image;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при конвертации изображения в base64: {ex.Message}");
            }
        }
    }
}