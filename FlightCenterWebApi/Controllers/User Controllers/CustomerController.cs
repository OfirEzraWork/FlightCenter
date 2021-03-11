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
    [CustomerAuthentication]
    public class CustomerController : ApiController
    {
        [Route("Customer/Flights")]
        [HttpGet]
        public IHttpActionResult GetMyFlights()
        {
            LoginToken<Customer> token = (LoginToken<Customer>)Request.Properties["User"];
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            IList<Flight> flights =  facade.GetAllMyFlights(token);
            if(flights.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(flights);
            }
        }

        [Route("Customer/PurchaseTicket/{flightID}")]
        [HttpPost]
        public IHttpActionResult PurchaseTicket([FromUri]int flightID)
        {
            if (flightID <= 0)
            {
                return BadRequest();
            }
            LoginToken<Customer> token = (LoginToken<Customer>)Request.Properties["User"];
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            Flight flight = facade.GetFlightById(flightID);
            if(flight == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            Ticket t = facade.PurchaseTicket(token ,flight);
            if(t == null)
            {
                return NotFound();
            }
            return Ok();

        }
        [Route("Customer/CancelTicket/{flightID}")]
        [HttpPut]
        public IHttpActionResult CancelTicket([FromUri]int flightID)
        {
            if (flightID <= 0)
            {
                return BadRequest();
            }
            LoginToken<Customer> token = (LoginToken<Customer>)Request.Properties["User"];
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            Ticket t = facade._ticketDAO.GetTicketByFlightAndCustomerID(flightID, token.User.ID);
            if (t == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            facade.CancelTicket(token, t);
            return Ok();
        }
    }
}
