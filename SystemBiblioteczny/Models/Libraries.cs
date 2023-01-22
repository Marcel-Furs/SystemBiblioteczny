using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace SystemBiblioteczny.Models
{
     class Libraries
    {
        private List<Library> GetLibrariesList()
       {

          
            List<Library> list = new();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "../../../DataBases/Libraries.txt");


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
                     string newCity = splitted[1];
                     string newStreet = splitted[2];
                     string newLocal = splitted[3];

                  Library lib = new(newId, newCity, newStreet, newLocal);
               
                    list.Add(lib);
              
            }

          
            return list;

        }

        public void AddLibraryToDB(Library library)
        {
            string path = System.IO.Path.Combine("../../../DataBases/Libraries.txt");
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
                writer.WriteLine(library?.ID.ToString() + " " + library?.City + " " + library?.Street + " " + library?.Local);
                writer.Close();
            }
           
        }

        public int ReturnUniqueID()
        {
            List<Library> list = this.GetLibrariesList();
            int max = 0;
            foreach (Library l in list)
            {
                if (l.ID > max) max = l.ID;
            }
            return max+1;
        }

        public bool CheckIfCanAdd(string city, string street, string local)
        {
            List<Library> list = this.GetLibrariesList();
            foreach (Library l in list)
            {
                if (l.City == city && l.Street == street && l.Local == local) {
                    MessageBox.Show("Biblioteka o takich danych już istnieje!");
                    return false;
                }
            }
            if (city.Length < 3)
            {
                MessageBox.Show("Miasto musi mieć przynajmniej 3 znaki!");
                return false;
            }
            if (street.Length < 3)
            {
                MessageBox.Show("Ulica musi mieć przynajmniej 3 znaki!");
                return false;
            }
            if (local.Length < 1)
            {
                MessageBox.Show("Numer lokalu nie może być pusty!");
                return false;
            }
            return true;
        }

        internal List<Library> GetListOfLibraries()
        {
            AccountBase a = new();
            List<String> listOfString = a.GetListOfDataBaseLines("Libraries");
            List<Library> list = new();
            for (int i = 0; i < listOfString.Count; i++)
            {
                string line = listOfString[i];

                string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                int id = int.Parse(splitted[0]);
                string city = splitted[1];
                string street = splitted[2];
                string local = splitted[3];

                Library tmp = new(id, city, street, local);

                list.Add(tmp);

            }
            return list;
        }
    }
    }

