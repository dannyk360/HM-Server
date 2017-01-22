using System;

namespace HM_DBA.model
{
    public class Shift
    {
        public DateTime date = new DateTime();
        public DateTime start = new DateTime();
        public DateTime end = new DateTime();
        public string comment = "";
        public Shift()
        {
        }

        public Shift(DateTime date, DateTime start, DateTime end, string comment)
        {
            this.date = date;
            this.start = start;
            this.end = end;
            this.comment = comment;
        }
    }
}
