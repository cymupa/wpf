using AdminPanelBeta.ConnectHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdminPanelBeta.Pages
{
    public partial class StatisticsDataPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public StatisticsDataPage()
        {
            InitializeComponent();
            Loaded += async (sender, e) => await LoadStatistics();

            // Загружаем список категорий при запуске страницы
            LoadCategories();
        }

        private async Task LoadStatistics()
        {
            try
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
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Получаем статистику о продажах
                HttpResponseMessage statisticsResponse = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/sales");
                statisticsResponse.EnsureSuccessStatusCode();
                string statisticsJson = await statisticsResponse.Content.ReadAsStringAsync();
                SalesStatistics statistics = JsonConvert.DeserializeObject<SalesStatistics>(statisticsJson);

                // Отображаем статистику на странице
                SalesTodayTextBlock.Text = statistics.Sales_Today.ToString();
                SalesLastMonthTextBlock.Text = statistics.Sales_Last_Month.ToString();
                SalesLastYearTextBlock.Text = statistics.Sales_Last_Year.ToString();
                SalesTotalTextBlock.Text = statistics.Sales_Total.ToString();

                // Загружаем список категорий
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadCategories()
        {
            try
            {
                // Очищаем ComboBoxCategories перед загрузкой категорий
                ComboBoxCategories.ItemsSource = null;

                // Получаем список категорий
                HttpResponseMessage categoriesResponse = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/categories");
                categoriesResponse.EnsureSuccessStatusCode();
                string categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(categoriesJson);

                // Отображаем категории в ComboBox
                ComboBoxCategories.ItemsSource = categories;

                // Сбрасываем выбранную категорию
                ComboBoxCategories.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ComboBoxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxCategories.SelectedItem != null)
                {
                    int categoryId = (ComboBoxCategories.SelectedItem as Category).Id;

                    // Получаем статистику продаж по выбранной категории
                    HttpResponseMessage categoryStatisticsResponse = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/sales/category/{categoryId}");
                    categoryStatisticsResponse.EnsureSuccessStatusCode();
                    string categoryStatisticsJson = await categoryStatisticsResponse.Content.ReadAsStringAsync();
                    SalesStatistics categoryStatistics = JsonConvert.DeserializeObject<SalesStatistics>(categoryStatisticsJson);

                    // Отображаем статистику продаж по выбранной категории на странице
                    SalesTodayTextBlock.Text = categoryStatistics.Sales_Today.ToString();
                    SalesLastMonthTextBlock.Text = categoryStatistics.Sales_Last_Month.ToString();
                    SalesLastYearTextBlock.Text = categoryStatistics.Sales_Last_Year.ToString();
                    SalesTotalTextBlock.Text = categoryStatistics.Sales_Total.ToString();
                }
                else
                {
                    // Если категория не выбрана, отображаем базовую статистику
                    await LoadStatistics();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики продаж: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public class SalesStatistics
        {
            public decimal Sales_Today { get; set; }
            public decimal Sales_Last_Month { get; set; }
            public decimal Sales_Last_Year { get; set; }
            public decimal Sales_Total { get; set; }
        }

        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }
}