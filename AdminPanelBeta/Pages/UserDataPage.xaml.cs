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
using static AdminPanelBeta.ConnectHttp.APIConfig;

namespace AdminPanelBeta.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserData.xaml
    /// </summary>
    public partial class UserDataPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public UserDataPage()
        {
            InitializeComponent();
            Loaded += async (sender, e) => await LoadUserData();
        }
        public async Task LoadUserData()
        {
            await LoadUsersDataAsync();
        }

        private async Task LoadUsersDataAsync()
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

                    // Выполняем запрос к API для получения данных о пользователях
                    HttpResponseMessage response = await client.GetAsync($"{APIConfig.BaseUrl}/users");
                    response.EnsureSuccessStatusCode();

                    // Читаем содержимое ответа
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Десериализуем JSON-ответ в список сотрудников
                    var users = JsonConvert.DeserializeObject<List<User>>(responseBody)
                        .Where(user => user.Role_id == "2" || user.Role_id == "3") // Фильтруем пользователей по ID роли
                        .ToList();

                    // Устанавливаем список сотрудников в ListBox
                    ListBoxUsersList.ItemsSource = users;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private class User
        {
            public string Name { get; set; }
            public string Tel { get; set; }
            public string Role_id { get; set; }

            public string Role
            {
                get
                {
                    if (Role_id == "2")
                        return "Администратор";
                    else if (Role_id == "3")
                        return "Менеджер";
                    else
                        return "";
                }
            }
        }
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var adduserdatapage = new AddUserDataPage();
            adduserdatapage.ShowDialog();
        }

        private void SelectionChangedUsers(object sender, SelectionChangedEventArgs e)
        {
            // Логика выбора сотрудника
        }

        private void EditUsersToNavigateWin(object sender, RoutedEventArgs e)
        {
            // Логика редактирования сотрудника
        }

        private void DeleteUsers(object sender, RoutedEventArgs e)
        {
            // Логика удаления сотрудника
        }
    }
}