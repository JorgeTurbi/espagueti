using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class email_click : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            /// 1.- Recuperar los parámetros de la url codificada
            string encoded_parameters = !String.IsNullOrEmpty(Request.QueryString["ep"]) ? Utils.toDecodeString(Utils.ValidarQueryStringCifrada(Request.QueryString["ep"])) : string.Empty;
            if (!String.IsNullOrEmpty(encoded_parameters))
            {
                /// 2.- Obtener los parámetros
                List<string> lst_parameters = encoded_parameters.Split('&').ToList();
                if (lst_parameters.Count > 0)
                {
                    long id_click = -1;
                    long id_action = -1;
                    long id_origin = -1;
                    long id_user = -1;
                    long id_mail = -1;
                    string utm_source = string.Empty;
                    string utm_medium = string.Empty;
                    string utm_campaign = string.Empty;
                    string utm_content = string.Empty;

                    foreach (var parameter in lst_parameters)
                    {
                        string param_name = parameter.Split('=')[0];
                        switch (param_name)
                        {
                            case "id_click":
                                id_click = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "ida":
                                id_action = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "ido":
                                id_origin = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "idu":
                                id_user = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "idl":
                                id_mail = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "utm_source":
                                utm_source = parameter.Split('=')[1];
                                break;
                            case "utm_medium":
                                utm_medium = parameter.Split('=')[1];
                                break;
                            case "utm_campaign":
                                utm_campaign = parameter.Split('=')[1];
                                break;
                            case "utm_content":
                                utm_content = parameter.Split('=')[1];
                                break;
                        }
                    }

                    /// 3.- Comprobar que vienen todos los parámetros
                    if (id_click == -1 || id_action == -1 || id_origin == -1 || id_user == -1 || id_mail == -1 || String.IsNullOrEmpty(utm_source) || String.IsNullOrEmpty(utm_medium) || String.IsNullOrEmpty(utm_campaign) || String.IsNullOrEmpty(utm_content))
                    {
                        LogUtils.InsertarLog(" ERROR - email-click.cs::encoded_parameters()");
                        LogUtils.InsertarLog("- MSG: Se ha producido un error al recibir los parámetros de la url, los parámetros son los siguientes: id_click: " + id_click + ", id_action: " + id_action + ", id_origin: " + id_origin + ", id_user: " + id_user + ", id_mail: " + id_mail + ", utm_source: " + utm_source + ", utm_medium: " + utm_medium + ", utm_campaign: " + utm_campaign + ", utm_content: " + utm_content);
                    }
                    else
                    {
                        /// 4.- Añadir la cookie y la visita
                        Cookie_Page.ComprobarCookie(this.Page, id_action, id_origin, -1, id_user, id_mail, utm_source, utm_medium, utm_campaign, id_click.ToString(), utm_content);

                        /// 5.- Redireccionar a la url deseada
                        List<EMAIL_CLICKS> lst_clicks = da.getEmailClicksById(id_click);
                        if (lst_clicks.Count == 1)
                            Response.Redirect(lst_clicks[0].url_destino);
                    }
                }
            }*/
        }

        protected void btn_redireccionar_Click(object sender, EventArgs e)
        {
            /// 1.- Recuperar los parámetros de la url codificada
            string encoded_parameters = !String.IsNullOrEmpty(Request.QueryString["ep"]) ? Utils.toDecodeString(Utils.ValidarQueryStringCifrada(Request.QueryString["ep"])) : string.Empty;
            if (!String.IsNullOrEmpty(encoded_parameters))
            {
                /// 2.- Obtener los parámetros
                List<string> lst_parameters = encoded_parameters.Split('&').ToList();
                if (lst_parameters.Count > 0)
                {
                    long id_click = -1;
                    long id_action = -1;
                    long id_origin = -1;
                    long id_user = -1;
                    long id_mail = -1;
                    string utm_source = string.Empty;
                    string utm_medium = string.Empty;
                    string utm_campaign = string.Empty;
                    string utm_content = string.Empty;

                    foreach (var parameter in lst_parameters)
                    {
                        string param_name = parameter.Split('=')[0];
                        switch (param_name)
                        {
                            case "id_click":
                                id_click = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "ida":
                                id_action = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "ido":
                                id_origin = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "idu":
                                id_user = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "idl":
                                id_mail = long.Parse(parameter.Split('=')[1]);
                                break;
                            case "utm_source":
                                utm_source = parameter.Split('=')[1];
                                break;
                            case "utm_medium":
                                utm_medium = parameter.Split('=')[1];
                                break;
                            case "utm_campaign":
                                utm_campaign = parameter.Split('=')[1];
                                break;
                            case "utm_content":
                                utm_content = parameter.Split('=')[1];
                                break;
                        }
                    }

                    /// 3.- Comprobar que vienen todos los parámetros
                    if (id_click == -1 || id_action == -1 || id_origin == -1 || id_user == -1 || id_mail == -1 || String.IsNullOrEmpty(utm_source) || String.IsNullOrEmpty(utm_medium) || String.IsNullOrEmpty(utm_campaign) || String.IsNullOrEmpty(utm_content))
                    {
                        LogUtils.InsertarLog(" ERROR - email-click.cs::encoded_parameters()");
                        LogUtils.InsertarLog("- MSG: Se ha producido un error al recibir los parámetros de la url, los parámetros son los siguientes: id_click: " + id_click + ", id_action: " + id_action + ", id_origin: " + id_origin + ", id_user: " + id_user + ", id_mail: " + id_mail + ", utm_source: " + utm_source + ", utm_medium: " + utm_medium + ", utm_campaign: " + utm_campaign + ", utm_content: " + utm_content);
                    }
                    else
                    {
                        /// 4.- Añadir la cookie y la visita
                        Cookie_Page.ComprobarCookie(this.Page, id_action, id_origin, -1, id_user, id_mail, utm_source, utm_medium, utm_campaign, id_click.ToString(), utm_content);

                        /// 5.- Redireccionar a la url deseada
                        List<EMAIL_CLICKS> lst_clicks = da.getEmailClicksById(id_click);
                        if (lst_clicks.Count == 1)
                        {
                            if (lst_clicks[0].url_destino.Contains("amasinfo.aspx"))
                            {
                                if (!String.IsNullOrEmpty(utm_content))
                                {
                                    List<EMAIL_CAMPAIGNS> lst_campaigns = da.getCampaignsById(long.Parse(utm_content));
                                    if (lst_campaigns.Count == 1)
                                    {
                                        string _url_cod = lst_clicks[0].url_destino.Split('?')[0];
                                        string _parameters = lst_clicks[0].url_destino.Split('?')[1];

                                        /// 6.- Recuperar los paramétros
                                        List<string> lst_parametros = _parameters.Split('&').ToList();
                                        if (lst_parametros.Count > 0)
                                        {
                                            long id_course = -1;
                                            string utm_term = string.Empty;

                                            foreach (var _parameter in lst_parametros)
                                            {
                                                string param_name = _parameter.Split('=')[0];
                                                switch (param_name)
                                                {
                                                    case "idc":
                                                        id_course = long.Parse(_parameter.Split('=')[1]);
                                                        break;
                                                    case "utm_term":
                                                        utm_content = _parameter.Split('=')[1];
                                                        break;
                                                }
                                            }

                                            string _utm_term = string.Empty;
                                            if (!String.IsNullOrEmpty(utm_term))
                                            {
                                                _utm_term = utm_term;
                                                if (!String.IsNullOrEmpty(lst_campaigns[0].tags_clic))
                                                    _utm_term = _utm_term + "," + lst_campaigns[0].tags_clic;
                                            }
                                            else if (!String.IsNullOrEmpty(lst_campaigns[0].tags_clic))
                                                _utm_term = lst_campaigns[0].tags_clic;

                                            string url = _url_cod + "?idc=" + id_course + "&ida=99&ido=248&idu=" + id_user + "&utm_source=email&utm_medium=email&utm_campaign=" + utm_campaign + "&utm_content=" + utm_content + (!String.IsNullOrEmpty(_utm_term) ? "&utm_term=" + _utm_term : string.Empty);
                                            Response.Redirect(url);
                                        }
                                    }
                                }
                                else
                                    Response.Redirect(lst_clicks[0].url_destino);
                            }
                            else
                                Response.Redirect(lst_clicks[0].url_destino);
                        }
                    }
                }
            }
        }
    }
}