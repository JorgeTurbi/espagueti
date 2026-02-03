using campus_sbs_admin.Models;
using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using CLIENTES = sbs_DAL.CLIENTES;

namespace campus_sbs_admin
{
    // =========================
    // NUEVO: AuthContext (puedes moverlo a Models/AuthContext.cs)
    // =========================
    public class AuthContext
    {
        public CLIENTES User { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
        public List<string> RoleNames { get; set; } = new List<string>();
        public bool IsSpecialMailUser { get; set; }
    }

    // =========================
    // NUEVO: AuthService (puedes moverlo a Services/AuthService.cs)
    // =========================
    public class AuthService
    {
        public AuthContext BuildAuthContext(CLIENTES user)
        {

            try
            {
                var auth = new AuthContext { User = user };

                using (var db = new SpainBS_Connection())
                {
                    // Roles activos del usuario (ClienteRoles -> Roles)
                    var roles = (from cr in db.ClienteRoles
                                 join r in db.Roles on cr.RoleId equals r.RoleId
                                 where cr.id_cliente == user.id_cliente && r.IsActive
                                 select new { r.RoleId, r.Name })
                                .Distinct()
                                .ToList();

                    auth.RoleIds = roles.Select(x => x.RoleId).ToList();
                    auth.RoleNames = roles.Select(x => x.Name).ToList();
                }

                // Usuarios especiales por config (mismo comportamiento que ya tienes)
                var list_users_mails = (ConfigurationManager.AppSettings["users_special_mail"] ?? "")
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                auth.IsSpecialMailUser = list_users_mails.Contains(user.id_cliente.ToString());

                return auth;

            }
            catch (Exception ex)
            {
                var full = ex.ToString();
                var inner1 = ex.InnerException?.ToString();
                var inner2 = ex.InnerException?.InnerException?.ToString();

                LogUtils.InsertarLog("AUTH ERROR BuildAuthContext()");
                LogUtils.InsertarLog(full);
                if (inner1 != null) LogUtils.InsertarLog("INNER1: " + inner1);
                if (inner2 != null) LogUtils.InsertarLog("INNER2: " + inner2);

                return null;
            }


        }
    }

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
                        var user = list_user[0];

                        // =========================
                        // CAMBIO: construir AuthContext y guardarlo en Session
                        // =========================
                        var auth = new AuthService().BuildAuthContext(user);

                        Session["usuario"] = user;     // lo mantienes si otras pantallas lo usan
                        Session["auth"] = auth;        // NUEVO

                        Cookie_Page.updateCookie(user.login, Request.Cookies["ShopOnline"]);

                        // Mantener tu lógica de redirección actual
                        if (auth.IsSpecialMailUser)
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
                else // Entramos con Login y clave
                {
                    /// 1.- Codificamos la contraseña
                    _password = Utils.toCodeString(_password);

                    /// 2.- Comprobar que el login y la contraseña son correctos
                    List<CLIENTES> list_clients = da.getCorrectUser(_login, _password);
                    if (list_clients.Count > 0)
                    {
                        var user = list_clients[0];

                        /// 3.- Comprobar que estan activos
                        if (user.fecha_baja == null && user.activo == ((int)Constantes.activo.Activo).ToString())
                        {
                            /// 4.- Comprobar que es un administrador o un comercial
                            if (user.Comercial != null && user.Comercial.Value) { _escomercial = 1; }
                            if (!String.IsNullOrEmpty(user.Administrador) && user.Administrador == ((int)Constantes.activo.Activo).ToString())
                            { _esadmin = 1; }

                            // =========================================
                            // BLOQUE COMÚN: actualizar investigación/acciones (igual que antes)
                            // Lo uso en ambos casos para evitar duplicación.
                            // =========================================
                            void UpdateResearchAndActions()
                            {
                                string cookie = Cookie_Page.getCookie(this.Page);
                                if (String.IsNullOrEmpty(cookie)) return;

                                List<sbs_DAL.campus_INVESTIGA> listInvestiga = da.getResearch(cookie);
                                if (listInvestiga.Count > 0)
                                {
                                    int scoring = listInvestiga[0].Scoring.Value;
                                    bool update_scoring = da.updateResearch(user.id_cliente, cookie, scoring);
                                    if (update_scoring)
                                    {
                                        bool update_client = da.updatePersonAction(user.id_cliente, cookie);
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

                            // =========================================
                            // CAMBIO: construir AuthContext una sola vez, guardarlo en Session
                            // =========================================
                            var auth = new AuthService().BuildAuthContext(user);
                            Session["usuario"] = user;
                            Session["auth"] = auth;
                            Cookie_Page.updateCookie(user.login, Request.Cookies["ShopOnline"]);

                            // Comercial y NO admin
                            if (user.Comercial != null && user.Comercial.Value && _esadmin == 0)
                            {
                                UpdateResearchAndActions();

                                if (auth.IsSpecialMailUser)
                                    Response.Redirect("lista-suscriptores.aspx");
                                else
                                    Response.Redirect("listado-leads-crm.aspx");
                            }
                            // Rol Administrador
                            else if (!String.IsNullOrEmpty(user.Administrador) && user.Administrador == ((int)Constantes.activo.Activo).ToString())
                            {
                                UpdateResearchAndActions();

                                if (auth.IsSpecialMailUser)
                                    Response.Redirect("lista-suscriptores.aspx");
                                else
                                    Response.Redirect("listado-leads-crm.aspx");
                            }
                            else
                            {
                                txt_error.InnerHtml = "El Usuario introducido no es un administrador o un comercial.<br /> Si es un error, por favor contacte con la escuela <a href='mailto:info@spainbs.com'>info@spainbs.com</a> o en el Telf. 91 719 10 00.";
                            }
                        }
                        else
                        {
                            txt_error.InnerHtml = "El Usuario introducido está dado de baja o se encuentra inactivo en estos momentos.<br /> Si es un error, por favor contacte con la escuela <a href='mailto:info@spainbs.com'>info@spainbs.com</a> o en el Telf. 91 719 10 00.";
                        }
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
