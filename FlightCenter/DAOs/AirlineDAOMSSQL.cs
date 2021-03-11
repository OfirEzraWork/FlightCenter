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
    class AirlineDAOMSSQL : IAirlineDAO
    {
        public void Add(AirlineCompany t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies WHERE USER_NAME='{t.Username}' OR AIRLINE_NAME='{t.AirlineName}'", conn))
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
                using (SqlCommand cmd = new SqlCommand($"INSERT INTO {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies (AIRLINE_NAME, USER_NAME, PASSWORD, COUNTRY_CODE) VALUES(" +
                        $"'{t.AirlineName}', '{t.Username}', '{t.Password}', {t.CountryID})", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public AirlineCompany Get(int ID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies WHERE ID={ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        AirlineCompany c = new AirlineCompany((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["USER_NAME"], (string)reader["PASSWORD"], (int)reader["COUNTRY_CODE"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }

        public AirlineCompany GetAirlineByUsername(string name)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies WHERE USER_NAME='{name}'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        AirlineCompany c = new AirlineCompany((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["USER_NAME"], (string)reader["PASSWORD"], (int)reader["COUNTRY_CODE"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }

        public IList<AirlineCompany> GetAll()
        {
            IList<AirlineCompany> companies = new List<AirlineCompany>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AirlineCompany c = new AirlineCompany((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["USER_NAME"], (string)reader["PASSWORD"], (int)reader["COUNTRY_CODE"]);
                        companies.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return companies;
        }

        public IList<AirlineCompany> GetAllAirlinesByCountry(int CountryID)
        {
            IList<AirlineCompany> companies = new List<AirlineCompany>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies WHERE COUNTRY_CODE={CountryID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AirlineCompany c = new AirlineCompany((int)reader["ID"], (string)reader["AIRLINE_NAME"], (string)reader["USER_NAME"], (string)reader["PASSWORD"], (int)reader["COUNTRY_CODE"]);
                        companies.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return companies;
        }

        public void Remove(AirlineCompany t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies WHERE ID={t.ID}", conn))
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
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public void Update(AirlineCompany t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}AirlineCompanies SET" +
                    $" AIRLINE_NAME = '{t.AirlineName}'," +
                    $" USER_NAME = '{t.Username}'," +
                    $" PASSWORD = '{t.Password}'," +
                    $" COUNTRY_CODE = {t.CountryID}" +
                    $" WHERE ID={t.ID}", conn))
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
