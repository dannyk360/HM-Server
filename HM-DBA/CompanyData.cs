using System.Collections.Generic;
using System.Data;
using HM_DBA.model;
using MySql.Data.MySqlClient;

namespace HM_DBA
{
    internal class CompanyData
    {
        public Company Get(MySqlConnection conn,int companyId)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            Company returnValue = new Company();
            conn.Open();
            query.CommandText = "SELECT * FROM Company WHERE id = " + companyId;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                returnValue = null;
            else
            {
                returnValue.address = (string)reader["address"];
                returnValue.field = (string)reader["field"];
                returnValue.name = (string)reader["name"];
                returnValue.id = (int)reader["id"];
                returnValue.visaId = (int)reader["visaId"];
            }
            conn.Close();
            return returnValue;
        }

        public List<Company> GetAll(MySqlConnection conn)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            List<Company> returnValue = new List<Company>();
            conn.Open();
            query.CommandText = "SELECT * FROM Company";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            while (reader.Read())
            {
                var company = new Company();
                company.id = (int)reader["id"];
                company.visaId = (int)reader["visaId"];
                company.address = (string)reader["address"];
                company.name = (string) reader["name"];
                company.field = (string) reader["field"];
                returnValue.Add(company);
            }
            conn.Close();
            return returnValue;
        }

        public bool CheckExist(MySqlConnection conn, string companyName)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            bool returnValue = false;
            conn.Open();
            query.CommandText = "SELECT * FROM Company WHERE name = '"+companyName + "'";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (reader.Read())
                returnValue = true;
            conn.Close();
            return returnValue;
        }

        public int Create(MySqlConnection conn, Company company,int lastId)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;

            conn.Open();
            query.CommandText = "INSERT INTO Company (id, name, address, field, visaId) VALUES ('"
                                 +(lastId + 1)+"', '"+company.name+ "', '"+company.address+ "', '"
                                 +company.field + "', '"+ company.visaId + "');";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
            return lastId + 1;
        }

        public int GetLastId(MySqlConnection conn)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            int max  = 0;
            conn.Open();
            query.CommandText = "SELECT * FROM Company";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            while (reader.Read())
            {
                int nextNumber = (int)reader["id"];
                if (nextNumber > max)
                    max = nextNumber;
            }
            conn.Close();
            return max;
        }

        public int Update(MySqlConnection conn, int id, Company company)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            conn.Open();
            query.CommandText = "UPDATE Company SET address = '"+company.address+ "', field = '" + company.field +
                                "',name = '" + company.name + "' WHERE id = " + id + ";";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();

            conn.Close();
            return Get(conn, id).visaId;
        }

        public void Delete(MySqlConnection conn, int id)
        {
            var query = new MySqlCommand();

            conn.Open();
            query.CommandText = "DELETE FROM Company WHERE id = " + id;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }
    }
}