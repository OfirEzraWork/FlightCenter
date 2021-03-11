using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter
{
    public class FlightWithNames : IPoco
    {
        public int ID { get; private set; }
        public string AirlineCompanyName { get; private set; }
        public string OriginCountryName { get; private set; }
        public string DestinationCountryName { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime LandingTime { get; private set; }
        public FlightWithNames()
        {

        }

        public FlightWithNames(int iD, string airlineCompanyName, string originCountryName, string destinationCountryName, DateTime departureTime, DateTime landingTime)
        {
            ID = iD;
            AirlineCompanyName = airlineCompanyName;
            OriginCountryName = originCountryName;
            DestinationCountryName = destinationCountryName;
            DepartureTime = departureTime;
            LandingTime = landingTime;
        }
    }
}
