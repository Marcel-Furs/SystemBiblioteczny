using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class NetworkAdmin : Person
    {

        public NetworkAdmin(string username, string password) {
            this.UserName = username;
            this.Password = password;
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
