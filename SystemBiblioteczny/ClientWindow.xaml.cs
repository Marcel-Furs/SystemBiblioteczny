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
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

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
            String name = AuthorsName.Text;
            String lastname = AuthorsLastname.Text;
            int libraryID;
            if (!int.TryParse(LibraryID.Text, out libraryID)) MessageBox.Show("Numer biblioteki podaj liczbą!");
            DateTime? date = Date.SelectedDate;
            String hour = EventTime.Text;
            String duration = EventDuration.Text;
            String phoneNumber = ContactNumber.Text;

            //AuthorsEvening newAuthorsEvening = new(name, lastname, libraryID, date, hour, duration, phoneNumber);

        }

        
        private void OptAvailability_Checked(object sender, RoutedEventArgs e)
        {
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            TableBooks.Items.Clear();
            if (OptAvailability.IsChecked == true)
            {
                foreach (var i in listofBooks)
                {
                    if (i.Availability == true) TableBooks.Items.Add(i);
                }
            }
        }

        private void OptAll_Checked(object sender, RoutedEventArgs e)
        {
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            TableBooks.Items.Clear();
            if (OptAll.IsChecked == true)
            {
                var sort1 = listofBooks.OrderBy(x => x.Id_Book).ToList();
                sort1.ForEach(x =>
                {
                    TableBooks.Items.Add(x);
                });
            }
        }

        private void OptAuthor_Checked(object sender, RoutedEventArgs e)
        {
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            TableBooks.Items.Clear();
            if (OptAuthor.IsChecked == true)
            {
                var sort1 = listofBooks.OrderBy(x => x.Author).ToList();
                sort1.ForEach(x =>
                {
                    TableBooks.Items.Add(x);
                });

            }
        }

        private void OptTitle_Checked(object sender, RoutedEventArgs e)
        {
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            if (OptTitle.IsChecked == true)
            {
                var sort1 = listofBooks.OrderBy(x => x.Title).ToList();
                sort1.ForEach(x =>
                {
                    TableBooks.Items.Add(x);
                });
            }
        }



    }

    
    }


