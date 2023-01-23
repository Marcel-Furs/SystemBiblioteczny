using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class ApplicationBook:Book
    {
        private AccountBase accountModel = new();
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

        public ApplicationBook()
        {

        }

        public List<ApplicationBook> GetApplicationBooksList()
        {
            List<ApplicationBook> list = new();
            List<string> lines = accountModel.GetListOfDataBaseLines("BookApplicationList");

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                string newTitle = splitted[0];
                string newAuthor = splitted[1];
                string newQuantity = splitted[2];
                string newRequestor = splitted[3];

                ApplicationBook book = new(newTitle, newAuthor, newQuantity, newRequestor, false);

                list.Add(book);

            }
            return list;
        }
    }
}
