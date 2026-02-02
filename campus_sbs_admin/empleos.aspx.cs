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
    public partial class empleos : System.Web.UI.Page
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
                    loadJobs(list_user[0]);
            }
        }
        
        protected void btnBorrarEmpleo_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_job = false;

            try
            {
                long idJob = !String.IsNullOrEmpty(hidIdEmpleo.Value) ? long.Parse(hidIdEmpleo.Value) : -1;
                if (idJob > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<PRA_EMPLEO> lst_empleos = da.getWorksById(idJob);
                    if (lst_empleos.Count == 1)
                    {
                        /// 2.- Eliminar el contacto
                        delete_job = da.deleteJob(idJob);

                        /// 3.- Modificar en empresa el nº de contactos
                        List<PRA_EMPRESA> lst_empresas = da.getBusinessById(lst_empleos[0].idEmpresa);
                        if (lst_empresas.Count == 1)
                        {
                            PRA_EMPRESA empresa = lst_empresas[0];
                            empresa.num_empleos = empresa.num_empleos - 1;

                            bool update_company = da.updateCompany(empresa);
                            if (!update_company)
                            {
                                delete_job = false;
                                LogUtils.InsertarLog(" ERROR - practicas.cs::btnBorrarEmpleo_Click()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al modificar en empresa el nº de empleos");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el empleo');</script>");

                LogUtils.InsertarLog(" ERROR - contactos.cs::btnBorrarEmpleo_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_job)
                Response.Redirect("empleos.aspx" + (!String.IsNullOrEmpty(Request.QueryString["idb"]) ? "?idb=" + Request.QueryString["idb"] : string.Empty));
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el empleo');</script>");
        }

        private void loadJobs(CLIENTES user)
        {
            /// 0.- Recuperar datos de la url
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;

            /// 1.- Sacar los datos de la BBDD
            List<PRA_EMPLEO> lst_trabajos = da.getWorksById(-1);
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(-1);
            //List<CLIENTES> lst_users = da.getActiveUsers(true);

            /// 1.1.- Filtrar los trabajos por empresa
            if (idEmpresa > 0)
                lst_trabajos = lst_trabajos.Where(c => c.idEmpresa == idEmpresa).ToList();

            /// 1.2.- Sacar los alumnos
            List<long> _users = lst_trabajos.Select(_ => _.idAlumno).Distinct().ToList();
            List<CLIENTES> lst_users = da.getUserByList(_users);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Empleos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Datos Empresa</th>");
            sbuild.Append("<th>Datos Alumno</th>");
            sbuild.Append("<th>Contrato</th>");
            sbuild.Append("<th>Comentarios</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar los trabajos
            foreach (var empleo in lst_trabajos)
            {
                /// 2.3.1.- Sacar los datos por el empleo
                List<PRA_EMPRESA> lst_empresa = lst_empresas.Where(emp => emp.idEmpresa == empleo.idEmpresa).ToList();
                List<CLIENTES> lst_student = lst_users.Where(c => c.id_cliente == empleo.idAlumno).ToList();

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + empleo.FechaAlta.ToShortDateString() + "</td>");

                if (lst_empresa.Count > 0)
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + empleo.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + lst_empresa[0].RazonSocial + " [" + lst_empresa[0].idEmpresa + "] </a>");
                else
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + empleo.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + empleo.idEmpresa + "</a>");

                if (lst_student.Count > 0)
                {
                    if (lst_student[0].fecha_baja.HasValue)
                        sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + empleo.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-red'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                    else if (!String.IsNullOrEmpty(lst_student[0].activo) && lst_student[0].activo == ((int)Constantes.activo.NoActivo).ToString())
                        sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + empleo.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-orange'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                    else
                        sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + empleo.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "] (" + lst_student[0].email + ")</a></td>");
                }
                else
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + empleo.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + empleo.idAlumno + "</a></td>");

                if (idEmpresa > 0)
                    sbuild.Append("<td><a href=\"empleo-mantenimiento.aspx?idb=" + idEmpresa + "&ide=" + empleo.idEmpleo + "\" title=\"Contrato\">" + empleo.Contrato + "</a></td>");
                else
                    sbuild.Append("<td><a href=\"empleo-mantenimiento.aspx?ide=" + empleo.idEmpleo + "\" title=\"Contrato\">" + empleo.Contrato + "</a></td>");

                if (idEmpresa > 0)
                    sbuild.Append("<td><a href=\"empleo-mantenimiento.aspx?idb=" + idEmpresa + "&ide=" + empleo.idEmpleo + "\" title=\"Contrato\">" + empleo.Comentarios + "</a></td>");
                else
                    sbuild.Append("<td><a href=\"empleo-mantenimiento.aspx?ide=" + empleo.idEmpleo + "\" title=\"Contrato\">" + empleo.Comentarios + "</a></td>");
                
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el empleo?\")){eliminarEmpleo(" + empleo.idEmpleo + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_empleos.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            if (idEmpresa > 0)
                txt_empleos.InnerHtml = "<i class='fas fa-business-time'></i> Listado de Empleos <a href='empleo-mantenimiento.aspx?idb=" + idEmpresa + "' title='Nuevo empleo' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Nuevo empleo</small></a>";
            else
                txt_empleos.InnerHtml = "<i class='fas fa-business-time'></i> Listado de Empleos <a href='empleo-mantenimiento.aspx' title='Nuevo empleo' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Nuevo empleo</small></a>";
        }
    }
}