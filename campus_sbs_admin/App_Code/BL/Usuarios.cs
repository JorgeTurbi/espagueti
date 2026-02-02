using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Usuarios
    {
        public Usuarios()
        {
        }

        //Campos generales del Evento
        private long _id_usuario;
        private string _nombre_completo;
        private string _nombre;
        private string _apellidos;
        private string _email;

        [DataMemberAttribute]
        public long id_usuario
        {
            get { return _id_usuario; }
            set { _id_usuario = value; }
        }

        [DataMemberAttribute]
        public string nombre_completo
        {
            get { return _nombre_completo; }
            set { _nombre_completo = value; }
        }

        [DataMemberAttribute]
        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        [DataMemberAttribute]
        public string apellidos
        {
            get { return _apellidos; }
            set { _apellidos = value; }
        }

        [DataMemberAttribute]
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
    }
}