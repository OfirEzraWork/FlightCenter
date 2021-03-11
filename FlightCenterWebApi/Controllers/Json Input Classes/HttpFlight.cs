using FlightCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightCenterWebApi.Controllers.Json_Input_Classes
{
    public class HTTPFlight
    {
        public int AirlineCompanyID { get; set; }
        public int OriginCountryID { get; set; }
        public int DestinationCountryID { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime LandingTime { get; set; }
        public int RemainingTickets { get; set; }
        public int MaxTickets { get; set; }

        public HTTPFlight()
        {

        }
        public HTTPFlight(Flight flight)
        {
            AirlineCompanyID = flight.AirlineCompanyID;
            OriginCountryID = flight.OriginCountryID;
            DestinationCountryID = flight.DestinationCountryID;
            DepartureTime = flight.DepartureTime;
            LandingTime = flight.LandingTime;
            MaxTickets = flight.MaxTickets;
            RemainingTickets = flight.RemainingTickets;
        }
    }
}