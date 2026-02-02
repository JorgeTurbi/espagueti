using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class seguimiento_comercial_crm : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el usuario
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (Session["usuario"] != null)
                list_user.Add((CLIENTES)Session["usuario"]);
            else
                Response.Redirect("login.aspx");
            
            if (!IsPostBack)
            {
                /// Comprobar al usuario
                bool comprobate_user = Utilities.comprobate_users(list_user[0]);
                if (!comprobate_user)
                    Response.Redirect("login.aspx");
                else
                {
                    /// 1.- Sacar los datos obligatorios
                    long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                    long idPeticion = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;
                    long idSeguimiento = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;
                    if (idUsuario > 0 && idPeticion > 0)
                        cargar_datos(list_user[0], idUsuario, idPeticion, idSeguimiento);
                    else if (idUsuario > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        Response.Redirect("listado-leads-crm.aspx");

                    /// 2.- Botón de volver
                    btn_back.HRef = "ficha-alumno-crm.aspx?idu=" + idUsuario;

                    /// 3.- Datos de los fileuploads
                    file_adjunto.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre el adjunto en el área seleccionada</span><input id='fileupload_adjunto' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idu\": \"" + idUsuario + "\", \"type\": \"file_mail\", \"accion\": \"update\" }' />";
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
            long idPeticion = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;
            long idSeguimiento = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;
            List<CLIENTES> _comercial = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comercial.Add((CLIENTES)Session["usuario"]);
            long idResultado = -1;

            try
            {
                /// 1.- Recuperar los datos del formulario
                DateTime fecha_seguimiento = DateTime.Parse(txtFechaSeg.Value).AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second).AddMilliseconds(DateTime.Now.Millisecond);
                long idCurso = long.Parse(ddlCurso.Value);

                /// 1.1.- Acción comercial
                int accion_comercial = 0;
                if (action_mail.Checked)
                    accion_comercial = int.Parse(action_mail.Value);
                else if (action_phone.Checked)
                    accion_comercial = int.Parse(action_phone.Value);
                else if (action_whatsapp.Checked)
                    accion_comercial = int.Parse(action_whatsapp.Value);
                else
                    accion_comercial = int.Parse(action_advise.Value);

                /// 1.2.- Estados
                int estado = 0;
                if (status_sin_contactar.Checked)
                    estado = int.Parse(status_sin_contactar.Value);
                else if (status_indeciso.Checked)
                    estado = int.Parse(status_indeciso.Value);
                else if (status_interesado.Checked)
                    estado = int.Parse(status_interesado.Value);
                else if (status_send.Checked)
                    estado = int.Parse(status_send.Value);
                else if (status_receive.Checked)
                    estado = int.Parse(status_receive.Value);
                else if (status_pago.Checked)
                    estado = int.Parse(status_pago.Value);
                else if (status_futuro.Checked)
                    estado = int.Parse(status_futuro.Value);
                else if (status_duplicado.Checked)
                    estado = int.Parse(status_duplicado.Value);
                else if (status_rechazado.Checked)
                    estado = int.Parse(status_rechazado.Value);
                else if (status_cerrar.Checked)
                    estado = int.Parse(status_cerrar.Value);
                else if (status_matriculado.Checked)
                    estado = int.Parse(status_matriculado.Value);

                /// 1.3.- Sacar el motivo si es un status rechazado
                int _motive = 0;
                if (estado == (int)Constantes.type_status_action.status_rechazado)
                {
                    if (motive1.Checked)
                        _motive = int.Parse(motive1.Value);
                    else if (motive2.Checked)
                        _motive = int.Parse(motive2.Value);
                    else if (motive3.Checked)
                        _motive = int.Parse(motive3.Value);
                    else if (motive4.Checked)
                        _motive = int.Parse(motive4.Value);
                }

                /// 2.- Procesar la AP
                if (idSeguimiento > 0)
                {
                    /// 2.1.- Recuperar los datos del seguimiento
                    List<campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercialById(idSeguimiento);
                    if (_seguimientos.Count == 1)
                    {
                        /// 2.2.- Recordatorio
                        if (accion_comercial == (int)Constantes.type_action_canal.action_advise && (_seguimientos[0].recordatorio != null && _seguimientos[0].recordatorio.Value))
                        {
                            int _hours = int.Parse(ddlHour.Value.Split(':')[0]);
                            int _minutes = int.Parse(ddlHour.Value.Split(':')[1]);
                            DateTime _fecha = DateTime.Parse(txtDateProg.Value).AddHours(_hours).AddMinutes(_minutes);

                            string sComentario = txt_comentarios.Value.Trim(); // Comentario de la Acción

                            campus_SEG_COMERCIAL _seguimiento = _seguimientos[0];
                            _seguimiento.fecha = _fecha;
                            _seguimiento.Comentarios = sComentario;

                            bool _update_seguimiento = da.updateSegComercial(_seguimiento);
                            if (_update_seguimiento)
                            {
                                /// 3.0.- Poner el idResultado
                                idResultado = 1;

                                /// 3.1.- Actualizar la AP asociada
                                List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAP(idPeticion);
                                if (_leads.Count == 1)
                                {
                                    /// 3.2.- Recuperar los datos del resumen
                                    string resumen_metodologia = null;
                                    if (method_online.Checked)
                                        resumen_metodologia = method_online.Value;
                                    else if (method_semi.Checked)
                                        resumen_metodologia = method_semi.Value;

                                    string resumen_cuando = null;
                                    if (cuando_menor_7.Checked)
                                        resumen_cuando = cuando_menor_7.Value;
                                    else if (cuando_menor_12.Checked)
                                        resumen_cuando = cuando_menor_12.Value;
                                    else if (cuando_mayor_12.Checked)
                                        resumen_cuando = cuando_mayor_12.Value;

                                    string resumen_comentarios = resumen_comentarios_user.Value;

                                    /// 3.3.- Actualizar el lead                            
                                    campus_ACCIONES_PERSONA _lead = _leads[0];
                                    _lead.Procesado = true;
                                    _lead.idComercial = _comercial[0].id_cliente;
                                    _lead.ult_comentario = sComentario;
                                    _lead.ult_seg_fecha = _seguimiento.fecha;
                                    _lead.resumen_metodologia = resumen_metodologia;
                                    _lead.resumen_cuando = resumen_cuando;
                                    _lead.resumen_comentarios = resumen_comentarios;
                                    _lead.resumen_fecha_act = DateTime.Now;

                                    bool update_client = da.updatePersonAction(_lead);
                                    if (update_client)
                                    {
                                        /// 3.4.- Actualizar los seguimientos comerciales anteriores
                                        List<campus_SEG_COMERCIAL> _seguimientos_comerciales = da.getSeguimientoComercialByAP(idPeticion);
                                        if (_seguimientos_comerciales.Count > 0)
                                        {
                                            /// 3.5.- Recorrer los seguimientos 
                                            _seguimientos_comerciales = _seguimientos_comerciales.Where(_ => _.recordatorio == null || !_.recordatorio.Value || (_.recordatorio.Value && _.fecha < fecha_seguimiento)).ToList();
                                            if (_seguimientos_comerciales.Count > 0)
                                            {
                                                foreach (var _seg_comercial in _seguimientos_comerciales)
                                                {
                                                    campus_SEG_COMERCIAL _seguimiento_comercial = _seg_comercial;
                                                    _seguimiento_comercial.estado = _seguimiento.estado;

                                                    bool _update_seg = da.updateSegComercial(_seguimiento_comercial);
                                                    if (!_update_seg)
                                                    {
                                                        LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercial()");
                                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el seguimiento comercial " + _seg_comercial.id_SegComercial);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar el seguimiento";
                        }
                        else
                        {
                            /// 2.1.- Obtener los campos del formulario que faltan
                            string sComentario = txt_comentarios.Value.Trim(); // Comentario de la Acción
                            string asunto_mail = null;
                            string cuerpo_mail = null;
                            
                            /// 2.2.- Pintar el comentario del motivo de rechazo
                            if (_motive > 0)
                            {
                                if (_motive == (int)Constantes.motivo_rechazo.Usuario_falso)
                                    sComentario = "Usuario falso (mail/telf no pertenece a ese lead). " + (!String.IsNullOrEmpty(sComentario) ? " [" + sComentario + "]" : string.Empty);
                                else if (_motive == (int)Constantes.motivo_rechazo.Usuario_no_contactable)
                                    sComentario = "Usuario no contactable. " + (!String.IsNullOrEmpty(sComentario) ? " [" + sComentario + "]" : string.Empty);
                                else if (_motive == (int)Constantes.motivo_rechazo.Usuario_no_permite_explicar)
                                    sComentario = "Usuario no permite explicar el producto. [" + sComentario + "]";
                                else if (_motive == (int)Constantes.motivo_rechazo.Usuario_no_pide_info)
                                    sComentario = "Usuario no pide info de este máster ni parecido. " + (!String.IsNullOrEmpty(sComentario) ? " [" + sComentario + "]" : string.Empty);
                            }

                            /// 2.3.- Comprobar si tiene que enviar mail
                            if (accion_comercial == int.Parse(action_mail.Value) && (chkEnviarMail.Checked || !String.IsNullOrEmpty(txt_cuerpo.Value)))
                            {
                                asunto_mail = txt_asunto.Value;
                                cuerpo_mail = txt_cuerpo.Value;
                            }

                            campus_SEG_COMERCIAL _seguimiento = _seguimientos[0];
                            _seguimiento.fecha = fecha_seguimiento.Date.AddHours(_seguimiento.fecha.Hour).AddMinutes(_seguimiento.fecha.Minute);
                            _seguimiento.accion = accion_comercial;
                            _seguimiento.estado = estado;
                            _seguimiento.Asunto_mail = asunto_mail;
                            _seguimiento.Cuerpo_mail = cuerpo_mail;
                            _seguimiento.idComercial = _comercial[0].id_cliente;
                            _seguimiento.Comentarios = sComentario;

                            bool _update_seguimiento = da.updateSegComercial(_seguimiento);
                            if (_update_seguimiento)
                            {
                                /// 3.- Poner el idResultado
                                idResultado = 1;

                                /// 3.0.- Comprobar que es último seguimiento
                                List<campus_SEG_COMERCIAL> _seguimientos_comerciales = da.getSeguimientoComercialByAP(idPeticion);
                                if (_seguimientos_comerciales.Count > 0)
                                {
                                    long _last_seg = _seguimientos_comerciales.Select(_ => _.id_SegComercial).Last();
                                    if (idSeguimiento == _last_seg)
                                    {
                                        /// 3.1.- Actualizar la AP asociada
                                        List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAP(idPeticion);
                                        if (_leads.Count == 1)
                                        {
                                            /// 3.2.- Recuperar los datos del resumen
                                            string resumen_metodologia = null;
                                            if (method_online.Checked)
                                                resumen_metodologia = method_online.Value;
                                            else if (method_semi.Checked)
                                                resumen_metodologia = method_semi.Value;

                                            string resumen_cuando = null;
                                            if (cuando_menor_7.Checked)
                                                resumen_cuando = cuando_menor_7.Value;
                                            else if (cuando_menor_12.Checked)
                                                resumen_cuando = cuando_menor_12.Value;
                                            else if (cuando_mayor_12.Checked)
                                                resumen_cuando = cuando_mayor_12.Value;

                                            string resumen_comentarios = resumen_comentarios_user.Value;

                                            /// 3.3.- Actualizar el lead                            
                                            campus_ACCIONES_PERSONA _lead = _leads[0];
                                            _lead.Procesado = true;
                                            _lead.idComercial = _comercial[0].id_cliente;
                                            _lead.estado_lead = estado;
                                            _lead.ult_comentario = sComentario;
                                            _lead.ult_seg_fecha = _seguimiento.fecha;
                                            _lead.resumen_metodologia = resumen_metodologia;
                                            _lead.resumen_cuando = resumen_cuando;
                                            _lead.resumen_comentarios = resumen_comentarios;
                                            _lead.resumen_fecha_act = DateTime.Now;                                            

                                            bool update_client = da.updatePersonAction(_lead);
                                            if (update_client)
                                            {
                                                /// 3.5.- Recorrer los seguimientos 
                                                _seguimientos_comerciales = _seguimientos_comerciales.Where(_ => _.recordatorio == null || !_.recordatorio.Value || (_.recordatorio.Value && _.fecha < fecha_seguimiento)).ToList();
                                                if (_seguimientos_comerciales.Count > 0)
                                                {
                                                    foreach (var _seg_comercial in _seguimientos_comerciales)
                                                    {
                                                        campus_SEG_COMERCIAL _seguimiento_comercial = _seg_comercial;
                                                        _seguimiento_comercial.estado = _seguimiento.estado;

                                                        bool _update_seg = da.updateSegComercial(_seguimiento_comercial);
                                                        if (!_update_seg)
                                                        {
                                                            LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercial()");
                                                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el seguimiento comercial " + _seg_comercial.id_SegComercial);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar el seguimiento";
                        }
                    }
                }
                else
                {
                    /// 2.1.- Obtener los campos del formulario que faltan
                    string tipo_accion_comercial = Constantes.tipo_accion_comercial.Seguimiento.GetStringValue();
                    string sComentario = txt_comentarios.Value.Trim(); // Comentario de la Acción
                    bool reasignar_comercial = chkReasignarC.Checked;
                    long _comercial_reasignado = long.Parse(ddlComerciales.Value);
                    bool recordatorio = chkProgRec.Checked;
                    DateTime? _date_future = null;
                    string asunto_mail = null;
                    string cuerpo_mail = null;
                    string file_adjuntos = null;
                    long idPlantillaMail = -1;
                    long? idMail = null;

                    /// 2.2.- Recuperar los datos de un recordatorio. Si es seg. a futuro o recordatorio manual
                    if ((estado == (int)Constantes.type_status_action.status_futuro && recordatorio) || (recordatorio && estado != (int)Constantes.type_status_action.status_duplicado && estado != (int)Constantes.type_status_action.status_rechazado))
                    {
                        int _hours = int.Parse(ddlHour.Value.Split(':')[0]);
                        int _minutes = int.Parse(ddlHour.Value.Split(':')[1]);
                        _date_future = DateTime.Parse(txtDateProg.Value).AddHours(_hours).AddMinutes(_minutes);
                    }

                    /// 2.3.- Pintar el comentario del motivo de rechazo
                    if (_motive > 0)
                    {
                        if (_motive == (int)Constantes.motivo_rechazo.Usuario_falso)
                            sComentario = "Usuario falso (mail/telf no pertenece a ese lead). " + (!String.IsNullOrEmpty(sComentario) ? " [" + sComentario + "]" : string.Empty);
                        else if (_motive == (int)Constantes.motivo_rechazo.Usuario_no_contactable)
                            sComentario = "Usuario no contactable. " + (!String.IsNullOrEmpty(sComentario) ? " [" + sComentario + "]" : string.Empty);
                        else if (_motive == (int)Constantes.motivo_rechazo.Usuario_no_permite_explicar)
                            sComentario = "Usuario no permite explicar el producto. [" + sComentario + "]";
                        else if (_motive == (int)Constantes.motivo_rechazo.Usuario_no_pide_info)
                            sComentario = "Usuario no pide info de este máster ni parecido. " + (!String.IsNullOrEmpty(sComentario) ? " [" + sComentario + "]" : string.Empty);
                    }

                    /// 2.4.- Comprobar si tiene que enviar mail
                    if (accion_comercial == int.Parse(action_mail.Value) && (chkEnviarMail.Checked || !String.IsNullOrEmpty(txt_cuerpo.Value)))
                    {
                        asunto_mail = txt_asunto.Value;
                        cuerpo_mail = txt_cuerpo.Value;
                        idPlantillaMail = long.Parse(ddlPlantilla.Value);

                        string adjuntos = hidAdjuntos.Value;
                        if (!String.IsNullOrEmpty(adjuntos))
                        {
                            List<string> list_files_routes = new List<string>();

                            /// 2.4.1.- Sacar los adjuntos
                            List<string> list_files = adjuntos.Split(',').ToList();
                            if (list_files.Count > 0)
                            {
                                List<campus_PLANTILLA_MAIL> _plantilla = new List<campus_PLANTILLA_MAIL>();
                                if (idPlantillaMail > 0)
                                    _plantilla = da.getPlantillasMail(idPlantillaMail);

                                /// 2.4.2.- Guardar el fichero en la carpeta correcta
                                string ruta = ConfigurationManager.AppSettings["routeTemplateMailFree"];
                                string ruta_destino = ruta + idUsuario + "\\" + DateTime.Today.ToString("yyyyMMdd") + "\\";
                                if (!(Directory.Exists(ruta_destino)))
                                    Directory.CreateDirectory(ruta_destino);

                                foreach (var _file in list_files)
                                {
                                    List<campus_PLANTILLA_MAIL> _plantilla_filter = _plantilla.Where(_ => _.adjunto_1 == _file || _.adjunto_2 == _file || _.adjunto_3 == _file || _.adjunto_4 == _file || _.adjunto_5 == _file).ToList();
                                    if (_plantilla_filter.Count == 1)
                                        list_files_routes.Add(ConfigurationManager.AppSettings["routeTemplateMail"] + idPlantillaMail + "/" + _file);
                                    else
                                    {
                                        /// 2.4.2.1.- Sacar las rutas
                                        string ruta_origen = ruta + "temp\\" + _file;
                                        string ruta_destino_file = ruta_destino + _file;

                                        /// 2.4.2.2.- Copiar el fichero
                                        File.Copy(ruta_origen, ruta_destino_file, true);

                                        /// 2.4.2.3.- Guardar la ruta
                                        list_files_routes.Add(ruta_destino_file);
                                    }
                                }
                            }

                            /// 2.4.3.- Devolver los adjuntos
                            int index = 0;
                            foreach (var route in list_files_routes)
                            {
                                if (index == 0)
                                    file_adjuntos = route;
                                else
                                    file_adjuntos += "," + route;
                                index++;
                            }
                        }

                        if (accion_comercial == int.Parse(action_mail.Value) && chkEnviarMail.Checked)
                        {
                            /// 2.4.4.- Crear un mail
                            List<CLIENTES> _user = da.getUserById(idUsuario);

                            /// 2.4.5.- Obtener los datos del cuerpo del mail
                            string template = string.Empty;
                            List<campus_ACCIONES_PERSONA> _lead = da.getActionsByAP(idPeticion);
                            if (_lead.Count == 1 && _lead[0].idPertenencia != null)
                            {
                                List<campus_AUX> _pertenencia = da.getAuxiliars(Constantes.aux.pertenencia.GetStringValue());
                                _pertenencia = _pertenencia.Where(_ => _.ID_Aux == _lead[0].idPertenencia).ToList();
                                template = Utilities.getPlantillaMail(_pertenencia[0].PlantillaMail, ConfigurationManager.AppSettings["urlTemplate"]);

                                if (String.IsNullOrEmpty(template))
                                    template = Utilities.getPlantillaMail("plantilla_mail", ConfigurationManager.AppSettings["urlTemplate"]);
                            }
                            else
                                template = Utilities.getPlantillaMail("plantilla_mail", ConfigurationManager.AppSettings["urlTemplate"]);

                            //string template = Utilities.getPlantillaMail("plantilla_mail", ConfigurationManager.AppSettings["urlTemplate"]);
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###BODY###", cuerpo_mail);
                                template = template.Replace("###IDC###", idCurso.ToString());
                                template = template.Replace("###KEY###", _user[0].Key);
                            }

                            /// 2.4.6.- Añadir los datos de envío del mail
                            EMAIL_CONTENT _mail_data = new EMAIL_CONTENT();
                            _mail_data.nombreTo = _user[0].Nombre_Completo;
                            _mail_data.mailTo = _user[0].email;
                            _mail_data.priority = 1;
                            _mail_data.asunto = asunto_mail;
                            _mail_data.body = template;
                            _mail_data.adjuntos = file_adjuntos;

                            if (_lead.Count == 1 && _lead[0].idPertenencia != null)
                            {
                                List<campus_CLIENTES_COMERCIAL> _pert_comerciales = da.getPerteneciasById(((CLIENTES)Session["usuario"]).id_cliente);
                                if (_pert_comerciales.Count > 0)
                                {
                                    _mail_data.mailFrom = _pert_comerciales.Where(_ => _.idPertenencia == _lead[0].idPertenencia).Select(_ => _.MailPertenencia).FirstOrDefault();
                                    _mail_data.replyTo = _pert_comerciales.Where(_ => _.idPertenencia == _lead[0].idPertenencia).Select(_ => _.MailPertenencia).FirstOrDefault();
                                }                        
                            }

                            if (String.IsNullOrEmpty(_mail_data.nombreFrom) && !String.IsNullOrEmpty(((CLIENTES)Session["usuario"]).Nombre_Completo))
                                _mail_data.nombreFrom = ((CLIENTES)Session["usuario"]).Nombre_Completo;

                            if (String.IsNullOrEmpty(_mail_data.mailFrom) && !String.IsNullOrEmpty(((CLIENTES)Session["usuario"]).email))
                                _mail_data.mailFrom = ((CLIENTES)Session["usuario"]).email;                            

                            if (String.IsNullOrEmpty(_mail_data.replyTo))
                            {
                                if (!String.IsNullOrEmpty(((CLIENTES)Session["usuario"]).email))
                                    _mail_data.replyTo = ((CLIENTES)Session["usuario"]).email;
                                else
                                    _mail_data.replyTo = "info@spainbs.com";
                            }

                            idMail = da.insertEmailContent(_mail_data);
                            if (idMail > 0)
                            {
                                /// 2.4.7.- Recuperar los datos del mail
                                List<EMAIL_CONTENT> _mail = da.getMailById(idMail.Value);
                                if (_mail.Count > 0)
                                {
                                    template = _mail[0].body.Replace("###IDM###", idMail.ToString());
                                    bool update_mail = da.updateEmailContent(idMail.Value, template);
                                    if (!update_mail)
                                    {
                                        LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::updateEmailContent()");
                                        LogUtils.InsertarLog("- MSG: Error al actualizar el mail.");
                                    }
                                }
                            }
                            else
                            {
                                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertEmailContent()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al guardar el mail");
                            }
                        }
                    }

                    /// 2.5.- Pintar el recordatorio
                    if (recordatorio)
                    {
                        campus_ACCIONES_PERSONA _lead_recordatorio = new campus_ACCIONES_PERSONA();
                        _lead_recordatorio.idPersona = idUsuario;
                        _lead_recordatorio.idAccion = (long)Constantes.accion.Peticion_informacion;
                        _lead_recordatorio.Fecha = _date_future.Value;
                        _lead_recordatorio.IdOrigen = (long)Constantes.origen.SBS_Admin;
                        _lead_recordatorio.IdCurso = idCurso;
                        _lead_recordatorio.Comentario = "Recordatorio automático";
                        if (reasignar_comercial)
                            _lead_recordatorio.idComercial = _comercial_reasignado;
                        else
                            _lead_recordatorio.idComercial = _comercial[0].id_cliente;
                        _lead_recordatorio.estado_lead = (int)Constantes.type_status_action.status_nuevo;
                        _lead_recordatorio.ult_comentario = sComentario;

                        long _insert_lead = da.insertPersonAction(_lead_recordatorio);
                        if (_insert_lead > 0)
                        {
                            /// 2.5.1.- Añadir seguimiento comercial
                            campus_SEG_COMERCIAL _seguimiento_recordatorio = new campus_SEG_COMERCIAL();
                            _seguimiento_recordatorio.fecha = _date_future.Value;
                            _seguimiento_recordatorio.idCurso = idCurso;
                            if (reasignar_comercial)
                                _seguimiento_recordatorio.idComercial = _comercial_reasignado;
                            else
                                _seguimiento_recordatorio.idComercial = _comercial[0].id_cliente;
                            _seguimiento_recordatorio.idAlumno = idUsuario;
                            _seguimiento_recordatorio.Comentarios = sComentario;
                            _seguimiento_recordatorio.idAccionPersona = idPeticion;
                            _seguimiento_recordatorio.tipo = tipo_accion_comercial;
                            _seguimiento_recordatorio.accion = (int)Constantes.type_action_canal.action_advise;
                            _seguimiento_recordatorio.estado = estado;
                            _seguimiento_recordatorio.recordatorio = true;

                            long _insert_seg = da.insertSegComercial(_seguimiento_recordatorio);
                            if (_insert_seg < 1)
                            {
                                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercial()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al guardar el seguimiento de recordatorio");
                            }
                        }
                        else
                        {
                            LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertPersonAction()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al guardar la AP de recordatorio");
                        }
                    }

                    /// 3.- Guardar el seguimiento
                    campus_SEG_COMERCIAL _seguimiento = new campus_SEG_COMERCIAL();
                    _seguimiento.fecha = fecha_seguimiento;
                    _seguimiento.idCurso = idCurso;
                    if (reasignar_comercial)
                        _seguimiento.idComercial = _comercial_reasignado;
                    else
                        _seguimiento.idComercial = _comercial[0].id_cliente;
                    _seguimiento.idAlumno = idUsuario;
                    _seguimiento.Comentarios = sComentario;
                    _seguimiento.idAccionPersona = idPeticion;
                    _seguimiento.tipo = tipo_accion_comercial;
                    _seguimiento.accion = accion_comercial;
                    _seguimiento.estado = estado;
                    if (idPlantillaMail > 0)
                        _seguimiento.idPlantillaMail = idPlantillaMail;
                    _seguimiento.idMail = idMail;
                    _seguimiento.Asunto_mail = asunto_mail;
                    _seguimiento.Cuerpo_mail = cuerpo_mail;
                    _seguimiento.Adjuntos_mail = file_adjuntos;

                    idResultado = da.insertSegComercial(_seguimiento);
                    if (idResultado > 0)
                    {
                        /// 3.1.- Actualizar la AP asociada
                        List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAP(idPeticion);
                        if (_leads.Count == 1)
                        {
                            /// 3.2.- Recuperar los datos del resumen
                            string resumen_metodologia = null;
                            if (method_online.Checked)
                                resumen_metodologia = method_online.Value;
                            else if (method_semi.Checked)
                                resumen_metodologia = method_semi.Value;

                            string resumen_cuando = null;
                            if (cuando_menor_7.Checked)
                                resumen_cuando = cuando_menor_7.Value;
                            else if (cuando_menor_12.Checked)
                                resumen_cuando = cuando_menor_12.Value;
                            else if (cuando_mayor_12.Checked)
                                resumen_cuando = cuando_mayor_12.Value;

                            string resumen_comentarios = resumen_comentarios_user.Value;

                            /// 3.3.- Actualizar el lead                            
                            campus_ACCIONES_PERSONA _lead = _leads[0];
                            _lead.Procesado = true;
                            if (reasignar_comercial)
                                _lead.idComercial = _comercial_reasignado;
                            else
                                _lead.idComercial = _comercial[0].id_cliente;
                            _lead.estado_lead = estado;
                            _lead.ult_comentario = sComentario;
                            _lead.ult_seg_fecha = _seguimiento.fecha;
                            if (estado == (int)Constantes.type_status_action.status_duplicado || estado == (int)Constantes.type_status_action.status_rechazado)
                                _lead.test_interno = true;
                            else
                                _lead.test_interno = false;
                            _lead.resumen_metodologia = resumen_metodologia;
                            _lead.resumen_cuando = resumen_cuando;
                            _lead.resumen_comentarios = resumen_comentarios;
                            _lead.resumen_fecha_act = DateTime.Now;

                            bool update_client = da.updatePersonAction(_lead);
                            if (update_client)
                            {
                                /// 3.4.- Actualizar los seguimientos comerciales anteriores
                                List<campus_SEG_COMERCIAL> _seguimientos_comerciales = da.getSeguimientoComercialByAP(idPeticion);
                                if (_seguimientos_comerciales.Count > 0)
                                {
                                    int _estado_anterior = -1;

                                    /// 3.5.- Recorrer los seguimientos 
                                    _seguimientos_comerciales = _seguimientos_comerciales.Where(_ => _.recordatorio == null || !_.recordatorio.Value || (_.recordatorio.Value && _.fecha < fecha_seguimiento)).ToList();
                                    if (_seguimientos_comerciales.Count > 0)
                                    {
                                        /// 3.5.1.- Sacar el último estado
                                        _estado_anterior = _seguimientos_comerciales.Where(_ => _.estado != null).Select(_ => _.estado.Value).Last();

                                        /// 3.5.2.- Actualizar los estados de un seguimiento
                                        foreach (var _seg_comercial in _seguimientos_comerciales)
                                        {
                                            campus_SEG_COMERCIAL _seguimiento_comercial = _seg_comercial;
                                            _seguimiento_comercial.estado = estado;

                                            bool _update_seg = da.updateSegComercial(_seguimiento_comercial);
                                            if (!_update_seg)
                                            {
                                                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercial()");
                                                LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el seguimiento comercial " + _seg_comercial.id_SegComercial);
                                            }
                                        }
                                    }

                                    /// 3.6.- Actualizar los contadores
                                    long _comercial_inf = -1;
                                    if (reasignar_comercial)
                                        _comercial_inf = _comercial_reasignado;
                                    else
                                        _comercial_inf = _comercial[0].id_cliente;

                                    List<campus_SEG_COMERCIAL_INF> _inf_seguimientos = da.getSegComercialInf(_comercial_inf);
                                    if (_inf_seguimientos.Count > 0)
                                    {
                                        if (_estado_anterior != estado)
                                        {
                                            List<campus_SEG_COMERCIAL_INF> _inf_seg_estados_anteriores = _inf_seguimientos.Where(_ => _.estado == _estado_anterior).ToList();
                                            if (_inf_seg_estados_anteriores.Count > 0)
                                            {
                                                campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                                _informe.idComercial = _inf_seg_estados_anteriores[0].idComercial;
                                                _informe.estado = _inf_seg_estados_anteriores[0].estado;
                                                _informe.numero = _inf_seg_estados_anteriores[0].numero - 1;

                                                bool _update_inf = da.updateSegComercialInf(_informe);
                                                if (!_update_inf)
                                                {
                                                    LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::updateSegComercialInf()");
                                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al actulaizar el informe de seguimientos comerciales con estado " + _informe.estado);
                                                }
                                            }
                                        }

                                        List<campus_SEG_COMERCIAL_INF> _inf_seg_estados_actual = _inf_seguimientos.Where(_ => _.estado == estado).ToList();
                                        if (_inf_seg_estados_actual.Count > 0)
                                        {
                                            if (_estado_anterior != estado)
                                            {
                                                campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                                _informe.idComercial = _inf_seg_estados_actual[0].idComercial;
                                                _informe.estado = _inf_seg_estados_actual[0].estado;
                                                _informe.numero = _inf_seg_estados_actual[0].numero + 1;

                                                bool _update_inf = da.updateSegComercialInf(_informe);
                                                if (!_update_inf)
                                                {
                                                    LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::updateSegComercialInf()");
                                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al actulaizar el informe de seguimientos comerciales con estado " + _informe.estado);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                            _informe.idComercial = _comercial_inf;
                                            _informe.estado = estado;
                                            _informe.numero = 1;

                                            long _insert_inf = da.insertSegComercialInf(_informe);
                                            if (_insert_inf < 1)
                                            {
                                                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercialInf()");
                                                LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el informe de seguimientos comerciales");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                        _informe.idComercial = _comercial_inf;
                                        _informe.estado = estado;
                                        _informe.numero = 1;

                                        long _insert_inf = da.insertSegComercialInf(_informe);
                                        if (_insert_inf < 1)
                                        {
                                            LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercialInf()");
                                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el informe de seguimientos comerciales");
                                        }
                                    }
                                }

                                /// 5.- Asignar la pertencia del lead al comercial
                                List<CLIENTES_TAG> _tags = da.getClientTags(idUsuario);

                                /// 5.1.- Filtrar los leads
                                _tags = _tags.Where(_ => _.tag == _comercial[0].login.ToUpper() && _.tipo_tag == "pertenencia_comercial" && _.comentario == _comercial[0].id_cliente.ToString()).ToList();
                                if (_tags.Count == 0)
                                {
                                    /// 5.2.- Añadir tag
                                    CLIENTES_TAG _tag = new CLIENTES_TAG();
                                    _tag.idUser = idUsuario;
                                    _tag.tag = _comercial[0].login.ToUpper();
                                    _tag.tipo_tag = "pertenencia_comercial";
                                    _tag.comentario = _comercial[0].id_cliente.ToString();
                                    _tag.fecha_ult_act = DateTime.Now;

                                    long insert_tag = da.insertClientTag(_tag);
                                    if (insert_tag < 1)
                                    {
                                        LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertClientTag()");
                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el tag de pertenencia a un comercial");
                                    }
                                }
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar el lead";
                        }
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al guardar el seguimiento comercial";
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else if (idSeguimiento > 0)
                txt_error.InnerHtml = "Se ha producido un error al actualizar el seguimiento comercial";
            else
                txt_error.InnerHtml = "Se ha producido un error al guardar el seguimiento comercial";
        }
        protected void btn_reenviar_Click(object sender, EventArgs e)
        {
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
            long idPeticion = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;
            long idSeguimiento = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;
            List<CLIENTES> _comercial = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comercial.Add((CLIENTES)Session["usuario"]);
            long idResultado = -1;

            try
            {
                /// 1.- Sacar los datos del usuario
                List<CLIENTES> _user = da.getUserById(idUsuario);

                /// 2.- Sacar los datos del seguimiento
                List<campus_SEG_COMERCIAL> _seguimiento = da.getSeguimientoComercialById(idSeguimiento);
                if (_seguimiento.Count == 1)
                {
                    /// 2.1.- Recuperar la plantilla del mail
                    string template = Utilities.getPlantillaMail("plantilla_mail", ConfigurationManager.AppSettings["urlTemplate"]);
                    if (!String.IsNullOrEmpty(template))
                    {
                        template = template.Replace("###BODY###", _seguimiento[0].Cuerpo_mail);
                        template = template.Replace("###IDC###", _seguimiento[0].idCurso.ToString());
                        template = template.Replace("###KEY###", _user[0].Key);
                    }

                    /// 2.2.- Añadir los datos de envío del mail
                    EMAIL_CONTENT _mail_data = new EMAIL_CONTENT();
                    _mail_data.nombreTo = _user[0].Nombre_Completo;
                    _mail_data.mailTo = _user[0].email;
                    _mail_data.priority = 1;
                    _mail_data.asunto = _seguimiento[0].Asunto_mail;
                    _mail_data.body = template;
                    _mail_data.adjuntos = _seguimiento[0].Adjuntos_mail;
                    if (!String.IsNullOrEmpty(((CLIENTES)Session["usuario"]).email))
                        _mail_data.replyTo = ((CLIENTES)Session["usuario"]).email;

                    long idMail = da.insertEmailContent(_mail_data);
                    if (idMail > 0)
                    {
                        /// 2.3.- Recuperar los datos del mail
                        List<EMAIL_CONTENT> _mail = da.getMailById(idMail);
                        if (_mail.Count > 0)
                        {
                            string _cuerpo_mail = _mail[0].body.Replace("###IDM###", idMail.ToString());
                            bool update_mail = da.updateEmailContent(idMail, _cuerpo_mail);
                            if (!update_mail)
                            {
                                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::updateEmailContent()");
                                LogUtils.InsertarLog("- MSG: Error al actualizar el mail.");
                            }
                        }

                        /// 3.- Guardar el seguimiento
                        campus_SEG_COMERCIAL _seguimiento_mail = new campus_SEG_COMERCIAL();
                        _seguimiento_mail.fecha = DateTime.Now;
                        _seguimiento_mail.idCurso = _seguimiento[0].idCurso;
                        _seguimiento_mail.idComercial = _seguimiento[0].idComercial;
                        _seguimiento_mail.idAlumno = idUsuario;
                        _seguimiento_mail.Comentarios = _seguimiento[0].Comentarios;
                        _seguimiento_mail.idAccionPersona = idPeticion;
                        _seguimiento_mail.tipo = _seguimiento[0].tipo;
                        _seguimiento_mail.accion = _seguimiento[0].accion;
                        _seguimiento_mail.estado = _seguimiento[0].estado;
                        _seguimiento_mail.idPlantillaMail = _seguimiento[0].idPlantillaMail;
                        _seguimiento_mail.idMail = idMail;
                        _seguimiento_mail.Asunto_mail = _seguimiento[0].Asunto_mail;
                        _seguimiento_mail.Cuerpo_mail = _seguimiento[0].Cuerpo_mail;
                        _seguimiento_mail.Adjuntos_mail = _seguimiento[0].Adjuntos_mail;

                        idResultado = da.insertSegComercial(_seguimiento_mail);
                        if (idResultado > 0)
                        {
                            /// 3.1.- Actualizar la AP asociada
                            List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAP(idPeticion);
                            if (_leads.Count == 1)
                            {
                                /// 3.2.- Recuperar los datos del resumen
                                string resumen_metodologia = null;
                                if (method_online.Checked)
                                    resumen_metodologia = method_online.Value;
                                else if (method_semi.Checked)
                                    resumen_metodologia = method_semi.Value;

                                string resumen_cuando = null;
                                if (cuando_menor_7.Checked)
                                    resumen_cuando = cuando_menor_7.Value;
                                else if (cuando_menor_12.Checked)
                                    resumen_cuando = cuando_menor_12.Value;
                                else if (cuando_mayor_12.Checked)
                                    resumen_cuando = cuando_mayor_12.Value;

                                string resumen_comentarios = resumen_comentarios_user.Value;

                                /// 3.3.- Actualizar el lead                            
                                campus_ACCIONES_PERSONA _lead = _leads[0];
                                _lead.Procesado = true;
                                _lead.idComercial = _seguimiento_mail.idComercial;
                                _lead.estado_lead = _seguimiento_mail.estado;
                                _lead.ult_comentario = _seguimiento_mail.Comentarios;
                                _lead.ult_seg_fecha = _seguimiento_mail.fecha;
                                _lead.resumen_metodologia = resumen_metodologia;
                                _lead.resumen_cuando = resumen_cuando;
                                _lead.resumen_comentarios = resumen_comentarios;
                                _lead.resumen_fecha_act = DateTime.Now;                                

                                bool update_client = da.updatePersonAction(_lead);
                                if (update_client)
                                {
                                    /// 3.4.- Actualizar los seguimientos comerciales anteriores
                                    List<campus_SEG_COMERCIAL> _seguimientos_comerciales = da.getSeguimientoComercialByAP(idPeticion);
                                    if (_seguimientos_comerciales.Count > 0)
                                    {
                                        int _estado_anterior = -1;

                                        /// 3.5.- Recorrer los seguimientos 
                                        _seguimientos_comerciales = _seguimientos_comerciales.Where(_ => _.recordatorio == null || !_.recordatorio.Value || (_.recordatorio.Value && _.fecha < _seguimiento_mail.fecha)).ToList();
                                        if (_seguimientos_comerciales.Count > 0)
                                        {
                                            /// 3.5.1.- Sacar el último estado
                                            _estado_anterior = _seguimientos_comerciales.Where(_ => _.estado != null).Select(_ => _.estado.Value).Last();

                                            /// 3.5.2.- Actualizar los estados de un seguimiento
                                            foreach (var _seg_comercial in _seguimientos_comerciales)
                                            {
                                                campus_SEG_COMERCIAL _seguimiento_comercial = _seg_comercial;
                                                _seguimiento_comercial.estado = _seguimiento_mail.estado;

                                                bool _update_seg = da.updateSegComercial(_seguimiento_comercial);
                                                if (!_update_seg)
                                                {
                                                    LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercial()");
                                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el seguimiento comercial " + _seg_comercial.id_SegComercial);
                                                }
                                            }
                                        }

                                        /// 3.6.- Actualizar los contadores
                                        long _comercial_inf = -1;
                                        _comercial_inf = _seguimiento_mail.idComercial;

                                        List<campus_SEG_COMERCIAL_INF> _inf_seguimientos = da.getSegComercialInf(_comercial_inf);
                                        if (_inf_seguimientos.Count > 0)
                                        {
                                            if (_estado_anterior != _seguimiento_mail.estado)
                                            {
                                                List<campus_SEG_COMERCIAL_INF> _inf_seg_estados_anteriores = _inf_seguimientos.Where(_ => _.estado == _estado_anterior).ToList();
                                                if (_inf_seg_estados_anteriores.Count > 0)
                                                {
                                                    campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                                    _informe.idComercial = _inf_seg_estados_anteriores[0].idComercial;
                                                    _informe.estado = _inf_seg_estados_anteriores[0].estado;
                                                    _informe.numero = _inf_seg_estados_anteriores[0].numero - 1;

                                                    bool _update_inf = da.updateSegComercialInf(_informe);
                                                    if (!_update_inf)
                                                    {
                                                        LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::updateSegComercialInf()");
                                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al actulaizar el informe de seguimientos comerciales con estado " + _informe.estado);
                                                    }
                                                }
                                            }

                                            List<campus_SEG_COMERCIAL_INF> _inf_seg_estados_actual = _inf_seguimientos.Where(_ => _.estado == _seguimiento_mail.estado).ToList();
                                            if (_inf_seg_estados_actual.Count > 0)
                                            {
                                                if (_estado_anterior != _seguimiento_mail.estado)
                                                {
                                                    campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                                    _informe.idComercial = _inf_seg_estados_actual[0].idComercial;
                                                    _informe.estado = _inf_seg_estados_actual[0].estado;
                                                    _informe.numero = _inf_seg_estados_actual[0].numero + 1;

                                                    bool _update_inf = da.updateSegComercialInf(_informe);
                                                    if (!_update_inf)
                                                    {
                                                        LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::updateSegComercialInf()");
                                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al actulaizar el informe de seguimientos comerciales con estado " + _informe.estado);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                                _informe.idComercial = _comercial_inf;
                                                _informe.estado = _seguimiento_mail.estado.Value;
                                                _informe.numero = 1;

                                                long _insert_inf = da.insertSegComercialInf(_informe);
                                                if (_insert_inf < 1)
                                                {
                                                    LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercialInf()");
                                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el informe de seguimientos comerciales");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            campus_SEG_COMERCIAL_INF _informe = new campus_SEG_COMERCIAL_INF();
                                            _informe.idComercial = _comercial_inf;
                                            _informe.estado = _seguimiento_mail.estado.Value;
                                            _informe.numero = 1;

                                            long _insert_inf = da.insertSegComercialInf(_informe);
                                            if (_insert_inf < 1)
                                            {
                                                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertSegComercialInf()");
                                                LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el informe de seguimientos comerciales");
                                            }
                                        }
                                    }
                                }
                                else
                                    txt_error.InnerHtml = "Se ha producido un error al actualizar el lead";
                            }
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al guardar el seguimiento comercial";
                    }
                    else
                    {
                        LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::insertEmailContent()");
                        LogUtils.InsertarLog("- MSG: Se ha producido un error al guardar el mail");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - seguimiento-comercial-crm.cs::btn_reenviar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else if (idSeguimiento > 0)
                txt_error.InnerHtml = "Se ha producido un error al reenviar el mail del seguimiento comercial";
        }

        private void cargar_datos(CLIENTES _comercial, long idUsuario, long idPeticion, long idSeguimiento)
        {
            /// 1.- Sacar los datos del usuario
            List<CLIENTES> _user = da.getUserById(idUsuario);
            if (_user.Count == 1)
                txt_user.InnerHtml = _user[0].Nombre_Completo + " (" + idUsuario + ")";

            /// 2.- Sacar los datos de la petición de información
            List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAP(idPeticion);
            if (_leads.Count == 1)
            {
                /// 2.0.- Comprobar si tiene idPertenencia
                if (_leads[0].idPertenencia != null)
                {
                    /// 2.1.- Cargar combos para el seguimiento comercial
                    cargar_combos(_comercial, _leads[0]);

                    /// 2.2.- Comprobar si viene un seguimiento comercial
                    if (idSeguimiento > 0)
                        cargar_seguimiento(_leads[0], idSeguimiento);
                    else
                    {
                        txtFechaSeg.Value = DateTime.Today.ToShortDateString();

                        /// 2.3.- Poner el resumen
                        if (_leads[0].resumen_metodologia == "Online")
                            method_online.Checked = true;
                        else if (_leads[0].resumen_metodologia == "Semipresencial")
                            method_semi.Checked = true;

                        if (_leads[0].resumen_cuando == "De 0 a 6 meses")
                            cuando_menor_7.Checked = true;
                        else if (_leads[0].resumen_cuando == "De 7 a 12 meses")
                            cuando_menor_12.Checked = true;
                        else if (_leads[0].resumen_cuando == "+ 12 meses")
                            cuando_mayor_12.Checked = true;

                        resumen_comentarios_user.Value = _leads[0].resumen_comentarios;
                        beca_user.Value = _leads[0].resumen_beca.ToString();
                        descuento_user.Value = _leads[0].resumen_descuento.ToString();
                        chkEnviarMail.Checked = true;

                        /// Poner la pertenencia
                        List<campus_AUX> _pertenencia = da.getAuxiliars(Constantes.aux.pertenencia.GetStringValue());
                        _pertenencia = _pertenencia.Where(_ => _.ID_Aux == _leads[0].idPertenencia).ToList();
                        txt_user.InnerHtml += " <span class='badge badge-pill badge-primary' data-toggle='tooltip' data-placement='top' data-html='true' title='Pertenencia'>" + _pertenencia.Select(_ => _.Tags).FirstOrDefault() + "</span>";
                    }
                }
                else
                    Response.Redirect("/ficha-alumno-crm-aux.aspx?idu=" + idUsuario + "&idta=1&idap=" + idPeticion);
            }
        }

        private void cargar_combos(CLIENTES _user, campus_ACCIONES_PERSONA _lead)
        {
            /// 1.- Cargar los origenes
            List<campus_AUX> _origenes = da.getAuxiliars(Constantes.aux.origen.GetStringValue());
            if (_origenes.Count > 0)
            {
                ddlOrigen.DataSource = _origenes;
                ddlOrigen.DataTextField = "Nombre";
                ddlOrigen.DataValueField = "ID_Aux";
                ddlOrigen.DataBind();
                if (_lead.IdOrigen != null)
                    ddlOrigen.Value = _lead.IdOrigen.Value.ToString();
                else
                    ddlOrigen.Value = ((int)Constantes.origen.Web).ToString();
            }

            /// 2.- Cargar los cursos
            List<campus_CURSO> _cursos = da.getAllCourses();
            if (_cursos.Count > 0)
            {
                ddlCurso.DataSource = _cursos;
                ddlCurso.DataTextField = "Nombre";
                ddlCurso.DataValueField = "ID_Curso";
                ddlCurso.DataBind();
                if (_lead.IdCurso != null)
                    ddlCurso.Value = _lead.IdCurso.Value.ToString();
                else
                    ddlCurso.Value = ((int)Constantes.course.Sin_determinar).ToString();
            }

            /// 3.- Cargar los comerciales
            List<CLIENTES> _comerciales = da.getCommercialToReassign();
            List<CLIENTES> _comerciales_filter = _comerciales.Where(_ => _.id_cliente != _user.id_cliente).OrderBy(_ => _.Nombre_Completo).ToList();
            if (_comerciales_filter.Count > 0)
            {
                ddlComerciales.DataSource = _comerciales_filter;
                ddlComerciales.DataTextField = "Nombre_Completo";
                ddlComerciales.DataValueField = "id_cliente";
                ddlComerciales.DataBind();
                ddlComerciales.Items.Insert(0, new ListItem("Seleccione un comercial", "-1"));
                ddlComerciales.Value = "-1";
            }

            /// 4.- Cargar las plantillas de mail
            List<campus_PLANTILLA_MAIL> _plantillas = da.getPlantillasMail(-1);
            if (_plantillas.Count > 0)
            {
                /// 4.1.- Sacar los id de los cursos que tienen plantilla
                List<long> _id_cursos = _plantillas.Select(_ => _.idCurso).Distinct().ToList();

                /// 4.2.- Sacar los id de los usuarios que tienen plantilla
                List<long> _id_comerciales = _plantillas.Where(_ => _.idQuien > 0).Select(_ => _.idQuien).Distinct().ToList();

                /// 4.3.- Ordenar los ids de los comerciales
                _id_comerciales = ordenar_comerciales(_user, _id_comerciales);

                /// 4.4.- Filtrar los comerciales
                List<CLIENTES> _comerciales_plantilla = _comerciales.Where(_ => _id_comerciales.Contains(_.id_cliente)).Distinct().ToList();

                /// 4.5.- Pintar las plantillas
                ddlPlantilla.Items.Add(new ListItem("Seleccione una plantilla", "-1"));
                ddlPlantilla.Items.Add(new ListItem("Libre", "0"));

                /// 4.6.- Recorrer lo comerciales y sacar las plantillas asociadas
                foreach (var _comercial in _id_comerciales)
                {
                    /// 4.6.1.- Sacar las plantillas por comercial
                    List<campus_PLANTILLA_MAIL> _plantillas_filter = _plantillas.Where(_ => _.idQuien == _comercial).ToList();
                    if (_plantillas_filter.Count > 0)
                    {
                        /// 4.6.2.- Sacar las plantillas por curso
                        foreach (var _curso in _id_cursos)
                        {
                            List<campus_PLANTILLA_MAIL> _plantillas_filter_course = _plantillas_filter.Where(_ => _.idCurso == _curso).OrderBy(_ => _.nombre_plantilla).ToList();
                            if (_plantillas_filter_course.Count > 0)
                            {
                                foreach (var _plantilla in _plantillas_filter_course)
                                {
                                    /// 4.6.3.- Sacar el login del usuario
                                    string login = string.Empty;
                                    if (_comercial == 0)
                                        login = "general";
                                    else
                                        login = _comerciales_plantilla.Where(_ => _.id_cliente == _comercial).Select(_ => _.login).FirstOrDefault();

                                    /// 4.6.4.- Pintar la plantilla
                                    ddlPlantilla.Items.Add(new ListItem(login + " - " + _plantilla.nombre_plantilla + " - " + _cursos.Where(_ => _.ID_Curso == _curso).Select(_ => _.Nombre).FirstOrDefault(), _plantilla.idPlantillaMail.ToString()));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void cargar_seguimiento(campus_ACCIONES_PERSONA _lead, long idSeguimiento)
        {
            /// 1.- Recuperar los datos del seguimiento
            List<campus_SEG_COMERCIAL> _seguimiento = da.getSeguimientoComercialById(idSeguimiento);
            if (_seguimiento.Count == 1)
            {
                /// 1.1.- Poner los datos obligatorios
                txtFechaSeg.Value = _seguimiento[0].fecha.ToShortDateString();
                blk_change_user.Visible = false;
                txt_comentarios.Value = _seguimiento[0].Comentarios;
                blk_prog_rec.Attributes["class"] = blk_prog_rec.Attributes["class"].Insert(blk_prog_rec.Attributes["class"].Length, " hidden");

                if (_seguimiento[0].accion == (int)Constantes.type_action_canal.action_mail)
                    action_mail.Checked = true;
                else if (_seguimiento[0].accion == (int)Constantes.type_action_canal.action_phone)
                    action_phone.Checked = true;
                else if (_seguimiento[0].accion == (int)Constantes.type_action_canal.action_whatsapp)
                    action_whatsapp.Checked = true;
                else if (_seguimiento[0].accion == (int)Constantes.type_action_canal.action_advise)
                    action_advise.Checked = true;

                if (_seguimiento[0].accion == (int)Constantes.type_action_canal.action_advise && (_seguimiento[0].recordatorio != null && _seguimiento[0].recordatorio.Value))
                {
                    blk_estado.Visible = false;
                    blk_accion_comercial.Visible = false;
                    blk_prog_rec.Attributes["class"] = blk_prog_rec.Attributes["class"].Replace("hidden", string.Empty);
                    blk_program.Attributes["class"] = blk_program.Attributes["class"].Replace("hidden", string.Empty);
                    chkProgRec.Checked = true;
                    chkProgRec.Disabled = true;
                    txtDateProg.Value = _seguimiento[0].fecha.ToShortDateString();
                    ddlHour.Value = _seguimiento[0].fecha.ToString("HH:mm");
                }
                else
                {
                    if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_sin_contactar)
                        status_sin_contactar.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_indeciso)
                        status_indeciso.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_interesado)
                        status_interesado.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_send)
                        status_send.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_receive)
                        status_receive.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_pago)
                        status_pago.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_futuro)
                        status_futuro.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_duplicado)
                        status_duplicado.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_rechazado)
                    {
                        status_rechazado.Checked = true;
                        blk_rejected.Attributes["class"] = blk_rejected.Attributes["class"].Replace("hidden", string.Empty);

                        if (_seguimiento[0].Comentarios.Contains("Usuario falso (mail/telf no pertenece a ese lead)"))
                            motive1.Checked = true;
                        else if (_seguimiento[0].Comentarios.Contains("Usuario no contactable"))
                            motive2.Checked = true;
                        else if (_seguimiento[0].Comentarios.Contains("Usuario no permite explicar el producto"))
                            motive3.Checked = true;
                        else
                            motive4.Checked = true;
                    }
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_cerrar)
                        status_cerrar.Checked = true;
                    else if (_seguimiento[0].estado == (int)Constantes.type_status_action.status_matriculado)
                        status_matriculado.Checked = true;

                    /// 1.2.- Resumen
                    if (_lead.resumen_metodologia == "Online")
                        method_online.Checked = true;
                    else if (_lead.resumen_metodologia == "Semipresencial")
                        method_semi.Checked = true;

                    if (_lead.resumen_cuando == "De 0 a 6 meses")
                        cuando_menor_7.Checked = true;
                    else if (_lead.resumen_cuando == "De 7 a 12 meses")
                        cuando_menor_12.Checked = true;
                    else if (_lead.resumen_cuando == "+ 12 meses")
                        cuando_mayor_12.Checked = true;

                    resumen_comentarios_user.Value = _lead.resumen_comentarios;
                    beca_user.Value = _lead.resumen_beca.ToString();
                    descuento_user.Value = _lead.resumen_descuento.ToString();

                    if (_seguimiento[0].accion == (int)Constantes.type_action_canal.action_mail)
                    {
                        blk_mail.Attributes["class"] = blk_mail.Attributes["class"].Replace("hidden", string.Empty);
                        chkEnviarMail.Disabled = true;

                        ddlPlantilla.Value = _seguimiento[0].idPlantillaMail.ToString();
                        ddlPlantilla.Disabled = true;
                        txt_asunto.Value = _seguimiento[0].Asunto_mail;
                        txt_cuerpo.Value = _seguimiento[0].Cuerpo_mail;
                        blk_adjuntos.Visible = false;
                        btn_reenviar.Visible = true;
                    }
                }
            }
        }

        private List<long> ordenar_comerciales(CLIENTES _user, List<long> _ids)
        {
            List<long> _id_comerciales = new List<long>();
            _id_comerciales.Add(_user.id_cliente);
            _id_comerciales.Add(0);
            foreach (var id in _id_comerciales)
            {
                if (!_id_comerciales.Contains(id))
                    _id_comerciales.Add(id);
            }
            return _id_comerciales;
        }

        [WebMethod(Description = "Busca una plantilla de la BBDD")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Plantilla_mail> search_plantilla(long idPlantilla)
        {
            DataAccess da = new DataAccess();
            List<Plantilla_mail> _plantilla = new List<Plantilla_mail>();

            if (idPlantilla > 0)
            {
                List<campus_PLANTILLA_MAIL> _plantilla_mail = da.getPlantillasMail(idPlantilla);
                if (_plantilla_mail.Count == 1)
                {
                    string _adjuntos = obtener_adjuntos(_plantilla_mail[0]);
                    _plantilla = _plantilla_mail.Select(_ => new Plantilla_mail { idPlantillaMail = _.idPlantillaMail, idCurso = _.idCurso, idQuien = _.idQuien, nombre_plantilla = _.nombre_plantilla, asunto = _.asunto, cuerpo = _.cuerpo, adjuntos = _adjuntos }).ToList();
                }                
            }
            return _plantilla;
        }

        private static string obtener_adjuntos(campus_PLANTILLA_MAIL _plantilla)
        {
            string _adjuntos = string.Empty;

            if (!String.IsNullOrEmpty(_plantilla.adjunto_1))
                _adjuntos = _plantilla.adjunto_1;
            if (!String.IsNullOrEmpty(_plantilla.adjunto_2))
                _adjuntos = !String.IsNullOrEmpty(_adjuntos) ? _adjuntos + "," + _plantilla.adjunto_2 : _plantilla.adjunto_2;
            if (!String.IsNullOrEmpty(_plantilla.adjunto_3))
                _adjuntos = !String.IsNullOrEmpty(_adjuntos) ? _adjuntos + "," + _plantilla.adjunto_3 : _plantilla.adjunto_3;
            if (!String.IsNullOrEmpty(_plantilla.adjunto_4))
                _adjuntos = !String.IsNullOrEmpty(_adjuntos) ? _adjuntos + "," + _plantilla.adjunto_4 : _plantilla.adjunto_4;
            if (!String.IsNullOrEmpty(_plantilla.adjunto_5))
                _adjuntos = !String.IsNullOrEmpty(_adjuntos) ? _adjuntos + "," + _plantilla.adjunto_5 : _plantilla.adjunto_5;

            return _adjuntos;
        }
    }
}