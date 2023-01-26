using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SystemBiblioteczny.Models;

using iTextSharp.text; 
using iTextSharp.text.pdf;
using System.Reflection.Metadata;
using Org.BouncyCastle.Asn1.X509.SigI;
using MahApps.Metro.Controls;
using System.Globalization;
using System.Windows.Documents;
using iTextSharp.text.xml;

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
        private string titleFromGui = "";
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

            NewApplicationsData.Items.Clear();
            for (int i = 0; i < listofApplicationBooks.Count; i++)
            {
                ApplicationBook book = listofApplicationBooks[i];
                if (book.Approved.CompareTo(true) == 0)
                {

                }
                else NewApplicationsData.Items.Add(book);
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

        

        private void OrderBook(object sender, RoutedEventArgs e)
        {
            ApplicationBook IdFromGui = (ApplicationBook)(NewApplicationsData.SelectedItem);
           
            if (IdFromGui == null) MessageBox.Show("Nie wybrano książki");
            else
            {
                List<ApplicationBook> list = applicationBookModel.GetApplicationBooksList();
                for (int i = 0; i < list.Count; i++)
                {

                    string path = System.IO.Path.Combine("../../../DataBases/BookApplicationList.txt");
                    List<string> lines = accountModel.GetListOfDataBaseLines("BookApplicationList");

                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        for (int j = 0; j < lines.Count; j++)
                        {
                            string line = lines[j];
                            if (list[j].ID.CompareTo(IdFromGui.ID) == 0)
                            {
                                writer.WriteLine(list[j].ID + " " + list[j].Title + " " + list[j].Author + " " + list[j].Quantity + " " + list[j].Librarian + " " + "True");
                                
                            }
                            else writer.WriteLine(line);
                        }

                        writer.Close();
                    }

                }
                RefreshTableApplicationsData();
            }


        }

        private void RejectBook(object sender, RoutedEventArgs e)
        {
            ApplicationBook IdFromGui = (ApplicationBook)(NewApplicationsData.SelectedItem);
            
            if (IdFromGui == null) MessageBox.Show("Nie wybrano książki");
            else
            {
                List<ApplicationBook> list = applicationBookModel.GetApplicationBooksList();
                for (int i = 0; i < list.Count; i++)
                {

                    string path = System.IO.Path.Combine("../../../DataBases/BookApplicationList.txt");
                    List<string> lines = accountModel.GetListOfDataBaseLines("BookApplicationList");

                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        for (int j = 0; j < lines.Count; j++)
                        {
                            string line = lines[j];
                            if (list[j].ID.CompareTo(IdFromGui.ID) != 0) writer.WriteLine(line);
                        }

                        writer.Close();
                    }

                }
                RefreshTableApplicationsData();
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
            if (eveningModel != null) eveningsModel.ChangeApprovedToTrue(eveningModel);
            LoadEventData();
        }

        private void Reject_button(object sender, RoutedEventArgs e)
        {
            eveningModel = (AuthorsEvening)AuthorsEvnings.SelectedItem;
            if (eveningModel != null) eveningsModel.RemoveFromList(eveningModel);
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



        







        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? chosenStartDate = startDatePicker.SelectedDate;
            DateTime? chosenEndDate = endDatePicker.SelectedDate;
            int activeCustomers = 0;
            using (StreamReader reader = new StreamReader("../../../DataBases/BookHistory.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    DateTime startDate = DateTime.ParseExact(parts[5], "MM.dd.yyyy", CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(parts[6], "MM.dd.yyyy", CultureInfo.InvariantCulture);
                    if (chosenStartDate != null && chosenEndDate != null || (startDate >= chosenStartDate && endDate <= chosenEndDate))
                    {
                        activeCustomers++;
                    }
                }
            
            }


            // ilosc ksiazek w danej bibliotece 
            Books a = new();
            List<Book> AllBooksList = a.GetBooksList();
            int BooksCount = 0;
            for (int i=0; i < AllBooksList.Count; i++)
            {
                if (AllBooksList[i].Id_Library == localAdmin.LibraryId)
                {
                    BooksCount++;
                }
            }

            // ilosc wypozyczen w danej bibliotece
            AccountBase history = new();
            BooksReserved b = new();
            List<BookReserved> AllBooksReservedList = b.GetReservedBooksList();
            List<string> listHistory = history.GetListOfDataBaseLines("BookHistory");
            int ReservedBooksCount = 0;
            for(int i=0; i < AllBooksReservedList.Count; i++)
            {
                if (AllBooksReservedList[i].Availability == true)
                {
                    ReservedBooksCount++;
                }
            }
            for (int j = 0; j < listHistory.Count; j++)
            {
                ReservedBooksCount++;
            }


            // ilosc zarejestrowanych klientow
            AccountBase c = new();
            int AllClients = c.GetClientList().Count;

            // ilosc aktywnych klientow w danej bibliotece
            List<string> list = c.GetListOfDataBaseLines("BookHistory");
            List<string> userList = new();
            userList.Add("");
            int AllActiveClients = 0;
            for(int i = 0; i < list.Count; i++)
            {
                string line = list[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                int libId = int.Parse(splitted[1]);
                string user = splitted[2];
                if (libId==localAdmin.LibraryId)
                {
                    for(int j = 0; j < userList.Count; j++)
                    {
                        if (userList[j].CompareTo(user) != 0)
                        {
                            AllActiveClients++;
                            userList.Add(user);
                        }
                    }
                }

            }


            // ilosc bibliotekarzy w danej bibliotece
            List<Librarian> AllLibrarians= c.GetLibrarianList();
            int allLibrans = 0;
            for(int i=0;i<AllLibrarians.Count;i++)
            {
                Librarian librarian = AllLibrarians[i];
                if (librarian.LibraryId == localAdmin.LibraryId)
                {
                    allLibrans++;
                }
            }


            // ilosc wieczorkow autorskich w danej bibliotece
            AuthorsEvenings evenings = new();
            List<AuthorsEvening> AuthorEvenings = evenings.GetEventList();
            int AllAuthorEvenings = 0;
            for(int i = 0; i < AuthorEvenings.Count; i++)
            {
                AuthorsEvening f = AuthorEvenings[i];
                if (f.LibraryID == localAdmin.LibraryId)
                {
                    AllAuthorEvenings++;
                }
                
            }

            



            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string fileName = "raport-id-" + localAdmin.LibraryId + "-" + localAdmin.UserName + "-" + currentDateTime + ".pdf";
            string path = @"../../../Raporty/" + fileName;

            if(!Directory.Exists("../../../Raporty/"))
            {
                Directory.CreateDirectory("../../../Raporty/");
            }

            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfWriter.GetInstance(doc, new FileStream(path,FileMode.Create));

                doc.Open();

            string textFile = "ListofBookstxt.png";
            string textFilePath = Path.Combine(@"../../../DataBases/", textFile);
            if (activeCustomers != 0)
            {
                PdfPTable tablee = new PdfPTable(1);
                tablee.WidthPercentage = 100;
                tablee.AddCell(new PdfPCell(new Phrase("Liczba aktywnych klientów w zakresie: " + chosenStartDate + " - " + chosenEndDate + ": ")));
                tablee.AddCell(new iTextSharp.text.Paragraph(activeCustomers.ToString()));
                doc.Add(tablee);
                iTextSharp.text.Image tXt = iTextSharp.text.Image.GetInstance(textFilePath);
                tXt.SetAbsolutePosition(0, 0);
                tXt.ScaleToFit(13, 13);
                doc.Add(tXt);
                doc.Close();
            }
            else
            {
                PdfPTable table = new PdfPTable(6);
                table.AddCell(new PdfPCell(new Phrase("Liczba ksiazek")));
                table.AddCell(new PdfPCell(new Phrase("Liczba wyporzyczonych ksiazek")));
                table.AddCell(new PdfPCell(new Phrase("Liczba klientów")));
                table.AddCell(new PdfPCell(new Phrase("Liczba aktywnych klientów")));
                table.AddCell(new PdfPCell(new Phrase("Liczba bibliotekarzy")));
                table.AddCell(new PdfPCell(new Phrase("Liczba wieczorków autorskich")));

                table.AddCell(new iTextSharp.text.Paragraph(BooksCount.ToString()));
                table.AddCell(new iTextSharp.text.Paragraph(ReservedBooksCount.ToString()));
                table.AddCell(new iTextSharp.text.Paragraph(AllClients.ToString()));
                table.AddCell(new iTextSharp.text.Paragraph(AllActiveClients.ToString()));
                table.AddCell(new iTextSharp.text.Paragraph(allLibrans.ToString()));
                table.AddCell(new iTextSharp.text.Paragraph(AllAuthorEvenings.ToString()));
                doc.Add(table);
                iTextSharp.text.Image tXt = iTextSharp.text.Image.GetInstance(textFilePath);
                tXt.SetAbsolutePosition(0, 0);
                tXt.ScaleToFit(13, 13);
                doc.Add(tXt);
                doc.Close();
            }
            MessageBox.Show("Utworzono raport.");
        }
    }
}
