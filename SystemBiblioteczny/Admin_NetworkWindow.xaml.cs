using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy Admin_NetworkWindow.xaml
    /// </summary>
    public partial class Admin_NetworkWindow : Window
    {
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
    }
}
