using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class informe_matriculas : System.Web.UI.Page
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
            string _type = radTipo.SelectedValue;

            /// 2.- Cargar las matrículas
            load_matriculas(_date_start, _date_end, _type);
        }

        private void load_matriculas(DateTime _date_start, DateTime _date_end, string _type)
        {
            /// 1.- Sacar los datos de la BBDD
            List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);
            List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercial(-1, -1, -1);
            List<CLIENTES> _comerciales = new List<CLIENTES>();

            /// 2.- Filtrar por las fechas
            if (_type == "E")
                _docencias = _docencias.Where(_ => _.FInicio >= _date_start && _.FInicio < _date_end.AddDays(1)).ToList();
            else
                _asignaciones = _asignaciones.Where(_ => _.Fecha_Matricula >= _date_start && _.Fecha_Matricula < _date_end.AddDays(1)).ToList();                

            /// 3.- Sacar los ids de las docencias
            List<long> _ids = new List<long>();
            List<long> _ids_comerciales = new List<long>();
            if (_type == "V")
                _ids = _asignaciones.Select(_ => _.idDocencia).Distinct().ToList();
            else if (_type == "C")
            {
                _ids = _asignaciones.Select(_ => _.idDocencia).Distinct().ToList();
                _ids_comerciales = _asignaciones.Where(_ => _.idVendedor != null).Select(_ => _.idVendedor.Value).Distinct().ToList();
            }
            else if (_type == "T")
            {
                _ids = _asignaciones.Select(_ => _.idDocencia).Distinct().ToList();
                _ids_comerciales = _asignaciones.Where(_ => _.idVendedor != null).Select(_ => _.idVendedor.Value).Distinct().ToList().Union(_asignaciones.Select(_ => _.idAlumno).Distinct().ToList()).Distinct().ToList();
            }
            else
                _ids = _docencias.Select(_ => _.ID_Docencia).Distinct().ToList();

            /// 4.- Sacar los pagos
            List<campus_DATA_COMERCIAL> _datas = da.getDataComercialByIdDocencia(_ids);

            /// 5.- Sacar los comerciales
            if (_type == "C" || _type == "T")
                _comerciales = da.getUserByList(_ids_comerciales);

            /// 6.- Pintar la tabla
            if (_type == "C")
                tabla_matriculas.InnerHtml = paint_table_comercial(_asignaciones, _ids_comerciales, _comerciales, _datas);
            else if (_type == "T")
                tabla_matriculas.InnerHtml = paint_table_all(_asignaciones, _comerciales, _docencias, _datas);
            else
                tabla_matriculas.InnerHtml = paint_table(_asignaciones, _ids, _docencias, _datas);
        }
        
        private string paint_table(List<campus_ASIG_COMERCIAL> _asignaciones, List<long> _ids, List<campus_DOCENCIA> _docencias, List<campus_DATA_COMERCIAL> _datas)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Matriculas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>Nº</th>");
            sbuild.Append("<th>Venta Total</th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Fund.</th>");
            sbuild.Append("<th>Univ.</th>");
            sbuild.Append("<th>Pendiente</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th></th><th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer las docencias
            foreach (var _id in _ids)
            {
                List<campus_ASIG_COMERCIAL> _matriculas = _asignaciones.Where(_ => _.idDocencia == _id).ToList();
                if (_matriculas.Count > 0)
                {
                    /// 3.1.- Sacar el dinero pendiente
                    List<long> _users = _matriculas.Select(_ => _.idAlumno).Distinct().ToList();
                    List<campus_DATA_COMERCIAL> _pendientes = _datas.Where(_ => _users.Contains(_.idAlumno)).ToList();

                    decimal _pvp = _matriculas.Where(_ => _.EUR_PVP_Becado != null).Sum(_ => _.EUR_PVP_Becado).Value;
                    decimal _fundacion = _matriculas.Where(_ => _.EUR_Aportacion_Fundacion != null).Sum(_ => _.EUR_Aportacion_Fundacion).Value;
                    decimal _universidad = _matriculas.Where(_ => _.EUR_Universidad != null).Sum(_ => _.EUR_Universidad).Value;
                    decimal _real = _pendientes.Where(_ => _.idDocencia == _id && _.EUR_real != null).Sum(_ => _.EUR_real).Value;
                    decimal _pendiente = _pvp + _fundacion + _universidad - _real;

                    sbuild.Append("<tr><td><span class='hidden'>" + _id + "</span></td><td>" + _docencias.Where(_ => _.ID_Docencia == _id).Select(_ => _.Nombre).FirstOrDefault() + " (" + _id + ")</td>");
                    sbuild.Append("<td>" + _matriculas.Count + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp + _fundacion + _universidad) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_fundacion) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_universidad) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pendiente) + "</td>");
                    sbuild.Append("<td><a href='https://campusadmin.spainbs.com/admin/Admin-Listado-Clase.aspx?idd=" + _id + "' title='Listado de clase' runat='server' target='_blank'><i class='fas fa-clipboard-list fa-2x'></i></a></td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
        private string paint_table_comercial(List<campus_ASIG_COMERCIAL> _asignaciones, List<long> _ids, List<CLIENTES> _comerciales, List<campus_DATA_COMERCIAL> _datas)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Matriculas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Nº</th>");
            sbuild.Append("<th>Venta Total</th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Fund.</th>");
            sbuild.Append("<th>Univ.</th>");
            sbuild.Append("<th>Pendiente</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th></th><th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer las docencias
            foreach (var _id in _ids)
            {
                List<campus_ASIG_COMERCIAL> _matriculas = _asignaciones.Where(_ => _.idVendedor == _id).ToList();
                if (_matriculas.Count > 0)
                {
                    /// 3.1.- Sacar el dinero pendiente
                    List<long> _users = _matriculas.Select(_ => _.idAlumno).Distinct().ToList();
                    List<campus_DATA_COMERCIAL> _pendientes = _datas.Where(_ => _users.Contains(_.idAlumno)).ToList();

                    decimal _pvp = _matriculas.Where(_ => _.EUR_PVP_Becado != null).Sum(_ => _.EUR_PVP_Becado).Value;
                    decimal _fundacion = _matriculas.Where(_ => _.EUR_Aportacion_Fundacion != null).Sum(_ => _.EUR_Aportacion_Fundacion).Value;
                    decimal _universidad = _matriculas.Where(_ => _.EUR_Universidad != null).Sum(_ => _.EUR_Universidad).Value;
                    decimal _real = _pendientes.Where(_ => _.EUR_real != null).Sum(_ => _.EUR_real).Value;
                    decimal _pendiente = _pvp + _fundacion + _universidad - _real;

                    sbuild.Append("<tr><td><span class='hidden'>" + _id + "</span></td><td>" + _comerciales.Where(_ => _.id_cliente == _id).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _id + ")</td>");
                    sbuild.Append("<td>" + _matriculas.Count + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp + _fundacion + _universidad) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_fundacion) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_universidad) + "</td>");
                    sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pendiente) + "</td>");
                    //sbuild.Append("<td><a href='https://campusadmin.spainbs.com/admin/Admin-Listado-Clase.aspx?idd=" + _id + "' title='Listado de clase' runat='server' target='_blank'><i class='fas fa-clipboard-list fa-2x'></i></a></td>");
                    sbuild.Append("<td></td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
        private string paint_table_all(List<campus_ASIG_COMERCIAL> _asignaciones, List<CLIENTES> _users, List<campus_DOCENCIA> _docencias, List<campus_DATA_COMERCIAL> _datas)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar el resto de datos de la BBDD
            List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Matriculas_All\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Nacionalidad</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>F. Lead</th>");
            sbuild.Append("<th>F. Mat.</th>");
            sbuild.Append("<th>F. Pago</th>");
            sbuild.Append("<th>LTV</th>");
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
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idVendedor).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</td>");
                sbuild.Append("<td>" + (_users.Where(_ => _.id_cliente == _matricula.idAlumno && _.online != null && _.online.Value).Count() == 1 ? "ONLINE" : "SEMIP") + "</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nacionalidad).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + _origins.Where(_ => _.ID_Aux == _matricula.idOrigen).Select(_ => _.Nombre).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + _docencias.Where(_ => _.ID_Docencia == _matricula.idDocencia).Select(_ => _.Nombre).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Lead != null ? _matricula.Fecha_Lead.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Env_Contrato != null ? _matricula.Fecha_Env_Contrato.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Matricula != null ? _matricula.Fecha_Matricula.Value.ToShortDateString() : string.Empty) + "</td>");
                if (_matricula.Fecha_Matricula != null && _matricula.Fecha_Lead != null)
                    sbuild.Append("<td>" + _matricula.Fecha_Matricula.Value.Subtract(_matricula.Fecha_Lead.Value).Days + "</td>");
                else
                    sbuild.Append("<td></td>");

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
        }

        [WebMethod(Description = "Recupera la tabla día y origen")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable(long id, string _date_start, string _date_end, string _type)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            if (_type == "C")
            {
                /// 1.- Sacar datos de la BBDD
                List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);
                List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");
                List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercialByVendedor(id);
                _asignaciones = _asignaciones.Where(_ => _.Fecha_Matricula >= DateTime.Parse(_date_start) && _.Fecha_Matricula < DateTime.Parse(_date_end).AddDays(1)).ToList();
                List<long> _ids = _asignaciones.Select(_ => _.idDocencia).Distinct().ToList();
                List<long> _id_users = _asignaciones.Select(_ => _.idAlumno).Distinct().ToList();
                List<CLIENTES> _users = da.getUserByList(_id_users).ToList();
                List<campus_DATA_COMERCIAL> _pays = da.getDataComercialByIdUser(_id_users);

                /// 2.- Filtrar los pagos por la docencia
                _pays = _pays.Where(_ => _ids.Contains(_.idDocencia)).ToList();

                /// 3.- pintar la tabla
                list.Add(paint_subtable_comercial(_asignaciones, _users, _docencias, _origins, _pays));
            }
            else
            {
                /// 1.- Sacar los datos de la BBDD
                List<campus_DOCENCIA> _docencias = da.getDocenciaById(id);
                List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercial(id, -1, -1);
                List<long> _id_users = _asignaciones.Select(_ => _.idAlumno).Distinct().ToList();
                _id_users = _id_users.Union(_asignaciones.Where(_ => _.idVendedor != null).Select(_ => _.idVendedor.Value).Distinct().ToList()).ToList();
                List<CLIENTES> _users = da.getUserByList(_id_users).ToList();
                List<campus_AUX> _origins = da.getAuxiliars("ORIGEN");
                List<campus_DATA_COMERCIAL> _pays = da.getDataComercialByIdUser(_id_users);

                /// 2.- Filtrar si es tipo venta
                if (_type == "V")
                    _asignaciones = _asignaciones.Where(_ => _.Fecha_Matricula >= DateTime.Parse(_date_start) && _.Fecha_Matricula < DateTime.Parse(_date_end).AddDays(1)).ToList();

                /// 3.- pintar la tabla
                list.Add(paint_subtable(_asignaciones, _users, _docencias, _origins, _pays));
            }

            return list;
        }
        
        private static string paint_subtable(List<campus_ASIG_COMERCIAL> _asignaciones, List<CLIENTES> _users, List<campus_DOCENCIA> _docencias, List<campus_AUX> _origins, List<campus_DATA_COMERCIAL> _pays)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Matriculas_level2\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            //sbuild.Append("<th></th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Nacionalidad</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>F. Lead</th>");
            sbuild.Append("<th>F. Mat.</th>");
            sbuild.Append("<th>F. Pago</th>");
            sbuild.Append("<th>LTV</th>");
            sbuild.Append("<th>€ Total</th>");
            sbuild.Append("<th>€ Pte</th>");
            sbuild.Append("<th>€ Prog.</th>");
            sbuild.Append("<th>€ Fund.</th>");
            sbuild.Append("<th>€ Univ.</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer las matriculas
            foreach (var _matricula in _asignaciones)
            {
                if (_matricula.Fecha_Matricula != null && _matricula.Fecha_Env_Contrato != null)
                {
                    if (_matricula.Fecha_Matricula.Value.AddDays(15) < DateTime.Today && _matricula.Fecha_Env_Contrato.Value.AddDays(15) < DateTime.Today)
                        sbuild.Append("<tr class='text-color-green'><td><string class='hidden'>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-green' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                    else if (_matricula.Fecha_Matricula.Value.AddDays(15) >= DateTime.Today && _matricula.Fecha_Env_Contrato.Value.AddDays(15) < DateTime.Today)
                        sbuild.Append("<tr class='text-color-orange'><td><string class='hidden'>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-orange' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                    else
                        sbuild.Append("<tr class='text-color-red'><td><string class='hidden'>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-red' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                }
                else
                    sbuild.Append("<tr class='text-color-red'><td><string class='hidden'>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-red' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");

                //sbuild.Append("<td><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                //sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")" + "</td>");
                sbuild.Append("<td>" + (_users.Where(_ => _.id_cliente == _matricula.idAlumno && _.online != null && _.online.Value).Count() == 1 ? "ONLINE" : "SEMIP") + "</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nacionalidad).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + _origins.Where(_ => _.ID_Aux == _matricula.idOrigen).Select(_ => _.Nombre).FirstOrDefault() + " (" + _matricula.idOrigen + ")" + "</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idVendedor).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idVendedor + ")" + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Lead != null ? _matricula.Fecha_Lead.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Env_Contrato != null ? _matricula.Fecha_Env_Contrato.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Matricula != null ? _matricula.Fecha_Matricula.Value.ToShortDateString() : string.Empty) + "</td>");
                if (_matricula.Fecha_Matricula != null && _matricula.Fecha_Lead != null)
                    sbuild.Append("<td>" + _matricula.Fecha_Matricula.Value.Subtract(_matricula.Fecha_Lead.Value).Days + "</td>");
                else
                    sbuild.Append("<td></td>");

                decimal _pvp = _matricula.EUR_PVP_Becado != null ? _matricula.EUR_PVP_Becado.Value : 0;
                decimal _fundacion = _matricula.EUR_Aportacion_Fundacion != null ? _matricula.EUR_Aportacion_Fundacion.Value : 0;
                decimal _universidad = _matricula.EUR_Universidad != null ? _matricula.EUR_Universidad.Value : 0;
                decimal _real = _pays.Where(_ => _.idAlumno == _matricula.idAlumno && _.idDocencia == _matricula.idDocencia && _.EUR_real != null).Sum(_ => _.EUR_real).Value;
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
        }
        private static string paint_subtable_comercial(List<campus_ASIG_COMERCIAL> _asignaciones, List<CLIENTES> _users, List<campus_DOCENCIA> _docencias, List<campus_AUX> _origins, List<campus_DATA_COMERCIAL> _pays)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Matriculas_level2\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            //sbuild.Append("<th></th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Nacionalidad</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>F. Lead</th>");
            sbuild.Append("<th>F. Mat.</th>");
            sbuild.Append("<th>F. Pago</th>");
            sbuild.Append("<th>LTV</th>");
            sbuild.Append("<th>€ Total</th>");
            sbuild.Append("<th>€ Pte</th>");
            sbuild.Append("<th>€ Prog.</th>");
            sbuild.Append("<th>€ Fund.</th>");
            sbuild.Append("<th>€ Univ.</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer las matriculas
            foreach (var _matricula in _asignaciones)
            {
                if (_matricula.Fecha_Matricula != null && _matricula.Fecha_Env_Contrato != null)
                {
                    if (_matricula.Fecha_Matricula.Value.AddDays(15) < DateTime.Today && _matricula.Fecha_Env_Contrato.Value.AddDays(15) < DateTime.Today)
                        sbuild.Append("<tr class='text-color-green'><td><string class='hidden'>"+ _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-green' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                    else if (_matricula.Fecha_Matricula.Value.AddDays(15) >= DateTime.Today && _matricula.Fecha_Env_Contrato.Value.AddDays(15) < DateTime.Today)
                        sbuild.Append("<tr class='text-color-orange'><td><string class='hidden'>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-orange' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                    else
                        sbuild.Append("<tr class='text-color-red'><td><string class='hidden'>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-red' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                }
                else
                    sbuild.Append("<tr class='text-color-red'><td><string class='hidden'>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + "</string><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' class='text-color-red' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");

                //sbuild.Append("<td><a href='ficha-alumno-crm.aspx?idu=" + _matricula.idAlumno + "' title='Ficha Alumno' runat='server' target='_blank'><i class='fas fa-user'></i> " + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")</a></td>");
                //sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idAlumno + ")" + "</td>");
                sbuild.Append("<td>" + (_users.Where(_ => _.id_cliente == _matricula.idAlumno && _.online != null && _.online.Value).Count() == 1 ? "ONLINE" : "SEMIP") + "</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idAlumno).Select(_ => _.Nacionalidad).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + _origins.Where(_ => _.ID_Aux == _matricula.idOrigen).Select(_ => _.Nombre).FirstOrDefault() + " (" + _matricula.idOrigen + ")" + "</td>");
                sbuild.Append("<td>" + _docencias.Where(_ => _.ID_Docencia == _matricula.idDocencia).Select(_ => _.Nombre).FirstOrDefault() + " (" + _matricula.idDocencia + ")" + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Lead != null ? _matricula.Fecha_Lead.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Env_Contrato != null ? _matricula.Fecha_Env_Contrato.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Matricula != null ? _matricula.Fecha_Matricula.Value.ToShortDateString() : string.Empty) + "</td>");
                if (_matricula.Fecha_Matricula != null && _matricula.Fecha_Lead != null)
                    sbuild.Append("<td>" + _matricula.Fecha_Matricula.Value.Subtract(_matricula.Fecha_Lead.Value).Days + "</td>");
                else
                    sbuild.Append("<td></td>");

                decimal _pvp = _matricula.EUR_PVP_Becado != null ? _matricula.EUR_PVP_Becado.Value : 0;
                decimal _fundacion = _matricula.EUR_Aportacion_Fundacion != null ? _matricula.EUR_Aportacion_Fundacion.Value : 0;
                decimal _universidad = _matricula.EUR_Universidad != null ? _matricula.EUR_Universidad.Value : 0;
                decimal _real = _pays.Where(_ => _.idAlumno == _matricula.idAlumno && _.EUR_real != null).Sum(_ => _.EUR_real).Value;
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
        }
    }
}