using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class Client : Person
    {
        public string? Statistics { get; set; }

        public void MakeReservation() { }

        public void ShowStats() { }
        public void ProposeEvent() { }

    }
}
