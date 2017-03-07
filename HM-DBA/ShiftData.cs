using System;
using System.Collections.Generic;
using System.Data;
using HM_DBA.model;
using MySql.Data.MySqlClient;

namespace HM_DBA
{
    internal class ShiftData
    {
        public List<Shift> GetById(MySqlConnection conn, int userId)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            List<Shift> returnValue = new List<Shift>();
            conn.Open();
            query.CommandText = "SELECT * FROM Shifts WHERE userId = " + userId ;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            while (reader.Read())
            {
                Shift newShift = new Shift(reader["date"].ToString(), reader["start"].ToString(),reader["endDate"].ToString(), (string)reader["comment"]);
                returnValue.Add(newShift);
            }
            conn.Close();
            return returnValue;
        }

        public void DeleteByUser(MySqlConnection conn, int userId)
        {
            var query = new MySqlCommand();

            conn.Open();
            query.CommandText = "DELETE FROM Shifts WHERE userId = " + userId;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }

        public void Create(MySqlConnection conn, int userId, List<Shift> shifts)
        {
            var query = new MySqlCommand();
            shifts.ForEach(shift =>
                {
                    query.CommandText = "INSERT INTO Shifts (date, start, endDate, comment, id, userId" + ") VALUES ('"
                                        + shift.date + "', '" + shift.start + "', '" + shift.end + "', '"
                                        + shift.comment + "', '" + GetLast(conn) + "', '" + userId+ "');";
                    query.CommandType = CommandType.Text;
                    conn.Open();
                    query.Connection = conn;

                    query.ExecuteReader();
                    conn.Close();
                }
            );
        }

        private int GetLast(MySqlConnection conn)
        {
            var query = new MySqlCommand();
            MySqlDataReader reader;
            int max = 0;
            conn.Open();
            query.CommandText = "SELECT * FROM Shifts";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                max = 1;
            else {
                do
                {
                    int nextNumber;
                    int.TryParse(reader["id"].ToString() ,out nextNumber);
                    if (nextNumber > max)
                        max = nextNumber;
                } while (reader.Read());
            }
            conn.Close();
            return max+1;
        }
    }
}