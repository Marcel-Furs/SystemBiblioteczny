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
using SystemBiblioteczny.Methods;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny
{
    /// <summary>
    /// Logika interakcji dla klasy Admin_NetworkWindow.xaml
    /// </summary>
    public partial class Admin_NetworkWindow : Window
    {
        private AccountBase accountModel = new();
        private LoginMethod loginMethod = new();
        public Admin_NetworkWindow()
        {
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            LoadLibrariesData();
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
                if (role == AccountBase.RoleTypeEnum.Librarian) path =  System.IO.Path.Combine("../../../DataBases/LibrarianList.txt");
               
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
    }
}
