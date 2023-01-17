using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace SystemBiblioteczny.Models
{
     class Libraries
    {


        public List<Library> GetLibrariesList()
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
                     string newName = splitted[1];
                     LocalAdmin newAdmin = new(splitted[2]);
                     string newAddress = splitted[3];

                  Library lib = new(newName, newAdmin, newAddress, newId);
               
                    list.Add(lib);
              
            }

          
            return list;

        }

        public async Task AddLibraryToDB(Library library)
        {
            //brak testowania
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"SystemBiblioteczny\DataBases\Libraries.txt");
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                await File.WriteAllTextAsync("Libraries.txt", line + "\n");
            }

            await File.WriteAllTextAsync("Libraries.txt", library?.Id.ToString() + " " + library?.Name + " " + library?.Admin?.UserName + " " + library?.Address);

            System.Console.ReadKey();
        }
       

    }
    }

