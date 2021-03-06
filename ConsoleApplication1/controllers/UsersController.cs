﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using ConsoleApplication1;
using HM_DBA;
using HM_DBA.model;

namespace OwinSelfhostSample
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            MainAccess main = new MainAccess();

            var response = new HttpResponseMessage();
            var idString = request.Headers.GetValues("token").First();

            if (!main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש אינו אדמיניסטרטור");
                return response;
            }
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
                response.Content = new StringContent("שם המשתמש קיים במערכת");
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
            var response = new HttpResponseMessage();

            var idString = request.Headers.GetValues("token").First();

            if (!main.IsUserManager(main.GetUser(user.id).companyId , int.Parse(idString)) && !main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש לא קיים בחברה");
                return response;
            }

            if (main.CheckIsUserExist(user.username))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("שם המשתמש קיים כבר במערכת");
                return response;
            }

            main.CreateUser(user.companyId,user);

            response.StatusCode = HttpStatusCode.Created;
            return response;
        }

        public HttpResponseMessage Post(int id,HttpRequestMessage request)
        {
            var main = new MainAccess();
            var response = new HttpResponseMessage();
            if (id != -1)
            {
                response.Content = new StringContent("id is not -1");
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            var userIds = request.Content.ReadAsAsync<List<int>>().Result;
            if (!userIds.Any())
            {
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            var idString = request.Headers.GetValues("token").First();

            if (!main.IsUserManager(main.GetUser(userIds[0]).companyId, int.Parse(idString)) && !main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש לא קיים בחברה");
                return response;
            }

            userIds.ForEach((userId) => main.DeleteUser(userId));

            response.StatusCode = HttpStatusCode.Created;
            return response;
        }
        public HttpResponseMessage Put(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var user = request.Content.ReadAsAsync<User>().Result;

            var response = new HttpResponseMessage();

            var idString = request.Headers.GetValues("token").First();

            if (!main.isUserInCompany(main.GetUser(id).companyId, int.Parse(idString)) && !main.IsUserManager(main.GetUser(user.id).companyId, int.Parse(idString)) && !main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש לא קיים בחברה");
                return response;
            }

            if (main.CheckIfUserExist(id))
            {
                response.Content = new StringContent("המשתמש לא קיים במערכת");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            main.UpdateUser(id,user);
            response.StatusCode = HttpStatusCode.OK;
            return response;

        }

        public HttpResponseMessage Delete(int id, HttpRequestMessage request)
        {
            var main = new MainAccess();
            var response = new HttpResponseMessage();
            var idString = request.Headers.GetValues("token").First();

            if (!main.IsUserManager(main.GetUser(id).companyId, int.Parse(idString)) && !main.CheckIsAdmin(int.Parse(idString)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("המשתמש לא קיים בחברה");
                return response;
            }

            main.DeleteUser(id);

            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}