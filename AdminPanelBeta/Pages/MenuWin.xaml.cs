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
    }
}
