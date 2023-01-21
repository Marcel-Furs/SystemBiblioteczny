using Accessibility;
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
    /// Logika interakcji dla klasy Admin_localWindow.xaml
    /// </summary>
    public partial class Admin_LocalWindow : Window
    {

        LocalAdmin userDataFinal = new();


        public Admin_LocalWindow(LocalAdmin userData)
        {
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            userDataFinal = userData;

            nazwaLabel.Content = userData.UserName;
            numerLabel.Content = userData.LibraryId;

            RefreshTableData();
        }
        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m= new();
            m.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SendBookLabel.Text == "") { SendBookLabel.Text = "-1"; }
            BookExchange books = new();
            List<BookExchange> listofBooks = books.GetExchangeBooksList();
            bool info = false;
            for(int i = 0; i < listofBooks.Count; i++)
            {
                int idExchange = listofBooks[i].ExchangeId;
                int SendBookLabelInt = int.Parse(SendBookLabel.Text);
                if (idExchange.CompareTo(SendBookLabelInt) == 0) {
                    info = true;
                    if ((numerLabel.Content).ToString()!.CompareTo(listofBooks[i].Id_Library.ToString()) == 0) {
                        MessageBox.Show("Możesz wysyłać książki tylko do innych bibliotek");
                    }
                    else { 
                    bool isAvailable = listofBooks[i].Availability;
                        if (isAvailable == true) {
                            
                            string path = System.IO.Path.Combine("../../../DataBases/ExchangeBookList.txt");
                            List<string> lines = new();
                            using (StreamReader reader = new(path))
                            {
                                var line = reader.ReadLine();

                                while (line != null)
                                {
                                    lines.Add(line);
                                    line = reader.ReadLine();

                                }
                                reader.Close();

                            }
                            using (StreamWriter writer = new StreamWriter(path))
                            {
                                for (int j = 0; j < lines.Count; j++) {
                                   
                                    string line = lines[j];
                                    string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                                    int newId = int.Parse(splitted[0]);

                                    int echangeId = int.Parse(splitted[0]);
                                    int bookId = int.Parse(splitted[1]);
                                    string newRequestor = splitted[2];
                                   
                                    string newAuthor = splitted[3];
                                    string newTitle = splitted[4];
                                    
                                    int newIdLibrary = int.Parse(splitted[5]);


                                    if (newId.CompareTo(SendBookLabelInt) == 0) {}
                                    else if (echangeId > SendBookLabelInt) { writer.WriteLine((echangeId - 1)+" "+ bookId  + " " + newRequestor  + " " + newAuthor + " " + newTitle  + " " + newIdLibrary); }
                                    else writer.WriteLine(line);
                                }
                              
                                writer.Close();
                            }

                            MessageBox.Show("Wysłano książke");

                            RefreshTableData();


                        } 
                    else MessageBox.Show("Książka aktualnie nie jest dostępna");
                    }
                } 
            }
            if (info == false) { MessageBox.Show("Nie istnieje zlecenie o podanym id"); }

        }

        private void RequestForABook(object sender, RoutedEventArgs e)
        {
            string bookId = RequestBookLabel.Text;
            if (bookId == "") MessageBox.Show("Proszę podać wartość");
            else {
                if (SendBookLabel.Text == "") { SendBookLabel.Text = "-1"; }
                Books books = new();
                List<Book> listofBooks = books.GetBooksList();
               
                for (int i = 0; i < listofBooks.Count; i++)
                {
                    int idBookFromList = listofBooks[i].Id_Book;
                    int idBookFromGui = int.Parse(bookId);
                    if (idBookFromList.CompareTo(idBookFromGui) == 0)
                    {
                        
                        if ((numerLabel.Content).ToString()!.CompareTo(listofBooks[i].Id_Library.ToString()) == 0)
                        {
                            MessageBox.Show("Możesz wysyłać prośby tylko do innych bibliotek");
                        }
                        else
                        {
                            if (listofBooks[i].Availability == false) { MessageBox.Show("Możesz wysyłać prośby dotyczące tylko dostępnych książek"); }
                            else {
                                string path = System.IO.Path.Combine("../../../DataBases/ExchangeBookList.txt");
                                string path2 = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                                List<string> lines = new();
                                using (StreamReader reader = new(path))
                                {
                                    var line = reader.ReadLine();

                                    while (line != null)
                                    {
                                        lines.Add(line);
                                        line = reader.ReadLine();

                                    }
                                    reader.Close();

                                }
                                using (StreamWriter writer = new StreamWriter(path))
                                {
                                    foreach (string line in lines)
                                    {
                                        writer.WriteLine(line);
                                    }

                                    int echangeId = lines.Count + 1;
                                    int bnewBookId = listofBooks[i].Id_Book;
                                    string newRequestor = nazwaLabel.Content.ToString()!;

                                    string newAuthor = listofBooks[i].Author;
                                    string newTitle = listofBooks[i].Title;

                                    int newIdLibrary = listofBooks[i].Id_Library;

                                    writer.WriteLine(echangeId + " " + bnewBookId + " " + newRequestor + " " + newAuthor + " " + newTitle + " " + newIdLibrary);
                                    writer.Close();
                                }

                                
                                using (StreamWriter writer = new StreamWriter(path2))
                                {
                                   
  
                                    for (int k = 0; k < listofBooks.Count; k++) {
                                      
                                        if (idBookFromGui == listofBooks[k].Id_Book) writer.WriteLine(listofBooks[k].Id_Book+" "+ listofBooks[k].Author+" "+ listofBooks[k].Title+" "+ "False" + " "+ listofBooks[k].Id_Library);
                                        else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                                    }
                                    writer.Close();

                                    
                                   
                                }

                                MessageBox.Show("Wysłano prośbę");

                                RefreshTableData();

                            }


                        }
                    }
                }
               
            }
        }
        void RefreshTableData() {
            BookExchange books3 = new();
            List<BookExchange> listofBooks3 = books3.GetExchangeBooksList();
            TableExchangeBooks.Items.Clear();
            foreach (BookExchange e3 in listofBooks3)
            {
                TableExchangeBooks.Items.Add(e3);
            }
            TableExchangeBooks.IsReadOnly = true;
            Books books5 = new();
            List<Book> listofBooks5 = books5.GetBooksList();
            TableBooks.Items.Clear();
            foreach (Book e5 in listofBooks5)
            {
                TableBooks.Items.Add(e5);
            }
            TableBooks.IsReadOnly = true;
        }

        private void CancelRequestButton(object sender, RoutedEventArgs e)
        {
            if (SendBookLabel.Text == "") { SendBookLabel.Text = "-1"; }
            BookExchange books = new();
            List<BookExchange> listofBooks = books.GetExchangeBooksList();
            bool info = false;
            for (int i = 0; i < listofBooks.Count; i++)
            {
                int idExchange = listofBooks[i].ExchangeId;
                int SendBookLabelInt = int.Parse(SendBookLabel.Text);
                if (idExchange.CompareTo(SendBookLabelInt) == 0)
                {
                    info = true;
                    if ((numerLabel.Content).ToString()!.CompareTo(listofBooks[i].Id_Library.ToString()) != 0)
                    {
                        MessageBox.Show("Możesz odrzucać prośby tylko do swojej biblioteki");
                    }
                    else
                    {
                       
                            
                            string path = System.IO.Path.Combine("../../../DataBases/ExchangeBookList.txt");
                            string path2 = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                            List<string> lines = new();
                            using (StreamReader reader = new(path))
                            {
                                var line = reader.ReadLine();

                                while (line != null)
                                {
                                    lines.Add(line);
                                    line = reader.ReadLine();

                                }
                                reader.Close();

                            }
                        int BookIdToDelete = -1;
                            using (StreamWriter writer = new StreamWriter(path))
                            {
                                for (int j = 0; j < lines.Count; j++)
                                {

                                    string line = lines[j];
                                    string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                                    int newId = int.Parse(splitted[0]);

                                    int echangeId = int.Parse(splitted[0]);
                                    int bookId = int.Parse(splitted[1]);
                                    string newRequestor = splitted[2];

                                    string newAuthor = splitted[3];
                                    string newTitle = splitted[4];

                                    int newIdLibrary = int.Parse(splitted[5]);


                                    if (newId.CompareTo(SendBookLabelInt) == 0) { BookIdToDelete = bookId; }
                                    else if (echangeId > SendBookLabelInt) { writer.WriteLine((echangeId - 1) + " " + bookId + " " + newRequestor + " " + newAuthor + " " + newTitle + " " + newIdLibrary); }
                                    else writer.WriteLine(line);
                                }

                                writer.Close();
                            }

                            MessageBox.Show("Odrzucono prośbę");
                            using (StreamWriter writer = new StreamWriter(path2))
                            {
                                
                                for (int k = 0; k < listofBooks.Count; k++)
                                {

                                    if (BookIdToDelete == listofBooks[k].Id_Book) writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + "True" + " " + listofBooks[k].Id_Library);
                                    else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                                }
                                writer.Close();



                            }

                            RefreshTableData();


                        
                    }
                }
            }
            if (info == false) { MessageBox.Show("Nie istnieje zlecenie o podanym id"); }
        }
    }
}
