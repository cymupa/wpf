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
    public partial class EditUserDataPage : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly UserDataPage.User _user;
        private readonly string _token;

        public EditUserDataPage(UserDataPage.User user, string token)
        {
            InitializeComponent();
            _user = user;
            _token = token;
            DataContext = _user;
        }

        private async void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем токен доступа из настроек приложения
                string token = Properties.Settings.Default.Token;

                // Проверяем, есть ли токен доступа
                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("Отсутствует токен доступа. Пожалуйста, войдите в систему.");
                    return;
                }

                // Создаем объект с обновленными данными пользователя
                var updatedUser = new
                {
                    id = _user.Id,
                    name = _user.Name,
                    surname = _user.Surname,
                    patronymic = _user.Patronymic,
                    birth = _user.Birth.ToString("yyyy-MM-dd"),
                    tel = _user.Tel,
                    address = _user.Address,
                    password = _user.Password,
                    role_id = _user.Role_id
                };

                // Добавляем токен доступа в заголовок запроса
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Отправляем обновленные данные на сервер методом POST
                HttpResponseMessage response = await _httpClient.PostAsync($"{APIConfig.BaseUrl}/users",
                    new StringContent(JsonConvert.SerializeObject(updatedUser), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Данные пользователя успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close(); // Закрываем окно редактирования после успешного обновления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExitPage(object sender, RoutedEventArgs e)
        {
            Close(); // Закрываем окно редактирования при нажатии кнопки "Отмена"
        }
    }
}