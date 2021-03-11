using FlightCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlightCenterDBGenerator.ViewModel;

namespace FlightCenterDBGenerator
{
    static class Converters
    {
        private static Random r = new Random();
        public static void ConvertToAdministrator(List<RandomUser> randomUsers, List<Administrator> administrators, int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                Administrator administrator = new Administrator(-1, randomUsers[0].login.username, randomUsers[0].login.password);
                administrators.Add(administrator);
                randomUsers.RemoveAt(0);
            }
        }
        public static void ConvertToCustomer(List<RandomUser> randomUsers, List<Customer> customers, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Customer customer = new Customer(-1, randomUsers[0].name.first, randomUsers[0].name.last, randomUsers[0].login.username,
                    randomUsers[0].login.password, randomUsers[0].location.ToString(), randomUsers[0].phone, randomUsers[0].cell);
                customers.Add(customer);
                randomUsers.RemoveAt(0);
            }
        }
        public static void ConvertToAirlineCompany(List<RandomUser> randomUsers, List<AirlineCompany> airlineCompanies, List<Country> countries, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                AirlineCompany airlineCompany = new AirlineCompany(-1, randomUsers[0].login.username
                    , randomUsers[0].login.username, randomUsers[0].login.password, countries[r.Next(0,countries.Count)].ID);
                airlineCompanies.Add(airlineCompany);
                randomUsers.RemoveAt(0);
            }
        }
        public static void ConvertToCountry(List<CountriesApi> countriesApis, List<Country> countries, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int index = r.Next(0,countriesApis.Count);
                Country country = new Country(-1, countriesApis[index].name);
                countries.Add(country);
                countriesApis.RemoveAt(index);
            }
        }
    }
}
