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
    public partial class candidatos : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                long idSolicitud = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;

                if (idEmpresa > 0 && idSolicitud > 0)
                    cargar_datos(idEmpresa, idSolicitud);
                else
                    cargar_datos();

                //Response.Redirect("solicitud-practica.aspx?idb=" + idEmpresa);
            }
        }
        
        protected void btnBorrarCandidato_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_candidate = false;

            try
            {
                long idCandidate = !String.IsNullOrEmpty(hidIdCandidato.Value) ? long.Parse(hidIdCandidato.Value) : -1;
                if (idCandidate > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<PRA_CANDIDATOS> lst_candidatos = da.getCandidatesById(idCandidate);
                    if (lst_candidatos.Count == 1)
                    {
                        /// 2.- Eliminar el contacto
                        delete_candidate = da.deleteCandidate(idCandidate);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar al candidato');</script>");

                LogUtils.InsertarLog(" ERROR - candidatos.cs::btnBorrarCandidato_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_candidate)
                Response.Redirect("candidatos.aspx?idb=" + Request.QueryString["idb"] + "&ids=" + Request.QueryString["ids"]);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar al candidato');</script>");
        }

        private void cargar_datos()
        {
            /// 1.- Sacar los datos de la BBDD
            List<PRA_CANDIDATOS> lst_candidatos = da.getCandidatesByIdRequest(-1);
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(-1);
            //List<CLIENTES> lst_users = da.getActiveUsers(true);
            List<PRA_SOLICITUD_PRACTICA> lst_solicitudes = da.getListPracticesRequestsById(-1);

            /// 1.2.- Sacar los alumnos
            List<long> _users = lst_candidatos.Select(_ => _.idPersona).Distinct().ToList();
            List<CLIENTES> lst_users = da.getUserByList(_users);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Candidatos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Empresa</th>");
            sbuild.Append("<th>Candidato</th>");
            sbuild.Append("<th>Comentarios</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las solicitudes
            foreach (var candidato in lst_candidatos)
            {
                /// 2.3.1.- Sacar los datos de los candidatos
                List<CLIENTES> lst_student = lst_users.Where(c => c.id_cliente == candidato.idPersona).ToList();
                List<PRA_SOLICITUD_PRACTICA> lst_solicitud = lst_solicitudes.Where(s => s.idSolicitud == candidato.idSolicitud).ToList();
                List<PRA_EMPRESA> lst_empresa = new List<PRA_EMPRESA>();
                if (lst_solicitud.Count > 0)
                    lst_empresa = lst_empresas.Where(emp => emp.idEmpresa == lst_solicitud[0].idEmpresa).ToList();

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + candidato.fecha.ToShortDateString() + "</td>");

                if (lst_empresa.Count > 0)
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + lst_empresa[0].idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + lst_empresa[0].RazonSocial + " [" + lst_empresa[0].idEmpresa + "] </a>");
                else
                    sbuild.Append("<td><a href='empresa-mantenimiento.aspx?idb=-1' title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> -1 </a>");

                /*if (lst_student.Count > 0)
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                else
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + candidato.idPersona + "</a></td>");*/

                if (lst_student.Count > 0)
                {
                    if (lst_student[0].fecha_baja.HasValue)
                        sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-red'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                    else if (!String.IsNullOrEmpty(lst_student[0].activo) && lst_student[0].activo == ((int)Constantes.activo.NoActivo).ToString())
                        sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-orange'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                    else
                        sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                }
                else
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + candidato.idPersona + "</a></td>");

                if (lst_empresa.Count > 0)
                {
                    sbuild.Append("<td><a href=\"candidatos-mantenimiento.aspx?idb=" + lst_empresa[0].idEmpresa + "&ids=" + lst_solicitud[0].idSolicitud + "&idc=" + candidato.idCandidato + "\" title=\"" + candidato.comentarios + "\" target=\"_blank\">" + candidato.comentarios + "</a></td>");
                    sbuild.Append("<td><a href=\"candidatos-mantenimiento.aspx?idb=" + lst_empresa[0].idEmpresa + "&ids=" + lst_solicitud[0].idSolicitud + "&idc=" + candidato.idCandidato + "\" title=\"Editar\" target=\"_blank\"><i class=\"fas fa-edit fa-1-6x\"></i></a></td>");
                }
                else
                {
                    sbuild.Append("<td>" + candidato.comentarios + "</td>");
                    sbuild.Append("<td></td>");
                }

                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar al candidato?\")){eliminarCandidato(" + candidato.idCandidato + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_candidatos.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            txt_candidato.InnerHtml = "<i class='fas fa-user-plus'></i> Listado de candidatos";
        }

        private void cargar_datos(long idEmpresa, long idSolicitud)
        {
            /// 1.- Sacar los datos de la BBDD
            List<PRA_CANDIDATOS> lst_candidatos = da.getCandidatesByIdRequest(idSolicitud);
            List<CLIENTES> lst_users = da.getActiveUsers(true);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Candidatos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Candidato</th>");
            sbuild.Append("<th>Comentarios</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las solicitudes
            foreach (var candidato in lst_candidatos)
            {
                /// 2.3.1.- Sacar los datos de los candidatos
                List<CLIENTES> lst_student = lst_users.Where(c => c.id_cliente == candidato.idPersona).ToList();

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + candidato.fecha.ToShortDateString() + "</td>");
                if (lst_student.Count > 0)
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                else
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + candidato.idPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + candidato.idPersona + "</a></td>");
                sbuild.Append("<td><a href=\"candidatos-mantenimiento.aspx?idb=" + idEmpresa + "&ids=" + idSolicitud + "&idc=" + candidato.idCandidato + "\" title=\"" + candidato.comentarios + "\" target=\"_blank\">" + candidato.comentarios + "</a></td>");
                sbuild.Append("<td><a href=\"candidatos-mantenimiento.aspx?idb=" + idEmpresa + "&ids=" + idSolicitud + "&idc=" + candidato.idCandidato + "\" title=\"Editar\" target=\"_blank\"><i class=\"fas fa-edit fa-1-6x\"></i></a></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar al candidato?\")){eliminarCandidato(" + candidato.idCandidato + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_candidatos.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            txt_candidato.InnerHtml = "<i class='fas fa-user-plus'></i> Listado de candidatos <a href='candidatos-mantenimiento.aspx?idb=" + idEmpresa + "&ids=" + idSolicitud + "' title='Nuevo candidato' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Nuevo candidato</small></a>";
        }

        protected void hidIdCandidato_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}