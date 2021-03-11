using FlightCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightCenterWebApi.Controllers.Json_Input_Classes
{
    public class HTTPCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string CreditCardNumber { get; set; }

        public HTTPCustomer()
        {
        }
        public HTTPCustomer(Customer customer)
        {
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Username = customer.Username;
            Password = customer.Password;
            Address = customer.Address;
            PhoneNo = customer.PhoneNo;
            CreditCardNumber = customer.CreditCardNumber;
        }
    }
}