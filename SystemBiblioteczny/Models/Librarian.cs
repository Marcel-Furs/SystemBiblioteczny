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
        public Librarian(string username, string password, int libraryId) {
            this.UserName= username;
            this.Password= password;
            this.LibraryId = libraryId;
        }
        public void RentBook() { }

        public void EndRental() { }
    }
}
