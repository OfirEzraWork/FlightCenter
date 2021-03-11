using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.DAOs
{
    public interface ITicketsHistoryDAO
    {
        void RemoveAllByFlightID(int FlightID);
        IList<Ticket> GetTicketsHistoryByAirline(AirlineCompany airline);
    }
}
