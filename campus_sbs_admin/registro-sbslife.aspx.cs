using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace campus_sbs_admin
{
    public partial class registro_sbslife : System.Web.UI.Page
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
                        name_excel = "Excel_suscriptor_sbslife_" + DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + "_" + Utilities.RemoverSignosAcentos(archivo.Name).Replace(" ", "_");

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
                            IEnumerable<CsvSuscriptores> items = LinqToCSV.ReadFileWithExceptionHandling<CsvSuscriptores>(path);
                            if (items.Count() > 0)
                            {
                                /// 4.2.- Comprobar si tiene errores
                                bool _list_errors = false;
                                foreach (var _suscripcion in items)
                                {
                                    if (_suscripcion.Dias == 0 || _suscripcion.Id_Persona == 0)
                                    {
                                        _list_errors = true;
                                        break;
                                    }
                                }

                                /// 4.3.- Pintamos la tabla con los datos extraídos del Excel
                                StringBuilder sbuild = new StringBuilder();
                                sbuild.Append("<table id =\"tabla_suscriptores\" class=\"display compact\" style =\"width:100%\"><thead><tr><th>Linea</th><th>Id_Persona</th><th>Fecha_Inicio</th><th>Dias</th><th>Importe</th><th>Comentarios</th></tr></thead><tbody>");

                                if (_list_errors)
                                {
                                    /// 4.4.- Pintamos la tabla de errores
                                    int _index = 1;
                                    foreach (var _suscripcion in items)
                                    {
                                        if (_suscripcion.Dias == 0 || _suscripcion.Id_Persona == 0)
                                        {
                                            sbuild.Append("<tr>");

                                            sbuild.Append("<td>" + _index + "</td>");
                                            if (_suscripcion.Id_Persona == 0)
                                                sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                            else
                                                sbuild.Append($"<td>{_suscripcion.Id_Persona}</td>");
                                            sbuild.Append($"<td>{(_suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value.ToShortDateString() : DateTime.Today.ToShortDateString())}</td>");
                                            if (_suscripcion.Dias == 0)
                                                sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                            else
                                                sbuild.Append($"<td>{_suscripcion.Dias}</td>");
                                            sbuild.Append($"<td>{_suscripcion.Importe}</td>");
                                            sbuild.Append($"<td>{_suscripcion.Comentarios}</td>");
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
                                    foreach (var _suscripcion in items)
                                    {
                                        sbuild.Append("<tr>");

                                        sbuild.Append("<td>" + _index + "</td>");
                                        sbuild.Append($"<td>{_suscripcion.Id_Persona}</td>");
                                        sbuild.Append($"<td>{(_suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value.ToShortDateString() : DateTime.Today.ToShortDateString())}</td>");
                                        sbuild.Append($"<td>{_suscripcion.Dias}</td>");
                                        sbuild.Append($"<td>{_suscripcion.Importe}</td>");
                                        sbuild.Append($"<td>{_suscripcion.Comentarios}</td>");
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
            int suscripcion_correcta = 0;
            int suscripcion_incorrecta = 0;

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
                IEnumerable<CsvSuscriptores> items = LinqToCSV.ReadFileWithExceptionHandling<CsvSuscriptores>(path);
                if (items.Count() > 0)
                {
                    /// 4.2.- Sacar los clientes
                    List<long> _id_users = items.Select(_ => _.Id_Persona).ToList();
                    List<CLIENTES> _users = da.getUserByList(_id_users);
                    List<EDU_Suscriptores> _suscriptores = da.getEduSuscriptores(_id_users);

                    /// 4.3.- Recorrer las suscripciones
                    foreach (var _suscripcion in items)
                    {
                        CLIENTES _user = _users.Where(_ => _suscripcion.Id_Persona == _.id_cliente).FirstOrDefault();
                        if (_user != null)
                        {
                            if (!_user.fecha_baja.HasValue)
                            {
                                if (_suscripcion.Importe.HasValue && _suscripcion.Importe.Value > new decimal(0))
                                {
                                    /// 5.- Comprobar si el usuario tiene una suscripción
                                    List<EDU_Suscriptores> _suscripciones_user = _suscriptores.Where(_ => _.Id_Edu_Producto == (long)Constantes.producto.SBS_Life && _.Id_Persona == _suscripcion.Id_Persona).ToList();
                                    if (_suscripciones_user.Count == 0)
                                    {
                                        /// 5.1.- Registro en SBS Life
                                        bool _register_user = Utilities.RegisterUser(_suscripcion.Id_Persona);
                                        if (_register_user)
                                        {
                                            /// 5.1.- Añadir una suscripción
                                            EDU_Suscriptores _suscripcion_user = new EDU_Suscriptores();
                                            _suscripcion_user.Id_Edu_Producto = (long)Constantes.producto.SBS_Life;
                                            _suscripcion_user.Id_Persona = _suscripcion.Id_Persona;
                                            _suscripcion_user.Fecha_Alta = _suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value : DateTime.Today;
                                            _suscripcion_user.Fecha_Fin = _suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value.AddDays(_suscripcion.Dias) : DateTime.Today.AddDays(_suscripcion.Dias);
                                            _suscripcion_user.Importe = _suscripcion.Importe.Value;
                                            _suscripcion_user.Comentarios = _suscripcion.Comentarios;
                                            _suscripcion_user.Activo = true;

                                            int _insert_suscripcion = da.insertEduSuscriptor(_suscripcion_user);
                                            if (_insert_suscripcion > 0)
                                            {
                                                Utilities.SendEmailUser(_user, _suscripcion_user, false);
                                                suscripcion_correcta++;
                                            }
                                            else
                                            {
                                                suscripcion_incorrecta++;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona);
                                                sbuild.Append($"<tr><td>Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona + "</td></tr>");
                                            }
                                        }
                                        else
                                        {
                                            suscripcion_incorrecta++;
                                            LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion.Id_Persona);
                                            sbuild.Append($"<tr><td>Se ha producido un error al añadir la suscripción al usuario " + _suscripcion.Id_Persona + "</td></tr>");
                                        }
                                    }
                                    else
                                    {
                                        DateTime _max_date = _suscripciones_user.Where(_ => _.Fecha_Fin.HasValue).Select(_ => _.Fecha_Fin.Value).Max();
                                        DateTime _fecha_inicio = _suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value : DateTime.Today;
                                        if (_max_date >= _fecha_inicio)
                                        {
                                            /// 5.1.- Añadir una suscripción
                                            EDU_Suscriptores _suscripcion_user = new EDU_Suscriptores();
                                            _suscripcion_user.Id_Edu_Producto = (long)Constantes.producto.SBS_Life;
                                            _suscripcion_user.Id_Persona = _suscripcion.Id_Persona;
                                            _suscripcion_user.Fecha_Alta = _max_date.AddDays(1);
                                            _suscripcion_user.Fecha_Fin = _max_date.AddDays(_suscripcion.Dias + 1);
                                            _suscripcion_user.Importe = _suscripcion.Importe.Value;
                                            _suscripcion_user.Comentarios = _suscripcion.Comentarios;
                                            _suscripcion_user.Activo = true;

                                            int _insert_suscripcion = da.insertEduSuscriptor(_suscripcion_user);
                                            if (_insert_suscripcion > 0)
                                            {
                                                Utilities.SendEmailUser(_user, _suscripcion_user, false);
                                                suscripcion_correcta++;
                                            }
                                            else
                                            {
                                                suscripcion_incorrecta++;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona);
                                                sbuild.Append($"<tr><td>Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona + "</td></tr>");
                                            }
                                        }
                                        else
                                        {
                                            /// 5.1.- Añadir una suscripción
                                            EDU_Suscriptores _suscripcion_user = new EDU_Suscriptores();
                                            _suscripcion_user.Id_Edu_Producto = (long)Constantes.producto.SBS_Life;
                                            _suscripcion_user.Id_Persona = _suscripcion.Id_Persona;
                                            _suscripcion_user.Fecha_Alta = _suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value : DateTime.Today;
                                            _suscripcion_user.Fecha_Fin = _suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value.AddDays(_suscripcion.Dias) : DateTime.Today.AddDays(_suscripcion.Dias);
                                            _suscripcion_user.Importe = _suscripcion.Importe.Value;
                                            _suscripcion_user.Comentarios = _suscripcion.Comentarios;
                                            _suscripcion_user.Activo = true;

                                            int _insert_suscripcion = da.insertEduSuscriptor(_suscripcion_user);
                                            if (_insert_suscripcion > 0)
                                            {
                                                Utilities.SendEmailUser(_user, _suscripcion_user, false);
                                                suscripcion_correcta++;
                                            }
                                            else
                                            {
                                                suscripcion_incorrecta++;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona);
                                                sbuild.Append($"<tr><td>Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona + "</td></tr>");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    /// 5.- Comprobar si el usuario tiene una suscripción
                                    List<EDU_Suscriptores> _suscripciones_user = _suscriptores.Where(_ => _.Id_Edu_Producto == (long)Constantes.producto.SBS_Life && _.Id_Persona == _suscripcion.Id_Persona).ToList();
                                    if (_suscripciones_user.Count == 0)
                                    {
                                        /// 5.1.- Registro en SBS Life
                                        //bool _register_user = RegisterUser(_suscripcion.Id_Persona);
                                        bool _register_user = Utilities.RegisterUser(_suscripcion.Id_Persona);
                                        if (_register_user)
                                        {
                                            /// 5.1.- Añadir una suscripción
                                            EDU_Suscriptores _suscripcion_user = new EDU_Suscriptores();
                                            _suscripcion_user.Id_Edu_Producto = (long)Constantes.producto.SBS_Life;
                                            _suscripcion_user.Id_Persona = _suscripcion.Id_Persona;
                                            _suscripcion_user.Fecha_Alta = _suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value : DateTime.Today;
                                            _suscripcion_user.Fecha_Fin = _suscripcion.Fecha_Inicio.HasValue ? _suscripcion.Fecha_Inicio.Value.AddDays(_suscripcion.Dias) : DateTime.Today.AddDays(_suscripcion.Dias);
                                            _suscripcion_user.Importe = _suscripcion.Importe.HasValue ? _suscripcion.Importe.Value : 0;
                                            _suscripcion_user.Comentarios = _suscripcion.Comentarios;
                                            _suscripcion_user.Activo = true;

                                            int _insert_suscripcion = da.insertEduSuscriptor(_suscripcion_user);
                                            if (_insert_suscripcion > 0)
                                            {
                                                Utilities.SendEmailUser(_user, _suscripcion_user, false);
                                                suscripcion_correcta++;
                                            }
                                            else
                                            {
                                                suscripcion_incorrecta++;
                                                LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona);
                                                sbuild.Append($"<tr><td>Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona + "</td></tr>");
                                            }
                                        }
                                        else
                                        {
                                            suscripcion_incorrecta++;
                                            LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion.Id_Persona);
                                            sbuild.Append($"<tr><td>Se ha producido un error al añadir la suscripción al usuario " + _suscripcion.Id_Persona + "</td></tr>");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    txt_error.InnerHtml = "No se han encontrado datos en el CSV";

                /// 7.- Pintamos el resultado de la carga
                table_resultados.InnerHtml = "<strong>Total:</strong> " + items.Count() + "<br /><strong>Nuevos:</strong> " + suscripcion_correcta + "<br /><strong>Errores:</strong> " + suscripcion_incorrecta;
                table_usuarios.InnerHtml = string.Empty;
                if (suscripcion_incorrecta > 0)
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

        /*private bool RegisterUser(long id_Persona)
        {
            bool _register = false;
            //Uri url = new Uri("https://localhost:44349/api/migrate/register/" + id_Persona);
            Uri url = new Uri(ConfigurationManager.AppSettings["url_register_sbslife"] + id_Persona);

            try
            {
                HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(url);
                // *** Set properties
                loHttp.Timeout = 100000;     // 10 secs
                loHttp.UserAgent = "Code Sample Web Client";

                // *** Retrieve request info headers
                HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
                if (loWebResponse.StatusCode == HttpStatusCode.OK)
                    _register = true;
                else
                    LogUtils.InsertarLog(string.Format(" ERROR - La llamada al servidor remoto {0} {1} resultó en un error http {2} {3}.", "GET", url, loWebResponse.StatusCode, loWebResponse.StatusDescription));

                loWebResponse.Close();
            }
            catch (WebException wex)
            {
                var httpResponse = wex.Response as HttpWebResponse;
                if (httpResponse != null)
                    LogUtils.InsertarLog(string.Format(" ERROR - La llamada al servidor remoto {0} {1} resultó en un error http {2} {3}.", "GET", url, httpResponse.StatusCode, httpResponse.StatusDescription));
                else
                    LogUtils.InsertarLog(string.Format(" ERROR - La llamada al servidor remoto {0} {1} resultó en un error.", "GET", url));

                _register = false;
            }
            catch (Exception)
            {
                _register = false;
            }

            return _register;
        }
        private void SendEmailUser(CLIENTES _user, EDU_Suscriptores _suscripcion)
        {
            /// 1.- Mandar mail al usuario
            string template = Utilities.getPlantillaMail("suscripcion-sbs-life", ConfigurationManager.AppSettings["urlTemplate"]);
            if (!String.IsNullOrEmpty(template))
            {
                template = template.Replace("###Nombre###", _user.Nombre_Completo);
                template = template.Replace("###fecha_inicio###", _suscripcion.Fecha_Alta.ToShortDateString());
                template = template.Replace("###fecha_fin###", _suscripcion.Fecha_Fin.Value.ToShortDateString());
                template = template.Replace("###login###", _user.email);
                template = template.Replace("###password###", Utils.toDecodeString(_user.password));
                template = template.Replace("###url###", "https://life.spainbs.com/");
                template = template.Replace("###KEY###", _user.Key);
            }

            /// 6.1.8.- Resto de datos necesarios para el mail
            EMAIL_CONTENT email_data = new EMAIL_CONTENT();
            email_data.nombreTo = _user.Nombre_Completo;
            email_data.mailTo = _user.email;
            email_data.priority = 1;
            email_data.asunto = "¡Bienvenido a Spain Business School!";
            email_data.body = template;

            long insert_mail = da.insertEmailContent(email_data);
            if (insert_mail > 0)
            {
                /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                long idLog = da.insertLog(_user.id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                if (idLog < 1)
                    LogUtils.InsertarLog(" ERROR - Error al guardar la entrada en el log del usuario " + _user.id_cliente);
            }
            else
                LogUtils.InsertarLog(" ERROR - Se ha producido un error al mandar el mail de activación al usuario " + _user.id_cliente);
        }*/
    }
}