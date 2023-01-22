using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

        private List<Client> ClientList()
        {
            List<Client> list = new();
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
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
               
                string username = splitted[0];
                string password = splitted[1];
                string firstName = splitted[2];
                string lastName = splitted[3];
                string email = splitted[4];
                string phone = splitted[5];

                Client client = new(username, password, firstName,lastName,email,phone);

                list.Add(client);

            }

            return list;

        }
        private List<LocalAdmin> LocalAdminList() {
            List<LocalAdmin> list = new();
            string path = System.IO.Path.Combine("../../../DataBases/LocalAdminList.txt");
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
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string username = splitted[0];
                string password = splitted[1];
                int newIdLibrary = int.Parse(splitted[2]);

                LocalAdmin admin = new(username,password,newIdLibrary);

                list.Add(admin);

            }

            return list;

        }
        private List<Librarian> LibrarianList()
        {
            List<Librarian> list = new();
            string path = System.IO.Path.Combine("../../../DataBases/LibrarianList.txt");
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
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string username = splitted[0];
                string password = splitted[1];
                int newIdLibrary = int.Parse(splitted[2]);

                Librarian librarian = new(username, password, newIdLibrary);

                list.Add(librarian);

            }

            return list;

        }
        private List<NetworkAdmin> NetworkAdminList()
        {
            List<NetworkAdmin> list = new();
            string path = System.IO.Path.Combine("../../../DataBases/NetworkAdminList.txt");
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
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                string username = splitted[0];
                string password = splitted[1];
               

                NetworkAdmin admin = new(username, password);

                list.Add(admin);

            }

            return list;

        }
    }
}
