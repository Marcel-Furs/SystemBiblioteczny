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
                string text = newAuthorsEvening.Date.ToString();
                string[] splitted = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                writer.WriteLine(newAuthorsEvening.User + " " + newAuthorsEvening.FirstName + " " + newAuthorsEvening.LastName 
                    + " " + newAuthorsEvening.LibraryID + " " + splitted[0] + " " 
                    + " " + newAuthorsEvening.Hour + " " + newAuthorsEvening.PhoneNumber);

                writer.Close();
            }
        }
        public List<AuthorsEvening> GetEventList()
        {
            List<AuthorsEvening> list = new();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "../../../DataBases/AuthorsEveningList.txt");

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

                string username = splitted[0];
                string authorsName = splitted[1];
                string authorsLastname = splitted[2];
                int libraryID = int.Parse(splitted[3]);
                string newDate = splitted[4];
                int newHour = int.Parse(splitted[5]);
                string newPhoneNumber = splitted[6];
                DateTime? newDate1 = DateTime.Parse(newDate);

                AuthorsEvening newEvent = new(username, authorsName, authorsLastname, libraryID, newDate1, newHour, newPhoneNumber);

                list.Add(newEvent);

            }
            return list;
        }
    }
}
