using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class cargar_usuarios : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
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
                else
                {
                    /// 1.- Pintar el botón volver
                    btn_back.HRef = "listado-leads-crm.aspx";
                }
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            /// 1.- Limpiar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Comprobar si ha subido el fichero
            if (fuFile.HasFile)
            {
                /// 2.1.- Comprobar que ha subido un excel
                string extension = Path.GetExtension(fuFile.FileName);
                if (extension == ".csv")
                {
                    /// 2.2.- Buscar la ruta donde se van a dejar los ficheros
                    string ruta_fichero = ConfigurationManager.AppSettings["route_files"];
                    if (!(Directory.Exists(ruta_fichero)))
                        Directory.CreateDirectory(ruta_fichero);

                    /// 2.3.- Comprobar si ha subido un fichero, si ya había uno cargado, en este caso eliminar el cargado previamente
                    if (!String.IsNullOrEmpty(name_file.Value))
                        File.Delete(ruta_fichero + name_file.Value);

                    /// 3.- Cargar el fichero excel            
                    string name_excel = string.Empty;
                    if (Directory.Exists(ruta_fichero))
                    {
                        FileInfo archivo = new FileInfo(fuFile.PostedFile.FileName);
                        name_excel = "Excel_usuario_" + DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + "_" + Utilities.RemoverSignosAcentos(archivo.Name).Replace(" ", "_");

                        try
                        {
                            fuFile.PostedFile.SaveAs(ruta_fichero + name_excel);
                        }
                        catch
                        {
                            name_excel = string.Empty;
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al subir el fichero');</script>");
                        }
                    }

                    /// 4.- Si ha guardado el excel, precargamos los datos
                    if (!String.IsNullOrEmpty(name_excel))
                    {
                        try
                        {
                            string path = ruta_fichero + name_excel;

                            int usuarios_correctos = 0;
                            int usuarios_errores = 0;

                            /// 4.1.- Cargamos los datos del excel                        
                            IEnumerable<CsvUsuarios> items = LinqToCSV.ReadFileWithExceptionHandling<CsvUsuarios>(path);
                            if (items.Count() > 0)
                            {
                                /// 4.2.- Comprobar si tiene errores
                                bool _list_errors = false;
                                foreach (var _user in items)
                                {
                                    //if (String.IsNullOrEmpty(_user.Nombre) || !Utils.esStringCorrecto(_user.Nombre) || String.IsNullOrEmpty(_user.Mail) || !Utils.esMailCorrecto(_user.Mail))
                                    if (!((_user.Campaign != null && _user.Campaign.Value > 0) && ((_user.ID != null && Utils.IsNumeric(_user.ID.ToString()) && _user.ID.Value > 0) || (!String.IsNullOrEmpty(_user.Nombre) && Utils.esStringCorrecto(_user.Nombre) && _user.Nombre.ToUpper().Trim() != "ANONIMO" && !String.IsNullOrEmpty(_user.Mail) && Utils.esMailCorrecto(_user.Mail)))) || (_user.Campaign != null && _user.Campaign == (long)Constantes.accion.Matriculacion && (_user.Docencia == null || _user.Docencia < 1)))
                                    {
                                        _list_errors = true;
                                        break;
                                    }
                                }

                                /// 4.3.- Pintamos la tabla con los datos extraídos del Excel
                                StringBuilder sbuild = new StringBuilder();
                                sbuild.Append("<table id =\"tabla_usuarios\" class=\"display compact\" style =\"width:100%\"><thead><tr><th>Linea</th><th>ID</th><th>Nombre</th><th title='Apellidos'>Ape</th><th>Telf</th><th>Mail</th><th>Pais</th><th>Prov</th><th>Ciudad</th><th>Origen</th><th>Curso</th><th title='Docencia'>Doc</th><th>Acción</th><th>Landing</th><th>Fecha</th><th>utm_source</th><th>utm_med</th><th>utm_campaign</th><th>utm_cont</th><th>utm_term</th><th>tags</th><th title='Procesado'>Proc</th></tr></thead><tbody>");

                                if (_list_errors)
                                {
                                    /// 4.4.- Pintamos la tabla de errores
                                    int _index = 1;
                                    foreach (var _user in items)
                                    {
                                        //if (String.IsNullOrEmpty(_user.Nombre) || !Utils.esStringCorrecto(_user.Nombre) || String.IsNullOrEmpty(_user.Mail) || !Utils.esMailCorrecto(_user.Mail))
                                        if (!((_user.Campaign != null && _user.Campaign.Value > 0) && ((_user.ID != null && Utils.IsNumeric(_user.ID.ToString()) && _user.ID.Value > 0) || (!String.IsNullOrEmpty(_user.Nombre) && Utils.esStringCorrecto(_user.Nombre) && _user.Nombre.ToUpper().Trim() != "ANONIMO" && !String.IsNullOrEmpty(_user.Mail) && Utils.esMailCorrecto(_user.Mail)))) || (_user.Campaign != null && _user.Campaign == (long)Constantes.accion.Matriculacion && (_user.Docencia == null || _user.Docencia < 1)))
                                        {
                                            sbuild.Append("<tr>");

                                            sbuild.Append("<td>" + _index + "</td>");

                                            if (!((_user.ID != null && Utils.IsNumeric(_user.ID.ToString()) && _user.ID.Value > 0) || (!String.IsNullOrEmpty(_user.Nombre) && Utils.esStringCorrecto(_user.Nombre) && _user.Nombre.ToUpper().Trim() != "ANONIMO" && !String.IsNullOrEmpty(_user.Mail) && Utils.esMailCorrecto(_user.Mail))))
                                                sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                            else
                                                sbuild.Append($"<td>{_user.ID}</td>");

                                            if (!String.IsNullOrEmpty(_user.Nombre) && Utils.esStringCorrecto(_user.Nombre) && _user.Nombre.ToUpper().Trim() != "ANONIMO")
                                                sbuild.Append("<td>" + _user.Nombre + "</td>");
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(_user.Nombre) && !(_user.ID != null && Utils.IsNumeric(_user.ID.ToString()) && _user.ID.Value > 0))
                                                    sbuild.Append("<td><span class='text-color-red'>" + _user.Nombre + "</span></td>");
                                                else
                                                    sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                            }

                                            sbuild.Append("<td>" + _user.Apellidos + "</td>");
                                            sbuild.Append("<td>" + _user.Telefono + "</td>");

                                            if (!String.IsNullOrEmpty(_user.Mail) && Utils.esMailCorrecto(_user.Mail))
                                                sbuild.Append("<td>" + _user.Mail + "</td>");
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(_user.Mail) && !String.IsNullOrEmpty(_user.Mail) && !(_user.ID != null && Utils.IsNumeric(_user.ID.ToString()) && _user.ID.Value > 0))
                                                    sbuild.Append("<td><span class='text-color-red'>" + _user.Mail + "</span></td>");
                                                else
                                                    sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                            }

                                            sbuild.Append("<td>" + _user.Pais + "</td>");
                                            sbuild.Append("<td>" + _user.Provincia + "</td>");
                                            sbuild.Append("<td>" + _user.Ciudad + "</td>");
                                            sbuild.Append("<td>" + _user.Origen + "</td>");
                                            sbuild.Append("<td>" + _user.Curso + "</td>");
                                            if (_user.Campaign != null && _user.Campaign.Value == (long)Constantes.accion.Matriculacion && (_user.Docencia == null || _user.Docencia < 1))
                                                sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                            else
                                                sbuild.Append("<td>" + _user.Docencia + "</td>");
                                            if (_user.Campaign == null || _user.Campaign.Value < 1)
                                                sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                            else
                                                sbuild.Append("<td>" + _user.Campaign + "</td>");
                                            sbuild.Append("<td>" + _user.idLanding + "</td>");
                                            sbuild.Append("<td>" + (_user.Fecha != null ? _user.Fecha.Value.ToShortDateString() : string.Empty) + "</td>");
                                            sbuild.Append("<td>" + _user.utm_source + "</td>");
                                            sbuild.Append("<td>" + _user.utm_medium + "</td>");
                                            sbuild.Append("<td>" + _user.utm_campaign + "</td>");
                                            sbuild.Append("<td>" + _user.utm_content + "</td>");
                                            sbuild.Append("<td>" + _user.utm_term + "</td>");
                                            sbuild.Append("<td>" + _user.Tags + "</td>");
                                            sbuild.Append("<td>" + _user.procesado + "</td>");

                                            sbuild.Append("</tr>");
                                            usuarios_errores++;
                                        }
                                        else
                                            usuarios_correctos++;

                                        _index++;
                                    }
                                }
                                else
                                {
                                    /// 4.5.- Pintamos la tabla completa
                                    int _index = 1;
                                    foreach (var _user in items)
                                    {
                                        sbuild.Append("<tr>");
                                        sbuild.Append("<td>" + _index + "</td>");
                                        sbuild.Append($"<td>{_user.ID}</td>");
                                        sbuild.Append($"<td>{(!String.IsNullOrEmpty(_user.Nombre) ? _user.Nombre.Replace("?", "").Replace(".", "").Replace("+", "") : string.Empty)}</td>");
                                        sbuild.Append("<td>" + _user.Apellidos + "</td>");
                                        sbuild.Append("<td>" + _user.Telefono + "</td>");
                                        sbuild.Append("<td>" + _user.Mail + "</td>");
                                        sbuild.Append("<td>" + _user.Pais + "</td>");
                                        sbuild.Append("<td>" + _user.Provincia + "</td>");
                                        sbuild.Append("<td>" + _user.Ciudad + "</td>");
                                        sbuild.Append("<td>" + _user.Origen + "</td>");
                                        sbuild.Append("<td>" + _user.Curso + "</td>");
                                        sbuild.Append("<td>" + _user.Docencia + "</td>");
                                        sbuild.Append("<td>" + _user.Campaign + "</td>");
                                        sbuild.Append("<td>" + _user.idLanding + "</td>");
                                        sbuild.Append("<td>" + (_user.Fecha != null ? _user.Fecha.Value.ToShortDateString() : string.Empty) + "</td>");
                                        sbuild.Append("<td>" + _user.utm_source + "</td>");
                                        sbuild.Append("<td>" + _user.utm_medium + "</td>");
                                        sbuild.Append("<td>" + _user.utm_campaign + "</td>");
                                        sbuild.Append("<td>" + _user.utm_content + "</td>");
                                        sbuild.Append("<td>" + _user.utm_term + "</td>");
                                        sbuild.Append("<td>" + _user.Tags + "</td>");
                                        sbuild.Append("<td>" + _user.procesado + "</td>");
                                        sbuild.Append("</tr>");

                                        usuarios_correctos++;

                                        _index++;
                                    }
                                }

                                sbuild.Append("</tbody></table>");
                                table_usuarios.InnerHtml = sbuild.ToString();

                                /// 4.6.- Añadimos el nombre del excel al campo oculto y al texto
                                name_file.Value = name_excel;
                                txt_name_file.InnerHtml = name_excel;

                                /// 4.7.- Pintamos el resultado de la carga
                                table_resultados.InnerHtml = "<strong>Total:</strong> " + items.Count() + "<br /><strong>Correctos:</strong> " + usuarios_correctos + "<br /><strong>Errores:</strong> " + usuarios_errores;

                                /// 4.8.- Desbloqueamos el bloque
                                block_usuarios.Attributes["class"] = block_usuarios.Attributes["class"].Replace("hidden", string.Empty);

                                /// 4.9.- Ocultar los botones 
                                if (usuarios_errores > 0)
                                {
                                    btnGuardar.Visible = false;
                                    btnCargar.Visible = true;
                                    txt_name_file.InnerHtml = string.Empty;
                                }
                                else
                                {
                                    btnGuardar.Visible = true;
                                    btnCargar.Visible = false;
                                    fuFile.Visible = false;
                                }
                            }
                            else
                                txt_error.InnerHtml = "No se han encontrado datos en el CSV";
                        }
                        catch (Exception ex)
                        {
                            LogUtils.InsertarLog("[Error] Error al procesar fichero CSV " + ex.Message);

                            if (ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
                            {
                                StringBuilder sbuild = new StringBuilder();
                                sbuild.Append("<ul class='padding10'>");
                                foreach (var _exception in ((LINQtoCSV.AggregatedException)ex).m_InnerExceptionsList)
                                {
                                    sbuild.Append("<li>" + _exception.Message + "</li>");
                                }
                                sbuild.Append("</ul>");
                                table_resultados.InnerHtml = "Excepciones en la carga del fichero: " + sbuild.ToString();
                            }
                            else
                                table_resultados.InnerHtml = ex.Message;

                            btnGuardar.Visible = false;
                            btnCargar.Visible = true;
                            txt_name_file.InnerHtml = string.Empty;
                        }
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al subir el fichero";
                }
                else
                    txt_error.InnerHtml = "El fichero subido no es un fichero tipo CSV";
            }
            else
                txt_error.InnerHtml = "Hay que subir un fichero tipo CSV";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 0.- Inicializar los datos
            int usuarios_nuevos = 0;
            int usuarios_bbdd = 0;
            int usuarios_con_errores = 0;
            int lead_incorrectos = 0;

            /// 1.- Limpiar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Ruta de los ficheros
            string ruta_fichero = ConfigurationManager.AppSettings["route_files"];
            if (!(Directory.Exists(ruta_fichero)))
                Directory.CreateDirectory(ruta_fichero);

            /// 3.- Sacar la ruta
            string name_excel = name_file.Value;
            if (!String.IsNullOrEmpty(name_excel))
            {
                StringBuilder sbuild = new StringBuilder();

                /// 4.- Sacar la ruta del fichero
                string path = ruta_fichero + name_excel;

                /// 4.1.- Cargamos los datos del excel
                IEnumerable<CsvUsuarios> items = LinqToCSV.ReadFileWithExceptionHandling<CsvUsuarios>(path);
                if (items.Count() > 0)
                {
                    /// 4.2.- Sacar los paises y las provincias
                    List<Paises> _paises = da.getCountries();
                    List<Provincias> _provincias = da.getProvinceById(-1);

                    /// 4.3.- Comprobar si son matriculaciones
                    List<campus_DOCENCIA_GRUPO> _matriculas = new List<campus_DOCENCIA_GRUPO>();
                    List<campus_DOCENCIA> _docencias = new List<campus_DOCENCIA>();
                    bool _matriculacion = items.Where(_ => _.Campaign.Value == (long)Constantes.accion.Matriculacion).Count() > 0 ? true : false;
                    if (_matriculacion)
                    {
                        _matriculas = da.getDocenciasGrupo(null);
                        _docencias = da.getDocenciaById(-1);
                    }

                    var _index = 1;
                    foreach (var _user in items)
                    {
                        /// 5.- Buscar al usuario
                        List<CLIENTES> _usuario = new List<CLIENTES>();

                        /// 5.1.- Buscar por id
                        if (_user.ID != null && Utils.IsNumeric(_user.ID.ToString()) && _user.ID.Value > 0)
                            _usuario = da.getUserById(_user.ID.Value);
                        else if (!String.IsNullOrEmpty(_user.Mail))
                            _usuario = da.getUserForMailOrLogin(_user.Mail);

                        /// 5.2.- Procesar los datos
                        if (_usuario.Count == 1)
                        {
                            usuarios_bbdd++;

                            /// 6.- Saca la Acción
                            long id_accion = _user.Campaign.Value;
                            if (id_accion == (long)Constantes.accion.Matriculacion)
                            {
                                bool _error_matriculacion = false;

                                /// 6.1.1.- Comprobar si el usuario está matriculado
                                List<campus_DOCENCIA_GRUPO> _matricula = _matriculas.Where(_ => _.ID_Docencia == _user.Docencia && _.ID_Persona == _usuario[0].id_cliente).ToList();
                                if (_matricula.Count == 0)
                                {
                                    /// 6.1.2.- Matriculamos al alumno en la docencia
                                    campus_DOCENCIA_GRUPO _dg = new campus_DOCENCIA_GRUPO();
                                    _dg.ID_Docencia = _user.Docencia.Value;
                                    _dg.ID_Persona = _usuario[0].id_cliente;
                                    _dg.PrecioPagado = 1;
                                    _dg.FechaAlta = DateTime.Today;

                                    long insert_mat = da.insertDocenciaGrupo(_dg);
                                    if (insert_mat > 0)
                                    {
                                        /// 6.1.3.- Sacar el curso de la docencia, si no viene
                                        long idCurso = -1;
                                        if (_user.Curso != null && _user.Curso > 0)
                                            idCurso = _user.Curso.Value;
                                        else
                                        {
                                            List<campus_DOCENCIA> _docencias_user = _docencias.Where(_ => _.ID_Docencia == _user.Docencia.Value).ToList();
                                            if (_docencias_user.Count == 1)
                                                idCurso = _docencias_user[0].ID_Curso;
                                            else
                                                idCurso = (long)Constantes.course.Sin_determinar;
                                        }

                                        /// 6.1.4.- Añadir AP
                                        campus_ACCIONES_PERSONA _lead = new campus_ACCIONES_PERSONA();
                                        _lead.idPersona = _usuario[0].id_cliente;
                                        _lead.idAccion = id_accion;
                                        _lead.Fecha = _user.Fecha != null ? _user.Fecha.Value : DateTime.Today;
                                        _lead.IdOrigen = _user.Origen != null ? _user.Origen.Value : (long)Constantes.origen.Web;
                                        _lead.IdCurso = idCurso;
                                        _lead.Procesado = _user.procesado != null ? (_user.procesado.Value == 1 ? true : false) : false;
                                        _lead.idLanding = _user.idLanding;
                                        _lead.utm_source = _user.utm_source;
                                        _lead.utm_medium = _user.utm_medium;
                                        _lead.utm_campaign = _user.utm_campaign;
                                        _lead.utm_term = _user.utm_term;
                                        _lead.utm_content = _user.utm_content;
                                        _lead.tags = _user.Tags;

                                        long insert_lead = da.insertPersonAction(_lead);
                                        if (insert_lead < 1)
                                        {
                                            lead_incorrectos++;
                                            _error_matriculacion = true;
                                            LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir lead al usuario " + _usuario[0].id_cliente);
                                            sbuild.Append($"<tr><td>Se ha producido un error al añadir lead al usuario {_usuario[0].id_cliente}</td></tr>");
                                        }
                                    }
                                    else
                                    {
                                        lead_incorrectos++;
                                        _error_matriculacion = true;
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la matricula al usuario " + _usuario[0].id_cliente + " y docencia " + _user.Docencia);
                                        sbuild.Append($"<tr><td>Se ha producido un error al añadir la matricula al usuario {_usuario[0].id_cliente} y docencia {_user.Docencia}</td></tr>");
                                    }
                                }

                                if (!_error_matriculacion)
                                {
                                    /// 6.1.5.- Comprobar si el usuario está activado, sino generar un login/password al usuario si no tiene
                                    if (_usuario[0].activo == ((int)Constantes.activo.NoActivo).ToString() || Utils.esDateTimeCorrecto(_usuario[0].login))
                                    {
                                        /// 6.1.6.- Actualizar al usuario
                                        CLIENTES _usuario_act = _usuario[0];

                                        if (Utils.esDateTimeCorrecto(_usuario_act.login))
                                        {
                                            _usuario_act.login = Utilities.generate_login(_usuario_act);
                                            _usuario_act.password = Utilities.generate_password(_usuario_act);
                                        }

                                        _usuario_act.activo = ((int)Constantes.activo.Activo).ToString();
                                        if (_usuario_act.fecha_activacion == null)
                                            _usuario_act.fecha_activacion = DateTime.Now;
                                                                                
                                        bool _update_user = da.updateClient(_usuario_act);
                                        if (_update_user)
                                        {
                                            long idCurso = -1;
                                            if (_user.Curso != null && _user.Curso > 0)
                                                idCurso = _user.Curso.Value;
                                            else
                                            {
                                                List<campus_DOCENCIA> _docencias_user = _docencias.Where(_ => _.ID_Docencia == _user.Docencia.Value).ToList();
                                                if (_docencias_user.Count == 1)
                                                    idCurso = _docencias_user[0].ID_Curso;
                                                else
                                                    idCurso = (long)Constantes.course.Sin_determinar;
                                            }

                                            /// 6.1.7.- Mandar el mail
                                            string template = Utilities.getPlantillaMail("campus-activacion", ConfigurationManager.AppSettings["urlTemplate"]);
                                            if (!String.IsNullOrEmpty(template))
                                            {
                                                template = template.Replace("###Nombre###", _usuario_act.Nombre_Completo);
                                                template = template.Replace("###Docencia###", _docencias.Where(_ => _.ID_Docencia == _user.Docencia.Value).Select(_ => _.Nombre).FirstOrDefault());
                                                template = template.Replace("###login###", _usuario_act.login);
                                                template = template.Replace("###password###", Utils.toDecodeString(_usuario_act.password));
                                                template = template.Replace("###url###", "https://campus.spainbs.com/");
                                                template = template.Replace("###IDC###", idCurso.ToString());
                                                template = template.Replace("###KEY###", _usuario_act.Key);
                                            }

                                            /// 6.1.8.- Resto de datos necesarios para el mail
                                            EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                                            email_data.nombreTo = _usuario_act.Nombre_Completo;
                                            email_data.mailTo = _usuario_act.email;
                                            email_data.priority = 1;
                                            email_data.asunto = "¡Bienvenido a Spain Business School!";
                                            email_data.body = template;

                                            long insert_mail = da.insertEmailContent(email_data);
                                            if (insert_mail > 0)
                                            {
                                                /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                                long idLog = da.insertLog(_usuario_act.id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                                if (idLog < 1)
                                                    LogUtils.InsertarLog(" ERROR - Error al guardar la entrada en el log del usuario " + _usuario_act.id_cliente);
                                            }
                                            else
                                            {
                                                lead_incorrectos++;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al mandar el mail de activación al usuario " + _usuario_act.id_cliente);
                                                sbuild.Append($"<tr><td>Se ha producido un error al mandar el mail de activación al usuario {_usuario_act.id_cliente}</td></tr>");
                                            }
                                        }
                                        else
                                        {
                                            lead_incorrectos++;
                                            LogUtils.InsertarLog(" ERROR - Se ha producido un error al actualizar al usuario " + _usuario_act.id_cliente);
                                            sbuild.Append($"<tr><td>Se ha producido un error al actualizar al usuario {_usuario_act.id_cliente}</td></tr>");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                /// 6.1.- Añadir AP
                                campus_ACCIONES_PERSONA _lead = new campus_ACCIONES_PERSONA();
                                _lead.idPersona = _usuario[0].id_cliente;
                                _lead.idAccion = id_accion;
                                _lead.Fecha = _user.Fecha != null ? _user.Fecha.Value : DateTime.Today;
                                _lead.IdOrigen = _user.Origen != null ? _user.Origen.Value : (long)Constantes.origen.Web;
                                _lead.IdCurso = _user.Curso != null ? _user.Curso.Value : (long)Constantes.course.Sin_determinar;
                                _lead.Procesado = _user.procesado != null ? (_user.procesado.Value == 1 ? true : false) : false;
                                _lead.idLanding = _user.idLanding;
                                _lead.utm_source = _user.utm_source;
                                _lead.utm_medium = _user.utm_medium;
                                _lead.utm_campaign = _user.utm_campaign;
                                _lead.utm_term = _user.utm_term;
                                _lead.utm_content = _user.utm_content;
                                _lead.tags = _user.Tags;

                                long insert_lead = da.insertPersonAction(_lead);
                                if (insert_lead < 1)
                                {
                                    lead_incorrectos++;
                                    LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir lead al usuario " + _usuario[0].id_cliente);
                                    sbuild.Append($"<tr><td>Se ha producido un error al añadir lead al usuario {_usuario[0].id_cliente}</td></tr>");
                                }
                            }
                        }
                        else
                        {
                            /// 6.0.- Sacar los apellidos a partir del nombre
                            string _nombre = string.Empty;
                            string _apellidos = string.Empty;
                            if (String.IsNullOrEmpty(_user.Apellidos))
                            {
                                string _name = _user.Nombre.Replace("?", "").Replace(".", "").Replace("+", "");
                                _apellidos = _name.Replace(_name.Split(' ')[0], string.Empty).Trim();
                                _nombre = _name.Split(' ')[0].Trim();

                                if (String.IsNullOrEmpty(_apellidos))
                                    _apellidos = ".";
                            }
                            else
                            {
                                _nombre = _user.Nombre;
                                _apellidos = _user.Apellidos;
                            }
                            
                            /// 6.1.- Sacar el pais y la provincia
                            int _pais = (int)Constantes.pais.Others;
                            int _provincia = (int)Constantes.provincias.Others;
                            if (!String.IsNullOrEmpty(_user.Pais))
                            {
                                /// 6.1.1.- Sacar el pais
                                if (!Utils.IsNumeric(_user.Pais))
                                {
                                    /// Buscar el pais por descripcion
                                    List<Paises> lst_paises = _paises.Where(_ => _.nombre.ToUpper().Equals(_user.Pais.ToUpper())).ToList();
                                    if (lst_paises.Count == 1)
                                        _pais = (int)lst_paises[0].id_pais;
                                }
                                else
                                    _pais = int.Parse(_user.Pais);

                                /// 6.1.2.- Sacar la provincia
                                if (!String.IsNullOrEmpty(_user.Provincia))
                                {
                                    if (!Utils.IsNumeric(_user.Provincia))
                                    {
                                        /// Buscar por pais
                                        if (_pais == (int)Constantes.pais.Spain)
                                        {
                                            List<Provincias> lst_provincias = _provincias.Where(_ => _.id_pais == _pais && _.nombre.ToUpper().Equals(_user.Provincia.ToUpper())).ToList();
                                            if (lst_provincias.Count == 1)
                                                _provincia = (int)lst_provincias[0].id_provincia;
                                            else
                                                _provincia = (int)Constantes.provincias.Sin_Asignar;
                                        }
                                        else
                                        {
                                            List<Provincias> lst_provincias = _provincias.Where(_ => _.id_pais == _pais).ToList();
                                            if (lst_provincias.Count == 1)
                                                _provincia = (int)lst_provincias[0].id_provincia;
                                            else
                                                _provincia = (int)Constantes.provincias.Others;
                                        }
                                    }
                                    else
                                        _provincia = int.Parse(_user.Provincia);
                                }
                                else if (_provincia == (int)Constantes.provincias.Others)
                                {
                                    if (_pais == (int)Constantes.pais.Spain)
                                        _provincia = (int)Constantes.provincias.Sin_Asignar;
                                    else
                                    {
                                        long provincia = _provincias.Where(_ => _.id_pais == _pais).Select(_ => _.id_provincia).FirstOrDefault();
                                        if (provincia > 0)
                                            _provincia = (int)provincia;
                                    }
                                }

                                if (_pais == (int)Constantes.pais.Others && _provincia == (int)Constantes.provincias.Others)
                                {
                                    /// 6.1.3.- Sacar el pais por telefono
                                    if (!String.IsNullOrEmpty(_user.Telefono))
                                    {
                                        if (_user.Telefono.StartsWith("+") || _user.Telefono.StartsWith("00") || _user.Telefono.StartsWith("("))
                                        {
                                            string phone_clear = Utils.cleanString(_user.Telefono);
                                            if (_user.Telefono.StartsWith("00"))
                                                phone_clear = phone_clear.Substring(2);

                                            /// Buscar el pais limpiando los valores
                                            List<Paises> _paises_clean = limpiar_valores_paises(_paises);
                                            int pais = buscar_pais(phone_clear, _paises_clean);
                                            if (pais > 0)
                                                _pais = pais;
                                        }
                                    }

                                    if (_pais != (int)Constantes.pais.Others)
                                    {
                                        if (_pais == (int)Constantes.pais.Spain)
                                            _provincia = (int)Constantes.provincias.Sin_Asignar;
                                        else
                                        {
                                            long provincia = _provincias.Where(_ => _.id_pais == _pais).Select(_ => _.id_provincia).FirstOrDefault();
                                            if (provincia > 0)
                                                _provincia = (int)provincia;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                /// 6.1.1.- Intentar sacar el país a partir del telefono
                                _pais = (int)Constantes.pais.Others;
                                _provincia = (int)Constantes.provincias.Others;
                                if (!String.IsNullOrEmpty(_user.Telefono))
                                {
                                    if (_user.Telefono.StartsWith("+") || _user.Telefono.StartsWith("00") || _user.Telefono.StartsWith("("))
                                    {
                                        string phone_clear = Utils.cleanString(_user.Telefono);
                                        if (_user.Telefono.StartsWith("00"))
                                            phone_clear = phone_clear.Substring(2);

                                        /// Buscar el pais limpiando los valores
                                        List<Paises> _paises_clean = limpiar_valores_paises(_paises);
                                        int pais = buscar_pais(phone_clear, _paises_clean);
                                        if (pais > 0)
                                            _pais = pais;
                                    }
                                }

                                if (_pais != (int)Constantes.pais.Others)
                                {
                                    if (_pais == (int)Constantes.pais.Spain)
                                        _provincia = (int)Constantes.provincias.Sin_Asignar;
                                    else
                                    {
                                        long provincia = _provincias.Where(_ => _.id_pais == _pais).Select(_ => _.id_provincia).FirstOrDefault();
                                        if (provincia > 0)
                                            _provincia = (int)provincia;
                                    }
                                }
                            }
                            
                            /// 6.3.- Crear un usuario nuevo
                            CLIENTES _usuario_nuevo = new CLIENTES();
                            _usuario_nuevo.nombre = _nombre;
                            _usuario_nuevo.apellidos = _apellidos;
                            _usuario_nuevo.telefono_contacto = _user.Telefono;
                            _usuario_nuevo.email = _user.Mail;
                            _usuario_nuevo.id_pais = _pais;
                            _usuario_nuevo.id_provincia = _provincia;
                            _usuario_nuevo.localidad = _user.Ciudad;
                            _usuario_nuevo.fecha_alta = DateTime.Now;
                            _usuario_nuevo.fecha_modificacion = DateTime.Now;
                            _usuario_nuevo.activo = ((int)Constantes.activo.NoActivo).ToString();
                            _usuario_nuevo.id_idioma = (int)Constantes.activo.Activo;
                            _usuario_nuevo.login = DateTime.Now + "." + DateTime.Now.Millisecond.ToString("000");
                            _usuario_nuevo.Nombre_Completo = _usuario_nuevo.nombre + " " + _usuario_nuevo.apellidos;
                            _usuario_nuevo.Key = Guid.NewGuid().ToString().ToUpper();

                            long idResultado = da.insertClient(_usuario_nuevo);
                            if (idResultado > 0)
                            {
                                usuarios_nuevos++;

                                /// 6.4.- Saca la Acción
                                long id_accion = _user.Campaign.Value;
                                if (id_accion == (long)Constantes.accion.Matriculacion)
                                {
                                    /// 6.4.1.- Matriculamos al alumno en la docencia
                                    campus_DOCENCIA_GRUPO _dg = new campus_DOCENCIA_GRUPO();
                                    _dg.ID_Docencia = _user.Docencia.Value;
                                    _dg.ID_Persona = idResultado;
                                    _dg.PrecioPagado = 1;
                                    _dg.FechaAlta = DateTime.Today;

                                    long insert_mat = da.insertDocenciaGrupo(_dg);
                                    if (insert_mat > 0)
                                    {
                                        /// 6.4.2.- Sacar el curso de la docencia, si no viene
                                        long idCurso = -1;
                                        if (_user.Curso != null && _user.Curso > 0)
                                            idCurso = _user.Curso.Value;
                                        else
                                        {
                                            List<campus_DOCENCIA> _docencias_user = _docencias.Where(_ => _.ID_Docencia == _user.Docencia.Value).ToList();
                                            if (_docencias_user.Count == 1)
                                                idCurso = _docencias_user[0].ID_Curso;
                                            else
                                                idCurso = (long)Constantes.course.Sin_determinar;
                                        }

                                        /// 6.4.3.- Añadir AP
                                        campus_ACCIONES_PERSONA _lead = new campus_ACCIONES_PERSONA();
                                        _lead.idPersona = idResultado;
                                        _lead.idAccion = id_accion;
                                        _lead.Fecha = _user.Fecha != null ? _user.Fecha.Value : DateTime.Today;
                                        _lead.IdOrigen = _user.Origen != null ? _user.Origen.Value : (long)Constantes.origen.Web;
                                        _lead.IdCurso = idCurso;
                                        _lead.Procesado = _user.procesado != null ? (_user.procesado.Value == 1 ? true : false) : false;
                                        _lead.idLanding = _user.idLanding;
                                        _lead.utm_source = _user.utm_source;
                                        _lead.utm_medium = _user.utm_medium;
                                        _lead.utm_campaign = _user.utm_campaign;
                                        _lead.utm_term = _user.utm_term;
                                        _lead.utm_content = _user.utm_content;
                                        _lead.tags = _user.Tags;

                                        long insert_lead = da.insertPersonAction(_lead);
                                        if (insert_lead > 0)
                                        {
                                            /// 6.4.4.- Actualizar al usuario
                                            List<CLIENTES> _usuarios_act = da.getUserById(idResultado);
                                            if (_usuarios_act.Count > 0)
                                            {
                                                /// 6.4.5.- Actualizar al usuario
                                                CLIENTES _usuario_act = _usuarios_act[0];

                                                if (Utils.esDateTimeCorrecto(_usuario_act.login))
                                                {
                                                    _usuario_act.login = Utilities.generate_login(_usuario_act);
                                                    _usuario_act.password = Utilities.generate_password(_usuario_act);
                                                }

                                                _usuario_act.activo = ((int)Constantes.activo.Activo).ToString();
                                                if (_usuario_act.fecha_activacion == null)
                                                    _usuario_act.fecha_activacion = DateTime.Now;

                                                bool _update_user = da.updateClient(_usuario_act);
                                                if (_update_user)
                                                {
                                                    /// 6.4.6.- Mandar el mail
                                                    string template = Utilities.getPlantillaMail("campus-activacion", ConfigurationManager.AppSettings["urlTemplate"]);
                                                    if (!String.IsNullOrEmpty(template))
                                                    {
                                                        template = template.Replace("###Nombre###", _usuario_act.Nombre_Completo);
                                                        template = template.Replace("###Docencia###", _docencias.Where(_ => _.ID_Docencia == _user.Docencia.Value).Select(_ => _.Nombre).FirstOrDefault());
                                                        template = template.Replace("###login###", _usuario_act.login);
                                                        template = template.Replace("###password###", Utils.toDecodeString(_usuario_act.password));
                                                        template = template.Replace("###url###", "https://campus.spainbs.com/");
                                                        template = template.Replace("###IDC###", idCurso.ToString());
                                                        template = template.Replace("###KEY###", _usuario_act.Key);
                                                    }

                                                    /// 6.4.7.- Resto de datos necesarios para el mail
                                                    EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                                                    email_data.nombreTo = _usuario_act.Nombre_Completo;
                                                    email_data.mailTo = _usuario_act.email;
                                                    email_data.priority = 1;
                                                    email_data.asunto = "¡Bienvenido a Spain Business School!";
                                                    email_data.body = template;

                                                    long insert_mail = da.insertEmailContent(email_data);
                                                    if (insert_mail > 0)
                                                    {
                                                        /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                                        long idLog = da.insertLog(_usuario_act.id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                                        if (idLog < 1)
                                                            LogUtils.InsertarLog(" ERROR - Error al guardar la entrada en el log del usuario " + _usuario_act.id_cliente);
                                                    }
                                                    else
                                                    {
                                                        lead_incorrectos++;
                                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al mandar el mail de activación al usuario " + _usuario_act.id_cliente);
                                                        sbuild.Append($"<tr><td>Se ha producido un error al mandar el mail de activación al usuario {_usuario_act.id_cliente}</td></tr>");
                                                    }
                                                }
                                                else
                                                {
                                                    lead_incorrectos++;
                                                    LogUtils.InsertarLog(" ERROR - Se ha producido un error al actualizar al usuario " + _usuario_act.id_cliente);
                                                    sbuild.Append($"<tr><td>Se ha producido un error al actualizar al usuario {_usuario_act.id_cliente}</td></tr>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lead_incorrectos++;
                                            LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir lead al usuario " + _usuario[0].id_cliente);
                                            sbuild.Append($"<tr><td>Se ha producido un error al añadir lead al usuario {_usuario[0].id_cliente}</td></tr>");
                                        }
                                    }
                                    else
                                    {
                                        lead_incorrectos++;
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la matricula al usuario " + _usuario[0].id_cliente + " y docencia " + _user.Docencia);
                                        sbuild.Append($"<tr><td>Se ha producido un error al añadir la matricula al usuario {_usuario[0].id_cliente} y docencia {_user.Docencia}</td></tr>");
                                    }
                                }
                                else
                                {
                                    /// 6.1.- Añadir AP
                                    campus_ACCIONES_PERSONA _lead = new campus_ACCIONES_PERSONA();
                                    _lead.idPersona = idResultado;
                                    _lead.idAccion = id_accion;
                                    _lead.Fecha = _user.Fecha != null ? _user.Fecha.Value : DateTime.Today;
                                    _lead.IdOrigen = _user.Origen != null ? _user.Origen.Value : (long)Constantes.origen.Web;
                                    _lead.IdCurso = _user.Curso != null ? _user.Curso.Value : (long)Constantes.course.Sin_determinar;
                                    _lead.Procesado = _user.procesado != null ? (_user.procesado.Value == 1 ? true : false) : false;
                                    _lead.idLanding = _user.idLanding;
                                    _lead.utm_source = _user.utm_source;
                                    _lead.utm_medium = _user.utm_medium;
                                    _lead.utm_campaign = _user.utm_campaign;
                                    _lead.utm_term = _user.utm_term;
                                    _lead.utm_content = _user.utm_content;
                                    _lead.tags = _user.Tags;

                                    long insert_lead = da.insertPersonAction(_lead);
                                    if (insert_lead < 1)
                                    {
                                        lead_incorrectos++;
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir lead al usuario " + _usuario_nuevo.Nombre_Completo + " (" + _usuario_nuevo.email + ")");
                                        sbuild.Append($"<tr><td>Se ha producido un error al añadir al usuario {_usuario_nuevo.Nombre_Completo} ({_usuario_nuevo.email})</td></tr>");
                                    }
                                }
                            }
                            else
                            {
                                usuarios_con_errores++;
                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir al usuario " + _usuario_nuevo.Nombre_Completo + " (" + _usuario_nuevo.email + ")");
                                sbuild.Append($"<tr><td>Se ha producido un error al añadir al usuario {_usuario_nuevo.Nombre_Completo} ({_usuario_nuevo.email})</td></tr>");
                            }
                        }

                        _index++;
                    }
                }
                else
                    txt_error.InnerHtml = "No se han encontrado datos en el CSV";

                /// 7.- Pintamos el resultado de la carga
                table_resultados.InnerHtml = "<strong>Total:</strong> " + items.Count() + "<br /><strong>Nuevos:</strong> " + usuarios_nuevos + "<br /><strong>Antiguos:</strong> " + usuarios_bbdd + "<br /><strong>Errores:</strong> " + usuarios_con_errores;
                table_usuarios.InnerHtml = string.Empty;
                if (usuarios_con_errores > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table id =\"tabla_errores\" class=\"display compact\" style =\"width:100%\"><thead><tr><th>Error</th></tr></thead><tbody>");
                    sb.Append(sbuild.ToString());
                    sb.Append("</tbody></table>");
                    table_usuarios.InnerHtml = sb.ToString();
                }

                /// 8.- Eliminar fichero
                //File.Delete(path);
                block_upload.Visible = false;
                txt_name_file.InnerHtml = string.Empty;
            }
            else
                txt_error.InnerHtml = "No se ha encontrado el fichero";
        }

        private List<Paises> limpiar_valores_paises(List<Paises> lst_paises)
        {
            List<Paises> _paises = new List<Paises>();
            foreach (var _pais in lst_paises)
            {
                _pais.valor = Utils.cleanString(_pais.valor);
                _paises.Add(_pais);
            }
            return _paises;
        }
        private int buscar_pais(string phone_clear, List<Paises> _paises)
        {
            int idPais = 0;
            int index = 0;
            while (idPais == 0)
            {
                string number = phone_clear.Substring(0, index + 1);
                List<Paises> lista_paises = _paises.Where(_ => _.valor.StartsWith(number)).ToList();
                if (lista_paises.Count == 1)
                    idPais = (int)lista_paises[0].id_pais;
                else if (lista_paises.Count == 0)
                    idPais = -1;
                index++;
            }
            return idPais;
        }
    }
}