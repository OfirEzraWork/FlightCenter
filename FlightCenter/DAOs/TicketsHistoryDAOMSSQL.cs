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
    class TicketsHistoryDAOMSSQL : ITicketsHistoryDAO
    {
        public void Add(Ticket t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory WHERE FLIGHT_ID={t.FlightID} AND CUSTOMER_ID={t.CustomerID}", conn))
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
                using (SqlCommand cmd = new SqlCommand($"INSERT INTO {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory (FLIGHT_ID, CUSTOMER_ID) VALUES(" +
                        $"{t.FlightID}, {t.CustomerID})", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public Ticket Get(int ID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory WHERE ID={ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Ticket c = new Ticket((int)reader["ID"], (int)reader["FLIGHT_ID"], (int)reader["CUSTOMER_ID"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }

        public IList<Ticket> GetAll()
        {
            IList<Ticket> tickets = new List<Ticket>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Ticket c = new Ticket((int)reader["ID"], (int)reader["FLIGHT_ID"], (int)reader["CUSTOMER_ID"]);
                        tickets.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return tickets;
        }

        public IList<Ticket> GetTicketsHistoryByAirline(AirlineCompany airline)
        {
            IList<Ticket> tickets = new List<Ticket>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory" +
                                                       $"JOIN {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies ON AirlineCompanies.ID = AIRLINE_COMPANY_ID.ID " +
                                                       $"JOIN {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory ON TicketsHistory.FLIGHT_ID = Flights.ID", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Ticket c = new Ticket((int)reader["ID"], (int)reader["FLIGHT_ID"], (int)reader["CUSTOMER_ID"]);
                        tickets.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return tickets;
        }

        public void Remove(Ticket t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory WHERE ID={t.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }
        public void RemoveAllByFlightID(int FlightID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory WHERE FLIGHT_ID={FlightID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public void Update(Ticket t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}TicketsHistory SET" +
                    $" FLIGHT_ID = {t.FlightID}," +
                    $" CUSTOMER_ID = {t.CustomerID}" +
                    $" WHERE TicketsHistory.ID = {t.ID}", conn))
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
