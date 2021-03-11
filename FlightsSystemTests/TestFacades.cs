using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightCenter;
using System.Configuration;
using FlightCenter.Facades;
using System.Collections.Generic;
using FlightCenter.DAOs;
using System.Data.SqlClient;
using System.Data;

namespace FlightsSystemTests
{
    public class TestFacades
    {
        public void ResetWhatIsNeeded()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"delete from {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
                using (SqlCommand cmd = new SqlCommand($"delete from {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Tickets", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
                using (SqlCommand cmd = new SqlCommand($"delete from {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}delete from Customers where FIRST_NAME = 'david'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
                using (SqlCommand cmd = new SqlCommand($"delete from {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}delete from Customers where FIRST_NAME = 'john'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }
        [TestMethod]
        public void TestChangePassword()
        {
            ResetWhatIsNeeded();
            LoginService ls = new LoginService();
            LoginToken<AirlineCompany> loginToken = new LoginToken<AirlineCompany>();
            ls.TryAirlineLogin("company", "company", out loginToken);
            LoggedInAirlineFacade loggedInAirlineFacade = new LoggedInAirlineFacade();

            loggedInAirlineFacade.ChangeMyPassword(loginToken, loginToken.User.Password, "hey");

            Assert.IsTrue(ls.TryAirlineLogin("company", "hey", out loginToken));

            loggedInAirlineFacade.ChangeMyPassword(loginToken, loginToken.User.Password, "company");

            Assert.IsTrue(ls.TryAirlineLogin("company", "company", out loginToken));
        }
        [TestMethod]
        public void CRUDAirlineCompanyAndAnonymousFacadeTests()
        {
            ResetWhatIsNeeded();
            LoginService ls = new LoginService();
            LoginToken<AirlineCompany> loginToken = new LoginToken<AirlineCompany>();
            ls.TryAirlineLogin("company", "company", out loginToken);
            LoggedInAirlineFacade loggedInAirlineFacade = new LoggedInAirlineFacade();
            AnonymousUserFacade anonymousUserFacade = new AnonymousUserFacade();

            //create flight test
            DateTime departureTime = DateTime.Now.AddDays(1);
            DateTime landingTime = departureTime.AddDays(1).AddHours(1);
            Flight f = new Flight(-1,loginToken.User.ID,1,3, departureTime, landingTime, 10,10);
            loggedInAirlineFacade.CreateFlight(loginToken, f);

            IList<Flight> flights = anonymousUserFacade.GetAllFlights();

            //anonymous - get all flights test
            Assert.IsTrue(flights.Count==1);
            f = flights[0];

            //anonymous - get flights vacancies
            Dictionary<Flight, int> dict = anonymousUserFacade.GetAllFlightsVacancy();
            int remainingTickets;
            Assert.IsTrue(dict.TryGetValue(f, out remainingTickets));
            Assert.AreEqual(10, remainingTickets);

            //anonymous - get flight by ID
            Assert.AreEqual(f,anonymousUserFacade.GetFlightById(f.ID));

            //anonymous - get flight by departure date
            Assert.IsTrue(checkForFlightInList(f, anonymousUserFacade.GetFlightsByDepatrureDate(f.DepartureTime)));

            //anonymous - get flight by landing date
            Assert.IsTrue(checkForFlightInList(f, anonymousUserFacade.GetFlightsByLandingDate(f.LandingTime)));

            //anonymous - get flight by destination country
            Assert.IsTrue(checkForFlightInList(f, anonymousUserFacade.GetFlightsByDestinationCountry(f.DestinationCountryID)));

            //anonymous - get flight by origin country
            Assert.IsTrue(checkForFlightInList(f, anonymousUserFacade.GetFlightsByOriginCountry(f.OriginCountryID)));

            //update flight
            Flight newFlight = new Flight(f.ID, f.AirlineCompanyID, f.OriginCountryID, f.DestinationCountryID, f.DepartureTime.AddMinutes(30), f.LandingTime.AddMinutes(30), f.RemainingTickets, f.MaxTickets);
            loggedInAirlineFacade.UpdateFlight(loginToken, newFlight);
            f = anonymousUserFacade.GetFlightById(newFlight.ID);
            Assert.IsTrue(f.DepartureTime.Equals(newFlight.DepartureTime) & f.DepartureTime.Equals(newFlight.DepartureTime));

            //cancel flight - test only the cancelling part, not the removing ticket part as there are
            //no tickets at this point
            loggedInAirlineFacade.CancelFlight(loginToken, newFlight);
            Assert.IsTrue(anonymousUserFacade.GetFlightById(newFlight.ID)==null);

        }
        private bool checkForFlightInList(Flight f, IList<Flight> flights)
        {
            ResetWhatIsNeeded();
            foreach (Flight flight in flights)
            {
                if (flight.Equals(f))
                {
                    return true;
                }
            }
            return false;
        }
        [TestMethod]
        public void TestGetAllAirlineCompanies()
        {
            ResetWhatIsNeeded();
            AnonymousUserFacade anonymousUserFacade = new AnonymousUserFacade();
            IList<AirlineCompany> airlineCompanies = anonymousUserFacade.GetAllAirlineCompanies();
            Assert.AreEqual(3, airlineCompanies.Count);
        }
        [TestMethod]
        public void TestModifyAirlineDetails()
        {
            ResetWhatIsNeeded();
            LoginService ls = new LoginService();
            LoginToken<AirlineCompany> loginToken = new LoginToken<AirlineCompany>();
            ls.TryAirlineLogin("wefly", "wefly", out loginToken);
            LoggedInAirlineFacade loggedInAirlineFacade = new LoggedInAirlineFacade();

            AirlineCompany newAirlineCompany = new AirlineCompany(loginToken.User.ID, loginToken.User.AirlineName, loginToken.User.Username, "youfly", loginToken.User.CountryID);
            loggedInAirlineFacade.MofidyAirlineDetails(loginToken, newAirlineCompany);

            Assert.IsTrue(ls.TryAirlineLogin("wefly", "youfly", out loginToken));

            newAirlineCompany = new AirlineCompany(loginToken.User.ID, loginToken.User.AirlineName, loginToken.User.Username, "wefly", loginToken.User.CountryID);
            loggedInAirlineFacade.MofidyAirlineDetails(loginToken, newAirlineCompany);
        }
        [TestMethod]
        public void TestGetAllTicketsOfAirlineAndCancelFlight()
        {
            ResetWhatIsNeeded();
            FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetFlyingCenterSystem();
            
            //sign in as a airline company
            LoginToken<AirlineCompany> companyLoginToken = flyingCenterSystem.AttemptLoginAirlineCompany("company", "company");
            LoggedInAirlineFacade loggedInAirlineFacade = (LoggedInAirlineFacade)flyingCenterSystem.GetFacade(companyLoginToken);

            //sign in as admin and add 2 customers
            LoginToken<Administrator> administratorLoginToken = flyingCenterSystem.AttemptLoginAdministrator("admin", "admin");
            LoggedInAdministratorFacade loggedInAdministratorFacade = (LoggedInAdministratorFacade)flyingCenterSystem.GetFacade(administratorLoginToken);
            loggedInAdministratorFacade.CreateNewCustomer(administratorLoginToken,
                new Customer(-1, "john", "bravo", "john", "john", "places", "number", "anotherNumber"));
            loggedInAdministratorFacade.CreateNewCustomer(administratorLoginToken,
                new Customer(-1, "david", "david", "david", "david", "somewhere", "longnumber", "longnumber"));

            //create 4 flights
            DateTime now = DateTime.Now;
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 2,
                now.AddHours(1), now.AddHours(2), 10, 10));
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 2,
                now.AddHours(2), now.AddHours(3), 10, 10));
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 2,
                now.AddHours(3), now.AddHours(4), 10, 10));
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 2,
                now.AddHours(4), now.AddHours(5), 10, 10));

            IList<Flight> flights = loggedInAirlineFacade.GetFlightsByDestinationCountry(2);

            //login as 2 customers
            LoginToken<Customer> customer1 = flyingCenterSystem.AttemptLoginCustomer("john", "john");
            LoginToken<Customer> customer2 = flyingCenterSystem.AttemptLoginCustomer("david", "david");

            LoggedInCustomerFacade loggedInCustomerFacade = (LoggedInCustomerFacade)flyingCenterSystem.GetFacade(customer1);

            //purchase tickets
            loggedInCustomerFacade.PurchaseTicket(customer1, flights[0]);
            loggedInCustomerFacade.PurchaseTicket(customer1, flights[1]);
            loggedInCustomerFacade.PurchaseTicket(customer1, flights[2]);
            loggedInCustomerFacade.PurchaseTicket(customer1, flights[3]);
            loggedInCustomerFacade.PurchaseTicket(customer2, flights[0]);
            loggedInCustomerFacade.PurchaseTicket(customer2, flights[1]);

            //check to see if the flights data has been updated
            Flight flight0 = loggedInCustomerFacade.GetFlightById(flights[0].ID);
            Flight flight1 = loggedInCustomerFacade.GetFlightById(flights[1].ID);
            Flight flight2 = loggedInCustomerFacade.GetFlightById(flights[2].ID);
            Flight flight3 = loggedInCustomerFacade.GetFlightById(flights[3].ID);

            Assert.AreEqual(8, flight0.RemainingTickets);
            Assert.AreEqual(8, flight1.RemainingTickets);
            Assert.AreEqual(9, flight2.RemainingTickets);
            Assert.AreEqual(9, flight3.RemainingTickets);

            //check to see if tickets table has been updated
            IList<Ticket> tickets = loggedInAirlineFacade.GetAllTickets(companyLoginToken);
            Assert.IsTrue(LookForTicket(tickets, customer1.User.ID, flight0.ID));
            Assert.IsTrue(LookForTicket(tickets, customer1.User.ID, flight1.ID));
            Assert.IsTrue(LookForTicket(tickets, customer1.User.ID, flight2.ID));
            Assert.IsTrue(LookForTicket(tickets, customer1.User.ID, flight3.ID));
            Assert.IsTrue(LookForTicket(tickets, customer2.User.ID, flight0.ID));
            Assert.IsTrue(LookForTicket(tickets, customer2.User.ID, flight1.ID));

            //remove the flights
            loggedInAirlineFacade.CancelFlight(companyLoginToken, flight0);
            loggedInAirlineFacade.CancelFlight(companyLoginToken, flight1);
            loggedInAirlineFacade.CancelFlight(companyLoginToken, flight2);
            loggedInAirlineFacade.CancelFlight(companyLoginToken, flight3);

            tickets = loggedInAirlineFacade.GetAllTickets(companyLoginToken);
            Assert.IsTrue(tickets.Count == 0);

            flights = loggedInAirlineFacade.GetAllFlights(companyLoginToken);
            Assert.IsTrue(flights.Count == 0);
        }
        public bool LookForTicket(IList<Ticket> tickets, int customerID, int flightID)
        {
            ResetWhatIsNeeded();
            foreach (Ticket ticket in tickets)
            {
                if(ticket.CustomerID == customerID & ticket.FlightID == flightID)
                {
                    return true;
                }
            }
            return false;
        }
        [TestMethod]
        public void TestCustomerFacade()
        {
            ResetWhatIsNeeded();
            FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetFlyingCenterSystem();

            //sign in as a airline company
            LoginToken<AirlineCompany> companyLoginToken = flyingCenterSystem.AttemptLoginAirlineCompany("company", "company");
            LoggedInAirlineFacade loggedInAirlineFacade = (LoggedInAirlineFacade)flyingCenterSystem.GetFacade(companyLoginToken);

            //create 4 flights
            DateTime now = DateTime.Now;
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 3,
                now.AddHours(1), now.AddHours(2), 10, 10));
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 3,
                now.AddHours(2), now.AddHours(3), 10, 10));
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 3,
                now.AddHours(3), now.AddHours(4), 10, 10));
            loggedInAirlineFacade.CreateFlight(companyLoginToken,
                new Flight(-1, companyLoginToken.User.ID, 1, 3,
                now.AddHours(4), now.AddHours(5), 10, 10));

            IList<Flight> flights = loggedInAirlineFacade.GetFlightsByDestinationCountry(3);

            //login as the customer
            LoginToken<Customer> customer = flyingCenterSystem.AttemptLoginCustomer("customer", "customer");
            LoggedInCustomerFacade loggedInCustomerFacade = (LoggedInCustomerFacade)flyingCenterSystem.GetFacade(customer);

            //buy a ticket for 4 flights
            loggedInCustomerFacade.PurchaseTicket(customer, flights[0]);
            loggedInCustomerFacade.PurchaseTicket(customer, flights[1]);
            loggedInCustomerFacade.PurchaseTicket(customer, flights[2]);
            loggedInCustomerFacade.PurchaseTicket(customer, flights[3]);

            TicketDAOMSSQL _ticketDAO = new TicketDAOMSSQL();
            IList<Ticket> tickets = _ticketDAO.GetAll();

            Assert.IsTrue(LookForTicket(tickets, customer.User.ID, flights[0].ID));
            Assert.IsTrue(LookForTicket(tickets, customer.User.ID, flights[1].ID));
            Assert.IsTrue(LookForTicket(tickets, customer.User.ID, flights[2].ID));
            Assert.IsTrue(LookForTicket(tickets, customer.User.ID, flights[3].ID));

            Assert.AreEqual(4, loggedInCustomerFacade.GetAllMyFlights(customer).Count);

            loggedInCustomerFacade.CancelTicket(customer, tickets[0]);
            loggedInCustomerFacade.CancelTicket(customer, tickets[1]);
            loggedInCustomerFacade.CancelTicket(customer, tickets[2]);
            loggedInCustomerFacade.CancelTicket(customer, tickets[3]);

            tickets = _ticketDAO.GetAll();
            Assert.IsTrue(tickets.Count == 0);

            flights = loggedInAirlineFacade.GetFlightsByDestinationCountry(3);

            Assert.IsTrue(flights[0].RemainingTickets == 10);
            Assert.IsTrue(flights[1].RemainingTickets == 10);
            Assert.IsTrue(flights[2].RemainingTickets == 10);
            Assert.IsTrue(flights[3].RemainingTickets == 10);


        }
    }
}
