using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter
{
    public class AirlineCompany : IPoco, IUser
    {
        public int ID { get; private set; }
        public string AirlineName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int CountryID { get; private set; }

        public AirlineCompany()
        {

        }

        public AirlineCompany(int iD, string airlineName, string username, string password, int countryID)
        {
            ID = iD;
            AirlineName = airlineName;
            Username = username;
            Password = password;
            CountryID = countryID;
        }

        public override bool Equals(object obj)
        {
            AirlineCompany airlineCompany = obj as AirlineCompany;
            if(airlineCompany == null)
            {
                return false;
            }
            return this == airlineCompany;
        }

        public override int GetHashCode()
        {
            return ID;
        }
        public static bool operator ==(AirlineCompany a,AirlineCompany b)
        {
            if(ReferenceEquals(a, null) & ReferenceEquals(b, null))
            {
                return true;
            }
            else if(ReferenceEquals(a, null) | ReferenceEquals(b, null))
            {
                return false;
            }
            else
            {
                return a.ID == b.ID;
            }
        }
        public static bool operator !=(AirlineCompany a, AirlineCompany b)
        {
            return !(a == b);
        }
    }
}
