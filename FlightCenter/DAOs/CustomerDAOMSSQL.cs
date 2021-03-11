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
    class CustomerDAOMSSQL : ICustomerDAO
    {
        public void Add(Customer t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers WHERE USER_NAME='{t.Username}'", conn))
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
                using (SqlCommand cmd = new SqlCommand($"INSERT INTO {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers (FIRST_NAME, LAST_NAME, USER_NAME, PASSWORD, ADDRESS, PHONE_NO, CREDIT_CARD_NUMBER) VALUES(" +
                        $"'{t.FirstName}', '{t.LastName}', '{t.Username}', '{t.Password}', '{t.Address}', '{t.PhoneNo}', '{t.CreditCardNumber}')", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public Customer Get(int ID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers WHERE ID={ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Customer c = new Customer((int)reader["ID"], (string)reader["FIRST_NAME"], (string)reader["LAST_NAME"],
                            (string)reader["USER_NAME"], (string)reader["PASSWORD"], (string)reader["ADDRESS"],
                            (string)reader["PHONE_NO"], (string)reader["CREDIT_CARD_NUMBER"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }

        public IList<Customer> GetAll()
        {
            IList<Customer> customers = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Customer c = new Customer((int)reader["ID"], (string)reader["FIRST_NAME"], (string)reader["LAST_NAME"],
                            (string)reader["USER_NAME"], (string)reader["PASSWORD"], (string)reader["ADDRESS"],
                            (string)reader["PHONE_NO"], (string)reader["CREDIT_CARD_NUMBER"]);
                        customers.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return customers;
        }

        public Customer GetCustomerByUsername(string Username)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers WHERE USER_NAME='{Username}'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Customer c = new Customer((int)reader["ID"], (string)reader["FIRST_NAME"], (string)reader["LAST_NAME"],
                            (string)reader["USER_NAME"], (string)reader["PASSWORD"], (string)reader["ADDRESS"],
                            (string)reader["PHONE_NO"], (string)reader["CREDIT_CARD_NUMBER"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }

        public void Remove(Customer t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers WHERE ID={t.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public void Update(Customer t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers SET" +
                    $" FIRST_NAME = '{t.FirstName}'," +
                    $" LAST_NAME = '{t.LastName}'," +
                    $" USER_NAME = '{t.Username}'," +
                    $" PASSWORD = '{t.Password}'," +
                    $" ADDRESS = '{t.Address}'," +
                    $" PHONE_NO = '{t.PhoneNo}'," +
                    $" CREDIT_CARD_NUMBER = '{t.CreditCardNumber}'" +
                    $" WHERE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Customers.ID = {t.ID}", conn))
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
