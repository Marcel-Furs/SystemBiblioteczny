using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class Person
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public int? Email { get; set; }

        public int? Phone { get; set; }

        //login data
        public Person(string UserName =" ", String Password= " ") {
        this.UserName = UserName;
        this.Password = Password;
        }
    }
}
