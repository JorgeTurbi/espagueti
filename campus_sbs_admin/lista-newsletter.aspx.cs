using DocumentFormat.OpenXml.Spreadsheet;
using sbs_DAL;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_newsletter : System.Web.UI.Page
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
                    /// 1.- Cargar el listado de newsletters
                    load_newsletters(list_user[0]);
            }
        }
                
        protected void btnBorrarListado_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_newsletter = false;

            try
            {
                long id_campaing = !String.IsNullOrEmpty(hidIdListado.Value) ? long.Parse(hidIdListado.Value) : -1;
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        /// 2.- Eliminar los adjuntos si tiene
                        if (!String.IsNullOrEmpty(lst[0].adjuntos))
                        {
                            string[] list_adjuntos = lst[0].adjuntos.Split(',');
                            if (list_adjuntos.Length > 0)
                            {
                                foreach (var adjunto in list_adjuntos)
                                {
                                    File.Delete(adjunto);
                                }
                            }
                        }

                        /// 3.- Eliminar la campaña
                        delete_newsletter = da.deleteEmailCampaign(id_campaing);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la campaña');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::btnBorrarListado_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_newsletter)
                Response.Redirect("lista-newsletter.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la campaña');</script>");
        }

        protected void btnCerrarListado_Click(object sender, ImageClickEventArgs e)
        {
            bool close_newsletter = false;

            try
            {
                long id_campaing = !String.IsNullOrEmpty(hidIdListado.Value) ? long.Parse(hidIdListado.Value) : -1;
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        bool update_newsletter = da.updateStatusEmailCampaigns(lst[0], (int)Constantes.status_newsletter.Cerrado);
                        if (update_newsletter)
                            close_newsletter = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la campaña');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::btnCerrarListado_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (close_newsletter)
                Response.Redirect("lista-newsletter.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la campaña');</script>");
        }

        protected void btnCrearOpens_Click(object sender, ImageClickEventArgs e)
        {
            bool create_list = false;

            try
            {
                long id_campaing = !String.IsNullOrEmpty(hidIdListado.Value) ? long.Parse(hidIdListado.Value) : -1;
                if (id_campaing > 0)
                {
                    /// 1.- Sacar el estado de la campaña
                    List<EMAIL_CAMPAIGNS_STATUS> lst_status = da.getCampaignsStatusById(id_campaing);
                    if (lst_status.Count > 0)
                    {
                        /// 2.- Filtrar los de status open
                        lst_status = lst_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Open).ToList();
                        if (lst_status.Count > 0)
                        {
                            /// 3.- Sacar los datos de la campaña
                            List<EMAIL_CAMPAIGNS> list_newsletters = da.getCampaignsById(id_campaing);
                            if (list_newsletters.Count == 1)
                            {
                                /// 4.- Sacar los datos de la lista de suscriptores
                                List<EMAIL_LISTADO_SUSCRIPCIONES> lst_suscriptions = da.getEmailListSubscriptionsById(list_newsletters[0].id_els.Value);
                                if (lst_suscriptions.Count == 1)
                                {
                                    /// 5.- Sacar las suscripciones
                                    List<EMAIL_SUSCRIPCIONES> list_suscriptions = da.getEmailSubscriptionsById(lst_suscriptions[0].id_els);

                                    /// 5.1.- Filtrar las suscripciones
                                    list_suscriptions = list_suscriptions.Where(sus => lst_status.Select(s => s.id_s).ToList().Contains(sus.id_s) && sus.fecha_baja == null).ToList();
                                    if (list_suscriptions.Count > 0)
                                    {
                                        /// 6.- Crear una lista de suscriptores
                                        EMAIL_LISTADO_SUSCRIPCIONES suscripcion = new EMAIL_LISTADO_SUSCRIPCIONES();
                                        suscripcion.nombre = "temp_" + DateTime.Today.Year.ToString("00") + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + "_" + lst_suscriptions[0].nombre + "_opens";
                                        suscripcion.fecha_alta = DateTime.Today;
                                        suscripcion.num_total = list_suscriptions.Count;
                                        suscripcion.num_actual = 0;
                                        suscripcion.num_bajas = 0;
                                        suscripcion.historico = "Lista actualizada el " + DateTime.Today.ToShortDateString() + " con " + list_suscriptions.Count + " nuevos suscriptores.";

                                        /// 6.1.- Comprobar si es un usuario especial
                                        List<CLIENTES> list_user = new List<CLIENTES>();
                                        if (list_user.Count == 0 && Session["usuario"] != null)
                                            list_user.Add((CLIENTES)Session["usuario"]);

                                        List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                                        if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                                            suscripcion.id_usuario = list_user[0].id_cliente;

                                        long insert_list = da.insertEmailListSubscription(suscripcion);
                                        if (insert_list > 0)
                                        {
                                            bool _correct = true;
                                            foreach (var suscriptor in list_suscriptions)
                                            {
                                                EMAIL_SUSCRIPCIONES _suscriptor = new EMAIL_SUSCRIPCIONES();
                                                _suscriptor.id_els = insert_list;
                                                _suscriptor.nombre_completo = suscriptor.nombre_completo;
                                                _suscriptor.mail = suscriptor.mail;
                                                _suscriptor.id_alumno = suscriptor.id_alumno;
                                                _suscriptor.fecha_alta = DateTime.Today;

                                                long insert_sus = da.insertEmailSubscription(_suscriptor);
                                                if (insert_sus < 1)
                                                {
                                                    _correct = false;
                                                    break;
                                                }
                                            }

                                            if (_correct)
                                                create_list = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al generar la tabla de opens');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::btnCrearOpens_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (create_list)
                Response.Redirect("lista-suscriptores.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al generar la tabla de opens');</script>");
        }

        protected void btnCrearNoOpens_Click(object sender, ImageClickEventArgs e)
        {
            long id_newsletter = !String.IsNullOrEmpty(hidIdListado.Value) ? long.Parse(hidIdListado.Value) : -1;
            if (id_newsletter > 0)
            {
                List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                if (lst_newsletter.Count == 1)
                {
                    EMAIL_CAMPAIGNS campaign = lst_newsletter[0];

                    bool update_status = da.updateStatusPlanEmailCampaigns(campaign, (int)Constantes.status_campaign.Reenvio_no_opens);
                    if (update_status)
                    {
                        bool update_status_campaign = da.updateStatusEmailCampaigns(lst_newsletter[0], (int)Constantes.status_newsletter.Pendiente);
                        if (update_status_campaign)
                            Response.Redirect("lista-newsletter.aspx");
                        else
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al planificar el reenvío de no opens');</script>");
                    }
                    else
                        ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al planificar el reenvío de no opens');</script>");
                }
            }
        }

        protected void btnCopiarNewsletter_Click(object sender, ImageClickEventArgs e)
        {
            bool copy_newsletter = false;

            try
            {
                long id_campaing = !String.IsNullOrEmpty(hidIdListado.Value) ? long.Parse(hidIdListado.Value) : -1;
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        /// 2.- Duplicar la campaña
                        EMAIL_CAMPAIGNS newsletter = new EMAIL_CAMPAIGNS();
                        newsletter.nombre = lst[0].nombre + " - copy";
                        newsletter.fecha_scheduled = DateTime.Today;
                        newsletter.priority = lst[0].priority;
                        newsletter.mailFrom = lst[0].mailFrom;
                        newsletter.nombreFrom = lst[0].nombreFrom;
                        newsletter.replyTo = lst[0].replyTo;
                        newsletter.asunto = lst[0].asunto;
                        newsletter.body = lst[0].body;
                        newsletter.adjuntos = lst[0].adjuntos;
                        newsletter.id_usuario = lst[0].id_usuario;

                        /*long insert_newsletter = da.insertEmailCampaigns(newsletter);
                        if (insert_newsletter > 0)
                            copy_newsletter = true;*/

                        long insert_newsletter = da.insertEmailCampaigns(newsletter);
                        if (insert_newsletter > 0)
                        {
                            if (!String.IsNullOrEmpty(newsletter.adjuntos))
                            {
                                string rutas_adjuntos = string.Empty;

                                /// 3.- Copiar los adjuntos a la carpeta correspondiente
                                string[] list_adjuntos = newsletter.adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    /// 3.- Sacar la ruta original
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + lst[0].id_camp + "\\";
                                    string route_destiny = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + insert_newsletter + "\\";

                                    /// 4.- Crear el directorio de destino
                                    if (!(Directory.Exists(route_destiny)))
                                        Directory.CreateDirectory(route_destiny);

                                    /// 5.- Recorrer los adjuntos
                                    foreach (var _adjunto in list_adjuntos)
                                    {
                                        /// 5.1.- Sacar el ajunto original
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        try
                                        {
                                            /// 5.2.- Copiar el ajunto antiguo del destino antiguo al destino nuevo
                                            File.Copy(route + ajunto_clean, route_destiny + ajunto_clean, true);

                                            if (String.IsNullOrEmpty(rutas_adjuntos))
                                                rutas_adjuntos = route_destiny + ajunto_clean;
                                            else
                                                rutas_adjuntos = rutas_adjuntos + "," + route_destiny + ajunto_clean;
                                        }
                                        catch(Exception ex)
                                        {
                                            LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::btnCopiarNewsletter_Click()");
                                            LogUtils.InsertarLog("- MSG:" + ex.Message);
                                            LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                                        }
                                    }
                                }

                                /// 6.- Actualizar los adjuntos
                                List<EMAIL_CAMPAIGNS> lst_campaigns = da.getCampaignsById(insert_newsletter);
                                if (lst_campaigns.Count == 1)
                                {
                                    EMAIL_CAMPAIGNS campaign = lst_campaigns[0];
                                    campaign.adjuntos = rutas_adjuntos;

                                    bool update_newsletter = da.updateEmailCampaigns(campaign);
                                    if (update_newsletter)
                                        copy_newsletter = true;
                                }
                            }
                            else
                                copy_newsletter = true;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al duplicar la campaña');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::btnCopiarNewsletter_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (copy_newsletter)
                Response.Redirect("lista-newsletter.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al duplicar la campaña');</script>");
        }

        protected void btnCrearExcel_Click(object sender, ImageClickEventArgs e)
        {
            /// 0.- Sacar los datos del usuario 
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (list_user.Count == 0 && Session["usuario"] != null)
                list_user.Add((CLIENTES)Session["usuario"]);

            if (list_user.Count == 1)
            {
                /// 1.- Comprobar si es un usuario de envío de mails especiales
                long id_user_special = -1;
                List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();

                if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                    id_user_special = list_user[0].id_cliente;

                /// 2.- Sacar datos de la BBDD
                List<EMAIL_CAMPAIGNS> list_newsletters = da.getCampaigns();
                List<EMAIL_CAMPAIGNS_STATUS> list_status = da.getCampaignsStatusByList(new List<long>());
                List<EMAIL_LISTADO_SUSCRIPCIONES> listado_suscripciones = da.getEmailListSubscriptions();
                List<sbs_inf_landing> list_landings = da.getInfLandings();

                /// 3.- Filtrar las listas por el usuario especial
                if (id_user_special > 0)
                    list_newsletters = list_newsletters.Where(news => news.id_usuario == id_user_special).ToList();

                /// 4.- Recorrer las campañas y crear el excel
                SLDocument sl = new SLDocument();

                SLStyle style = sl.CreateStyle();
                style.Font.FontSize = 14;
                style.Font.FontColor = System.Drawing.Color.White;
                style.Font.Bold = true;
                style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Black, System.Drawing.Color.White);
                style.Alignment.Horizontal = HorizontalAlignmentValues.Center;
                sl.SetCellStyle(1, 1, 1, 15, style);

                sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Campañas");
                sl.SetCellValue(1, 1, "Nombre campaña");
                sl.SetCellValue(1, 2, "Asunto");
                sl.SetCellValue(1, 3, "Fecha");
                sl.SetCellValue(1, 4, "Hora");
                sl.SetCellValue(1, 5, "Día");
                sl.SetCellValue(1, 6, "Suscriptores");
                sl.SetCellValue(1, 7, "Envíos");
                sl.SetCellValue(1, 8, "Bajas");
                sl.SetCellValue(1, 9, "Bajas %");
                sl.SetCellValue(1, 10, "Opens");
                sl.SetCellValue(1, 11, "Opens %");
                sl.SetCellValue(1, 12, "Clics");
                sl.SetCellValue(1, 13, "Clics %");
                sl.SetCellValue(1, 14, "Lead");
                sl.SetCellValue(1, 15, "Lead %");

                int _index = 2;
                foreach (var campaing in list_newsletters)
                {
                    sl.SetCellValue(_index, 1, campaing.nombre + " (" + Utilities.obtenerNombreEstadoNewsletter(campaing.estado) + ")" + (campaing.id_camp_relacionada != null ? (campaing.id_camp_relacionada.Value > 0 ? "[" + list_newsletters.Where(news => news.id_camp == campaing.id_camp_relacionada).Select(news => news.nombre).FirstOrDefault() + "]" : string.Empty) : string.Empty) + " [" + listado_suscripciones.Where(ls => ls.id_els == campaing.id_els).Select(ls => ls.nombre).FirstOrDefault() + "]");
                    sl.SetCellValue(_index, 2, campaing.asunto);
                    sl.SetCellValue(_index, 3, campaing.fecha_scheduled.ToShortDateString());
                    sl.SetCellValue(_index, 4, campaing.fecha_scheduled.ToShortTimeString());
                    sl.SetCellValue(_index, 5, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(campaing.fecha_scheduled.ToString("dddd", new CultureInfo("es-Es"))));
                    sl.SetCellValue(_index, 6, campaing.num_suscriptores != null ? campaing.num_suscriptores.Value : 0);
                    sl.SetCellValue(_index, 7, campaing.num_envios != null ? campaing.num_envios.Value : 0);
                    sl.SetCellValue(_index, 8, campaing.num_unsubscribe != null ? campaing.num_unsubscribe.Value : 0);
                    sl.SetCellValue(_index, 9, campaing.pct_unsubscribe != null ? campaing.pct_unsubscribe.Value : 0);
                    sl.SetCellValue(_index, 10, campaing.num_opens != null ? campaing.num_opens.Value : 0);
                    sl.SetCellValue(_index, 11, campaing.pct_opens != null ? campaing.pct_opens.Value : 0);
                    sl.SetCellValue(_index, 12, campaing.num_clics != null ? campaing.num_clics.Value : 0);
                    sl.SetCellValue(_index, 13, campaing.pct_clicks != null ? campaing.pct_clicks.Value : 0);

                    List<sbs_inf_landing> lst_landing = list_landings.Where(l => l.idLanding == campaing.id_landing).ToList();
                    if (lst_landing.Count == 1)
                    {
                        sl.SetCellValue(_index, 14, lst_landing[0].n_leads);
                        sl.SetCellValue(_index, 15, lst_landing[0].pct_leads);
                    }
                    else
                    {
                        sl.SetCellValue(_index, 14, 0);
                        sl.SetCellValue(_index, 15, 0);
                    }

                    _index++;
                }

                sl.Sort(2, 1, _index, 15, 1, true);
                sl.AutoFitColumn(1, 15);

                string filename = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_Campañas.xlsx";

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                sl.SaveAs(Response.OutputStream);
                Response.End();
            }
        }

        private void load_newsletters(CLIENTES user)
        {
            /// 1.- Comprobar si es un usuario de envío de mails especiales
            long id_user_special = -1;
            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();

            if (list_users_mails.Contains(user.id_cliente.ToString()))
                id_user_special = user.id_cliente;

            /// 2.- Sacar datos de la BBDD
            int num_newsletters = int.Parse(ConfigurationManager.AppSettings["num_newsletters"]);
            List<EMAIL_CAMPAIGNS> list_newsletters = da.getCampaigns(num_newsletters);
            //List<EMAIL_CAMPAIGNS_STATUS> list_status = da.getCampaignsStatusByList(new List<long>());
            List<EMAIL_LISTADO_SUSCRIPCIONES> listado_suscripciones = da.getEmailListSubscriptions();
            List<sbs_inf_landing> list_landings = da.getInfLandings();

            /// 3.- Filtrar las listas por el usuario especial
            List<EMAIL_CAMPAIGNS_STATUS> list_status = new List<EMAIL_CAMPAIGNS_STATUS>();
            if (id_user_special > 0)
                list_newsletters = list_newsletters.Where(news => news.id_usuario == id_user_special).ToList();

            List<long> ids = list_newsletters.Select(_ => _.id_camp).Distinct().ToList();
            list_status = da.getCampaignsStatusByList(ids);

            //if (id_user_special > 0)
            //{
            //    list_newsletters = list_newsletters.Where(news => news.id_usuario == id_user_special).ToList();
            //    List<long> ids = list_newsletters.Select(_ => _.id_camp).Distinct().ToList();
            //    list_status = da.getCampaignsStatusByList(ids);
            //}
            //else
            //    list_status = da.getCampaignsStatusByList(new List<long>());

            /// 4.- Pintar la tabla
            table_listado_newsletters.InnerHtml = paint_table(list_newsletters, list_status, listado_suscripciones, list_landings);

            /// 5.- Pintar el título
            txt_lista_newsletter.InnerHtml = "<i class='far fa-list-alt'></i> Listado de campañas (Última act. " + list_newsletters.Where(ec => ec.fecha_act != null).OrderByDescending(ec => ec.fecha_act).Select(ec => ec.fecha_act).FirstOrDefault() + ") <a href='lista-newsletter-mantenimiento.aspx' title='Añadir campaña' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir campaña</small></a><a href='newsletter-mantenimiento.aspx' title='Añadir newsletter' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir newsletter</small></a><a href='javascript:void(0);' onclick='create_excel()' title='Crear excel' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='far fa-file-excel fa-2x text-color-green'></i>  Crear excel</small></a><a href='informe-servidor.aspx' title='Informe servidor' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-chart-bar fa-2x'></i> Informe servidor</small></a>";
        }

        private string paint_table(List<EMAIL_CAMPAIGNS> list_newsletters, List<EMAIL_CAMPAIGNS_STATUS> list_status, List<EMAIL_LISTADO_SUSCRIPCIONES> listado_suscripciones, List<sbs_inf_landing> list_landings)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Listados\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Hora</th>");
            sbuild.Append("<th>Día</th>");
            sbuild.Append("<th>Num</th>");
            sbuild.Append("<th>Envíos</th>");
            sbuild.Append("<th>Bajas</th>");
            sbuild.Append("<th>%</th>");
            sbuild.Append("<th>Opens</th>");
            sbuild.Append("<th>%</th>");
            sbuild.Append("<th>Clics</th>");
            sbuild.Append("<th>%</th>");
            sbuild.Append("<th>Lead</th>");
            sbuild.Append("<th>%</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar las campañas
            foreach (var campaing in list_newsletters)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td><strong>" + campaing.nombre + "</strong> (" + (campaing.estado == (int)Constantes.status_newsletter.Enviado ? "<span class='bg-color-green text-color-white padding-lr-5'>" + Utilities.obtenerNombreEstadoNewsletter(campaing.estado) + "</span>" : (campaing.estado == (int)Constantes.status_newsletter.Cerrado ? "<span class='bg-color-black text-color-white padding-lr-5'>" + Utilities.obtenerNombreEstadoNewsletter(campaing.estado) + "</span>" : (campaing.estado == (int)Constantes.status_newsletter.Enviando ? "<span class='bg-color-secondary text-color-white padding-lr-5'>" + Utilities.obtenerNombreEstadoNewsletter(campaing.estado) + "</span>" : Utilities.obtenerNombreEstadoNewsletter(campaing.estado)))) + ") " + (campaing.id_camp_relacionada != null ? (campaing.id_camp_relacionada.Value > 0 ? "[<span class='text-color-primary'>" + list_newsletters.Where(news => news.id_camp == campaing.id_camp_relacionada).Select(news => news.nombre).FirstOrDefault() + "</span>]" : string.Empty) : string.Empty) + " [" + listado_suscripciones.Where(ls => ls.id_els == campaing.id_els).Select(ls => ls.nombre).FirstOrDefault() + "] <br /> <i>" + campaing.asunto + "</i></td>");
                sbuild.Append("<td>" + campaing.fecha_scheduled.ToShortDateString() + "</td>");
                sbuild.Append("<td>" + campaing.fecha_scheduled.ToShortTimeString() + "</td>");
                sbuild.Append("<td>" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(campaing.fecha_scheduled.ToString("dddd", new CultureInfo("es-Es"))) + "</td>");
                sbuild.Append("<td>" + campaing.num_suscriptores + "</td>");
                sbuild.Append("<td>" + campaing.num_envios + "</td>");
                sbuild.Append("<td>" + campaing.num_unsubscribe+ "</td>");
                sbuild.Append("<td>" + (campaing.pct_unsubscribe != null ? campaing.pct_unsubscribe + "%" : "0,00%") + "</td>");
                sbuild.Append("<td>" + campaing.num_opens + "</td>");
                sbuild.Append("<td>" + (campaing.pct_opens != null ? campaing.pct_opens + "%" : "0,00%") + "</td>");
                sbuild.Append("<td>" + campaing.num_clics + "</td>");
                sbuild.Append("<td>" + (campaing.pct_clicks != null ? campaing.pct_clicks + "%" : "0,00%") + "</td>");

                List<sbs_inf_landing> lst_landing = list_landings.Where(l => l.idLanding == campaing.id_landing).ToList();
                if (lst_landing.Count == 1)
                {
                    sbuild.Append("<td>" + lst_landing[0].n_leads + "</td>");
                    sbuild.Append("<td>" + lst_landing[0].pct_leads + "%" + "</td>");
                }
                else
                {
                    sbuild.Append("<td>0</td>");
                    sbuild.Append("<td>0,00%</td>");
                }

                sbuild.Append("<td><a href='lista-newsletter-mantenimiento.aspx?idc=" + campaing.id_camp + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");

                List<EMAIL_CAMPAIGNS_STATUS> lst_status_filter = list_status.Where(s => s.id_camp == campaing.id_camp).ToList();
                if (lst_status_filter.Count > 0)
                {
                    if (campaing.estado == 4)
                        sbuild.Append("<td></td>");
                    else
                        sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea cerrar la campaña " + campaing.nombre.Replace('"', ' ').Replace("  ", " ") + "?\")){cerrarNewsletter(" + campaing.id_camp + ")}' title='Cerrar campaña'><i class=\"fas fa-times-circle fa-1-6x\"></i></a></td>");
                }
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la campaña " + campaing.nombre.Replace('"', ' ').Replace("  ", " ") + "?\")){eliminarNewsletter(" + campaing.id_camp + ")}' title='Eliminar campaña'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                if (campaing.status != null && campaing.status.Value > 0)
                {
                    if (campaing.status == (int)Constantes.status_campaign.Planificado)
                        sbuild.Append("<td><i class='fas fa-hourglass-start fa-1-6x text-color-danger' title='Estado campaña planificada'></i></td>");
                    else if (campaing.status == (int)Constantes.status_campaign.Proceso)
                        sbuild.Append("<td><i class='fas fa-hourglass-half fa-1-6x text-color-secondary' title='Estado campaña en proceso'></i></td>");
                    else if (campaing.status == (int)Constantes.status_campaign.Finalizado)
                        sbuild.Append("<td><i class='fas fa-hourglass-end fa-1-6x text-color-primary' title='Estado campaña finalizada'></i></td>");
                    else if (campaing.status == (int)Constantes.status_campaign.Reenvio_open)
                        sbuild.Append("<td><i class='fas fa-hourglass-start fa-1-6x text-color-danger' title='Estado campaña planificada reenvío opens'></i></td>");
                    else if (campaing.status == (int)Constantes.status_campaign.Reenvio_no_opens)
                        sbuild.Append("<td><i class='fas fa-hourglass-start fa-1-6x text-color-danger' title='Estado campaña planificada reenvío no opens'></i></td>");
                    else if (campaing.status == (int)Constantes.status_campaign.Reenvio_clicks)
                        sbuild.Append("<td><i class='fas fa-hourglass-start fa-1-6x text-color-danger' title='Estado campaña planificada reenvío clicks'></i></td>");
                }
                else
                    sbuild.Append("<td title='Estado planificación de la campaña'></td>");

                sbuild.Append("<td><a href='informe-newsletter.aspx?idc=" + campaing.id_camp + "' title='Report'><i class='fas fa-chart-line fa-1-6x'></i></a></td>");

                sbuild.Append("<td><a href='javascript:void(0)' onclick='crearListaOpens(" + campaing.id_camp + ");' title='Crear lista opens'><i class='fas fa-envelope-open-text fa-1-6x'></i></a></td>");

                List<EMAIL_CAMPAIGNS_STATUS> lst_status_no_opens = lst_status_filter.Where(ecs => ecs.status == (int)Constantes.status_mail.Send).ToList();
                if (lst_status_no_opens.Count > 0)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"Vas a realizar un reenvío a " + lst_status_no_opens.Count + " usuarios no open ¿Es correcto?\")){crearListaNoOpens(" + campaing.id_camp + ")}' title='Reenviar no opens'><i class='fas fa-mail-bulk fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td><i class='fas fa-mail-bulk text-color-primary fa-1-6x'></i></td>");

                //sbuild.Append("<td><a href='javascript:void(0)' onclick='crearListaNoOpens(" + campaing.id_camp + ");' title='Reenviar no opens'><i class='fas fa-mail-bulk fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='copyNewsletter(" + campaing.id_camp + ");' title='Copiar / Duplicar'><i class='fas fa-copy fa-1-6x'></i></a></td>");
                
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }
    }
}