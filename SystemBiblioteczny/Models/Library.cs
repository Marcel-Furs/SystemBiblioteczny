using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class Library 
    {
        public int ID { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Local { get; set; }

        public Library() { }
        public Library(int id, string city, string street, string local)  {
            this.ID = id;
            this.City = city;
            this.Street = street;
            this.Local = local;
        }

        public Libraries ListOfLibraries = new();

        //public List<Library>? GetListOfLibraries() { return ListOfLibraries.GetLibrariesList(); }

    }
}
