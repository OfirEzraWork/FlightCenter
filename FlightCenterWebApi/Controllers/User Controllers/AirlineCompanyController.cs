using FlightCenter;
using FlightCenter.Facades;
using FlightCenterWebApi.Controllers.Json_Input_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FlightCenterWebApi.Controllers.User_Controllers
{
    [AirlineCompanyAuthentication]
    public class AirlineCompanyController : ApiController
    {
        [Route("Airline/GetAllFlights")]
        [HttpPost]
        public IHttpActionResult GetAllFlights()
        {
            LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)Request.Properties["User"];
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            return Ok(facade.GetAllFlights(token));
        }

        [Route("Airline/GetAllTickets")]
        [HttpPost]
        public IHttpActionResult GetAllTickets()
        {
            LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)Request.Properties["User"];
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            return Ok(facade.GetAllTickets(token));
        }

        [Route("Airline/CancelFlight/{flightID}")]
        [HttpDelete]
        public IHttpActionResult CancelFlight([FromUri]int flightID)
        {
            LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)Request.Properties["User"];
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            Flight flight = facade.GetFlightById(flightID);
            if (flight == null)
            {
                return BadRequest();
            }
            else if(flight.AirlineCompanyID != token.User.ID)
            {
                return Unauthorized();
            }
            facade.CancelFlight(token, flight);
            return Ok();

        }

        [Route("Airline/CreateFlight")]
        [HttpPost]
        public IHttpActionResult CreateFlight([FromBody]HTTPFlight flight)
        {
            LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)Request.Properties["User"];
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            if (!CheckNewFlightInput(flight, token, facade))
            {
                return BadRequest();
            }

            facade.CreateFlight(token, new Flight(-1,flight.AirlineCompanyID,flight.OriginCountryID,flight.DestinationCountryID,flight.DepartureTime,
                flight.LandingTime,flight.RemainingTickets,flight.MaxTickets));

            return Ok();
        }

        [Route("Airline/UpdateFlight/{flightID}")]
        [HttpPut]
        public IHttpActionResult UpdateAirlineCompany(int flightID, [FromBody]HTTPFlight flight)
        {
            if ((flight.AirlineCompanyID <= 0 & flight.OriginCountryID <= 0 & 
                flight.DestinationCountryID <= 0 & flight.DepartureTime == null &
                flight.LandingTime == null & flight.RemainingTickets < 0 &
                flight.MaxTickets <= 0))
            {
                return BadRequest();
            }

            LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)Request.Properties["User"];
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            Flight original = facade.GetFlightById(flightID);
            if (!CheckUpdateFlightInput(original, flight, token, facade))
            {
                return BadRequest();
            }

            HTTPFlight tempFlight = new HTTPFlight(original);
            if (flight.AirlineCompanyID > 0)
            {
                tempFlight.AirlineCompanyID = flight.AirlineCompanyID;
            }
            if (flight.OriginCountryID > 0)
            {
                tempFlight.OriginCountryID = flight.OriginCountryID;
            }
            if (flight.DestinationCountryID > 0)
            {
                tempFlight.DestinationCountryID = flight.DestinationCountryID;
            }
            if (flight.DepartureTime != DateTime.MinValue)
            {
                tempFlight.DepartureTime = flight.DepartureTime;
            }
            if (flight.LandingTime != DateTime.MinValue)
            {
                tempFlight.LandingTime = flight.LandingTime;
            }
            if (flight.RemainingTickets > 0)
            {
                tempFlight.RemainingTickets = flight.RemainingTickets;
            }
            if (flight.MaxTickets > 0)
            {
                tempFlight.MaxTickets = flight.MaxTickets;
            }

            facade.UpdateFlight(token, new Flight(flightID, tempFlight.AirlineCompanyID, tempFlight.OriginCountryID, tempFlight.DestinationCountryID,
                tempFlight.DepartureTime, tempFlight.LandingTime, tempFlight.RemainingTickets, tempFlight.MaxTickets));

            return Ok();
        }
        private bool CheckUpdateFlightInput(Flight original, HTTPFlight flight, LoginToken<AirlineCompany> token, LoggedInAirlineFacade facade)
        {
            //check airline company id
            if(flight.AirlineCompanyID != 0)
            {
                if (facade._airlineDAO.Get(flight.AirlineCompanyID) == null)
                {
                    return false;
                }
            }

            //check countries info
            if (flight.OriginCountryID > 0)
            {
                if (facade._countryDAO.Get(flight.OriginCountryID) == null)
                {
                    return false;
                }
            }
            if (flight.DestinationCountryID > 0)
            {
                if (facade._countryDAO.Get(flight.DestinationCountryID) == null)
                {
                    return false;
                }
            }

            //check times
            bool newDepTime = false;
            bool newLandTime = false;
            if(flight.DepartureTime != DateTime.MinValue)
            {
                newDepTime = true;
            }
            if(flight.LandingTime != DateTime.MinValue)
            {
                newLandTime = true;
            }

            if(newDepTime & newLandTime)
            {
                if (flight.DepartureTime <= DateTime.Now ||
                    flight.LandingTime <= DateTime.Now ||
                    flight.DepartureTime >= flight.LandingTime)
                {
                    return false;
                }
            }
            else if (newDepTime)
            {
                if(flight.DepartureTime<= DateTime.Now || flight.DepartureTime >= original.LandingTime)
                {
                    return false;
                }
            }
            else if (newLandTime)
            {
                if (flight.LandingTime <= DateTime.Now || original.DepartureTime >= flight.LandingTime)
                {
                    return false;
                }
            }

            //check tickets
            if (flight.MaxTickets < 0 ||
                flight.RemainingTickets < 0 ||
                flight.MaxTickets < flight.RemainingTickets)
            {
                return false;
            }

            return true;
        }

        private bool CheckNewFlightInput(HTTPFlight flight, LoginToken<AirlineCompany> token, LoggedInAirlineFacade facade)
        {
            //check airline company id
            if (flight.AirlineCompanyID <= 0 || token.User.ID != flight.AirlineCompanyID)
            {
                return false;
            }

            //check countries info
            if (facade._countryDAO.Get(flight.OriginCountryID) == null |
                facade._countryDAO.Get(flight.DestinationCountryID) == null)
            {
                return false;
            }

            //check times
            if (flight.DepartureTime <= DateTime.Now ||
                flight.LandingTime <= DateTime.Now ||
                flight.DepartureTime >= flight.LandingTime)
            {
                return false;
            }

            //check tickets
            if (flight.MaxTickets <= 0 ||
                flight.RemainingTickets < 0 ||
                flight.MaxTickets < flight.RemainingTickets)
            {
                return false;
            }

            return true;
        }


    }
}
