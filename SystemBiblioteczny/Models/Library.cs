using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    internal class Library
    {
        public string? Name { get; set; }
        public LocalAdmin? Admin { get; set; }
        public string? Address { get; set; }
        public int? Id { get; set; }

        public Library(string Name, LocalAdmin Admin, string Address, int Id) { 
            this.Name = Name;
            this.Admin = Admin;
            this.Address = Address;
            this.Id = Id;
        }

        public List<Library>? LibrariesList = new Libraries().GetLibrariesList(); //TODO sprawdzic czy dziala lista pobierana z bazy danych z pliku *.txt

        public List<Library>? GetDataList() { return LibrariesList; }

    }
}
