using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class ApplicationBook:Book
    {
        public string Quantity { get; set; }
        public string Librarian { get; set; }

        public bool Approved { get; set; }

        public ApplicationBook(string title, string author, string quantity, string librarian, bool approved)
        {
            this.Title = title;
            this.Author = author;
            this.Quantity = quantity;
            this.Librarian = librarian;
            this.Approved = approved;

        }
    }
}
