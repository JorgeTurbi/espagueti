using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solicitud_opinion_process
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccess da = new DataAccess();

            try
            {
                Console.WriteLine("Inicio del proceso");
                LogUtils.InsertarLog("Inicio del proceso", true);

                /// 1.- Comprobar si hay solicitudes de opinión programadas
                List<campus_OPINIONES_SOLICITUD> lst_solicitudes = da.getSolicitudOpinion(false);
                if (lst_solicitudes.Count > 0)
                {
                    foreach (var solicitud in lst_solicitudes)
                    {
                        Solicitar_opinion solicitar = new Solicitar_opinion();
                        bool process = solicitar.process(solicitud);
                        if (process)
                        {
                            /// 1.1.- Cambiar la solicitud a procesada
                            List<campus_OPINIONES_SOLICITUD> list_solicitudes = da.getSolicitudOpinionById(solicitud.id_solicitud_opinion);
                            if (list_solicitudes.Count == 1)
                            {
                                campus_OPINIONES_SOLICITUD _solicitud = list_solicitudes[0];
                                bool update_solicitud = da.updateSolicitudOpinionProcess(_solicitud, true);
                                if (!update_solicitud)
                                    LogUtils.InsertarLog(" ERROR - Al procesar la solicitud en campus_OPINIONES_SOLICITUD", true);
                            }
                        }
                    }
                }

                /// 2.- Comprobar si hay eventos programados
                List<campus_AGENDA> lst_events = da.getEventsProcess(true);
                if (lst_events.Count > 0)
                {
                    foreach (var evento in lst_events)
                    {
                        Evento _evento = new Evento();
                        bool process = _evento.process(evento);
                        if (process)
                        {
                            /// 1.1.- Cambiar la solicitud a procesada
                            List<campus_AGENDA> list_eventos = da.getEventById(evento.ID_Agenda);
                            if (list_eventos.Count == 1)
                            {
                                campus_AGENDA _event = list_eventos[0];
                                bool update_event = da.updateEventProcess(_event, false);
                                if (!update_event)
                                    LogUtils.InsertarLog(" ERROR - Al procesar la solicitud en campus_AGENDA", true);
                            }
                        }
                    }
                }

                /// 3.- Comprobar si hay curso automatizados
                List<campus_TUTORIA> lst_tutorias = da.getTutoriasAuto();
                if (lst_tutorias.Count > 0)
                {
                    foreach (var tutoria in lst_tutorias)
                    {
                        int num_horas = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["num_horas"]) ? int.Parse(ConfigurationManager.AppSettings["num_horas"]) : 0;
                        int num_minutos = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["num_minutos"]) ? int.Parse(ConfigurationManager.AppSettings["num_minutos"]) : 0;

                        List <campus_DOCENCIA> lst_docencia = da.getDocenciaById(tutoria.idDocencia);
                        if (lst_docencia.Count == 1)
                        {
                            if (lst_docencia[0].PDP != null && lst_docencia[0].PDP.Value)
                            {
                                /// 3.0.- Sacamos los cursos abiertos por el usuario
                                List<campus_DOCENCIA_ALUMNO_AUTO> lst_doc_auto = da.getDocenciaAlumnoAuto(tutoria.idDocencia, tutoria.idCurso);

                                /// 3.1.- Filtrar los que no han terminado
                                lst_doc_auto = lst_doc_auto.Where(auto => auto.Fecha_Fin >= DateTime.Today).ToList();
                                if (lst_doc_auto.Count > 0)
                                {
                                    foreach (var user_auto in lst_doc_auto)
                                    {
                                        DateTime fecha_apertura = user_auto.Fecha_Inicio.AddHours(num_horas).AddMinutes(num_minutos);
                                        if (DateTime.Now >= fecha_apertura)
                                        {
                                            /// 3.2.- Sacar datos de la BBDD
                                            List<campus_AUTO_CURSO> lst_auto = da.getAutoCourseByCourse(user_auto.idCurso);
                                            List<campus_AUTO_USER> lst_auto_user = da.getAutoUser(user_auto.idDocencia, user_auto.idCurso, user_auto.idUsuario);

                                            /// 3.3.- Filtrar las tareas
                                            List<long> lst_id = lst_auto_user.Select(ad => ad.Id_Automatizacion).ToList();
                                            lst_auto = lst_auto.Where(ac => !lst_id.Contains(ac.Id_Automatizacion)).ToList();

                                            if (lst_auto.Count > 0)
                                            {
                                                foreach (var tarea in lst_auto)
                                                {
                                                    DateTime fecha_tarea = new DateTime();
                                                    if (tarea.Tipo == (int)Constantes.type_auto_curso.type_msg && tarea.Num_Dias == -1)
                                                        fecha_tarea = user_auto.Fecha_Fin.AddHours(num_horas).AddMinutes(num_minutos);
                                                    else
                                                        fecha_tarea = fecha_apertura.AddDays(tarea.Num_Dias);
                                                    if (DateTime.Now >= fecha_tarea)
                                                    {
                                                        AutoCurso ac = new AutoCurso();
                                                        bool process = ac.process(tutoria, tarea, user_auto);
                                                        if (process)
                                                        {
                                                            /// 3.2.- Añadir la solicitud a procesada
                                                            campus_AUTO_USER task_doc = new campus_AUTO_USER();
                                                            task_doc.ID_Docencia = user_auto.idDocencia;
                                                            task_doc.ID_Curso = user_auto.idCurso;
                                                            task_doc.Id_Usuario = user_auto.idUsuario;
                                                            task_doc.Id_Automatizacion = tarea.Id_Automatizacion;

                                                            long insert_task = da.insertTaskAutoUser(task_doc);
                                                            if (insert_task < 1)
                                                                LogUtils.InsertarLog(" ERROR - Al procesar la tarea en campus_AUTO_USER", true);
                                                            else
                                                                Console.WriteLine("Tarea " + tarea.Id_Automatizacion + " en campus_AUTO_USER procesada");
                                                        }
                                                        else
                                                            LogUtils.InsertarLog(" ERROR - Se ha producido un error en la tarea " + tarea.Id_Automatizacion + " del usuario " + user_auto.idUsuario + " de la docencia " + user_auto.idDocencia + " y curso " + user_auto.idCurso, true);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DateTime fecha_apertura = tutoria.FechaInicio.AddHours(num_horas).AddMinutes(num_minutos);
                                if (DateTime.Now >= fecha_apertura)
                                {
                                    /// 3.0.- Sacar datos de la BBDD
                                    List<campus_AUTO_CURSO> lst_auto = da.getAutoCourseByCourse(tutoria.idCurso);
                                    List<campus_AUTO_DOCENCIA> lst_auto_doc = da.getAutoDocencia(tutoria.idDocencia, tutoria.idCurso);

                                    /// 3.1.- Filtrar las tareas
                                    List<long> lst_id = lst_auto_doc.Select(ad => ad.Id_Automatizacion).ToList();
                                    lst_auto = lst_auto.Where(ac => !lst_id.Contains(ac.Id_Automatizacion)).ToList();

                                    if (lst_auto.Count > 0)
                                    {
                                        foreach (var tarea in lst_auto)
                                        {
                                            DateTime fecha_tarea = new DateTime();
                                            if (tarea.Tipo == (int)Constantes.type_auto_curso.type_msg && tarea.Num_Dias == -1)
                                                fecha_tarea = tutoria.FechaFin.AddHours(num_horas).AddMinutes(num_minutos);
                                            else
                                                fecha_tarea = fecha_apertura.AddDays(tarea.Num_Dias);
                                            if (DateTime.Now >= fecha_tarea)
                                            {
                                                AutoCurso ac = new AutoCurso();
                                                bool process = ac.process(tutoria, tarea);
                                                if (process)
                                                {
                                                    /// 3.2.- Añadir la solicitud a procesada
                                                    campus_AUTO_DOCENCIA task_doc = new campus_AUTO_DOCENCIA();
                                                    task_doc.ID_Docencia = tutoria.idDocencia;
                                                    task_doc.ID_Curso = tarea.ID_Curso;
                                                    task_doc.Id_Automatizacion = tarea.Id_Automatizacion;

                                                    long insert_task = da.insertTaskAutoDocencia(task_doc);
                                                    if (insert_task < 1)
                                                        LogUtils.InsertarLog(" ERROR - Al procesar la tarea en campus_AUTO_DOCENCIA", true);
                                                    else
                                                        Console.WriteLine("Tarea " + tarea.Id_Automatizacion + " en campus_AUTO_DOCENCIA procesada");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                /// 4.- Comprobar si hay casos practicos programados
                List<campus_REC_CP> lst_cases = da.getPracticalCasesProcess(true);
                if (lst_cases.Count > 0)
                {
                    foreach (var caso in lst_cases)
                    {
                        Caso_Practico caso_practico = new Caso_Practico();
                        bool process = caso_practico.process(caso);
                        if (process)
                        {
                            /// 4.1.- Cambiar la solicitud a procesada
                            List<campus_REC_CP> list_casos = da.getPracticalCasesById(caso.idCP);
                            if (list_casos.Count == 1)
                            {
                                campus_REC_CP practica_case = list_casos[0];
                                bool update_case = da.updateRecCP(practica_case, false);
                                if (!update_case)
                                    LogUtils.InsertarLog(" ERROR - Al procesar la solicitud en campus_REC_CP", true);
                            }
                        }
                    }
                }

                /// 5.- Comprobar si hay anuncios programados
                List<campus_ANUNCIO> lst_anuncios = da.getAnuncios(true);
                if (lst_anuncios.Count > 0)
                {
                    foreach (var anuncio in lst_anuncios)
                    {
                        Anuncio _anuncio = new Anuncio();
                        bool process = _anuncio.process(anuncio);
                        if (process)
                        {
                            /// 5.1.- Cambiar la solicitud a procesada
                            List<campus_ANUNCIO> list_anuncios = da.getAnunciosbyId(anuncio.ID_Anuncio);
                            if (list_anuncios.Count == 1)
                            {
                                campus_ANUNCIO ad = list_anuncios[0];
                                bool update_ad = da.updateAnuncio(ad, false);
                                if (!update_ad)
                                    LogUtils.InsertarLog(" ERROR - Al procesar la solicitud en campus_ANUNCIO", true);
                            }
                        }
                    }
                }

                LogUtils.InsertarLog("Fin del proceso", true);
                Console.WriteLine("Fin del proceso");
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Program.cs::Main()", true);
                LogUtils.InsertarLog("- MSG:" + ex.Message, true);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message), true);
            }
        }
    }
}
