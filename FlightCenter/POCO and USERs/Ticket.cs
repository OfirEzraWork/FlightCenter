using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter
{
    public class Ticket : IPoco
    {
        public int ID { get; private set; }
        public int FlightID { get; private set; }
        public int CustomerID { get; private set; }
        public Ticket()
        {
        }

        public Ticket(int iD, int flightID, int customerID)
        {
            ID = iD;
            FlightID = flightID;
            CustomerID = customerID;
        }

        public override bool Equals(object obj)
        {
            Ticket ticket = obj as Ticket;
            if (ticket == null)
            {
                return false;
            }
            return this == ticket;
        }

        public override int GetHashCode()
        {
            return ID;
        }
        public static bool operator ==(Ticket a, Ticket b)
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
        public static bool operator !=(Ticket a, Ticket b)
        {
            return !(a == b);
        }
    }
}
