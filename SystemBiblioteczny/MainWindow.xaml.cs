using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            LoginMethod l = new();
            bool canProceed = l.CheckIfAllDataIsCorrectAndCanCreateAccount(username, password, confirmPassword, name, lastname, email);
            if (canProceed == false) return;
            Client newClient = new(username, password, name, lastname, email, phoneNumber);
            l.AddUserToDB(newClient);
            ClientWindow clientwindow = new();
            clientwindow.Show();
            this.Close();
        }

        private void Sign_Client(object sender, RoutedEventArgs e)
        {
            var username = LoginEmail.Text;
            var password = LoginPassword.Password;

            AccountBase.RoleTypeEnum role = AccountBase.RoleTypeEnum.Client;

            if (clientOption.IsChecked == true) role = AccountBase.RoleTypeEnum.Client;
            if (admin_localOption.IsChecked == true) role = AccountBase.RoleTypeEnum.LocalAdmin;
            if (admin_networkOption.IsChecked == true) role = AccountBase.RoleTypeEnum.NetworkAdmin;
            if (librarianOption.IsChecked == true) role = AccountBase.RoleTypeEnum.Librarian;

            LoginMethod loginMethod = new LoginMethod();
            bool logged = loginMethod.CheckLogin(username, password, role);

            if (logged == true) { this.Close(); }

        }

        private void Hint(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Opcjonalnie");
        }

        protected override void OnClosing(CancelEventArgs e) //nie wiem czemu ale nie dziala
        {
            base.OnClosing(e);
        }

        private void RegisterUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginMethod l = new();
            RegisterUsername.Text = l.EraseWhiteSpace(RegisterUsername.Text);
        }

        private void RegisterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginMethod l = new();
            RegisterName.Text = l.EraseWhiteSpace(RegisterName.Text);
        }

        private void RegisterLastname_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginMethod l = new();
            RegisterLastname.Text = l.EraseWhiteSpace(RegisterLastname.Text);
        }

        private void RegisterEmailAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginMethod l = new();
            RegisterEmailAddress.Text = l.EraseWhiteSpace(RegisterEmailAddress.Text);
        }

        private void RegisterPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginMethod l = new();
            RegisterPhoneNumber.Text = l.EraseWhiteSpace(RegisterPhoneNumber.Text);
        }

        private void RegisterPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginMethod l = new();
            RegisterPassword.Password = l.EraseWhiteSpace(RegisterPassword.Password);
        }
        private void RegisterPasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginMethod l = new();
            RegisterPasswordConfirmation.Password = l.EraseWhiteSpace(RegisterPasswordConfirmation.Password);
        }
    }
}