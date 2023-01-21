using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace SystemBiblioteczny.Models
{
    public class AuthorsEvening
    {
        String FirstName { get; set; }
        String LastName { get; set; }
        int LibraryID { get; set; }
        DateTime? Date { get; set; }
        int Hour { get; set; }
        int Duration { get; set; }
        String PhoneNumber { get; set; }
        public AuthorsEvening(string firstname, string lastName, int id, DateTime? date, int hour, int duration, String phoneNumber) {
            FirstName = firstname;
            LastName = lastName;
            LibraryID = id;
            Date = date;
            Hour = hour;
            Duration = duration;
            PhoneNumber = phoneNumber;
        }

        public bool CheckIfCanAddToDataBase()
        {
            if (FirstName.Length < 2) {
                MessageBox.Show("Imię autora powinno mieć przynajmniej 2 znaki!");
                return false;
            }
            if (LastName.Length < 2)
            {
                MessageBox.Show("Nazwisko autora powinno mieć przynajmniej 2 znaki!");
                return false;
            }
            AuthorsEvenings a = new();
            if (!a.CheckIfLibraryExist(LibraryID)) {
                MessageBox.Show("Biblioteka o takim numerze nie istnieje!");
                return false;
            }
            if (Date == null)
            {
                MessageBox.Show("Wybierz datę!");
                return false;
            }
            if (Hour > 22 || Hour < 8)
            {
                MessageBox.Show("Podaj pełną godzinę od 8 do 22!");
                return false;
            }
            if (Duration < 1 || Duration > 300) 
            {
                MessageBox.Show("Ustaw czas trwania jako liczbę z przedziału od 1 do 300!");
                return false;
            }
            if (!Regex.Match(PhoneNumber, "^\\d{9}$").Success)
            {
                MessageBox.Show("Podaj numer telefonu jako 9 cyfr!");
                return false;
            }
            a.Add(this);
            return true;
        }
    }
}
