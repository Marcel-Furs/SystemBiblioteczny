using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Logika interakcji dla klasy Admin_localWindow.xaml
    /// </summary>
    public partial class Admin_LocalWindow : Window
    {
        public Admin_LocalWindow(LocalAdmin userData)
        {
            InitializeComponent();
            nazwaLabel.Content = userData.UserName;
            numerLabel.Content = userData.LibraryId;

            BookExchange books = new();
            List<BookExchange> listofBooks = books.GetExchangeBooksList();
            foreach (BookExchange e in listofBooks)
            {
                TableExchangeBooks.Items.Add(e);
            }
            TableExchangeBooks.IsReadOnly = true;
        }
        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m= new();
            m.Show();
            this.Close();
        }

       
        
    }
}
