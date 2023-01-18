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
                                    ClientWindow clientwindow = new();
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
       public void AddUserToDB(Client newClient)
        {
            string path = System.IO.Path.Combine("../../../DataBases/ClientList.txt");
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
         
                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
                writer.WriteLine(newClient.UserName + " " + newClient.Password + " " + newClient.FirstName + " " + newClient.LastName + " " + newClient.Email + " " + newClient.Phone);
                writer.Close();
            }
           

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
            //AddUserToDB(username, password);
            MessageBox.Show("Użytkownik został utworzony");
            return true;
        }
    }
    }

