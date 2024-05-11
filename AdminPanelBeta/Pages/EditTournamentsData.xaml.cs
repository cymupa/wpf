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
    public partial class EditTournamentsData : Window
    {
        public TournamentsData.Tournament SelectedTournament { get; set; }

        public EditTournamentsData(TournamentsData.Tournament selectedTournament)
        {
            InitializeComponent();
            SelectedTournament = selectedTournament;
            LoadTournamentData(SelectedTournament);
        }

        public void LoadTournamentData(TournamentsData.Tournament selectedTournament)
        {
            // Проверка наличия выбранного турнира
            if (selectedTournament != null)
            {
                // Заполнение элементов управления данными о выбранном турнире
                NameTextBox.Text = selectedTournament.Name;
                DescriptionTextBox.Text = selectedTournament.Description;
                StartDateTextBox.SelectedDate = selectedTournament.Start;
                EndDateTextBox.SelectedDate = selectedTournament.End;
                PaymentTextBox.Text = selectedTournament.Payment.ToString();
            }
        }

        private void ExitPage(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}