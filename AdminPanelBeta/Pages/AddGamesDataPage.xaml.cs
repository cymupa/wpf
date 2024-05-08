using AdminPanelBeta.ConnectHttp;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

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

                // Чтение содержимого изображения в массив байтов
                byte[] imageData = File.ReadAllBytes(_photoPath);

                // Создаем объект с данными новой игры
                var newGame = new
                {
                    name = NameTextBox.Text,
                    description = DescriptionTextBox.Text,
                    photo = imageData // Отправляем массив байтов изображения
                };

                // Создаем контент для POST-запроса
                var content = new StringContent(JsonConvert.SerializeObject(newGame), Encoding.UTF8, "application/json");

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

                // Отправляем POST-запрос на сервер для добавления новой игры
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
    }
}
