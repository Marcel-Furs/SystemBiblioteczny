using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBiblioteczny.Models
{
     class Statistics
    {
        //rentals = Clients
        // asdasdasdasdasdsadasd
        public int NumberOfClients { get; set; }
        public int DaysOfClients { get; set;}

        public List<Client> ListOfCurrentClients = new();

        public double MoneySpent { get; set; }

    }
}
