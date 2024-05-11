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
using static AdminPanelBeta.Pages.UserDataPage;

namespace AdminPanelBeta.Pages
{
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
                string token = Properties.Settings.Default.Token;

                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Отсутствует токен доступа. Пожалуйста, войдите в систему.");
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/games");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var games = JsonConvert.DeserializeObject<List<Game>>(responseBody);

                ListBoxTovarList.ItemsSource = games;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void EditGamesToNavigateWin(object sender, RoutedEventArgs e)
        {
            Game selectedGame = (sender as FrameworkElement).DataContext as Game;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/games/{selectedGame.Id}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var gameData = JsonConvert.DeserializeObject<Game>(responseBody);

                var editGamesDataPage = new EditGamesDataPage(gameData);
                editGamesDataPage.ShowDialog();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных игры: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class Game
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Photo { get; set; }

        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            var addGamesDataPage = new AddGamesDataPage();
            addGamesDataPage.ShowDialog();
        }

        private void DeleteGame(object sender, RoutedEventArgs e)
        {
            // Логика удаления игры
        }
    }
}