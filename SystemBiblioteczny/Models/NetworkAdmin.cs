using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    internal class NetworkAdmin : Person
    {
        public void AddLibrary(Library library) {
            Libraries lib = new Libraries();
            _ = lib.AddLibraryToDB(library);
           }

        public void RemoveLibrary(Library library) {
           //Zmienianie pliku tekstowego
        }
        public void CreateRaport() { }

        public void ManageAccounts() { }
    }
}
