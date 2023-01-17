﻿using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny.Methods
{
    class LoginMethod
    {
        public bool CheckLogin(string Login, string Password, AccountBase.RoleTypeEnum role)
        {
            bool Logged = false;

            List<Person> list = new();
            List<string> lines = new();
            string path = "";

            switch (role) {
                case (AccountBase.RoleTypeEnum.Client):path = System.IO.Path.Combine("../../../DataBases/ClientList.txt"); break;
                case (AccountBase.RoleTypeEnum.Librarian): path = System.IO.Path.Combine("../../../DataBases/LibrarianList.txt"); break;
                case (AccountBase.RoleTypeEnum.LocalAdmin): path = System.IO.Path.Combine("../../../DataBases/LocalAdminList.txt"); break;
                case (AccountBase.RoleTypeEnum.NetworkAdmin): path = System.IO.Path.Combine("../../../DataBases/NetworkAdminList.txt"); break;
            }

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

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string newLogin = splitted[0];
                string newPassword = splitted[1];

                var person = new Person(newLogin, newPassword);
                list.Add(person);
            }

            bool wrongPassword = false;
            bool correctLogin = false;

            for (int j = 0; j < list.Count; j++)
            {
                string CheckLogin = list[j].UserName!;
                if (CheckLogin.CompareTo(Login) == 0)
                {
                    string CheckPassword = list[j].Password!;
                    if (CheckPassword.CompareTo(Password) == 0)
                    {
                        MessageBox.Show("Poprawnie zalogowano");
                        correctLogin = true;
                        Logged = true;
                        switch (role)
                        {
                            case (AccountBase.RoleTypeEnum.Client): {
                                    ClientWindow clientwindow = new ClientWindow();
                                    clientwindow.Show();
                                } break;
                            case (AccountBase.RoleTypeEnum.Librarian): {
                                    LibrarianWindow librarianwindow = new();
                                    librarianwindow.Show();
                                } break;
                            case (AccountBase.RoleTypeEnum.LocalAdmin): {
                                    Admin_LocalWindow admin_localwindow = new();
                                    admin_localwindow.Show();
                                } break;
                            case (AccountBase.RoleTypeEnum.NetworkAdmin): {
                                    Admin_NetworkWindow admin_networkwindow = new();
                                    admin_networkwindow.Show();
                                } break;
                        }
                        
                    }
                    else wrongPassword = true;
                }
                
            }

            if (wrongPassword == true)  MessageBox.Show("Podane hasło jest nieprawidłowe"); 
            else if (correctLogin == false) MessageBox.Show("Nie znaleziono danego użytkownika");
            return Logged;
        }
       
    }
    }
