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

namespace Addictus
{
    /// <summary>
    /// Interaktionslogik für CreateAccountView.xaml
    /// </summary>
    public partial class CreateAccountView : Window
    {
        public CreateAccountView()
        {
            InitializeComponent();
            DataContext = new ViewModel.CreateAccountViewModel(this);
        }

        public string test { get; set; }
        private void btn_GoBack(object sender, RoutedEventArgs e)
        {
            GoBackMethod();
        }

        public void GoBackMethod()
        {
            LoginView login = new LoginView();
            login.Show();
            this.Close();
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;
            string s1 = CreateAccount_PasswordBox.Password;
            string s2 = pb.Password;
            bool match = s1.Equals(s2);
            if (!match)
            {
                CreateAccount_ConfirmButton.IsEnabled = false;
            }
            else
            {
                CreateAccount_ConfirmButton.IsEnabled = true;
            }
        }
    }
}
