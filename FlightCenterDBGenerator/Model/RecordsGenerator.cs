using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlightCenter;
using FlightCenter.Facades;

namespace FlightCenterDBGenerator
{
    class RecordsGenerator
    {

        private int AmountOfUsersNeeded { get; set; }
        private int AirlineCompaniesAmount { get; set; }
        private int CustomersAmount { get; set; }
        private int AdministratorsAmount { get; set; }
        private int FlightsPerCompanyAmount { get; set; }
        private int TicketsPerCustomerAmount { get; set; }
        private int CountriesAmount { get; set; }
        private List<RandomUser> RandomUsers { get; set; }
        private List<CountriesApi> RandomCountries { get; set; }

        public RecordsGenerator(int airlineCompaniesAmount,int customersAmount
            ,int administratorsAmount, int flightsPerCompanyAmount,
            int ticketsPerCustomerAmount, int countriesAmount)
        {
            AirlineCompaniesAmount = airlineCompaniesAmount;
            CustomersAmount = customersAmount;
            AdministratorsAmount = administratorsAmount;
            FlightsPerCompanyAmount = flightsPerCompanyAmount;
            TicketsPerCustomerAmount = ticketsPerCustomerAmount;
            CountriesAmount = countriesAmount;
        }
        /// <summary>
        /// The main method that generates the records
        /// </summary>
        /// <param name="vm"></param>
        public async void GenerateRecords(ViewModel vm)
        {
            vm.IsIdle = false;
            await Task.Delay(500);
            //Get Admin Privileges
            FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetFlyingCenterSystem();
            LoginToken<Administrator> loginToken = FlyingCenterSystem.GetFlyingCenterSystem()
                .AttemptLoginAdministrator("admin","admin");
            if (loginToken == null)
            {
                MessageBox.Show($"Error gaining admin privileges", "Privilege Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(loginToken);

            //Get random users from a web service
            AmountOfUsersNeeded = AirlineCompaniesAmount + CustomersAmount + AdministratorsAmount;
            GetRandomUsers();

            //Generate Admins
            GenerateAdmins(facade, loginToken);
            vm.ProgressBarValue++;
            vm.StatusText += "\nFinished Generating Admins";
            await Task.Delay(500);

            //Get a list of countries from a web service
            GetCountries();

            //Generate Countries
            GenerateCountries(facade, loginToken);
            vm.ProgressBarValue++;
            vm.StatusText += "\nFinished Generating Countries";
            await Task.Delay(500);

            //Generate Customers
            GenerateCostumers(facade, loginToken);
            vm.ProgressBarValue++;
            vm.StatusText += "\nFinished Generating Customers";
            await Task.Delay(500);

            //Generate AirlineCompanies
            GenerateAirlineCompanies(facade, loginToken);
            vm.ProgressBarValue++;
            vm.StatusText += "\nFinished Generating Airline Companies";
            await Task.Delay(500);

            //Generate Flights
            GenerateFlights(facade, loginToken, flyingCenterSystem);
            vm.ProgressBarValue++;
            vm.StatusText += "\nFinished Generating Flights";
            await Task.Delay(500);

            //Generate Tickets
            GenerateTickets(facade, loginToken, flyingCenterSystem);
            vm.ProgressBarValue++;
            vm.StatusText += "\nFinished Generating Tickets";
            await Task.Delay(500);

            vm.StatusText += "\nDatabase is Populated";

            vm.IsIdle = true;
            return;
        }
        /// <summary>
        /// The main method that clears the database
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="loginToken"></param>
        public static void ClearAll(ViewModel vm)
        {
            vm.IsIdle = false;

            //Get Admin Privileges
            FlyingCenterSystem flyingCenterSystem = FlyingCenterSystem.GetFlyingCenterSystem();
            LoginToken<Administrator> loginToken = FlyingCenterSystem.GetFlyingCenterSystem()
                .AttemptLoginAdministrator("admin", "admin");
            if (loginToken == null)
            {
                MessageBox.Show($"Error gaining admin privileges", "Privilege Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(loginToken);
            facade.RemoveAllAdmins(loginToken);
            facade._ticketDAO.RemoveAll();
            facade._flightDAO.RemoveAll();
            facade._customerDAO.RemoveAll();
            facade._airlineDAO.RemoveAll();
            facade._countryDAO.RemoveAll();
            vm.StatusText += "\nDatabase cleared successfully";

            vm.IsIdle = true;
        }
        /// <summary>
        /// Connects to a web service that returns a list of users
        /// </summary>
        /// <returns></returns>
        private async void GetRandomUsers()
        {
            //Get a list of users from randomuser.me
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://randomuser.me/api/?results="+AmountOfUsersNeeded+"&nat=us,dk,fr,gb");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                RootObject root = JsonConvert.DeserializeObject<RootObject>(await response.Content.ReadAsStringAsync());
                RandomUsers = root.results;
                client.Dispose();
            }
            else
            {
                MessageBox.Show($"Error contacting user web service\n{(int)response.StatusCode} ({response.ReasonPhrase})", "Generator Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private async void GetCountries()
        {
            //Get a list of countries from restcountries.eu
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://restcountries.eu/rest/v2/all?fields=name");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                List<CountriesApi> countries = JsonConvert.DeserializeObject<List<CountriesApi>>(await response.Content.ReadAsStringAsync());
                RandomCountries = countries;
                client.Dispose();
            }
            else
            {
                MessageBox.Show($"Error contacting countries web service\n{(int)response.StatusCode} ({response.ReasonPhrase})", "Generator Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void GenerateAdmins(LoggedInAdministratorFacade facade, LoginToken<Administrator> loginToken)
        {
            List<Administrator> administrators = new List<Administrator>();
            Converters.ConvertToAdministrator(RandomUsers, administrators, AdministratorsAmount);
            for(int i = 0; i < administrators.Count; i++)
            {
                try
                {
                    facade.CreateNewAdministrator(loginToken, administrators[i]);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Username {administrators[i].Username} already exists", "Generator Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void GenerateCostumers(LoggedInAdministratorFacade facade, LoginToken<Administrator> loginToken)
        {
            List<Customer> customers = new List<Customer>();
            Converters.ConvertToCustomer(RandomUsers, customers, CustomersAmount);
            for (int i = 0; i < customers.Count; i++)
            {
                try
                {
                    facade.CreateNewCustomer(loginToken, customers[i]);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Username {customers[i].Username} already exists", "Generator Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void GenerateAirlineCompanies(LoggedInAdministratorFacade facade, LoginToken<Administrator> loginToken)
        {
            List<AirlineCompany> airlineCompanies = new List<AirlineCompany>();
            List<Country> countries = (List<Country>)facade.GetAllCountries(loginToken);
            Converters.ConvertToAirlineCompany(RandomUsers, airlineCompanies, countries, AirlineCompaniesAmount);
            for (int i = 0; i < airlineCompanies.Count; i++)
            {
                try
                {
                    facade.CreateNewAirline(loginToken, airlineCompanies[i]);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Username {airlineCompanies[i].Username} already exists", "Generator Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void GenerateCountries(LoggedInAdministratorFacade facade, LoginToken<Administrator> loginToken)
        {
            List<Country> countries = new List<Country>();
            Converters.ConvertToCountry(RandomCountries, countries, CountriesAmount);
            for (int i = 0; i < CountriesAmount; i++)
            {
                try
                {
                    facade.CreateNewCountry(loginToken, countries[0]);
                    countries.RemoveAt(0);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Country {countries[i].CountryName} already exists", "Generator Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void GenerateFlights(LoggedInAdministratorFacade facade, LoginToken<Administrator> loginToken,
            FlyingCenterSystem flyingCenterSystem)
        {
            Random r = new Random();
            List<AirlineCompany> airlineCompanies = (List<AirlineCompany>)facade.GetAllAirlineCompanies();
            List<Country> countries = (List<Country>)facade.GetAllCountries(loginToken);
            foreach (AirlineCompany airlineCompany in airlineCompanies)
            {
                //login as the airline to get access to the CreateFlight method
                LoginToken<AirlineCompany> AirlineCompanyLoginToken = flyingCenterSystem.AttemptLoginAirlineCompany(airlineCompany.Username,airlineCompany.Password);
                LoggedInAirlineFacade airlineFacade = (LoggedInAirlineFacade)FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(AirlineCompanyLoginToken);

                for (int i = 0; i < TicketsPerCustomerAmount; i++)
                {
                    //create departue date
                    DateTime newFlightDeparture = DateTime.Now;
                    newFlightDeparture = newFlightDeparture.AddDays(r.Next(0, 8));
                    newFlightDeparture = newFlightDeparture.AddHours(r.Next(0, 25));
                    newFlightDeparture = newFlightDeparture.AddMinutes(r.Next(0, 61));
                    newFlightDeparture = newFlightDeparture.AddSeconds(r.Next(0, 61));

                    //create landing date
                    DateTime newFlightLanding = newFlightDeparture;
                    newFlightLanding = newFlightLanding.AddHours(r.Next(0, 25));
                    newFlightLanding = newFlightLanding.AddMinutes(r.Next(0, 61));
                    newFlightLanding = newFlightLanding.AddSeconds(r.Next(0, 61));

                    //choose countries for the flight
                    Country country1 = countries[r.Next(0, countries.Count)];
                    Country country2 = null;
                    do
                    {
                        country2 = countries[r.Next(0, countries.Count)];
                    }
                    while (ReferenceEquals(country2, null) || country1.Equals(country2));

                    //create the flight and add it to the DB
                    int availableTickets = r.Next(30, 61);
                    Flight toAdd = new Flight(-1, airlineCompany.ID, country1.ID, country2.ID, newFlightDeparture, newFlightLanding, availableTickets, availableTickets);
                    airlineFacade.CreateFlight(AirlineCompanyLoginToken,toAdd);
                }
            }
        }
        private void GenerateTickets(LoggedInAdministratorFacade facade, LoginToken<Administrator> loginToken,
            FlyingCenterSystem flyingCenterSystem)
        {
            Random r = new Random();
            List<Customer> customers = (List<Customer>)facade._customerDAO.GetAll();
            List<Flight> Flights = (List<Flight>)facade.GetAllFlights();
            foreach (Customer customer in customers)
            {
                //login as the customer to get access to the PurchaseTicket method
                LoginToken<Customer> customerLoginToken = flyingCenterSystem.AttemptLoginCustomer(customer.Username, customer.Password);
                LoggedInCustomerFacade customerFacade = (LoggedInCustomerFacade)FlyingCenterSystem.GetFlyingCenterSystem().GetFacade(customerLoginToken);

                for (int i = 0; i < TicketsPerCustomerAmount; i++)
                {
                    /*in large amounts random might choose many flights that the customer already bought ticket to
                    therefore i am limiting the amount of times a random chooses this kind of flight to 3 and from there on
                    im moving to a for loop on all flights*/
                    int isNullCounter = 0;
                    while (isNullCounter < 3)
                    {
                        if(customerFacade.PurchaseTicket(customerLoginToken, Flights[r.Next(0, Flights.Count)]) == null)
                        {
                            isNullCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (isNullCounter == 3)
                    {
                        for (int j = 0; j < Flights.Count; i++)
                        {
                            if (customerFacade.PurchaseTicket(customerLoginToken, Flights[r.Next(0, Flights.Count)]) != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

    }
}
