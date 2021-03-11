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
    class AdministratorDAOMSSQL : IAdminDAO
    {
        public void Add(Administrator t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins WHERE USER_NAME='{t.Username}'", conn))
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
                using (SqlCommand cmd = new SqlCommand($"INSERT INTO {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins (USER_NAME, PASSWORD) VALUES(" +
                        $"'{t.Username}', '{t.Password}')", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public Administrator Get(int ID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins WHERE ID={ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Administrator c = new Administrator((Int32)reader["ID"], (string)reader["USER_NAME"], (string)reader["PASSWORD"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }

        public IList<Administrator> GetAll()
        {
            IList<Administrator> administrators = new List<Administrator>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Administrator c = new Administrator((Int32)reader["ID"], (string)reader["USER_NAME"], (string)reader["PASSWORD"]);
                        administrators.Add(c);
                    }
                    cmd.Connection.Close();
                }
            }
            return administrators;
        }

        public Administrator GetAdministratorByUsername(string Username)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins WHERE USER_NAME='{Username}'", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Administrator c = new Administrator((int)reader["ID"], (string)reader["USER_NAME"], (string)reader["PASSWORD"]);
                        cmd.Connection.Close();
                        return c;
                    }
                    cmd.Connection.Close();
                }
            }
            return null;
        }

        public void Remove(Administrator t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins WHERE ID={t.ID}", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
            }
        }

        public void Update(Administrator t)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"UPDATE {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins SET" +
                    $" USER_NAME = '{t.Username}'," +
                    $" PASSWORD = '{t.Password}'" +
                    $" WHERE Admins.ID = {t.ID}", conn))
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
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM {ConfigurationManager.ConnectionStrings["PreTableText"].ConnectionString}Admins WHERE NOT USER_NAME='admin'", conn))
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
