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
    public partial class automatizacion_acciones_mantenimiento : System.Web.UI.Page
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos del tipo de automatización
            int idType = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;
            if (idType > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string acciones = hidAccion.Value;
                if (!String.IsNullOrEmpty(acciones))
                {
                    /// 3.- Sacar las acciones por tipo
                    List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(idType);
                    if (lst_types.Count == 1)
                    {
                        /// 4.- Pintar los ids de las acciones
                        List<long> lst_id_actions = new List<long>();
                        if (!String.IsNullOrEmpty(acciones))
                        {
                            List<string> lst_ids = acciones.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var action in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(action) && !lst_id_actions.Contains(long.Parse(action)))
                                        lst_id_actions.Add(long.Parse(action));
                                }
                            }
                        }

                        string sAction = lst_types[0].acciones;
                        if (!String.IsNullOrEmpty(sAction))
                        {
                            List<string> lst_ids = sAction.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var action in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(action) && !lst_id_actions.Contains(long.Parse(action)))
                                        lst_id_actions.Add(long.Parse(action));
                                }
                            }
                        }

                        string list_actions = string.Empty;
                        foreach (var action in lst_id_actions)
                        {
                            if (String.IsNullOrEmpty(list_actions))
                                list_actions = action.ToString();
                            else
                                list_actions = list_actions + "," + action.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_actions))
                        {
                            /// 4.- Actualizar el tipo de automatización
                            campus_TIPO_AUTOMATIZACION type = lst_types[0];
                            type.acciones = list_actions;

                            bool update_type = da.updateTypeAutomation(type);
                            if (update_type)
                                Response.Redirect("automatizacion-acciones-listado.aspx?idt=" + idType);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar el tipo de automatización";
                        }
                    }
                }
            }
        }

        private void load_actions(int idType, CLIENTES user)
        {
            StringBuilder sbuild = new StringBuilder();
            
            /// 1.- Sacar las acciones por tipo
            List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(idType);

            /// 2.- Sacar acciones de la BBDD
            List<campus_AUX> lst_actions = da.getAuxiliars("TIPO_ACCION");

            /// 3.- Sacar las acciones que contienen el tipo de automatización
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

            lst_actions = lst_actions.Where(a => !lst_ids.Contains(a.ID_Aux)).ToList();

            /// 2.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Listados\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 3.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Acción</th>");
            sbuild.Append("<th>ID</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var action in lst_actions)
            {
                sbuild.Append("<tr>");

                sbuild.Append("<td><input type=\"checkbox\" value=\"" + action.ID_Aux + "\" onclick=\"chk_mark(this)\"></td>");
                sbuild.Append("<td>" + action.Nombre + "</td>");
                sbuild.Append("<td>" + action.ID_Aux + "</td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            table_listado_tipos.InnerHtml = sbuild.ToString();

            /// 5.- Pintar el título
            txt_lista_tipos.InnerHtml = "<i class='fas fa-tools'></i> Lista de acciones";
            //txt_lista_tipos.InnerHtml = "<i class='fas fa-tools'></i> Lista de acciones <a href='tipo-automatizacion-mantenimiento.aspx' title='Añadir todas las acciones' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir todas las acciones</small></a>";

            /// 6.- Poner el botón volver
            lnk_back.HRef = "automatizacion-acciones-listado.aspx?idt=" + idType;
        }        
    }
}