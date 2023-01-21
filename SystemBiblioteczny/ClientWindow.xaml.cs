using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{
    public partial class ClientWindow : Window
    {

        public ClientWindow()
        {
            InitializeComponent();
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            foreach (Book e in listofBooks)
            {
                TableBooks.Items.Add(e);
            }
            TableBooks.IsReadOnly = true;
            Date.FontSize = 10;
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m = new();
            m.Show();
            this.Close();
        }

        private void Register_Evening(object sender, RoutedEventArgs e)
        {
            if (Date.SelectedDate == null) {
                MessageBox.Show("date null!");
                return;
            }

            String name = AuthorsName.Text;
            String lastname = AuthorsLastname.Text;
            int libraryID;
            if (int.TryParse(LibraryID.Text, out libraryID))
            {
                // TODO sprawdzić czy biblioteka istnieje
            }
            else {
                MessageBox.Show("Numer biblioteki podaj liczbą!");
            }
}

        private void Find(object sender, RoutedEventArgs e)
        {
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            TableBooks.Items.Clear();
            if (OptDostepnosc.IsChecked == true)
            {
                foreach (Book i in listofBooks)
                {
                    if (i.Availability == true) TableBooks.Items.Add(i);
                }
            }

            if (OptWszystkie.IsChecked == true)
            {
                foreach (Book j in listofBooks)
                {
                    TableBooks.Items.Add(j);
                }
            }
        }

        //String date = Date.SelectedDate?.ToString("HH:mm:ss");
        //String hour = EventTime.Text;
        //String duration = EventDuration.Text;
        //String phoneNumber = ContactNumber.Text;

        //AuthorsEvening newAuthorsEvening = new();



    }

    
    }


