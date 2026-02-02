using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class suscriptores : System.Web.UI.Page
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
                        load_suscripciones(idList);
                    else
                        Response.Redirect("lista-suscriptores.aspx");                        
                }
            }
        }
        
        protected void btnBorrarSuscriptor_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_list = false;
            long idList = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"]) : -1;

            try
            {
                long id_suscriptor = !String.IsNullOrEmpty(hidIdSuscriptor.Value) ? long.Parse(hidIdSuscriptor.Value) : -1;
                if (id_suscriptor > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EMAIL_SUSCRIPCIONES> lst = da.getEmailSubscriptionsByIdSuscriptor(id_suscriptor);
                    if (lst.Count == 1)
                        /// 2.- Dar de baja al suscriptor
                        delete_list = da.updateEmailSubscription(lst[0], DateTime.Today);
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
                Response.Redirect("suscriptores.aspx?idl=" + idList);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al dar de baja al suscriptor');</script>");
        }

        private void load_suscripciones(long idList)
        {
            /// 1.- Sacar los datos de la BBDD
            List<EMAIL_SUSCRIPCIONES> list_suscripciones = da.getEmailSubscriptionsById(idList);
            List<EMAIL_LISTADO_SUSCRIPCIONES> list_listados = da.getEmailListSubscriptionsById(idList);

            /// 2.- Pintar la tabla
            table_suscriptores.InnerHtml = paint_table(list_suscripciones);

            /// 3.- Pintar el histórico de la lista
            table_historico.InnerHtml = list_listados[0].historico;

            /// 4.- Pintar el título
            txt_suscriptores.InnerHtml = "<i class='far fa-list-alt'></i> Listado de suscriptores <a href='suscriptores-mantenimiento.aspx?idl=" + idList + "' title='Añadir suscriptores' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-user-plus fa-2x'></i> Añadir suscriptores</small></a>";
        }

        private string paint_table(List<EMAIL_SUSCRIPCIONES> list_suscripciones)
        {
            StringBuilder sbuild = new StringBuilder();
            
            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_suscriptores\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>ID Alumno</th>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Mail</th>");
            sbuild.Append("<th>Fecha Alta</th>");
            sbuild.Append("<th>Fecha Baja</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los listados de suscriptores
            foreach (var suscripcion in list_suscripciones)
            {
                if (suscripcion.fecha_baja != null)
                    sbuild.Append("<tr class='text-color-red'>");
                else
                    sbuild.Append("<tr>");
                sbuild.Append("<td>" + suscripcion.id_alumno + "</td>");
                sbuild.Append("<td>" + suscripcion.nombre_completo + "</td>");
                sbuild.Append("<td>" + suscripcion.mail + "</td>");
                sbuild.Append("<td>" + suscripcion.fecha_alta.ToShortDateString() + "</td>");
                sbuild.Append("<td>" + (suscripcion.fecha_baja != null ? suscripcion.fecha_baja.Value.ToShortDateString() : string.Empty) + "</td>");
                if (suscripcion.fecha_baja != null)
                    sbuild.Append("<td></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea dar de baja al suscriptor con mail " + suscripcion.mail + "?\")){eliminarSuscriptor(" + suscripcion.id_s + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }
    }
}