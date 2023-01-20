using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SystemBiblioteczny.Models
{
    public class BookExchange : Book
    {
        public int ExchangeId { get; set; }
        public string RequestorUsername { get; set; }
        public string ReceiverUsername { get; set; }

        public BookExchange(int ExchangeId, string RequestorUsername, string ReceiverUsername, int id_Book, string author, string title, bool availability, int id_Library) {
        this.ExchangeId = ExchangeId;
        this.RequestorUsername= RequestorUsername;
        this.ReceiverUsername= ReceiverUsername;
             Id_Book = id_Book;
             Author = author;
             Title = title;
             Availability = availability;
             Id_Library = id_Library;
        }
        public BookExchange()
        {
            
        }

        public List<BookExchange> GetExchangeBooksList()
        {
            List<BookExchange> list = new();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "../../../DataBases/ExchangeBookList.txt");

            List<string> lines = new();

            using (StreamReader reader = new(path))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
            }

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                int exchangeId = int.Parse(splitted[0]);
                int bookId = int.Parse(splitted[1]);
                string newRequestor = splitted[2];
                string newReciever = splitted[3];
                string newAuthor = splitted[4];
                string newTitle = splitted[5];
                bool newAvailibility = bool.Parse(splitted[6]);
                int newIdLibrary = int.Parse(splitted[7]);

                BookExchange book = new (exchangeId, newRequestor, newReciever, bookId, newAuthor, newTitle, newAvailibility, newIdLibrary);
                
                list.Add(book);

            }
            return list;
        }
    }
}
