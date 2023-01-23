using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class BookReserverd : Book
    {
        public  string DateTime1 { get; set; }
        public BookReserverd(int id_Book, string author, string title, bool availability, int id_Library, string dateTime)
        {
            Id_Book = id_Book;
            Author = author;
            Title = title;
            Availability = availability;
            Id_Library = id_Library;
            DateTime1 = dateTime;
        }

        public BookReserverd()
        {
        }

        public bool AccountBalance(string date11)
        {
            DateTime zmiana = DateTime.ParseExact(date11, "MM/dd/yyyy", null );
            DateOnly dateczas = DateOnly.FromDateTime(zmiana);
            DateOnly dateczas1 = DateOnly.FromDateTime(DateTime.Now);

            bool status = false;
            if((dateczas.Day - dateczas1.Day) > 7) status = true;

            return status;
        }
    }
}
