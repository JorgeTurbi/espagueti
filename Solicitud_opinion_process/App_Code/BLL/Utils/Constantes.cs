using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solicitud_opinion_process
{
    class Constantes
    {
        #region Enumeración para Activo

        public enum activo
        {
            Activo = 1,
            NoActivo = 0
        }

        #endregion

        #region Enumeración para Cursos

        public enum course
        {
            Sin_determinar = 22
        }

        #endregion

        #region Enumeración para type_clase

        public enum type_clase
        {
            Clase_presencial = 1,
            Clase_online = 2
        }

        #endregion

        #region Enumeración para type_agenda

        public enum type_agenda
        {
            Otras = 1,
            Fecha_entrega = 2,
            Clase_presencial = 3,
            Clase_online = 4
        }

        #endregion

        #region Enumeración para el log

        public enum log
        {
            [Utils.StringValue("Acceso OK")] Acceso_OK,
            [Utils.StringValue("Acceso Error")] Acceso_error,
            [Utils.StringValue("Mandar Password")] Mandar_Password,
            [Utils.StringValue("Ver Anuncio")] Ver_anuncio,
            [Utils.StringValue("Ver Programa")] Ver_programa,
            [Utils.StringValue("Ver Documentos")] Ver_documentos,
            [Utils.StringValue("Mandar Mensaje")] Mandar_mensaje,
            [Utils.StringValue("Acceso Foro")] Acceso_foro,
            [Utils.StringValue("Activar Usuario")] Activar_usuario,
            [Utils.StringValue("Descargar pdf curso")] Descargar_pdf_curso,
            [Utils.StringValue("Ver caso práctico")] Ver_caso_practico,
            [Utils.StringValue("Abrir enunciado caso práctico")] Abrir_enunciado_caso_practico,
            [Utils.StringValue("Abrir feedback caso práctico")] Abrir_feedback_caso_practico,
            [Utils.StringValue("Error al recibir datos de terceros")] Error_integracion
        }

        #endregion

        #region Enumeración para type_message

        public enum type_message
        {
            Aviso = 1,
            Mensaje = 2,
            Contenido = 3,
            Email = 4
        }

        #endregion

        #region Enumeración para type_auto_curso

        public enum type_auto_curso
        {
            type_foro = 1,
            type_msg_foro = 2,
            type_msg = 3,
            type_contenido = 4,
            type_cp = 5
        }

        #endregion
    }
}
