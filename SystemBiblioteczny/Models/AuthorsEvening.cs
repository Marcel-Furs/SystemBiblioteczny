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
        DateTime? Date { get; set; }
        String Hour { get; set; }
        int Duration { get; set; }
        String PhoneNumber { get; set; }
        public AuthorsEvening(string firstname, string lastName, int id, DateTime? date, String Hour, int duration, String phoneNumber) {
            FirstName = firstname;
            LastName = lastName;
            LibraryID = id;
            Date = date;
            Duration = duration;
            PhoneNumber = phoneNumber;
        }
    }
}
