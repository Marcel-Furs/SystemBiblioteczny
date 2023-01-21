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
        

        public BookExchange(int ExchangeId, string RequestorUsername, int id_Book, string author, string title,  int id_Library) {
        this.ExchangeId = ExchangeId;
        this.RequestorUsername= RequestorUsername;
        
             Id_Book = id_Book;
             Author = author;
             Title = title;
           
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
               
                string newAuthor = splitted[3];
                string newTitle = splitted[4];
               
                int newIdLibrary = int.Parse(splitted[5]);

                BookExchange book = new (exchangeId, newRequestor, bookId, newAuthor, newTitle, newIdLibrary);
                
                list.Add(book);

            }
            return list;
        }
    }
}
