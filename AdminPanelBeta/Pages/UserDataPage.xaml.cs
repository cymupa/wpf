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
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Patronymic { get; set; }
            public string Pass { get; set; }
            public string Tel { get; set; }
            public string Address { get; set; }
            public string Password { get; set; }
            public DateTime Birth { get; set; }
            public string Role_id { get; set; }

            // Список ролей для ComboBox
            public List<Role> RolesList => new List<Role>
             {
                 new Role { Id = "1", Name = "Юзер" },
                 new Role { Id = "2", Name = "Администратор" },
                 new Role { Id = "3", Name = "Менеджер" }
             };

            private Role _selectedRole;

            public Role SelectedRole
            {
                get { return _selectedRole; }
                set
                {
                    _selectedRole = value;
                    Role_id = value.Id;
                }
            }
            public string RoleName
            {
                get
                {
                    switch (Role_id)
                    {
                        case "1":
                            return "Юзер";
                        case "2":
                            return "Администратор";
                        case "3":
                            return "Менеджер";
                        default:
                            return "";
                    }
                }
            }
        }

        public class Role
        {
            public string Id { get; set; }
            public string Name { get; set; }
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
            // Получение выбранного пользователя
            User selectedUser = (sender as FrameworkElement).DataContext as User;

            // Открытие окна редактирования с передачей выбранного пользователя и токена
            var edituserdatapage = new EditUserDataPage(selectedUser, Properties.Settings.Default.Token);
            edituserdatapage.ShowDialog();
        }

        private async void DeleteUsers(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Получение выбранного пользователя
                User selectedUser = (sender as FrameworkElement).DataContext as User;

                // Проверка, действительно ли пользователь хочет удалить
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить пользователя {selectedUser.Name}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return; // Если пользователь отказался от удаления, ничего не делаем
                }

                // Получение токена из настроек приложения
                string token = Properties.Settings.Default.Token;

                // Проверка наличия токена
                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Отсутствует токен доступа. Пожалуйста, войдите в систему.");
                    return;
                }

                // Установка токена в заголовок Authorization
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Отправка запроса на сервер для удаления пользователя
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{APIConfig.BaseUrl}/users/{selectedUser.Id}");
                response.EnsureSuccessStatusCode();

                // Удаление пользователя из списка на клиентской стороне
                if (ListBoxUsersList.ItemsSource is List<User> users)
                {
                    users.Remove(selectedUser);
                    ListBoxUsersList.ItemsSource = null;
                    ListBoxUsersList.ItemsSource = users;
                }

                MessageBox.Show("Пользователь успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}