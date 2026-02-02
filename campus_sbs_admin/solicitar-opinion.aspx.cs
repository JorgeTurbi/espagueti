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
    public partial class solicitar_opinion : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            // http://localhost:3063/solicitar-opinion.aspx?k=F464D3EB-D005-4BF2-BDE9-2FBC15929606&idd=418

            /// 0.- Sacar los datos del usuario 
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                    Session.Add("usuario", list_user[0]);
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

                /// 1.- Cargar combos
                cargar_tipo_solicitud();

                /// 2.- Cargar el cuerpo del mail
                txt_cuerpo.Value = "Hola ###NOMBRE###:<br /><br />En <strong>Spain Business School</strong>, nos importa mucho tu opinión.<br /><br />Por favor, contesta a la siguiente pregunta. No te llevará más de 20 segundos.";

                /// 3.- Cargar tabla con las solicitudes
                long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"]) : -1;
                table_listado_solicitudes.InnerHtml = paint_table(idDocencia);

                /// 4.- Pintar el título
                title_solicitud.InnerHtml = "<i class='far fa-comment-dots'></i> Solicitar opinión <a href='javascript:void(0);' onclick='ver_solicitudes()' title='Ver todas las solicitudes' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i> Ver todas las solicitudes</small></a>";
            }
        }
                
        protected void btn_send_Click(object sender, EventArgs e)
        {
            /// 1.- Recuperar el tipo de solicitud del formmulario
            string solicitud = ddlTipoSolicitud.Value;

            /// 2.- Recuperar la docencia
            long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"]) : -1;
            if (idDocencia > 0)
            {
                /// 3.- Sacar los alumnos de la docencia
                List<campus_DOCENCIA_GRUPO> lst_doc_group = da.getDocenciaGrupoByDocencia(idDocencia);
                if (lst_doc_group.Count > 0)
                {
                    /// 4.- Sacar los usuarios de la docencia
                    List<CLIENTES> lst_clients = da.getUserByList(lst_doc_group.Select(c => c.ID_Persona).Distinct().ToList());

                    /// 4.1.- Sacar sólo los alumnos
                    lst_clients = lst_clients.Where(c => (String.IsNullOrEmpty(c.Profesor) || c.Profesor == ((int)Constantes.activo.NoActivo).ToString()) && (String.IsNullOrEmpty(c.Administrador) || c.Administrador == ((int)Constantes.activo.NoActivo).ToString())).ToList();
                    if (lst_clients.Count > 0)
                    {
                        List<campus_OPINIONES_SOLICITUD> lst_solicitudes = da.getSolicitudOpinion(idDocencia, int.Parse(solicitud));
                        if (lst_solicitudes.Count == 0)
                        {
                            /// 4.2.- Sacar los datos del formulario
                            string nombre_from = txt_nombre_from.Value.Trim();
                            string mail_from = txt_mail_from.Value.Trim();
                            string replyTo = txt_reply_to.Value.Trim();
                            string asunto = txt_asunto.Value.Trim();
                            string cuerpo = txt_cuerpo.Value.Trim();

                            /// 4.3.- Programar una solicitud de opinión
                            campus_OPINIONES_SOLICITUD solicitud_opinion = new campus_OPINIONES_SOLICITUD();
                            solicitud_opinion.id_docencia = idDocencia;
                            solicitud_opinion.id_tso = int.Parse(solicitud);
                            solicitud_opinion.fecha = DateTime.Now;
                            if (!String.IsNullOrEmpty(nombre_from))
                                solicitud_opinion.nombreFrom = nombre_from;
                            if (!String.IsNullOrEmpty(mail_from))
                                solicitud_opinion.mailFrom = mail_from;
                            if (!String.IsNullOrEmpty(replyTo))
                                solicitud_opinion.replyTo = replyTo;
                            solicitud_opinion.asunto = asunto;
                            solicitud_opinion.body = cuerpo;
                            solicitud_opinion.procesado = false;

                            /// 4.4.- Insertar la solicitud
                            long insert_solicitud = da.insertSolicitudOpinion(solicitud_opinion);
                            if (insert_solicitud > 0)
                                Response.Redirect("solicitar-opinion.aspx?idd=" + idDocencia);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al añadir la solicitud";
                        }
                        else
                            txt_error.InnerHtml = "Ya existe una solicitud para ese tipo en esta docencia";
                    }
                }
            }
        }

        protected void btn_link_Click(object sender, EventArgs e)
        {
            /// 1.- Recuperar el tipo de solicitud del formmulario
            string solicitud = ddlTipoSolicitud.Value;

            /// 2.- Recuperar la docencia
            long idDocencia = !String.IsNullOrEmpty(Request.QueryString["idd"]) ? long.Parse(Request.QueryString["idd"]) : -1;
            if (idDocencia > 0)
            {
                /// 3.- Sacar los alumnos de la docencia
                List<campus_DOCENCIA_GRUPO> lst_doc_group = da.getDocenciaGrupoByDocencia(idDocencia);
                if (lst_doc_group.Count > 0)
                {
                    /// 4.- Sacar los usuarios de la docencia
                    List<CLIENTES> lst_clients = da.getUserByList(lst_doc_group.Select(c => c.ID_Persona).Distinct().ToList());

                    /// 4.1.- Sacar sólo los alumnos
                    lst_clients = lst_clients.Where(c => (String.IsNullOrEmpty(c.Profesor) || c.Profesor == ((int)Constantes.activo.NoActivo).ToString()) && (String.IsNullOrEmpty(c.Administrador) || c.Administrador == ((int)Constantes.activo.NoActivo).ToString())).ToList();
                    if (lst_clients.Count > 0)
                        table_listado_links.InnerHtml = paint_table(lst_clients, idDocencia, solicitud);
                }
            }
        }

        protected void btn_ver_Click(object sender, EventArgs e)
        {
            /// 1.- Pintar todas las solicitudes
            table_listado_solicitudes.InnerHtml = paint_table(-1);
        }

        private void cargar_tipo_solicitud()
        {
            /// 1.- Cargar el tipo solicitud
            List<campus_TIPO_SOLICITUD_OPINION> lst_types = da.getTypeOpinion(true);
            if (lst_types.Count > 0)
            {
                ddlTipoSolicitud.DataSource = lst_types;
                ddlTipoSolicitud.DataTextField = "solicitud";
                ddlTipoSolicitud.DataValueField = "id_tso";
                ddlTipoSolicitud.DataBind();
                ddlTipoSolicitud.Items.Add(new ListItem("Seleccione", "-1"));
                ddlTipoSolicitud.Value = "-1";
            }
        }

        private string paint_table(long idDocencia)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Listados_Solicitudes\" class=\"display compact\" style =\"width:100%\"><thead>");
            
            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha Envío</th>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>Pregunta</th>");
            sbuild.Append("<th>Nº Envios</th>");
            sbuild.Append("<th>Nº Opiniones</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");
            
            /// 3.- Sacar las opiniones de la docencia
            List<campus_OPINIONES_SOLICITUD> lst_solicitudes = da.getSolicitudOpinion(idDocencia, -1);
            if (lst_solicitudes.Count > 0)
            {
                /// 3.1.- Sacar los datos de la BBDD
                List<campus_DOCENCIA> lst_docencias = da.getDocenciaById(idDocencia);
                List<EMAIL_CONTENT> lst_mails = new List<EMAIL_CONTENT>();
                List<campus_OPINIONES> lst_opinions = new List<campus_OPINIONES>();
                List<campus_TIPO_SOLICITUD_OPINION> lst_types = da.getTypeOpinion(true);

                if (lst_solicitudes.Count == 1)
                {
                    lst_mails = da.getMailByIdOpinion(lst_solicitudes[0].id_solicitud_opinion);
                    lst_opinions = da.getOpinionsByIdSO(lst_solicitudes[0].id_solicitud_opinion);
                }
                else
                {
                    lst_mails = da.getMailByIdOpinion(-1);
                    lst_opinions = da.getOpinionsByIdSO(-1);
                }
                
                /// 3.2.- Recorrer las solicitudes
                foreach (var solicitud_opinion in lst_solicitudes)
                {
                    sbuild.Append("<tr>");

                    sbuild.Append("<td>" + solicitud_opinion.fecha.ToShortDateString() + "</td>");
                    sbuild.Append("<td>" + lst_docencias.Where(d => d.ID_Docencia == solicitud_opinion.id_docencia).Select(d => d.Nombre).FirstOrDefault() + " (" + solicitud_opinion.id_docencia + ")");
                    sbuild.Append("<td>" + lst_types.Where(t => t.id_tso == solicitud_opinion.id_tso).Select(t => t.solicitud).FirstOrDefault() + "</td>");
                    sbuild.Append("<td>" + lst_mails.Where(ec => ec.id_os == solicitud_opinion.id_solicitud_opinion).Count() + "</td>");
                    sbuild.Append("<td>" + lst_opinions.Where(op => op.id_os == solicitud_opinion.id_solicitud_opinion).Count() + "</td>");
                    
                    sbuild.Append("</tr>");
                }
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }

        private string paint_table(List<CLIENTES> lst_clients, long idDocencia, string solicitud)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Listados\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Url</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar los alumnos
            foreach (var user in lst_clients)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(user.Nombre_Completo) + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + user.id_cliente + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + user.Nombre_Completo + "</a></td>");
                sbuild.Append("<td>https://www.spainbs.com/tu-opinion-importa.aspx?k=" + user.Key + "&idd=" + idDocencia + "&tp=" + solicitud + "</td>");
                sbuild.Append("<td><a href='https://www.spainbs.com/tu-opinion-importa.aspx?k=" + user.Key + "&idd=" + idDocencia + "&tp=" + solicitud + "' title='Ver' target='_blank'><i class='fas fa-eye fa-1-6x'></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}