using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightCenter.DAOs
{
    class FlightDAOMSSQL : IFlightDAO
    {
        private string dateFormat = "yyyy-MM-dd HH:mm:ss";
        public void Add(Flight t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE ID={t.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cmd.Connection.Close();
                        return;
                    }
                    cmd.Connection.Close();
                }
                using (SqlCommand cmd = new SqlCommand($"INSERT INTO {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights (AIRLINE_COMPANY_ID, ORIGIN_COUNTRY_ID, DESTINATION_COUNTRY_ID, DEPARTURE_TIME, LANDING_TIME, REMAINING_TICKETS, MAX_TICKETS) VALUES(" +
                        $"{t.AirlineCompanyID}, {t.OriginCountryID}, {t.DestinationCountryID}, '{t.DepartureTime.ToString(dateFormat)}', '{t.LandingTime.ToString(dateFormat)}', {t.RemainingTickets}, {t.MaxTickets})", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public Flight Get(int ID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE ID={ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }
        public IList<FlightWithNames> GetUpcomingDepartures()
        {
            string preText = ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString;
            IList<FlightWithNames> flights = new List<FlightWithNames>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                        $"SELECT {preText}Flights.ID, Origin.COUNTRY_NAME AS ORIGIN_COUNTRY, Destination.COUNTRY_NAME AS DESTINATION_COUNTRY, {preText}Flights.DEPARTURE_TIME, {preText}Flights.LANDING_TIME, {preText}AirlineCompanies.AIRLINE_NAME " +
                        $"FROM {preText}Flights " +
                        $"LEFT JOIN {preText}AirlineCompanies ON {preText}Flights.AIRLINE_COMPANY_ID = {preText}AirlineCompanies.ID " +
                        $"LEFT JOIN {preText}Countries AS Origin ON {preText}Flights.ORIGIN_COUNTRY_ID = Origin.ID " +
                        $"LEFT JOIN {preText}Countries AS Destination ON {preText}Flights.DESTINATION_COUNTRY_ID = Destination.ID " +
                        $"WHERE DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', DEPARTURE_TIME) <= 12 AND DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', DEPARTURE_TIME) > 0 " +
                        $"ORDER BY {preText}Flights.DEPARTURE_TIME;"
                        , conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FlightWithNames c = new FlightWithNames((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["ORIGIN_COUNTRY"], (string)reader["DESTINATION_COUNTRY"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }
        public IList<FlightWithNames> GetUpcomingDepartures(string key, string value)
        {
            string preText = ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString;
            IList<FlightWithNames> flights = new List<FlightWithNames>();
            string query = "";
            if (key == "ID")
            {
                query = preText + "Flights." + key + "=" + value;
            }
            else if (key == "AIRLINE_NAME")
            {
                query = preText + "AirlineCompanies." + key + "='" + value + "'";
            }
            else if (key == "ORIGIN_COUNTRY")
            {
                query = "Origin.COUNTRY_NAME='" + value + "'";
            }
            else if (key == "DESTINATION_COUNTRY")
            {
                query = "Destination.COUNTRY_NAME='" + value + "'";
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                        $"SELECT {preText}Flights.ID, Origin.COUNTRY_NAME AS ORIGIN_COUNTRY, Destination.COUNTRY_NAME AS DESTINATION_COUNTRY, {preText}Flights.DEPARTURE_TIME, {preText}Flights.LANDING_TIME, {preText}AirlineCompanies.AIRLINE_NAME " +
                        $"FROM {preText}Flights " +
                        $"LEFT JOIN {preText}AirlineCompanies ON {preText}Flights.AIRLINE_COMPANY_ID = {preText}AirlineCompanies.ID " +
                        $"LEFT JOIN {preText}Countries AS Origin ON {preText}Flights.ORIGIN_COUNTRY_ID = Origin.ID " +
                        $"LEFT JOIN {preText}Countries AS Destination ON {preText}Flights.DESTINATION_COUNTRY_ID = Destination.ID " +
                        $"WHERE DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', DEPARTURE_TIME) <= 12 AND DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', DEPARTURE_TIME) > 0 AND {query}" +
                        $"ORDER BY {preText}Flights.DEPARTURE_TIME;"
                        , conn))
                {
                        cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FlightWithNames c = new FlightWithNames((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["ORIGIN_COUNTRY"], (string)reader["DESTINATION_COUNTRY"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }
        public IList<FlightWithNames> GetUpcomingLandings()
        {
            string preText = ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString;
            IList<FlightWithNames> flights = new List<FlightWithNames>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                        $"SELECT {preText}Flights.ID, Origin.COUNTRY_NAME AS ORIGIN_COUNTRY, Destination.COUNTRY_NAME AS DESTINATION_COUNTRY, {preText}Flights.DEPARTURE_TIME, {preText}Flights.LANDING_TIME, {preText}AirlineCompanies.AIRLINE_NAME " +
                        $"FROM {preText}Flights " +
                        $"LEFT JOIN {preText}AirlineCompanies ON {preText}Flights.AIRLINE_COMPANY_ID = {preText}AirlineCompanies.ID " +
                        $"LEFT JOIN {preText}Countries AS Origin ON {preText}Flights.ORIGIN_COUNTRY_ID = Origin.ID " +
                        $"LEFT JOIN {preText}Countries AS Destination ON {preText}Flights.DESTINATION_COUNTRY_ID = Destination.ID " +
                        $"WHERE DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', LANDING_TIME) <= 12 AND DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', LANDING_TIME) > -4 " +
                        $"ORDER BY {preText}Flights.LANDING_TIME;"
                        , conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FlightWithNames c = new FlightWithNames((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["ORIGIN_COUNTRY"], (string)reader["DESTINATION_COUNTRY"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }
        public IList<FlightWithNames> GetUpcomingLandings(string key, string value)
        {
            string preText = ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString;
            IList<FlightWithNames> flights = new List<FlightWithNames>();
            string query = "";
            if (key == "ID")
            {
                query = preText + "Flights." + key + "=" + value;
            }
            else if (key == "AIRLINE_NAME")
            {
                query = preText + "AirlineCompanies." + key + "='" + value + "'";
            }
            else if (key == "ORIGIN_COUNTRY")
            {
                query = "Origin.COUNTRY_NAME='" + value + "'";
            }
            else if (key == "DESTINATION_COUNTRY")
            {
                query = "Destination.COUNTRY_NAME='" + value + "'";
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                string s = preText+"Flights."+query;
                using (SqlCommand cmd = new SqlCommand(
                        $"SELECT {preText}Flights.ID, Origin.COUNTRY_NAME AS ORIGIN_COUNTRY, Destination.COUNTRY_NAME AS DESTINATION_COUNTRY, {preText}Flights.DEPARTURE_TIME, {preText}Flights.LANDING_TIME, {preText}AirlineCompanies.AIRLINE_NAME " +
                        $"FROM {preText}Flights " +
                        $"LEFT JOIN {preText}AirlineCompanies ON {preText}Flights.AIRLINE_COMPANY_ID = {preText}AirlineCompanies.ID " +
                        $"LEFT JOIN {preText}Countries AS Origin ON {preText}Flights.ORIGIN_COUNTRY_ID = Origin.ID " +
                        $"LEFT JOIN {preText}Countries AS Destination ON {preText}Flights.DESTINATION_COUNTRY_ID = Destination.ID " +
                        $"WHERE DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', LANDING_TIME) <= 12 AND DATEDIFF(HOUR, '{DateTime.Now.ToString(dateFormat)}', LANDING_TIME) > -4 AND {query} " +
                        $"ORDER BY {preText}Flights.LANDING_TIME;"
                        , conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FlightWithNames c = new FlightWithNames((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["ORIGIN_COUNTRY"], (string)reader["DESTINATION_COUNTRY"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }

        public IList<Flight> GetAll()
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }

        public IList<Flight> GetFlightsByAirline(AirlineCompany airline)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE AIRLINE_COMPANY_ID={airline.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }

        public void Remove(Flight t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE ID={t.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public void RemoveAllByAirlineCompany(AirlineCompany airline)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE AIRLINE_COMPANY_ID={airline.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public void Update(Flight t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights SET" +
                    $" AIRLINE_COMPANY_ID = {t.AirlineCompanyID}," +
                    $" ORIGIN_COUNTRY_ID = {t.OriginCountryID}," +
                    $" DESTINATION_COUNTRY_ID = {t.DestinationCountryID}," +
                    $" DEPARTURE_TIME = '{t.DepartureTime.ToString(dateFormat)}'," +
                    $" LANDING_TIME = '{t.LandingTime.ToString(dateFormat)}'," +
                    $" REMAINING_TICKETS = {t.RemainingTickets}," +
                    $" MAX_TICKETS = {t.MaxTickets}" +
                    $" WHERE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights.ID = {t.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public Dictionary<Flight, int> GetAllFlightVacancy()
        {
            Dictionary<Flight, int> toReturn = new Dictionary<Flight, int>();
            IList<Flight> flights = GetAll();
            for(int i = 0; i < flights.Count; i++)
            {
                toReturn.Add(flights[i], flights[i].RemainingTickets);
            }
            return toReturn;
        }

        public Flight GetFlightByID(int ID)
        {
            return Get((int)ID);
        }

        public IList<Flight> GetFlightsByCustomer(Customer customer)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights" +
                                                       $" JOIN {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Tickets ON Flights.ID = Tickets.FLIGHT_ID " +
                                                       $" JOIN {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers ON Customers.ID = Tickets.CUSTOMER_ID WHERE Customers.ID = {customer.ID} ", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            //$"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE DEPARTURE_TIME='{departureDate.ToString(dateFormat)}'", conn)
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights " +
                    $"WHERE DEPARTURE_TIME>='{departureDate.ToString(dateFormat)}'" +
                    $"and DEPARTURE_TIME<'{departureDate.AddDays(1).ToString(dateFormat)}'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryID)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE DESTINATION_COUNTRY_ID={countryID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights " +
                $"WHERE LANDING_TIME>='{landingDate.ToString(dateFormat)}'" +
                $"and LANDING_TIME<'{landingDate.AddDays(1).ToString(dateFormat)}'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryID)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights WHERE ORIGIN_COUNTRY_ID={countryID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Flight c = new Flight((int)reader["ID"], (int)reader["AIRLINE_COMPANY_ID"], (int)reader["ORIGIN_COUNTRY_ID"], (int)reader["DESTINATION_COUNTRY_ID"], (DateTime)reader["DEPARTURE_TIME"], (DateTime)reader["LANDING_TIME"], (int)reader["REMAINING_TICKETS"], (int)reader["MAX_TICKETS"]);
                        flights.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return flights;
        }
        public void BuyTicket(Flight f)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights SET REMAINING_TICKETS = REMAINING_TICKETS-1 WHERE Flights.ID = {f.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }
        public void CancelTicket(Ticket t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights SET REMAINING_TICKETS = REMAINING_TICKETS+1 WHERE Flights.ID = {t.FlightID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public void RemoveAll()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Flights", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }
    }
}
