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
    public partial class automatizacion_acciones_listado : System.Web.UI.Page
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
                    /// 1.- Sacar los datos del tipo de automatización
                    int idType = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;
                    if (idType > 0)
                        load_actions(idType, list_user[0]);
                    else
                        Response.Redirect("lista-tipos-automatizacion.aspx");
                }
            }
        }

        protected void btnBorrarListado_Click(object sender, ImageClickEventArgs e)
        {
            int idType = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;
            if (idType > 0)
            {
                /// 2.- Sacar la accion
                string action = hidIdTipoAutomatizacion.Value;
                if (!String.IsNullOrEmpty(action))
                {
                    List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(idType);
                    if (lst_types.Count == 1)
                    {
                        /// 4.- Pintar los ids de las acciones
                        List<long> lst_id_origins = new List<long>();
                        string sAction = lst_types[0].acciones;
                        if (!String.IsNullOrEmpty(sAction))
                        {
                            List<string> lst_ids = sAction.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var _action in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(_action) && !lst_id_origins.Contains(long.Parse(_action)))
                                        lst_id_origins.Add(long.Parse(_action));
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(action))
                        {
                            List<string> lst_ids = action.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var _action in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(_action) && lst_id_origins.Contains(long.Parse(_action)))
                                        lst_id_origins.Remove(long.Parse(_action));
                                }
                            }
                        }

                        string list_actions = string.Empty;
                        foreach (var accion in lst_id_origins)
                        {
                            if (String.IsNullOrEmpty(list_actions))
                                list_actions = accion.ToString();
                            else
                                list_actions = list_actions + "," + accion.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_actions))
                        {
                            /// 3.- Actualizar la regla
                            campus_TIPO_AUTOMATIZACION type_automation = lst_types[0];
                            type_automation.acciones = list_actions;

                            bool update_type = da.updateTypeAutomation(type_automation);
                            if (update_type)
                                Response.Redirect("automatizacion-acciones-listado.aspx?idt=" + idType);
                            else
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la acción');</script>");
                        }
                    }
                }
            }
        }

        private void load_actions(int idType, CLIENTES user)
        {
            List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(idType);

            /// 1.- Pintar la tabla
            table_listado_tipos.InnerHtml = paint_table(lst_types);

            /// 2.- Pintar el título
            txt_lista_tipos.InnerHtml = "<i class='fas fa-tools'></i> Lista de acciones asociadas al tipo de automatización <a href='automatizacion-acciones-mantenimiento.aspx?idt=" + idType + "' title='Añadir acciones al tipo de automatización' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir acciones al tipo de automatización</small></a>";

            /// 3.- Poner el botón volver
            lnk_back.HRef = "lista-tipos-automatizacion.aspx";
        }

        private string paint_table(List<campus_TIPO_AUTOMATIZACION> lst_types)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar las acciones
            List<campus_AUX> lst_actions = da.getAuxiliars("TIPO_ACCION");

            /// 1.- Sacar las acciones que contienen el tipo de automatización
            List<long> lst_ids = new List<long>();
            if (!String.IsNullOrEmpty(lst_types[0].acciones))
            {
                List<string> list = lst_types[0].acciones.Split(',').ToList();
                if (list.Count > 0)
                {
                    foreach (var action in list)
                    {
                        if (!String.IsNullOrEmpty(action) && !lst_ids.Contains(long.Parse(action)))
                            lst_ids.Add(long.Parse(action));
                    }
                }
            }

            lst_actions = lst_actions.Where(a => lst_ids.Contains(a.ID_Aux)).ToList();

            /// 2.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Listados\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 3.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Acciones</th>");
            sbuild.Append("<th>ID</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var action in lst_actions)
            {
                sbuild.Append("<tr>");

                sbuild.Append("<td>" + action.Nombre + "</td>");
                sbuild.Append("<td>" + action.ID_Aux + "</td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la acción " + action.Nombre + "?\")){eliminarAccion(" + action.ID_Aux + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}