using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class AuthorsEvening
    {
        String FirstName { get; set; }
        String LastName { get; set; }
        int LibraryID { get; set; }
        String Date { get; set; }
        int Duration { get; set; }
        String PhoneNumber { get; set; }
        public AuthorsEvening(string firstname, string lastName, int id, string date, int duration, String phoneNumber) {
            FirstName = firstname;
            LastName = lastName;
            LibraryID = id;
            Date = date;
            Duration = duration;
            PhoneNumber = phoneNumber;
        }
    }
}
