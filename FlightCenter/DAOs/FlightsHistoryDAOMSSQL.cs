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
    class FlightsHistoryDAOMSSQL : IFlightsHistoryDAO
    {
        public void Add(Flight t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE ID={t.ID}", conn))
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
                using (SqlCommand cmd = new SqlCommand($"INSERT INTO {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory (AIRLINE_COMPANY_ID, ORIGIN_COUNTRY_ID, DESTINATION_COUNTRY_ID, DEPARTURE_TIME, LANDING_TIME, REMAINING_TICKETS, MAX_TICKETS) VALUES(" +
                        $"{t.AirlineCompanyID}, {t.OriginCountryID}, {t.DestinationCountryID}, {t.DepartureTime}, {t.LandingTime}, {t.RemainingTickets}, {t.MaxTickets})", conn))
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
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE ID={ID}", conn))
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

        public IList<Flight> GetAll()
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory", conn))
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

        public IList<Flight> GetFlightsHistoryByAirline(AirlineCompany airline)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE AIRLINE_COMPANY_ID={airline.ID}", conn))
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
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE ID={t.ID}", conn))
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
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory SET" +
                    $" AIRLINE_COMPANY_ID = {t.AirlineCompanyID}," +
                    $" ORIGIN_COUNTRY_ID = {t.OriginCountryID}," +
                    $" DESTINATION_COUNTRY_ID = {t.DestinationCountryID}," +
                    $" DEPARTURE_TIME = {t.DepartureTime}," +
                    $" LANDING_TIME = {t.LandingTime}," +
                    $" REMAINING_TICKETS = {t.RemainingTickets}," +
                    $" MAX_TICKETS = {t.MaxTickets}" +
                    $" WHERE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory.ID = {t.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public Flight GetFlightByID(int ID)
        {
            return Get((int)ID);
        }

        public IList<Flight> GetFlightsHistoryByCustomer(Customer customer)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory" +
                                                       $"JOIN {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Tickets ON Flights.ID = Tickets.ID " +
                                                       $"JOIN {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers ON Customers.ID = Tickets.ID", conn))
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

        public IList<Flight> GetFlightsHistoryByDepatrureDate(DateTime departureDate)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE DEPARTURE_TIME={departureDate}", conn))
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

        public IList<Flight> GetFlightsHistoryByDestinationCountry(int countryID)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE DESTINATION_COUNTRY_ID={countryID}", conn))
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

        public IList<Flight> GetFlightsHistoryByLandingDate(DateTime landingDate)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE LANDING_TIME={landingDate}", conn))
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

        public IList<Flight> GetFlightsHistoryByOriginCountry(int countryID)
        {
            IList<Flight> flights = new List<Flight>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}FlightsHistory WHERE ORIGIN_COUNTRY_ID={countryID}", conn))
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
    }
}
