using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.Facades
{
    public interface IAnonymousUserFacade
    {
        IList<Flight> GetAllFlights();
        IList<AirlineCompany> GetAllAirlineCompanies();
        Dictionary<Flight, int> GetAllFlightsVacancy();
        Flight GetFlightById(int id);
        IList<Flight> GetFlightsByOriginCountry(int countryCode);
        IList<Flight> GetFlightsByDestinationCountry(int countryCode);
        IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate);
        IList<Flight> GetFlightsByLandingDate(DateTime landingDate);
        IList<FlightWithNames> GetUpcomingDepatrures();
        IList<FlightWithNames> GetUpcomingDepatrures(string key, string value);
        IList<FlightWithNames> GetUpcomingLandings();
        IList<FlightWithNames> GetUpcomingLandings(string key, string value);
    }
}
