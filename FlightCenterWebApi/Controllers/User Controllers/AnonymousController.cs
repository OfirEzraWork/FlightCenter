using FlightCenter;
using FlightCenter.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FlightCenterWebApi.Controllers.User_Controllers
{
    public class AnonymousController : ApiController
    {
        [Route("User/Flights")]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<Flight> flights = facade.GetAllFlights();
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Ok(flights);
            }
        }
        [Route("User/AirlineCompanies")]
        [HttpGet]
        public IHttpActionResult GetAllAirlineCompanies()
        {
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<AirlineCompany> airlineCompanies = facade.GetAllAirlineCompanies();
            if (airlineCompanies.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Ok(airlineCompanies);
            }
        }
        [Route("User/FlightsVacancies")]
        [HttpGet]
        public IHttpActionResult GetAllFlightsVacancies()
        {
            AnonymousUserFacade facade = new AnonymousUserFacade();
            Dictionary<Flight,int> vacancies = facade.GetAllFlightsVacancy();
            if (vacancies.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                //converting from Flight,int dictionary to int,int dictionary
                Dictionary<int, int> toReturn = new Dictionary<int, int>();
                foreach(KeyValuePair<Flight,int> pair in vacancies)
                {
                    toReturn.Add(pair.Key.ID, pair.Value);
                }
                return Ok(toReturn);
            }
        }
        [Route("User/FlightByID/{flightID}")]
        [HttpGet]
        public IHttpActionResult GetFlightByID([FromUri]int flightID)
        {
            if (flightID <= 0)
            {
                return BadRequest();
            }
            AnonymousUserFacade facade = new AnonymousUserFacade();
            Flight flight = facade.GetFlightById(flightID);
            if (flight == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flight);

        }
        [Route("User/FlightsByOriginCountry/{originCountryID}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByOriginCountry([FromUri]int originCountryID)
        {
            if (originCountryID <= 0)
            {
                return BadRequest();
            }
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<Flight> flights = facade.GetFlightsByOriginCountry(originCountryID);
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
        [Route("User/FlightsByDestinationCountry/{destinationCountryID}")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDestinationCountry([FromUri]int destinationCountryID)
        {
            if (destinationCountryID <= 0)
            {
                return BadRequest();
            }
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<Flight> flights = facade.GetFlightsByDestinationCountry(destinationCountryID);
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
        [Route("User/FlightsByDepartureDate")]
        [HttpGet]
        public IHttpActionResult GetFlightsByDepartureDate(DateTime departureDate)
        {
            if (departureDate <= DateTime.Now)
            {
                return BadRequest();
            }
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<Flight> flights = facade.GetFlightsByDepatrureDate(departureDate);
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
        [Route("User/UpcomingDepartures")]
        [HttpGet]
        public IHttpActionResult GetUpcomingDepartures()
        {
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<FlightWithNames> flights = facade.GetUpcomingDepatrures();
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
        [Route("User/SearchUpcomingDepartures/params")]
        [HttpGet]
        public IHttpActionResult GetUpcomingDepartures([FromUri]string key, [FromUri]string value)
        {
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<FlightWithNames> flights = facade.GetUpcomingDepatrures(key,value);
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
        [Route("User/FlightsByLandingDate")]
        [HttpGet]
        public IHttpActionResult GetFlightsByLandingDate(DateTime landingDate)
        {
            if (landingDate <= DateTime.Now)
            {
                return BadRequest();
            }
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<Flight> flights = facade.GetFlightsByLandingDate(landingDate);
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
        [Route("User/UpcomingArrivals")]
        [HttpGet]
        public IHttpActionResult GetUpcomingLandings()
        {
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<FlightWithNames> flights = facade.GetUpcomingLandings();
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
        [Route("User/SearchUpcomingArrivals/params")]
        [HttpGet]
        public IHttpActionResult GetUpcomingLandings([FromUri]string key, [FromUri]string value)
        {
            AnonymousUserFacade facade = new AnonymousUserFacade();
            IList<FlightWithNames> flights = facade.GetUpcomingLandings(key, value);
            if (flights.Count == 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(flights);

        }
    }
}
