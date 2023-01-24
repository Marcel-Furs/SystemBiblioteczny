﻿using System;
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
    /// Logika interakcji dla klasy LibrarianWindow.xaml
    /// </summary>
    public partial class LibrarianWindow : Window
    {
        private ApplicationBook applicationBookModel = new();
        private AccountBase accountModel = new();
        private Librarian librarianModel = new();
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
            var title = TitleInput.Text;
            var author = AuthorInput.Text;
            var quantity = QuantityInput.Text;
            var librarian = librarianModel.UserName!;
            ApplicationBook applicationBook = new(title, author, quantity, librarian, false);

            

            if (title.Any() && author.Any()  && quantity.Any())
            {
                MessageBox.Show("Wysłano zgłoszenie zapotrzebowania na: " + "\n" + author + " " + title + " - ilość: " + quantity);


                List<string> lines = accountModel.GetListOfDataBaseLines("BookApplicationList");
                accountModel.WriteToDataBase("BookApplicationList", TitleInput.Text + " " + AuthorInput.Text + " " + QuantityInput.Text + " " + librarian + " " + false);
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

        private void ApprovedApplicationsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBook book = (ApplicationBook)ApprovedApplicationsTable.SelectedItem;
            if (book != null) titleFromGui = book.Title;
        }

        void RefreshTableApprovedApplicationsData()
        {
            List<ApplicationBook> listofApplicationBooks = applicationBookModel.GetApplicationBooksList();
            
            ApprovedApplicationsTable.Items.Clear();
            for (int i = 0; i < listofApplicationBooks.Count; i++)
            {
                ApplicationBook book = listofApplicationBooks[i];
                if (book.Approved.CompareTo(false) == 0)
                {
                    
                }
                else ApprovedApplicationsTable.Items.Add(book);
            }

            ApprovedApplicationsTable.IsReadOnly = true;

        }

        private void CollectBook(object sender, RoutedEventArgs e)
        {
            Book book1 = new();
            bool info = false;
            if (titleFromGui == "") MessageBox.Show("Nie wybrano książki");
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
                            if (list[j].Title.CompareTo(titleFromGui) == 0 && info == false)
                            {
                                info = true;
                                accountModel.WriteToDataBase("BookList", book1.Id_Book + " " + list[j].Author + " " + list[j].Title + " " + book1.Availability + " " + book1.Id_Library);
                            }
                            else writer.WriteLine(line);
                        }

                        writer.Close();
                    }

                }
                RefreshTableApprovedApplicationsData();
            }
        }
        //Wypozycz ksiazki
        private void RefreshTextBoxes()
        {
            if (RequestBookRLabel.Text == "") { RequestBookRLabel.Text = "0"; }
            bookRFromGui = int.Parse(RequestBookRLabel.Text); ;
        }
        private void RequestBookRLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(RequestBookRLabel.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                RequestBookRLabel.Text = RequestBookRLabel.Text.Remove(RequestBookRLabel.Text.Length - 1);
            }
        }

        private void TableRentBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Book book = (Book)TableRentBook.SelectedItem;
            if (book != null)
            {
                RequestBookRLabel.Text = book.Id_Book.ToString();
            }
        }
        private void UptodateTable()
        {
            TableRentBook.Items.Clear();
            BooksReserved books = new();
            List<BookReserved> listofBooks = books.GetReservedBooksList();
            var listofBooksR = listofBooks.Where(x => x.Id_Library == librarianModel.LibraryId && x.Availability == false).ToList();
            foreach (var e in listofBooksR)
            {
                TableRentBook.Items.Add(e);
            }

        }
        private void Rent_Book(object sender, RoutedEventArgs e)
        {
            RefreshTextBoxes();
            BookReserved bookRe = new();
            BooksReserved booksR = new();

            string czas = DateTime.Now.ToString("MM/dd/yyyy");

            //Book book = new();
            // book = (Book)TableBooks.SelectedItem;
            BooksReserved books = new();

            List<BookReserved> listofBorrowedBooks = books.GetReservedBooksList();
            //BookReserved bookBorrowed = new();
            bool info = false;

            bool status1 = false;

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

        private void Cancel_Reservations(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_All(object sender, RoutedEventArgs e)
        {

        }
    }
}