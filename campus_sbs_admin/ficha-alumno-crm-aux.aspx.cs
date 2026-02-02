using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class ficha_alumno_crm_aux : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el usuario
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (list_user.Count == 0 && Session["usuario"] != null)
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
                    int _type = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? int.Parse(Request.QueryString["idta"].ToString()) : -1;

                    if (idUsuario > 0 && _type > 0)
                    {
                        if (_type == (int)Constantes.tipo_ficha_aux.peticion_informacion)
                            cargar_datos_peticion_info(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.origen)
                            cargar_datos_origen(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.link)
                            cargar_datos_links(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.fundacion)
                            cargar_datos_fundacion(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.cambio_edicion)
                            cargar_datos_cambio_edicion(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.precio_fin_acceso)
                            cargar_datos_precio_fin_acceso(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.asignacion_comercial)
                            cargar_datos_asignacion_comercial(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.asignacion_comercial_all)
                            cargar_datos_asignacion_comercial_all(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.pago)
                            cargar_datos_pago(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.documento)
                            cargar_datos_documentacion(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.informe_tripartita)
                            cargar_datos_inf_tripartita(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.comentarios)
                            cargar_datos_comentarios(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.avance)
                            cargar_datos_avance(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.unificar_usuarios)
                            cargar_datos_usuario_unificar(idUsuario);
                        else if (_type == (int)Constantes.tipo_ficha_aux.incidencia)
                            cargar_datos_incidencia(idUsuario);
                    }
                    else if (idUsuario > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        Response.Redirect("listado-leads-crm.aspx");
                }
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar al usuario y la AP
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long id_ap = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                long id_origen = long.Parse(ddlOrigenAP.Value);
                long id_pertenecia = long.Parse(ddlPertenenciaAP.Value);
                long id_curso = long.Parse(ddlCursoAP.Value);
                DateTime fecha_lead = DateTime.Parse(txtFechaLead.Value);

                /// 4.- Actualizar o añadir la entrada en campus_ACCIONES_PERSONA
                if (id_ap > 0)
                {
                    /// 4.1.- Recuperar los datos del lead
                    List<campus_ACCIONES_PERSONA> _ap = da.getActionsByAP(id_ap);
                    if (_ap.Count == 1)
                    {
                        /// 4.1.0.- Guardar el lead antiguo
                        long? idOrigenOld = _ap[0].IdOrigen;
                        DateTime fecha_lead_old = DateTime.Parse(_ap[0].Fecha.ToShortDateString());
                        long idCursoOld = _ap[0].IdCurso.HasValue ? long.Parse(_ap[0].IdCurso.ToString()) : -1;

                        /// 4.1.1.- Actualizar el lead
                        campus_ACCIONES_PERSONA _lead = _ap[0];
                        _lead.IdCurso = id_curso;
                        _lead.IdOrigen = id_origen;
                        if (id_pertenecia > 0)
                            _lead.idPertenencia = id_pertenecia;
                        _lead.Fecha = fecha_lead.AddHours(_ap[0].Fecha.Hour).AddMinutes(_ap[0].Fecha.Minute).AddSeconds(_ap[0].Fecha.Second);
                        _lead.idComercial = _comerciales[0].id_cliente;

                        bool _update_client = da.updatePersonAction(_lead);
                        if (_update_client)
                        {
                            /// 5.- Comprobar si el lead tiene una asignación comercial asociada
                            List<campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercialByAP(_lead.idAccionPersona);
                            if (_seguimientos.Count > 0)
                            {
                                if (_comerciales[0].Administrador == ((int)Constantes.activo.Activo).ToString())
                                {
                                    /// 6.- Sacar las asignaciones comerciales del usuario
                                    List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercial(-1, idCursoOld, idUsuario);
                                    _asignaciones = _asignaciones.Where(_ => _.Fecha_Lead == fecha_lead_old).ToList();
                                    if (_asignaciones.Count > 0)
                                    {
                                        if (_asignaciones.Count == 1)
                                        {
                                            if (_seguimientos.Count > 0 && (_lead.estado_lead == null || _lead.estado_lead == (int)Constantes.type_status_action.status_matriculado) && _asignaciones.Where(_ => _.idOrigen == idOrigenOld && _.Fecha_Lead == fecha_lead_old).Count() == 1)
                                            {
                                                /// 6.1.- Guardar los datos de la asignación comercial antigua
                                                campus_ASIG_COMERCIAL _asignacion_old = _asignaciones[0];

                                                /// 4.2.- Eliminar la asignación antigua
                                                bool _delete_asignacion = da.deleteAsigComercial(_asignaciones[0].idAlumno, _asignaciones[0].idCurso, _asignaciones[0].idDocencia);
                                                if (_delete_asignacion)
                                                {
                                                    /// 4.3.- Añadir la asignación comercial nueva
                                                    campus_ASIG_COMERCIAL _asig_comercial = new campus_ASIG_COMERCIAL();
                                                    _asig_comercial.idAlumno = idUsuario;
                                                    _asig_comercial.idDocencia = _asignacion_old.idDocencia;
                                                    _asig_comercial.idCurso = id_curso;
                                                    _asig_comercial.Fecha = _asignacion_old.Fecha;
                                                    _asig_comercial.idOrigen = id_origen;
                                                    _asig_comercial.idVendedor = _asignacion_old.idVendedor;
                                                    _asig_comercial.Precio = _asignacion_old.Precio;
                                                    _asig_comercial.Comentarios = _asignacion_old.Comentarios;
                                                    _asig_comercial.Fecha_Lead = fecha_lead;
                                                    _asig_comercial.Fecha_Env_Contrato = _asignacion_old.Fecha_Env_Contrato;
                                                    _asig_comercial.Fecha_Recep_Contrato = _asignacion_old.Fecha_Recep_Contrato;
                                                    _asig_comercial.Fecha_Matricula = _asignacion_old.Fecha_Matricula;
                                                    _asig_comercial.EUR_PVP = _asignacion_old.EUR_PVP;
                                                    _asig_comercial.Beca_Fundacion = _asignacion_old.Beca_Fundacion;
                                                    _asig_comercial.Otros_Descuentos = _asignacion_old.Otros_Descuentos;
                                                    _asig_comercial.EUR_Aportacion_Fundacion = _asignacion_old.EUR_Aportacion_Fundacion;
                                                    _asig_comercial.EUR_Universidad = _asignacion_old.EUR_Universidad;
                                                    _asig_comercial.EUR_PVP_Becado = _asignacion_old.EUR_PVP_Becado;
                                                    _asig_comercial.PDF_Contrato = _asignacion_old.PDF_Contrato;
                                                    _asig_comercial.PDF_Factura = _asignacion_old.PDF_Factura;

                                                    long _insert_asignacion = da.insertAsigComercial(_asig_comercial);
                                                    if (_insert_asignacion > 0)
                                                    {
                                                        /// 5.- Actualizar los pagos para el curso - docencia
                                                        List<campus_DATA_COMERCIAL> _pagos = da.getDataComercialByIdUser(idUsuario);
                                                        _pagos = _pagos.Where(_ => _.idCurso == _asignacion_old.idCurso && _.idDocencia == _asignacion_old.idDocencia).ToList();
                                                        if (_pagos.Count > 0)
                                                        {
                                                            bool _update_correct = true;

                                                            /// 5.1.- Actualizar los pagos
                                                            foreach (var _pago in _pagos)
                                                            {
                                                                campus_DATA_COMERCIAL _pay = _pago;
                                                                _pay.idCurso = id_curso;
                                                                _pay.idDocencia = _asignacion_old.idDocencia;

                                                                _update_correct = da.updateDataComercial(_pay);
                                                                if (!_update_correct)
                                                                    break;
                                                            }

                                                            if (_update_correct)
                                                                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                                            else
                                                                txt_error.InnerHtml = "Se ha producido un error al actualizar los pagos del usuario";
                                                        }
                                                        else
                                                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                                    }
                                                    else
                                                    {
                                                        txt_error.InnerHtml = "Se ha producido un error al añadir la asignación comercial";

                                                        LogUtils.InsertarLog(" ERROR - ficha-usuario.cs::btnGuardar_Click()");
                                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la asignación comercial");
                                                        LogUtils.InsertarLog("- idAlumno " + _asignacion_old.idAlumno);
                                                        LogUtils.InsertarLog("- idDocencia " + _asignacion_old.idDocencia);
                                                        LogUtils.InsertarLog("- idCurso " + _asignacion_old.idCurso);
                                                        LogUtils.InsertarLog("- Fecha " + _asignacion_old.Fecha);
                                                        LogUtils.InsertarLog("- idOrigen " + _asignacion_old.idOrigen);
                                                        LogUtils.InsertarLog("- idVendedor " + _asignacion_old.idVendedor);
                                                        LogUtils.InsertarLog("- Precio " + _asignacion_old.Precio);
                                                        LogUtils.InsertarLog("- Comentarios " + _asignacion_old.Comentarios);
                                                        LogUtils.InsertarLog("- Fecha_Lead " + _asignacion_old.Fecha_Lead);
                                                        LogUtils.InsertarLog("- Fecha_Env_Contrato " + _asignacion_old.Fecha_Env_Contrato);
                                                        LogUtils.InsertarLog("- Fecha_Recep_Contrato " + _asignacion_old.Fecha_Recep_Contrato);
                                                        LogUtils.InsertarLog("- Fecha_Matricula " + _asignacion_old.Fecha_Matricula);
                                                        LogUtils.InsertarLog("- EUR_PVP " + _asignacion_old.EUR_PVP);
                                                        LogUtils.InsertarLog("- Beca_Fundacion " + _asignacion_old.Beca_Fundacion);
                                                        LogUtils.InsertarLog("- Otros_Descuentos " + _asignacion_old.Otros_Descuentos);
                                                        LogUtils.InsertarLog("- EUR_Aportacion_Fundacion " + _asignacion_old.EUR_Aportacion_Fundacion);
                                                        LogUtils.InsertarLog("- EUR_Universidad " + _asignacion_old.EUR_Universidad);
                                                        LogUtils.InsertarLog("- EUR_PVP_Becado " + _asignacion_old.EUR_PVP_Becado);
                                                        LogUtils.InsertarLog("- PDF_Contrato " + _asignacion_old.EUR_Universidad);
                                                        LogUtils.InsertarLog("- PDF_Factura " + _asignacion_old.EUR_PVP_Becado);
                                                    }
                                                }
                                                else
                                                    txt_error.InnerHtml = "Se ha producido un error al eliminar la asignación comercial";
                                            }
                                            else
                                                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                        }
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al haber más de una asignación comercial para el mismo curso y fecha de lead";
                                    }
                                    else
                                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                }
                                else
                                    Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                            }
                            else
                                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar la AP";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar la AP";
                }
                else
                {
                    campus_ACCIONES_PERSONA _lead = new campus_ACCIONES_PERSONA();
                    _lead.idPersona = idUsuario;
                    _lead.idAccion = (int)Constantes.accion.Peticion_informacion;
                    _lead.IdCurso = id_curso;
                    _lead.IdOrigen = id_origen;
                    if (id_pertenecia > 0)
                        _lead.idPertenencia = id_pertenecia;
                    _lead.Fecha = fecha_lead.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                    _lead.idComercial = _comerciales[0].id_cliente;

                    long _insert_ap = da.insertPersonAction(_lead);
                    if (_insert_ap > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir la AP";
                }
            }
        }
        protected void btnGuardarOrigen_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar al usuario y la AP
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                long id_origen = long.Parse(ddlOrigenOR.Value);
                DateTime fecha_lead = DateTime.Parse(txtFechaOrigen.Value);

                /// 4.- Añadir el origen
                campus_ACCIONES_PERSONA _lead = new campus_ACCIONES_PERSONA();
                _lead.idPersona = idUsuario;
                _lead.idAccion = (int)Constantes.accion.Accion_Manual;
                _lead.IdOrigen = id_origen;
                _lead.Fecha = fecha_lead.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                _lead.idComercial = _comerciales[0].id_cliente;

                long _insert_ap = da.insertPersonAction(_lead);
                if (_insert_ap > 0)
                    Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir el origen";
            }
        }
        protected void btnGuardarLink_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar al usuario y el link
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long id_link = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                long id_tipo_link = long.Parse(ddlLinks.Value);
                string url = txt_url_link.Value;

                /// 4.- Actualizar o añadir la entrada en campus_OTROS
                if (id_link > 0)
                {
                    /// 4.1.- Recuperar los datos del link
                    List<campus_OTROS> _link_user = da.getLinksById(id_link);
                    if (_link_user.Count == 1)
                    {
                        campus_OTROS _link = _link_user[0];
                        _link.URL = url;

                        bool _update_link = da.update_link(_link);
                        if (_update_link)
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el link";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar el link";
                }
                else
                {
                    campus_OTROS _link = new campus_OTROS();
                    _link.id_cliente = idUsuario;
                    _link.Tipo = id_tipo_link;
                    _link.URL = url;

                    long _insert_link = da.insert_link(_link);
                    if (_insert_link > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir el link";
                }
            }
        }
        protected void btnGuardarFund_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar al usuario y la AP
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long id_ap = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;
                long id_seg = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                DateTime fecha = DateTime.Parse(txtFechaSeguimientoFund.Value);
                int beca = int.Parse(txt_beca_fund.Value);
                int descuento = int.Parse(txt_desc_fund.Value);
                string comentarios = txt_comentarios_fund.Value;

                /// 4.- Actualizar o añadir la entrada en campus_SEG_COMERCIAL
                if (id_seg > 0)
                {
                    /// 4.1.- Recuperar los datos del lead
                    List<campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercialById(id_seg);
                    if (_seguimientos.Count == 1)
                    {
                        campus_SEG_COMERCIAL _seguimiento = _seguimientos[0];
                        _seguimiento.fecha = fecha.AddHours(_seguimiento.fecha.Hour).AddMinutes(_seguimiento.fecha.Minute).AddSeconds(_seguimiento.fecha.Second);
                        _seguimiento.fundacion = beca;
                        _seguimiento.descuento = descuento;
                        _seguimiento.Comentarios = comentarios;

                        bool _update_seguimiento = da.updateSegComercial(_seguimiento);
                        if (_update_seguimiento)
                        {
                            /// 5.- Actualizar el lead
                            List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAP(id_ap);
                            if (_leads.Count == 1)
                            {
                                campus_ACCIONES_PERSONA _lead = _leads[0];
                                _lead.resumen_beca = beca;
                                _lead.resumen_descuento = descuento;
                                _lead.resumen_fecha_act = DateTime.Now;

                                bool _update_lead = da.updatePersonAction(_lead);
                                if (_update_lead)
                                    Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                else
                                    txt_error.InnerHtml = "Se ha producido un error al actualizar la AP";
                            }
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el seguimiento";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar el seguimiento";
                }
                else
                {
                    /// 4.1.- Buscar los datos del lead
                    List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAP(id_ap);
                    if (_leads.Count == 1)
                    {
                        long _comercial = -1;
                        int estado = 0;

                        /// 4.2.- Sacar los datos del último seguimiento
                        List<campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercialByAP(id_ap);
                        if (_seguimientos.Count > 0)
                        {
                            _comercial = _seguimientos.Select(_ => _.idComercial).Last();
                            estado = _seguimientos.Where(_ => _.estado != null).Select(_ => _.estado.Value).Last();
                        }
                        else
                            _comercial = _comerciales[0].id_cliente;

                        /// 4.3.- Añadir el seguimiento
                        campus_SEG_COMERCIAL _seguimiento = new campus_SEG_COMERCIAL();
                        _seguimiento.fecha = fecha.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                        _seguimiento.idCurso = (long)_leads[0].IdCurso;
                        _seguimiento.idComercial = _comercial;
                        _seguimiento.idAlumno = idUsuario;
                        _seguimiento.Comentarios = comentarios;
                        _seguimiento.idAccionPersona = id_ap;
                        _seguimiento.tipo = Constantes.tipo_accion_comercial.Fundacion.GetStringValue();
                        _seguimiento.estado = estado;
                        _seguimiento.fundacion = beca;
                        _seguimiento.descuento = descuento;
                        _seguimiento.recordatorio = false;

                        long _insert_seguimiento = da.insertSegComercial(_seguimiento);
                        if (_insert_seguimiento > 0)
                        {
                            /// 5.- Actualizar el lead
                            campus_ACCIONES_PERSONA _lead = _leads[0];
                            _lead.resumen_beca = beca;
                            _lead.resumen_descuento = descuento;
                            _lead.resumen_fecha_act = DateTime.Now;

                            bool _update_lead = da.updatePersonAction(_lead);
                            if (_update_lead)
                                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la AP";
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al añadir el seguimiento";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar la AP";
                }
            }
        }
        protected void btnGuardarEdicion_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar al usuario y la docencia
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                long idDocenciaNueva = long.Parse(ddlNuevaEdicion.Value);

                /// 4.- Sacar los datos de la docencia grupo 
                List<campus_DOCENCIA_GRUPO> _dg = da.getDocenciaGrupo(idUsuario, idDocencia);
                if (_dg.Count == 1)
                {
                    /// 5.- Sacar los datos de la docencia
                    List<campus_DOCENCIA> _docencias = da.getDocenciaById(idDocencia);

                    /// 6.- Poner los comentarios
                    string _comentarios = (!String.IsNullOrEmpty(_dg[0].Comentarios) ? _dg[0].Comentarios + " / " : string.Empty) + "Cambio de edición. Edición anterior: (" + idDocencia + ") " + (_docencias.Count == 1 ? _docencias[0].Nombre : string.Empty) + " " + DateTime.Today.ToShortDateString();

                    /// 7.- Actualizar el docencia grupo
                    campus_DOCENCIA_GRUPO _docencia_grupo = _dg[0];
                    _docencia_grupo.ID_Docencia = idDocenciaNueva;
                    _docencia_grupo.Comentarios = _comentarios;

                    bool _update_docencia = da.updateDocenciaGrupo(_docencia_grupo);
                    if (_update_docencia)
                    {
                        /// 8.- Sacar los datos del usuario de campus_DOCENCIA_GRUPO_EVALUACION
                        List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluaciones = da.getDocenciasGrupoEvaluacion(idUsuario);
                        _evaluaciones = _evaluaciones.Where(_ => _.idDocencia == idDocencia).ToList();
                        if (_evaluaciones.Count > 0)
                        {
                            /// 8.1.- Filtrar las notas aprobadas
                            _evaluaciones = _evaluaciones.Where(_ => _.nota != null && _.nota.Value >= 4).ToList();
                            if (_evaluaciones.Count > 0)
                            {
                                bool _correct = true;

                                /// 8.2.- Recorrer las evaluaciones
                                foreach (var _evaluacion in _evaluaciones)
                                {
                                    /// 8.3.- Sacar la nota
                                    decimal _nota = _evaluacion.nota.Value < 5 ? new decimal(5) : _evaluacion.nota.Value;

                                    /// 8.4.- Inserta una entrada en campus_DOCENCIA_GRUPO_EVALUACION por cada asignatura aprobada
                                    campus_DOCENCIA_GRUPO_EVALUACION _evaluacion_nueva = new campus_DOCENCIA_GRUPO_EVALUACION();
                                    _evaluacion_nueva.idPersona = _evaluacion.idPersona;
                                    _evaluacion_nueva.idCurso = _evaluacion.idCurso;
                                    _evaluacion_nueva.idDocencia = idDocenciaNueva;
                                    _evaluacion_nueva.fecha = _evaluacion.fecha;
                                    _evaluacion_nueva.nota = _nota;
                                    _evaluacion_nueva.comentarios = _evaluacion.comentarios;
                                    _evaluacion_nueva.idProfesor = _evaluacion.idProfesor;
                                    _evaluacion_nueva.ord_fecha = _evaluacion.fecha;
                                    _evaluacion_nueva.ord_nota = _nota;
                                    _evaluacion_nueva.comentarios = _evaluacion.comentarios;
                                    _evaluacion_nueva.ord_idProfesor = _evaluacion.idProfesor;

                                    long _insert_evaluacion = da.insertEvaluation(_evaluacion_nueva);
                                    if (_insert_evaluacion < 1)
                                    {
                                        _correct = false;
                                        break;
                                    }
                                }

                                if (!_correct)
                                    txt_error.InnerHtml = "Se ha producido un error al actualizar las entradas en campus_DOCENCIA_GRUPO_EVALUACION del usuario " + idUsuario + " y la docencia " + idDocencia;
                                else
                                {
                                    /// 9.- Actualizar las encuestas
                                    List<campus_ENCUESTA> _encuestas = da.getEncuestaByParams(idUsuario, idDocencia);
                                    if (_encuestas.Count > 0)
                                    {
                                        foreach (var _encuesta_user in _encuestas)
                                        {
                                            campus_ENCUESTA _encuesta = _encuesta_user;
                                            _encuesta.ID_Docencia = idDocenciaNueva;

                                            _correct = da.updateEncuesta(_encuesta);
                                            if (!_correct)
                                                break;
                                        }
                                    }

                                    if (_correct)
                                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                    else
                                        txt_error.InnerHtml = "Se ha producido un error al actualizar las encuestas del usuario";
                                }
                            }
                            else
                                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        }
                        else
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la matrícula";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar la docencia";
            }
        }
        protected void btnGuardarPrecio_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar al usuario y la docencia
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

                /// 3.- Sacar los datos de la docencia grupo 
                List<campus_DOCENCIA_GRUPO> _dg = da.getDocenciaGrupo(idUsuario, idDocencia);
                if (_dg.Count == 1)
                {
                    /// 4.- Recuperar los datos del formulario
                    decimal _precio = !String.IsNullOrEmpty(txt_precio.Value) ? decimal.Parse(txt_precio.Value.Replace('.', ',')) : new decimal(0);
                    DateTime? _fecha_fin_acceso = null;
                    if (!String.IsNullOrEmpty(txtFechaFinAcceso.Value))
                        _fecha_fin_acceso = DateTime.Parse(txtFechaFinAcceso.Value);

                    /// 5.- Actualizar la matrícula
                    campus_DOCENCIA_GRUPO _matricula = _dg[0];
                    _matricula.PrecioPagado = _precio;
                    _matricula.FFinAcceso = _fecha_fin_acceso;

                    bool _update_docencia = da.updateDocenciaGrupo(_matricula);
                    if (_update_docencia)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la matrícula";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar la docencia";
            }
        }
        protected void btnGuardarAsig_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar los parámetros
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long id_ap = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;
                long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
                long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                DateTime fecha_asignacion = DateTime.Parse(txtFechaAsignacionComercial.Value);
                long idComercial = long.Parse(ddlComerciales.Value);
                decimal _precio = !String.IsNullOrEmpty(txt_precio_asig.Value) ? decimal.Parse(txt_precio_asig.Value.Replace('.', ',')) : new decimal(0);
                long id_docencia = long.Parse(ddlDocenciaAsig.Value);
                long id_curso = long.Parse(ddlCursoAsig.Value);
                string _comentarios = txt_comentarios_asig.Value;
                long? idPertenencia = null;
                if (long.Parse(ddlPertenenciaAsig.Value) > 0)
                    idPertenencia = long.Parse(ddlPertenenciaAsig.Value);
                long? idMetodologia = null;
                if (long.Parse(ddlMetodologiaAsig.Value) > 0)
                    idMetodologia = long.Parse(ddlMetodologiaAsig.Value);
                long? idTipoCaptacion = null;
                if (long.Parse(ddlCaptacionTipoAsig.Value) > 0)
                    idTipoCaptacion = long.Parse(ddlCaptacionTipoAsig.Value);
                long? idOrigen = null;
                if (long.Parse(ddlCaptacionOrigenAsig.Value) > 0)
                    idOrigen = long.Parse(ddlCaptacionOrigenAsig.Value);
                long? idAcceso = null;
                if (long.Parse(ddlAcceso.Value) > 0)
                    idAcceso = long.Parse(ddlAcceso.Value);
                bool titulo_propio = chkTitPropio.Checked;
                bool titulo_ucam = chkTitUcam.Checked;
                bool titulo_sbs_ad = chkTitSbsAd.Checked;
                bool titulo_sbs_do = chkTitSbsDo.Checked;
                bool titulo_cualificam = chkTitCualificam.Checked;

                if (idDocencia > 0)
                {
                    if (idDocencia == id_docencia && idCurso == id_curso)
                    {
                        List<campus_ASIG_COMERCIAL> _asignacion = da.getAsigComercial(idDocencia, idCurso, idUsuario);
                        if (_asignacion.Count == 1)
                        {
                            campus_ASIG_COMERCIAL _asig_comercial = _asignacion[0];
                            _asig_comercial.idVendedor = idComercial;
                            _asig_comercial.Fecha = fecha_asignacion;
                            _asig_comercial.Precio = _precio;
                            _asig_comercial.Comentarios = _comentarios;
                            if (idPertenencia != null)
                                _asig_comercial.pertenecia = idPertenencia;
                            if (idMetodologia != null)
                                _asig_comercial.metodologia = idMetodologia;
                            if (idTipoCaptacion != null)
                                _asig_comercial.captacion_tipo = idTipoCaptacion;
                            if (idOrigen != null)
                                _asig_comercial.captacion_origen = idOrigen;
                            if (idAcceso != null)
                                _asig_comercial.acceso_por = idAcceso; 
                            _asig_comercial.titulo_propio = titulo_propio;
                            _asig_comercial.titulo_ucam = titulo_ucam;
                            _asig_comercial.titulo_sbs_ad = titulo_sbs_ad;
                            _asig_comercial.titulo_sbs_do = titulo_sbs_do;
                            _asig_comercial.titulo_cualificam = titulo_cualificam;

                            bool _update_asig = da.updateAsigComercial(_asig_comercial);
                            if (_update_asig)
                                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la asignación comercial";
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al buscar la asignación comercial";
                    }
                    else
                    {                        
                        /// 4.- Sacar los datos de la asignación comercial del alumno
                        List<campus_ASIG_COMERCIAL> _asignacion = da.getAsigComercial(idDocencia, idCurso, idUsuario);
                        if (_asignacion.Count == 1)
                        {
                            /// 4.1.- Guardar los datos de la asignación comercial antigua
                            campus_ASIG_COMERCIAL _asignacion_old = _asignacion[0];

                            /// 4.2.- Eliminar la asignación antigua
                            bool _delete_asignacion = da.deleteAsigComercial(_asignacion[0].idAlumno, _asignacion[0].idCurso, _asignacion[0].idDocencia);
                            if (_delete_asignacion)
                            {
                                /// 4.3.- Añadir la asignación comercial nueva
                                campus_ASIG_COMERCIAL _asig_comercial = new campus_ASIG_COMERCIAL();
                                _asig_comercial.idAlumno = idUsuario;
                                _asig_comercial.idDocencia = id_docencia;
                                _asig_comercial.idCurso = id_curso;
                                _asig_comercial.Fecha = fecha_asignacion;
                                _asig_comercial.idOrigen = _asignacion_old.idOrigen;
                                _asig_comercial.idVendedor = idComercial;
                                _asig_comercial.Precio = _precio;
                                _asig_comercial.Comentarios = _comentarios;
                                if (idPertenencia != null)
                                    _asig_comercial.pertenecia = idPertenencia;
                                if (idMetodologia != null)
                                    _asig_comercial.metodologia = idMetodologia;
                                if (idTipoCaptacion != null)
                                    _asig_comercial.captacion_tipo = idTipoCaptacion;
                                if (idOrigen != null)
                                    _asig_comercial.captacion_origen = idOrigen;
                                if (idAcceso != null)
                                    _asig_comercial.acceso_por = idAcceso;
                                _asig_comercial.titulo_propio = titulo_propio;
                                _asig_comercial.titulo_ucam = titulo_ucam;
                                _asig_comercial.titulo_sbs_ad = titulo_sbs_ad;
                                _asig_comercial.titulo_sbs_do = titulo_sbs_do;
                                _asig_comercial.titulo_cualificam = titulo_cualificam;
                                _asig_comercial.Fecha_Lead = _asignacion_old.Fecha_Lead;
                                _asig_comercial.Fecha_Env_Contrato = _asignacion_old.Fecha_Env_Contrato;
                                _asig_comercial.Fecha_Recep_Contrato = _asignacion_old.Fecha_Recep_Contrato;
                                _asig_comercial.Fecha_Matricula = _asignacion_old.Fecha_Matricula;
                                _asig_comercial.EUR_PVP = _asignacion_old.EUR_PVP;
                                _asig_comercial.Beca_Fundacion = _asignacion_old.Beca_Fundacion;
                                _asig_comercial.Otros_Descuentos = _asignacion_old.Otros_Descuentos;
                                _asig_comercial.EUR_Aportacion_Fundacion = _asignacion_old.EUR_Aportacion_Fundacion;
                                _asig_comercial.EUR_Universidad = _asignacion_old.EUR_Universidad;
                                _asig_comercial.EUR_PVP_Becado = _asignacion_old.EUR_PVP_Becado;
                                _asig_comercial.PDF_Contrato = _asignacion_old.PDF_Contrato;
                                _asig_comercial.PDF_Factura = _asignacion_old.PDF_Factura;

                                long _insert_asignacion = da.insertAsigComercial(_asig_comercial);
                                if (_insert_asignacion > 0)
                                {
                                    /// 5.- Actualizar los pagos para el curso - docencia
                                    List<campus_DATA_COMERCIAL> _pagos = da.getDataComercialByIdUser(idUsuario);
                                    _pagos = _pagos.Where(_ => _.idCurso == idCurso && _.idDocencia == idDocencia).ToList();
                                    if (_pagos.Count > 0)
                                    {
                                        bool _update_correct = true;

                                        /// 5.1.- Actualizar los pagos
                                        foreach (var _pago in _pagos)
                                        {
                                            campus_DATA_COMERCIAL _pay = _pago;
                                            _pay.idCurso = id_curso;
                                            _pay.idDocencia = id_docencia;

                                            _update_correct = da.updateDataComercial(_pay);
                                            if (!_update_correct)
                                                break;
                                        }

                                        if (_update_correct)
                                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar los pagos del usuario";
                                    }
                                    else
                                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                                }
                                else
                                {
                                    txt_error.InnerHtml = "Se ha producido un error al añadir la asignación comercial";

                                    LogUtils.InsertarLog(" ERROR - ficha-usuario.cs::btnGuardarAsig_Click()");
                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la asignación comercial");
                                    LogUtils.InsertarLog("- idAlumno " + _asignacion_old.idAlumno);
                                    LogUtils.InsertarLog("- idDocencia " + _asignacion_old.idDocencia);
                                    LogUtils.InsertarLog("- idCurso " + _asignacion_old.idCurso);
                                    LogUtils.InsertarLog("- Fecha " + _asignacion_old.Fecha);
                                    LogUtils.InsertarLog("- idOrigen " + _asignacion_old.idOrigen);
                                    LogUtils.InsertarLog("- idVendedor " + _asignacion_old.idVendedor);
                                    LogUtils.InsertarLog("- Precio " + _asignacion_old.Precio);
                                    LogUtils.InsertarLog("- Comentarios " + _asignacion_old.Comentarios);
                                    LogUtils.InsertarLog("- Fecha_Lead " + _asignacion_old.Fecha_Lead);
                                    LogUtils.InsertarLog("- Fecha_Env_Contrato " + _asignacion_old.Fecha_Env_Contrato);
                                    LogUtils.InsertarLog("- Fecha_Recep_Contrato " + _asignacion_old.Fecha_Recep_Contrato);
                                    LogUtils.InsertarLog("- Fecha_Matricula " + _asignacion_old.Fecha_Matricula);
                                    LogUtils.InsertarLog("- EUR_PVP " + _asignacion_old.EUR_PVP);
                                    LogUtils.InsertarLog("- Beca_Fundacion " + _asignacion_old.Beca_Fundacion);
                                    LogUtils.InsertarLog("- Otros_Descuentos " + _asignacion_old.Otros_Descuentos);
                                    LogUtils.InsertarLog("- EUR_Aportacion_Fundacion " + _asignacion_old.EUR_Aportacion_Fundacion);
                                    LogUtils.InsertarLog("- EUR_Universidad " + _asignacion_old.EUR_Universidad);
                                    LogUtils.InsertarLog("- EUR_PVP_Becado " + _asignacion_old.EUR_PVP_Becado);
                                    LogUtils.InsertarLog("- PDF_Contrato " + _asignacion_old.EUR_Universidad);
                                    LogUtils.InsertarLog("- PDF_Factura " + _asignacion_old.EUR_PVP_Becado);
                                }
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al eliminar la asignación comercial";
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al buscar la asignación comercial";
                    }
                }
                else
                {
                    /// 4.- Sacar los datos de la accion
                    DateTime _fecha_lead = new DateTime();
                    long idOrigenAP = -1;
                    List<campus_ACCIONES_PERSONA> _ap = da.getActionsByAP(id_ap);
                    if (_ap.Count == 1)
                    {
                        _fecha_lead = DateTime.Parse(_ap[0].Fecha.ToShortDateString());
                        idOrigenAP = _ap[0].IdOrigen.Value;
                    }

                    /// 4.1.- Sacar el curso a partir de la docencia
                    ///assd


                    /// 5.- Añadir la asignación comercial
                    campus_ASIG_COMERCIAL _asig_comercial = new campus_ASIG_COMERCIAL();
                    _asig_comercial.idAlumno = idUsuario;
                    _asig_comercial.idCurso = id_curso;
                    _asig_comercial.idDocencia = id_docencia;
                    _asig_comercial.Fecha = fecha_asignacion;
                    _asig_comercial.idOrigen = idOrigenAP;
                    _asig_comercial.idVendedor = idComercial;
                    _asig_comercial.Precio = _precio;
                    _asig_comercial.Comentarios = _comentarios;
                    _asig_comercial.Fecha_Lead = _fecha_lead;
                    if (idPertenencia != null)
                        _asig_comercial.pertenecia = idPertenencia;
                    if (idMetodologia != null)
                        _asig_comercial.metodologia = idMetodologia;
                    if (idTipoCaptacion != null)
                        _asig_comercial.captacion_tipo = idTipoCaptacion;
                    if (idOrigen != null)
                        _asig_comercial.captacion_origen = idOrigen;
                    if (idAcceso != null)
                        _asig_comercial.acceso_por = idAcceso;
                    _asig_comercial.titulo_propio = titulo_propio;
                    _asig_comercial.titulo_ucam = titulo_ucam;
                    _asig_comercial.titulo_sbs_ad = titulo_sbs_ad;
                    _asig_comercial.titulo_sbs_do = titulo_sbs_do;
                    _asig_comercial.titulo_cualificam = titulo_cualificam;
                    _asig_comercial.EUR_PVP = 0;
                    _asig_comercial.Beca_Fundacion = 0;
                    _asig_comercial.Otros_Descuentos = 0;
                    _asig_comercial.EUR_Aportacion_Fundacion = 0;
                    _asig_comercial.EUR_Universidad = 0;
                    _asig_comercial.EUR_PVP_Becado = 0;

                    long _insert_asignacion = da.insertAsigComercial(_asig_comercial);
                    if (_insert_asignacion > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir la asignación comercial";
                }
            }
        }
        protected void btnGuardarAsigAll_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar los parámetros
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
                long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                DateTime? _fecha_matricula = null;
                if (!String.IsNullOrEmpty(txtFechaPagoMatricula.Value))
                    _fecha_matricula = DateTime.Parse(txtFechaPagoMatricula.Value);
                DateTime? _fecha_env_contrato = null;
                if (!String.IsNullOrEmpty(txtFechaEnvContrato.Value))
                    _fecha_env_contrato = DateTime.Parse(txtFechaEnvContrato.Value);
                DateTime? _fecha_recep_contrato = null;
                if (!String.IsNullOrEmpty(txtFechaRecepContrato.Value))
                    _fecha_recep_contrato = DateTime.Parse(txtFechaRecepContrato.Value);
                decimal price_pvp = !String.IsNullOrEmpty(txt_pvp.Value) ? decimal.Parse(txt_pvp.Value.Replace('.', ',')) : new decimal(0);
                decimal price_pvp_becado = !String.IsNullOrEmpty(txt_pvp_becado.Value) ? decimal.Parse(txt_pvp_becado.Value.Replace('.', ',')) : new decimal(0);
                int beca = !String.IsNullOrEmpty(txt_beca_fundacion.Value) ? int.Parse(txt_beca_fundacion.Value) : 0;
                int descuentos = !String.IsNullOrEmpty(txt_otros_descuentos.Value) ? int.Parse(txt_otros_descuentos.Value) : 0;
                decimal price_fundacion = !String.IsNullOrEmpty(txt_fundacion.Value) ? decimal.Parse(txt_fundacion.Value.Replace('.', ',')) : new decimal(0);
                decimal price_universidad = !String.IsNullOrEmpty(txt_universidad.Value) ? decimal.Parse(txt_universidad.Value.Replace('.', ',')) : new decimal(0);

                if (idDocencia > 0)
                {
                    List<campus_ASIG_COMERCIAL> _asignacion = da.getAsigComercial(idDocencia, idCurso, idUsuario);
                    if (_asignacion.Count == 1)
                    {
                        campus_ASIG_COMERCIAL _asig_comercial = _asignacion[0];
                        _asig_comercial.Fecha_Matricula = _fecha_matricula;
                        _asig_comercial.Fecha_Env_Contrato = _fecha_env_contrato;
                        _asig_comercial.Fecha_Recep_Contrato = _fecha_recep_contrato;
                        _asig_comercial.EUR_PVP = price_pvp;
                        _asig_comercial.EUR_PVP_Becado = price_pvp_becado;
                        _asig_comercial.Beca_Fundacion = beca;
                        _asig_comercial.Otros_Descuentos = descuentos;
                        _asig_comercial.EUR_Aportacion_Fundacion = price_fundacion;
                        _asig_comercial.EUR_Universidad = price_universidad;

                        bool _update_asig = da.updateAsigComercial(_asig_comercial);
                        if (_update_asig)
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar la asignación comercial";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar la asignación comercial";
                }
            }
        }
        protected void btnGuardarPago_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar los parámetros
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
                long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;
                long id_pago = !String.IsNullOrEmpty(Request.QueryString["idp"]) ? long.Parse(Request.QueryString["idp"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                DateTime _fecha_estimada = DateTime.Parse(txtFechaEstimada.Value);
                decimal _precio_est = !String.IsNullOrEmpty(txt_euros_est.Value) ? decimal.Parse(txt_euros_est.Value.Replace('.', ',')) : new decimal(0);
                DateTime? _fecha_real = null;
                if (!String.IsNullOrEmpty(txtFechaReal.Value))
                    _fecha_real = DateTime.Parse(txtFechaReal.Value);
                decimal _precio_real = !String.IsNullOrEmpty(txt_euros_real.Value) ? decimal.Parse(txt_euros_real.Value.Replace('.', ',')) : new decimal(0);

                if (id_pago > 0)
                {
                    /// 4.- Actualizar el pago
                    List<campus_DATA_COMERCIAL> _pagos = da.getDataComercialById(id_pago);
                    if (_pagos.Count == 1)
                    {
                        campus_DATA_COMERCIAL _pago = _pagos[0];
                        _pago.Fecha_est = _fecha_estimada;
                        _pago.EUR_est = _precio_est;
                        _pago.Fecha_real = _fecha_real;
                        _pago.EUR_real = _precio_real;

                        bool _update_pay = da.updateDataComercial(_pago);
                        if (_update_pay)
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el pago";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar el pago";
                }
                else
                {
                    /// 4.- Añadir un pago
                    campus_DATA_COMERCIAL _pago = new campus_DATA_COMERCIAL();
                    _pago.idAlumno = idUsuario;
                    _pago.idCurso = idCurso;
                    _pago.idDocencia = idDocencia;
                    _pago.Fecha_est = _fecha_estimada;
                    _pago.EUR_est = _precio_est;
                    _pago.Fecha_real = _fecha_real;
                    _pago.EUR_real = _precio_real;

                    long _insert_pay = da.insertDataComercial(_pago);
                    if (_insert_pay > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir el pago";
                }
            }
        }
        protected void btnGuardarDoc_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar los parámetros
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long idDocumento = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                string _tipo_documento = ddlTipoDoc.Value;
                string _descripcion = txt_descripcion_doc.Value;
                string _fichero = documento_usuario.Value;
                DateTime? _fecha_caducidad = null;
                if (!String.IsNullOrEmpty(txtFechaCaducidad.Value))
                    _fecha_caducidad = DateTime.Parse(txtFechaCaducidad.Value);
                long? _idDocencia = null;
                if (ddlDocenciaDocumento.Value != "-1")
                    _idDocencia = long.Parse(ddlDocenciaDocumento.Value);

                if (idDocumento > 0)
                {
                    /// 4.- Actualizar el documento
                    List<campus_CLIENTES_DOC> _documentos = da.getDocsClientes(idDocumento);
                    if (_documentos.Count == 1)
                    {
                        /// 4.1.- Añadir el documento nuevo
                        if (!String.IsNullOrEmpty(_fichero) && _fichero != _documentos[0].Fichero)
                        {
                            /// 4.1.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["ruta_documentos"];

                            string ruta_origen = ruta + "temp\\" + _fichero;
                            string ruta_destino = ruta + idUsuario + "\\";

                            /// 4.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + _fichero;

                            /// 4.1.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 4.1.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                            /// 4.1.5.- Borramos el directorio temp
                            if (Directory.GetFiles(ruta + "temp\\").Count() == 0)
                            {
                                if ((Directory.Exists(ruta + "temp\\")))
                                    Directory.Delete(ruta + "temp\\");
                            }
                            else
                            {
                                foreach (var file in Directory.GetFiles(ruta + "temp\\"))
                                {
                                    File.Delete(file);
                                }

                                if ((Directory.Exists(ruta + "temp\\")))
                                    Directory.Delete(ruta + "temp\\");
                            }
                        }

                        /// 4.2.- Actualizar los datos del documento
                        campus_CLIENTES_DOC _documento = _documentos[0];
                        _documento.tipo_documento = _tipo_documento;
                        _documento.Descripcion = _descripcion;
                        _documento.Fichero = _fichero;
                        _documento.fecha_caducidad = _fecha_caducidad;
                        _documento.id_docencia = _idDocencia;

                        bool _update_doc = da.updateDocumento(_documento);
                        if (_update_doc)
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el documento";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar el documento";
                }
                else
                {
                    /// 4.1.- Comprobar documento nuevo
                    if (!String.IsNullOrEmpty(_fichero))
                    {
                        /// 4.1.1.- Guardar el fichero en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_documentos"];

                        string ruta_origen = ruta + "temp\\" + _fichero;
                        string ruta_destino = ruta + idUsuario + "\\";

                        /// 4.1.2.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        ruta_destino = ruta_destino + _fichero;

                        /// 4.1.3.- Copiar el fichero
                        File.Copy(ruta_origen, ruta_destino, true);

                        /// 4.1.4.- Borramos el fichero de la carpeta origen
                        File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                        /// 4.1.5.- Borramos el directorio temp
                        if (Directory.GetFiles(ruta + "temp\\").Count() == 0)
                        {
                            if ((Directory.Exists(ruta + "temp\\")))
                                Directory.Delete(ruta + "temp\\");
                        }
                        else
                        {
                            foreach (var file in Directory.GetFiles(ruta + "temp\\"))
                            {
                                File.Delete(file);
                            }

                            if ((Directory.Exists(ruta + "temp\\")))
                                Directory.Delete(ruta + "temp\\");
                        }
                    }

                    /// 5.- Añadir un documento
                    campus_CLIENTES_DOC _documento = new campus_CLIENTES_DOC();
                    _documento.idCliente = idUsuario;
                    _documento.tipo_documento = _tipo_documento;
                    _documento.Descripcion = _descripcion;
                    _documento.Fichero = _fichero;
                    _documento.fecha_caducidad = _fecha_caducidad;
                    _documento.id_docencia = _idDocencia;
                    _documento.Fecha = DateTime.Now;

                    long _insert_document = da.insertDocumento(_documento);
                    if (_insert_document > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir el documento";
                }
            }
        }
        protected void btnGuardarComentario_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar al comercial
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            if (_comerciales.Count == 1)
            {
                /// 2.- Sacar los parámetros
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                long idComentario = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;

                /// 3.- Recuperar los datos del formulario
                string _comentario_user = txt_comentarios_user.Value;

                if (idComentario > 0)
                {
                    /// 4.- Actualizar el comentario
                    List<CLIENTES_COMENTARIOS> _comentarios = da.getComentariosClientes(idComentario);
                    if (_comentarios.Count == 1)
                    {
                        /// 4.1.- Actualizar los datos del comentario
                        CLIENTES_COMENTARIOS _comentario = _comentarios[0];
                        _comentario.Comentario = _comentario_user;

                        bool _update_doc = da.updateComentario(_comentario);
                        if (_update_doc)
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el comentario";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar el comentario";
                }
                else
                {
                    /// 5.- Añadir un comentario
                    CLIENTES_COMENTARIOS _comentario = new CLIENTES_COMENTARIOS();
                    _comentario.Fecha = DateTime.Now;
                    _comentario.Comentario = _comentario_user;
                    _comentario.idUsuario = idUsuario;
                    _comentario.idComercial = _comerciales[0].id_cliente;

                    long _insert_comment = da.insertComentario(_comentario);
                    if (_insert_comment > 0)
                        Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir el comentario";
                }
            }
        }
        protected void btnUnificarUsuario_Click(object sender, EventArgs e)
        {

        }

        protected void btn_back_Click(object sender, EventArgs e)
        {
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
            if (idUsuario > 0)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                Response.Redirect("listado-leads-crm.aspx");
        }

        #region Bloque Petición Información

        private void cargar_datos_peticion_info(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_AP.Attributes["class"] = blk_AP.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar la AP
            long id_ap = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;

            /// 2.- cargar combos para Petición de información
            cargar_combos_ap();

            /// 3.- Pintar los datos de la AP
            if (id_ap > 0)
            {
                List<campus_ACCIONES_PERSONA> _lead = da.getActionsByAP(id_ap);
                if (_lead.Count == 1)
                {
                    ddlOrigenAP.Value = _lead[0].IdOrigen != null ? _lead[0].IdOrigen.Value.ToString() : "-1";
                    ddlCursoAP.Value = _lead[0].IdCurso != null ? _lead[0].IdCurso.Value.ToString() : "-1";
                    ddlPertenenciaAP.Value = _lead[0].idPertenencia != null ? _lead[0].idPertenencia.Value.ToString() : "-1";
                    txtFechaLead.Value = _lead[0].Fecha.ToShortDateString();
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar la AP";
            }
            else
                txtFechaLead.Value = DateTime.Today.ToShortDateString();
        }

        private void cargar_combos_ap()
        {
            /// 1.- Cargar los origenes
            List<campus_AUX> _origenes = da.getAuxiliars(Constantes.aux.origen.GetStringValue());
            if (_origenes.Count > 0)
            {
                ddlOrigenAP.DataSource = _origenes;
                ddlOrigenAP.DataTextField = "Nombre";
                ddlOrigenAP.DataValueField = "ID_Aux";
                ddlOrigenAP.DataBind();
                ddlOrigenAP.Items.Insert(0, new ListItem("Seleccione un origen", "-1"));
                ddlOrigenAP.Value = "-1";
            }

            /// 1.2.- Comprobar que es comercial             
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (list_user.Count == 0 && Session["usuario"] != null)
                list_user.Add((CLIENTES)Session["usuario"]);

            //if (list_user[0].Comercial != null && list_user[0].Comercial.Value)
            //{
                /// 1.2.1.- Sacar las pertenecias del comercial
                List<campus_CLIENTES_COMERCIAL> _pert_comerciales = da.getPerteneciasById(list_user[0].id_cliente);
                if (_pert_comerciales.Count > 0)
                {
                    List<long> _ids = _pert_comerciales.Select(_ => _.idPertenencia).ToList();
                    List<campus_AUX> _pertenencia = da.getAuxiliars(Constantes.aux.pertenencia.GetStringValue());
                    _pertenencia = _pertenencia.Where(_ => _ids.Contains(_.ID_Aux)).ToList();
                    if (_pertenencia.Count > 0)
                    {
                        ddlPertenenciaAP.DataSource = _pertenencia;
                        ddlPertenenciaAP.DataTextField = "Nombre";
                        ddlPertenenciaAP.DataValueField = "ID_Aux";
                        ddlPertenenciaAP.DataBind();
                        ddlPertenenciaAP.Items.Insert(0, new ListItem("Seleccione una pertenencia", "-1"));
                        ddlPertenenciaAP.Value = "-1";
                    }
                }
                else
                {
                    ddlPertenenciaAP.Items.Insert(0, new ListItem("Seleccione una pertenencia", "-1"));
                    ddlPertenenciaAP.Value = "-1";
                }
            //}
            //else
            //{
            //    ddlPertenenciaAP.Items.Insert(0, new ListItem("Seleccione una pertenencia", "-1"));
            //    ddlPertenenciaAP.Value = "-1";
            //}

            /// 2.- Cargar los cursos
            List<campus_CURSO> _cursos = da.getAllCourses();
            if (_cursos.Count > 0)
            {
                /// 2.1.- Limpiar cursos
                ddlCursoAP.Items.Clear();

                /// 2.2.- Poner el orden de los cursos
                List<int> _types = new List<int>();
                _types.Add((int)Constantes.tipo_curso.Master_presencial);
                _types.Add((int)Constantes.tipo_curso.Master);
                _types.Add((int)Constantes.tipo_curso.Master_semipresencial);
                _types.Add((int)Constantes.tipo_curso.Master_online);
                _types.Add((int)Constantes.tipo_curso.Master_fundamentals);
                _types.Add((int)Constantes.tipo_curso.Experto_Universitario);
                _types.Add((int)Constantes.tipo_curso.Postgrado);
                _types.Add((int)Constantes.tipo_curso.Programa);
                _types.Add((int)Constantes.tipo_curso.Curso);
                _types.Add((int)Constantes.tipo_curso.Gratuito);
                _types.Add((int)Constantes.tipo_curso.Seminario);
                _types.Add(-1);

                /// 3.- Recorrer los cursos por tipo
                foreach (var _type in _types)
                {
                    /// 3.1.- Filtrar los cursos por tipo
                    List<campus_CURSO> _cursos_type = _cursos.Where(_ => _.idTipo_Curso == _type).OrderBy(_ => _.Nombre).ToList();

                    /// 3.2.- Recorrer los cursos
                    foreach (var _curso in _cursos_type)
                    {
                        ddlCursoAP.Items.Add(new ListItem(_curso.Nombre + " (" + _curso.ID_Curso + ")", _curso.ID_Curso.ToString()));

                        if (!_curso.Activo)
                            ddlCursoAP.Items[ddlCursoAP.Items.Count - 1].Attributes.Add("class", "text-color-red");
                    }
                }

                /// 4.- Inicializar el combo
                ddlCursoAP.Items.Insert(0, new ListItem("Seleccione un curso", "-1"));
                ddlCursoAP.Value = "-1";
            }
        }

        #endregion

        #region Bloque Origenes

        private void cargar_datos_origen(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_origen.Attributes["class"] = blk_origen.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Sacar las AP del usuario
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByUser(idUsuario);

            /// 2.- Sacar los origenes del usuario
            List<long> _ids_origenes = _actions.Where(_ => _.IdOrigen != null).Select(_ => _.IdOrigen.Value).ToList();

            /// 3.- Cargar los origenes
            List<campus_AUX> _origenes = da.getAuxiliars(Constantes.aux.origen.GetStringValue());

            /// 3.1.- Filtrar los origenes que ya tienen
            _origenes = _origenes.Where(_ => !_ids_origenes.Contains(_.ID_Aux)).ToList();
            if (_origenes.Count > 0)
            {
                ddlOrigenOR.DataSource = _origenes;
                ddlOrigenOR.DataTextField = "Nombre";
                ddlOrigenOR.DataValueField = "ID_Aux";
                ddlOrigenOR.DataBind();
                ddlOrigenOR.Items.Insert(0, new ListItem("Seleccione un origen", "-1"));
                ddlOrigenOR.Value = "-1";
            }

            /// 4.- Poner la fecha
            txtFechaOrigen.Value = DateTime.Today.ToShortDateString();
        }

        #endregion

        #region Bloque links

        private void cargar_datos_links(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_links.Attributes["class"] = blk_links.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar los parámetros de la url
            long id_link = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"].ToString()) : -1;

            /// 2.- Sacar los links de la BBDD
            List<campus_AUX> _links = da.getAuxiliars(Constantes.aux.links.GetStringValue());
            if (_links.Count > 0)
            {
                ddlLinks.DataSource = _links;
                ddlLinks.DataTextField = "Nombre";
                ddlLinks.DataValueField = "ID_Aux";
                ddlLinks.DataBind();
                ddlLinks.Items.Insert(0, new ListItem("Seleccione un link", "-1"));
                ddlLinks.Value = "-1";
            }

            if (id_link > 0)
            {
                /// 3.- Recuperar los links
                List<campus_OTROS> _link_user = da.getLinksById(id_link);
                if (_link_user.Count == 1)
                {
                    ddlLinks.Value = _link_user[0].Tipo.Value.ToString();
                    ddlLinks.Disabled = true;
                    txt_url_link.Value = _link_user[0].URL;
                }
            }
        }

        #endregion

        #region Fundación

        private void cargar_datos_fundacion(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_fundacion.Attributes["class"] = blk_fundacion.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar el seguimiento
            long id_seg = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;

            /// 2.- Pintar los datos del seguimiento
            if (id_seg > 0)
            {
                List<campus_SEG_COMERCIAL> _seguimiento = da.getSeguimientoComercialById(id_seg);
                if (_seguimiento.Count == 1)
                {
                    txtFechaSeguimientoFund.Value = _seguimiento[0].fecha.ToShortDateString();
                    txt_beca_fund.Value = _seguimiento[0].fundacion.ToString();
                    txt_desc_fund.Value = _seguimiento[0].descuento.ToString();
                    txt_comentarios_fund.Value = _seguimiento[0].Comentarios;
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar el seguimiento";
            }
            else
            {
                txtFechaSeguimientoFund.Value = DateTime.Today.ToShortDateString();
                txt_beca_fund.Value = "0";
                txt_desc_fund.Value = "0";
            }
        }

        #endregion

        #region Cambio edición

        private void cargar_datos_cambio_edicion(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_cambio_edicion.Attributes["class"] = blk_cambio_edicion.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar la docencia
            long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

            /// 2.- Pintar los datos de las docencias
            if (idDocencia > 0)
            {
                List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);
                if (_docencias.Count > 0)
                {
                    /// 3.- Sacar los datos de la docencia actual
                    List<campus_DOCENCIA> _docencia_actual = _docencias.Where(_ => _.ID_Docencia == idDocencia).ToList();
                    if (_docencia_actual.Count == 1)
                    {
                        /// 4.- Poner las docencia actual
                        ddlEdicionActual.DataSource = _docencias;
                        ddlEdicionActual.DataTextField = "Nombre";
                        ddlEdicionActual.DataValueField = "ID_Docencia";
                        ddlEdicionActual.DataBind();
                        ddlEdicionActual.Value = idDocencia.ToString();

                        /// 5.- Sacar las docencias con fecha posterior
                        List<campus_DOCENCIA> _docencias_nuevas = _docencias.Where(_ => _.ID_Docencia != idDocencia && _.FInicio >= _docencia_actual[0].FInicio && (_.PDP == null || !_.PDP.Value)).OrderBy(_ => _.Nombre).ToList();
                        if (_docencias_nuevas.Count > 0)
                        {
                            ddlNuevaEdicion.DataSource = _docencias_nuevas;
                            ddlNuevaEdicion.DataTextField = "Nombre";
                            ddlNuevaEdicion.DataValueField = "ID_Docencia";
                            ddlNuevaEdicion.DataBind();
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al buscar las nuevas docencias";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al buscar la docencia actual";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar las docencias";
            }
            else
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
        }

        #endregion

        #region Precio y Fecha fin acceso

        private void cargar_datos_precio_fin_acceso(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_precio.Attributes["class"] = blk_precio.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar la docencia
            long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

            /// 2.- Pintar los datos de la matrícula
            if (idDocencia > 0)
            {
                List<campus_DOCENCIA_GRUPO> _matricula = da.getDocenciaGrupo(idUsuario, idDocencia);
                if (_matricula.Count == 1)
                {
                    txtFechaFinAcceso.Value = _matricula[0].FFinAcceso != null ? _matricula[0].FFinAcceso.Value.ToShortDateString() : string.Empty;
                    txt_precio.Value = _matricula[0].PrecioPagado != null ? _matricula[0].PrecioPagado.Value.ToString() : "0";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar la matrícula";
            }
            else
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
        }

        #endregion

        #region Asignación comercial

        private void cargar_datos_asignacion_comercial(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_asignacion_comercial.Attributes["class"] = blk_asignacion_comercial.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar la petición y la asignación comercial
            long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
            long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

            /// 2.- Cargar combos
            cargar_combos_asig();

            /// 3.- Pintar los datos de la asignación comercial
            if (idDocencia > 0)
            {
                List<campus_ASIG_COMERCIAL> _asignacion = da.getAsigComercial(idDocencia, idCurso, idUsuario);
                if (_asignacion.Count == 1)
                {
                    txtFechaAsignacionComercial.Value = _asignacion[0].Fecha.ToShortDateString();
                    txt_precio_asig.Value = _asignacion[0].Precio != null ? _asignacion[0].Precio.Value.ToString() : "0";
                    ddlComerciales.Value = _asignacion[0].idVendedor.Value.ToString();
                    ddlDocenciaAsig.Value = _asignacion[0].idDocencia.ToString();
                    //ddlDocenciaAsig.Disabled = true;
                    ddlCursoAsig.Value = _asignacion[0].idCurso.ToString();
                    ddlCursoAsig.Disabled = true;
                    txt_comentarios_asig.Value = _asignacion[0].Comentarios;

                    if (_asignacion[0].pertenecia != null)
                        ddlPertenenciaAsig.Value = _asignacion[0].pertenecia.Value.ToString();
                    if (_asignacion[0].metodologia != null)
                        ddlMetodologiaAsig.Value = _asignacion[0].metodologia.Value.ToString();
                    if (_asignacion[0].captacion_tipo != null)
                        ddlCaptacionTipoAsig.Value = _asignacion[0].captacion_tipo.Value.ToString();
                    if (_asignacion[0].captacion_origen != null)
                        ddlCaptacionOrigenAsig.Value = _asignacion[0].captacion_origen.Value.ToString();
                    chkTitPropio.Checked = _asignacion[0].titulo_propio;
                    chkTitUcam.Checked = _asignacion[0].titulo_ucam;
                    chkTitSbsAd.Checked = _asignacion[0].titulo_sbs_ad;
                    chkTitSbsDo.Checked = _asignacion[0].titulo_sbs_do;
                    chkTitCualificam.Checked = _asignacion[0].titulo_cualificam;
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar la asignación comercial";
            }
            else
            {
                List<campus_DOCENCIA_GRUPO> _matricula = da.getDocenciaGrupo(idUsuario, idDocencia);
                if (_matricula.Count == 1)
                    txt_precio_asig.Value = _matricula[0].PrecioPagado != null ? _matricula[0].PrecioPagado.Value.ToString() : "0";
                else
                    txt_precio_asig.Value = "0";

                txtFechaAsignacionComercial.Value = DateTime.Today.ToShortDateString();
                ddlCursoAsig.Value = idCurso.ToString();
                ddlCursoAsig.Disabled = true;
            }
        }

        private void cargar_combos_asig()
        {
            /// 1.- Cargar los cursos
            List<campus_CURSO> _cursos = da.getAllCourses();
            if (_cursos.Count > 0)
            {
                /// 1.1.- Limpiar cursos
                ddlCursoAsig.Items.Clear();

                /// 1.2.- Poner el orden de los cursos
                List<int> _types = new List<int>();
                _types.Add((int)Constantes.tipo_curso.Master_presencial);
                _types.Add((int)Constantes.tipo_curso.Master);
                _types.Add((int)Constantes.tipo_curso.Master_semipresencial);
                _types.Add((int)Constantes.tipo_curso.Master_online);
                _types.Add((int)Constantes.tipo_curso.Master_fundamentals);
                _types.Add((int)Constantes.tipo_curso.Experto_Universitario);
                _types.Add((int)Constantes.tipo_curso.Postgrado);
                _types.Add((int)Constantes.tipo_curso.Programa);
                _types.Add((int)Constantes.tipo_curso.Curso);
                _types.Add((int)Constantes.tipo_curso.Gratuito);
                _types.Add((int)Constantes.tipo_curso.Seminario);
                _types.Add(-1);

                /// 1.3.- Recorrer los cursos por tipo
                foreach (var _type in _types)
                {
                    /// 1.3.1.- Filtrar los cursos por tipo
                    List<campus_CURSO> _cursos_type = _cursos.Where(_ => _.idTipo_Curso == _type).OrderBy(_ => _.Nombre).ToList();

                    /// 1.3.2.- Recorrer los cursos
                    foreach (var _curso in _cursos_type)
                    {
                        ddlCursoAsig.Items.Add(new ListItem(_curso.Nombre + " (" + _curso.ID_Curso + ")", _curso.ID_Curso.ToString()));

                        if (!_curso.Activo)
                            ddlCursoAsig.Items[ddlCursoAsig.Items.Count - 1].Attributes.Add("class", "text-color-red");
                    }
                }

                /// 1.4.- Inicializar el combo
                ddlCursoAsig.Items.Insert(0, new ListItem("Seleccione un curso", "-1"));
                ddlCursoAsig.Value = "-1";
            }

            /// 2.- Cargar las docencias de postgrado
            List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);
            List<int> _tipos_cursos = new List<int>();
            _tipos_cursos.Add((int)Constantes.tipo_curso.Master_presencial);
            _tipos_cursos.Add((int)Constantes.tipo_curso.Master);
            _tipos_cursos.Add((int)Constantes.tipo_curso.Master_semipresencial);
            _tipos_cursos.Add((int)Constantes.tipo_curso.Master_online);
            _tipos_cursos.Add((int)Constantes.tipo_curso.Master_fundamentals);
            _tipos_cursos.Add((int)Constantes.tipo_curso.Experto_Universitario);
            _tipos_cursos.Add((int)Constantes.tipo_curso.Postgrado);
            List<long> _ids_cursos = _cursos.Where(_ => _.idTipo_Curso != null && _tipos_cursos.Contains(_.idTipo_Curso.Value)).Select(_ => _.ID_Curso).ToList();

            /// 2.- Filtrar las docencias
            _docencias = _docencias.Where(_ => _ids_cursos.Contains(_.ID_Curso) && _.FFin >= DateTime.Today).OrderBy(_ => _.Nombre).ToList();
            if (_docencias.Count > 0)
            {
                ddlDocenciaAsig.DataSource = _docencias;
                ddlDocenciaAsig.DataTextField = "Nombre";
                ddlDocenciaAsig.DataValueField = "ID_Docencia";
                ddlDocenciaAsig.DataBind();
                ddlDocenciaAsig.Items.Insert(0, new ListItem("Seleccione una docencia", "-1"));
                ddlDocenciaAsig.Value = "-1";
            }

            /// 3.- Cargar los comerciales
            List<CLIENTES> _comerciales = da.getCommercialToReassign();
            _comerciales = _comerciales.OrderBy(_ => _.Nombre_Completo).ToList();
            if (_comerciales.Count > 0)
            {
                ddlComerciales.DataSource = _comerciales;
                ddlComerciales.DataTextField = "Nombre_Completo";
                ddlComerciales.DataValueField = "id_cliente";
                ddlComerciales.DataBind();
                ddlComerciales.Items.Insert(0, new ListItem("Seleccione un comercial", "-1"));
                ddlComerciales.Value = "-1";
            }

            /// 4.- Cargar las pertenencias
            List<campus_AUX> _pertenencias = da.getAuxiliars(Constantes.aux.pertenencia.GetStringValue());
            _pertenencias = _pertenencias.OrderBy(_ => _.Nombre).ToList();
            if (_pertenencias.Count > 0)
            {
                ddlPertenenciaAsig.DataSource = _pertenencias;
                ddlPertenenciaAsig.DataTextField = "Nombre";
                ddlPertenenciaAsig.DataValueField = "ID_Aux";
                ddlPertenenciaAsig.DataBind();
                ddlPertenenciaAsig.Items.Insert(0, new ListItem("Seleccione una pertenencia", "-1"));
                ddlPertenenciaAsig.Value = "-1";
            }

            /// 5.- Cargar las metodologias
            List<campus_AUX> _metodologias = da.getAuxiliars(Constantes.aux.metodologia.GetStringValue());
            _metodologias = _metodologias.OrderBy(_ => _.Nombre).ToList();
            if (_metodologias.Count > 0)
            {
                ddlMetodologiaAsig.DataSource = _metodologias;
                ddlMetodologiaAsig.DataTextField = "Nombre";
                ddlMetodologiaAsig.DataValueField = "ID_Aux";
                ddlMetodologiaAsig.DataBind();
                ddlMetodologiaAsig.Items.Insert(0, new ListItem("Seleccione una pertenencia", "-1"));
                ddlMetodologiaAsig.Value = "-1";
            }

            /// 6.- Cargar los tipos de captación
            List<campus_AUX> _tipos = da.getAuxiliars(Constantes.aux.captacion_tipo.GetStringValue());
            _tipos = _tipos.OrderBy(_ => _.Nombre).ToList();
            if (_tipos.Count > 0)
            {
                ddlCaptacionTipoAsig.DataSource = _tipos;
                ddlCaptacionTipoAsig.DataTextField = "Nombre";
                ddlCaptacionTipoAsig.DataValueField = "ID_Aux";
                ddlCaptacionTipoAsig.DataBind();
                ddlCaptacionTipoAsig.Items.Insert(0, new ListItem("Seleccione un tipo de captación", "-1"));
                ddlCaptacionTipoAsig.Value = "-1";
            }

            /// 7.- Cargar los origenes
            List<campus_AUX> _origenes = da.getAuxiliars(Constantes.aux.origen.GetStringValue());
            _origenes = _origenes.OrderBy(_ => _.Nombre).ToList();
            if (_origenes.Count > 0)
            {
                ddlCaptacionOrigenAsig.DataSource = _origenes;
                ddlCaptacionOrigenAsig.DataTextField = "Nombre";
                ddlCaptacionOrigenAsig.DataValueField = "ID_Aux";
                ddlCaptacionOrigenAsig.DataBind();
                ddlCaptacionOrigenAsig.Items.Insert(0, new ListItem("Seleccione una pertenencia", "-1"));
                ddlCaptacionOrigenAsig.Value = "-1";
            }

            /// 8.- Cargar los accesos
            List<campus_AUX> _accesos = da.getAuxiliars(Constantes.aux.acceso_por.GetStringValue());
            _accesos = _accesos.OrderBy(_ => _.Nombre).ToList();
            if (_accesos.Count > 0)
            {
                ddlAcceso.DataSource = _accesos;
                ddlAcceso.DataTextField = "Nombre";
                ddlAcceso.DataValueField = "ID_Aux";
                ddlAcceso.DataBind();
                ddlAcceso.Items.Insert(0, new ListItem("Seleccione un acceso", "-1"));
                ddlAcceso.Value = "-1";
            }
        }

        #endregion

        #region Asignación comercial 2

        private void cargar_datos_asignacion_comercial_all(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_asignacion_comercial_all.Attributes["class"] = blk_asignacion_comercial_all.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar la petición y la asignación comercial
            long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
            long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

            /// 2.- Pintar los datos de la asignación comercial
            if (idDocencia > 0)
            {
                List<campus_ASIG_COMERCIAL> _asignacion = da.getAsigComercial(idDocencia, idCurso, idUsuario);
                if (_asignacion.Count == 1)
                {
                    txtFechaPagoMatricula.Value = _asignacion[0].Fecha_Matricula != null ? _asignacion[0].Fecha_Matricula.Value.ToShortDateString() : string.Empty;
                    txtFechaEnvContrato.Value = _asignacion[0].Fecha_Env_Contrato != null ? _asignacion[0].Fecha_Env_Contrato.Value.ToShortDateString() : string.Empty;
                    txtFechaRecepContrato.Value = _asignacion[0].Fecha_Recep_Contrato != null ? _asignacion[0].Fecha_Recep_Contrato.Value.ToShortDateString() : string.Empty;
                    txt_pvp.Value = _asignacion[0].EUR_PVP != null ? _asignacion[0].EUR_PVP.Value.ToString() : "0";
                    txt_pvp_becado.Value = _asignacion[0].EUR_PVP_Becado != null ? _asignacion[0].EUR_PVP_Becado.Value.ToString() : "0";
                    txt_beca_fundacion.Value = _asignacion[0].Beca_Fundacion != null ? _asignacion[0].Beca_Fundacion.Value.ToString() : "0";
                    txt_otros_descuentos.Value = _asignacion[0].Otros_Descuentos != null ? _asignacion[0].Otros_Descuentos.Value.ToString() : "0";
                    txt_fundacion.Value = _asignacion[0].EUR_Aportacion_Fundacion != null ? _asignacion[0].EUR_Aportacion_Fundacion.Value.ToString() : "0";
                    txt_universidad.Value = _asignacion[0].EUR_Universidad != null ? _asignacion[0].EUR_Universidad.Value.ToString() : "0";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar la asignación comercial";
            }
            else
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
        }

        #endregion

        #region Pagos

        private void cargar_datos_pago(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_pago.Attributes["class"] = blk_pago.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar el pago
            long idPago = !String.IsNullOrEmpty(Request.QueryString["idp"]) ? long.Parse(Request.QueryString["idp"].ToString()) : -1;

            /// 2.- Pintar los datos de pago
            if (idPago > 0)
            {
                List<campus_DATA_COMERCIAL> _pago = da.getDataComercialById(idPago);
                if (_pago.Count == 1)
                {
                    txtFechaEstimada.Value = _pago[0].Fecha_est.ToShortDateString();
                    txt_euros_est.Value = _pago[0].EUR_est.ToString();
                    txtFechaReal.Value = _pago[0].Fecha_real != null ? _pago[0].Fecha_real.Value.ToShortDateString() : string.Empty;
                    txt_euros_real.Value = _pago[0].EUR_real != null ? _pago[0].EUR_real.Value.ToString() : string.Empty;
                }
            }
        }

        #endregion

        #region Documentación

        private void cargar_datos_documentacion(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_documentacion.Attributes["class"] = blk_documentacion.Attributes["class"].Replace("hidden", string.Empty);

            /// 0.1.- Poner el fileupload
            file_documento.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre el documento del usuario en el área seleccionada</span><input id='fileupload_documento' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"type\": \"file_doc\", \"accion\": \"update\" }' />";

            /// 1.- Recuperar el documento
            long idDocumento = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"].ToString()) : -1;

            /// 2.- Cargar combos
            cargar_docencias_doc();

            /// 3.- Poner los datos del documento
            if (idDocumento > 0)
            {
                List<campus_CLIENTES_DOC> _documento = da.getDocsClientes(idDocumento);
                if (_documento.Count == 1)
                {
                    ddlTipoDoc.Value = _documento[0].tipo_documento;
                    txtFechaCaducidad.Value = _documento[0].fecha_caducidad != null ? _documento[0].fecha_caducidad.Value.ToShortDateString() : string.Empty;
                    if (_documento[0].id_docencia != null)
                        ddlDocenciaDocumento.Value = _documento[0].id_docencia.Value.ToString();
                    txt_descripcion_doc.Value = _documento[0].Descripcion;
                    documento_usuario.Value = _documento[0].Fichero;

                    if (!String.IsNullOrEmpty(documento_usuario.Value))
                    {
                        block_delete_documento.Attributes["class"] = block_delete_documento.Attributes["class"].Replace("hidden", string.Empty);
                        block_see.InnerHtml = "<label class='full-width'>&nbsp;</label><a id='lnk_documento' href='" + ConfigurationManager.AppSettings["url_documentos"] + _documento[0].idCliente + "/" + documento_usuario.Value + "' target='_blank' title='Ver documento' class='fas fa-eye fa-2x' runat='server'></a>";
                        block_see.Attributes["class"] = block_see.Attributes["class"].Replace("hidden", string.Empty);
                        block_upload_documento.Attributes["class"] = block_upload_documento.Attributes["class"].Insert(block_upload_documento.Attributes["class"].Length, " hidden");
                    }
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar el documento";
            }
        }

        private void cargar_docencias_doc()
        {
            /// 1.- Sacar las docencias de la BBDD
            List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);

            /// 2.- Recorrer las docencias
            if (_docencias.Count > 0)
            {
                ddlDocenciaDocumento.DataSource = _docencias;
                ddlDocenciaDocumento.DataTextField = "Nombre";
                ddlDocenciaDocumento.DataValueField = "ID_Docencia";
                ddlDocenciaDocumento.DataBind();
                ddlDocenciaDocumento.Items.Insert(0, new ListItem("Seleccione una docencia", "-1"));
                ddlDocenciaDocumento.Value = "-1";
            }
        }

        [WebMethod(Description = "Eliminar el documento del usuario")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool eliminar_documento(long id_document, string document)
        {
            DataAccess da = new DataAccess();
            bool _delete = false;
            try
            {
                if (id_document > 0)
                {
                    List<campus_CLIENTES_DOC> _documento = da.getDocsClientes(id_document);
                    if (_documento.Count == 1)
                    {
                        /// 1.0.- Sacar los datos del documento
                        campus_CLIENTES_DOC _document = _documento[0];

                        /// 1.1.- Sacar la ruta de la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_documentos"] + _documento[0].idCliente + "\\";

                        /// 1.2.- Eliminar el fichero
                        if (!String.IsNullOrEmpty(_document.Fichero))
                            File.Delete(ruta + _document.Fichero);

                        /// 1.3.- Actualizar los datos del documento                
                        _document.Fichero = null;

                        /// 1.4.- Actualizar la BBDD
                        _delete = da.updateDocumento(_document);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-usuario.cs::eliminar_documento()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _delete = false;
            }
            return _delete;
        }

        #endregion

        #region Informe tripartita

        private void cargar_datos_inf_tripartita(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_inf_tripartita.Attributes["class"] = blk_inf_tripartita.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Sacar las entradas del log y los datos del usuario
            List<campus_LOG> _logs = da.getLogEntries(idUsuario);
            List<CLIENTES> _user = da.getUserById(idUsuario);

            /// 2.- Pintar las entradas
            if (_user.Count == 1)
                table_list_tripartita.InnerHtml = paint_table_inf_tripartita(_user[0], _logs);
        }

        private string paint_table_inf_tripartita(CLIENTES _user, List<campus_LOG> _logs)
        {
            StringBuilder sbuild = new StringBuilder();
                        
            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_inf_tripartita\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Id</th>");
            sbuild.Append("<th>Nombre Completo</th>");
            sbuild.Append("<th>Mail</th>");
            sbuild.Append("<th>Descripción</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer las entradas en el log
            foreach (var _log in _logs)
            {
                sbuild.Append("<tr>");
                sbuild.Append($"<td>{_log.Fecha_Hora}</td>");
                sbuild.Append($"<td>{_user.id_cliente}</td>");
                sbuild.Append($"<td>{_user.Nombre_Completo}</td>");
                sbuild.Append($"<td>{_user.email}</td>");
                sbuild.Append($"<td>{_log.Descripcion}</td>");
                sbuild.Append($"</tr>");
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        #endregion

        #region Comentarios

        private void cargar_datos_comentarios(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_comentarios.Attributes["class"] = blk_comentarios.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar la petición y la asignación comercial
            long idComentario = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
            if (idComentario > 0)
            {
                /// 2.- Recuperar el comentario de la BBDD
                List<CLIENTES_COMENTARIOS> _comentario = da.getComentariosClientes(idComentario);
                if (_comentario.Count == 1)
                    txt_comentarios_user.InnerHtml = _comentario[0].Comentario;
            }
        }

        #endregion

        #region Avance

        private void cargar_datos_avance(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_avance.Attributes["class"] = blk_avance.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Sacar las docencias del usuario
            List<campus_DOCENCIA_GRUPO> _docencias_grupo = da.getDocenciasGrupo(idUsuario);
            if (_docencias_grupo.Count > 0)
            {
                /// 2.- Sacar el resto de datos de la BBDD
                List<long> _ids_docencias = _docencias_grupo.Select(_ => _.ID_Docencia).Distinct().ToList();
                List<campus_DOCENCIA> _docencias = da.getDocencias(null);
                _docencias = _docencias.Where(_ => _ids_docencias.Contains(_.ID_Docencia)).ToList();
                List<campus_TUTORIA> _tutorias = da.getTutorias(-1);
                List<campus_CONTENIDO_PROGRAMA> _programas = da.getContenidoPrograma();
                List<campus_CURSO> _cursos = da.getCourses(null);
                List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluaciones = da.getDocenciasGrupoEvaluacion(idUsuario);

                /// 3.- Pintar las entradas
                table_list_avance.InnerHtml = paint_table_avances(idUsuario, _docencias, _cursos, _programas, _tutorias, _evaluaciones);
            }
        }

        private string paint_table_avances(long idUsuario, List<campus_DOCENCIA> _docencias, List<campus_CURSO> _cursos, List<campus_CONTENIDO_PROGRAMA> _programas, List<campus_TUTORIA> _tutorias, List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluaciones)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar el resto de datos
            List<campus_CONTENIDO_DOCENCIA> _contents_all = da.getContentDocencia(-1, -1, true);
            List<campus_LOG> _recursos = da.getLogEntries(idUsuario);
            _recursos = _recursos.Where(l => l.Descripcion.StartsWith("View resource")).ToList();

            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (Session["usuario"] != null)
                _comerciales.Add((CLIENTES)Session["usuario"]);

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_avance\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>F. Inicio</th>");
            sbuild.Append("<th>F. Fin</th>");
            sbuild.Append("<th>% Avance</th>");
            sbuild.Append("<th>Calificación</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer las entradas
            foreach (var _docencia in _docencias)
            {
                List<long> _id_cursos = _programas.Where(_ => _.ID_Programa == _docencia.ID_Curso).Select(_ => _.ID_Curso).ToList();
                _id_cursos.Add(_docencia.ID_Curso);
                if (_id_cursos.Count > 0)
                {
                    foreach (var _curso in _id_cursos)
                    {
                        sbuild.Append("<tr>");
                        sbuild.Append($"<td>{_docencia.Nombre} ({_docencia.ID_Docencia})</td>");
                        sbuild.Append($"<td>{_cursos.Where(_ => _.ID_Curso == _curso).Select(_ => _.Nombre).FirstOrDefault()} ({_curso})</td>");
                        sbuild.Append($"<td>{_tutorias.Where(_ => _.idDocencia == _docencia.ID_Docencia && _.idCurso == _curso).Select(_ => _.FechaInicio.ToShortDateString()).FirstOrDefault()}</td>");
                        sbuild.Append($"<td>{_tutorias.Where(_ => _.idDocencia == _docencia.ID_Docencia && _.idCurso == _curso).Select(_ => _.FechaFin.ToShortDateString()).FirstOrDefault()}</td>");
                        sbuild.Append($"<td>{search_advance(_docencia.ID_Docencia, _curso, _programas, _recursos, _contents_all)} %</td>");
                        sbuild.Append($"<td>{_evaluaciones.Where(_ => _.idDocencia == _docencia.ID_Docencia && _.idCurso == _curso).Select(_ => _.calificacion).FirstOrDefault()} ({_evaluaciones.Where(_ => _.idDocencia == _docencia.ID_Docencia && _.idCurso == _curso).Select(_ => _.nota).FirstOrDefault()})</td>");
                        if (_comerciales.Count == 1)
                            sbuild.Append($"<td><a title='Calificar' href='https://campuspro.spainbs.com/calificar.aspx?idd={_docencia.ID_Docencia}&idc={_curso}&idu={idUsuario}&k={_comerciales[0].Key}' target='_blank'><i class='fas fa-graduation-cap fa-2x'></i></a></td>");
                        else
                            sbuild.Append($"<td><a title='Calificar' href='https://campuspro.spainbs.com/calificar.aspx?idd={_docencia.ID_Docencia}&idc={_curso}&idu={idUsuario}' target='_blank'><i class='fas fa-graduation-cap fa-2x'></i></a></td>");
                        sbuild.Append("</tr>");
                    }
                }
            }
            sbuild.Append("</tbody></table>");
            return sbuild.ToString();
        }
        
        private decimal search_advance(long idDocencia, long idCurso, List<campus_CONTENIDO_PROGRAMA> _programas, List<campus_LOG> _recursos, List<campus_CONTENIDO_DOCENCIA> _contenidos)
        {
            decimal _advance = new decimal(0);

            try
            {
                /// 1.- Sacar los contenidos visibles
                List<campus_CONTENIDO_DOCENCIA> list_contents_all = new List<campus_CONTENIDO_DOCENCIA>();

                /// 1.1.- Sacar los cursos que componen un programa
                List<long> list_id_courses = _programas.Where(p => p.ID_Programa == idCurso).Select(p => (long)p.ID_Curso).ToList();
                if (list_id_courses.Count > 0)
                    list_contents_all = _contenidos.Where(cd => cd.ID_Docencia == idDocencia).ToList();
                else
                    list_contents_all = _contenidos.Where(cd => cd.ID_Docencia == idDocencia && cd.ID_Curso == idCurso).ToList();

                /// 2.- Sacar los contenidos obligatorios
                List<campus_CONTENIDO_DOCENCIA> list_contents = list_contents_all.Where(c => c.Lectura == 0).ToList();
                if (list_contents.Count > 0)
                {                   
                    /// 3.- Obtener los id de los recursos vistos
                    List<long> list_id = new List<long>();
                    if (_recursos.Count > 0)
                    {
                        for (int index = 0; index < _recursos.Count; index++)
                        {
                            list_id.Add(long.Parse(_recursos[index].Descripcion.Substring(_recursos[index].Descripcion.IndexOf('(') + 1).Replace(')', ' ').Trim()));
                        }
                    }
                    list_id = list_id.Distinct().ToList();

                    /// 5.- Obtener el nº de recursos vistos de la docencia - curso
                    List<campus_CONTENIDO_DOCENCIA> list_contents_viewed = new List<campus_CONTENIDO_DOCENCIA>();
                    if (list_contents.Count > 0 && list_id.Count > 0)
                        list_contents_viewed = list_contents.Where(c => list_id.Contains(c.ID_Recurso)).ToList();

                    /// 6.- Sacar el porcentaje
                    _advance = Math.Round((((decimal)list_contents_viewed.Count / (decimal)list_contents.Count) * 100), 2);
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - listado_alumnos.cs::search_advance()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _advance = 0;
            }
            return _advance;
        }

        #endregion

        #region Unificar alumnos

        private void cargar_datos_usuario_unificar(long idUsuario)
        {
            /// 0.- Mostrar el bloque
            blk_unificar_usuarios.Attributes["class"] = blk_unificar_usuarios.Attributes["class"].Replace("hidden", string.Empty);

            /// 1.- Recuperar el usuario a eliminar
            long idUsuarioOld = !String.IsNullOrEmpty(Request.QueryString["ido"]) ? long.Parse(Request.QueryString["ido"].ToString()) : -1;
            if (idUsuarioOld == -1)
            {
                blk_data_user.Visible = false;
                txt_user.Value = idUsuario.ToString();
                btnUnificarUsuario.Visible = false;
            }
            else
            {

            }
        }

        #endregion

        private void cargar_datos_incidencia(long idUsuario)
        {
            /// 1.- Buscar si tiene una entrada de incidencia
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByUserIncidence(idUsuario, (long)Constantes.course.Incidencia);
            if (_actions.Count > 0)
                Response.Redirect($"seguimiento-comercial-crm.aspx?idu={idUsuario}&idap={_actions[0].idAccionPersona}");
            else
            {
                /// 2.- Crear una entrada de incidencia
                campus_ACCIONES_PERSONA _lead = new campus_ACCIONES_PERSONA();
                _lead.idAccion = (long)Constantes.accion.Peticion_informacion;
                _lead.IdOrigen = (long)Constantes.origen.Alumno_Ex;
                _lead.Fecha = DateTime.Now;
                _lead.IdCurso = (long)Constantes.course.Incidencia;
                _lead.idPersona = idUsuario;
                _lead.Procesado = true;

                long _insert_lead = da.insertPersonAction(_lead);
                if (_insert_lead > 0)
                    Response.Redirect($"seguimiento-comercial-crm.aspx?idu={idUsuario}&idap={_insert_lead}");
            }
        }
    }
}