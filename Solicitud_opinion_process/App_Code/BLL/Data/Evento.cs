using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solicitud_opinion_process
{
    class Evento
    {
        DataAccess da = new DataAccess();

        public bool process(campus_AGENDA evento)
        {
            bool procesado = true;

            /// 1.- Sacar los datos
            List<campus_DOCENCIA_GRUPO> list_docencias_grupo = da.getDocenciasGrupo(null);
            List<campus_DOCENCIA> list_docencias = da.getDocenciaById(-1);
            List<CLIENTES> list_clients = da.getActiveUsers(true);

            /// 2.- Sacar a los alumnos que pertenecen a la docencia
            List<campus_DOCENCIA_GRUPO> lst_doc_group = list_docencias_grupo.Where(dg => dg.ID_Docencia == evento.ID_Docencia_Grupo.Value).ToList();
            List<CLIENTES> lst_clients = list_clients.Where(c => lst_doc_group.Select(dg => dg.ID_Persona).Distinct().ToList().Contains(c.id_cliente)).ToList();
            lst_clients = lst_clients.Where(c => (String.IsNullOrEmpty(c.Profesor) || c.Profesor == ((int)Constantes.activo.NoActivo).ToString()) && (String.IsNullOrEmpty(c.Administrador) || c.Administrador == ((int)Constantes.activo.NoActivo).ToString())).ToList();

            /// 3.- Sacar las docencias activas
            List<campus_DOCENCIA> list_docencias_actives = list_docencias.Where(d => (d.FInicio == null || d.FInicio <= DateTime.Today) && (d.FFin == null || d.FFin >= DateTime.Today)).OrderBy(d => d.Nombre).ToList();
            List<campus_DOCENCIA> lst_docencias = list_docencias.Where(d => d.ID_Docencia == evento.ID_Docencia_Grupo.Value).ToList();

            /// 4.- Recorrer a los alumnos
            if (lst_clients.Count > 0 && lst_docencias.Count == 1)
            {
                foreach (var student in lst_clients)
                {
                    /// 4.1.- Comprobar si es un alumno activo
                    bool student_active = Utilities.comprobar_alumno(lst_doc_group, lst_docencias[0], evento.ID_Docencia_Grupo.Value, student.id_cliente);
                    if (student_active)
                    {
                        /// 4.2.- Añadir una entrada en la tabla campus_clase_asistencia
                        campus_CLASE_ASISTENCIA clase_asistencia = new campus_CLASE_ASISTENCIA();
                        clase_asistencia.fecha = evento.Fecha;
                        clase_asistencia.idAlumno = student.id_cliente;
                        clase_asistencia.titulo_clase = evento.Titulo;
                        clase_asistencia.idDocenciaGlobal = evento.ID_Docencia_Grupo.Value;

                        /// 4.3.- Sacar las docencias activas
                        List<campus_DOCENCIA> list_docencias_actives_user = list_docencias_actives.Where(d => list_docencias_grupo.Where(dg => dg.ID_Persona == student.id_cliente).Select(dg => dg.ID_Docencia).Distinct().ToList().Contains(d.ID_Docencia) && d.idAsociado == null && d.ID_Docencia != evento.ID_Docencia_Grupo.Value).ToList().OrderBy(d => d.ID_Docencia).ToList();
                        if (list_docencias_actives_user.Count > 0)
                            clase_asistencia.idDocencia = list_docencias_actives_user[0].ID_Docencia;

                        clase_asistencia.tipo_clase = (int)Constantes.type_clase.Clase_presencial;

                        /// 4.4.- Insertar la clase
                        long insert_class = da.insertClaseAsistencia(clase_asistencia);
                        if (insert_class > 0)
                        {
                            /// 5.- Generar la entrada en el envío de mails
                            
                            /// 5.1.- Cuerpo del mail
                            string template = Utilities.getPlantillaMail("crear-evento-clase", ConfigurationManager.AppSettings["urlTemplate"]);
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###USER###", student.Nombre_Completo);
                                template = template.Replace("###FECHA###", evento.Fecha.ToShortDateString());
                                template = template.Replace("###TITULO###", evento.Titulo);

                                if (evento.idCurso != null && evento.idCurso.Value > 0)
                                    template = template.Replace("###IDC###", evento.idCurso.Value.ToString());
                                else
                                    template = template.Replace("###IDC###", ((int)Constantes.course.Sin_determinar).ToString());
                                template = template.Replace("###KEY###", student.Key);

                                template = template.Replace("###BOTON###", "<a target='_blank' style='text-decoration: none; display: block; font-family: \"Arial Black\", Gadget, sans-serif; font-size: 14px; color: #fff; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px; text-align: center; background-color: green; border: 3px solid green; padding: 10px; border-radius: 3px; -moz-border-radius: 3px; -webkit-border-radius: 3px;' href='https://campus.spainbs.com/encuesta-especial.aspx?idca=" + insert_class + "&idq=1&idrp=1&k=" + student.Key + "'>Sí, asistiré</a>");
                                template = template.Replace("###BOTON2###", "<a target='_blank' style='text-decoration: none; display: block; font-family: \"Arial Black\", Gadget, sans-serif; font-size: 14px; color: #fff; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px; text-align: center; background-color: red; border: 3px solid red; padding: 10px; border-radius: 3px; -moz-border-radius: 3px; -webkit-border-radius: 3px;' href='https://campus.spainbs.com/encuesta-especial.aspx?idca=" + insert_class + "&idq=1&idrp=2&k=" + student.Key + "'>No podré asistir</a>");
                                template = template.Replace("###BOTON3###", "<a target='_blank' style='text-decoration: none; display: block; font-family: \"Arial Black\", Gadget, sans-serif; font-size: 14px; color: #fff; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px; text-align: center; background-color: orange; border: 3px solid orange; padding: 10px; border-radius: 3px; -moz-border-radius: 3px; -webkit-border-radius: 3px;' href='https://campus.spainbs.com/encuesta-especial.aspx?idca=" + insert_class + "&idq=1&idrp=3&k=" + student.Key + "'>Aún no lo sé</a>");
                            }

                            /// 5.2.- Añadir los datos de envío del mail
                            EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                            email_data.nombreTo = student.Nombre_Completo;
                            email_data.mailTo = student.email;
                            email_data.priority = 1;
                            email_data.asunto = "Spain Business School informa: Confirmación de asistencia";
                            email_data.body = template;
                            email_data.date_schedule = (evento.Fecha.AddDays(-4) >= DateTime.Today ? evento.Fecha.AddDays(-4) : DateTime.Today);

                            /// 5.3.- Guardar el mail en la BBDD
                            long insert_mail = da.insertEmailContent(email_data);
                            if (insert_mail > 0)
                            {
                                /// 5.4.- Recuperar el mail
                                List<EMAIL_CONTENT> list_mail = da.getMailById(insert_mail);
                                if (list_mail.Count > 0)
                                {
                                    template = list_mail[0].body.Replace("###IDM###", insert_mail.ToString());
                                    bool update_mail = da.updateEmailContent(insert_mail, template);
                                    if (!update_mail)
                                    {
                                        LogUtils.InsertarLog(" ERROR - crear-mensaje.cs::save_mail()", true);
                                        LogUtils.InsertarLog("- MSG: Error al actualizar el mail.", true);
                                    }
                                    else
                                        Console.WriteLine("Confirmación de asistencia del usuario " + student.Nombre_Completo + " procesada");
                                }
                            }
                            else
                            {
                                //procesado = false;
                                LogUtils.InsertarLog(" ERROR - Evento.cs::save_mail()", true);
                                LogUtils.InsertarLog("- MSG: Error al guardar el mail del usuario: " + student.id_cliente + ".", true);
                            }
                        }
                        else
                        {
                            //procesado = false;
                            LogUtils.InsertarLog(" ERROR - Evento.cs::insertClaseAsistencia()", true);
                            LogUtils.InsertarLog("- MSG: Se ha producido un error al generar la asistencia a la clase del usuario: " + student.id_cliente + ".", true);
                        }
                    }
                }
            }

            return procesado;
        }
    }
}