﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class BooksReserved
    {
        private AccountBase account = new();
       
        public List<BookReserverd> GetReservedBooksList()
        {
            List<BookReserverd> list = new();

            List<string> lines = account.GetListOfDataBaseLines("ReservedBooks");

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                int newId = int.Parse(splitted[0]);
                string newAuthor = splitted[1];
                string newTitle = splitted[2];
                bool newAvailibility = bool.Parse(splitted[3]);
                int newIdLibrary = int.Parse(splitted[4]);
                string newDate = splitted[5];

                BookReserverd book = new(newId, newAuthor, newTitle, newAvailibility, newIdLibrary, newDate);

                list.Add(book);

            }
            return list;
        }

        public void SaveReservedBooks(BookReserverd bookR)
        {
            account.WriteToDataBase("ReservedBooks", bookR.Id_Book + " " + bookR.Author + " " + bookR.Title + " " + "False" + " " + bookR.Id_Library + " " + bookR.DateTime1);
        }
    }
}
