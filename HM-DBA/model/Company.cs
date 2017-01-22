using System.Collections.Generic;

namespace HM_DBA.model
{
    public class Company
    {
        public int id;
        public string name;
        public string address;
        public string field;
        public Visa visa;
        public List<User> users;
        public int visaId;
        public Company()
        {
        }

        public Company(Visa visa)
        {
            this.visa = visa;
        }
    }
}
