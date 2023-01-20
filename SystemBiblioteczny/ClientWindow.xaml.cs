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

        }

        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m = new();
            m.Show();
            this.Close();
        }

        private void AuthorsEvening(object sender, RoutedEventArgs e)
        {
            AuthorEveningEventWindow m = new();
            m.Show();
            this.Close();
        }

        private void Book_Available(object sender, RoutedEventArgs e)
        {
            BookAvalilableWindow w = new();
            w.Show();
            this.Close();
        }

        private void Register_Evening(object sender, RoutedEventArgs e)
        {

        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
    }
}
