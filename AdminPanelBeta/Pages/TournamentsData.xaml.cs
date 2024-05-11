using System;
using AdminPanelBeta.ConnectHttp;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для TournamentsData.xaml
    /// </summary>
    public partial class TournamentsData
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public TournamentsData()
        {
            InitializeComponent();
            LoadTournaments();
        }

        private async Task<List<Tournament>> LoadTournamentsFromApi(string token)
        {
            try
            {
                string url = $"{APIConfig.BaseUrl}/tournaments";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Tournament> tournaments = JsonConvert.DeserializeObject<List<Tournament>>(responseBody);
                return tournaments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке турниров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private async void LoadTournaments()
        {
            string token = Properties.Settings.Default.Token;
            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("Отсутствует токен доступа. Пожалуйста, войдите в систему.");
                return;
            }

            List<Tournament> tournaments = await LoadTournamentsFromApi(token);
            if (tournaments != null)
            {
                ListBoxTovarList.ItemsSource = tournaments;
            }
        }
        public class Tournament
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public decimal Payment { get; set; }
            public string LiveLink { get; set; }
            public int GameId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
        private void AddTournamentsButton(object sender, RoutedEventArgs e)
        {
            var addtournamentsdata = new AddTournamentsData();
            addtournamentsdata.ShowDialog();
        }

        private void EditTournamentsToNavigateWin(object sender, RoutedEventArgs e)
        {

        }

    }
}
