using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{
    /// <summary>
    /// Logika interakcji dla klasy LibrarianWindow.xaml
    /// </summary>
    public partial class LibrarianWindow : Window
    {
        private AccountBase accountModel = new();
        private Librarian librarianModel = new();
        public LibrarianWindow(Librarian userData)
        {
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            librarianModel = userData;
            
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m = new();
            m.Show();
            this.Close();
        }

        private void SendApplication(object sender, RoutedEventArgs e)
        {
            var title = TitleInput.Text;
            var author = AuthorInput.Text;
            var quantity = QuantityInput.Text;
            var librarian = librarianModel.UserName!;
            ApplicationBook applicationBook = new(title, author, quantity, librarian, false);

            

            if (title.Any() && author.Any()  && quantity.Any())
            {
                MessageBox.Show("Wysłano zgłoszenie zapotrzebowania na: " + "\n" + author + " " + title + " - ilość: " + quantity);


                List<string> lines = accountModel.GetListOfDataBaseLines("BookApplicationList");
                accountModel.WriteToDataBase("BookApplicationList", TitleInput.Text + " " + AuthorInput.Text + " " + QuantityInput.Text + " " + librarian + " " + false);
            }
            else
            {
                MessageBox.Show("Niewystarczająca ilość danych, uzupełnij wszystkie dane!");
            }
        }

    }
}