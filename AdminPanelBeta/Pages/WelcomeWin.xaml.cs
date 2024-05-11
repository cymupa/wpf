using AdminPanelBeta.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminPanelBeta
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class WelcomeWin : Window
    {
        public WelcomeWin()
        {
            InitializeComponent();
        }
        private void ExitApp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Вы хотите выйти из программы?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void ButtonNavigateToAutorizatWindow(object sender, RoutedEventArgs e)
        {
            var autorizationWin = new AutorizationWin();
            autorizationWin.Show();
            this.Close();
        }
    }
}
