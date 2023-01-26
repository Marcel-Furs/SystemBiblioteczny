using System;

namespace SystemBiblioteczny.Models
{
    public class BookReserved : Book
    {
        public string UserName { get; set; }
        public string DateTime1 { get; set; }
        public BookReserved(int id_Book, string author, string title, bool availability, int id_Library, string dateTime, string userName)
        {
            Id_Book = id_Book;
            Author = author;
            Title = title;
            Availability = availability;
            Id_Library = id_Library;
            DateTime1 = dateTime;
            UserName = userName;
        }

        public BookReserved()
        {
        }

        public bool AccountBalance(string date11)
        {
            DateTime zmiana = DateTime.ParseExact(date11, "MM/dd/yyyy", null);
            DateOnly dateczas = DateOnly.FromDateTime(zmiana);
            DateOnly dateczas1 = DateOnly.FromDateTime(DateTime.Now);

            bool status = false;
            if ((dateczas.Day - dateczas1.Day) > 7) status = true;

            return status;
        }
    }
}
