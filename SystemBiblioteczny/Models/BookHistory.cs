using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class BookHistory : BookReserved
    {
        public string DateReturn { get; set; }

        public List<BookHistory> listH = new();

        public BookHistory (int id, int libraryId ,string author, string title,string userName,string dateTaken, string dateReturn)
        {
            Id_Book = id;
            Id_Library = libraryId;
            Author = author;
            Title = title;
            UserName = userName;
            DateTime1 = dateTaken;
            DateReturn = dateReturn;
        }

        public BookHistory()
        {
        }
    }
}
