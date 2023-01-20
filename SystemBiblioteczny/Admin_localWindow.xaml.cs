using Accessibility;
using System;
using System.Collections.Generic;
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

            userDataFinal = userData;

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
                            //TODO zmiana id biblioteki na ten w ktorym jest wysylajacy zlecenie wymiany
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
                                    string newReciever = splitted[3];
                                    string newAuthor = splitted[4];
                                    string newTitle = splitted[5];
                                    bool newAvailibility = bool.Parse(splitted[6]);
                                    int newIdLibrary = int.Parse(splitted[7]);


                                    if (newId.CompareTo(SendBookLabelInt) == 0) {}
                                    else if (echangeId > SendBookLabelInt) { writer.WriteLine((echangeId - 1)+" "+ bookId  + " " + newRequestor  + " " + newReciever + " " + newAuthor + " " + newTitle + " " + newAvailibility + " " + newIdLibrary); }
                                    else writer.WriteLine(line);
                                }
                              
                                writer.Close();
                            }

                            MessageBox.Show("Wysłano książke");

                            Admin_LocalWindow w = new(userDataFinal);
                            w.Show();
                            this.Close();
                           

                        } 
                    else MessageBox.Show("Książka aktualnie nie jest dostępna");
                    }
                } 
            }
            if (info == false) { MessageBox.Show("Nie istnieje zlecenie o podanym id"); }

        }
    }
}
