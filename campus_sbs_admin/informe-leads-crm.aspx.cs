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
    public partial class informe_leads_crm : System.Web.UI.Page
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
                else
                {
                    int? day = null;
                    int? month = null;
                    int? year = null;

                    if (!String.IsNullOrEmpty(Request.QueryString["d"]))
                        day = int.Parse(Request.QueryString["d"]);
                    if (!String.IsNullOrEmpty(Request.QueryString["m"]))
                        month = int.Parse(Request.QueryString["m"]);
                    if (!String.IsNullOrEmpty(Request.QueryString["y"]))
                        year = int.Parse(Request.QueryString["y"]);

                    /// 1.- Cargar el listado de leads
                    load_leads(day, month, year, list_user[0]);
                }                
            }
        }

        private void load_leads(int? dia, int? mes, int? anyo, CLIENTES _user)
        {
            /// 0.- Sacar datos de la BBDD
            int _day = DateTime.Today.Day;
            int _month = DateTime.Today.Month;
            int _year = DateTime.Today.Year;

            if (dia != null)
                _day = dia.Value;
            if (mes != null)
                _month = mes.Value;
            if (anyo != null)
                _year = anyo.Value;

            DateTime fecha = new DateTime(_year, _month, _day);
            DateTime fecha_mes = new DateTime(_year, _month, 1);
            hid_month.Value = _month.ToString();
            hid_year.Value = _year.ToString();
            hid_day.Value = _day.ToString();
            hid_day_month.Value = _month.ToString();
            hid_day_year.Value = _year.ToString();

            /// Sacar las acciones
            List<long> _id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                _id_act.Add(long.Parse(act));
            }

            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByAction(_id_act, fecha_mes, fecha_mes.AddMonths(1));
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");

            /// 1.- Pintar el bloque del día

            /// 1.0.- Filtrar los leads que son recordatorios
            _actions = _actions.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.1.- Filtrar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _actions.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.2.- Filtrar los leads
            _actions = _actions.Except(_leads_incorrectos).ToList();
            
            /// 1.3.- Filtrar los leads por comercial
            if (_user.Comercial != null && _user.Comercial.Value)
                _actions = _actions.Where(_ => _.idComercial == _user.id_cliente).ToList();
            else
                _actions = _actions.Where(_ => _.idComercial == _user.id_cliente || _.idComercial == null).ToList();

            /// 1.4.- Filtrar los origenes no visibles para informes
            List<long> _ids_origins = _origins.Where(_ => _.No_Visible_Informes != null && _.No_Visible_Informes.Value).Select(_ => _.ID_Aux).Distinct().ToList();
            _actions = _actions.Where(_ => _.IdOrigen != null && !_ids_origins.Contains(_.IdOrigen.Value)).ToList();

            /// 1.5.- Filtrar las acciones
            List <campus_ACCIONES_PERSONA> _actions_day = _actions.Where(_ => _.Fecha >= fecha && _.Fecha < fecha.AddDays(1)).ToList();
                        
            /// 1.6.- Pintar tabla de leads
            table_listado_leads_day.InnerHtml = paint_table(_actions_day, _origins, true);

            /// 1.7.- Pintar los botones
            lnk_day.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_day_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_day_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + fecha.Day + " " + Utilities.MonthName(fecha.Month) + " " + fecha.Year + "</span>";

            /// 2.- Pintar el bloque del mes

            /// 2.1.- Pintar tabla de leads
            table_listado_leads_month.InnerHtml = paint_table(_actions, _origins, false);

            /// 2.2.- Pintar los botones
            lnk_month.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + Utilities.MonthName(fecha_mes.Month) + " " + fecha_mes.Year + "</span>";
        }        

        private static string paint_table(List<campus_ACCIONES_PERSONA> _actions, List<campus_AUX> _origins, bool _day)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los origenes distintos y acciones
            List<long> lst_id_origins = _actions.Where(_ => _.IdOrigen != null).Select(_ => _.IdOrigen.Value).Distinct().ToList();
            List<long> lst_id_ap = _actions.Select(_ => _.idAccionPersona).ToList();

            /// 1.- Inicio tabla
            if (_day)
                sbuild.Append("<table id =\"tabla_List_Leads\" class=\"display compact\" style =\"width:100%\"><thead>");
            else
                sbuild.Append("<table id =\"tabla_List_Leads_Month\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>N. Leads</th>");
            sbuild.Append("<th>N. Procesados</th>");
            sbuild.Append("<th>N. Sin tratar</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Pintar los leads por origen
            foreach (var _origen in lst_id_origins)
            {
                /// 3.1.- Filtrar las acciones por origen
                List<campus_ACCIONES_PERSONA> _actions_filter = _actions.Where(_ => _.IdOrigen != null && _.IdOrigen == _origen).ToList();
                if (_actions_filter.Count > 0)
                {
                    sbuild.Append("<tr>");
                    sbuild.Append("<td><span class='hidden'>" + _origen + "</span></td>");
                    sbuild.Append("<td>" + _origins.Where(_ => _.ID_Aux == _origen).Select(_ => _.Nombre).FirstOrDefault() + "</td>");
                    sbuild.Append("<td>" + _actions_filter.Count + "</td>");
                    sbuild.Append("<td>" + _actions_filter.Where(act => act.Procesado != null && act.Procesado.Value).Count() + "</td>");

                    int sin_procesar = _actions_filter.Where(act => act.Procesado == null || !act.Procesado.Value).Count();
                    if (sin_procesar > 0)
                        sbuild.Append("<td class='text-color-red'>" + sin_procesar + "</td>");
                    else
                        sbuild.Append("<td>" + sin_procesar + "</td>");
                    sbuild.Append("</tr>");
                }
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
        
        [WebMethod(Description = "Recupera la tabla día")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_table_day(int day, int month, int year)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// Sacar al usuario
            List<CLIENTES> _user = new List<CLIENTES>();
            if (HttpContext.Current.Session["usuario"] != null)
                _user.Add((CLIENTES)HttpContext.Current.Session["usuario"]);

            /// 0.- Sacar las acciones
            List<long> _id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                _id_act.Add(long.Parse(act));
            }

            /// 1.- Pintar el bloque del día
            DateTime fecha = new DateTime(year, month, day);
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByAction(_id_act, fecha, fecha.AddDays(1));
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");
            
            /// 1.0.- Filtrar los leads que son recordatorios
            _actions = _actions.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.1.- Filtrar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _actions.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.2.- Filtrar los leads
            _actions = _actions.Except(_leads_incorrectos).ToList();

            /// 1.3.- Filtrar los leads por comercial
            if (_user.Count == 1)
            {
                if (_user[0].Comercial != null && _user[0].Comercial.Value)
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente).ToList();
                else
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente || _.idComercial == null).ToList();
            }

            /// 1.4.- Filtrar los origenes no visibles para informes
            List<long> _ids_origins = _origins.Where(_ => _.No_Visible_Informes != null && _.No_Visible_Informes.Value).Select(_ => _.ID_Aux).Distinct().ToList();
            _actions = _actions.Where(_ => _.IdOrigen != null && !_ids_origins.Contains(_.IdOrigen.Value)).ToList();

            /// 2.- Recuperar la tabla del día
            list.Add(paint_table(_actions, _origins, true));

            return list;
        }

        [WebMethod(Description = "Recupera la tabla mes")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_table_month(int month, int year)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// Sacar al usuario
            List<CLIENTES> _user = new List<CLIENTES>();
            if (HttpContext.Current.Session["usuario"] != null)
                _user.Add((CLIENTES)HttpContext.Current.Session["usuario"]);

            /// 0.- Sacar las acciones
            List<long> _id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                _id_act.Add(long.Parse(act));
            }

            /// 1.- Pintar el bloque del día
            DateTime fecha = new DateTime(year, month, 1);
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByAction(_id_act, fecha, fecha.AddMonths(1));
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");

            /// 1.0.- Filtrar los leads que son recordatorios
            _actions = _actions.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.1.- Filtrar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _actions.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.2.- Filtrar los leads
            _actions = _actions.Except(_leads_incorrectos).ToList();

            /// 1.3.- Filtrar los leads por comercial
            if (_user.Count == 1)
            {
                if (_user[0].Comercial != null && _user[0].Comercial.Value)
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente).ToList();
                else
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente || _.idComercial == null).ToList();
            }

            /// 1.4.- Filtrar los origenes no visibles para informes
            List<long> _ids_origins = _origins.Where(_ => _.No_Visible_Informes != null && _.No_Visible_Informes.Value).Select(_ => _.ID_Aux).Distinct().ToList();
            _actions = _actions.Where(_ => _.IdOrigen != null && !_ids_origins.Contains(_.IdOrigen.Value)).ToList();

            /// 2.- Recuperar la tabla del mes
            list.Add(paint_table(_actions, _origins, false));

            return list;
        }

        [WebMethod(Description = "Recupera la tabla día y origen")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable(long id, int day, int month, int year)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// Sacar al usuario
            List<CLIENTES> _user = new List<CLIENTES>();
            if (HttpContext.Current.Session["usuario"] != null)
                _user.Add((CLIENTES)HttpContext.Current.Session["usuario"]);

            /// 0.- Sacar las acciones
            List<long> _id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                _id_act.Add(long.Parse(act));
            }

            /// 1.- Sacar datos de la BBDD            
            DateTime fecha = new DateTime(year, month, day);
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByAction(_id_act, fecha, fecha.AddDays(1));
            _actions = _actions.Where(_ => _.IdOrigen == id).ToList();

            /// 1.0.- Filtrar los leads que son recordatorios
            _actions = _actions.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.1.- Filtrar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _actions.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.2.- Filtrar los leads
            _actions = _actions.Except(_leads_incorrectos).ToList();

            /// 1.3.- Filtrar los leads por comercial
            if (_user.Count == 1)
            {
                if (_user[0].Comercial != null && _user[0].Comercial.Value)
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente).ToList();
                else
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente || _.idComercial == null).ToList();
            }

            /// 2.- Sacar el resto de datos de la BBDD
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");
            List<long> _id_users = _actions.Select(_ => _.idPersona).Distinct().ToList();
            List<CLIENTES> _users = new List<CLIENTES>();
            if (_id_users.Count > 0)
                _users = da.getUserByList(_id_users);
            List<campus_CURSO> _courses = da.getCourses(null);
            List<long> _ids_ap = _actions.Select(_ => _.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> _seguimientos = new List<campus_SEG_COMERCIAL>();
            if (_ids_ap.Count > 0)
                _seguimientos = da.getSeguimientoComercial(_ids_ap);

            /// 3.- Recuperar la tabla del mes
            list.Add(paint_subtable(_actions, _seguimientos, _origins, _users, _courses, true));

            return list;
        }

        [WebMethod(Description = "Recupera la tabla mes y origen")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable_month(long id, int month, int year)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// Sacar al usuario
            List<CLIENTES> _user = new List<CLIENTES>();
            if (HttpContext.Current.Session["usuario"] != null)
                _user.Add((CLIENTES)HttpContext.Current.Session["usuario"]);

            /// 0.- Sacar las acciones
            List<long> _id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                _id_act.Add(long.Parse(act));
            }

            /// 1.- Sacar datos de la BBDD            
            DateTime fecha = new DateTime(year, month, 1);
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByAction(_id_act, fecha, fecha.AddMonths(1));
            _actions = _actions.Where(_ => _.IdOrigen == id).ToList();

            /// 1.0.- Filtrar los leads que son recordatorios
            _actions = _actions.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.1.- Filtrar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _actions.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.2.- Filtrar los leads
            _actions = _actions.Except(_leads_incorrectos).ToList();

            /// 1.3.- Filtrar los leads por comercial
            if (_user.Count == 1)
            {
                if (_user[0].Comercial != null && _user[0].Comercial.Value)
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente).ToList();
                else
                    _actions = _actions.Where(_ => _.idComercial == _user[0].id_cliente || _.idComercial == null).ToList();
            }

            /// 2.- Sacar el resto de datos de la BBDD
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");
            List<long> _id_users = _actions.Select(_ => _.idPersona).Distinct().ToList();
            List<CLIENTES> _users = new List<CLIENTES>();
            if (_id_users.Count > 0)
                _users = da.getUserByList(_id_users);
            List<campus_CURSO> _courses = da.getCourses(null);
            List<long> _ids_ap = _actions.Select(_ => _.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> _seguimientos = new List<campus_SEG_COMERCIAL>();
            if (_ids_ap.Count > 0)
                _seguimientos = da.getSeguimientoComercial(_ids_ap);

            /// 3.- Recuperar la tabla del mes
            list.Add(paint_subtable(_actions, _seguimientos, _origins, _users, _courses, false));

            return list;
        }

        private static string paint_subtable(List<campus_ACCIONES_PERSONA> _actions, List<campus_SEG_COMERCIAL> _seguimientos, List<campus_AUX> _origins, List<CLIENTES> _users, List<campus_CURSO> _courses, bool _day)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            if (_day)
                sbuild.Append("<table id =\"tabla_List_Leads_Origin\" class=\"display compact\" style =\"width:100%\"><thead>");
            else
                sbuild.Append("<table id =\"tabla_List_Leads_Month_Origin\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha Lead</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Estado</th>");
            sbuild.Append("<th>N. Seguimientos</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los leads
            foreach (var _action in _actions)
            {
                sbuild.Append("<tr>");
                sbuild.Append($"<td>{_action.Fecha}</td>");
                sbuild.Append($"<td>{_users.Where(_ => _.id_cliente == _action.idPersona).Select(_ => _.Nombre_Completo).FirstOrDefault()}</td>");
                sbuild.Append($"<td>{_courses.Where(_ => _.ID_Curso == _action.IdCurso).Select(_ => _.Nombre).FirstOrDefault()}</td>");

                if (_action.estado_lead == (int)Constantes.type_status_action.status_sin_contactar)
                    sbuild.Append("<td class='text-color-red'>Sin Contactar</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_cerrar)
                    sbuild.Append("<td>Cerrado</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_duplicado)
                    sbuild.Append("<td>Duplicado</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_futuro)
                    sbuild.Append("<td>Futuro</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_indeciso)
                    sbuild.Append("<td>Indeciso</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_interesado)
                    sbuild.Append("<td>Interesado</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_matriculado)
                    sbuild.Append("<td>Matriculado</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_nuevo)
                    sbuild.Append("<td>Nuevo</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_pago)
                    sbuild.Append("<td>Pago</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_receive)
                    sbuild.Append("<td>Recibir contrato</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_rechazado)
                    sbuild.Append("<td>Rechazado</td>");
                else if (_action.estado_lead == (int)Constantes.type_status_action.status_send)
                    sbuild.Append("<td>Enviar Contrato</td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append($"<td>{_seguimientos.Where(_ => _.idAccionPersona == _action.idAccionPersona).Count()}</td>");
                sbuild.Append("<td><a href='/ficha-alumno-crm.aspx?idu=" + _action.idPersona + "' target='_blank'><i class='fas fa-user'></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}