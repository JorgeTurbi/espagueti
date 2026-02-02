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
    public partial class comunidad_social : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 1.- Sacar datos de la BBDD
            List<sbs_comunidad_social> lst_rrss = da.getRRSSById(-1);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_RRSS\" class=\"display compact\" style =\"width:100%\"><thead>");
            
            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Título</th>");
            sbuild.Append("<th>Url</th>");
            sbuild.Append("<th>Orden</th>");            
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar los links
            foreach (var lnk in lst_rrss)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + lnk.titulo + "</td>");
                sbuild.Append("<td>" + lnk.url + "</td>");
                sbuild.Append("<td>" + lnk.orden + "</td>");
                sbuild.Append("<td>" + lnk.fecha + "</td>");
                sbuild.Append("<td><a href='rrss-mantenimiento.aspx?idr=" + lnk.id_rrss + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                if (lnk.activo)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea desactivar el link: " + lnk.id_rrss + "?\")){activarRRSS(" + lnk.id_rrss + ")}'><i class=\"fas fa-power-off text-color-green fa-1-6x\" style=\"cursor: pointer\" title=\"Desactivar link.\"></i></a></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea activar el link: " + lnk.id_rrss + "?\")){activarRRSS(" + lnk.id_rrss + ")}'><i class=\"fas fa-power-off text-color-red fa-1-6x\" style=\"cursor: pointer\" title=\"Activar link.\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_rrss.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            txt_rrss.InnerHtml = "<i class='fas fa-rss'></i> Listado de links rrss <a href='rrss-mantenimiento.aspx' title='Nuevo link rrss' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Nuevo link rrss</small></a>";
        }

        protected void btnActivarRRSS_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Recuperar el link de rrss
            long idRRSS = !String.IsNullOrEmpty(hidIdRRSS.Value) ? long.Parse(hidIdRRSS.Value) : -1;
            if (idRRSS > 0)
            {
                List<sbs_comunidad_social> lst_rrss = da.getRRSSById(idRRSS);
                if (lst_rrss.Count == 1)
                {
                    sbs_comunidad_social _rrss = lst_rrss[0];
                    _rrss.activo = !lst_rrss[0].activo;

                    bool update_rrss = da.updateRRSS(_rrss);
                    if (update_rrss)
                        Response.Redirect("comunidad-social.aspx");
                    else
                        ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar el link de rrss');</script>");
                }
            }
        }
    }
}