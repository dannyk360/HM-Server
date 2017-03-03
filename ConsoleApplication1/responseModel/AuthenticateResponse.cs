using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HM_DBA.model;

namespace ConsoleApplication1.responseModel
{
    class AuthenticateResponse
    {
        public User user;
        public Company company;
        public string adminCode;
        public string token;

        public AuthenticateResponse()
        {
            token = "token";
        }

        public AuthenticateResponse(User user, Company company)
        {
            this.user = user;
            this.company = company;
            token = user.id.ToString();
        }
    }
}
