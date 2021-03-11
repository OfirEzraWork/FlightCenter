using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightCenter;
using FlightCenter.POCO;
using System.Configuration;

namespace FlightsSystemTests
{
    [TestClass]
    public class LoginServiceTests
    {
        [TestMethod]
        public void TestAdminLogin()
        {
            LoginService ls = new LoginService();
            LoginToken<Administrator> loginToken = new LoginToken<Administrator>();
            ls.TryAdminLogin("admin", "admin", out loginToken);
            Assert.IsTrue(loginToken.User.Username == "admin" & loginToken.User.Password == "admin");
        }
        [TestMethod]
        public void TestCustomerLogin()
        {
            LoginService ls = new LoginService();
            LoginToken<Customer> loginToken = new LoginToken<Customer>();
            ls.TryCustomerLogin("customer", "customer", out loginToken);
            Assert.IsTrue(loginToken.User.Username == "customer" & loginToken.User.Password == "customer");
        }
        [TestMethod]
        public void TestAirlineCompanyLogin()
        {
            LoginService ls = new LoginService();
            LoginToken<AirlineCompany> loginToken = new LoginToken<AirlineCompany>();
            ls.TryAirlineLogin("company", "company", out loginToken);
            Assert.IsTrue(loginToken.User.Username == "company" & loginToken.User.Password == "company");
        }
    }
}
