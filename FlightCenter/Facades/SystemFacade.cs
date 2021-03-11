using FlightCenter.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.Facades
{
    class SystemFacade : FacadeBase, ISystemFacade
    {
        private TicketsHistoryDAOMSSQL _ticketsHistoryDAO = new TicketsHistoryDAOMSSQL();
        private FlightsHistoryDAOMSSQL _flightsHistoryDAO = new FlightsHistoryDAOMSSQL();
        public void MoveExpiredObjects()
        {
            IList<Flight> flights = _flightDAO.GetAll();
            IList<Flight> expired = new List<Flight>();
            foreach(Flight flight in flights)
            {
                if (DateTime.Now.AddHours(-3).CompareTo(flight.LandingTime) > 0)
                {
                    expired.Add(flight);
                }
            }
            MoveExpiredTickets(expired);
            foreach (Flight flight in expired)
            {
                _flightsHistoryDAO.Add(flight);
                _flightDAO.Remove(flight);
            }
        }

        private void MoveExpiredTickets(IList<Flight> flights)
        {
            foreach(Flight flight in flights)
            {
                int flightID = flight.ID;
                IList<Ticket> tickets = _ticketDAO.GetTicketsByFlight(flight);
                foreach(Ticket ticket in tickets)
                {
                    _ticketsHistoryDAO.Add(ticket);
                }
                _ticketDAO.RemoveAllByFlightID(flightID);
            }
        }
    }
}
