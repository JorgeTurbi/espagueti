using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Noticia
    {
        public Noticia()
        {
        }

        //Campos generales del Evento
        private long _id_noticia;
        private string _titulo;

        [DataMemberAttribute]
        public long id_noticia
        {
            get { return _id_noticia; }
            set { _id_noticia = value; }
        }

        [DataMemberAttribute]
        public string titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }
    }
}