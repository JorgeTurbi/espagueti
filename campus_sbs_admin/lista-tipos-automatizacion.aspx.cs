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
    public partial class lista_tipos_automatizacion : System.Web.UI.Page
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
                    load_types(list_user[0]);
            }
        }

        protected void btnBorrarListado_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_list = false;
            
            try
            {
                int idType = !String.IsNullOrEmpty(hidIdTipoAutomatizacion.Value) ? int.Parse(hidIdTipoAutomatizacion.Value) : -1;
                if (idType > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(idType);
                    if (lst_types.Count == 1)
                        /// 2.- Eliminar la lista
                        delete_list = da.deleteTypeAutomation(idType);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el tipo de automatización');</script>");

                LogUtils.InsertarLog(" ERROR - lista-tipos-automatizacion.cs::btnBorrarListado_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_list)
                Response.Redirect("lista-tipos-automatizacion.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el tipo de automatización');</script>");
        }

        private void load_types(CLIENTES user)
        {
            List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(-1);

            /// 1.- Pintar la tabla
            table_listado_tipos.InnerHtml = paint_table(lst_types);

            /// 2.- Pintar el título
            txt_lista_tipos.InnerHtml = "<i class='fas fa-tools'></i> Lista de tipos de automatización <a href='tipo-automatizacion-mantenimiento.aspx' title='Añadir tipo automatización' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir tipo automatización</small></a>";
        }

        private string paint_table(List<campus_TIPO_AUTOMATIZACION> lst_types)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Listados\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Acciones</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var type in lst_types)
            {
                sbuild.Append("<tr>");

                sbuild.Append("<td>" + type.nombre + "</td>");
                sbuild.Append("<td><a href='automatizacion-acciones-listado.aspx?idt=" + type.id_type + "' title='Acciones'><i class='fas fa-cogs fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='tipo-automatizacion-mantenimiento.aspx?idt=" + type.id_type + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el tipo " + type.nombre + "?\")){eliminarTipo(" + type.id_type + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}