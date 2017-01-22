using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HM_DBA.model;

namespace HM_DBA
{
    internal class VisaData
    {
        public Visa Get(SqlConnection conn, int visaId)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
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
                returnValue.expiretionDate = (DateTime)reader["expiretionDate"];
                returnValue.visaNumber = (string)reader["visaNumber"];
            }
            conn.Close();
            return returnValue;
        }

        public int GetLastId(SqlConnection conn)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
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

        public void Create(SqlConnection conn, Company company)
        {
            var query = new SqlCommand();

            conn.Open();
            query.CommandText = "INSERT INTO Visa (id, visaNumber, expiretionDate, cvv) VALUES ('"
                                 + (company.visaId) + "', '" + company.visa.visaNumber + "', '" + company.visa.expiretionDate + "', '"
                                 + company.visa.cvv+"');";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }

        public void Update(SqlConnection conn, int visaId, Visa companyVisa)
        {
            var query = new SqlCommand();
            conn.Open();
            query.CommandText = "UPDATE Visa SET visaNumber = '" + companyVisa.visaNumber + "', expiretionDate = '" + companyVisa.expiretionDate +
                                "',cvv = " + companyVisa.cvv + " WHERE id = " + visaId + ";";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();

            conn.Close();
        }
    }
    }