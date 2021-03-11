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
    [AdministratorsAuthentication]
    public class AdministratorController : ApiController
    {
        [Route("Admin/CreateNewAirlineCompany")]
        [HttpPost]
        public IHttpActionResult CreateNewAirlineCompany([FromBody]HTTPAirlineCompany airlineCompany)
        {
            if (airlineCompany.AirlineName == null | airlineCompany.Username == null | airlineCompany.Password == null | airlineCompany.CountryID <=0)
            {
                return BadRequest();
            }
            LoginToken<Administrator> token = (LoginToken<Administrator>)Request.Properties["User"];
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            try
            {
                facade.CreateNewAirline(token, new AirlineCompany(-1, airlineCompany.AirlineName, airlineCompany.Username, airlineCompany.Password, airlineCompany.CountryID));
            }
            catch (UsernameAlreadyExistsException)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Ok();
        }

        [Route("Admin/UpdateAirlineCompany/{airlineCompanyID}")]
        [HttpPut]
        public IHttpActionResult UpdateAirlineCompany(int airlineCompanyID, [FromBody]HTTPAirlineCompany airlineCompany)
        {
            if (airlineCompany.AirlineName == null & airlineCompany.Username == null & airlineCompany.Password == null & airlineCompany.CountryID <= 0)
            {
                return BadRequest();
            }
            LoginToken<Administrator> token = (LoginToken<Administrator>)Request.Properties["User"];
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            AirlineCompany original = facade._airlineDAO.Get(airlineCompanyID);
            HTTPAirlineCompany tempAirline = new HTTPAirlineCompany(original);
            if (airlineCompany.AirlineName != null)
            {
                tempAirline.AirlineName = airlineCompany.AirlineName;
            }
            if (airlineCompany.Username != null)
            {
                tempAirline.Username = airlineCompany.Username;
            }
            if (airlineCompany.Password != null)
            {
                tempAirline.Password = airlineCompany.Password;
            }
            if (airlineCompany.CountryID > 0)
            {
                tempAirline.CountryID = airlineCompany.CountryID;
            }
            try
            {
                facade.UpdateAirlineDetails(token, new AirlineCompany(airlineCompanyID, tempAirline.AirlineName, tempAirline.Username, tempAirline.Password, tempAirline.CountryID));
            }
            catch (UsernameAlreadyExistsException)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Ok();
        }

        [Route("Admin/DeleteAirlineCompany/{airlineCompanyID}")]
        [HttpDelete]
        public IHttpActionResult DeleteAirlineCompany([FromUri]int airlineCompanyID)
        {
            LoginToken<Administrator> token = (LoginToken<Administrator>)Request.Properties["User"];
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            AirlineCompany airline = facade._airlineDAO.Get(airlineCompanyID);
            if(airline == null)
            {
                return BadRequest();
            }
            facade.RemoveAirline(token, airline);
            return Ok();

        }

        [Route("Admin/CreateNewCustomer")]
        [HttpPost]
        public IHttpActionResult CreateNewCustomer([FromBody]HTTPCustomer customer)
        {
            if (customer.FirstName == null | customer.LastName == null | customer.Username == null |
                customer.Password == null | customer.Address == null | customer.PhoneNo == null |
                customer.CreditCardNumber == null)
            {
                return BadRequest();
            }
            LoginToken<Administrator> token = (LoginToken<Administrator>)Request.Properties["User"];
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            try
            {
                facade.CreateNewCustomer(token, new Customer(
                    -1, customer.FirstName, customer.LastName, customer.Username, customer.Password,
                    customer.Address, customer.PhoneNo, customer.CreditCardNumber));
            }
            catch (UsernameAlreadyExistsException)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Ok();

        }

        [Route("Admin/UpdateCustomer/{customerID}")]
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int customerID, [FromBody]HTTPCustomer customer)
        {
            if (customer.FirstName == null & customer.LastName == null & customer.Username == null &
                customer.Password == null & customer.Address == null & customer.PhoneNo == null &
                customer.CreditCardNumber == null)
            {
                return BadRequest();
            }
            LoginToken<Administrator> token = (LoginToken<Administrator>)Request.Properties["User"];
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            Customer original = facade._customerDAO.Get(customerID);
            HTTPCustomer tempCustomer = new HTTPCustomer(original);
            if (customer.FirstName != null)
            {
                tempCustomer.FirstName = customer.FirstName;
            }
            if (customer.LastName != null)
            {
                tempCustomer.LastName = customer.LastName;
            }
            if (customer.Username != null)
            {
                tempCustomer.Username = customer.Username;
            }
            if (customer.Password != null)
            {
                tempCustomer.Password = customer.Password;
            }
            if (customer.Address != null)
            {
                tempCustomer.Address = customer.Address;
            }
            if (customer.PhoneNo != null)
            {
                tempCustomer.PhoneNo = customer.PhoneNo;
            }
            if (customer.CreditCardNumber != null)
            {
                tempCustomer.CreditCardNumber = customer.CreditCardNumber;
            }
            try
            {
                facade.UpdateCustomerDetails(token, new Customer(customerID, tempCustomer.FirstName, tempCustomer.LastName, tempCustomer.Username, tempCustomer.Password,
                    tempCustomer.Address, tempCustomer.PhoneNo, tempCustomer.CreditCardNumber));
            }
            catch (UsernameAlreadyExistsException)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }
            return Ok();
        }

        [Route("Admin/DeleteCustomer/{customerID}")]
        [HttpDelete]
        public IHttpActionResult DeleteCustomer([FromUri]int customerID)
        {
            LoginToken<Administrator> token = (LoginToken<Administrator>)Request.Properties["User"];
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)(FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(token));
            Customer customer = facade._customerDAO.Get(customerID);
            if (customer == null)
            {
                return BadRequest();
            }
            facade.RemoveCustomer(token, customer);
            return Ok();

        }
    }
}
