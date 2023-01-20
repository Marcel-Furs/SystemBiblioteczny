using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class LocalAdmin : Person
    {
        public int LibraryId { get; set; }
       public LocalAdmin(string UserName, string Password = "password", int LibraryId = 1, string LastName = "LastName", string FirstName = "FirstName")
        {
            this.Password = Password;
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.LibraryId = LibraryId;
        }
        public void ManageEvent() { }

        public void ExchangeBooks() { }
        public void CreateRaport() { }

        public void ManageAccounts() { }
    }
}
