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
    public partial class solicitud_practica : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;

                cargar_datos(idEmpresa);
                /*if (idEmpresa > 0)
                    cargar_datos(idEmpresa);
                else
                    Response.Redirect("empresas.aspx");*/
            }
        }
        
        protected void btnBorrarSolicitud_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_request = false;

            try
            {
                long idRequest = !String.IsNullOrEmpty(hidIdSolicitud.Value) ? long.Parse(hidIdSolicitud.Value) : -1;
                if (idRequest > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<PRA_SOLICITUD_PRACTICA> lst_solicitudes = da.getListPracticesRequestsById(idRequest);
                    if (lst_solicitudes.Count == 1)
                    {
                        /// 2.- Eliminar la solicitud
                        delete_request = da.deleteRequest(idRequest);

                        /// 3.- Modificar en empresa el nº de contactos
                        List<PRA_EMPRESA> lst_empresas = da.getBusinessById(lst_solicitudes[0].idEmpresa);
                        if (lst_empresas.Count == 1)
                        {
                            PRA_EMPRESA empresa = lst_empresas[0];
                            empresa.num_solicitudes = empresa.num_solicitudes - 1;

                            bool update_company = da.updateCompany(empresa);
                            if (!update_company)
                            {
                                delete_request = false;
                                LogUtils.InsertarLog(" ERROR - solicitud-practica.cs::btnBorrarSolicitud_Click()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al modificar en empresa el nº de solicitudes de prácticas");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la solicitud de la práctica');</script>");

                LogUtils.InsertarLog(" ERROR - solicitud-practica.cs::btnBorrarSolicitud_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_request)
                Response.Redirect("solicitud-practica.aspx?idb=" + Request.QueryString["idb"]);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la solicitud de la práctica');</script>");
        }

        private void cargar_datos(long idEmpresa)
        {
            /// 1.- Sacar los datos de la BBDD
            List<PRA_SOLICITUD_PRACTICA> lst_solicitudes = da.getListPracticesRequestsById(-1);
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idEmpresa);
            List<PRA_CANDIDATOS> lst_candidatos = da.getCandidatesById(-1);
            List<PRA_CONTACTO> lst_contacts = da.getContactsByIdCompany(idEmpresa);

            if (idEmpresa > 0)
                lst_solicitudes = lst_solicitudes.Where(sol => sol.idEmpresa == idEmpresa).ToList();

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Solicitudes\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha Alta</th>");
            //sbuild.Append("<th>Fecha Cierre</th>");
            sbuild.Append("<th>Empresa</th>");
            sbuild.Append("<th>Contacto</th>");
            /*sbuild.Append("<th>Nº Horas</th>");
            sbuild.Append("<th>Cantidad</th>");*/
            sbuild.Append("<th>Descripción puesto</th>");
            sbuild.Append("<th>Nº candidatos</th>");
            sbuild.Append("<th>Comentarios</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las solicitudes
            foreach (var solicitud in lst_solicitudes)
            {
                /// 2.3.1.- Sacar los datos de la empresa
                List<PRA_EMPRESA> lst_empresa = lst_empresas.Where(emp => emp.idEmpresa == solicitud.idEmpresa).ToList();
                List<PRA_CONTACTO> lst_contacto = lst_contacts.Where(cont => cont.idContacto == solicitud.idContacto).ToList();
                int num_candidatos = lst_candidatos.Where(cand => cand.idSolicitud == solicitud.idSolicitud).Count();

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + solicitud.fecha.ToShortDateString() + "</td>");

                if (lst_empresa.Count > 0)
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + solicitud.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + lst_empresa[0].RazonSocial + " [" + lst_empresa[0].idEmpresa + "]</a></td>");
                else
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + solicitud.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + solicitud.idEmpresa + "</a></td>");

                if (lst_contacto.Count > 0)
                    sbuild.Append("<td><a href=\"contacto-mantenimiento.aspx?idc=" + solicitud.idContacto + "\" title=\"" + lst_contacto[0].Nombre + "\" target=\"_blank\"><i class='fas fa-user-plus'></i> " + lst_contacto[0].Nombre + " " + lst_contacto[0].Apellidos + " (" + solicitud.idContacto + ")" + (!String.IsNullOrEmpty(lst_contacto[0].Mail) ? "<br />" + lst_contacto[0].Mail : string.Empty) + (!String.IsNullOrEmpty(lst_contacto[0].TelefonoMovil) ? "<br />(" + lst_contacto[0].TelefonoMovil + (!String.IsNullOrEmpty(lst_contacto[0].Telefono) ? " - " + lst_contacto[0].Telefono + ")" : ")") : !String.IsNullOrEmpty(lst_contacto[0].Telefono) ? "<br />(" + lst_contacto[0].Telefono + ")" : string.Empty) + "</a></td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append("<td><a href=\"solicitud-mantenimiento.aspx?idb=" + solicitud.idEmpresa + "&ids=" + solicitud.idSolicitud + "\" title=\"Descripción puesto\">" + solicitud.descripcion_puesto + "</a></td>");
                sbuild.Append("<td><a href=\"candidatos.aspx?idb=" + solicitud.idEmpresa + "&ids=" + solicitud.idSolicitud + "\" title=\"Nº Candidatos de prácticas\" target=\"_blank\"><span class='v-top'>" + num_candidatos + "</span> </a>");
                sbuild.Append("<a href=\"candidatos-mantenimiento.aspx?idb=" + solicitud.idEmpresa + "&ids=" + solicitud.idSolicitud + "\" title=\"Nuevo candidato\" target=\"_blank\"><i class='fas fa-plus-circle fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"solicitud-mantenimiento.aspx?idb=" + solicitud.idEmpresa + "&ids=" + solicitud.idSolicitud + "\" title=\"Comentarios\">" + solicitud.comentarios + "</a></td>");
                sbuild.Append("<td><a href=\"solicitud-mantenimiento.aspx?idb=" + solicitud.idEmpresa + "&ids=" + solicitud.idSolicitud + "\" title=\"Editar\"><i class='fas fa-edit fa-1-6x'></i></a></td>");
                /// Si tiene candidatos no se puede borrar
                if (num_candidatos > 0)
                    sbuild.Append("<td></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la solicitud?\")){eliminarSolicitud(" + solicitud.idSolicitud + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_solicitudes.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            if (idEmpresa > 0)
                txt_solicitud.InnerHtml = "<i class='fas fa-file-signature'></i> Listado de solicitudes de Prácticas de la empresa <b>" + lst_empresas[0].RazonSocial + "</b> <a href='solicitud-mantenimiento.aspx?idb=" + idEmpresa + "' title='Nueva solicitud de prácticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva solicitud</small></a>";
            else
                txt_solicitud.InnerHtml = "<i class='fas fa-file-signature'></i> Listado de solicitudes de Prácticas de la empresa";
        }        
    }
}