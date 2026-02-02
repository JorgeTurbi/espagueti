using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Provinces
    {
        public Provinces()
        {
        }

        //Campos generales del Evento
        private long _id_provincia;
        private long _id_pais;
        private string _nombre;
        private string _descripcion;
        private string _valor;

        [DataMemberAttribute]
        public long id_provincia
        {
            get { return _id_provincia; }
            set { _id_provincia = value; }
        }

        [DataMemberAttribute]
        public long id_pais
        {
            get { return _id_pais; }
            set { _id_pais = value; }
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
        public string valor
        {
            get { return _valor; }
            set { _valor = value; }
        }
    }
}