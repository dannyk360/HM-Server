using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

using ConsoleApplication1.responseModel;
using HM_DBA;
using HM_DBA.model;

namespace OwinSelfhostSample
{
    public class AuthenticateController : ApiController
    {

        // POST api/values 
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            MainAccess main = new MainAccess();
            var requestData = request.Content.ReadAsAsync<User>().Result;
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
                    response.Content = new StringContent("username not found");
                }
                else if (e.ParamName == "password")
                {
                    response.Content = new StringContent("the password is not correct");
                }
                return response;
            }

            Company company = main.GetCompany(user.companyId);

            response.Content = new ObjectContent<AuthenticateResponse>(new AuthenticateResponse(user,company), new JsonMediaTypeFormatter());
   
            return response;

        }

    }
}