using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.Facades
{
    public class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
    {
        public LoggedInCustomerFacade()
        {
        }
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            if (_ticketDAO.GetTicketByFlightAndCustomerID(ticket.FlightID, ticket.CustomerID) == null)
            {
                return;
            }
            //remove customer ticket
            _ticketDAO.Remove(ticket);

            //return back to the flight
            _flightDAO.CancelTicket(ticket);
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            return _flightDAO.GetFlightsByCustomer(token.User);
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            if (_ticketDAO.GetTicketByFlightAndCustomerID(flight.ID, token.User.ID) != null)
            {
                return null;
            }
            //reduce amount from flight
            _flightDAO.BuyTicket(flight);

            //add to customer
            Ticket t = new Ticket(-1, flight.ID, token.User.ID);
            _ticketDAO.Add(t);
            t = _ticketDAO.GetTicketByFlightAndCustomerID((int)flight.ID, (int)token.User.ID);

            return t;
        }
    }
}
