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
    public partial class automatizar_curso : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el parámetro 'k' en la url
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                {
                    Session.Add("usuario", list_user[0]);
                }
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
                    /// 1.- Sacar los datos del tipo de automatización
                    long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
                    if (idCurso > 0)
                        load_auto(idCurso, list_user[0]);
                    else
                        Response.Redirect("login.aspx");
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            bool delete_task = false;

            try
            {
                long idTask = !String.IsNullOrEmpty(hidTask.Value) ? long.Parse(hidTask.Value) : -1;
                if (idTask > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idTask);
                    if (lst_tasks.Count == 1)
                    {
                        /// 3.- Eliminar el contacto
                        delete_task = da.deleteTaskAutoCourse(idTask);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - contactos.cs::btnEliminar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                delete_task = false;
            }

            /// Si no hay errores recargar la página
            if (delete_task)
                Response.Redirect("automatizar-curso.aspx" + (!String.IsNullOrEmpty(Request.QueryString["idc"]) ? "?idc=" + Request.QueryString["idc"] : string.Empty));
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la tarea');</script>");
        }

        protected void btnGuardarContenido_Click(object sender, EventArgs e)
        {
            long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
            if (idCurso > 0)
            {
                bool _correct = true;

                /// 1.- Sacar los datos de la BBDD
                List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourseByCourse(idCurso);
                List<campus_CONTENIDO_CURSO> list_contents = da.getContentCourse(idCurso);
                List<campus_RECURSO> list_resources = da.getResources().Where(r => r.Activo == "1").ToList();
                List<long> lst_id = list_resources.Select(r => r.ID_Recurso).ToList();

                /// 2.- Filtrar los contenidos
                list_contents = list_contents.Where(cc => lst_id.Contains(cc.ID_Recurso)).ToList();
                if (list_contents.Count > 0)
                {
                    /// 3.- Recorrer los contenidos
                    foreach (var content in list_contents)
                    {
                        /// 3.1.- Sacar el valor del elemento del formulario
                        string id_resource = "chk_" + content.ID_Recurso;
                        bool _check = false;
                        if (!String.IsNullOrEmpty(Request.Form[id_resource]))
                            _check = Request.Form[id_resource].Equals("on");

                        int num_dias = -1;
                        string id_days = "txt_" + content.ID_Recurso;
                        if (!String.IsNullOrEmpty(Request.Form[id_days]))
                            num_dias = int.Parse(Request.Form[id_days]);

                        /// 3.2.- Comprobar si hay que actualizar o añadir
                        if (_check)
                        {
                            List<campus_AUTO_CURSO> lst_task = lst_tasks.Where(ac => ac.Id_Recurso != null && ac.Id_Recurso == content.ID_Recurso).ToList();
                            if (lst_task.Count == 0)
                            {
                                campus_AUTO_CURSO task = new campus_AUTO_CURSO();
                                task.ID_Curso = idCurso;
                                task.Tipo = (int)Constantes.type_auto_curso.type_contenido;
                                task.Num_Dias = num_dias;
                                task.Id_Recurso = content.ID_Recurso;
                                task.Asunto = list_resources.Where(r => r.ID_Recurso == content.ID_Recurso).Select(r => r.Titulo).FirstOrDefault();
                                
                                long insert_task = da.insertTaskAutoCourse(task);
                                if (insert_task < 1)
                                    _correct = false;
                            }
                            else if (lst_task.Count == 1)
                            {
                                if (lst_task[0].Num_Dias != num_dias)
                                {
                                    campus_AUTO_CURSO task = lst_task[0];
                                    task.Num_Dias = num_dias;
                                    task.Asunto = list_resources.Where(r => r.ID_Recurso == content.ID_Recurso).Select(r => r.Titulo).FirstOrDefault();

                                    bool update_task = da.updateTaskAutoCourse(task);
                                    if (!update_task)
                                        _correct = false;
                                }
                            }
                        }
                    }
                }

                if (_correct)
                    Response.Redirect("automatizar-curso.aspx?idc=" + idCurso);
                else
                {
                    LogUtils.InsertarLog("Se ha producido un error al actualizar los datos");
                    Response.Redirect("automatizar-curso.aspx?idc=" + idCurso);
                }
            }
        }

        /*protected void btnGuardarForo_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos del formulario
            string titulo = txt_titulo_foro.Value;
            string descripcion = txt_descripcion_foro.Value;
            int num_dias = int.Parse(txt_dias_foro.Value);

            long id_curso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            if (id_curso > 0)
            {
                long idTask = !String.IsNullOrEmpty(hidIdAutoforo.Value) ? long.Parse(hidIdAutoforo.Value) : -1;
                if (idTask > 0)
                {
                    List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idTask);
                    if (lst_tasks.Count == 1)
                    {
                        campus_AUTO_CURSO task = lst_tasks[0];
                        task.Asunto = titulo;
                        task.Descripcion = descripcion;
                        task.Num_Dias = num_dias;

                        bool update_task = da.updateTaskAutoCourse(task);
                        if (update_task)
                            Response.Redirect("automatizar-curso.aspx?idc=" + id_curso);
                        else
                        {
                            /// Mostrar el bloque

                            txt_error_foro.InnerHtml = "Se ha producido un error al actualizar el foro";
                            txt_error_foro.Focus();
                        }
                    }
                }
                else
                {
                    campus_AUTO_CURSO task = new campus_AUTO_CURSO();
                    task.ID_Curso = id_curso;
                    task.Tipo = (int)Constantes.type_auto_curso.type_foro;
                    task.Asunto = titulo;
                    task.Descripcion = descripcion;
                    task.Num_Dias = num_dias;
                    task.Procesado = false;

                    long insert_task = da.insertTaskAutoCourse(task);
                    if (insert_task > 0)
                        Response.Redirect("automatizar-curso.aspx?idc=" + id_curso);
                    else
                    {
                        /// Mostrar el bloque
                        txt_error_foro.InnerHtml = "Se ha producido un error al guardar el foro";
                        txt_error_foro.Focus();                            
                    }
                }
            }
        }*/

        private void load_auto(long idCurso, CLIENTES user)
        {
            /// 0.- Sacar datos del curso
            List<campus_CURSO> lst_courses = da.getCourseById(idCurso);
            List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourseByCourse(idCurso);

            /// 1.- Pintar la tabla
            table_listado_tasks.InnerHtml = paint_table(lst_tasks);

            /// 2.- Pintar el título
            txt_title.InnerHtml = "<i class='fas fa-tools'></i> Automatización de tareas del curso " + lst_courses[0].Nombre + " (" + idCurso + ") <a href='automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=5' title='Nuevo Caso Práctico' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> CP</small></a><a href='#block_content' title='Nuevo contenido' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Contenido</small></a><a href='automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=3' title='Nuevo mensaje' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Mensaje</small></a><a href='automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=2' title='Nuevo Mensaje foro' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Mensaje foro</small></a><a href='automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=" + (int)Constantes.type_auto_curso.type_foro + "' title='Nuevo Foro' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i> Foro</small></a>";

            /// 3.- Poner el valor por defecto del nº de días
            //txt_dias_foro.Value = "0";
            /// 3.- Poner el enlace de nuevo foro
            lnk_foro.HRef = "automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=" + (int)Constantes.type_auto_curso.type_foro;

            /// 4.- Pintar el listado del foro
            List<campus_AUTO_CURSO> lst_tasks_foro = lst_tasks.Where(ac => ac.Tipo == (int)Constantes.type_auto_curso.type_foro).ToList();
            table_list_foros.InnerHtml = paint_table_foro(lst_tasks_foro);

            /// 5.- Poner el enlace de nuevo mensaje del foro
            link_msg_foro.HRef = "automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=" + (int)Constantes.type_auto_curso.type_msg_foro;

            /// 6.- Pintar el listado de mensaje en el foro
            List<campus_AUTO_CURSO> lst_tasks_msg_foro = lst_tasks.Where(ac => ac.Tipo == (int)Constantes.type_auto_curso.type_msg_foro).ToList();
            table_list_msg_foro.InnerHtml = paint_table_msg_foro(lst_tasks_foro, lst_tasks_msg_foro);

            /// 7.- Poner el enlace de nuevo mensaje del foro
            link_msg.HRef = "automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=" + (int)Constantes.type_auto_curso.type_msg;

            /// 8.- Pintar el listado de mensaje en el foro
            List<campus_AUTO_CURSO> lst_tasks_msg = lst_tasks.Where(ac => ac.Tipo == (int)Constantes.type_auto_curso.type_msg).ToList();
            table_list_msg.InnerHtml = paint_table_msg(lst_tasks_msg);

            /// 9.- Poner el enlace de nuevo mensaje del foro
            link_cp.HRef = "automatizar-curso-mantenimiento.aspx?idc=" + idCurso + "&idt=" + (int)Constantes.type_auto_curso.type_cp;

            /// 8.- Pintar el listado de mensaje en el foro
            List<campus_AUTO_CURSO> lst_tasks_cp = lst_tasks.Where(ac => ac.Tipo == (int)Constantes.type_auto_curso.type_cp).ToList();
            table_list_cp.InnerHtml = paint_table_cp(lst_tasks_cp);

            /// 9.- Pintar los contenidos
            List<campus_AUTO_CURSO> lst_tasks_content = lst_tasks.Where(ac => ac.Tipo == (int)Constantes.type_auto_curso.type_contenido).ToList();
            block_contents.InnerHtml = paint_contents(idCurso, lst_tasks_content);
        }

        private string paint_table(List<campus_AUTO_CURSO> lst_tasks)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Tareas_Auto\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nº Días</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Asunto</th>");
            sbuild.Append("<th>Descripción</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var task in lst_tasks)
            {
                sbuild.Append("<tr>");

                if (task.Tipo == (int)Constantes.type_auto_curso.type_msg && task.Num_Dias == -1)
                    sbuild.Append("<td>N</td>");
                else
                    sbuild.Append("<td><span class='hidden'>" + task.Num_Dias.ToString("000") + "</span>" + task.Num_Dias + "</td>");

                if (task.Tipo == (int)Constantes.type_auto_curso.type_foro)
                    sbuild.Append("<td>FORO</td>");
                else if (task.Tipo == (int)Constantes.type_auto_curso.type_msg_foro)
                    sbuild.Append("<td>MSG FORO</td>");
                else if (task.Tipo == (int)Constantes.type_auto_curso.type_msg)
                    sbuild.Append("<td>MENSAJE</td>");
                else if (task.Tipo == (int)Constantes.type_auto_curso.type_contenido)
                    sbuild.Append("<td>CONTENIDO</td>");
                else if (task.Tipo == (int)Constantes.type_auto_curso.type_cp)
                    sbuild.Append("<td>CASO PRACTICO</td>");

                sbuild.Append("<td>" + task.Asunto + "</td>");
                sbuild.Append("<td>" + (!String.IsNullOrEmpty(task.Descripcion) ? Utilities.recortarCadena(task.Descripcion, 60) : string.Empty) + "</td>");

                if (task.Tipo == (int)Constantes.type_auto_curso.type_foro || task.Tipo == (int)Constantes.type_auto_curso.type_msg_foro || task.Tipo == (int)Constantes.type_auto_curso.type_msg || task.Tipo == (int)Constantes.type_auto_curso.type_cp)
                    sbuild.Append("<td><a href='automatizar-curso-mantenimiento.aspx?idc=" + task.ID_Curso + "&idt=" + task.Tipo + "&idau=" + task.Id_Automatizacion + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td><a href='#block_content' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la tarea de automatización?\")){eliminarAccion(" + task.Id_Automatizacion + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
        private string paint_table_foro(List<campus_AUTO_CURSO> lst_tasks)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Tareas_Auto_Foro\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nº Días</th>");
            sbuild.Append("<th>Título</th>");
            sbuild.Append("<th>Descripción</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var task in lst_tasks)
            {
                sbuild.Append("<tr>");

                sbuild.Append("<td>" + task.Num_Dias + " </td>");
                sbuild.Append("<td>" + task.Asunto + "</td>");
                sbuild.Append("<td>" + Utilities.recortarCadena(task.Descripcion, 80) + "</td>");
                sbuild.Append("<td><a href='automatizar-curso-mantenimiento.aspx?idc=" + task.ID_Curso + "&idt=" + (int)Constantes.type_auto_curso.type_foro + "&idau=" + task.Id_Automatizacion + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la tarea de automatización?\")){eliminarAccion(" + task.Id_Automatizacion + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
        private string paint_table_msg_foro(List<campus_AUTO_CURSO> lst_tasks_foro, List<campus_AUTO_CURSO> lst_tasks)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Tareas_Auto_MSG_Foro\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nº Días</th>");
            sbuild.Append("<th>Foro</th>");
            sbuild.Append("<th>Asunto</th>");
            sbuild.Append("<th>Cuerpo</th>");
            sbuild.Append("<th>Adjuntos</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var task in lst_tasks)
            {
                sbuild.Append("<tr>");

                sbuild.Append("<td>" + task.Num_Dias + "</td>");
                sbuild.Append("<td>" + lst_tasks_foro.Where(f => f.Id_Automatizacion == task.Id_Foro).Select(f => f.Asunto).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + task.Asunto + "</td>");
                sbuild.Append("<td>" + Utilities.recortarCadena(task.Descripcion, 80) + "</td>");
                if (!String.IsNullOrEmpty(task.Adjuntos))
                    sbuild.Append("<td><i class='fas fa-copy text-color-primary fa-1-6x'></i></td>");
                else
                    sbuild.Append("<td></td>");
                sbuild.Append("<td><a href='automatizar-curso-mantenimiento.aspx?idc=" + task.ID_Curso + "&idt=" + (int)Constantes.type_auto_curso.type_msg_foro + "&idau=" + task.Id_Automatizacion + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la tarea de automatización?\")){eliminarAccion(" + task.Id_Automatizacion + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
        private string paint_table_msg(List<campus_AUTO_CURSO> lst_tasks)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Tareas_Auto_MSG\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nº Días</th>");
            sbuild.Append("<th>Asunto</th>");
            sbuild.Append("<th>Cuerpo</th>");
            sbuild.Append("<th>Adjuntos</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var task in lst_tasks)
            {
                sbuild.Append("<tr>");

                if (task.Num_Dias == -1)
                    sbuild.Append("<td>N</td>");
                else
                    sbuild.Append("<td><span class='hidden'>" + task.Num_Dias.ToString("000") + "</span>" + task.Num_Dias + "</td>");

                sbuild.Append("<td>" + task.Asunto + "</td>");
                sbuild.Append("<td>" + Utilities.recortarCadena(task.Descripcion, 80) + "</td>");
                if (!String.IsNullOrEmpty(task.Adjuntos))
                    sbuild.Append("<td><i class='fas fa-copy text-color-primary fa-1-6x'></i></td>");
                else
                    sbuild.Append("<td></td>");
                sbuild.Append("<td><a href='automatizar-curso-mantenimiento.aspx?idc=" + task.ID_Curso + "&idt=" + (int)Constantes.type_auto_curso.type_msg + "&idau=" + task.Id_Automatizacion + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la tarea de automatización?\")){eliminarAccion(" + task.Id_Automatizacion + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
        private string paint_table_cp(List<campus_AUTO_CURSO> lst_tasks)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Tareas_Auto_CP\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nº Días</th>");
            sbuild.Append("<th>Caso</th>");
            sbuild.Append("<th>Nº Días FL</th>");
            sbuild.Append("<th>Comentario</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Pintar los listados de tipos de automatización
            foreach (var task in lst_tasks)
            {
                sbuild.Append("<tr>");

                sbuild.Append("<td>" + task.Num_Dias + "</td>");
                if (!String.IsNullOrEmpty(task.Adjuntos))
                    sbuild.Append("<td><a href='" + ConfigurationManager.AppSettings["url_automatizacion"] + task.ID_Curso + "/" + task.Asunto + "' target='_blank'><i class='fas fa-file-alt text-color-primary fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td></td>");

                if (task.Num_Dias_Fecha_Limite == -1)
                    sbuild.Append("<td>N</td>");
                else
                    sbuild.Append("<td><span class='hidden'>" + task.Num_Dias_Fecha_Limite.Value.ToString("000") + "</span>" + task.Num_Dias_Fecha_Limite + "</td>");

                //sbuild.Append("<td>" + task.Fecha_limite.Value.ToShortDateString() + "</td>");
                sbuild.Append("<td>" + Utilities.recortarCadena(task.Descripcion, 80) + "</td>");
                sbuild.Append("<td><a href='automatizar-curso-mantenimiento.aspx?idc=" + task.ID_Curso + "&idt=" + (int)Constantes.type_auto_curso.type_cp + "&idau=" + task.Id_Automatizacion + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la tarea de automatización?\")){eliminarAccion(" + task.Id_Automatizacion + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }

        private string paint_contents(long idCurso, List<campus_AUTO_CURSO> lst_tasks_content)
        {
            string block = string.Empty;

            /// 1.- Sacar los contenidos del curso de la BBDD
            List<campus_CONTENIDO_CURSO> list_contents = da.getContentCourse(idCurso);
            List<campus_RECURSO> list_resources = da.getResources().Where(r => r.Activo == "1").ToList();
            List<long> lst_id = list_resources.Select(r => r.ID_Recurso).ToList();

            list_contents = list_contents.Where(cc => lst_id.Contains(cc.ID_Recurso)).ToList();
            if (list_contents.Count > 0)
            {
                List<campus_LITERALES> list_literals = da.getLiterals();
                block = paintContents(lst_tasks_content, list_contents, list_literals, list_resources, idCurso);
            }

            return block;
        }
        private string paintContents(List<campus_AUTO_CURSO> lst_tasks_content, List<campus_CONTENIDO_CURSO> list_contents, List<campus_LITERALES> list_literals, List<campus_RECURSO> list_resources, long idCurso)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Sacar las sesiones
            List<long> list_sesions = list_contents.Select(c => (long)c.Sesion).OrderBy(s => s).Distinct().ToList();
            foreach (var sesion in list_sesions)
            {
                sbuild.Append("<legend class='text-color-black'><i class='fa fa-bookmark text-color-secondary'></i> Sesión " + sesion + ": " + list_literals.Where(l => l.ID_Padre == idCurso && l.Valor1 == sesion).Select(l => l.Descripcion).FirstOrDefault() + "</legend>");
                sbuild.Append("<div class='col-sm-12'>");

                /// 2.- Sacar los contenidos por sesion
                List<campus_CONTENIDO_CURSO> list_contents_sesion = list_contents.Where(c => c.Sesion == sesion).ToList();
                if (list_contents_sesion.Count > 0)
                {
                    /// 3.- Sacar los contenidos que no son lectura recomendada
                    List<campus_CONTENIDO_CURSO> list_content_principal = list_contents_sesion.Where(c => c.Lectura == 0).ToList();
                    if (list_content_principal.Count > 0)
                    {
                        sbuild.Append("<ul  class='list-unstyled'>");
                        foreach (var content in list_content_principal)
                        {
                            sbuild.Append("<li>" + getResource(content, lst_tasks_content, list_resources) + "</li>");
                        }
                        sbuild.Append("</ul>");
                    }

                    /// 4.- Sacar los contenidos que son lectura recomendada
                    List<campus_CONTENIDO_CURSO> list_content_secondary = list_contents_sesion.Where(c => c.Lectura == 1).ToList();
                    if (list_content_secondary.Count > 0)
                    {
                        sbuild.Append("<p class='h5 bold text-color-black'> Lecturas adicionales </p>");
                        sbuild.Append("<ul  class='list-unstyled'>");
                        foreach (var content in list_content_secondary)
                        {
                            sbuild.Append("<li>" + getResource(content, lst_tasks_content, list_resources) + "</li>");
                        }
                        sbuild.Append("</ul>");
                    }
                }
                sbuild.Append("</div>");
            }

            return sbuild.ToString();
        }
        private string getResource(campus_CONTENIDO_CURSO content, List<campus_AUTO_CURSO> lst_tasks_content, List<campus_RECURSO> list_resources)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Sacar el recurso
            List<campus_RECURSO> list_resource = list_resources.Where(r => r.ID_Recurso == content.ID_Recurso).ToList();
            if (list_resource.Count > 0)
            {
                sbuild.Append("<div class='checkbox'>");

                List<campus_AUTO_CURSO> lst_task = lst_tasks_content.Where(ac => ac.Id_Recurso != null && ac.Id_Recurso == content.ID_Recurso).ToList();
                if (lst_task.Count == 0)
                {
                    sbuild.Append("<input type='text' id='txt_" + content.ID_Recurso + "' name='txt_" + content.ID_Recurso + "' class='form-control' value='0' runat='server' />");
                    sbuild.Append("<input type='checkbox' id='chk_" + content.ID_Recurso + "' name='chk_" + content.ID_Recurso + "' runat='server'/>");                    
                    sbuild.Append("<label for='chk_" + content.ID_Recurso + "'>");
                }
                else
                {
                    sbuild.Append("<a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la tarea de automatización?\")){eliminarAccion(" + lst_task[0].Id_Automatizacion + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a>");
                    sbuild.Append("<input type='text' id='txt_" + content.ID_Recurso + "' name='txt_" + content.ID_Recurso + "' class='form-control' value='" + lst_task[0].Num_Dias + "' runat='server' />");
                    sbuild.Append("<input type='checkbox' id='chk_" + content.ID_Recurso + "' name='chk_" + content.ID_Recurso + "' checked='checked' runat='server'/>");                    
                    sbuild.Append("<label for='chk_" + content.ID_Recurso + "'>");
                    sbuild.Append("(" + lst_task[0].Num_Dias + ")");
                }
                
                if (list_resource[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Nota_Tecnica)
                    sbuild.Append("<i class='far fa-file-pdf margin-lr-5 big-icon text-color-red'></i> <a href='" + ConfigurationManager.AppSettings["nota_tecnica"] + list_resource[0].Rec_Interno + "' class='text-color-text' runat='server' target='_blank'>" + list_resource[0].Titulo + "</a>");
                else if (list_resource[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Caso_Practico)
                    sbuild.Append("<i class='fas fa-cogs margin-lr-5 big-icon'></i> <a href='" + ConfigurationManager.AppSettings["nota_tecnica"] + list_resource[0].Rec_Interno + "' class='text-color-text' runat='server' target='_blank'>" + list_resource[0].Titulo + "</a>");
                else if (list_resource[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Multimedia)
                {
                    if (!String.IsNullOrEmpty(list_resource[0].Rec_Externo))
                        sbuild.Append("<i class='fas fa-video margin-lr-5 big-icon'></i> <a href='" + list_resource[0].Rec_Externo + "' class='text-color-text' runat='server' target='_blank'>" + list_resource[0].Titulo + "</a>");
                    else if (!String.IsNullOrEmpty(list_resource[0].Rec_Interno))
                        sbuild.Append("<i class='fas fa-video margin-lr-5 big-icon'></i> <a href='" + ConfigurationManager.AppSettings["multimedia"] + list_resource[0].Rec_Interno + "' class='text-color-text' runat='server' target='_blank'>" + list_resource[0].Titulo + "</a>");
                }
                else if (list_resource[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Examen)
                    sbuild.Append("<i class='fas fa-book margin-lr-5 big-icon'></i>" + list_resource[0].Titulo);

                sbuild.Append("</label></div>");
            }
            return sbuild.ToString();
        }
    }
}



//// localhost:3063/automatizar-curso.aspx?k=F464D3EB-D005-4BF2-BDE9-2FBC15929606&idc=4