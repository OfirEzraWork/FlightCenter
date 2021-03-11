using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightCenter.DAOs;
using FlightCenter.POCO;

namespace FlightCenter
{
    public class LoginService : ILoginService
    {
        private AirlineDAOMSSQL airlineDAOMSSQL = new AirlineDAOMSSQL();
        private CustomerDAOMSSQL customerDAOMSSQL = new CustomerDAOMSSQL();
        private AdministratorDAOMSSQL administratorDAOMSSQL = new AdministratorDAOMSSQL();
        public LoginService()
        {

        }
        public bool TryAdminLogin(string userName, string password, out LoginToken<Administrator> token)
        {
            Administrator admin = administratorDAOMSSQL.GetAdministratorByUsername(userName);
            if (admin == null)
            {
                token = null;
                return false;
            }
            else if(admin.Password != password)
            {
                throw new WrongPasswordException();
            }
            else
            {
                token = new LoginToken<Administrator>();
                token.User = admin;
                return true;
            }
        }

        public bool TryAirlineLogin(string userName, string password, out LoginToken<AirlineCompany> token)
        {
            AirlineCompany airlineCompany = airlineDAOMSSQL.GetAirlineByUsername(userName);
            if (airlineCompany == null)
            {
                token = null;
                return false;
            }
            else if (airlineCompany.Password != password)
            {
                throw new WrongPasswordException();
            }
            else
            {
                token = new LoginToken<AirlineCompany>();
                token.User = airlineCompany;
                return true;
            }
        }

        public bool TryCustomerLogin(string userName, string password, out LoginToken<Customer> token)
        {
            Customer customer = customerDAOMSSQL.GetCustomerByUsername(userName);
            if (customer == null)
            {
                token = null;
                return false;
            }
            else if (customer.Password != password)
            {
                throw new WrongPasswordException();
            }
            else
            {
                token = new LoginToken<Customer>();
                token.User = customer;
                return true;
            }
        }
    }
}
