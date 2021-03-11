using FlightCenter.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.Facades
{
    public abstract class FacadeBase
    {
        public IAirlineDAO _airlineDAO = new AirlineDAOMSSQL();
        public ICountryDAO _countryDAO = new CountryDAOMSSQL();
        public ICustomerDAO _customerDAO = new CustomerDAOMSSQL();
        public IFlightDAO _flightDAO = new FlightDAOMSSQL();
        public ITicketDAO _ticketDAO = new TicketDAOMSSQL();
    }
}
