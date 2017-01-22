using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using ConsoleApplication1;
using HM_DBA;
using HM_DBA.model;

namespace OwinSelfhostSample
{
    public class CompanyUsersController : ApiController
    {


        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var companyUsers = request.Content.ReadAsAsync<List<User>>().Result;
            var companyId = id;

            main.DeleteUsersOfCompany(companyId);

            main.AddUsersToCompany(companyId,companyUsers);

            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

    }
}