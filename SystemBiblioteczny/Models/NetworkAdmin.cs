using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class NetworkAdmin : Person
    {

        public NetworkAdmin(string UserName, string Password, string FirstName, string LastName, string Email, string Phone)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Password = Password;
            this.UserName = UserName;
            this.Email = Email;
            this.Phone = Phone;
        }

        public NetworkAdmin()
        {
        }

        public void AddLibrary(Library library) {
            Libraries lib = new Libraries();
            lib.AddLibraryToDB(library);
           }

        public void RemoveLibrary(Library library) {
           //Zmienianie pliku tekstowego
        }
        public void CreateRaport() { }

        public void ManageAccounts() { }
    }
}
