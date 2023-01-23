using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class BooksReserved
    {
        public List<BookReserverd> GetReservedBooksList()
        {
            List<BookReserverd> list = new();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "../../../DataBases/ReservedBooks.txt");

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

        public void SaveReservedBooks(BookReserverd b)
        {
            List<BookReserverd> listofBooks = new();
            listofBooks.Add(b);
            List<string> lines = new();

            string path2 = System.IO.Path.Combine("../../../DataBases/ReservedBooks.txt");
            using (StreamWriter writer = new StreamWriter(path2))
            {
                for (int k = 0; k < listofBooks.Count; k++)
                {
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }
                    writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + "False" + " " + listofBooks[k].Id_Library + " " + listofBooks[k].DateTime1);
                    //else writer.WriteLine(listofBooks[k].Id_Book + " " + listofBooks[k].Author + " " + listofBooks[k].Title + " " + listofBooks[k].Availability + " " + listofBooks[k].Id_Library);
                }
                writer.Close();
            }
        }
    }
}
