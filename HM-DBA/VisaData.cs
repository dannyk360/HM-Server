using System;
using System.Collections.Generic;
using System.Data;
using HM_DBA.model;
using MySql.Data.MySqlClient;

namespace HM_DBA
{
    internal class VisaData
    {
        public Visa Get(MySqlConnection conn, int visaId)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            Visa returnValue;
            conn.Open();
            query.CommandText = "SELECT * FROM Visa WHERE id = " + visaId;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                returnValue = null;
            else
            {
                returnValue = new Visa();
                returnValue.cvv = (int)reader["cvv"];
                returnValue.expirationDate =reader["expiretionDate"].ToString();
                returnValue.visaNumber = (string)reader["visaNumber"];
            }
            conn.Close();
            return returnValue;
        }

        public int GetLastId(MySqlConnection conn)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            int max = 0;
            conn.Open();
            query.CommandText = "SELECT * FROM Visa";
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

        public void Create(MySqlConnection conn, Company company)
        {
            var query = new MySqlCommand();
          //  string expiretionDateFormat = "yyyy-dd-mm HH:MM:ss";
            conn.Open();
            query.CommandText = "INSERT INTO Visa (id, visaNumber, expiretionDate, cvv) VALUES ('"
                                 + (company.visaId) + "', '" + company.visa.visaNumber + "', '" + company.visa.expirationDate+ "', '"
                                 + company.visa.cvv+"');";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }

        public void Update(MySqlConnection conn, int visaId, Visa companyVisa)
        {
            var query = new MySqlCommand();
            conn.Open();
            query.CommandText = "UPDATE Visa SET visaNumber = '" + companyVisa.visaNumber + "', expiretionDate = '" + companyVisa.expirationDate +
                                "',cvv = " + companyVisa.cvv + " WHERE id = " + visaId + ";";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();

            conn.Close();
        }

        public void DeleteByCompany(MySqlConnection conn, int id)
        {
            var query = new MySqlCommand();

            conn.Open();
            query.CommandText = "DELETE FROM Visa WHERE id = " + id;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }
    }
    }