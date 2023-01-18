using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml.Linq;
using SystemBiblioteczny.Methods;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            clientOption.IsChecked = true;
        }

        private void Register_Client(object sender, RoutedEventArgs e)
        {
            var username = RegisterUsername.Text;
            var password = RegisterPassword.Password;
            var confirmPassword = RegisterPasswordConfirmation.Password;
            var name = RegisterName.Text;
            var lastname = RegisterLastname.Text;
            var email = RegisterEmailAddress.Text;
            var phoneNumber = RegisterPhoneNumber.Text;
            AccountBase.RoleTypeEnum role = AccountBase.RoleTypeEnum.Client;
            LoginMethod c = new();
            bool canProceed = c.CheckIfAllDataIsCorrectAndCanCreateAccount(username, password, confirmPassword, name, lastname, email);
            if (canProceed == false) return;
            Client newClient = new(username, password, name, lastname, email, phoneNumber);
            c.AddUserToDB(newClient);
        }

        private void Sign_Client(object sender, RoutedEventArgs e)
        {
            var username = LoginEmail.Text;
            var password = LoginPassword.Password;

            AccountBase.RoleTypeEnum role = AccountBase.RoleTypeEnum.Client;
            
            if (clientOption.IsChecked == true)  role = AccountBase.RoleTypeEnum.Client; 
            if (admin_localOption.IsChecked == true)  role = AccountBase.RoleTypeEnum.LocalAdmin; 
            if (admin_networkOption.IsChecked == true)  role = AccountBase.RoleTypeEnum.NetworkAdmin; 
            if (librarianOption.IsChecked == true) role = AccountBase.RoleTypeEnum.Librarian; 

            LoginMethod loginMethod = new LoginMethod();
            bool logged = loginMethod.CheckLogin(username, password, role);

            if(logged == true) { this.Close(); }
       
        }

        private void Hint(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Opcjonalnie");
        }

        protected override void OnClosing(CancelEventArgs e) //nie wiem czemu ale nie dziala
        {
            base.OnClosing(e);
        }

    }
}