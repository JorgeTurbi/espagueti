using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Campaign_plan_process
{
    class Planificar_newsletter
    {
        DataAccess da = new DataAccess();

        public bool process(EMAIL_CAMPAIGNS campaign)
        {
            bool procesado = true;

            /// 1.- Sacar el cuerpo de la newsletter
            string template_body = campaign.body;

            /// 2.- Sustituir las url por las especiales
            List<string> list_urls = obtener_urls(template_body);
            if (list_urls.Count > 0)
            {
                /// 2.1.- Sacar las urls de la BBDD
                List<EMAIL_CLICKS> lst_mails = da.getEmailClicks();

                /// 2.2.- Recorrer y sustituir las urls
                foreach (var url in list_urls)
                {
                    string _url = url.Replace("&", "&amp;");

                    /// 2.2.1.- Comprobar si existe
                    List<EMAIL_CLICKS> lst_mail_click = lst_mails.Where(ec => ec.url_destino == url).ToList();
                    if (lst_mail_click.Count == 1)
                    {
                        template_body = template_body.Replace(_url, ConfigurationManager.AppSettings["ur_email_click"] + lst_mail_click[0].id_click);
                        template_body = template_body.Replace(url, ConfigurationManager.AppSettings["ur_email_click"] + lst_mail_click[0].id_click);
                    }
                    else if (lst_mail_click.Count == 0)
                    {
                        /// 2.2.2.- Añadir a la BBDD la url nueva
                        EMAIL_CLICKS click_mail = new EMAIL_CLICKS();
                        click_mail.url_destino = url;

                        long id_click = da.insertEmailClick(click_mail);
                        if (id_click > 0)
                        {
                            template_body = template_body.Replace(_url, ConfigurationManager.AppSettings["ur_email_click"] + id_click);
                            template_body = template_body.Replace(url, ConfigurationManager.AppSettings["ur_email_click"] + id_click);
                        }
                        else
                        {
                            LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::insertEmailClick()", true);
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la url (" + url + ") en la tabla EMAIL_CLICKS de la BBDD", true);
                        }
                    }
                }

                /// 2.3.- Actualizar el cuerpo de la newsletter
                EMAIL_CAMPAIGNS newsletter = campaign;
                newsletter.body = template_body;

                bool update_newsletter = da.updateEmailCampaigns(newsletter);
                if (!update_newsletter)
                {
                    LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::insertEmailClick()", true);
                    LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar la campaña", true);
                    template_body = string.Empty;
                }
            }

            /// 3.- Comprobar si se ha modificado el cuerpo de la newsletter
            if (String.IsNullOrEmpty(template_body))
            {
                procesado = false;
                LogUtils.InsertarLog(" ERROR - Se han producido un error al modificar el cuerpo de la newsletter", true);
            }
            else
            {
                /// 4.- Recuperar datos de la BBDD
                List<EMAIL_LISTADO_SUSCRIPCIONES> lst_listado = da.getEmailListSubscriptionsById(campaign.id_els.Value);
                List<EMAIL_SUSCRIPCIONES> lst_subscriptions = da.getEmailSubscriptionsById(campaign.id_els.Value);
                List<EMAIL_CAMPAIGNS_STATUS> list_newsletter_status = da.getCampaignsStatusById(campaign.id_camp);

                /// 5.- Sacar los suscriptores activos
                if (list_newsletter_status.Count == 0)
                    lst_subscriptions = lst_subscriptions.Where(sus => sus.fecha_baja == null).ToList();
                else
                {
                    List<long> list_id = list_newsletter_status.Select(ecs => ecs.id_s).ToList();
                    lst_subscriptions = lst_subscriptions.Where(es => !list_id.Contains(es.id_s)).ToList();
                    lst_subscriptions = lst_subscriptions.Where(sus => sus.fecha_baja == null).ToList();
                }

                if (lst_subscriptions.Count > 0)
                {
                    /// 6.- Sacar los datos de clientes de los suscriptores activos
                    List<long> lst_id_clients = lst_subscriptions.Select(sus => sus.id_alumno).Distinct().ToList();
                    List<CLIENTES> lst_clients = new List<CLIENTES>();
                    if (lst_id_clients.Count > 2000)
                    {
                        lst_clients = da.getUserById(-1);
                        lst_clients = lst_clients.Where(c => lst_id_clients.Contains(c.id_cliente)).ToList();
                    }
                    else
                        lst_clients = da.getUserByList(lst_id_clients);

                    /// 7.- Recorrer los suscriptores activos
                    if (lst_subscriptions.Count > 0)
                    {
                        List<long> lst_id_user = new List<long>();

                        /// 7.1.- Sacar la plantilla de la campaña
                        string template_user = string.Empty;
                        if (campaign.id_usuario != null)
                        {
                            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                            if (list_users_mails.Contains(campaign.id_usuario.Value.ToString()))
                            {
                                if (campaign.id_usuario.Value == (long)Constantes.usuario_especial_mail.Aniacam)
                                    template_user = "email_campaign_aniacam";
                                else if (campaign.id_usuario.Value == (long)Constantes.usuario_especial_mail.Sbs_Comunicacion)
                                    template_user = "email_campaign_comunicacion";
                                else if (campaign.id_usuario.Value == (long)Constantes.usuario_especial_mail.Munuslingua)
                                    template_user = "email_campaign_munuslingua";
                                else
                                    template_user = "email_campaign";
                            }
                            else
                                template_user = "email_campaign";
                        }
                        else
                            template_user = "email_campaign";

                        /// 7.2.- Recorremos los suscriptores
                        foreach (var suscriptor in lst_subscriptions)
                        {
                            /// 7.3.- Comprobar si el alumno ya se ha enviado
                            if (!lst_id_user.Contains(suscriptor.id_alumno))
                            {
                                bool correct_mail = true;

                                /// 7.3.- Poner los datos del alumno
                                lst_id_user.Add(suscriptor.id_alumno);
                                
                                /// 8.- Sacar los datos del usuario
                                List<CLIENTES> lst_user = lst_clients.Where(c => c.id_cliente == suscriptor.id_alumno).ToList();
                                if (lst_user.Count == 1)
                                {
                                    /// 9.- Ruta donde vamos a guardar el html
                                    string route_html = ConfigurationManager.AppSettings["routeVerWeb"] + campaign.id_camp + "\\";
                                    if (!(Directory.Exists(route_html)))
                                        Directory.CreateDirectory(route_html);
                                    string name_html = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + suscriptor.id_alumno + ".html";

                                    /// 10.- Generar un mail
                                    string template = string.Empty;
                                    if (campaign.newsletter != null && campaign.newsletter.Value)
                                    {
                                        template = template_body;
                                        if (!String.IsNullOrEmpty(template))
                                        {
                                            /// 10.1.- Sustirtuir el resto de campos
                                            template = template.Replace("###URL_WEB###", "https://www.spainbs.com/view-mail.aspx?es=" + Utils.toCodeString(route_html + name_html));
                                            template = template.Replace("###NOMBRE_LISTA###", lst_listado[0].nombre);
                                            template = template.Replace("###NOMBRE###", suscriptor.nombre_completo);
                                            template = template.Replace("###URL_PERFIL###", "https://www.spainbs.com/miperfil.aspx?idcl=" + lst_user[0].Key);
                                            template = template.Replace("###URL_BAJA###", "https://www.spainbs.com/baja-suscripcion.aspx?un=" + Utils.toCodeString("id_els=" + campaign.id_els + "&id_s=" + suscriptor.id_s));
                                            template = template.Replace("###URL_PIXEL###", "https://www.spainbs.com/controls/pixel.ashx?ida=191&ido=248&k=" + lst_user[0].Key + "&idl=###IDM###");
                                        }
                                    }
                                    else
                                    {
                                        template = Utilities.getPlantillaMail(template_user, ConfigurationManager.AppSettings["urlTemplate"]);
                                        if (!String.IsNullOrEmpty(template))
                                        {
                                            /// 10.1.- Sustirtuir el resto de campos
                                            template = template.Replace("###URL_WEB###", "https://www.spainbs.com/view-mail.aspx?es=" + Utils.toCodeString(route_html + name_html));
                                            template = template.Replace("###BODY###", template_body);
                                            template = template.Replace("###NOMBRE###", suscriptor.nombre_completo);
                                            template = template.Replace("###NOMBRE_LISTA###", lst_listado[0].nombre);
                                            template = template.Replace("###URL_PERFIL###", "https://www.spainbs.com/miperfil.aspx?idcl=" + lst_user[0].Key);
                                            template = template.Replace("###MAIL_FROM###", campaign.mailFrom);
                                            template = template.Replace("###URL_BAJA###", "https://www.spainbs.com/baja-suscripcion.aspx?un=" + Utils.toCodeString("id_els=" + campaign.id_els + "&id_s=" + suscriptor.id_s));
                                            template = template.Replace("###URL_PIXEL###", "https://www.spainbs.com/controls/pixel.ashx?ida=191&ido=248&k=" + lst_user[0].Key + "&idl=###IDM###");
                                        }
                                    }

                                    /// 11.- Guardar el mail
                                    EMAIL_CONTENT _mail = new EMAIL_CONTENT();
                                    _mail.nombreTo = lst_user[0].Nombre_Completo;
                                    _mail.mailTo = lst_user[0].email;
                                    _mail.date_schedule = campaign.fecha_scheduled;
                                    _mail.priority = campaign.priority;
                                    _mail.nombreFrom = campaign.nombreFrom;
                                    _mail.mailFrom = campaign.mailFrom;
                                    _mail.replyTo = campaign.replyTo;
                                    _mail.asunto = campaign.asunto;
                                    _mail.body = template;
                                    _mail.adjuntos = campaign.adjuntos;

                                    long insert_mail = da.insertEmailContent(_mail);
                                    if (insert_mail > 0)
                                    {
                                        /// 12.- Buscar los datos del mail
                                        List<EMAIL_CONTENT> lst_mail = da.getMailById(insert_mail);
                                        if (lst_mail.Count == 1)
                                        {
                                            string body = lst_mail[0].body;

                                            /// 12.1.- Sustituir las urls
                                            List<string> lst_urls_clicks = obtener_urls_clicks(lst_mail[0].body);
                                            if (lst_urls_clicks.Count > 0)
                                            {
                                                /// 12.2.- Ordenar descendente las urls
                                                lst_urls_clicks = lst_urls_clicks.OrderByDescending(url => url).ToList();
                                                foreach (var url_click in lst_urls_clicks)
                                                {
                                                    string id_click = url_click.Replace(ConfigurationManager.AppSettings["ur_email_click"], string.Empty);
                                                    string encoded_parameters = "id_click=" + id_click + "&ida=192&ido=248&idu=" + lst_user[0].id_cliente + "&idl=" + insert_mail.ToString() + "&utm_source=email&utm_medium=email&utm_campaign=" + campaign.nombre + "&utm_content=" + campaign.id_camp + (!String.IsNullOrEmpty(campaign.tags_clic) ? "&utm_term=" + campaign.tags_clic : string.Empty);
                                                    string url_encoded = url_click.Split('?')[0] + "?ep=" + Utils.toCodeString(encoded_parameters);
                                                    body = body.Replace(url_click, url_encoded);
                                                }
                                            }

                                            //string body = lst_mail[0].body.Replace("###IDM###", insert_mail.ToString());
                                            body = body.Replace("###IDM###", insert_mail.ToString());
                                            bool update_mail = da.updateEmailContent(insert_mail, body);
                                            if (!update_mail)
                                            {
                                                procesado = false;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al actualizar el mail " + insert_mail, true);
                                                correct_mail = false;
                                            }
                                        }
                                        else
                                        {
                                            procesado = false;
                                            LogUtils.InsertarLog(" ERROR - Se ha producido errores al recuperar los datos del mail " + insert_mail, true);
                                            correct_mail = false;
                                        }

                                        if (correct_mail)
                                        {
                                            /// 13.- Guardamos el html
                                            StreamWriter stream = new StreamWriter(route_html + name_html);

                                            /// 13.1.- Reemplazar el idMail
                                            template = template.Replace("###IDM###", insert_mail.ToString());

                                            /// 13.2.- Sustituir las urls
                                            List<string> lst_urls_clicks = obtener_urls_clicks(template);
                                            if (lst_urls_clicks.Count > 0)
                                            {
                                                /// 12.2.- Ordenar descendente las urls
                                                lst_urls_clicks = lst_urls_clicks.OrderByDescending(url => url).ToList();
                                                foreach (var url_click in lst_urls_clicks)
                                                {
                                                    string id_click = url_click.Replace(ConfigurationManager.AppSettings["ur_email_click"], string.Empty);
                                                    string encoded_parameters = "id_click=" + id_click + "&ida=192&ido=248&idu=" + lst_user[0].id_cliente + "&idl=" + insert_mail.ToString() + "&utm_source=email&utm_medium=email&utm_campaign=" + campaign.nombre + "&utm_content=" + campaign.id_camp + (!String.IsNullOrEmpty(campaign.tags_clic) ? "&utm_term=" + campaign.tags_clic : string.Empty);
                                                    string url_encoded = url_click.Split('?')[0] + "?ep=" + Utils.toCodeString(encoded_parameters);
                                                    template = template.Replace(url_click, url_encoded);
                                                }
                                            }

                                            stream.Write(template);
                                            stream.Close();

                                            /// 14.- Guardar una entrada en EMAIL_CAMPAIGNS_STATUS
                                            EMAIL_CAMPAIGNS_STATUS newsletter_status = new EMAIL_CAMPAIGNS_STATUS();
                                            newsletter_status.id_camp = campaign.id_camp;
                                            newsletter_status.id_s = suscriptor.id_s;
                                            newsletter_status.id_ec = insert_mail;
                                            newsletter_status.fecha_hora = DateTime.Now;
                                            newsletter_status.status = (int)Constantes.status_mail.Not_Send;

                                            long insert_campaign_status = da.insertEmailCampaignsStatus(newsletter_status);
                                            if (insert_campaign_status < 1)
                                            {
                                                procesado = false;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido errores al guardar el status de la campaña en EMAIL_CAMPAIGNS_STATUS del usuario " + lst_user[0].Nombre_Completo, true);
                                            }
                                            else
                                                Console.WriteLine("Suscriptor procesado " + suscriptor.nombre_completo);
                                        }
                                    }
                                    else
                                    {
                                        procesado = false;
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al mandar el mail al usuario " + lst_user[0].Nombre_Completo, true);
                                    }
                                }
                                else
                                    LogUtils.InsertarLog(" ERROR - Se ha producido un error al recuperar los datos del usuario " + suscriptor.id_alumno, true);
                            }
                        }
                    }
                    else
                    {
                        procesado = false;
                        LogUtils.InsertarLog(" ERROR - La lista asociada (" + lst_listado[0].nombre + ") no tiene suscriptores activos", true);
                    }
                }
                else
                {
                    procesado = false;
                    LogUtils.InsertarLog(" ERROR - La lista asociada (" + lst_listado[0].nombre + ") no tiene suscriptores activos", true);
                }
            }

            return procesado;
        }
        
        public bool process_no_opens(EMAIL_CAMPAIGNS campaign)
        {
            bool procesado = true;

            /// 1.- Sacar el cuerpo de la newsletter
            string template_body = campaign.body;

            /// 2.- Sustituir las url por las especiales
            List<string> list_urls = obtener_urls(template_body);

            if (list_urls.Count > 0)
            {
                /// 2.1.- Sacar las urls de la BBDD
                List<EMAIL_CLICKS> lst_mails = da.getEmailClicks();

                /// 2.2.- Recorrer y sustituir las urls
                foreach (var url in list_urls)
                {
                    string _url = url.Replace("&", "&amp;");

                    /// 2.2.1.- Comprobar si existe
                    List<EMAIL_CLICKS> lst_mail_click = lst_mails.Where(ec => ec.url_destino == url).ToList();
                    if (lst_mail_click.Count == 1)
                    {
                        template_body = template_body.Replace(_url, ConfigurationManager.AppSettings["ur_email_click"] + lst_mail_click[0].id_click);
                        template_body = template_body.Replace(url, ConfigurationManager.AppSettings["ur_email_click"] + lst_mail_click[0].id_click);
                    }
                    else if (lst_mail_click.Count == 0)
                    {
                        /// 2.2.2.- Añadir a la BBDD la url nueva
                        EMAIL_CLICKS click_mail = new EMAIL_CLICKS();
                        click_mail.url_destino = url;

                        long id_click = da.insertEmailClick(click_mail);
                        if (id_click > 0)
                        {
                            template_body = template_body.Replace(_url, ConfigurationManager.AppSettings["ur_email_click"] + id_click);
                            template_body = template_body.Replace(url, ConfigurationManager.AppSettings["ur_email_click"] + id_click);
                        }
                        else
                        {
                            LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::insertEmailClick()", true);
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la url (" + url + ") en la tabla EMAIL_CLICKS de la BBDD", true);
                        }
                    }
                }

                /// 2.3.- Actualizar el cuerpo de la newsletter
                EMAIL_CAMPAIGNS newsletter = campaign;
                newsletter.body = template_body;

                bool update_newsletter = da.updateEmailCampaigns(newsletter);
                if (!update_newsletter)
                {
                    LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::insertEmailClick()", true);
                    LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar la campaña", true);
                    template_body = string.Empty;
                }
            }

            /// 3.- Comprobar si se ha modificado el cuerpo de la newsletter
            if (String.IsNullOrEmpty(template_body))
            {
                procesado = false;
                LogUtils.InsertarLog(" ERROR - Se han producido un error al modificar el cuerpo de la newsletter", true);
            }
            else
            {
                /// 5.- Recuperar datos de la BBDD
                List<EMAIL_LISTADO_SUSCRIPCIONES> lst_listado = da.getEmailListSubscriptionsById(campaign.id_els.Value);
                List<EMAIL_SUSCRIPCIONES> lst_subscriptions = da.getEmailSubscriptionsById(campaign.id_els.Value);
                List<EMAIL_CAMPAIGNS_STATUS> list_newsletter_status = da.getCampaignsStatusById(campaign.id_camp);

                /// 5.1.- Sacar los suscriptores con status no open y activos
                list_newsletter_status = list_newsletter_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Send).ToList();
                if (list_newsletter_status.Count > 0)
                {
                    List<long> list_id = list_newsletter_status.Select(ecs => ecs.id_s).ToList();
                    lst_subscriptions = lst_subscriptions.Where(es => list_id.Contains(es.id_s)).ToList();
                    lst_subscriptions = lst_subscriptions.Where(sus => sus.fecha_baja == null).ToList();
                }
                else
                    lst_subscriptions = new List<EMAIL_SUSCRIPCIONES>();

                if (lst_subscriptions.Count > 0)
                {
                    /// 7.- Sacar los datos de clientes de los suscriptores activos
                    List<long> lst_id_clients = lst_subscriptions.Select(sus => sus.id_alumno).Distinct().ToList();
                    List<CLIENTES> lst_clients = new List<CLIENTES>();
                    if (lst_id_clients.Count > 2000)
                    {
                        lst_clients = da.getUserById(-1);
                        lst_clients = lst_clients.Where(c => lst_id_clients.Contains(c.id_cliente)).ToList();
                    }
                    else
                        lst_clients = da.getUserByList(lst_id_clients);

                    /// 8.- Recorrer los suscriptores activos
                    if (lst_subscriptions.Count > 0)
                    {
                        List<long> lst_status = new List<long>();

                        /// 8.1.- Sacar la plantilla de la campaña
                        string template_user = string.Empty;
                        if (campaign.id_usuario != null)
                        {
                            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                            if (list_users_mails.Contains(campaign.id_usuario.Value.ToString()))
                            {
                                if (campaign.id_usuario.Value == (long)Constantes.usuario_especial_mail.Aniacam)
                                    template_user = "email_campaign_aniacam";
                                else if (campaign.id_usuario.Value == (long)Constantes.usuario_especial_mail.Sbs_Comunicacion)
                                    template_user = "email_campaign_comunicacion";
                                else if (campaign.id_usuario.Value == (long)Constantes.usuario_especial_mail.Munuslingua)
                                    template_user = "email_campaign_munuslingua";
                                else
                                    template_user = "email_campaign";
                            }
                            else
                                template_user = "email_campaign";
                        }
                        else
                            template_user = "email_campaign";

                        /// 8.2.- Recorremos los suscriptores
                        foreach (var suscriptor in lst_subscriptions)
                        {
                            if (!lst_status.Contains(suscriptor.id_s))
                            {
                                bool correct_mail = true;

                                /// 8.3.- Sacar los datos del usuario
                                List<CLIENTES> lst_user = lst_clients.Where(c => c.id_cliente == suscriptor.id_alumno).ToList();
                                if (lst_user.Count == 1)
                                {
                                    /// 9.- Ruta donde vamos a guardar el html
                                    string route_html = ConfigurationManager.AppSettings["routeVerWeb"] + campaign.id_camp + "\\";
                                    if (!(Directory.Exists(route_html)))
                                        Directory.CreateDirectory(route_html);
                                    string name_html = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + suscriptor.id_alumno + ".html";

                                    /// 10.- Generar un mail
                                    string template = string.Empty;
                                    if (campaign.newsletter != null && campaign.newsletter.Value)
                                    {
                                        template = template_body;
                                        if (!String.IsNullOrEmpty(template))
                                        {
                                            /// 10.1.- Sustirtuir el resto de campos
                                            template = template.Replace("###URL_WEB###", "https://www.spainbs.com/view-mail.aspx?es=" + Utils.toCodeString(route_html + name_html));
                                            template = template.Replace("###NOMBRE_LISTA###", lst_listado[0].nombre);
                                            template = template.Replace("###NOMBRE###", suscriptor.nombre_completo);
                                            template = template.Replace("###URL_PERFIL###", "https://www.spainbs.com/miperfil.aspx?idcl=" + lst_user[0].Key);
                                            template = template.Replace("###URL_BAJA###", "https://www.spainbs.com/baja-suscripcion.aspx?un=" + Utils.toCodeString("id_els=" + campaign.id_els + "&id_s=" + suscriptor.id_s));
                                            template = template.Replace("###URL_PIXEL###", "https://www.spainbs.com/controls/pixel.ashx?ida=191&ido=248&k=" + lst_user[0].Key + "&idl=###IDM###");
                                        }
                                    }
                                    else
                                    {
                                        template = Utilities.getPlantillaMail(template_user, ConfigurationManager.AppSettings["urlTemplate"]);
                                        if (!String.IsNullOrEmpty(template))
                                        {
                                            /// 10.1.- Sustirtuir el resto de campos
                                            template = template.Replace("###URL_WEB###", "https://www.spainbs.com/view-mail.aspx?es=" + Utils.toCodeString(route_html + name_html));
                                            template = template.Replace("###BODY###", template_body);
                                            template = template.Replace("###NOMBRE###", suscriptor.nombre_completo);
                                            template = template.Replace("###NOMBRE_LISTA###", lst_listado[0].nombre);
                                            template = template.Replace("###URL_PERFIL###", "https://www.spainbs.com/miperfil.aspx?idcl=" + lst_user[0].Key);
                                            template = template.Replace("###MAIL_FROM###", campaign.mailFrom);
                                            template = template.Replace("###URL_BAJA###", "https://www.spainbs.com/baja-suscripcion.aspx?un=" + Utils.toCodeString("id_els=" + campaign.id_els + "&id_s=" + suscriptor.id_s));
                                            template = template.Replace("###URL_PIXEL###", "https://www.spainbs.com/controls/pixel.ashx?ida=191&ido=248&k=" + lst_user[0].Key + "&idl=###IDM###");
                                        }
                                    }

                                    /// 11.- Guardar el mail
                                    EMAIL_CONTENT _mail = new EMAIL_CONTENT();
                                    _mail.nombreTo = lst_user[0].Nombre_Completo;
                                    _mail.mailTo = lst_user[0].email;
                                    _mail.date_schedule = campaign.fecha_scheduled;
                                    _mail.priority = campaign.priority;
                                    _mail.nombreFrom = campaign.nombreFrom;
                                    _mail.mailFrom = campaign.mailFrom;
                                    _mail.replyTo = campaign.replyTo;
                                    _mail.asunto = campaign.asunto;
                                    _mail.body = template;
                                    _mail.adjuntos = campaign.adjuntos;

                                    long insert_mail = da.insertEmailContent(_mail);
                                    if (insert_mail > 0)
                                    {
                                        /// 12.- Buscar los datos del mail
                                        List<EMAIL_CONTENT> lst_mail = da.getMailById(insert_mail);
                                        if (lst_mail.Count == 1)
                                        {
                                            string body = lst_mail[0].body;

                                            /// 12.1.- Sustituir las urls
                                            List<string> lst_urls_clicks = obtener_urls_clicks(lst_mail[0].body);
                                            if (lst_urls_clicks.Count > 0)
                                            {
                                                /// 12.2.- Ordenar descendente las urls
                                                lst_urls_clicks = lst_urls_clicks.OrderByDescending(url => url).ToList();
                                                foreach (var url_click in lst_urls_clicks)
                                                {
                                                    string id_click = url_click.Replace(ConfigurationManager.AppSettings["ur_email_click"], string.Empty);
                                                    string encoded_parameters = "id_click=" + id_click + "&ida=192&ido=248&idu=" + lst_user[0].id_cliente + "&idl=" + insert_mail.ToString() + "&utm_source=email&utm_medium=email&utm_campaign=" + campaign.nombre + "&utm_content=" + campaign.id_camp + (!String.IsNullOrEmpty(campaign.tags_clic) ? "&utm_term=" + campaign.tags_clic : string.Empty);
                                                    string url_encoded = url_click.Split('?')[0] + "?ep=" + Utils.toCodeString(encoded_parameters);
                                                    body = body.Replace(url_click, url_encoded);
                                                }
                                            }

                                            body = body.Replace("###IDM###", insert_mail.ToString());
                                            bool update_mail = da.updateEmailContent(insert_mail, body);
                                            if (!update_mail)
                                            {
                                                procesado = false;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al actualizar el mail " + insert_mail, true);
                                                correct_mail = false;
                                            }
                                        }
                                        else
                                        {
                                            procesado = false;
                                            LogUtils.InsertarLog(" ERROR - Se ha producido errores al recuperar los datos del mail " + insert_mail, true);
                                            correct_mail = false;
                                        }

                                        if (correct_mail)
                                        {
                                            /// 13.- Guardamos el html
                                            StreamWriter stream = new StreamWriter(route_html + name_html);

                                            /// 13.1.- Reemplazar el idMail
                                            template = template.Replace("###IDM###", insert_mail.ToString());

                                            /// 13.2.- Sustituir las urls
                                            List<string> lst_urls_clicks = obtener_urls_clicks(template);
                                            if (lst_urls_clicks.Count > 0)
                                            {
                                                /// 12.2.- Ordenar descendente las urls
                                                lst_urls_clicks = lst_urls_clicks.OrderByDescending(url => url).ToList();
                                                foreach (var url_click in lst_urls_clicks)
                                                {
                                                    string id_click = url_click.Replace(ConfigurationManager.AppSettings["ur_email_click"], string.Empty);
                                                    string encoded_parameters = "id_click=" + id_click + "&ida=192&ido=248&idu=" + lst_user[0].id_cliente + "&idl=" + insert_mail.ToString() + "&utm_source=email&utm_medium=email&utm_campaign=" + campaign.nombre + "&utm_content=" + campaign.id_camp + (!String.IsNullOrEmpty(campaign.tags_clic) ? "&utm_term=" + campaign.tags_clic : string.Empty);
                                                    string url_encoded = url_click.Split('?')[0] + "?ep=" + Utils.toCodeString(encoded_parameters);
                                                    template = template.Replace(url_click, url_encoded);
                                                }
                                            }

                                            stream.Write(template);
                                            stream.Close();

                                            /// 14.- Guardar una entrada en EMAIL_CAMPAIGNS_STATUS
                                            EMAIL_CAMPAIGNS_STATUS newsletter_status = new EMAIL_CAMPAIGNS_STATUS();
                                            newsletter_status.id_camp = campaign.id_camp;
                                            newsletter_status.id_s = suscriptor.id_s;
                                            newsletter_status.id_ec = insert_mail;
                                            newsletter_status.fecha_hora = DateTime.Now;
                                            newsletter_status.status = (int)Constantes.status_mail.Not_Send;
                                            newsletter_status.reenvio = true;

                                            long insert_campaign_status = da.insertEmailCampaignsStatus(newsletter_status);
                                            if (insert_campaign_status < 1)
                                            {
                                                procesado = false;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido errores al guardar el status de la campaña en EMAIL_CAMPAIGNS_STATUS del usuario " + lst_user[0].Nombre_Completo, true);
                                            }
                                            else
                                            {
                                                /// 15.- Insertar una entrada en EMAIL_CAMPAIGNS_STATUS_REENVIO
                                                EMAIL_CAMPAIGNS_STATUS_REENVIO newsletter_status_reenv = new EMAIL_CAMPAIGNS_STATUS_REENVIO();
                                                newsletter_status_reenv.id_ecs = insert_campaign_status;
                                                newsletter_status_reenv.fecha_reenvio = DateTime.Now;
                                                newsletter_status_reenv.tipo = (int)Constantes.type_status_mail.No_Open;

                                                long insert_campaign_status_reenv = da.insertEmailCampaignsStatusReenvio(newsletter_status_reenv);
                                                if (insert_campaign_status_reenv < 1)
                                                {
                                                    procesado = false;
                                                    LogUtils.InsertarLog(" ERROR - Se ha producido errores al guardar el status de la campaña en EMAIL_CAMPAIGNS_STATUS_REENVIO del usuario " + lst_user[0].Nombre_Completo, true);
                                                }
                                                else
                                                {
                                                    /// 16.- Sacar el dato de EMAIL_CAMPAIGNS_STATUS original
                                                    List<EMAIL_CAMPAIGNS_STATUS> lst_newsletter_status = list_newsletter_status.Where(ecs => ecs.id_s == suscriptor.id_s).ToList();
                                                    if (lst_newsletter_status.Count == 1)
                                                    {
                                                        /// 17.- Actualizar los datos de EMAIL_CAMPAIGNS_STATUS
                                                        EMAIL_CAMPAIGNS_STATUS _newsletter_status = lst_newsletter_status[0];
                                                        _newsletter_status.status = (int)Constantes.status_mail.Reenviado;
                                                        _newsletter_status.id_ecs_reenv = insert_campaign_status_reenv;

                                                        bool update_newsletter = da.updateEmailCampaignsStatus(_newsletter_status);
                                                        if (update_newsletter)
                                                            Console.WriteLine("Suscriptor procesado " + suscriptor.nombre_completo);
                                                        else
                                                        {
                                                            procesado = false;
                                                            LogUtils.InsertarLog(" ERROR - Se ha producido errores al actualizar los datos en EMAIL_CAMPAIGNS_STATUS del usuario " + lst_user[0].Nombre_Completo, true);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        procesado = false;
                                                        LogUtils.InsertarLog(" ERROR - Se ha producido errores al buscar los datos del suscriptor de la campaña en EMAIL_CAMPAIGNS_STATUS del usuario " + lst_user[0].Nombre_Completo, true);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        procesado = false;
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al mandar el mail al usuario " + lst_user[0].Nombre_Completo, true);
                                    }
                                }
                                else
                                    LogUtils.InsertarLog(" ERROR - Se ha producido un error al recuperar los datos del usuario " + suscriptor.id_alumno, true);
                            }
                        }
                    }
                    else
                    {
                        procesado = false;
                        LogUtils.InsertarLog(" ERROR - No hay suscriptores no opens y activos en la campaña " + campaign.nombre, true);
                    }
                }
                else
                {
                    procesado = false;
                    LogUtils.InsertarLog(" ERROR - No hay suscriptores no opens y activos en la campaña " + campaign.nombre, true);
                }
            }

            return procesado;
        }

        public bool process_activators(EMAIL_ACTIVADORES activator)
        {
            bool procesado = true;

            /// 1.- Sacar los datos de la campaña principal
            List<EMAIL_CAMPAIGNS> list_campaigns = da.getCampaignsById(activator.id_camp);
            if (list_campaigns.Count == 1)
            {
                /// 2.- Recuperar datos de la BBDD
                List<EMAIL_LISTADO_SUSCRIPCIONES> lst_listado = da.getEmailListSubscriptionsById(list_campaigns[0].id_els.Value);
                List<EMAIL_SUSCRIPCIONES> lst_subscriptions = da.getEmailSubscriptionsById(list_campaigns[0].id_els.Value);
                List<EMAIL_CAMPAIGNS_STATUS> list_newsletter_status = da.getCampaignsStatusById(list_campaigns[0].id_camp);

                /// 2.0.- Filtrar los datos
                list_newsletter_status = list_newsletter_status.Where(ecs => ecs.status != (int)Constantes.status_mail.Reenviado).ToList();

                int data_bajas = list_newsletter_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Bounced).Count();
                int data_opens = list_newsletter_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Open || ecs.status == (int)Constantes.status_mail.Clic).Count();
                int data_clics = list_newsletter_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Clic).Count();
                int data_users = list_newsletter_status.Select(ecs => ecs.id_s).Distinct().Count();

                /// 2.1.- Actualizar los activadores                
                activator.data_bajas = data_bajas;
                activator.data_opens = data_opens;
                activator.data_clics = data_clics;
                activator.data_users = data_users;

                bool update_activator = da.updateEmailActivator(activator);
                if (!update_activator)
                {
                    LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::insertEmailClick()", true);
                    LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el activador", true);
                    procesado = false;
                }
                else
                {
                    /// 3.- Sacar el cuerpo de la newsletter
                    string template_body = activator.body;

                    /// 4.- Sustituir las url por las especiales
                    List<string> list_urls = obtener_urls(template_body);
                    if (list_urls.Count > 0)
                    {
                        /// 4.1.- Sacar las urls de la BBDD
                        List<EMAIL_CLICKS> lst_mails = da.getEmailClicks();

                        /// 4.2.- Recorrer y sustituir las urls
                        foreach (var url in list_urls)
                        {
                            string _url = url.Replace("&", "&amp;");

                            /// 4.2.1.- Comprobar si existe
                            List<EMAIL_CLICKS> lst_mail_click = lst_mails.Where(ec => ec.url_destino == url).ToList();
                            if (lst_mail_click.Count == 1)
                            {
                                template_body = template_body.Replace(_url, ConfigurationManager.AppSettings["ur_email_click"] + lst_mail_click[0].id_click);
                                template_body = template_body.Replace(url, ConfigurationManager.AppSettings["ur_email_click"] + lst_mail_click[0].id_click);
                            }
                            else if (lst_mail_click.Count == 0)
                            {
                                /// 4.2.2.- Añadir a la BBDD la url nueva
                                EMAIL_CLICKS click_mail = new EMAIL_CLICKS();
                                click_mail.url_destino = url;

                                long id_click = da.insertEmailClick(click_mail);
                                if (id_click > 0)
                                {
                                    template_body = template_body.Replace(_url, ConfigurationManager.AppSettings["ur_email_click"] + id_click);
                                    template_body = template_body.Replace(url, ConfigurationManager.AppSettings["ur_email_click"] + id_click);
                                }
                                else
                                {
                                    LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::insertEmailClick()", true);
                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la url (" + url + ") en la tabla EMAIL_CLICKS de la BBDD", true);
                                }
                            }
                        }

                        /// 4.3.- Actualizar el cuerpo de la newsletter
                        EMAIL_ACTIVADORES _activator = activator;
                        _activator.body = template_body;

                        bool _update_activator = da.updateEmailActivator(_activator);
                        if (!_update_activator)
                        {
                            LogUtils.InsertarLog(" ERROR - lista-newsletter-mantenimiento.cs::insertEmailClick()", true);
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el activador", true);
                            template_body = string.Empty;
                        }
                    }

                    /// 5.- Comprobar si se ha modificado el cuerpo de la newsletter
                    if (String.IsNullOrEmpty(template_body))
                    {
                        procesado = false;
                        LogUtils.InsertarLog(" ERROR - Se han producido un error al modificar el cuerpo de la newsletter", true);
                    }
                    else
                    {
                        List<EMAIL_SUSCRIPCIONES> lst_subscriptores = new List<EMAIL_SUSCRIPCIONES>();

                        /// 6.- Sacar los datos a mostrar
                        if (activator.tipo == (int)Constantes.status_campaign.Reenvio_no_opens)
                        {
                            List<EMAIL_CAMPAIGNS_STATUS> lst_news_status = list_newsletter_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Send).ToList();
                            if (list_newsletter_status.Count > 0)
                            {
                                List<long> list_id = lst_news_status.Select(ecs => ecs.id_s).ToList();
                                lst_subscriptores = lst_subscriptions.Where(es => list_id.Contains(es.id_s)).ToList();
                                lst_subscriptores = lst_subscriptores.Where(sus => sus.fecha_baja == null).ToList();
                            }
                        }
                        else if (activator.tipo == (int)Constantes.status_campaign.Reenvio_open)
                        {
                            List<EMAIL_CAMPAIGNS_STATUS> lst_news_status = list_newsletter_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Open).ToList();
                            if (list_newsletter_status.Count > 0)
                            {
                                List<long> list_id = lst_news_status.Select(ecs => ecs.id_s).ToList();
                                lst_subscriptores = lst_subscriptions.Where(es => list_id.Contains(es.id_s)).ToList();
                                lst_subscriptores = lst_subscriptores.Where(sus => sus.fecha_baja == null).ToList();
                            }
                        }
                        else if (activator.tipo == (int)Constantes.status_campaign.Reenvio_clicks)
                        {
                            List<EMAIL_CAMPAIGNS_STATUS> lst_news_status = list_newsletter_status.Where(ecs => ecs.status == (int)Constantes.status_mail.Clic).ToList();
                            if (list_newsletter_status.Count > 0)
                            {
                                List<long> list_id = lst_news_status.Select(ecs => ecs.id_s).ToList();
                                lst_subscriptores = lst_subscriptions.Where(es => list_id.Contains(es.id_s)).ToList();
                                lst_subscriptores = lst_subscriptores.Where(sus => sus.fecha_baja == null).ToList();
                            }
                        }

                        if (lst_subscriptores.Count > 0)
                        {
                            /// 7.- Sacar los datos de clientes de los suscriptores activos
                            List<long> lst_id_clients = lst_subscriptores.Select(sus => sus.id_alumno).Distinct().ToList();
                            List<CLIENTES> lst_clients = new List<CLIENTES>();
                            if (lst_id_clients.Count > 2000)
                            {
                                lst_clients = da.getUserById(-1);
                                lst_clients = lst_clients.Where(c => lst_id_clients.Contains(c.id_cliente)).ToList();
                            }
                            else
                                lst_clients = da.getUserByList(lst_id_clients);

                            /// 8.- Recorrer los suscriptores activos
                            if (lst_subscriptores.Count > 0)
                            {
                                List<long> lst_status = new List<long>();

                                /// 8.1.- Sacar la plantilla de la campaña
                                string template_user = string.Empty;
                                if (list_campaigns[0].id_usuario != null)
                                {
                                    List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                                    if (list_users_mails.Contains(list_campaigns[0].id_usuario.Value.ToString()))
                                    {
                                        if (list_campaigns[0].id_usuario.Value == (long)Constantes.usuario_especial_mail.Aniacam)
                                            template_user = "email_campaign_aniacam";
                                        else if (list_campaigns[0].id_usuario.Value == (long)Constantes.usuario_especial_mail.Sbs_Comunicacion)
                                            template_user = "email_campaign_comunicacion";
                                        else if (list_campaigns[0].id_usuario.Value == (long)Constantes.usuario_especial_mail.Munuslingua)
                                            template_user = "email_campaign_munuslingua";
                                        else
                                            template_user = "email_campaign";
                                    }
                                    else
                                        template_user = "email_campaign";
                                }
                                else
                                    template_user = "email_campaign";

                                /// 8.2.- Recorremos los suscriptores
                                foreach (var suscriptor in lst_subscriptores)
                                {

                                    if (!lst_status.Contains(suscriptor.id_s))
                                    {
                                        bool correct_mail = true;

                                        /// 8.3.- Sacar los datos del usuario
                                        List<CLIENTES> lst_user = lst_clients.Where(c => c.id_cliente == suscriptor.id_alumno).ToList();
                                        if (lst_user.Count == 1)
                                        {
                                            /// 9.- Ruta donde vamos a guardar el html
                                            string route_html = ConfigurationManager.AppSettings["routeVerWeb"] + activator.id_camp + "\\";
                                            if (!(Directory.Exists(route_html)))
                                                Directory.CreateDirectory(route_html);
                                            string name_html = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + suscriptor.id_alumno + ".html";

                                            /// 10.- Generar un mail
                                            string template = string.Empty;
                                            if (list_campaigns[0].newsletter != null && list_campaigns[0].newsletter.Value)
                                            {
                                                template = template_body;
                                                if (!String.IsNullOrEmpty(template))
                                                {
                                                    /// 10.1.- Sustirtuir el resto de campos
                                                    template = template.Replace("###URL_WEB###", "https://www.spainbs.com/view-mail.aspx?es=" + Utils.toCodeString(route_html + name_html));
                                                    template = template.Replace("###NOMBRE_LISTA###", lst_listado[0].nombre);
                                                    template = template.Replace("###NOMBRE###", suscriptor.nombre_completo);
                                                    template = template.Replace("###URL_PERFIL###", "https://www.spainbs.com/miperfil.aspx?idcl=" + lst_user[0].Key);
                                                    template = template.Replace("###URL_BAJA###", "https://www.spainbs.com/baja-suscripcion.aspx?un=" + Utils.toCodeString("id_els=" + list_campaigns[0].id_els + "&id_s=" + suscriptor.id_s));
                                                    template = template.Replace("###URL_PIXEL###", "https://www.spainbs.com/controls/pixel.ashx?ida=191&ido=248&k=" + lst_user[0].Key + "&idl=###IDM###");
                                                }
                                            }
                                            else
                                            {
                                                template = Utilities.getPlantillaMail(template_user, ConfigurationManager.AppSettings["urlTemplate"]);
                                                if (!String.IsNullOrEmpty(template))
                                                {
                                                    /// 10.1.- Sustirtuir el resto de campos
                                                    template = template.Replace("###URL_WEB###", "https://www.spainbs.com/view-mail.aspx?es=" + Utils.toCodeString(route_html + name_html));
                                                    template = template.Replace("###BODY###", template_body);
                                                    template = template.Replace("###NOMBRE###", suscriptor.nombre_completo);
                                                    template = template.Replace("###NOMBRE_LISTA###", lst_listado[0].nombre);
                                                    template = template.Replace("###URL_PERFIL###", "https://www.spainbs.com/miperfil.aspx?idcl=" + lst_user[0].Key);
                                                    template = template.Replace("###MAIL_FROM###", activator.mailFrom);
                                                    template = template.Replace("###URL_BAJA###", "https://www.spainbs.com/baja-suscripcion.aspx?un=" + Utils.toCodeString("id_els=" + list_campaigns[0].id_els + "&id_s=" + suscriptor.id_s));
                                                    template = template.Replace("###URL_PIXEL###", "https://www.spainbs.com/controls/pixel.ashx?ida=191&ido=248&k=" + lst_user[0].Key + "&idl=###IDM###");
                                                }
                                            }

                                            /// 11.- Guardar el mail
                                            EMAIL_CONTENT _mail = new EMAIL_CONTENT();
                                            _mail.nombreTo = lst_user[0].Nombre_Completo;
                                            _mail.mailTo = lst_user[0].email;
                                            _mail.date_schedule = activator.fecha_act;
                                            _mail.priority = activator.priority;
                                            _mail.nombreFrom = activator.nombreFrom;
                                            _mail.mailFrom = activator.mailFrom;
                                            _mail.replyTo = activator.replyTo;
                                            _mail.asunto = activator.asunto;
                                            _mail.body = template;
                                            _mail.adjuntos = activator.adjuntos;

                                            long insert_mail = da.insertEmailContent(_mail);
                                            if (insert_mail > 0)
                                            {
                                                /// 12.- Buscar los datos del mail
                                                List<EMAIL_CONTENT> lst_mail = da.getMailById(insert_mail);
                                                if (lst_mail.Count == 1)
                                                {
                                                    string body = lst_mail[0].body;

                                                    /// 12.1.- Sustituir las urls
                                                    List<string> lst_urls_clicks = obtener_urls_clicks(lst_mail[0].body);
                                                    if (lst_urls_clicks.Count > 0)
                                                    {
                                                        /// 12.2.- Ordenar descendente las urls
                                                        lst_urls_clicks = lst_urls_clicks.OrderByDescending(url => url).ToList();
                                                        foreach (var url_click in lst_urls_clicks)
                                                        {
                                                            string id_click = url_click.Replace(ConfigurationManager.AppSettings["ur_email_click"], string.Empty);
                                                            string encoded_parameters = "id_click=" + id_click + "&ida=192&ido=248&idu=" + lst_user[0].id_cliente + "&idl=" + insert_mail.ToString() + "&utm_source=email&utm_medium=email&utm_campaign=" + list_campaigns[0].nombre + "&utm_content=" + activator.id_camp + (!String.IsNullOrEmpty(list_campaigns[0].tags_clic) ? "&utm_term=" + list_campaigns[0].tags_clic : string.Empty);
                                                            string url_encoded = url_click.Split('?')[0] + "?ep=" + Utils.toCodeString(encoded_parameters);
                                                            body = body.Replace(url_click, url_encoded);
                                                        }
                                                    }

                                                    body = body.Replace("###IDM###", insert_mail.ToString());
                                                    bool update_mail = da.updateEmailContent(insert_mail, body);
                                                    if (!update_mail)
                                                    {
                                                        //procesado = false;
                                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al actualizar el mail " + insert_mail, true);
                                                        correct_mail = false;
                                                    }
                                                }
                                                else
                                                {
                                                    //procesado = false;
                                                    LogUtils.InsertarLog(" ERROR - Se ha producido errores al recuperar los datos del mail " + insert_mail, true);
                                                    correct_mail = false;
                                                }

                                                if (correct_mail)
                                                {
                                                    /// 13.- Guardamos el html
                                                    StreamWriter stream = new StreamWriter(route_html + name_html);

                                                    /// 13.1.- Reemplazar el idMail
                                                    template = template.Replace("###IDM###", insert_mail.ToString());

                                                    /// 13.2.- Sustituir las urls
                                                    List<string> lst_urls_clicks = obtener_urls_clicks(template);
                                                    if (lst_urls_clicks.Count > 0)
                                                    {
                                                        /// 12.2.- Ordenar descendente las urls
                                                        lst_urls_clicks = lst_urls_clicks.OrderByDescending(url => url).ToList();
                                                        foreach (var url_click in lst_urls_clicks)
                                                        {
                                                            string id_click = url_click.Replace(ConfigurationManager.AppSettings["ur_email_click"], string.Empty);
                                                            string encoded_parameters = "id_click=" + id_click + "&ida=192&ido=248&idu=" + lst_user[0].id_cliente + "&idl=" + insert_mail.ToString() + "&utm_source=email&utm_medium=email&utm_campaign=" + list_campaigns[0].nombre + "&utm_content=" + activator.id_camp + (!String.IsNullOrEmpty(list_campaigns[0].tags_clic) ? "&utm_term=" + list_campaigns[0].tags_clic : string.Empty);
                                                            string url_encoded = url_click.Split('?')[0] + "?ep=" + Utils.toCodeString(encoded_parameters);
                                                            template = template.Replace(url_click, url_encoded);
                                                        }
                                                    }

                                                    stream.Write(template);
                                                    stream.Close();

                                                    /// 14.- Guardar una entrada en EMAIL_CAMPAIGNS_STATUS
                                                    EMAIL_CAMPAIGNS_STATUS newsletter_status = new EMAIL_CAMPAIGNS_STATUS();
                                                    newsletter_status.id_camp = activator.id_camp;
                                                    newsletter_status.id_s = suscriptor.id_s;
                                                    newsletter_status.id_ec = insert_mail;
                                                    newsletter_status.fecha_hora = DateTime.Now;
                                                    if (activator.tipo == (int)Constantes.status_campaign.Reenvio_open)
                                                        newsletter_status.status = (int)Constantes.status_mail.Open;
                                                    else if (activator.tipo == (int)Constantes.status_campaign.Reenvio_clicks)
                                                        newsletter_status.status = (int)Constantes.status_mail.Clic;
                                                    else
                                                        newsletter_status.status = (int)Constantes.status_mail.Not_Send;
                                                    newsletter_status.reenvio = true;

                                                    long insert_campaign_status = da.insertEmailCampaignsStatus(newsletter_status);
                                                    if (insert_campaign_status < 1)
                                                    {
                                                        //procesado = false;
                                                        LogUtils.InsertarLog(" ERROR - Se ha producido errores al guardar el status de la campaña en EMAIL_CAMPAIGNS_STATUS del usuario " + lst_user[0].Nombre_Completo, true);
                                                    }
                                                    else
                                                    {
                                                        /// 15.- Insertar una entrada en EMAIL_CAMPAIGNS_STATUS_REENVIO
                                                        EMAIL_CAMPAIGNS_STATUS_REENVIO newsletter_status_reenv = new EMAIL_CAMPAIGNS_STATUS_REENVIO();
                                                        newsletter_status_reenv.id_ecs = insert_campaign_status;
                                                        newsletter_status_reenv.fecha_reenvio = DateTime.Now;
                                                        if (activator.tipo == (int)Constantes.status_campaign.Reenvio_no_opens)
                                                            newsletter_status_reenv.tipo = (int)Constantes.type_status_mail.No_Open;
                                                        else if (activator.tipo == (int)Constantes.status_campaign.Reenvio_open)
                                                            newsletter_status_reenv.tipo = (int)Constantes.type_status_mail.Open;
                                                        else if (activator.tipo == (int)Constantes.status_campaign.Reenvio_clicks)
                                                            newsletter_status_reenv.tipo = (int)Constantes.type_status_mail.Clic;

                                                        long insert_campaign_status_reenv = da.insertEmailCampaignsStatusReenvio(newsletter_status_reenv);
                                                        if (insert_campaign_status_reenv < 1)
                                                        {
                                                            //procesado = false;
                                                            LogUtils.InsertarLog(" ERROR - Se ha producido errores al guardar el status de la campaña en EMAIL_CAMPAIGNS_STATUS_REENVIO del usuario " + lst_user[0].Nombre_Completo, true);
                                                        }
                                                        else
                                                        {
                                                            /// 16.- Sacar el dato de EMAIL_CAMPAIGNS_STATUS original
                                                            List<EMAIL_CAMPAIGNS_STATUS> lst_newsletter_status = list_newsletter_status.Where(ecs => ecs.id_s == suscriptor.id_s).ToList();
                                                            if (lst_newsletter_status.Count == 1)
                                                            {
                                                                /// 17.- Actualizar los datos de EMAIL_CAMPAIGNS_STATUS
                                                                EMAIL_CAMPAIGNS_STATUS _newsletter_status = lst_newsletter_status[0];
                                                                _newsletter_status.status = (int)Constantes.status_mail.Reenviado;
                                                                _newsletter_status.id_ecs_reenv = insert_campaign_status_reenv;

                                                                bool update_newsletter = da.updateEmailCampaignsStatus(_newsletter_status);
                                                                if (update_newsletter)
                                                                    Console.WriteLine("Suscriptor procesado " + suscriptor.nombre_completo);
                                                                else
                                                                {
                                                                    //procesado = false;
                                                                    LogUtils.InsertarLog(" ERROR - Se ha producido errores al actualizar los datos en EMAIL_CAMPAIGNS_STATUS del usuario " + lst_user[0].Nombre_Completo, true);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //procesado = false;
                                                                LogUtils.InsertarLog(" ERROR - Se ha producido errores al buscar los datos del suscriptor de la campaña en EMAIL_CAMPAIGNS_STATUS del usuario " + lst_user[0].Nombre_Completo, true);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //procesado = false;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al mandar el mail al usuario " + lst_user[0].Nombre_Completo, true);
                                            }
                                        }
                                        else
                                            LogUtils.InsertarLog(" ERROR - Se ha producido un error al recuperar los datos del usuario " + suscriptor.id_alumno, true);
                                    }
                                }
                            }
                            else
                            {
                                procesado = false;
                                LogUtils.InsertarLog(" ERROR - No hay suscriptores activos en la campaña " + list_campaigns[0].nombre, true);
                            }
                        }
                        else
                        {
                            procesado = false;
                            LogUtils.InsertarLog(" ERROR - No hay suscriptores activos en la campaña " + list_campaigns[0].nombre, true);
                        }
                    }
                }
            }

            return procesado;
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

                    if (list_href[0].Contains("&amp;"))
                    {
                        if (!list_href[0].Contains("###") && !list_href[0].Contains("mailto") && !list_href[0].Contains("email-click") && !list_href[0].Contains("tel:") && !list_urls.Contains(list_href[0].Replace("&amp;", "&")))
                            list_urls.Add(list_href[0].Replace("&amp;", "&"));
                    }
                    else
                    {
                        if (!list_href[0].Contains("###") && !list_href[0].Contains("mailto") && !list_href[0].Contains("email-click") && !list_href[0].Contains("tel:") && !list_urls.Contains(list_href[0]))
                            list_urls.Add(list_href[0]);
                    }
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
    }
}