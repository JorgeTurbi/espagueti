using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class matricula_pdp_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa y del contacto
                long idActivacionPDP = !String.IsNullOrEmpty(Request.QueryString["idm"]) ? long.Parse(Request.QueryString["idm"].ToString()) : -1;

                /// 2.- Cargar combos
                cargar_origenes();

                /// 3.- Cargar los datos de la activación
                if (idActivacionPDP > 0)
                    cargar_datos(idActivacionPDP);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idMatriculación
            long idMatriculacionPDP = !String.IsNullOrEmpty(Request.QueryString["idm"]) ? long.Parse(Request.QueryString["idm"]) : -1;

            /// 2.- Sacar los datos del formulario
            //long idStudent = long.Parse(idAlumno.Value);
            string nombre = txt_mat_nombre.Value;
            string apellidos = txt_mat_apellidos.Value;
            string mail = txt_mat_mail.Value;
            decimal? precio = null;
            if (!String.IsNullOrEmpty(txt_mat_precio.Value))
                precio = decimal.Parse(txt_mat_precio.Value);
            int num_cursos = int.Parse(txt_mat_cursos.Value);
            string mail_supervisor = txt_mat_mail_super.Value;
            long IdOrigen = long.Parse(ddlOrigen.SelectedValue);
            string comentarios = txt_comentarios.Value;

            /// 3.- Modificar o Insertar
            if (idMatriculacionPDP > 0)
            {
                List<campus_ACTIVACIONES_PDP> lst_activaciones = da.getListActivationsById(idMatriculacionPDP);
                if (lst_activaciones.Count == 1)
                {
                    /// 2.- Sacar resto de datos de la BBDD
                    List<campus_PDP> lst_pdp = da.getProgramByIdPDP(lst_activaciones[0].idPDP);
                    if (lst_pdp.Count == 1)
                    {
                        campus_PDP matricula_pdp = lst_pdp[0];
                        matricula_pdp.precio = precio;
                        matricula_pdp.num_Cursos = num_cursos;
                        matricula_pdp.mail_supervisor = mail_supervisor;
                        matricula_pdp.comentarios = comentarios;

                        bool update_pdp = da.updatePDP(matricula_pdp);
                        if (update_pdp)
                            Response.Redirect("matricula-pdp.aspx");
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar campus_PDP.";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar campus_PDP.";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al actualizar campus_PDP.";
            }
            else
            {
                /// 3.1.- Comprobar si existe el usuario
                List<CLIENTES> lst_user = da.getUserForMailOrLogin(mail);
                if (lst_user.Count > 0)
                {
                    if (lst_user.Count == 1)
                    {
                        /// 3.1.1.- Añadir la acción persona
                        campus_ACCIONES_PERSONA accion_persona = new campus_ACCIONES_PERSONA();
                        accion_persona.idPersona = lst_user[0].id_cliente;
                        accion_persona.idAccion = (long)Constantes.accion.Send_Activation_PDP;
                        accion_persona.Fecha = DateTime.Now;
                        accion_persona.IdOrigen = IdOrigen;

                        long insert_action = da.insertPersonAction(accion_persona);
                        if (insert_action > 0)
                        {
                            /// 3.1.2.- Añadir entrada en campus_PDP 
                            campus_PDP pdp = new campus_PDP();
                            pdp.fecha = DateTime.Now;
                            pdp.idCliente = lst_user[0].id_cliente;
                            pdp.num_Cursos = num_cursos;
                            pdp.precio = precio;
                            if (!String.IsNullOrEmpty(mail_supervisor))
                                pdp.mail_supervisor = mail_supervisor;
                            if (!String.IsNullOrEmpty(comentarios))
                                pdp.comentarios = comentarios;
                            pdp.procesado = false;

                            long insert_pdp = da.insertPDP(pdp);
                            if (insert_pdp > 0)
                            {
                                Guid guid = Guid.NewGuid();

                                /// 3.1.3.- Añadir entrada en campus_ACTIVACIONES_PDP
                                campus_ACTIVACIONES_PDP activation = new campus_ACTIVACIONES_PDP();
                                activation.key_Activacion = guid.ToString().ToUpper();
                                activation.idCliente = lst_user[0].id_cliente;
                                activation.idPDP = insert_pdp;
                                activation.activado = false;

                                long insert_activation = da.insertActivationPDP(activation);
                                if (insert_activation > 0)
                                {
                                    /// 1.- Cuerpo del mail
                                    string template = Utilities.getPlantillaMail("admin-matricular-pdp", ConfigurationManager.AppSettings["urlTemplate"]);
                                    if (!String.IsNullOrEmpty(template))
                                    {
                                        string url = "https://www.spainbs.com/activacion-programa.aspx?ka=" + guid.ToString().ToUpper();
                                        template = template.Replace("###Url###", url);
                                        template = template.Replace("###IDC###", ((long)Constantes.course.Sin_determinar).ToString());
                                        template = template.Replace("###KEY###", lst_user[0].Key);
                                    }

                                    /// 2.- Resto de datos necesarios para el mail
                                    string asunto = Utilities.getAsuntoMail("admin-matricular-pdp", ConfigurationManager.AppSettings["urlAsunto"]);
                                    int priority = 1;
                                    string mailTo = lst_user[0].email;
                                    string nameTo = lst_user[0].Nombre_Completo;

                                    /// 3.- Añadir los datos de envío del mail
                                    EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                                    email_data.nombreTo = nameTo;
                                    email_data.mailTo = mailTo;
                                    email_data.priority = priority;
                                    email_data.asunto = asunto;
                                    email_data.body = template;

                                    long insert_mail = da.insertEmailContent(email_data);
                                    if (insert_mail > 0)
                                    {
                                        /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                        long idLog = da.insertLog(lst_user[0].id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                        if (idLog < 1)
                                        {
                                            LogUtils.InsertarLog(" ERROR - matricula-pdp-mantenimiento.cs::btnGuardar_Click()");
                                            LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                                        }

                                        /// Redireccionar a matricula-pdp
                                        Response.Redirect("matricula-pdp.aspx");
                                    }
                                    else
                                        txt_error.InnerHtml = "Se ha producido un error al insertar el mail.";
                                }
                                else
                                    txt_error.InnerHtml = "Se ha producido un error al generar la Activación de PDP.";
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al generar el PDP.";
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al añadir la acción persona.";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error ya que existen dos o más usuarios con el mismo mail.";
                }
                else
                {
                    /// 3.2.- Crear a un usuario nuevo
                    CLIENTES client = new CLIENTES();
                    client.nombre = nombre;
                    client.apellidos = apellidos;
                    client.email = mail;
                    client.id_pais = (int)Constantes.pais.Spain;
                    client.id_provincia = 65;
                    client.id_idioma = 1;
                    client.activo = ((int)Constantes.activo.NoActivo).ToString();
                    client.fecha_alta = DateTime.Now;
                    client.login = DateTime.Now.ToString();
                    client.Key = Guid.NewGuid().ToString().ToUpper();
                    client.Nombre_Completo = client.nombre + " " + client.apellidos;

                    long insert_user = da.insertClient(client);
                    if (insert_user > 0)
                    {
                        /// 3.1.1.- Añadir la acción persona
                        campus_ACCIONES_PERSONA accion_persona = new campus_ACCIONES_PERSONA();
                        accion_persona.idPersona = insert_user;
                        accion_persona.idAccion = (long)Constantes.accion.Send_Activation_PDP;
                        accion_persona.Fecha = DateTime.Now;
                        accion_persona.IdOrigen = IdOrigen;

                        long insert_action = da.insertPersonAction(accion_persona);
                        if (insert_action > 0)
                        {
                            /// 3.1.2.- Añadir entrada en campus_PDP 
                            campus_PDP pdp = new campus_PDP();
                            pdp.fecha = DateTime.Now;
                            pdp.idCliente = insert_user;
                            pdp.num_Cursos = num_cursos;
                            pdp.precio = precio;
                            if (!String.IsNullOrEmpty(mail_supervisor))
                                pdp.mail_supervisor = mail_supervisor;
                            if (!String.IsNullOrEmpty(comentarios))
                                pdp.comentarios = comentarios;
                            pdp.procesado = false;

                            long insert_pdp = da.insertPDP(pdp);
                            if (insert_pdp > 0)
                            {
                                Guid guid = Guid.NewGuid();

                                /// 3.1.3.- Añadir entrada en campus_ACTIVACIONES_PDP
                                campus_ACTIVACIONES_PDP activation = new campus_ACTIVACIONES_PDP();
                                activation.key_Activacion = guid.ToString().ToUpper();
                                activation.idCliente = insert_user;
                                activation.idPDP = insert_pdp;
                                activation.activado = false;

                                long insert_activation = da.insertActivationPDP(activation);
                                if (insert_activation > 0)
                                {
                                    /// 1.- Cuerpo del mail
                                    string template = Utilities.getPlantillaMail("admin-matricular-pdp", ConfigurationManager.AppSettings["urlTemplate"]);
                                    if (!String.IsNullOrEmpty(template))
                                    {
                                        string url = "https://www.spainbs.com/activacion-programa.aspx?ka=" + guid.ToString().ToUpper();
                                        template = template.Replace("###Url###", url);
                                        template = template.Replace("###IDC###", ((long)Constantes.course.Sin_determinar).ToString());
                                        template = template.Replace("###KEY###", client.Key);
                                    }

                                    /// 2.- Resto de datos necesarios para el mail
                                    string asunto = Utilities.getAsuntoMail("admin-matricular-pdp", ConfigurationManager.AppSettings["urlAsunto"]);
                                    int priority = 1;
                                    string mailTo = mail;
                                    string nameTo = nombre + " " + apellidos;

                                    /// 3.- Añadir los datos de envío del mail
                                    EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                                    email_data.nombreTo = nameTo;
                                    email_data.mailTo = mailTo;
                                    email_data.priority = priority;
                                    email_data.asunto = asunto;
                                    email_data.body = template;

                                    long insert_mail = da.insertEmailContent(email_data);
                                    if (insert_mail > 0)
                                    {
                                        /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                        long idLog = da.insertLog(insert_user, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                        if (idLog < 1)
                                        {
                                            LogUtils.InsertarLog(" ERROR - matricula-pdp-mantenimiento.cs::btnGuardar_Click()");
                                            LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                                        }

                                        /// Redireccionar a matricula-pdp
                                        Response.Redirect("matricula-pdp.aspx");
                                    }
                                    else
                                        txt_error.InnerHtml = "Se ha producido un error al insertar el mail.";
                                }
                                else
                                    txt_error.InnerHtml = "Se ha producido un error al generar la Activación de PDP.";
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al generar el PDP.";
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al añadir la acción persona.";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al generar al usuario.";
                }
            }
        }

        [WebMethod(Description = "Busca alumnos a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> search_student(string name)
        {
            DataAccess da = new DataAccess();

            List<Usuarios> list_users = new List<Usuarios>();
            List<CLIENTES> lst_users = da.getUserBySearch(name);
            if (lst_users.Count > 0)
                list_users = lst_users.Select(user => new Usuarios { id_usuario = user.id_cliente, nombre_completo = user.Nombre_Completo, nombre = user.nombre, apellidos = user.apellidos, email = user.email }).ToList();
            return list_users;
        }

        private void cargar_datos(long idActivacionPDP)
        {
            /// 1.- Obtener los datos de la activación PDP de la BBDD
            List<campus_ACTIVACIONES_PDP> lst_activaciones = da.getListActivationsById(idActivacionPDP);
            if (lst_activaciones.Count == 1)
            {
                /// 2.- Sacar resto de datos de la BBDD
                List<campus_PDP> lst_pdp = da.getProgramByIdPDP(lst_activaciones[0].idPDP);
                List<CLIENTES> lst_user = da.getUserById(lst_activaciones[0].idCliente);
                if (lst_user.Count == 1 && lst_pdp.Count == 1)
                {
                    idAlumno.Value = lst_activaciones[0].idCliente.ToString();
                    txt_mat_nombre.Value = lst_user[0].nombre;
                    txt_mat_apellidos.Value = lst_user[0].apellidos;
                    txt_mat_mail.Value = lst_user[0].email;
                    txt_mat_precio.Value = lst_pdp[0].precio != null ? lst_pdp[0].precio.Value.ToString() : string.Empty;
                    txt_mat_cursos.Value = lst_pdp[0].num_Cursos.ToString();
                    txt_mat_mail_super.Value = lst_pdp[0].mail_supervisor;
                    txt_comentarios.Value = lst_pdp[0].comentarios;                    

                    search_form.Attributes["class"] = search_form.Attributes["class"].Insert(search_form.Attributes["class"].Length, " hidden");
                    txt_mat_nombre.Disabled = true;
                    txt_mat_apellidos.Disabled = true;
                    txt_mat_mail.Disabled = true;
                }                
            }
        }

        private void cargar_origenes()
        {
            /// 1.- Cargar las empresas
            List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");
            if (lst_origins.Count > 0)
            {
                this.ddlOrigen.DataSource = lst_origins;
                this.ddlOrigen.DataTextField = "Nombre";
                this.ddlOrigen.DataValueField = "ID_Aux";
                this.ddlOrigen.DataBind();
                ddlOrigen.SelectedValue = "208";
            }
        }
    }
}