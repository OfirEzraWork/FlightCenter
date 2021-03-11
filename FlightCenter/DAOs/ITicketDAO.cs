using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.DAOs
{
    public interface ITicketDAO : IBasicDB<Ticket>
    {
        void RemoveAllByFlightID(int FlightID);
        IList<Ticket> GetTicketsByAirline(AirlineCompany airline);
        IList<Ticket> GetTicketsByFlight(Flight FlightID);
        Ticket GetTicketByFlightAndCustomerID(int flightID, int CustomerID);
        void RemoveAllByCustomerID(int CustomerID);
    }
}
