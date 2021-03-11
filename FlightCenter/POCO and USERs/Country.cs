using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter
{
    public class Country : IPoco
    {
        public int ID { get; private set; }
        public string CountryName { get; private set; }
        public Country()
        {
        }

        public Country(int iD, string countryName)
        {
            ID = iD;
            CountryName = countryName;
        }
        public override bool Equals(object obj)
        {
            Country country = obj as Country;
            if (country == null)
            {
                return false;
            }
            return this == country;
        }

        public override int GetHashCode()
        {
            return ID;
        }
        public static bool operator ==(Country a, Country b)
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
        public static bool operator !=(Country a, Country b)
        {
            return !(a == b);
        }
    }
}
