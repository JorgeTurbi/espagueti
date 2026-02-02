using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class cursos : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private int tipo;
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
                    txt_titulo.InnerHtml = "<i class='fas fa-file'></i> Listado de cursos <a href='cursos-mantenimiento.aspx' title='Añadir curso' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir curso</small></a>";
                    if (!String.IsNullOrEmpty(Request.QueryString["tipo"]))
                    {
                        tipo = int.Parse(Request.QueryString["tipo"].ToString());

                        cargar_cursos(da.getCourses(tipo, true));
                    }
                    else
                    {
                        tipo = -1;
                        cargar_cursos(da.getCourses(true));
                    }
                }
            }
        }

        private void cargar_cursos(List<campus_CURSO> _cursos)
        {
            /// 1.- Sacar datos de la BBDD
            /// da.getCampusCursosTipoCurso()

            List<sbs2_area_funcional> _areas = da.getAreaFuncionalByIdArea(-1);
            List<CLIENTES> _profesores = new List<CLIENTES>();
            List<long> _ids = new List<long>();
            foreach (var item in _cursos.Select(_ => _.ID_Responsable).Distinct().ToList())
            {
                if (item.HasValue)
                    _ids.Add((long)item.Value);
            }
            if (_ids.Count > 0)
                _profesores = da.getUserByList(_ids);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_listado\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Cod. Curso</th>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Activo</th>");
            sbuild.Append("<th>Sesiones</th>");
            sbuild.Append("<th>Área Funcional</th>");
            sbuild.Append("<th>Responsable</th>");
            sbuild.Append("<th>Programa PDF</th>");
            sbuild.Append("<th>Tags</th>");
            sbuild.Append("<th>Título</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");

            //sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");
            /// 2.3.- Pintar las reglas
            foreach (var item in _cursos)
            {
                sbuild.Append("<tr>");
                sbuild.Append($"<td>{item.COD_Curso}</td>");
                sbuild.Append($"<td class='text-left'>{item.Nombre}</td>");
                sbuild.Append($"<td>{(item.Activo ? "SI" : "NO")}</td>");
                sbuild.Append($"<td>{item.Num_Sesiones}</td>");
                sbuild.Append($"<td>{_areas.Where(_ => _.idArea == item.ID_Area_Funcional).Select(_ => _.nombre).FirstOrDefault() }</td>");
                sbuild.Append($"<td>{_profesores.Where(_ => _.id_cliente == item.ID_Responsable).Select(_ => _.Nombre_Completo).FirstOrDefault()}</td>");
                if (!string.IsNullOrEmpty(item.ProgramaPDFCompleto))
                    sbuild.Append($"<td><a href='{ConfigurationManager.AppSettings["ruta_programas_web"]}{item.ProgramaPDFCompleto}' target='_blank'><i class='far fa-file-pdf fa-1-6x text-color-red'></i></a></td>");
                else
                    sbuild.Append($"<td></td>");
                sbuild.Append($"<td>");
                var tags = string.IsNullOrEmpty(item.Tags) ? new string[0] : item.Tags.Split(',');
                foreach (var tag in tags)
                    if (!string.IsNullOrEmpty(tag))
                        sbuild.Append($"<span class='badge badge-primary' style='margin-right:3px'>{tag}</span>");
                sbuild.Append("</td>");
                sbuild.Append($"<td>{(item.generar_titulo.HasValue ? (item.generar_titulo.Value ? "SI" : "NO") : "NO")}</td>");
                sbuild.Append("<td><a href=\"cursos-mantenimiento.aspx?id=" + item.ID_Curso + "\" title=\"Editar\"><i class='fa fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"docencias.aspx?idc=" + item.ID_Curso + "\" title=\"Docencias\"><i class='fa fa-graduation-cap fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"https://www.spainbs.com/ficha.aspx?idc=" + item.ID_Curso + "&force=1\" title=\"Ir a la web\" target='_blank'><i class='fa fa-globe fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"fichaCurso.aspx?idc=" + item.ID_Curso + "\" title=\"Ir a la ficha\"><i class='fa fa-clipboard-list fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"encuestas-curso.aspx?idc=" + item.ID_Curso + "\" title=\"Encuestas\"><i class='fa fa-tasks fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"ventas-curso.aspx?idc=" + item.ID_Curso + "\" title=\"Ventas\"><i class='fa fa-euro-sign fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"lista-opiniones.aspx?idc=" + item.ID_Curso + "\" title=\"Comentarios\"><i class='fa fa-comments fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"automatizar-curso.aspx?idc=" + item.ID_Curso + "\" title=\"Automatizar\"><i class='fa fa-cogs fa-1-6x'></i></a></td>");
                sbuild.Append("</tr>");
            }
            

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado.InnerHtml = sbuild.ToString();
        }
    }
}