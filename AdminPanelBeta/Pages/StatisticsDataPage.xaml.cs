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
                SalesTodayTextBlock.Text = statistics.SalesToday.ToString();
                SalesLastMonthTextBlock.Text = statistics.SalesLastMonth.ToString();
                SalesLastYearTextBlock.Text = statistics.SalesLastYear.ToString();
                SalesTotalTextBlock.Text = statistics.SalesTotal.ToString();

                // Загружаем список категорий
                await LoadCategories();
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
                // Получаем список категорий
                HttpResponseMessage categoriesResponse = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/categories");
                categoriesResponse.EnsureSuccessStatusCode();
                string categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(categoriesJson);

                // Отображаем категории в ComboBox
                ComboBoxCategories.ItemsSource = categories;
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
                    string categoryName = (ComboBoxCategories.SelectedItem as Category).Name;

                    // Получаем продукты по выбранной категории
                    HttpResponseMessage productsResponse = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/sales/category/{categoryName}");
                    productsResponse.EnsureSuccessStatusCode();
                    string productJson = await productsResponse.Content.ReadAsStringAsync();
                    Product product = JsonConvert.DeserializeObject<Product>(productJson);

                    // Отображаем продукт на странице
                    // Вместо ListBoxProducts.ItemsSource используем прямое присвоение
                    //ListBoxProducts.ItemsSource = product;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке продуктов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchNameTovarTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchNameTovarTextBox.Text;

                // Проверяем, пуст ли текстовый запрос
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    // Если запрос пуст, загружаем общую статистику
                    await LoadStatistics();
                }
                else
                {
                    // Выполняем запрос к API для поиска товаров по тексту
                    HttpResponseMessage searchResponse = await _httpClient.GetAsync($"{APIConfig.BaseUrl}/sales/search?query={searchText}");
                    searchResponse.EnsureSuccessStatusCode();
                    string searchJson = await searchResponse.Content.ReadAsStringAsync();
                    SalesStatistics searchStatistics = JsonConvert.DeserializeObject<SalesStatistics>(searchJson);

                    // Отображаем статистику по найденным товарам
                    SalesTodayTextBlock.Text = searchStatistics.SalesToday.ToString();
                    SalesLastMonthTextBlock.Text = searchStatistics.SalesLastMonth.ToString();
                    SalesLastYearTextBlock.Text = searchStatistics.SalesLastYear.ToString();
                    SalesTotalTextBlock.Text = searchStatistics.SalesTotal.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private class SalesStatistics
        {
            public decimal SalesToday { get; set; }
            public decimal SalesLastMonth { get; set; }
            public decimal SalesLastYear { get; set; }
            public decimal SalesTotal { get; set; }
        }

        private class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }

        private class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public string Photo { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }
}
