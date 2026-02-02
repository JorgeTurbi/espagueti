using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class ficha_alumno_crm : System.Web.UI.Page
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
                bool comprobate_user = Utilities.comprobate_users(list_user[0]);
                if (!comprobate_user)
                    Response.Redirect("login.aspx");
                else
                {
                    /// 0.- Sacar la tabla AUX y cargar los combos
                    List<campus_AUX> _aux = da.getAuxForId(-1);
                    cargar_combos(_aux);

                    /// 1.- Sacar los datos del usuario
                    long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                    if (idUsuario > 0)
                        cargar_datos(idUsuario, _aux, list_user[0]);
                    else
                    {
                        txtFechaAlta.Value = DateTime.Today.ToShortDateString();
                        lblStatus.Attributes["class"] = lblStatus.Attributes["class"].Replace("bg-green", string.Empty);

                        /// Ocultar los botones 
                        lnk_mail.Visible = false;
                        lnk_mail2.Visible = false;

                        ws_user.Visible = false;
                        ws2_user.Visible = false;

                        /// Ocultar los bloques
                        blk_pet_inf.Visible = false;
                        blk_cursos.Visible = false;
                        blk_admin.Visible = false;
                        blk_ventas.Visible = false;
                        blk_documentos.Visible = false;
                        blk_acciones.Visible = false;
                        blk_empleos.Visible = false;
                    }

                    /// 2.- Datos de los fileuploads
                    file_foto.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre la foto del usuario en el área seleccionada</span><input id='fileupload_logo' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idu\": \"" + idUsuario + "\", \"type\": \"img_ficha\", \"accion\": \"update\" }' />";
                }
            }
        }

        protected void btn_Password_Click(object sender, EventArgs e)
        {
            /// 0.- Sacar el idUsuario
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
            if (idUsuario > 0)
            {
                /// 1.- Sacar los datos del usuario
                List<CLIENTES> _users = da.getUserById(idUsuario);
                if (_users.Count > 0)
                {
                    /// 2.- Crear los datos del cliente
                    CLIENTES _user = _users[0];

                    /// 2.1.- Comprobar el login del usuario
                    if (Utils.esDateTimeCorrecto(_user.login))
                        _user.login = Utilities.generate_login(_user);

                    /// 3.- Generar el password nuevo
                    _user.password = Utilities.generate_password(_user);
                    if (!String.IsNullOrEmpty(_user.password))
                    {
                        /// 4.- Actualizar la password
                        bool _update_user = da.updateClient(_user);
                        if (_update_user)
                        {
                            /// 5.- Mandar mail al usuario
                            string template = Utilities.getPlantillaMail("change-password", ConfigurationManager.AppSettings["urlTemplate"]);
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###Nombre###", _user.Nombre_Completo);
                                template = template.Replace("###Usuario###", _user.login);
                                template = template.Replace("###Clave###", Utils.toDecodeString(_user.password));
                                template = template.Replace("###IDC###", ((long)Constantes.course.Sin_determinar).ToString());
                                template = template.Replace("###KEY###", _user.Key);
                            }

                            /// 5.1.- Resto de datos necesarios para el mail
                            string asunto = Utilities.getAsuntoMail("change-password", ConfigurationManager.AppSettings["urlAsunto"]);
                            int priority = 1;
                            string mailTo = _user.email;
                            string nameTo = _user.Nombre_Completo;

                            /// 5.2.- Añadir los datos de envío del mail
                            EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                            email_data.nombreTo = nameTo;
                            email_data.mailTo = mailTo;
                            email_data.priority = priority;
                            email_data.asunto = asunto;
                            email_data.body = template;

                            long insert_mail = da.insertEmailContent(email_data);
                            if (insert_mail > 0)
                            {
                                /// 5.3.- Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                long idLog = da.insertLog(_user.id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                if (idLog < 1)
                                {
                                    LogUtils.InsertarLog(" ERROR - remember-password.cs::btn_forget_Click()");
                                    LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                                }

                                /// 5.4.- Redirigir a la página
                                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                            }
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el password";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al generar el password";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al buscar al usuario";
            }
            else
                txt_error.InnerHtml = "Se ha producido un error al generar la nueva contraseña.";
        }

        protected void btn_guardar_Click(object sender, EventArgs e)
        {
            /// 0.- Sacar el idUsuario
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
            long idResultado = -1;
            bool _validate = false;

            try
            {
                /// 1.- Sacar los datos del formulario
                string nombre = txt_nombre.Value.Trim();
                string apellidos = txt_apellidos.Value.Trim();
                string mail = txt_mail.Value.Trim();
                string telefono = txt_telefono.Value;
                string mail2 = null;
                if (!String.IsNullOrEmpty(txt_mail2.Value))
                    mail2 = txt_mail2.Value.Trim();
                string telefono2 = null;
                if (!String.IsNullOrEmpty(txt_telefono2.Value))
                    telefono2 = txt_telefono2.Value;
                DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
                DateTime? fecha_baja = null;
                if (!String.IsNullOrEmpty(txtFechaBaja.Value))
                    fecha_baja = DateTime.Parse(txtFechaBaja.Value);

                string pais_nacimiento = ddlPaisNac.Value;
                string ciudad_nacimiento = null;
                if (!String.IsNullOrEmpty(txt_ciudad_nac.Value))
                    ciudad_nacimiento = txt_ciudad_nac.Value;
                
                int pais_residencia = int.Parse(ddlPaisResidencia.Value);
                int provincia_residencia = int.Parse(ddlProvResidencia.Value);
                string ciudad_residencia = null;
                if (!String.IsNullOrEmpty(txt_ciudad_residencia.Value))
                    ciudad_residencia = txt_ciudad_residencia.Value;

                DateTime? fecha_nacimiento = null;
                if (!String.IsNullOrEmpty(txtFechaNacimiento.Value))
                    fecha_nacimiento = DateTime.Parse(txtFechaNacimiento.Value);
                string dni = null;
                if (!String.IsNullOrEmpty(dni_user.Value))
                    dni = dni_user.Value;
                string pasaporte = null;
                if (!String.IsNullOrEmpty(passport_user.Value))
                    pasaporte = passport_user.Value;
                string sexo = sex_M.Checked ? sex_M.Value : (sex_V.Checked ? sex_V.Value : null);
                string foto = null;
                if (!String.IsNullOrEmpty(txt_foto.Value))
                    foto = txt_foto.Value;

                int nivel_estudios = int.Parse(ddlNivelEstudios.Value);
                int experiencia = int.Parse(ddlExperiencia.Value);
                int situacion_laboral = int.Parse(ddlSituacion.Value);
                string sector_profesional = null;
                if (!String.IsNullOrEmpty(sector_user.Value))
                    sector_profesional = sector_user.Value;

                string descripcion_ficha_base = null;
                if (!String.IsNullOrEmpty(descripcion_user.Value))
                    descripcion_ficha_base = descripcion_user.Value;

                string comentarios_internos = null;
                if (!String.IsNullOrEmpty(comentarios_user.Value))
                    comentarios_internos = comentarios_user.Value;

                string com_publico = null;
                if (!String.IsNullOrEmpty(comentario_publico.Value))
                    com_publico = comentario_publico.Value;

                /// 2.- Validar si el mail es único
                List<CLIENTES> _user_mail = da.getUserForMailOrLogin(mail);
                if (_user_mail.Count == 0)
                {
                    if (!String.IsNullOrEmpty(mail2))
                    {
                        _user_mail = da.getUserForMailOrLogin(mail2);
                        if (_user_mail.Count == 0 || (_user_mail.Count == 1 && _user_mail[0].id_cliente == idUsuario))
                            _validate = true;
                    }
                    else
                        _validate = true;
                }
                else if (_user_mail.Count == 1 && _user_mail[0].id_cliente == idUsuario)
                    _validate = true;

                if (_validate)
                {
                    /// 3.- Modificar o Insertar
                    if (idUsuario > 0)
                    {
                        /// 3.0.- Sacar los datos del usuario
                        List<CLIENTES> _users = da.getUserById(idUsuario);
                        if (_users.Count == 1)
                        {
                            /// 3.0.1.- Recuperar los datos del usuario
                            CLIENTES _user = _users[0];

                            /// 3.1.- Comprobar foto
                            if (!String.IsNullOrEmpty(foto) && foto != _user.Foto)
                            {
                                /// 3.1.1.- Guardar el fichero en la carpeta correcta
                                string ruta = ConfigurationManager.AppSettings["routeUserPhoto"];

                                /// 3.1.1.0.- Eliminar el fichero anterior
                                if (!String.IsNullOrEmpty(_user.Foto))
                                    File.Delete(ruta + _user.Foto);

                                /// 3.1.1.1.- Rutas nuevas
                                string ruta_origen = ruta + "temp\\" + foto;
                                string ruta_destino = ruta;

                                /// 3.1.2.- Si no existe el directorio lo creamos.
                                if (!(Directory.Exists(ruta_destino)))
                                    Directory.CreateDirectory(ruta_destino);

                                ruta_destino = ruta_destino + foto;

                                /// 3.1.3.- Copiar el fichero
                                File.Copy(ruta_origen, ruta_destino, true);

                                /// 3.1.4.- Borramos el fichero de la carpeta origen
                                File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                                /// 3.1.5.- Borramos el directorio temp
                                if (Directory.GetFiles(ruta + "temp\\").Count() == 0)
                                {
                                    if ((Directory.Exists(ruta + "temp\\")))
                                        Directory.Delete(ruta + "temp\\");
                                }
                                else
                                {
                                    foreach (var file in Directory.GetFiles(ruta + "temp\\"))
                                    {
                                        File.Delete(file);
                                    }

                                    if ((Directory.Exists(ruta + "temp\\")))
                                        Directory.Delete(ruta + "temp\\");
                                }
                            }

                            /// 3.2.- Actualizar los datos del usuario 
                            _user.nombre = nombre;
                            _user.apellidos = apellidos;
                            _user.email = mail;
                            _user.telefono_contacto = telefono;
                            _user.Email2 = mail2;
                            _user.telefono = telefono2;
                            _user.fecha_alta = fecha_alta;
                            _user.fecha_baja = fecha_baja;
                            _user.fecha_modificacion = DateTime.Now;

                            _user.Nacionalidad = pais_nacimiento;
                            _user.Lugar_nac = ciudad_nacimiento;
                            _user.id_pais = pais_residencia;
                            if (provincia_residencia > 0)
                                _user.id_provincia = provincia_residencia;
                            else
                                _user.id_provincia = (int)Constantes.provincias.Others;
                            _user.localidad = ciudad_residencia;

                            _user.fecha_nac = fecha_nacimiento;
                            _user.nif = dni;
                            _user.pasaporte = pasaporte;
                            _user.sexo = sexo;
                            _user.Foto = foto;
                            _user.Nivel_Estudios = nivel_estudios;
                            _user.Experiencia = experiencia;
                            _user.Situacion_Actual = situacion_laboral;
                            _user.Sector_Profesional = sector_profesional;
                            _user.Comentarios_Internos = comentarios_internos;
                            _user.descripcion_ficha_base = descripcion_ficha_base;
                            _user.comentario_publico_alumno = com_publico;
                            _user.Nombre_Completo = _user.nombre + " " + _user.apellidos;

                            bool _update_user = da.updateClient(_user);
                            if (_update_user)
                                idResultado = idUsuario;
                        }
                    }
                    else
                    {
                        /// 3.1.- Insertar un nuevo usuario
                        CLIENTES _user = new CLIENTES();

                        /// 3.1.- Comprobar foto
                        if (!String.IsNullOrEmpty(foto))
                        {
                            /// 3.1.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["routeUserPhoto"];

                            /// 3.1.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + foto;
                            string ruta_destino = ruta;

                            /// 3.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + foto;

                            /// 3.1.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 3.1.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                            /// 3.1.5.- Borramos el directorio temp
                            if (Directory.GetFiles(ruta + "temp\\").Count() == 0)
                            {
                                if ((Directory.Exists(ruta + "temp\\")))
                                    Directory.Delete(ruta + "temp\\");
                            }
                            else
                            {
                                foreach (var file in Directory.GetFiles(ruta + "temp\\"))
                                {
                                    File.Delete(file);
                                }

                                if ((Directory.Exists(ruta + "temp\\")))
                                    Directory.Delete(ruta + "temp\\");
                            }
                        }

                        /// 3.2.- Actualizar los datos del usuario 
                        _user.nombre = nombre;
                        _user.apellidos = apellidos;
                        _user.email = mail;
                        _user.telefono_contacto = telefono;
                        _user.Email2 = mail2;
                        _user.telefono = telefono2;
                        _user.fecha_alta = fecha_alta;
                        _user.fecha_baja = fecha_baja;
                        _user.fecha_modificacion = DateTime.Now;

                        _user.Nacionalidad = pais_nacimiento;
                        _user.Lugar_nac = ciudad_nacimiento;
                        _user.id_pais = pais_residencia;
                        if (provincia_residencia > 0)
                            _user.id_provincia = provincia_residencia;
                        else
                            _user.id_provincia = (int)Constantes.provincias.Others;
                        _user.localidad = ciudad_residencia;

                        _user.fecha_nac = fecha_nacimiento;
                        _user.nif = dni;
                        _user.pasaporte = pasaporte;
                        _user.sexo = sexo;
                        _user.Foto = foto;
                        _user.Nivel_Estudios = nivel_estudios;
                        _user.Experiencia = experiencia;
                        _user.Situacion_Actual = situacion_laboral;
                        _user.Sector_Profesional = sector_profesional;
                        _user.Comentarios_Internos = comentarios_internos;
                        _user.descripcion_ficha_base = descripcion_ficha_base;
                        _user.comentario_publico_alumno = com_publico;
                        _user.Nombre_Completo = _user.nombre + " " + _user.apellidos;
                        _user.Key = Guid.NewGuid().ToString().ToUpper();
                        _user.activo = ((int)Constantes.activo.NoActivo).ToString();
                        _user.id_idioma = (int)Constantes.activo.Activo;
                        _user.login = DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("000");

                        idResultado = da.insertClient(_user);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
            {
                if (idUsuario > 0)
                    Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                else
                    Response.Redirect("ficha-alumno-crm.aspx?idu=" + idResultado);
            }
            else
            {
                if (!_validate)
                    txt_error.InnerHtml = "El mail o el mail2 introducidos ya existen en la BBDD";
                else if (idUsuario > 0)
                    txt_error.InnerHtml = "Se ha producido un error al actualizar al usuario";
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir un usuario";
            }
        }
        protected void btn_save_admin_Click(object sender, EventArgs e)
        {
            /// 0.- Sacar el idUsuario
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
            long idResultado = -1;

            try
            {
                /// 1.- Sacar los datos del formulario
                bool _activo = chkActivo.Checked;
                bool _profesor = chkProfesor.Checked;
                bool _comercial = chkComercial.Checked;
                bool _administrador = chkAdministrador.Checked;
                bool _no_encuesta = chkNoEncuesta.Checked;
                bool _online = chkOnline.Checked;
                bool _practicas = chkPracticas.Checked;
                bool _baja_impago = chkBajaImpago.Checked;

                /// 2.- Modificar o Insertar
                if (idUsuario > 0)
                {
                    /// 2.0.- Sacar los datos del usuario
                    List<CLIENTES> _users = da.getUserById(idUsuario);
                    if (_users.Count == 1)
                    {
                        /// 2.1.- Inicializar al usuario
                        CLIENTES _user = _users[0];

                        /// 2.2.- Actualizar los datos del usuario 
                        _user.activo = _activo ? ((int)Constantes.activo.Activo).ToString() : ((int)Constantes.activo.NoActivo).ToString();
                        _user.Profesor = _profesor ? ((int)Constantes.activo.Activo).ToString() : ((int)Constantes.activo.NoActivo).ToString();
                        _user.Comercial = _comercial;
                        _user.Administrador = _administrador ? ((int)Constantes.activo.Activo).ToString() : ((int)Constantes.activo.NoActivo).ToString();
                        _user.Encuesta = _no_encuesta ? ((int)Constantes.activo.Activo).ToString() : ((int)Constantes.activo.NoActivo).ToString();
                        _user.online = _online;
                        _user.practicas = _practicas;
                        _user.Baja_Impago = _baja_impago;
                        _user.fecha_modificacion = DateTime.Now;

                        bool _update_user = da.updateClient(_user);
                        if (_update_user)
                            idResultado = idUsuario;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_save_admin_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al actualizar al usuario";
        }
        
        protected void btn_baja_Click(object sender, EventArgs e)
        {
            /// 0.- Sacar el idUsuario
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
            string motivo = motivo_baja.Value.Trim();
            if (idUsuario > 0 && !String.IsNullOrEmpty(motivo))
            {
                /// 1.- Sacar los datos del usuario
                List<CLIENTES> _users = da.getUserById(idUsuario);
                if (_users.Count > 0)
                {
                    /// 2.- Crear los datos del usuario
                    CLIENTES _user = _users[0];

                    /// 3.- Actualizar los datos del usuario
                    _user.activo = ((int)Constantes.activo.NoActivo).ToString();
                    if (String.IsNullOrEmpty(_user.Comentarios_Internos))
                        _user.Comentarios_Internos = "Motivo baja: " + motivo;
                    else
                        _user.Comentarios_Internos = _user.Comentarios_Internos + "\r\nMotivo baja: " + motivo;
                    _user.fecha_baja = DateTime.Now;

                    /// 4.- Actualizar la password
                    bool _update_user = da.updateClient(_user);
                    if (_update_user)
                    {
                        /// 5.- Añadir una entrada en campus_ACCIONES_PERSONA
                        campus_ACCIONES_PERSONA action = new campus_ACCIONES_PERSONA();
                        action.idPersona = _user.id_cliente;
                        action.idAccion = (long)Constantes.accion.Dar_baja;
                        action.Fecha = DateTime.Now;
                        action.IdOrigen = (long)Constantes.origen.SBS_Admin;
                        action.Comentario = motivo;

                        long insert_action = da.insertPersonAction(action);
                        if (insert_action > 0)
                        {
                            /// 6.- Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                            long idLog = da.insertLog(_user.id_cliente, Utils.GetStringValue(Constantes.log.Baja_usuario).ToUpper());
                            if (idLog < 1)
                            {
                                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_baja_Click()");
                                LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                            }

                            /// 7.- Redirigir a la página
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al generar la AP";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar el usuario";
                }
            }
        }

        protected void btn_send_mail_Click(object sender, EventArgs e)
        {
            /// 0.- Sacar el idUsuario
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
            if (idUsuario > 0)
            {
                /// 1.- Sacar los datos del usuario
                List<CLIENTES> _users = da.getUserById(idUsuario);
                if (_users.Count == 1)
                {
                    /// 2.- Sacar la activación 
                    List<campus_ACTIVACIONES> _activaciones = da.getActivationByUser(idUsuario);
                    _activaciones = _activaciones.Where(_ => _.Activado == (int)Constantes.activo.NoActivo).ToList();
                    if (_activaciones.Count > 0)
                    {
                        /// 3.- Mandar mail al usuario
                        string template = Utilities.getPlantillaMail("admin-matricular", ConfigurationManager.AppSettings["urlTemplate"]);
                        if (!String.IsNullOrEmpty(template))
                        {
                            template = template.Replace("###Nombre###", _users[0].Nombre_Completo);
                            string _url = "https://campus.spainbs.com/activacion.aspx?ID=" + _activaciones[0].ID_Activacion;
                            template = template.Replace("###Url###", _url);
                            template = template.Replace("###IDC###", ((long)Constantes.course.Sin_determinar).ToString());
                            template = template.Replace("###KEY###", _users[0].Key);
                        }

                        /// 3.1.- Resto de datos necesarios para el mail
                        string asunto = Utilities.getAsuntoMail("admin-matricular-pdp", ConfigurationManager.AppSettings["urlAsunto"]);
                        int priority = 1;
                        string mailTo = _users[0].email;
                        string nameTo = _users[0].Nombre_Completo;

                        /// 3.2.- Añadir los datos de envío del mail
                        EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                        email_data.nombreTo = nameTo;
                        email_data.mailTo = mailTo;
                        email_data.priority = priority;
                        email_data.asunto = asunto;
                        email_data.body = template;

                        long insert_mail = da.insertEmailContent(email_data);
                        if (insert_mail > 0)
                        {
                            /// 3.3.- Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                            long idLog = da.insertLog(_users[0].id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                            if (idLog < 1)
                            {
                                LogUtils.InsertarLog(" ERROR - ficha-alumnno-crm.cs::btn_send_mail_Click()");
                                LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                            }

                            /// 3.4.- Redirigir a la página
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        }
                    }
                }
            }
        }
        protected void btn_send_pago_Click(object sender, EventArgs e)
        {
            bool send_pago = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_pago = !String.IsNullOrEmpty(hid_pago.Value) ? long.Parse(hid_pago.Value) : -1;
                if (id_pago > 0)
                {


                    /*


            /// 0.- Sacar el idUsuario
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
            if (idUsuario > 0)
            {
                /// 1.- Sacar los datos del usuario
                List<CLIENTES> _users = da.getUserById(idUsuario);
                if (_users.Count == 1)
                {
                    /// 2.- Sacar la activación 
                    List<campus_ACTIVACIONES> _activaciones = da.getActivationByUser(idUsuario);
                    _activaciones = _activaciones.Where(_ => _.Activado == (int)Constantes.activo.NoActivo).ToList();
                    if (_activaciones.Count > 0)
                    {
                        /// 3.- Mandar mail al usuario
                        string template = Utilities.getPlantillaMail("admin-matricular", ConfigurationManager.AppSettings["urlTemplate"]);
                        if (!String.IsNullOrEmpty(template))
                        {
                            template = template.Replace("###Nombre###", _users[0].Nombre_Completo);
                            string _url = "https://campus.spainbs.com/activacion.aspx?ID=" + _activaciones[0].ID_Activacion;
                            template = template.Replace("###Url###", _url);
                            template = template.Replace("###IDC###", ((long)Constantes.course.Sin_determinar).ToString());
                            template = template.Replace("###KEY###", _users[0].Key);
                        }

                        /// 3.1.- Resto de datos necesarios para el mail
                        string asunto = Utilities.getAsuntoMail("admin-matricular-pdp", ConfigurationManager.AppSettings["urlAsunto"]);
                        int priority = 1;
                        string mailTo = _users[0].email;
                        string nameTo = _users[0].Nombre_Completo;

                        /// 3.2.- Añadir los datos de envío del mail
                        EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                        email_data.nombreTo = nameTo;
                        email_data.mailTo = mailTo;
                        email_data.priority = priority;
                        email_data.asunto = asunto;
                        email_data.body = template;

                        long insert_mail = da.insertEmailContent(email_data);
                        if (insert_mail > 0)
                        {
                            /// 3.3.- Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                            long idLog = da.insertLog(_users[0].id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                            if (idLog < 1)
                            {
                                LogUtils.InsertarLog(" ERROR - ficha-alumnno-crm.cs::btn_send_mail_Click()");
                                LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                            }

                            /// 3.4.- Redirigir a la página
                            Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
                        }
                    }
                }
            }*/
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_pago_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (send_pago)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al enviar el recordatorio del pago";
        }

        protected void btn_procesar_peticion_Click(object sender, EventArgs e)
        {
            bool update_pet_info = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_pet_info = !String.IsNullOrEmpty(hid_peticion_info.Value) ? long.Parse(hid_peticion_info.Value) : -1;
                if (id_pet_info > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_ACCIONES_PERSONA> _action = da.getActionsByAP(id_pet_info);
                    if (_action.Count == 1)
                    {
                        /// 2.- Procesar la AP
                        campus_ACCIONES_PERSONA _lead = _action[0];
                        _lead.Procesado = true;
                        update_pet_info = da.updatePersonAction(_lead);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_procesar_peticion_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (update_pet_info)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al procesar una petición de información";
        }

        protected void btn_delete_link_Click(object sender, EventArgs e)
        {
            bool delete_link = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_link = !String.IsNullOrEmpty(hid_link.Value) ? long.Parse(hid_link.Value) : -1;
                if (id_link > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_OTROS> lst_rule = da.getLinksById(id_link);
                    if (lst_rule.Count == 1)
                    {
                        /// 2.- Eliminar la regla 
                        delete_link = da.delete_link(id_link);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_link_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_link)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar el link";
        }
        protected void btn_delete_pet_info_Click(object sender, EventArgs e)
        {
            bool delete_pet_info = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_pet_info = !String.IsNullOrEmpty(hid_peticion_info.Value) ? long.Parse(hid_peticion_info.Value) : -1;
                if (id_pet_info > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_ACCIONES_PERSONA> _action = da.getActionsByAP(id_pet_info);
                    if (_action.Count == 1)
                    {
                        /// 2.- Eliminar la regla 
                        delete_pet_info = da.deletePersonAction(id_pet_info);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_pet_info_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
            
            /// Si no hay errores recargar la página
            if (delete_pet_info)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar una petición de información";
        }
        protected void btn_delete_seguimiento_Click(object sender, EventArgs e)
        {
            bool delete_seguimiento = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_seguimiento = !String.IsNullOrEmpty(hid_seguimiento.Value) ? long.Parse(hid_seguimiento.Value) : -1;
                if (id_seguimiento > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_SEG_COMERCIAL> _seguimiento = da.getSeguimientoComercialById(id_seguimiento);
                    if (_seguimiento.Count == 1)
                    {
                        /// 2.- Eliminar la regla 
                        delete_seguimiento = da.deleteSegComercial(id_seguimiento);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_seguimiento_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_seguimiento)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar el seguimiento comercial";
        }
        protected void btn_delete_asig_comercial_Click(object sender, EventArgs e)
        {
            bool delete_asig_comercial = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_curso = !String.IsNullOrEmpty(hid_curso.Value) ? long.Parse(hid_curso.Value) : -1;
                long id_docencia = !String.IsNullOrEmpty(hid_docencia.Value) ? long.Parse(hid_docencia.Value) : -1;
                if (idUsuario > 0 && id_curso > 0 && id_docencia > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_ASIG_COMERCIAL> _asignacion_comercial = da.getAsigComercial(id_docencia, id_curso, idUsuario);
                    if (_asignacion_comercial.Count == 1)
                    {
                        /// 2.- Eliminar la asignación comercial
                        delete_asig_comercial = da.deleteAsigComercial(idUsuario, id_curso, id_docencia);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_asig_comercial_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_asig_comercial)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar la asignación comercial";
        }
        protected void btn_delete_pago_Click(object sender, EventArgs e)
        {
            bool delete_pago = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_pago = !String.IsNullOrEmpty(hid_pago.Value) ? long.Parse(hid_pago.Value) : -1;
                if (id_pago > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_DATA_COMERCIAL> _pago = da.getDataComercialById(id_pago);
                    if (_pago.Count == 1)
                    {
                        /// 2.- Eliminar el pago
                        delete_pago = da.deleteDataComercial(id_pago);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_pago_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_pago)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar el pago";
        }
        protected void btn_delete_documento_Click(object sender, EventArgs e)
        {
            bool delete_documento = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_documento = !String.IsNullOrEmpty(hid_documento.Value) ? long.Parse(hid_documento.Value) : -1;
                if (id_documento > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_CLIENTES_DOC> _documento = da.getDocsClientes(id_documento);
                    if (_documento.Count == 1)
                    {
                        /// 2.- Eliminar el fichero
                        if (!String.IsNullOrEmpty(_documento[0].Fichero))
                        {
                            /// 2.1.- Sacar la ruta del fichero
                            string ruta_fichero = ConfigurationManager.AppSettings["ruta_documentos"] + idUsuario + "\\";

                            /// 2.2.- Eliminar el fichero
                            File.Delete(ruta_fichero + _documento[0].Fichero);
                        }

                        /// 3.- Eliminar el documento
                        delete_documento = da.deleteDocumento(id_documento);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_documento_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_documento)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar el documento";
        }
        protected void btn_delete_tag_Click(object sender, EventArgs e)
        {
            bool update_tag = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                
                if (!String.IsNullOrEmpty(hid_tag.Value))
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<CLIENTES_TAG> _tags = da.getClientTags(idUsuario);
                    _tags = _tags.Where(_ => _.tipo_tag == "general" && _.tag == hid_tag.Value).ToList();
                    if (_tags.Count == 1)
                    {
                        /// 1.- Poner la fecha de baja
                        CLIENTES_TAG _tag = _tags[0];
                        _tag.fecha_baja = DateTime.Now;

                        /// 2.- Actualizar el tag 
                        update_tag = da.updateClientTag(_tag);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_tag_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (update_tag)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar el tag";
        }
        protected void btn_delete_comentario_Click(object sender, EventArgs e)
        {
            bool delete_comentario = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long id_comentario = !String.IsNullOrEmpty(hid_comentario.Value) ? long.Parse(hid_comentario.Value) : -1;
                if (id_comentario > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<CLIENTES_COMENTARIOS> _comentarios = da.getComentariosClientes(id_comentario);
                    if (_comentarios.Count == 1)
                    {
                        /// 2.- Eliminar el comentario
                        delete_comentario = da.deleteComentario(id_comentario);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_delete_comentario_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_comentario)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar el comentario";
        }

        #region Cargas de combos

        private void cargar_combos(List<campus_AUX> _aux)
        {
            cargar_paises();
            cargar_provincias();
            cargar_nivel_estudios(_aux);
            cargar_experiencia(_aux);
            cargar_situacion_actual(_aux);
        }

        private void cargar_paises()
        {
            /// 1.- Pintar los paises
            List<Paises> lst_paises = da.getCountries();
            lst_paises = lst_paises.OrderBy(_ => _.nombre).ToList();
            if (lst_paises.Count > 0)
            {
                /// 1.1.- País de nacimiento
                ddlPaisNac.DataSource = lst_paises;
                ddlPaisNac.DataTextField = "nombre";
                ddlPaisNac.DataValueField = "nombre";
                ddlPaisNac.DataBind();
                ddlPaisNac.Items.Insert(0, new ListItem("Seleccione un país", ""));
                ddlPaisNac.Value = "";

                /// 1.2.- País de residencia
                ddlPaisResidencia.DataSource = lst_paises;
                ddlPaisResidencia.DataTextField = "nombre";
                ddlPaisResidencia.DataValueField = "id_pais";
                ddlPaisResidencia.DataBind();
                ddlPaisResidencia.Items.Insert(0, new ListItem("Seleccione un país", "-1"));
                ddlPaisResidencia.Value = "-1";
            }
        }

        private void cargar_provincias()
        {
            /// 1.- Pintar los paises
            List<Provincias> lst_provincias = da.getProvinceById(-1);
            lst_provincias = lst_provincias.OrderBy(_ => _.nombre).ToList();
            if (lst_provincias.Count > 0)
            {
                /// 1.1.- Provincia de residencia
                ddlProvResidencia.DataSource = lst_provincias;
                ddlProvResidencia.DataTextField = "nombre";
                ddlProvResidencia.DataValueField = "id_provincia";
                ddlProvResidencia.DataBind();
                ddlProvResidencia.Items.Insert(0, new ListItem("Seleccione una provincia", "-1"));
                ddlProvResidencia.Value = "-1";
            }
        }

        private void cargar_nivel_estudios(List<campus_AUX> _aux)
        {
            /// 1.- Pintar los niveles
            List<campus_AUX> _niveles = _aux.Where(_ => _.Tabla.Equals(Constantes.aux.nivel_educacion.GetStringValue())).OrderBy(_ => _.Nombre).ToList();
            if (_niveles.Count > 0)
            {
                ddlNivelEstudios.DataSource = _niveles;
                ddlNivelEstudios.DataTextField = "Nombre";
                ddlNivelEstudios.DataValueField = "ID_Aux";
                ddlNivelEstudios.DataBind();
                ddlNivelEstudios.Items.Insert(0, new ListItem("Seleccione un nivel", "-1"));
                ddlNivelEstudios.Value = "-1";
            }
        }

        private void cargar_experiencia(List<campus_AUX> _aux)
        {
            /// 1.- Pintar las experiencias
            List<campus_AUX> _experiencias = _aux.Where(_ => _.Tabla.Equals(Constantes.aux.experiencia.GetStringValue())).ToList();
            if (_experiencias.Count > 0)
            {
                ddlExperiencia.DataSource = _experiencias;
                ddlExperiencia.DataTextField = "Nombre";
                ddlExperiencia.DataValueField = "ID_Aux";
                ddlExperiencia.DataBind();
                ddlExperiencia.Items.Insert(0, new ListItem("Seleccione una experiencia", "-1"));
                ddlExperiencia.Value = "-1";
            }
        }

        private void cargar_situacion_actual(List<campus_AUX> _aux)
        {
            /// 1.- Pintar las experiencias
            List<campus_AUX> _situaciones = _aux.Where(_ => _.Tabla.Equals(Constantes.aux.situacion.GetStringValue())).ToList();
            if (_situaciones.Count > 0)
            {
                ddlSituacion.DataSource = _situaciones;
                ddlSituacion.DataTextField = "Nombre";
                ddlSituacion.DataValueField = "ID_Aux";
                ddlSituacion.DataBind();
                ddlSituacion.Items.Insert(0, new ListItem("Seleccione una situación laboral", "-1"));
                ddlSituacion.Value = "-1";
            }
        }

        #endregion

        private void cargar_datos(long idUsuario, List<campus_AUX> _aux, CLIENTES _comercial)
        {
            /// 0.- Sacar datos de la BBDD
            List<CLIENTES> _user = da.getUserById(idUsuario);
            if (_user.Count == 1)
            {
                /// 1.- Sacar el resto de los datos de la BBDD
                List<campus_ACCIONES_PERSONA> _actions = da.getActionsByUser(idUsuario);
                List<campus_DOCENCIA> _docencias = da.getDocenciaById(-1);
                List<campus_CURSO> _cursos = da.getAllCourses();
                List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercial(-1, -1, idUsuario);

                /// 2.- Actualizar los mail leídos en los seguimientos
                bool _update_mails = actualizar_mails_leidos(_user[0], _actions);
                if (_update_mails)
                {
                    /// 3.- Cargar los datos del usuario
                    cargar_datos_user(_user[0], _comercial, _aux, _actions);
                    txt_title.InnerHtml = $"<i class='fas fa-user-edit'></i> Datos del usuario <a href='#txt_title_empleos' title='Prácticas y empleos del alumno' class='pull-right px-2'><small><i class='fas fa-briefcase fa-1-4x'></i></small></a><a href='#txt_acciones' title='Acciones del usuario' class='pull-right px-2'><small><i class='fas fa-tasks fa-1-4x'></i></small></a><a href='#txt_documentacion' title='Documentación del alumno' class='pull-right px-2'><small><i class='far fa-file-alt fa-1-4x'></i></small></a><a href='#txt_ventas' title='Ventas' class='pull-right px-2'><small><i class='fas fa-euro-sign fa-1-4x'></i></small></a><a href='#txt_cursos' title='Programas realizados en la escuela' class='pull-right px-2'><small><i class='fas fa-university fa-1-4x'></i></small></a><a href='#txt_peticion_info' title='Listado de peticiones de información' class='pull-right px-2'><small><i class='far fa-list-alt fa-1-4x'></i></small></a>";
                    
                    /// 4.- Cargar las peticiones de información
                    cargar_peticiones_info(_user[0], _comercial, _aux, _actions, _cursos);
                    txt_peticion_info.InnerHtml = $"<i class='far fa-list-alt'></i> Listado de peticiones de información <a href='ficha-alumno-crm-aux.aspx?idu={_user[0].id_cliente}&idta=1' title='Añadir petición información' class='pull-right'><small><i class='fas fa-plus-circle'></i> Añadir petición información</small></a><a href='ficha-alumno-crm-aux.aspx?idu={_user[0].id_cliente}&idta=15' title='Incidencia' class='pull-right pr-2'><small><i class='fas fa-plus-circle'></i> Incidencia</small></a>";

                    /// 5.- Cargar los cursos
                    cargar_cursos(_user[0], _comercial, _docencias);

                    /// 6.- Documentación
                    cargar_documentos(_user[0], _asignaciones, _docencias);
                    txt_documentacion.InnerHtml = $"<i class='far fa-file-alt'></i> Documentación del alumno <a href='ficha-alumno-crm-aux.aspx?idu={_user[0].id_cliente}&idta=10' title='Añadir documento' class='pull-right'><small><i class='fas fa-plus-circle'></i> Añadir documento</small></a>";

                    /// 7.- Bloque especial administrador 
                    if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
                    {
                        chkActivo.Checked = _user[0].activo == ((int)Constantes.activo.Activo).ToString() ? true : false;
                        chkProfesor.Checked = _user[0].Profesor == ((int)Constantes.activo.Activo).ToString() ? true : false;
                        chkAdministrador.Checked = _user[0].Administrador == ((int)Constantes.activo.Activo).ToString() ? true : false;
                        chkComercial.Checked = _user[0].Comercial != null ? _user[0].Comercial.Value : false;
                        chkNoEncuesta.Checked = _user[0].Encuesta == ((int)Constantes.activo.Activo).ToString() ? true : false;
                        chkOnline.Checked = _user[0].online != null ? _user[0].online.Value : false;
                        chkPracticas.Checked = _user[0].practicas != null ? _user[0].practicas.Value : false;
                        chkBajaImpago.Checked = _user[0].Baja_Impago != null ? _user[0].Baja_Impago.Value : false;

                        //txt_administracion.InnerHtml = $"<i class='fas fa-user-cog'></i> Datos del alumno <a href='ficha-alumno-crm-aux.aspx?idu={_user[0].id_cliente}&idta=11' title='Informe tripartita' class='pull-right'><small><i class='fas fa-university'></i> Informe tripartita</small></a>";

                        txt_administracion.InnerHtml = $"<i class='fas fa-user-cog'></i> Datos del alumno <a href='ficha-alumno-crm-aux.aspx?idu={_user[0].id_cliente}&idta=13' title='Avance' class='pl-2 pull-right' target='_blank'><i class='fas fa-user-clock'></i> Avance</a>";

                        //txt_administracion.InnerHtml = $"<i class='fas fa-user-cog'></i> Datos del alumno <a href='ficha-alumno-crm-aux.aspx?idu={_user[0].id_cliente}&idta=13' title='Avance' class='pl-2 pull-right'><small><i class='fas fa-user-clock'></i> Avance</small></a><a href='ficha-alumno-crm-aux.aspx?idu={_user[0].id_cliente}&idta=14' title='Unificar usuarios' class='pull-right'><small><i class='fas fa-users-cog'></i> Unificar usuarios</small></a>";
                    }
                    else
                        blk_admin.Visible = false;

                    /// 8.- Bloque de ventas
                    if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
                    {
                        if (_asignaciones.Count > 0)
                            cargar_ventas(_asignaciones, _docencias, _aux, _user[0], _comercial);
                        else
                            blk_ventas.Visible = false;
                    }
                    else
                        blk_ventas.Visible = false;

                    /// 9.- Bloque de acciones
                    if (_actions.Count > 0)
                        cargar_acciones(_actions, _aux, _docencias, _cursos);
                    else
                        blk_acciones.Visible = false;

                    /// 10.- Bloque de Prácticas y empleos
                    if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
                        cargar_empleos(_user[0]);
                    else
                        blk_empleos.Visible = false;

                    /// 11.- Bloque de exámenes
                    if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
                        cargar_examenes(_user[0], _docencias, _cursos);
                    else
                        blk_examenes.Visible = false;
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al actualizar los mails leídos";
            }
        }

        private bool actualizar_mails_leidos(CLIENTES _user, List<campus_ACCIONES_PERSONA> _actions)
        {
            bool _update_mail = true;

            /// 1.- Sacar los seguimientos del usuario
            List <campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercialByIdUser(_user.id_cliente);

            /// 2.- Sacar los seguimientos de mails con no leído
            _seguimientos = _seguimientos.Where(_ => _.idMail != null && _.accion != null && _.accion.Value == (int)Constantes.type_action_canal.action_mail && (_.leer == null || !_.leer.Value)).ToList();
            if (_seguimientos.Count > 0)
            {
                /// 3.- Recorrer los seguimientos
                foreach (var _seguimiento in _seguimientos)
                {
                    /// 3.1.- Buscar si hay una accion persona de leer el mail
                    int _mail_leido = _actions.Where(_ => _.idLanding == _seguimiento.idMail).Count();
                    if (_mail_leido > 0)
                    {
                        campus_SEG_COMERCIAL _seguimiento_mail = _seguimiento;
                        _seguimiento_mail.leer = true;

                        _update_mail = da.updateSegComercial(_seguimiento_mail);
                        if (!_update_mail)
                            break;
                    }
                }
            }

            return _update_mail;
        }

        private void cargar_datos_user(CLIENTES _user, CLIENTES _comercial, List<campus_AUX> _aux, List<campus_ACCIONES_PERSONA> _actions)
        {
            /// 1.- Pintar los datos personales del usuario
            if (!String.IsNullOrEmpty(_user.Foto))
            {
                foto_user.Src = ConfigurationManager.AppSettings["urlUserPhoto"] + _user.Foto;
                foto_user.Alt = _user.Nombre_Completo;
                txt_foto.Value = _user.Foto;
                delete_photo.Attributes["class"] = delete_photo.Attributes["class"].Replace("hidden", string.Empty);
            }
            else
                upload_photo.Attributes["class"] = upload_photo.Attributes["class"].Replace("hidden", string.Empty);

            txt_nombre.Value = _user.nombre;
            txt_apellidos.Value = _user.apellidos;
            txt_mail.Value = _user.email;
            lnk_mail.HRef = "#";

            txt_telefono.Value = _user.telefono_contacto;
            ws_user.HRef = "https://web.whatsapp.com/send?phone=" + Utils.cleanString(_user.telefono_contacto).Replace(" ", "").Trim();

            if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
            {
                login_user.HRef = "https://www.spainbs.com/miperfil.aspx?idcl=" + _user.Key;
                login_user.Title = _user.login;
                campus_user.HRef = "https://campus.spainbs.com/login.aspx?k=" + _user.Key + "&debug=1";
                if (_user.Profesor == ((int)Constantes.activo.Activo).ToString())
                    campus_pro.HRef = "https://campuspro.spainbs.com/login.aspx?k=" + _user.Key;
                else
                    campus_pro.Visible = false;
            }
            else
            {
                lnk_mail_activacion.Visible = false;
                login_user.Visible = false;
                campus_user.Visible = false;
                campus_pro.Visible = false;
                lnk_change_password.Visible = false;
                lnk_baja.Visible = false;
            }

            txt_mail2.Value = _user.Email2;
            if (!String.IsNullOrEmpty(_user.Email2))
                lnk_mail2.HRef = "#";
            else
                lnk_mail2.Visible = false;
                
            txt_telefono2.Value = _user.telefono;
            if (!String.IsNullOrEmpty(_user.telefono))
                ws2_user.HRef = "https://web.whatsapp.com/send?phone=" + Utils.cleanString(_user.telefono).Replace(" ", "").Trim();
            else
                ws2_user.Visible = false;

            if (_user.fecha_alta != null)
                txtFechaAlta.Value = _user.fecha_alta.Value.ToShortDateString();
            if (_user.fecha_baja != null)
                txtFechaBaja.Value = _user.fecha_baja.Value.ToShortDateString();

            if (!String.IsNullOrEmpty(_user.Nacionalidad))
            {
                List<Paises> list = da.getCountryByName(_user.Nacionalidad);
                if (list.Count > 0)
                    ddlPaisNac.Value = list[0].nombre;
            }

            if (!String.IsNullOrEmpty(_user.Lugar_nac))
                txt_ciudad_nac.Value = _user.Lugar_nac;

            ddlPaisResidencia.Value = _user.id_pais.Value.ToString();
            ddlProvResidencia.Value = _user.id_provincia.Value.ToString();
            if (!String.IsNullOrEmpty(_user.localidad))
                txt_ciudad_residencia.Value = _user.localidad;

            if (!String.IsNullOrEmpty(_user.sexo))
            {
                if (_user.sexo == sex_V.Value)
                    sex_V.Checked = true;
                else if (_user.sexo == sex_M.Value)
                    sex_M.Checked = true;
            }

            if (_user.fecha_nac != null)
                txtFechaNacimiento.Value = _user.fecha_nac.Value.ToShortDateString();
            dni_user.Value = _user.nif;
            passport_user.Value = _user.pasaporte;

            ddlNivelEstudios.Value = _user.Nivel_Estudios.ToString();
            ddlExperiencia.Value = _user.Experiencia.ToString();
            ddlSituacion.Value = _user.Situacion_Actual.ToString();
            sector_user.Value = _user.Sector_Profesional;
            if (_user.Situacion_Actual == (int)Constantes.situacion_laboral.Trabajador || _user.Situacion_Actual == (int)Constantes.situacion_laboral.Emprendedor || _user.Situacion_Actual == (int)Constantes.situacion_laboral.Practicas)
                sector_form_all.Attributes["class"] = sector_form_all.Attributes["class"].Replace("hidden", string.Empty);

            descripcion_user.Value = _user.descripcion_ficha_base;
            comentarios_user.Value = _user.Comentarios_Internos;
            comentario_publico.Value = _user.comentario_publico_alumno;
            
            #region Tipo usuario

            if (_user.Comercial != null && _user.Comercial.Value)
            {
                lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Comercial");
                if (_user.fecha_baja != null)
                {
                    lblStatus.InnerText = "C";                    
                    lblStatus.Attributes["class"] = lblStatus.Attributes["class"].Replace("bg-green", "bg-red");
                }
                else
                {
                    if (_user.activo == ((int)Constantes.activo.Activo).ToString())
                        lblStatus.InnerText = "C";
                    else
                    {
                        lblStatus.InnerText = "C";
                        lblStatus.Attributes["class"] = lblStatus.Attributes["class"].Replace("bg-green", "bg-orange-opacity");
                    }
                }
            }
            else if (_user.Profesor == ((int)Constantes.activo.Activo).ToString())
            {
                lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Profesor");
                if (_user.fecha_baja != null)
                {
                    lblStatus.InnerText = "P";
                    lblStatus.Attributes["class"] = lblStatus.Attributes["class"].Replace("bg-green", "bg-red");
                }
                else
                {
                    if (_user.activo == ((int)Constantes.activo.Activo).ToString())
                        lblStatus.InnerText = "P";
                    else
                    {
                        lblStatus.InnerText = "P";
                        lblStatus.Attributes["class"] = lblStatus.Attributes["class"].Replace("bg-green", "bg-orange-opacity");
                    }
                }
            }
            else
            {
                if (_user.fecha_baja != null)
                {
                    if (_user.fecha_activacion != null)
                    {
                        lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Usuario");
                        lblStatus.InnerText = "U";
                    }
                    else
                    {
                        decimal _pay = da.getDocenciasGrupo(_user.id_cliente).Where(_ => _.PrecioPagado != null).Sum(_ => _.PrecioPagado.Value);
                        if (_pay > 0)
                        {
                            lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Alumno pago");
                            lblStatus.InnerText = "AP";
                        }
                        else
                        {
                            lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Alumno");
                            lblStatus.InnerText = "A";
                        }
                    }
                    lblStatus.Attributes["class"] = lblStatus.Attributes["class"].Replace("bg-green", "bg-red");
                }
                else
                {
                    if (_user.activo == ((int)Constantes.activo.Activo).ToString())
                    {
                        decimal _pay = da.getDocenciasGrupo(_user.id_cliente).Where(_ => _.PrecioPagado != null).Sum(_ => _.PrecioPagado.Value);
                        if (_pay > 0)
                        {
                            lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Alumno pago");
                            lblStatus.InnerText = "AP";
                        }
                        else
                        {
                            lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Alumno");
                            lblStatus.InnerText = "A";
                        }
                    }
                    else
                    {
                        if (_user.fecha_activacion == null)
                        {
                            lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Usuario");
                            lblStatus.InnerText = "U";
                        }
                        else
                        {
                            decimal _pay = da.getDocenciasGrupo(_user.id_cliente).Where(_ => _.PrecioPagado != null).Sum(_ => _.PrecioPagado.Value);
                            if (_pay > 0)
                            {
                                lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Alumno pago");
                                lblStatus.InnerText = "AP";
                            }
                            else
                            {
                                lblStatus.Attributes["title"] = lblStatus.Attributes["title"].Insert(0, "Alumno");
                                lblStatus.InnerText = "A";
                            }
                        }
                        lblStatus.Attributes["class"] = lblStatus.Attributes["class"].Replace("bg-green", "bg-orange-opacity");
                    }
                }
            }

            #endregion

            #region Scoring Global

            List<campus_INVESTIGA> _ci = da.getResearch(_user.id_cliente);
            lblCookies.InnerHtml = _ci.Count().ToString();
            lblScoring.InnerHtml = _ci.Where(_ => _.Scoring != null).Sum(_ => _.Scoring.Value).ToString();

            #endregion

            #region Links

            List<campus_OTROS> _links = da.getLinks(_user.id_cliente);
            if (_links.Count > 0)
            {
                StringBuilder sbuild = new StringBuilder();

                /// 2.0.- Pintar el título del bloque de origenes
                sbuild.Append($"<label class='w-100'><i class='fas fa-link'></i> Redes sociales y links personales <a href='ficha-alumno-crm-aux.aspx?idu={_user.id_cliente}&idta=3' title='Añadir origen'><small><i class='fas fa-plus-circle'></i> Añadir link</small></a></label>");

                /// 2.1.- Recorrer los links
                foreach (var _link in _links)
                {
                    sbuild.Append("<div class='btn-group dropup px-3 py-2'>");

                    if (_link.Tipo == (int)Constantes.links.Linkedin)
                        sbuild.Append($"<a class='fab fa-linkedin-in fa-1-6x c-pointer' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'></a>");
                    else if (_link.Tipo == (int)Constantes.links.Twitter)
                        sbuild.Append($"<a class='fab fa-twitter fa-1-6x c-pointer' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'></a>");
                    else if (_link.Tipo == (int)Constantes.links.Facebook)
                        sbuild.Append($"<a class='fab fa-facebook-square fa-1-6x c-pointer' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'></a>");
                    else if (_link.Tipo == (int)Constantes.links.Blog)
                        sbuild.Append($"<a class='fas fa-blog fa-1-6x c-pointer' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'></a>");
                    else if (_link.Tipo == (int)Constantes.links.Web || _link.Tipo == (int)Constantes.links.Trabajo)
                        sbuild.Append($"<a class='fas fa-globe fa-1-6x c-pointer' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'></a>");
                    
                    sbuild.Append("<div class='dropdown-menu'>");
                    sbuild.Append($"<a href='{_link.URL}' title='Ver' class='pl-3' target='_blank'><i class='fas fa-eye'></i></a>");
                    sbuild.Append($"<a href='ficha-alumno-crm-aux.aspx?idu={_user.id_cliente}&idta=3&idl={_link.ID_Otros}' title='Editar' class='px-3'><i class='fas fa-edit'></i></a>");
                    sbuild.Append("<a href='javascript:void(0)' onclick='if (confirm_message(\"¿Desea eliminar el link?\")){eliminar_link(" + _link.ID_Otros + ")}' title='Eliminar'><i class='fas fa-trash-alt'></i></a>");
                    sbuild.Append("</div></div>");
                }

                blk_links.InnerHtml = sbuild.ToString();
            }
            else
                blk_links.InnerHtml = $"<label class='w-100'><i class='fas fa-link'></i> Redes sociales y links personales <a href='ficha-alumno-crm-aux.aspx?idu={_user.id_cliente}&idta=3' title='Añadir origen'><small><i class='fas fa-plus-circle'></i> Añadir link</small></a></label>";

            #endregion

            #region Origenes

            /// 2.- Pintar los origenes del usuario
            List<campus_ACCIONES_PERSONA> _origenes = _actions.Where(_ => _.IdOrigen != null).OrderBy(_ => _.Fecha).ToList();
            if (_origenes.Count > 0)
            {
                StringBuilder sbuild = new StringBuilder();

                /// 2.0.- Pintar el título del bloque de origenes
                sbuild.Append($"<label class='w-100'><i class='fas fa-info-circle'></i> Origenes <a href='ficha-alumno-crm-aux.aspx?idu={_user.id_cliente}&idta=2' title='Añadir origen'><small><i class='fas fa-plus-circle'></i> Añadir origen</small></a></label>");

                /// 2.1.- Quitar los origenes procesados
                List<long> _ids_procesados = new List<long>();

                /// 2.2.- Pintar el primer y último origen
                if (_origenes.Where(_ => _.IdOrigen != null).Select(_ => _.IdOrigen.Value).Distinct().Count() > 1)
                {
                    campus_ACCIONES_PERSONA _first_origin = _origenes.Where(_ => _.IdOrigen != null).First();
                    campus_ACCIONES_PERSONA _last_origin = _origenes.Where(_ => _.IdOrigen != null).Last();

                    _ids_procesados.Add(_first_origin.IdOrigen.Value);
                    _ids_procesados.Add(_last_origin.IdOrigen.Value);

                    sbuild.Append("<span title='Primer origen' class='badge badge-warning text-white'>" + _aux.Where(_ => _.ID_Aux == _first_origin.IdOrigen.Value).Select(_ => _.Nombre).FirstOrDefault() + " " + _first_origin.Fecha + "</span>");
                    sbuild.Append(" <span title='Último origen' class='badge badge-success'>" + _aux.Where(_ => _.ID_Aux == _last_origin.IdOrigen.Value).Select(_ => _.Nombre).FirstOrDefault() + " " + _last_origin.Fecha + "</span>");
                }
                else if (_origenes.Where(_ => _.IdOrigen != null).Select(_ => _.IdOrigen.Value).Distinct().Count() == 1)
                {
                    campus_ACCIONES_PERSONA _last_origin = _origenes.Where(_ => _.IdOrigen != null).Last();
                    _ids_procesados.Add(_last_origin.IdOrigen.Value);
                    sbuild.Append(" <span title='Último origen' class='badge badge-success'>" + _aux.Where(_ => _.ID_Aux == _last_origin.IdOrigen.Value).Select(_ => _.Nombre).FirstOrDefault() + " " + _last_origin.Fecha + "</span>");
                }

                /// 3.- Sacar los ids de los origenes
                List<long> _ids_origenes = _origenes.Where(_ => !_ids_procesados.Contains(_.IdOrigen.Value)).Select(_ => _.IdOrigen.Value).Distinct().ToList();
                if (_ids_origenes.Count > 0)
                {
                    /// 3.1.- Recorrer los origenes
                    foreach (var _origen in _ids_origenes)
                    {
                        sbuild.Append(" <span class='badge badge-light'>" + _aux.Where(_ => _.ID_Aux == _origen).Select(_ => _.Nombre).FirstOrDefault() + "</span>");
                    }
                }
                blk_origins.InnerHtml = sbuild.ToString();
            }
            else
                blk_origins.InnerHtml = $"<label class='w-100'><i class='fas fa-info-circle'></i> Origenes <a href='ficha-alumno-crm-aux.aspx?idu={_user.id_cliente}&idta=2' title='Añadir origen'><small><i class='fas fa-plus-circle'></i> Añadir origen</small></a></label>";

            #endregion

            #region Tags

            /// 3.- Sacar los Tags del usuario
            List<CLIENTES_TAG> _tags = da.getClientTags(_user.id_cliente);

            /// 3.1.- Filtrar los tags que sean tipo_tag='general'
            List<CLIENTES_TAG> _tags_general = _tags.Where(_ => _.tipo_tag == "general" && _.fecha_baja == null).ToList();

            /// 3.2.- Pintar los tags
            blk_tags.InnerHtml = paint_tags(_tags_general);
            
            /// 3.3.- Filtrar los tags que sean tipo_tag='general'
            List <CLIENTES_TAG> _tags_geo_ip = _tags.Where(_ => _.tipo_tag == "geo_ip").ToList();
            if (_tags_geo_ip.Count == 1)
            {
                txt_geo_ip.InnerHtml = _tags_geo_ip[0].tag;
                txt_date_geo_ip.InnerHtml = _tags_geo_ip[0].fecha_ult_act.ToString("dd MMMM yyyy");
            }
            else
            {
                txt_geo_ip.InnerHtml = "&nbsp;";
                txt_date_geo_ip.InnerHtml = "&nbsp;";
            }

            #endregion

            #region Comentarios

            /// 2.- Sacar los comentarios de la BBDD
            List<CLIENTES_COMENTARIOS> _comentarios = da.getComentariosClientesById(_user.id_cliente);
            if (_comentarios.Count > 0)
            {
                /// 2.0.- Pintar el título
                txt_title_comentarios.InnerHtml = $"<i class='far fa-comments'></i> Comentarios <a href='ficha-alumno-crm-aux.aspx?idu={_user.id_cliente}&idta=12' title='Añadir comentario' class='pull-right'><small><i class='fas fa-plus-circle'></i> Añadir comentario</small></a>";

                /// 2.1.- Pintar los comentarios
                table_comentarios.InnerHtml = paint_table_comentarios(_user.id_cliente, _comercial, _comentarios);
            }
            else
                txt_title_comentarios.InnerHtml = $"<i class='far fa-comments'></i> Comentarios <a href='ficha-alumno-crm-aux.aspx?idu={_user.id_cliente}&idta=12' title='Añadir comentario' class='pull-right'><small><i class='fas fa-plus-circle'></i> Añadir comentario</small></a>";

            #endregion
        }

        private string paint_table_comentarios(long id_cliente, CLIENTES _comercial, List<CLIENTES_COMENTARIOS> _comentarios)
        {
            /// 0.- Inicializar tablas
            List<long> _ids = _comentarios.Select(_ => _.idComercial).Distinct().ToList();
            List<CLIENTES> _comerciales = da.getUserByList(_ids);
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Pintar la tabla
            sbuild.Append("<table id =\"tabla_comentarios\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 1.1.- Pintar la cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Quien</th>");
            sbuild.Append("<th>Comentario</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 2.2.- Recorrer los comentarios
            foreach (var _comentario in _comentarios)
            {
                sbuild.Append("<tr>");
                sbuild.Append($"<td>{_comentario.Fecha}</td>");
                sbuild.Append($"<td>{_comerciales.Where(_ => _.id_cliente == _comentario.idComercial).Select(_ => _.Nombre_Completo).FirstOrDefault()} ({_comentario.idComercial})</td>");
                sbuild.Append($"<td>{_comentario.Comentario}</td>");
                if (_comentario.idComercial == _comercial.id_cliente)
                {
                    sbuild.Append($"<td><a href='ficha-alumno-crm-aux.aspx?idu={_comentario.idUsuario}&idc={_comentario.idComentario}&idta=12' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el comentario?\")){eliminar_comentario(" + _comentario.idComentario + ")}' title='Eliminar comentario'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                }
                else
                {
                    sbuild.Append($"<td></td>");
                    sbuild.Append($"<td></td>");
                }
                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        #region Peticiones de información

        private void cargar_peticiones_info(CLIENTES _user, CLIENTES _comercial, List<campus_AUX> _aux, List<campus_ACCIONES_PERSONA> _actions, List<campus_CURSO> _cursos)
        {
            /// 0.- Sacar las acciones
            List<long> _id_act = new List<long>();
            List<string> list_act = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["list_actions"]) ? ConfigurationManager.AppSettings["list_actions"].Split(',').ToList() : new List<string>();
            foreach (var act in list_act)
            {
                _id_act.Add(long.Parse(act));
            }

            /// 2.- Sacar los leads de la BBDD
            List<campus_ACCIONES_PERSONA> _leads = _actions.Where(_ => _id_act.Contains(_.idAccion)).ToList();

            /// 2.1.- Filtrar los leads incorrectos 243 y curso 22
            List<campus_ACCIONES_PERSONA> _leads_incorrectos = _leads.Where(_ => _.idAccion == (int)Constantes.accion.Exito_Landing && _.IdCurso != null && _.IdCurso == (int)Constantes.course.Sin_determinar).ToList();

            /// 2.2.- Filtrar los leads
            _leads = _leads.Except(_leads_incorrectos).ToList();

            /// 2.3.- Filtrar por comercial
            /*if (_comercial.Comercial != null && _comercial.Comercial.Value)
                _leads = _leads.Where(_ => _.idComercial == _comercial.id_cliente).ToList();
            else
                _leads = _leads.Where(_ => _.idComercial == null).ToList();*/

            /// 2.3.- Actualizar el scoring de los leads
            int num_days = int.Parse(ConfigurationManager.AppSettings["dias_antes_scoring_lead"]);
            _leads = actualizar_scoring(_leads, _actions, num_days);

            /// 2.4.- Pintar la tabla
            if (_leads.Count > 0)
            {
                /// 2.5.- Sacar los datos de la BBDD
                List<Paises> _countries = da.getCountries();

                table_pet_info.InnerHtml = paint_table_leads(_leads, _user, _cursos, _aux, _countries, _comercial);
            }
            else
                block_pet_inf.Visible = false;
        }

        private List<campus_ACCIONES_PERSONA> actualizar_scoring(List<campus_ACCIONES_PERSONA> _leads, List<campus_ACCIONES_PERSONA> _actions, int num_days)
        {
            List<campus_ACCIONES_PERSONA> _leads_actualizados = new List<campus_ACCIONES_PERSONA>();

            /// 1.- Recorrer los leads
            foreach (var _lead in _leads)
            {
                if (_lead.estado_lead != (int)Constantes.type_status_action.status_duplicado && _lead.estado_lead != (int)Constantes.type_status_action.status_rechazado && _lead.estado_lead != (int)Constantes.type_status_action.status_cerrar && _lead.estado_lead != (int)Constantes.type_status_action.status_matriculado && _lead.IdCurso != (long)Constantes.course.Incidencia)
                {
                    DateTime _fecha = DateTime.Parse(_lead.Fecha.ToShortDateString()).AddDays(-num_days);
                    int _scoring = _actions.Where(_ => _.Fecha >= _fecha && _.Scoring != null).Sum(_ => _.Scoring.Value);

                    campus_ACCIONES_PERSONA _lead_act = _lead;
                    _lead_act.scoring_lead = _scoring;

                    bool _update_lead = da.updatePersonAction(_lead_act);
                    if (_update_lead)
                        _leads_actualizados.Add(_lead_act);
                    else
                        _leads_actualizados.Add(_lead);
                }
                else
                    _leads_actualizados.Add(_lead);
            }

            return _leads_actualizados;
        }

        private string paint_table_leads(List<campus_ACCIONES_PERSONA> _leads, CLIENTES _user, List<campus_CURSO> _cursos, List<campus_AUX> _aux, List<Paises> _countries, CLIENTES _comercial)
        {
            /// 0.- Inicializar tablas            
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Sacar los seguimientos y comerciales de la BBDD
            List<long> _ids_ap = _leads.Select(_ => _.idAccionPersona).Distinct().ToList();
            List<campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercial(_ids_ap);
            List<long> _ids_comerciales = _leads.Where(_ => _.idComercial != null).Select(_ => _.idComercial.Value).ToList();
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (_ids_comerciales.Count > 0)
                _comerciales = da.getUserByList(_ids_comerciales);
            List<campus_ASIG_COMERCIAL> _asignaciones = da.getAsigComercial(-1, -1, _user.id_cliente);

            /// 2.- Pintar la tabla
            sbuild.Append("<table id =\"tabla_AP\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 3.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Estado</th>");
            sbuild.Append("<th>Ult. Comentario</th>");
            sbuild.Append("<th>Scoring</th>");
            sbuild.Append("<th>Resumen</th>");
            sbuild.Append("<th title='Fecha último seguimento'>F. Seg.</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 4.- Pintar las AP
            foreach (var _ap in _leads)
            {
                if (_ap.Procesado != null && _ap.Procesado.Value)
                {
                    if (_ap.test_interno != null && _ap.test_interno.Value)
                        sbuild.Append("<tr class='text-color-primary'>");
                    else
                        sbuild.Append("<tr>");
                }
                else
                    sbuild.Append("<tr class='text-color-red'>");
                sbuild.Append($"<td><span class='hidden'>{_ap.idAccionPersona}</span></td>");
                sbuild.Append($"<td>{_ap.Fecha}</td>");
                sbuild.Append($"<td>{(_ap.IdOrigen != null ? _aux.Where(_ => _.ID_Aux == _ap.IdOrigen).Select(_ => _.Nombre).FirstOrDefault() + " (" + _ap.IdOrigen + ")" : string.Empty)}</td>");

                if (_ap.IdCurso == (long)Constantes.course.Incidencia)
                    sbuild.Append($"<td><span class='badge badge-pill badge-danger'>{(_ap.IdCurso != null ? _cursos.Where(_ => _.ID_Curso == _ap.IdCurso).Select(_ => _.Nombre).FirstOrDefault() + " (" + _ap.IdCurso + ")" : string.Empty)}</span></td>");
                else
                    sbuild.Append($"<td>{(_ap.IdCurso != null ? _cursos.Where(_ => _.ID_Curso == _ap.IdCurso).Select(_ => _.Nombre).FirstOrDefault() + " (" + _ap.IdCurso + ")" : string.Empty)}</td>");
                sbuild.Append($"<td>{(_ap.idComercial != null ? _comerciales.Where(_ => _.id_cliente == _ap.idComercial).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _ap.idComercial + ")" : string.Empty)}</td>");
                if (_ap.estado_lead != null)
                {
                    if (_ap.estado_lead == (int)Constantes.type_status_action.status_nuevo)
                        sbuild.Append($"<td><span class='badge badge-pill badge bg-primary text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Nuevo'>Nuevo</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_rechazado)
                        sbuild.Append($"<td><span class='badge badge-pill badge-purple' data-toggle='tooltip' data-placement='top' data-html='true' title='Rechazado'>Rechazado</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_duplicado)
                        sbuild.Append($"<td><span class='badge badge-pill badge-purple' data-toggle='tooltip' data-placement='top' data-html='true' title='Duplicado'>Duplicado</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_futuro)
                        sbuild.Append($"<td><span class='badge badge-pill badge-primary' data-toggle='tooltip' data-placement='top' data-html='true' title='Futuro'>Futuro</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_cerrar)
                        sbuild.Append($"<td><span class='badge badge-pill badge-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Cerrado'>Cerrado</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_sin_contactar)
                        sbuild.Append($"<td><span class='badge badge-pill badge-danger' data-toggle='tooltip' data-placement='top' data-html='true' title='Sin contactar'>Sin contactar</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_indeciso)
                        sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Indeciso'>Indeciso</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_interesado)
                        sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Interesado'>Interesado</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_send)
                        sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Envio contrato'>Envio contrato</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_receive)
                        sbuild.Append($"<td><span class='badge badge-pill badge-yellow text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Recibir contrato'>Recibir contrato</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_pago)
                        sbuild.Append($"<td><span class='badge badge-pill badge-yellow text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Pago'>Pago</span></td>");
                    else if (_ap.estado_lead == (int)Constantes.type_status_action.status_matriculado)
                        sbuild.Append($"<td><span class='badge badge-pill badge-success' data-toggle='tooltip' data-placement='top' data-html='true' title='Matriculado'>Matriculado</span></td>");
                    else
                        sbuild.Append("<td></td>");
                }
                else
                    sbuild.Append($"<td></td>");
                
                sbuild.Append($"<td>{_ap.ult_comentario}</td>");
                sbuild.Append($"<td>{_ap.scoring_lead}</td>");
                sbuild.Append($"<td>{(!String.IsNullOrEmpty(_ap.resumen_metodologia) ? "<span class='badge badge-pill badge-info' data-toggle='tooltip' data-placement='top' data-html='true' title='Metodología'>" + _ap.resumen_metodologia + "</span>" : string.Empty)} {(!String.IsNullOrEmpty(_ap.resumen_cuando) ? "<span class='badge badge-pill badge-warning' data-toggle='tooltip' data-placement='top' data-html='true' title='Cuando'>" + _ap.resumen_cuando + "</span>" : string.Empty)} {(_ap.resumen_beca != null ? "<span class='badge badge-pill badge-success' data-toggle='tooltip' data-placement='top' data-html='true' title='Beca'>" + _ap.resumen_beca + "</span>" : string.Empty)} {(_ap.resumen_descuento != null ? "<span class='badge badge-pill badge-success' data-toggle='tooltip' data-placement='top' data-html='true' title='Descuento'>" + _ap.resumen_descuento + "</span>" : string.Empty)} {(!String.IsNullOrEmpty(_ap.resumen_comentarios) ? "<span class='badge badge-pill badge-secondary badge-comments' data-toggle='tooltip' data-placement='top' data-html='true' title='Comentarios'>" + _ap.resumen_comentarios + "</span>" : string.Empty)}</td>");
                sbuild.Append($"<td>{_ap.ult_seg_fecha}</td>");

                /// Seguimiento comercial o procesar recordatorio
                if (_ap.Comentario == "Recordatorio automático")
                    sbuild.Append($"<td><a href='javascript:void(0)' onclick='procesar_pet_info({_ap.idAccionPersona})' title='Procesar petición'><i class=\"fas fa-plus-circle fa-1-6x\"></i></a></td>");
                else if (_ap.IdCurso != null && _ap.IdOrigen != null)
                    sbuild.Append($"<td><a href='seguimiento-comercial-crm.aspx?idu={_ap.idPersona}&idap={_ap.idAccionPersona}' title='Nuevo seguimiento'><i class='fas fa-plus-circle fa-1-6x'></i></a></td>");
                else
                    sbuild.Append($"<td></td>");

                /// Asignación comercial
                List<campus_SEG_COMERCIAL> _seguimientos_ap = _seguimientos.Where(s => s.idAccionPersona == _ap.idAccionPersona).ToList();
                if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
                {
                    DateTime _fecha_lead = DateTime.Parse(_ap.Fecha.ToShortDateString());
                    if (_seguimientos_ap.Count > 0 && (_ap.estado_lead == null || _ap.estado_lead == (int)Constantes.type_status_action.status_matriculado) && _asignaciones.Where(_ => _.idOrigen == _ap.IdOrigen && _.Fecha_Lead == _fecha_lead).Count() == 0)
                        sbuild.Append($"<td><a title='Asignación Comercial' href='ficha-alumno-crm-aux.aspx?idu={_ap.idPersona}&idap={_ap.idAccionPersona}&idc={_ap.IdCurso}&idta=7'><i class='fas fa-briefcase fa-1-6x'></i></a></td>");
                    else if (_seguimientos_ap.Count > 0 && (_ap.estado_lead == null || _ap.estado_lead == (int)Constantes.type_status_action.status_matriculado) && _asignaciones.Where(_ => _.idOrigen == _ap.IdOrigen && _.Fecha_Lead == _fecha_lead).Count() == 1)
                        sbuild.Append($"<td><a title='Asignación Comercial' href='ficha-alumno-crm-aux.aspx?idu={_ap.idPersona}&idap={_ap.idAccionPersona}&idd={_asignaciones.Where(_ => _.idOrigen == _ap.IdOrigen && _.Fecha_Lead == _fecha_lead).Select(_ => _.idDocencia).First()}&idc={_ap.IdCurso}&idta=7'><i class='fas fa-briefcase text-color-green fa-1-6x'></i></a></td>");
                    else
                        sbuild.Append($"<td></td>");
                }
                else
                    sbuild.Append($"<td></td>");

                /// Fundación
                if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
                {
                    if (_ap.Comentario != "Recordatorio automático" && _ap.resumen_beca == null && _ap.resumen_descuento == null)
                        sbuild.Append($"<td><a href='ficha-alumno-crm-aux.aspx?idu={_ap.idPersona}&idta=4&idap={_ap.idAccionPersona}' title='Fundación'><i class='fas fa-school fa-1-6x'></i></a></td>");
                    else
                        sbuild.Append($"<td></td>");
                }
                else
                    sbuild.Append($"<td></td>");

                /// Editar
                if (_ap.Comentario != "Recordatorio automático")
                    sbuild.Append($"<td><a href='ficha-alumno-crm-aux.aspx?idu={_ap.idPersona}&idta=1&idap={_ap.idAccionPersona}' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                else
                    sbuild.Append($"<td></td>");

                /// Borrar
                if (_seguimientos_ap.Count == 0)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la petición de información?\")){eliminarPeticionInfo(" + _ap.idAccionPersona + ")}' title='Eliminar la petición de información'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        #endregion

        #region Cursos

        private void cargar_cursos(CLIENTES _user, CLIENTES _comercial, List<campus_DOCENCIA> _docencias)
        {
            /// 1.- Sacar los cursos del alumno
            List<campus_DOCENCIA_GRUPO> _dg = da.getDocenciasGrupo(_user.id_cliente);
            List<campus_CONTENIDO_PROGRAMA> _programas = da.getContenidoPrograma();

            /// 2.- Pintar la tabla
            if (_dg.Count > 0)
            {
                /// 2.1.- Filtrar las docencias
                List<long> _ids = _dg.Select(_ => _.ID_Docencia).ToList();
                _docencias = _docencias.Where(_ => _ids.Contains(_.ID_Docencia)).ToList();

                /// 2.2.- Pintar la tabla de los cursos realizados
                table_list_cursos.InnerHtml = paint_table_courses(_user, _comercial, _dg, _docencias, _programas);

                /// 2.3.- Poner la fecha de último acceso 
                List<long> _id_students = new List<long>();
                List<campus_LOG> list_logs = da.getLogEntries(_id_students);
                last_access.InnerHtml = (list_logs.Where(l => l.ID_Persona == _user.id_cliente).Count() > 0 ? list_logs.Where(l => l.ID_Persona == _user.id_cliente).Max(l => l.Fecha_Hora).ToShortDateString() : "&nbsp;");

                /// 2.4.- Si es administrador, ver el informe de acceso
                if (_comercial.Administrador == ((int)Constantes.activo.Activo).ToString())
                    inf_acceso.HRef = "ficha-alumno-crm-aux.aspx?idu=" + _user.id_cliente + "&idta=11";
                else
                    inf_acceso.Visible = false;
            }
            else
                blk_cursos.Visible = false;
        }

        private string paint_table_courses(CLIENTES _user, CLIENTES _comercial, List<campus_DOCENCIA_GRUPO> _dg, List<campus_DOCENCIA> _docencias, List<campus_CONTENIDO_PROGRAMA> _programas)
        {
            /// 0.- Inicializar tablas
            List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluaciones = new List<campus_DOCENCIA_GRUPO_EVALUACION>();
            List<UNI_ALUMNOS> _student_university = new List<UNI_ALUMNOS>();
            List<CVE_CERTIFICADO> _certificados = new List<CVE_CERTIFICADO>();
            List<campus_ENCUESTA> _encuestas = new List<campus_ENCUESTA>();            
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Pintar la tabla
            sbuild.Append("<table id =\"tabla_cursos\" class=\"display compact\" style =\"width:100%\"><thead>");
            
            if (_comercial.Comercial != null && _comercial.Comercial.Value)
            {
                /// 1.1.- Pintar la cabecera
                sbuild.Append("<tr>");
                sbuild.Append("<th>Fecha Inicio</th>");
                sbuild.Append("<th>Fecha Fin</th>");
                sbuild.Append("<th>Programa</th>");
                sbuild.Append("<th>Fecha Mat.</th>");
                sbuild.Append("</tr>");
                sbuild.Append("</thead><tbody>");

                /// 2.2.- Recorrer las docencias
                foreach (var _matricula in _dg)
                {
                    List<campus_DOCENCIA> _docencia = _docencias.Where(_ => _.ID_Docencia == _matricula.ID_Docencia).ToList();
                    
                    sbuild.Append("<tr>");
                    sbuild.Append($"<td>{(_docencia[0].FInicio != null ? _docencia[0].FInicio.Value.ToShortDateString() : string.Empty)}</td>");
                    sbuild.Append($"<td>{(_docencia[0].FFin != null ? _docencia[0].FFin.Value.ToShortDateString() : string.Empty)}</td>");
                    sbuild.Append($"<td>{_docencia[0].Nombre}</td>");
                    sbuild.Append($"<td>{(_matricula.FechaAlta != null ? _matricula.FechaAlta.Value.ToShortDateString() : string.Empty)}</td>");
                    sbuild.Append("</tr>");
                }
            }
            else
            {
                /// 2.- Sacar datos de la BBDD
                _evaluaciones = da.getDocenciasGrupoEvaluacion(_user.id_cliente);
                _student_university = da.getUniversityStudents(_user.id_cliente);
                _certificados = da.getCertificatesByUser(_user.id_cliente);
                _encuestas = da.getEncuestasByUser(_user.id_cliente);
                int _num_programas_all = _programas.Where(_ => _docencias.Select(_d => _d.ID_Curso).Distinct().ToList().Contains(_.ID_Programa)).Count();

                /// 3.- Pintar la cabecera
                sbuild.Append("<tr>");
                sbuild.Append("<th>Fecha Inicio</th>");
                sbuild.Append("<th>Fecha Fin</th>");
                sbuild.Append("<th>Programa</th>");
                sbuild.Append("<th>Fecha Mat.</th>");
                sbuild.Append("<th>Estado</th>");
                sbuild.Append("<th>Euros</th>");
                if (_certificados.Count > 0)
                    sbuild.Append("<th></th>");
                //sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                if (_docencias.Where(_ => _.PDP == null || !_.PDP.Value).Count() > 0)
                    sbuild.Append("<th></th>");
                if (_num_programas_all >= 1)
                    sbuild.Append("<th></th>");
                if (_num_programas_all >= 1)
                    sbuild.Append("<th></th>");
                if (_student_university.Count > 0)
                    sbuild.Append("<th></th>");                
                sbuild.Append("</tr>");
                sbuild.Append("</thead><tbody>");

                /// 3.1.- Recorrer las docencias
                foreach (var _matricula in _dg)
                {
                    List<campus_DOCENCIA> _docencia = _docencias.Where(_ => _.ID_Docencia == _matricula.ID_Docencia).ToList();
                    List<campus_DOCENCIA_GRUPO_EVALUACION> _evaluacion = _evaluaciones.Where(_ => _.idDocencia == _matricula.ID_Docencia && _.idCurso == _docencia[0].ID_Curso).ToList();
                    List<campus_ENCUESTA> _encuesta = _encuestas.Where(_ => _.ID_Docencia == _matricula.ID_Docencia && _.ID_Curso == _docencia[0].ID_Curso).ToList();
                    List<CVE_CERTIFICADO> _certificado = _certificados.Where(_ => _.idCurso == _docencia[0].ID_Curso).ToList();
                    int _num_programas = _programas.Where(_ => _.ID_Programa == _docencia[0].ID_Curso).Count();

                    if (_docencia.Count == 1)
                    {
                        sbuild.Append("<tr>");
                        sbuild.Append($"<td>{(_docencia[0].FInicio != null ? _docencia[0].FInicio.Value.ToShortDateString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_docencia[0].FFin != null ? _docencia[0].FFin.Value.ToShortDateString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{_docencia[0].Nombre}</td>");
                        sbuild.Append($"<td>{(_matricula.FechaAlta != null ? _matricula.FechaAlta.Value.ToShortDateString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_evaluacion.Count == 1 ? _evaluacion[0].calificacion : string.Empty)} {(_evaluacion.Count == 1 ? "(" + _evaluacion[0].nota + ")" : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_matricula.PrecioPagado != null ? Utilities.PonerPuntoMiles(_matricula.PrecioPagado.Value) : "0")}€</td>");
                        if (_certificados.Count > 0)
                            sbuild.Append($"<td>{(_certificado.Count == 1 ? "<a title='Ver CVE' href='https://cve.spainbs.com/Certificado.aspx?cve=" + Utils.toCodeString(_certificado[0].CVE) + "' target='_blank'><i class='fas fa-scroll fa-1-6x text-color-green'></i></a>" : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_encuesta.Count == 1 ? "<a title='Encuesta' href='https://campus.spainbs.com/encuesta.aspx?idd=" + _matricula.ID_Docencia + "&idc=" + _docencia[0].ID_Curso + "&idu=" + _user.id_cliente + "&key=" + ConfigurationManager.AppSettings["Key_Encuesta"] + "' target='_blank'><i class='fas fa-comments fa-1-6x text-color-green'></i></a>" : "<i title='Encuesta' class='fas fa-comments fa-1-6x text-color-primary'></i>")}</td>");
                        sbuild.Append($"<td><a title='Calificar' href='https://campuspro.spainbs.com/calificar.aspx?idd={_matricula.ID_Docencia}&idc={_docencia[0].ID_Curso}&idu={_user.id_cliente}&k={_comercial.Key}'><i class='fas fa-graduation-cap fa-1-6x'></i></a></td>");
                        //sbuild.Append($"<td><a title='Asignación Comercial' href='#'><i class='fas fa-briefcase fa-1-6x'></i></a></td>");
                        sbuild.Append($"<td><a title='Ficha Docencia' href='https://campusadmin.spainbs.com/admin/Admin_Gestion_Docencia.aspx?idc={_matricula.ID_Docencia}'><i class='fas fa-book fa-1-6x'></i></a></td>");
                        sbuild.Append($"<td><a title='Campus' href='https://campus.spainbs.com/curso.aspx?idd={_matricula.ID_Docencia}&idc={_docencia[0].ID_Curso}&k={_user.Key}&debug=1' target='_blank'><i class='fas fa-globe fa-1-6x'></i></a></td>");
                        sbuild.Append($"<td><a title='Editar precio' href='ficha-alumno-crm-aux.aspx?idd={_matricula.ID_Docencia}&idu={_user.id_cliente}&idta=6'><i class='fas fa-coins fa-1-6x'></i></a></td>");
                        sbuild.Append("<td><a href='javascript:void(0)' title='Eliminar docencia' onclick='if(confirm_message(\"¿Desea eliminar la docencia: " + _docencia[0].Nombre + "?\")){eliminar_docencia(" + _docencia[0].ID_Docencia + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                        if (_docencias.Where(_ => _.PDP == null || !_.PDP.Value).Count() > 0)
                        {
                            if (_docencia[0].PDP == null || !_docencia[0].PDP.Value)
                                sbuild.Append($"<td><a title='Cambiar edición' href='ficha-alumno-crm-aux.aspx?idd={_matricula.ID_Docencia}&idu={_user.id_cliente}&idta=5'><i class='far fa-calendar-check fa-1-6x'></i></a></td>");
                            else
                                sbuild.Append("<td></td>");
                        }

                        //   Aqui he estado tocando   MACEDRUN     los > 1 por >=1   y comentarios

                        /// Solicitar diploma
                        sbuild.Append($"<td><a href='javascript:void(0)' onclick='procesar_pet_diploma({_matricula.ID_Docencia},{_docencia[0].ID_Curso})' title='Solicitar diploma PDF auto CVE'><i class=\"fa-solid fa-file fa-1-6x\"></i></a></td>");
                        
                        //if (_num_programas_all >= 1)
                        //{
                            string _params = "idd=" + _matricula.ID_Docencia + "&idu=" + _user.id_cliente + "&cert=1&date=" + DateTime.Now.AddSeconds(60).ToString();
                            sbuild.Append($"<td>{(_num_programas >= 1 && _docencia[0].idAsociado == null ? "<a title='Generar certificado' href='https://10.10.100.206/certificado-academico.aspx?certificate=" + Utils.toCodeString(_params) + "'><i class='fas fa-scroll fa-1-6x'></i></a>" : string.Empty)}</td>");
                        //}
                        //if (_num_programas_all >= 1)
                        //{
                        //    string _params = "idd=" + _matricula.ID_Docencia + "&idu=" + _user.id_cliente + "&cert=1&date=" + DateTime.Now.AddSeconds(60).ToString();
                            sbuild.Append($"<td>{(_num_programas >= 1 && _docencia[0].idAsociado == null ? "<a title='Generar título' href='https://10.10.100.206/titulo-academico.aspx?certificate=" + Utils.toCodeString(_params) + "'><i class='fas fa-regular fa-clipboard fa-1-6x'></i></a>" : string.Empty)}</td>");
                        //}
                        if (_student_university.Count > 0)
                        {
                            //string _params = "idd=" + _matricula.ID_Docencia + "&idu=" + _user.id_cliente + "&cert=1&date=" + DateTime.Now.AddSeconds(60).ToString();
                            sbuild.Append($"<td>{(_student_university.Where(_ => _.id_docencia_sbs == _matricula.ID_Docencia).Count() == 1 ? "<a title='Generar certificado universidad' href='https://10.10.100.206/certificado-academico-universidad.aspx?certificate=" + Utils.toCodeString(_params) + "'><i class='fas fa-university fa-1-6x'></i></a>" : string.Empty)}</td>");
                        }
                        sbuild.Append("</tr>");
                    }
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        protected void btn_solicitar_diploma_Click(object sender, EventArgs e)
        {
            bool _process = false;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;

            try
            {
                long idDocencia = !String.IsNullOrEmpty(hid_idDocencia.Value) ? long.Parse(hid_idDocencia.Value) : -1;
                long idCurso = !String.IsNullOrEmpty(hid_idCurso.Value) ? long.Parse(hid_idCurso.Value) : -1;

                if (idUsuario > 0 && idDocencia > 0 && idCurso > 0)
                {
                    /// 1.- Comprobar si el curso esta marcado como generar_documento
                    List<campus_CURSO> _cursos = da.getCourseById(idCurso);
                    if (_cursos.Count == 1)
                    {
                        bool generar_diploma = _cursos[0].generar_titulo != null ? _cursos[0].generar_titulo.Value : false;
                        if (!generar_diploma)
                        {
                            var curso = _cursos[0];
                            if (curso != null)
                            {
                                curso.generar_titulo = true;
                                da.updateCursoGenerarDiploma(curso);
                            }
                        }
                    }

                    /// 2.- Añadir la petición del diploma
                    CVE_PETICION _petition = new CVE_PETICION();
                    _petition.idUsuario = idUsuario;
                    _petition.idDocencia = idDocencia;
                    _petition.idCurso = idCurso;
                    _petition.fecha_peticion = DateTime.Now;

                    long insert_petition = da.insertPetitionCVE(_petition);
                    if (insert_petition > 0)
                        _process = true;
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-alumno-crm.cs::btn_solicitar_diploma_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (_process)
                Response.Redirect("ficha-alumno-crm.aspx?idu=" + idUsuario);
            else
                txt_error.InnerHtml = "Se ha producido un error al Añadir la petición del diploma";
        }

        #endregion

        #region Ventas

        private void cargar_ventas(List<campus_ASIG_COMERCIAL> _asignaciones, List<campus_DOCENCIA> _docencias, List<campus_AUX> _aux, CLIENTES _user, CLIENTES _comercial)
        {
            /// 1.- Sacar las docencias y los comerciales
            List<long> _ids_docencias = _asignaciones.Select(_ => _.idDocencia).Distinct().ToList();
            List<long> _ids_comerciales = _asignaciones.Where(_ => _.idVendedor != null).Select(_ => _.idVendedor.Value).Distinct().ToList();

            /// 2.- Sacar los pagos
            List<campus_DATA_COMERCIAL> _datas = da.getDataComercialByIdDocencia(_ids_docencias);

            /// 3.- Sacar los comerciales
            List<CLIENTES> _comerciales = da.getUserByList(_ids_comerciales);

            /// 4.- Filtrar las docencias
            _docencias = _docencias.Where(_ => _ids_docencias.Contains(_.ID_Docencia)).ToList();

            /// 5.- Pintar la tabla de las ventas
            table_list_ventas.InnerHtml = paint_table_ventas(_asignaciones, _docencias, _aux, _comerciales, _datas, _user, _comercial);
        }

        private string paint_table_ventas(List<campus_ASIG_COMERCIAL> _asignaciones, List<campus_DOCENCIA> _docencias, List<campus_AUX> _aux, List<CLIENTES> _users, List<campus_DATA_COMERCIAL> _datas, CLIENTES _user, CLIENTES _comercial)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar el resto de datos
            List<campus_AUX> _origins = _aux.Where(_ => _.Tabla.Equals(Constantes.aux.origen.GetStringValue())).ToList();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Matriculas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>F. Lead</th>");
            sbuild.Append("<th>F. Mat.</th>");
            sbuild.Append("<th>F. Pago</th>");
            sbuild.Append("<th>LTV</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>€ Total</th>");
            sbuild.Append("<th>€ Pte</th>");
            sbuild.Append("<th>€ Prog.</th>");
            sbuild.Append("<th>€ Fund.</th>");
            sbuild.Append("<th>€ Univ.</th>");
            sbuild.Append("<th>Comentarios</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer las matriculas
            foreach (var _matricula in _asignaciones)
            {
                if (_matricula.Fecha_Matricula != null && _matricula.Fecha_Env_Contrato != null)
                {
                    if (_matricula.Fecha_Matricula.Value.AddDays(15) < DateTime.Today && _matricula.Fecha_Env_Contrato.Value.AddDays(15) < DateTime.Today)
                        sbuild.Append($"<tr class='text-color-green'><td><span class='hidden'>{_matricula.idAlumno},{_matricula.idCurso},{_matricula.idDocencia}</span></td>");
                    else if (_matricula.Fecha_Matricula.Value.AddDays(15) >= DateTime.Today && _matricula.Fecha_Env_Contrato.Value.AddDays(15) < DateTime.Today)
                        sbuild.Append($"<tr class='text-color-orange'><td><span class='hidden'>{_matricula.idAlumno},{_matricula.idCurso},{_matricula.idDocencia}</span></td>");
                    else
                        sbuild.Append($"<tr class='text-color-red'><td><span class='hidden'>{_matricula.idAlumno},{_matricula.idCurso},{_matricula.idDocencia}</span></td>");
                }
                else
                    sbuild.Append($"<tr class='text-color-red'><td><span class='hidden'>{_matricula.idAlumno},{_matricula.idCurso},{_matricula.idDocencia}</span></td>");

                sbuild.Append("<td>" + (_matricula.Fecha_Lead != null ? _matricula.Fecha_Lead.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Env_Contrato != null ? _matricula.Fecha_Env_Contrato.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + (_matricula.Fecha_Matricula != null ? _matricula.Fecha_Matricula.Value.ToShortDateString() : string.Empty) + "</td>");
                if (_matricula.Fecha_Matricula != null && _matricula.Fecha_Lead != null)
                    sbuild.Append("<td>" + _matricula.Fecha_Matricula.Value.Subtract(_matricula.Fecha_Lead.Value).Days + "</td>");
                else
                    sbuild.Append("<td></td>");
                sbuild.Append("<td>" + _origins.Where(_ => _.ID_Aux == _matricula.idOrigen).Select(_ => _.Nombre).FirstOrDefault() + " (" + _matricula.idOrigen + ")</td>");
                sbuild.Append("<td>" + _users.Where(_ => _.id_cliente == _matricula.idVendedor).Select(_ => _.Nombre_Completo).FirstOrDefault() + " (" + _matricula.idVendedor + ")</td>");
                sbuild.Append("<td>" + _docencias.Where(_ => _.ID_Docencia == _matricula.idDocencia).Select(_ => _.Nombre).FirstOrDefault() + " (" + _matricula.idDocencia + ")</td>");
                sbuild.Append("<td>" + (_user.online != null && _user.online.Value ? "ONLINE" : "SEMIP") + "</td>");                
                
                decimal _pvp = _matricula.EUR_PVP_Becado != null ? _matricula.EUR_PVP_Becado.Value : 0;
                decimal _fundacion = _matricula.EUR_Aportacion_Fundacion != null ? _matricula.EUR_Aportacion_Fundacion.Value : 0;
                decimal _universidad = _matricula.EUR_Universidad != null ? _matricula.EUR_Universidad.Value : 0;
                decimal _real = _datas.Where(_ => _.idAlumno == _matricula.idAlumno && _.idDocencia == _matricula.idDocencia && _.EUR_real != null).Sum(_ => _.EUR_real).Value;
                decimal _estimado = _datas.Where(_ => _.idAlumno == _matricula.idAlumno && _.idDocencia == _matricula.idDocencia).Sum(_ => _.EUR_est);
                decimal _pendiente = _pvp + _fundacion + _universidad - _real;

                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp + _fundacion + _universidad) + "</td>");
                sbuild.Append("<td>" + (_pendiente == 0 ? Utilities.PonerPuntoMiles(_pendiente) : "<span class='badge badge-pill badge-danger'>" + Utilities.PonerPuntoMiles(_pendiente) + "</span>") + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_pvp) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_fundacion) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(_universidad) + "</td>");
                sbuild.Append("<td>" + _matricula.Comentarios + "</td>");

                if ((_pvp + _fundacion + _universidad - _estimado) != 0)
                    sbuild.Append($"<td><a href='ficha-alumno-crm-aux.aspx?idu={_matricula.idAlumno}&idc={_matricula.idCurso}&idd={_matricula.idDocencia}&idta=9' title='Añadir pago'><i class='fas fa-plus-circle fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td></td>");
                sbuild.Append($"<td><a href='ficha-alumno-crm-aux.aspx?idu={_matricula.idAlumno}&idc={_matricula.idCurso}&idd={_matricula.idDocencia}&idta=8' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                if (_real == new decimal(0))
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la asignación comercial?\")){eliminar_asignacion_comercial(" + _matricula.idCurso + "," + _matricula.idDocencia + ")}' title='Eliminar la petición de información'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append("</tr>");
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        #endregion

        #region Documentación

        private void cargar_documentos(CLIENTES _user, List<campus_ASIG_COMERCIAL> _asignaciones, List<campus_DOCENCIA> _docencias)
        {
            /// 1.- Sacar los documentos del alumno
            List<campus_CLIENTES_DOC> _documentos = da.getDocsClientesById(_user.id_cliente);
            if (_documentos.Count > 0 || _asignaciones.Count > 0)
                table_documentos.InnerHtml = paint_table_documentos(_user.id_cliente, _documentos, _asignaciones, _docencias);
            else
                block_documentos.Visible = false;
        }

        private string paint_table_documentos(long id_cliente, List<campus_CLIENTES_DOC> _documentos, List<campus_ASIG_COMERCIAL> _asignaciones, List<campus_DOCENCIA> _docencias)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar la ruta de los ficheros
            string _ruta_fichero = ConfigurationManager.AppSettings["url_documentos"] + id_cliente + "/";

            /// 1.- Pintar la tabla
            sbuild.Append("<table id =\"tabla_documentos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 1.1.- Pintar la cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Descripción</th>");
            sbuild.Append("<th>Fichero</th>");
            sbuild.Append("<th>Fecha Caducidad</th>");
            sbuild.Append("<th>Docencia</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 2.- Recorrer los documentos
            if (_documentos.Count > 0)
            {
                foreach (var _documento in _documentos)
                {
                    sbuild.Append("<tr>");
                    sbuild.Append($"<td>{_documento.tipo_documento}</td>");
                    if (_documento.tipo_documento != "Url")
                    {
                        sbuild.Append($"<td>{_documento.Descripcion}</td>");
                        sbuild.Append($"<td>{(!String.IsNullOrEmpty(_documento.Fichero) ? "<a href='" + _ruta_fichero + _documento.Fichero + "' target='_blank'><i class='far fa-file-archive fa-1-6x text-color-green'></i></a>" : "<i class='far fa-file-archive fa-1-6x'></i>")}</td>");
                    }
                    else
                    {
                        sbuild.Append($"<td>Carpeta disco virtual</td>");
                        sbuild.Append($"<td><a href='" + _documento.Descripcion + "' target='_blank'><i class='fas fa-globe fa-1-6x'></i></a></td>");
                    }
                    sbuild.Append($"<td>{(_documento.fecha_caducidad != null ? _documento.fecha_caducidad.Value.ToShortDateString() : string.Empty)}</td>");
                    sbuild.Append($"<td>{(_documento.id_docencia != null ? _docencias.Where(_ => _.ID_Docencia == _documento.id_docencia).Select(_ => _.Nombre).FirstOrDefault() + " (" + _documento.id_docencia + ")" : string.Empty)}</td>");
                    sbuild.Append($"<td><a title='Editar documento' href='ficha-alumno-crm-aux.aspx?idu={_documento.idCliente}&idd={_documento.idClienteDoc}&idta=10'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                    sbuild.Append("<td><a href='javascript:void(0)' title='Eliminar documento' onclick='if(confirm_message(\"¿Desea eliminar el documento?\")){eliminar_documento(" + _documento.idClienteDoc + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                    sbuild.Append($"</tr>");
                }
            }

            /// 3.- Recorrer las asignaciones comerciales
            if (_asignaciones.Count > 0)
            {
                foreach (var _documento in _asignaciones)
                {
                    if (!String.IsNullOrEmpty(_documento.PDF_Contrato))
                    {
                        sbuild.Append("<tr>");
                        sbuild.Append($"<td></td>");
                        sbuild.Append($"<td>Pdf Contrato</td>");
                        sbuild.Append($"<td><a href='" + _ruta_fichero + _documento.PDF_Contrato + "' target='_blank'><i class='far fa-file-archive fa-1-6x text-color-green'></i></a></td>");
                        sbuild.Append($"<td></td>");
                        sbuild.Append($"<td>{(_docencias.Where(_ => _.ID_Docencia == _documento.idDocencia).Select(_ => _.Nombre).FirstOrDefault() + " (" + _documento.idDocencia + ")")}</td>");
                        sbuild.Append("<td></td>");
                        sbuild.Append("<td></td>");
                        sbuild.Append($"</tr>");
                    }

                    if (!String.IsNullOrEmpty(_documento.PDF_Factura))
                    {
                        sbuild.Append("<tr>");
                        sbuild.Append($"<td></td>");
                        sbuild.Append($"<td>Pdf Factura</td>");
                        sbuild.Append($"<td><a href='" + _ruta_fichero + _documento.PDF_Factura + "' target='_blank'><i class='far fa-file-archive fa-1-6x text-color-green'></i></a></td>");
                        sbuild.Append($"<td></td>");
                        sbuild.Append($"<td>{(_docencias.Where(_ => _.ID_Docencia == _documento.idDocencia).Select(_ => _.Nombre).FirstOrDefault() + " (" + _documento.idDocencia + ")")}</td>");
                        sbuild.Append("<td></td>");
                        sbuild.Append("<td></td>");
                        sbuild.Append($"</tr>");
                    }
                }
            }

            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        #endregion

        #region Acciones

        private void cargar_acciones(List<campus_ACCIONES_PERSONA> _actions, List<campus_AUX> _aux, List<campus_DOCENCIA> _docencias, List<campus_CURSO> _cursos)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar datos de la BBDD
            List<Paises> _paises = da.getCountries();

            /// 1.- Pintar la tabla
            sbuild.Append("<table id =\"tabla_acciones\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 1.1.- Pintar la cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Acción</th>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>País</th>");
            sbuild.Append("<th>Host</th>");
            sbuild.Append("<th>Referer</th>");
            sbuild.Append("<th>Página</th>");
            sbuild.Append("<th>Comentario</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 2.- Recorrer las acciones
            if (_actions.Count > 0)
            {
                foreach (var _accion in _actions)
                {
                    if (_accion.idAccion == (long)Constantes.accion.Dar_baja)
                        sbuild.Append("<tr class='text-color-red'>");
                    else if (_accion.idAccion == (long)Constantes.accion.Carrito)
                        sbuild.Append("<tr class='text-color-primary'>");
                    else if (_accion.idAccion == (long)Constantes.accion.Compra || _accion.idAccion == (long)Constantes.accion.Compra2 || _accion.idAccion == (long)Constantes.accion.Compra3 || _accion.idAccion == (long)Constantes.accion.Compra4)
                        sbuild.Append("<tr class='text-color-primary font-weigth-bold'>");
                    else if (_accion.idAccion == (long)Constantes.accion.Peticion_informacion || _accion.idAccion == (long)Constantes.accion.Informacion_general_becas)
                        sbuild.Append("<tr class='text-color-green'>");
                    else if (_accion.idAccion == (long)Constantes.accion.Matriculacion)
                        sbuild.Append("<tr class='text-color-green font-weigth-bold'>");
                    else if (_accion.idAccion == (long)Constantes.accion.Landing)
                        sbuild.Append("<tr class='text-color-orange'>");
                    else if (_accion.idAccion == (long)Constantes.accion.Exito_Landing)
                        sbuild.Append("<tr class='text-color-orange font-weigth-bold'>");
                    else if (_accion.idAccion == (long)Constantes.accion.Send_Mail || _accion.idAccion == (long)Constantes.accion.Open_Mail || _accion.idAccion == (long)Constantes.accion.Click_Mail || _accion.idAccion == (long)Constantes.accion.Download_File)
                        sbuild.Append("<tr class='text-color-black font-weigth-bold'>");
                    else
                        sbuild.Append("<tr>");

                    sbuild.Append($"<td>{_accion.Fecha}</td>");
                    sbuild.Append($"<td>{(_accion.IdOrigen != null ? _aux.Where(_ => _.ID_Aux == _accion.IdOrigen).Select(_ => _.Nombre).FirstOrDefault() + " (" + _accion.IdOrigen + ")" : string.Empty)}</td>");
                    sbuild.Append($"<td>{_aux.Where(_ => _.ID_Aux == _accion.idAccion).Select(_ => _.Nombre).FirstOrDefault()} ({_accion.idAccion})</td>");
                    sbuild.Append($"<td>{(_accion.IdCurso != null ? _cursos.Where(_ => _.ID_Curso == _accion.IdCurso).Select(_ => _.Nombre).FirstOrDefault() + " (" + _accion.IdCurso + ")" : string.Empty)}</td>");
                    sbuild.Append($"<td>{(_accion.idPais != null ? _paises.Where(_ => _.id_pais == _accion.idPais).Select(_ => _.nombre).FirstOrDefault() + " (" + _accion.idPais + ")" : string.Empty)}</td>");
                    sbuild.Append($"<td>{_accion.Http_Host}</td>");
                    sbuild.Append($"<td>{(!String.IsNullOrEmpty(_accion.Http_Referer) && _accion.Http_Referer.Length > 30 ? "<span data-toggle='tooltip' data-placement='top' data-html='true' title='" + _accion.Http_Referer + "'>" + _accion.Http_Referer.Substring(0, 30) + " ...</span>" : _accion.Http_Referer)}</td>");
                    sbuild.Append($"<td>{(!String.IsNullOrEmpty(_accion.Http_URL) && _accion.Http_URL.Length > 30 ? "<span data-toggle='tooltip' data-placement='top' data-html='true' title='" + _accion.Http_URL + "'>" + _accion.Http_URL.Substring(0, 30) + " ...</span>" : _accion.Http_URL)}</td>");
                    sbuild.Append($"<td>{(!String.IsNullOrEmpty(_accion.Comentario) && _accion.Comentario.Length > 30 ? "<span data-toggle='tooltip' data-placement='top' data-html='true' title='" + _accion.Comentario + "'>" + _accion.Comentario.Substring(0, 30) + " ...</span>" : _accion.Comentario)}</td>");
                    sbuild.Append($"</tr>");
                }
            }

            sbuild.Append("</tbody></table>");

            table_list_acciones.InnerHtml = sbuild.ToString();
        }

        #endregion

        #region Prácticas y empleos

        private void cargar_empleos(CLIENTES _user)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar datos de la BBDD
            List<PRA_PRACTICAS> _practicas = da.getListPracticesByIdUser(_user.id_cliente);
            List<PRA_EMPLEO> _empleos = da.getWorksByIdUser(_user.id_cliente);

            /// 1.- Pintar el título
            txt_title_empleos.InnerHtml = $"<i class='fas fa-briefcase'></i> Prácticas y empleos del alumno <a href='practica-mantenimiento.aspx' title='Práctica' class='pull-right pl-2' target='_blank'><small><i class='fas fa-plus-circle'></i> Nueva práctica</small></a> <a href='empleo-mantenimiento.aspx' title='Empleo' class='pull-right' target='_blank'><small><i class='fas fa-plus-circle'></i> Nuevo empleo</small></a>";
            
            /// 2.- Comprobar que tenga prácticas o empleos
            if (_practicas.Count > 0 || _empleos.Count > 0)
            {
                /// 3.- Sacar los datos de los tutores de la escuela y las empresas
                List<long> _ids_tutores = _practicas.Select(_ => _.idTutorEscuela).Distinct().ToList();
                List<CLIENTES> _tutores = new List<CLIENTES>();
                if (_ids_tutores.Count > 0)
                    _tutores = da.getUserByList(_ids_tutores);
                List<long> _ids_empresas = _practicas.Select(_ => _.idEmpresa).Distinct().ToList();
                _ids_empresas = _ids_empresas.Union(_empleos.Select(_ => _.idEmpresa).Distinct().ToList()).Distinct().ToList();
                List<PRA_EMPRESA> _empresas = da.getBusinessById(_ids_empresas);

                /// 4.- Pintar la tabla
                sbuild.Append("<table id =\"tabla_empleos\" class=\"display compact\" style =\"width:100%\"><thead>");

                /// 4.1.- Pintar la cabecera
                sbuild.Append("<tr>");
                sbuild.Append("<th>Fecha Alta</th>");
                sbuild.Append("<th>Fecha Baja</th>");
                sbuild.Append("<th>Tipo</th>");
                sbuild.Append("<th>Empresa</th>");
                sbuild.Append("<th>Tutor Escuela</th>");
                sbuild.Append("<th>Datos</th>");
                sbuild.Append("<th></th>");
                sbuild.Append("</tr>");
                sbuild.Append("</thead><tbody>");

                /// 5.- Recorrer las prácticas
                if (_practicas.Count > 0)
                {
                    foreach (var _practica in _practicas)
                    {
                        sbuild.Append("<tr>");
                        sbuild.Append($"<td>{_practica.FechaAlta.ToShortDateString()}</td>");
                        sbuild.Append($"<td>{(_practica.FechaBaja != null ? _practica.FechaBaja.Value.ToShortDateString() : string.Empty)}</td>");
                        sbuild.Append($"<td>PRÁCTICA</td>");
                        sbuild.Append($"<td><i class='fas fa-building'></i> {_empresas.Where(_ => _.idEmpresa == _practica.idEmpresa).Select(_ => _.RazonSocial).FirstOrDefault()} ({_practica.idEmpresa})</td>");
                        sbuild.Append($"<td><i class='fas fa-user-tie'></i> {_tutores.Where(_ => _.id_cliente == _practica.idTutorEscuela).Select(_ => _.Nombre_Completo).FirstOrDefault()} ({_practica.idTutorEscuela})</td>");
                        sbuild.Append($"<td>Duración: {_practica.Duracion}<br />Ayuda: {_practica.AyudaEstudioMes}€<br />Horas: {_practica.HoraSemana}</td>");
                        sbuild.Append($"<td><a href=\"practica-mantenimiento.aspx?idp={_practica.idPractica}\" title=\"Editar\"><i class=\"fas fa-edit fa-1-6x\"></i></a></td>");
                        sbuild.Append("</tr>");
                    }
                }

                /// 6.- Recorrer los empleos
                if (_empleos.Count > 0)
                {
                    foreach (var _empleo in _empleos)
                    {
                        sbuild.Append("<tr>");
                        sbuild.Append($"<td>{_empleo.FechaAlta.ToShortDateString()}</td>");
                        sbuild.Append($"<td></td>");
                        sbuild.Append($"<td>EMPLEO</td>");
                        sbuild.Append($"<td><i class='fas fa-building'></i> {_empresas.Where(_ => _.idEmpresa == _empleo.idEmpresa).Select(_ => _.RazonSocial).FirstOrDefault()} ({_empleo.idEmpresa})</td>");
                        sbuild.Append($"<td></td>");
                        sbuild.Append($"<td>{_empleo.Contrato}<br />{_empleo.Comentarios}</td>");
                        sbuild.Append($"<td><a href=\"empleo-mantenimiento.aspx?ide={_empleo.idEmpleo}\" title=\"Editar\"><i class=\"fas fa-edit fa-1-6x\"></i></a></td>");
                        sbuild.Append("</tr>");
                    }
                }    

                sbuild.Append("</tbody></table>");
                table_empleos.InnerHtml = sbuild.ToString();
            }
        }

        #endregion

        #region Exámenes

        private void cargar_examenes(CLIENTES _user, List<campus_DOCENCIA> _docencias, List<campus_CURSO> _cursos)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar datos de la BBDD
            List<campus_DOCENCIA_EXAMEN> _examenes = da.getDocenciaExamenByIdUser(_user.id_cliente);
            List<Test_Examen> _tests = da.getTest_ExamenByIdTestAlumno(_user.id_cliente);

            /// 1.- Pintar el título
            txt_title_examenes.InnerHtml = $"<i class='fas fa-tasks'></i> Exámenes del alumno";

            /// 2.- Comprobar que tenga exámenes
            if (_examenes.Count > 0 || _tests.Count > 0)
            {
                /// 3.- Pintar la tabla
                sbuild.Append("<table id =\"tabla_examenes\" class=\"display compact\" style =\"width:100%\"><thead>");

                /// 3.1.- Pintar la cabecera
                sbuild.Append("<tr>");
                sbuild.Append("<th>Fecha</th>");
                sbuild.Append("<th>Título</th>");
                sbuild.Append("<th>Docencia</th>");
                sbuild.Append("<th>Curso</th>");
                sbuild.Append("<th>Fecha Inicio</th>");
                sbuild.Append("<th>Fecha Fin</th>");
                sbuild.Append("<th>Apto</th>");
                sbuild.Append("<th>Nota</th>");
                sbuild.Append("<th></th>");
                sbuild.Append("</tr>");
                sbuild.Append("</thead><tbody>");

                /// 4.- Recorrer los exámenes
                if (_examenes.Count > 0)
                {
                    /// 4.1.- Sacar los cursos de los exámenes
                    List<long> _idCursos = _examenes.Select(_ => _.ID_Curso_Examen).Distinct().ToList();
                    List<campus_CURSO_EXAMEN> _cursos_examenes = da.getExamenByListIdExam(_idCursos);

                    foreach (var _examen in _examenes)
                    {
                        /// 4.2.- Sacar el idCurso
                        long idCurso = _cursos_examenes.Where(_ => _.ID_Curso_Examen == _examen.ID_Curso_Examen).Select(_ => _.ID_Curso).FirstOrDefault();

                        sbuild.Append("<tr>");
                        sbuild.Append($"<td>{(_examen.Fecha.HasValue ? _examen.Fecha.Value.ToShortDateString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{_cursos_examenes.Where(_ => _.ID_Curso_Examen == _examen.ID_Curso_Examen).Select(_ => _.Titulo).FirstOrDefault()}</td>");
                        sbuild.Append($"<td>{_docencias.Where(_ => _.ID_Docencia == _examen.ID_Docencia).Select(_ => _.Nombre).FirstOrDefault()} ({_examen.ID_Docencia})</td>");
                        sbuild.Append($"<td>{_cursos.Where(_ => _.ID_Curso == idCurso).Select(_ => _.Nombre).FirstOrDefault()} ({idCurso})</td>");
                        sbuild.Append($"<td>{(_examen.Hora_Inicio.HasValue ? _examen.Hora_Inicio.Value.ToString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_examen.Hora_Fin.HasValue ? _examen.Hora_Fin.Value.ToString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_examen.Apto == (int)Constantes.activo.Activo ? "<i class='far fa-check-square text-color-primary fa-1-6x'></i>" : string.Empty)}</td>");
                        sbuild.Append($"<td>{_examen.Puntuacion}</td>");
                        sbuild.Append($"<td><a title='Ver examen' href='https://campus.spainbs.com/contenido-examen.aspx?idd={_examen.ID_Docencia}&idc={idCurso}&ide=" + Utils.toCodeString(_examen.ID_Docencia_Examen + "&see=1") + "&key=FBB14E99-6361-4A57-B325-526AAC88DE9D&idu=" + _examen.ID_Persona + "' target='_blank'><i class='fas fa-tasks fa-1-6x v-top text-color-green'></i></td>");
                        sbuild.Append("</tr>");
                    }
                }

                /// 5.- Recorrer los tests
                if (_tests.Count > 0)
                {
                    /// 5.1.- Sacar los ids
                    List<int> _idTests = _tests.Select(_ => _.Id_Test).Distinct().ToList();
                    List<Test_Test> _test_data = da.getTest_TestsByList(_idTests);

                    foreach (var _test in _tests)
                    {
                        sbuild.Append("<tr>");
                        sbuild.Append($"<td>{_test.Fecha.ToShortDateString()}</td>");
                        sbuild.Append($"<td>{_test_data.Where(_ => _.Id == _test.Id_Test).Select(_ => _.Nombre).FirstOrDefault()}</td>");
                        sbuild.Append($"<td>{_docencias.Where(_ => _.ID_Docencia == _test.Id_Docencia).Select(_ => _.Nombre).FirstOrDefault()} ({_test.Id_Docencia})</td>");
                        sbuild.Append($"<td>{_cursos.Where(_ => _.ID_Curso == _test.Id_Curso).Select(_ => _.Nombre).FirstOrDefault()} ({_test.Id_Curso})</td>");
                        sbuild.Append($"<td>{(_test.FechaHora_Inicio.HasValue ? _test.FechaHora_Inicio.Value.ToString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_test.FechaHora_Fin.HasValue ? _test.FechaHora_Fin.Value.ToString() : string.Empty)}</td>");
                        sbuild.Append($"<td>{(_test.Apto.HasValue && _test.Apto.Value ? "<i class='far fa-check-square text-color-primary fa-1-6x'></i>" : string.Empty)}</td>");
                        sbuild.Append($"<td>{_test.Nota_Normalizada}</td>");
                        sbuild.Append($"<td><a href='https://campus.spainbs.com/contenido-examen-smart.aspx?it={_test.Id_Test}&ni={_test.Num_Intento}&key=FBB14E99-6361-4A57-B325-526AAC88DE9D&idu={_test.Id_Alumno}' title='Ver test' target='_blank'><i class='fas fa-tasks fa-1-6x v-top text-color-green'></i></a></td>");
                        sbuild.Append("</tr>");
                    }
                }             

                sbuild.Append("</tbody></table>");
                table_examenes.InnerHtml = sbuild.ToString();
            }
        }

        #endregion

        [WebMethod(Description = "Busca las provincias de la BBDD a partir de un país dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Provinces> searchProvinces(string country)
        {
            DataAccess da = new DataAccess();
            List<Provinces> _lst_provincias = new List<Provinces>();
            
            if (!String.IsNullOrEmpty(country))
            {
                int idCountry = int.Parse(country);

                List<int> _countries = new List<int>();
                _countries.Add((int)Constantes.pais.Spain);
                _countries.Add((int)Constantes.pais.France);
                _countries.Add((int)Constantes.pais.Germany);
                _countries.Add((int)Constantes.pais.UK);

                if (!_countries.Contains(idCountry))
                    idCountry = (int)Constantes.pais.Others;

                List<Provincias> _provincias = da.getProvinceByIdCountry(idCountry);
                _lst_provincias = _provincias.Select(_ => new Provinces { nombre = _.nombre, valor = _.id_provincia.ToString() }).OrderBy(_ => _.nombre).ToList();
            }
            return _lst_provincias;
        }

        #region Métodos para buscar los seguimientos comerciales de un AP

        [WebMethod(Description = "Recupera la tabla de seguimientos comerciales")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable_ap(long id)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            /// 1.- Sacar los seguimientos
            List<long> _id_ap = new List<long>();
            _id_ap.Add(id);
            List<campus_SEG_COMERCIAL> _seguimientos = da.getSeguimientoComercial(_id_ap);

            /// 2.- Sacar los datos de los comerciales 
            List<CLIENTES> _comerciales = new List<CLIENTES>();
            if (_seguimientos.Count > 0)
            {
                List<long> _ids_comerciales = _seguimientos.Select(_ => _.idComercial).Distinct().ToList();
                _comerciales = da.getUserByList(_ids_comerciales);
            }

            /// 1.- Recuperar la tabla de los seguimientos
            list.Add(paint_table_seg(_seguimientos, _comerciales));

            return list;
        }

        private static string paint_table_seg(List<campus_SEG_COMERCIAL> _seguimientos, List<CLIENTES> _comerciales)
        {
            DataAccess da = new DataAccess();
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los mails que no tengan cuerpo mail      
            List<EMAIL_CONTENT> _mails_users = new List<EMAIL_CONTENT>();
            List<long> _ids_mails = _seguimientos.Where(_ => _.idMail != null && String.IsNullOrEmpty(_.Cuerpo_mail)).Select(_ => _.idMail.Value).ToList();
            if (_ids_mails.Count > 0)
                _mails_users = da.getMailByList(_ids_mails);

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_AP_Seg\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Estado</th>");
            sbuild.Append("<th>Comercial</th>");
            sbuild.Append("<th>Comentarios</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Pintar los seg
            foreach (var _seguimiento in _seguimientos)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + _seguimiento.fecha + "</td>");

                if (_seguimiento.accion == (int)Constantes.type_action_canal.action_mail)
                {
                    if (_seguimiento.leer != null && _seguimiento.leer.Value)
                    {
                        if (!String.IsNullOrEmpty(_seguimiento.Cuerpo_mail))
                            sbuild.Append("<td><a href='javascript:void(0)' onclick=\"add_cuerpo_mail(`" + _seguimiento.Cuerpo_mail.Replace('"', '\'') + "`, '" + limpiar_adjuntos(_seguimiento.Adjuntos_mail) + "')\"><i class='fas fa-envelope fa-1-6x text-color-green'></i></a></td>");
                        else
                            sbuild.Append("<td><a href='javascript:void(0)' onclick=\"add_cuerpo_mail(`" + (_mails_users.Where(_ => _.id_ec == _seguimiento.idMail).Select(_ => _.body).Count() > 0 ? _mails_users.Where(_ => _.id_ec == _seguimiento.idMail).Select(_ => _.body).FirstOrDefault().Replace('"', '\'').Replace("https://www.spainbs.com/controls/pixel.ashx", string.Empty) : string.Empty) + "`, '" + limpiar_adjuntos(_seguimiento.Adjuntos_mail) + "')\"><i class='fas fa-envelope fa-1-6x text-color-green'></i></a></td>");
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_seguimiento.Cuerpo_mail))
                            sbuild.Append("<td><a href='javascript:void(0)' onclick=\"add_cuerpo_mail(`" + _seguimiento.Cuerpo_mail.Replace('"', '\'') + "`, '" + limpiar_adjuntos(_seguimiento.Adjuntos_mail) + "')\"><i class='fas fa-envelope fa-1-6x text-color-primary'></i></a></td>");
                        else
                            sbuild.Append("<td><a href='javascript:void(0)' onclick=\"add_cuerpo_mail(`" + (_mails_users.Where(_ => _.id_ec == _seguimiento.idMail).Select(_ => _.body).Count() > 0 ? _mails_users.Where(_ => _.id_ec == _seguimiento.idMail).Select(_ => _.body).FirstOrDefault().Replace('"', '\'').Replace("https://www.spainbs.com/controls/pixel.ashx", string.Empty) : string.Empty) + "`, '" + limpiar_adjuntos(_seguimiento.Adjuntos_mail) + "')\"><i class='fas fa-envelope fa-1-6x text-color-primary'></i></a></td>");
                    }
                }
                else if (_seguimiento.accion == (int)Constantes.type_action_canal.action_phone)
                    sbuild.Append("<td><i class='fas fa-phone-square-alt fa-1-6x text-color-primary'></i></td>");
                else if (_seguimiento.accion == (int)Constantes.type_action_canal.action_whatsapp)
                    sbuild.Append("<td><i class='fab fa-whatsapp fa-1-6x text-color-primary'></i></td>");
                else if (_seguimiento.accion == (int)Constantes.type_action_canal.action_advise)
                    sbuild.Append("<td><i class='fas fa-bell fa-1-6x text-color-secondary'></i></td>");
                else if (_seguimiento.tipo == Constantes.tipo_accion_comercial.Fundacion.GetStringValue())
                    sbuild.Append("<td><i class='fas fa-school fa-1-6x text-color-primary'></i></td>");
                else
                    sbuild.Append("<td></td>");

                if (_seguimiento.estado == (int)Constantes.type_status_action.status_nuevo)
                    sbuild.Append($"<td><span class='badge badge-pill badge bg-primary text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Nuevo'>Nuevo</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_rechazado)
                    sbuild.Append($"<td><span class='badge badge-pill badge-purple' data-toggle='tooltip' data-placement='top' data-html='true' title='Rechazado'>Rechazado</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_duplicado)
                    sbuild.Append($"<td><span class='badge badge-pill badge-purple' data-toggle='tooltip' data-placement='top' data-html='true' title='Duplicado'>Duplicado</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_futuro)
                    sbuild.Append($"<td><span class='badge badge-pill badge-primary' data-toggle='tooltip' data-placement='top' data-html='true' title='Futuro'>Futuro</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_cerrar)
                    sbuild.Append($"<td><span class='badge badge-pill badge-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Cerrado'>Cerrado</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_sin_contactar)
                    sbuild.Append($"<td><span class='badge badge-pill badge-danger' data-toggle='tooltip' data-placement='top' data-html='true' title='Sin contactar'>Sin contactar</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_indeciso)
                    sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Indeciso'>Indeciso</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_interesado)
                    sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Interesado'>Interesado</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_send)
                    sbuild.Append($"<td><span class='badge badge-pill badge-orange text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Envio contrato'>Envio contrato</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_receive)
                    sbuild.Append($"<td><span class='badge badge-pill badge-yellow text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Recibir contrato'>Recibir contrato</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_pago)
                    sbuild.Append($"<td><span class='badge badge-pill badge-yellow text-dark' data-toggle='tooltip' data-placement='top' data-html='true' title='Pago'>Pago</span></td>");
                else if (_seguimiento.estado == (int)Constantes.type_status_action.status_matriculado)
                    sbuild.Append($"<td><span class='badge badge-pill badge-success' data-toggle='tooltip' data-placement='top' data-html='true' title='Matriculado'>Matriculado</span></td>");
                else
                    sbuild.Append("<td></td>");

                sbuild.Append($"<td>{_comerciales.Where(_ => _.id_cliente == _seguimiento.idComercial).Select(_ => _.Nombre_Completo).FirstOrDefault()} ({_seguimiento.idComercial})</td>");
                sbuild.Append("<td>" + _seguimiento.Comentarios + "</td>");
                if (_seguimiento.tipo == Constantes.tipo_accion_comercial.Fundacion.GetStringValue())
                    sbuild.Append($"<td><a href='ficha-alumno-crm-aux.aspx?idu={_seguimiento.idAlumno}&idta=4&idap={_seguimiento.idAccionPersona}&ids={_seguimiento.id_SegComercial}' title='Fundación'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td><a href='seguimiento-comercial-crm.aspx?idu=" + _seguimiento.idAlumno + "&idap=" + _seguimiento.idAccionPersona + "&ids=" + _seguimiento.id_SegComercial + "' title='Editar seguimiento'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el seguimiento?\")){eliminar_seguimiento(" + _seguimiento.id_SegComercial + ")}' title='Eliminar seguimiento'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        private static string limpiar_adjuntos(string adjuntos)
        {
            string file_adjuntos = string.Empty;

            /// 1.- Limpiar los adjuntos
            if (!String.IsNullOrEmpty(adjuntos))
            {
                List<string> list_files_routes = new List<string>();

                /// 1.1.- Sacar los adjuntos
                List<string> list_files = adjuntos.Split(',').ToList();
                if (list_files.Count > 0)
                {
                    /// 1.2.- Recorrer los adjuntos
                    foreach (var _file in list_files)
                    {
                        if (_file.Contains("adjuntos_free"))
                            list_files_routes.Add(ConfigurationManager.AppSettings["urlTemplateMailFree"] + _file.Replace(ConfigurationManager.AppSettings["routeTemplateMailFree"], string.Empty).Replace('\\', '/'));
                        else
                            list_files_routes.Add(ConfigurationManager.AppSettings["urlTemplateMail"] + _file.Replace(ConfigurationManager.AppSettings["routeTemplateMail"], string.Empty).Replace('\\', '/'));
                    }

                    /// 1.3.- Devolver los adjuntos
                    int index = 0;
                    foreach (var route in list_files_routes)
                    {
                        if (index == 0)
                            file_adjuntos = route;
                        else
                            file_adjuntos += "," + route;
                        index++;
                    }
                }
            }

            return file_adjuntos;
        }

        #endregion

        #region Métodos para buscar los pagos de una asignación comercial

        [WebMethod(Description = "Recupera la tabla de pagos")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_subtable_ventas(string id)
        {
            DataAccess da = new DataAccess();
            List<string> list = new List<string>();

            try
            {
                /// 0.- Sacar los datos
                long idAlumno = -1;
                long idCurso = -1;
                long idDocencia = -1;
                List<string> _params = id.Split(',').ToList();
                int index = 0;
                foreach (var _param in _params)
                {
                    if (index == 0)
                        idAlumno = long.Parse(_param);
                    else if (index == 1)
                        idCurso = long.Parse(_param);
                    else if (index == 2)
                        idDocencia = long.Parse(_param);

                    index++;
                }

                /// 1.- Comprobar que vengan todos los parámetros
                if (idAlumno > 0 && idCurso > 0 && idDocencia > 0)
                {
                    /// 2.- Sacar los pagos
                    List<campus_DATA_COMERCIAL> _pagos = da.getDataComercialByIdUser(idAlumno);
                    _pagos = _pagos.Where(_ => _.idCurso == idCurso && _.idDocencia == idDocencia).ToList();
                    if (_pagos.Count > 0)
                        list.Add(paint_table_ventas(_pagos));
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-usuario-crm.cs::search_subtable_ventas()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                list = new List<string>();
            }

            return list;
        }

        private static string paint_table_ventas(List<campus_DATA_COMERCIAL> _pagos)
        {
            DataAccess da = new DataAccess();
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar los datos del alunmo
            List<CLIENTES> _user = da.getUserById(_pagos[0].idAlumno);

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_pays\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha Estimada</th>");
            sbuild.Append("<th>Fecha Real</th>");
            sbuild.Append("<th>Euros Estimados</th>");
            sbuild.Append("<th>Euros Reales</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tbody>");

            /// 3.- Recorrer los pagos
            foreach (var _pago in _pagos)
            {
                if (_pago.Fecha_real != null)
                {
                    TimeSpan days_limit = _pago.Fecha_real.Value.Subtract(_pago.Fecha_est);
                    if ((_pago.Fecha_est != _pago.Fecha_real && days_limit.Days > int.Parse(ConfigurationManager.AppSettings["limit_days"])) || _pago.EUR_est != _pago.EUR_real)
                        sbuild.Append("<tr class='text-color-red'>");
                    else
                        sbuild.Append("<tr>");

                    sbuild.Append($"<td>{_pago.Fecha_est.ToShortDateString()}</td>");
                    sbuild.Append($"<td>{_pago.Fecha_real.Value.ToShortDateString()}</td>");
                    sbuild.Append($"<td>{Utilities.PonerPuntoMiles(_pago.EUR_est)}€</td>");
                    sbuild.Append($"<td>{Utilities.PonerPuntoMiles(_pago.EUR_real.Value)}€</td>");
                }
                else
                {
                    sbuild.Append("<tr>");
                    sbuild.Append($"<td>{_pago.Fecha_est.ToShortDateString()}</td>");
                    sbuild.Append($"<td></td>");
                    sbuild.Append($"<td>{Utilities.PonerPuntoMiles(_pago.EUR_est)}€</td>");
                    sbuild.Append("<td></td>");
                }

                sbuild.Append($"<td><a href='ficha-alumno-crm-aux.aspx?idu={_pago.idAlumno}&idc={_pago.idCurso}&idd={_pago.idDocencia}&idp={_pago.idDataComercial}&idta=9' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                if (_pago.Fecha_real == null)
                {
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el pago?\")){eliminar_pago(" + _pago.idDataComercial + ")}' title='Eliminar el pago'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea enviar un recordatorio de pago?\")){enviar_pago(" + _pago.idDataComercial + ")}' title='Enviar recordatorio de pago'><i class=\"fas fa-envelope fa-1-6x\"></i></a></td>");
                }
                else
                    sbuild.Append("<td></td><td></td>");

                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");
            return sbuild.ToString();
        }

        #endregion

        #region Métodos para la foto del usuario

        [WebMethod(Description = "Eliminar la foto del usuario")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool eliminar_photo(long id_user, string photo)
        {
            DataAccess da = new DataAccess();
            bool _delete = false;
            try
            {
                if (id_user > 0)
                {
                    /// 1.- Buscar al usuario
                    List<CLIENTES> _users = da.getUserById(id_user);
                    if (_users.Count == 1)
                    {
                        /// 1.0.- Sacar los datos del usuario
                        CLIENTES _user = _users[0];

                        /// 1.1.- Sacar la ruta de la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["routeUserPhoto"];

                        /// 1.2.- Eliminar el fichero
                        if (!String.IsNullOrEmpty(_user.Foto))
                            File.Delete(ruta + _user.Foto);

                        /// 1.3.- Actualizar los datos del usuario                
                        _user.Foto = null;

                        /// 1.4.- Actualizar la BBDD
                        _delete = da.updateClient(_user);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-usuario.cs::delete_photo()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _delete = false;
            }
            return _delete;
        }

        [WebMethod(Description = "Guarda la foto del usuario")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool save_photo(long id_user, string photo)
        {
            DataAccess da = new DataAccess();
            bool _save = false;
            try
            {
                if (id_user > 0)
                {
                    /// 1.- Buscar al usuario
                    List<CLIENTES> _users = da.getUserById(id_user);
                    if (_users.Count == 1)
                    {
                        /// 1.0.- Sacar los datos del usuario
                        CLIENTES _user = _users[0];

                        /// 1.1.- Sacar la ruta de la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["routeUserPhoto"];
                        string ruta_origen = ruta + "temp\\" + photo;
                        string ruta_destino = ruta;

                        /// 1.2.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        /// 1.3.- Poner la ruta de destino
                        ruta_destino = ruta_destino + photo;

                        /// 1.4.- Copiar el fichero
                        File.Copy(ruta_origen, ruta_destino, true);

                        /// 1.5.- Borramos el fichero de la carpeta origen
                        File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                        /// 1.6.- Borramos el directorio temp
                        if (Directory.GetFiles(ruta + "temp\\").Count() == 0)
                        {
                            if ((Directory.Exists(ruta + "temp\\")))
                                Directory.Delete(ruta + "temp\\");
                        }
                        else
                        {
                            foreach (var file in Directory.GetFiles(ruta + "temp\\"))
                            {
                                File.Delete(file);
                            }

                            if ((Directory.Exists(ruta + "temp\\")))
                                Directory.Delete(ruta + "temp\\");
                        }

                        /// 1.7.- Actualizar los datos del usuario                
                        _user.Foto = photo;

                        /// 1.8.- Actualizar la BBDD
                        _save = da.updateClient(_user);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ficha-usuario.cs::save_file()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _save = false;
            }
            return _save;
        }

        #endregion

        #region Métodos de los tags

        [WebMethod(Description = "Añade un tag al usuario")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> add_tag_user(long idUsuario, string tag)
        {
            DataAccess da = new DataAccess();

            /// 1.- Inicializar el objeto
            List<string> _lst_tags = new List<string>();

            /// 2.- Sacar los tags
            List<CLIENTES_TAG> _tags = da.getClientTags(idUsuario);

            /// 2.1.- Filtrar sólo los tags de tipo general
            _tags = _tags.Where(_ => _.tipo_tag == "general").ToList();

            /// 2.2.- Comprobar si existe el tag
            List<CLIENTES_TAG> _tags_users = _tags.Where(_ => _.tag == tag.ToUpper().Trim()).ToList();
            if (_tags_users.Count == 0)
            {
                /// 3.- Añadir tag
                CLIENTES_TAG _tag = new CLIENTES_TAG();
                _tag.idUser = idUsuario;
                _tag.tag = tag.ToUpper().Trim();
                _tag.tipo_tag = "general";
                _tag.fecha_ult_act = DateTime.Now;

                long _insert_tag = da.insertClientTag(_tag);
                if (_insert_tag > 0)
                {
                    _tags = da.getClientTags(idUsuario);
                    _tags = _tags.Where(_ => _.tipo_tag == "general" && _.fecha_baja == null).ToList();
                    _lst_tags.Add(paint_tags(_tags));
                }
            }
            if (_tags_users.Count == 1)
            {
                /// 3.- Actualizar tag
                CLIENTES_TAG _tag = _tags_users[0];
                _tag.fecha_ult_act = DateTime.Now;
                _tag.fecha_baja = null;

                bool update_tag = da.updateClientTag(_tag);
                if (update_tag)
                {
                    _tags = da.getClientTags(idUsuario);
                    _tags = _tags.Where(_ => _.tipo_tag == "general" && _.fecha_baja == null).ToList();
                    _lst_tags.Add(paint_tags(_tags));
                }
            }
            else
            {
                /// 3.- Sacar los tags activos
                _tags = _tags.Where(_ => _.fecha_baja == null).ToList();
                _lst_tags.Add(paint_tags(_tags));
            }

            return _lst_tags;
        }

        private static string paint_tags(List<CLIENTES_TAG> _tags_general)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Recorrer los tags
            if (_tags_general.Count > 0)
            {
                /// 1.1.- Pintar el título del bloque de origenes
                sbuild.Append("<label class='w-100'>Tags <a href='javascript:void(0)' onclick='show_tag()' title='Añadir tag'><small><i class='fas fa-plus-circle'></i> Añadir tag</small></a><span id='blk_add_tag' class='pl-2 hidden'><span class='d-inline-block align-middle'><input type='text' id='tag_user' class='form-control form-control-sm col-sm-8' runat='server' /><a id='lnk_add_tag' href='javascript:void(0);' class='fas fa-check text-color-green col-sm-2 pt-2 px-2' onclick='add_tag()'></a><a id='lnk_back_tag' href='javascript:void(0);' class='fas fa-times-circle text-color-red col-sm-2 pt-2 px-0' onclick='close_tag()'></a></span></span></label>");

                /// 1.2.- Recorrer los tags
                foreach (var _tag in _tags_general)
                {
                    sbuild.Append("<a href='javascript:void(0)' onclick='if (confirm_message(\"¿Desea eliminar el tag: " + _tag.tag + "?\")){eliminar_tag(\"" + _tag.tag + "\")}' class='badge badge-light'>" + _tag.tag + " <i class='fas fa-times-circle text-color-red'></i></a> ");
                }
            }
            else
                sbuild.Append("<label class='w-100'>Tags <a href='javascript:void(0)' onclick='show_tag()' title='Añadir tag'><small><i class='fas fa-plus-circle'></i> Añadir tag</small></a><span id='blk_add_tag' class='pl-2 hidden'><span class='d-inline-block align-middle'><input type='text' id='tag_user' class='form-control form-control-sm col-sm-8' runat='server' /><a id='lnk_add_tag' href='javascript:void(0);' class='fas fa-check text-color-green col-sm-2 pt-2 px-2' onclick='add_tag()'></a><a id='lnk_back_tag' href='javascript:void(0);' class='fas fa-times-circle text-color-red col-sm-2 pt-2 px-0' onclick='close_tag()'></a></span></span></label>");

            return sbuild.ToString();
        }

        #endregion        
    }
}