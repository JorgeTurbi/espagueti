using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class informe_leads : System.Web.UI.Page
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
                    /// 1.- Poner las fechas de inicio y de fin
                    date_start.Value = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).ToShortDateString();
                    date_end.Value = DateTime.Today.ToShortDateString();

                    /// 2.- Cargar los meses
                    load_months();
                }
            }
        }

        private void load_months()
        {
            ddlMes.Items.Clear();

            ddlMes.Items.Add(new ListItem("Selección por mes", string.Empty));
            ddlMes.Items.Add(new ListItem("Enero", "1"));
            ddlMes.Items.Add(new ListItem("Febrero", "2"));
            ddlMes.Items.Add(new ListItem("Marzo", "3"));
            ddlMes.Items.Add(new ListItem("Abril", "4"));
            ddlMes.Items.Add(new ListItem("Mayo", "5"));
            ddlMes.Items.Add(new ListItem("Junio", "6"));
            ddlMes.Items.Add(new ListItem("Julio", "7"));
            ddlMes.Items.Add(new ListItem("Agosto", "8"));
            ddlMes.Items.Add(new ListItem("Septiembre", "9"));
            ddlMes.Items.Add(new ListItem("Octubre", "10"));
            ddlMes.Items.Add(new ListItem("Noviembre", "11"));
            ddlMes.Items.Add(new ListItem("Diciembre", "12"));
        }

        protected void img_filter_Click(object sender, ImageClickEventArgs e)
        {
            /// 0.- Ocultar los bloques
            blk_comercial.Visible = false;
            blk_origen.Visible = false;

            /// 1.- Paramétros del formulario
            DateTime _date_start = DateTime.Today;
            DateTime _date_end = _date_start.AddDays(1);

            if (!String.IsNullOrEmpty(ddlMes.Value))
            {
                /// 1.0.- Sacar la fechas del mes
                _date_start = new DateTime(DateTime.Today.Year, int.Parse(ddlMes.Value), 1);
                _date_end = _date_start.AddMonths(1);

                /// 1.1.- Pintar los botones
                lnk_month_origenes.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + Utilities.MonthName(_date_start.Month) + " " + _date_start.Year + "</span>";
                lnk_month_comerciales.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + Utilities.MonthName(_date_start.Month) + " " + _date_start.Year + "</span>";
                lnk_month_programas.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + Utilities.MonthName(_date_start.Month) + " " + _date_start.Year + "</span>";
                lnk_month_todos.InnerHtml = "<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + Utilities.MonthName(_date_start.Month) + " " + _date_start.Year + "</span>";

                /// 1.2.- Pintar el mes y año
                hid_month.Value = _date_start.Month.ToString();
                hid_year.Value = _date_start.Year.ToString();
            }
            else
            {
                _date_start = DateTime.Parse(date_start.Value);
                _date_end = DateTime.Parse(date_end.Value).AddDays(1);

                /// 1.1.- Pintar los botones
                lnk_month_origenes.InnerHtml = string.Empty;
                lnk_month_comerciales.InnerHtml = string.Empty;
                lnk_month_programas.InnerHtml = string.Empty;
                lnk_month_todos.InnerHtml = string.Empty;

                /// 1.2.- Pintar el mes y año
                hid_month.Value = string.Empty;
                hid_year.Value = string.Empty;
            }
            string _type = radTipo.SelectedValue;

            /// 2.- Cargar las matrículas
            if (_type == "P")
                load_programs(_date_start, _date_end);
            else
                load_leads(_date_start, _date_end, _type);
        }               

        private void load_programs(DateTime _date_start, DateTime _date_end)
        {
            /// 0.- Sacar el listado de acciones aprocesar
            List<long> _actions = new List<long>();
            foreach (var id in ConfigurationManager.AppSettings["list_actions"].Split(',').ToList())
            {
                if (!_actions.Contains(long.Parse(id)))
                    _actions.Add(long.Parse(id));
            }

            /// 0.1.- Añadir las visitas
            _actions.Add((long)Constantes.accion.Visita);
            _actions.Add((long)Constantes.accion.Landing);

            /// 1.- Sacar los datos de la BBDD
            List<campus_ACCIONES_PERSONA> _leads = da.getActionsByListActionsWithCourse(_actions, _date_start, _date_end);
            List<CLIENTES> _users = new List<CLIENTES>();
            List<campus_AUX> _origenes = _origenes = da.getAuxiliars("ORIGEN");

            /// 1.0.- Filtrar los origenes
            List<long> _ids_origenes = _origenes.Where(_ => _.No_Visible_Informes == null || !_.No_Visible_Informes.Value).Select(_ => _.ID_Aux).ToList();

            /// 1.1.- Filtrar los que no tienen curso
            _leads = _leads.Where(_ => _.IdCurso != null && _.IdCurso > 0).ToList();

            /// 1.2.- Filtrar los leads que son recordatorios
            _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.3.- Sacar los leads incorrectos (242, 243) y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _leads.Where(_ => (_.idAccion == (int)Constantes.accion.Exito_Landing || _.idAccion == (int)Constantes.accion.Landing) && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.4.- Filtrar los leads
            _leads = _leads.Except(_leads_incorrectos).ToList();

            /// 1.5.- Filtrar los leads a partir de los origenes
            _leads = _leads.Where(_ => _.IdOrigen == null || _ids_origenes.Contains(_.IdOrigen.Value)).ToList();

            /// 2.- Sacar los cursos
            List<long> _id_cursos = _leads.Where(_ => _.IdCurso != null).Select(_ => (long)_.IdCurso.Value).Distinct().ToList();

            /// 2.1.- Sacar los cursos de la BBDD
            List<campus_CURSO> _cursos = da.getCourseByList(_id_cursos);

            /// 2.2.- Mostrar el bloque contenedor
            blk_programa.Visible = true;

            /// 2.3.- Pintar la tabla origenes
            tabla_leads_programas.InnerHtml = paint_table_programs(_leads, _cursos);
        }
        private void load_leads(DateTime _date_start, DateTime _date_end, string _type)
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
            List<CLIENTES> _users = new List<CLIENTES>();
            List<campus_AUX> _origenes = _origenes = da.getAuxiliars("ORIGEN");

            /// 1.0.- Filtrar los origenes
            List<long> _ids_origenes = _origenes.Where(_ => _.No_Visible_Informes == null || !_.No_Visible_Informes.Value).Select(_ => _.ID_Aux).ToList();
            
            /// 1.1.- Filtrar los leads que son recordatorios
            _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.2.- Sacar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _leads.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.3.- Filtrar los leads
            _leads = _leads.Except(_leads_incorrectos).ToList();

            /// 1.4.- Filtrar los leads a partir de los origenes
            _leads = _leads.Where(_ => _.IdOrigen == null || _ids_origenes.Contains(_.IdOrigen.Value)).ToList();

            /// 2.- Pintar la tabla
            if (_type == "O")
            {
                /*
                /// 2.1.- Sacar los origenes de la BBDD
                _origenes = da.getAuxiliars("ORIGEN");

                /// 2.2.- Filtrar los origenes
                _origenes = _origenes.Where(_ => _.No_Visible_Informes == null || !_.No_Visible_Informes.Value).ToList();
                */
                /// 2.3.- Mostrar el bloque contenedor
                blk_origen.Visible = true;
            
                /// 2.4.- Pintar la tabla origenes
                tabla_leads_origenes.InnerHtml = paint_table_origin(_leads, _origenes, _date_start, _date_end);                
            }
            else if(_type == "T")
            {
                /// 2.- Sacar los usuarios y los comerciales
                List<long> _comerciales = _leads.Where(_ => _.idComercial != null).Select(_ => _.idComercial.Value).Distinct().ToList();

                /// 2.1.- Sacar los usuarios y los comerciales de la BBDD
                _users = da.getUserByList(_comerciales);

                /// 3.- Sacar los cursos
                List<long> _id_cursos = _leads.Where(_ => _.IdCurso != null).Select(_ => (long)_.IdCurso.Value).Distinct().ToList();

                /// 3.1.- Sacar los cursos de la BBDD
                List<campus_CURSO> _cursos = da.getCourseByList(_id_cursos);

                /// 4.- Mostrar el bloque contenedor
                blk_todos.Visible = true;

                /// 5.- Pintar la tabla origenes
                tabla_leads_todos.InnerHtml = paint_table_leads(_leads, _users, _cursos, _origenes);
            }
            else
            {
                /// 2.1.- Sacar los comerciales
                List<long> _comerciales = _leads.Where(_ => _.idComercial != null).Select(_ => _.idComercial.Value).Distinct().ToList();

                /// 2.2.- Sacar los comerciales de la BBDD
                _users = da.getUserByList(_comerciales);

                /// 2.3.- Mostrar el bloque contenedor
                blk_comercial.Visible = true;

                /// 2.3.- Pintar la tabla origenes
                tabla_leads_comerciales.InnerHtml = paint_table_commercial(_leads, _users);
            }
        }

        private static string paint_table_origin(List<campus_ACCIONES_PERSONA> _leads, List<campus_AUX> _origenes, DateTime _date_start, DateTime _date_end)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Origenes\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Leads</th>");
            sbuild.Append("<th>Leads Ef</th>");
            sbuild.Append("<th>Ratio</th>");
            sbuild.Append("<th>Matriculas</th>");
            sbuild.Append("<th>Ratio M/L</th>");
            sbuild.Append("<th>Ratio M/LEF</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer los origenes
            foreach (var _origen in _origenes)
            {
                /// 3.1.- Sacar los leads por el origen
                List<campus_ACCIONES_PERSONA> _ap_origen = _leads.Where(_ => _.IdOrigen != null && _.IdOrigen == _origen.ID_Aux).ToList();
                if (_ap_origen.Count > 0)
                {
                    int total = _ap_origen.Count;
                    int validos = _ap_origen.Where(_ => _.estado_lead != null && _.estado_lead.Value != (int)Constantes.type_status_action.status_duplicado && _.estado_lead.Value != (int)Constantes.type_status_action.status_rechazado).Count();
                    int matriculas = _ap_origen.Where(_ => _.estado_lead != null && _.estado_lead.Value == (int)Constantes.type_status_action.status_matriculado).Count();
                    decimal porcentaje_leads = ((decimal)validos / (decimal)total) * 100;
                    decimal porcentaje_matriculas = ((decimal)matriculas / (decimal)total) * 100;
                    decimal porcentaje_matriculas_val = validos > 0 ? ((decimal)matriculas / (decimal)validos) * 100 : 0;

                    sbuild.Append("<tr>");
                    sbuild.Append("<td><a href='control-lead.aspx?ds=" + _date_start.ToShortDateString() + "&de=" + _date_end.ToShortDateString() + "&ido=" + _origen.ID_Aux + "' target='_blank'><i class='fas fa-globe fa-1-6x v-middle'></i> " + _origen.Nombre + " (" + _origen.ID_Aux + ")</a></td>");
                    sbuild.Append("<td>" + total + "</td>");
                    sbuild.Append("<td>" + validos + "</td>");                    
                    sbuild.Append("<td>" + Math.Round(porcentaje_leads, 2) + "%</td>");
                    sbuild.Append("<td>" + matriculas + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas, 2) + "%</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas_val, 2) + "%</td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
        private static string paint_table_commercial(List<campus_ACCIONES_PERSONA> _leads, List<CLIENTES> _users)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Comerciales\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Leads</th>");
            sbuild.Append("<th>Leads Ef</th>");
            sbuild.Append("<th>Ratio</th>");
            sbuild.Append("<th>Matriculas</th>");
            sbuild.Append("<th>Ratio M/L</th>");
            sbuild.Append("<th>Ratio M/LEF</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer los comerciales
            foreach (var _comercial in _users)
            {
                /// 3.1.- Sacar los leads por el comercial
                List<campus_ACCIONES_PERSONA> _ap_comercial = _leads.Where(_ => _.idComercial != null && _.idComercial == _comercial.id_cliente).ToList();
                if (_ap_comercial.Count > 0)
                {
                    int total = _ap_comercial.Count;
                    int validos = _ap_comercial.Where(_ => _.estado_lead != null && _.estado_lead.Value != (int)Constantes.type_status_action.status_duplicado && _.estado_lead.Value != (int)Constantes.type_status_action.status_rechazado).Count();
                    int matriculas = _ap_comercial.Where(_ => _.estado_lead != null && _.estado_lead.Value == (int)Constantes.type_status_action.status_matriculado).Count();
                    decimal porcentaje_leads = ((decimal)validos / (decimal)total) * 100;
                    decimal porcentaje_matriculas = ((decimal)matriculas / (decimal)total) * 100;
                    decimal porcentaje_matriculas_val = validos > 0 ? ((decimal)matriculas / (decimal)validos) * 100 : 0;

                    sbuild.Append("<tr>");
                    sbuild.Append("<td><span class='hidden'>" + _comercial.id_cliente + "</span></td>");
                    sbuild.Append("<td>" + _comercial.Nombre_Completo + " (" + _comercial.id_cliente + ")</td>");
                    sbuild.Append("<td>" + total + "</td>");
                    sbuild.Append("<td>" + validos + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_leads, 2) + "%</td>");
                    sbuild.Append("<td>" + matriculas + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas, 2) + "%</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas_val, 2) + "%</td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
        private static string paint_table_programs(List<campus_ACCIONES_PERSONA> _leads, List<campus_CURSO> _cursos)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Programas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Visitas</th>");
            sbuild.Append("<th>Leads</th>");
            sbuild.Append("<th>CTR</th>");
            sbuild.Append("<th>Leads Ef</th>");
            sbuild.Append("<th>Ratio</th>");
            sbuild.Append("<th>Matriculas</th>");
            sbuild.Append("<th>Ratio M/L</th>");
            sbuild.Append("<th>Ratio M/LEF</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Sacar el listado de acciones leads
            List<long> _actions = new List<long>();
            foreach (var id in ConfigurationManager.AppSettings["list_actions"].Split(',').ToList())
            {
                if (!_actions.Contains(long.Parse(id)))
                    _actions.Add(long.Parse(id));
            }

            /// 4.- Recorrer los programas
            foreach (var _curso in _cursos)
            {
                List<campus_ACCIONES_PERSONA> _ap_programas = _leads.Where(_ => _.IdCurso != null && _.IdCurso == _curso.ID_Curso).ToList();
                if (_ap_programas.Count > 0)
                {
                    List<campus_ACCIONES_PERSONA> _leads_validos = _ap_programas.Where(_ => _actions.Contains(_.idAccion)).ToList();
                    int total_leads = _leads_validos.Count;
                    int visitas = _ap_programas.Count - _leads_validos.Count;
                    int validos = _leads_validos.Where(_ => _.estado_lead != null && _.estado_lead.Value != (int)Constantes.type_status_action.status_duplicado && _.estado_lead.Value != (int)Constantes.type_status_action.status_rechazado).Count();
                    int matriculas = _leads_validos.Where(_ => _.estado_lead != null && _.estado_lead.Value == (int)Constantes.type_status_action.status_matriculado).Count();

                    decimal porcentaje_visitas = visitas > 0 ? ((decimal)total_leads / (decimal)visitas) * 100 : 0;
                    decimal porcentaje_leads = total_leads > 0 ? ((decimal)validos / (decimal)total_leads) * 100 : 0;
                    decimal porcentaje_matriculas = total_leads > 0 ? ((decimal)matriculas / (decimal)total_leads) * 100 : 0;
                    decimal porcentaje_matriculas_val = validos > 0 ? ((decimal)matriculas / (decimal)validos) * 100 : 0;

                    sbuild.Append("<tr>");
                    sbuild.Append("<td><span class='hidden'>" + _curso.ID_Curso + "</span></td>");
                    sbuild.Append("<td>" + _curso.Nombre + " (" + _curso.ID_Curso + ")</td>");
                    sbuild.Append("<td>" + visitas + "</td>");
                    sbuild.Append("<td>" + total_leads + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_visitas, 2) + "%</td>");
                    sbuild.Append("<td>" + validos + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_leads, 2) + "%</td>");
                    sbuild.Append("<td>" + matriculas + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas, 2) + "%</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas_val, 2) + "%</td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
        private static string paint_table_leads(List<campus_ACCIONES_PERSONA> _leads, List<CLIENTES> _users, List<campus_CURSO> _cursos, List<campus_AUX> _origenes)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Todos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Leads</th>");
            sbuild.Append("<th>Leads Ef</th>");
            sbuild.Append("<th>Ratio</th>");
            sbuild.Append("<th>Matriculas</th>");
            sbuild.Append("<th>Ratio M/L</th>");
            sbuild.Append("<th>Ratio M/LEF</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer los origenes
            foreach (var _origen in _origenes)
            {
                /// 3.1.- Sacar los leads por el origen
                List<campus_ACCIONES_PERSONA> _ap_origen = _leads.Where(_ => _.IdOrigen != null && _.IdOrigen == _origen.ID_Aux).ToList();
                if (_ap_origen.Count > 0)
                {
                    /// 3.2.- Sacar los programas filtrados por origen
                    List<long> _ids_cursos = _ap_origen.Where(_ => _.IdCurso != null).Select(_ => (long)_.IdCurso).Distinct().ToList();
                    if (_ids_cursos.Count > 0)
                    {
                        /// 3.3.- Recorrer los cursos
                        foreach (var _curso in _ids_cursos)
                        {
                            /// 3.4.- Sacar los leads por curso - origen
                            List<campus_ACCIONES_PERSONA> _ap_curso = _ap_origen.Where(_ => _.IdCurso != null && _.IdCurso == _curso).ToList();
                            if (_ap_curso.Count > 0)
                            {
                                /// 3.5.- Sacar los comerciales filtrados por curso - origen
                                List<long> _comerciales = _ap_curso.Where(_ => _.idComercial != null).Select(_ => _.idComercial.Value).Distinct().ToList();
                                if (_comerciales.Count > 0)
                                {
                                    /// 3.6.- Recorrer los comerciales
                                    foreach (var _comercial in _comerciales)
                                    {
                                        /// 3.4.- Sacar los leads por curso - origen - comercial
                                        List<campus_ACCIONES_PERSONA> _ap_comerciales = _ap_curso.Where(_ => _.idComercial != null && _.idComercial == _comercial).ToList();
                                        if (_ap_comerciales.Count > 0)
                                        {
                                            int total = _ap_comerciales.Count;
                                            int validos = _ap_comerciales.Where(_ => _.estado_lead != null && _.estado_lead.Value != (int)Constantes.type_status_action.status_duplicado && _.estado_lead.Value != (int)Constantes.type_status_action.status_rechazado).Count();
                                            int matriculas = _ap_comerciales.Where(_ => _.estado_lead != null && _.estado_lead.Value == (int)Constantes.type_status_action.status_matriculado).Count();
                                            decimal porcentaje_leads = ((decimal)validos / (decimal)total) * 100;
                                            decimal porcentaje_matriculas = ((decimal)matriculas / (decimal)total) * 100;
                                            decimal porcentaje_matriculas_val = validos > 0 ? ((decimal)matriculas / (decimal)validos) * 100 : 0;

                                            sbuild.Append("<tr>");
                                            sbuild.Append($"<td>{_origen.Nombre} ({_origen.ID_Aux})</td>");
                                            sbuild.Append($"<td>{_cursos.Where(_ => _.ID_Curso == _curso).Select(_ => _.Nombre).FirstOrDefault()} ({_curso})</td>");
                                            sbuild.Append($"<td>{_users.Where(_ => _.id_cliente == _comercial).Select(_ => _.Nombre_Completo).FirstOrDefault()}</td>");
                                            sbuild.Append("<td>" + total + "</td>");
                                            sbuild.Append("<td>" + validos + "</td>");
                                            sbuild.Append("<td>" + Math.Round(porcentaje_leads, 2) + "%</td>");
                                            sbuild.Append("<td>" + matriculas + "</td>");
                                            sbuild.Append("<td>" + Math.Round(porcentaje_matriculas, 2) + "%</td>");
                                            sbuild.Append("<td>" + Math.Round(porcentaje_matriculas_val, 2) + "%</td>");
                                            sbuild.Append("</tr>");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        [WebMethod(Description = "Recupera los leads")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable(long id, string _date_start, string _date_end)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// 1.- Sacar los datos de la BBDD
            List<campus_AUX> _origenes = da.getAuxiliars("ORIGEN");
            List<long> _actions = new List<long>();
            foreach (var _id in ConfigurationManager.AppSettings["list_actions"].Split(',').ToList())
            {
                if (!_actions.Contains(long.Parse(_id)))
                    _actions.Add(long.Parse(_id));
            }

            List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAction(_actions, DateTime.Parse(_date_start), DateTime.Parse(_date_end).AddDays(1));

            /// 1.0.- Filtrar los origenes
            List<long> _ids_origenes = _origenes.Where(_ => _.No_Visible_Informes == null || !_.No_Visible_Informes.Value).Select(_ => _.ID_Aux).ToList();
            
            /// 1.1.- Filtrar los leads que son recordatorios
            _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.2.- Filtrar los leads por comercial
            _leads = _leads.Where(_ => _.idComercial == id).ToList();

            /// 1.3.- Filtrar los leads a partir de los origenes
            _leads = _leads.Where(_ => _.IdOrigen == null || _ids_origenes.Contains(_.IdOrigen.Value)).ToList();

            /// 2.- pintar la tabla
            list.Add(paint_subtable(_leads, _origenes));

            return list;
        }

        private static string paint_subtable(List<campus_ACCIONES_PERSONA> _leads, List<campus_AUX> _origenes)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Comerciales_level2\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Leads</th>");
            sbuild.Append("<th>Leads Ef</th>");
            sbuild.Append("<th>Ratio</th>");
            sbuild.Append("<th>Matriculas</th>");
            sbuild.Append("<th>Ratio M/L</th>");
            sbuild.Append("<th>Ratio M/LEF</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer los origenes
            foreach (var _origen in _origenes)
            {
                /// 3.1.- Sacar los leads por el origen
                List<campus_ACCIONES_PERSONA> _ap_origen = _leads.Where(_ => _.IdOrigen != null && _.IdOrigen == _origen.ID_Aux).ToList();
                if (_ap_origen.Count > 0)
                {
                    int total = _ap_origen.Count;
                    int validos = _ap_origen.Where(_ => _.estado_lead != null && _.estado_lead.Value != (int)Constantes.type_status_action.status_duplicado && _.estado_lead.Value != (int)Constantes.type_status_action.status_rechazado).Count();
                    int matriculas = _ap_origen.Where(_ => _.estado_lead != null && _.estado_lead.Value == (int)Constantes.type_status_action.status_matriculado).Count();
                    decimal porcentaje_leads = ((decimal)validos / (decimal)total) * 100;
                    decimal porcentaje_matriculas = ((decimal)matriculas / (decimal)total) * 100;
                    decimal porcentaje_matriculas_val = validos > 0 ? ((decimal)matriculas / (decimal)validos) * 100 : 0;

                    sbuild.Append("<tr>");
                    sbuild.Append("<td>" + _origen.Nombre + " (" + _origen.ID_Aux + ")</td>");
                    sbuild.Append("<td>" + total + "</td>");
                    sbuild.Append("<td>" + validos + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_leads, 2) + "%</td>");
                    sbuild.Append("<td>" + matriculas + "</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas, 2) + "%</td>");
                    sbuild.Append("<td>" + Math.Round(porcentaje_matriculas_val, 2) + "%</td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        [WebMethod(Description = "Recupera los leads por programa")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable_programs(long id, string _date_start, string _date_end)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// 1.- Sacar los datos de la BBDD
            List<campus_AUX> _origenes = da.getAuxiliars("ORIGEN");
            List<long> _actions = new List<long>();
            foreach (var _id in ConfigurationManager.AppSettings["list_actions"].Split(',').ToList())
            {
                if (!_actions.Contains(long.Parse(_id)))
                    _actions.Add(long.Parse(_id));
            }
            List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAction(_actions, DateTime.Parse(_date_start), DateTime.Parse(_date_end).AddDays(1));

            /// 1.0.- Filtrar los origenes
            List<long> _ids_origenes = _origenes.Where(_ => _.No_Visible_Informes == null || !_.No_Visible_Informes.Value).Select(_ => _.ID_Aux).ToList();

            /// 1.1.- Filtrar los leads que son recordatorios
            _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

            /// 1.2.- Sacar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _leads.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 1.3.- Filtrar los leads
            _leads = _leads.Except(_leads_incorrectos).ToList();

            /// 1.4.- Filtrar por el curso
            _leads = _leads.Where(_ => _.IdCurso != null && _.IdCurso == id).ToList();

            /// 1.5.- Filtrar los leads a partir de los origenes
            _leads = _leads.Where(_ => _.IdOrigen == null || _ids_origenes.Contains(_.IdOrigen.Value)).ToList();

            /// 2.- Sacar los usuarios
            List<long> _id_users = _leads.Select(_ => _.idPersona).Distinct().ToList();
            List<CLIENTES> _usuarios = da.getUserByList(_id_users);

            /// 2.- pintar la tabla
            list.Add(paint_subtable_programs(_leads, _origenes, _usuarios));

            return list;
        }

        private static string paint_subtable_programs(List<campus_ACCIONES_PERSONA> _leads, List<campus_AUX> _origenes, List<CLIENTES> _usuarios)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Programs_level2\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Usuario</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Estado</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer los leads
            foreach (var _lead in _leads)
            {
                sbuild.Append("<tr>");
                sbuild.Append($"<td>{_lead.Fecha}</td>");
                sbuild.Append($"<td><a href='ficha-alumno-crm.aspx?idu={_lead.idPersona}' title='Ficha Alumno' runat='server' target='_blank'><i class='fas fa-user'></i> {_usuarios.Where(_ => _.id_cliente == _lead.idPersona).Select(_ => _.Nombre_Completo).FirstOrDefault()} ({_lead.idPersona})</td>");
                sbuild.Append($"<td>{_origenes.Where(_ => _.ID_Aux == _lead.IdOrigen).Select(_ => _.Nombre).FirstOrDefault()} ({_lead.IdOrigen})</td>");
                sbuild.Append($"<td>{(_lead.estado_lead != null ? Utilities.obtenerEstadoSeguimiento(_lead.estado_lead.Value) : " - ")}</td>");
                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        [WebMethod(Description = "Saca los leads por meses")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_table_month(int month, int year, string type)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// 0.- Sacar las fechas  
            DateTime _date_start = new DateTime(year, month, 1);
            DateTime _date_end = _date_start.AddMonths(1);

            /// 1.- Sacar el listado de acciones aprocesar
            List<long> _actions = new List<long>();
            foreach (var id in ConfigurationManager.AppSettings["list_actions"].Split(',').ToList())
            {
                if (!_actions.Contains(long.Parse(id)))
                    _actions.Add(long.Parse(id));
            }

            if (type == "P")
            {
                /// 1.1.- Añadir las visitas
                _actions.Add((long)Constantes.accion.Visita);
                _actions.Add((long)Constantes.accion.Landing);

                /// 2.- Sacar los datos de la BBDD
                List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAction(_actions, _date_start, _date_end);
                List<CLIENTES> _users = new List<CLIENTES>();
                List<campus_AUX> _origenes = _origenes = da.getAuxiliars("ORIGEN");

                /// 2.0.- Filtrar los origenes
                List<long> _ids_origenes = _origenes.Where(_ => _.No_Visible_Informes == null || !_.No_Visible_Informes.Value).Select(_ => _.ID_Aux).ToList();

                /// 2.1.- Filtrar los que no tienen curso
                _leads = _leads.Where(_ => _.IdCurso != null && _.IdCurso > 0).ToList();

                /// 2.2.- Filtrar los leads que son recordatorios
                _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

                /// 2.3.- Sacar los leads incorrectos (242, 243) y curso 22
                List<campus_ACCIONES_PERSONA> _leads_incorrectos = _leads.Where(_ => (_.idAccion == (int)Constantes.accion.Exito_Landing || _.idAccion == (int)Constantes.accion.Landing) && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

                /// 2.4.- Filtrar los leads
                _leads = _leads.Except(_leads_incorrectos).ToList();

                /// 2.5.- Filtrar los leads a partir de los origenes
                _leads = _leads.Where(_ => _.IdOrigen == null || _ids_origenes.Contains(_.IdOrigen.Value)).ToList();

                /// 3.- Sacar los cursos
                List<long> _id_cursos = _leads.Where(_ => _.IdCurso != null).Select(_ => (long)_.IdCurso.Value).Distinct().ToList();

                /// 3.1.- Sacar los cursos de la BBDD
                List<campus_CURSO> _cursos = da.getCourseByList(_id_cursos);

                /// 4.- Recuperar la tabla de programas
                list.Add(paint_table_programs(_leads, _cursos));
            }
            else
            {
                /// 2.- Sacar los datos de la BBDD
                List<campus_ACCIONES_PERSONA> _leads = da.getActionsByAction(_actions, _date_start, _date_end);
                List<CLIENTES> _users = new List<CLIENTES>();
                List<campus_AUX> _origenes = _origenes = da.getAuxiliars("ORIGEN");

                /// 2.0.- Filtrar los origenes
                List<long> _ids_origenes = _origenes.Where(_ => _.No_Visible_Informes == null || !_.No_Visible_Informes.Value).Select(_ => _.ID_Aux).ToList();

                /// 2.1.- Filtrar los leads que son recordatorios
                _leads = _leads.Where(_ => _.Comentario != "Recordatorio automático").ToList();

                /// 2.2.- Sacar los leads incorrectos 243 y curso 22
                List<campus_ACCIONES_PERSONA> _leads_incorrectos = _leads.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

                /// 2.3.- Filtrar los leads
                _leads = _leads.Except(_leads_incorrectos).ToList();

                /// 2.4.- Filtrar los leads a partir de los origenes
                _leads = _leads.Where(_ => _.IdOrigen == null || _ids_origenes.Contains(_.IdOrigen.Value)).ToList();

                if (type == "O")
                    /// 3.- Recuperar la tabla de origenes
                    list.Add(paint_table_origin(_leads, _origenes, _date_start, _date_end));
                else if (type == "T")
                {
                    /// 3.- Sacar los usuarios y los comerciales
                    List<long> _comerciales = _leads.Where(_ => _.idComercial != null).Select(_ => _.idComercial.Value).Distinct().ToList();

                    /// 3.1.- Sacar los usuarios y los comerciales de la BBDD
                    _users = da.getUserByList(_comerciales);

                    /// 4.- Sacar los cursos
                    List<long> _id_cursos = _leads.Where(_ => _.IdCurso != null).Select(_ => (long)_.IdCurso.Value).Distinct().ToList();

                    /// 4.1.- Sacar los cursos de la BBDD
                    List<campus_CURSO> _cursos = da.getCourseByList(_id_cursos);

                    /// 5.- Pintar la tabla origenes
                    list.Add(paint_table_leads(_leads, _users, _cursos, _origenes));
                }
                else
                {
                    /// 3.1.- Sacar los comerciales
                    List<long> _comerciales = _leads.Where(_ => _.idComercial != null).Select(_ => _.idComercial.Value).Distinct().ToList();

                    /// 3.2.- Sacar los comerciales de la BBDD
                    _users = da.getUserByList(_comerciales);

                    /// 4.- Recuperar la tabla de comerciales
                    list.Add(paint_table_commercial(_leads, _users));
                }
            }
            
            return list;
        }
    }
}