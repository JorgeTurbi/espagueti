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
    public partial class automatizar_curso_mantenimiento : System.Web.UI.Page
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
                    /// 1.- Sacar los datos de la url
                    long idCurso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;
                    int idTipo = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;
                    long idAuto = !String.IsNullOrEmpty(Request.QueryString["idau"]) ? long.Parse(Request.QueryString["idau"].ToString()) : -1;

                    if (idCurso > 0 && idTipo > 0)
                        load_auto(idCurso, idTipo, idAuto);
                    else
                        Response.Redirect("automatizar-curso.aspx?idc=" + idCurso);

                    /// 2.- Botón de volver
                    btn_back.HRef = "automatizar-curso.aspx?idc=" + idCurso;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            long id_curso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            int idTipo = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;
            long idAuto = !String.IsNullOrEmpty(Request.QueryString["idau"]) ? long.Parse(Request.QueryString["idau"].ToString()) : -1;

            if (idTipo == (int)Constantes.type_auto_curso.type_foro)
            {
                /// 1.- Sacar los datos del formulario
                string titulo = txt_asunto.Value;
                string descripcion = txt_cuerpo.Value;
                int num_dias = !String.IsNullOrEmpty(txt_dias_foro.Value) ? int.Parse(txt_dias_foro.Value) : -1;

                if (idAuto > 0)
                {
                    List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idAuto);
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

                            txt_error.InnerHtml = "Se ha producido un error al actualizar la tarea";
                            txt_error.Focus();
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

                    long insert_task = da.insertTaskAutoCourse(task);
                    if (insert_task > 0)
                        Response.Redirect("automatizar-curso.aspx?idc=" + id_curso);
                    else
                    {
                        /// Mostrar el bloque
                        txt_error.InnerHtml = "Se ha producido un error al guardar la tarea";
                        txt_error.Focus();
                    }
                }
            }
            else if (idTipo == (int)Constantes.type_auto_curso.type_msg_foro || idTipo == (int)Constantes.type_auto_curso.type_msg)
            {
                /// 1.- Sacar los datos del formulario
                string titulo = txt_asunto.Value;
                string descripcion = txt_cuerpo.Value;

                int num_dias = 0;
                if (idTipo == (int)Constantes.type_auto_curso.type_msg_foro)
                    num_dias = !String.IsNullOrEmpty(txt_dias_foro.Value) ? int.Parse(txt_dias_foro.Value) : 0;
                else if (idTipo == (int)Constantes.type_auto_curso.type_msg)
                {
                    if (txt_dias_foro.Value == "N")
                        num_dias = -1;
                    else
                        num_dias = !String.IsNullOrEmpty(txt_dias_foro.Value) ? int.Parse(txt_dias_foro.Value) : 0;
                }
                string adjuntos = hidAdjuntos.Value;
                long idForo = !String.IsNullOrEmpty(ddlForo.Value) ? long.Parse(ddlForo.Value) : -1;
                
                /// 1.- Guardar los adjuntos
                string file_adjuntos = string.Empty;
                if (!String.IsNullOrEmpty(adjuntos))
                {
                    List<string> list_files_routes = new List<string>();

                    /// 1.1.- Sacar los adjuntos
                    List<string> list_files = adjuntos.Split(',').ToList();
                    if (list_files.Count > 0)
                    {
                        /// 1.2.- Guardar el fichero en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_automatizacion"];
                        string ruta_destino = ruta + id_curso + "\\";
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        foreach (var _file in list_files)
                        {
                            /// 1.2.1.- Sacar las rutas
                            string ruta_origen = ruta + "temp\\" + _file;
                            string ruta_destino_file = ruta_destino + _file;

                            /// 1.2.2.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino_file, true);

                            /// 1.2.3.- Guardar la ruta
                            list_files_routes.Add(ruta_destino_file);
                        }

                        /// 1.3.- Eliminar la carpeta temporal
                        Directory.Delete(ruta + "temp", true);
                    }

                    /// 1.4.- Devolver los adjuntos
                    int index = 0;
                    foreach (var route in list_files_routes)
                    {
                        if (index == 0)
                            file_adjuntos = route;
                        else
                            file_adjuntos += "," + route;
                        index++;
                    }
                }

                if (idAuto > 0)
                {
                    List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idAuto);
                    if (lst_tasks.Count == 1)
                    {
                        /// Añadir adjuntos
                        if (!String.IsNullOrEmpty(file_adjuntos))
                        {
                            if (!String.IsNullOrEmpty(lst_tasks[0].Adjuntos))
                            {
                                string[] list_adjuntos = lst_tasks[0].Adjuntos.Split(',');
                                foreach (var route in list_adjuntos)
                                {
                                    file_adjuntos += "," + route;
                                }
                            }
                        }
                        else
                            file_adjuntos = lst_tasks[0].Adjuntos;

                        campus_AUTO_CURSO task = lst_tasks[0];
                        task.Asunto = titulo;
                        task.Descripcion = descripcion;
                        task.Num_Dias = num_dias;
                        task.Adjuntos = file_adjuntos;
                        if (idTipo == (int)Constantes.type_auto_curso.type_msg_foro)
                            task.Id_Foro = idForo;

                        bool update_task = da.updateTaskAutoCourse(task);
                        if (update_task)
                            Response.Redirect("automatizar-curso.aspx?idc=" + id_curso);
                        else
                        {
                            /// Mostrar el bloque
                            txt_error.InnerHtml = "Se ha producido un error al actualizar la tarea";
                        }
                    }
                }
                else
                {
                    campus_AUTO_CURSO task = new campus_AUTO_CURSO();
                    task.ID_Curso = id_curso;
                    task.Tipo = idTipo;
                    task.Asunto = titulo;
                    task.Descripcion = descripcion;
                    task.Num_Dias = num_dias;
                    task.Adjuntos = file_adjuntos;
                    if (idTipo == (int)Constantes.type_auto_curso.type_msg_foro)
                        task.Id_Foro = idForo;

                    long insert_task = da.insertTaskAutoCourse(task);
                    if (insert_task > 0)
                        Response.Redirect("automatizar-curso.aspx?idc=" + id_curso);
                    else
                    {
                        /// Mostrar el bloque
                        txt_error.InnerHtml = "Se ha producido un error al guardar la tarea";
                        txt_error.Focus();
                    }
                }
            }
            else if (idTipo == (int)Constantes.type_auto_curso.type_cp)
            {
                /// 1.- Sacar los datos del formulario
                string descripcion = txt_cuerpo.Value;
                string adjuntos = hidAdjuntos.Value;
                int num_dias_cp = !String.IsNullOrEmpty(txt_dias_cp.Value) ? int.Parse(txt_dias_cp.Value) : -1;
                //DateTime fecha_limite = !String.IsNullOrEmpty(date_limit.Value) ? DateTime.Parse(date_limit.Value) : new DateTime();
                int num_dias_lim_cp = 0;
                if (txt_dias_lim_cp.Value == "N")
                    num_dias_lim_cp = -1;
                else
                    num_dias_lim_cp = !String.IsNullOrEmpty(txt_dias_lim_cp.Value) ? int.Parse(txt_dias_lim_cp.Value) : 0;
                string ajunto_clean = string.Empty;

                /// 1.- Guardar los adjuntos
                string file_adjuntos = string.Empty;
                if (!String.IsNullOrEmpty(adjuntos))
                {
                    List<string> list_files_routes = new List<string>();

                    /// 1.1.- Sacar los adjuntos
                    List<string> list_files = adjuntos.Split(',').ToList();
                    if (list_files.Count > 0)
                    {
                        /// 1.2.- Guardar el fichero en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_automatizacion"];
                        string ruta_destino = ruta + id_curso + "\\";
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        foreach (var _file in list_files)
                        {
                            /// 1.2.1.- Sacar las rutas
                            string ruta_origen = ruta + "temp\\" + _file;
                            string ruta_destino_file = ruta_destino + _file;

                            /// 1.2.2.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino_file, true);

                            /// 1.2.3.- Guardar la ruta
                            list_files_routes.Add(ruta_destino_file);

                            /// 1.2.4.- Añadir el nombre del fichero
                            ajunto_clean = _file;
                        }

                        /// 1.3.- Eliminar la carpeta temporal
                        Directory.Delete(ruta + "temp", true);
                    }

                    /// 1.4.- Devolver los adjuntos
                    int index = 0;
                    foreach (var route in list_files_routes)
                    {
                        if (index == 0)
                            file_adjuntos = route;
                        else
                            file_adjuntos += "," + route;
                        index++;
                    }
                }
                
                if (idAuto > 0)
                {
                    List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idAuto);
                    if (lst_tasks.Count == 1)
                    {
                        campus_AUTO_CURSO task = lst_tasks[0];
                        if (!String.IsNullOrEmpty(ajunto_clean))
                            task.Asunto = ajunto_clean;
                        task.Descripcion = descripcion;
                        task.Num_Dias = num_dias_cp;
                        if (!String.IsNullOrEmpty(file_adjuntos))
                            task.Adjuntos = file_adjuntos;
                        //task.Fecha_limite = fecha_limite;
                        task.Num_Dias_Fecha_Limite = num_dias_lim_cp;

                        bool update_task = da.updateTaskAutoCourse(task);
                        if (update_task)
                            Response.Redirect("automatizar-curso.aspx?idc=" + id_curso);
                        else
                        {
                            /// Mostrar el bloque
                            txt_error.InnerHtml = "Se ha producido un error al actualizar la tarea";
                        }
                    }
                }
                else
                {
                    campus_AUTO_CURSO task = new campus_AUTO_CURSO();
                    task.ID_Curso = id_curso;
                    task.Tipo = idTipo;
                    task.Asunto = ajunto_clean;
                    task.Descripcion = descripcion;
                    task.Num_Dias = num_dias_cp;
                    task.Adjuntos = file_adjuntos;
                    //task.Fecha_limite = fecha_limite;
                    task.Num_Dias_Fecha_Limite = num_dias_lim_cp;

                    long insert_task = da.insertTaskAutoCourse(task);
                    if (insert_task > 0)
                        Response.Redirect("automatizar-curso.aspx?idc=" + id_curso);
                    else
                    {
                        /// Mostrar el bloque
                        txt_error.InnerHtml = "Se ha producido un error al guardar la tarea";
                        txt_error.Focus();
                    }
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            bool delete_task = false;

            long id_curso = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            int idTipo = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;
            long idAuto = !String.IsNullOrEmpty(Request.QueryString["idau"]) ? long.Parse(Request.QueryString["idau"].ToString()) : -1;
            
            try
            {                
                string adjunto = hidAdjuntos.Value;
                if (!String.IsNullOrEmpty(adjunto))
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idAuto);
                    if (lst_tasks.Count == 1)
                    {
                        string adjuntos = string.Empty;
                        string mail_adjuntos = lst_tasks[0].Adjuntos;
                        if (!String.IsNullOrEmpty(mail_adjuntos))
                        {
                            string _file_adjunto = ConfigurationManager.AppSettings["ruta_automatizacion"] + id_curso + "\\" + adjunto;
                            mail_adjuntos = mail_adjuntos.Replace(_file_adjunto, string.Empty);
                            string[] list_adjuntos = mail_adjuntos.Split(',');

                            int cont = 0;
                            if (list_adjuntos.Length > 0)
                            {
                                foreach (string _adjunto in list_adjuntos)
                                {
                                    if (!String.IsNullOrEmpty(_adjunto))
                                    {
                                        if (cont == 0)
                                            adjuntos = _adjunto;
                                        else
                                            adjuntos += "," + _adjunto;
                                        cont++;
                                    }
                                }
                            }

                            /// Eliminar el fichero
                            if (File.Exists(_file_adjunto))
                                File.Delete(_file_adjunto);
                        }

                        campus_AUTO_CURSO task = lst_tasks[0];
                        task.Adjuntos = adjuntos;

                        delete_task = da.updateTaskAutoCourse(task);
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
                Response.Redirect("automatizar-curso-mantenimiento.aspx?idc=" + id_curso + "&idt=" + idTipo + (idAuto > 0 ? "&idau=" + idAuto : string.Empty));
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        private void load_auto(long idCurso, int idTipo, long idAuto)
        {
            if (idTipo == (int)Constantes.type_auto_curso.type_foro)
            {
                /// 0.- Poner el título
                txt_title.InnerHtml = "<i class='fas fa-comments'></i> Foro";

                /// 2.- Poner el botón de guardar
                btn_save_foro.Attributes["class"] = btn_save_foro.Attributes["class"].Replace("hidden", string.Empty);

                /// 3.- Inicializar el nº de días
                txt_dias_foro.Value = "0";

                /// 4.- Desbloquear el bloque
                block_msg.Attributes["class"] = block_msg.Attributes["class"].Replace("hidden", string.Empty);

                /// 5.- Ocultar bloque adjuntos
                blk_adjuntos.Attributes["class"] = blk_adjuntos.Attributes["class"].Insert(blk_adjuntos.Attributes["class"].Length, " hidden");

                /// 6.- Cargar el foro
                if (idAuto > 0)
                    load_foro(idAuto);
            }
            else if (idTipo == (int)Constantes.type_auto_curso.type_msg_foro)
            {
                /// 0.- Poner el título
                txt_title.InnerHtml = "<i class='fas fa-comment-dots'></i> Mandar mensaje en el foro";

                /// 1.- Mostrar el bloque foro
                blk_foro.Attributes["class"] = blk_foro.Attributes["class"].Replace("hidden", string.Empty);

                /// Sacar los foros de automatización
                List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourseByCourse(idCurso);
                lst_tasks = lst_tasks.Where(t => t.Tipo == (int)Constantes.type_auto_curso.type_foro).ToList();

                /// 2.- Pintar los foros
                if (lst_tasks.Count > 0)
                {
                    /// 2.1.- Foro
                    ddlForo.DataSource = lst_tasks;
                    ddlForo.DataTextField = "Asunto";
                    ddlForo.DataValueField = "Id_Automatizacion";
                    ddlForo.DataBind();
                    ddlForo.Items.Insert(0, new ListItem("Seleccione un foro", "-1"));
                    ddlForo.Value = "-1";
                }

                /// 3.- Bloque adjuntos
                file_adjunto.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre la foto del usuario en el área seleccionada</span><input id='fileupload_adjunto' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idc\": \"" + idCurso + "\", \"type\": \"auto\", \"accion\": \"update\" }' />";

                /// 4.- Poner el botón de guardar
                btn_save.Attributes["class"] = btn_save.Attributes["class"].Replace("hidden", string.Empty);

                /// 5.- Inicializar el nº de días
                txt_dias_foro.Value = "0";

                /// 6.- Desbloquear el bloque
                block_msg.Attributes["class"] = block_msg.Attributes["class"].Replace("hidden", string.Empty);

                /// 7.- Cargar el mensaje del foro
                if (idAuto > 0)
                    load_msg_foro(idAuto);
            }
            else if (idTipo == (int)Constantes.type_auto_curso.type_msg)
            {
                /// 0.- Poner el título
                txt_title.InnerHtml = "<i class='fas fa-envelope'></i> Mandar mensaje";

                /// 1.- Bloque adjuntos
                file_adjunto.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre la foto del usuario en el área seleccionada</span><input id='fileupload_adjunto' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idc\": \"" + idCurso + "\", \"type\": \"auto\", \"accion\": \"update\" }' />";

                /// 2.- Poner el botón de guardar
                btn_save_msg.Attributes["class"] = btn_save_msg.Attributes["class"].Replace("hidden", string.Empty);

                /// 3.- Inicializar el nº de días
                txt_dias_foro.Value = "0";

                /// 4.- Desbloquear el bloque
                block_msg.Attributes["class"] = block_msg.Attributes["class"].Replace("hidden", string.Empty);

                /// 7.- Cargar el mensaje del foro
                if (idAuto > 0)
                    load_msg(idAuto);
            }
            else if (idTipo == (int)Constantes.type_auto_curso.type_cp)
            {
                /// 0.- Poner el título
                txt_title.InnerHtml = "<i class='fas fa-file'></i> Caso Práctico";

                /// 1.- Bloque adjuntos
                file_adjunto.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre la foto del usuario en el área seleccionada</span><input id='fileupload_adjunto' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idc\": \"" + idCurso + "\", \"type\": \"auto\", \"accion\": \"update\" }' />";

                /// 2.- Poner el botón de guardar
                btn_save_cp.Attributes["class"] = btn_save_cp.Attributes["class"].Replace("hidden", string.Empty);

                /// 3.- Inicializar el nº de días
                txt_dias_cp.Value = "0";

                /// 4.- Desbloquear el bloque
                blk_cp.Attributes["class"] = blk_cp.Attributes["class"].Replace("hidden", string.Empty);

                /// 5.- Cargar el mensaje del foro
                if (idAuto > 0)
                    load_cp(idAuto);
            }
        }
        
        private void load_foro(long idTask)
        {
            List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idTask);
            if (lst_tasks.Count == 1)
            {
                txt_asunto.Value = lst_tasks[0].Asunto;
                txt_dias_foro.Value = lst_tasks[0].Num_Dias.ToString();
                txt_cuerpo.Value = lst_tasks[0].Descripcion;
            }
        }
        private void load_msg_foro(long idTask)
        {
            List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idTask);
            if (lst_tasks.Count == 1)
            {
                txt_asunto.Value = lst_tasks[0].Asunto;
                txt_dias_foro.Value = lst_tasks[0].Num_Dias.ToString();
                txt_cuerpo.Value = lst_tasks[0].Descripcion;
                ddlForo.Value = lst_tasks[0].Id_Foro.Value.ToString();

                if (!String.IsNullOrEmpty(lst_tasks[0].Adjuntos))
                {
                    StringBuilder sbuild = new StringBuilder();

                    string route = ConfigurationManager.AppSettings["ruta_automatizacion"] + lst_tasks[0].ID_Curso + "\\";
                    string route_see = ConfigurationManager.AppSettings["url_automatizacion"] + lst_tasks[0].ID_Curso + "/";

                    string[] list_adjuntos = lst_tasks[0].Adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        /// 1.- Inicio tabla
                        sbuild.Append("<table id =\"tabla_Adjuntos\" class=\"display compact\" style =\"width:100%\"><thead>");

                        /// 2.- Cabecera
                        sbuild.Append("<tr>");
                        sbuild.Append("<th>Nombre</th>");
                        sbuild.Append("<th></th>");
                        sbuild.Append("<th></th>");
                        sbuild.Append("</tr>");
                        sbuild.Append("</thead><tbody>");

                        foreach (var _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();
                            sbuild.Append("<tr>");
                            sbuild.Append("<td>" + ajunto_clean + "</td>");
                            sbuild.Append("<td><a href='" + route_see + ajunto_clean + "' target='_blank'><i class='fas fa-eye fa-1-6x'></i></a></td>");
                            sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el adjunto?\")){eliminarAdjunto(\"" + ajunto_clean + "\")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                            sbuild.Append("</tr>");
                        }

                        sbuild.Append("</tbody></table>");
                    }

                    block_adjuntos_lst.InnerHtml = sbuild.ToString();
                }
            }
        }
        private void load_msg(long idTask)
        {
            List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idTask);
            if (lst_tasks.Count == 1)
            {
                txt_asunto.Value = lst_tasks[0].Asunto;
                if (lst_tasks[0].Num_Dias == -1)
                    txt_dias_foro.Value = "N";
                else
                    txt_dias_foro.Value = lst_tasks[0].Num_Dias.ToString();
                txt_cuerpo.Value = lst_tasks[0].Descripcion;

                if (!String.IsNullOrEmpty(lst_tasks[0].Adjuntos))
                {
                    StringBuilder sbuild = new StringBuilder();

                    string route = ConfigurationManager.AppSettings["ruta_automatizacion"] + lst_tasks[0].ID_Curso + "\\";
                    string route_see = ConfigurationManager.AppSettings["url_automatizacion"] + lst_tasks[0].ID_Curso + "/";

                    string[] list_adjuntos = lst_tasks[0].Adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        /// 1.- Inicio tabla
                        sbuild.Append("<table id =\"tabla_Adjuntos\" class=\"display compact\" style =\"width:100%\"><thead>");

                        /// 2.- Cabecera
                        sbuild.Append("<tr>");
                        sbuild.Append("<th>Nombre</th>");
                        sbuild.Append("<th></th>");
                        sbuild.Append("<th></th>");
                        sbuild.Append("</tr>");
                        sbuild.Append("</thead><tbody>");

                        foreach (var _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();
                            sbuild.Append("<tr>");
                            sbuild.Append("<td>" + ajunto_clean + "</td>");
                            sbuild.Append("<td><a href='" + route_see + ajunto_clean + "' target='_blank'><i class='fas fa-eye fa-1-6x'></i></a></td>");
                            sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el adjunto?\")){eliminarAdjunto(\"" + ajunto_clean + "\")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                            sbuild.Append("</tr>");
                        }

                        sbuild.Append("</tbody></table>");
                    }

                    block_adjuntos_lst.InnerHtml = sbuild.ToString();
                }
            }
        }
        private void load_cp(long idTask)
        {
            List<campus_AUTO_CURSO> lst_tasks = da.getAutoCourse(idTask);
            if (lst_tasks.Count == 1)
            {
                txt_cuerpo.Value = lst_tasks[0].Descripcion;
                txt_dias_cp.Value = lst_tasks[0].Num_Dias.ToString();
                if (lst_tasks[0].Num_Dias_Fecha_Limite == -1)
                    txt_dias_lim_cp.Value = "N";
                else
                    txt_dias_lim_cp.Value = lst_tasks[0].Num_Dias_Fecha_Limite.ToString();
                //date_limit.Value = lst_tasks[0].Fecha_limite.Value.ToShortDateString();
                
                if (!String.IsNullOrEmpty(lst_tasks[0].Adjuntos))
                {
                    lbl_adjuntos.Attributes["class"] = lbl_adjuntos.Attributes["class"].Insert(lbl_adjuntos.Attributes["class"].Length, " hidden");

                    StringBuilder sbuild = new StringBuilder();

                    string route = ConfigurationManager.AppSettings["ruta_automatizacion"] + lst_tasks[0].ID_Curso + "\\";
                    string route_see = ConfigurationManager.AppSettings["url_automatizacion"] + lst_tasks[0].ID_Curso + "/";

                    string[] list_adjuntos = lst_tasks[0].Adjuntos.Split(',');
                    if (list_adjuntos.Length > 0)
                    {
                        /// 1.- Inicio tabla
                        sbuild.Append("<table id =\"tabla_Adjuntos\" class=\"display compact\" style =\"width:100%\"><thead>");

                        /// 2.- Cabecera
                        sbuild.Append("<tr>");
                        sbuild.Append("<th>Nombre</th>");
                        sbuild.Append("<th></th>");
                        sbuild.Append("<th></th>");
                        sbuild.Append("</tr>");
                        sbuild.Append("</thead><tbody>");

                        foreach (var _adjunto in list_adjuntos)
                        {
                            string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();
                            sbuild.Append("<tr>");
                            sbuild.Append("<td>" + ajunto_clean + "</td>");
                            sbuild.Append("<td><a href='" + route_see + ajunto_clean + "' target='_blank'><i class='fas fa-eye fa-1-6x'></i></a></td>");
                            sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el adjunto?\")){eliminarAdjunto(\"" + ajunto_clean + "\")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                            sbuild.Append("</tr>");
                        }

                        sbuild.Append("</tbody></table>");
                    }

                    block_adjuntos_lst.InnerHtml = sbuild.ToString();
                }
            }
        }
    }
}