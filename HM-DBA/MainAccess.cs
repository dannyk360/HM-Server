using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HM_DBA.model;

namespace HM_DBA
{
    public class MainAccess
    {
        private ShiftData shifts;
        private UserData users;
        private VisaData visas;
        private CompanyData companies;
        public SqlConnection connection=null;
        public MainAccess()
        {
            connection = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=HM_DB;Integrated Security=True");
            shifts = new ShiftData();
            users = new UserData();
            visas = new VisaData();
            companies = new CompanyData();
        }

        public User authenticateUser(string username,string password)
        {
            var acctualPassword = users.GetUsersPassword(connection,username);
            if(acctualPassword.Equals(""))
                throw new ArgumentOutOfRangeException("username");

            if(!acctualPassword.Equals(password))
                throw new ArgumentOutOfRangeException("password");
            
            var user =  users.Get(connection,username);
            user.shifts = shifts.GetById(connection, user.id);

            return user;
        }

        public bool CheckIsUserExist(int id)
        {
            throw new NotImplementedException();
        }

        public Company GetCompany(int companyId)
        {
            Company company =  companies.Get(connection,companyId);
            company.employees = users.GetByCompanyId(connection, companyId);
            company.employees.ForEach(user => user.shifts = shifts.GetById(connection, user.id));
            company.visa = visas.Get(connection, company.visaId);

            return company;
        }

        public List<Company> GetCompanies()
        {
            var allCompanies = companies.GetAll(connection);
            allCompanies.ForEach(company =>
            {
                company.visa = visas.Get(connection, company.id);
                company.employees = users.GetByCompanyId(connection, company.id);
                company.employees.ForEach(userInComapny =>
                {
                    userInComapny.shifts = shifts.GetById(connection, userInComapny.id);
                });
            } );
            return allCompanies;
        }

        public bool CheckIfUserExist(int id)
        {
            return users.CheckExist(connection, id);
        }

        public bool CheckIfCompanyExist(string companyName)
        {
            return companies.CheckExist(connection, companyName);
        }

        public bool CheckIfUserExist(string username)
        {
            return users.CheckExist(connection, username);
        }

        public int CreateCompany(Company company)
        {
            company.visaId = visas.GetLastId(connection) + 1;
            var companyId = companies.Create(connection, company, companies.GetLastId(connection));
            visas.Create(connection, company);
            return companyId;
        }

        public void CreateUser(int companyId, User user)
        {
            user.id = users.GetLast(connection)+1;
            users.Create(connection,companyId, user);
            
        }

        public void UpdateCompany(int id, Company company)
        {
            var visaId = companies.Update(connection, id, company);
            visas.Update(connection,visaId,company.visa);
        }

        public void DeleteUsersOfCompany(int companyId)
        {
            var usersIds = users.GetByCompanyId(connection, companyId).Select(user => user.id).ToList();
            usersIds.ForEach(userId => shifts.DeleteByUser(connection,userId));
            users.DeleteByCompany(connection,companyId);

        }

        public void AddUsersToCompany(int companyId,List<User> companyUsers)
        {
            companyUsers.ForEach(userInCompany =>
            {
                users.Create(connection, companyId, userInCompany);
                var usersIds = users.GetByCompanyId(connection, companyId).Select(user => user.id).ToList();
                usersIds.ForEach(userId => shifts.Create(connection, userId, userInCompany.shifts));
            });
        }

        public List<User> GetUsers()
        {
            var allUsers = users.GetAll(connection);
            allUsers.ForEach(user => user.shifts = shifts.GetById(connection,user.id));
            return allUsers;
        }

        public User GetUser(int id)
        {
            var user = users.Get(connection, id);
            if (user == null)
                return null;
            user.shifts = shifts.GetById(connection, user.id);
            return user;
        }

        public bool CheckIsUserExist(string username)
        {
            return users.CheckExist(connection, username);
        }

        public void UpdateUser(int id,User user)
        {
            users.Update(connection,id, user);
            shifts.DeleteByUser(connection,id);
            shifts.Create(connection,id,user.shifts);
        }

        public void DeleteUser(int id)
        {
            users.Delete(connection,id);
            shifts.DeleteByUser(connection,id);
        }

        public bool CheckIsAdmin(int userId)
        {
            return users.CheckIsAdmin(connection, userId);
        }

        public bool isUserInCompany(int companyId, int userId)
        {
            return users.CheckIsUserInCompany(connection, companyId, userId);
        }

        public bool IsUserManager(int companyId, int tokenId)
        {
            return users.CheckIsUserManager(connection,companyId, tokenId);
        }

        public void DeleteCompany(int id)
        {
            var usersInCompany = users.GetByCompanyId(connection, id);
            usersInCompany.ForEach(user =>
            {
                shifts.DeleteByUser(connection, user.id);
                users.Delete(connection,user.id);
            });
            visas.DeleteByCompany(connection, companies.Get(connection,id).visaId);
            companies.Delete(connection, id);
        }
    }
}
