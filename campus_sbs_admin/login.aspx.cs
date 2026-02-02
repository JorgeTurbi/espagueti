using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class login : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Cookie
                Cookie_Page.ComprobarCookie(this.Page, null, null, null, null);
                
                /// 2.- Comprobar si viene el parámetro 'k' en la url
                if (!String.IsNullOrEmpty(Request.QueryString["k"]))
                {
                    List<CLIENTES> list_user = da.getUserByKey(Request.QueryString["k"]);
                    if (list_user.Count > 0)
                    {
                        Session.Add("usuario", list_user[0]);
                        Cookie_Page.updateCookie(list_user[0].login, Request.Cookies["ShopOnline"]);

                        /// 3.- Comprobar si es un usuario de envío de mails especiales
                        List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                        long id_user_special = -1;

                        if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                            id_user_special = list_user[0].id_cliente;

                        if (id_user_special > 0)
                            Response.Redirect("lista-suscriptores.aspx");
                        else
                            Response.Redirect("empresas.aspx");
                    }
                }
            }
        }

        protected void btn_Acceder_Click(object sender, EventArgs e)
        {
            string _login = login_user.Value.Trim();
            string _password = password_user.Value.Trim();

            int _esadmin = 0;
            int _escomercial = 0;

            if (String.IsNullOrEmpty(_login) || String.IsNullOrEmpty(_password))
            {
                txt_error.InnerHtml = "El nombre de usuario o contraseña que proporcionó es incorrecto. Inténtalo de nuevo.";
                if (String.IsNullOrEmpty(_login))
                    user_login.Attributes["class"] = user_login.Attributes["class"].Insert(user_login.Attributes["class"].Length, " has-error");
                if (String.IsNullOrEmpty(_password))
                    user_password.Attributes["class"] = user_password.Attributes["class"].Insert(user_password.Attributes["class"].Length, " has-error");
            }
            else
            {
                if (!Utils.esStringCorrecto(_login) || !Utils.esStringCorrecto(_password))
                {
                    txt_error.InnerHtml = "El nombre de usuario o contraseña que proporcionó es incorrecto. Inténtalo de nuevo.";

                    LogUtils.InsertarLog(" ERROR - String Incorrecto en login_miperfil.aspx::btn_Acceder_Click()");
                    LogUtils.InsertarLog("- STRING:" + _login);
                    LogUtils.InsertarLog("- STRING:" + _password);

                    user_login.Attributes["class"] = user_login.Attributes["class"].Replace(" has-error", "");
                    user_password.Attributes["class"] = user_password.Attributes["class"].Replace(" has-error", "");

                    if (!Utils.esStringCorrecto(_login))
                        user_login.Attributes["class"] = user_login.Attributes["class"].Insert(user_login.Attributes["class"].Length, " has-error");
                    if (!Utils.esStringCorrecto(_password))
                        user_password.Attributes["class"] = user_password.Attributes["class"].Insert(user_password.Attributes["class"].Length, " has-error");
                }
                else     // Entramos con Login y clave
                {                    
                    /// 1.- Codificamos la contraseña
                    _password = Utils.toCodeString(_password);

                    /// 2.- Comprobar que el login y la contraseña son correctos
                    List<CLIENTES> list_clients = da.getCorrectUser(_login, _password);
                    if (list_clients.Count > 0)
                    {
                        /// 3.- Comprobar que estan activos
                        if (list_clients[0].fecha_baja == null && list_clients[0].activo == ((int)Constantes.activo.Activo).ToString())
                        {
                            /// 4.- Comprobar que es un administrador o un comercial
                            /// 

                            if (list_clients[0].Comercial != null && list_clients[0].Comercial.Value)
                            { _escomercial = 1; }
                            if (!String.IsNullOrEmpty(list_clients[0].Administrador) && list_clients[0].Administrador == ((int)Constantes.activo.Activo).ToString())
                            { _esadmin = 1; }


                                // Comercial y NO admin
                            if (list_clients[0].Comercial != null && list_clients[0].Comercial.Value && _esadmin == 0)
                            {
                                /// 4.1.- Actualizar las acciones persona con el id del usuario
                                string cookie = Cookie_Page.getCookie(this.Page);
                                if (!String.IsNullOrEmpty(cookie))
                                {
                                    /// 4.2.- Recuperar la investigación
                                    List<campus_INVESTIGA> listInvestiga = da.getResearch(cookie);
                                    if (listInvestiga.Count > 0)
                                    {
                                        int scoring = listInvestiga[0].Scoring.Value;
                                        bool update_scoring = da.updateResearch(list_clients[0].id_cliente, cookie, scoring);
                                        if (update_scoring)
                                        {
                                            /// 4.3.- Actualizar en Acciones Persona el IdPersona a partir de su cookie
                                            bool update_client = da.updatePersonAction(list_clients[0].id_cliente, cookie);
                                            if (!update_client)
                                            {
                                                LogUtils.InsertarLog(" ERROR - login.cs::btn_Acceder_Click()");
                                                LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar las acciones persona.");
                                            }
                                        }
                                        else
                                        {
                                            LogUtils.InsertarLog(" ERROR - login.cs::btn_Acceder_Click()");
                                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar la investigación.");
                                        }
                                    }
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - login.cs::btn_Acceder_Click()");
                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al buscar la investigación.");
                                    }
                                }

                                Session.Add("usuario", list_clients[0]);
                                Cookie_Page.updateCookie(list_clients[0].login, Request.Cookies["ShopOnline"]);

                                List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                                long id_user_special = -1;

                                if (list_users_mails.Contains(list_clients[0].id_cliente.ToString()))
                                    id_user_special = list_clients[0].id_cliente;

                                if (id_user_special > 0)
                                    Response.Redirect("lista-suscriptores.aspx");
                                else
                                    Response.Redirect("listado-leads-crm.aspx");
                            }
                            // Rol Administrador
                            else if (!String.IsNullOrEmpty(list_clients[0].Administrador) && list_clients[0].Administrador == ((int)Constantes.activo.Activo).ToString())
                            {
                                /// 4.1.- Actualizar las acciones persona con el id del usuario
                                string cookie = Cookie_Page.getCookie(this.Page);
                                if (!String.IsNullOrEmpty(cookie))
                                {
                                    /// 4.2.- Recuperar la investigación
                                    List<campus_INVESTIGA> listInvestiga = da.getResearch(cookie);
                                    if (listInvestiga.Count > 0)
                                    {
                                        int scoring = listInvestiga[0].Scoring.Value;
                                        bool update_scoring = da.updateResearch(list_clients[0].id_cliente, cookie, scoring);
                                        if (update_scoring)
                                        {
                                            /// 4.3.- Actualizar en Acciones Persona el IdPersona a partir de su cookie
                                            bool update_client = da.updatePersonAction(list_clients[0].id_cliente, cookie);
                                            if (!update_client)
                                            {
                                                LogUtils.InsertarLog(" ERROR - login.cs::btn_Acceder_Click()");
                                                LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar las acciones persona.");
                                            }
                                        }
                                        else
                                        {
                                            LogUtils.InsertarLog(" ERROR - login.cs::btn_Acceder_Click()");
                                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar la investigación.");
                                        }
                                    }
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - login.cs::btn_Acceder_Click()");
                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al buscar la investigación.");
                                    }
                                }

                                Session.Add("usuario", list_clients[0]);

                                Cookie_Page.updateCookie(list_clients[0].login, Request.Cookies["ShopOnline"]);

                                List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                                long id_user_special = -1;

                                if (list_users_mails.Contains(list_clients[0].id_cliente.ToString()))
                                    id_user_special = list_clients[0].id_cliente;

                                if (id_user_special > 0)
                                    Response.Redirect("lista-suscriptores.aspx");
                                else
                                    Response.Redirect("listado-leads-crm.aspx");
                            }
                            else
                                txt_error.InnerHtml = "El Usuario introducido no es un administrador o un comercial.<br /> Si es un error, por favor contacte con la escuela <a href='mailto:info@spainbs.com'>info@spainbs.com</a> o en el Telf. 91 719 10 00.";
                        }
                        else
                            txt_error.InnerHtml = "El Usuario introducido está dado de baja o se encuentra inactivo en estos momentos.<br /> Si es un error, por favor contacte con la escuela <a href='mailto:info@spainbs.com'>info@spainbs.com</a> o en el Telf. 91 719 10 00.";
                    }
                    else
                    {
                        txt_error.InnerHtml = "El nombre de usuario o contraseña que proporcionó es incorrecto. Inténtalo de nuevo.";

                        user_login.Attributes["class"] = user_login.Attributes["class"].Replace(" has-error", "");
                        user_password.Attributes["class"] = user_password.Attributes["class"].Replace(" has-error", "");

                        user_login.Attributes["class"] = user_login.Attributes["class"].Insert(user_login.Attributes["class"].Length, " has-error");
                        user_password.Attributes["class"] = user_password.Attributes["class"].Insert(user_password.Attributes["class"].Length, " has-error");
                    }
                }
            }
        }

      

    }
}