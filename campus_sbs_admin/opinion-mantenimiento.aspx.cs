using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class opinion_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la opinión
                long idOpinion = !String.IsNullOrEmpty(Request.QueryString["ido"]) ? long.Parse(Request.QueryString["ido"].ToString()) : -1;

                /// 2.- Cargar combos
                cargar_cursos();
                cargar_tipo_solicitud();

                /// 3.- Cargar los datos del contacto
                if (idOpinion > 0)
                    cargar_datos(idOpinion);
                else
                    cargar_docencias();

                /// 4.- Datos del fileupload
                file_anexo.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre el fichero del anexo en el área seleccionada</span><input id='fileupload_foto' type='file' data-url='controls/UploadHandler.ashx' data-form-data='{\"ido\": \"" + idOpinion + "\", \"type\": \"img_opinion\", \"accion\": \"update\" }' />";
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos de la opinión
            long idOpinion = !String.IsNullOrEmpty(Request.QueryString["ido"]) ? long.Parse(Request.QueryString["ido"].ToString()) : -1;

            /// 2.- Sacar los datos del formulario
            long idStudent = long.Parse(idAlumno.Value);
            string cargo = txt_cargo.Value;
            long idCurso = long.Parse(ddlCurso.Value);
            //long idDocencia = long.Parse(ddlDocencia.Value);
            long idDocencia = -1;
            string solicitud = null;
            if (ddlTextoPregunta.Value != "-1")
                solicitud = ddlTextoPregunta.Value;
            string foto_opinion = txtFoto.Value;
            string comentarios = txt_comentarios.Value;
            string video = txt_video.Value;
            bool contacto = chkContacto.Checked;
            int? valoracion = null;
            if (int.Parse(ddlValoracion.SelectedValue) > 0)
                valoracion = int.Parse(ddlValoracion.SelectedValue);
            bool recomendacion = chkRecomendacion.Checked;
            bool visible = chkVisible.Checked;

            /// 2.5.- Comprobar que no haya dos opiniones para el mismo curso y pregunta
            bool error = false;
            if (solicitud != null)
            {
                List<campus_OPINIONES> lst_opiniones = da.getOpinions(idStudent, idCurso);
                if (lst_opiniones.Count > 0)
                {
                    lst_opiniones = lst_opiniones.Where(o => o.texto_pregunta == solicitud).ToList();
                    if (lst_opiniones.Count == 1)
                    {
                        if (idOpinion == -1 || (idOpinion > 0 && lst_opiniones[0].IdOpinion != idOpinion))
                            error = true;
                    }
                }
            }

            if (error)
                txt_error.InnerHtml = "Ya existe una opinión para ese alumno, curso y pregunta";
            else
            {
                /// 3.- Sacar datos de la BBDD
                List<campus_CURSO> lst_courses = da.getCourseById(idCurso);
                List<campus_DOCENCIA> lst_docencias = da.getDocenciaById(idDocencia);
                List<CLIENTES> lst_user = da.getUserById(idStudent);

                /// 4.- Modificar o Insertar
                if (idOpinion > 0)
                {
                    List<campus_OPINIONES> lst_opinion = da.getOpinionById(idOpinion);
                    if (lst_opinion.Count == 1)
                    {
                        /// 4.0.- Cambiar carpeta de la foto de la opinión si se ha modificado el idCurso
                        if (!String.IsNullOrEmpty(foto_opinion) && idCurso != lst_opinion[0].IdCurso)
                        {
                            /// 4.0.0.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["ruta_foto_opinion"];

                            /// 4.0.1.- Rutas nuevas
                            string ruta_origen = ruta + lst_opinion[0].IdCurso + "\\" + foto_opinion;
                            string ruta_destino = ruta + idCurso + "\\";

                            /// 4.0.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + foto_opinion;

                            /// 4.0.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 4.0.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta de origen
                        }

                        /// 4.1.- Comprobar foto opinión
                        if (!String.IsNullOrEmpty(foto_opinion) && foto_opinion != lst_opinion[0].Foto)
                        {
                            /// 4.1.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["ruta_foto_opinion"];

                            /// 4.1.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(lst_opinion[0].Foto))
                                File.Delete(ruta + lst_opinion[0].IdCurso + "\\" + lst_opinion[0].Foto);

                            /// 4.1.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + foto_opinion;
                            string ruta_destino = ruta + lst_opinion[0].IdCurso + "\\";

                            /// 4.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + foto_opinion;

                            /// 4.1.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 4.1.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                            /// 4.1.5.- Borramos el directorio temp
                            if (Directory.GetFiles(ruta + "temp\\").Count() == 0)
                            {
                                if ((Directory.Exists(ruta + "temp\\")))
                                    Directory.Delete(ruta + "temp\\");
                            }
                            else
                            {
                                foreach (var file in Directory.GetFiles(ruta + "temp\\"))
                                {
                                    File.Delete(file);
                                }

                                if ((Directory.Exists(ruta + "temp\\")))
                                    Directory.Delete(ruta + "temp\\");
                            }
                        }

                        /// 4.2.- Actualizar los datos de la práctica
                        campus_OPINIONES opinion = lst_opinion[0];
                        if (lst_user.Count == 1)
                            opinion.Nombre = lst_user[0].Nombre_Completo;
                        if (!String.IsNullOrEmpty(cargo))
                            opinion.Cargo = cargo;
                        if (lst_courses.Count == 1)
                            opinion.Curso = lst_courses[0].Nombre;
                        opinion.Comentario = comentarios;
                        if (!String.IsNullOrEmpty(foto_opinion))
                            opinion.Foto = foto_opinion;
                        if (!String.IsNullOrEmpty(video))
                            opinion.Video = video;
                        opinion.IdPersona = idStudent;
                        opinion.IdCurso = idCurso;
                        if (idDocencia > 0)
                            opinion.IdDocencia = idDocencia;
                        opinion.permite_contacto = contacto;
                        opinion.valoracion = valoracion;
                        opinion.recomendacion = recomendacion;
                        opinion.visible = visible;
                        opinion.texto_pregunta = solicitud;

                        bool update_opinion = da.updateOpinion(opinion);
                        if (update_opinion)
                            Response.Redirect("lista-opiniones.aspx");
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar los datos de la opinión";
                    }
                }
                else
                {
                    /// 4.1.- Añadir los datos de la opinión
                    campus_OPINIONES opinion = new campus_OPINIONES();
                    opinion.Fecha = DateTime.Now;
                    if (lst_user.Count == 1)
                        opinion.Nombre = lst_user[0].Nombre_Completo;
                    if (!String.IsNullOrEmpty(cargo))
                        opinion.Cargo = cargo;
                    if (lst_courses.Count == 1)
                        opinion.Curso = lst_courses[0].Nombre;
                    opinion.Comentario = comentarios;
                    if (!String.IsNullOrEmpty(foto_opinion))
                        opinion.Foto = foto_opinion;
                    if (!String.IsNullOrEmpty(video))
                        opinion.Video = video;
                    opinion.IdPersona = idStudent;
                    opinion.IdCurso = idCurso;
                    if (idDocencia > 0)
                        opinion.IdDocencia = idDocencia;
                    opinion.permite_contacto = contacto;
                    opinion.valoracion = valoracion;
                    opinion.recomendacion = recomendacion;
                    opinion.visible = visible;
                    opinion.texto_pregunta = solicitud;

                    long insert_opinion = da.insertOpinion(opinion);
                    if (insert_opinion > 0)
                    {
                        bool process = true;

                        /// 4.2.- Comprobar la foto nueva
                        if (!String.IsNullOrEmpty(foto_opinion))
                        {
                            try
                            {
                                /// 4.2.1.- Guardar el fichero en la carpeta correcta
                                string ruta = ConfigurationManager.AppSettings["ruta_foto_opinion"];

                                string ruta_origen = ruta + "temp\\" + foto_opinion;
                                string ruta_destino = ruta + idCurso + "\\";

                                /// 4.2.2.- Si no existe el directorio lo creamos.
                                if (!(Directory.Exists(ruta_destino)))
                                    Directory.CreateDirectory(ruta_destino);

                                ruta_destino = ruta_destino + foto_opinion;

                                /// 4.2.3.- Copiar el fichero
                                File.Copy(ruta_origen, ruta_destino, true);

                                /// 4.2.4.- Borramos el fichero de la carpeta origen
                                File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                                /// 4.2.5.- Borramos el directorio temp
                                if (Directory.GetFiles(ruta + "temp\\").Count() == 0)
                                {
                                    if ((Directory.Exists(ruta + "temp\\")))
                                        Directory.Delete(ruta + "temp\\");
                                }
                                else
                                {
                                    foreach (var file in Directory.GetFiles(ruta + "temp\\"))
                                    {
                                        File.Delete(file);
                                    }

                                    if ((Directory.Exists(ruta + "temp\\")))
                                        Directory.Delete(ruta + "temp\\");
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtils.InsertarLog(" ERROR - opinion-mantenimiento.cs::btnGuardar_Click()");
                                LogUtils.InsertarLog("- MSG:" + ex.Message);
                                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                                process = false;
                            }
                        }

                        if (process)
                            Response.Redirect("lista-opiniones.aspx");
                        else
                            txt_error.InnerHtml = "Se ha producido un error al guardar la foto";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir los datos de la opinión";
                }
            }
        }

        [WebMethod(Description = "Busca alumnos a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> search_student(string name)
        {
            DataAccess da = new DataAccess();

            List<Usuarios> list_users = new List<Usuarios>();
            List<CLIENTES> lst_users = da.getUserBySearch(name, null);
            if (lst_users.Count > 0)
                list_users = lst_users.Select(user => new Usuarios { id_usuario = user.id_cliente, nombre_completo = user.Nombre_Completo }).ToList();
            return list_users;
        }

        [WebMethod(Description = "Busca Tutor de empresa a partir de un id de empresa")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Docencia> cargarDocenciasWS(long idCurso)
        {
            DataAccess da = new DataAccess();

            List<Docencia> list_docencias = new List<Docencia>();
            List<campus_DOCENCIA> lst_docencias = da.getDocenciaByIdCurso(idCurso);
            if (lst_docencias.Count > 0)
                list_docencias = lst_docencias.Select(doc => new Docencia { id_docencia = doc.ID_Docencia, nombre = doc.Nombre }).ToList();
            return list_docencias;
        }

        private void cargar_cursos()
        {
            /// 1.- Cargar los cursos
            List<campus_CURSO> lst_courses = da.getCoursesWeb(true).OrderBy(c => c.Nombre).ToList();
            if (lst_courses.Count > 0)
            {
                /*foreach (var course in lst_courses)
                {
                    if (!course.Activo)
                    {
                        ListItem li = new ListItem(course.Nombre + " (" + course.COD_Curso + ") V" + course.Version, course.ID_Curso.ToString());
                        li.Attributes.Add("style", "color:red");

                        ddlCurso.Items.Add(li);
                    }
                    else
                        ddlCurso.Items.Add(new ListItem(course.Nombre + " (" + course.COD_Curso + ") V" + course.Version, course.ID_Curso.ToString()));
                }*/
                ddlCurso.DataSource = lst_courses;
                ddlCurso.DataTextField = "Nombre";
                ddlCurso.DataValueField = "ID_Curso";
                ddlCurso.DataBind();
                ddlCurso.Items.Add(new ListItem("Seleccione", "-1"));
                ddlCurso.Value = "-1";
            }
        }
        private void cargar_tipo_solicitud()
        {
            /// 1.- Cargar el tipo solicitud
            List<campus_TIPO_SOLICITUD_OPINION> lst_types = da.getTypeOpinion(true);
            if (lst_types.Count > 0)
            {
                ddlTextoPregunta.DataSource = lst_types;
                ddlTextoPregunta.DataTextField = "solicitud";
                ddlTextoPregunta.DataValueField = "solicitud";
                ddlTextoPregunta.DataBind();
                ddlTextoPregunta.Items.Add(new ListItem("Seleccione", "-1"));
                ddlTextoPregunta.Value = "-1";
            }
        }
        private void cargar_datos(long idOpinion)
        {
            List<campus_OPINIONES> lst_opinion = da.getOpinionById(idOpinion);
            if (lst_opinion.Count == 1)
            {
                txt_alumno.Value = lst_opinion[0].Nombre;
                idAlumno.Value = lst_opinion[0].IdPersona.ToString();
                txt_cargo.Value = lst_opinion[0].Cargo;
                ddlCurso.Value = lst_opinion[0].IdCurso.Value.ToString();

                //cargar_docencias();

                //ddlDocencia.Value = lst_opinion[0].IdDocencia != null ? lst_opinion[0].IdDocencia.Value.ToString() : "-1";
                //id_Docencia.Value = lst_opinion[0].IdDocencia != null ? lst_opinion[0].IdDocencia.Value.ToString() : string.Empty;
                ddlTextoPregunta.Value = lst_opinion[0].texto_pregunta;
                txtFoto.Value = lst_opinion[0].Foto;

                if (!String.IsNullOrEmpty(lst_opinion[0].Foto))
                {
                    txt_img_foto.HRef = ConfigurationManager.AppSettings["url_foto_opinion"] + lst_opinion[0].IdCurso + "/" + lst_opinion[0].Foto;
                    txt_img_foto.Attributes["class"] = txt_img_foto.Attributes["class"].Replace("hidden", string.Empty);
                }
                txt_comentarios.Value = lst_opinion[0].Comentario;
                txt_video.Value = lst_opinion[0].Video;
                chkContacto.Checked = lst_opinion[0].permite_contacto != null ? lst_opinion[0].permite_contacto.Value : false;
                ddlValoracion.SelectedValue = lst_opinion[0].valoracion != null ? lst_opinion[0].valoracion.Value.ToString() : "-1";
                chkRecomendacion.Checked = lst_opinion[0].recomendacion != null ? lst_opinion[0].recomendacion.Value : false;
                chkVisible.Checked = lst_opinion[0].visible != null ? lst_opinion[0].visible.Value : false;
                txt_huella.Value = lst_opinion[0].huella;

                if (lst_opinion[0].id_os != null && lst_opinion[0].id_os > 0)
                    ddlTextoPregunta.Attributes.Add("disabled", "disabled");
            }
        }
        private void cargar_docencias()
        {
            /*List<campus_DOCENCIA> lst_docencias = da.getDocenciaByIdCurso(-1);
            if (lst_docencias.Count > 0)
            {
                List<Docencia> list_docencias = lst_docencias.Select(doc => new Docencia { id_docencia = doc.ID_Docencia, nombre = doc.Nombre }).ToList();
                ddlDocencia.DataSource = list_docencias;
                ddlDocencia.DataTextField = "nombre";
                ddlDocencia.DataValueField = "id_docencia";
                ddlDocencia.DataBind();
                ddlDocencia.Items.Add(new ListItem("Seleccione", "-1"));
                ddlDocencia.Value = "-1";
            }*/
        }
    }
}