using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

namespace Addictus.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private LoginView caller;
        public LoginViewModel(LoginView window)
        {
            caller = window;
        }
        private string _username { get; set; }
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(Username);
            }
        }

        private string usernameContent = "Username";
        private string passwordContent = "Password";
        private string createAccount_btnContent = "Create Account";
        private string login_btnContent = "Login";

        private RelayCommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new RelayCommand(
                        param => this.LoginCommandMethod(param),
                        param => true);
                }
                return loginCommand;
            }
        }
        public void LoginCommandMethod(object password)
        {
            PasswordBox pb = (PasswordBox)password;
            bool valid = false;
            ManageAccount manage = new ManageAccount();
            valid = manage.CheckUserValidation(Username, pb.Password);
            if (valid)
            {
                User user = new User(Username);
                caller.CreateMainWindow(user);
            }
            else
            {
                MessageBox.Show("Username or password are not valid");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public string UsernameContent
        {
            get
            {
                return usernameContent;
            }
        }

        public string PasswordContent
        {
            get
            {
                return passwordContent;
            }
        }

        public string CreateAccountButtonContent
        {
            get
            {
                return createAccount_btnContent;
            }
        }

        public string LoginButtonContent
        {
            get
            {
                return login_btnContent;
            }
        }
    }
}
