using FlightCenter.Facades;
using FlightCenter.POCO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightCenter
{
    public class FlyingCenterSystem
    {
        private static FlyingCenterSystem flyingCenterSystem;
        private static object key = new object();
        private DateTime nextMoveExpiredTicketsTime;
        private SystemFacade systemFacade = new SystemFacade();

        LoginService LoginService { get; set; }
        private FlyingCenterSystem()
        {
            LoginService = new LoginService();
            Task.Run(StartMovingTimer);
        }
        public static FlyingCenterSystem GetFlyingCenterSystem()
        {
            if(flyingCenterSystem == null)
            {
                lock (key)
                {
                    if(flyingCenterSystem == null)
                    {
                        flyingCenterSystem = new FlyingCenterSystem();
                    }
                }
            }
            return flyingCenterSystem;
        }
        public LoginToken<Administrator> AttemptLoginAdministrator(string username, string password)
        {
            LoginToken<Administrator> tokenAdmin;
            try
            {
                LoginService.TryAdminLogin(username, password, out tokenAdmin);
                return tokenAdmin;
            }
            catch(WrongPasswordException e)
            {
                throw e;
            }
        }
        public LoginToken<AirlineCompany> AttemptLoginAirlineCompany(string username, string password)
        {
            LoginToken<AirlineCompany> tokenAirline;
            try
            {
                LoginService.TryAirlineLogin(username, password, out tokenAirline);
                return tokenAirline;
            }
            catch (WrongPasswordException e)
            {
                throw e;
            }
        }
        public LoginToken<Customer> AttemptLoginCustomer(string username, string password)
        {
            LoginToken<Customer> tokenCustomer;
            try
            {
                LoginService.TryCustomerLogin(username, password, out tokenCustomer);
                return tokenCustomer;
            }
            catch (WrongPasswordException e)
            {
                throw e;
            }
        }
        public FacadeBase GetFacade<T>(LoginToken<T> token)
        {
            Type tokenType = typeof(T);
            if(tokenType == typeof(Administrator))
            {
                return new LoggedInAdministratorFacade();
            }
            else if(tokenType == typeof(AirlineCompany))
            {
                return new LoggedInAirlineFacade();
            }
            else if (tokenType == typeof(Customer))
            {
                return new LoggedInCustomerFacade();
            }
            return null;

        }
        private void StartMovingTimer()
        {
            nextMoveExpiredTicketsTime = DateTime.Now;
            nextMoveExpiredTicketsTime.AddDays(1);
            nextMoveExpiredTicketsTime.AddHours(-1 * nextMoveExpiredTicketsTime.Hour);
            nextMoveExpiredTicketsTime.AddHours(int.Parse(ConfigurationManager.AppSettings["moveExpiredTicketsHour"]));
            nextMoveExpiredTicketsTime.AddMinutes(-1 * nextMoveExpiredTicketsTime.Minute);
            nextMoveExpiredTicketsTime.AddMinutes(int.Parse(ConfigurationManager.AppSettings["moveExpiredTicketsMinute"]));
            nextMoveExpiredTicketsTime.AddSeconds(-1 * nextMoveExpiredTicketsTime.Second);
            nextMoveExpiredTicketsTime.AddSeconds(int.Parse(ConfigurationManager.AppSettings["moveExpiredTicketsSecond"]));
            TimeSpan timeToSleep = nextMoveExpiredTicketsTime - DateTime.Now;
            Timer t = new Timer(new TimerCallback(MoveExpired), null, 0, Convert.ToInt32(timeToSleep.TotalMilliseconds));
        }
        private void MoveExpired(object state)
        {
            systemFacade.MoveExpiredObjects();
            Task.Run(StartMovingTimer);
        }

    }
}
