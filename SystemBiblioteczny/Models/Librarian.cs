﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class Librarian : Person
    {
        public int LibraryId { get; set; }
        
        public Librarian(string UserName, string Password, string FirstName, string LastName, string Email, int LibraryId, string Phone="0")
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Password = Password;
            this.UserName = UserName;
            this.Email = Email;
            this.LibraryId = LibraryId;
            this.Phone = Phone;
        }
        public void RentBook() { }

        public void EndRental() { }
    }
}
