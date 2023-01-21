using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SystemBiblioteczny
{
    /// <summary>
    /// Logika interakcji dla klasy LibrarianWindow.xaml
    /// </summary>
    public partial class LibrarianWindow : Window
    {
        public LibrarianWindow()
        {
            InitializeComponent();
            base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            MainWindow m = new();
            m.Show();
            this.Close();
        }

        private void SendApplication(object sender, RoutedEventArgs e)
        {
            var title = TitleInput.Text;
            var authorsName = NameInput.Text;
            var authorsSurname = SurnameInput.Text;
            var quantity = QuantityInput.Text;

            

            

            if (title.Any() && authorsName.Any() && authorsSurname.Any() && quantity.Any())
            {
                MessageBox.Show("Wysłano zgłoszenie zapotrzebowania na: " + "\n" + authorsName + " " + authorsSurname + " " + title + " - ilość: " + quantity);



                string path = System.IO.Path.Combine("../../../DataBases/BookApplicationList.txt");
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
                    writer.WriteLine(NameInput.Text + " " + SurnameInput.Text + " " + TitleInput.Text + " " + QuantityInput.Text);
                    writer.Close();
                }
            }
            else
            {
                MessageBox.Show("Niewystarczająca ilość danych, uzupełnij wszystkie dane!");
            }
        }

    }
}