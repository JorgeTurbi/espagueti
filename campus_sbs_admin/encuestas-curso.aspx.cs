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
    public partial class encuestas_curso : System.Web.UI.Page
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
                    txt_titulo.InnerHtml = "<i class='fas fa-tasks'></i> Listado de encuestas por curso";
                    if (!String.IsNullOrEmpty(Request.QueryString["idc"]))
                    {
                        _idc = int.Parse(Request.QueryString["idc"].ToString());

                        render_elements(da.getCourseById(_idc), da.getEncuestaByCurso(idCurso: _idc));
                    }
                    else
                    {
                        _idc = -1;
                        render_elements(da.getCourses(null), da.getAllEncuestas());
                    }

                }
            }
        }

        private void render_elements(List<campus_CURSO> cursos, List<campus_ENCUESTA> encuestas)
        {
            /// 1.- Sacar datos de la BBDD
            List<campus_DOCENCIA> docencias = da.getDocenciasByCurso(cursos.Select(_ => _.ID_Curso).ToList());
            var matriculas = da.getDocenciasGrupoByListDocencias(docencias.Select(_ => _.ID_Docencia).ToList());
            var idsnoalumnos = da.getIdsProfesoresAdmins();
            var P11av = da.getPromedioEncuestas(11);
            var P12av = da.getPromedioEncuestas(12);
            var P13av = da.getPromedioEncuestas(13);
            var P14av = da.getPromedioEncuestas(14);
            var P15av = da.getPromedioEncuestas(15);
            var P16av = da.getPromedioEncuestas(16);
            var P17av = da.getPromedioEncuestas(17);
            var P21av = da.getPromedioEncuestas(21);
            var P22av = da.getPromedioEncuestas(22);
            var P23av = da.getPromedioEncuestas(23);
            var P24av = da.getPromedioEncuestas(24);
            var P31av = da.getPromedioEncuestas(31);
            var P32av = da.getPromedioEncuestas(32);
            var P33av = da.getPromedioEncuestas(33);
            var P34av = da.getPromedioEncuestas(34);
            var P35av = da.getPromedioEncuestas(35);
            var P41av = da.getPromedioEncuestas(41);
            var P42av = da.getPromedioEncuestas(42);
            var P43av = da.getPromedioEncuestas(43);
            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();
            //var matriculas = da.getDocenciasGrupoByListDocencias(docencias.Select(_=>_.ID_Docencia).ToList());
            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_listado\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th title='Número de alumnos'>Alu</th>");
            sbuild.Append("<th title='Número de encuestas'>Enc</th>");
            sbuild.Append("<th>Ediciones</th>");
            sbuild.Append("<th title='Los contenidos han cubierto sus expectativas.'>P11</th>");
            sbuild.Append("<th title='Los temas se han tratado en la profundidad esperada.'>P12</th>");
            sbuild.Append("<th title='La duración del curso ha sido adecuada.'>P13</th>");
            sbuild.Append("<th title='La relación teórico/práctico ha sido adecuada.'>P14</th>");
            sbuild.Append("<th title='La metodología de enseñanza ha sido adecuada.'>P15</th>");
            sbuild.Append("<th title='Los criterios de evaluación han sido correctos.'>P16</th>");
            sbuild.Append("<th title='En general considero que el curso (contenido, duración y metodología) ha sido adecuado.'>P17</th>");
            sbuild.Append("<th title='Me he sentido orientado durante el desarrollo del curso.'>P21</th>");
            sbuild.Append("<th title='He encontrado apoyo del tutor cuando lo he necesitado.'>P22</th>");
            sbuild.Append("<th title='Me ha resultado sencillo y rápido contactar con el tutor.'>P23</th>");
            sbuild.Append("<th title='En general el profesorado es adecuado a los objetivos y al programa formativo del curso.'>P24</th>");
            sbuild.Append("<th title='No ha habido ninguna cuestión técnica relacionada con la informática que me ha entorpecido en el desarrollo normal del curso.'>P31</th>");
            sbuild.Append("<th title='Me ha resultado fácil y sencillo el funcionamiento de la plataforma.'>P32</th>");
            sbuild.Append("<th title='Me ha resultado fácil orientarme dentro de la plataforma al navegar por ella.'>P33</th>");
            sbuild.Append("<th title='El entorno gráfico es el adecuado.'>P34</th>");
            sbuild.Append("<th title='En general considero la plataforma adecuada.'>P35</th>");
            sbuild.Append("<th title='En general se han cubierto mis expectativas previas.'>P41</th>");
            sbuild.Append("<th title='Globalmente estoy satisfecho con la formación recibida.'>P42</th>");
            sbuild.Append("<th title='Para futuras ediciones si mantendría al profesor.'>P43</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var item in cursos)
            {
                sbuild.Append("<tr>");
                sbuild.Append($"<td><a href='encuestas-profesores.aspx?idc={item.ID_Curso}'> {item.Nombre} </a></td>");
                sbuild.Append($"<td class='text-left'>{ getMatriculas(docencias.Where(_ => _.ID_Curso == item.ID_Curso), matriculas, idsnoalumnos)}</td>");
                sbuild.Append($"<td class='text-left'>{encuestas.Count(_ => _.ID_Curso == item.ID_Curso)}</td>");
                sbuild.Append($"<td class='text-left'>{docencias.Count(_ => _.ID_Curso == item.ID_Curso)}</td>");

                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P11av)} (Los contenidos han cubierto sus expectativas.)'>{RenderAverage(encuestas, item, 11, P11av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P12av)} (Los temas se han tratado en la profundidad esperada.)'>{RenderAverage(encuestas, item, 12, P12av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P13av)} (La duración del curso ha sido adecuada.)'>{RenderAverage(encuestas, item, 13, P13av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P14av)} (La relación teórico/práctico ha sido adecuada.)'>{RenderAverage(encuestas, item, 14, P14av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P15av)} (La metodología de enseñanza ha sido adecuada.)'>{RenderAverage(encuestas, item, 15, P15av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P16av)} (Los criterios de evaluación han sido correctos.)'>{RenderAverage(encuestas, item, 16, P16av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P17av)} (En general considero que el curso (contenido, duración y metodología) ha sido adecuado.)'>{RenderAverage(encuestas, item, 17, P17av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P21av)} (Me he sentido orientado durante el desarrollo del curso.)'>{RenderAverage(encuestas, item, 21, P21av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P22av)} (He encontrado apoyo del tutor cuando lo he necesitado.)'>{RenderAverage(encuestas, item, 22, P22av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P23av)} (Me ha resultado sencillo y rápido contactar con el tutor.)'>{RenderAverage(encuestas, item, 23, P23av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P24av)} (En general el profesorado es adecuado a los objetivos y al programa formativo del curso.)'>{RenderAverage(encuestas, item, 24, P24av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P31av)} (No ha habido ninguna cuestión técnica relacionada con la informática que me ha entorpecido en el desarrollo normal del curso.)'>{RenderAverage(encuestas, item, 31, P31av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P32av)} (Me ha resultado fácil y sencillo el funcionamiento de la plataforma.)'>{RenderAverage(encuestas, item, 32, P32av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P33av)} (Me ha resultado fácil orientarme dentro de la plataforma al navegar por ella.)'>{RenderAverage(encuestas, item, 33, P33av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P34av)} (El entorno gráfico es el adecuado.)'>{RenderAverage(encuestas, item, 34, P34av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P35av)} (En general considero la plataforma adecuada.)'>{RenderAverage(encuestas, item, 35, P35av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P41av)} (En general se han cubierto mis expectativas previas.)'>{RenderAverage(encuestas, item, 41, P41av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P42av)} (Globalmente estoy satisfecho con la formación recibida.)'>{RenderAverage(encuestas, item, 42, P42av)}</td>");
                sbuild.Append($@"<td title='Promedio global: {string.Format("{0:N2}", P43av)} (Para futuras ediciones si mantendría al profesor.)'>{RenderAverage(encuestas, item, 43, P43av)}</td>");
                sbuild.Append("</tr>");

            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado.InnerHtml = sbuild.ToString();
        }

        private string getcssclass(double v)
        {
            if (v == 0)
                return "black";
            if (v < 3)
                return "red";
            if (v < 4)
                return "warning";
            return "green";
        }

        private int getMatriculas(IEnumerable<campus_DOCENCIA> docencias, List<campus_DOCENCIA_GRUPO> matriculas, List<long> idsnoalumnos)
        {
            int countmatriculas = 0;
            var docmatriculas = da.getDocenciasGrupoByListDocencias(docencias.Select(_ => _.ID_Docencia).ToList());

            foreach (var item in docmatriculas)
            {
                if (!idsnoalumnos.Contains(item.ID_Persona))
                    countmatriculas++;
            }
            return countmatriculas;
        }

        private double Round(double v)
        {
            return (Math.Truncate(v * 100) / 100);
        }

        private string RenderAverage(List<campus_ENCUESTA> encuestas, campus_CURSO curso, int p, double pav)
        {
            var hassurvey = encuestas.Any(_ => _.ID_Curso == curso.ID_Curso);
            var cursoencuestas = encuestas.Where(_ => _.ID_Curso == curso.ID_Curso);
            double valor = 0;
            if (hassurvey)
                switch (p)
                {
                    case 11:
                        valor = Round(cursoencuestas.Average(_ => _.P11));
                        break;
                    case 12:
                        valor = Round(cursoencuestas.Average(_ => _.P12));
                        break;
                    case 13:
                        valor = Round(cursoencuestas.Average(_ => _.P13));
                        break;
                    case 14:
                        valor = Round(cursoencuestas.Average(_ => _.P14));
                        break;
                    case 15:
                        valor = Round(cursoencuestas.Average(_ => _.P15));
                        break;
                    case 16:
                        valor = Round(cursoencuestas.Average(_ => _.P16));
                        break;
                    case 17:
                        valor = Round(cursoencuestas.Average(_ => _.P17));
                        break;
                    case 21:
                        valor = Round(cursoencuestas.Average(_ => _.P21));
                        break;
                    case 22:
                        valor = Round(cursoencuestas.Average(_ => _.P22));
                        break;
                    case 23:
                        valor = Round(cursoencuestas.Average(_ => _.P23));
                        break;
                    case 24:
                        valor = Round(cursoencuestas.Average(_ => _.P24));
                        break;
                    case 31:
                        valor = Round(cursoencuestas.Average(_ => _.P31));
                        break;
                    case 32:
                        valor = Round(cursoencuestas.Average(_ => _.P32));
                        break;
                    case 33:
                        valor = Round(cursoencuestas.Average(_ => _.P33));
                        break;
                    case 34:
                        valor = Round(cursoencuestas.Average(_ => _.P34));
                        break;
                    case 35:
                        valor = Round(cursoencuestas.Average(_ => _.P35));
                        break;
                    case 41:
                        valor = Round(cursoencuestas.Average(_ => _.P41));
                        break;
                    case 42:
                        valor = Round(cursoencuestas.Average(_ => _.P42));
                        break;
                    case 43:
                        valor = Round(cursoencuestas.Average(_ => _.P43));
                        break;
                    default:
                        break;
                }
            string reference = "";
            if (Round(valor) < Round(pav))
                reference = "<i  title='Valor por debajo de la media' class='fas fa-sort-down'></i>";
            if (Round(valor) > Round(pav))
                reference = "<i  title='Valor por encima de la media' class='fas fa-sort-up'></i>"; ;
            if (Round(valor) == Round(pav))
                reference = "<i  title='Valor en la media' class='fas fa-minus'></i>";
            return $"<span class='badge badge-{getcssclass(valor)} phand'>{string.Format("{0:N2}", valor)} {reference}</span>";
        }
    }
}