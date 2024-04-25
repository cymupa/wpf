using AdminPanelBeta.ConnectHttp;
using System;
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
using static AdminPanelBeta.ConnectHttp.APIConfig;

namespace AdminPanelBeta.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserData.xaml
    /// </summary>
    public partial class UserDataPage : Page
    {
        public UserDataPage()
        {
            InitializeComponent();
            LoadUsers();
        }
        private async void LoadUsers()
        {
            try
            {
                // Создаем экземпляр класса APIConfig
                APIConfig apiConfig = new APIConfig();

                // Получаем список пользователей из API
                List<User> users = await apiConfig.GetUsers(10);

                if (users != null)
                {
                    // Очищаем ListBox
                    ListBoxUsersList.Items.Clear();

                    // Добавляем каждого пользователя в ListBox
                    foreach (var user in users)
                    {
                        ListBoxUsersList.Items.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void SelectionChangedUsers(object sender, SelectionChangedEventArgs e)
        {
            // Обработка изменения выбранного элемента в ListBox
        }
        private void EditUsersToNavigateWin(object sender, RoutedEventArgs e) 
        {
        
        }
        private void DeleteUsers(object sender, RoutedEventArgs e)
        {

        }


    }

}