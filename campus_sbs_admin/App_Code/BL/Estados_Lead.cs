using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Estados_Lead
    {
        public Estados_Lead()
        {
        }

        //Campos generales del Estados Lead
        private long _id_estado;
        private string _nombre;

        [DataMemberAttribute]
        public long id_estado
        {
            get { return _id_estado; }
            set { _id_estado = value; }
        }

        [DataMemberAttribute]
        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
    }
}