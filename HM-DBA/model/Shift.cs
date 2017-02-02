using System;

namespace HM_DBA.model
{
    public class Shift
    {
        public string date = "";
        public string start = "";
        public string end ="";
        public string comment = "";
        public Shift()
        {
        }

        public Shift(string date, string start, string end, string comment)
        {
            this.date = date;
            this.start = start;
            this.end = end;
            this.comment = comment;
        }
    }
}
