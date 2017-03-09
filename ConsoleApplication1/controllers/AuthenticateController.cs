using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Remoting.Channels;
using System.Web.Http;
using System.Web.Http.Cors;
using ConsoleApplication1.responseModel;
using HM_DBA;
using HM_DBA.model;
using Newtonsoft.Json.Linq;

namespace OwinSelfhostSample
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class AuthenticateController : ApiController
    {
        public HttpResponseMessage Get(int id,HttpResponseMessage request)
        {
            MainAccess main = new MainAccess();
            var idString = request.Headers.GetValues("token").First();
            var response = new HttpResponseMessage();
            if (id != int.Parse(idString))
            {
                response.Content = new StringContent("the token is not correct");
                response.StatusCode = HttpStatusCode.Conflict;
                return response;
            }

            User user = main.GetUser(id);
            Company company = main.GetCompany(user.companyId);

            response.Content = new ObjectContent<AuthenticateResponse>(new AuthenticateResponse(user, company), new JsonMediaTypeFormatter());

            response.StatusCode = HttpStatusCode.OK;
            return response;

        }

        // POST api/values 
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            MainAccess main = new MainAccess();
            var json = request.Content.ReadAsStringAsync().Result;
            var requestData = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(json);
            var response = new HttpResponseMessage();
            User user;

            try
            {
                user = main.authenticateUser(requestData.username, requestData.password);
            }
            catch (ArgumentOutOfRangeException e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                if (e.ParamName == "username")
                {
                    response.Content = new StringContent("שם משתמש או סיסמא שגויים");
                }
                else if (e.ParamName == "password")
                {
                    response.Content = new StringContent("שם משתמש או סיסמא שגויים");
                }
                return response;
            }

            Company company = main.GetCompany(user.companyId);

            response.Content = new ObjectContent<AuthenticateResponse>(new AuthenticateResponse(user,company), new JsonMediaTypeFormatter());
   
            return response;

        }

    }
}