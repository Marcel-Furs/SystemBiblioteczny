using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny
{
    public class Books
    {
        public List<Book> GetBooksList()
        {
            List<Book> list = new();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "../../../DataBases/BookList.txt");

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

                Book book = new(newId, newAuthor, newTitle, newAvailibility, newIdLibrary);

                list.Add(book);

            }
            return list;
        }
    }
}