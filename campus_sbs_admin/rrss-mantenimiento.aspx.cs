using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class rrss_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos del link de rrss
                long idRRSS = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"].ToString()) : -1;

                /// 2.- Cargar los datos del link
                if (idRRSS > 0)
                    cargar_datos(idRRSS);
                else
                {
                    chkActivo.Checked = true;
                    txt_orden.Value = "1";
                }
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos del link de rrss
            long idRRSS = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"].ToString()) : -1;

            /// 2.- Sacar los datos del formulario
            string titulo = txt_title.Value.Trim();
            string url = txt_url.Value.Trim();
            int orden = int.Parse(txt_orden.Value);
            bool activo = chkActivo.Checked;

            /// 3.- Modificar o Insertar
            if (idRRSS > 0)
            {
                List<sbs_comunidad_social> lst_rrss = da.getRRSSById(idRRSS);
                if (lst_rrss.Count == 1)
                {
                    sbs_comunidad_social _rrss = lst_rrss[0];
                    _rrss.titulo = titulo;
                    _rrss.url = url;
                    _rrss.orden = orden;
                    _rrss.activo = activo;

                    bool update_rrss = da.updateRRSS(_rrss);
                    if (update_rrss)
                        Response.Redirect("comunidad-social.aspx");
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar el link de rrss";
                }
            }
            else
            {
                sbs_comunidad_social _rrss = new sbs_comunidad_social();
                _rrss.titulo = titulo;
                _rrss.url = url;
                _rrss.orden = orden;
                _rrss.activo = activo;
                _rrss.fecha = DateTime.Now;

                long insert_rrss = da.insertRRSS(_rrss);
                if (insert_rrss > 0)
                    Response.Redirect("comunidad-social.aspx");
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir el link de rrss a la BBDD";
            }
        }

        private void cargar_datos(long idRRSS)
        {
            List<sbs_comunidad_social> lst_rrss = da.getRRSSById(idRRSS);
            if (lst_rrss.Count == 1)
            {
                txt_title.Value = lst_rrss[0].titulo;
                txt_url.Value = lst_rrss[0].url;
                txt_orden.Value = lst_rrss[0].orden.ToString();
                chkActivo.Checked = lst_rrss[0].activo;
            }
        }
    }
}