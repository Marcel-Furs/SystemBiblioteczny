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
        public Admin_NetworkWindow()
        {
            InitializeComponent();
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
    }
}
