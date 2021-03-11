using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.Facades
{
    public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        public LoggedInAirlineFacade()
        {

        }
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            //This method assumes that the flight BELONGS to the airline in the token

            //remove all tickets to the flight
            _ticketDAO.RemoveAllByFlightID(flight.ID);

            //remove the flight
            _flightDAO.Remove(flight);
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            if (!token.User.Password.Equals(newPassword))
            {
                AirlineCompany airlineCompany = new AirlineCompany(
                    token.User.ID, token.User.AirlineName, 
                    token.User.Username, newPassword, 
                    token.User.CountryID);
                token.User = airlineCompany;
                _airlineDAO.Update(token.User);
            }
        }
        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            _flightDAO.Add(flight);
        }
        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            return _flightDAO.GetFlightsByAirline(token.User);
        }
        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            return _ticketDAO.GetTicketsByAirline(token.User);
        }
        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            _airlineDAO.Update(airline);
        }
        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            _flightDAO.Update(flight);
        }
    }
}
