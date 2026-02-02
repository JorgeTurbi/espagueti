using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class informe_servidor : System.Web.UI.Page
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
                    load_report(list_user[0]);
            }
        }

        protected void btnActivarServidor_Click(object sender, ImageClickEventArgs e)
        {
            bool active_server = false;

            try
            {
                long id_server = !String.IsNullOrEmpty(hidIdServer.Value) ? long.Parse(hidIdServer.Value) : -1;
                if (id_server > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_SERVER> lst = da.getEmailServerById(id_server);
                    if (lst.Count == 1)
                    {
                        bool update_server = da.updateEmailServer(lst[0], true);
                        if (update_server)
                            active_server = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - informe-servidor.cs::btnActivarServidor_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (active_server)
                Response.Redirect("informe-servidor.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar el servidor');</script>");
        }

        protected void btnDesactivarServidor_Click(object sender, ImageClickEventArgs e)
        {
            bool active_server = false;

            try
            {
                long id_server = !String.IsNullOrEmpty(hidIdServer.Value) ? long.Parse(hidIdServer.Value) : -1;
                if (id_server > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_SERVER> lst = da.getEmailServerById(id_server);
                    if (lst.Count == 1)
                    {
                        bool update_server = da.updateEmailServer(lst[0], false);
                        if (update_server)
                            active_server = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - informe-servidor.cs::btnDesactivarServidor_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (active_server)
                Response.Redirect("informe-servidor.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al desactivar el servidor');</script>");
        }

        private void load_report(CLIENTES user)
        {
            /// 1.- Comprobar si es un usuario de envío de mails especiales
            long id_user_special = -1;
            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();

            if (list_users_mails.Contains(user.id_cliente.ToString()))
                id_user_special = user.id_cliente;

            /// 2.- Sacar datos de la BBDD
            List<EMAIL_SERVER> lst_servers = da.getEmailServers(null);
            List<EMAIL_SERVER_STATUS> lst_servers_status = da.getEmailServerStatus();
            List<EMAIL_SERVER_ESTADISTICAS> lst_server_statistics = da.getEmailServerEstadisticasByUser(id_user_special);

            /// 3.- Pintar la tabla de servidores activos
            table_listado_servidores_activos.InnerHtml = paint_table(lst_servers, lst_servers_status, lst_server_statistics, id_user_special);

            /// 4.- Pintar la tabla de histórico de envíos
            table_listado_historico_envios.InnerHtml = paint_table_sends(lst_server_statistics);

            /// 5.- Pintar el título
            txt_informe_servidor.InnerHtml = "<i class='fas fa-chart-bar'></i> Informe de los servidores de envío";
        }

        private string paint_table(List<EMAIL_SERVER> lst_servers, List<EMAIL_SERVER_STATUS> lst_servers_status, List<EMAIL_SERVER_ESTADISTICAS> lst_server_statistics, long id_user_special)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los servidores visibles
            lst_servers = lst_servers.Where(serv => serv.visible).ToList();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Servidores\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Identificador</th>");
            sbuild.Append("<th>Hoy</th>");
            sbuild.Append("<th>Ayer</th>");
            sbuild.Append("<th>Semana</th>");
            sbuild.Append("<th>Mes</th>");
            sbuild.Append("<th>Mes anterior</th>");
            sbuild.Append("<th>Año</th>");
            sbuild.Append("<th>Prioritario</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead>");
            sbuild.Append("<tfoot><tr><th>Total</th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr></tfoot>");
            sbuild.Append("<tbody>");
            
            /// 3.- Pintar las campañas
            foreach (var server in lst_servers)
            {
                /// 3.1.- Sacar el estado del servidor
                List<EMAIL_SERVER_STATUS> lst_status = lst_servers_status.Where(ss => ss.id_es == server.id_es).ToList();
                if (lst_status.Count > 0)
                {
                    /// 3.2.- Filtrar los datos estadísticos del servidor
                    List<EMAIL_SERVER_ESTADISTICAS> lst_estadisticas = lst_server_statistics.Where(ss => ss.id_es == server.id_es).ToList();

                    if (server.activo)
                    {
                        if (lst_status[0].bloqueado)
                            sbuild.Append("<tr class='text-color-orange'>");
                        else
                            sbuild.Append("<tr>");
                    }
                    else
                        sbuild.Append("<tr class='text-color-red'>");

                    sbuild.Append("<td>" + server.identificador + "</td>");
                    sbuild.Append("<td>" + lst_status[0].num_envios + "</td>");
                    sbuild.Append("<td>" + (lst_estadisticas.Where(ss => ss.type == "AYER").Count() == 1 ? Utilities.PonerPuntoMiles(lst_estadisticas.Where(ss => ss.type == "AYER").Select(ss => ss.data).First()) : "0") + "</td>");
                    sbuild.Append("<td>" + (lst_estadisticas.Where(ss => ss.type == "SEMANA").Count() == 1 ? Utilities.PonerPuntoMiles(lst_estadisticas.Where(ss => ss.type == "SEMANA").Select(ss => ss.data).First()) : "0") + "</td>");

                    string month = Utilities.MonthName(DateTime.Today.Month).ToUpper();
                    sbuild.Append("<td>" + (lst_estadisticas.Where(ss => ss.type == month && ss.year == DateTime.Today.Year).Count() == 1 ? Utilities.PonerPuntoMiles(lst_estadisticas.Where(ss => ss.type == month && ss.year == DateTime.Today.Year).Select(ss => ss.data).First()) : "0") + "</td>");

                    DateTime fecha_last_month = DateTime.Today.AddMonths(-1);
                    string last_month = Utilities.MonthName(fecha_last_month.Month).ToUpper();
                    sbuild.Append("<td>" + (lst_estadisticas.Where(ss => ss.type == last_month && ss.year == fecha_last_month.Year).Count() == 1 ? Utilities.PonerPuntoMiles(lst_estadisticas.Where(ss => ss.type == last_month && ss.year == fecha_last_month.Year).Select(ss => ss.data).First()) : "0") + "</td>");

                    sbuild.Append("<td>" + (lst_estadisticas.Where(ss => ss.type == "TOTAL" && ss.year == DateTime.Today.Year).Count() == 1 ? Utilities.PonerPuntoMiles(lst_estadisticas.Where(ss => ss.type == "TOTAL" && ss.year == DateTime.Today.Year).Select(ss => ss.data).First()) : "0") + "</td>");

                    if (server.prioritario)
                        sbuild.Append("<td><i class='fas fa-check fa-1-6x'></i></td>");
                    else
                        sbuild.Append("<td></td>");

                    if (id_user_special == -1)
                    {
                        sbuild.Append("<td><a href=\"informe-servidor-mantenimiento.aspx?ides=" + server.id_es + "\" title=\"Editar\"><i class='fas fa-edit fa-1-6x'></i></a></td>");
                        if (server.activo)
                            sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea desactivar el servidor " + server.identificador + "?\")){desactivarServidor(" + server.id_es + ")}' title='Desactivar servidor'><i class=\"fas fa-times-circle fa-1-6x\"></i></a></td>");
                        else
                            sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea activar el servidor " + server.identificador + "?\")){activarServidor(" + server.id_es + ")}' title='Activar servidor'><i class=\"fas fa-check-circle fa-1-6x\"></i></a></td>");
                    }
                    else
                        sbuild.Append("<td></td><td></td>");

                    sbuild.Append("</tr>");
                }
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }

        private string paint_table_sends(List<EMAIL_SERVER_ESTADISTICAS> lst_server_statistics)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Historico\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Año</th>");
            sbuild.Append("<th>Total</th>");
            sbuild.Append("<th>Enero</th>");
            sbuild.Append("<th>Febrero</th>");
            sbuild.Append("<th>Marzo</th>");
            sbuild.Append("<th>Abril</th>");
            sbuild.Append("<th>Mayo</th>");
            sbuild.Append("<th>Junio</th>");
            sbuild.Append("<th>Julio</th>");
            sbuild.Append("<th>Agosto</th>");
            sbuild.Append("<th>Septiembre</th>");
            sbuild.Append("<th>Octubre</th>");
            sbuild.Append("<th>Noviembre</th>");
            sbuild.Append("<th>Diciembre</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar las estadísticas
            if (lst_server_statistics.Count > 0)
            {
                /// 3.1.- Sacar los años de las estadísticas
                List<int> lst_years = lst_server_statistics.Where(se => se.type == "TOTAL").Select(se => se.year).Distinct().ToList();

                /// 3.2.- Recorrer los años
                foreach (var year in lst_years)
                {
                    /// 3.3.- Filtrar los datos de los años
                    List<EMAIL_SERVER_ESTADISTICAS> lst_statistics_filter = lst_server_statistics.Where(se => se.year == year).ToList();
                    if (lst_statistics_filter.Count > 0)
                    {
                        sbuild.Append("<tr>");

                        sbuild.Append("<td>" + year + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "TOTAL").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "ENERO").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "FEBRERO").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "MARZO").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "ABRIL").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "MAYO").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "JUNIO").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "JULIO").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "AGOSTO").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "SEPTIEMBRE").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "OCTUBRE").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "NOVIEMBRE").Select(se => se.data).Sum()) + "</td>");
                        sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_statistics_filter.Where(se => se.type == "DICIEMBRE").Select(se => se.data).Sum()) + "</td>");

                        sbuild.Append("</tr>");
                    }
                }
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }        
    }
}