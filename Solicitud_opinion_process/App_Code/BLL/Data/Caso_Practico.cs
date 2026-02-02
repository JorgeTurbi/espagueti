using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sbs_DAL;
using System.Configuration;

namespace Solicitud_opinion_process
{
    class Caso_Practico
    {
        DataAccess da = new DataAccess();

        public bool process(campus_REC_CP caso_practico)
        {
            bool procesado = true;

            /// 1.- Sacar los datos
            List<campus_DOCENCIA_GRUPO> lst_doc_group = da.getDocenciaGrupoByDocencia(caso_practico.id_Docencia);
            if (lst_doc_group.Count > 0)
            {
                /// 2.- Sacar los datos de la docencia, curso y profesor
                List<campus_DOCENCIA> list_docencias = da.getDocenciaById(caso_practico.id_Docencia);
                List<campus_CURSO> list_courses = da.getCourseById(caso_practico.id_Curso);
                List<CLIENTES> lst_teacher = da.getUserById(caso_practico.id_Profesor);
                List<campus_TUTORIA> lst_tutorias = da.getTutorias(caso_practico.id_Curso);
                lst_tutorias = lst_tutorias.Where(t => t.idDocencia == caso_practico.id_Docencia).ToList();

                /// 3.- Sacar los usuarios de la docencia
                List<CLIENTES> lst_clients = da.getUserByList(lst_doc_group.Select(c => c.ID_Persona).Distinct().ToList());
                
                /// 3.1.- Sacar sólo los alumnos
                lst_clients = lst_clients.Where(c => (String.IsNullOrEmpty(c.Profesor) || c.Profesor == ((int)Constantes.activo.NoActivo).ToString()) && (String.IsNullOrEmpty(c.Administrador) || c.Administrador == ((int)Constantes.activo.NoActivo).ToString())).ToList();

                /// 3.2.- Sacar sólo los activos
                lst_clients = lst_clients.Where(c => c.activo == ((int)Constantes.activo.Activo).ToString()).ToList();
                if (lst_clients.Count > 0 && list_docencias.Count == 1)
                {
                    foreach (var student in lst_clients)
                    {
                        /// 4.- Comprobar si el alumno está activo
                        bool student_active = Utilities.comprobar_alumno(lst_doc_group, list_docencias[0], caso_practico.id_Docencia, student.id_cliente);
                        if (student_active)
                        {
                            /// 5.- Añadir entrada en campus_REC_CP_TRABAJO
                            campus_REC_CP_TRABAJO cp_job = new campus_REC_CP_TRABAJO();
                            cp_job.idCP = caso_practico.idCP;
                            cp_job.id_alumno = student.id_cliente;
                            cp_job.leido_enunciado = false;
                            cp_job.leido_feedback = false;

                            long insert_cp_job = da.insertRecCPTrabajo(cp_job);
                            if (insert_cp_job > 0)
                            {
                                /// 6.1.- Obtener los datos del cuerpo     
                                string template = Utilities.getPlantillaMail("admin-caso-practico", ConfigurationManager.AppSettings["urlTemplate"]);
                                if (!String.IsNullOrEmpty(template))
                                {
                                    if (list_courses.Count == 1)
                                        template = template.Replace("###Curso###", list_courses[0].Nombre);
                                    else
                                        template = template.Replace("###Curso###", caso_practico.id_Curso.ToString());
                                    if (list_docencias.Count == 1)
                                        template = template.Replace("###Docencia###", list_docencias[0].Nombre);
                                    else
                                        template = template.Replace("###Docencia###", caso_practico.id_Docencia.ToString());

                                    template = template.Replace("###IDC###", caso_practico.id_Curso.ToString());
                                    template = template.Replace("###KEY###", student.Key);
                                }

                                /// 6.2.- Añadir datos de envío de mail
                                EMAIL_CONTENT mail = new EMAIL_CONTENT();
                                mail.nombreTo = student.Nombre_Completo;
                                mail.mailTo = student.email;
                                mail.priority = 1;
                                mail.nombreFrom = lst_teacher[0].Nombre_Completo;
                                mail.mailFrom = lst_teacher[0].email;
                                mail.replyTo = lst_teacher[0].email;
                                mail.asunto = "Spain Business School informa: Disponible el caso práctico de " + (list_courses.Count == 1 ? list_courses[0].Nombre : caso_practico.id_Curso.ToString());
                                mail.body = template;

                                long insert_mail = da.insertEmailContent(mail);
                                if (insert_mail < 0)
                                {
                                    //procesado = false;
                                    LogUtils.InsertarLog(" ERROR - Caso_Practico.cs::insertRecCPTrabajo()", true);
                                    LogUtils.InsertarLog("- MSG: Error al guardar el mail del usuario: " + student.id_cliente + ".", true);
                                }
                                else
                                    Console.WriteLine("Caso práctico del usuario " + student.Nombre_Completo + " procesado");
                            }
                            else
                            {
                                //procesado = false;
                                LogUtils.InsertarLog(" ERROR - Caso_Practico.cs::save_mail()", true);
                                LogUtils.InsertarLog("- MSG: Error al añadir la entrada en campus_REC_CP_TRABAJO del usuario: " + student.id_cliente + ".", true);
                            }
                        }
                    }

                    /// 7.- Añadir entrada en la agenda
                    campus_AGENDA agenda = new campus_AGENDA();
                    agenda.Fecha = caso_practico.cp_fecha_max;
                    agenda.Titulo = "Fecha límite para la entrega del caso práctico de " + list_courses[0].Nombre;
                    agenda.ID_Docencia_Grupo = caso_practico.id_Docencia;
                    agenda.ID_Persona_Publica = caso_practico.id_Profesor;
                    agenda.Fecha_Publica = DateTime.Now;
                    agenda.idCurso = caso_practico.id_Curso;
                    agenda.idTipo = (int)Constantes.type_agenda.Fecha_entrega;
                    agenda.All_Day = true;

                    /// 7.1.- Añadir entrada en la BBDD
                    long insert_agenda = da.insertEvent(agenda);
                    if (insert_agenda < 0)
                    {
                        //procesado = false;
                        LogUtils.InsertarLog(" ERROR - Caso_Practico.cs::insertEvent()", true);
                        LogUtils.InsertarLog("- MSG: Error al añadir la entrada en campus_AGENDA", true);
                    }
                }
            }

            return procesado;
        }
    }
}
