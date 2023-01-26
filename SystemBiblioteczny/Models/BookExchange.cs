using System;
using System.Collections.Generic;

namespace SystemBiblioteczny.Models
{
    public class BookExchange : Book
    {
        public int ExchangeId { get; set; }
        public string RequestorUsername { get; set; }
        private AccountBase accountModel = new();

        public BookExchange(int ExchangeId, string RequestorUsername, int id_Book, string author, string title, int id_Library)
        {
            this.ExchangeId = ExchangeId;
            this.RequestorUsername = RequestorUsername;

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
            List<string> lines = accountModel.GetListOfDataBaseLines("ExchangeBookList");

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i] != "")
                {
                    string line = lines[i];
                    string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                    int exchangeId = int.Parse(splitted[0]);
                    int bookId = int.Parse(splitted[1]);
                    string newRequestor = splitted[2];
                    string newAuthor = splitted[3];
                    string newTitle = splitted[4];
                    int newIdLibrary = int.Parse(splitted[5]);

                    BookExchange book = new(exchangeId, newRequestor, bookId, newAuthor, newTitle, newIdLibrary);

                    list.Add(book);
                }
            }
            return list;
        }
    }
}
