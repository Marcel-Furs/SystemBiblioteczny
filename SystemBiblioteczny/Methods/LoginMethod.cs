using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Xml;
using SystemBiblioteczny.Models;

namespace SystemBiblioteczny.Methods
{
    class LoginMethod
    {
        private AccountBase accountModel = new();
        public bool CheckLogin(string Login, string Password, AccountBase.RoleTypeEnum role)
        {
            bool Logged = false;
            string file = "";

            switch (role) {
                case (AccountBase.RoleTypeEnum.Client):file = "ClientList" ; break;
                case (AccountBase.RoleTypeEnum.Librarian): file = "LibrarianList"; break;
                case (AccountBase.RoleTypeEnum.LocalAdmin): file = "LocalAdminList"; break;
                case (AccountBase.RoleTypeEnum.NetworkAdmin): file = "NetworkAdminList"; break;
            }

            List<Person> list = new();
            List<string> lines = accountModel.GetListOfDataBaseLines(file);


            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string newLogin = splitted[0];
                string newPassword = splitted[1];
                string firstName = splitted[2];
                string lastName = splitted[3];
                string email = splitted[4];
                int newIdLibrary = 0;
                if (splitted.Length >= 6) newIdLibrary = int.Parse(splitted[5]);
                string phone = "";
                if (splitted.Length >= 7) phone = splitted[6];

                switch (role)
                {
                    case (AccountBase.RoleTypeEnum.Client):
                        {
                            var person = new Person(newLogin, newPassword);
                            list.Add(person);
                        }
                        break;
                    case (AccountBase.RoleTypeEnum.Librarian):
                        {
                            var person = new Person(newLogin, newPassword);
                            list.Add(person);
                        }
                        break;
                    case (AccountBase.RoleTypeEnum.LocalAdmin):
                        {
                            LocalAdmin person = new(newLogin, newPassword, firstName, lastName, email, newIdLibrary, phone);
                            list.Add(person);
                        }
                        break;
                    case (AccountBase.RoleTypeEnum.NetworkAdmin):
                        {
                            var person = new Person(newLogin, newPassword);
                            list.Add(person);
                        }
                        break;
                }
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
                                    Client userData = new(list[j].UserName);
                                    ClientWindow clientwindow = new(userData);
                                    clientwindow.Show();
                                } break;
                            case (AccountBase.RoleTypeEnum.Librarian): {
                                    LibrarianWindow librarianwindow = new();
                                    librarianwindow.Show();
                                } break;
                            case (AccountBase.RoleTypeEnum.LocalAdmin): {
                                    LocalAdmin userData = (LocalAdmin)list[j];
                                    Admin_LocalWindow admin_localwindow = new(userData);
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
        private bool CheckIfUsernameIsUnique(string Login)
        {

            List<Person> list = new();
            List<string> lines = new();

            string path = System.IO.Path.Combine("../../../DataBases/ClientList.txt");
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

                string CheckLogin = splitted[0];
                if (CheckLogin.CompareTo(Login) == 0)
                {
                    return false;
                }
            }


            return true;
        }
      
        public bool CheckIfAllDataIsCorrectAndCanCreateAccount(string username, string password, string confirmPassword, string name, string lastname, string email)
        {
            bool unique = this.CheckIfUsernameIsUnique(username);
            if (username.Length < 4)
            {
                MessageBox.Show("Nazwa użytkownika musi mieć przynajmniej 4 znaki!");
                return false;
            }
            if (unique == false)
            {
                MessageBox.Show("Użytkownik o takiej nazwie już istnieje!");
                return false;
            }
            if (password.Length < 4)
            {
                MessageBox.Show("Hasło musi mieć przynajmiej 4 znaki!");
                return false;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Powtórzone hasło nie jest takie samo!");
                return false;
            }
            if (name.Length < 1)
            {
                MessageBox.Show("Imię nie może być puste!");
                return false;
            }
            if (lastname.Length < 1)
            {
                MessageBox.Show("Nazwisko nie może być puste!");
                return false;
            }
            try
            {
                MailAddress m = new MailAddress(email);
            }
            catch (FormatException)
            {
                MessageBox.Show("Błędny format email!");
                return false;
            }
            MessageBox.Show("Użytkownik został utworzony");
            return true;
        }
        public String EraseWhiteSpace(string s1) {
            for (int i = 0; i < s1.Length; i++)
            {
                if (Char.IsWhiteSpace(s1[i]))
                {
                    MessageBox.Show("Nie używaj spacji! \n Zamist tego użyj _'");
                    s1 = s1.Replace(" ", "");
                }
            }
            return s1;
        }
    }
    }

