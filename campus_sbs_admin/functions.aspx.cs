using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class functions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(Description = "Elimina la sesión del usuario")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool exit()
        {
            bool _exit = false;
            try
            {
                HttpContext.Current.Session.Remove("usuario");
                _exit = true;
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - functions.cs::exit()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _exit = false;
            }
            return _exit;
        }

        [WebMethod(Description = "Geolocaliza al user en la BBDD")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool getIp(string ip)
        {
            DataAccess da = new DataAccess();

            bool _validate = true;

            /// 1.- Sacar la cookie
            string cookie = getCookie();
            if (!String.IsNullOrEmpty(cookie))
            {
                try
                {
                    /// 2.- Añadir la accion persona
                    campus_ACCIONES_PERSONA action_user = new campus_ACCIONES_PERSONA();

                    /// 2.1.- Añadir los datos de geolocalización
                    action_user.Http_IP = ip;

                    /// Añadir los datos a la tabla GEO_IP
                    List<GEO_IP> lst_geo_ip = da.getGeoIP(ip);
                    if (lst_geo_ip.Count == 0)
                    {
                        GEO_IP geo = new GEO_IP();
                        geo.ip = ip;
                        geo.fecha_act = DateTime.Now;

                        bool insert_geolocation = da.insertGeoIP(geo);
                        if (!insert_geolocation)
                        {
                            LogUtils.InsertarLog(" ERROR - functions.cs::insertGeoIP()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la ip a la BBDD.");
                            _validate = false;
                        }
                    }

                    /// 2.2.- Acción y origen
                    long idOrigin = (long)Constantes.origen.Web;
                    long idAction = (long)Constantes.accion.Identificar_Ip;

                    /// 2.3.- Buscar al usuario
                    List<campus_INVESTIGA> listInvestiga = da.getResearch(cookie);
                    if (listInvestiga.Count > 0)
                    {
                        /// 2.3.1.- Añadir los campos que nos faltan al objeto action
                        action_user.idPersona = listInvestiga[0].IdPersona;
                        action_user.idAccion = idAction;
                        action_user.Fecha = DateTime.Now;
                        action_user.IdOrigen = idOrigin;
                        action_user.Cookie = cookie;
                        action_user.Scoring = 0;

                        long insert_action = da.insertPersonAction(action_user);
                        if (insert_action < 1)
                        {
                            LogUtils.InsertarLog(" ERROR - functions.cs::insertPersonAction()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la accion persona.");
                            _validate = false;
                        }
                    }
                    else
                    {
                        /// Añadir los campos que nos faltan al objeto action
                        action_user.idPersona = long.Parse(ConfigurationManager.AppSettings["Anonimo"]);
                        action_user.idAccion = idAction;
                        action_user.Fecha = DateTime.Now;
                        action_user.IdOrigen = idOrigin;
                        action_user.Cookie = cookie;
                        action_user.Scoring = 0;

                        long insert_action = da.insertPersonAction(action_user);
                        if (insert_action < 1)
                        {
                            LogUtils.InsertarLog(" ERROR - functions.cs::insertPersonAction()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la accion persona.");
                            _validate = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.InsertarLog(" ERROR - functions.cs::getIp()");
                    LogUtils.InsertarLog("- MSG:" + ex.Message);
                    LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                    _validate = false;
                }
            }

            return _validate;
        }

        private static string getCookie()
        {
            string cookie = string.Empty;

            try
            {
                if (HttpContext.Current.Request.Cookies["ShopOnline"] != null)
                {
                    if (HttpContext.Current.Request.Cookies["ShopOnline"]["session"] != null)
                        cookie = HttpContext.Current.Request.Cookies["ShopOnline"]["session"].ToString();
                    else
                    {
                        cookie = HttpContext.Current.Session.SessionID;
                        HttpContext.Current.Request.Cookies["ShopOnline"]["session"] = HttpContext.Current.Session.SessionID;
                        HttpContext.Current.Response.Cookies.Add(HttpContext.Current.Request.Cookies["ShopOnline"]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - functions.cs::getCookie()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                cookie = string.Empty;
            }

            return cookie;
        }

        #region WS Buscar usuario

        [WebMethod(Description = "Busca alumnos a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> search_student(string name)
        {
            DataAccess da = new DataAccess();

            /// 1.- Comprobar si es comercial
            List<CLIENTES> _comercial = new List<CLIENTES>();
            if (HttpContext.Current.Session["usuario"] != null)
                _comercial.Add((CLIENTES)HttpContext.Current.Session["usuario"]);

            /// 2.- Sacar los usuarios que pertenecen al comercial, si tiene activado el check de sólo tus contactos
            List<long> _id_users = new List<long>();
            if (_comercial.Count == 1 && _comercial[0].Comercial != null && _comercial[0].Comercial.Value)
            {
                List<campus_ACL> _acl = da.getACL(_comercial[0].id_cliente);
                if (_acl.Count == 1)
                {
                    if (!_acl[0].Todos_alumnos)
                    {
                        /// 2.1.- Buscar los usuarios de la tabla CLIENTES_TAG
                        List<CLIENTES_TAG> _tags = da.getClientTags(_comercial[0].login.ToUpper(), "pertenencia_comercial", _comercial[0].id_cliente.ToString());
                        _id_users = _tags.Select(_ => _.idUser).Distinct().ToList();
                    }
                }
            }

            /// 3.- Buscar los usuarios
            List<Usuarios> lst_users = new List<Usuarios>();
            List<CLIENTES> _users = da.getUserBySearch(name, _id_users, null);
            if (_users.Count > 0)
                lst_users = _users.Select(_user => new Usuarios
                {
                    id_usuario = _user.id_cliente,
                    nombre_completo = search_data_user(_user)
                }).ToList();

            return lst_users;
        }
        private static string search_data_user(CLIENTES _user)
        {
            if (_user.Comercial != null && _user.Comercial.Value)
            {
                if (_user.fecha_baja != null)
                    return "<i class='fas fa-user-tie text-color-red'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else if (!String.IsNullOrEmpty(_user.activo) && _user.activo == ((int)Constantes.activo.NoActivo).ToString())
                    return "<i class='fas fa-user-tie text-color-orange'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else
                    return "<i class='fas fa-user-tie text-color-green'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
            }
            else if (!String.IsNullOrEmpty(_user.Administrador) && _user.Administrador == ((int)Constantes.activo.Activo).ToString())
            {
                if (_user.fecha_baja != null)
                    return "<i class='fas fa-user-cog text-color-red'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else if (!String.IsNullOrEmpty(_user.activo) && _user.activo == ((int)Constantes.activo.NoActivo).ToString())
                    return "<i class='fas fa-user-cog text-color-orange'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else
                    return "<i class='fas fa-user-cog text-color-green'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
            }
            else if (!String.IsNullOrEmpty(_user.Profesor) && _user.Profesor == ((int)Constantes.activo.Activo).ToString())
            {
                if (_user.fecha_baja != null)
                    return "<i class='fas fa-chalkboard-teacher text-color-red'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else if (!String.IsNullOrEmpty(_user.activo) && _user.activo == ((int)Constantes.activo.NoActivo).ToString())
                    return "<i class='fas fa-chalkboard-teacher text-color-orange'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else
                    return "<i class='fas fa-chalkboard-teacher text-color-green'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
            }
            else
            {
                if (_user.fecha_baja != null)
                    return "<i class='fas fa-user text-color-red'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else if (!String.IsNullOrEmpty(_user.activo) && _user.activo == ((int)Constantes.activo.NoActivo).ToString())
                    return "<i class='fas fa-user text-color-orange'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
                else
                    return "<i class='fas fa-user text-color-green'></i> " + _user.Nombre_Completo + " (" + _user.id_cliente + ")";
            }
        }

        #endregion
    }
}