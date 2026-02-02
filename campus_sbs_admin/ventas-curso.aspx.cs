using campus_sbs_admin;
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
    public partial class ventas_curso : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private int _idc;
        private bool _active;
        private bool _close;

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
                    if (!String.IsNullOrEmpty(Request.QueryString["idc"]))
                        _idc = int.Parse(Request.QueryString["idc"].ToString());
                    else
                        _idc = -1;

                    _active = !String.IsNullOrEmpty(Request.QueryString["active"]) ? bool.Parse(Request.QueryString["active"]) : true;
                    _close = !String.IsNullOrEmpty(Request.QueryString["close"]) ? bool.Parse(Request.QueryString["close"]) : false;
                    chk_active.Checked = _active;
                    chk_close.Checked = _close;
                    loadSalesCourses(_active, _close);
                }
            }
            else
            {
                _active = Request.Form["chk_active"] == "on";
                _close = Request.Form["chk_close"] == "on";
                loadSalesCourses(_active, _close);
            }
        }

        private void loadSalesCourses(bool active, bool close)
        {
            /// 1.- Sacar los cursos de la BBDD
            var list_courses_search = new List<campus_CURSO>();
            if (_idc > 0)
                list_courses_search.AddRange(da.getCourseById(_idc));
            else
                list_courses_search = da.getCourses(active);

            list_courses_search = list_courses_search.Where(_ => _.Tipo_Curso == 1).ToList();

            List<campus_CURSO> list_courses_filter = getCourses(list_courses_search, active, close);
            campus_CURSO[] list_courses = list_courses_filter.ToArray();

            /// 2.- Sacar los autores y la tutorias de la BBDD
            var list_autores = da.getAutorias(0);
            var list_tutorias = da.getTutorias(0);

            /// 3.- Sacar el resto de tablas
            var list_clients = da.getUserById(0);
            var list_teachers = da.getProfesores();
            var list_programs = da.getContenidoPrograma();
            var list_students = da.getDocenciasGrupo(null);
            var list_docencias = da.getDocencias(null);

            /// 4.- Pintar los datos
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id='tabla_listado' class='dataTable compact display' runat='server'><thead><th>COD</th><th>Nombre</th><th>Ventas</th><th>Alumnos</th><th>Media</th><th>Eur. Autor</th><th>Media Autor/Alum</th><th>Eur. Tutor</th><th>Media Tutor/Alum</th></thead><tbody>");
            foreach (var _course in list_courses)
            {
                sbuild.Append("<td>" + _course.COD_Curso + "</td>");
                if (_course.Activo)
                    sbuild.Append("<td class='namecolumn'><a href='ventas-docencia.aspx?idc=" + _course.ID_Curso + "' title='' runat='server' target='_blank'>" + _course.Nombre + "</a></td>");
                else
                    sbuild.Append("<td><a href='ventas-docencia.aspx?idc=" + _course.ID_Curso + "' title='' runat='server' target='_blank' class='text-danger'><span>" + _course.Nombre + "</span></a></td>");

                /// 5.- Sacar las docencias que tenemos en tutoria
                var lista_tutorias = getTutoriasByCourse(list_tutorias, _course.ID_Curso);

                /// 6.- Sacar las docencias 
                List<long> list_id_docencias = getIdDocencias(lista_tutorias);

                /// 7.- Sacar las ventas totales y el nº de alumnos
                List<InformeEconomico> list_inf = getInfEc(list_id_docencias, list_students);

                /// 8.- Pintar las ventas 
                decimal price_sales = getSales(lista_tutorias, list_inf, list_programs, list_docencias);
                sbuild.Append("<td>" + Format(Round((double)price_sales)) + "€</td>");

                /// 9.- Pintar el nº de alumnos
                decimal num_students = getNumberStudents(list_students, list_teachers, list_id_docencias);
                sbuild.Append("<td>" + num_students + "</td>");

                /// 10.- Pintar la media
                if (num_students > 0)
                {
                    decimal _media = price_sales / num_students;
                    sbuild.Append("<td>" + Math.Round(_media, 2) + "€</td>");
                }
                else
                    sbuild.Append("<td>0</td>");

                /// 11.- Sacar los autores
                List<campus_AUTORIA> lista_autores = getListAutores(_course.ID_Curso, list_autores);
                if (lista_autores.Count > 0)
                {
                    decimal price = new decimal(0);
                    decimal _media_autor_total = new decimal(0);
                    string _autors = string.Empty;
                    string _media_autores = string.Empty;
                    foreach (campus_AUTORIA autor in lista_autores)
                    {
                        decimal price_calc = ((autor.Pct.Value / 100) * price_sales);
                        price = price + price_calc;
                        decimal media_calc = 0;
                        if (num_students > 0)
                            media_calc = (price_calc / num_students);

                        _media_autor_total = _media_autor_total + media_calc;

                        _autors += "<br /><span class='badge badge-primary' style='margin-right:3px'><a href='ventas-profesores.aspx?ida=" + autor.idAutor + "' title='Ficha Alumno' runat='server' target='_blank'>" + getAutor(autor.idAutor, list_clients) + " [" + autor.Pct + "%]: " + Format(Round((double)price_calc)) + "€</a><a href='ficha_profesor.aspx?idUser=" + autor.idAutor + "' class='ml-3'><i class='fas fa-user'></i></a></span>";
                        _media_autores += "<br/><span class='badge badge-primary' style='margin-right:3px'>" + Format(Round((double)media_calc)) + "€</span>";
                    }
                    sbuild.Append("<td><span class='badge badge-green' style='margin-right:3px'><b>Total: " + Format(Round((double)price)) + "€</b></span>" + _autors + "</td>");
                    sbuild.Append("<td><span class='badge badge-green' style='margin-right:3px'><b>" + Format(Round((double)_media_autor_total)) + "€</b></span>" + _media_autores + "</td>");
                }
                else
                    sbuild.Append("<td> - </td><td> - </td>");

                /// 12.- Bloque tutores
                List<long> list_tutors = getTutors(lista_tutorias);
                if (list_tutors.Count > 0)
                {
                    decimal price_tutor_total = 0;
                    decimal _media_tutor_total = 0;
                    string _tutors = string.Empty;
                    string _media_tutores = string.Empty;
                    foreach (long _idTutor in list_tutors)
                    {
                        /// 12.1.- Sacar la tutorias del tutor
                        var list_tutorias_by_tutor = getTutoriaByTutor(lista_tutorias, _idTutor);

                        /// 12.2.- Sacar los id de las docencias de las tutorias seleccionadas
                        List<long> list_id_docencias_tutor = getIdDocencias(list_tutorias_by_tutor);

                        /// 12.3.- Sacar las ventas totales
                        List<InformeEconomico> list_inf_tutor = getInfEc(list_id_docencias_tutor, list_students);
                        decimal price_sales_tutor = getSalesTutor(list_tutorias_by_tutor, list_inf_tutor, list_programs, list_docencias);
                        price_tutor_total = price_tutor_total + price_sales_tutor;

                        /// 12.4.- Sacar el nº alumnos
                        decimal num_students_tutor = getNumberStudents(list_students, list_teachers, list_id_docencias_tutor);

                        _tutors += "<br/><span class='badge badge-primary' style='margin-right:3px'><a href='ventas-tutor.aspx?idt=" + _idTutor + "' title='Ficha Alumno' runat='server' target='_blank'>" + getAutor(_idTutor, list_clients) + ": " + Format(Round((double)price_sales_tutor)) + "€</a><a href='ventas-tutor.aspx?idt=" + _idTutor + "'></a><a href='Admin_Ficha_Profesor.aspx?idUser=" + _idTutor + "' class='ml-3'><i class='fas fa-user'></i></a></span>";

                        decimal media_calc = 0;
                        if (num_students_tutor > 0)
                        {
                            media_calc = (price_sales_tutor / num_students_tutor);
                            _media_tutor_total = _media_tutor_total + media_calc;
                        }

                        _media_tutores += "<br/><span class='badge badge-primary' style='margin-right:3px'>" + Format(Round((double)media_calc)) + "€</span>";
                    }

                    sbuild.Append("<td><span class='badge badge-green' style='margin-right:3px'><b>Total: " + Format(Round((double)price_tutor_total)) + "€</b></span>" + _tutors + "</td>");
                    sbuild.Append("<td><span class='badge badge-green' style='margin-right:3px'><b>" + Format(Round((double)(_media_tutor_total / list_tutors.Count))) + "€</b></span>" + _media_tutores + "</td>");
                }
                else
                    sbuild.Append("<td></td><td></td>");
                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");
            table_listado.InnerHtml = sbuild.ToString();
        }

        private double Round(double v)
        {
            return (Math.Truncate(v * 100) / 100);
        }

        private List<campus_CURSO> getCourses(List<campus_CURSO> list_courses_search, bool active, bool close)
        {
            List<campus_CURSO> list_courses_filter = new List<campus_CURSO>();
            list_courses_filter = list_courses_search.Where(_ => _.Activo == active).ToList();

            if (close)
                list_courses_filter = list_courses_filter.Where(_ => _.FBaja.HasValue && _.FBaja < DateTime.Now.Date).ToList();

            return list_courses_filter;
        }

        private string Format(object toformat)
        {
            return string.Format("{0:N2}", toformat);
        }

        #region Calculos

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
            foreach (var tutoria in lista_tutorias)
            {
                decimal pct = getPorcentajeCP(tutoria.idDocencia, tutoria.idCurso, list_programs, list_docencias);
                decimal price_sale = getSale(tutoria.idDocencia, list_sales);
                price += (pct / 100) * price_sale;
            }
            return price;
        }
        private static decimal getPorcentajeCP(long idDocencia, long idCurso, List<campus_CONTENIDO_PROGRAMA> list_programs, List<campus_DOCENCIA> list_docencias)
        {
            decimal percent = new decimal(0);
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

            foreach (var program in list_programs)
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

            foreach (var _teacher in list_teachers)
            {
                if (_teacher.id_cliente == idStudent)
                {
                    teacher = true;
                    break;
                }
            }

            return teacher;
        }

        #endregion

        #region autor

        private List<campus_AUTORIA> getListAutores(long idCourse, List<campus_AUTORIA> list_autores)
        {
            List<campus_AUTORIA> lista = new List<campus_AUTORIA>();
            foreach (campus_AUTORIA autor in list_autores)
            {
                if (autor.idCurso == idCourse)
                    lista.Add(autor);
            }
            return lista;
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

        #endregion

        #region Tutor

        private List<long> getTutors(List<campus_TUTORIA> lista_tutorias)
        {
            List<long> list = new List<long>();
            foreach (var tutor in lista_tutorias)
            {
                if (!list.Contains(tutor.idTutor))
                    list.Add(tutor.idTutor);
            }
            return list;
        }
        private List<campus_TUTORIA> getTutoriaByTutor(List<campus_TUTORIA> lista_tutorias, long idTutor)
        {
            List<campus_TUTORIA> lista = new List<campus_TUTORIA>();
            foreach (var tutoria in lista_tutorias)
            {
                if (tutoria.idTutor == idTutor)
                    lista.Add(tutoria);
            }
            return lista;
        }

        private decimal getSalesTutor(List<campus_TUTORIA> lista_tutorias, List<InformeEconomico> list_sales, List<campus_CONTENIDO_PROGRAMA> list_programs,
            List<campus_DOCENCIA> list_docencias)
        {
            decimal price = new decimal(0);
            foreach (var tutoria in lista_tutorias)
            {
                decimal pct = getPorcentajeCP(tutoria.idDocencia, tutoria.idCurso, list_programs, list_docencias);
                decimal price_sale = getSale(tutoria.idDocencia, list_sales);
                decimal aux = (tutoria.Pct.HasValue ? tutoria.Pct.Value : 0);
                decimal aux1 = (aux / 100);
                decimal aux2 = (pct / 100);
                price += aux1 * aux2 * price_sale;
            }
            return price;
        }

        #endregion

    }
}