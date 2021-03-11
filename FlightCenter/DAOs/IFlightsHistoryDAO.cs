using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.DAOs
{
    public interface IFlightsHistoryDAO
    {
        Flight GetFlightByID(int ID);
        IList<Flight> GetFlightsHistoryByOriginCountry(int countryID);
        IList<Flight> GetFlightsHistoryByDestinationCountry(int countryID);
        IList<Flight> GetFlightsHistoryByDepatrureDate(DateTime departureDate);
        IList<Flight> GetFlightsHistoryByLandingDate(DateTime landingDate);
        IList<Flight> GetFlightsHistoryByCustomer(Customer customer);
        IList<Flight> GetFlightsHistoryByAirline(AirlineCompany airline);
    }
}
