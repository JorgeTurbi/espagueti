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
    public partial class lista_suscriptores : System.Web.UI.Page
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
                    long idList = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"]) : -1;
                    if (idList > 0)
                        /// 1.- Comprobar si hay que actualizar alguna lista
                        update_list(idList);
                    else
                        /// 2.- Cargar el listado de lista de suscripciones
                        load_suscripciones(list_user[0]);
                }
            }
        }

        protected void btnBorrarListado_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_list = false;

            try
            {
                long idList = !String.IsNullOrEmpty(hidIdListado.Value) ? long.Parse(hidIdListado.Value) : -1;
                if (idList > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_LISTADO_SUSCRIPCIONES> lst = da.getEmailListSubscriptionsById(idList);
                    if (lst.Count == 1)
                        /// 2.- Eliminar la lista
                        delete_list = da.deleteEmailListSubscription(idList);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la lista');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::btnBorrarListado_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_list)
                Response.Redirect("lista-suscriptores.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la lista');</script>");
        }

        protected void btnRefrescarListado_Click(object sender, ImageClickEventArgs e)
        {
            bool refresh_list = false;

            try
            {
                long idList = !String.IsNullOrEmpty(hidIdListado.Value) ? long.Parse(hidIdListado.Value) : -1;
                if (idList > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_LISTADO_SUSCRIPCIONES> lst = da.getEmailListSubscriptionsById(idList);
                    if (lst.Count == 1)
                    {
                        bool insert_correct = true;
                        int total_suscriptores = 0;

                        /// 2.- Sacar los datos del listado de los suscriptores de la lista
                        List<EMAIL_SUSCRIPCIONES> lst_suscriptores = da.getEmailSubscriptionsById(idList);
                        List<long> lst_id_alumnos = lst_suscriptores.Select(sus => sus.id_alumno).Distinct().ToList();

                        /// 3.- Sacar los suscriptores nuevos
                        List<EMAIL_SUSCRIPCIONES> listado_suscriptores = obtener_suscriptores(lst[0].sql);
                        listado_suscriptores = listado_suscriptores.Where(sus => !lst_id_alumnos.Contains(sus.id_alumno)).ToList();
                        if (listado_suscriptores.Count > 0)
                        {
                            foreach (var suscriptor in listado_suscriptores)
                            {
                                if (Utilities.validarEmail(suscriptor.mail))
                                {
                                    EMAIL_SUSCRIPCIONES _suscriptor = new EMAIL_SUSCRIPCIONES();
                                    _suscriptor.id_els = idList;
                                    _suscriptor.nombre_completo = suscriptor.nombre_completo;
                                    _suscriptor.mail = suscriptor.mail;
                                    _suscriptor.id_alumno = suscriptor.id_alumno;
                                    _suscriptor.fecha_alta = DateTime.Today;

                                    long insert_suscriptor = da.insertEmailSubscription(_suscriptor);
                                    if (insert_suscriptor == -1)
                                    {
                                        insert_correct = false;
                                        break;
                                    }

                                    total_suscriptores++;
                                }
                            }
                        }

                        if (insert_correct)
                        {
                            /// 4.- Actualizar los datos de la lista
                            //int num_suscriptions = listado_suscriptores.Count;
                            int num_suscriptions = total_suscriptores;
                            if (num_suscriptions > 0)
                            {
                                /// 5.- Actualizar los datos
                                EMAIL_LISTADO_SUSCRIPCIONES _suscripcion = lst[0];
                                _suscripcion.num_total = (lst[0].num_total != null ? lst[0].num_total.Value : 0) + num_suscriptions;
                                _suscripcion.num_actual = (lst[0].num_actual != null ? lst[0].num_actual.Value : 0) + num_suscriptions;
                                _suscripcion.num_bajas = lst[0].num_bajas != null ? lst[0].num_bajas.Value : 0;
                                if (!String.IsNullOrEmpty(lst[0].historico))
                                    _suscripcion.historico = lst[0].historico + "<br />Lista actualizada el " + DateTime.Today.ToShortDateString() + " con " + num_suscriptions + " nuevos suscriptores.";
                                else
                                    _suscripcion.historico = "Lista actualizada el " + DateTime.Today.ToShortDateString() + " con " + num_suscriptions + " nuevos suscriptores.";
                                _suscripcion.fecha_actualizacion = DateTime.Today;

                                bool update_list = da.updateEmailListSubscription(_suscripcion);
                                if (update_list)
                                    refresh_list = true;
                                else
                                    ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al actualizar los datos de la lista');</script>");
                            }
                            else
                            {
                                /// 4.- Actualizar los datos
                                EMAIL_LISTADO_SUSCRIPCIONES _suscripcion = lst[0];
                                _suscripcion.fecha_actualizacion = DateTime.Today;

                                bool update_list = da.updateEmailListSubscription(_suscripcion);
                                if (update_list)
                                    refresh_list = true;
                                else
                                    ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al actualizar los datos de la lista');</script>");
                            }
                        }                            
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al actualizar los datos de la lista');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::btnRefrescarListado_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (refresh_list)
                Response.Redirect("lista-suscriptores.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al actualizar los datos de la lista');</script>");
        }
        
        private void update_list(long idList)
        {
            /// 1.- Sacar las suscripciones de la lista
            List<EMAIL_SUSCRIPCIONES> list = da.getEmailSubscriptionsById(idList);
            if (list.Count > 0)
            {
                /// 2.- Sacar las suscripciones activas
                List<EMAIL_SUSCRIPCIONES> list_active = list.Where(es => es.fecha_baja == null).ToList();

                int num_total = list.Count;
                int num_actual = list_active.Count;
                int num_bajas = num_total - num_actual;

                /// 3.- Sacar el listado
                List<EMAIL_LISTADO_SUSCRIPCIONES> lst_sus = da.getEmailListSubscriptionsById(idList);
                if (lst_sus.Count > 0)
                {
                    bool update_list = da.updateEmailListSubscription(lst_sus[0], num_total, num_bajas, num_actual);
                    if (update_list)
                        Response.Redirect("lista-suscriptores.aspx");
                    else
                    {
                        LogUtils.InsertarLog(" ERROR - lista-suscriptores.cs::updateEmailListSubscription()");
                        LogUtils.InsertarLog("- MSG: ERROR - Al actualizar los datos en EMAIL_LISTADO_SUSCRIPCIONES");
                    }
                }
            }
        }

        private void load_suscripciones(CLIENTES user)
        {
            /// 1.- Comprobar si es un usuario de envío de mails especiales
            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
            long id_user_special = -1;

            if (list_users_mails.Contains(user.id_cliente.ToString()))
                id_user_special = user.id_cliente;

            /// 2.- Sacar el listado de listas de suscripción
            List<EMAIL_LISTADO_SUSCRIPCIONES> listado_suscripciones = da.getEmailListSubscriptions();

            /// 3.- Filtrar las listas por el usuario especial
            if (id_user_special > 0)
                listado_suscripciones = listado_suscripciones.Where(els => els.id_usuario == id_user_special).ToList();

            /// 4.- Pintar la tabla
            table_listado_suscriptores.InnerHtml = paint_table(listado_suscripciones);

            /// 5.- Pintar el título
            txt_lista_suscriptores.InnerHtml = "<i class='far fa-list-alt'></i> Listado de listas de suscriptores <a href='lista-suscriptores-mantenimiento.aspx' title='Añadir lista suscripciones' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir lista suscripciones</small></a>";
        }

        private string paint_table(List<EMAIL_LISTADO_SUSCRIPCIONES> listado_suscripciones)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Sacar los suscriptores de las distintas tablas
            //List<EMAIL_SUSCRIPCIONES> list_suscripciones = da.getEmailSubscriptionsByListELS(listado_suscripciones.Select(els => els.id_els).Distinct().ToList());

            /// 2.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Listados\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 3.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Fecha alta</th>");
            sbuild.Append("<th>Total</th>");
            sbuild.Append("<th>Actual</th>");
            sbuild.Append("<th>Bajas</th>");
            sbuild.Append("<th>%</th>");
            sbuild.Append("<th>Auto</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los listados de suscriptores
            foreach (var lista in listado_suscripciones)
            {
                sbuild.Append("<tr>");
                if (lista.auto != null && lista.auto.Value)
                    sbuild.Append("<td>@" + lista.nombre + (lista.fecha_actualizacion != null ? " (" + lista.fecha_actualizacion.Value.ToShortDateString() + ")" : string.Empty) + "</td>");
                else
                    sbuild.Append("<td>" + lista.nombre + "</td>");

                sbuild.Append("<td>" + lista.fecha_alta.ToShortDateString() + "</td>");
                sbuild.Append("<td>" + (lista.num_total != null ? lista.num_total.Value : 0) + "</td>");
                sbuild.Append("<td>" + (lista.num_actual != null ? lista.num_actual.Value : 0) + "</td>");
                sbuild.Append("<td>" + (lista.num_bajas != null ? lista.num_bajas.Value : 0) + "</td>");

                if (lista.num_bajas != null && lista.num_total != null && lista.num_total.Value > 0)
                {
                    decimal pct_bajas = ((decimal)lista.num_bajas.Value / (decimal)lista.num_total.Value) * 100;
                    sbuild.Append("<td>" + Math.Round(pct_bajas, 2) + "</td>");
                }
                else
                    sbuild.Append("<td>0</td>");

                if (lista.auto != null && lista.auto.Value)
                    sbuild.Append("<td><i class='fas fa-check fa-1-6x text-color-primary'></i></td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append("<td><a href='suscriptores.aspx?idl=" + lista.id_els + "' title='Ver'><i class='fas fa-eye fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='lista-suscriptores-mantenimiento.aspx?idl=" + lista.id_els + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                if (lista.auto != null && lista.auto.Value)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='refrescarListado(" + lista.id_els + ");' title='Refrescar'><i class='fas fa-sync-alt fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td></td>");
                sbuild.Append("<td><a href='suscriptores-mantenimiento.aspx?idl=" + lista.id_els + "' title='Añadir a la lista'><i class='fas fa-user-plus fa-1-6x'></i></a></td>");
                /*List<EMAIL_SUSCRIPCIONES> list_filter = list_suscripciones.Where(es => es.id_els == lista.id_els).ToList();
                if (list_filter.Count > 0)*/
                if(lista.num_total != null && lista.num_total > 0)
                    sbuild.Append("<td></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la lista " + lista.nombre + "?\")){eliminarListado(" + lista.id_els + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }

        private List<EMAIL_SUSCRIPCIONES> obtener_suscriptores(string sql)
        {
            string cadena_conexion = ConfigurationManager.AppSettings["CadenaConexion"];

            List<EMAIL_SUSCRIPCIONES> list = new List<EMAIL_SUSCRIPCIONES>();
            Conexion_BBDD AccesoDatos = new Conexion_BBDD(cadena_conexion);
            AccesoDatos.Conectar();

            AccesoDatos.SQL = sql;
            AccesoDatos.CargarSelect();
            while (AccesoDatos.Leer())
            {
                EMAIL_SUSCRIPCIONES suscripcion = new EMAIL_SUSCRIPCIONES();
                suscripcion.nombre_completo = AccesoDatos.SacarDato(0);
                suscripcion.mail = AccesoDatos.SacarDato(1);
                suscripcion.id_alumno = long.Parse(AccesoDatos.SacarDato(2));
                list.Add(suscripcion);
            }

            AccesoDatos.Liberar();
            AccesoDatos.Desconectar();

            return list;
        }
    }
}