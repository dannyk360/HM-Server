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
    public class UsersController : ApiController
    {
        public HttpResponseMessage Get()
        {
            MainAccess main = new MainAccess();

            var response = new HttpResponseMessage();
            var users = main.GetUsers();

            response.Content = new ObjectContent<List<User>>(users, new JsonMediaTypeFormatter());
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        public HttpResponseMessage Get(int id)
        {
            var main = new MainAccess();

            var response = new HttpResponseMessage();

            if (main.CheckIfUserExist(id))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("username exist in the system");
                return response;
            }
            var user = main.GetUser(id);

            response.Content = new ObjectContent<User>(user, new JsonMediaTypeFormatter());
            response.StatusCode = HttpStatusCode.OK;
            return response;

        }

        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            var main = new MainAccess();
            var user = request.Content.ReadAsAsync<User>().Result;
            var result = new HttpResponseMessage();

            if (main.CheckIsUserExist(user.username))
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Content = new StringContent("username exist in the system");
                return result;
            }

            main.CreateUser(user.companyId,user);

            result.StatusCode = HttpStatusCode.Created;
            return result;
        }

        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var user = request.Content.ReadAsAsync<User>().Result;

            var result = new HttpResponseMessage();

            if (main.CheckIfUserExist(id))
            {
                result.Content = new StringContent("the user does not exist");
                result.StatusCode = HttpStatusCode.NotFound;
                return result;
            }

            main.UpdateUser(id,user);
            result.StatusCode = HttpStatusCode.OK;
            return result;

        }

        public HttpResponseMessage Delete(int id)
        {
            var main = new MainAccess();
            var result = new HttpResponseMessage();

            main.DeleteUser(id);

            result.StatusCode = HttpStatusCode.OK;
            return result;
        }
    }
}