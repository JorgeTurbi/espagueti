using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class informe_servidor_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos del servidor
                long idServidor = !String.IsNullOrEmpty(Request.QueryString["ides"]) ? long.Parse(Request.QueryString["ides"].ToString()) : -1;

                /// 2.- Cargar los datos del servidor
                if (idServidor > 0)
                    cargar_datos(idServidor);
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos del servidor
            long idServidor = !String.IsNullOrEmpty(Request.QueryString["ides"]) ? long.Parse(Request.QueryString["ides"].ToString()) : -1;

            /// 2.- Sacar los datos del formulario
            string identificador = txt_identificador.Value;
            bool prioritario = chkPriority.Checked;
            bool visible = chkVisible.Checked;
            int? limite_dia = null;
            if (!String.IsNullOrEmpty(txt_limit_day.Value))
                limite_dia = int.Parse(txt_limit_day.Value);
            int? limite_mes = null;
            if (!String.IsNullOrEmpty(txt_limit_month.Value))
                limite_mes = int.Parse(txt_limit_month.Value);
            int? limite_anyo = null;
            if (!String.IsNullOrEmpty(txt_limit_year.Value))
                limite_anyo = int.Parse(txt_limit_year.Value);
            int? dia_mes = null;
            if (!String.IsNullOrEmpty(txt_start_day.Value))
                dia_mes = int.Parse(txt_start_day.Value);
            int? mes_anyo = null;
            if (int.Parse(ddlStartMonth.SelectedValue) > 0)
                mes_anyo = int.Parse(ddlStartMonth.SelectedValue);
            
            /// 3.- Modificar o Insertar
            if (idServidor > 0)
            {
                /// 3.1.- Sacar los datos del servidor
                List<EMAIL_SERVER> lst_servers = da.getEmailServerById(idServidor);
                if (lst_servers.Count == 1)
                {
                    /// 3.2.- Actualizar los datos del servidor
                    EMAIL_SERVER server = lst_servers[0];
                    server.identificador = identificador;
                    server.prioritario = prioritario;
                    server.visible = visible;
                    server.limite = limite_dia;
                    server.limite_mes = limite_mes;
                    server.limite_anyo = limite_anyo;
                    server.fecha_inicio_dia = dia_mes;
                    server.fecha_inicio_mes = mes_anyo;

                    bool update_server = da.updateEmailServer(server);
                    if (update_server)
                        Response.Redirect("informe-servidor.aspx");
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar los datos del servidor";
                }
            }
        }

        private void cargar_datos(long idServidor)
        {
            /// 1.- Obtener los datos del contacto  de la BBDD
            List<EMAIL_SERVER> lst_servers = da.getEmailServerById(idServidor);
            if (lst_servers.Count == 1)
            {
                txt_identificador.Value = lst_servers[0].identificador;
                chkPriority.Checked = lst_servers[0].prioritario;
                chkVisible.Checked = lst_servers[0].visible;
                txt_limit_day.Value = lst_servers[0].limite != null ? lst_servers[0].limite.Value.ToString() : string.Empty;
                txt_limit_month.Value = lst_servers[0].limite_mes != null ? lst_servers[0].limite_mes.Value.ToString() : string.Empty;
                txt_limit_year.Value = lst_servers[0].limite_anyo != null ? lst_servers[0].limite_anyo.Value.ToString() : string.Empty;
                txt_start_day.Value = lst_servers[0].fecha_inicio_dia != null ? lst_servers[0].fecha_inicio_dia.Value.ToString() : string.Empty;
                ddlStartMonth.SelectedValue = lst_servers[0].fecha_inicio_mes == null ? "-1" : lst_servers[0].fecha_inicio_mes.Value.ToString();
            }
        }
    }
}