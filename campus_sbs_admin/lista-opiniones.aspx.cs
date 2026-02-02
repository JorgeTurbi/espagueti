using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_opiniones : System.Web.UI.Page
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
                    txt_buscar_opiniones.InnerHtml = "<i class='fas fa-search'></i> Buscar opiniones";
                    txt_lista_opiniones.InnerHtml = "<i class='far fa-list-alt'></i> Listado de opiniones <a href='opinion-mantenimiento.aspx' title='Añadir opinión' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir opinión</small></a><a href='javascript:void(0);' onclick='opiniones_activas()' title='Ver opiniones activas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-eye fa-2x'></i> Ver opiniones activas</small></a><a href='javascript:void(0);' onclick='opiniones_pendientes()' title='Ver opiniones pendientes' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-eye-slash fa-2x'></i> Ver opiniones pendientes</small></a><a href='informe-opiniones.aspx' title='Ver estadísticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-chart-bar fa-2x'></i> Ver estadísticas</small></a>";

                    /// 2.- Cargar los programas
                    cargar_cursos();

                    if (!String.IsNullOrEmpty(Request.QueryString["idc"]))
                    {
                        _idc = int.Parse(Request.QueryString["idc"].ToString());

                        table_listado_opiniones.InnerHtml = paint_table(da.getOpinions(0, _idc));
                    }
                    else
                    {
                        _idc = -1;
                        table_listado_opiniones.InnerHtml = paint_table(da.getOpinions(0, 0));
                    }

                    ddlPrograma.ClearSelection(); //making sure the previous selection has been cleared
                    ddlPrograma.Items.FindByValue(_idc.ToString()).Selected = true;
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            /// 1.- Recuperar los datos del formulario
            long idUser = !String.IsNullOrEmpty(idAlumno.Value) ? long.Parse(idAlumno.Value) : -1;
            long idCurso = long.Parse(ddlPrograma.SelectedValue);

            if (idUser > 0 || idCurso > 0)
            {
                /// 2.- Sacar las opiniones
                List<campus_OPINIONES> lst_opinions = da.getOpinions(idUser, idCurso);

                /// 3.- Pintar las opiniones
                table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
            }
        }

        protected void btnActivarOpinion_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Activar la opinión
            bool activar_opinion = false;

            try
            {
                long idOpinion = !String.IsNullOrEmpty(hidIdOpinion.Value) ? long.Parse(hidIdOpinion.Value) : -1;
                if (idOpinion > 0)
                {
                    List<campus_OPINIONES> lst = da.getOpinionById(idOpinion);
                    if (lst.Count == 1)
                        /// 2.- Activar opinión
                        activar_opinion = da.updateOpinion(lst[0], true);
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - lista-opiniones.cs::btnActivarOpinion_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (activar_opinion)
            {
                /// 3.- Recuperar los datos del formulario
                long idUser = !String.IsNullOrEmpty(idAlumno.Value) ? long.Parse(idAlumno.Value) : -1;
                long idCurso = long.Parse(ddlPrograma.SelectedValue);

                if (idUser > 0 || idCurso > 0)
                {
                    /// 4.- Sacar las opiniones
                    List<campus_OPINIONES> lst_opinions = da.getOpinions(idUser, idCurso);

                    /// 5.- Pintar las opiniones
                    table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
                }
                else if (!String.IsNullOrEmpty(hidPendientes.Value) && hidPendientes.Value == "1")
                {
                    /// 4.- Sacar las opiniones
                    List<campus_OPINIONES> lst_opinions = da.getOpinions(false);

                    /// 5.- Pintar las opiniones
                    table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
                }
                else if (!String.IsNullOrEmpty(hidPendientes.Value) && hidPendientes.Value == "0")
                {
                    /// 4.- Sacar las opiniones
                    List<campus_OPINIONES> lst_opinions = da.getOpinions(true);

                    /// 5.- Pintar las opiniones
                    table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
                }
            }
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar la opinión');</script>");
        }

        protected void btnDesactivarOpinion_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Desactivar la opinión
            bool activar_opinion = false;

            try
            {
                long idOpinion = !String.IsNullOrEmpty(hidIdOpinion.Value) ? long.Parse(hidIdOpinion.Value) : -1;
                if (idOpinion > 0)
                {
                    List<campus_OPINIONES> lst = da.getOpinionById(idOpinion);
                    if (lst.Count == 1)
                        /// 2.- Desactivar opinión
                        activar_opinion = da.updateOpinion(lst[0], false);
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - lista-opiniones.cs::btnActivarOpinion_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (activar_opinion)
            {
                /// 3.- Recuperar los datos del formulario
                long idUser = !String.IsNullOrEmpty(idAlumno.Value) ? long.Parse(idAlumno.Value) : -1;
                long idCurso = long.Parse(ddlPrograma.SelectedValue);

                if (idUser > 0 || idCurso > 0)
                {
                    /// 4.- Sacar las opiniones
                    List<campus_OPINIONES> lst_opinions = da.getOpinions(idUser, idCurso);

                    /// 5.- Pintar las opiniones
                    table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
                }
                else if (!String.IsNullOrEmpty(hidPendientes.Value) && hidPendientes.Value == "1")
                {
                    /// 4.- Sacar las opiniones
                    List<campus_OPINIONES> lst_opinions = da.getOpinions(false);

                    /// 5.- Pintar las opiniones
                    table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
                }
                else if (!String.IsNullOrEmpty(hidPendientes.Value) && hidPendientes.Value == "0")
                {
                    /// 4.- Sacar las opiniones
                    List<campus_OPINIONES> lst_opinions = da.getOpinions(true);

                    /// 5.- Pintar las opiniones
                    table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
                }
            }
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al desactivar la opinión');</script>");
        }

        protected void btnOpinionesPendientes_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Sacar las opiniones
            List<campus_OPINIONES> lst_opinions = da.getOpinions(false);

            /// 2.- Pintar las opiniones
            table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
        }

        protected void btnOpinionesActivas_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Sacar las opiniones
            List<campus_OPINIONES> lst_opinions = da.getOpinions(true);

            /// 2.- Pintar las opiniones
            table_listado_opiniones.InnerHtml = paint_table(lst_opinions);
        }

        [WebMethod(Description = "Busca alumnos a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> search_student(string name)
        {
            DataAccess da = new DataAccess();

            List<Usuarios> list_users = new List<Usuarios>();
            List<CLIENTES> lst_users = da.getUserBySearch(name, null);
            if (lst_users.Count > 0)
                list_users = lst_users.Select(user => new Usuarios { id_usuario = user.id_cliente, nombre_completo = user.Nombre_Completo }).ToList();
            return list_users;
        }

        private void cargar_cursos()
        {
            /// 1.- Cargar los cursos
            List<campus_CURSO> lst_courses = da.getCourses(null).OrderBy(c => c.Nombre).ToList();
            if (lst_courses.Count > 0)
            {
                foreach (var course in lst_courses)
                {
                    if (!course.Activo)
                    {
                        ListItem li = new ListItem(course.Nombre + " (" + course.COD_Curso + ") V" + course.Version, course.ID_Curso.ToString());
                        li.Attributes.Add("style", "color:red");

                        ddlPrograma.Items.Add(li);
                    }
                    else
                        ddlPrograma.Items.Add(new ListItem(course.Nombre + " (" + course.COD_Curso + ") V" + course.Version, course.ID_Curso.ToString()));
                }

                ddlPrograma.Items.Add(new ListItem("Seleccione", "-1"));
                ddlPrograma.SelectedValue = "-1";
            }
        }

        private string paint_table(List<campus_OPINIONES> lst_opinions)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los cursos
            List<campus_CURSO> lst_courses = da.getCourses(null);

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Opiniones\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>Valoración</th>");
            sbuild.Append("<th>Comentario</th>");
            sbuild.Append("<th>Recomienda</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar las opiniones
            foreach (var opinion in lst_opinions)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + opinion.Fecha.Value.ToShortDateString() + "</td>");

                if (opinion.IdPersona != null)
                    sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(opinion.Nombre) + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + opinion.IdPersona + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + opinion.Nombre + " [" + opinion.IdPersona.Value + "]</a></td>");
                else
                    sbuild.Append("<td>" + opinion.Nombre + "</td>");

                sbuild.Append("<td>" + lst_courses.Where(c => c.ID_Curso == opinion.IdCurso).Select(c => c.Nombre).FirstOrDefault() + " (" + opinion.IdCurso + ")</td>");
                sbuild.Append("<td>" + opinion.valoracion + "</td>");
                sbuild.Append("<td>" + opinion.Comentario + "</td>");

                if (opinion.recomendacion != null && opinion.recomendacion.Value)
                    sbuild.Append("<td><i class='fas fa-check fa-1-6x text-color-primary'></i></td>");
                else
                    sbuild.Append("<td></td>");

                if (opinion.visible != null && opinion.visible.Value)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='desactivarOpinion(" + opinion.IdOpinion + ");' title='Ocultar opinion'><i class='fas fa-eye fa-1-6x text-color-green'></i></a></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='activarOpinion(" + opinion.IdOpinion + ");' title='Mostrar opinion'><i class='fas fa-eye-slash fa-1-6x text-color-red'></i></a></td>");

                sbuild.Append("<td><a href='opinion-mantenimiento.aspx?ido=" + opinion.IdOpinion + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}