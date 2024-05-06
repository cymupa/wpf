using AdminPanelBeta.ConnectHttp;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdminPanelBeta.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddGamesDataPage.xaml
    /// </summary>
    public partial class AddGamesDataPage : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public AddGamesDataPage()
        {
            InitializeComponent();
        }

        private async void AddGameButton_Click(object sender, RoutedEventArgs e)
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

                // Получаем байтовый массив изображения
                byte[] imageData = GetImageBytes(img_1);

                // Проверяем, было ли выбрано изображение
                if (imageData == null)
                {
                    MessageBox.Show("Пожалуйста, выберите изображение для игры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем объект с данными новой игры
                var gameData = new
                {
                    name = NameTextBox.Text,
                    description = DescriptionTextBox.Text,
                    photoData = Convert.ToBase64String(imageData)
                };

                // Создаем контент для POST-запроса
                var content = new StringContent(JsonConvert.SerializeObject(gameData), Encoding.UTF8, "application/json");

                // Формируем URL для отправки запроса
                string url = $"{APIConfig.BaseUrl}/games";

                // Добавляем токен в заголовок запроса
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Отправляем POST-запрос на сервер для добавления новой игры
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                // Проверяем ответ сервера
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    MessageBox.Show("Игра успешно добавлена.");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при отправке запроса на сервер: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonSerializationException ex)
            {
                MessageBox.Show($"Ошибка при сериализации данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
                openFileDialog.Multiselect = false; // Разрешаем выбирать только один файл

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedImagePath = openFileDialog.FileName;
                    DisplaySelectedImage(selectedImagePath); // Отображаем выбранное изображение
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplaySelectedImage(string imagePath)
        {
            try
            {
                // Устанавливаем источник выбранного изображения в img_1
                img_1.Source = new BitmapImage(new Uri(imagePath));

                // Показываем img_1 и кнопку удаления
                img_1.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отображении выбранного изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Устанавливаем источник изображения img_1 равным изображению NoPhoto.png
                img_1.Source = new BitmapImage(new Uri("pack://application:,,,/Images/no_photo.png"));

                // Скрываем кнопку удаления на img_1
                deleteButton.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitPage(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        // Метод для получения байтового массива изображения
        private byte[] GetImageBytes(Image image)
        {
            if (image.Source is BitmapImage bitmapImage)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Используйте соответствующий кодировщик в зависимости от формата изображения
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(ms);
                    return ms.ToArray();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
