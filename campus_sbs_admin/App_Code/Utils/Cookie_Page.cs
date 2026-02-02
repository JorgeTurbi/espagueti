using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace campus_sbs_admin
{
    public class Cookie_Page
    {
        public static void ComprobarCookie(Page page, long? idAction, long? idOrigin, long? idUser, long? idCourse)
        {
            string cookie = getCookie(page);
            if (!String.IsNullOrEmpty(cookie))
                searchIdCliente(cookie, page, idAction, idOrigin, idUser, idCourse, Constantes.accion.Visita);
            else
            {
                CrearCookie(page);
                ComprobarCookie(page, idAction, idOrigin, idUser, idCourse);
            }
        }

        public static void ComprobarCookie(Page page, long idAction, long idOrigin, long idCourse, long id_user, long idLanding, string utm_source, string utm_medium, string utm_campaign, string utm_term, string utm_content)
        {
            try
            {
                string cookie = getCookie(page);
                if (!String.IsNullOrEmpty(cookie))
                {
                    searchIdCliente(cookie, page, idAction, idOrigin, idCourse, id_user, idLanding, utm_source, utm_medium, utm_campaign, utm_term, utm_content);
                }
                else
                {
                    CrearCookie(page);
                    ComprobarCookie(page, idAction, idOrigin, idCourse, id_user, idLanding, utm_source, utm_medium, utm_campaign, utm_term, utm_content);
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - sbs_ku.cs::ComprobarCookie()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }
        
        public static void ComprobarCookieRegister(Page page, long? idAction, long? idOrigin, long? idUser, long? idCourse, string comentarios)
        {
            string cookie = getCookie(page);
            if (!String.IsNullOrEmpty(cookie))
                searchIdClienteRegister(cookie, page, idAction, idOrigin, idUser, idCourse, Constantes.accion.Visita, comentarios);
            else
            {
                CrearCookie(page);
                ComprobarCookie(page, idAction, idOrigin, idUser, idCourse);
            }
        }

        /// <summary>
        /// Añade la visita a la BBDD
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="page"></param>
        /// <param name="idAction"></param>
        /// <param name="idOrigin"></param>
        /// <param name="idUser"></param>
        /// <param name="idCourse"></param>
        /// <param name="accion"></param>
        private static void searchIdCliente(string cookie, Page page, long? idAction, long? idOrigin, long? idUser, long? idCourse, Constantes.accion accion)
        {
            DataAccess da = new DataAccess();

            try
            {
                /// 1.- Generar un objeto de campus_ACCIONES_PERSONA
                campus_ACCIONES_PERSONA action = new campus_ACCIONES_PERSONA();

                /// 1.1.- Recuperar las variables del servidor
                string server_variables = !String.IsNullOrEmpty(page.Request.ServerVariables["ALL_HTTP"]) ? page.Request.ServerVariables["ALL_HTTP"] : string.Empty;
                if (!String.IsNullOrEmpty(server_variables))
                    action = Utils.loadParamsServer(server_variables);

                /// 1.2.- Si no viene una acción, poner la acción por defecto
                if (idAction == null || idAction.Value < 1)
                    action.idAccion = (long)accion;
                else
                    action.idAccion = idAction.Value;

                /// 1.3.- Origen
                if (idOrigin == null || idOrigin.Value < 1)
                {
                    if (page.Session["idOrigin"] != null && (long)page.Session["idOrigin"] > 0)
                        action.IdOrigen = (long)page.Session["idOrigin"];
                    else
                        action.IdOrigen = (long)Constantes.origen.Web;
                }
                else
                    action.IdOrigen = idOrigin.Value;

                /// 2.- Scoring por la visita
                int scoring = (int)Constantes.scoring.scoring_Pag;

                /// 3.- Comprobar si hay una entrada en campus_INVESTIGA con la misma cookie
                List<campus_INVESTIGA> list_research = da.getResearch(cookie);
                if (list_research.Count > 0)
                {
                    /// 4.- Añadir los campos que nos faltan al objeto action

                    /// 4.1.- idUser
                    if (idUser != null && idUser.Value > 0 && list_research[0].IdPersona != idUser.Value)
                        action.idPersona = idUser.Value;
                    else
                        action.idPersona = list_research[0].IdPersona;

                    /// 4.2.- Fecha
                    action.Fecha = DateTime.Now;

                    /// 4.3.- idCurso
                    if (idCourse != null && idCourse.Value > 0)
                        action.IdCurso = idCourse;

                    /// 4.4.- Cookie
                    action.Cookie = cookie;

                    /// 4.5.- Scoring
                    action.Scoring = scoring;

                    /// 5.- Insertar la acción
                    long insert_action = da.insertPersonAction(action);
                    if (insert_action > 0)
                    {
                        if (idUser != null && idUser.Value > 0 && list_research[0].IdPersona != idUser.Value)
                        {
                            if (list_research[0].IdPersona == long.Parse(ConfigurationManager.AppSettings["Anonimo"]))
                                scoring = list_research[0].Scoring.Value + scoring;
                        }
                        else
                            scoring = list_research[0].Scoring.Value + scoring;

                        /// 6.- Actualizar la entrada en la tabla campus_INVESTIGA de la BBDD
                        bool update_res = da.updateResearch(action.idPersona, cookie, scoring);
                        if (update_res)
                        {
                            /// Actualizar en Acciones Persona el IdPersona a partir de su cookie
                            bool update_client = da.updatePersonAction(action.idPersona, cookie);
                            if (!update_client)
                            {
                                LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::updatePersonAction()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar las acciones persona.");
                            }
                        }
                        else
                        {
                            LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::updateResearch()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar la entrada en la tabla campus_INVESTIGA de la BBDD.");
                        }
                    }
                    else
                    {
                        LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::insertPersonAction()");
                        LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la acciones persona en la BBDD.");
                    }
                }
                else
                {
                    /// 4.- Añadir los campos que nos faltan al objeto action

                    /// 4.1.- idUser
                    if (idUser != null && idUser.Value > 0)
                        action.idPersona = idUser.Value;
                    else
                        action.idPersona = long.Parse(ConfigurationManager.AppSettings["Anonimo"]);

                    /// 4.2.- Fecha
                    action.Fecha = DateTime.Now;

                    /// 4.3.- idCurso
                    if (idCourse != null && idCourse.Value > 0)
                        action.IdCurso = idCourse;

                    /// 4.4.- Cookie
                    action.Cookie = cookie;

                    /// 4.5.- Scoring
                    action.Scoring = scoring;

                    /// 5.- Insertar la acción
                    long insert_action = da.insertPersonAction(action);
                    if (insert_action > 0)
                    {
                        /// 6.- Insertar la entrada en la tabla campus_INVESTIGA de la BBDD
                        long insert_research = da.insertResearch(action.idPersona, cookie, scoring);
                        if (insert_research < 1)
                        {
                            LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::insertResearch()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir una entrada en la tabla campus_INVESTIGA de la BBDD.");
                        }
                    }
                    else
                    {
                        LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::insertPersonAction()");
                        LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la acciones persona en la BBDD.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::searchIdCliente()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }

        private static void searchIdCliente(string cookie, Page page, long idAction, long idOrigin, long idCourse, long id_user, long idLanding, string utm_source, string utm_medium, string utm_campaign, string utm_term, string utm_content)
        {
            DataAccess da = new DataAccess();

            /// Recuperar las variables del servidor
            campus_ACCIONES_PERSONA action = new campus_ACCIONES_PERSONA();
            string server_variables = !String.IsNullOrEmpty(page.Request.ServerVariables["ALL_HTTP"]) ? page.Request.ServerVariables["ALL_HTTP"] : string.Empty;
            if (!String.IsNullOrEmpty(server_variables))
                action = Utils.loadParamsServer(server_variables);

            if (idLanding > 0)
                action.idLanding = idLanding;

            /// Parámetros de Google
            if (!String.IsNullOrEmpty(utm_source))
                action.utm_source = utm_source;
            if (!String.IsNullOrEmpty(utm_medium))
                action.utm_medium = utm_medium;
            if (!String.IsNullOrEmpty(utm_campaign))
                action.utm_campaign = utm_campaign;
            if (!String.IsNullOrEmpty(utm_term))
                action.utm_term = utm_term;
            if (!String.IsNullOrEmpty(utm_content))
                action.utm_content = utm_content;

            try
            {
                /// Ponemos el scoring por la visita
                int scoring = (int)Constantes.scoring.scoring_Pag;

                List<campus_INVESTIGA> listInvestiga = da.getResearch(cookie);
                if (listInvestiga.Count > 0)
                {
                    /// Añadir los campos que nos faltan al objeto action
                    if (id_user > 0)
                        action.idPersona = id_user;
                    else
                        action.idPersona = listInvestiga[0].IdPersona;

                    if (idAction > -1)
                        action.idAccion = idAction;
                    else
                        action.idAccion = (int)Constantes.accion.Visita;

                    if (idOrigin > -1)
                        action.IdOrigen = idOrigin;
                    else
                        action.IdOrigen = (int)Constantes.origen.Web;

                    if (idCourse > -1)
                        action.IdCurso = idCourse;

                    action.Fecha = DateTime.Now;
                    action.Cookie = cookie;
                    action.Scoring = scoring;

                    long insert_action = da.insertPersonAction(action);
                    if (insert_action > 0)
                    {
                        scoring = Convert.ToInt32(listInvestiga[0].Scoring) + scoring;
                        bool update_res = da.updateResearch(action.idPersona, cookie, scoring);
                        if (!update_res)
                        {
                            LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::updateResearch()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar los datos en campus_INVESTIGA");
                        }
                        else
                        {
                            /// Actualizar en Acciones Persona el IdPersona a partir de su cookie
                            bool update_client = da.updatePersonAction(action.idPersona, cookie);
                            if (!update_client)
                            {
                                LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::updatePersonAction()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar las acciones persona.");
                            }
                        }
                    }
                }
                else
                {
                    /// Añadir los campos que nos faltan al objeto action
                    if (id_user > 0)
                        action.idPersona = id_user;
                    else
                        action.idPersona = listInvestiga[0].IdPersona;

                    if (idAction > -1)
                        action.idAccion = idAction;
                    else
                        action.idAccion = (int)Constantes.accion.Visita;

                    if (idOrigin > -1)
                        action.IdOrigen = idOrigin;
                    else
                        action.IdOrigen = (int)Constantes.origen.Web;

                    if (idCourse > -1)
                        action.IdCurso = idCourse;

                    action.Fecha = DateTime.Now;
                    action.Cookie = cookie;
                    action.Scoring = scoring;

                    long insert_action = da.insertPersonAction(action);
                    if (insert_action > 0)
                    {
                        long insert_res = da.insertResearch(action.idPersona, cookie, scoring);
                        if (insert_res < 0)
                        {
                            LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::insertResearch()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir los datos a campus_INVESTIGA");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::searchIdCliente()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }

        private static void searchIdClienteRegister(string cookie, Page page, long? idAction, long? idOrigin, long? idUser, long? idCourse, Constantes.accion accion, string comentarios)
        {
            DataAccess da = new DataAccess();

            try
            {
                /// 1.- Generar un objeto de campus_ACCIONES_PERSONA
                campus_ACCIONES_PERSONA action = new campus_ACCIONES_PERSONA();

                /// 1.1.- Recuperar las variables del servidor
                string server_variables = !String.IsNullOrEmpty(page.Request.ServerVariables["ALL_HTTP"]) ? page.Request.ServerVariables["ALL_HTTP"] : string.Empty;
                if (!String.IsNullOrEmpty(server_variables))
                    action = Utils.loadParamsServer(server_variables);

                /// 1.2.- Si no viene una acción, poner la acción por defecto
                if (idAction == null || idAction.Value < 1)
                    action.idAccion = (long)accion;
                else
                    action.idAccion = idAction.Value;

                /// 1.3.- Origen
                if (idOrigin == null || idOrigin.Value < 1)
                {
                    if (page.Session["idOrigin"] != null && (long)page.Session["idOrigin"] > 0)
                        action.IdOrigen = (long)page.Session["idOrigin"];
                    else
                        action.IdOrigen = (long)Constantes.origen.Web;
                }
                else
                    action.IdOrigen = idOrigin.Value;

                /// 1.4.- Comentarios
                if (!String.IsNullOrEmpty(comentarios))
                    action.Comentario = comentarios;

                /// 2.- Scoring por el registro
                int scoring = (int)Constantes.scoring.scoring_Registro;

                /// 3.- Comprobar si hay una entrada en campus_INVESTIGA con la misma cookie
                List<campus_INVESTIGA> list_research = da.getResearch(cookie);
                if (list_research.Count > 0)
                {
                    /// 4.- Añadir los campos que nos faltan al objeto action

                    /// 4.1.- idUser
                    if (idUser != null && idUser.Value > 0 && list_research[0].IdPersona != idUser.Value)
                        action.idPersona = idUser.Value;
                    else
                        action.idPersona = list_research[0].IdPersona;

                    /// 4.2.- Fecha
                    action.Fecha = DateTime.Now;

                    /// 4.3.- idCurso
                    if (idCourse != null && idCourse.Value > 0)
                        action.IdCurso = idCourse;

                    /// 4.4.- Cookie
                    action.Cookie = cookie;

                    /// 4.5.- Scoring
                    action.Scoring = scoring;

                    /// 5.- Insertar la acción
                    long insert_action = da.insertPersonAction(action);
                    if (insert_action > 0)
                    {
                        if (idUser != null && idUser.Value > 0 && list_research[0].IdPersona != idUser.Value)
                        {
                            if (list_research[0].IdPersona == long.Parse(ConfigurationManager.AppSettings["Anonimo"]))
                                scoring = list_research[0].Scoring.Value + scoring;
                        }
                        else
                            scoring = list_research[0].Scoring.Value + scoring;

                        /// 6.- Actualizar la entrada en la tabla campus_INVESTIGA de la BBDD
                        bool update_res = da.updateResearch(action.idPersona, cookie, scoring);
                        if (update_res)
                        {
                            /// Actualizar en Acciones Persona el IdPersona a partir de su cookie
                            bool update_client = da.updatePersonAction(action.idPersona, cookie);
                            if (!update_client)
                            {
                                LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::updatePersonAction()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar las acciones persona.");
                            }
                        }
                        else
                        {
                            LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::updateResearch()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar la entrada en la tabla campus_INVESTIGA de la BBDD.");
                        }
                    }
                    else
                    {
                        LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::insertPersonAction()");
                        LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la acciones persona en la BBDD.");
                    }
                }
                else
                {
                    /// 4.- Añadir los campos que nos faltan al objeto action

                    /// 4.1.- idUser
                    if (idUser != null && idUser.Value > 0)
                        action.idPersona = idUser.Value;
                    else
                        action.idPersona = long.Parse(ConfigurationManager.AppSettings["Anonimo"]);

                    /// 4.2.- Fecha
                    action.Fecha = DateTime.Now;

                    /// 4.3.- idCurso
                    if (idCourse != null && idCourse.Value > 0)
                        action.IdCurso = idCourse;

                    /// 4.4.- Cookie
                    action.Cookie = cookie;

                    /// 4.5.- Scoring
                    action.Scoring = scoring;

                    /// 5.- Insertar la acción
                    long insert_action = da.insertPersonAction(action);
                    if (insert_action > 0)
                    {
                        /// 6.- Insertar la entrada en la tabla campus_INVESTIGA de la BBDD
                        long insert_research = da.insertResearch(action.idPersona, cookie, scoring);
                        if (insert_research < 1)
                        {
                            LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::insertResearch()");
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir una entrada en la tabla campus_INVESTIGA de la BBDD.");
                        }
                    }
                    else
                    {
                        LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::insertPersonAction()");
                        LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir la acciones persona en la BBDD.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::searchIdCliente()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }

        /// <summary>
        /// Busca una cookie
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string getCookie(Page page)
        {
            string cookie = string.Empty;
            try
            {
                if (page.Request.Cookies["ShopOnline"] != null)
                {
                    if (page.Request.Cookies["ShopOnline"]["session"] != null)
                        cookie = page.Request.Cookies["ShopOnline"]["session"].ToString();
                    else
                    {
                        cookie = page.Session.SessionID;
                        page.Request.Cookies["ShopOnline"]["session"] = page.Session.SessionID;
                        page.Response.Cookies.Add(page.Request.Cookies["ShopOnline"]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Cookie_Page.cs::getCookie()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                cookie = string.Empty;
            }
            return cookie;
        }

        /// <summary>
        /// Crea una cookie
        /// </summary>
        /// <param name="page"></param>
        private static void CrearCookie(Page page)
        {
            int intNumDias = int.Parse(ConfigurationManager.AppSettings["DiasCaducidadCookie"]);
            HttpCookie myCookie = new HttpCookie("ShopOnline");
            if (myCookie["session"] == null)
                myCookie["session"] = page.Session.SessionID;

            myCookie["AceptacionCookie"] = Constantes.activo.NoActivo.ToString();
            myCookie["fecha"] = DateTime.Today.ToShortDateString();
            if (page.Session["usuario"] != null)
                myCookie["usuario"] = ((CLIENTES)page.Session["usuario"]).login;
            myCookie.Expires = DateTime.Now.AddDays(intNumDias);
            myCookie.Domain = "spainbs.com";            
            page.Response.Cookies.Add(myCookie);
        }

        /// <summary>
        /// Actualiza una cookie
        /// </summary>
        /// <param name="login"></param>
        /// <param name="httpCookie"></param>
        public static void updateCookie(string login, HttpCookie httpCookie)
        {
            int intNumDias = int.Parse(ConfigurationManager.AppSettings["DiasCaducidadCookie"]);
            if (httpCookie != null)
            {
                string _session = httpCookie["session"];
                string _acept_cookie = httpCookie["AceptacionCookie"];
                HttpContext.Current.Response.Cookies.Remove("ShopOnline");

                HttpCookie _Cookie = new HttpCookie("ShopOnline");
                _Cookie["session"] = _session;
                _Cookie["AceptacionCookie"] = _acept_cookie;
                _Cookie["fecha"] = DateTime.Today.ToShortDateString();
                _Cookie["usuario"] = login;
                _Cookie.Expires = DateTime.Now.AddDays(intNumDias);
                _Cookie.Domain = "spainbs.com";
                HttpContext.Current.Response.Cookies.Add(_Cookie);
            }
        }
    }
}