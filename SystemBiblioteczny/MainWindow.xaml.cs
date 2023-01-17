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
            User user = new User();
            var username = LoginEmail.Text;
            var password = LoginPassword.Password;

            user.UserName = "MarcelekFelek";
            user.Password = "123";

            if (username == null || password == null)
            {
                MessageBox.Show("Nie moga byc puste pola!");
                return;
            }

            if(username == user.UserName && password == user.Password)
            {
                MessageBox.Show("Zalogowano!");
                ClientWindow clientwindow = new ClientWindow(user);
                clientwindow.Show();
            }
            else
            {
                MessageBox.Show("Niepoprawne haslo");
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
