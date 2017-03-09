using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.Cors;
using ConsoleApplication1;

using ConsoleApplication1.responseModel;
using HM_DBA;
using HM_DBA.model;

namespace OwinSelfhostSample
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class CompaniesController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var main = new MainAccess();
            var response = new HttpResponseMessage();
            var idString = request.Headers.GetValues("token").First();

            if (!main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש אינו אדמיניסטרטור");
                return response;
            }
            
            var companies = main.GetCompanies();
            
            response.Content = new ObjectContent<List<Company>>(companies, new JsonMediaTypeFormatter());
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public HttpResponseMessage Get(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var idString = request.Headers.GetValues("token").First();
            var response = new HttpResponseMessage();
            if (!main.isUserInCompany(id, int.Parse(idString)) && !main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש לא קיים בחברה");
                return response;
            }
            var company = main.GetCompany(id);
            if (company == null)
            {

                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            response.Content = new ObjectContent<Company>(company, new JsonMediaTypeFormatter());
            response.StatusCode = HttpStatusCode.OK;
            return response;

        }

        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var main = new MainAccess();

            var requestData = request.Content.ReadAsAsync<AuthenticateResponse>().Result;
            var company = requestData.company;
            var user = requestData.user;
            var adminCode = requestData.adminCode;
            var response = new HttpResponseMessage();

            if (company.name == "")
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("חסר שם משתמש");
                return response;
            }
            if (!main.CheckIfCompanyExist(company.name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("קיימת חברה עם השם הנבחר");
                return response;
            }

            if(user.username == "" || user.password == "")
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("חייב להזין שם משתמש ושם חברה");
                return response;
            }

            if (!main.CheckIfUserExist(user.username))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("קיים שם משתמש כזה במערכת");
                return response;
            }


            var companyId = main.CreateCompany(company);

            if (adminCode == "braude")
                user.isAdmin = true;

            main.CreateUser(companyId, user);

            response.StatusCode = HttpStatusCode.Created;
            response.Content = new StringContent("");
            return response;

        }

        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var company = request.Content.ReadAsAsync<Company>().Result;
            var idString = request.Headers.GetValues("token").First();
            var response = new HttpResponseMessage();

            if (!main.IsUserManager(id, int.Parse(idString)) && !main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש לא קיים בחברה");
                return response;
            }

            if (main.CheckIfCompanyExist(company.name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("שם החברה כבר קיים");
                return response;
            }

            main.UpdateCompany(id, company);

            response.StatusCode = HttpStatusCode.OK;

            return response;

        }

        public HttpResponseMessage Delete(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var response = new HttpResponseMessage();
            var idString = request.Headers.GetValues("token").First();

            if (!main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש אינו אדמיניסטרטור");
                return response;
            }

            main.DeleteCompany(id);

            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

    }
}