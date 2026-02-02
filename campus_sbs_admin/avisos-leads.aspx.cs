using NAudio.Wave;
using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class avisos_leads : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Sacar las acciones
            List<long> list_id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                list_id_act.Add(long.Parse(act));
            }

            /// 1.- Sacar los datos de la BBDD
            List<campus_ACCIONES_PERSONA> lst_actions = da.getActionsByListAct(list_id_act);

            /// 2.- Quitar los resultados que tengan idAccion=243 y idCurso=22
            lst_actions = lst_actions.Where(act => !(act.idAccion == 243 && act.IdCurso == 22)).ToList();

            /// 3.- Quitar los recordatorios
            lst_actions = lst_actions.Where(act => act.IdOrigen != 318).ToList();

            /// 4.- Sacar los resultados de hoy y anteriores
            DateTime fecha = DateTime.Today.AddDays(1);
            lst_actions = lst_actions.Where(act => act.Fecha < fecha).ToList();

            /// 5.- Pintar la tabla
            if (lst_actions.Count > 0)
            {
                /// 5.1.- Sacar los datos de la BBDD
                List<long> list_users = lst_actions.Select(act => act.idPersona).Distinct().ToList();
                List<CLIENTES> lst_users = da.getUserByList(list_users);
                List<campus_CURSO> lst_courses = da.getCourses(null);
                List<campus_AUX> lst_aux = da.getAuxiliars(string.Empty);
                List<Paises> lst_countries = da.getCountries();

                table_listado_leads.InnerHtml = paint_table(lst_actions, lst_users, lst_courses, lst_aux, lst_countries);

                section_leads.Attributes["class"] = section_leads.Attributes["class"].Insert(section_leads.Attributes["class"].Length, " bg-color-red");
                
                /// 5.2.- Pitido
                //SystemSounds.Beep.Play();
                
                /*var url = ConfigurationManager.AppSettings["ruta_sonido"];
                using (var mf = new MediaFoundationReader(url))
                using (var wo = new WaveOutEvent())
                {
                    wo.Init(mf);
                    wo.Play();
                    while (wo.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }*/
            }

            /// 6.- Pintar el título
            txt_ventas_tpv.InnerHtml = "<i class='fas fa-tasks'></i> Listado de Leads";
        }

        private string paint_table(List<campus_ACCIONES_PERSONA> lst_actions, List<CLIENTES> lst_users, List<campus_CURSO> lst_courses, List<campus_AUX> lst_aux, List<Paises> lst_countries)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Leads\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Acción</th>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>Usuario</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los leads
            foreach (var lead in lst_actions)
            {
                sbuild.Append("<tr>");

                /// 3.1.- Fecha
                sbuild.Append("<td>" + lead.Fecha + "</td>");

                /// 3.2.- Origen
                if (lead.IdOrigen != null)
                    sbuild.Append("<td>" + lst_aux.Where(aux => aux.ID_Aux == lead.IdOrigen).Select(aux => aux.Nombre).FirstOrDefault() + " (" + lead.IdOrigen + ")</td>");
                else
                    sbuild.Append("<td></td>");

                /// 3.3.- Acción
                if (lead.idAccion == 91)
                    sbuild.Append("<td>BECAS (" + lead.idAccion + ")</td>");
                else if (lead.idAccion == 99)
                    sbuild.Append("<td>INFO PROG. (" + lead.idAccion + ")</td>");
                else if (lead.idAccion == 243)
                    sbuild.Append("<td>LANDING. (" + lead.idAccion + ")</td>");
                else
                    sbuild.Append("<td>(" + lead.idAccion + ")</td>");

                /// 3.4.- Curso
                if (lead.IdCurso != null)
                    sbuild.Append("<td>" + lst_courses.Where(c => c.ID_Curso == lead.IdCurso).Select(c => c.Nombre).FirstOrDefault() + " (" + lead.IdCurso + ")</td>");
                else
                    sbuild.Append("<td>(" + lead.IdCurso + ")</td>");

                /// 3.5.- Usuario
                long id_country = lst_users.Where(u => u.id_cliente == lead.idPersona).Select(u => u.id_pais).FirstOrDefault() != null ? lst_users.Where(u => u.id_cliente == lead.idPersona).Select(u => u.id_pais).FirstOrDefault().Value : -1;
                sbuild.Append("<td>" + lst_users.Where(u => u.id_cliente == lead.idPersona).Select(u => u.Nombre_Completo).FirstOrDefault() + " (" + lead.idPersona + ") [" + lst_countries.Where(p => p.id_pais == id_country).Select(p => p.nombre).FirstOrDefault() + "]</td>");

                sbuild.Append("</tr>");
            }
            
            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return (sbuild.ToString());
        }
    }
}