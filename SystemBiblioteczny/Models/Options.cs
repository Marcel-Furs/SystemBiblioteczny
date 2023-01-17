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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny.Models
{
    public class Options
    {
        public MainWindow mainwwindow = new();
        private string login;
        private string password;

        public void ClientOpt(string name, string password1)
        {
            Person person = new Person();
            person.UserName = "m";
            person.Password = "123";

            if (name == null || password1 == null)
            {
                MessageBox.Show("Nie moga byc puste pola!");
                return;
            }

            if (name == person.UserName && password1 == person.Password)
            {
                MessageBox.Show("Zalogowano!");
                ClientWindow clientwindow = new ClientWindow();
                clientwindow.Show();
                mainwwindow.Close();
            }
            else
            {
                MessageBox.Show("Niepoprawne haslo lub login");
            }
        }

        public void Admin_localOpt(string name, string password1)
        {
            Person person = new Person();
            person.UserName = "m";
            person.Password = "123";

            if (name == null || password1 == null)
            {
                MessageBox.Show("Nie moga byc puste pola!");
                return;
            }

            if (name == person.UserName && password1 == person.Password)
            {
                MessageBox.Show("Zalogowano!");
                Admin_LocalWindow admin_localwindow = new();
                admin_localwindow.Show();
                mainwwindow.Close();
            }
            else
            {
                MessageBox.Show("Niepoprawne haslo lub login");
            }
        }

        public void Admin_networklOpt(string name, string password1)
        {
            Person person = new Person();
            person.UserName = "m";
            person.Password = "123";

            if (name == null || password1 == null)
            {
                MessageBox.Show("Nie moga byc puste pola!");
                return;
            }

            if (name == person.UserName && password1 == person.Password)
            {
                MessageBox.Show("Zalogowano!");
                Admin_NetworkWindow admin_networkwindow = new();
                admin_networkwindow.Show();
                mainwwindow.Close();
            }
            else
            {
                MessageBox.Show("Niepoprawne haslo lub login");
            }
        }

        public void LibrarianOpt(string name, string password1)
        {
            Person person = new Person();
            person.UserName = "m";
            person.Password = "123";

            if (name == null || password1 == null)
            {
                MessageBox.Show("Nie moga byc puste pola!");
                return;
            }

            if (name == person.UserName && password1 == person.Password)
            {
                MessageBox.Show("Zalogowano!");
                LibrarianWindow librarianwindow = new();
                librarianwindow.Show();
                mainwwindow.Close();
            }
            else
            {
                MessageBox.Show("Niepoprawne haslo lub login");
            }
        }

    }
}
