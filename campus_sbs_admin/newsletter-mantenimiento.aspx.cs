using sbs_blog_DAL;
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
    public partial class newsletter_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        Data_Access da_ac = new Data_Access();

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
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos del formulario
            long noticia_principal = !String.IsNullOrEmpty(id_noticia_principal.Value) ? long.Parse(id_noticia_principal.Value) : -1;
            long noticia_secundaria = !String.IsNullOrEmpty(id_noticia_secundaria_1.Value) ? long.Parse(id_noticia_secundaria_1.Value) : -1;
            long noticia_secundaria_2 = !String.IsNullOrEmpty(id_noticia_secundaria_2.Value) ? long.Parse(id_noticia_secundaria_2.Value) : -1;
            long noticia_secundaria_3 = !String.IsNullOrEmpty(id_noticia_secundaria_3.Value) ? long.Parse(id_noticia_secundaria_3.Value) : -1;
            string texto_1 = txt_text_1.Value.Trim();
            string url_1 = txt_url_1.Value.Trim();
            string texto_2 = txt_text_2.Value.Trim();
            string url_2 = txt_url_2.Value.Trim();

            /// 4.- Crear la newsletter
            EMAIL_CAMPAIGNS newsletter = new EMAIL_CAMPAIGNS();
            newsletter.nombre = "NL " + DateTime.Today.ToShortDateString().Replace('/', '-');
            newsletter.fecha_scheduled = DateTime.Today;
            newsletter.priority = 5;
            //newsletter.id_els = long.Parse(ConfigurationManager.AppSettings["lista_suscriptor_newsletter"]);
            newsletter.newsletter = true;
                        
            /// 4.1.- Comprobar si es un usuario especial
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (list_user.Count == 0 && Session["usuario"] != null)
                list_user.Add((CLIENTES)Session["usuario"]);

            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
            if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                newsletter.id_usuario = list_user[0].id_cliente;

            /// 4.2.1.- Sacar los datos de las noticias
            List<long> lst_ids = new List<long>();
            if (noticia_principal > 0)
                lst_ids.Add(noticia_principal);
            if (noticia_secundaria > 0)
                lst_ids.Add(noticia_secundaria);
            if (noticia_secundaria_2 > 0)
                lst_ids.Add(noticia_secundaria_2);
            if (noticia_secundaria_3 > 0)
                lst_ids.Add(noticia_secundaria_3);

            /// 4.2.2.- Sacar las noticias
            List<NOT_NOTICIAS> lst_noticias = da_ac.ListadoNoticiasPorId(lst_ids);

            /// 4.2.3.- Sacar las imágenes de las noticias
            List<IMG_IMAGENES> lst_imagenes = da_ac.ListadoImagenesByNoticia(lst_ids);

            /// 4.3.- Sacar el cuerpo de la newsletter
            newsletter.body = paint_template(noticia_principal, noticia_secundaria, noticia_secundaria_2, noticia_secundaria_3, texto_1, url_1, texto_2, url_2, lst_noticias, lst_imagenes);

            /// 5.- Reenviar al editor
            long insert_newsletter = da.insertEmailCampaigns(newsletter);
            if (insert_newsletter > 0)
                Response.Redirect("lista-newsletter-mantenimiento.aspx?idc=" + insert_newsletter);
            else
                txt_error.InnerHtml = "Se ha producido un error al guardar la campaña";
        }

        [WebMethod(Description = "Busca alumnos a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Noticia> search_news(string name, long noticia1, long noticia2, long noticia3, long noticia4)
        {
            Data_Access da = new Data_Access();
            List<Noticia> list_noticias = new List<Noticia>();

            /// 1.- Comprobar si hay id ya utilizados
            List<long> lst_ids = new List<long>();
            if (noticia1 > 0)
                lst_ids.Add(noticia1);
            if (noticia2 > 0)
                lst_ids.Add(noticia2);
            if (noticia3 > 0)
                lst_ids.Add(noticia3);
            if (noticia4 > 0)
                lst_ids.Add(noticia4);

            /// 2.- Sacar las noticias
            List<NOT_NOTICIAS> lst_noticias = da.ListadoNoticiasPorBusqueda(name);

            /// 3.- Filtrar por las noticias activas
            lst_noticias = lst_noticias.Where(not => not.NOT_ACTIVA).ToList();

            /// 4.- Filtrar las noticias que no tenemos
            lst_noticias = lst_noticias.Where(not => !lst_ids.Contains(not.NOT_ID)).ToList();

            /// 5.- Ordenar por fecha de publicación
            lst_noticias = lst_noticias.OrderByDescending(not => not.NOT_FECHA_PUBLICACION).ToList();

            /// 5.- Devolver el listado de noticias
            if (lst_noticias.Count > 0)
                list_noticias = lst_noticias.Select(news => new Noticia { id_noticia = news.NOT_ID, titulo = news.NOT_TITULO }).ToList();

            return list_noticias;
        }

        [WebMethod(Description = "Recupera la plantilla de la newsletter rellena")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string cargarNewsletter(long noticia_principal, long noticia_secundaria, long noticia_secundaria_2, long noticia_secundaria_3, string texto_1, string url_1, string texto_2, string url_2)
        {
            Data_Access da = new Data_Access();

            /// 1.- Sacar los datos de las noticias
            List<long> lst_ids = new List<long>();
            if (noticia_principal > 0)
                lst_ids.Add(noticia_principal);
            if (noticia_secundaria > 0)
                lst_ids.Add(noticia_secundaria);
            if (noticia_secundaria_2 > 0)
                lst_ids.Add(noticia_secundaria_2);
            if (noticia_secundaria_3 > 0)
                lst_ids.Add(noticia_secundaria_3);

            /// 2.- Sacar las noticias
            List<NOT_NOTICIAS> lst_noticias = da.ListadoNoticiasPorId(lst_ids);

            /// 3.- Sacar las imágenes de las noticias
            List<IMG_IMAGENES> lst_imagenes = da.ListadoImagenesByNoticia(lst_ids);

            string html = paint_template(noticia_principal, noticia_secundaria, noticia_secundaria_2, noticia_secundaria_3, texto_1, url_1, texto_2, url_2, lst_noticias, lst_imagenes);
            
            return html;
        }

        private static string paint_template(long noticia_principal, long noticia_secundaria, long noticia_secundaria_2, long noticia_secundaria_3, string texto_1, string url_1, string texto_2, string url_2, List<NOT_NOTICIAS> lst_noticias, List<IMG_IMAGENES> lst_imagenes)
        {   
            /// 1.- Recuperar la plantilla
            string template = Utilities.getPlantillaMail("newsletter", ConfigurationManager.AppSettings["urlTemplate"]);
            if (!String.IsNullOrEmpty(template))
            {
                /// 2.- Pintar los datos de la noticia principal
                List<NOT_NOTICIAS> lst_noticia_principal = lst_noticias.Where(not => not.NOT_ID == noticia_principal).ToList();
                if (lst_noticia_principal.Count == 1)
                {
                    /// 2.1.- Sacar las imágenes de la noticia principal
                    List<IMG_IMAGENES> lst_imagenes_principal = lst_imagenes.Where(not => not.NOT_ID == noticia_principal).ToList();

                    /// 2.2.- Sustituir los datos en la plantilla
                    template = template.Replace("###titulo_noticia_principal###", Utilities.quitarAcentos(lst_noticia_principal[0].NOT_TITULO));
                    template = template.Replace("###alt_noticia_principal###", Utilities.quitarAcentos(lst_noticia_principal[0].NOT_TITULO));
                    template = template.Replace("###descripcion_noticia_principal###", Utilities.quitarAcentos(lst_noticia_principal[0].NOT_DESC_CORTA));
                    template = template.Replace("###url_noticia_principal###", "https://blog.spainbs.com/" + lst_noticia_principal[0].NOT_FECHA_PUBLICACION.Value.Year + "/" + lst_noticia_principal[0].NOT_FECHA_PUBLICACION.Value.Month.ToString("00") + "/" + noticia_principal + "/" + lst_noticia_principal[0].NOT_META_URL);

                    string img_noticia_principal = dameImagenNoticia(lst_imagenes_principal, (int)Constantes.ImagenSituacion.Interior, (int)Constantes.ImagenResponsive.Grande);
                    if (String.IsNullOrEmpty(img_noticia_principal))
                        img_noticia_principal = dameImagenNoticia(lst_imagenes_principal, (int)Constantes.ImagenSituacion.Portada, (int)Constantes.ImagenResponsive.Grande);
                    template = template.Replace("###imagen_noticia_principal###", img_noticia_principal);
                }

                /// 3.- Pintar el bloque de noticias secundarias
                if (noticia_secundaria != -1 || noticia_secundaria_2 != -1 || noticia_secundaria_3 != -1)
                {
                    StringBuilder sbuild = new StringBuilder();

                    sbuild.Append("<tr>");
                    sbuild.Append("<td align=\"center\" valign=\"top\" id=\"templateColumns\" style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;background:#fafafa none no-repeat center/cover;border-top: 0;border-bottom: 0;padding-top: 0;padding-bottom: 0;\">");
                    sbuild.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\" style=\"background-color: #fafafa;border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;max-width: 600px !important;\">");
                    sbuild.Append("<tbody>");
                    sbuild.Append("<tr>");
                    sbuild.Append("<td valign=\"top\" style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;background-color: #fff;\">");
                    
                    List<long> lst_ids = new List<long>();
                    if (noticia_secundaria > 0)
                        lst_ids.Add(noticia_secundaria);
                    if (noticia_secundaria_2 > 0)
                        lst_ids.Add(noticia_secundaria_2);
                    if (noticia_secundaria_3 > 0)
                        lst_ids.Add(noticia_secundaria_3);

                    List<NOT_NOTICIAS> lst_noticias_secundarias = lst_noticias.Where(not => lst_ids.Contains(not.NOT_ID)).ToList();

                    /// 3.1.- Tres noticias secundarias
                    if (lst_noticias_secundarias.Count == 3)
                    {
                        sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                        sbuild.Append("<table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width: 600px;\">");
                        sbuild.Append("<tr>");

                        foreach (var not_secundaria in lst_noticias_secundarias)
                        {
                            /// 2.1.- Sacar las imágenes de la noticia principal
                            List<IMG_IMAGENES> lst_imagenes_secundarias = lst_imagenes.Where(not => not.NOT_ID == not_secundaria.NOT_ID).ToList();
                                                                                                                                        
                            sbuild.Append("<td align=\"center\" valign=\"top\" width=\"200\" style=\"width:200px;\">");
                            sbuild.Append("<![endif]-->");

                            sbuild.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"200\" class=\"columnWrapper\" style=\"background-color: #fff;border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td valign=\"top\" class=\"columnContainer\" style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnCaptionBlock\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody class=\"mcnCaptionBlockOuter\">");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnCaptionBlockInner\" valign=\"top\" style=\"padding: 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnCaptionBottomContent\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnCaptionBottomImageContent\" align=\"center\" valign=\"top\" style=\"padding: 0 9px 9px 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");

                            string img_noticia_principal = dameImagenNoticia(lst_imagenes_secundarias, (int)Constantes.ImagenSituacion.Portada, (int)Constantes.ImagenResponsive.Grande);
                            sbuild.Append("<img alt=\"" + Utilities.quitarAcentos(not_secundaria.NOT_TITULO) + "\" src=\"" + img_noticia_principal + "\" width=\"164\" style=\"max-width: 4032px;border: 0;height: auto;outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;vertical-align: bottom;\" class=\"mcnImage\" />");

                            sbuild.Append("</td></tr>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnTextContent\" valign=\"top\" style=\"padding: 0 9px 0 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;word-break: break-word;color: #202020;font-family: Helvetica;font-size: 16px;line-height: 150%;text-align: left;\" width=\"164\">");
                            sbuild.Append("<a href=\"https://blog.spainbs.com/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Year + "/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Month.ToString("00") + "/" + not_secundaria.NOT_ID + "/" + not_secundaria.NOT_META_URL + "\">");
                            sbuild.Append("<strong>" + Utilities.quitarAcentos(not_secundaria.NOT_TITULO) + "</strong>");
                            sbuild.Append("</a></td></tr>");

                            /// Botón +info
                            sbuild.Append("<tr>");
                            sbuild.Append("<td valign=\"top\" style=\"background-color: #fff;padding: 18px 0 9px;color: #aaa;font-family: Helvetica;font-size: 14px;font-weight: normal;text-align: center;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;word-break: break-word;line-height: 125%;\" width=\"164\">");
                            sbuild.Append("<a target=\"_blank\" href=\"https://blog.spainbs.com/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Year + "/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Month.ToString("00") + "/" + not_secundaria.NOT_ID + "/" + not_secundaria.NOT_META_URL + "\" style =\"background: #223266 none repeat scroll 0 0; border-color: #223266; border-radius: 3px; border-style: solid; border-width: 5px 10px; color: #efb74a; font-weight: bold; text-decoration: none;\">+ INFO</a>");
                            sbuild.Append("</td></tr>");
                            
                            sbuild.Append("</tbody></table></td></tr></tbody></table></td></tr></tbody></table>");
                            sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                            sbuild.Append("</td>");
                        }

                        sbuild.Append("</tr>");
                        sbuild.Append("</table>");
                        sbuild.Append("<![endif]-->");
                    }
                    else if (lst_noticias_secundarias.Count == 2)
                    {
                        sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                        sbuild.Append("<table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width: 600px;\">");
                        sbuild.Append("<tr>");

                        foreach (var not_secundaria in lst_noticias_secundarias)
                        {
                            /// 2.1.- Sacar las imágenes de la noticia principal
                            List<IMG_IMAGENES> lst_imagenes_secundarias = lst_imagenes.Where(not => not.NOT_ID == not_secundaria.NOT_ID).ToList();

                            sbuild.Append("<td align=\"center\" valign=\"top\" width=\"300\" style=\"width:300px;\">");
                            sbuild.Append("<![endif]-->");

                            sbuild.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"300\" class=\"columnWrapper\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td valign=\"top\" class=\"columnContainer\" style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnCaptionBlock\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody class=\"mcnCaptionBlockOuter\">");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnCaptionBlockInner\" valign=\"top\" style=\"padding: 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnCaptionBottomContent\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnCaptionBottomImageContent\" align=\"center\" valign=\"top\" style=\"padding: 0 9px 9px 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");

                            string img_noticia_principal = dameImagenNoticia(lst_imagenes_secundarias, (int)Constantes.ImagenSituacion.Portada, (int)Constantes.ImagenResponsive.Grande);
                            sbuild.Append("<img alt=\"" + Utilities.quitarAcentos(not_secundaria.NOT_TITULO) + "\" src=\"" + img_noticia_principal + "\" width=\"246\" style=\"max-width: 4032px;border: 0;height: auto;outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;vertical-align: bottom;\" class=\"mcnImage\" />");

                            sbuild.Append("</td></tr>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnTextContent\" valign=\"top\" style=\"padding: 0 9px 0 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;word-break: break-word;color: #202020;font-family: Helvetica;font-size: 16px;line-height: 150%;text-align: left;\" width=\"246\">");
                            sbuild.Append("<a href=\"https://blog.spainbs.com/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Year + "/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Month.ToString("00") + "/" + not_secundaria.NOT_ID + "/" + not_secundaria.NOT_META_URL + "\">");
                            sbuild.Append("<strong>" + Utilities.quitarAcentos(not_secundaria.NOT_TITULO) + "</strong>");
                            sbuild.Append("</a></td></tr>");

                            /// Botón +info
                            sbuild.Append("<tr>");
                            sbuild.Append("<td valign=\"top\" style=\"background-color: #fff;padding: 18px 0 9px;color: #aaa;font-family: Helvetica;font-size: 14px;font-weight: normal;text-align: center;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;word-break: break-word;line-height: 125%;\" width=\"246\">");
                            sbuild.Append("<a target=\"_blank\" href=\"https://blog.spainbs.com/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Year + "/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Month.ToString("00") + "/" + not_secundaria.NOT_ID + "/" + not_secundaria.NOT_META_URL + "\" style=\"background: #223266 none repeat scroll 0 0; border-color: #223266; border-radius: 3px; border-style: solid; border-width: 5px 10px; color: #efb74a; font-weight: bold; text-decoration: none;\">+ INFO</a>");
                            sbuild.Append("</td></tr>");

                            sbuild.Append("</tbody></table></td></tr></tbody></table></td></tr></tbody></table>");
                            sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                            sbuild.Append("</td>");
                        }

                        sbuild.Append("</tr>");
                        sbuild.Append("</table>");
                        sbuild.Append("<![endif]-->");
                    }
                    else if (lst_noticias_secundarias.Count == 1)
                    {
                        sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                        sbuild.Append("<table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width: 600px;\">");
                        sbuild.Append("<tr>");

                        foreach (var not_secundaria in lst_noticias_secundarias)
                        {
                            /// 2.1.- Sacar las imágenes de la noticia principal
                            List<IMG_IMAGENES> lst_imagenes_secundarias = lst_imagenes.Where(not => not.NOT_ID == not_secundaria.NOT_ID).ToList();

                            sbuild.Append("<td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\">");
                            sbuild.Append("<![endif]-->");

                            sbuild.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" class=\"columnWrapper\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td valign=\"top\" class=\"columnContainer\" style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnCaptionBlock\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody class=\"mcnCaptionBlockOuter\">");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnCaptionBlockInner\" valign=\"top\" style=\"padding: 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnCaptionBottomContent\" style=\"border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                            sbuild.Append("<tbody>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnCaptionBottomImageContent\" align=\"center\" valign=\"top\" style=\"padding: 0 9px 9px 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");

                            string img_noticia_principal = dameImagenNoticia(lst_imagenes_secundarias, (int)Constantes.ImagenSituacion.Portada, (int)Constantes.ImagenResponsive.Grande);
                            sbuild.Append("<img alt=\"" + Utilities.quitarAcentos(not_secundaria.NOT_TITULO) + "\" src=\"" + img_noticia_principal + "\" width=\"564\" style=\"max-width: 4032px;border: 0;height: auto;outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;vertical-align: bottom;\" class=\"mcnImage\" />");

                            sbuild.Append("</td></tr>");
                            sbuild.Append("<tr>");
                            sbuild.Append("<td class=\"mcnTextContent\" valign=\"top\" style=\"padding: 0 9px 0 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;word-break: break-word;color: #202020;font-family: Helvetica;font-size: 16px;line-height: 150%;text-align: left;\" width=\"564\">");
                            sbuild.Append("<a href=\"https://blog.spainbs.com/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Year + "/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Month.ToString("00") + "/" + not_secundaria.NOT_ID + "/" + not_secundaria.NOT_META_URL + "\">");
                            sbuild.Append("<strong>" + Utilities.quitarAcentos(not_secundaria.NOT_TITULO) + "</strong>");
                            sbuild.Append("</a></td></tr>");

                            /// Botón +info
                            sbuild.Append("<tr>");
                            sbuild.Append("<td valign=\"top\" style=\"background-color: #fff;padding: 18px 0 9px;color: #aaa;font-family: Helvetica;font-size: 14px;font-weight: normal;text-align: center;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;word-break: break-word;line-height: 125%;\" width=\"564\">");
                            sbuild.Append("<a target=\"_blank\" href=\"https://blog.spainbs.com/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Year + "/" + not_secundaria.NOT_FECHA_PUBLICACION.Value.Month.ToString("00") + "/" + not_secundaria.NOT_ID + "/" + not_secundaria.NOT_META_URL + "\" style=\"background: #223266 none repeat scroll 0 0; border-color: #223266; border-radius: 3px; border-style: solid; border-width: 5px 10px; color: #efb74a; font-weight: bold; text-decoration: none;\">+ INFO</a>");
                            sbuild.Append("</td></tr>");

                            sbuild.Append("</tbody></table></td></tr></tbody></table></td></tr></tbody></table>");
                            sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                            sbuild.Append("</td>");
                        }

                        sbuild.Append("</tr>");
                        sbuild.Append("</table>");
                        sbuild.Append("<![endif]-->");
                    }

                    sbuild.Append("</td>");
                    sbuild.Append("</tr>");
                    sbuild.Append("</tbody>");
                    sbuild.Append("</table>");
                    sbuild.Append("</td>");
                    sbuild.Append("</tr>");

                    sbuild.Append(bloque_separador("templateBody"));

                    template = template.Replace("###SECONDARY_NEWS###", sbuild.ToString());
                }
                else
                    template = template.Replace("###SECONDARY_NEWS###", string.Empty);

                /// 4.- Pintar los enlaces de próximo avance
                if (!String.IsNullOrEmpty(texto_1) || !String.IsNullOrEmpty(texto_2))
                {
                    StringBuilder sbuild = new StringBuilder();

                    sbuild.Append("<tr>");
                    sbuild.Append("<td align=\"center\" valign=\"top\" id=\"templatePreFooter\" style=\"background:#fafafa none no-repeat center/cover;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;background-color: #fafafa;background-image: none;background-repeat: no-repeat;background-position: center;background-size: cover;border-top: 0;border-bottom: 0;padding-top: 0;padding-bottom: 0;\">");
                    sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                    sbuild.Append("<table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\">");
                    sbuild.Append("<tr>");
                    sbuild.Append("<td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\">");
                    sbuild.Append("<![endif]-->");
                    sbuild.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\" style=\"background-color:#ffffff;border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;max-width: 600px !important;\">");
                    sbuild.Append("<tbody>");
                    sbuild.Append("<tr>");
                    sbuild.Append("<td valign=\"top\" class=\"footerContainer\" style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                    sbuild.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnTextBlock\" style=\"min-width: 100%;border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                    sbuild.Append("<tbody class=\"mcnTextBlockOuter\">");
                    sbuild.Append("<tr>");
                    sbuild.Append("<td valign=\"top\" class=\"mcnTextBlockInner\" style=\"padding-top: 9px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
                                        
                    sbuild.Append("<!--[if mso]>");
                    sbuild.Append("<table align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100%;\">");
                    sbuild.Append("<tr>");
                    sbuild.Append("<![endif]-->");
                    sbuild.Append("<!--[if mso]>");
                    sbuild.Append("<td valign=\"top\" width=\"600\" style=\"width: 600px;\">");
                    sbuild.Append("<![endif]-->");

                    sbuild.Append("<table align='left' border='0' cellpadding='0' cellspacing='0' style='max-width: 100%; min-width: 100%; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;' width='100%' class='mcnTextContentContainer'>");
                    sbuild.Append("<tbody>");
                    sbuild.Append("<tr>");
                    sbuild.Append("<td valign=\"top\" class=\"mcnTextContent\" style=\"padding: 0 18px; mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; word-break: break-word; color: #656565;font-family: Helvetica;font-size: 12px;line-height: 150%;text-align: center;\">");
                    sbuild.Append("<p style=\"display: block;margin: 0;padding: 0;color: #444444;font-family: Helvetica;font-size: 24px;font-style: normal;font-weight: bold;line-height: 125%;letter-spacing: normal;text-align: left;\">");
                    sbuild.Append("PR&Oacute;XIMO AVANCE");
                    sbuild.Append("</p>");

                    sbuild.Append("<ul>");

                    if (!String.IsNullOrEmpty(texto_1))
                    {
                        sbuild.Append("<li style=\"margin: 20px 0;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;color: #000;font-family: Helvetica;font-size: 14px;line-height: 150%;text-align:left;\" >");
                        if (!String.IsNullOrEmpty(url_1))
                            sbuild.Append("<a href=\"" + url_1 + "\" style=\"color: #000; font-size: 14px; font-weight: bold;\">" + Utilities.quitarAcentos(texto_1) + "</a>");
                        else
                            sbuild.Append(Utilities.quitarAcentos(texto_1));
                        sbuild.Append("</li>");
                    }

                    if (!String.IsNullOrEmpty(texto_2))
                    {
                        sbuild.Append("<li style=\"margin: 20px 0;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;color: #000;font-family: Helvetica;font-size: 14px;line-height: 150%;text-align:left;\" >");
                        if (!String.IsNullOrEmpty(url_2))
                            sbuild.Append("<a href=\"" + url_2 + "\" style=\"color: #000; font-size: 14px; font-weight: bold;\">" + Utilities.quitarAcentos(texto_2) + "</a>");
                        else
                            sbuild.Append(Utilities.quitarAcentos(texto_2));
                        sbuild.Append("</li>");
                    }

                    sbuild.Append("</ul></td></tr></tbody></table>");

                    sbuild.Append("<!--[if mso]>");
                    sbuild.Append("</td>");
                    sbuild.Append("<![endif]-->");
                    sbuild.Append("<!--[if mso]>");
                    sbuild.Append("</tr>");
                    sbuild.Append("</table>");
                    sbuild.Append("<![endif]-->");

                    sbuild.Append("</td>");
                    sbuild.Append("</tr>");
                    sbuild.Append("</tbody>");
                    sbuild.Append("</table>");
                    sbuild.Append("</td>");
                    sbuild.Append("</tr>");
                    sbuild.Append("</tbody>");
                    sbuild.Append("</table>");
                    sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
                    sbuild.Append("</td>");
                    sbuild.Append("</tr>");
                    sbuild.Append("</table>");
                    sbuild.Append("<![endif]-->");
                    sbuild.Append("</td>");
                    sbuild.Append("</tr>");

                    sbuild.Append(bloque_separador("templateBody3"));

                    template = template.Replace("###NEXT_EVENTS###", sbuild.ToString());
                }
                else
                    template = template.Replace("###NEXT_EVENTS###", string.Empty);
            }

            return template;
        }
        
        private static string dameImagenNoticia(List<IMG_IMAGENES> lImagenes, int portada, int grande)
        {
            string urlImagen = string.Empty;
            string ruta_img = ConfigurationManager.AppSettings["url_img_blog"];

            if (lImagenes.Count > 0)
                urlImagen = ruta_img + lImagenes[0].NOT_ID + "/" + lImagenes[0].IMG_NOMBRE;
            else
                urlImagen = ruta_img + "no-image.jpg";

            return urlImagen;
        }

        private static string bloque_separador(string name_id)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<tr>");
            sbuild.Append("<td align=\"center\" valign=\"top\" id=\"" + name_id + "\" style=\"background:#fafafa none no-repeat center/cover;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;background-color: #fafafa;background-image: none;background-repeat: no-repeat;background-position: center;background-size: cover;border-top: 0;border-bottom: 0;padding-top: 0;padding-bottom: 0;\">");
            sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
            sbuild.Append("<table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\">");
            sbuild.Append("<tr>");
            sbuild.Append("<td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\">");
            sbuild.Append("<![endif]-->");
            sbuild.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\" style=\"background-color: #fff;border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;max-width: 600px !important;\">");
            sbuild.Append("<tbody>");
            sbuild.Append("<tr>");
            sbuild.Append("<td valign=\"top\" class=\"bodyContainer\" style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
            sbuild.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width: 100%;border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;table-layout: fixed !important;\">");
            sbuild.Append("<tbody class=\"mcnDividerBlockOuter\">");
            sbuild.Append("<tr>");
            sbuild.Append("<td class=\"mcnDividerBlockInner\" style=\"min-width: 100%;padding: 18px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
            sbuild.Append("<table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width: 100%;border-top: 2px solid #EAEAEA;border-collapse: collapse;mso-table-lspace: 0pt;mso-table-rspace: 0pt;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
            sbuild.Append("<tbody>");
            sbuild.Append("<tr>");
            sbuild.Append("<td style=\"mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;\">");
            sbuild.Append("<span></span>");
            sbuild.Append("</td>");
            sbuild.Append("</tr>");
            sbuild.Append("</tbody>");
            sbuild.Append("</table>");
            sbuild.Append("</td>");
            sbuild.Append("</tr>");
            sbuild.Append("</tbody>");
            sbuild.Append("</table>");
            sbuild.Append("</td>");
            sbuild.Append("</tr>");
            sbuild.Append("</tbody>");
            sbuild.Append("</table>");
            sbuild.Append("<!--[if (gte mso 9)|(IE)]>");
            sbuild.Append("</td>");
            sbuild.Append("</tr>");
            sbuild.Append("</table>");
            sbuild.Append("<![endif]-->");
            sbuild.Append("</td>");
            sbuild.Append("</tr>");

            return sbuild.ToString();
        }
    }
}