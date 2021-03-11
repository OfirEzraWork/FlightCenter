using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightCenter.DAOs;

namespace FlightCenter.Facades
{
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        AdministratorDAOMSSQL _administratorDAO = new AdministratorDAOMSSQL();
        public LoggedInAdministratorFacade()
        {
        }
        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (_customerDAO.GetCustomerByUsername(airline.AirlineName) != null |
                _airlineDAO.GetAirlineByUsername(airline.AirlineName) != null |
                _administratorDAO.GetAdministratorByUsername(airline.AirlineName) != null)
            {
                throw new UsernameAlreadyExistsException();
            }
            _airlineDAO.Add(airline);
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (_customerDAO.GetCustomerByUsername(customer.Username) != null |
                _airlineDAO.GetAirlineByUsername(customer.Username) != null |
                _administratorDAO.GetAdministratorByUsername(customer.Username) != null)
            {
                throw new UsernameAlreadyExistsException();
            }
            _customerDAO.Add(customer);
        }
        public void CreateNewAdministrator(LoginToken<Administrator> token, Administrator administrator)
        {
            if (_customerDAO.GetCustomerByUsername(administrator.Username) != null |
                _airlineDAO.GetAirlineByUsername(administrator.Username) != null |
                _administratorDAO.GetAdministratorByUsername(administrator.Username) != null)
            {
                throw new UsernameAlreadyExistsException();
            }
            _administratorDAO.Add(administrator);
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (_airlineDAO.GetAirlineByUsername(airline.Username) == null)
            {
                throw new UserDoesNotExistsException();
            }
            IList<Flight> flights = _flightDAO.GetFlightsByAirline(airline);
            foreach (Flight f in flights)
            {
                _ticketDAO.RemoveAllByFlightID(f.ID);
            }
            _flightDAO.RemoveAllByAirlineCompany(airline);
            _airlineDAO.Remove(airline);
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (_customerDAO.GetCustomerByUsername(customer.Username) == null)
            {
                throw new UserDoesNotExistsException();
            }
            _ticketDAO.RemoveAllByCustomerID(customer.ID);
            _customerDAO.Remove(customer);
        }

        public void RemoveAdministrator(LoginToken<Administrator> token, Administrator administrator)
        {
            if (_administratorDAO.GetAdministratorByUsername(administrator.Username) == null)
            {
                throw new UserDoesNotExistsException();
            }
            _administratorDAO.Remove(administrator);
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (_airlineDAO.Get((int)airline.ID) == null)
            {
                throw new UserDoesNotExistsException();
            }
            _airlineDAO.Update(airline);
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            if (_customerDAO.Get((int)customer.ID) == null)
            {
                throw new UserDoesNotExistsException();
            }
            _customerDAO.Update(customer);
        }

        public void UpdateAdministratorDetails(LoginToken<Administrator> token, Administrator administrator)
        {
            if (_administratorDAO.Get((int)administrator.ID) == null)
            {
                throw new UserDoesNotExistsException();
            }
            _administratorDAO.Update(administrator);
        }

        public void CreateNewCountry(LoginToken<Administrator> token, Country country)
        {
            if (_countryDAO.GetCountryByName(country.CountryName) != null)
            {
                throw new CountryAlreadyExistsInDatabaseException();
            }
            _countryDAO.Add(country);
        }
        public IList<Country> GetAllCountries(LoginToken<Administrator> token)
        {
            return _countryDAO.GetAll();
        }
        public void RemoveAllAdmins(LoginToken<Administrator> token)
        {
            _administratorDAO.RemoveAll();
        }
    }
}
