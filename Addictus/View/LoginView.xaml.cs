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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Addictus
{
    /// <summary>
    /// Interaktionslogik für LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new ViewModel.LoginViewModel(this);
        }

        private void btn_createAccount(object sender, RoutedEventArgs e)
        {
            CreateAccountMethod();
        }

        public void CreateAccountMethod()
        {
            CreateAccountView createAccount = new CreateAccountView();
            createAccount.Show();
            this.Close();
        }

        public void CreateMainWindow(User user)
        {
            MainWindowView mainWindowView = new MainWindowView();
            mainWindowView.CurrentUser = user;
            mainWindowView.Show();
            this.Close();
        }
    }
}
