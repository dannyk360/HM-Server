using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HM_DBA.model;

namespace HM_DBA
{
    public class UserData
    {
        
        public string GetUsersPassword(SqlConnection conn,string username)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            string returnValue;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE username = '" + username+"'" ;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                returnValue = "";
            else
            {
                returnValue = (string)reader["password"];
            }
            conn.Close();
            return returnValue;
        }

        public User Get(SqlConnection conn,string username)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            User returnValue;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE username = '" + username + "'";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                returnValue = null;
            else
            {
                returnValue = new User();
                returnValue.companyId = (int)reader["companyId"];
                returnValue.id = (int)reader["id"];
                returnValue.address = (string)reader["address"];
                returnValue.department = (string)reader["department"];
                returnValue.email = (string)reader["email"];
                returnValue.firstName = (string)reader["firstName"];
                returnValue.username = (string)reader["username"];
                returnValue.lastName = (string)reader["lastName"];
                returnValue.isAdmin = (bool)reader["isAdmin"];
                returnValue.isManager = (bool)reader["isManager"];
                returnValue.password  = (string)reader["password"];
                returnValue.phone = (string)reader["phone"];
                returnValue.role = (string)reader["role"];
            }
            conn.Close();
            return returnValue;
        }

        public List<User> GetByCompanyId(SqlConnection conn, int companyId)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            List<User> returnValue = new List<User>();
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE companyId = " + companyId;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            while(reader.Read())
            { 
                var user = new User();
                user.companyId = (int)reader["companyId"];
                user.id = (int)reader["id"];
                user.address = (string)reader["address"];
                user.department = (string)reader["department"];
                user.email = (string)reader["email"];
                user.firstName = (string)reader["firstName"];
                user.username = (string)reader["username"];
                user.lastName = (string)reader["lastName"];
                user.isAdmin = (bool)reader["isAdmin"];
                user.isManager = (bool)reader["isManager"];
                user.password = (string)reader["password"];
                user.phone = (string)reader["phone"];
                user.role = (string)reader["role"];
                returnValue.Add(user);
            }
            conn.Close();
            return returnValue;
        }

        internal bool CheckExist(SqlConnection conn, int id)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            bool returnValue = false;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE id = " + id;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                returnValue = true;
            conn.Close();
            return returnValue;
        }

        public bool CheckExist(SqlConnection conn, string username)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            bool returnValue = false;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE username = '" + username + "'";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                returnValue = true;
            conn.Close();
            return returnValue;

         }

        public int GetLast(SqlConnection conn)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            int max = 0;
            conn.Open();
            query.CommandText = "SELECT * FROM Users";
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
            return max+1;
        }

        internal User Get(SqlConnection conn, int id)
        {

            var query = new SqlCommand();
            SqlDataReader reader;
            User returnValue;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE id = " + id;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            if (!reader.Read())
                returnValue = null;
            else
            {
                returnValue = new User();
                returnValue.companyId = (int)reader["companyId"];
                returnValue.id = (int)reader["id"];
                returnValue.address = (string)reader["address"];
                returnValue.department = (string)reader["department"];
                returnValue.email = (string)reader["email"];
                returnValue.firstName = (string)reader["firstName"];
                returnValue.username = (string)reader["username"];
                returnValue.lastName = (string)reader["lastName"];
                returnValue.isAdmin = (bool)reader["isAdmin"];
                returnValue.isManager = (bool)reader["isManager"];
                returnValue.password = (string)reader["password"];
                returnValue.phone = (string)reader["phone"];
                returnValue.role = (string)reader["role"];
            }
            conn.Close();
            return returnValue;
        }

        public void Create(SqlConnection conn, int companyId, User user)
        {
            var query = new SqlCommand();

            query.CommandText = "INSERT INTO Users (id, username, password, firstName, lastName, email" +
                                ", phone, address, department, role, isManager, isAdmin, companyId" + ") VALUES ('"
                                 + GetLast(conn) + "', '" + user.username + "', '" + user.password + "', '"
                                 + user.firstName + "', '" + user.lastName+  "', '" + user.email 
                                 + "', '" + user.phone + "', '" + user.address + "', '" + user.department + "', '" +
                                 user.role + "', '" + user.isManager + "', '" + user.isAdmin + "', '" + companyId
                                 + "');";
            query.CommandType = CommandType.Text;
            conn.Open();
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }


        public void DeleteByCompany(SqlConnection conn,int companyId)
        {
            var query = new SqlCommand();

            conn.Open();
            query.CommandText = "DELETE FROM Users WHERE companyId = " + companyId;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }

        public List<User> GetAll(SqlConnection conn)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            List<User> returnValue = new List<User>();
            conn.Open();
            query.CommandText = "SELECT * FROM Users";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();
            while (reader.Read())
            {
                var user = new User();
                user.companyId = (int)reader["companyId"];
                user.id = (int)reader["id"];
                user.address = (string)reader["address"];
                user.department = (string)reader["department"];
                user.email = (string)reader["email"];
                user.firstName = (string)reader["firstName"];
                user.username = (string)reader["username"];
                user.lastName = (string)reader["lastName"];
                user.isAdmin = (bool)reader["isAdmin"];
                user.isManager = (bool)reader["isManager"];
                user.password = (string)reader["password"];
                user.phone = (string)reader["phone"];
                user.role = (string)reader["role"];
                returnValue.Add(user);
            }

            
            conn.Close();
            return returnValue;
        }

        public void Update(SqlConnection conn, int id, User user)
        {
            var query = new SqlCommand();
            SqlDataReader reader;
            conn.Open();
            query.CommandText = "UPDATE Users SET address = '" + user.address + "', department = '" + user.department +
                                "',email = '" + user.email + "', firstName = '" + user.firstName +
                                "',username = '" + user.username + "', lastName = '" + user.lastName +
                                "',isAdmin = '" + user.isAdmin + "', isManager = '" + user.isManager +
                                "',password = '" + user.password + "', phone = '" + user.phone +
                                "',role = '" + user.role + "' WHERE id = " + id + ";";
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            reader = query.ExecuteReader();

            conn.Close();
            
        }

        public void Delete(SqlConnection conn, int id)
        {
            var query = new SqlCommand();

            conn.Open();
            query.CommandText = "DELETE FROM Users WHERE id = " + id;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            query.ExecuteReader();
            conn.Close();
        }

        public bool CheckIsAdmin(SqlConnection conn, int id)
        {
            var query = new SqlCommand();
            bool response;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE id = " + id;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            var reader = query.ExecuteReader();
            if (reader.Read())
            {
                response = (bool)reader["isAdmin"];
            }
            else
            {
                conn.Close();
                return false;
            }


            conn.Close();
            return response;
        }

        public bool CheckIsUserInCompany(SqlConnection conn,int companyId, int userId)
        {
            var query = new SqlCommand();
            int actualCompany;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE id = " + userId;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            var reader = query.ExecuteReader();
            if (reader.Read())
            {
                actualCompany = (int)reader["companyId"];
            }
            else
            {
                conn.Close();
                return false;
            }


            conn.Close();
            if (actualCompany != companyId)
            {
                return false;
            }
            return true;
        }

        public bool CheckIsUserManager(SqlConnection conn, int companyId, int tokenId)
        {
            var query = new SqlCommand();
            int actualCompany;
            bool isManager;
            conn.Open();
            query.CommandText = "SELECT * FROM Users WHERE id = " + tokenId;
            query.CommandType = CommandType.Text;
            query.Connection = conn;

            var reader = query.ExecuteReader();
            if (reader.Read())
            {
                actualCompany = (int)reader["companyId"];
                isManager = (bool) reader["isManager"];
            }
            else
            {
                conn.Close();
                return false;
            }


            conn.Close();
            if (actualCompany != companyId || !isManager)
            {
                return false;
            }
            return true;
        }
    }
}
