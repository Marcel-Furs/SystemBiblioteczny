﻿using System;
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
        private AccountBase accountModel = new();
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
            Person_Table.Items.Clear();
            List<Client> clients = accountModel.GetClientList();
            foreach (Client c in clients) {
                Person_Table.Items.Add(c);
            }
            Person_Table.IsReadOnly = true;
        }
        private void ShowLibrarianList(object sender, RoutedEventArgs e)
        {
            Person_Table.Items.Clear();
            List<Librarian> librarians = accountModel.GetLibrarianList();
            foreach (Librarian l in librarians)
            {
                Person_Table.Items.Add(l);
            }
            Person_Table.IsReadOnly = true;
        }

        private void ShowAdminList(object sender, RoutedEventArgs e)
        {
            Person_Table.Items.Clear();
            List<LocalAdmin> admins = accountModel.GetLocalAdminList();
            foreach (LocalAdmin a in admins)
            {
                Person_Table.Items.Add(a);
            }
            Person_Table.IsReadOnly = true;
        }
        
    }
}
