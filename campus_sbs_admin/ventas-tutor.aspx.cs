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
    public partial class ventas_tutor : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private long _idt;
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
                    txt_titulo.InnerHtml = "<i class='fas fa-euro-sign'></i> Listado de ventas tutor";
                    if (!String.IsNullOrEmpty(Request.QueryString["idt"]))
                        _idt = int.Parse(Request.QueryString["idt"].ToString());
                    else
                        _idt = -1;

                    if (_idt > 0)
                    {
                        loadCoursesByTutor(_idt);
                    }
                    else
                        Response.Redirect("ventas-curso.aspx");
                }
            }
        }

        private void loadCoursesByTutor(long idTutor)
        {
            /// 1.- Sacar las tutorias de la BBDD
            List<campus_TUTORIA> list_tutorias = da.getTutorias();
            List<campus_TUTORIA> list_tutorias_tutor = getFilterTutoriasByTutor(idTutor, list_tutorias);

            /// 2.- Sacar el resto de tablas
            List<CLIENTES> list_clients = da.getUserById(0);
            List<CLIENTES> list_teachers = da.getProfesores();
            List<campus_CONTENIDO_PROGRAMA> list_programs = da.getContenidoPrograma();
            List<campus_DOCENCIA_GRUPO> list_students = da.getDocenciasGrupo();
            List<campus_DOCENCIA> list_docencias = da.getDocencias(null);

            /// 3.- Sacar las docencias 
            List<long> list_id_docencias = getIdDocencias(list_tutorias_tutor);

            /// 4.- Sacar las ventas totales
            List<InformeEconomico> list_inf = getInfEc(list_id_docencias, list_students);

            /// 5.- Pintar los datos
            StringBuilder sbuild = new StringBuilder();
            sbuild.Append("<table id='table_sales_tutor' class='dataTable compact display' runat='server'><thead><th>Docencia</th><th>Nombre</th><th>Ventas</th><th>Alumnos</th><th>Media</th><th>Ratio C/P</th><th>Ratio Ventas</th><th>Ratio Media</th><th>Eur. Tutor</th><th>Media Tutor/Alum</th></thead><tbody>");
            foreach (InformeEconomico _info in list_inf)
            {
                /// 6.- Datos de la docencia
                sbuild.Append("<tr><td>" + _info.IdDocencia + "</td>");
                if (!endTutoria(_info.IdDocencia, list_tutorias_tutor))
                    sbuild.Append("<td>" + getNameDocencia(_info.IdDocencia, list_docencias) + "</td>");
                else
                    sbuild.Append("<td class='text-danger'>" + getNameDocencia(_info.IdDocencia, list_docencias) + "</td>");

                /// 7.- Ventas por docencia
                List<campus_TUTORIA> lista_tutorias = getFilterByIdDocencia(_info.IdDocencia, list_tutorias_tutor);
                decimal price_sales = getPrice(lista_tutorias, _info, list_programs, list_docencias);
                sbuild.Append("<td>" + Format(Round((double)price_sales)) + "€</td>");

                /// 8.- Pintar el nº de alumnos
                decimal num_students = getNumberStudents(list_students, list_teachers, _info.IdDocencia);
                sbuild.Append("<td>" + num_students + "</td>");

                /// 9.- Pintar la media
                if (num_students > 0)
                {
                    decimal _media = price_sales / num_students;
                    sbuild.Append("<td>" + Format(Round((double)_media)) + "</td>");
                }
                else
                    sbuild.Append("<td>0</td>");

                /// 10.- Sacar el Porcentaje C/P
                if (lista_tutorias.Count > 0)
                {
                    string _c_p = string.Empty;
                    string _price = string.Empty;
                    string _media = string.Empty;
                    foreach (campus_TUTORIA tutoria in lista_tutorias)
                    {
                        decimal pct = getPorcentajeCP(tutoria.idDocencia, tutoria.idCurso, list_programs, list_docencias);
                        decimal price_sale = _info.Venta;
                        decimal price = ((decimal)pct / 100) * price_sale;
                        decimal _media_sale = 0;
                        if (num_students > 0)
                            _media_sale = price / num_students;

                        if (String.IsNullOrEmpty(_c_p))
                            _c_p = "<span class='badge badge-primary' style='margin-right:3px'>(" + tutoria.idCurso + "):" + Format(Round((double)pct)) + "</span>";
                        else
                            _c_p += "<br/><span class='badge badge-primary' style='margin-right:3px'>(" + tutoria.idCurso + "):" + Format(Round((double)pct)) + "</span>";

                        if (String.IsNullOrEmpty(_price))
                            _price = $"<span class='badge badge-primary' style='margin-right:3px'>{Format(Round((double)price))}</span>";
                        else
                            _price += "<br /><span class='badge badge-primary' style='margin-right:3px'>" + Format(Round((double)price) + "</span>");

                        if (String.IsNullOrEmpty(_media))
                            _media = $"<span class='badge badge-primary' style='margin-right:3px'> {Format(Round((double)_media_sale))}</span>";
                        else
                            _media += "<br /><span class='badge badge-primary' style='margin-right:3px'>" + Format(Round((double)_media_sale) + "</span>");
                    }

                    sbuild.Append("<td>" + _c_p + "</td><td>" + _price + "</td><td>" + _media + "</td>");
                }
                else
                    sbuild.Append("<td></td><td></td><td></td>");

                /// 11.- Bloque tutores
                /// 11.1.- Sacar la tutorias del tutor
                List<campus_TUTORIA> list_tutorias_by_tutor = getTutoriaByTutor(lista_tutorias, idTutor, _info.IdDocencia);

                /// 11.2.- Sacar los id de las docencias de las tutorias seleccionadas
                List<long> list_id_docencias_tutor = getIdDocencias(list_tutorias_by_tutor);

                /// 11.3.- Sacar las ventas totales
                List<InformeEconomico> list_inf_tutor = getInfEc(list_id_docencias_tutor, list_students);
                decimal price_sales_tutor = getSalesTutor(list_tutorias_by_tutor, list_inf_tutor, list_programs, list_docencias);

                /// 11.4.- Sacar el nº alumnos
                decimal num_students_tutor = getNumberStudents(list_students, list_teachers, _info.IdDocencia);
                sbuild.Append("<td><span class='badge badge-primary' style='margin-right:3px'>" + getAutor(idTutor, list_clients) + ": " + Format(Round((double)price_sales_tutor)) + "€</span></td>");

                decimal media_calc = 0;
                if (num_students_tutor > 0)
                    media_calc = (price_sales_tutor / num_students_tutor);
                sbuild.Append("<td><span class='badge badge-primary' style='margin-right:3px'>" + Format(Round((double)media_calc)) + "€</span></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");
            table_listado.InnerHtml = sbuild.ToString();
        }

        private List<campus_TUTORIA> getFilterTutoriasByTutor(long idTutor, List<campus_TUTORIA> list_tutorias)
        {
            List<campus_TUTORIA> list = new List<campus_TUTORIA>();
            foreach (campus_TUTORIA tutor in list_tutorias)
            {
                if (tutor.idTutor == idTutor)
                    list.Add(tutor);
            }
            return list;
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
                decimal price = 0;
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
        private bool endTutoria(long idDocencia, List<campus_TUTORIA> lista_tutorias)
        {
            bool end = false;
            foreach (campus_TUTORIA tutoria in lista_tutorias)
            {
                if (tutoria.idDocencia == idDocencia && tutoria.FechaFin < DateTime.Today)
                {
                    end = true;
                    break;
                }
            }
            return end;
        }

        private List<campus_TUTORIA> getFilterByIdDocencia(long idDocencia, List<campus_TUTORIA> list_tutorias)
        {
            List<campus_TUTORIA> list = new List<campus_TUTORIA>();
            foreach (campus_TUTORIA tutor in list_tutorias)
            {
                if (tutor.idDocencia == idDocencia)
                    list.Add(tutor);
            }
            return list;
        }
        private decimal getPrice(List<campus_TUTORIA> lista_tutorias, InformeEconomico _info, List<campus_CONTENIDO_PROGRAMA> list_programs, List<campus_DOCENCIA> list_docencias)
        {
            decimal price = 0;
            foreach (campus_TUTORIA tutoria in lista_tutorias)
            {
                decimal pct = getPorcentajeCP(tutoria.idDocencia, tutoria.idCurso, list_programs, list_docencias);
                decimal price_sale = _info.Venta;
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
            decimal pct = 0;

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
            decimal price = 0;
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
        private decimal getSalesTutor(List<campus_TUTORIA> lista_tutorias, List<InformeEconomico> list_sales, List<campus_CONTENIDO_PROGRAMA> list_programs, List<campus_DOCENCIA> list_docencias)
        {
            decimal price = 0;
            foreach (campus_TUTORIA tutoria in lista_tutorias)
            {
                decimal pct = getPorcentajeCP(tutoria.idDocencia, tutoria.idCurso, list_programs, list_docencias);
                decimal price_sale = getSale(tutoria.idDocencia, list_sales);
                decimal pctt = tutoria.Pct.HasValue ? tutoria.Pct.Value : 0;
                price += (pctt / 100) * (pct / 100) * price_sale;
            }
            return price;
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