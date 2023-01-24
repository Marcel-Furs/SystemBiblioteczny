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
    /// Logika interakcji dla klasy LibrarianWindow.xaml
    /// </summary>
    public partial class LibrarianWindow : Window
    {
        private ApplicationBook applicationBookModel = new();
        private AccountBase accountModel = new();
        private Librarian librarianModel = new();
        private string titleFromGui = "";
        public LibrarianWindow(Librarian userData)
        {
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            librarianModel = userData;
            RefreshTableApprovedApplicationsData();
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

    }
}