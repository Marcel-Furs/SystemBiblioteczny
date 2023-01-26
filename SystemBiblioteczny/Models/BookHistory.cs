using System.Collections.Generic;

namespace SystemBiblioteczny.Models
{
    public class BookHistory : BookReserved
    {
        public string DateReturn { get; set; }

        public List<BookHistory> listH = new();

        public BookHistory(int id, int libraryId, string author, string title, string userName, string dateTaken, string dateReturn)
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
