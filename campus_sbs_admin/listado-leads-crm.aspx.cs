using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class listado_leads_crm : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el parámetro 'k' en la url
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                    Session.Add("usuario", list_user[0]);
            }

            if (list_user.Count == 0)
            {
                if (list_user.Count == 0 && Session["usuario"] != null)
                    list_user.Add((CLIENTES)Session["usuario"]);
                else
                    Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                /// Comprobar al usuario
                bool comprobate_user = Utilities.comprobate_users(list_user[0]);
                if (!comprobate_user)
                    Response.Redirect("login.aspx");

                /// 1.- Cargar el listado de leads de nuevos
                int _count_nuevos = 0;
                int _count_nuevos_all = 0;
                int _count_avisos = 0;
                if (list_user[0].Comercial != null && list_user[0].Comercial.Value)
                {
                    table_list_leads.InnerHtml = load_leads((int)Constantes.tab_position_AP.nuevos, (int)Constantes.type_status_action.status_nuevo, list_user[0].id_cliente, false, ref _count_nuevos, ref _count_nuevos_all, ref _count_avisos, false);
                    count_nuevos.InnerHtml = _count_nuevos.ToString();
                    count_avisos.InnerHtml = _count_avisos.ToString();
                    count_nuevos_all.Visible = false;
                    hid_comercial.Value = list_user[0].id_cliente.ToString();
                    hid_comercial_avisos.Value = list_user[0].id_cliente.ToString();
                }
                else
                {
                    table_list_leads.InnerHtml = load_leads((int)Constantes.tab_position_AP.nuevos, (int)Constantes.type_status_action.status_nuevo, list_user[0].id_cliente, false, ref _count_nuevos, ref _count_nuevos_all, ref _count_avisos, true);
                    count_nuevos.InnerHtml = _count_nuevos.ToString();
                    count_nuevos_all.InnerHtml = _count_nuevos_all.ToString();
                    count_avisos.InnerHtml = _count_avisos.ToString();
                    hid_comercial_avisos.Value = list_user[0].id_cliente.ToString();
                }

                /// 2.- Poner los contadores de las pestañas
                load_counts_tabs(list_user[0].id_cliente, _count_nuevos, _count_avisos);

                /// 3.- Cargar los combos
                cargar_comerciales(list_user[0].id_cliente);
                cargar_status();
            }
        }

        private void load_counts_tabs(long _comercial, int _count_nuevos, int _count_avisos)
        {
            /// 1.- Sacar los contadores de las tabs
            List<campus_SEG_COMERCIAL_INF> _seguimientos_inf = da.getSegComercialInf(_comercial);
            if (_seguimientos_inf.Count > 0)
            {
                /// 1.- Sin contactar
                count_sin_contactar.InnerHtml = _seguimientos_inf.Where(_ => _.estado == (int)Constantes.type_status_action.status_sin_contactar).Sum(_ => _.numero).ToString();

                /// 2.- En proceso
                count_proceso.InnerHtml = _seguimientos_inf.Where(_ => _.estado == (int)Constantes.type_status_action.status_indeciso || _.estado == (int)Constantes.type_status_action.status_interesado || _.estado == (int)Constantes.type_status_action.status_pago || _.estado == (int)Constantes.type_status_action.status_receive || _.estado == (int)Constantes.type_status_action.status_send).Sum(_ => _.numero).ToString();

                /// 3.- Futuro
                count_futuro.InnerHtml = _seguimientos_inf.Where(_ => _.estado == (int)Constantes.type_status_action.status_futuro).Sum(_ => _.numero).ToString();

                /// 4.- Todos
                count_todos.InnerHtml = (_seguimientos_inf.Sum(_ => _.numero) + _count_nuevos + _count_avisos).ToString();
            }
            else
                count_todos.InnerHtml = (_count_nuevos + _count_avisos).ToString();
        }
        private void cargar_comerciales(long _comercial)
        {
            /// 1.- Sacar los comerciales
            List<CLIENTES> lst_users = da.getCommercialToReassign();
            if (lst_users.Count > 0)
            {
                /// 1.1.- Sin contactar
                contact_person.DataSource = lst_users;
                contact_person.DataTextField = "Nombre_Completo";
                contact_person.DataValueField = "id_cliente";
                contact_person.DataBind();
                contact_person.Items.Add(new ListItem("Todos", "-1"));
                contact_person.Value = _comercial.ToString();

                /// 1.2.- En proceso
                process_person.DataSource = lst_users;
                process_person.DataTextField = "Nombre_Completo";
                process_person.DataValueField = "id_cliente";
                process_person.DataBind();
                process_person.Items.Add(new ListItem("Todos", "-1"));
                process_person.Value = _comercial.ToString();

                /// 1.3.- Futuro
                future_person.DataSource = lst_users;
                future_person.DataTextField = "Nombre_Completo";
                future_person.DataValueField = "id_cliente";
                future_person.DataBind();
                future_person.Items.Add(new ListItem("Todos", "-1"));
                future_person.Value = _comercial.ToString();

                /// 1.4.- Todos
                all_person.DataSource = lst_users;
                all_person.DataTextField = "Nombre_Completo";
                all_person.DataValueField = "id_cliente";
                all_person.DataBind();
                all_person.Items.Add(new ListItem("Todos", "-1"));
                all_person.Value = _comercial.ToString();
            }
        }
        private void cargar_status()
        {
            all_status.Items.Clear();
            all_status.Items.Add(new ListItem("Todos los estados", "-1"));
            all_status.Items.Add(new ListItem("Rechazado", ((int)Constantes.type_status_action.status_rechazado).ToString()));
            all_status.Items.Add(new ListItem("Duplicado", ((int)Constantes.type_status_action.status_duplicado).ToString()));
            all_status.Items.Add(new ListItem("Futuro", ((int)Constantes.type_status_action.status_futuro).ToString()));
            all_status.Items.Add(new ListItem("Cerrado", ((int)Constantes.type_status_action.status_cerrar).ToString()));
            all_status.Items.Add(new ListItem("Sin contactar", ((int)Constantes.type_status_action.status_sin_contactar).ToString()));
            all_status.Items.Add(new ListItem("Indeciso", ((int)Constantes.type_status_action.status_indeciso).ToString()));
            all_status.Items.Add(new ListItem("Interesado", ((int)Constantes.type_status_action.status_interesado).ToString()));
            all_status.Items.Add(new ListItem("Enviar contrato", ((int)Constantes.type_status_action.status_send).ToString()));
            all_status.Items.Add(new ListItem("Recibir contrato", ((int)Constantes.type_status_action.status_receive).ToString()));
            all_status.Items.Add(new ListItem("Pago", ((int)Constantes.type_status_action.status_pago).ToString()));
            all_status.Items.Add(new ListItem("Matriculado", ((int)Constantes.type_status_action.status_matriculado).ToString()));
        }

        private static string load_leads(int _tab, int _status, long _comercial, bool _aviso, ref int _count_nuevos, ref int _count_nuevos_all, ref int _count_avisos, bool _admin)
        {
            DataAccess da = new DataAccess();

            /// 0.- Sacar las acciones
            List<long> _id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                _id_act.Add(long.Parse(act));
            }

            /// 1.- Sacar los estados
            List<int> _id_status = new List<int>();
            if (_status > -1 && _status != (int)Constantes.type_status_action.status_proceso)
                _id_status.Add(_status);
            else if (_status == (int)Constantes.type_status_action.status_proceso)
            {
                _id_status.Add((int)Constantes.type_status_action.status_indeciso);
                _id_status.Add((int)Constantes.type_status_action.status_interesado);
                _id_status.Add((int)Constantes.type_status_action.status_send);
                _id_status.Add((int)Constantes.type_status_action.status_receive);
                _id_status.Add((int)Constantes.type_status_action.status_pago);
            }

            /// 2.- Sacar los leads de la BBDD
            List<Acciones_Persona> _leads = new List<Acciones_Persona>();
            if (_status == (int)Constantes.type_status_action.status_nuevo)
            {
                if (_admin)
                    _leads = da.getAP(_id_status, _id_act, false, -1);
                else
                    _leads = da.getAP(_id_status, _id_act, false, _comercial);
            }
            else if (_status == -1)
                _leads = da.getAP(_id_status, _id_act, null, _comercial);
            else
                _leads = da.getAP(_id_status, _id_act, true, _comercial);

            /// 2.1.- Filtrar los leads incorrectos 243 y curso 22
            List<Acciones_Persona> _leads_incorrectos = _leads.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 2.2.- Filtrar los leads
            _leads = _leads.Except(_leads_incorrectos).ToList();

            /// 2.3.- Sacar los leads de hoy y anteriores
            DateTime fecha = DateTime.Today.AddDays(1);
            _leads = _leads.Where(act => act.Fecha < fecha).ToList();

            /// 3.- Pintar las tablas
            if (_tab == (int)Constantes.tab_position_AP.nuevos)
            {
                /// 3.0.- Sacar el contador de avisos
                List<Acciones_Persona> _leads_avisos = _leads.Where(_ => _.Comentario == "Recordatorio automático").ToList();
                if (_leads_avisos.Count > 0)
                {
                    List<CLIENTES> lst_usuario = new List<CLIENTES>();
                    if (HttpContext.Current.Session["usuario"] != null)
                        lst_usuario.Add((CLIENTES)HttpContext.Current.Session["usuario"]);

                    _count_avisos = _leads_avisos.Where(_ => _.idComercial == lst_usuario[0].id_cliente).Count();
                }

                /// 3.1.- Filtrar los leads que son recordatorios
                _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

                /// 3.2.- Poner contador global
                _count_nuevos_all = _leads.Count;

                /// 3.3.- Filtrar por comercial
                if (_comercial > 0 && !_admin)
                    _leads = _leads.Where(_ => _.idComercial == _comercial).ToList();
                else
                    _leads = _leads.Where(_ => _.idComercial == null || _.idComercial == _comercial).ToList();

                /// 3.4.- Poner contador
                _count_nuevos = _leads.Count;

                /// 3.5.- Pintar la tabla
                if (_leads.Count > 0)
                {
                    /// 3.6.- Sacar los datos de la BBDD
                    List<long> _id_users = _leads.Select(_ => _.idPersona).Distinct().ToList();
                    List<CLIENTES> _users = da.getUserByList(_id_users);
                    List<campus_CURSO> _courses = da.getCourses(null);
                    List<campus_AUX> _aux = da.getAuxiliars(string.Empty);
                    List<Paises> _countries = da.getCountries();

                    return paint_table_leads(_tab, _leads, _users, _courses, _aux, _countries, _comercial);
                }
                else
                    return paint_table_leads(_tab, _leads, new List<CLIENTES>(), new List<campus_CURSO>(), new List<campus_AUX>(), new List<Paises>(), _comercial);
            }
            else if (_tab == (int)Constantes.tab_position_AP.avisos)
            {
                /// 3.1.- Filtrar los leads que son recordatorios
                _leads = _leads.Where(_ => _.Comentario == "Recordatorio automático").ToList();

                /// 3.2.- Filtrar por comercial
                if (_comercial > 0)
                    _leads = _leads.Where(_ => _.idComercial == _comercial).ToList();

                /// 3.3.- Pintar la tabla
                if (_leads.Count > 0)
                {
                    /// 3.4.- Sacar los datos de la BBDD
                    List<long> _id_users = _leads.Select(_ => _.idPersona).Distinct().ToList();
                    List<CLIENTES> _users = da.getUserByList(_id_users);
                    List<campus_CURSO> _courses = da.getCourses(null);
                    List<campus_AUX> _aux = da.getAuxiliars(string.Empty);
                    List<Paises> _countries = da.getCountries();

                    return paint_table_leads(_tab, _leads, _users, _courses, _aux, _countries, _comercial);
                }
                else
                    return paint_table_leads(_tab, _leads, new List<CLIENTES>(), new List<campus_CURSO>(), new List<campus_AUX>(), new List<Paises>(), _comercial);
            }
            else
            {
                /// 3.1.- Pintar la tabla
                if (_leads.Count > 0)
                {
                    /// 3.2.- Sacar los datos de la BBDD
                    List<long> _id_users = _leads.Select(_ => _.idPersona).Distinct().ToList();
                    List<CLIENTES> _users = da.getUserByList(_id_users);
                    List<campus_CURSO> _courses = da.getCourses(null);
                    List<campus_AUX> _aux = da.getAuxiliars(string.Empty);
                    List<Paises> _countries = da.getCountries();

                    return paint_table_leads(_tab, _leads, _users, _courses, _aux, _countries, _comercial);
                }
                else
                    return paint_table_leads(_tab, _leads, new List<CLIENTES>(), new List<campus_CURSO>(), new List<campus_AUX>(), new List<Paises>(), _comercial);
            }
        }
        private static string paint_table_leads(int _tab, List<Acciones_Persona> _leads, List<CLIENTES> _users, List<campus_CURSO> _courses, List<campus_AUX> _aux, List<Paises> _countries, long _comercial)
        {
            DataAccess da = new DataAccess();
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            if (_tab == (int)Constantes.tab_position_AP.nuevos)
                sbuild.Append("<table id =\"tabla_Leads_Nuevos\" class=\"display compact\" style =\"width:100%\"><thead>");
            else if (_tab == (int)Constantes.tab_position_AP.avisos)
                sbuild.Append("<table id =\"tabla_Leads_Avisos\" class=\"display compact\" style =\"width:100%\"><thead>");
            else if (_tab == (int)Constantes.tab_position_AP.sin_contactar)
                sbuild.Append("<table id =\"tabla_Leads_Sin_Contactar\" class=\"display compact\" style =\"width:100%\"><thead>");
            else if (_tab == (int)Constantes.tab_position_AP.proceso)
                sbuild.Append("<table id =\"tabla_Leads_Proceso\" class=\"display compact\" style =\"width:100%\"><thead>");
            else if (_tab == (int)Constantes.tab_position_AP.futuro)
                sbuild.Append("<table id =\"tabla_Leads_Futuro\" class=\"display compact\" style =\"width:100%\"><thead>");
            else if (_tab == (int)Constantes.tab_position_AP.todos)
                sbuild.Append("<table id =\"tabla_Leads_Todos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Acción</th>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>Usuario</th>");
            sbuild.Append("<th>Scoring</th>");
            sbuild.Append("<th>Resumen</th>");
            if (_tab == (int)Constantes.tab_position_AP.proceso || _tab == (int)Constantes.tab_position_AP.todos)
                sbuild.Append("<th>Estado</th>");

            if (_tab != (int)Constantes.tab_position_AP.nuevos && _tab != (int)Constantes.tab_position_AP.avisos)
                sbuild.Append("<th title='Fecha último seguimiento'>F. Seg</th>");

            if (_tab != (int)Constantes.tab_position_AP.nuevos)
                sbuild.Append("<th>Comentario</th>");

            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los leads
            if (_leads.Count > 0)
            {
                /// 3.1.- Comprueba si puede ver los origenes
                bool _ver_origenes = true;
                if (_comercial > 0)
                {
                    List<campus_ACL> _acl = da.getACL(_comercial);
                    if (_acl.Count == 1)
                    {
                        if (!_acl[0].Ver_origenes)
                            _ver_origenes = false;
                    }
                }

                /// 3.2.- Recorre los origenes
                foreach (var _lead in _leads)
                {
                    sbuild.Append("<tr>");

                    /// 3.1.- Fecha
                    sbuild.Append($"<td>{_lead.Fecha}</td>");

                    /// 3.2.- Origen
                    if (_ver_origenes && _lead.IdOrigen != null)
                        sbuild.Append($"<td>{_aux.Where(_ => _.ID_Aux == _lead.IdOrigen).Select(_ => _.Nombre).FirstOrDefault()} ({_lead.IdOrigen})</td>");
                    else
                        sbuild.Append("<td></td>");

                    /// 3.3.- Acción
                    if (_lead.idAccion == 91)
                        sbuild.Append($"<td>BECAS ({_lead.idAccion})</td>");
                    else if (_lead.idAccion == 99)
                        sbuild.Append($"<td>INFO PROG. ({_lead.idAccion})</td>");
                    else if (_lead.idAccion == 243)
                        sbuild.Append($"<td>LANDING. ({_lead.idAccion})</td>");
                    else
                        sbuild.Append($"<td>({_lead.idAccion})</td>");

                    /// 3.4.- Curso
                    if (_lead.IdCurso != null)
                        sbuild.Append($"<td>{_courses.Where(_ => _.ID_Curso == _lead.IdCurso).Select(_ => _.Nombre).FirstOrDefault()} ({_lead.IdCurso})</td>");
                    else
                        sbuild.Append($"<td>({_lead.IdCurso})</td>");

                    /// 3.5.- Usuario
                    long _country = _users.Where(_ => _.id_cliente == _lead.idPersona).Select(_ => _.id_pais).FirstOrDefault() != null ? _users.Where(_ => _.id_cliente == _lead.idPersona).Select(_ => _.id_pais).FirstOrDefault().Value : -1;
                    sbuild.Append($"<td>{_users.Where(_ => _.id_cliente == _lead.idPersona).Select(u => u.Nombre_Completo).FirstOrDefault()} ({_lead.idPersona}) [{_countries.Where(_ => _.id_pais == _country).Select(_ => _.nombre).FirstOrDefault()}]</td>");

                    /// 3.6.- Scoring
                    sbuild.Append($"<td>{_lead.scoring_lead}</td>");

                    /// 3.7.- Resumen
                    sbuild.Append($"<td>{(!String.IsNullOrEmpty(_lead.resumen_metodologia) ? "<span class='badge badge-pill badge-info' data-toggle='tooltip' data-placement='top' data-html='true' title='Metodología'>" + _lead.resumen_metodologia + "</span>" : string.Empty)} {(!String.IsNullOrEmpty(_lead.resumen_cuando) ? "<span class='badge badge-pill badge-warning' data-toggle='tooltip' data-placement='top' data-html='true' title='Cuando'>" + _lead.resumen_cuando + "</span>" : string.Empty)} {(_lead.resumen_beca != null ? "<span class='badge badge-pill badge-success' data-toggle='tooltip' data-placement='top' data-html='true' title='Beca'>" + _lead.resumen_beca + "</span>" : string.Empty)} {(_lead.resumen_descuento != null ? "<span class='badge badge-pill badge-success' data-toggle='tooltip' data-placement='top' data-html='true' title='Descuento'>" + _lead.resumen_descuento + "</span>" : string.Empty)} {(!String.IsNullOrEmpty(_lead.resumen_comentarios) ? "<span class='badge badge-pill badge-secondary badge-comments' data-toggle='tooltip' data-placement='top' data-html='true' title='Comentarios'>" + _lead.resumen_comentarios + "</span>" : string.Empty)}</td>");
                    //sbuild.Append($"<td>{(!String.IsNullOrEmpty(_lead.resumen_metodologia) ? "<span class='badge badge-pill badge-info'>" + _lead.resumen_metodologia + "</span>" : string.Empty)} {(!String.IsNullOrEmpty(_lead.resumen_cuando) ? "<span class='badge badge-pill badge-warning'>" + _lead.resumen_cuando + "</span>" : string.Empty)} {(_lead.resumen_beca != null ? "<span class='badge badge-pill badge-success'>" + _lead.resumen_beca + "</span>" : string.Empty)} {(_lead.resumen_descuento != null ? "<span class='badge badge-pill badge-success'>" + _lead.resumen_descuento + "</span>" : string.Empty)} {(!String.IsNullOrEmpty(_lead.resumen_comentarios) ? "<span class='badge badge-pill badge-secondary'>" + _lead.resumen_comentarios + "</span>" : string.Empty)}</td>");

                    /// 3.8.- Estado
                    if (_tab == (int)Constantes.tab_position_AP.proceso || _tab == (int)Constantes.tab_position_AP.todos)
                    {
                        if (_lead.estado_lead == (int)Constantes.type_status_action.status_nuevo)
                            sbuild.Append($"<td><span class='badge badge-pill badge bg-primary text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Nuevo'>Nuevo</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_rechazado)
                            sbuild.Append($"<td><span class='badge badge-pill badge-purple' data-toggle='tooltip' data-placement='top' data-html='true' title='Rechazado'>Rechazado</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_duplicado)
                            sbuild.Append($"<td><span class='badge badge-pill badge-purple' data-toggle='tooltip' data-placement='top' data-html='true' title='Duplicado'>Duplicado</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_futuro)
                            sbuild.Append($"<td><span class='badge badge-pill badge-primary' data-toggle='tooltip' data-placement='top' data-html='true' title='Futuro'>Futuro</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_cerrar)
                            sbuild.Append($"<td><span class='badge badge-pill badge-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Cerrado'>Cerrado</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_sin_contactar)
                            sbuild.Append($"<td><span class='badge badge-pill badge-danger' data-toggle='tooltip' data-placement='top' data-html='true' title='Sin contactar'>Sin contactar</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_indeciso)
                            sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Indeciso'>Indeciso</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_interesado)
                            sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Interesado'>Interesado</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_send)
                            sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Envio contrato'>Envio contrato</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_receive)
                            sbuild.Append($"<td><span class='badge badge-pill badge-yellow text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Recibir contrato'>Recibir contrato</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_pago)
                            sbuild.Append($"<td><span class='badge badge-pill badge-yellow text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Pago'>Pago</span></td>");
                        else if (_lead.estado_lead == (int)Constantes.type_status_action.status_matriculado)
                            sbuild.Append($"<td><span class='badge badge-pill badge-success' data-toggle='tooltip' data-placement='top' data-html='true' title='Matriculado'>Matriculado</span></td>");
                        else
                            sbuild.Append("<td></td>");
                    }

                    /// 3.9.- Poner la fecha del último seguimiento
                    if (_tab != (int)Constantes.tab_position_AP.nuevos && _tab != (int)Constantes.tab_position_AP.avisos)
                        sbuild.Append($"<td>{_lead.ult_seg_fecha}</td>");

                    /// 3.10.- Poner el último comentario   
                    if (_tab != (int)Constantes.tab_position_AP.nuevos)
                        sbuild.Append($"<td>{_lead.ult_comentario}</td>");

                    /// 3.11.- Ir a ficha usuario
                    sbuild.Append($"<td><a href='ficha-alumno-crm.aspx?idu={_lead.idPersona}' target='_blank'><i class='fas fa-user fa-1-4x'></i></td>");

                    sbuild.Append("</tr>");
                }
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }

        #region WS Buscar leads

        [WebMethod(Description = "Buscar datos de las tablas de leads")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_table(int tab, int status, long comercial, bool aviso)
        {
            int _count_nuevos = 0;
            int _count_nuevos_all = 0;
            int _count_avisos = 0;
            List<string> lst = new List<string>();
            lst.Add(load_leads(tab, status, comercial, aviso, ref _count_nuevos, ref _count_nuevos_all, ref _count_avisos, false));
            return lst;
        }

        #endregion
    }
}