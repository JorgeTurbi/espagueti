using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sbs_DAL;
using System.Configuration;

namespace Solicitud_opinion_process
{
    class Anuncio
    {
        DataAccess da = new DataAccess();

        public bool process(campus_ANUNCIO anuncio)
        {
            bool procesado = true;

            /// 0.- Sacar los datos
            List<campus_DOCENCIA_GRUPO> lst_doc_group = da.getDocenciaGrupoByDocencia(anuncio.ID_Docencia_Grupo.Value);
            if (lst_doc_group.Count > 0)
            {
                /// 1.- Sacar los datos de la docencia, curso y profesor
                List<campus_DOCENCIA> list_docencias = da.getDocenciaById(anuncio.ID_Docencia_Grupo.Value);
                List<campus_CURSO> list_courses = da.getCourseById(anuncio.ID_Curso.Value);
                List<CLIENTES> lst_teacher = da.getUserById(anuncio.ID_Persona_Publica);

                /// 2.- Comprobar si va a un grupo
                List<long> _users = new List<long>();
                if (anuncio.ID_Docencia_Subgrupo != null)
                {
                    List<campus_DOCENCIA_SUBGRUPO> _grupos = da.getDocenciasSubGrupoByIdG(anuncio.ID_Docencia_Subgrupo);
                    if (_grupos.Count > 0)
                        _users = _grupos.Select(_ => _.ID_Persona).Distinct().ToList();
                }

                /// 3.- Sacar los usuarios de la docencia
                List<CLIENTES> _clients = new List<CLIENTES>();
                if (_users.Count > 0)
                    _clients = da.getUserByList(_users);
                else
                    _clients = da.getUserByList(lst_doc_group.Select(_ => _.ID_Persona).Distinct().ToList());

                /// 3.1.- Sacar sólo los alumnos
                _clients = _clients.Where(_ => (String.IsNullOrEmpty(_.Profesor) || _.Profesor == ((int)Constantes.activo.NoActivo).ToString()) && (String.IsNullOrEmpty(_.Administrador) || _.Administrador == ((int)Constantes.activo.NoActivo).ToString())).ToList();

                /// 3.2.- Sacar sólo los activos
                _clients = _clients.Where(_ => _.activo == ((int)Constantes.activo.Activo).ToString()).ToList();
                
                if (_clients.Count > 0 && list_docencias.Count == 1)
                {
                    foreach (var student in _clients)
                    {
                        bool student_active = Utilities.comprobar_alumno(lst_doc_group, list_docencias[0], anuncio.ID_Docencia_Grupo.Value, student.id_cliente);
                        if (student_active)
                        {
                            /// 3.2.- Generar la entrada en el envío de mails

                            /// 3.2.1.- Cuerpo del mail
                            string template = Utilities.getPlantillaMail("campus-mensaje", ConfigurationManager.AppSettings["urlTemplate"]);
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###USER###", lst_teacher[0].Nombre_Completo + " [" + lst_teacher[0].email + "]");
                                if (anuncio.ID_Docencia_Grupo.Value > 0 && anuncio.ID_Curso.Value > 0)
                                    template = template.Replace("###CURSO###", "CURSO: " + list_courses[0].Nombre + " perteneciente al programa " + list_docencias[0].Nombre + "<br />");
                                else if (anuncio.ID_Docencia_Grupo.Value > 0)
                                    template = template.Replace("###CURSO###", "Programa " + list_docencias[0].Nombre + "<br />");
                                else
                                    template = template.Replace("###CURSO###", string.Empty + "<br />");
                                template = template.Replace("###MSG###", anuncio.Cuerpo.Replace("\n", "<br>"));

                                if (anuncio.ID_Curso.Value > 0)
                                    template = template.Replace("###IDC###", anuncio.ID_Curso.Value.ToString());
                                else
                                    template = template.Replace("###IDC###", ((int)Constantes.course.Sin_determinar).ToString());
                                template = template.Replace("###KEY###", student.Key);
                                template = template.Replace("###BOTON###", string.Empty);
                            }

                            /// 3.2.2.- Añadir los datos de envío del mail
                            EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                            email_data.nombreTo = student.Nombre_Completo;
                            email_data.mailTo = student.email;
                            email_data.nombreFrom = lst_teacher[0].Nombre_Completo;
                            email_data.mailFrom = lst_teacher[0].email;
                            email_data.replyTo = lst_teacher[0].email;
                            email_data.priority = 1;
                            email_data.asunto = anuncio.Titulo;
                            email_data.body = template;
                            if (!String.IsNullOrEmpty(anuncio.Adjuntos))
                                email_data.adjuntos = anuncio.Adjuntos;

                            /// 3.2.3.- Guardar el mail en la BBDD
                            long insert_mail = da.insertEmailContent(email_data);
                            if (insert_mail > 0)
                            {
                                /// 4.- Recuperar el mail
                                List<EMAIL_CONTENT> list_mail = da.getMailById(insert_mail);
                                if (list_mail.Count > 0)
                                {
                                    template = list_mail[0].body.Replace("###IDM###", insert_mail.ToString());
                                    bool update_mail = da.updateEmailContent(insert_mail, template);
                                    if (!update_mail)
                                    {
                                        LogUtils.InsertarLog(" ERROR - anuncio.cs::updateEmailContent()", true);
                                        LogUtils.InsertarLog("- MSG: Error al actualizar el mail.", true);
                                    }

                                    /// 7.- Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                    long idLog = da.insertLog(lst_teacher[0].id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                    if (idLog < 1)
                                    {
                                        LogUtils.InsertarLog(" ERROR - anuncio.cs::insertLog()", true);
                                        LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.", true);
                                    }

                                    /// 8.- Guardar el anuncio
                                    campus_ANUNCIO _anuncio = new campus_ANUNCIO();
                                    _anuncio.Fecha = DateTime.Now;
                                    _anuncio.Titulo = anuncio.Titulo;
                                    _anuncio.Cuerpo = template;
                                    _anuncio.Tipo_Anuncio = (long)Constantes.type_message.Email;
                                    _anuncio.ID_Persona = student.id_cliente;
                                    if (anuncio.ID_Docencia_Grupo.Value > 0)
                                        _anuncio.ID_Docencia_Grupo = anuncio.ID_Docencia_Grupo.Value;
                                    _anuncio.ID_Docencia_Subgrupo = anuncio.ID_Docencia_Subgrupo;
                                    if (anuncio.ID_Curso.Value > 0)
                                        _anuncio.ID_Curso = anuncio.ID_Curso.Value;
                                    _anuncio.ID_Persona_Publica = lst_teacher[0].id_cliente;
                                    _anuncio.Fecha_Publica = DateTime.Now;
                                    _anuncio.Id_Mail = insert_mail;

                                    long insert_anuncio = da.insertAnuncio(_anuncio);
                                    if (insert_anuncio < 1)
                                    {
                                        LogUtils.InsertarLog(" ERROR - anuncio.cs::insertAnuncio()", true);
                                        LogUtils.InsertarLog("- MSG: Error al guardar el anuncio.", true);
                                    }
                                    else
                                        Console.WriteLine("Email del usuario " + student.Nombre_Completo + " procesado");
                                }
                            }
                            else
                            {
                                LogUtils.InsertarLog(" ERROR - anuncio.cs::save_mail()", true);
                                LogUtils.InsertarLog("- MSG: Error al guardar el mail del usuario: " + student.id_cliente + ".", true);
                            }
                        }
                    }
                }
            }

            return procesado;
        }
    }
}