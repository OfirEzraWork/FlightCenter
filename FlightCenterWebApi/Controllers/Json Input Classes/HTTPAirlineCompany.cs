using FlightCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightCenterWebApi.Controllers.Json_Input_Classes
{
    public class HTTPAirlineCompany
    {
        public string AirlineName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CountryID { get; set; }

        public HTTPAirlineCompany()
        {
        }
        public HTTPAirlineCompany(AirlineCompany airline)
        {
            AirlineName = airline.AirlineName;
            Username = airline.Username;
            Password = airline.Password;
            CountryID = airline.CountryID;
        }
    }
}