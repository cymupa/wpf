using System;
using System.Net.Http;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace AdminPanelBeta.ConnectHttp
{
    public class APIConfig
    {
        private readonly HttpClient httpClient;

        public APIConfig()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<List<User>> GetUsers(int limit)
        {
            try
            {
                string apiUrl = $"https://jsonplaceholder.typicode.com/users?_limit={limit}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Преобразуем полученные данные JSON в список пользователей
                List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);

                // Показываем уведомление об успешном подключении
                MessageBox.Show("Подключение к API успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                return users;
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если запрос не удался
                MessageBox.Show($"Ошибка при получении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public class User
        {
            public string phone { get; set; }
            public string name { get; set; } 
            public string City { get; set; }
            public string Role { get; set; } // Роль: "Администратор" или "Менеджер"
        }
    }
}