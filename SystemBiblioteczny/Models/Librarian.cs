using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class Librarian : Person
    {
        public int LibraryId { get; set; }
        public Librarian(string Username, string Password, int LibraryId) {
            this.UserName= Username;
            this.Password= Password;
            this.LibraryId = LibraryId;
        }
        public void RentBook() { }

        public void EndRental() { }
    }
}
