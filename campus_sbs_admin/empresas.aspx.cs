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
    public partial class empresas : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el parámetro 'k' en la url
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                {
                    Session.Add("usuario", list_user[0]);
                }
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
                    loadBusiness(list_user[0]);
            }
        }

        protected void btnBorrarEmpresa_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_company = false;

            try
            {
                long idCompany = !String.IsNullOrEmpty(hidIdEmpresa.Value) ? long.Parse(hidIdEmpresa.Value) : -1;
                if (idCompany > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idCompany);
                    if (lst_empresas.Count == 1)
                    {
                        /// 2.- Eliminar el fichero 
                        if (!String.IsNullOrEmpty(lst_empresas[0].FicheroConvenio))
                            File.Delete(ConfigurationManager.AppSettings["ruta_convenio"] + lst_empresas[0].FicheroConvenio);

                        /// 3.- Eliminar el logo
                        if (!String.IsNullOrEmpty(lst_empresas[0].Logo))
                            File.Delete(ConfigurationManager.AppSettings["ruta_logo_empresa"] + lst_empresas[0].Logo);

                        /// 4.- Eliminar la empresa
                        delete_company = da.deleteCompany(idCompany);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la empresa');</script>");

                LogUtils.InsertarLog(" ERROR - empresas.cs::btnBorrarNoticia_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_company)
                Response.Redirect("empresas.aspx");
        }

        private void loadBusiness(CLIENTES user)
        {
            /// 1.- Sacar los datos de la BBDD
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(-1);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Empresas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>F. Inicio</th>");
            //sbuild.Append("<th>F. Fin</th>");
            sbuild.Append("<th>Logo</th>");
            sbuild.Append("<th>Empresa</th>");
            sbuild.Append("<th>Mail</th>");
            sbuild.Append("<th>Teléfono</th>");
            sbuild.Append("<th>Fecha Convenio</th>");
            sbuild.Append("<th>Conv.</th>");
            sbuild.Append("<th>Nº Contacto</th>");
            sbuild.Append("<th>Nº Práctica</th>");
            sbuild.Append("<th>Nº Empleo</th>");
            sbuild.Append("<th>Nº Solicitud</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las empresas
            foreach (var business in lst_empresas)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + business.FechaAlta.ToShortDateString() + "</td>");
                //sbuild.Append("<td>" + (business.FechaBaja != null ? business.FechaBaja.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + ((String.IsNullOrEmpty(business.Logo) ? string.Empty : "<a href='" + ConfigurationManager.AppSettings["url_logo_empresa"] + business.Logo + "' runat='server' target='_blank'><img alt='Logo " + business.RazonSocial + "' src='" + ConfigurationManager.AppSettings["url_logo_empresa"] + business.Logo + "' width='40px' height='auto'/></a>")) + "</td>");
                sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(business.RazonSocial) + "</span><a href=\"empresa-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"" + business.RazonSocial + "\">" + business.RazonSocial + " (" + business.Cif + ")</a></td>");
                sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"" + business.Mail + "\">" + business.Mail + " </a></td>");
                sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"" + business.Telefono + "\">" + business.Telefono + " </a></td>");
                sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"Fecha convenio\">" + (business.FechaConvenio != null ? business.FechaConvenio.Value.ToShortDateString() : string.Empty) + " </a></td>");
                sbuild.Append("<td>" + (String.IsNullOrEmpty(business.FicheroConvenio) ? string.Empty : "<a href='" + ConfigurationManager.AppSettings["url_convenio"] + business.FicheroConvenio + "' runat='server' target='_blank'><i class='far fa-file-pdf fa-1-6x text-color-red'></i></a>") + "</td>");
                sbuild.Append("<td><a href=\"contactos.aspx?idb=" + business.idEmpresa + "\" title=\"Nº de contactos\" target=\"_blank\"><span class='fa-1-3x v-bottom'>" + business.num_contactos + "</span> </a>");
                sbuild.Append("<a href=\"contacto-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"Nuevo contacto\" target=\"_blank\"><i class='fas fa-plus-circle fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"practicas.aspx?idb=" + business.idEmpresa + "\" title=\"Nº de prácticas\" target=\"_blank\"><span class='fa-1-3x v-bottom'>" + business.num_practicas + "</span> </a>");
                sbuild.Append("<a href=\"practica-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"Nueva práctica\" target=\"_blank\"><i class='fas fa-plus-circle fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"empleos.aspx?idb=" + business.idEmpresa + "\" title=\"Nº de empleos\" target=\"_blank\"><span class='fa-1-3x v-bottom'>" + business.num_empleos + "</span> </a>");
                sbuild.Append("<a href=\"empleo-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"Nuevo empleo\" target=\"_blank\"><i class='fas fa-plus-circle fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"solicitud-practica.aspx?idb=" + business.idEmpresa + "\" title=\"Nº Solicitudes de prácticas\" target=\"_blank\"><span class='fa-1-3x v-bottom'>" + business.num_solicitudes + "</span> </a>");
                sbuild.Append("<a href=\"solicitud-mantenimiento.aspx?idb=" + business.idEmpresa + "\" title=\"Nueva solicitud de prácticas\" target=\"_blank\"><i class='fas fa-plus-circle fa-1-6x'></i></a></td>");
                if (business.num_contactos > 0 || business.num_empleos > 0 || business.num_practicas > 0 || business.num_solicitudes > 0)
                    sbuild.Append("<td></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la empresa: " + business.RazonSocial + "?\")){eliminarEmpresa(" + business.idEmpresa + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_empresas.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            txt_empresas.InnerHtml = "<i class='fas fa-building'></i> Listado de Empresas <a href='empresa-mantenimiento.aspx' title='Nueva empresa' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva empresa</small></a>";
        }
    }
}