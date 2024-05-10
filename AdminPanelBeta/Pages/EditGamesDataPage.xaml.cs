using AdminPanelBeta.ConnectHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class EditGamesDataPage : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly GamesDataPage.Game _game;

        public EditGamesDataPage(GamesDataPage.Game game)
        {
            InitializeComponent();
            _game = game;
            Loaded += EditGamesDataPage_Loaded;
        }

        private async void EditGamesDataPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/games/{_game.Id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var gameData = JsonConvert.DeserializeObject<GamesDataPage.Game>(responseBody);

                NameTextBox.Text = gameData.Name;
                DescriptionTextBox.Text = gameData.Description;

                // Вызов метода для загрузки изображения
                // await LoadGameImageData();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task LoadGameImageData()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/games/{_game.Id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var gameData = JsonConvert.DeserializeObject<GamesDataPage.Game>(responseBody);

                NameTextBox.Text = gameData.Name;
                DescriptionTextBox.Text = gameData.Description;

                // Загрузка изображения
                if (!string.IsNullOrEmpty(gameData.Photo))
                {
                    // Получаем изображение по URL
                    HttpResponseMessage imageResponse = await _httpClient.GetAsync(gameData.Photo);
                    imageResponse.EnsureSuccessStatusCode();

                    // Получаем байты изображения
                    byte[] imageData = await imageResponse.Content.ReadAsByteArrayAsync();

                    // Конвертируем байты в изображение
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();

                    // Устанавливаем изображение как источник для элемента Image
                    img_1.Source = bitmapImage;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Добавьте здесь логику для сохранения изменений
        }

        private void ExitPage(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}