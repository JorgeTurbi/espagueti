using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Docencia
    {
        public Docencia()
        {
        }

        //Campos generales del Evento
        private long _id_docencia;
        private string _nombre;
        private string _descripcion;
        private string _comentarios;

        [DataMemberAttribute]
        public long id_docencia
        {
            get { return _id_docencia; }
            set { _id_docencia = value; }
        }

        [DataMemberAttribute]
        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        [DataMemberAttribute]
        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        [DataMemberAttribute]
        public string comentarios
        {
            get { return _comentarios; }
            set { _comentarios = value; }
        }
    }
}