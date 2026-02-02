using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace campus_sbs_admin
{
    public class Constantes
    {
        #region Enumeración para Activo

        public enum activo
        {
            Activo = 1,
            NoActivo = 0
        }

        #endregion

        #region Enumeración para Visible

        public enum visible
        {
            Visible = 1,
            NoVisible = 0
        }

        #endregion

        #region Constantes para Scoring

        public enum scoring
        {
            scoring_Pag = 3,
            scoring_Open_Mail = 5,
            scoring_ClickMail = 10,
            scoring_Registro = 50

            /*
            [Utils.StringValue("3")]
            scoring_Pag,
            [Utils.StringValue("5")]
            scoring_Curso,
            [Utils.StringValue("10")]
            scoring_Carrito,
            [Utils.StringValue("2")]
            scoring_Documento,
            [Utils.StringValue("15")]
            scoring_Form,
            [Utils.StringValue("50")]
            scoring_Registro,
            [Utils.StringValue("10")]
            scoring_ClickMail,
            [Utils.StringValue("5")]
            scoring_Open_Mail,
            [Utils.StringValue("5")]
            scoring_NurturingClickMail,
            [Utils.StringValue("15")]
            scoring_NurturingDownloadPdf*/
        }

        #endregion

        #region Constantes para Origenes

        public enum origen
        {
            Web = 103,
            Alumno_Ex = 113,
            Email_MK = 248,
            SBS_Admin = 318
        }

        #endregion

        #region Constantes para Acciones

        public enum accion
        {
            Dar_baja = 52,
            Informacion_general_becas = 91,
            Peticion_informacion = 99,
            Matriculacion = 100,
            Visita = 118,
            Carrito = 119,
            Datos_Form = 186,
            Send_Mail = 190,
            Open_Mail = 191,
            Click_Mail = 192,
            Download_File = 193,
            Compra = 194,
            Compra2 = 195,
            Compra3 = 196,
            Compra4 = 197,
            Landing = 242,
            Exito_Landing = 243,
            Accion_Manual = 253,
            Send_Activation_PDP = 273,
            Identificar_Ip = 342


            



            //[Utils.StringValue("118")] Visita


            /*[Utils.StringValue("91")] Informacion_general_becas,
            [Utils.StringValue("99")] Peticion_informacion,
            [Utils.StringValue("100")] Matriculacion,
            ,
            [Utils.StringValue("119")] Carrito,


            [Utils.StringValue("190")] Send_Mail,
            [Utils.StringValue("191")] Open_Mail,
            [Utils.StringValue("192")] Click_Mail,

            [Utils.StringValue("193")] Download_File,
            [Utils.StringValue("194")] Identificacion,
            [Utils.StringValue("196")] TPV,
            [Utils.StringValue("197")] Fin_compra,
            [Utils.StringValue("243")] Exito_Landing,
            [Utils.StringValue("248")] Email_MK,
            [Utils.StringValue("268")] Baja_Parcial,
            [Utils.StringValue("269")] Reactivacion_Alta,
            [Utils.StringValue("276")] Nurturing_Open_Mail,
            [Utils.StringValue("277")] Nurturing_Click_Mail,
            [Utils.StringValue("280")] Skype,
            [Utils.StringValue("281")] Estudios_presenciales,
            [Utils.StringValue("291")] Nurturing_Inbound_Marketing,
            [Utils.StringValue("292")] Nurturing_Ver_Recurso,
            [Utils.StringValue("295")] Nurturing_General,
            [Utils.StringValue("306")] Promo_Web*/
        }

        #endregion

        #region Enumeración para Cursos

        public enum course
        {
            Sin_determinar = 22,
            Incidencia = 359
        }

        #endregion

        #region Enumeración para el log

        public enum log
        {
            [Utils.StringValue("Acceso OK")]
            Acceso_OK,
            [Utils.StringValue("Acceso Error")]
            Acceso_error,
            [Utils.StringValue("Mandar Password")]
            Mandar_Password,
            [Utils.StringValue("Ver Anuncio")]
            Ver_anuncio,
            [Utils.StringValue("Ver Programa")]
            Ver_programa,
            [Utils.StringValue("Ver Documentos")]
            Ver_documentos,
            [Utils.StringValue("Mandar Mensaje")]
            Mandar_mensaje,
            [Utils.StringValue("Acceso Foro")]
            Acceso_foro,
            [Utils.StringValue("Activar Usuario")]
            Activar_usuario,
            [Utils.StringValue("Descargar pdf curso")]
            Descargar_pdf_curso,
            [Utils.StringValue("Ver caso práctico")]
            Ver_caso_practico,
            [Utils.StringValue("Abrir enunciado caso práctico")]
            Abrir_enunciado_caso_practico,
            [Utils.StringValue("Abrir feedback caso práctico")]
            Abrir_feedback_caso_practico,
            [Utils.StringValue("Error al recibir datos de terceros")]
            Error_integracion,
            [Utils.StringValue("Baja usuario")]
            Baja_usuario
        }

        #endregion

        #region Enumeración para el calification

        public enum calification
        {
            [Utils.StringValue("No presentado")]
            No_presentado,
            [Utils.StringValue("No Apto")]
            No_apto,
            [Utils.StringValue("Apto")]
            Apto,
            [Utils.StringValue("Apto-")]
            Apto_min,
            [Utils.StringValue("Apto+")]
            Apto_max,
            [Utils.StringValue("Apto ME")]
            Apto_ME
        }

        #endregion

        #region Enumeración Informe Encuestas

        public enum encuestas
        {
            [Utils.StringValue("CURSO")]
            inf_curso,
            [Utils.StringValue("DOCENCIA")]
            inf_docencia,
            [Utils.StringValue("PROGRAMA")]
            inf_programa,
            [Utils.StringValue("PROGRAMA WEB")]
            inf_programa_web
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

        #region Enumeración para desistimiento

        public enum desistimiento
        {
            [Utils.StringValue("Aceptado desistimiento")]
            desistimiento_ok,
            [Utils.StringValue("Rechazado desistimiento")]
            desistimiento_ko
        }

        #endregion

        #region Enumeración para type_recurso

        public enum type_recurso
        {
            Nota_Tecnica = 1,
            Caso_Practico = 2,
            Multimedia = 3,
            Examen = 4
        }

        #endregion

        #region Enumeración Tipo Auxiliar

        public enum aux
        {
            [Utils.StringValue("NIVEL_EDUCA")] nivel_educacion,
            [Utils.StringValue("TIPO_EXP")] experiencia,
            [Utils.StringValue("TIPO_SITUACION")] situacion,
            [Utils.StringValue("CARGO")] cargo,
            [Utils.StringValue("ORIGEN")] origen,
            [Utils.StringValue("TIPO_DATO")] links,
            [Utils.StringValue("PERTENENCIA")] pertenencia,
            [Utils.StringValue("METODOLOGIA")] metodologia,
            [Utils.StringValue("CAPTACION_TIPO")] captacion_tipo,
            [Utils.StringValue("ACCESO_POR")] acceso_por
        }

        #endregion

        #region Enumeración Paises

        public enum pais
        {
            Spain = 1,
            France = 4,
            Germany = 5,
            UK = 6,
            Others = 7
        }

        #endregion        

        #region Enumeración Provincias

        public enum provincias
        {
            Others = 62,
            Sin_Asignar = 65
        }

        #endregion        

        #region Enumeración Area funcional

        public enum area_funcional
        {
            LIB = 11
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

        #region Enumeración para type_clase

        public enum type_clase
        {
            Clase_presencial = 1,
            Clase_online = 2
        }

        #endregion

        #region Enumeración Links Redes Sociales

        public enum links
        {
            Blog = 41,
            Facebook = 42,
            Linkedin = 43,
            Trabajo = 44,
            Twitter = 45,
            Web = 46
        }

        #endregion

        #region Enumeración para Status Campaña

        public enum status_newsletter
        {
            Pendiente = 1,
            Enviando = 2,
            Enviado = 3,
            Cerrado = 4
        }

        #endregion

        #region Enumeración para Status Mail

        public enum status_mail
        {
            Not_Send = 0,
            Send = 1,
            Open = 2,
            Bounced = 3,
            Clic = 4,
            Reenviado = 5,
            Open_Padre = 6
        }

        #endregion

        #region Enumeración para usuario especial mails

        public enum usuario_especial_mail
        {
            Aniacam = 19820,
            Sbs_Comunicacion = 19819,
            Munuslingua = 47066
        }

        #endregion
        
        public enum ImagenResponsive
        {
            Grande = 1,
            Mediana = 2
        }

        public enum ImagenSituacion
        {
            Portada = 1,
            Interior = 2
        }

        #region Enumeración para Status Campaña

        public enum status_campaign
        {
            Planificado = 1,
            Proceso = 2,
            Finalizado = 3,
            Reenvio_open = 4,
            Reenvio_no_opens = 5,
            Reenvio_clicks = 6
        }

        #endregion

        #region Enumeración para Tipos de acciones de las reglas

        public enum type_action_rule
        {
            Comun = 1,
            Reasignar = 2,
            Mail = 3,
            Seguimiento = 4
        }

        #endregion

        #region Constantes para type_action_canal

        public enum type_action_canal
        {
            action_mail = 1,
            action_phone = 2,
            action_whatsapp = 3,
            action_advise = 4
        }

        #endregion

        #region Enumeración para type_status_action

        public enum type_status_action
        {
            status_nuevo = 0,
            status_rechazado = 1,
            status_duplicado = 2,
            status_futuro = 3,
            status_cerrar = 4,
            status_sin_contactar = 5,
            status_indeciso = 6,
            status_interesado = 7,
            status_send = 8,
            status_receive = 9,
            status_pago = 10,
            status_matriculado = 11,
            status_proceso = 12
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

        #region Enumeración para tab_position_AP

        public enum tab_position_AP
        {
            nuevos = 0,
            avisos = 1,
            sin_contactar = 2,
            proceso = 3,
            futuro = 4,
            todos = 5
        }

        #endregion

        #region Enumeración Links Redes Sociales

        public enum links_rrss
        {
            Blog = 41,
            Facebook = 42,
            Linkedin = 43,
            Web_trabajo = 44,
            Twitter = 45,
            Web = 46
        }

        #endregion

        #region Enumeración Situaciones laborales

        public enum situacion_laboral
        {
            Sin_empleo = 60,
            Trabajador = 61,
            Emprendedor = 62,
            Estudiante = 63,
            Practicas = 271
        }

        #endregion

        #region Enumeración Tipo Curso

        public enum tipo_curso
        {
            Gratuito = 0,
            Curso = 1,
            Programa = 2,
            Postgrado = 3,
            Master = 4,
            Seminario = 5,
            Experto_Universitario = 6,
            Master_presencial = 7,
            Master_semipresencial = 8,
            Master_online = 9,
            Master_fundamentals = 10
        }

        #endregion

        #region Enumeración Tipo Ficha Aux

        public enum tipo_ficha_aux
        {
            peticion_informacion = 1,
            origen = 2,
            link = 3,
            fundacion = 4,
            cambio_edicion = 5,
            precio_fin_acceso = 6,
            asignacion_comercial = 7,
            asignacion_comercial_all = 8,
            pago = 9,
            documento = 10,
            informe_tripartita = 11,
            comentarios = 12,
            avance = 13,
            unificar_usuarios = 14,
            incidencia = 15
        }

        #endregion

        #region Constantes para tipo_accion_comercial

        public enum tipo_accion_comercial
        {
            [Utils.StringValue("S")] Seguimiento,
            [Utils.StringValue("F")] Fundacion
        }

        #endregion

        #region Constantes para motivos

        public enum motivo_rechazo
        {
            Usuario_falso = 1,
            Usuario_no_contactable = 2,
            Usuario_no_permite_explicar = 3,
            Usuario_no_pide_info = 4
        }

        #endregion

        #region Enumeración Productos

        public enum producto
        {
            SBS_Life = 1
        }

        #endregion
    }
}