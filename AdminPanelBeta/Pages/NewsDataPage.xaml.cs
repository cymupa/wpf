using AdminPanelBeta.ConnectHttp;
using AdminPanelBeta.Pages.Manager;
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
    /// Логика взаимодействия для NewsDataPage.xaml
    /// </summary>
    public partial class NewsDataPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public NewsDataPage()
        {
            InitializeComponent();
            Loaded += async (sender, e) => await LoadNewsData();
        }

        private async Task LoadNewsData()
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

                    // Выполняем запрос к API для получения данных о новостях
                    HttpResponseMessage response = await client.GetAsync($"{APIConfig.BaseUrl}/news");
                    response.EnsureSuccessStatusCode();

                    // Читаем содержимое ответа
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Десериализуем JSON-ответ в список новостей
                    var news = JsonConvert.DeserializeObject<List<News>>(responseBody);

                    // Устанавливаем список новостей в ListBox
                    ListBoxNewsList.ItemsSource = news;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            var addnewsdatapage = new AddNewsDataPage();
            addnewsdatapage.ShowDialog();
        }

        private class News
        {
            public string Name { get; set; }
        }
    }
}