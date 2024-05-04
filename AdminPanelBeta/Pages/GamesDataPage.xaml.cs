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
    /// Логика взаимодействия для GamesWin.xaml
    /// </summary>
    public partial class GamesDataPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public GamesDataPage()
        {
            InitializeComponent();
            Loaded += async (sender, e) => await LoadGamesData();
        }

        private async Task LoadGamesData()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Получаем токен из настроек приложения
                    string token = Properties.Settings.Default.Token;

                    // Проверяем, есть ли токен
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        MessageBox.Show("Отсутствует токен доступа. Пожалуйста, войдите в систему.");
                        return;
                    }

                    // Устанавливаем токен в заголовок Authorization
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // Выполняем запрос к API для получения данных об играх
                    HttpResponseMessage response = await client.GetAsync($"{APIConfig.BaseUrl}/games");
                    response.EnsureSuccessStatusCode();

                    // Читаем содержимое ответа
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Десериализуем JSON-ответ в список игр
                    var games = JsonConvert.DeserializeObject<List<Game>>(responseBody);

                    // Устанавливаем список игр в ListBox
                    ListBoxTovarList.ItemsSource = games;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private class Game
        {
            public string Name { get; set; }
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            var addgamesdatapage = new AddGamesDataPage();
            addgamesdatapage.ShowDialog();
        }

        private void EditGame(object sender, RoutedEventArgs e)
        {
            // Логика редактирования игры
        }

        private void DeleteGame(object sender, RoutedEventArgs e)
        {
            // Логика удаления игры
        }
    }
}