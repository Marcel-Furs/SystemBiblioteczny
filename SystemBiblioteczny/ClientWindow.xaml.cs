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

        private void AuthorsEvening(object sender, RoutedEventArgs e)
        {
            
        }

        private void Book_Available(object sender, RoutedEventArgs e)
        {

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
            if (OptAvailability.IsChecked == true)
            {
                foreach (var i in listofBooks)
                {
                    if (i.Availability == true) TableBooks.Items.Add(i);
                }
            }

            if (OptAll.IsChecked == true)
            {
                foreach (var j in listofBooks)
                {
                    TableBooks.Items.Add(j);
                }
            }

        }

        private void Sort(object sender, RoutedEventArgs e)
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
