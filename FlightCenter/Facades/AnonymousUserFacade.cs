using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.Facades
{
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
    {
        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }

        public IList<Flight> GetAllFlights()
        {
            return _flightDAO.GetAll();
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightVacancy();
        }

        public Flight GetFlightById(int id)
        {
            return _flightDAO.GetFlightByID(id);
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightsByDepatrureDate(departureDate);
        }

        public IList<FlightWithNames> GetUpcomingDepatrures()
        {
            return _flightDAO.GetUpcomingDepartures();
        }
        public IList<FlightWithNames> GetUpcomingDepatrures(string key, string value)
        {
            return _flightDAO.GetUpcomingDepartures(key, value);
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryID)
        {
            return _flightDAO.GetFlightsByDestinationCountry(countryID);
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            return _flightDAO.GetFlightsByLandingDate(landingDate);
        }
        public IList<FlightWithNames> GetUpcomingLandings()
        {
            return _flightDAO.GetUpcomingLandings();
        }
        public IList<FlightWithNames> GetUpcomingLandings(string key, string value)
        {
            return _flightDAO.GetUpcomingLandings(key, value);
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryID)
        {
            return _flightDAO.GetFlightsByOriginCountry(countryID);
        }

    }
}
