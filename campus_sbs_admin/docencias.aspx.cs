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
    public partial class docencias : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private int _idc;
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
                    /// 1.- Pintar el título
                    txt_titulo.InnerHtml = "<i class='fas fa-file'></i> Listado de docencias <a href='docencia-mantenimiento.aspx' title='Añadir docencia' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir docencia</small></a>";
                    if (!String.IsNullOrEmpty(Request.QueryString["idc"]))
                    {
                        _idc = int.Parse(Request.QueryString["idc"].ToString());

                        render_docencias(da.getDocenciaByIdCurso(_idc));
                    }
                    else
                    {
                        _idc = -1;
                        render_docencias(da.getDocencias(false));
                    }

                    /// 2.- Cargar los programas
                }
            }
        }

        private void render_docencias(List<campus_DOCENCIA> docencias)
        {
            /// 1.- Sacar datos de la BBDD


            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_listado\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Descripción</th>");
            sbuild.Append("<th>Inicio</th>");
            sbuild.Append("<th>Fin</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");
            /// 2.3.- Pintar las reglas
            foreach (var item in docencias)
            {
                sbuild.Append("<tr>");

                sbuild.Append($"<td>{item.Nombre}</td>");
                sbuild.Append($"<td class='text-left'>{item.Descripcion}</td>");
                sbuild.Append($"<td>{(item.FInicio.HasValue ? item.FInicio.Value.ToString("dd/MM/yyyy") : "")}</td>");
                sbuild.Append($"<td>{(item.FFin.HasValue ? item.FFin.Value.ToString("dd/MM/yyyy") : "")}</td>");

                sbuild.Append("<td><a href=\"docencia-mantenimiento.aspx?idd=" + item.ID_Docencia + "\" title=\"Editar\"><i class='fa fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado.InnerHtml = sbuild.ToString();
        }
    }
}