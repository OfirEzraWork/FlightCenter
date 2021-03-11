using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightCenter;
using FlightCenter.POCO;
using System.Configuration;
using FlightCenter.Facades;

namespace FlightsSystemTests
{
    [TestClass]
    public class TestAdministratorFacade
    {
        [TestMethod]
        public void AddAndRemoveNewCustomer()
        {
            //log in as admin
            LoginService ls = new LoginService();
            LoginToken<Administrator> loginToken = new LoginToken<Administrator>();
            ls.TryAdminLogin("admin", "admin", out loginToken);
            LoggedInAdministratorFacade administratorFacade = new LoggedInAdministratorFacade();

            //add a customer
            Customer c = new Customer(-1, "a", "a", "testCustomer", "testCustomer", "a", "a", "a");
            administratorFacade.CreateNewCustomer(loginToken, c);

            //login as that customer
            LoginToken<Customer> customerLoginToken = new LoginToken<Customer>();
            ls.TryCustomerLogin("testCustomer", "testCustomer", out customerLoginToken);

            //check if it worked
            Assert.AreEqual("testCustomer",customerLoginToken.User.Username);

            //remove that customer
            administratorFacade.RemoveCustomer(loginToken,customerLoginToken.User);

            //check if that worked
            Assert.IsTrue(!ls.TryCustomerLogin("testCustomer", "testCustomer", out customerLoginToken));
        }
        [TestMethod]
        public void AddAndRemoveNewAdministrator()
        {
            LoginService ls = new LoginService();
            LoginToken<Administrator> loginToken = new LoginToken<Administrator>();
            ls.TryAdminLogin("admin", "admin", out loginToken);
            LoggedInAdministratorFacade administratorFacade = new LoggedInAdministratorFacade();

            Administrator a = new Administrator(-1, "testAdministrator", "testAdministrator");
            administratorFacade.CreateNewAdministrator(loginToken, a);

            LoginToken<Administrator> adminLoginToken = new LoginToken<Administrator>();
            ls.TryAdminLogin("testAdministrator", "testAdministrator", out adminLoginToken);

            Assert.AreEqual("testAdministrator", adminLoginToken.User.Username);

            administratorFacade.RemoveAdministrator(loginToken, adminLoginToken.User);

            Assert.IsTrue(!ls.TryAdminLogin("testAdministrator", "testAdministrator", out adminLoginToken));

        }
        [TestMethod]
        public void AddAndRemoveNewAirlineCompany()
        {
            LoginService ls = new LoginService();
            LoginToken<Administrator> loginToken = new LoginToken<Administrator>();
            ls.TryAdminLogin("admin", "admin", out loginToken);
            LoggedInAdministratorFacade administratorFacade = new LoggedInAdministratorFacade();

            AirlineCompany ac = new AirlineCompany(-1, "a", "testAirlineCompany", "testAirlineCompany", 1);
            administratorFacade.CreateNewAirline(loginToken, ac);

            LoginToken<AirlineCompany> airlineLoginToken = new LoginToken<AirlineCompany>();
            ls.TryAirlineLogin("testAirlineCompany", "testAirlineCompany", out airlineLoginToken);

            Assert.AreEqual("testAirlineCompany", airlineLoginToken.User.Username);

            administratorFacade.RemoveAirline(loginToken, airlineLoginToken.User);

            Assert.IsTrue(!ls.TryAirlineLogin("testAirlineCompany", "testAirlineCompany", out airlineLoginToken));
        }
    }
}
