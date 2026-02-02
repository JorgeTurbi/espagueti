using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_tipo_recursos_directo : System.Web.UI.Page
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
                    long idRecDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
                    if (idRecDirecto > 0)
                    {
                        /// 0.- Poner el botón volver
                        btn_back.HRef = "recurso-directo.aspx?idrd=" + idRecDirecto + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty);
                        
                        /// 1.- Pintar el título
                        txt_recursos.InnerHtml = "<i class='fas fa-photo-video'></i> Listado de recursos <a href='recurso-mantenimiento.aspx?idrd=" + idRecDirecto + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty) + "' title='Añadir recurso' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Añadir recurso</small></a>";

                        /// 2.- Cargar los programas
                        cargar_recursos(idRecDirecto);
                    }
                    else
                        Response.Redirect("lista-recursos-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "?lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
                }
            }
        }

        protected void btn_next_Click(object sender, EventArgs e)
        {
            long idResultado = 1;
            long idRecDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            if (idRecDirecto > 0)
            {                
                /// 1.- Sacar los recursos del recurso en directo
                List<campus_RD_R> _recursos_directos = da.getRecDirectosByRecurso(idRecDirecto);
                List<long> _ids_recursos = _recursos_directos.Select(_ => _.ID_Recurso).Distinct().ToList();
                List<campus_RECURSO> _recursos = new List<campus_RECURSO>();
                if (_ids_recursos.Count > 0)
                    _recursos = da.getResourcesByList(_ids_recursos);

                /// 2.- Recorrer los recursos
                if (_recursos.Count > 0)
                {
                    long _id_curso_directo = long.Parse(ConfigurationManager.AppSettings["curso_directo"]);
                    long _id_docencia_directo = long.Parse(ConfigurationManager.AppSettings["docencia_directo"]);

                    /// 3.- Sacar los contenidos del curso y los datos del curso
                    List<campus_CURSO> _curso = da.getCourseById(_id_curso_directo);
                    List<campus_CONTENIDO_CURSO> _contents_course = da.getContentCourse(_id_curso_directo);
                    List<campus_CONTENIDO_DOCENCIA> _contents_docencia = da.getContentDocencia(_id_docencia_directo, _id_curso_directo, null);

                    /// 3.1.- Sacar los recursos
                    List<long> _id_cont_recursos = _contents_course.Select(_ => _.ID_Recurso).Distinct().ToList();

                    /// 3.2.- Filtrar los contenidos a añadir
                    List<campus_RECURSO> _recursos_filter = _recursos.Where(_ => !_id_cont_recursos.Contains(_.ID_Recurso)).ToList();
                    if (_recursos_filter.Count > 0)
                    {
                        /// 3.3.- Añadir en campus_CONTENIDO_CURSO los recursos nuevos
                        foreach (var _recurso in _recursos_filter)
                        {
                            campus_CONTENIDO_CURSO _contenido_curso = new campus_CONTENIDO_CURSO();
                            _contenido_curso.ID_Curso = _id_curso_directo;
                            _contenido_curso.ID_Recurso = _recurso.ID_Recurso;
                            if (_curso.Count == 1)
                                _contenido_curso.COD_Curso = _curso[0].COD_Curso;
                            _contenido_curso.COD_Recurso = _recurso.COD_Recurso;
                            _contenido_curso.Sesion = 1;
                            _contenido_curso.Lectura = 0;
                            _contenido_curso.version = _recurso.Version;
                            _contenido_curso.activo = _recurso.Activo == ((int)Constantes.activo.Activo).ToString() ? true : false;

                            idResultado = da.insertContentCurso(_contenido_curso);
                            if (idResultado < 0)
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el contenido del recurso");
                        }
                    }

                    /// 3.4.- Actualizar los activos o no
                    List<campus_RECURSO> _recursos_filter_act = _recursos.Where(_ => _id_cont_recursos.Contains(_.ID_Recurso)).ToList();
                    if (_recursos_filter_act.Count > 0)
                    {
                        foreach (var _recurso in _recursos_filter_act)
                        {
                            List<campus_CONTENIDO_CURSO> _cont_curso = _contents_course.Where(_ => _.ID_Recurso == _recurso.ID_Recurso).ToList();
                            if (_cont_curso.Count == 1)
                            {
                                bool _activo = _recurso.Activo == ((int)Constantes.activo.Activo).ToString() ? true : false;
                                if (_activo != _cont_curso[0].activo)
                                {
                                    campus_CONTENIDO_CURSO _contenido = _cont_curso[0];
                                    _contenido.activo = _activo;

                                    bool _update = da.updateContentCurso(_contenido);
                                    if (!_update)
                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el contenido del curso");
                                }
                            }
                        }
                    }

                    /// 3.5.- Publicar en la docencia
                    _contents_course = da.getContentCourse(_id_curso_directo);
                    foreach (var _contenido in _contents_course)
                    {
                        /// 3.6.- Comprobar si está añadido
                        List<campus_CONTENIDO_DOCENCIA> _cont_docencia = _contents_docencia.Where(_ => _.ID_Recurso == _contenido.ID_Recurso).ToList();
                        if (_cont_docencia.Count == 0)
                        {
                            campus_CONTENIDO_DOCENCIA _content = new campus_CONTENIDO_DOCENCIA();
                            _content.ID_Docencia = _id_docencia_directo;
                            _content.ID_Curso = _id_curso_directo;
                            _content.ID_Recurso = _contenido.ID_Recurso;
                            _content.COD_Curso = _contenido.COD_Curso;
                            _content.COD_Recurso = _contenido.COD_Recurso;
                            _content.Sesion = _contenido.Sesion;
                            _content.Lectura = _contenido.Lectura;
                            _content.Fecha = DateTime.Now;
                            _content.Visible = (_contenido.activo != null && _contenido.activo.Value) ? 1 : 0;

                            idResultado = da.insertContentDocencia(_content);
                            if (idResultado < 0)
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el contenido a campus_CONTENIDO_DOCENCIA");
                        }
                        else if (_cont_docencia.Count == 1 && _cont_docencia[0].Visible != ((_contenido.activo != null && _contenido.activo.Value) ? 1 : 0))
                        {
                            campus_CONTENIDO_DOCENCIA _content = _cont_docencia[0];
                            _content.Visible = (_contenido.activo != null && _contenido.activo.Value) ? 1 : 0;

                            bool _update = da.updateContentDocencia(_content);
                            if (!_update)
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el contenido a campus_CONTENIDO_DOCENCIA");
                        }
                    }

                    /// 4.- Actualizar el recurso en directo y ponerlo visible
                    List<campus_RECURSO_DIRECTO> lst_rec = da.getRecursoDirectoByIdRD(idRecDirecto);
                    if (lst_rec.Count == 1 && !lst_rec[0].visible)
                    {
                        /// 4.1.- Actualizar los datos del recurso directo
                        campus_RECURSO_DIRECTO _rec_directo = lst_rec[0];
                        _rec_directo.visible = true;

                        bool _update = da.updateRecursoDirecto(_rec_directo);
                        if (!_update)
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al actualizar el recurso en directo en campus_RECURSO_DIRECTO");
                    }

                    if (idResultado > 0)
                        Response.Redirect("lista-recursos-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "?lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
                }
                else
                    Response.Redirect("lista-recursos-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "?lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
            }
        }

        protected void btnActivarRecurso_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Recuperar la regla
            long _id_recurso = !String.IsNullOrEmpty(hidIdRecurso.Value) ? long.Parse(hidIdRecurso.Value) : -1;
            if (_id_recurso > 0)
            {
                List<campus_RECURSO> _recursos = da.getResourcesById(_id_recurso);
                if (_recursos.Count == 1)
                {
                    campus_RECURSO _recurso = _recursos[0];
                    if (_recurso.Activo == ((int)Constantes.activo.Activo).ToString())
                        _recurso.Activo = ((int)Constantes.activo.NoActivo).ToString();
                    else
                        _recurso.Activo = ((int)Constantes.activo.Activo).ToString();

                    bool _update = da.updateRecurso(_recurso);
                    if (_update)
                    {
                        /// 2.0.- Sacar el idRecurso directo
                        long idRecDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;

                        /*                        
                        if (idRecDirecto > 0)
                        {
                            /// 2.1.- Comprobar si hay que añadir la entrada a campus_RD_R
                            List<campus_RD_R> _recursos_rd = da.getRecDirectosByRecurso(idRecDirecto);

                            /// 2.2.- Filtrar por el recurso
                            _recursos_rd = _recursos_rd.Where(_ => _.ID_Recurso == _id_recurso).ToList();

                            /// 2.3.- Eliminar o añadir entrada en campus_RD_R
                            if (_recursos_rd.Count == 1 && _recurso.Activo == ((int)Constantes.activo.NoActivo).ToString())
                            {
                                /// 2.3.1.- Eliminar
                                bool _delete_rd = da.deleteRD_R(_recursos_rd[0].id_RecursoDirecto);
                                if (!_delete_rd)
                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al eliminar el recurso directo " + _recursos_rd[0].id_RecursoDirecto + " de campus_RD_R");
                            }
                            else if (_recursos_rd.Count == 0 && _recurso.Activo == ((int)Constantes.activo.Activo).ToString())
                            {
                                /// 2.3.2.- Añadir
                                campus_RD_R _rd = new campus_RD_R();
                                _rd.id_RecursoDirecto = idRecDirecto;
                                _rd.ID_Recurso = _id_recurso;

                                long insert_rd = da.insertRD_R(_rd);
                                if (insert_rd < 1)
                                    LogUtils.InsertarLog("- MSG: Se ha producido un error al añadir el recurso en directo");
                            }
                        }*/

                        /// 3.- Recargar la página
                        Response.Redirect("lista-tipo-recursos-directo.aspx?idrd=" + idRecDirecto + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
                    }
                    else
                        ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar el recurso');</script>");
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
                    List<campus_RECURSO> _recursos = da.getResourcesById(_id_recurso);
                    if (_recursos.Count == 1)
                    {
                        if (!String.IsNullOrEmpty(_recursos[0].Rec_Interno))
                        {
                            string ruta = string.Empty;
                            if (_recursos[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Multimedia)
                                ruta = ConfigurationManager.AppSettings["route_multimedia"];
                            else
                                ruta = ConfigurationManager.AppSettings["route_nota_tecnica"];

                            /// 1.1.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta + _recursos[0].Rec_Interno);
                        }

                        /// 2.- Eliminar la regla 
                        _delete = da.deleteRecurso(_id_recurso);
                        if (_delete)
                        {
                            /// 3.- Eliminar las entradas del recurso de campus_RD_R
                            List<campus_RD_R> _recursos_rd = da.getRecDirectosByRecursoId(_id_recurso);
                            if (_recursos_rd.Count > 0)
                            {
                                foreach (var _recurso in _recursos_rd)
                                {
                                    bool _delete_rd = da.deleteRD_R(_recurso.id_RecursoDirecto);
                                    if (!_delete_rd)
                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al eliminar el recurso directo " + _recurso.id_RecursoDirecto + " de campus_RD_R");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el recurso');</script>");

                LogUtils.InsertarLog(" ERROR - lista-tipo-recursos-directo.cs::btnBorrarRecurso_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (_delete)
                Response.Redirect("lista-tipo-recursos-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
        }

        private void cargar_recursos(long idRecDirecto)
        {
            /// 1.- Sacar datos de la BBDD
            List<campus_RD_R> _recursos_directos = da.getRecDirectosByRecurso(idRecDirecto);
            List<long> _ids_recursos = _recursos_directos.Select(_ => _.ID_Recurso).Distinct().ToList();
            List<campus_RECURSO> _recursos = new List<campus_RECURSO>();
            if (_ids_recursos.Count > 0)
                _recursos = da.getResourcesByList(_ids_recursos);
            List<sbs2_area_funcional> _areas = da.getAreaFuncionalByIdArea(-1);
            List<sbs2_tematica> _tematicas = da.getTematicaByIdTematica(-1);
            List<campus_TIPO_RECURSO> _tipos = da.getTypeResource();

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Recursos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Cod. Recurso</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Área</th>");
            sbuild.Append("<th>Titulo</th>");           
            sbuild.Append("<th>Versión</th>");
            sbuild.Append("<th>Derechos</th>");
            sbuild.Append("<th>Rec. Interno</th>");
            sbuild.Append("<th>Rec. Externo</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las reglas
            foreach (var _recurso in _recursos)
            {
                sbuild.Append("<tr>");

                sbuild.Append($"<td>{_recurso.COD_Recurso}</td>");
                sbuild.Append($"<td>{_tipos.Where(_ => _.ID_Tipo_Recurso == _recurso.ID_Tipo_Recurso).Select(_ => _.Nombre).FirstOrDefault()}</td>");
                sbuild.Append($"<td>{_areas.Where(_ => _.idArea == _recurso.ID_Area_Funcional).Select(_ => _.nombre).FirstOrDefault() }</td>");
                sbuild.Append($"<td>{_recurso.Titulo}</td>");
                sbuild.Append($"<td>{_recurso.Version}</td>");
                sbuild.Append($"<td>{_recurso.Derechos}</td>");
                if (!String.IsNullOrEmpty(_recurso.Rec_Interno))
                {
                    if (_recurso.ID_Tipo_Recurso == (int)Constantes.type_recurso.Multimedia)
                        sbuild.Append($"<td><a href='{ConfigurationManager.AppSettings["multimedia"] + _recurso.Rec_Interno}' target='_blank'><i class='far fa-file fa-1-6x'></i></a></td>");
                    else
                        sbuild.Append($"<td><a href='{ConfigurationManager.AppSettings["nota_tecnica"] + _recurso.Rec_Interno}' target='_blank'><i class='far fa-file fa-1-6x'></i></a></td>");
                }
                else
                    sbuild.Append("<td></td>");
                if (!String.IsNullOrEmpty(_recurso.Rec_Externo))
                    sbuild.Append($"<td><a href='{_recurso.Rec_Externo}' target='_blank'><i class='fas fa-globe fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td></td>");
                if (_recurso.Activo == ((int)Constantes.activo.Activo).ToString())
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea desactivar el recurso: " + _recurso.ID_Recurso + "?\")){activarRecurso(" + _recurso.ID_Recurso + ")}'><i class=\"fas fa-power-off text-color-green fa-1-6x\" style=\"cursor: pointer\" title=\"Desactivar recurso.\"></i></a></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea activar el recurso: " + _recurso.ID_Recurso + "?\")){activarRecurso(" + _recurso.ID_Recurso + ")}'><i class=\"fas fa-power-off text-color-red fa-1-6x\" style=\"cursor: pointer\" title=\"Activar recurso.\"></i></a></td>");
                sbuild.Append($"<td><a href='recurso-mantenimiento.aspx?idrd={idRecDirecto}&idr={_recurso.ID_Recurso}{(!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty)}' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el recurso " + _recurso.Titulo + "?\")){eliminarRecurso(" + _recurso.ID_Recurso + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_recursos.InnerHtml = sbuild.ToString();
        }
    }
}