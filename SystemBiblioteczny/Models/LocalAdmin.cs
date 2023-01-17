using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    internal class LocalAdmin : Person
    {
       public LocalAdmin(string UserName, string FirstName = "FirstName", string LastName = "LastName") {
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
        public void ManageEvent() { }

        public void ExchangeBooks() { }
        public void CreateRaport() { }

        public void ManageAccounts() { }
    }
}
