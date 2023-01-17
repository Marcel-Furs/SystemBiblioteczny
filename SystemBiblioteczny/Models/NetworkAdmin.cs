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
            Library lib = new Library();
            List<Library> Libraries = lib.GetLibrariesList();
            Libraries.Add(library);}

        public void RemoveLibrary(Library library) {
            Library lib = new Library();
            List<Library> Libraries = lib.GetLibrariesList();
            Libraries.Remove(library);
        }
        public void CreateRaport() { }

        public void ManageAccounts() { }
    }
}
