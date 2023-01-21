using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SystemBiblioteczny.Models
{
    public class AuthorsEvenings
    {
        public bool CheckIfLibraryExist(int id)
        {

            List<string> lines = new();
            String path = System.IO.Path.Combine("../../../DataBases/Libraries.txt");
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
                if (int.Parse(splitted[0]) == id) return true;
            }
            return false;
        }

        public void Add(AuthorsEvening newAuthorsEvening)
        {
            string path = System.IO.Path.Combine("../../../DataBases/AuthorsEveningList.txt");
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
            using (StreamWriter writer = new StreamWriter(path))
            {

                foreach (string line in lines)
                {
                    writer.WriteLine(line);
                }
                string text = newAuthorsEvening.Date.Value.ToString();
                string[] splitted = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                writer.WriteLine(newAuthorsEvening.FirstName + " " + newAuthorsEvening.LastName 
                    + " " + newAuthorsEvening.LibraryID + " " + splitted + " " 
                    + " " + newAuthorsEvening.Hour + " " + newAuthorsEvening.Duration + " "
                    + " " + newAuthorsEvening.PhoneNumber);

                writer.Close();
            }
        }
    }
}
