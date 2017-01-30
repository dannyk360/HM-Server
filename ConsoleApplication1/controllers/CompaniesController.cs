using System;
using System.Collections.Generic;
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
        public HttpResponseMessage Get()
        {
            var main = new MainAccess();

            var response = new HttpResponseMessage();
            var companies = main.GetCompanies();
            
            response.Content = new ObjectContent<List<Company>>(companies, new JsonMediaTypeFormatter());
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public HttpResponseMessage Get(int id)
        {
            var main = new MainAccess();

            var response = new HttpResponseMessage();
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
            var response = new HttpResponseMessage();

            if (company.name == "")
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("there is not a name");
                return response;
            }
            if (!main.CheckIfCompanyExist(company.name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("there is a company with that name");
                return response;
            }

            if(user.username == "" || user.password == "")
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("the user must be set with username and password");
                return response;
            }

            if (!main.CheckIfUserExist(user.username))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("there is a user with that name");
                return response;
            }


            var companyId = main.CreateCompany(company);
            
            main.CreateUser(companyId, user);

            response.StatusCode = HttpStatusCode.Created;
            return response;

        }

        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var company = request.Content.ReadAsAsync<Company>().Result;

            main.UpdateCompany(id, company);

            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            return response;

        }

       

    }
}