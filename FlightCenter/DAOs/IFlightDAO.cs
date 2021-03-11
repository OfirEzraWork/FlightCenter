using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.DAOs
{
    public interface IFlightDAO : IBasicDB<Flight>
    {
        Dictionary<Flight, int> GetAllFlightVacancy();
        Flight GetFlightByID(int ID);
        IList<Flight> GetFlightsByOriginCountry(int countryID);
        IList<Flight> GetFlightsByDestinationCountry(int countryID);
        IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate);
        IList<Flight> GetFlightsByLandingDate(DateTime landingDate);
        IList<Flight> GetFlightsByCustomer(Customer customer);
        IList<Flight> GetFlightsByAirline(AirlineCompany airline);
        void BuyTicket(Flight f);
        void CancelTicket(Ticket t);
    }
}
