using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Updaters
{
    public class ClientUpdater
    {
        public static void Update(Client source, Client targer)
        {
            targer.Name = source.Name;
            targer.Address = source.Address;
            targer.City = source.City;
            targer.PostalCode = source.PostalCode;
            targer.Country = source.Country;
        }
    }
}
