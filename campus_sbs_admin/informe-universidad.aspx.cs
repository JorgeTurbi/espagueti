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
    public partial class informe_universidad : System.Web.UI.Page
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
                }
            }
        }

        protected void img_filter_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Paramétros del formulario
            DateTime _date_start = DateTime.Parse(date_start.Value);
            DateTime _date_end = DateTime.Parse(date_end.Value);

            /// 2.- Cargar las matrículas
            load_matriculas(_date_start, _date_end);
        }

        private void load_matriculas(DateTime _date_start, DateTime _date_end)
        {            
            /// 1.- Sacar los datos de la BBDD
            List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);

            /// 1.1.- Filtrar las docencias por las fechas
            _docencias = _docencias.Where(_ => _.FInicio >= _date_start && _.FInicio < _date_end.AddDays(1)).ToList();

            /// 1.2.- Sacar que tengan relleno el campo Curso
            _docencias = _docencias.Where(_ => _.Curso != null).ToList();

            /// 2.- Sacar los ids de las docencias
            List<long> _ids = _docencias.Select(_ => _.ID_Docencia).Distinct().ToList();
            
            /// 2.1.- Sacar las docencias grupo
            List<campus_DOCENCIA_GRUPO> _docencias_group = da.getDocenciasGrupoByListDocencias(_ids);

            /// 2.2.- Sacar los usuarios
            List<long> _id_users = _docencias_group.Select(_ => _.ID_Persona).Distinct().ToList();

            /// 3.- Sacar las asignaciones comerciales
            List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercialByUsers(_id_users);

            /// 4.- Sacar los pagos
            List<campus_DATA_COMERCIAL> _datas = da.getDataComercialByIdDocencia(_ids);

            /// 5.- Sacar las calificaciones
            List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluations = da.getDocenciasGrupoEvaluacionByListDoc(_ids);

            /// 6.- Sacar los comerciales
            List<CLIENTES> _users = da.getUserByList(_id_users);

            /// 6.1.- Filtrar los profesores o administradores
            _users = _users.Where(_ => !((!String.IsNullOrEmpty(_.Profesor) && _.Profesor == ((int)Constantes.activo.Activo).ToString()) || (!String.IsNullOrEmpty(_.Administrador) && _.Administrador == ((int)Constantes.activo.Activo).ToString()))).ToList();

            /// 6.2.- Filtrar los profesores de campus_DOCENCIA_GRUPO
            List<long> _students = _users.Select(_ => _.id_cliente).Distinct().ToList();
            _docencias_group = _docencias_group.Where(_ => _students.Contains(_.ID_Persona)).ToList();

            /// 7.- Pintar la tabla
            tabla_matriculas.InnerHtml = paint_table(_asignaciones, _users, _docencias, _datas, _evaluations, _docencias_group);
        }

        private string paint_table(List<campus_ASIG_COMERCIAL> _asignaciones, List<CLIENTES> _users, List<campus_DOCENCIA> _docencias, List<campus_DATA_COMERCIAL> _datas, List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluations, List<campus_DOCENCIA_GRUPO> _docencias_group)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar el resto de datos de la BBDD
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");
            List<Paises> _countries = da.getCountries();
            List<campus_AUX> _auxiliars = da.getAuxForId(-1);

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Matriculas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Calificación</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Edad</th>");
            sbuild.Append("<th>Nacionalidad</th>");
            sbuild.Append("<th>Pais residencia</th>");
            sbuild.Append("<th>Nivel estudios</th>");
            sbuild.Append("<th>Experiencia</th>");
            sbuild.Append("<th>Situacion laboral</th>");
            sbuild.Append("<th>€ Total</th>");
            sbuild.Append("<th>€ Pte</th>");
            sbuild.Append("<th>€ Prog.</th>");
            sbuild.Append("<th>€ Fund.</th>");
            sbuild.Append("<th>€ Univ.</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th colspan='10'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer los alumnos
            foreach (var _dg in _docencias_group)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + _docencias.Where(_ => _.ID_Docencia == _dg.ID_Docencia).Select(_ => _.Nombre).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _dg.ID_Persona).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _dg.ID_Persona + ")</td>");
                sbuild.Append($"<td>{_evaluations.Where(_ => _.idPersona == _dg.ID_Persona && _.idDocencia == _dg.ID_Docencia && _.idCurso == _docencias.Where(d => d.ID_Docencia == _dg.ID_Docencia).Select(d => d.ID_Curso).FirstOrDefault()).Select(_ => _.calificacion).FirstOrDefault()}</td>");
                sbuild.Append("<td>" + (_users.Where(_ => _.id_cliente == _dg.ID_Persona && _.online != null && _.online.Value).Count() == 1 ? "ONLINE" : "SEMIP") + "</td>");
                DateTime? _birthday = _users.Where(_ => _.id_cliente == _dg.ID_Persona && _.fecha_nac != null).Select(_ => _.fecha_nac).FirstOrDefault();
                int _years = 0;
                if (_birthday != null)
                    _years = DateTime.Today.AddTicks(-_birthday.Value.Ticks).Year - 1;
                sbuild.Append($"<td>{_years}</td>");
                sbuild.Append($"<td>{_users.Where(_ => _.id_cliente == _dg.ID_Persona).Select(_ => _.Nacionalidad).FirstOrDefault()}</td>");
                long _pais = _users.Where(_ => _.id_cliente == _dg.ID_Persona && _.id_pais != null).Select(_ => _.id_pais.Value).FirstOrDefault();
                sbuild.Append($"<td>{_countries.Where(_ => _.id_pais == _pais).Select(_ => _.nombre).FirstOrDefault()}</td>");
                int? _level = _users.Where(_ => _.id_cliente == _dg.ID_Persona).Select(_ => _.Nivel_Estudios).FirstOrDefault();
                sbuild.Append($"<td>{(_level != null ? _auxiliars.Where(_ => _.ID_Aux == _level).Select(_ => _.Nombre).FirstOrDefault() : string.Empty)}</td>");
                int? _exp = _users.Where(_ => _.id_cliente == _dg.ID_Persona).Select(_ => _.Experiencia).FirstOrDefault();
                sbuild.Append($"<td>{(_exp != null ? _auxiliars.Where(_ => _.ID_Aux == _exp).Select(_ => _.Nombre).FirstOrDefault() : string.Empty)}</td>");
                int? _situation = _users.Where(_ => _.id_cliente == _dg.ID_Persona).Select(_ => _.Situacion_Actual).FirstOrDefault();
                sbuild.Append($"<td>{(_situation != null ? _auxiliars.Where(_ => _.ID_Aux == _situation).Select(_ => _.Nombre).FirstOrDefault() : string.Empty)}</td>");

                List<campus_ASIG_COMERCIAL> _asignacion = _asignaciones.Where(_ => _.idAlumno == _dg.ID_Persona && _.idDocencia == _dg.ID_Docencia).ToList();
                if (_asignacion.Count == 1)
                {
                    decimal _pvp = _asignacion[0].EUR_PVP_Becado != null ? _asignacion[0].EUR_PVP_Becado.Value : 0;
                    decimal _fundacion = _asignacion[0].EUR_Aportacion_Fundacion != null ? _asignacion[0].EUR_Aportacion_Fundacion.Value : 0;
                    decimal _universidad = _asignacion[0].EUR_Universidad != null ? _asignacion[0].EUR_Universidad.Value : 0;
                    decimal _real = _datas.Where(_ => _.idAlumno == _asignacion[0].idAlumno && _.idDocencia == _asignacion[0].idDocencia && _.EUR_real != null).Sum(_ => _.EUR_real).Value;
                    decimal _pendiente = _pvp + _fundacion + _universidad - _real;

                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp + _fundacion + _universidad) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pendiente) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_fundacion) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_universidad) + "</td>");
                }
                else
                    sbuild.Append("<td>0</td><td>0</td><td>0</td><td>0</td><td>0</td>");
                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }


        /*private void load_matriculas(DateTime _date_start, DateTime _date_end)
        {
            /// 1.- Sacar los datos de la BBDD
            List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);
            List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercial(-1, -1, -1);
            List<CLIENTES> _comerciales = new List<CLIENTES>();

            /// 2.- Filtrar por las fechas
            _asignaciones = _asignaciones.Where(_ => _.Fecha_Matricula >= _date_start && _.Fecha_Matricula < _date_end.AddDays(1)).ToList();

            /// 3.- Sacar los ids de las docencias
            List<long> _ids = _ids = _asignaciones.Select(_ => _.idDocencia).Distinct().ToList();
            List<long> _ids_comerciales = _ids_comerciales = _asignaciones.Where(_ => _.idVendedor != null).Select(_ => _.idVendedor.Value).Distinct().ToList().Union(_asignaciones.Select(_ => _.idAlumno).Distinct().ToList()).Distinct().ToList();
            
            /// 4.- Sacar los pagos
            List<campus_DATA_COMERCIAL> _datas = da.getDataComercialByIdDocencia(_ids);

            /// 5.- Sacar las calificaciones
            List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluations = da.getDocenciasGrupoEvaluacionByListDoc(_ids);

            /// 5.- Sacar los comerciales
            _comerciales = da.getUserByList(_ids_comerciales);

            /// 6.- Pintar la tabla
            tabla_matriculas.InnerHtml = paint_table(_asignaciones, _comerciales, _docencias, _datas, _evaluations);
        }
        private string paint_table(List<campus_ASIG_COMERCIAL> _asignaciones, List<CLIENTES> _users, List<campus_DOCENCIA> _docencias, List<campus_DATA_COMERCIAL> _datas, List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluations)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar el resto de datos de la BBDD
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");            
            List<Paises> _countries = da.getCountries();
            List<campus_AUX> _auxiliars = da.getAuxForId(-1);

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Matriculas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Calificación</th>");            
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Edad</th>");
            sbuild.Append("<th>Nacionalidad</th>");
            sbuild.Append("<th>Pais residencia</th>");
            sbuild.Append("<th>Nivel estudios</th>");
            sbuild.Append("<th>Experiencia</th>");
            sbuild.Append("<th>Situacion laboral</th>");
            sbuild.Append("<th>€ Total</th>");
            sbuild.Append("<th>€ Pte</th>");
            sbuild.Append("<th>€ Prog.</th>");
            sbuild.Append("<th>€ Fund.</th>");
            sbuild.Append("<th>€ Univ.</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th colspan='10'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer las matriculas
            foreach (var _matricula in _asignaciones)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + _docencias.Where(_ => _.ID_Docencia == _matricula.idDocencia).Select(_ => _.Nombre).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</td>");
                sbuild.Append($"<td>{_evaluations.Where(_ => _.idPersona == _matricula.idAlumno && _.idDocencia == _matricula.idDocencia && _.idCurso == _matricula.idCurso).Select(_ => _.calificacion).FirstOrDefault()}</td>");
                sbuild.Append("<td>" + (_users.Where(_ => _.id_cliente == _matricula.idAlumno && _.online != null && _.online.Value).Count() == 1 ? "ONLINE" : "SEMIP") + "</td>");
                DateTime? _birthday = _users.Where(_ => _.id_cliente == _matricula.idAlumno && _.fecha_nac != null).Select(_ => _.fecha_nac).FirstOrDefault();
                int _years = 0;
                if (_birthday != null)
                {
                    TimeSpan _age = DateTime.Today.Subtract(_birthday.Value);
                    DateTime totalTime = new DateTime(_age.Ticks);
                    _years = totalTime.Year;
                }
                sbuild.Append($"<td>{_years}</td>");
                sbuild.Append($"<td>{_users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nacionalidad).FirstOrDefault()}</td>");
                long _pais = _users.Where(_ => _.id_cliente == _matricula.idAlumno && _.id_pais != null).Select(_ => _.id_pais.Value).FirstOrDefault();
                sbuild.Append($"<td>{_countries.Where(_ => _.id_pais == _pais).Select(_ => _.nombre).FirstOrDefault()}</td>");
                int? _level = _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nivel_Estudios).FirstOrDefault();                
                sbuild.Append($"<td>{(_level != null ? _auxiliars.Where(_ => _.ID_Aux == _level).Select(_ => _.Nombre).FirstOrDefault() : string.Empty)}</td>");
                int? _exp = _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Experiencia).FirstOrDefault();
                sbuild.Append($"<td>{(_exp != null ? _auxiliars.Where(_ => _.ID_Aux == _exp).Select(_ => _.Nombre).FirstOrDefault() : string.Empty)}</td>");
                int? _situation = _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Situacion_Actual).FirstOrDefault();
                sbuild.Append($"<td>{(_situation != null ? _auxiliars.Where(_ => _.ID_Aux == _situation).Select(_ => _.Nombre).FirstOrDefault() : string.Empty)}</td>");
                
                decimal _pvp = _matricula.EUR_PVP_Becado != null ? _matricula.EUR_PVP_Becado.Value : 0;
                decimal _fundacion = _matricula.EUR_Aportacion_Fundacion != null ? _matricula.EUR_Aportacion_Fundacion.Value : 0;
                decimal _universidad = _matricula.EUR_Universidad != null ? _matricula.EUR_Universidad.Value : 0;
                decimal _real = _datas.Where(_ => _.idAlumno == _matricula.idAlumno && _.idDocencia == _matricula.idDocencia && _.EUR_real != null).Sum(_ => _.EUR_real).Value;
                decimal _pendiente = _pvp + _fundacion + _universidad - _real;

                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp + _fundacion + _universidad) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pendiente) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_fundacion) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_universidad) + "</td>");
                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }*/
    }
}