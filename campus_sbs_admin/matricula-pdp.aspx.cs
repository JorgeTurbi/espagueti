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
    public partial class matricula_pdp : System.Web.UI.Page
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
                    load_registrations(list_user[0]);
            }
        }
        
        protected void btnBorrarMatricula_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_registration = false;

            try
            {
                long idMatricula = !String.IsNullOrEmpty(hidIdMatricula.Value) ? long.Parse(hidIdMatricula.Value) : -1;
                if (idMatricula > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_ACTIVACIONES_PDP> list_activations = da.getListActivationsById(idMatricula);
                    if (list_activations.Count == 1)
                    {
                        /// 2.- Eliminar la activación pdp
                        bool delete_act = da.deleteActivationPDP(idMatricula);
                        if (delete_act)
                            delete_registration = da.deletePDP(list_activations[0].idPDP);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la matriculación por pdp');</script>");

                LogUtils.InsertarLog(" ERROR - empresas.cs::btnBorrarMatricula_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_registration)
                Response.Redirect("matricula-pdp.aspx");
        }

        protected void btnSend_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Sacar los datos de la BBDD
            List<campus_ACTIVACIONES_PDP> lst_activations = da.getListActivations(false);
            List<CLIENTES> lst_clients = da.getUserByList(lst_activations.Select(act => act.idCliente).ToList());
            List<campus_PDP> lst_pdp = da.getProgramByIdPDP(-1);
            List<EMAIL_CONTENT> lst_mails = da.getMailByTitle("Activación usuario en PDP");

            /// 2.- Recorrer las activaciones
            if (lst_activations.Count > 0)
            {
                bool send = true;
                foreach (var pdp in lst_activations)
                {
                    /// 2.1.- Sacar el nº de mails enviados
                    long number_mails = lst_mails.Where(m => m.body.Contains("PDP_Record-" + pdp.idCliente + "#" + pdp.idPDP)).Count() + 1;

                    /// 2.2.- Sacar los datos del usuario
                    List<CLIENTES> lst_user = lst_clients.Where(c => c.id_cliente == pdp.idCliente).ToList();

                    /// 2.3.- Generar el mail
                    string template = Utilities.getPlantillaMail("admin-matricular-pdp-mails", ConfigurationManager.AppSettings["urlTemplate"]);
                    if (!String.IsNullOrEmpty(template))
                    {
                        string url = "https://www.spainbs.com/activacion-programa.aspx?ka=" + pdp.key_Activacion;
                        template = template.Replace("###Url###", url);
                        template = template.Replace("###IDC###", ((long)Constantes.course.Sin_determinar).ToString());
                        template = template.Replace("###KEY###", lst_user[0].Key);

                        template = template.Replace("###NOMBRE###", lst_user[0].Nombre_Completo);
                        template = template.Replace("###NUM_MAILS###", number_mails.ToString());
                        template = template.Replace("###KEY_PDP###", "PDP_Record-" + pdp.idCliente + "#" + pdp.idPDP);
                    }

                    /// 2.- Resto de datos necesarios para el mail
                    string asunto = Utilities.getAsuntoMail("admin-matricular-pdp-mails", ConfigurationManager.AppSettings["urlAsunto"]).Replace("###NUM_MAILS###", number_mails.ToString());
                    int priority = 1;
                    string mailTo = lst_user[0].email;
                    string nameTo = lst_user[0].Nombre_Completo;

                    string mail_supervisor = string.Empty;
                    List<campus_PDP> lista_pdp = lst_pdp.Where(p => p.idPDP == pdp.idPDP).ToList();
                    if (lista_pdp.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(lista_pdp[0].mail_supervisor))
                            mail_supervisor = lista_pdp[0].mail_supervisor;
                    }

                    /// 3.- Añadir los datos de envío del mail
                    EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                    email_data.nombreTo = nameTo;
                    email_data.mailTo = mailTo;
                    email_data.priority = priority;
                    email_data.asunto = asunto;
                    email_data.body = template;
                    if (!String.IsNullOrEmpty(mail_supervisor))
                        email_data.mailCCO = mail_supervisor;

                    long insert_mail = da.insertEmailContent(email_data);
                    if (insert_mail > 0)
                    {
                        /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                        long idLog = da.insertLog(lst_user[0].id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                        if (idLog < 1)
                        {
                            LogUtils.InsertarLog(" ERROR - matricula-pdp.cs::btnSend_Click()");
                            LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                        }
                    }
                    else
                    {
                        send = false;
                        break;
                    }
                }

                if (send)
                    Response.Redirect("matricula-pdp.aspx");
                else
                    ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al enviar los recordatorios');</script>");
            }
        }

        private void load_registrations(CLIENTES user)
        {
            /// 1.- Sacar los datos de la BBDD
            List<campus_ACTIVACIONES_PDP> lst_activations = da.getListActivations(false);
            List<CLIENTES> lst_clients = da.getUserByList(lst_activations.Select(act => act.idCliente).ToList());
            List<campus_PDP> lst_pdp = da.getProgramByIdPDP(-1);
            List<EMAIL_CONTENT> lst_mails = da.getMailByTitle("Activación usuario en PDP");

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Matriculas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Nº Mails</th>");
            sbuild.Append("<th>Usuario</th>");
            sbuild.Append("<th>Precio</th>");
            sbuild.Append("<th>Nº Cursos</th>");
            sbuild.Append("<th>Url</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las matriculas
            foreach(var matricula in lst_activations)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + lst_pdp.Where(p => p.idPDP == matricula.idPDP).Select(p => p.fecha).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + lst_mails.Where(m => m.body.Contains("PDP_Record-" + matricula.idCliente + "#" + matricula.idPDP)).Count() + "</td>");

                List<CLIENTES> list_clients = lst_clients.Where(c => c.id_cliente == matricula.idCliente).ToList();
                if (list_clients.Count > 0)
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + matricula.idCliente + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + list_clients[0].Nombre_Completo + " (" + list_clients[0].id_cliente + ") [" + list_clients[0].email + "]</a></td>");
                else
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + matricula.idCliente + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + matricula.idCliente + "</a></td>");
                sbuild.Append("<td>" + lst_pdp.Where(p => p.idPDP == matricula.idPDP).Select(p => p.precio).FirstOrDefault() + "</td>");
                sbuild.Append("<td>" + lst_pdp.Where(p => p.idPDP == matricula.idPDP).Select(p => p.num_Cursos).FirstOrDefault() + "</td>");
                sbuild.Append("<td><a href='https://www.spainbs.com/activacion-programa.aspx?ka=" + matricula.key_Activacion + "' target='_blank'><i class='fas fa-globe-europe fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href=\"matricula-pdp-mantenimiento.aspx?idm=" + matricula.idActivacion_PDP + "\" title=\"Editar\"><i class=\"fas fa-edit fa-1-6x\"></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la matricula?\")){eliminarMatricula(" + matricula.idActivacion_PDP + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }            

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_matriculas.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            txt_matricula_pdp.InnerHtml = "<i class='fas fa-user-plus'></i> Listado de matriculaciones pdp <a href='matricula-pdp-mantenimiento.aspx' title='Nueva matriculación' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva matriculación pdp</small></a><a href='javascript:void(0);' onclick='send_mails()' title='Envío de recordatorios' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-mail-bulk fa-2x'></i>  Envío de recordatorios</small></a>";
        }
    }
}