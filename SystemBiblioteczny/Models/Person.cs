using System;

namespace SystemBiblioteczny.Models
{
    public class Person
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        //login data
        public Person(string UserName = " ", String Password = " ")
        {
            this.UserName = UserName;
            this.Password = Password;
        }
    }
}
