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
    public partial class tests_subcategoria : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Test_Subcategoria> items = da.getTest_Subcategorias();
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");            
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            
            foreach (Test_Subcategoria item in items)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + item.Nombre + "</td>");

                sbuild.Append("<td style='width:25px;'><a href=\"tests_subcategoria-mantenimiento.aspx?id=" + item.Id + "\" title=\"Editar\"><i class='fa fa-edit fa-1-6x' style='margin-right:15px'></i></a></td>");
                sbuild.Append("<td style='width:25px'><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la Subcategoria: " + item.Nombre + "?\")){eliminar(" + item.Id + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            title.InnerHtml = "<i class='fa fa-tags'></i> Listado de Subcategorías <a href='tests_subcategoria-mantenimiento.aspx' title='Nueva Subcategoría' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva Subcategoría</small></a>";
        }

        protected void btnBorrar_Click(object sender, ImageClickEventArgs e)
        {
            bool delete = false;

            try
            {
                int id = !String.IsNullOrEmpty(hidId.Value) ? int.Parse(hidId.Value) : -1;
                if (id > 0)
                {
                    da.DeleteTest_Subcategoria(id);
                    delete = true;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la Subcategoría');</script>");

                LogUtils.InsertarLog(" ERROR - tests_subcategoria.cs::btnBorrar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete)
                Response.Redirect("tests_subcategoria.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la Subcategoría');</script>");
        }
    }
}