using AdminPanelBeta.Pages.Manager;
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

namespace AdminPanelBeta.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuWin.xaml
    /// </summary>
    public partial class MenuWin : Window
    {

        public MenuWin()
        {
            InitializeComponent();
            // Получаем данные пользователя из настроек приложения
            string name = Properties.Settings.Default.Name;
            string surname = Properties.Settings.Default.Surname;
            string role = Properties.Settings.Default.Role;

            // Отображаем данные в соответствующих TextBlock элементах
            TextBlockFIOUser.Text = $"{name} {surname}";
            TextBlockPositionUser.Text = GetRoleName(role);

            // Скрываем или показываем кнопки в зависимости от роли
            switch (role)
            {
                case "2": // Администратор
                    NewsButton.Visibility = Visibility.Collapsed; // Скрываем кнопку Новости
                    BroadcastsButton.Visibility = Visibility.Collapsed; // Скрываем кнопку Трансляции
                    break;
                case "3": // Менеджер
                    TournamentsButton.Visibility = Visibility.Collapsed; // Скрываем кнопку Турниры
                    GamesButton.Visibility = Visibility.Collapsed; // Скрываем кнопку Игры
                    StatisticsButton.Visibility = Visibility.Collapsed; // Скрываем кнопку Статистика
                    EmployeesButton.Visibility = Visibility.Collapsed; // Скрываем кнопку Статистика
                    break;
                default:
                    break;
            }
        }

        // Метод для получения текстового представления роли по числовому значению
        private string GetRoleName(string roleId)
        {
            switch (roleId)
            {
                case "2":
                    return "Администратор";
                case "3":
                    return "Менеджер";
                default:
                    return "Неизвестная роль";
            }
        }

        private void ButtonNavigateToWelcomeWindow(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Вы хотите выйти из учетной записи?", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
            {
                return;
            }
            Properties.Settings.Default.Reset();
            var welcomeWindow = new WelcomeWin();
            welcomeWindow.Show();
            this.Close();
        }

        private void NavigateFrame(Page page)
        {
            FrameNavigateToPage.NavigationService.Navigate(page);
        }

        private void EmployeesToButton(object sender, RoutedEventArgs e)
        {
            UserDataPage userDataPage = new UserDataPage();
            NavigateFrame(userDataPage);
        }
        private void NewsToButton(object sender, RoutedEventArgs e)
        {
            NewsDataPage newsdatapage = new NewsDataPage();
            NavigateFrame (newsdatapage);
        }

        private void GameToButton(object sender, RoutedEventArgs e)
        {
            GamesDataPage GamesDataPage = new GamesDataPage();
            NavigateFrame(GamesDataPage);
        }
        private void StatisticsToButton(object sender, RoutedEventArgs e)
        {
            StatisticsDataPage statisticsdatapage = new StatisticsDataPage();
            NavigateFrame(statisticsdatapage);
        }
    }
}