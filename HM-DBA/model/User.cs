using System.Collections.Generic;

namespace HM_DBA.model
{
    public class User
    {
        public int id = 0;
        public string username = "";
        public string password = "";
        public string firstName = "";
        public string lastName = "";
        public string email = "";
        public string phone = "";
        public string address = "";
        public string department = "";
        public string role = "";
        public bool isManager = false;
        public bool isAdmin = false;
        public int companyId = 0;
        public List<Shift> shifts = new List<Shift>();

        public User()
        {
        }

    }
}
