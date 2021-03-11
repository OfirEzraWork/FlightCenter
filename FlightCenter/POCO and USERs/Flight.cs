using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter
{
    public class Flight : IPoco
    {

        public int ID { get; private set; }
        public int AirlineCompanyID { get; private set; }
        public int OriginCountryID { get; private set; }
        public int DestinationCountryID { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime LandingTime { get; private set; }
        public int RemainingTickets { get; private set; }
        public int MaxTickets { get; private set; }
        public Flight()
        {

        }

        public Flight(int iD, int airlineCompanyID, int originCountryID, int destinationCountryID, DateTime departureTime, DateTime landingTime, int remainingTickets, int maxTickets)
        {
            ID = iD;
            AirlineCompanyID = airlineCompanyID;
            OriginCountryID = originCountryID;
            DestinationCountryID = destinationCountryID;
            DepartureTime = departureTime;
            LandingTime = landingTime;
            RemainingTickets = remainingTickets;
            MaxTickets = maxTickets;
        }

        public override bool Equals(object obj)
        {
            Flight flight = obj as Flight;
            if (flight == null)
            {
                return false;
            }
            return this == flight;
        }

        public override int GetHashCode()
        {
            return ID;
        }
        public static bool operator ==(Flight a, Flight b)
        {
            if (ReferenceEquals(a, null) & ReferenceEquals(b, null))
            {
                return true;
            }
            else if (ReferenceEquals(a, null) | ReferenceEquals(b, null))
            {
                return false;
            }
            else
            {
                return a.ID == b.ID;
            }
        }
        public static bool operator !=(Flight a, Flight b)
        {
            return !(a == b);
        }
    }
}
