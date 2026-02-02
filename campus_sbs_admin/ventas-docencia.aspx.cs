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
    public partial class ventas_docencia : System.Web.UI.Page
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
                    txt_titulo.InnerHtml = "<i class='fas fa-euro-sign'></i> Listado de ventas por curso";
                    if (!String.IsNullOrEmpty(Request.QueryString["idc"]))
                        _idc = int.Parse(Request.QueryString["idc"].ToString());
                    else
                        _idc = -1;

                    if (_idc > 0)
                    {
                        loadCourse(_idc);
                        loadDocenciasByCourse(_idc);
                    }
                    else
                        Response.Redirect("Admin_Venta.aspx");
                }
            }
        }
        private void loadCourse(long idCurso)
        {
            if (_idc > 0)
            {
                var cursos = da.getCourseById(_idc);
                if (cursos != null)
                    txt_titulo.InnerHtml = "Ventas por docencias del curso <span style='font-style: italic;'>" + cursos[0].Nombre + "</span>";
            }
            else
                txt_titulo.InnerHtml = "Ventas por docencias";
        }
        private void loadDocenciasByCourse(long idCurso)
        {
            /// 1.- Sacar los autores y la tutorias de la BBDD
            var list_autores = da.getAutorias(idCurso);
            var list_tutorias = da.getTutorias();

            /// 2.- Sacar el resto de tablas
            var list_clients = da.getUserById(0);
            var list_teachers = da.getProfesores();
            var list_programs = da.getContenidoPrograma();
            var list_students = da.getDocenciasGrupo(null);
            var list_docencias = da.getDocencias(null);

            /// 3.- Sacar las docencias que tenemos en tutoria
            var lista_tutorias = getTutoriasByCourse(list_tutorias, idCurso);

            /// 4.- Sacar las docencias 
            List<long> list_id_docencias = getIdDocencias(lista_tutorias);

            /// 5.- Sacar las ventas totales
            List<InformeEconomico> list_inf = getInfEc(list_id_docencias, list_students);

            /// 6.- Pintar los datos
            StringBuilder sbuild = new StringBuilder();
            sbuild.Append("<table id='Table_sales_courses' class='dataTable compact display' runat='server'><thead><th>Docencia</th><th>Ventas</th><th>Alumnos</th><th>Media</th><th>Eur. Autor</th><th>Media Autor/Alum</th><th>Eur. Tutor</th><th>Media Tutor/Alum</th></thead><tbody>");

            foreach (InformeEconomico _info in list_inf)
            {
                /// 7.- Datos de la docencia
                sbuild.Append("<tr>");
                if (!endTutoria(_info.IdDocencia, lista_tutorias))
                    sbuild.Append("<td>" + getNameDocencia(_info.IdDocencia, list_docencias) + "</td>");
                else
                    sbuild.Append("<td class='text-danger'>" + getNameDocencia(_info.IdDocencia, list_docencias) + "</td>");

                /// 8.- Ventas por docencia
                decimal price_sales = getPrice(idCurso, _info.IdDocencia, _info, list_programs, list_docencias);
                sbuild.Append("<td>" + Format(Round((double)price_sales)) + "€</td>");

                /// 9.- Pintar el nº de alumnos
                decimal num_students = getNumberStudents(list_students, list_teachers, _info.IdDocencia);
                sbuild.Append("<td>" + num_students + "</td>");

                /// 10.- Pintar la media
                if (num_students > 0)
                {
                    decimal _media = price_sales / num_students;
                    sbuild.Append("<td>" + Math.Round(_media, 2) + "</td>");
                }
                else
                    sbuild.Append("<td>0</td>");

                /// 11.- Sacar los autores
                if (list_autores.Count > 0)
                {
                    decimal price = new decimal(0);
                    decimal _media_autor_total = new decimal(0);
                    string _autors = string.Empty;
                    string _media_autores = string.Empty;
                    foreach (campus_AUTORIA autor in list_autores)
                    {
                        decimal price_calc = ((autor.Pct.Value / 100) * price_sales);
                        price = price + price_calc;
                        decimal media_calc = 0;
                        if (num_students > 0)
                            media_calc = (price_calc / num_students);

                        _media_autor_total = _media_autor_total + media_calc;

                        _autors += "<br/><span class='badge badge-primary' style='margin-right:3px'><a href='ventas-profesores.aspx?ida=" + autor.idAutor + "' title='Ficha Alumno' runat='server' target='_blank'>" + getAutor(autor.idAutor, list_clients) + " [" + autor.Pct + "%]: " + Format(Round((double)price_calc)) + "€</a></span>";
                        _media_autores += "<br/><span class='badge badge-primary' style='margin-right:3px'>" + Format(Round((double)media_calc)) + "€</span>";
                    }
                    sbuild.Append("<td><span class='badge badge-green' style='margin-right:3px'><b>Total: " + Format(Round((double)price)) + "€</b></span>" + _autors + "</td>");
                    sbuild.Append("<td><span class='badge badge-green' style='margin-right:3px'><b>" + Format(Round((double)_media_autor_total)) + "€</b></span>" + _media_autores + "</td>");
                }
                else
                    sbuild.Append("<td> - </td><td> - </td>");

                /// 12.- Bloque tutores
                List<long> list_tutors = getTutors(_info.IdDocencia, lista_tutorias);
                if (list_tutors.Count > 0)
                {
                    decimal price_tutor_total = new decimal(0);
                    decimal _media_tutor_total = new decimal(0);
                    string _tutors = string.Empty;
                    string _media_tutores = string.Empty;
                    foreach (long _idTutor in list_tutors)
                    {
                        /// 12.1.- Sacar la tutorias del tutor
                        List<campus_TUTORIA> list_tutorias_by_tutor = getTutoriaByTutor(lista_tutorias, _idTutor, _info.IdDocencia);

                        /// 12.2.- Sacar los id de las docencias de las tutorias seleccionadas
                        List<long> list_id_docencias_tutor = getIdDocencias(list_tutorias_by_tutor);

                        /// 12.3.- Sacar las ventas totales
                        List<InformeEconomico> list_inf_tutor = getInfEc(list_id_docencias_tutor, list_students);
                        decimal price_sales_tutor = getSalesTutor(list_tutorias_by_tutor, list_inf_tutor, list_programs, list_docencias);
                        price_tutor_total = price_tutor_total + price_sales_tutor;

                        /// 12.4.- Sacar el nº alumnos
                        decimal num_students_tutor = getNumberStudents(list_students, list_teachers, _info.IdDocencia);
                        _tutors += "<br/><span class='badge badge-primary' style='margin-right:3px'><a href='ventas-tutor.aspx?idt=" + _idTutor + "' title='Ficha Alumno' runat='server' target='_blank'>" + getAutor(_idTutor, list_clients) + ": " + Format(Round((double)price_sales_tutor)) + "€</a></span>";

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

        private decimal getPrice(long idCurso, long idDocencia, InformeEconomico _info, List<campus_CONTENIDO_PROGRAMA> list_programs, List<campus_DOCENCIA> list_docencias)
        {
            decimal pct = getPorcentajeCP(idDocencia, idCurso, list_programs, list_docencias);
            decimal price_sale = _info.Venta;
            decimal price = (pct / 100) * price_sale;

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
            decimal pct = 0;

            foreach (var program in list_programs)
            {
                if (program.ID_Programa == idCourse && program.ID_Curso == idCurso)
                {
                    pct = program.Pct.HasValue ? program.Pct.Value : 0;
                    break;
                }
            }
            if (pct == new decimal(0))
                pct = new decimal(100);

            return pct;
        }

        private bool endTutoria(long idDocencia, List<campus_TUTORIA> lista_tutorias)
        {
            bool end = false;
            foreach (var tutoria in lista_tutorias)
            {
                if (tutoria.idDocencia == idDocencia && tutoria.FechaFin < DateTime.Today)
                {
                    end = true;
                    break;
                }
            }
            return end;
        }
        private string getNameDocencia(long idDocencia, List<campus_DOCENCIA> list_docencias)
        {
            string name = string.Empty;
            foreach (campus_DOCENCIA docencia in list_docencias)
            {
                if (docencia.ID_Docencia == idDocencia)
                {
                    name = docencia.Nombre;
                    break;
                }
            }
            return name;
        }
        private decimal getNumberStudents(List<campus_DOCENCIA_GRUPO> list_students, List<CLIENTES> list_teachers, long idDocencia)
        {
            decimal number = 0;
            foreach (campus_DOCENCIA_GRUPO student in list_students)
            {
                if (student.ID_Docencia == idDocencia)
                {
                    if (!getProfesor(list_teachers, student.ID_Persona))
                        number++;
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
        private string getAutor(long idAutor, List<CLIENTES> list_clients)
        {
            string _autor = string.Empty;
            foreach (var _client in list_clients)
            {
                if (_client.id_cliente == idAutor)
                {
                    _autor = _client.Nombre_Completo;
                    break;
                }
            }
            return _autor;
        }

        #region Tutor

        private List<long> getTutors(long idDocencia, List<campus_TUTORIA> lista_tutorias)
        {
            List<long> list = new List<long>();
            foreach (var tutoria in lista_tutorias)
            {
                if (tutoria.idDocencia == idDocencia)
                    list.Add(tutoria.idTutor);
            }
            return list;
        }
        private List<campus_TUTORIA> getTutoriaByTutor(List<campus_TUTORIA> lista_tutorias, long idTutor, long idDocencia)
        {
            List<campus_TUTORIA> lista = new List<campus_TUTORIA>();
            foreach (campus_TUTORIA tutoria in lista_tutorias)
            {
                if (tutoria.idTutor == idTutor && tutoria.idDocencia == idDocencia)
                    lista.Add(tutoria);
            }
            return lista;
        }

        private decimal getSalesTutor(List<campus_TUTORIA> lista_tutorias, List<InformeEconomico> list_sales, List<campus_CONTENIDO_PROGRAMA> list_programs, List<campus_DOCENCIA> list_docencias)
        {
            decimal price = new decimal(0);
            foreach (var tutoria in lista_tutorias)
            {
                decimal pct = getPorcentajeCP(tutoria.idDocencia, tutoria.idCurso, list_programs, list_docencias);
                decimal price_sale = getSale(tutoria.idDocencia, list_sales);
                int pctt = tutoria.Pct.HasValue ? tutoria.Pct.Value : 0;
                decimal aux1 = ((decimal)pctt / 100);
                decimal aux2 = (pct / 100);
                decimal aux3 = aux1 * aux2;
                price += aux3 * price_sale;
            }
            return price;
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

        #endregion
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