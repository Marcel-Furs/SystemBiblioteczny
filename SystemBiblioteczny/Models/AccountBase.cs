using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SystemBiblioteczny.Models
{
    internal class AccountBase
    {
        private List<Client> clients = new();

        private List<Librarian> librarians = new();

        private List<NetworkAdmin> networkAdmins = new(); 

        private List<LocalAdmin> localAdmins = new();

        public enum RoleTypeEnum { Client, Librarian, LocalAdmin, NetworkAdmin }

        public List<LocalAdmin> GetLocalAdminList() {
            localAdmins = LocalAdminList();
            return localAdmins;
        }
        public List<NetworkAdmin> GetNetworkAdminList()
        {
            networkAdmins = NetworkAdminList();
            return networkAdmins;
        }
        public List<Librarian> GetLibrarianList()
        {
            librarians = LibrarianList();
            return librarians;
        }
        public List<Client> GetClientList()
        {
            clients = ClientList();
            return clients;
        }
        public List<string> GetListOfDataBaseLines(string fileName) {
            
            string path = System.IO.Path.Combine("../../../DataBases/"+fileName+".txt");
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
            return lines;
        }
        public void WriteToDataBase(string fileName, string newLine) {

            List<string> lines = GetListOfDataBaseLines(fileName);
            string path = System.IO.Path.Combine("../../../DataBases/" + fileName + ".txt");

            using (StreamWriter writer = new StreamWriter(path))
            {

                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
                writer.WriteLine(newLine);
                writer.Close();
            }

        }
        public void WriteDataBase(string fileName, List<string> lines)
        {
            string path = System.IO.Path.Combine("../../../DataBases/" + fileName + ".txt");

            using (StreamWriter writer = new StreamWriter(path))
            {

                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
                writer.Close();
            }

        }

        public void AddClientToList(Client client) {

            WriteToDataBase("ClientList", client.UserName+" "+client.Password+" "+client.FirstName+" "+client.LastName+" "+client.Email+" "+client.Phone);

        }

        public void AddLibrarianToListAndDeleteFromClients(Librarian librarian)
        {
            WriteToDataBase("LibrarianList", librarian.UserName + " " + librarian.Password + " " + librarian.FirstName + " " + librarian.LastName + " " + librarian.Email + " " + librarian.LibraryId + " " + librarian.Phone);

            string path = System.IO.Path.Combine("../../../DataBases/ClientList.txt");
            List<string> lines = GetListOfDataBaseLines("ClientList");
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];
                    string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    string userName = splitted[0];
                    if (userName.CompareTo(librarian.UserName) == 0) { }
                    else { writer.WriteLine(line); }
                }
                writer.Close();
            }
        }
        public void AddLocalAdminToListAndDeleteFromClients(LocalAdmin admin)
        {
            WriteToDataBase("LocalAdminList", admin.UserName + " " + admin.Password + " " + admin.FirstName + " " + admin.LastName + " " + admin.Email + " " + admin.LibraryId + " " + admin.Phone);

            string path = System.IO.Path.Combine("../../../DataBases/ClientList.txt");
            List<string> lines = GetListOfDataBaseLines("ClientList");
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];
                    string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    string userName = splitted[0];
                    if (userName.CompareTo(admin.UserName) == 0) { }
                    else { writer.WriteLine(line); }
                }
                writer.Close();
            }
        }

        private List<Client> ClientList()
        {
            List<Client> list = new();
            List<string> lines = GetListOfDataBaseLines("ClientList");
          
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
               
                string username = splitted[0];
                string password = splitted[1];
                string firstName = splitted[2];
                string lastName = splitted[3];
                string email = splitted[4];
                string phone;
                if (splitted.Length < 6) phone = ""; 
                else phone = splitted[5];

                Client client = new(username, password, firstName,lastName,email,phone);
                list.Add(client);

            }

            return list;

        }

        private List<LocalAdmin> LocalAdminList() {

            List<LocalAdmin> list = new();
            List<string> lines = GetListOfDataBaseLines("LocalAdminList");

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string username = splitted[0];
                string password = splitted[1];
                string firstName = splitted[2];
                string lastName = splitted[3];
                string email = splitted[4];
                int newIdLibrary = int.Parse(splitted[5]);
                string phone;
                if (splitted.Length < 7) phone = "";
                else phone = splitted[6];

                LocalAdmin admin = new(username, password, firstName, lastName, email, newIdLibrary, phone);
                list.Add(admin);

            }

            return list;

        }
        private List<Librarian> LibrarianList()
        {
            List<Librarian> list = new();
            List<string> lines = GetListOfDataBaseLines("LibrarianList");

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string username = splitted[0];
                string password = splitted[1];
                string firstName = splitted[2];
                string lastName = splitted[3];
                string email = splitted[4];
                int newIdLibrary = int.Parse(splitted[5]);
                string phone;
                if (splitted.Length < 7) phone = "";
                else phone = splitted[6];

                Librarian librarian = new(username, password, firstName, lastName, email, newIdLibrary, phone);
                list.Add(librarian);

            }

            return list;

        }
        private List<NetworkAdmin> NetworkAdminList()
        {
            List<NetworkAdmin> list = new();
            List<string> lines = GetListOfDataBaseLines("NetworkAdminList");

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string username = splitted[0];
                string password = splitted[1];
                string firstName = splitted[2];
                string lastName = splitted[3];
                string email = splitted[4];
                string phone;
                if (splitted.Length < 6) phone = "";
                else phone = splitted[5];

                NetworkAdmin admin = new(username, password, firstName, lastName, email, phone);
                list.Add(admin);

            }

            return list;

        }
    }
}
