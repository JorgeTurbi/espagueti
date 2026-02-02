using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_newsletter_mantenimiento : System.Web.UI.Page
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
                        load_newsletter(id_newsletter, list_user[0]);
                }
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Borrar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Sacar los datos del formulario
            string nombre = txt_nombre.Value.Trim();
            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio.Value) ? DateTime.Parse(txt_fecha_envio.Value).AddHours(int.Parse(ddl_hora.SelectedValue)).AddMinutes(int.Parse(ddl_minutos.SelectedValue)) : new DateTime();
            int prioridad = !String.IsNullOrEmpty(ddl_prioridad.SelectedValue) ? int.Parse(ddl_prioridad.SelectedValue) : -1;

            /// 3.- Comprobar si hay errores
            if (String.IsNullOrEmpty(nombre) || fecha_envio == new DateTime() || prioridad == -1)
                txt_error.InnerHtml = "Todos los campos son obligatorios";
            else
            {
                long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
                if (id_newsletter == -1)
                {
                    /// 4.- Crear la campaña
                    EMAIL_CAMPAIGNS newsletter = new EMAIL_CAMPAIGNS();
                    newsletter.nombre = nombre;
                    newsletter.fecha_scheduled = fecha_envio;
                    newsletter.priority = prioridad;

                    /// 4.1.- Comprobar si es un usuario especial
                    List<CLIENTES> list_user = new List<CLIENTES>();
                    if (list_user.Count == 0 && Session["usuario"] != null)
                        list_user.Add((CLIENTES)Session["usuario"]);

                    List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                    if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                        newsletter.id_usuario = list_user[0].id_cliente;

                    long insert_newsletter = da.insertEmailCampaigns(newsletter);
                    if (insert_newsletter > 0)
                        Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + insert_newsletter);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al guardar la campaña";
                }
            }
        }

        protected void btnGuardarAll_Click(object sender, EventArgs e)
        {
            /// 1.- Borrar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Sacar los datos del formulario
            string nombre = txt_nombre.Value.Trim();
            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio.Value) ? DateTime.Parse(txt_fecha_envio.Value).AddHours(int.Parse(ddl_hora.SelectedValue)).AddMinutes(int.Parse(ddl_minutos.SelectedValue)) : new DateTime();
            int prioridad = !String.IsNullOrEmpty(ddl_prioridad.SelectedValue) ? int.Parse(ddl_prioridad.SelectedValue) : -1;

            /// 3.- Comprobar si hay errores
            if (String.IsNullOrEmpty(nombre) || fecha_envio == new DateTime() || prioridad == -1)
                txt_error.InnerHtml = "Todos los campos son obligatorios";
            else
            {
                long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
                if (id_newsletter > 0)
                {
                    /// 4.- Sacar el resto de parámetros            
                    string name_from = txt_nombre_from.Value;
                    string mail_from = txt_mail_from.Value;
                    string reply_to = txt_reply_to.Value;
                    string mail_asunto = txt_asunto.Value;
                    string mail_body = txt_cuerpo.Value;
                    string adjuntos = string.Empty;
                    bool validate_files = true;

                    #region Adjuntos

                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    string adjunto1 = string.Empty;
                    string adjunto2 = string.Empty;
                    string adjunto3 = string.Empty;
                    string adjunto4 = string.Empty;
                    string adjunto5 = string.Empty;

                    /// Adjunto 1
                    if (fuAdjunto1.HasFile)
                    {
                        if (Directory.Exists(route))
                        {
                            FileInfo archivo = new FileInfo(fuAdjunto1.PostedFile.FileName);
                            adjunto1 = fuAdjunto1.PostedFile.FileName.Replace(" ", "-");
                            try
                            {
                                fuAdjunto1.PostedFile.SaveAs(route + adjunto1);
                            }
                            catch
                            {
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                validate_files = false;
                            }
                        }
                    }
                    else
                        adjunto1 = lnkAdjunto1.InnerText;

                    /// Adjunto 2
                    if (fuAdjunto2.HasFile)
                    {
                        if (Directory.Exists(route))
                        {
                            FileInfo archivo = new FileInfo(fuAdjunto2.PostedFile.FileName);
                            adjunto2 = fuAdjunto2.PostedFile.FileName.Replace(" ", "-");
                            try
                            {
                                fuAdjunto2.PostedFile.SaveAs(route + adjunto2);
                            }
                            catch
                            {
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                validate_files = false;
                            }
                        }
                    }
                    else
                        adjunto2 = lnkAdjunto2.InnerText;

                    /// Adjunto 3
                    if (fuAdjunto3.HasFile)
                    {
                        if (Directory.Exists(route))
                        {
                            FileInfo archivo = new FileInfo(fuAdjunto3.PostedFile.FileName);
                            adjunto3 = fuAdjunto3.PostedFile.FileName.Replace(" ", "-");
                            try
                            {
                                fuAdjunto3.PostedFile.SaveAs(route + adjunto3);
                            }
                            catch
                            {
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                validate_files = false;
                            }
                        }
                    }
                    else
                        adjunto3 = lnkAdjunto3.InnerText;

                    /// Adjunto 4
                    if (fuAdjunto4.HasFile)
                    {
                        if (Directory.Exists(route))
                        {
                            FileInfo archivo = new FileInfo(fuAdjunto4.PostedFile.FileName);
                            adjunto4 = fuAdjunto4.PostedFile.FileName.Replace(" ", "-");
                            try
                            {
                                fuAdjunto4.PostedFile.SaveAs(route + adjunto4);
                            }
                            catch
                            {
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                validate_files = false;
                            }
                        }
                    }
                    else
                        adjunto4 = lnkAdjunto4.InnerText;

                    /// Adjunto 5
                    if (fuAdjunto5.HasFile)
                    {
                        if (Directory.Exists(route))
                        {
                            FileInfo archivo = new FileInfo(fuAdjunto5.PostedFile.FileName);
                            adjunto5 = fuAdjunto5.PostedFile.FileName.Replace(" ", "-");
                            try
                            {
                                fuAdjunto5.PostedFile.SaveAs(route + adjunto5);
                            }
                            catch
                            {
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                validate_files = false;
                            }
                        }
                    }
                    else
                        adjunto5 = lnkAdjunto5.InnerText;

                    List<string> list_adjuntos = new List<string>();
                    if (!String.IsNullOrEmpty(adjunto1))
                        list_adjuntos.Add(adjunto1);
                    if (!String.IsNullOrEmpty(adjunto2))
                        list_adjuntos.Add(adjunto2);
                    if (!String.IsNullOrEmpty(adjunto3))
                        list_adjuntos.Add(adjunto3);
                    if (!String.IsNullOrEmpty(adjunto4))
                        list_adjuntos.Add(adjunto4);
                    if (!String.IsNullOrEmpty(adjunto5))
                        list_adjuntos.Add(adjunto5);

                    int cont = 0;
                    if (list_adjuntos.Count > 0)
                    {
                        foreach (string _adjunto in list_adjuntos)
                        {
                            if (cont == 0)
                                adjuntos = route + _adjunto;
                            else
                                adjuntos += "," + route + _adjunto;
                            cont++;
                        }
                    }

                    #endregion

                    long idCampaignPrincipal = !String.IsNullOrEmpty(ddl_newsletter_principal.SelectedValue) ? long.Parse(ddl_newsletter_principal.SelectedValue) : -1;
                    long idListSubscription = !String.IsNullOrEmpty(dll_lista_suscriptores.SelectedValue) ? long.Parse(dll_lista_suscriptores.SelectedValue) : -1;

                    long idLanding = !String.IsNullOrEmpty(ddl_lista_landings.SelectedValue) ? long.Parse(ddl_lista_landings.SelectedValue) : -1;
                    string lst_tags = txt_tags.Value;
                    string lst_tags_clic = txt_tags_clics.Value;
                    long idUsuarioEspecial = !String.IsNullOrEmpty(ddlUsuarioEspecial.SelectedValue) ? long.Parse(ddlUsuarioEspecial.SelectedValue) : -1;

                    if (validate_files)
                    {
                        List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                        if (lst_newsletter.Count == 1)
                        {
                            EMAIL_CAMPAIGNS newsletter = lst_newsletter[0];
                            newsletter.nombre = nombre;
                            newsletter.fecha_scheduled = fecha_envio;
                            newsletter.priority = prioridad;
                            newsletter.nombreFrom = name_from;
                            newsletter.mailFrom = mail_from;
                            newsletter.replyTo = reply_to;
                            newsletter.asunto = mail_asunto;
                            /*if (lst_newsletter[0].newsletter != null && lst_newsletter[0].newsletter.Value)
                                newsletter.body = lst_newsletter[0].body;
                            else
                                newsletter.body = mail_body;*/
                            newsletter.body = mail_body;
                            newsletter.adjuntos = adjuntos;
                            newsletter.id_camp_relacionada = idCampaignPrincipal;
                            newsletter.id_els = idListSubscription;
                            newsletter.id_landing = idLanding;
                            newsletter.tags = lst_tags;
                            newsletter.tags_clic = lst_tags_clic;
                            if (idUsuarioEspecial > 0)
                                newsletter.id_usuario = idUsuarioEspecial;
                            else
                                newsletter.id_usuario = null;

                            bool update_newsletter = da.updateEmailCampaigns(newsletter);
                            if (update_newsletter)
                                Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la campaña";
                        }
                        else
                            txt_error.InnerHtml = "Se han producido un error al buscar la campaña";
                    }
                    else
                        txt_error.InnerHtml = "Se han producido errores al guardar los adjuntos";
                }
            }
        }

        protected void btnMailPrueba_Click(object sender, EventArgs e)
        {
            /// 1.- Limpiar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Recuperar el mail del formulario
            string mail_prueba = Utils.esMailCorrecto(txt_mail_prueba.Value.Trim()) ? txt_mail_prueba.Value.Trim() : string.Empty;

            /// 3.- Validar que el mail es correcto
            if (!String.IsNullOrEmpty(mail_prueba))
            {
                long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
                if (id_newsletter > 0)
                {
                    List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                    if (lst_newsletter.Count == 1)
                    {
                        /// 4.- Generar mail de prueba

                        /// 4.1.- Obtener los datos del cuerpo
                        string template = string.Empty;
                        if (lst_newsletter[0].newsletter != null && lst_newsletter[0].newsletter.Value)
                        {
                            template = lst_newsletter[0].body;
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###URL_WEB###", string.Empty);
                                template = template.Replace("###NOMBRE_LISTA###", string.Empty);
                                template = template.Replace("###URL_PERFIL###", string.Empty);

                                template = template.Replace("###URL_BAJA###", string.Empty);
                                template = template.Replace("###URL_PIXEL###", string.Empty);
                            }
                        }
                        else
                        {
                            /*List<CLIENTES> list_user = new List<CLIENTES>();
                            if (list_user.Count == 0 && Session["usuario"] != null)
                                list_user.Add((CLIENTES)Session["usuario"]);*/

                            string template_user = string.Empty;
                            /*List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                            if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                            {
                                if (list_user[0].id_cliente == (long)Constantes.usuario_especial_mail.Aniacam)
                                    template_user = "email_campaign_aniacam";
                                else if (list_user[0].id_cliente == (long)Constantes.usuario_especial_mail.Sbs_Comunicacion)
                                    template_user = "email_campaign_comunicacion";
                                else if (list_user[0].id_cliente == (long)Constantes.usuario_especial_mail.Munuslingua)
                                    template_user = "email_campaign_munuslingua";
                            }
                            else
                                template_user = "email_campaign";*/

                            if (lst_newsletter[0].id_usuario != null)
                            {
                                List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                                if (list_users_mails.Contains(lst_newsletter[0].id_usuario.Value.ToString()))
                                {
                                    if (lst_newsletter[0].id_usuario.Value == (long)Constantes.usuario_especial_mail.Aniacam)
                                        template_user = "email_campaign_aniacam";
                                    else if (lst_newsletter[0].id_usuario.Value == (long)Constantes.usuario_especial_mail.Sbs_Comunicacion)
                                        template_user = "email_campaign_comunicacion";
                                    else if (lst_newsletter[0].id_usuario.Value == (long)Constantes.usuario_especial_mail.Munuslingua)
                                        template_user = "email_campaign_munuslingua";
                                    else
                                        template_user = "email_campaign";
                                }
                                else
                                    template_user = "email_campaign";
                            }
                            else
                                template_user = "email_campaign";

                            template = Utilities.getPlantillaMail(template_user, ConfigurationManager.AppSettings["urlTemplate"]);
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###URL_WEB###", string.Empty);
                                template = template.Replace("###BODY###", lst_newsletter[0].body);

                                template = template.Replace("###NOMBRE###", "PRUEBA MAIL");
                                template = template.Replace("###NOMBRE_LISTA###", string.Empty);
                                template = template.Replace("###URL_PERFIL###", string.Empty);
                                template = template.Replace("###MAIL_FROM###", lst_newsletter[0].mailFrom);

                                template = template.Replace("###URL_BAJA###", string.Empty);
                                template = template.Replace("###URL_PIXEL###", string.Empty);
                            }
                        }

                        EMAIL_CONTENT _mail = new EMAIL_CONTENT();
                        _mail.mailTo = mail_prueba;
                        _mail.priority = 1;
                        _mail.nombreFrom = lst_newsletter[0].nombreFrom;
                        _mail.mailFrom = lst_newsletter[0].mailFrom;                        
                        _mail.asunto = lst_newsletter[0].asunto;
                        _mail.body = template;
                        _mail.adjuntos = lst_newsletter[0].adjuntos;

                        long insert_mail = da.insertEmailContent(_mail);
                        if (insert_mail > 0)
                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al mandar el mail de prueba";
                    }
                }
            }
            else
                txt_error.InnerHtml = "Hay que introducir un mail de prueba";
        }

        protected void btnEnvio_Click(object sender, EventArgs e)
        {
            /// 1.- Limpiar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Sacar los datos de la campaña
            long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            if (id_newsletter > 0)
            {
                List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                if (lst_newsletter.Count == 1)
                {
                    EMAIL_CAMPAIGNS campaign = lst_newsletter[0];

                    bool update_status = da.updateStatusPlanEmailCampaigns(campaign, (int)Constantes.status_campaign.Planificado);
                    if (update_status)
                        Response.Redirect("lista-newsletter.aspx");
                    else
                        txt_error.InnerHtml = "Se ha producido un error al planificar el envío";
                }
            }
        }
        
        protected void btnEnvioNoOpens_Click(object sender, EventArgs e)
        {
            /// 1.- Limpiar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Sacar los datos de la campaña
            long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            if (id_newsletter > 0)
            {
                List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                if (lst_newsletter.Count == 1)
                {
                    EMAIL_CAMPAIGNS campaign = lst_newsletter[0];

                    bool update_status = da.updateStatusPlanEmailCampaigns(campaign, (int)Constantes.status_campaign.Reenvio_no_opens);
                    if (update_status)
                    {
                        bool update_campaigns = da.updateStatusEmailCampaigns(lst_newsletter[0], (int)Constantes.status_newsletter.Pendiente);
                        if (update_campaigns)
                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al planificar el envío";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al planificar el envío";
                }
            }
        }
        
        protected void btn_del_Adjunto1_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_1 = false;
            long id_campaing = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;

            try
            {
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_campaing + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto1.InnerText))
                        {
                            string file_delete = route + lnkAdjunto1.InnerText;
                            File.Delete(file_delete);
                            lnkAdjunto1.InnerText = string.Empty;
                            fuAdjunto1.Visible = true;
                            blk_del_1.Attributes["class"] = blk_del_1.Attributes["class"].Insert(blk_del_1.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_1 = da.updateAdjuntosEmailCampaigns(lst[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::btn_del_Adjunto1_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_1)
                Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_campaing);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto2_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_2 = false;
            long id_campaing = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;

            try
            {
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_campaing + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto2.InnerText))
                        {
                            string file_delete = route + lnkAdjunto2.InnerText;
                            File.Delete(file_delete);
                            lnkAdjunto2.InnerText = string.Empty;
                            fuAdjunto2.Visible = true;
                            blk_del_2.Attributes["class"] = blk_del_2.Attributes["class"].Insert(blk_del_2.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_2 = da.updateAdjuntosEmailCampaigns(lst[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::btn_del_Adjunto2_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_2)
                Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_campaing);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto3_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_3 = false;
            long id_campaing = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;

            try
            {
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_campaing + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto3.InnerText))
                        {
                            string file_delete = route + lnkAdjunto3.InnerText;
                            File.Delete(file_delete);
                            lnkAdjunto3.InnerText = string.Empty;
                            fuAdjunto3.Visible = true;
                            blk_del_3.Attributes["class"] = blk_del_3.Attributes["class"].Insert(blk_del_3.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_3 = da.updateAdjuntosEmailCampaigns(lst[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::btn_del_Adjunto3_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_3)
                Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_campaing);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto4_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_4 = false;
            long id_campaing = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;

            try
            {
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_campaing + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto4.InnerText))
                        {
                            string file_delete = route + lnkAdjunto4.InnerText;
                            File.Delete(file_delete);
                            lnkAdjunto4.InnerText = string.Empty;
                            fuAdjunto4.Visible = true;
                            blk_del_4.Attributes["class"] = blk_del_4.Attributes["class"].Insert(blk_del_4.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_4 = da.updateAdjuntosEmailCampaigns(lst[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::btn_del_Adjunto4_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_4)
                Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_campaing);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto5_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_5 = false;
            long id_campaing = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;

            try
            {
                if (id_campaing > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_CAMPAIGNS> lst = da.getCampaignsById(id_campaing);
                    if (lst.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_campaing + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto5.InnerText))
                        {
                            string file_delete = route + lnkAdjunto5.InnerText;
                            File.Delete(file_delete);
                            lnkAdjunto5.InnerText = string.Empty;
                            fuAdjunto5.Visible = true;
                            blk_del_5.Attributes["class"] = blk_del_5.Attributes["class"].Insert(blk_del_5.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_5 = da.updateAdjuntosEmailCampaigns(lst[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::btn_del_Adjunto5_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_5)
                Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_campaing);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        private void load_newsletter(long id_newsletter, CLIENTES user)
        {
            /// Sacar los datos de la campaña
            List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
            if (lst_newsletter.Count == 1)
            {
                /// 1.- Desbloquear el bloque de la campaña
                block_all.Attributes["class"] = block_all.Attributes["class"].Replace("hidden", string.Empty);

                /// 2.- Ocultar el bloque de los botones de la campaña inicial
                block_save.Attributes["class"] = block_save.Attributes["class"].Insert(block_save.Attributes["class"].Length, " hidden");

                /// 3.- Cargar los datos de la campaña inicial

                /// 3.1.- Nombre de la campaña 
                txt_nombre.Value = lst_newsletter[0].nombre;

                /// 3.2.- Fecha de envío
                txt_fecha_envio.Value = lst_newsletter[0].fecha_scheduled.ToShortDateString();

                /// 3.2.1.- Hora
                ddl_hora.SelectedValue = lst_newsletter[0].fecha_scheduled.Hour.ToString("00");

                /// 3.2.1.- Minutos
                ddl_minutos.SelectedValue = lst_newsletter[0].fecha_scheduled.Minute.ToString("00");

                /// 3.3.- Prioridad
                ddl_prioridad.SelectedValue = lst_newsletter[0].priority.ToString();

                /// 4.- Cargar los datos de los combos
                List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();

                /// 4.1.- Listado de campañas principales
                List<EMAIL_CAMPAIGNS> list_camp = da.getCampaigns();
                list_camp = list_camp.Where(camp => camp.id_camp != id_newsletter).ToList();
                
                if (list_users_mails.Contains(user.id_cliente.ToString()))
                    list_camp = list_camp.Where(camp => camp.id_usuario == user.id_cliente).ToList();
                if (list_camp.Count > 0)
                {
                    this.ddl_newsletter_principal.DataSource = list_camp.OrderBy(camp => camp.nombre).ToList();
                    this.ddl_newsletter_principal.DataTextField = "Nombre";
                    this.ddl_newsletter_principal.DataValueField = "id_camp";
                    this.ddl_newsletter_principal.DataBind();
                    this.ddl_newsletter_principal.Items.Add(new ListItem("Seleccione", "-1"));
                }

                /// 4.2.- Listado de listas de suscriptores
                List<EMAIL_LISTADO_SUSCRIPCIONES> listado_suscripciones = da.getEmailListSubscriptions();
                if (list_users_mails.Contains(user.id_cliente.ToString()))
                    listado_suscripciones = listado_suscripciones.Where(els => els.id_usuario == user.id_cliente).ToList();
                if (listado_suscripciones.Count > 0)
                {
                    this.dll_lista_suscriptores.DataSource = listado_suscripciones.OrderBy(els => els.nombre).ToList();
                    this.dll_lista_suscriptores.DataTextField = "Nombre";
                    this.dll_lista_suscriptores.DataValueField = "id_els";
                    this.dll_lista_suscriptores.DataBind();
                    this.dll_lista_suscriptores.Items.Add(new ListItem("Seleccione", "-1"));
                }

                /// 4.3.- Listado de landings abiertas
                List<sbs_Landing> lst_landings = da.getLandingById(-1);
                if (lst_landings.Count > 0)
                {
                    ddl_lista_landings.DataSource = lst_landings.Where(l => l.fecha_cierre == null).OrderBy(l => l.title).ToList();
                    ddl_lista_landings.DataTextField = "title";
                    ddl_lista_landings.DataValueField = "idCampaign";
                    ddl_lista_landings.DataBind();
                    ddl_lista_landings.Items.Add(new ListItem("Seleccione", "-1"));
                }

                /// 4.4.- Listado de usuarios especiales
                List<long> _users_specials = new List<long>();
                foreach (var _id_user_special in list_users_mails)
                {
                    _users_specials.Add(long.Parse(_id_user_special));
                }
                if (list_users_mails.Contains(user.id_cliente.ToString()))
                    _users_specials = _users_specials.Where(_ => _ == user.id_cliente).ToList();
                List<CLIENTES> _usuarios_especiales = da.getUserByList(_users_specials);
                if (_usuarios_especiales.Count > 0)
                {
                    ddlUsuarioEspecial.DataSource = _usuarios_especiales.OrderBy(_ => _.Nombre_Completo).ToList();
                    ddlUsuarioEspecial.DataTextField = "Nombre_Completo";
                    ddlUsuarioEspecial.DataValueField = "id_cliente";
                    ddlUsuarioEspecial.DataBind();
                    ddlUsuarioEspecial.Items.Add(new ListItem("Seleccione", "-1"));
                }

                /// 5.- Cargar los datos de la campaña completa

                /// 5.1.- Cargar datos de los mails
                txt_nombre_from.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                txt_mail_from.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                txt_reply_to.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                txt_asunto.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                txt_cuerpo.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                /// 5.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1.Visible = false;
                                lnkAdjunto1.Visible = true;
                                lnkAdjunto1.HRef = route_see + ajunto_clean;
                                lnkAdjunto1.InnerText = ajunto_clean;
                                blk_del_1.Attributes["class"] = blk_del_1.Attributes["class"].Replace("hidden", string.Empty);
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2.Visible = false;
                                lnkAdjunto2.Visible = true;
                                lnkAdjunto2.HRef = route_see + ajunto_clean;
                                lnkAdjunto2.InnerText = ajunto_clean;
                                blk_del_2.Attributes["class"] = blk_del_2.Attributes["class"].Replace("hidden", string.Empty);
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3.Visible = false;
                                lnkAdjunto3.Visible = true;
                                lnkAdjunto3.HRef = route_see + ajunto_clean;
                                lnkAdjunto3.InnerText = ajunto_clean;
                                blk_del_3.Attributes["class"] = blk_del_3.Attributes["class"].Replace("hidden", string.Empty);
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4.Visible = false;
                                lnkAdjunto4.Visible = true;
                                lnkAdjunto4.HRef = route_see + ajunto_clean;
                                lnkAdjunto4.InnerText = ajunto_clean;
                                blk_del_4.Attributes["class"] = blk_del_4.Attributes["class"].Replace("hidden", string.Empty);
                            }
                            else
                            {
                                fuAdjunto5.Visible = false;
                                lnkAdjunto5.Visible = true;
                                lnkAdjunto5.HRef = route_see + ajunto_clean;
                                lnkAdjunto5.InnerText = ajunto_clean;
                                blk_del_5.Attributes["class"] = blk_del_5.Attributes["class"].Replace("hidden", string.Empty);
                            }
                            index++;
                        }
                    }
                }

                /// 5.3.- Cargar la campaña principal
                ddl_newsletter_principal.SelectedValue = lst_newsletter[0].id_camp_relacionada != null ? lst_newsletter[0].id_camp_relacionada.Value.ToString() : "-1";

                /// 5.4.- Cargar la lista de suscriptores
                dll_lista_suscriptores.SelectedValue = lst_newsletter[0].id_els != null ? lst_newsletter[0].id_els.Value.ToString() : "-1";

                /// 5.5.- Desbloquear Planificar envío si tiene definida una lista de suscriptores
                if (lst_newsletter[0].id_els != null && lst_newsletter[0].id_els > 0 && !String.IsNullOrEmpty(lst_newsletter[0].asunto) && !String.IsNullOrEmpty(lst_newsletter[0].body) && !String.IsNullOrEmpty(lst_newsletter[0].mailFrom))
                {
                    List<EMAIL_SUSCRIPCIONES> list_subscriptions = da.getEmailSubscriptionsById(lst_newsletter[0].id_els.Value).Where(es => es.fecha_baja == null).ToList();
                    List<EMAIL_LISTADO_SUSCRIPCIONES> list_listado = listado_suscripciones.Where(els => els.id_els == lst_newsletter[0].id_els.Value).ToList();

                    btnMailPrueba.Visible = true;
                    blk_send_mail.Attributes["class"] = blk_send_mail.Attributes["class"].Replace("hidden", string.Empty);

                    if (lst_newsletter[0].status == null)
                    {
                        btn_envio.Attributes["class"] = btn_envio.Attributes["class"].Replace("hidden", string.Empty);
                        btn_envio.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + lst_newsletter[0].nombre + " a la lista " + list_listado[0].nombre + " con " + list_subscriptions.Count + " suscriptores para la fecha " + lst_newsletter[0].fecha_scheduled.ToShortDateString() + "')){planificar_envio()}");
                    }

                    /*if (lst_newsletter[0].estado == null)
                    {
                        btn_envio.Attributes["class"] = btn_envio.Attributes["class"].Replace("hidden", string.Empty);
                        btn_envio.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + lst_newsletter[0].nombre + " a la lista " + list_listado[0].nombre + " con " + list_subscriptions.Count + " suscriptores para la fecha " + lst_newsletter[0].fecha_scheduled.ToShortDateString() + "')){planificar_envio()}");
                    }*/

                    if (lst_newsletter[0].estado == (int)Constantes.status_newsletter.Enviado)
                        btnEnvioNoOpens.Visible = true;

                    //// Cargar bloques de reenvíos
                    load_blocks_reenvios(lst_newsletter[0]);
                }

                /// 5.6.- Cargar la lista de suscriptores
                ddl_lista_landings.SelectedValue = lst_newsletter[0].id_landing != null ? lst_newsletter[0].id_landing.Value.ToString() : "-1";

                /// 5.7.- Cargar los tags
                txt_tags.Value = lst_newsletter[0].tags;

                /// 5.8.- Cargar los tags clics
                txt_tags_clics.Value = lst_newsletter[0].tags_clic;

                /// 5.9.- Cargar los usuarios especiales
                ddlUsuarioEspecial.SelectedValue = lst_newsletter[0].id_usuario.HasValue ? lst_newsletter[0].id_usuario.Value.ToString() : "-1";

                /// 6.- Añadir el tipo de newsletter
                if (lst_newsletter[0].newsletter != null && lst_newsletter[0].newsletter.Value)
                    type_newsletter.Value = "1";
            }
        }
        
        private List<string> obtener_urls(string body)
        {
            List<string> list_urls = new List<string>();

            /// 1.- Dividir el texto en párrafos
            List<string> lst_paragraphs = body.Split(("<a").Split(','), StringSplitOptions.None).ToList();

            /// 2.- Filtrar los párrafos que contengan href
            lst_paragraphs = lst_paragraphs.Where(p => p.Contains("href=")).ToList();

            /// 3.- Limpiar hrefs
            List<string> lst_hrefs = new List<string>();
            foreach (var paragraph in lst_paragraphs)
            {
                List<string> lst = paragraph.Split(("href=\"").Split(','), StringSplitOptions.None).ToList();
                lst_hrefs.Add(lst[1]);
            }

            /// 4.- Sacar la urls
            if (lst_hrefs.Count > 0)
            {
                foreach (var href in lst_hrefs)
                {
                    List<string> list_href = href.Split('"').ToList();

                    if (!list_href[0].Contains("###") && !list_href[0].Contains("mailto") && !list_href[0].Contains("email-click") && !list_href[0].Contains("tel:") && !list_urls.Contains(list_href[0]))
                        list_urls.Add(list_href[0].Replace("&amp;", "&"));
                }
            }

            return list_urls;
        }

        private List<string> obtener_urls_clicks(string template)
        {
            List<string> list_urls = new List<string>();

            /// 1.- Dividir el texto en párrafos
            List<string> lst_paragraphs = template.Split(("<a").Split(','), StringSplitOptions.None).ToList();

            /// 2.- Filtrar los párrafos que contengan href
            lst_paragraphs = lst_paragraphs.Where(p => p.Contains("href=")).ToList();

            /// 3.- Limpiar hrefs
            List<string> lst_hrefs = new List<string>();
            foreach (var paragraph in lst_paragraphs)
            {
                List<string> lst = paragraph.Split(("href=\"").Split(','), StringSplitOptions.None).ToList();
                lst_hrefs.Add(lst[1]);
            }

            /// 4.- Sacar la urls
            if (lst_hrefs.Count > 0)
            {
                foreach (var href in lst_hrefs)
                {
                    List<string> list_href = href.Split('"').ToList();

                    if (list_href[0].Contains("email-click") && !list_urls.Contains(list_href[0]))
                        list_urls.Add(list_href[0]);
                }
            }

            return list_urls;
        }

        private void load_blocks_reenvios(EMAIL_CAMPAIGNS newsletter)
        {
            /// 1.- Mostrar los bloques
            block_no_opens.Attributes["class"] = block_no_opens.Attributes["class"].Replace("hidden", string.Empty);
            block_no_clics.Attributes["class"] = block_no_clics.Attributes["class"].Replace("hidden", string.Empty);
            block_clics.Attributes["class"] = block_clics.Attributes["class"].Replace("hidden", string.Empty);

            List<EMAIL_ACTIVADORES> lst_activators = da.getActivatorsByIdCampaign(newsletter.id_camp);
            if (lst_activators.Count > 0)
            {
                /// 2.- No opens
                List<EMAIL_ACTIVADORES> lst_activators_no_opens = lst_activators.Where(ea => ea.tipo == (int)Constantes.status_campaign.Reenvio_no_opens).ToList();
                if (lst_activators_no_opens.Count > 0)
                {
                    /// 2.1.- Reintento 1
                    List<EMAIL_ACTIVADORES> lst_activators_no_opens_1 = lst_activators_no_opens.Where(ea => ea.reintento == 1).ToList();
                    if (lst_activators_no_opens_1.Count == 1)
                        load_no_opens(newsletter, 1, lst_activators_no_opens_1[0]);

                    /// 2.2.- Reintento 2
                    List<EMAIL_ACTIVADORES> lst_activators_no_opens_2 = lst_activators_no_opens.Where(ea => ea.reintento == 2).ToList();
                    if (lst_activators_no_opens_2.Count == 1)
                        load_no_opens(newsletter, 2, lst_activators_no_opens_2[0]);

                    /// 2.3.- Reintento 1
                    List<EMAIL_ACTIVADORES> lst_activators_no_opens_3 = lst_activators_no_opens.Where(ea => ea.reintento == 3).ToList();
                    if (lst_activators_no_opens_3.Count == 1)
                        load_no_opens(newsletter, 3, lst_activators_no_opens_3[0]);
                }

                /// 3.- Opens
                List<EMAIL_ACTIVADORES> lst_activators_opens = lst_activators.Where(ea => ea.tipo == (int)Constantes.status_campaign.Reenvio_open).ToList();
                if (lst_activators_no_opens.Count > 0)
                {
                    /// 3.1.- Reintento 1
                    List<EMAIL_ACTIVADORES> lst_activators_opens_1 = lst_activators_opens.Where(ea => ea.reintento == 1).ToList();
                    if (lst_activators_opens_1.Count == 1)
                        load_open(newsletter, 1, lst_activators_opens_1[0]);

                    /// 3.2.- Reintento 2
                    List<EMAIL_ACTIVADORES> lst_activators_opens_2 = lst_activators_opens.Where(ea => ea.reintento == 2).ToList();
                    if (lst_activators_opens_2.Count == 1)
                        load_open(newsletter, 2, lst_activators_opens_2[0]);

                    /// 3.3.- Reintento 1
                    List<EMAIL_ACTIVADORES> lst_activators_opens_3 = lst_activators_opens.Where(ea => ea.reintento == 3).ToList();
                    if (lst_activators_opens_3.Count == 1)
                        load_open(newsletter, 3, lst_activators_opens_3[0]);
                }

                /// 4.- Clics
                List<EMAIL_ACTIVADORES> lst_activators_clics = lst_activators.Where(ea => ea.tipo == (int)Constantes.status_campaign.Reenvio_clicks).ToList();
                if (lst_activators_no_opens.Count > 0)
                {
                    /// 4.1.- Reintento 1
                    List<EMAIL_ACTIVADORES> lst_activators_clics_1 = lst_activators_clics.Where(ea => ea.reintento == 1).ToList();
                    if (lst_activators_clics_1.Count == 1)
                        load_clics(newsletter, 1, lst_activators_clics_1[0]);

                    /// 4.2.- Reintento 2
                    List<EMAIL_ACTIVADORES> lst_activators_clics_2 = lst_activators_clics.Where(ea => ea.reintento == 2).ToList();
                    if (lst_activators_clics_2.Count == 1)
                        load_clics(newsletter, 2, lst_activators_clics_2[0]);

                    /// 4.3.- Reintento 1
                    List<EMAIL_ACTIVADORES> lst_activators_clics_3 = lst_activators_clics.Where(ea => ea.reintento == 3).ToList();
                    if (lst_activators_clics_3.Count == 1)
                        load_clics(newsletter, 3, lst_activators_clics_3[0]);
                }
            }

            /// 5.- Poner los botones de copiar datos
            btn_copy_open.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1)}");
            btn_copy_open2.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2)}");
            btn_copy_open3.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3)}");

            btn_copy_no_clic.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_open + ", 1)}");
            btn_copy_no_clic2.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_open + ", 2)}");
            btn_copy_no_clic3.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_open + ", 3)}");

            btn_copy_clic.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1)}");
            btn_copy_clic2.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2)}");
            btn_copy_clic3.Attributes.Add("onclick", "if(confirm_message('Va a copiar los datos de la campaña " + newsletter.nombre + "')){copiar_datos(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3)}");

            /// 6.- Poner el botón de planificar los envíos
            btn_envio_no_open.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1)}");
            btn_envio_no_open2.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2)}");
            btn_envio_no_open3.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3)}");

            btn_envio_no_clic.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1)}");
            btn_envio_no_clic2.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2)}");
            btn_envio_no_clic3.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3)}");

            btn_envio_clic.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1)}");
            btn_envio_clic2.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2)}");
            btn_envio_clic3.Attributes.Add("onclick", "if(confirm_message('Va a planificar el envío de la campaña " + newsletter.nombre + "')){planificar_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3)}");
        }                

        private void load_no_opens(EMAIL_CAMPAIGNS newsletter, int reintento, EMAIL_ACTIVADORES activador)
        {
            if (reintento == 1)
            {
                card_no_open_1.InnerHtml = "<h4 class='text-color-primary'>Reintento 1  <i class='far fa-check-square'></i></h4>";

                #region Reintento 1

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_open_1.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_open_1.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_open_1.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_open.Value = activador.nombreFrom;
                txt_mail_from_open.Value = activador.mailFrom;
                txt_reply_to_open.Value = activador.replyTo;
                txt_asunto_open.Value = activador.asunto;
                txt_cuerpo_open_1.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_open.Attributes.Add("class", "hidden");
                                lnkAdjunto1_open.Visible = true;
                                lnkAdjunto1_open.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_open.InnerText = ajunto_clean;
                                blk_del_1_open.Attributes["class"] = blk_del_1_open.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_open.Attributes.Add("class", "hidden");
                                lnkAdjunto2_open.Visible = true;
                                lnkAdjunto2_open.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_open.InnerText = ajunto_clean;
                                blk_del_2_open.Attributes["class"] = blk_del_2_open.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_open.Attributes.Add("class", "hidden");
                                lnkAdjunto3_open.Visible = true;
                                lnkAdjunto3_open.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_open.InnerText = ajunto_clean;
                                blk_del_3_open.Attributes["class"] = blk_del_3_open.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_open.Attributes.Add("class", "hidden");
                                lnkAdjunto4_open.Visible = true;
                                lnkAdjunto4_open.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_open.InnerText = ajunto_clean;
                                blk_del_4_open.Attributes["class"] = blk_del_4_open.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 4)");
                            }
                            else
                            {
                                fuAdjunto5_open.Attributes.Add("class", "hidden");
                                lnkAdjunto5_open.Visible = true;
                                lnkAdjunto5_open.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_open.InnerText = ajunto_clean;
                                blk_del_5_open.Attributes["class"] = blk_del_5_open.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
            else if (reintento == 2)
            {
                card_no_open_2.InnerHtml = "<h4 class='text-color-primary'>Reintento 2  <i class='far fa-check-square'></i></h4>";

                #region Reintento 2

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_open_2.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_open_2.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_open_2.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_open2.Value = activador.nombreFrom;
                txt_mail_from_open2.Value = activador.mailFrom;
                txt_reply_to_open2.Value = activador.replyTo;
                txt_asunto_open2.Value = activador.asunto;
                txt_cuerpo_open_2.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_open2.Attributes.Add("class", "hidden");
                                lnkAdjunto1_open2.Visible = true;
                                lnkAdjunto1_open2.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_open2.InnerText = ajunto_clean;
                                blk_del_1_open2.Attributes["class"] = blk_del_1_open2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_open2.Attributes.Add("class", "hidden");
                                lnkAdjunto2_open2.Visible = true;
                                lnkAdjunto2_open2.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_open2.InnerText = ajunto_clean;
                                blk_del_2_open2.Attributes["class"] = blk_del_2_open2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_open2.Attributes.Add("class", "hidden");
                                lnkAdjunto3_open2.Visible = true;
                                lnkAdjunto3_open2.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_open2.InnerText = ajunto_clean;
                                blk_del_3_open2.Attributes["class"] = blk_del_3_open2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_open2.Attributes.Add("class", "hidden");
                                lnkAdjunto4_open2.Visible = true;
                                lnkAdjunto4_open2.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_open2.InnerText = ajunto_clean;
                                blk_del_4_open2.Attributes["class"] = blk_del_4_open2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 4)");
                            }
                            else
                            {
                                fuAdjunto5_open2.Attributes.Add("class", "hidden");
                                lnkAdjunto5_open2.Visible = true;
                                lnkAdjunto5_open2.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_open2.InnerText = ajunto_clean;
                                blk_del_5_open2.Attributes["class"] = blk_del_5_open2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
            else if (reintento == 3)
            {
                card_no_open_3.InnerHtml = "<h4 class='text-color-primary'>Reintento 3  <i class='far fa-check-square'></i></h4>";

                #region Reintento 3

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_open_3.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_open_3.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_open_3.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_open3.Value = activador.nombreFrom;
                txt_mail_from_open3.Value = activador.mailFrom;
                txt_reply_to_open3.Value = activador.replyTo;
                txt_asunto_open3.Value = activador.asunto;
                txt_cuerpo_open_3.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_open3.Attributes.Add("class", "hidden");
                                lnkAdjunto1_open3.Visible = true;
                                lnkAdjunto1_open3.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_open3.InnerText = ajunto_clean;
                                blk_del_1_open3.Attributes["class"] = blk_del_1_open3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_open3.Attributes.Add("class", "hidden");
                                lnkAdjunto2_open3.Visible = true;
                                lnkAdjunto2_open3.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_open3.InnerText = ajunto_clean;
                                blk_del_2_open3.Attributes["class"] = blk_del_2_open3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_open3.Attributes.Add("class", "hidden");
                                lnkAdjunto3_open3.Visible = true;
                                lnkAdjunto3_open3.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_open3.InnerText = ajunto_clean;
                                blk_del_3_open3.Attributes["class"] = blk_del_3_open3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_open3.Attributes.Add("class", "hidden");
                                lnkAdjunto4_open3.Visible = true;
                                lnkAdjunto4_open3.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_open3.InnerText = ajunto_clean;
                                blk_del_4_open3.Attributes["class"] = blk_del_4_open3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 4)");
                            }
                            else
                            {
                                fuAdjunto5_open3.Attributes.Add("class", "hidden");
                                lnkAdjunto5_open3.Visible = true;
                                lnkAdjunto5_open3.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_open3.InnerText = ajunto_clean;
                                blk_del_5_open3.Attributes["class"] = blk_del_5_open3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
        }

        private void load_open(EMAIL_CAMPAIGNS newsletter, int reintento, EMAIL_ACTIVADORES activador)
        {
            if (reintento == 1)
            {
                card_no_clic_1.InnerHtml = "<h4 class='text-color-primary'>Reintento 1  <i class='far fa-check-square'></i></h4>";

                #region Reintento 1

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_no_clic_1.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_no_clic_1.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_no_clic_1.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_no_clic.Value = activador.nombreFrom;
                txt_mail_from_no_clic.Value = activador.mailFrom;
                txt_reply_to_no_clic.Value = activador.replyTo;
                txt_asunto_no_clic.Value = activador.asunto;
                txt_cuerpo_no_clic_1.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_no_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto1_no_clic.Visible = true;
                                lnkAdjunto1_no_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_no_clic.InnerText = ajunto_clean;
                                blk_del_1_no_clic.Attributes["class"] = blk_del_1_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_no_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto2_no_clic.Visible = true;
                                lnkAdjunto2_no_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_no_clic.InnerText = ajunto_clean;
                                blk_del_2_no_clic.Attributes["class"] = blk_del_2_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_no_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto3_no_clic.Visible = true;
                                lnkAdjunto3_no_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_no_clic.InnerText = ajunto_clean;
                                blk_del_3_no_clic.Attributes["class"] = blk_del_3_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_no_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto4_no_clic.Visible = true;
                                lnkAdjunto4_no_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_no_clic.InnerText = ajunto_clean;
                                blk_del_4_no_clic.Attributes["class"] = blk_del_4_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 4)");
                            }
                            else
                            {
                                fuAdjunto5_no_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto5_no_clic.Visible = true;
                                lnkAdjunto5_no_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_no_clic.InnerText = ajunto_clean;
                                blk_del_5_no_clic.Attributes["class"] = blk_del_5_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
            else if (reintento == 2)
            {
                card_no_clic_2.InnerHtml = "<h4 class='text-color-primary'>Reintento 2  <i class='far fa-check-square'></i></h4>";

                #region Reintento 2

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_no_clic_2.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_no_clic_2.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_no_clic_2.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_no_clic2.Value = activador.nombreFrom;
                txt_mail_from_no_clic2.Value = activador.mailFrom;
                txt_reply_to_no_clic2.Value = activador.replyTo;
                txt_asunto_no_clic2.Value = activador.asunto;
                txt_cuerpo_no_clic_2.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_no_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto1_no_clic2.Visible = true;
                                lnkAdjunto1_no_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_no_clic2.InnerText = ajunto_clean;
                                blk_del_1_no_clic2.Attributes["class"] = blk_del_1_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_no_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto2_no_clic2.Visible = true;
                                lnkAdjunto2_no_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_no_clic2.InnerText = ajunto_clean;
                                blk_del_2_no_clic2.Attributes["class"] = blk_del_2_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_no_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto3_no_clic2.Visible = true;
                                lnkAdjunto3_no_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_no_clic2.InnerText = ajunto_clean;
                                blk_del_3_no_clic2.Attributes["class"] = blk_del_3_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_no_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto4_no_clic2.Visible = true;
                                lnkAdjunto4_no_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_no_clic2.InnerText = ajunto_clean;
                                blk_del_4_no_clic2.Attributes["class"] = blk_del_4_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 4)");
                            }
                            else
                            {
                                fuAdjunto5_no_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto5_no_clic2.Visible = true;
                                lnkAdjunto5_no_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_no_clic2.InnerText = ajunto_clean;
                                blk_del_5_no_clic2.Attributes["class"] = blk_del_5_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
            else if (reintento == 3)
            {
                card_no_clic_3.InnerHtml = "<h4 class='text-color-primary'>Reintento 3  <i class='far fa-check-square'></i></h4>";

                #region Reintento 3

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_no_clic_3.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_no_clic_3.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_no_clic_3.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_no_clic3.Value = activador.nombreFrom;
                txt_mail_from_no_clic3.Value = activador.mailFrom;
                txt_reply_to_no_clic3.Value = activador.replyTo;
                txt_asunto_no_clic3.Value = activador.asunto;
                txt_cuerpo_no_clic_3.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_no_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto1_no_clic3.Visible = true;
                                lnkAdjunto1_no_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_no_clic3.InnerText = ajunto_clean;
                                blk_del_1_no_clic3.Attributes["class"] = blk_del_1_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_no_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto2_no_clic3.Visible = true;
                                lnkAdjunto2_no_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_no_clic3.InnerText = ajunto_clean;
                                blk_del_2_no_clic3.Attributes["class"] = blk_del_2_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_no_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto3_no_clic3.Visible = true;
                                lnkAdjunto3_no_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_no_clic3.InnerText = ajunto_clean;
                                blk_del_3_no_clic3.Attributes["class"] = blk_del_3_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_no_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto4_no_clic3.Visible = true;
                                lnkAdjunto4_no_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_no_clic3.InnerText = ajunto_clean;
                                blk_del_4_no_clic3.Attributes["class"] = blk_del_4_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 4)");
                            }
                            else
                            {
                                fuAdjunto5_no_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto5_no_clic3.Visible = true;
                                lnkAdjunto5_no_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_no_clic3.InnerText = ajunto_clean;
                                blk_del_5_no_clic3.Attributes["class"] = blk_del_5_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
        }

        private void load_clics(EMAIL_CAMPAIGNS newsletter, int reintento, EMAIL_ACTIVADORES activador)
        {
            if (reintento == 1)
            {
                card_clic_1.InnerHtml = "<h4 class='text-color-primary'>Reintento 1  <i class='far fa-check-square'></i></h4>";

                #region Reintento 1

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_clic_1.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_clic_1.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_clic_1.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_clic.Value = activador.nombreFrom;
                txt_mail_from_clic.Value = activador.mailFrom;
                txt_reply_to_clic.Value = activador.replyTo;
                txt_asunto_clic.Value = activador.asunto;
                txt_cuerpo_clic_1.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto1_clic.Visible = true;
                                lnkAdjunto1_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_clic.InnerText = ajunto_clean;
                                blk_del_1_clic.Attributes["class"] = blk_del_1_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto2_clic.Visible = true;
                                lnkAdjunto2_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_clic.InnerText = ajunto_clean;
                                blk_del_2_clic.Attributes["class"] = blk_del_2_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto3_clic.Visible = true;
                                lnkAdjunto3_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_clic.InnerText = ajunto_clean;
                                blk_del_3_clic.Attributes["class"] = blk_del_3_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto4_clic.Visible = true;
                                lnkAdjunto4_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_clic.InnerText = ajunto_clean;
                                blk_del_4_clic.Attributes["class"] = blk_del_4_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 4)");
                            }
                            else
                            {
                                fuAdjunto5_clic.Attributes.Add("class", "hidden");
                                lnkAdjunto5_clic.Visible = true;
                                lnkAdjunto5_clic.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_clic.InnerText = ajunto_clean;
                                blk_del_5_clic.Attributes["class"] = blk_del_5_clic.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
            else if (reintento == 2)
            {
                card_clic_2.InnerHtml = "<h4 class='text-color-primary'>Reintento 2  <i class='far fa-check-square'></i></h4>";

                #region Reintento 2

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_clic_2.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_clic_2.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_clic_2.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_clic2.Value = activador.nombreFrom;
                txt_mail_from_clic2.Value = activador.mailFrom;
                txt_reply_to_clic2.Value = activador.replyTo;
                txt_asunto_clic2.Value = activador.asunto;
                txt_cuerpo_clic_2.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto1_clic2.Visible = true;
                                lnkAdjunto1_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_clic2.InnerText = ajunto_clean;
                                blk_del_1_clic2.Attributes["class"] = blk_del_1_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto2_clic2.Visible = true;
                                lnkAdjunto2_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_clic2.InnerText = ajunto_clean;
                                blk_del_2_clic2.Attributes["class"] = blk_del_2_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto3_clic2.Visible = true;
                                lnkAdjunto3_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_clic2.InnerText = ajunto_clean;
                                blk_del_3_clic2.Attributes["class"] = blk_del_3_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto4_clic2.Visible = true;
                                lnkAdjunto4_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_clic2.InnerText = ajunto_clean;
                                blk_del_4_clic2.Attributes["class"] = blk_del_4_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 4)");
                            }
                            else
                            {
                                fuAdjunto5_clic2.Attributes.Add("class", "hidden");
                                lnkAdjunto5_clic2.Visible = true;
                                lnkAdjunto5_clic2.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_clic2.InnerText = ajunto_clean;
                                blk_del_5_clic2.Attributes["class"] = blk_del_5_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
            else if (reintento == 3)
            {
                card_clic_3.InnerHtml = "<h4 class='text-color-primary'>Reintento 3  <i class='far fa-check-square'></i></h4>";

                #region Reintento 3

                /// 3.1.0.- Pintar los datos
                txt_fecha_envio_clic_3.Value = activador.fecha_act.ToShortDateString();
                ddl_hora_clic_3.SelectedValue = activador.fecha_act.Hour.ToString("00");
                ddl_minutos_clic_3.SelectedValue = activador.fecha_act.Minute.ToString("00");

                /// 3.1.1.- Cargar datos del mail
                txt_nombre_from_clic3.Value = activador.nombreFrom;
                txt_mail_from_clic3.Value = activador.mailFrom;
                txt_reply_to_clic3.Value = activador.replyTo;
                txt_asunto_clic3.Value = activador.asunto;
                txt_cuerpo_clic_3.Value = activador.body;

                /// 3.1.2.- Cargar los adjuntos del mail
                string mail_adjuntos = !String.IsNullOrEmpty(activador.adjuntos) ? activador.adjuntos : string.Empty;
                if (!String.IsNullOrEmpty(mail_adjuntos))
                {
                    string[] list_adjuntos = mail_adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        int index = 1;
                        string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + activador.id_camp + "\\";
                        string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + activador.id_camp + "/";

                        foreach (string _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                            if (index == 1)
                            {
                                fuAdjunto1_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto1_clic3.Visible = true;
                                lnkAdjunto1_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto1_clic3.InnerText = ajunto_clean;
                                blk_del_1_clic3.Attributes["class"] = blk_del_1_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_1_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 1)");
                            }
                            else if (index == 2)
                            {
                                fuAdjunto2_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto2_clic3.Visible = true;
                                lnkAdjunto2_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto2_clic3.InnerText = ajunto_clean;
                                blk_del_2_clic3.Attributes["class"] = blk_del_2_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_2_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 2)");
                            }
                            else if (index == 3)
                            {
                                fuAdjunto3_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto3_clic3.Visible = true;
                                lnkAdjunto3_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto3_clic3.InnerText = ajunto_clean;
                                blk_del_3_clic3.Attributes["class"] = blk_del_3_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_3_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 3)");
                            }
                            else if (index == 4)
                            {
                                fuAdjunto4_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto4_clic3.Visible = true;
                                lnkAdjunto4_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto4_clic3.InnerText = ajunto_clean;
                                blk_del_4_clic3.Attributes["class"] = blk_del_4_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_4_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 4)");
                            }
                            else
                            {
                                fuAdjunto5_clic3.Attributes.Add("class", "hidden");
                                lnkAdjunto5_clic3.Visible = true;
                                lnkAdjunto5_clic3.HRef = route_see + ajunto_clean;
                                lnkAdjunto5_clic3.InnerText = ajunto_clean;
                                blk_del_5_clic3.Attributes["class"] = blk_del_5_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                blk_del_5_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 5)");
                            }
                            index++;
                        }
                    }
                }

                #endregion
            }
        }

        protected void btnCopiar_Datos_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar datos de la campaña
            long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            int tipo = !String.IsNullOrEmpty(hid_tipo.Value) ? int.Parse(hid_tipo.Value) : -1;
            int reintento = !String.IsNullOrEmpty(hid_reintento.Value) ? int.Parse(hid_reintento.Value) : -1;

            if (id_newsletter > 0 && tipo > 0 && reintento > 0)
            {
                /// 3.- Sacar los datos de la campaña
                List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                if (lst_newsletter.Count == 1)
                {                    
                    if (tipo == (int)Constantes.status_campaign.Reenvio_no_opens)
                    {
                        if (reintento == 1)
                        {
                            /// 0.- Abrir la pestaña
                            card_no_open_1.Attributes["class"] = card_no_open_1.Attributes["class"].Replace("collapsed", "");
                            card_no_open_1.Attributes["aria-expanded"] = card_no_open_1.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_no_opens_1.Attributes["class"] = collapse_no_opens_1.Attributes["class"].Insert(collapse_no_opens_1.Attributes["class"].Length, " show");
                            
                            #region Reintento 1

                            /// 3.1.1.- Cargar datos del mail
                            txt_nombre_from_open.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_open.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_open.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_open.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_open_1.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.1.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            //fuAdjunto1_open.Visible = false;
                                            fuAdjunto1_open.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_open.Visible = true;
                                            lnkAdjunto1_open.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_open.InnerText = ajunto_clean;
                                            blk_del_1_open.Attributes["class"] = blk_del_1_open.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            //fuAdjunto2_open.Visible = false;
                                            fuAdjunto2_open.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_open.Visible = true;
                                            lnkAdjunto2_open.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_open.InnerText = ajunto_clean;
                                            blk_del_2_open.Attributes["class"] = blk_del_2_open.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            //fuAdjunto3_open.Visible = false;
                                            fuAdjunto3_open.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_open.Visible = true;
                                            lnkAdjunto3_open.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_open.InnerText = ajunto_clean;
                                            blk_del_3_open.Attributes["class"] = blk_del_3_open.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            //fuAdjunto4_open.Visible = false;
                                            fuAdjunto4_open.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_open.Visible = true;
                                            lnkAdjunto4_open.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_open.InnerText = ajunto_clean;
                                            blk_del_4_open.Attributes["class"] = blk_del_4_open.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 4)");
                                        }
                                        else
                                        {
                                            //fuAdjunto5_open.Visible = false;
                                            fuAdjunto5_open.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_open.Visible = true;
                                            lnkAdjunto5_open.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_open.InnerText = ajunto_clean;
                                            blk_del_5_open.Attributes["class"] = blk_del_5_open.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_open.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 1, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 2)
                        {
                            /// 0.- Abrir la pestaña
                            card_no_open_2.Attributes["class"] = card_no_open_2.Attributes["class"].Replace("collapsed", "");
                            card_no_open_2.Attributes["aria-expanded"] = card_no_open_2.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_no_opens_2.Attributes["class"] = collapse_no_opens_2.Attributes["class"].Insert(collapse_no_opens_2.Attributes["class"].Length, " show");

                            #region Reintento 2

                            /// 3.1.1.- Cargar datos del mail
                            txt_nombre_from_open2.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_open2.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_open2.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_open2.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_open_2.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.1.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_open2.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_open2.Visible = true;
                                            lnkAdjunto1_open2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_open2.InnerText = ajunto_clean;
                                            blk_del_1_open2.Attributes["class"] = blk_del_1_open2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_open2.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_open2.Visible = true;
                                            lnkAdjunto2_open2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_open2.InnerText = ajunto_clean;
                                            blk_del_2_open2.Attributes["class"] = blk_del_2_open2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_open2.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_open2.Visible = true;
                                            lnkAdjunto3_open2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_open2.InnerText = ajunto_clean;
                                            blk_del_3_open2.Attributes["class"] = blk_del_3_open2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_open2.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_open2.Visible = true;
                                            lnkAdjunto4_open2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_open2.InnerText = ajunto_clean;
                                            blk_del_4_open2.Attributes["class"] = blk_del_4_open2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_open2.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_open2.Visible = true;
                                            lnkAdjunto5_open2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_open2.InnerText = ajunto_clean;
                                            blk_del_5_open2.Attributes["class"] = blk_del_5_open2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_open2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 2, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 3)
                        {
                            /// 0.- Abrir la pestaña
                            card_no_open_3.Attributes["class"] = card_no_open_3.Attributes["class"].Replace("collapsed", "");
                            card_no_open_3.Attributes["aria-expanded"] = card_no_open_3.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_no_opens_3.Attributes["class"] = collapse_no_opens_3.Attributes["class"].Insert(collapse_no_opens_3.Attributes["class"].Length, " show");

                            #region Reintento 3

                            /// 3.1.1.- Cargar datos del mail
                            txt_nombre_from_open3.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_open3.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_open3.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_open3.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_open_3.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.1.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_open3.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_open3.Visible = true;
                                            lnkAdjunto1_open3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_open3.InnerText = ajunto_clean;
                                            blk_del_1_open3.Attributes["class"] = blk_del_1_open3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_open3.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_open3.Visible = true;
                                            lnkAdjunto2_open3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_open3.InnerText = ajunto_clean;
                                            blk_del_2_open3.Attributes["class"] = blk_del_2_open3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_open3.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_open3.Visible = true;
                                            lnkAdjunto3_open3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_open3.InnerText = ajunto_clean;
                                            blk_del_3_open3.Attributes["class"] = blk_del_3_open3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_open3.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_open3.Visible = true;
                                            lnkAdjunto4_open3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_open3.InnerText = ajunto_clean;
                                            blk_del_4_open3.Attributes["class"] = blk_del_4_open3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_open3.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_open3.Visible = true;
                                            lnkAdjunto5_open3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_open3.InnerText = ajunto_clean;
                                            blk_del_5_open3.Attributes["class"] = blk_del_5_open3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_open3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_no_opens + ", 3, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    else if (tipo == (int)Constantes.status_campaign.Reenvio_open)
                    {
                        if (reintento == 1)
                        {
                            /// 0.- Abrir la pestaña
                            card_no_clic_1.Attributes["class"] = card_no_clic_1.Attributes["class"].Replace("collapsed", "");
                            card_no_clic_1.Attributes["aria-expanded"] = card_no_clic_1.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_no_clics_1.Attributes["class"] = collapse_no_clics_1.Attributes["class"].Insert(collapse_no_clics_1.Attributes["class"].Length, " show");

                            #region Reintento 1

                            /// 3.2.1.- Cargar datos del mail
                            txt_nombre_from_no_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_no_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_no_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_no_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_no_clic_1.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.2.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_no_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_no_clic.Visible = true;
                                            lnkAdjunto1_no_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_no_clic.InnerText = ajunto_clean;
                                            blk_del_1_no_clic.Attributes["class"] = blk_del_1_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_no_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_no_clic.Visible = true;
                                            lnkAdjunto2_no_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_no_clic.InnerText = ajunto_clean;
                                            blk_del_2_no_clic.Attributes["class"] = blk_del_2_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_no_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_no_clic.Visible = true;
                                            lnkAdjunto3_no_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_no_clic.InnerText = ajunto_clean;
                                            blk_del_3_no_clic.Attributes["class"] = blk_del_3_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_no_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_no_clic.Visible = true;
                                            lnkAdjunto4_no_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_no_clic.InnerText = ajunto_clean;
                                            blk_del_4_no_clic.Attributes["class"] = blk_del_4_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_no_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_no_clic.Visible = true;
                                            lnkAdjunto5_no_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_no_clic.InnerText = ajunto_clean;
                                            blk_del_5_no_clic.Attributes["class"] = blk_del_5_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_no_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 1, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 2)
                        {
                            /// 0.- Abrir la pestaña
                            card_no_clic_2.Attributes["class"] = card_no_clic_2.Attributes["class"].Replace("collapsed", "");
                            card_no_clic_2.Attributes["aria-expanded"] = card_no_clic_2.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_no_clics_2.Attributes["class"] = collapse_no_clics_2.Attributes["class"].Insert(collapse_no_clics_2.Attributes["class"].Length, " show");

                            #region Reintento 2

                            /// 3.2.1.- Cargar datos del mail
                            txt_nombre_from_no_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_no_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_no_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_no_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_no_clic_2.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.2.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_no_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_no_clic2.Visible = true;
                                            lnkAdjunto1_no_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_no_clic2.InnerText = ajunto_clean;
                                            blk_del_1_no_clic2.Attributes["class"] = blk_del_1_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_no_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_no_clic2.Visible = true;
                                            lnkAdjunto2_no_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_no_clic2.InnerText = ajunto_clean;
                                            blk_del_2_no_clic2.Attributes["class"] = blk_del_2_no_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_no_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_no_clic2.Visible = true;
                                            lnkAdjunto3_no_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_no_clic2.InnerText = ajunto_clean;
                                            blk_del_3_no_clic2.Attributes["class"] = blk_del_3_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_no_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_no_clic2.Visible = true;
                                            lnkAdjunto4_no_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_no_clic2.InnerText = ajunto_clean;
                                            blk_del_4_no_clic2.Attributes["class"] = blk_del_4_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_no_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_no_clic2.Visible = true;
                                            lnkAdjunto5_no_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_no_clic2.InnerText = ajunto_clean;
                                            blk_del_5_no_clic2.Attributes["class"] = blk_del_5_no_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_no_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 2, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 3)
                        {
                            /// 0.- Abrir la pestaña
                            card_no_clic_3.Attributes["class"] = card_no_clic_3.Attributes["class"].Replace("collapsed", "");
                            card_no_clic_3.Attributes["aria-expanded"] = card_no_clic_3.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_no_clics_3.Attributes["class"] = collapse_no_clics_3.Attributes["class"].Insert(collapse_no_clics_3.Attributes["class"].Length, " show");

                            #region Reintento 3

                            /// 3.2.1.- Cargar datos del mail
                            txt_nombre_from_no_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_no_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_no_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_no_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_no_clic_3.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.2.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_no_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_no_clic3.Visible = true;
                                            lnkAdjunto1_no_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_no_clic3.InnerText = ajunto_clean;
                                            blk_del_1_no_clic3.Attributes["class"] = blk_del_1_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_no_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_no_clic3.Visible = true;
                                            lnkAdjunto2_no_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_no_clic3.InnerText = ajunto_clean;
                                            blk_del_2_no_clic3.Attributes["class"] = blk_del_2_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_no_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_no_clic3.Visible = true;
                                            lnkAdjunto3_no_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_no_clic3.InnerText = ajunto_clean;
                                            blk_del_3_no_clic3.Attributes["class"] = blk_del_3_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_no_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_no_clic3.Visible = true;
                                            lnkAdjunto4_no_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_no_clic3.InnerText = ajunto_clean;
                                            blk_del_4_no_clic3.Attributes["class"] = blk_del_4_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_no_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_no_clic3.Visible = true;
                                            lnkAdjunto5_no_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_no_clic3.InnerText = ajunto_clean;
                                            blk_del_5_no_clic3.Attributes["class"] = blk_del_5_no_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_no_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_open + ", 3, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    else if (tipo == (int)Constantes.status_campaign.Reenvio_clicks)
                    {
                        if (reintento == 1)
                        {
                            /// 0.- Abrir la pestaña
                            card_clic_1.Attributes["class"] = card_clic_1.Attributes["class"].Replace("collapsed", "");
                            card_clic_1.Attributes["aria-expanded"] = card_clic_1.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_clics_1.Attributes["class"] = collapse_clics_1.Attributes["class"].Insert(collapse_clics_1.Attributes["class"].Length, " show");

                            #region Reintento 1

                            /// 3.1.1.- Cargar datos del mail
                            txt_nombre_from_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_clic.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_clic_1.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.1.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_clic.Visible = true;
                                            lnkAdjunto1_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_clic.InnerText = ajunto_clean;
                                            blk_del_1_clic.Attributes["class"] = blk_del_1_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_clic.Visible = true;
                                            lnkAdjunto2_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_clic.InnerText = ajunto_clean;
                                            blk_del_2_clic.Attributes["class"] = blk_del_2_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_clic.Visible = true;
                                            lnkAdjunto3_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_clic.InnerText = ajunto_clean;
                                            blk_del_3_clic.Attributes["class"] = blk_del_3_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_clic.Visible = true;
                                            lnkAdjunto4_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_clic.InnerText = ajunto_clean;
                                            blk_del_4_clic.Attributes["class"] = blk_del_4_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_clic.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_clic.Visible = true;
                                            lnkAdjunto5_clic.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_clic.InnerText = ajunto_clean;
                                            blk_del_5_clic.Attributes["class"] = blk_del_5_clic.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_clic.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 1, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 2)
                        {
                            /// 0.- Abrir la pestaña
                            card_clic_2.Attributes["class"] = card_clic_2.Attributes["class"].Replace("collapsed", "");
                            card_clic_2.Attributes["aria-expanded"] = card_clic_2.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_clics_2.Attributes["class"] = collapse_clics_2.Attributes["class"].Insert(collapse_clics_2.Attributes["class"].Length, " show");

                            #region Reintento 2

                            /// 3.1.1.- Cargar datos del mail
                            txt_nombre_from_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_clic2.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_clic_2.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.1.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_clic2.Visible = true;
                                            lnkAdjunto1_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_clic2.InnerText = ajunto_clean;
                                            blk_del_1_clic2.Attributes["class"] = blk_del_1_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_clic2.Visible = true;
                                            lnkAdjunto2_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_clic2.InnerText = ajunto_clean;
                                            blk_del_2_clic2.Attributes["class"] = blk_del_2_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_clic2.Visible = true;
                                            lnkAdjunto3_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_clic2.InnerText = ajunto_clean;
                                            blk_del_3_clic2.Attributes["class"] = blk_del_3_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_clic2.Visible = true;
                                            lnkAdjunto4_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_clic2.InnerText = ajunto_clean;
                                            blk_del_4_clic2.Attributes["class"] = blk_del_4_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_clic2.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_clic2.Visible = true;
                                            lnkAdjunto5_clic2.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_clic2.InnerText = ajunto_clean;
                                            blk_del_5_clic2.Attributes["class"] = blk_del_5_clic2.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_clic2.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 2, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 3)
                        {
                            /// 0.- Abrir la pestaña
                            card_clic_3.Attributes["class"] = card_clic_3.Attributes["class"].Replace("collapsed", "");
                            card_clic_3.Attributes["aria-expanded"] = card_clic_3.Attributes["aria-expanded"].Replace("false", "true");
                            collapse_clics_3.Attributes["class"] = collapse_clics_3.Attributes["class"].Insert(collapse_clics_3.Attributes["class"].Length, " show");

                            #region Reintento 3

                            /// 3.1.1.- Cargar datos del mail
                            txt_nombre_from_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].nombreFrom) ? lst_newsletter[0].nombreFrom : string.Empty;
                            txt_mail_from_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].mailFrom) ? lst_newsletter[0].mailFrom : string.Empty;
                            txt_reply_to_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].replyTo) ? lst_newsletter[0].replyTo : string.Empty;
                            txt_asunto_clic3.Value = !String.IsNullOrEmpty(lst_newsletter[0].asunto) ? lst_newsletter[0].asunto : string.Empty;
                            txt_cuerpo_clic_3.Value = !String.IsNullOrEmpty(lst_newsletter[0].body) ? lst_newsletter[0].body : string.Empty;

                            /// 3.1.2.- Cargar los adjuntos del mail
                            string mail_adjuntos = !String.IsNullOrEmpty(lst_newsletter[0].adjuntos) ? lst_newsletter[0].adjuntos : string.Empty;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                string[] list_adjuntos = mail_adjuntos.Split(',');
                                if (list_adjuntos.Length > 0)
                                {
                                    int index = 1;
                                    string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                    string route_see = ConfigurationManager.AppSettings["urlTemplateMailCampaign"] + id_newsletter + "/";

                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                        if (index == 1)
                                        {
                                            fuAdjunto1_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto1_clic3.Visible = true;
                                            lnkAdjunto1_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto1_clic3.InnerText = ajunto_clean;
                                            blk_del_1_clic3.Attributes["class"] = blk_del_1_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_1_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 1)");
                                        }
                                        else if (index == 2)
                                        {
                                            fuAdjunto2_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto2_clic3.Visible = true;
                                            lnkAdjunto2_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto2_clic3.InnerText = ajunto_clean;
                                            blk_del_2_clic3.Attributes["class"] = blk_del_2_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_2_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 2)");
                                        }
                                        else if (index == 3)
                                        {
                                            fuAdjunto3_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto3_clic3.Visible = true;
                                            lnkAdjunto3_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto3_clic3.InnerText = ajunto_clean;
                                            blk_del_3_clic3.Attributes["class"] = blk_del_3_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_3_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 3)");
                                        }
                                        else if (index == 4)
                                        {
                                            fuAdjunto4_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto4_clic3.Visible = true;
                                            lnkAdjunto4_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto4_clic3.InnerText = ajunto_clean;
                                            blk_del_4_clic3.Attributes["class"] = blk_del_4_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_4_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 4)");
                                        }
                                        else
                                        {
                                            fuAdjunto5_clic3.Attributes.Add("class", "hidden");
                                            lnkAdjunto5_clic3.Visible = true;
                                            lnkAdjunto5_clic3.HRef = route_see + ajunto_clean;
                                            lnkAdjunto5_clic3.InnerText = ajunto_clean;
                                            blk_del_5_clic3.Attributes["class"] = blk_del_5_clic3.Attributes["class"].Replace("hidden", string.Empty);
                                            blk_del_5_clic3.Attributes.Add("onclick", "eliminar_adjunto_reenvio(" + (int)Constantes.status_campaign.Reenvio_clicks + ", 3, 5)");
                                        }
                                        index++;
                                    }
                                }
                            }

                            #endregion
                        }
                    }                    
                }
            }
        }

        protected void btn_reenvio_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar datos de la campaña
            long id_newsletter = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            int tipo = !String.IsNullOrEmpty(hid_tipo.Value) ? int.Parse(hid_tipo.Value) : -1;
            int reintento = !String.IsNullOrEmpty(hid_reintento.Value) ? int.Parse(hid_reintento.Value) : -1;

            /// 2.- Borrar los errores
            txt_error.InnerHtml = string.Empty;

            if (id_newsletter > 0 && tipo > 0 && reintento > 0)
            {
                /// 3.- Sacar los datos de la campaña
                List<EMAIL_CAMPAIGNS> lst_newsletter = da.getCampaignsById(id_newsletter);
                if (lst_newsletter.Count == 1)
                {
                    /// 3.0.- Sacar los activadores de la campaña
                    List<EMAIL_ACTIVADORES> lst_activators = da.getActivatorsByIdCampaign(id_newsletter);

                    if (tipo == (int)Constantes.status_campaign.Reenvio_no_opens)
                    {
                        if (reintento == 1)
                        {
                            #region Reintento 1

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_open_1.Value) ? DateTime.Parse(txt_fecha_envio_open_1.Value).AddHours(int.Parse(ddl_hora_open_1.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_open_1.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_no_open_1.Attributes["class"] = card_no_open_1.Attributes["class"].Replace("collapsed", "");
                                card_no_open_1.Attributes["aria-expanded"] = card_no_open_1.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_no_opens_1.Attributes["class"] = collapse_no_opens_1.Attributes["class"].Insert(collapse_no_opens_1.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_open.Value;
                                string mail_from = txt_mail_from_open.Value;
                                string reply_to = txt_reply_to_open.Value;
                                string mail_asunto = txt_asunto_open.Value;
                                string mail_body = txt_cuerpo_open_1.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_open.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_open.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_open.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_open.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_open.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_open.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_open.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_open.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_open.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_open.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_open.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_open.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_open.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_open.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_open.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_open.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_open.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_open.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_open.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_open.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_open.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_open.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_open.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_open.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_open.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_no_opens = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_no_opens.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_no_opens[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 2)
                        {
                            #region Reintento 2

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_open_2.Value) ? DateTime.Parse(txt_fecha_envio_open_2.Value).AddHours(int.Parse(ddl_hora_open_2.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_open_2.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_no_open_2.Attributes["class"] = card_no_open_2.Attributes["class"].Replace("collapsed", "");
                                card_no_open_2.Attributes["aria-expanded"] = card_no_open_2.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_no_opens_2.Attributes["class"] = collapse_no_opens_2.Attributes["class"].Insert(collapse_no_opens_2.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_open2.Value;
                                string mail_from = txt_mail_from_open2.Value;
                                string reply_to = txt_reply_to_open2.Value;
                                string mail_asunto = txt_asunto_open2.Value;
                                string mail_body = txt_cuerpo_open_2.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_open2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_open2.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_open2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_open2.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_open2.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_open2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_open2.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_open2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_open2.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_open2.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_open2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_open2.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_open2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_open2.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_open2.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_open2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_open2.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_open2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_open2.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_open2.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_open2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_open2.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_open2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_open2.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_open2.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_no_opens = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_no_opens.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_no_opens[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 3)
                        {
                            #region Reintento 3

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_open_3.Value) ? DateTime.Parse(txt_fecha_envio_open_3.Value).AddHours(int.Parse(ddl_hora_open_3.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_open_3.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_no_open_3.Attributes["class"] = card_no_open_3.Attributes["class"].Replace("collapsed", "");
                                card_no_open_3.Attributes["aria-expanded"] = card_no_open_3.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_no_opens_3.Attributes["class"] = collapse_no_opens_3.Attributes["class"].Insert(collapse_no_opens_3.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_open3.Value;
                                string mail_from = txt_mail_from_open3.Value;
                                string reply_to = txt_reply_to_open3.Value;
                                string mail_asunto = txt_asunto_open3.Value;
                                string mail_body = txt_cuerpo_open_3.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_open3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_open3.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_open3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_open3.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_open3.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_open3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_open3.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_open3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_open3.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_open3.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_open3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_open3.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_open3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_open3.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_open3.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_open3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_open3.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_open3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_open3.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_open3.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_open3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_open3.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_open3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_open3.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_open3.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_no_opens = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_no_opens.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_no_opens[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    else if (tipo == (int)Constantes.status_campaign.Reenvio_open)
                    {
                        if (reintento == 1)
                        {
                            #region Reintento 1

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_no_clic_1.Value) ? DateTime.Parse(txt_fecha_envio_no_clic_1.Value).AddHours(int.Parse(ddl_hora_no_clic_1.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_no_clic_1.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_no_clic_1.Attributes["class"] = card_no_clic_1.Attributes["class"].Replace("collapsed", "");
                                card_no_clic_1.Attributes["aria-expanded"] = card_no_clic_1.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_no_clics_1.Attributes["class"] = collapse_no_clics_1.Attributes["class"].Insert(collapse_no_clics_1.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_no_clic.Value;
                                string mail_from = txt_mail_from_no_clic.Value;
                                string reply_to = txt_reply_to_no_clic.Value;
                                string mail_asunto = txt_asunto_no_clic.Value;
                                string mail_body = txt_cuerpo_no_clic_1.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_no_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_no_clic.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_no_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_no_clic.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_no_clic.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_no_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_no_clic.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_no_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_no_clic.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_no_clic.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_no_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_no_clic.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_no_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_no_clic.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_no_clic.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_no_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_no_clic.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_no_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_no_clic.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_no_clic.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_no_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_no_clic.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_no_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_no_clic.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_no_clic.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_no_clics = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_no_clics.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_no_clics[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 2)
                        {
                            #region Reintento 2

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_no_clic_2.Value) ? DateTime.Parse(txt_fecha_envio_no_clic_2.Value).AddHours(int.Parse(ddl_hora_no_clic_2.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_no_clic_2.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_no_clic_2.Attributes["class"] = card_no_clic_2.Attributes["class"].Replace("collapsed", "");
                                card_no_clic_2.Attributes["aria-expanded"] = card_no_clic_2.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_no_clics_2.Attributes["class"] = collapse_no_clics_2.Attributes["class"].Insert(collapse_no_clics_2.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_no_clic2.Value;
                                string mail_from = txt_mail_from_no_clic2.Value;
                                string reply_to = txt_reply_to_no_clic2.Value;
                                string mail_asunto = txt_asunto_no_clic2.Value;
                                string mail_body = txt_cuerpo_no_clic_2.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_no_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_no_clic2.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_no_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_no_clic2.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_no_clic2.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_no_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_no_clic2.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_no_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_no_clic2.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_no_clic2.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_no_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_no_clic2.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_no_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_no_clic2.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_no_clic2.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_no_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_no_clic2.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_no_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_no_clic2.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_no_clic2.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_no_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_no_clic2.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_no_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_no_clic2.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_no_clic2.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_no_clics = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_no_clics.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_no_clics[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 3)
                        {
                            #region Reintento 3

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_no_clic_3.Value) ? DateTime.Parse(txt_fecha_envio_no_clic_3.Value).AddHours(int.Parse(ddl_hora_no_clic_3.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_no_clic_3.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_no_clic_3.Attributes["class"] = card_no_clic_3.Attributes["class"].Replace("collapsed", "");
                                card_no_clic_3.Attributes["aria-expanded"] = card_no_clic_3.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_no_clics_3.Attributes["class"] = collapse_no_clics_3.Attributes["class"].Insert(collapse_no_clics_3.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_no_clic3.Value;
                                string mail_from = txt_mail_from_no_clic3.Value;
                                string reply_to = txt_reply_to_no_clic3.Value;
                                string mail_asunto = txt_asunto_no_clic3.Value;
                                string mail_body = txt_cuerpo_no_clic_3.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_no_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_no_clic3.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_no_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_no_clic3.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_no_clic3.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_no_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_no_clic3.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_no_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_no_clic3.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_no_clic3.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_no_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_no_clic3.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_no_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_no_clic3.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_no_clic3.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_no_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_no_clic3.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_no_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_no_clic3.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_no_clic3.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_no_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_no_clic3.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_no_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_no_clic3.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_no_clic3.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_no_clics = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_no_clics.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_no_clics[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    else if (tipo == (int)Constantes.status_campaign.Reenvio_clicks)
                    {
                        if (reintento == 1)
                        {
                            #region Reintento 1

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_clic_1.Value) ? DateTime.Parse(txt_fecha_envio_clic_1.Value).AddHours(int.Parse(ddl_hora_clic_1.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_clic_1.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_clic_1.Attributes["class"] = card_clic_1.Attributes["class"].Replace("collapsed", "");
                                card_clic_1.Attributes["aria-expanded"] = card_clic_1.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_clics_1.Attributes["class"] = collapse_clics_1.Attributes["class"].Insert(collapse_clics_1.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_clic.Value;
                                string mail_from = txt_mail_from_clic.Value;
                                string reply_to = txt_reply_to_clic.Value;
                                string mail_asunto = txt_asunto_clic.Value;
                                string mail_body = txt_cuerpo_clic_1.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_clic.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_clic.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_clic.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_clic.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_clic.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_clic.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_clic.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_clic.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_clic.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_clic.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_clic.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_clic.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_clic.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_clic.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_clic.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_clic.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_clic.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_clics = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_clics.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_clics[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 2)
                        {
                            #region Reintento 2

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_clic_2.Value) ? DateTime.Parse(txt_fecha_envio_clic_2.Value).AddHours(int.Parse(ddl_hora_clic_2.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_clic_2.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_clic_2.Attributes["class"] = card_clic_2.Attributes["class"].Replace("collapsed", "");
                                card_clic_2.Attributes["aria-expanded"] = card_clic_2.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_clics_2.Attributes["class"] = collapse_clics_2.Attributes["class"].Insert(collapse_clics_2.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_clic2.Value;
                                string mail_from = txt_mail_from_clic2.Value;
                                string reply_to = txt_reply_to_clic2.Value;
                                string mail_asunto = txt_asunto_clic2.Value;
                                string mail_body = txt_cuerpo_clic_2.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_clic2.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_clic2.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_clic2.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_clic2.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_clic2.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_clic2.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_clic2.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_clic2.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_clic2.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_clic2.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_clic2.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_clic2.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_clic2.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_clic2.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_clic2.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_clic2.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_clic2.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_clics = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_clics.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_clics[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (reintento == 3)
                        {
                            #region Reintento 3

                            /// 3.1.0.- Sacar los datos del formulario
                            DateTime fecha_envio = !String.IsNullOrEmpty(txt_fecha_envio_clic_3.Value) ? DateTime.Parse(txt_fecha_envio_clic_3.Value).AddHours(int.Parse(ddl_hora_clic_3.SelectedValue)).AddMinutes(int.Parse(ddl_minutos_clic_3.SelectedValue)) : new DateTime();

                            /// 3.1.2.- Comprobar si hay errores
                            if (fecha_envio == new DateTime())
                            {
                                card_clic_3.Attributes["class"] = card_clic_3.Attributes["class"].Replace("collapsed", "");
                                card_clic_3.Attributes["aria-expanded"] = card_clic_3.Attributes["aria-expanded"].Replace("false", "true");
                                collapse_clics_3.Attributes["class"] = collapse_clics_3.Attributes["class"].Insert(collapse_clics_3.Attributes["class"].Length, " show");

                                txt_error.InnerHtml = "La fecha de envío es obligatoria";
                            }
                            else
                            {
                                /// 3.1.3.- Sacar el resto de parámetros            
                                string name_from = txt_nombre_from_clic3.Value;
                                string mail_from = txt_mail_from_clic3.Value;
                                string reply_to = txt_reply_to_clic3.Value;
                                string mail_asunto = txt_asunto_clic3.Value;
                                string mail_body = txt_cuerpo_clic_3.Value;
                                string adjuntos = string.Empty;
                                bool validate_files = true;

                                #region Adjuntos

                                string route = ConfigurationManager.AppSettings["routeTemplateMailCampaign"] + id_newsletter + "\\";
                                if (!(Directory.Exists(route)))
                                    Directory.CreateDirectory(route);

                                string adjunto1 = string.Empty;
                                string adjunto2 = string.Empty;
                                string adjunto3 = string.Empty;
                                string adjunto4 = string.Empty;
                                string adjunto5 = string.Empty;

                                /// Adjunto 1
                                if (fuAdjunto1_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto1_clic3.PostedFile.FileName);
                                        adjunto1 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto1_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto1_clic3.PostedFile.SaveAs(route + adjunto1);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_1.Value == "1")
                                    adjunto1 = string.Empty;
                                else
                                    adjunto1 = lnkAdjunto1_clic3.InnerText;

                                /// Adjunto 2
                                if (fuAdjunto2_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto2_clic3.PostedFile.FileName);
                                        adjunto2 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto2_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto2_clic3.PostedFile.SaveAs(route + adjunto2);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_2.Value == "1")
                                    adjunto2 = string.Empty;
                                else
                                    adjunto2 = lnkAdjunto2_clic3.InnerText;

                                /// Adjunto 3
                                if (fuAdjunto3_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto3_clic3.PostedFile.FileName);
                                        adjunto3 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto3_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto3_clic3.PostedFile.SaveAs(route + adjunto3);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_3.Value == "1")
                                    adjunto3 = string.Empty;
                                else
                                    adjunto3 = lnkAdjunto3_clic3.InnerText;

                                /// Adjunto 4
                                if (fuAdjunto4_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto4_clic3.PostedFile.FileName);
                                        adjunto4 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto4_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto4_clic3.PostedFile.SaveAs(route + adjunto4);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_4.Value == "1")
                                    adjunto4 = string.Empty;
                                else
                                    adjunto4 = lnkAdjunto4_clic3.InnerText;

                                /// Adjunto 5
                                if (fuAdjunto5_clic3.HasFile)
                                {
                                    if (Directory.Exists(route))
                                    {
                                        FileInfo archivo = new FileInfo(fuAdjunto5_clic3.PostedFile.FileName);
                                        adjunto5 = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + fuAdjunto5_clic3.PostedFile.FileName.Replace(" ", "-");
                                        try
                                        {
                                            fuAdjunto5_clic3.PostedFile.SaveAs(route + adjunto5);
                                        }
                                        catch
                                        {
                                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                                            validate_files = false;
                                        }
                                    }
                                }
                                else if (hid_adj_5.Value == "1")
                                    adjunto5 = string.Empty;
                                else
                                    adjunto5 = lnkAdjunto5_clic3.InnerText;

                                List<string> list_adjuntos = new List<string>();
                                if (!String.IsNullOrEmpty(adjunto1))
                                    list_adjuntos.Add(adjunto1);
                                if (!String.IsNullOrEmpty(adjunto2))
                                    list_adjuntos.Add(adjunto2);
                                if (!String.IsNullOrEmpty(adjunto3))
                                    list_adjuntos.Add(adjunto3);
                                if (!String.IsNullOrEmpty(adjunto4))
                                    list_adjuntos.Add(adjunto4);
                                if (!String.IsNullOrEmpty(adjunto5))
                                    list_adjuntos.Add(adjunto5);

                                int cont = 0;
                                if (list_adjuntos.Count > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (cont == 0)
                                            adjuntos = route + _adjunto;
                                        else
                                            adjuntos += "," + route + _adjunto;
                                        cont++;
                                    }
                                }

                                #endregion                                

                                if (validate_files)
                                {
                                    List<EMAIL_ACTIVADORES> lst_activators_clics = lst_activators.Where(ea => ea.tipo == tipo && ea.reintento == reintento).ToList();
                                    if (lst_activators_clics.Count == 1)
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_activators_clics[0];
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        bool update_activation = da.updateEmailActivator(_activador);
                                        if (update_activation)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al actualizar el activador";
                                    }
                                    else
                                    {
                                        EMAIL_ACTIVADORES _activador = new EMAIL_ACTIVADORES();
                                        _activador.tipo = tipo;
                                        _activador.reintento = reintento;
                                        _activador.id_camp = id_newsletter;
                                        _activador.fecha_act = fecha_envio;
                                        _activador.priority = lst_newsletter[0].priority;
                                        _activador.nombreFrom = name_from;
                                        _activador.mailFrom = mail_from;
                                        _activador.replyTo = reply_to;
                                        _activador.asunto = mail_asunto;
                                        _activador.body = mail_body;
                                        _activador.adjuntos = adjuntos;

                                        long insert_activation = da.insertEmailActivator(_activador);
                                        if (insert_activation > 0)
                                            Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + id_newsletter);
                                        else
                                            txt_error.InnerHtml = "Se ha producido un error al añadir el activador";
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
            }
        }
    }
}