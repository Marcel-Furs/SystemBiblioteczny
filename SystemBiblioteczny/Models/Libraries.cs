using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemBiblioteczny.Models
{
    internal class Libraries
    {
       
       public List<Library> GetLibrariesList()
       {
            List<Library> list = new List<Library>();
            try {

                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DataBases\Libraries.txt");
                // string path = Path.Combine(Environment.CurrentDirectory, @"Libraries.txt");
                // "C:\Users\Lenovo\Documents\IOProjekt\SystemBiblioteczny\SystemBiblioteczny\DataBases\Libraries.txt"
                Console.WriteLine(path);
                string[] lines = File.ReadAllLines(path);
                MessageBox.Show(lines.Length.ToString());
                foreach (string line in lines)
                {
                    Console.WriteLine("\t" + line);
                    string[] splitted = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    int newId = int.Parse(splitted[0]);
                    LocalAdmin newAdmin = new LocalAdmin(splitted[2]);
                    list.Add(new Library(splitted[1], newAdmin, splitted[3], newId));
                }


                System.Console.ReadKey();

               


            } catch (Exception e) { MessageBox.Show(e.ToString()); }

            return list;

        }

        public async Task AddLibraryToDB(Library library)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DataBases\Libraries.txt");
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                Console.WriteLine("\t" + line);
                await File.WriteAllTextAsync("Libraries.txt", line);
            }

            await File.WriteAllTextAsync("Libraries.txt", library?.Id.ToString() + " " + library?.Name + " " + library?.Admin?.UserName + " " + library?.Address);

            System.Console.ReadKey();
        }
       

    }
    }

