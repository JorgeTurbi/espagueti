using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace campus_sbs_admin
{
    [DataContractAttribute]
    public class Plantilla_mail
    {
        public Plantilla_mail()
        {
        }

        //Campos generales del Evento
        private long _idPlantillaMail;
        private long _idCurso;
        private long _idQuien;
        private string _nombre_plantilla;
        private string _asunto;
        private string _cuerpo;
        private string _adjuntos;

        [DataMemberAttribute]
        public long idPlantillaMail
        {
            get { return _idPlantillaMail; }
            set { _idPlantillaMail = value; }
        }

        [DataMemberAttribute]
        public long idCurso
        {
            get { return _idCurso; }
            set { _idCurso = value; }
        }

        [DataMemberAttribute]
        public long idQuien
        {
            get { return _idQuien; }
            set { _idQuien = value; }
        }

        [DataMemberAttribute]
        public string nombre_plantilla
        {
            get { return _nombre_plantilla; }
            set { _nombre_plantilla = value; }
        }

        [DataMemberAttribute]
        public string asunto
        {
            get { return _asunto; }
            set { _asunto = value; }
        }

        [DataMemberAttribute]
        public string cuerpo
        {
            get { return _cuerpo; }
            set { _cuerpo = value; }
        }

        [DataMemberAttribute]
        public string adjuntos
        {
            get { return _adjuntos; }
            set { _adjuntos = value; }
        }
    }
}