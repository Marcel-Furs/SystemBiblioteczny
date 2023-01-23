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
        private ApplicationBook applicationBookModel = new();
        private BookExchange bookExchangeModel = new();
        private AccountBase accountModel = new();
        private Books bookModel = new();
        private AuthorsEvening eveningModel = new();
        private LocalAdmin localAdmin = new();
        private AuthorsEvenings eveningsModel = new();
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
            RefreshTableApplicationsData();
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
                    if (localAdmin.LibraryId.CompareTo(listofBooks[i].Id_Library) != 0) MessageBox.Show("Nie możesz wysłać książki należącej do innej biblioteki");
                    else
                    {
                        int bookId = SendBookIfAvaliable();
                        List<Book> lines = bookModel.GetBooksList();
                        string path = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                        using (StreamWriter writer = new StreamWriter(path))
                        {
                            for (int k = 0; k < lines.Count; k++)
                            {
                                int bookIdFromList = lines[k].Id_Book;
                                int newLibraryId = getRequestorLibraryId(listofBooks[i].RequestorUsername);
                                if (bookId.CompareTo(bookIdFromList) == 0) writer.WriteLine(lines[k].Id_Book + " " + lines[k].Author + " " + lines[k].Title + " " + "True" + " " + newLibraryId);
                                else writer.WriteLine(lines[k].Id_Book + " " + lines[k].Author + " " + lines[k].Title + " " + lines[k].Availability + " " + lines[k].Id_Library);
                            }
                            writer.Close();
                        }
                        MessageBox.Show("Wysłano książke");
                        RefreshTableData();
                    }
                }
            }
            if (info == false) MessageBox.Show("Nie istnieje zlecenie o podanym id");

        }
        private int getRequestorLibraryId(string requestorUsername)
        {
            int result = 0;
            List<LocalAdmin> list = accountModel.GetLocalAdminList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UserName!.CompareTo(requestorUsername) == 0) result = list[i].LibraryId;
            }
            return result;
        }
        private int SendBookIfAvaliable()
        {
            int resultBookId = 0;
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


                    if (newId.CompareTo(exchangeFromGui) == 0) resultBookId = bookId;
                    else if (echangeId > exchangeFromGui) { writer.WriteLine((echangeId - 1) + " " + bookId + " " + newRequestor + " " + newAuthor + " " + newTitle + " " + newIdLibrary); }
                    else writer.WriteLine(line);
                }

                writer.Close();
            }

            return resultBookId;
        }

        private void RequestForABook(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();
            bool info = false;
            List<Book> listofBooks = bookModel.GetBooksList();

            for (int i = 0; i < listofBooks.Count; i++)
            {
                int idBookFromList = listofBooks[i].Id_Book;

                if (idBookFromList.CompareTo(bookFromGui) == 0)
                {
                    info = true;
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
                            MessageBox.Show("Wysłano prośbę");

                        }

                    }
                }

            }
            if (info == false) MessageBox.Show("Nie istnieje zlecenie o podanym id");
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

        void RefreshTableApplicationsData()
        {
            List<ApplicationBook> listofApplicationBooks = applicationBookModel.GetApplicationBooksList();

            TableExchangeBooks.Items.Clear();

            foreach (ApplicationBook bookApplication in listofApplicationBooks)
            {
                NewApplicationsData.Items.Add(bookApplication);
            }

            NewApplicationsData.IsReadOnly = true;

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
                        List<Book> list = bookModel.GetBooksList();
                        using (StreamWriter writer = new StreamWriter(path))
                        {
                            for (int k = 0; k < list.Count; k++)
                            {

                                if (BookIdToDelete == list[k].Id_Book) writer.WriteLine(list[k].Id_Book + " " + list[k].Author + " " + list[k].Title + " " + "True" + " " + list[k].Id_Library);
                                else writer.WriteLine(list[k].Id_Book + " " + list[k].Author + " " + list[k].Title + " " + list[k].Availability + " " + list[k].Id_Library);
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
        private void TableExchangeBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookExchange book = (BookExchange)TableExchangeBooks.SelectedItem;
            if (book != null)
            {
                SendBookLabel.Text = book.ExchangeId.ToString();
                RequestBookLabel.Text = book.Id_Book.ToString();
            }
        }

        private void TableBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book book = (Book)TableBooks.SelectedItem;
            if (book != null)
            {
                SendBookLabel.Text = "0";
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
        private void LoadEventData()
        {
            AuthorsEvnings.Items.Clear();
            List<AuthorsEvening> listOfEvents = eveningsModel.GetEventList();
            foreach (AuthorsEvening e in listOfEvents)
            {
                if (localAdmin.LibraryId == e.LibraryID)
                    AuthorsEvnings.Items.Add(e);
            }
        }

        private void Approve_button(object sender, RoutedEventArgs e)
        {
            eveningModel = (AuthorsEvening)AuthorsEvnings.SelectedItem;
            // if (eveningModel != null) eveningsModel.ChangeApprovedToTrue(eveningModel);
            LoadEventData();
        }

        private void Reject_button(object sender, RoutedEventArgs e)
        {
            eveningModel = (AuthorsEvening)AuthorsEvnings.SelectedItem;
            // if (eveningModel != null) eveningsModel.RemoveFromList(eveningModel);
            LoadEventData();
        }
        private void ShowClientList(object sender, RoutedEventArgs e)
        {
            ShowClientListMethod();
        }
        private void ShowClientListMethod()
        {
            Person_Table.Items.Clear();
            List<Client> clients = accountModel.GetClientList();
            foreach (Client c in clients)
            {
                Person_Table.Items.Add(c);
            }
            Person_Table.IsReadOnly = true;
            TabelaName.Content = "Tabela pokazująca listę klientów";
        }
        private void ShowLibrarianList(object sender, RoutedEventArgs e)
        {
            ShowLibrarianListMethod();
        }
        private void ShowLibrarianListMethod()
        {
            Person_Table.Items.Clear();
            List<Librarian> librarians = accountModel.GetLibrarianList();
            foreach (Librarian l in librarians)
            {
                Person_Table.Items.Add(l);
            }
            Person_Table.IsReadOnly = true;
            TabelaName.Content = "Tabela pokazująca listę bibliotekarzy";
        }

        private void ShowAdminList(object sender, RoutedEventArgs e)
        {
            ShowAdminListMethod();
        }
        private void ShowAdminListMethod()
        {
            Person_Table.Items.Clear();
            List<LocalAdmin> admins = accountModel.GetLocalAdminList();
            foreach (LocalAdmin a in admins)
            {
                Person_Table.Items.Add(a);
            }
            Person_Table.IsReadOnly = true;
            TabelaName.Content = "Tabela pokazująca listę administratorów lokalnych";
        }
        private void MakeClientAnLibrarian(object sender, RoutedEventArgs e)
        {
            List<Client> list = accountModel.GetClientList();
            bool info = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UserName!.CompareTo(UserNameTextBox.Text) == 0)
                {
                    info = true;
                    if (IdLibraryLabel.Text == "") MessageBox.Show("Proszę wpisać poprawne id");
                    else
                    {
                        int newLibId = int.Parse(IdLibraryLabel.Text);
                        Librarian librarian = new(list[i].UserName!, list[i].Password!, list[i].FirstName!, list[i].LastName!, list[i].Email!, newLibId, list[i].Phone!);
                        accountModel.AddLibrarianToListAndDeleteFromClients(librarian);
                        MessageBox.Show("Nadano uprawnienia");
                        ShowLibrarianListMethod();
                    }

                }
            }
            if (info == false) MessageBox.Show("Nie istnieje klient o podanej nazwie");

        }
        private void MakePersonAnClient(object sender, RoutedEventArgs e)
        {
            List<Librarian> librarians = accountModel.GetLibrarianList();
            Client client = new();

            bool info = false;
            
            for (int i = 0; i < librarians.Count; i++)
            {
                if (librarians[i].UserName!.CompareTo(UserNameTextBox.Text) == 0)
                {
                    info = true;
                   
                    client = new(librarians[i].UserName!, librarians[i].Password!, librarians[i].FirstName!, librarians[i].LastName!, librarians[i].Email!, librarians[i].Phone!);
                }
            }
            if (info == true)
            {
                string path = System.IO.Path.Combine("../../../DataBases/LibrarianList.txt");

                List<string> lines = accountModel.GetListOfDataBaseLines("LibrarianList");

                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        string line = lines[i];
                        string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        string userName = splitted[0];
                        if (userName.CompareTo(UserNameTextBox.Text) == 0) { }
                        else { writer.WriteLine(line); }
                    }
                    writer.Close();
                }
                accountModel.AddClientToList(client);
                MessageBox.Show("Usunięto uprawnienia");
                ShowClientListMethod();
            }

            if (info == false) MessageBox.Show("Nie istnieje osoba o podanej nazwie");

        }

        private void IdLibraryLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(IdLibraryLabel.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                IdLibraryLabel.Text = IdLibraryLabel.Text.Remove(IdLibraryLabel.Text.Length - 1);
            }
        }
    }
}
