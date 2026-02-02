using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_banners : System.Web.UI.Page
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
                    /// 1.- Pintar el título
                    txt_banners.InnerHtml = "<i class='fas fa-ad'></i> Listado de banners <a href='banner-mantenimiento.aspx' title='Añadir banner' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir banner</small></a>";

                    /// 2.- Cargar los programas
                    cargar_banners();
                }
            }
        }
        
        protected void btnBorrarBanner_Click(object sender, ImageClickEventArgs e)
        {
            bool _delete = false;

            try
            {
                long _id_banner = !String.IsNullOrEmpty(hidIdBanner.Value) ? long.Parse(hidIdBanner.Value) : -1;
                if (_id_banner > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<EDU_Banners> _banners = da.getEduBanners(_id_banner);
                    if (_banners.Count == 1)
                    {
                        /// 2.- Eliminar las imagenes del banner
                        if (!String.IsNullOrEmpty(_banners[0].Banner))
                        {
                            string ruta = ConfigurationManager.AppSettings["ruta_banner"] + _banners[0].Id_Edu_Producto + "/";

                            /// 2.1.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta + _banners[0].Banner);
                        }

                        if (!String.IsNullOrEmpty(_banners[0].Banner_2x))
                        {
                            string ruta = ConfigurationManager.AppSettings["ruta_banner"] + _banners[0].Id_Edu_Producto + "/";

                            /// 2.1.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta + _banners[0].Banner_2x);
                        }

                        /// 3.- Eliminar la regla 
                        _delete = da.deleteEduBanner(_id_banner);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el banner');</script>");

                LogUtils.InsertarLog(" ERROR - lista-banners.cs::btnBorrarRecurso_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (_delete)
                Response.Redirect("lista-banners.aspx");
        }

        private void cargar_banners()
        {
            /// 1.- Sacar los datos de la BBDD
            List<EDU_Banners> _banners = da.getEduBanners(null);
            List<EDU_Productos> _productos = da.getEduProductos((long)Constantes.producto.SBS_Life);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Banners\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Producto</th>");
            sbuild.Append("<th>Banner</th>");
            sbuild.Append("<th>Banner 2</th>");
            sbuild.Append("<th>Fecha Inicio</th>");
            sbuild.Append("<th>Fecha Fin</th>");
            sbuild.Append("<th>Orden</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 2.3.- Pintar los banners
            foreach (var _banner in _banners)
            {
                if (_banner.Fecha_Fin.HasValue && _banner.Fecha_Fin.Value < DateTime.Today)
                    sbuild.Append("<tr class='text-color-red'>");
                else
                    sbuild.Append("<tr>");
                sbuild.Append($"<td>{_banner.Nombre}</td>");
                sbuild.Append($"<td>{(_productos.Count > 0 ? _productos[0].Nombre : string.Empty)}</td>");
                sbuild.Append($"<td>{(!String.IsNullOrEmpty(_banner.Banner) ? "<a href='" + ConfigurationManager.AppSettings["url_banner"] + _banner.Id_Edu_Producto + "/" + _banner.Banner + "' target='_blank'><i class='far fa-image fa-1-6x'></i></a>" : string.Empty)}</td>");
                sbuild.Append($"<td>{(!String.IsNullOrEmpty(_banner.Banner_2x) ? "<a href='" + ConfigurationManager.AppSettings["url_banner"] + _banner.Id_Edu_Producto + "/" + _banner.Banner_2x + "' target='_blank'><i class='far fa-image fa-1-6x'></i></a>" : string.Empty)}</td>");
                sbuild.Append($"<td>{_banner.Fecha_Inicio.ToShortDateString()}</td>");
                sbuild.Append($"<td>{(_banner.Fecha_Fin.HasValue ? _banner.Fecha_Fin.Value.ToShortDateString() : string.Empty)}</td>");
                sbuild.Append($"<td>{_banner.Orden}</td>");
                sbuild.Append($"<td>{(!String.IsNullOrEmpty(_banner.Link) ? "<a href='" + _banner.Link + "' target='_blank'><span class='badge badge-pill badge-info' data-toggle='tooltip' data-placement='top' data-html='true' title='" + _banner.Link + "'><i class='fas fa-globe fa-1-6x'></i></span></a>" : string.Empty)}</td>");
                sbuild.Append($"<td><a href='banner-mantenimiento.aspx?idb={_banner.Id_Banner}' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el banner " + _banner.Nombre + "?\")){eliminarBanner(" + _banner.Id_Banner + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_banners.InnerHtml = sbuild.ToString();
        }
    }
}