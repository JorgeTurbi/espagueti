using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sbs_DAL;
using System.Configuration;
using System.IO;

namespace Solicitud_opinion_process
{
    class AutoCurso
    {
        DataAccess da = new DataAccess();
        
        public bool process(campus_TUTORIA tutoria, campus_AUTO_CURSO tarea)
        {
            bool procesado = false;

            try
            {
                if (tarea.Tipo == (int)Constantes.type_auto_curso.type_foro)
                {
                    /// 1.- Generar el foro
                    campus_FORO forum = new campus_FORO();
                    forum.ID_Curso = tutoria.idCurso;
                    forum.ID_Docencia_Grupo = tutoria.idDocencia;
                    forum.Titulo = tarea.Asunto;
                    forum.Descripcion = tarea.Descripcion;
                    forum.FAlta = DateTime.Now;
                    forum.ID_Moderador = tutoria.idTutor;

                    /// 2.- Añadir el foro
                    long insert = da.insertForum(forum);
                    if (insert > 0)
                        procesado = true;
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_msg_foro)
                {
                    /// 1.- Obtener los datos del foro
                    List<campus_AUTO_CURSO> lst_auto = da.getAutoCourse(tarea.Id_Foro.Value);
                    List<campus_FORO> list_foro = da.getForo(tutoria.idCurso, tutoria.idDocencia, -1);
                    list_foro = list_foro.Where(f => f.Titulo == lst_auto[0].Asunto).ToList();

                    /// 1.- Guardar la pregunta
                    campus_FORO_MENSAJE _question = new campus_FORO_MENSAJE();
                    _question.ID_Foro = list_foro[0].ID_Foro;
                    _question.Titulo = tarea.Asunto;
                    _question.Mensaje = tarea.Descripcion;
                    _question.Fecha = DateTime.Now;
                    _question.ID_Persona = tutoria.idTutor;
                    _question.Borrador = ((int)Constantes.activo.NoActivo).ToString();

                    long insert_question = da.insertForumQuestion(_question);
                    if (insert_question > 0)
                    {
                        /// 2.- Guardar los adjuntos
                        string adjuntos = string.Empty;
                        if (!String.IsNullOrEmpty(tarea.Adjuntos))
                        {
                            /// 2.1.- Sacar los adjuntos
                            List<string> list_files = tarea.Adjuntos.Split(',').ToList();
                            if (list_files.Count > 0)
                            {
                                /// 2.2.- Guardar el fichero en la carpeta correcta
                                string ruta = ConfigurationManager.AppSettings["route_foro"] + tutoria.idDocencia + "\\" + tutoria.idCurso + "\\";
                                if (!(Directory.Exists(ruta)))
                                    Directory.CreateDirectory(ruta);

                                foreach (var _file in list_files)
                                {
                                    /// 2.2.0.- Cambiar los nombres de los ficheros
                                    FileInfo _file_name = new FileInfo(_file);
                                    string _name_file = tutoria.idTutor + "_" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString() + _file_name.Extension;

                                    /// 2.2.1.- Sacar las rutas
                                    string ruta_origen = _file;
                                    string ruta_destino = ruta + _name_file;

                                    /// 2.2.2.- Copiar el fichero
                                    File.Copy(ruta_origen, ruta_destino, true);

                                    /// 2.2.3.- Guardar el adjunto en la BBDD
                                    campus_FORO_ADJUNTOS _adjunto = new campus_FORO_ADJUNTOS();
                                    _adjunto.Depende_De = 0;
                                    _adjunto.ID_Mensaje = insert_question;
                                    _adjunto.ID_Foro = list_foro[0].ID_Foro;
                                    _adjunto.Titulo = _file_name.Name;
                                    _adjunto.Adjunto_Fichero = _name_file;

                                    bool _save = da.insertAttachmentAnswer(_adjunto);
                                    if (_save)
                                        procesado = true;
                                }
                            }
                        }
                        else
                            procesado = true;
                    }
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_msg)
                {
                    /// 1.- Guardar los adjuntos
                    string adjuntos = string.Empty;
                    if (!String.IsNullOrEmpty(tarea.Adjuntos))
                    {
                        List<string> list_files_routes = new List<string>();

                        /// 1.1.- Sacar los adjuntos
                        List<string> list_files = tarea.Adjuntos.Split(',').ToList();
                        if (list_files.Count > 0)
                        {
                            /// 1.2.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["route_mail"] + tutoria.idTutor + "\\" + tutoria.idDocencia + "\\" + tutoria.idCurso + "\\";
                            if (!(Directory.Exists(ruta)))
                                Directory.CreateDirectory(ruta);

                            foreach (var _file in list_files)
                            {
                                FileInfo _file_name = new FileInfo(_file);

                                /// 1.2.1.- Sacar las rutas
                                string ruta_origen = _file;
                                string ruta_destino = ruta + _file_name.Name;

                                /// 1.2.2.- Copiar el fichero
                                File.Copy(ruta_origen, ruta_destino, true);

                                /// 1.2.3.- Guardar la ruta
                                list_files_routes.Add(ruta_destino);
                            }
                        }

                        /// 1.3.- Devolver los adjuntos
                        int index = 0;
                        foreach (var route in list_files_routes)
                        {
                            if (index == 0)
                                adjuntos = route;
                            else
                                adjuntos += "," + route;
                            index++;
                        }
                    }

                    /// 2.- Generamos un anuncio para procesar
                    campus_ANUNCIO anuncio = new campus_ANUNCIO();
                    anuncio.Fecha = DateTime.Now;
                    anuncio.Titulo = tarea.Asunto;
                    anuncio.Cuerpo = tarea.Descripcion;
                    anuncio.Tipo_Anuncio = (long)Constantes.type_message.Email;
                    anuncio.ID_Docencia_Grupo = tutoria.idDocencia;
                    anuncio.ID_Curso = tutoria.idCurso;
                    anuncio.ID_Persona_Publica = tutoria.idTutor;
                    anuncio.Fecha_Publica = DateTime.Now;
                    if (!String.IsNullOrEmpty(adjuntos))
                        anuncio.Adjuntos = adjuntos;
                    anuncio.Pendiente = true;

                    long insert_anuncio = da.insertAnuncio(anuncio);
                    if (insert_anuncio > 0)
                        procesado = true;
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_contenido)
                {
                    /// 1.- Sacar el contenido curso
                    List<campus_CONTENIDO_CURSO> lst_content_course = da.getContentCourse(tutoria.idCurso);
                    List<campus_CURSO> list_courses = da.getCourseById(tutoria.idCurso);

                    /// 2.- Filtrar el contenido por el id_recurso
                    lst_content_course = lst_content_course.Where(cc => cc.ID_Recurso == tarea.Id_Recurso).ToList();
                    if (lst_content_course.Count == 1)
                    {
                        /// 2.1.- Comprobar si hay una entrada en campus_CONTENIDO_DOCENCIA
                        List<campus_CONTENIDO_DOCENCIA> lst_content_docencia = da.getContentDocencia(tutoria.idDocencia, tutoria.idCurso, null);
                        lst_content_docencia = lst_content_docencia.Where(cd => cd.ID_Recurso == tarea.Id_Recurso).ToList();
                        if (lst_content_docencia.Count == 0)
                        {
                            /// 3.- Generar un objeto campus_CONTENIDO_DOCENCIA
                            campus_CONTENIDO_DOCENCIA _content = new campus_CONTENIDO_DOCENCIA();
                            _content.ID_Docencia = tutoria.idDocencia;
                            _content.ID_Curso = tutoria.idCurso;
                            _content.ID_Recurso = tarea.Id_Recurso.Value;
                            _content.COD_Curso = lst_content_course[0].COD_Curso;
                            _content.COD_Recurso = lst_content_course[0].COD_Recurso;
                            _content.Sesion = lst_content_course[0].Sesion;
                            _content.Lectura = lst_content_course[0].Lectura;
                            _content.Fecha = DateTime.Today;
                            _content.Visible = (decimal)Constantes.activo.Activo;

                            /// 3.1.- Añadir una entrada en campus_CONTENIDO_DOCENCIA en la BBDD
                            long insert_content = da.insertContentDocencia(_content);
                            if (insert_content > 0)
                            {
                                /// 4.- Añadir una entrada en campus_ANUNCIO con el tipo_anuncio = contenido con la fecha actual.
                                string title = "Publicado nuevo contenido en ( " + list_courses[0].Nombre + " )";

                                /// 4.0.- Comprobar si hay una entrada igual en el día
                                List<campus_ANUNCIO> lst_anuncios = da.getAnunciosbyDocenciaAndCourse(tutoria.idDocencia, tutoria.idCurso);
                                lst_anuncios = lst_anuncios.Where(a => a.Titulo == title && a.Fecha == DateTime.Today).ToList();

                                if (lst_anuncios.Count == 0)
                                {
                                    /// 4.1.- Generar un objeto campus_ANUNCIO
                                    campus_ANUNCIO anuncio = new campus_ANUNCIO();
                                    anuncio.Fecha = DateTime.Today;
                                    anuncio.Titulo = title;
                                    anuncio.ID_Docencia_Grupo = tutoria.idDocencia;
                                    anuncio.Tipo_Anuncio = (int)Constantes.type_message.Contenido;
                                    anuncio.ID_Curso = tutoria.idCurso;
                                    anuncio.ID_Persona_Publica = tutoria.idTutor;
                                    anuncio.Fecha_Publica = DateTime.Now;

                                    /// 4.2.- Inserta en la BBDD el anuncio
                                    long insert_anuncio = da.insertAnuncio(anuncio);
                                    if (insert_anuncio > 0)
                                        procesado = true;
                                }
                                else
                                    procesado = true;
                            }
                        }
                        else if (lst_content_docencia.Count == 1 && lst_content_docencia[0].Visible != (decimal)Constantes.activo.Activo)
                        {
                            /// 3.1.- Actualizar una entrada en campus_CONTENIDO_DOCENCIA en la BBDD
                            bool update_content = da.updateContentDocencia(lst_content_docencia[0], (int)Constantes.activo.Activo);
                            if (update_content)
                            {
                                /// 4.- Añadir una entrada en campus_ANUNCIO con el tipo_anuncio = contenido con la fecha actual.
                                string title = "Publicado nuevo contenido en ( " + list_courses[0].Nombre + " )";

                                /// 4.0.- Comprobar si hay una entrada igual en el día
                                List<campus_ANUNCIO> lst_anuncios = da.getAnunciosbyDocenciaAndCourse(tutoria.idDocencia, tutoria.idCurso);
                                lst_anuncios = lst_anuncios.Where(a => a.Titulo == title && a.Fecha == DateTime.Today).ToList();
                                if (lst_anuncios.Count == 0)
                                {
                                    /// 4.1.- Generar un objeto campus_ANUNCIO
                                    campus_ANUNCIO anuncio = new campus_ANUNCIO();
                                    anuncio.Fecha = DateTime.Today;
                                    anuncio.Titulo = title;
                                    anuncio.ID_Docencia_Grupo = tutoria.idDocencia;
                                    anuncio.Tipo_Anuncio = (int)Constantes.type_message.Contenido;
                                    anuncio.ID_Curso = tutoria.idCurso;
                                    anuncio.ID_Persona_Publica = tutoria.idTutor;
                                    anuncio.Fecha_Publica = DateTime.Now;

                                    /// 4.2.- Inserta en la BBDD el anuncio
                                    long insert_anuncio = da.insertAnuncio(anuncio);
                                    if (insert_anuncio > 0)
                                        procesado = true;
                                }
                                else
                                    procesado = true;
                            }
                        }
                    }
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_cp)
                {
                    /// 1.- Guardar los adjuntos
                    if (!String.IsNullOrEmpty(tarea.Adjuntos))
                    {
                        /// 1.1.- Guardar el fichero en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["route_cp"] + tutoria.idDocencia + "\\" + tutoria.idCurso + "\\";
                        if (!(Directory.Exists(ruta)))
                            Directory.CreateDirectory(ruta);

                        /// 1.2.- Poner el caso práctico
                        FileInfo archivo = new FileInfo(tarea.Adjuntos);
                        string name_file = archivo.Name;

                        /// 1.3.- Sacar las rutas
                        string ruta_origen = tarea.Adjuntos;
                        string ruta_destino = ruta + name_file;

                        /// 1.4.- Copiar el fichero
                        File.Copy(ruta_origen, ruta_destino, true);

                        /// 2.- Generar el objeto campus_REC_CP
                        campus_REC_CP cp = new campus_REC_CP();
                        cp.id_Docencia = tutoria.idDocencia;
                        cp.id_Curso = tutoria.idCurso;
                        cp.id_Profesor = tutoria.idTutor;
                        cp.cp_fecha = DateTime.Today;
                        cp.cp_fichero = name_file;
                        cp.cp_comentarios = tarea.Descripcion;
                        if (tarea.Num_Dias_Fecha_Limite == -1)
                            cp.cp_fecha_max = tutoria.FechaFin;
                        else
                            cp.cp_fecha_max = tutoria.FechaInicio.AddDays(tarea.Num_Dias_Fecha_Limite.Value);
                        cp.pendiente = true;

                        long insert_cp = da.insertRecCP(cp);
                        if (insert_cp > 0)
                            procesado = true;
                    }
                }
            }
            catch
            {
                procesado = false;
            }

            return procesado;
        }

        public bool process(campus_TUTORIA tutoria, campus_AUTO_CURSO tarea, campus_DOCENCIA_ALUMNO_AUTO user_auto)
        {
            bool procesado = false;

            try
            {
                if (tarea.Tipo == (int)Constantes.type_auto_curso.type_foro)
                {
                    List<campus_AUTO_USER> lst_auto_user = da.getAutoUser(user_auto.idDocencia, user_auto.idCurso);
                    lst_auto_user = lst_auto_user.Where(au => au.Id_Automatizacion == tarea.Id_Automatizacion).ToList();
                    if (lst_auto_user.Count == 0)
                    {
                        /// 1.- Generar el foro
                        campus_FORO forum = new campus_FORO();
                        forum.ID_Curso = user_auto.idCurso;
                        forum.ID_Docencia_Grupo = user_auto.idDocencia;
                        forum.Titulo = tarea.Asunto;
                        forum.Descripcion = tarea.Descripcion;
                        forum.FAlta = DateTime.Now;
                        forum.ID_Moderador = tutoria.idTutor;

                        /// 2.- Añadir el foro
                        long insert = da.insertForum(forum);
                        if (insert > 0)
                            procesado = true;
                    }
                    else
                        procesado = true;
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_msg_foro)
                {
                    List<campus_AUTO_USER> lst_auto_user = da.getAutoUser(user_auto.idDocencia, user_auto.idCurso);
                    lst_auto_user = lst_auto_user.Where(au => au.Id_Automatizacion == tarea.Id_Automatizacion).ToList();
                    if (lst_auto_user.Count == 0)
                    {
                        /// 1.- Obtener los datos del foro
                        List<campus_AUTO_CURSO> lst_auto = da.getAutoCourse(tarea.Id_Foro.Value);
                        List<campus_FORO> list_foro = da.getForo(user_auto.idCurso, user_auto.idDocencia, -1);
                        list_foro = list_foro.Where(f => f.Titulo == lst_auto[0].Asunto).ToList();

                        /// 1.- Guardar la pregunta
                        campus_FORO_MENSAJE _question = new campus_FORO_MENSAJE();
                        _question.ID_Foro = list_foro[0].ID_Foro;
                        _question.Titulo = tarea.Asunto;
                        _question.Mensaje = tarea.Descripcion;
                        _question.Fecha = DateTime.Now;
                        _question.ID_Persona = tutoria.idTutor;
                        _question.Borrador = ((int)Constantes.activo.NoActivo).ToString();

                        long insert_question = da.insertForumQuestion(_question);
                        if (insert_question > 0)
                        {
                            /// 2.- Guardar los adjuntos
                            string adjuntos = string.Empty;
                            if (!String.IsNullOrEmpty(tarea.Adjuntos))
                            {
                                /// 2.1.- Sacar los adjuntos
                                List<string> list_files = tarea.Adjuntos.Split(',').ToList();
                                if (list_files.Count > 0)
                                {
                                    /// 2.2.- Guardar el fichero en la carpeta correcta
                                    string ruta = ConfigurationManager.AppSettings["route_foro"] + user_auto.idDocencia + "\\" + user_auto.idCurso + "\\";
                                    if (!(Directory.Exists(ruta)))
                                        Directory.CreateDirectory(ruta);

                                    foreach (var _file in list_files)
                                    {
                                        /// 2.2.0.- Cambiar los nombres de los ficheros
                                        FileInfo _file_name = new FileInfo(_file);
                                        string _name_file = tutoria.idTutor + "_" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString() + _file_name.Extension;

                                        /// 2.2.1.- Sacar las rutas
                                        string ruta_origen = _file;
                                        string ruta_destino = ruta + _name_file;

                                        /// 2.2.2.- Copiar el fichero
                                        File.Copy(ruta_origen, ruta_destino, true);

                                        /// 2.2.3.- Guardar el adjunto en la BBDD
                                        campus_FORO_ADJUNTOS _adjunto = new campus_FORO_ADJUNTOS();
                                        _adjunto.Depende_De = 0;
                                        _adjunto.ID_Mensaje = insert_question;
                                        _adjunto.ID_Foro = list_foro[0].ID_Foro;
                                        _adjunto.Titulo = _file_name.Name;
                                        _adjunto.Adjunto_Fichero = _name_file;

                                        bool _save = da.insertAttachmentAnswer(_adjunto);
                                        if (_save)
                                            procesado = true;
                                    }
                                }
                            }
                            else
                                procesado = true;
                        }
                    }
                    else
                        procesado = true;
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_msg)
                {
                    string adjuntos = string.Empty;

                    List<campus_AUTO_USER> lst_auto_user = da.getAutoUser(user_auto.idDocencia, user_auto.idCurso);
                    lst_auto_user = lst_auto_user.Where(au => au.Id_Automatizacion == tarea.Id_Automatizacion).ToList();
                    if (lst_auto_user.Count == 0)
                    {
                        /// 1.- Guardar los adjuntos                        
                        if (!String.IsNullOrEmpty(tarea.Adjuntos))
                        {
                            List<string> list_files_routes = new List<string>();

                            /// 1.1.- Sacar los adjuntos
                            List<string> list_files = tarea.Adjuntos.Split(',').ToList();
                            if (list_files.Count > 0)
                            {
                                /// 1.2.- Guardar el fichero en la carpeta correcta
                                string ruta = ConfigurationManager.AppSettings["route_mail"] + tutoria.idTutor + "\\" + user_auto.idDocencia + "\\" + user_auto.idCurso + "\\";
                                if (!(Directory.Exists(ruta)))
                                    Directory.CreateDirectory(ruta);

                                foreach (var _file in list_files)
                                {
                                    FileInfo _file_name = new FileInfo(_file);

                                    /// 1.2.1.- Sacar las rutas
                                    string ruta_origen = _file;
                                    string ruta_destino = ruta + _file_name.Name;

                                    /// 1.2.2.- Copiar el fichero
                                    File.Copy(ruta_origen, ruta_destino, true);

                                    /// 1.2.3.- Guardar la ruta
                                    list_files_routes.Add(ruta_destino);
                                }
                            }

                            /// 1.3.- Devolver los adjuntos
                            int index = 0;
                            foreach (var route in list_files_routes)
                            {
                                if (index == 0)
                                    adjuntos = route;
                                else
                                    adjuntos += "," + route;
                                index++;
                            }
                        }
                    }
                    else
                    {
                        /// 1.- Guardar los adjuntos                        
                        if (!String.IsNullOrEmpty(tarea.Adjuntos))
                        {
                            List<string> list_files_routes = new List<string>();

                            /// 1.1.- Sacar los adjuntos
                            List<string> list_files = tarea.Adjuntos.Split(',').ToList();
                            if (list_files.Count > 0)
                            {
                                /// 1.2.- Guardar el fichero en la carpeta correcta
                                string ruta = ConfigurationManager.AppSettings["route_mail"] + tutoria.idTutor + "\\" + user_auto.idDocencia + "\\" + user_auto.idCurso + "\\";
                                foreach (var _file in list_files)
                                {
                                    FileInfo _file_name = new FileInfo(_file);

                                    /// 1.2.1.- Sacar las rutas
                                    string ruta_destino = ruta + _file_name.Name;

                                    /// 1.2.2.- Guardar la ruta
                                    list_files_routes.Add(ruta_destino);
                                }
                            }

                            /// 1.3.- Devolver los adjuntos
                            int index = 0;
                            foreach (var route in list_files_routes)
                            {
                                if (index == 0)
                                    adjuntos = route;
                                else
                                    adjuntos += "," + route;
                                index++;
                            }
                        }
                    }

                    /// 2.- Sacamos los datos de la BBDD
                    List<CLIENTES> lst_teacher = da.getUserById(tutoria.idTutor);
                    List<CLIENTES> lst_user = da.getUserById(user_auto.idUsuario);
                    List<campus_DOCENCIA> list_docencias = da.getDocenciaById(user_auto.idDocencia);
                    List<campus_CURSO> list_courses = da.getCourseById(user_auto.idCurso);
                    if (lst_user.Count == 1)
                    {
                        /// 3.- Guardar el mail

                        /// 3.1.- Cuerpo del mail
                        string template = Utilities.getPlantillaMail("campus-mensaje", ConfigurationManager.AppSettings["urlTemplate"]);
                        if (!String.IsNullOrEmpty(template))
                        {
                            template = template.Replace("###USER###", lst_teacher[0].Nombre_Completo + " [" + lst_teacher[0].email + "]");
                            template = template.Replace("###CURSO###", "CURSO: " + list_courses[0].Nombre + " perteneciente al programa " + list_docencias[0].Nombre + "<br />");
                            template = template.Replace("###MSG###", tarea.Descripcion.Replace("\n", "<br>"));
                            template = template.Replace("###IDC###", user_auto.idCurso.ToString());
                            template = template.Replace("###KEY###", lst_user[0].Key);
                            template = template.Replace("###BOTON###", string.Empty);
                        }

                        /// 3.2.- Añadir los datos de envío del mail
                        EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                        email_data.nombreTo = lst_user[0].Nombre_Completo;
                        email_data.mailTo = lst_user[0].email;
                        email_data.nombreFrom = lst_teacher[0].Nombre_Completo;
                        email_data.mailFrom = lst_teacher[0].email;
                        email_data.replyTo = lst_teacher[0].email;
                        email_data.priority = 1;
                        email_data.asunto = tarea.Asunto;
                        email_data.body = template;
                        if (!String.IsNullOrEmpty(adjuntos))
                            email_data.adjuntos = adjuntos;

                        /// 3.3.- Guardar el mail en la BBDD
                        long insert_mail = da.insertEmailContent(email_data);
                        if (insert_mail > 0)
                        {
                            /// 3.4.- Recuperar el mail
                            List<EMAIL_CONTENT> list_mail = da.getMailById(insert_mail);
                            if (list_mail.Count > 0)
                            {
                                template = list_mail[0].body.Replace("###IDM###", insert_mail.ToString());
                                bool update_mail = da.updateEmailContent(insert_mail, template);
                                if (!update_mail)
                                {
                                    LogUtils.InsertarLog(" ERROR - crear-mensaje.cs::save_mail()");
                                    LogUtils.InsertarLog("- MSG: Error al actualizar el mail.");
                                }

                                /// 4.- Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                long idLog = da.insertLog(lst_teacher[0].id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                if (idLog < 1)
                                {
                                    LogUtils.InsertarLog(" ERROR - crear-mensaje.cs::save_mail()");
                                    LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                                }

                                /// 5.- Guardar el anuncio
                                campus_ANUNCIO anuncio = new campus_ANUNCIO();
                                anuncio.Fecha = DateTime.Now;
                                anuncio.Titulo = tarea.Asunto;
                                anuncio.Cuerpo = template;
                                anuncio.Tipo_Anuncio = (long)Constantes.type_message.Email;
                                anuncio.ID_Persona = lst_user[0].id_cliente;
                                anuncio.ID_Docencia_Grupo = user_auto.idDocencia;
                                anuncio.ID_Curso = user_auto.idCurso;
                                anuncio.ID_Persona_Publica = lst_teacher[0].id_cliente;
                                anuncio.Fecha_Publica = DateTime.Now;
                                anuncio.Id_Mail = insert_mail;

                                long insert_anuncio = da.insertAnuncio(anuncio);
                                if (insert_anuncio > 0)
                                    procesado = true;
                                else
                                {
                                    bool delete_mail = da.deleteEmailContent(insert_mail);
                                    if (!delete_mail)
                                    {
                                        LogUtils.InsertarLog(" ERROR - crear-mensaje.cs::save_mail()");
                                        LogUtils.InsertarLog("- MSG: Error al eliminar el email del usuario.");
                                    }

                                    LogUtils.InsertarLog(" ERROR - crear-mensaje.cs::save_mail()");
                                    LogUtils.InsertarLog("- MSG: Error al guardar el anuncio mail del usuario.");
                                }
                            }
                        }
                    }
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_contenido)
                {
                    List<campus_AUTO_USER> lst_auto_user = da.getAutoUser(user_auto.idDocencia, user_auto.idCurso);
                    lst_auto_user = lst_auto_user.Where(au => au.Id_Automatizacion == tarea.Id_Automatizacion).ToList();
                    if (lst_auto_user.Count == 0)
                    {
                        /// 1.- Sacar el contenido curso
                        List<campus_CONTENIDO_CURSO> lst_content_course = da.getContentCourse(user_auto.idCurso);
                        List<campus_CURSO> list_courses = da.getCourseById(user_auto.idCurso);

                        /// 2.- Filtrar el contenido por el id_recurso
                        lst_content_course = lst_content_course.Where(cc => cc.ID_Recurso == tarea.Id_Recurso).ToList();
                        if (lst_content_course.Count == 1)
                        {
                            /// 2.1.- Comprobar si hay una entrada en campus_CONTENIDO_DOCENCIA
                            List<campus_CONTENIDO_DOCENCIA> lst_content_docencia = da.getContentDocencia(user_auto.idDocencia, user_auto.idCurso, null);
                            lst_content_docencia = lst_content_docencia.Where(cd => cd.ID_Recurso == tarea.Id_Recurso).ToList();
                            if (lst_content_docencia.Count == 0)
                            {
                                /// 3.- Generar un objeto campus_CONTENIDO_DOCENCIA
                                campus_CONTENIDO_DOCENCIA _content = new campus_CONTENIDO_DOCENCIA();
                                _content.ID_Docencia = user_auto.idDocencia;
                                _content.ID_Curso = user_auto.idCurso;
                                _content.ID_Recurso = tarea.Id_Recurso.Value;
                                _content.COD_Curso = lst_content_course[0].COD_Curso;
                                _content.COD_Recurso = lst_content_course[0].COD_Recurso;
                                _content.Sesion = lst_content_course[0].Sesion;
                                _content.Lectura = lst_content_course[0].Lectura;
                                _content.Fecha = DateTime.Today;
                                _content.Visible = (decimal)Constantes.activo.Activo;

                                /// 3.1.- Añadir una entrada en campus_CONTENIDO_DOCENCIA en la BBDD
                                long insert_content = da.insertContentDocencia(_content);
                                if (insert_content > 0)
                                {
                                    /// 4.- Añadir una entrada en campus_ANUNCIO con el tipo_anuncio = contenido con la fecha actual.
                                    string title = "Publicado nuevo contenido en ( " + list_courses[0].Nombre + " )";

                                    /// 4.0.- Comprobar si hay una entrada igual en el día
                                    List<campus_ANUNCIO> lst_anuncios = da.getAnunciosbyDocenciaAndCourse(user_auto.idDocencia, user_auto.idCurso);
                                    lst_anuncios = lst_anuncios.Where(a => a.Titulo == title && a.Fecha == DateTime.Today).ToList();

                                    if (lst_anuncios.Count == 0)
                                    {
                                        /// 4.1.- Generar un objeto campus_ANUNCIO
                                        campus_ANUNCIO anuncio = new campus_ANUNCIO();
                                        anuncio.Fecha = DateTime.Today;
                                        anuncio.Titulo = title;
                                        anuncio.ID_Docencia_Grupo = user_auto.idDocencia;
                                        anuncio.Tipo_Anuncio = (int)Constantes.type_message.Contenido;
                                        anuncio.ID_Curso = user_auto.idCurso;
                                        anuncio.ID_Persona_Publica = tutoria.idTutor;
                                        anuncio.ID_Persona = user_auto.idUsuario;
                                        anuncio.Fecha_Publica = DateTime.Now;

                                        /// 4.2.- Inserta en la BBDD el anuncio
                                        long insert_anuncio = da.insertAnuncio(anuncio);
                                        if (insert_anuncio > 0)
                                            procesado = true;
                                    }
                                    else
                                        procesado = true;
                                }
                            }
                            else
                            {
                                /// 1.- Añadir una entrada en campus_ANUNCIO con el tipo_anuncio = contenido con la fecha actual.
                                string title = "Publicado nuevo contenido en ( " + list_courses[0].Nombre + " )";

                                /// 1.0.- Comprobar si hay una entrada igual en el día
                                List<campus_ANUNCIO> lst_anuncios = da.getAnunciosbyDocenciaAndCourse(user_auto.idDocencia, user_auto.idCurso);
                                lst_anuncios = lst_anuncios.Where(a => a.Titulo == title && a.Fecha == DateTime.Today && a.ID_Persona == user_auto.idUsuario).ToList();

                                if (lst_anuncios.Count == 0)
                                {
                                    /// 1.1.- Generar un objeto campus_ANUNCIO
                                    campus_ANUNCIO anuncio = new campus_ANUNCIO();
                                    anuncio.Fecha = DateTime.Today;
                                    anuncio.Titulo = title;
                                    anuncio.ID_Docencia_Grupo = user_auto.idDocencia;
                                    anuncio.Tipo_Anuncio = (int)Constantes.type_message.Contenido;
                                    anuncio.ID_Curso = user_auto.idCurso;
                                    anuncio.ID_Persona_Publica = tutoria.idTutor;
                                    anuncio.ID_Persona = user_auto.idUsuario;
                                    anuncio.Fecha_Publica = DateTime.Now;

                                    /// 1.2.- Inserta en la BBDD el anuncio
                                    long insert_anuncio = da.insertAnuncio(anuncio);
                                    if (insert_anuncio > 0)
                                        procesado = true;
                                }
                                else
                                    procesado = true;
                            }
                        }
                    }
                    else
                    {
                        List<campus_CURSO> list_courses = da.getCourseById(user_auto.idCurso);

                        /// 1.- Añadir una entrada en campus_ANUNCIO con el tipo_anuncio = contenido con la fecha actual.
                        string title = "Publicado nuevo contenido en ( " + list_courses[0].Nombre + " )";

                        /// 1.0.- Comprobar si hay una entrada igual en el día
                        List<campus_ANUNCIO> lst_anuncios = da.getAnunciosbyDocenciaAndCourse(user_auto.idDocencia, user_auto.idCurso);
                        lst_anuncios = lst_anuncios.Where(a => a.Titulo == title && a.Fecha == DateTime.Today && a.ID_Persona == user_auto.idUsuario).ToList();

                        if (lst_anuncios.Count == 0)
                        {
                            /// 1.1.- Generar un objeto campus_ANUNCIO
                            campus_ANUNCIO anuncio = new campus_ANUNCIO();
                            anuncio.Fecha = DateTime.Today;
                            anuncio.Titulo = title;
                            anuncio.ID_Docencia_Grupo = user_auto.idDocencia;
                            anuncio.Tipo_Anuncio = (int)Constantes.type_message.Contenido;
                            anuncio.ID_Curso = user_auto.idCurso;
                            anuncio.ID_Persona_Publica = tutoria.idTutor;
                            anuncio.ID_Persona = user_auto.idUsuario;
                            anuncio.Fecha_Publica = DateTime.Now;

                            /// 1.2.- Inserta en la BBDD el anuncio
                            long insert_anuncio = da.insertAnuncio(anuncio);
                            if (insert_anuncio > 0)
                                procesado = true;
                        }
                        else
                            procesado = true;
                    }
                }
                else if (tarea.Tipo == (int)Constantes.type_auto_curso.type_cp)
                {
                    List<campus_AUTO_USER> lst_auto_user = da.getAutoUser(user_auto.idDocencia, user_auto.idCurso);
                    lst_auto_user = lst_auto_user.Where(au => au.Id_Automatizacion == tarea.Id_Automatizacion).ToList();
                    if (lst_auto_user.Count == 0)
                    {
                        /// 1.- Guardar los adjuntos
                        if (!String.IsNullOrEmpty(tarea.Adjuntos))
                        {
                            /// 1.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["route_cp"] + user_auto.idDocencia + "\\" + user_auto.idCurso + "\\";
                            if (!(Directory.Exists(ruta)))
                                Directory.CreateDirectory(ruta);

                            /// 1.2.- Poner el caso práctico
                            FileInfo archivo = new FileInfo(tarea.Adjuntos);
                            string name_file = archivo.Name;

                            /// 1.3.- Sacar las rutas
                            string ruta_origen = tarea.Adjuntos;
                            string ruta_destino = ruta + name_file;

                            /// 1.4.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 2.- Generar el objeto campus_REC_CP
                            campus_REC_CP cp = new campus_REC_CP();
                            cp.id_Docencia = user_auto.idDocencia;
                            cp.id_Curso = user_auto.idCurso;
                            cp.id_Profesor = tutoria.idTutor;
                            cp.cp_fecha = DateTime.Today;
                            cp.cp_fichero = name_file;
                            cp.cp_comentarios = tarea.Descripcion;
                            cp.cp_fecha_max = user_auto.Fecha_Inicio.AddDays(3000);
                            //cp.cp_fecha_max = tarea.Fecha_limite.Value;
                            cp.pendiente = false;

                            long insert_cp = da.insertRecCP(cp);
                            if (insert_cp > 0)
                            {
                                /// 1.- Añadir entrada en campus_REC_CP_TRABAJO
                                campus_REC_CP_TRABAJO cp_job = new campus_REC_CP_TRABAJO();
                                cp_job.idCP = insert_cp;
                                cp_job.id_alumno = user_auto.idUsuario;
                                cp_job.leido_enunciado = false;
                                cp_job.leido_feedback = false;

                                long insert_cp_job = da.insertRecCPTrabajo(cp_job);
                                if (insert_cp_job > 0)
                                {
                                    /// 2.- Sacamos los datos de la BBDD
                                    List<CLIENTES> lst_teacher = da.getUserById(tutoria.idTutor);
                                    List<CLIENTES> lst_user = da.getUserById(user_auto.idUsuario);
                                    List<campus_DOCENCIA> list_docencias = da.getDocenciaById(user_auto.idDocencia);
                                    List<campus_CURSO> list_courses = da.getCourseById(user_auto.idCurso);

                                    /// 2.1.- Obtener los datos del cuerpo     
                                    string template = Utilities.getPlantillaMail("admin-caso-practico", ConfigurationManager.AppSettings["urlTemplate"]);
                                    if (!String.IsNullOrEmpty(template))
                                    {
                                        template = template.Replace("###Curso###", list_courses[0].Nombre);
                                        template = template.Replace("###Docencia###", list_docencias[0].Nombre);

                                        template = template.Replace("###IDC###", user_auto.idCurso.ToString());
                                        template = template.Replace("###KEY###", lst_user[0].Key);
                                    }

                                    /// 2.2.- Añadir datos de envío de mail
                                    EMAIL_CONTENT mail = new EMAIL_CONTENT();
                                    mail.nombreTo = lst_user[0].Nombre_Completo;
                                    mail.mailTo = lst_user[0].email;
                                    mail.priority = 1;
                                    mail.nombreFrom = lst_teacher[0].Nombre_Completo;
                                    mail.mailFrom = lst_teacher[0].email;
                                    mail.replyTo = lst_teacher[0].email;
                                    mail.asunto = "Spain Business School informa: Disponible el caso práctico de " + (list_courses.Count == 1 ? list_courses[0].Nombre : user_auto.idCurso.ToString());
                                    mail.body = template;

                                    long insert_mail = da.insertEmailContent(mail);
                                    if (insert_mail > 0)
                                        procesado = true;
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - Caso_Practico.cs::insertRecCPTrabajo()", true);
                                        LogUtils.InsertarLog("- MSG: Error al guardar el mail del usuario: " + lst_user[0].id_cliente + ".", true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        List<campus_REC_CP> lst_cp = da.getPracticalCases(user_auto.idCurso, user_auto.idDocencia);
                        if (lst_cp.Count > 0)
                        {
                            foreach (var caso_practico in lst_cp)
                            {
                                /// 1.- Añadir entrada en campus_REC_CP_TRABAJO
                                campus_REC_CP_TRABAJO cp_job = new campus_REC_CP_TRABAJO();
                                cp_job.idCP = caso_practico.idCP;
                                cp_job.id_alumno = user_auto.idUsuario;
                                cp_job.leido_enunciado = false;
                                cp_job.leido_feedback = false;

                                long insert_cp_job = da.insertRecCPTrabajo(cp_job);
                                if (insert_cp_job > 0)
                                {
                                    /// 2.- Sacamos los datos de la BBDD
                                    List<CLIENTES> lst_teacher = da.getUserById(tutoria.idTutor);
                                    List<CLIENTES> lst_user = da.getUserById(user_auto.idUsuario);
                                    List<campus_DOCENCIA> list_docencias = da.getDocenciaById(user_auto.idDocencia);
                                    List<campus_CURSO> list_courses = da.getCourseById(user_auto.idCurso);

                                    /// 2.1.- Obtener los datos del cuerpo     
                                    string template = Utilities.getPlantillaMail("admin-caso-practico", ConfigurationManager.AppSettings["urlTemplate"]);
                                    if (!String.IsNullOrEmpty(template))
                                    {
                                        template = template.Replace("###Curso###", list_courses[0].Nombre);
                                        template = template.Replace("###Docencia###", list_docencias[0].Nombre);

                                        template = template.Replace("###IDC###", caso_practico.id_Curso.ToString());
                                        template = template.Replace("###KEY###", lst_user[0].Key);
                                    }

                                    /// 2.2.- Añadir datos de envío de mail
                                    EMAIL_CONTENT mail = new EMAIL_CONTENT();
                                    mail.nombreTo = lst_user[0].Nombre_Completo;
                                    mail.mailTo = lst_user[0].email;
                                    mail.priority = 1;
                                    mail.nombreFrom = lst_teacher[0].Nombre_Completo;
                                    mail.mailFrom = lst_teacher[0].email;
                                    mail.replyTo = lst_teacher[0].email;
                                    mail.asunto = "Spain Business School informa: Disponible el caso práctico de " + (list_courses.Count == 1 ? list_courses[0].Nombre : caso_practico.id_Curso.ToString());
                                    mail.body = template;

                                    long insert_mail = da.insertEmailContent(mail);
                                    if (insert_mail > 0)
                                        procesado = true;
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - Auto_Curso.cs::insertEmailContent()", true);
                                        LogUtils.InsertarLog("- MSG: Error al guardar el mail del usuario: " + lst_user[0].id_cliente + ".", true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                procesado = false;
                LogUtils.InsertarLog(" ERROR - " + ex.Message, true);
            }

            return procesado;
        }
    }
}
