using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class informe_newsletter : System.Web.UI.Page
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
                    long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
                    if (id_newsletter > 1)
                        load_report(id_newsletter, list_user[0]);
                }
            }
        }

        private void load_report(long id_newsletter, CLIENTES user)
        {
            /// 1.- Pintar el título
            txt_informe_newsletter.InnerHtml = "<i class='fas fa-chart-line'></i> Informe de la campaña";

            /// 2.- Sacar los datos de la campaña
            List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
            if (lst_newsletter.Count == 1)
            {
                if (lst_newsletter[0].id_els == null)
                    Response.Redirect("lista-newsletter.aspx");
                else
                {
                    List<EMAIL_LISTADO_SUSCRIPCIONES> listado_suscripciones = da.getEmailListSubscriptionsById(lst_newsletter[0].id_els.Value);

                    /// 2.1.- Nombre de la campaña y asunto
                    nombre_newsletter.InnerHtml = "<strong>Nombre campaña:</strong> " + lst_newsletter[0].nombre + " (" + (lst_newsletter[0].estado == (int)Constantes.status_newsletter.Enviado ? "<span class='text-color-success'>" + Utilities.obtenerNombreEstadoNewsletter(lst_newsletter[0].estado) + "</span>" : Utilities.obtenerNombreEstadoNewsletter(lst_newsletter[0].estado)) + ") " + "[" + listado_suscripciones.Where(ls => ls.id_els == lst_newsletter[0].id_els).Select(ls => ls.nombre).FirstOrDefault() + "] [" + lst_newsletter[0].num_envios + " suscriptores]";
                    asunto_newsletter.InnerHtml = "<strong>Asunto:</strong> " + lst_newsletter[0].asunto;

                    /// 2.2.- Pintar los cuadros
                    StringBuilder sbuild = new StringBuilder();

                    /// 2.2.1.- OPEN RATE
                    sbuild.Append("<div class='col-sm-3'>");
                    sbuild.Append("<div class='course-box note margin-b-15'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center text-color-secondary bold'>" + lst_newsletter[0].pct_opens + "%</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("<hr class='separator margin-lr-15 margin-tb-5'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center bold'>OPEN RATE</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");

                    /// 2.2.2.- CLICKS
                    sbuild.Append("<div class='col-sm-3'>");
                    sbuild.Append("<div class='course-box note margin-b-15'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center text-color-secondary bold'>" + lst_newsletter[0].pct_clicks + "%</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("<hr class='separator margin-lr-15 margin-tb-5'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center bold'>CLICKS</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");

                    /// 2.2.3.- OPENS
                    sbuild.Append("<div class='col-sm-3'>");
                    sbuild.Append("<div class='course-box note margin-b-15'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center text-color-secondary bold'>" + lst_newsletter[0].num_opens + "</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("<hr class='separator margin-lr-15 margin-tb-5'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center bold'>OPENS</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");

                    /// 2.2.4.- UNSUBSCRIBED
                    sbuild.Append("<div class='col-sm-3'>");
                    sbuild.Append("<div class='course-box note margin-b-15'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center text-color-secondary bold'>" + lst_newsletter[0].pct_unsubscribe + "%</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("<hr class='separator margin-lr-15 margin-tb-5'>");
                    sbuild.Append("<div class='course-note'>");
                    sbuild.Append("<h3 class='course-title'><span class='h3 padding-t-5 text-center bold'>UNSUBSCRIBED</span></h3>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");
                    sbuild.Append("</div>");

                    block_informe_status.InnerHtml = sbuild.ToString();

                    /// 2.3.- Otros datos
                    sbuild = new StringBuilder();

                    /// 2.3.1.- Alcance 
                    sbuild.Append("<div class='col-sm-6'>");
                    sbuild.Append("<label>Alcance: <strong>" + lst_newsletter[0].num_envios + "</strong></label>");
                    sbuild.Append("</div>");

                    /// 2.3.2.- Bounced
                    sbuild.Append("<div class='col-sm-6'>");
                    sbuild.Append("<label>Bounced: <strong class='text-color-red'>" + lst_newsletter[0].num_bounced + " (" + lst_newsletter[0].pct_bounced + "%)</strong></label>");
                    sbuild.Append("</div>");

                    /// 2.3.3.- Successful deliveries
                    sbuild.Append("<div class='col-sm-6'>");
                    if (lst_newsletter[0].num_envios != null && lst_newsletter[0].num_envios.Value > 0 && lst_newsletter[0].num_bounced != null)
                    {
                        decimal pct_deliveries = ((decimal)(lst_newsletter[0].num_envios - lst_newsletter[0].num_bounced) / (decimal)(lst_newsletter[0].num_envios)) * 100;
                        sbuild.Append("<label>Successful deliveries: <strong>" + (lst_newsletter[0].num_envios - lst_newsletter[0].num_bounced) + " (" + Math.Round(pct_deliveries, 2) + "%)</strong></label>");
                    }
                    sbuild.Append("</div>");

                    /// 2.3.4.- Bajas
                    sbuild.Append("<div class='col-sm-6'>");
                    if (lst_newsletter[0].num_unsubscribe > 0)
                        sbuild.Append("<label>Bajas: <a href='#txt_bajas'><strong>" + lst_newsletter[0].num_unsubscribe + " (" + lst_newsletter[0].pct_unsubscribe + "%)</strong></a></label>");
                    else
                        sbuild.Append("<label>Bajas: <strong>" + lst_newsletter[0].num_unsubscribe + " (" + lst_newsletter[0].pct_unsubscribe + "%)</strong></label>");
                    sbuild.Append("</div>");

                    block_otros_datos_status.InnerHtml = sbuild.ToString();

                    /// 2.4.- fecha de envío
                    /*CultureInfo culture_info = new CultureInfo("Es-Es");
                    txt_envio_24.InnerHtml = "Envío realizado el " + culture_info.DateTimeFormat.GetDayName(lst_newsletter[0].fecha_scheduled.DayOfWeek) + " " + lst_newsletter[0].fecha_scheduled.Day.ToString("00") + " de " + Utilities.MonthName(lst_newsletter[0].fecha_scheduled.Month) + " de " + lst_newsletter[0].fecha_scheduled.Year;
                    txt_envio_week.InnerHtml = "Envío realizado el " + culture_info.DateTimeFormat.GetDayName(lst_newsletter[0].fecha_scheduled.DayOfWeek) + " " + lst_newsletter[0].fecha_scheduled.Day.ToString("00") + " de " + Utilities.MonthName(lst_newsletter[0].fecha_scheduled.Month) + " de " + lst_newsletter[0].fecha_scheduled.Year;*/
                    
                    /// 2.5.- Listado de bajas
                    List<EMAIL_SUSCRIPCIONES> lst_suscripciones = da.getEmailSubscriptionsById(lst_newsletter[0].id_els.Value);

                    /// 2.5.1.- Filtrar las suscripciones para sacar las bajas
                    List<EMAIL_SUSCRIPCIONES> lst_suscripciones_bajas = lst_suscripciones.Where(es => es.fecha_baja != null && es.fecha_baja >= lst_newsletter[0].fecha_scheduled).ToList();
                    if (lst_suscripciones_bajas.Count > 0)
                        table_listado_bajas.InnerHtml = paint_table(lst_suscripciones_bajas);

                    /// 2.6.- TOP LOCATIONS
                    List<EMAIL_CAMPAIGNS_STATUS> listado_status = da.getCampaignsStatusById(id_newsletter);
                    if (listado_status.Count > 0)
                    {
                        /// 2.6.0.- Poner las fechas de envío
                        CultureInfo culture_info = new CultureInfo("Es-Es");

                        txt_envio_24.InnerHtml = "Envío realizado el " + culture_info.DateTimeFormat.GetDayName(lst_newsletter[0].fecha_scheduled.DayOfWeek) + " " + lst_newsletter[0].fecha_scheduled.Day.ToString("00") + " de " + Utilities.MonthName(lst_newsletter[0].fecha_scheduled.Month) + " de " + lst_newsletter[0].fecha_scheduled.Year + " " + lst_newsletter[0].fecha_scheduled.ToShortTimeString();
                        txt_envio_week.InnerHtml = "Envío realizado el " + culture_info.DateTimeFormat.GetDayName(lst_newsletter[0].fecha_scheduled.DayOfWeek) + " " + lst_newsletter[0].fecha_scheduled.Day.ToString("00") + " de " + Utilities.MonthName(lst_newsletter[0].fecha_scheduled.Month) + " de " + lst_newsletter[0].fecha_scheduled.Year + " " + lst_newsletter[0].fecha_scheduled.ToShortTimeString();
                        List<EMAIL_CAMPAIGNS_STATUS> listado_status_reenvio = listado_status.Where(ecs => ecs.reenvio != null && ecs.reenvio.Value).ToList();
                        if (listado_status_reenvio.Count > 0)
                        {
                            List<DateTime> list_fechas = listado_status_reenvio.Select(ecs => DateTime.Parse(ecs.fecha_hora.ToShortDateString())).Distinct().ToList();
                            if (list_fechas.Count > 0)
                            {
                                int index = 2;
                                foreach (var _date in list_fechas)
                                {
                                    DateTime _fecha = listado_status_reenvio.Where(ecs => DateTime.Parse(ecs.fecha_hora.ToShortDateString()) == _date).Min(ecs => ecs.fecha_hora);
                                    txt_envio_24.InnerHtml += "<br />Envío " + index + " realizado el " + culture_info.DateTimeFormat.GetDayName(_fecha.DayOfWeek) + " " + _fecha.Day.ToString("00") + " de " + Utilities.MonthName(_fecha.Month) + " de " + _fecha.Year + " " + _fecha.ToShortTimeString();
                                    txt_envio_week.InnerHtml += "<br />Envío " + index + " realizado el " + culture_info.DateTimeFormat.GetDayName(_fecha.DayOfWeek) + " " + _fecha.Day.ToString("00") + " de " + Utilities.MonthName(_fecha.Month) + " de " + _fecha.Year + " " + _fecha.ToShortTimeString();
                                    index++;
                                }
                            }
                        }
                        
                        /// 2.6.1.- Sacar los clics
                        List<EMAIL_CAMPAIGNS_STATUS> lst_clicks = listado_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Clic).ToList();
                        if (lst_clicks.Count > 0)
                            table_listado_clics.InnerHtml = paint_table_status(lst_clicks, lst_suscripciones, (int)Constantes.status_mail.Clic);

                        /// 2.6.2.- Sacar los opens
                        List<EMAIL_CAMPAIGNS_STATUS> lst_opens = listado_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Open || ecs.status == (int)Constantes.status_mail.Clic).ToList();
                        if (lst_opens.Count > 0)
                            table_listado_opens.InnerHtml = paint_table_status(lst_opens, lst_suscripciones, (int)Constantes.status_mail.Open);

                        /// 2.6.3.- Sacar los bounced
                        List<EMAIL_CAMPAIGNS_STATUS> lst_bounced = listado_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Bounced).ToList();
                        if (lst_bounced.Count > 0)
                            table_listado_bounced.InnerHtml = paint_table_status(lst_bounced, lst_suscripciones, (int)Constantes.status_mail.Bounced);

                        /// 2.6.1.- Sacar los abiertos o clickeados
                        listado_status = listado_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Open || ecs.status == (int)Constantes.status_mail.Clic).ToList();

                        /// 2.6.2.- Sacar el país al que pertenece
                        List<long> lst_id_ec = listado_status.Select(ecs => ecs.id_ec).ToList();
                        List<campus_ACCIONES_PERSONA> lst_actions = da.getActionsByListEC(lst_id_ec);

                        /// 2.6.3.- Filtrar por los que no tienen paises
                        lst_actions = lst_actions.Where(ap => ap.idPais != null).ToList();

                        /// 2.6.4.-Sacar los países
                        List<int> lst_paises = new List<int>();
                        foreach (var action in lst_id_ec)
                        {
                            List<int?> lst_paises_actions = lst_actions.Where(ap => ap.idLanding == action).Select(ap => ap.idPais).Distinct().ToList();
                            if (lst_paises_actions.Count == 1)
                                lst_paises.Add(lst_paises_actions[0].Value);
                        }

                        if (lst_paises.Count > 0)
                            table_listado_locations.InnerHtml = paint_table_locations(lst_id_ec, lst_paises);
                    }
                }
            }
        }
        
        private string paint_table(List<EMAIL_SUSCRIPCIONES> lst_suscripciones)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Bajas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Mail</th>");
            sbuild.Append("<th>idAlumno</th>");
            sbuild.Append("<th>Fecha baja</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los listados de suscriptores
            foreach (var suscriptor in lst_suscripciones)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + suscriptor.nombre_completo + "</td>");
                sbuild.Append("<td>" + suscriptor.mail + "</td>");
                sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + suscriptor.id_alumno + "\" title=\"Datos del alumno\" target=\"_blank\">" + suscriptor.id_alumno + "</a></td>");
                sbuild.Append("<td>" + suscriptor.fecha_baja.Value.ToShortDateString() + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }

        private string paint_table_status(List<EMAIL_CAMPAIGNS_STATUS> lst_status, List<EMAIL_SUSCRIPCIONES> lst_suscripciones, int type_status)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            if (type_status == (int)Constantes.status_mail.Clic)
                sbuild.Append("<table id =\"tabla_Clics\" class=\"display compact\" style =\"width:100%\"><thead>");
            else if (type_status == (int)Constantes.status_mail.Bounced)
                sbuild.Append("<table id =\"tabla_Bounced\" class=\"display compact\" style =\"width:100%\"><thead>");
            else
                sbuild.Append("<table id =\"tabla_Opens\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Mail</th>");
            sbuild.Append("<th>idAlumno</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar el listado de clics
            foreach (var status in lst_status)
            {
                List<EMAIL_SUSCRIPCIONES> lst_suscriptor = lst_suscripciones.Where(es => es.id_s == status.id_s).ToList();
                if (lst_suscriptor.Count == 1)
                {
                    sbuild.Append("<tr>");
                    sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(lst_suscriptor[0].nombre_completo) + "</span>" + lst_suscriptor[0].nombre_completo + "</td>");
                    sbuild.Append("<td>" + lst_suscriptor[0].mail + "</td>");
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + lst_suscriptor[0].id_alumno + "\" title=\"Datos del alumno\" target=\"_blank\">" + lst_suscriptor[0].id_alumno + "</a></td>");
                    sbuild.Append("</tr>");
                }
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }
        
        private string paint_table_locations(List<long> lst_id_ec, List<int> lst_paises)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los paises
            List<Paises> lst_paises_all = da.getCountries();
            List<int> list = lst_paises.Distinct().ToList();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_locations\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Bandera</th>");
            sbuild.Append("<th>País</th>");
            sbuild.Append("<th>Nº</th>");
            sbuild.Append("<th>%</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");
            
            /// 3.- Pintar los listados de suscriptores
            foreach (var pais in list)
            {
                int num_paises = lst_paises.Where(p => p == pais).Count();
                decimal porcent_pais = ((decimal)num_paises / (decimal)lst_id_ec.Count) * 100;

                sbuild.Append("<tr>");
                sbuild.Append("<td><img alt='" + lst_paises_all.Where(p => p.id_pais == pais).Select(p => p.nombre).FirstOrDefault() + "' src='/App_Themes/support/img/flags/"+ lst_paises_all.Where(p => p.id_pais == pais).Select(p => p.descripcion).FirstOrDefault() + ".png' style='width: 32px;' /></td>");
                sbuild.Append("<td>" + lst_paises_all.Where(p => p.id_pais == pais).Select(p => p.nombre).FirstOrDefault() + "</td>");
                sbuild.Append("<td>"+ num_paises + "</td>");
                sbuild.Append("<td>" + Math.Round(porcent_pais, 2) + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }

        [WebMethod(Description = "Informe por horas del día")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Report> line24hours(long id_newsletter)
        {
            DataAccess da = new DataAccess();
            List<Report> list_data = new List<Report>();

            if (id_newsletter > 0)
            {
                /// 2.- Sacar los datos de la campaña
                List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                if (lst_newsletter.Count == 1)
                {
                    List<EMAIL_CAMPAIGNS_STATUS> listado_status = da.getCampaignsStatusById(id_newsletter);
                    if (listado_status.Count > 0)
                    {
                        /// 3.- Filtrar los que tienen hora de apertura
                        listado_status = listado_status.Where(ecs => ecs.fecha_open != null && ecs.status != (int)Constantes.status_mail.Open_Padre).ToList();
                        if (listado_status.Count > 0)
                        {
                            /// 4.- Sacar las horas que no son de reenvio
                            List<EMAIL_CAMPAIGNS_STATUS> listado_status_no_reenvio = listado_status.Where(ecs => ecs.reenvio == null || (ecs.reenvio != null && !ecs.reenvio.Value)).ToList();
                            if (listado_status_no_reenvio.Count > 0)
                            {
                                int hour_zero = 0;
                                for (var hour = hour_zero; hour < 24; hour++)
                                {
                                    /// 5.- Filtrar por horas
                                    List<EMAIL_CAMPAIGNS_STATUS> lst_status_filter = listado_status_no_reenvio.Where(ecs => ecs.fecha_open.Value.Hour >= hour && ecs.fecha_open.Value.Hour < (hour + 1)).ToList();

                                    Report informe = new Report();
                                    informe.hours = hour.ToString("00");
                                    informe.num_opens = lst_status_filter.Count;
                                    informe.envio = 1;
                                    list_data.Add(informe);
                                }
                            }

                            /// 5.- Sacar las horas de reenvio
                            List<EMAIL_CAMPAIGNS_STATUS> listado_status_reenvio = listado_status.Where(ecs => ecs.reenvio != null && ecs.reenvio.Value).ToList();
                            if (listado_status_reenvio.Count > 0)
                            {
                                List<DateTime> list_fechas = listado_status_reenvio.Select(ecs => DateTime.Parse(ecs.fecha_hora.ToShortDateString())).Distinct().ToList();
                                if (list_fechas.Count > 0)
                                {
                                    int index = 2;
                                    foreach (var fecha in list_fechas)
                                    {
                                        List<EMAIL_CAMPAIGNS_STATUS> listado_status_date = listado_status_reenvio.Where(ecs => DateTime.Parse(ecs.fecha_hora.ToShortDateString()) == fecha).ToList();
                                        if (listado_status_date.Count > 0)
                                        {
                                            int hour_zero = 0;
                                            for (var hour = hour_zero; hour < 24; hour++)
                                            {
                                                /// 5.- Filtrar por horas
                                                List<EMAIL_CAMPAIGNS_STATUS> lst_status_filter = listado_status_date.Where(ecs => ecs.fecha_open.Value.Hour >= hour && ecs.fecha_open.Value.Hour < (hour + 1)).ToList();

                                                Report informe = new Report();
                                                informe.hours = hour.ToString("00");
                                                informe.num_opens = lst_status_filter.Count;
                                                informe.envio = index;
                                                list_data.Add(informe);
                                            }
                                        }

                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list_data;
        }

        [WebMethod(Description = "Informe por días de la semana")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Report> line_week(long id_newsletter)
        {
            DataAccess da = new DataAccess();
            List<Report> list_data = new List<Report>();

            if (id_newsletter > 0)
            {
                /// 2.- Sacar los datos de la campaña
                List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                if (lst_newsletter.Count == 1)
                {
                    List<EMAIL_CAMPAIGNS_STATUS> listado_status = da.getCampaignsStatusById(id_newsletter);
                    if (listado_status.Count > 0)
                    {
                        /// 3.- Filtrar los que tienen hora de apertura
                        listado_status = listado_status.Where(ecs => ecs.fecha_open != null && ecs.status != (int)Constantes.status_mail.Open_Padre).ToList();
                        if (listado_status.Count > 0)
                        {
                            /// 4.- Sacar las horas que no son de reenvio
                            List<EMAIL_CAMPAIGNS_STATUS> listado_status_no_reenvio = listado_status.Where(ecs => ecs.reenvio == null || (ecs.reenvio != null && !ecs.reenvio.Value)).ToList();
                            if (listado_status_no_reenvio.Count > 0)
                            {
                                int day_zero = 1;
                                for (var day = day_zero; day < 8; day++)
                                {
                                    DayOfWeek _day = new DayOfWeek();
                                    if (day == 1)
                                        _day = DayOfWeek.Monday;
                                    else if (day == 2)
                                        _day = DayOfWeek.Tuesday;
                                    else if (day == 3)
                                        _day = DayOfWeek.Wednesday;
                                    else if (day == 4)
                                        _day = DayOfWeek.Thursday;
                                    else if (day == 5)
                                        _day = DayOfWeek.Friday;
                                    else if (day == 6)
                                        _day = DayOfWeek.Saturday;
                                    else if (day == 7)
                                        _day = DayOfWeek.Sunday;

                                    /// 5.- Filtrar por días
                                    List<EMAIL_CAMPAIGNS_STATUS> lst_status_filter = listado_status_no_reenvio.Where(ecs => ecs.fecha_open.Value.DayOfWeek == _day).ToList();

                                    Report informe = new Report();
                                    if (day == 1)
                                        informe.days = "Lunes";
                                    else if (day == 2)
                                        informe.days = "Martes";
                                    else if (day == 3)
                                        informe.days = "Miércoles";
                                    else if (day == 4)
                                        informe.days = "Jueves";
                                    else if (day == 5)
                                        informe.days = "Viernes";
                                    else if (day == 6)
                                        informe.days = "Sábado";
                                    else if (day == 7)
                                        informe.days = "Domingo";
                                    informe.num_opens = lst_status_filter.Count;
                                    informe.envio = 1;
                                    list_data.Add(informe);
                                }
                            }

                            /// 5.- Sacar las horas de reenvio
                            List<EMAIL_CAMPAIGNS_STATUS> listado_status_reenvio = listado_status.Where(ecs => ecs.reenvio != null && ecs.reenvio.Value).ToList();
                            if (listado_status_reenvio.Count > 0)
                            {
                                List<DateTime> list_fechas = listado_status_reenvio.Select(ecs => DateTime.Parse(ecs.fecha_hora.ToShortDateString())).Distinct().ToList();
                                if (list_fechas.Count > 0)
                                {
                                    int index = 2;
                                    foreach (var fecha in list_fechas)
                                    {
                                        List<EMAIL_CAMPAIGNS_STATUS> listado_status_date = listado_status_reenvio.Where(ecs => DateTime.Parse(ecs.fecha_hora.ToShortDateString()) == fecha).ToList();
                                        if (listado_status_date.Count > 0)
                                        {
                                            int day_zero = 1;
                                            for (var day = day_zero; day < 8; day++)
                                            {
                                                DayOfWeek _day = new DayOfWeek();
                                                if (day == 1)
                                                    _day = DayOfWeek.Monday;
                                                else if (day == 2)
                                                    _day = DayOfWeek.Tuesday;
                                                else if (day == 3)
                                                    _day = DayOfWeek.Wednesday;
                                                else if (day == 4)
                                                    _day = DayOfWeek.Thursday;
                                                else if (day == 5)
                                                    _day = DayOfWeek.Friday;
                                                else if (day == 6)
                                                    _day = DayOfWeek.Saturday;
                                                else if (day == 7)
                                                    _day = DayOfWeek.Sunday;

                                                /// 5.- Filtrar por días
                                                List<EMAIL_CAMPAIGNS_STATUS> lst_status_filter = listado_status_date.Where(ecs => ecs.fecha_open.Value.DayOfWeek == _day).ToList();

                                                Report informe = new Report();
                                                if (day == 1)
                                                    informe.days = "Lunes";
                                                else if (day == 2)
                                                    informe.days = "Martes";
                                                else if (day == 3)
                                                    informe.days = "Miércoles";
                                                else if (day == 4)
                                                    informe.days = "Jueves";
                                                else if (day == 5)
                                                    informe.days = "Viernes";
                                                else if (day == 6)
                                                    informe.days = "Sábado";
                                                else if (day == 7)
                                                    informe.days = "Domingo";
                                                informe.num_opens = lst_status_filter.Count;
                                                informe.envio = index;
                                                list_data.Add(informe);
                                            }
                                        }
                                        index++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list_data;
        }
    }
}
