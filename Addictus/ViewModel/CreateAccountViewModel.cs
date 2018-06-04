using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Addictus.ViewModel
{
    class CreateAccountViewModel : INotifyPropertyChanged
    {
        private CreateAccountView caller;
        public CreateAccountViewModel(CreateAccountView window)
        {
            caller = window;
        }
        private string _username;
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
        private string cpasswordContent = "Confirm Password";
        private string return_btnContent = "Return";
        private string confirm_btnContent = "Confirm";

        private RelayCommand confirmCommand;
        public ICommand ConfirmCommand
        {
            get
            {
                if (confirmCommand == null)
                {
                    confirmCommand = new RelayCommand(
                        param => this.ConfirmCommandMethod(param),
                        param => true);
                }
                return confirmCommand;
            }
        }
        public void ConfirmCommandMethod(object o)
        {
            ManageAccount manage = new ManageAccount();
            PasswordBox pb = (PasswordBox)o;
            string username = Username;
            string password = pb.Password;
            bool valid = manage.AddAccount(username, password);
            if (valid)
            {
                this.caller.GoBackMethod();
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

        public string CPasswordContent
        {
            get
            {
                return cpasswordContent;
            }
        }

        public string ReturnButtonContent
        {
            get
            {
                return return_btnContent;
            }
        }

        public string ConfirmButtonContent
        {
            get
            {
                return confirm_btnContent;
            }
        }
    }
}
