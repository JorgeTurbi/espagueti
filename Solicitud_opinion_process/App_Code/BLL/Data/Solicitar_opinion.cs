using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sbs_DAL;
using System.Configuration;

namespace Solicitud_opinion_process
{
    class Solicitar_opinion
    {
        DataAccess da = new DataAccess();

        public bool process(campus_OPINIONES_SOLICITUD solicitud)
        {
            bool procesado = true;

            /// 1.- Sacar los datos
            List<campus_DOCENCIA_GRUPO> lst_doc_group = da.getDocenciaGrupoByDocencia(solicitud.id_docencia);
            if (lst_doc_group.Count > 0)
            {
                /// 2.- Sacar los datos de la docencia
                List<campus_DOCENCIA> list_docencias = da.getDocenciaById(solicitud.id_docencia);

                /// 3.- Sacar los usuarios de la docencia
                List<CLIENTES> lst_clients = da.getUserByList(lst_doc_group.Select(c => c.ID_Persona).Distinct().ToList());

                /// 3.1.- Sacar sólo los alumnos
                lst_clients = lst_clients.Where(c => (String.IsNullOrEmpty(c.Profesor) || c.Profesor == ((int)Constantes.activo.NoActivo).ToString()) && (String.IsNullOrEmpty(c.Administrador) || c.Administrador == ((int)Constantes.activo.NoActivo).ToString())).ToList();

                /// 3.2.- Sacar sólo los alumnos activos
                lst_clients = lst_clients.Where(c => c.activo == ((int)Constantes.activo.Activo).ToString()).ToList();
                if (lst_clients.Count > 0 && list_docencias.Count == 1)
                {
                    foreach (var student in lst_clients)
                    {
                        bool student_active = Utilities.comprobar_alumno(lst_doc_group, list_docencias[0], solicitud.id_docencia, student.id_cliente);
                        if (student_active)
                        {
                            /// 3.2.- Generar la entrada en el envío de mails

                            /// 3.2.1.- Cuerpo del mail
                            string template = Utilities.getPlantillaMail("opinion", ConfigurationManager.AppSettings["urlTemplate"]);
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###CUERPO###", solicitud.body);

                                template = template.Replace("###NOMBRE###", student.Nombre_Completo);
                                template = template.Replace("###IDC###", ((int)Constantes.course.Sin_determinar).ToString());
                                template = template.Replace("###KEY###", student.Key);

                                template = template.Replace("###BOTON###", "<a target='_blank' style='text-decoration: none; display: block; font-family: \"Arial Black\", Gadget, sans-serif; font-size: 14px; color: #fff; padding-top: 10px; padding-right: 10px; padding-bottom: 10px; padding-left: 10px; text-align: center; background-color: #edab3a; border: 3px solid #edab3a; padding: 10px; border-radius: 3px; -moz-border-radius: 3px; -webkit-border-radius: 3px;' href='https://www.spainbs.com/tu-opinion-importa.aspx?k=" + student.Key + "&idd=" + solicitud.id_docencia + "&tp=" + solicitud.id_tso + "&idso=" + solicitud.id_solicitud_opinion + "'>QUIERO OPINAR</a>");
                            }

                            /// 3.2.2.- Añadir los datos de envío del mail
                            EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                            email_data.nombreTo = student.Nombre_Completo;
                            email_data.mailTo = student.email;
                            email_data.nombreFrom = solicitud.nombreFrom;
                            email_data.mailFrom = solicitud.mailFrom;
                            email_data.replyTo = solicitud.replyTo;
                            email_data.priority = 1;
                            email_data.asunto = solicitud.asunto;
                            email_data.body = template;
                            email_data.id_os = solicitud.id_solicitud_opinion;

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
                                        //procesado = false;
                                        LogUtils.InsertarLog(" ERROR - solicitar-opinion.cs::updateEmailContent()", true);
                                        LogUtils.InsertarLog("- MSG: Error al actualizar el mail.", true);
                                    }
                                    else
                                        Console.WriteLine("Opinión del usuario " + student.Nombre_Completo + " procesado");
                                }
                            }
                            else
                            {
                                //procesado = false;
                                LogUtils.InsertarLog(" ERROR - solicitar-opinion.cs::save_mail()", true);
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
