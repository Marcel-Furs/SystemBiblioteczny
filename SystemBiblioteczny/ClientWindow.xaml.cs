using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.IO;
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
using SystemBiblioteczny.Methods;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{
    public partial class ClientWindow : Window
    {
        Client loggedUser = new();
        private int bookFromGui = -1;
        LoginMethod loginMethod = new();
        public ClientWindow(Client user)
        {
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            loggedUser = user;
            
            AccountBase account = new AccountBase();
            List<Client> clients = account.GetClientList();
            Client client = new();
            foreach (var a in clients)
            {
                if (a.UserName == user.UserName && a.Password == user.Password)
                {
                    client.UserName = a.UserName;
                    client.Password = a.Password;
                    client.FirstName = a.FirstName;
                    client.LastName = a.LastName;
                    client.Email = a.Email; 
                    client.Phone = a.Phone;
                }
            }
            PersonStatistics(client.FirstName, client.LastName);

            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            foreach (Book e in listofBooks)
            {
                TableBooks.Items.Add(e);
            }
            Date.FontSize = 10;
            LoadEventData();
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
            if (!int.TryParse(LibraryID.Text, out libraryID))
            {
                MessageBox.Show("Numer biblioteki podaj liczbą!");
                return;
            }
            DateTime? date = Date.SelectedDate;
            int hour;
            if (!int.TryParse(EventTime.Text, out hour))
            {
                MessageBox.Show("Podaj pełną godzinę od 8 do 22 liczbą!");
                return;
            }
            String phoneNumber = ContactNumber.Text;

            AuthorsEvening newAuthorsEvening = new(false, loggedUser.UserName!, name, lastname, libraryID, date, hour, phoneNumber);
            if (newAuthorsEvening.TryAddToDataBase()) {
                MessageBox.Show("Pomyślnie wysłano propozycję wieczorka autorskiego!");
            }

            LoadEventData();
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
            TableBooks.Items.Clear();
            if (OptTitle.IsChecked == true)
            {
                var sort1 = listofBooks.OrderBy(x => x.Title).ToList();
                sort1.ForEach(x =>
                {
                    TableBooks.Items.Add(x);
                });
            }
        }

        private void Find_TextChanged(object sender, TextChangedEventArgs e)
        { }

        private void Find(object sender, RoutedEventArgs e)
        {
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            bool info = false;

            for (int i = 0; i < listofBooks.Count; i++)
            {
                if (listofBooks[i].Author == Find1.Text)
                {
                    TableBooks.Items.Clear();
                    info = true;
                    var find = listofBooks.Where(x => x.Author == Find1.Text).ToList();
                    find.ForEach(x =>
                    {
                        TableBooks.Items.Add(x);

                    });
                }
                if (listofBooks[i].Title == Find1.Text)
                {
                    TableBooks.Items.Clear();
                    info = true;
                    var find1 = listofBooks.Where(x => x.Title == Find1.Text).ToList();
                    find1.ForEach(x =>
                    {
                     TableBooks.Items.Add(x);
                    });
                }
                
            }
            if (info == false) { MessageBox.Show("Nie istnieje taki autor bądź tytuł w bazie danych"); }
        }

        private void TableBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book book = (Book)TableBooks.SelectedItem;
            if (book != null)
            {
                RequestBookLabel.Text = book.Id_Book.ToString();
            }
        }

        private void RequestBookLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(RequestBookLabel.Text, "[^0-9]"))
           {
                MessageBox.Show("Proszę wpisać numer.");
                RequestBookLabel.Text = RequestBookLabel.Text.Remove(RequestBookLabel.Text.Length - 1);
           }
        }

        private void RefreshTextBoxes()
        {
            if (RequestBookLabel.Text == "") { RequestBookLabel.Text = "0"; }
            bookFromGui = int.Parse(RequestBookLabel.Text);;
        }
        private void Book(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();
            BookReserverd bookRe = new();
            BooksReserved booksR = new();

            string czas= DateTime.Now.ToString("MM/dd/yyyy");

            //Book book = new();
            // book = (Book)TableBooks.SelectedItem;
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            List<Book> listofBorrowedBooks = books.GetBooksList();
            BookReserverd bookBorrowed = new();
            bool info = false;

            bool status1 = false;

            for (int i = 0; i < listofBooks.Count; i++)
            {
                int idSelected = listofBooks[i].Id_Book;

                if (idSelected.CompareTo(bookFromGui) == 0)
                {
                    if (listofBooks[i].Availability == true)
                    {
                        info = true;
                        int idBookFromGui = bookFromGui;
                        string path2 = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                        using (StreamWriter writer = new StreamWriter(path2))
                        {
                            for (int k = 0; k < listofBooks.Count; k++)
                            {
                                if (idBookFromGui == listofBooks[k].Id_Book) writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + "False" + " " + listofBooks[k].Id_Library);
                                else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                            }
                            writer.Close();
                        }
                        bookBorrowed.Id_Book = listofBooks[i].Id_Book;
                        bookBorrowed.Author = listofBooks[i].Author;
                        bookBorrowed.Title = listofBooks[i].Title;
                        bookBorrowed.Id_Library = listofBooks[i].Id_Library;
                        bookBorrowed.DateTime1 = czas;

                        listofBorrowedBooks.Add(bookBorrowed);
                        booksR.SaveReservedBooks(bookBorrowed);

                        status1 = bookRe.AccountBalance(bookBorrowed.DateTime1);

                        if (status1 == false) statusBook.Text = "Nie ma zaległych książek";
                        else statusBook.Text = "Trzeba zapłacić za zaległą książkę!";

                        MessageBox.Show("Zarezerwowano ksiązkę!");
                        UptodateTable();
                    }
                    else { MessageBox.Show("Ta ksiązka jest niedostępna!"); info = true; }
                }
            }
            if (info == false) { MessageBox.Show("Nie istnieje książka o podanym id"); }
            ReadFromReservedBooks();
        }

        private void ReadFromReservedBooks()
        {
            BooksReserved books = new();
            List<BookReserverd> b = books.GetReservedBooksList();
            foreach (var e in b)
            {
                TableBooks1.Items.Add(e);
            }
        }
        private void UptodateTable()
        {
            TableBooks.Items.Clear();
            Books books = new();
            List<Book> listofBooks = books.GetBooksList();
            foreach (var e in listofBooks)
            {
                TableBooks.Items.Add(e);
            }
        }

        private void LoadEventData()
        {
            AuthorsEvenings.Items.Clear();
            AuthorsEvenings events = new();
            List<AuthorsEvening> listOfEvents = events.GetEventList();
            foreach (AuthorsEvening e in listOfEvents)
            {
                if (e.User == loggedUser.UserName)
                    AuthorsEvenings.Items.Add(e);
            }
        }

        private void WithDraw(object sender, RoutedEventArgs e)
        {
            AuthorsEvening evening = new();
            evening = (AuthorsEvening)AuthorsEvenings.SelectedItem;
            AuthorsEvenings evenings = new();
            //List<AuthorsEvening> list = evenings.GetEventList();
           // if(evening != null) evenings.RemoveFromList(evening);
            LoadEventData();
        }

        private void PersonStatistics(string name, string lastname)
        {
            Name_S.Text = name;
            Lastname_S.Text = lastname;

        }

        private void AuthorsName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AuthorsLastname_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LibraryID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EventTime_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ContactNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
    }
}