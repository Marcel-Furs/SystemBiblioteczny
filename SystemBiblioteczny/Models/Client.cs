using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     public class Client : Person
    {
        public string? Statistics { get; set; }

        public Client(string userName, string password, string firstName, string lastName, string email, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            UserName = userName;
            Email = email;
            Phone = phone;
        }
        public Client() { }

        public Client(string? userName)
        {
            UserName = userName;
        }

        public void MakeReservation() { }

        public void ShowStats() { }
        public void ProposeEvent() { }

    }
}
