using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    internal class AccountBase
    {
        public List<Client> clients= new();

        public List<Librarian> librarians= new();   

        public List<NetworkAdmin> networkAdmins = new(); //owner? admin sieci?

        public List<LocalAdmin> localAdmins= new();

        public enum RoleTypeEnum { Client, Librarian, LocalAdmin, NetworkAdmin }
    }
}
