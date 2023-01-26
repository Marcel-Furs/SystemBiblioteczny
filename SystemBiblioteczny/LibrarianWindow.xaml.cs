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
using SystemBiblioteczny.Methods;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{
    public partial class LibrarianWindow : Window
    {
        LoginMethod loginMethod = new();
        private ApplicationBook applicationBookModel = new();
        private AccountBase accountModel = new();
        private Librarian librarianModel = new();
        private Books books = new();

        private string titleFromGui = "";
        private int bookRFromGui = -1;
        public LibrarianWindow(Librarian userData)
        {
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            librarianModel = userData;
            RefreshTableApprovedApplicationsData();
            LoadEventData();
            UptodateTable();

        }

        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m = new();
            m.Show();
            this.Close();
        }

        private void SendApplication(object sender, RoutedEventArgs e)
        {
            if (QuantityInput.Text == "" || QuantityInput.Text.CompareTo("0") == 0)
            {
                MessageBox.Show("Ilość nie może być zerem");
                return;
            }
            int max = 1;
            var title = TitleInput.Text;
            var author = AuthorInput.Text;
            int quantity = int.Parse(QuantityInput.Text);
            var librarian = librarianModel.UserName!;
            foreach (ApplicationBook a in applicationBookModel.GetApplicationBooksList())
            {
                if (a.ID > max) max = a.ID;
            }
            max++;
            if (title.Any() && author.Any())
            {
                MessageBox.Show("Wysłano zgłoszenie zapotrzebowania na: " + "\n" + author + " " + title + " - ilość: " + quantity);


                List<string> lines = accountModel.GetListOfDataBaseLines("BookApplicationList");
                accountModel.WriteToDataBase("BookApplicationList", max + " " + TitleInput.Text + " " + AuthorInput.Text + " " + QuantityInput.Text + " " + librarian + " " + false);
            }
            else
            {
                MessageBox.Show("Niewystarczająca ilość danych, uzupełnij wszystkie dane!");
            }
        }
        private void LoadEventData()
        {
            AuthorsEvenings eveningsModel = new();
            AuthorsEvnings.Items.Clear();
            List<AuthorsEvening> listOfEvents = eveningsModel.GetEventList();
            foreach (AuthorsEvening e in listOfEvents)
            {
                if (librarianModel.LibraryId == e.LibraryID && e.Approved == true)
                    AuthorsEvnings.Items.Add(e);
            }
        }

        void RefreshTableApprovedApplicationsData()
        {
            List<ApplicationBook> listofApplicationBooks = applicationBookModel.GetApplicationBooksList();

            ApprovedApplicationsTable.Items.Clear();
            for (int i = 0; i < listofApplicationBooks.Count; i++)
            {
                ApplicationBook book = listofApplicationBooks[i];
                if (book.Approved.CompareTo(false) != 0) ApprovedApplicationsTable.Items.Add(book);
            }
            ApprovedApplicationsTable.IsReadOnly = true;

        }

        private void CollectBook(object sender, RoutedEventArgs e)
        {


            ApplicationBook IdFromGui = (ApplicationBook)(ApprovedApplicationsTable.SelectedItem);
            if (IdFromGui == null) MessageBox.Show("Nie wybrano książki");
            else
            {
                List<Book> book2 = new();
                book2 = books.GetBooksList();
                int newID = book2.Capacity;
                newID++;
                List<ApplicationBook> list = applicationBookModel.GetApplicationBooksList();


                string path = System.IO.Path.Combine("../../../DataBases/BookApplicationList.txt");
                List<string> lines = accountModel.GetListOfDataBaseLines("BookApplicationList");

                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int j = 0; j < lines.Count; j++)
                    {
                        string line = lines[j];
                        if (list[j].ID.CompareTo(IdFromGui.ID) == 0)
                        {
                            for (int i = 0; i < list[j].Quantity; i++)
                            {
                                accountModel.WriteToDataBase("BookList", newID + " " + list[j].Author + " " + list[j].Title + " " + "True" + " " + librarianModel.LibraryId);
                                newID++;
                            }

                        }
                        else writer.WriteLine(line);
                    }

                    writer.Close();
                }


                RefreshTableApprovedApplicationsData();
            }
        }
        private void RefreshTextBoxes()
        {
            if (RequestBookRLabel.Text == "") { RequestBookRLabel.Text = "0"; }
            bookRFromGui = int.Parse(RequestBookRLabel.Text); ;

            if (CancelBookRLabel.Text == "") { CancelBookRLabel.Text = "0"; }
            bookRFromGui = int.Parse(CancelBookRLabel.Text); ;

            if (ReturnBookLabel.Text == "") { ReturnBookLabel.Text = "0"; }
            bookRFromGui = int.Parse(ReturnBookLabel.Text); ;
        }
        private void RequestBookRLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(RequestBookRLabel.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                RequestBookRLabel.Text = RequestBookRLabel.Text.Remove(RequestBookRLabel.Text.Length - 1);
            }
        }

        private void CancelBookRLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(CancelBookRLabel.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                CancelBookRLabel.Text = CancelBookRLabel.Text.Remove(CancelBookRLabel.Text.Length - 1);
            }
        }

        private void ReturnBookLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ReturnBookLabel.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                ReturnBookLabel.Text = ReturnBookLabel.Text.Remove(ReturnBookLabel.Text.Length - 1);
            }
        }

        private void TableRentBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book book = (Book)TableRentBook.SelectedItem;
            if (book != null)
            {
                RequestBookRLabel.Text = book.Id_Book.ToString();
                CancelBookRLabel.Text = book.Id_Book.ToString();
                ReturnBookLabel.Text = book.Id_Book.ToString();
            }

            Book book1 = (Book)TableReturnBook.SelectedItem;
            if (book1 != null)
            {
                ReturnBookLabel.Text = book1.Id_Book.ToString();
            }

        }
        private void UptodateTable()
        {
            TableRentBook.Items.Clear();
            BooksReserved books = new();
            List<BookReserved> listofBooks = books.GetReservedBooksList();
            var listofBooksR = listofBooks.Where(x => x.Id_Library == librarianModel.LibraryId && x.Availability == false).ToList();
            var listofBooksSort = listofBooksR.OrderBy(x => x.Id_Book).ToList();
            listofBooksSort.ForEach(x =>
            {
                TableRentBook.Items.Add(x);
            });

            TableReturnBook.Items.Clear();
            var listofBooksR1 = listofBooks.Where(x => x.Id_Library == librarianModel.LibraryId && x.Availability == true).ToList();
            var listofBooksSort1 = listofBooksR1.OrderBy(x => x.Id_Book).ToList();
            listofBooksSort1.ForEach(x =>
            {
                TableReturnBook.Items.Add(x);
            });
        }
        private void Rent_Book(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();
            BooksReserved books = new();

            List<BookReserved> listofBorrowedBooks = books.GetReservedBooksList();

            bool info = false;

            for (int i = 0; i < listofBorrowedBooks.Count; i++)
            {
                int idSelected = listofBorrowedBooks[i].Id_Book;

                if (idSelected.CompareTo(bookRFromGui) == 0)
                {
                    if (listofBorrowedBooks[i].Availability == false)
                    {
                        info = true;
                        int idBookFromGui = bookRFromGui;
                        string path2 = System.IO.Path.Combine("../../../DataBases/ReservedBooks.txt");
                        using (StreamWriter writer = new StreamWriter(path2))
                        {
                            for (int k = 0; k < listofBorrowedBooks.Count; k++)
                            {
                                if (idBookFromGui == listofBorrowedBooks[k].Id_Book) writer.WriteLine(listofBorrowedBooks[k].Id_Book + " " + listofBorrowedBooks[k].Author + " " + listofBorrowedBooks[k].Title + " " + "True" + " " + listofBorrowedBooks[k].Id_Library + " " + listofBorrowedBooks[k].DateTime1 + " " + listofBorrowedBooks[k].UserName);
                                else writer.WriteLine(listofBorrowedBooks[k].Id_Book + " " + listofBorrowedBooks[k].Author + " " + listofBorrowedBooks[k].Title + " " + listofBorrowedBooks[k].Availability + " " + listofBorrowedBooks[k].Id_Library + " " + listofBorrowedBooks[k].DateTime1 + " " + listofBorrowedBooks[k].UserName);
                            }
                            writer.Close();
                        };
                        MessageBox.Show("Wypożyczono ksiązkę!");
                        UptodateTable();
                    }
                    else { MessageBox.Show("Ta ksiązka jest niedostępna!"); info = true; }
                }
            }
            if (info == false) { MessageBox.Show("Nie istnieje książka o podanym id"); }
        }

        private void Cancel_Reservation(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();

            //string czas = DateTime.Now.ToString("MM/dd/yyyy");
            BooksReserved booksR = new();
            Books books = new();

            List<BookReserved> listofBorrowedBooks = booksR.GetReservedBooksList();
            List<Book> listofBooks = books.GetBooksList();

            bool info = false;

            for (int i = 0; i < listofBorrowedBooks.Count; i++)
            {
                int idSelected = listofBorrowedBooks[i].Id_Book;

                if (idSelected.CompareTo(bookRFromGui) == 0)
                {
                    if (listofBorrowedBooks[i].Availability == false)
                    {
                        info = true;
                        int idBookFromGui = bookRFromGui;
                        string path2 = System.IO.Path.Combine("../../../DataBases/ReservedBooks.txt");
                        using (StreamWriter writer = new StreamWriter(path2))
                        {
                            for (int k = 0; k < listofBorrowedBooks.Count; k++)
                            {
                                if (idBookFromGui != listofBorrowedBooks[k].Id_Book) writer.WriteLine(listofBorrowedBooks[k].Id_Book + " " + listofBorrowedBooks[k].Author + " " + listofBorrowedBooks[k].Title + " " + listofBorrowedBooks[k].Availability + " " + listofBorrowedBooks[k].Id_Library + " " + listofBorrowedBooks[k].DateTime1 + " " + listofBorrowedBooks[k].UserName);
                            }
                            writer.Close();
                        };
                        MessageBox.Show("Anulowano ksiązkę!");
                        UptodateTable();

                        string path3 = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                        using (StreamWriter writer = new StreamWriter(path3))
                        {
                            for (int k = 0; k < listofBooks.Count; k++)
                            {
                                if (idBookFromGui == listofBooks[k].Id_Book) writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + "True" + " " + listofBooks[k].Id_Library);
                                else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                            }
                            writer.Close();
                        };
                    }
                    else { MessageBox.Show("Ta ksiązka jest niedostępna!"); info = true; }
                }
            }
            if (info == false) { MessageBox.Show("Nie istnieje książka o podanym id"); }
        }

        private void Cancel_All(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Czy chcesz rozpocząć usuwanie książek?", "", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                RefreshTextBoxes();

                BooksReserved booksR = new();
                Books books = new();

                List<BookReserved> listofBorrowedBooks = booksR.GetReservedBooksList();
                List<Book> listofBooks = books.GetBooksList();

                List<int> takeId = new();
                foreach (var g in listofBorrowedBooks)
                {
                    if (g.Id_Library == librarianModel.LibraryId && g.Availability == false)
                    {
                        takeId.Add(g.Id_Book);
                    }
                }

                string path2 = System.IO.Path.Combine("../../../DataBases/ReservedBooks.txt");
                using (StreamWriter writer = new StreamWriter(path2))
                {
                    for (int i = 0; i < listofBorrowedBooks.Count; i++)
                    {
                        if (listofBorrowedBooks[i].Availability == false && listofBorrowedBooks[i].Id_Library == librarianModel.LibraryId) ;
                        else writer.WriteLine(listofBorrowedBooks[i].Id_Book + " " + listofBorrowedBooks[i].Author + " " + listofBorrowedBooks[i].Title + " " + listofBorrowedBooks[i].Availability + " " + listofBorrowedBooks[i].Id_Library + " " + listofBorrowedBooks[i].DateTime1 + " " + listofBorrowedBooks[i].UserName);

                    }
                    writer.Close();
                };
                MessageBox.Show("Anulowano wszystkie książki!");
                UptodateTable();

                string path3 = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                using (StreamWriter writer = new StreamWriter(path3))
                {
                    for (int k = 0; k < listofBooks.Count; k++)
                    {
                        bool tmp = false;
                        for (int jola = 0; jola < takeId.Count; jola++)
                        {

                            if (takeId[jola] == listofBooks[k].Id_Book && listofBorrowedBooks[jola].Id_Library == librarianModel.LibraryId)
                            {
                                tmp = true;
                                break;
                            }
                        }
                        if (tmp == true)
                        {
                            writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + tmp + " " + listofBooks[k].Id_Library);
                        }
                        else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                    }
                    writer.Close();
                };

            }
        }
        private void Return_Book(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();

            string ReturnDate = DateTime.Now.ToString("MM/dd/yyyy");

            BooksReserved booksR = new();
            Books books = new();
            List<BookReserved> listofBorrowedBooks = booksR.GetReservedBooksList();
            List<Book> listofBooks = books.GetBooksList();
            bool info = false;

            for (int i = 0; i < listofBorrowedBooks.Count; i++)
            {
                int idSelected = listofBorrowedBooks[i].Id_Book;

                if (idSelected.CompareTo(bookRFromGui) == 0)
                {
                    if (listofBorrowedBooks[i].Availability == true)
                    {
                        info = true;
                        int idBookFromGui = bookRFromGui;

                        string username = "";
                        string dateBorrow = "";

                        string path2 = System.IO.Path.Combine("../../../DataBases/ReservedBooks.txt");
                        using (StreamWriter writer = new StreamWriter(path2))
                        {
                            for (int k = 0; k < listofBorrowedBooks.Count; k++)
                            {
                                if (idBookFromGui == listofBorrowedBooks[k].Id_Book)
                                {
                                    username = listofBorrowedBooks[k].UserName;
                                    dateBorrow = listofBorrowedBooks[k].DateTime1;
                                }
                                else writer.WriteLine(listofBorrowedBooks[k].Id_Book + " " + listofBorrowedBooks[k].Author + " " + listofBorrowedBooks[k].Title + " " + listofBorrowedBooks[k].Availability + " " + listofBorrowedBooks[k].Id_Library + " " + listofBorrowedBooks[k].DateTime1 + " " + listofBorrowedBooks[k].UserName);
                            }
                            writer.Close();
                        };
                        MessageBox.Show("Zwrócono ksiązkę!");
                        UptodateTable();

                        string path3 = System.IO.Path.Combine("../../../DataBases/BookList.txt");
                        using (StreamWriter writer = new StreamWriter(path3))
                        {
                            for (int k = 0; k < listofBooks.Count; k++)
                            {
                                if (idBookFromGui == listofBooks[k].Id_Book) writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + "True" + " " + listofBooks[k].Id_Library);
                                else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                            }
                            writer.Close();
                        };

                        Bill(dateBorrow, ReturnDate);

                        for (int k = 0; k < listofBooks.Count; k++)
                        {
                            if (idBookFromGui == listofBooks[k].Id_Book) accountModel.WriteToDataBase("BookHistory", listofBooks[k].Id_Book + " " + listofBooks[k].Id_Library + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + username + " " + dateBorrow + " " + ReturnDate);
                        }
                    }
                    else { MessageBox.Show("Ta ksiązka jest niedostępna!"); info = true; }
                }
            }
            if (info == false) { MessageBox.Show("Nie istnieje książka o podanym id"); }

        }

        private void Bill(string dateBorrow,string dateReturn)
        {
            DateTime dateTimeBorrow = DateTime.ParseExact(dateBorrow, "MM/dd/yyyy", null);
            DateTime dateTimeReturn = DateTime.ParseExact(dateReturn, "MM/dd/yyyy", null);

            TimeSpan result = dateTimeBorrow - dateTimeReturn;

            double zaplata = result.Days * 0.1;
            if(zaplata > 1) { MessageBox.Show("Do zapłaty: " + zaplata + " zł," + " za " + result.Days + " dni"); }
            else { MessageBox.Show("Do zapłaty: " + zaplata + " groszy," + " za " + result.Days + " dni"); } 
            if (result.Days == 1) { MessageBox.Show("Do zapłaty: " + zaplata + " groszy," + " za " + result.Days + " dzień"); }
        }

        private void AuthorInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            AuthorInput.Text = loginMethod.EraseWhiteSpace(AuthorInput.Text);
        }

        private void TitleInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            AuthorInput.Text = loginMethod.EraseWhiteSpace(AuthorInput.Text);
        }

        private void QuantityInput_TextChanged(object sender, TextChangedEventArgs e)
        {
           if (System.Text.RegularExpressions.Regex.IsMatch(QuantityInput.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                QuantityInput.Text = QuantityInput.Text.Remove(QuantityInput.Text.Length - 1);
            }
        }
    }
}