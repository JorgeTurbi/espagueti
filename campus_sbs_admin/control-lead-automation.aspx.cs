using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class control_lead_automation : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Sacar los datos del usuario 
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
                bool comprobate_user = Utilities.comprobate_user(list_user[0]);
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
                    load_leads(day, month, year);
                }
            }
        }

        private void load_leads(int? dia, int? mes, int? anyo)
        {
            /// 0.- Sacar datos de la BBDD
            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            if (dia != null)
                day = dia.Value;
            if (mes != null)
                month = mes.Value;
            if (anyo != null)
                year = anyo.Value;

            DateTime fecha = new DateTime(year, month, day);
            DateTime fecha_mes = new DateTime(year, month, 1);
            hid_month.Value = month.ToString();
            hid_year.Value = year.ToString();
            hid_day.Value = day.ToString();
            hid_day_month.Value = month.ToString();
            hid_day_year.Value = year.ToString();

            List<campus_ACCIONES_PERSONA> lst_actions = da.getActionsByAction((long)Constantes.accion.Peticion_informacion, fecha_mes, fecha_mes.AddMonths(1));
            List<long> lst_id = lst_actions.Select(ap => ap.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> lst_seguimientos = da.getSeguimientoComercial(lst_id);
            List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");            
            
            /// 1.- Pintar el bloque del día

            /// 1.1.- Pintar título
            txt_control_leads.InnerHtml = "<i class='fas fa-chart-line'></i> Control de leads por día";

            /// 1.2.- Filtrar las acciones
            List<campus_ACCIONES_PERSONA> lst_actions_day = lst_actions.Where(act => act.Fecha >= fecha && act.Fecha < fecha.AddDays(1)).ToList();

            /// 1.3.- Pintar tabla de leads
            table_listado_leads_day.InnerHtml = paint_table_day(lst_actions_day, lst_seguimientos, lst_origins);

            /// 1.4.- Pintar los botones
            lnk_day.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_day_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_day_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + fecha.Day + " " + Utilities.MonthName(fecha.Month) + " " + fecha.Year + "</span>";

            /// 2.- Pintar el bloque del mes

            /// 2.1.- Pintar título
            txt_control_leads_month.InnerHtml = "<i class='fas fa-chart-line'></i> Control de leads por mes";

            /// 2.2.- Pintar tabla de leads
            table_listado_leads_month.InnerHtml = paint_table_month(lst_actions, lst_seguimientos, lst_origins);

            /// 2.3.- Pintar los botones
            lnk_month.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + Utilities.MonthName(fecha_mes.Month) + " " + fecha_mes.Year + "</span>";
        }

        private static string paint_table_day(List<campus_ACCIONES_PERSONA> lst_actions, List<campus_SEG_COMERCIAL> lst_seguimientos, List<campus_AUX> lst_origins)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los origenes distintos y acciones
            List<long> lst_id_origins = lst_actions.Where(act => act.IdOrigen != null).Select(act => act.IdOrigen.Value).Distinct().ToList();
            List<long> lst_id_ap = lst_actions.Select(ap => ap.idAccionPersona).ToList();

            /// 0.1.- Filtrar los seguimientos comerciales
            List<campus_SEG_COMERCIAL> lst_seguimientos_filter = lst_seguimientos.Where(_ => lst_id_ap.Contains(_.idAccionPersona)).ToList();
                        
            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Leads\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>N. Leads</th>");
            sbuild.Append("<th>N. Procesados</th>");
            sbuild.Append("<th>N. Sin tratar</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los leads por origen
            foreach (var origen in lst_id_origins)
            {
                /// 3.1.- Filtrar las acciones por origen
                List<campus_ACCIONES_PERSONA> lst_actions_filter = lst_actions.Where(act => act.IdOrigen != null && act.IdOrigen == origen).ToList();
                if (lst_actions_filter.Count > 0)
                {
                    /// 3.2.- Filtrar los seguimientos
                    List<long> lst_id_ap_origin = lst_actions_filter.Select(ap => ap.idAccionPersona).ToList();
                    List<campus_SEG_COMERCIAL> lst_seguimientos_filter_origin = lst_seguimientos_filter.Where(_ => lst_id_ap_origin.Contains(_.idAccionPersona)).ToList();
                    
                    sbuild.Append("<tr>");
                    sbuild.Append("<td><span class='hidden'>" + origen + "</span></td>");
                    sbuild.Append("<td>" + lst_origins.Where(o => o.ID_Aux == origen).Select(o => o.Nombre).FirstOrDefault() + "</td>");
                    sbuild.Append("<td>" + lst_actions_filter.Count + "</td>");
                    sbuild.Append("<td>" + lst_actions_filter.Where(act => act.Procesado != null && act.Procesado.Value).Count() + "</td>");

                    int sin_procesar = lst_actions_filter.Where(act => act.Procesado == null || !act.Procesado.Value).Count() + getSinProcesar(lst_seguimientos_filter_origin);
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
        private static string paint_table_month(List<campus_ACCIONES_PERSONA> lst_actions, List<campus_SEG_COMERCIAL> lst_seguimientos, List<campus_AUX> lst_origins)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los origenes distintos
            List<long> lst_id_origins = lst_actions.Where(act => act.IdOrigen != null).Select(act => act.IdOrigen.Value).Distinct().ToList();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Leads_Month\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>N. Leads</th>");
            sbuild.Append("<th>N. Sin procesar</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los leads por origen
            foreach (var origen in lst_id_origins)
            {
                /// 3.1.- Filtrar las acciones por origen
                List<campus_ACCIONES_PERSONA> lst_actions_filter = lst_actions.Where(act => act.IdOrigen != null && act.IdOrigen == origen).ToList();
                List<long> lst_id_ap = lst_actions_filter.Select(ap => ap.idAccionPersona).ToList();

                /// 3.2.- Filtrar los seguimientos comerciales
                List<campus_SEG_COMERCIAL> lst_seguimientos_filter = lst_seguimientos.Where(_ => lst_id_ap.Contains(_.idAccionPersona)).ToList();
                int sin_procesar = lst_actions_filter.Where(act => act.Procesado == null || !act.Procesado.Value).Count() + getSinProcesar(lst_seguimientos_filter);

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + lst_origins.Where(o => o.ID_Aux == origen).Select(o => o.Nombre).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + lst_actions_filter.Count + "</td>");
                sbuild.Append("<td>" + sin_procesar + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }        
        private static int getSinProcesar(List<campus_SEG_COMERCIAL> lst_seguimientos)
        {
            int sin_procesar = 0;

            /// 1.- Sacar los usuarios de los segimientos
            List<long> lst_id_person = lst_seguimientos.Select(seg => seg.idAlumno).Distinct().ToList();
            if (lst_id_person.Count > 0)
            {
                foreach (var alumno in lst_id_person)
                {
                    /// 2.- Filtrar los seguimientos por alumno
                    List<campus_SEG_COMERCIAL> lst_seguimientos_filter = lst_seguimientos.Where(_ => _.idAlumno == alumno).ToList();
                    if (lst_seguimientos_filter.Count > 0)
                    {
                        /// 3.- Filtrar las ap
                        List<long> lst_id_ap = lst_seguimientos_filter.Select(_ => _.idAccionPersona).Distinct().ToList();
                        if (lst_id_ap.Count > 0)
                        {
                            /// 4.- Recorrer los seguimientos
                            foreach (long id_ap in lst_id_ap)
                            {
                                /// 2.- Filtrar los seguimientos por alumno
                                List<campus_SEG_COMERCIAL> lst_seg_filter = lst_seguimientos_filter.Where(seg => seg.idAccionPersona == id_ap).ToList();
                                if (lst_seg_filter.Count == 1)
                                {
                                    if (lst_seg_filter[0].accion == (int)Constantes.type_action_canal.action_advise && lst_seg_filter[0].Comentarios.ToUpper().Trim().Equals("NUEVO"))
                                        sin_procesar++;
                                }
                            }
                        }
                    }
                }
            }

            return sin_procesar;
        }
        
        [WebMethod(Description = "Busca los eventos de un alumno")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Report> arrayLine(int? month, int? year)
        {
            DataAccess da = new DataAccess();
            List<Report> list_data = new List<Report>();

            /// 1.- Definir la fecha
            if (month == null)
                month = DateTime.Today.Month;
            if (year == null)
                year = DateTime.Today.Year;
            DateTime fecha = new DateTime(year.Value, month.Value, 1);

            /// 2.- Sacar las acciones del mes
            List<campus_ACCIONES_PERSONA> lst_actions = da.getActionsByAction((long)Constantes.accion.Peticion_informacion, fecha, fecha.AddMonths(1));
            List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");

            if (lst_actions.Count > 0)
            {
                /// 3.- Sacar los origenes distintos
                List<long> lst_id_origins = lst_actions.Where(act => act.IdOrigen != null).Select(act => act.IdOrigen.Value).Distinct().ToList();

                /// 4.- Pintar los leads por origen
                foreach (var origen in lst_id_origins)
                {
                    /// 4.1.- Filtrar las acciones por origen
                    List<campus_ACCIONES_PERSONA> lst_actions_filter = lst_actions.Where(act => act.IdOrigen != null && act.IdOrigen == origen).ToList();
                    if (lst_actions_filter.Count > 0)
                    {
                        int days_month = DateTime.DaysInMonth(year.Value, month.Value);
                        for (int _day = 1; _day <= days_month; _day++)
                        {
                            int num_actions = lst_actions_filter.Where(ap => ap.Fecha.Day == _day).Count();

                            Report informe = new Report();
                            informe.days = lst_origins.Where(o => o.ID_Aux == origen).Select(o => o.Nombre).FirstOrDefault();
                            informe.num_opens = num_actions;
                            informe.envio = _day;
                            list_data.Add(informe);
                        }
                    }
                }
            }

            return list_data;
        }

        [WebMethod(Description = "Recupera la tabla mes")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_table_month(int month, int year)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// 0.- Sacar datos de la BBDD            
            DateTime fecha = new DateTime(year, month, 1);

            List<campus_ACCIONES_PERSONA> lst_actions = da.getActionsByAction((long)Constantes.accion.Peticion_informacion, fecha, fecha.AddMonths(1));
            List<long> lst_id = lst_actions.Select(ap => ap.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> lst_seguimientos = da.getSeguimientoComercial(lst_id);
            List<CLIENTES> lst_users = da.getActiveUsers(true);
            List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");

            /// 1.- Recuperar la tabla del mes
            list.Add(paint_table_month(lst_actions, lst_seguimientos, lst_origins));

            return list;
        }

        [WebMethod(Description = "Recupera la tabla día")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_table_day(int day, int month, int year)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// 0.- Sacar datos de la BBDD            
            DateTime fecha = new DateTime(year, month, day);
            
            List<campus_ACCIONES_PERSONA> lst_actions = da.getActionsByAction((long)Constantes.accion.Peticion_informacion, fecha, fecha.AddDays(1));
            List<long> lst_id = lst_actions.Select(ap => ap.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> lst_seguimientos = da.getSeguimientoComercial(lst_id);
            List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");

            /// 1.- Recuperar la tabla del mes
            list.Add(paint_table_day(lst_actions, lst_seguimientos, lst_origins));
            
            return list;
        }

        [WebMethod(Description = "Recupera la tabla día y origen")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable(long id, int day, int month, int year)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// 0.- Sacar datos de la BBDD            
            DateTime fecha = new DateTime(year, month, day);
            List<campus_ACCIONES_PERSONA> lst_actions = da.getActionsByAction((long)Constantes.accion.Peticion_informacion, fecha, fecha.AddDays(1));
            lst_actions = lst_actions.Where(act => act.IdOrigen == id).ToList();
            List<long> lst_id = lst_actions.Select(ap => ap.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> lst_seguimientos = da.getSeguimientoComercial(lst_id);
            List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");
            List<CLIENTES> lst_users = da.getActiveUsers(true);
            List<campus_CURSO> lst_courses = da.getCourses(null);

            /// 1.- Recuperar la tabla del mes
            list.Add(paint_subtable_day(lst_actions, lst_seguimientos, lst_origins, lst_users, lst_courses));

            return list;
        }

        private static string paint_subtable_day(List<campus_ACCIONES_PERSONA> lst_actions, List<campus_SEG_COMERCIAL> lst_seguimientos, List<campus_AUX> lst_origins, List<CLIENTES> lst_users, List<campus_CURSO> lst_courses)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los origenes distintos y acciones
            List<long> lst_id_origins = lst_actions.Where(act => act.IdOrigen != null).Select(act => act.IdOrigen.Value).Distinct().ToList();
            List<long> lst_id_ap = lst_actions.Select(ap => ap.idAccionPersona).ToList();

            /// 0.1.- Filtrar los seguimientos comerciales
            List<campus_SEG_COMERCIAL> lst_seguimientos_filter = lst_seguimientos.Where(_ => lst_id_ap.Contains(_.idAccionPersona)).ToList();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Leads_Origin\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha Lead</th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Estado</th>");
            sbuild.Append("<th>N. Seguimientos</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los leads
            foreach (var action in lst_actions)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + action.Fecha + "</td>");
                long id_comercial = lst_seguimientos_filter.Where(_ => _.idAccionPersona == action.idAccionPersona).Select(seg => seg.idComercial).FirstOrDefault();
                sbuild.Append("<td>" + lst_users.Where(c => c.id_cliente == id_comercial).Select(c => c.Nombre_Completo).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + lst_courses.Where(c => c.ID_Curso == action.IdCurso).Select(c => c.Nombre).FirstOrDefault() + "</td>");

                int? status = lst_seguimientos_filter.Where(_ => _.idAccionPersona == action.idAccionPersona).Select(seg => seg.estado).FirstOrDefault();
                if (status == (int)Constantes.type_status_action.status_sin_contactar)
                    sbuild.Append("<td class='text-color-red'>Sin Contactar</td>");
                else if (status == (int)Constantes.type_status_action.status_cerrar)
                    sbuild.Append("<td>Cerrado</td>");
                else if (status == (int)Constantes.type_status_action.status_duplicado)
                    sbuild.Append("<td>Duplicado</td>");
                else if (status == (int)Constantes.type_status_action.status_futuro)
                    sbuild.Append("<td>Futuro</td>");
                else if (status == (int)Constantes.type_status_action.status_indeciso)
                    sbuild.Append("<td>Indeciso</td>");
                else if (status == (int)Constantes.type_status_action.status_interesado)
                    sbuild.Append("<td>Interesado</td>");
                else if (status == (int)Constantes.type_status_action.status_matriculado)
                    sbuild.Append("<td>Matriculado</td>");
                else if (status == (int)Constantes.type_status_action.status_nuevo)
                    sbuild.Append("<td>Nuevo</td>");
                else if (status == (int)Constantes.type_status_action.status_pago)
                    sbuild.Append("<td>Pago</td>");
                else if (status == (int)Constantes.type_status_action.status_receive)
                    sbuild.Append("<td>Recibir contrato</td>");
                else if (status == (int)Constantes.type_status_action.status_rechazado)
                    sbuild.Append("<td>Rechazado</td>");
                else if (status == (int)Constantes.type_status_action.status_send)
                    sbuild.Append("<td>Enviar Contrato</td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append("<td>" + lst_seguimientos_filter.Where(_ => _.idAccionPersona == action.idAccionPersona).Count() + "</td>");
                sbuild.Append("<td><a href='ficha-alumno-crm.aspx?idu=" + action.idPersona + "' target='_blank'><i class='fas fa-user'></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}