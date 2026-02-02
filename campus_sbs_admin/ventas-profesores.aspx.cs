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
    public partial class ventas_profesores : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private long _ida;
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
                    txt_titulo.InnerHtml = "<i class='fas fa-euro-sign'></i> Listado de ventas por curso";
                    if (!String.IsNullOrEmpty(Request.QueryString["ida"]))
                        _ida = long.Parse(Request.QueryString["ida"].ToString());
                    else
                        _ida = -1;

                    if (_ida > 0)
                    {
                        loadCoursesByAutor(_ida);
                    }
                    else
                        Response.Redirect("ventas-curso.aspx");
                }
            }
        }
        private void loadCoursesByAutor(long idAutor)
        {
            /// 1.- Sacar los autores
            List<campus_AUTORIA> list_autores = da.getAutorias(0);
            List<campus_AUTORIA> list_autor = getAutoriaByAutor(list_autores, idAutor);

            /// 2.- Sacar los id de los cursos
            List<long> list_id = getIdCourses(list_autor);

            /// 2.- Sacar los cursos de la BBDD
            List<campus_CURSO> list_courses_search = da.getCourses(null);
            List<campus_CURSO> list_courses = getFilterCourses(list_id, list_courses_search);

            /// 3.- Sacar la tutorias
            List<campus_TUTORIA> list_tutorias = da.getTutorias();

            /// 4.- Sacar el resto de tablas
            List<CLIENTES> list_clients = da.getUserById(0);
            List<CLIENTES> list_teachers = da.getProfesores();
            List<campus_CONTENIDO_PROGRAMA> list_programs = da.getContenidoPrograma();
            List<campus_DOCENCIA_GRUPO> list_students = da.getDocenciasGrupo();
            List<campus_DOCENCIA> list_docencias = da.getDocencias(null);

            /// 5.- Pintar los datos
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id='table_sales_autor' class='dataTable compact display' runat='server'><thead><th>ID</th><th>COD</th><th>Nombre</th><th>Ventas</th><th>Alumnos</th><th>Media</th><th>Eur. Autor</th><th>Media Autor/Alum</th></thead><tbody>");
            foreach (campus_CURSO _course in list_courses)
            {
                sbuild.Append("<tr><td>" + _course.ID_Curso + "</td>");
                sbuild.Append("<td>" + _course.COD_Curso + "</td>");
                if (_course.Activo)
                    sbuild.Append("<td><a href='ventas-docencia.aspx?idc=" + _course.ID_Curso + "' title='' runat='server' target='_blank'>" + _course.Nombre + "</a></td>");
                else
                    sbuild.Append("<td><a href='ventas-docencia.aspx?idc=" + _course.ID_Curso + "' title='' runat='server' target='_blank'><span class='red'>" + _course.Nombre + "</span></a></td>");

                /// 6.- Sacar las docencias que tenemos en tutoria
                List<campus_TUTORIA> lista_tutorias = getTutoriasByCourse(list_tutorias, _course.ID_Curso);

                /// 7.- Sacar las docencias 
                List<long> list_id_docencias = getIdDocencias(lista_tutorias);

                /// 8.- Sacar las ventas totales
                List<InformeEconomico> list_inf = getInfEc(list_id_docencias, list_students);

                /// 9.- Pintar las ventas 
                decimal price_sales = getSales(lista_tutorias, list_inf, list_programs, list_docencias);
                sbuild.Append("<td>" + Format(Round((double)price_sales)) + "€</td>");

                /// 10.- Pintar el nº de alumnos
                decimal num_students = getNumberStudents(list_students, list_teachers, list_id_docencias);
                sbuild.Append("<td>" + num_students + "</td>");

                /// 11.- Pintar la media
                if (num_students > 0)
                {
                    decimal _media = price_sales / num_students;
                    sbuild.Append("<td>" + Math.Round(_media, 2) + "€</td>");
                }
                else
                    sbuild.Append("<td>0</td>");

                /// 12.- Sacar los autores
                List<campus_AUTORIA> list_autor_course = getFilterAutoriaByCourse(_course.ID_Curso, list_autor);
                if (list_autor_course.Count > 0)
                {
                    decimal price_calc = ((list_autor_course[0].Pct.Value / 100) * price_sales);
                    decimal media_calc = new decimal(0);
                    if (num_students > 0)
                        media_calc = (price_calc / num_students);

                    sbuild.Append("<td><span class='badge badge-primary' style='margin-right:3px'>" + getAutor(list_autor_course[0].idAutor, list_clients) + " [" + list_autor_course[0].Pct + "%]: " + Format(Round((double)price_calc)) + "€<a class='ml-3' href='Admin_Ficha_Profesor.aspx?idUser=" + list_autor_course[0].idAutor + "'><i class='fas fa-user'></i></a></span></td>");
                    sbuild.Append("<td>" + Format(Round((double)media_calc)) + "€</td>");
                }
                else
                    sbuild.Append("<td></td><td></td>");
                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");
            table_listado.InnerHtml = sbuild.ToString();
        }

        private List<campus_AUTORIA> getAutoriaByAutor(List<campus_AUTORIA> list_autores, long idAutor)
        {
            List<campus_AUTORIA> list = new List<campus_AUTORIA>();
            foreach (campus_AUTORIA autor in list_autores)
            {
                if (autor.idAutor == idAutor)
                    list.Add(autor);
            }
            return list;
        }
        private List<long> getIdCourses(List<campus_AUTORIA> list_autor)
        {
            List<long> list = new List<long>();
            foreach (campus_AUTORIA autor in list_autor)
            {
                if (!list.Contains(autor.idCurso))
                    list.Add(autor.idCurso);
            }
            return list;
        }
        private List<campus_CURSO> getFilterCourses(List<long> list_id, List<campus_CURSO> list_courses)
        {
            List<campus_CURSO> list = new List<campus_CURSO>();
            foreach (campus_CURSO course in list_courses)
            {
                if (list_id.Contains(course.ID_Curso))
                    list.Add(course);
            }
            return list;
        }

        private List<campus_TUTORIA> getTutoriasByCourse(List<campus_TUTORIA> list, long idCurso)
        {
            List<campus_TUTORIA> lista = new List<campus_TUTORIA>();
            foreach (campus_TUTORIA turoria in list)
            {
                if (turoria.idCurso == idCurso)
                    lista.Add(turoria);
            }
            return lista;
        }
        private List<long> getIdDocencias(List<campus_TUTORIA> list)
        {
            List<long> lista = new List<long>();
            foreach (campus_TUTORIA tutoria in list)
            {
                if (!lista.Contains(tutoria.idDocencia))
                    lista.Add(tutoria.idDocencia);
            }
            return lista;
        }
        private List<InformeEconomico> getInfEc(List<long> list_id, List<campus_DOCENCIA_GRUPO> list_students)
        {
            List<InformeEconomico> list = new List<InformeEconomico>();
            foreach (long id in list_id)
            {
                decimal price = new decimal(0);
                foreach (campus_DOCENCIA_GRUPO student in list_students)
                {
                    if (student.ID_Docencia == id && student.PrecioPagado != null)
                        price += student.PrecioPagado.Value;
                }

                InformeEconomico oEconomic = new InformeEconomico();
                oEconomic.IdDocencia = id;
                oEconomic.Venta = price;
                list.Add(oEconomic);
            }

            return list;
        }

        private decimal getSales(List<campus_TUTORIA> lista_tutorias, List<InformeEconomico> list_sales, List<campus_CONTENIDO_PROGRAMA> list_programs, List<campus_DOCENCIA> list_docencias)
        {
            decimal price = new decimal(0);
            foreach (campus_TUTORIA tutoria in lista_tutorias)
            {
                decimal pct = getPorcentajeCP(tutoria.idDocencia, tutoria.idCurso, list_programs, list_docencias);
                decimal price_sale = getSale(tutoria.idDocencia, list_sales);
                //price += (decimal.Parse(tutoria.Pct) / 100) * (pct / 100) * price_sale;
                price += (pct / 100) * price_sale;
            }
            return price;
        }
        private static decimal getPorcentajeCP(long idDocencia, long idCurso, List<campus_CONTENIDO_PROGRAMA> list_programs, List<campus_DOCENCIA> list_docencias)
        {
            decimal percent = 0;
            foreach (campus_DOCENCIA docencia in list_docencias)
            {
                if (docencia.ID_Docencia == idDocencia)
                    percent = getPctProgram(docencia.ID_Curso, idCurso, list_programs);
            }
            return percent;
        }
        private static decimal getPctProgram(long idCourse, long idCurso, List<campus_CONTENIDO_PROGRAMA> list_programs)
        {
            decimal pct = new decimal(0);

            foreach (campus_CONTENIDO_PROGRAMA program in list_programs)
            {
                if (program.ID_Programa == idCourse && program.ID_Curso == idCurso)
                {
                    pct = program.Pct.HasValue ? program.Pct.Value : 0;
                    break;
                }
            }
            if (pct == 0)
                pct = 100;

            return pct;
        }
        private static decimal getSale(long idDocencia, List<InformeEconomico> list_sales)
        {
            decimal price = new decimal(0);
            foreach (InformeEconomico sale in list_sales)
            {
                if (sale.IdDocencia == idDocencia)
                {
                    price = sale.Venta;
                    break;
                }
            }
            return price;
        }

        private decimal getNumberStudents(List<campus_DOCENCIA_GRUPO> list_students, List<CLIENTES> list_teachers, List<long> list_id)
        {
            decimal number = new decimal(0);
            foreach (long id in list_id)
            {
                foreach (campus_DOCENCIA_GRUPO student in list_students)
                {
                    if (student.ID_Docencia == id)
                    {
                        if (!getProfesor(list_teachers, student.ID_Persona))
                            number++;
                    }
                }
            }
            return number;
        }
        private bool getProfesor(List<CLIENTES> list_teachers, long idStudent)
        {
            bool teacher = false;

            foreach (CLIENTES _teacher in list_teachers)
            {
                if (_teacher.id_cliente == idStudent)
                {
                    teacher = true;
                    break;
                }
            }

            return teacher;
        }
        private string getAutor(long idAutor, List<CLIENTES> list_clients)
        {
            string _autor = string.Empty;
            foreach (CLIENTES _client in list_clients)
            {
                if (_client.id_cliente == idAutor)
                {
                    _autor = _client.Nombre_Completo;
                    break;
                }
            }
            return _autor;
        }

        private List<campus_AUTORIA> getFilterAutoriaByCourse(long idCourse, List<campus_AUTORIA> list_autor)
        {
            List<campus_AUTORIA> list = new List<campus_AUTORIA>();
            foreach (campus_AUTORIA autor in list_autor)
            {
                if (autor.idCurso == idCourse)
                {
                    list.Add(autor);
                    break;
                }
            }
            return list;
        }

        private double Round(double v)
        {
            return (Math.Truncate(v * 100) / 100);
        }

        private string Format(object toformat)
        {
            return string.Format("{0:N2}", toformat);
        }
    }
}