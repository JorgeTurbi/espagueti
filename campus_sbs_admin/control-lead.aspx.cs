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
    public partial class control_lead : System.Web.UI.Page
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
                {
                    /// 1.- Sacar los datos de la url
                    DateTime _date_start = !String.IsNullOrEmpty(Request.QueryString["ds"]) ? DateTime.Parse(Request.QueryString["ds"]) : DateTime.Today;
                    DateTime _date_end = !String.IsNullOrEmpty(Request.QueryString["de"]) ? DateTime.Parse(Request.QueryString["de"]) : DateTime.Today.AddDays(1);
                    long _origen = !String.IsNullOrEmpty(Request.QueryString["ido"]) ? long.Parse(Request.QueryString["ido"]) : -1;
                    if (_origen > 0)
                        load_leads(_date_start, _date_end, _origen);
                    else
                        Response.Redirect("informe-leads.aspx");
                }
            }
        }

        private void load_leads(DateTime _date_start, DateTime _date_end, long _origen)
        {
            /// 0.- Sacar el listado de acciones aprocesar
            List<long> _actions = new List<long>();
            foreach (var id in ConfigurationManager.AppSettings["list_actions"].Split(',').ToList())
            {
                if (!_actions.Contains(long.Parse(id)))
                    _actions.Add(long.Parse(id));
            }

            /// 1.- Sacar los datos de la BBDD
            List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAction(_actions, _date_start, _date_end);
            _leads = _leads.Where(_ => _.IdOrigen == _origen).ToList();

            /// 1.1.- Filtrar los leads que son recordatorios
            _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.2.- Sacar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _leads.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.3.- Filtrar los leads
            _leads = _leads.Except(_leads_incorrectos).ToList();

            /// 1.4.- Sacar el resto de datos de la BBDD
            List<long> _id_courses = _leads.Where(_ => _.IdCurso != null).Select(_ => (long)_.IdCurso.Value).Distinct().ToList();
            List<campus_CURSO> _cursos = da.getCourseByList(_id_courses);
            List<long> _id_leads = _leads.Select(_ => _.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercial(_id_leads);
            List<long> _id_users = _leads.Select(_ => _.idPersona).Distinct().ToList();
            _id_users = _id_users.Union(_seguimientos.Select(_ => _.idComercial).Distinct().ToList()).ToList();
            List<CLIENTES> _users = da.getUserByList(_id_users).ToList();
            List<campus_AUX> _origin = da.getAuxForId(_origen);

            /// 2.- Pintar el título
            txt_title.InnerHtml = "<i class='fas fa-chart-line'></i> Informe de leads por el origen " + _origin.Select(_ => _.Nombre).FirstOrDefault();

            /// 3.- Pintar la tabla de leads
            tabla_leads.InnerHtml = paint_table(_leads, _seguimientos, _users, _cursos);
        }

        private string paint_table(List<campus_ACCIONES_PERSONA> _leads, List<campus_SEG_COMERCIAL> _seguimientos, List<CLIENTES> _users, List<campus_CURSO> _cursos)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Leads\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>N</th>");
            sbuild.Append("<th>F. Lead</th>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Estado</th>");
            sbuild.Append("<th>F. Seg</th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Comentarios</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer los leads
            foreach (var _lead in _leads)
            {
                /// 3.1.- Sacar los seguimientos
                List<campus_SEG_COMERCIAL> _seguimientos_filter = _seguimientos.Where(_ => _.idAccionPersona == _lead.idAccionPersona).OrderByDescending(_ => _.fecha).ToList();
                if (_seguimientos_filter.Count > 0)
                {
                    int _index = 0;
                    foreach (var _seguimiento in _seguimientos_filter)
                    {
                        if (_index == 0)
                            sbuild.Append("<tr><td>1</td>");
                        else
                            sbuild.Append("<tr><td>0</td>");
                        sbuild.Append($"<td>{_lead.Fecha}</td>");
                        sbuild.Append($"<td>{_cursos.Where(_ => _.ID_Curso == _lead.IdCurso).Select(_ => _.Nombre).FirstOrDefault()}</td>");
                        sbuild.Append($"<td><a href='ficha-alumno-crm.aspx?idu={_lead.idPersona}' title='Ficha Alumno' runat='server' target='_blank'><i class='fas fa-user'></i> {_users.Where(_ => _.id_cliente == _lead.idPersona).Select(_ => _.Nombre_Completo).FirstOrDefault()} ({_lead.idPersona}) {_users.Where(_ => _.id_cliente == _lead.idPersona).Select(_ => _.email).FirstOrDefault()}</a></td>");
                        sbuild.Append($"<td>{Utilities.obtenerEstadoSeguimiento(_seguimiento.estado.Value)}</td>");
                        sbuild.Append($"<td>{_seguimiento.fecha}</td>");
                        sbuild.Append($"<td><i class='fas fa-user'></i> {_users.Where(_ => _.id_cliente == _seguimiento.idComercial).Select(_ => _.Nombre_Completo).FirstOrDefault()} ({_seguimiento.idComercial}) {_users.Where(_ => _.id_cliente == _seguimiento.idComercial).Select(_ => _.email).FirstOrDefault()}</td>");
                        sbuild.Append($"<td>{_seguimiento.Comentarios}</td>");
                        sbuild.Append("</tr>");
                        _index++;
                    }
                }
                else
                {
                    sbuild.Append("<tr><td>1</td>");
                    sbuild.Append($"<td>{_lead.Fecha}</td>");
                    sbuild.Append($"<td>{_cursos.Where(_ => _.ID_Curso == _lead.IdCurso).Select(_ => _.Nombre).FirstOrDefault()}</td>");
                    sbuild.Append($"<td><a href='ficha-alumno-crm.aspx?idu={_lead.idPersona}' title='Ficha Alumno' runat='server' target='_blank'><i class='fas fa-user'></i> {_users.Where(_ => _.id_cliente == _lead.idPersona).Select(_ => _.Nombre_Completo).FirstOrDefault()} ({_lead.idPersona}) {_users.Where(_ => _.id_cliente == _lead.idPersona).Select(_ => _.email).FirstOrDefault()}</a></td>");
                    sbuild.Append("<td></td><td></td><td></td><td></td>");
                    sbuild.Append("</tr>");
                }
            }

            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
    }
}