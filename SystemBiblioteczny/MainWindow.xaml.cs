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
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Register_Client(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jeszcze ten knefel nie dziala");
        }

        private void Sign_Client(object sender, RoutedEventArgs e)
        {

            Options option = new();

            var username = LoginEmail.Text;
            var password = LoginPassword.Password;

            int opt = 0;
            bool opt1 = false;
            //to trzeba zoptymalizować, nie wiem jak do switch'a dac te opcje
            if(clientOption.IsChecked == true) opt = 1;
            if(admin_localOption.IsChecked == true) opt = 2;
            if(admin_networkOption.IsChecked == true) opt = 3;
            if(librarianOption.IsChecked ==true) opt = 4;

            switch (opt)
            {
                case 1:
                    option.ClientOpt(username, password);
                    this.Close();
                    break;
                case 2:
                    option.Admin_localOpt(username, password);
                    this.Close();
                    break;
                case 3:
                    option.Admin_networklOpt(username, password);
                    this.Close();
                    break;
                case 4:
                    option.LibrarianOpt(username, password);
                    this.Close();
                    break;
                default:
                    MessageBox.Show("Proszę wybrać opcje logowania");
                    break;
            }

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