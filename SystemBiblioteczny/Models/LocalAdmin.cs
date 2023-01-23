﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
    public class LocalAdmin : Person
    {
        public int LibraryId { get; set; }
        public LocalAdmin(string UserName, string Password, string FirstName, string LastName, string Email, int LibraryId, string Phone = "0")
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Password = Password;
            this.UserName = UserName;
            this.Email = Email;
            this.LibraryId = LibraryId;
            this.Phone = Phone;
        }
        public LocalAdmin() { }
        public void ManageEvent() { }

        public void ExchangeBooks() { }
        public void CreateRaport() { }

        public void ManageAccounts() { }
    }
}
