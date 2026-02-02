using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Report
    {
        public Report()
        {
        }

        //Campos generales del informe
        private string _days;
        private string _hours;
        private long _num_opens;
        private int _envio;

        [DataMemberAttribute]
        public string days
        {
            get { return _days; }
            set { _days = value; }
        }

        [DataMemberAttribute]
        public string hours
        {
            get { return _hours; }
            set { _hours = value; }
        }

        [DataMemberAttribute]
        public long num_opens
        {
            get { return _num_opens; }
            set { _num_opens = value; }
        }

        [DataMemberAttribute]
        public int envio
        {
            get { return _envio; }
            set { _envio = value; }
        }
    }
}