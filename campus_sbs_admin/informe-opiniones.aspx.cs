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
    public partial class informe_opiniones : System.Web.UI.Page
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
                    load_report();
            }
        }

        private void load_report()
        {
            /// 1.- Pintar el título
            txt_informe_estadisticas.InnerHtml = "<i class='fas fa-chart-line'></i> Informe de las estadísticas de opiniones";

            /// 2.- Sacar las opiniones
            List<campus_OPINIONES> lst_opiniones = da.getOpinions(null);
            if (lst_opiniones.Count > 0)
            {
                /// 3.- Pintar los cuadros
                StringBuilder sbuild = new StringBuilder();

                /// 3.1.- TODOS
                sbuild.Append("<div class='col-sm-6'>");
                sbuild.Append("<div class='course-box note margin-b-15'>");
                sbuild.Append("<div class='course-note'>");
                sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center bold'>OPINIONES</span></p>");

                /// 3.2.- Sacar la media
                int suma_all = lst_opiniones.Where(op => op.valoracion != null && op.valoracion.Value > 0).Sum(op => op.valoracion.Value);
                decimal media_all = new decimal();
                if (lst_opiniones.Count > 0)
                    media_all = Math.Round(((decimal)suma_all / (decimal)lst_opiniones.Count), 2);

                if (media_all < new decimal(0.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(0.25) && media_all < new decimal(0.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(0.75) && media_all < new decimal(1.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(1.25) && media_all < new decimal(1.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(1.75) && media_all < new decimal(2.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(2.25) && media_all < new decimal(2.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(2.75) && media_all < new decimal(3.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(3.25) && media_all < new decimal(3.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(3.75) && media_all < new decimal(4.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                else if (media_all >= new decimal(4.25) && media_all < new decimal(4.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> (" + media_all + ")</span></p>");
                else
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> (" + media_all + ")</span></p>");
                                
                /*if (media_all < new decimal(0.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(1))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(1.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(2))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(2.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(3))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(3.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(4))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(4.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='far fa-star'></i> (" + media_all + ")</span></p>");
                else if (media_all < new decimal(5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> (" + media_all + ")</span></p>");
                else
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> (" + media_all + ")</span></p>");*/

                sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'>" + Utilities.PonerPuntoMiles(lst_opiniones.Count) + " opiniones</span></p>");
                sbuild.Append("</div>");
                sbuild.Append("</div>");
                sbuild.Append("</div>");

                /// 3.3.- OPINIONES VISIBLES
                List<campus_OPINIONES> lst_opiniones_visibles = lst_opiniones.Where(op => op.visible != null && op.visible.Value).ToList();

                sbuild.Append("<div class='col-sm-6'>");
                sbuild.Append("<div class='course-box note margin-b-15'>");
                sbuild.Append("<div class='course-note'>");
                sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center bold'>OPINIONES VISIBLES</span></p>");

                /// 3.4.- Sacar la media
                int suma_visible = lst_opiniones_visibles.Where(op => op.valoracion != null && op.valoracion.Value > 0).Sum(op => op.valoracion.Value);
                decimal media_visible = new decimal(0);
                if (lst_opiniones_visibles.Count > 0)
                    media_visible = Math.Round(((decimal)suma_visible / (decimal)lst_opiniones_visibles.Count), 2);

                if (media_visible < new decimal(0.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(0.25) && media_visible < new decimal(0.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(0.75) && media_visible < new decimal(1.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(1.25) && media_visible < new decimal(1.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(1.75) && media_visible < new decimal(2.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(2.25) && media_visible < new decimal(2.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(2.75) && media_visible < new decimal(3.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(3.25) && media_visible < new decimal(3.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(3.75) && media_visible < new decimal(4.25))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='far fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                else if (media_visible >= new decimal(4.25) && media_visible < new decimal(4.75))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star-half-alt text-color-primary'></i> (" + media_visible + ")</span></p>");
                else
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> <i class='fas fa-star text-color-primary'></i> (" + media_visible + ")</span></p>");
                
                /*if (media_visible < new decimal(0.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(1))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(1.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(2))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(2.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(3))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(3.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='far fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(4))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(4.5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='far fa-star'></i> (" + media_visible + ")</span></p>");
                else if (media_visible < new decimal(5))
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star-half-alt'></i> (" + media_visible + ")</span></p>");
                else
                    sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'><i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> <i class='fas fa-star'></i> (" + media_visible + ")</span></p>");
                */

                sbuild.Append("<p class='course-title'><span class='h3 padding-t-5 text-center'>" + Utilities.PonerPuntoMiles(lst_opiniones_visibles.Count()) + " opiniones</span></p>");
                sbuild.Append("</div>");
                sbuild.Append("</div>");
                sbuild.Append("</div>");

                block_informe_status.InnerHtml = sbuild.ToString();

                /// 4.- Sacar los cursos de la BBDD
                List<campus_CURSO> lst_courses = da.getCourses(null);

                /// 5.- Pintar la tabla
                table_listado_programas.InnerHtml = paint_table(lst_opiniones, lst_courses, true);

                /// 6.- Pintar la tabla de opiniones activas
                table_listado_visibles.InnerHtml = paint_table(lst_opiniones_visibles, lst_courses, false);
            }
        }

        private string paint_table(List<campus_OPINIONES> lst_opiniones, List<campus_CURSO> lst_courses, bool todos)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los cursos
            List<long?> lst_id_courses = lst_opiniones.Where(op => op.IdCurso != null).Select(op => op.IdCurso).Distinct().ToList();

            /// 1.- Inicio tabla
            if (todos)
                sbuild.Append("<table id =\"tabla_programs\" class=\"display compact\" style =\"width:100%\"><thead>");
            else
                sbuild.Append("<table id =\"tabla_programs_active\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Valoración</th>");
            sbuild.Append("<th>Nº Valoraciones</th>");
            sbuild.Append("<th>Opiniones</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los listados de suscriptores
            foreach (var curso in lst_id_courses)
            {
                /// 3.1.- Sacar la opiniones del curso
                List<campus_OPINIONES> lst_opiniones_by_course = lst_opiniones.Where(op => op.IdCurso != null && op.IdCurso == curso).ToList();

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + lst_courses.Where(c => c.ID_Curso == curso).Select(c => c.Nombre).FirstOrDefault() + " (" + curso + ")</td>");

                int suma = lst_opiniones_by_course.Where(op => op.valoracion != null && op.valoracion.Value > 0).Sum(op => op.valoracion.Value);
                decimal media = new decimal(0);
                if (lst_opiniones_by_course.Count > 0)
                    media = Math.Round(((decimal)suma / (decimal)lst_opiniones_by_course.Count), 2);

                sbuild.Append("<td>" + media + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_opiniones_by_course.Where(op => op.valoracion != null && op.valoracion.Value > 0).Count()) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(lst_opiniones_by_course.Count) + "</td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}