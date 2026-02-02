using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Listado_Suscripciones
    {
        public Listado_Suscripciones()
        {
        }

        //Campos generales del Evento
        private long _id_els;
        private string _nombre;
        private DateTime _fecha_alta;
        private int? _num_total;
        private int? _num_actual;
        private int? _num_bajas;
        private string _historico;
        private long? _id_usuario;
        private bool? _auto;
        private DateTime? _fecha_actualizacion;
        private string _sql;

        [DataMemberAttribute]
        public long id_els
        {
            get { return _id_els; }
            set { _id_els = value; }
        }

        [DataMemberAttribute]
        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        
        [DataMemberAttribute]
        public DateTime fecha_alta
        {
            get { return _fecha_alta; }
            set { _fecha_alta = value; }
        }
        
        [DataMemberAttribute]
        public int? num_total
        {
            get { return _num_total; }
            set { _num_total = value; }
        }

        [DataMemberAttribute]
        public int? num_actual
        {
            get { return _num_actual; }
            set { _num_actual = value; }
        }

        [DataMemberAttribute]
        public int? num_bajas
        {
            get { return _num_bajas; }
            set { _num_bajas = value; }
        }

        [DataMemberAttribute]
        public string historico
        {
            get { return _historico; }
            set { _historico = value; }
        }

        [DataMemberAttribute]
        public long? id_usuario
        {
            get { return _id_usuario; }
            set { _id_usuario = value; }
        }

        [DataMemberAttribute]
        public bool? auto
        {
            get { return _auto; }
            set { _auto = value; }
        }

        [DataMemberAttribute]
        public DateTime? fecha_actualizacion
        {
            get { return _fecha_actualizacion; }
            set { _fecha_actualizacion = value; }
        }

        [DataMemberAttribute]
        public string sql
        {
            get { return _sql; }
            set { _sql = value; }
        }
    }
}