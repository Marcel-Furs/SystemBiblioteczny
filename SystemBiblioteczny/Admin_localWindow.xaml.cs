using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{
    /// <summary>
    /// Logika interakcji dla klasy Admin_localWindow.xaml
    /// </summary>
    public partial class Admin_LocalWindow : Window
    {
        private BookExchange bookExchangeModel = new();
        private AccountBase accountModel = new();
        private Books bookModel = new();
        private LocalAdmin localAdmin = new();
        private int bookFromGui = -1;
        private int exchangeFromGui = -1;
        public Admin_LocalWindow(LocalAdmin userData)
        {
            InitializeComponent();

            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            localAdmin = userData;
            nazwaLabel.Content = localAdmin.UserName;
            numerLabel.Content = localAdmin.LibraryId;
            LoadEventData();
            RefreshTableData();
        }
        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m = new();
            m.Show();
            this.Close();
        }

        private void SendBook_Click(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();

            List<BookExchange> listofBooks = bookExchangeModel.GetExchangeBooksList();
            bool info = false;

            for (int i = 0; i < listofBooks.Count; i++)
            {
                int idExchange = listofBooks[i].ExchangeId;

                if (idExchange.CompareTo(exchangeFromGui) == 0)
                {
                    info = true;
                    if (localAdmin.LibraryId.CompareTo(listofBooks[i].Id_Library) == 0) MessageBox.Show("Nie możesz wysłać książki do własnej biblioteki");
                    else
                    {
                        if (listofBooks[i].Availability == true) SendBookIfAvaliable();
                        else MessageBox.Show("Książka aktualnie nie jest dostępna");
                    }
                }
            }
            if (info == false) MessageBox.Show("Nie istnieje zlecenie o podanym id");

        }
        private void SendBookIfAvaliable()
        {
            string path = System.IO.Path.Combine("../../../DataBases/ExchangeBookList.txt");
            List<string> lines = accountModel.GetListOfDataBaseLines("ExchangeBookList");

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


                    if (newId.CompareTo(exchangeFromGui) == 0) { }
                    else if (echangeId > exchangeFromGui) { writer.WriteLine((echangeId - 1) + " " + bookId + " " + newRequestor + " " + newAuthor + " " + newTitle + " " + newIdLibrary); }
                    else writer.WriteLine(line);
                }

                writer.Close();
            }

            MessageBox.Show("Wysłano książke");

            RefreshTableData();

        }

        private void RequestForABook(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();

            List<Book> listofBooks = bookModel.GetBooksList();

            for (int i = 0; i < listofBooks.Count; i++)
            {
                int idBookFromList = listofBooks[i].Id_Book;

                if (idBookFromList.CompareTo(bookFromGui) == 0)
                {

                    if (localAdmin.LibraryId.CompareTo(listofBooks[i].Id_Library) == 0) MessageBox.Show("Możesz wysyłać prośby o książki tylko z innych bibliotek");
                    else
                    {
                        if (listofBooks[i].Availability == false) MessageBox.Show("Możesz wysyłać prośby dotyczące tylko dostępnych książek");
                        else
                        {
                            List<string> lines = accountModel.GetListOfDataBaseLines("ExchangeBookList");

                            int echangeId = lines.Count + 1;
                            int bnewBookId = listofBooks[i].Id_Book;
                            string newRequestor = localAdmin.UserName!;
                            string newAuthor = listofBooks[i].Author;
                            string newTitle = listofBooks[i].Title;
                            int newIdLibrary = listofBooks[i].Id_Library;

                            accountModel.WriteToDataBase("ExchangeBookList", echangeId + " " + bnewBookId + " " + newRequestor + " " + newAuthor + " " + newTitle + " " + newIdLibrary);
                            SendBookRequestIfAvaliable();

                        }

                    }
                }

            }
        }
        private void SendBookRequestIfAvaliable()
        {

            List<Book> listofBooks = bookModel.GetBooksList();
            string path2 = System.IO.Path.Combine("../../../DataBases/BookList.txt");
            using (StreamWriter writer = new StreamWriter(path2))
            {
                for (int k = 0; k < listofBooks.Count; k++)
                {

                    if (bookFromGui == listofBooks[k].Id_Book) writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + "False" + " " + listofBooks[k].Id_Library);
                    else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                }
                writer.Close();
            }

            MessageBox.Show("Wysłano prośbę");

            RefreshTableData();
        }
        void RefreshTableData()
        {
            List<BookExchange> listofExchangeBooks = bookExchangeModel.GetExchangeBooksList();
            List<Book> listofBooks = bookModel.GetBooksList();

            TableExchangeBooks.Items.Clear();
            TableBooks.Items.Clear();

            foreach (BookExchange bookExchange in listofExchangeBooks)
            {
                TableExchangeBooks.Items.Add(bookExchange);
            }

            foreach (Book book in listofBooks)
            {
                TableBooks.Items.Add(book);
            }

            TableExchangeBooks.IsReadOnly = true;
            TableBooks.IsReadOnly = true;
        }

        private void CancelRequestButton(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();

            List<BookExchange> listofBooks = bookExchangeModel.GetExchangeBooksList();
            bool info = false;

            for (int i = 0; i < listofBooks.Count; i++)
            {
                int idExchange = listofBooks[i].ExchangeId;

                if (idExchange.CompareTo(exchangeFromGui) == 0)
                {
                    info = true;
                    if (localAdmin.LibraryId.CompareTo(listofBooks[i].Id_Library) != 0) MessageBox.Show("Możesz odrzucać prośby wysłane tylko do swojej biblioteki");
                    else
                    {
                        int BookIdToDelete = -1;
                        string path = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                        BookIdToDelete = RejectBookIfAvaliable(BookIdToDelete);

                        using (StreamWriter writer = new StreamWriter(path))
                        {
                            for (int k = 0; k < listofBooks.Count; k++)
                            {

                                if (BookIdToDelete == listofBooks[k].Id_Book) writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + "True" + " " + listofBooks[k].Id_Library);
                                else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                            }
                            writer.Close();
                        }
                        MessageBox.Show("Odrzucono prośbę");
                        RefreshTableData();
                    }
                }
            }
            if (info == false) { MessageBox.Show("Nie istnieje zlecenie o podanym id"); }
        }
        private int RejectBookIfAvaliable(int BookIdToDelete)
        {

            string path = System.IO.Path.Combine("../../../DataBases/ExchangeBookList.txt");
            List<string> lines = accountModel.GetListOfDataBaseLines("ExchangeBookList");

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


                    if (newId.CompareTo(exchangeFromGui) == 0) { BookIdToDelete = bookId; }
                    else if (echangeId > exchangeFromGui) { writer.WriteLine((echangeId - 1) + " " + bookId + " " + newRequestor + " " + newAuthor + " " + newTitle + " " + newIdLibrary); }
                    else writer.WriteLine(line);
                }

                writer.Close();
            }
            return BookIdToDelete;
        }

        private void RequestBookLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(RequestBookLabel.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                RequestBookLabel.Text = RequestBookLabel.Text.Remove(RequestBookLabel.Text.Length - 1);
            }
        }

        private void SendBookLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(SendBookLabel.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                SendBookLabel.Text = SendBookLabel.Text.Remove(SendBookLabel.Text.Length - 1);
            }
        }

        private void RefreshTextBoxes()
        {
            if (SendBookLabel.Text == "") { SendBookLabel.Text = "0"; }
            if (RequestBookLabel.Text == "") { RequestBookLabel.Text = "0"; }
            bookFromGui = int.Parse(RequestBookLabel.Text);
            exchangeFromGui = int.Parse(SendBookLabel.Text);
        }

        public List<string> GetListOfDataBaseLines(string fileName)
        {

            string path = System.IO.Path.Combine("../../../DataBases/" + fileName + ".txt");
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
            return lines;
        }

        public void WriteToDataBase(string fileName, string newLine)
        {

            List<string> lines = GetListOfDataBaseLines(fileName);
            string path = System.IO.Path.Combine("../../../DataBases/" + fileName + ".txt");

            using (StreamWriter writer = new StreamWriter(path))
            {

                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
                writer.WriteLine(newLine);
                writer.Close();
            }

        }
        private void LoadEventData()
        {
            AuthorsEvnings.Items.Clear();
            AuthorsEvenings events = new();
            List<AuthorsEvening> listOfEvents = events.GetEventList();
            foreach (AuthorsEvening e in listOfEvents)
            {
                if (localAdmin.LibraryId == e.LibraryID)
                    AuthorsEvnings.Items.Add(e);
            }
        }

        private void Approve_button(object sender, RoutedEventArgs e)
        {
            AuthorsEvening evening = new();
            evening = (AuthorsEvening)AuthorsEvnings.SelectedItem;
            AuthorsEvenings evenings = new();
            if (evening != null) evenings.ChangeApprovedToTrue(evening);
            LoadEventData();
        }

        private void Reject_button(object sender, RoutedEventArgs e)
        {
            AuthorsEvening evening = new();
            evening = (AuthorsEvening)AuthorsEvnings.SelectedItem;
            AuthorsEvenings evenings = new();
            if (evening != null) evenings.RemoveFromList(evening);
            LoadEventData();
        }
    }
}
