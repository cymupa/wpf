﻿using System;
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
    /// Логика взаимодействия для EditNewsDataPage.xaml
    /// </summary>
    public partial class EditNewsDataPage : Window
    {
        public EditNewsDataPage()
        {
            InitializeComponent();
        }
        private void ExitPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
