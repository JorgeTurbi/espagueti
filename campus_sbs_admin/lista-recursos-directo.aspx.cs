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
    public partial class lista_recursos_directo : System.Web.UI.Page
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
                    /// 1.- Pintar el título
                    txt_recursos.InnerHtml = "<i class='fas fa-photo-video'></i> Listado de recursos en directo <a href='recurso-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "?lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty) + "' title='Añadir recurso en directo' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir recurso en directo</small></a>";
                    txt_recursos.InnerHtml += "<a href='lista-recursos-directo.aspx?lrc=2' title='Ver todos' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-eye fa-2x'></i> Todos</small></a>";
                    txt_recursos.InnerHtml += "<a href='lista-recursos-directo.aspx?lrc=1' title='Ver futuro' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-eye fa-2x'></i> Futuro</small></a>";
                    txt_recursos.InnerHtml += "<a href='lista-recursos-directo.aspx' title='Ver' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-eye fa-2x'></i> Ver</small></a>";

                    /// 2.- Cargar los programas
                    cargar_recursos();
                }
            }
        }
        
        protected void btnActivarRecurso_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Recuperar la regla
            long _id_recurso = !String.IsNullOrEmpty(hidIdRecurso.Value) ? long.Parse(hidIdRecurso.Value) : -1;
            if (_id_recurso > 0)
            {
                List<campus_RECURSO_DIRECTO> _recursos = da.getRecursoDirectoByIdRD(_id_recurso);
                if (_recursos.Count == 1)
                {
                    campus_RECURSO_DIRECTO _recurso = _recursos[0];
                    _recurso.visible = !_recurso.visible;

                    bool _update = da.updateRecursoDirecto(_recurso);
                    if (_update)
                        Response.Redirect("lista-recursos-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "?lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
                    else
                        ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar el recurso directo');</script>");
                }
            }
        }

        protected void btnBorrarRecurso_Click(object sender, ImageClickEventArgs e)
        {
            bool _delete = false;

            try
            {
                long _id_recurso = !String.IsNullOrEmpty(hidIdRecurso.Value) ? long.Parse(hidIdRecurso.Value) : -1;
                if (_id_recurso > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_RECURSO_DIRECTO> _recursos = da.getRecursoDirectoByIdRD(_id_recurso);
                    if (_recursos.Count == 1)
                    {
                        /// 2.- Eliminar la regla 
                        _delete = da.deleteRecursoDirecto(_id_recurso);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el recurso directo');</script>");

                LogUtils.InsertarLog(" ERROR - lista-recursos-directo.cs::btnBorrarRecurso_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (_delete)
                Response.Redirect("lista-recursos-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "?lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
        }

        [WebMethod(Description = "Recupera la tabla de usuarios")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable(long idRecursoDirecto, int todos)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            if (todos == 0) /// Asistentes
            {
                /// 1.- Sacar los datos de los asistentes
                List<campus_RD_Asistencia> _rec_asistencia = da.getRDAsistenciaProcesados();
                List<campus_RD_Asistencia> _usuarios_rd = _rec_asistencia.Where(_ => _.id_RecursoDirecto == idRecursoDirecto).ToList();
                List<campus_RD_R> _rds = da.getRecDirectosByRecurso(idRecursoDirecto);
                List<long> _recursos_en_directo = _rds.Select(_ => _.ID_Recurso).ToList();
                List<long> _assists = _usuarios_rd.Select(_u => _u.ID_Persona).Distinct().ToList();
                List<campus_LOG> _logs = da.getLogEntries(_assists);
                _logs = _logs.Where(_ => _.Descripcion.StartsWith("View resource")).ToList();
                List<campus_LOG> _rec_vistos = _logs.Where(_ => _recursos_en_directo.Contains(long.Parse(_.Descripcion.Substring(_.Descripcion.IndexOf('(') + 1).Replace(')', ' ').Trim()))).ToList();

                list.Add(paint_table(_rec_vistos));
            }
            else {

                /// 1.- Sacar los datos de los que han visto el recurso
                List<campus_RD_R> _rds = da.getRecDirectosByRecurso(idRecursoDirecto);
                List<long> _recursos_en_directo = _rds.Select(_ => _.ID_Recurso).ToList();
                /*if (_recursos_en_directo.Count > 0)
                {
                    List<campus_RECURSO> _recursos = da.getResourcesByList(_recursos_en_directo);
                    //_recursos = _recursos.Where(_ => _.ID_Tipo_Recurso == 3).ToList();
                    List<long> _id_Recurso = _recursos.Select(_ => _.ID_Recurso).ToList();

                    _recursos_en_directo = _recursos_en_directo.Where(_ => _id_Recurso.Contains(_)).ToList();
                }*/

                List<campus_LOG> _logs = da.getResourcesViewed(-1, _recursos_en_directo);
                _logs = _logs.Where(_ => _.ID_Persona != null).ToList();
                List<campus_LOG> _rec_vistos = _logs.Where(_ => _recursos_en_directo.Contains(long.Parse(_.Descripcion.Substring(_.Descripcion.IndexOf('(') + 1).Replace(')', ' ').Trim()))).ToList();

                list.Add(paint_table(_rec_vistos));
            }

            return list;
        }
        
        private void cargar_recursos()
        {
            /// 0.- Comprobar que datos sacamos
            int _tipo_listado = !String.IsNullOrEmpty(Request.QueryString["lrc"]) ? int.Parse(Request.QueryString["lrc"].ToString()) : -1;

            /// 1.- Sacar datos de la BBDD
            List<campus_RECURSO_DIRECTO> _recursos = da.getRecursoDirectoByIdRD(-1);
            if (_tipo_listado == -1)
                _recursos = _recursos.Where(_ => _.fecha < DateTime.Today.AddDays(1) && _.fecha >= DateTime.Today.AddDays(-(int.Parse(ConfigurationManager.AppSettings["num_dias_sbslife"])))).ToList();
            else if (_tipo_listado == 1)
                _recursos = _recursos.Where(_ => _.fecha >= DateTime.Today.AddDays(1)).ToList();
            
            List<campus_RD_R> _recursos_directos = da.getRecDirectosByRecurso(-1);
            long _id_curso_directo = long.Parse(ConfigurationManager.AppSettings["curso_directo"]);
            long _id_docencia_directo = long.Parse(ConfigurationManager.AppSettings["docencia_directo"]);
            List<campus_CONTENIDO_DOCENCIA> _contenidos = da.getContentDocencia(_id_docencia_directo, _id_curso_directo, true);
            List<sbs2_area_funcional> _areas = da.getAreaFuncionalByIdArea(-1);
            List<sbs2_tematica> _tematicas = da.getTematicaByIdTematica(-1);
            List<CLIENTES> _profesores = new List<CLIENTES>();
            List<long> _ids = _recursos.Select(_ => _.idProfesor).Distinct().ToList();
            if (_ids.Count > 0)
                _profesores = da.getUserByList(_ids);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Recursos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Titulo</th>");
            sbuild.Append("<th>Profesor</th>");
            sbuild.Append("<th>Área</th>");
            sbuild.Append("<th>Temática</th>");
            sbuild.Append("<th title='Nº Personas que han visto el recurso'>Nº Per</th>");
            sbuild.Append("<th title='Nº Asistentes'>Nº Asis</th>");
            sbuild.Append("<th title='Nº Personas que han valorado'>Nº Val</th>");
            sbuild.Append("<th>Val. Clase</th>");
            sbuild.Append("<th>Val.Profesor</th>");
            sbuild.Append("<th>Publicados</th>");
            sbuild.Append("<th>Visible</th>");
            sbuild.Append("<th title='Url Friendly'>Url</th>");
            sbuild.Append("<th title='Interno'>Int</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 2.3.- Pintar las reglas
            foreach (var _recurso in _recursos)
            {
                sbuild.Append("<tr>");

                sbuild.Append($"<td>{_recurso.fecha.ToShortDateString()}</td>");
                sbuild.Append($"<td>{_recurso.tipo}</td>");
                sbuild.Append($"<td>{_recurso.titulo}</td>");
                sbuild.Append($"<td>{_profesores.Where(_ => _.id_cliente == _recurso.idProfesor).Select(_ => _.Nombre_Completo).FirstOrDefault()}</td>");
                sbuild.Append($"<td>{_areas.Where(_ => _.idArea == _recurso.idArea).Select(_ => _.nombre).FirstOrDefault() }</td>");
                sbuild.Append($"<td>{_tematicas.Where(_ => _.idTematica == _recurso.idTematica).Select(_ => _.nombre).FirstOrDefault() }</td>");
                sbuild.Append($"<td onclick='MostrarListadoAlumnos({_recurso.id_RecursoDirecto},1)'>{_recurso.num_alumnos}</td>");
                sbuild.Append($"<td onclick='MostrarListadoAlumnos({_recurso.id_RecursoDirecto},0)'>{_recurso.num_asistentes}</td>");
                sbuild.Append($"<td>{_recurso.num_valoraciones}</td>");
                sbuild.Append($"<td>{_recurso.val_clase}</td>");
                sbuild.Append($"<td>{_recurso.val_profesor}</td>");

                List<campus_RD_R> _recursos_directos_class = _recursos_directos.Where(_ => _.id_RecursoDirecto == _recurso.id_RecursoDirecto).ToList();
                List<long> _id_rds = _recursos_directos_class.Select(_ => _.ID_Recurso).Distinct().ToList();
                sbuild.Append($"<td>{_contenidos.Where(_ => _id_rds.Contains(_.ID_Recurso)).Distinct().Count()} / {_recursos_directos_class.Count}</td>");
                
                if (_recurso.visible)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea desactivar el recurso directo: " + _recurso.tipo + "?\")){activarRecurso(" + _recurso.id_RecursoDirecto + ")}'><i class=\"fas fa-power-off text-color-green fa-1-6x\" style=\"cursor: pointer\" title=\"Desactivar recurso directo.\"></i></a></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea activar el recurso directo: " + _recurso.tipo + "?\")){activarRecurso(" + _recurso.id_RecursoDirecto + ")}'><i class=\"fas fa-power-off text-color-red fa-1-6x\" style=\"cursor: pointer\" title=\"Activar recurso directo.\"></i></a></td>");

                if (!String.IsNullOrEmpty(_recurso.Meta_Url))
                    sbuild.Append("<td><i class=\"far fa-check-square text-color-primary fa-1-6x\"></i></td>");
                else
                    sbuild.Append("<td></td>");

                if (_recurso.interno)
                    sbuild.Append("<td><i class=\"far fa-check-square text-color-primary fa-1-6x\"></i></td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append($"<td><a href='recurso-directo.aspx?idrd={_recurso.id_RecursoDirecto}{(!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty)}' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                if (_recursos_directos.Where(_ => _.id_RecursoDirecto == _recurso.id_RecursoDirecto).Count() > 0)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el recurso directo " + _recurso.tipo + "?\")){eliminarRecurso(" + _recurso.id_RecursoDirecto + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                else
                    sbuild.Append("<td></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_recursos.InnerHtml = sbuild.ToString();
        }

        private static string paint_table(List<campus_LOG> _rec_vistos)
        {
            DataAccess da = new DataAccess();
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los datos de los usuarios
            List<long> _users = _rec_vistos.Select(_ => _.ID_Persona.Value).Distinct().ToList();
            List<CLIENTES> _clients = da.getUserByList(_users);
            
            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Usuarios\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>ID</th>");
            sbuild.Append("<th>NOMBRE</th>");
            sbuild.Append("<th>FECHA</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Pintar los usuarios
            foreach (var _recurso in _rec_vistos)
            {
                List<CLIENTES> _client = _clients.Where(_ => _.id_cliente == _recurso.ID_Persona).ToList();

                if (_client[0].activo == "1")
                    sbuild.Append("<tr>");
                else
                    sbuild.Append("<tr class='red'>");

                sbuild.Append("<td>" + _client[0].id_cliente + "</td>");
                sbuild.Append("<td>" + _client[0].Nombre_Completo + "</td>");
                sbuild.Append("<td>" + _recurso.Fecha_Hora + "</td>");

                sbuild.Append("</tr>");
            }
            
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
        
    }
}