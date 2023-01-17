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

        public List<Library>? Libraries; //TODO lista pobierana z bazy danych z pliku *.txt

        public List<Library>? GetLibrariesList() { return Libraries; }

    }
}
