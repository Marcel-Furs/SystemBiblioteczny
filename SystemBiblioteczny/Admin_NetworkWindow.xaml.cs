using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
    /// <summary>
    /// Logika interakcji dla klasy Admin_NetworkWindow.xaml
    /// </summary>
    public partial class Admin_NetworkWindow : Window
    {
        private NetworkAdmin networkAdmin = new();
        private AccountBase accountModel = new();
        private LoginMethod loginMethod = new();
        public Admin_NetworkWindow(NetworkAdmin newNetworkAdmin)
        {
            networkAdmin = newNetworkAdmin;
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            LoadLibrariesData();
            EmailBox.Text = networkAdmin.Email;
            PhoneBox.Text = networkAdmin.Phone;
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m = new();
            m.Show();
            this.Close();
        }

        private void Add_Library(object sender, RoutedEventArgs e)
        {
            Libraries l = new();
            if (!l.CheckIfCanAdd(City.Text, Street.Text, Number.Text)) return;
            int newID = l.ReturnUniqueID();
            Library library = new(newID, City.Text, Street.Text, Number.Text);
            l.AddLibraryToDB(library);
            MessageBox.Show("Dodano Bibliotekę! \nID: " + newID + "\nAdres: " + City.Text + " " + Street.Text + " " + Number.Text);
            City.Text = "";
            Street.Text = "";
            Number.Text = "";
            LoadLibrariesData();
        }
        private void LoadLibrariesData()
        {
            Libraries_Table.Items.Clear();
            Libraries library = new();
            List<Library> listOfEvents = library.GetListOfLibraries();
            foreach (Library e in listOfEvents)
            {
                Libraries_Table.Items.Add(e);
            }
        }
        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password.CompareTo(PasswordBox2.Password) == 0)
            {
                if (PasswordBox2.Password.Length < 4) MessageBox.Show("Hasło musi mieć przynajmiej 4 znaki");
                else accountModel.ChangePersonData(networkAdmin, AccountBase.RoleTypeEnum.NetworkAdmin, PasswordBox1.Password);
            } 
            else MessageBox.Show("Podane hasła różnią się od siebie");
        }
       
        private void SaveChanges(object sender, RoutedEventArgs e)
        {
           try
           {
                if (EmailBox.Text.CompareTo(networkAdmin.Email) != 0) {
                    MailAddress mail = new MailAddress(EmailBox.Text);
                    accountModel.ChangePersonData(networkAdmin, AccountBase.RoleTypeEnum.NetworkAdmin,"",EmailBox.Text);
                }
                if (PhoneBox.Text.CompareTo(networkAdmin.Phone!.ToString()) != 0)
                {
                    if (PhoneBox.Text.CompareTo("") == 0) MessageBox.Show("Nie podano poprawnego numeru telefonu");
                    else accountModel.ChangePersonData(networkAdmin, AccountBase.RoleTypeEnum.NetworkAdmin, "", "",PhoneBox.Text);
                }

            }
           catch (FormatException)
           {
              MessageBox.Show("Błędny format email!");
           }

        }
        private void Password1Changed(object sender, RoutedEventArgs e)
        {
            string a = PasswordBox1.Password;
            string b = loginMethod.EraseWhiteSpace(PasswordBox1.Password);
            if (a != b) PasswordBox1.Password = b;
        }
        private void Password2Changed(object sender, RoutedEventArgs e)
        {
            string a = PasswordBox2.Password;
            string b = loginMethod.EraseWhiteSpace(PasswordBox2.Password);
            if (a != b) PasswordBox2.Password = b;
        }
        private void PhoneBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(PhoneBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Proszę wpisać numer.");
                PhoneBox.Text = PhoneBox.Text.Remove(PhoneBox.Text.Length - 1);
            }
        }
        private void ShowClientList(object sender, RoutedEventArgs e)
        {
            ShowClientListMethod();
        }
        private void ShowClientListMethod() {
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
        private void ShowLibrarianListMethod() {
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
        private void ShowAdminListMethod() {
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
            for (int i = 0; i < list.Count; i++) {
                if (list[i].UserName!.CompareTo(UserNameTextBox.Text) == 0) {
                    info = true;
                    if (IdLibraryLabel.Text == "") MessageBox.Show("Proszę wpisać poprawne id");
                    else {
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

        private void MakeClientAnAdmin(object sender, RoutedEventArgs e)
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
                        LocalAdmin admin = new(list[i].UserName!, list[i].Password!, list[i].FirstName!, list[i].LastName!, list[i].Email!, newLibId, list[i].Phone!);
                        accountModel.AddLocalAdminToListAndDeleteFromClients(admin);
                        MessageBox.Show("Nadano uprawnienia");
                        ShowAdminListMethod();
                    }

                }
            }
            if (info == false) MessageBox.Show("Nie istnieje klient o podanej nazwie");
        }

        private void MakePersonAnClient(object sender, RoutedEventArgs e)
        {
            List<LocalAdmin> admins = accountModel.GetLocalAdminList();
            List<Librarian> librarians = accountModel.GetLibrarianList();
            Client client = new();

            bool info = false;
            AccountBase.RoleTypeEnum role = AccountBase.RoleTypeEnum.Client;
            for (int i = 0; i < admins.Count; i++)
            {
                if (admins[i].UserName!.CompareTo(UserNameTextBox.Text) == 0)
                {
                    info = true;
                    role = AccountBase.RoleTypeEnum.LocalAdmin;
                    client = new(admins[i].UserName!, admins[i].Password!, admins[i].FirstName!, admins[i].LastName!, admins[i].Email!, admins[i].Phone!);
                }
            }
            for (int i = 0; i < librarians.Count; i++)
            {
                if (librarians[i].UserName!.CompareTo(UserNameTextBox.Text) == 0)
                {
                    info = true;
                    role = AccountBase.RoleTypeEnum.Librarian;
                    client = new(librarians[i].UserName!, librarians[i].Password!, librarians[i].FirstName!, librarians[i].LastName!, librarians[i].Email!, librarians[i].Phone!);
                }
            }
            if (info == true)
            {
                string path = "";
                if (role == AccountBase.RoleTypeEnum.LocalAdmin) path = System.IO.Path.Combine("../../../DataBases/LocalAdminList.txt");
                if (role == AccountBase.RoleTypeEnum.Librarian) path = System.IO.Path.Combine("../../../DataBases/LibrarianList.txt");

                List<string> lines = new();
                if (role == AccountBase.RoleTypeEnum.LocalAdmin) lines = accountModel.GetListOfDataBaseLines("LocalAdminList");
                if (role == AccountBase.RoleTypeEnum.Librarian) lines = accountModel.GetListOfDataBaseLines("LibrarianList");

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

        private void Remove_Library(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Dodanie na nowo biblioteki będzie wiązało się z nadawaniem uprawnień administratorom, bibliotekarzom i przypisywaniem książek!", "Czy chcesz rozpocząć usuwanie biblioteki?", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Library library = new();
                library = (Library)Libraries_Table.SelectedItem;
                if (library != null)
                {
                    Libraries libraries = new();
                    libraries.RemoveLibraryAndChangeIdTo0(library);
                }
                LoadLibrariesData();
            }
        }

        private void City_TextChanged(object sender, TextChangedEventArgs e)
        {
            City.Text = loginMethod.EraseWhiteSpace(City.Text);
        }

        private void Street_TextChanged(object sender, TextChangedEventArgs e)
        {
            Street.Text = loginMethod.EraseWhiteSpace(Street.Text);
        }

        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            Number.Text = loginMethod.EraseWhiteSpace(Number.Text);
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



            // ilosc ksiazek we wszystkich bibliotekach
            Books a = new();
            List<Book> AllBooksList = a.GetBooksList();
            int BooksCount = 0;
            for (int i = 0; i < AllBooksList.Count; i++)
            {
                BooksCount++;
            }

            // ilosc wypozyczen we wszystkich bibliotekach
            BooksReserved b = new();
            AccountBase history = new();
            List<string> listHistory = history.GetListOfDataBaseLines("BookHistory");
            List<BookReserved> AllBooksReservedList = b.GetReservedBooksList();
            int ReservedBooksCount = 0;
            for (int i = 0; i < AllBooksReservedList.Count; i++)
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


            // ilosc zarejestrowanych klientow we wszystkich bibliotekach
            AccountBase c = new();
            int AllClients = c.GetClientList().Count;

            // ilosc aktywnych klientow (czyli klientów którzy wypozyczyli co najmniej 1 ksiazke) we wszystkich bibliotekach
            List<string> list = c.GetListOfDataBaseLines("BookHistory");
            List<string> userList = new();
            userList.Add("");
            int AllActiveClients = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string line = list[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                int libId = int.Parse(splitted[1]);
                string user = splitted[2];
                    for (int j = 0; j < userList.Count; j++)
                    {
                        if (userList[j].CompareTo(user) != 0)
                        {
                            AllActiveClients++;
                            userList.Add(user);
                        }
                    }
                

            }


            // ilosc bibliotekarzy we wszystkich bibliotekach
            List<Librarian> AllLibrarians = c.GetLibrarianList();
            int allLibrans = 0;
            for (int i = 0; i < AllLibrarians.Count; i++)
            {
                Librarian librarian = AllLibrarians[i];
                allLibrans++;
                
            }


            // ilosc wieczorkow autorskich we wszystkich bibliotekach
            AuthorsEvenings evenings = new();
            List<AuthorsEvening> AuthorEvenings = evenings.GetEventList();
            int AllAuthorEvenings = 0;
            for (int i = 0; i < AuthorEvenings.Count; i++)
            {
                AuthorsEvening f = AuthorEvenings[i];
                AllAuthorEvenings++;
            }

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string fileName = "raport-" + networkAdmin.FirstName +"-" + currentDateTime + ".pdf";
            string path = @"../../../Raporty/" + fileName;

            if (!Directory.Exists("../../../Raporty/"))
            {
                Directory.CreateDirectory("../../../Raporty/");
            }

            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

            doc.Open();
            if (activeCustomers != 0)
            {
                PdfPTable tablee = new PdfPTable(1);
                tablee.WidthPercentage = 100;
                tablee.AddCell(new PdfPCell(new Phrase("Liczba aktywnych klientów w zakresie: " + chosenStartDate + " - " + chosenEndDate + ": ")));
                tablee.AddCell(new iTextSharp.text.Paragraph(activeCustomers.ToString()));
                doc.Add(tablee);
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
                doc.Close();
            }
            MessageBox.Show("Utworzono raport.");
        }

       
    }
}
