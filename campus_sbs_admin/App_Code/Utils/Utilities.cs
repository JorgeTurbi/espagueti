using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web;

namespace campus_sbs_admin
{
    public class Utilities
    {
        #region Plantillas Mail

        public static string getPlantillaMail(string page, string route)
        {
            string template = string.Empty;
            try
            {
                /// Obtenemos la ruta donde se encuentran los ficheros
                string rutaBase = HttpContext.Current.Server.MapPath(route);
                StreamReader sr = new StreamReader(rutaBase + "/" + page + ".html", System.Text.Encoding.Default);
                template = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utilities.cs::getPlantillaMail()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                template = string.Empty;
            }

            return template;
        }

        #endregion

        #region Asunto mail

        public static string getAsuntoMail(string literal, string route)
        {
            string asunto = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(HttpContext.Current.Server.MapPath(route));
                asunto = ds.Tables["asuntos"].Rows[0][literal].ToString();
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utilities.cs::getAsuntoMail()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                asunto = string.Empty;
            }
            return asunto;
        }

        #endregion

        public static string getCalificationByNote(decimal? _nota)
        {
            string calificacion = string.Empty;

            if (_nota == new decimal(-1))
                calificacion = "No presentado";
            else if (_nota < new decimal(4))
                calificacion = "No Apto";
            else if (_nota >= new decimal(4) && _nota <= new decimal(5))
                calificacion = "Apto-";
            else if (_nota > new decimal(5) && _nota <= new decimal(8))
                calificacion = "Apto";
            else
                calificacion = "Apto+";

            return calificacion;
        }

        #region Recuperar ficheros html

        public static string dameBloqueHtml(string nombre)
        {
            string block = string.Empty;

            try
            {
                string ruta = MapPath(ConfigurationManager.AppSettings["block_html"].Trim());

                StreamReader sr = new StreamReader(new FileStream(ruta + "/" + nombre + ".html", FileMode.Open, FileAccess.Read));
                block = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utilities.cs::dameBloqueHtml()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                block = string.Empty;
            }

            return block;
        }

        #endregion

        #region Gestion del MapPath

        /// <summary>
        /// Devuelve la ruta física de un fichero
        /// </summary>
        /// <param name="rutaFicheros"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string MapPath(string rutaFicheros, string localPath)
        {
            return Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + rutaFicheros + "/" + localPath);
        }

        /// <summary>
        /// Devuelve la ruta física de un fichero
        /// </summary>
        /// <param name="rutaFicheros"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string MapPath(string rutaFicheros)
        {
            return Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + rutaFicheros + "/");
        }

        #endregion

        #region Tipo de anuncios

        public static string getTypeMessage(long type)
        {
            string type_message = string.Empty;
            if (type == (long)Constantes.type_message.Aviso)
                type_message = "<i class='fas fa-bell fa-2x vertical-align-middle' aria-hidden='true'></i>";
            else if (type == (long)Constantes.type_message.Contenido)
                type_message = "<i class='fab fa-leanpub fa-2x vertical-align-middle' aria-hidden='true'></i>";
            else if (type == (long)Constantes.type_message.Email)
                type_message = "<i class='fas fa-at fa-2x vertical-align-middle' aria-hidden='true'></i>";
            else if (type == (long)Constantes.type_message.Mensaje)
                type_message = "<i class='fas fa-envelope fa-2x vertical-align-middle' aria-hidden='true'></i>";
            return type_message;
        }
        
        #endregion

        /// <summary>
        /// Devuelve un listado con los mails no leídos
        /// </summary>
        /// <param name="list_mails"></param>
        /// <param name="idUser"></param>
        /// <param name="idCurso"></param>
        /// <returns></returns>
        public static List<campus_ANUNCIO> getMessages(List<campus_ANUNCIO> list_mails, long idUser, long idCurso)
        {
            DataAccess da = new DataAccess();
            List<campus_ANUNCIO> list_messages = new List<campus_ANUNCIO>();

            /// 1.- Sacar los mails que tienen idMail != null
            list_mails = list_mails.Where(m => m.Id_Mail != null).ToList();

            /// 2.- Sacar las acciones persona del usuario
            List<campus_ACCIONES_PERSONA> list_actions = da.getActionsByUser(idUser);

            /// 3.- Sacar los idMail leídos
            List<long> list_leidos = list_actions.Where(a => a.idLanding != null).Select(a => a.idLanding.Value).Distinct().ToList();

            /// 4.- Devolver los mails no leídos
            list_messages = list_mails.Where(m => !list_leidos.Contains(m.Id_Mail.Value)).ToList();

            return list_messages;
        }

        #region Función para comprobar que el usuario está donde puede estar

        public static bool comprobate_user(CLIENTES user)
        {
            bool comprobate = false;
            DataAccess da = new DataAccess();

            if (!String.IsNullOrEmpty(user.Administrador) && user.Administrador == ((int)Constantes.activo.Activo).ToString())
                comprobate = true;
            else
            {
                List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                if (list_users_mails.Contains(user.id_cliente.ToString()))
                    comprobate = true;
            }

            return comprobate;
        }
        
        public static bool comprobate_users(CLIENTES _user)
        {
            bool _comprobate = false;
            DataAccess da = new DataAccess();

            if ((!String.IsNullOrEmpty(_user.Administrador) && _user.Administrador == ((int)Constantes.activo.Activo).ToString()) || (_user.Comercial != null && _user.Comercial.Value))
                _comprobate = true;
            else
            {
                List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                if (list_users_mails.Contains(_user.id_cliente.ToString()))
                    _comprobate = true;
            }

            return _comprobate;
        }

        #endregion

        public static string PonerPuntoMiles(decimal numero)
        {
            string PuntoMil = string.Empty;
            string signo = string.Empty;
            if (numero.ToString().Contains("-"))
                signo = "-";

            numero = Math.Abs(numero);
            if (numero.ToString().Contains(","))
            {
                string valor = numero.ToString().Split(',')[0];

                int num = valor.Length;

                if (num >= 4)
                {
                    if (num >= 7)
                    {
                        if (num >= 10)
                        {
                            PuntoMil = valor.Insert((num - 3), ".");
                            PuntoMil = PuntoMil.Insert((num - 6), ".");
                            PuntoMil = PuntoMil.Insert((num - 9), ".");
                            PuntoMil = PuntoMil + "," + numero.ToString().Split(',')[1];
                        }
                        else
                        {
                            PuntoMil = valor.Insert((num - 3), ".");
                            PuntoMil = PuntoMil.Insert((num - 6), ".");
                            PuntoMil = PuntoMil + "," + numero.ToString().Split(',')[1];
                        }
                    }
                    else
                    {
                        PuntoMil = valor.Insert((num - 3), ".");
                        PuntoMil = PuntoMil + "," + numero.ToString().Split(',')[1];
                    }
                }
                else
                    PuntoMil = numero.ToString();
            }
            else
            {
                int num = numero.ToString().Length;

                if (num >= 4)
                {
                    if (num >= 7)
                    {
                        if (num >= 10)
                        {
                            PuntoMil = numero.ToString().Insert((num - 3), ".");
                            PuntoMil = PuntoMil.Insert((num - 6), ".");
                            PuntoMil = PuntoMil.Insert((num - 9), ".");
                        }
                        else
                        {
                            PuntoMil = numero.ToString().Insert((num - 3), ".");
                            PuntoMil = PuntoMil.Insert((num - 6), ".");
                        }
                    }
                    else
                    {
                        PuntoMil = numero.ToString().Insert((num - 3), ".");
                    }
                }
                else
                    PuntoMil = numero.ToString();
            }
            if (signo != string.Empty)
                PuntoMil = signo + PuntoMil;

            return PuntoMil;
        }

        #region Fechas

        public static string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            string fecha = dtinfo.GetMonthName(month);
            return new CultureInfo("es-ES", false).TextInfo.ToTitleCase(fecha);
        }

        #endregion

        #region Función para reemplazar texto en un documento de Word

        /*public static void replaceText(Microsoft.Office.Interop.Word._Application oWord, object findText, object replaceWithText)
        {
            try
            {
                oWord.Selection.Find.ClearFormatting();
                oWord.Selection.Find.Replacement.ClearFormatting();

                object replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
                object matchCase = false;
                object matchWholeWord = true;
                object matchWildCards = false;
                object matchSoundsLike = false;
                object matchAllWordForms = false;
                object forward = true;
                object format = false;
                object matchKashida = false;
                object matchDiacritics = false;
                object matchAlefHamza = false;
                object matchControl = false;
                object read_only = false;
                object visible = true;
                object wrap = 1;

                oWord.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord, ref matchWildCards, ref matchSoundsLike,
                    ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace, ref matchKashida,
                    ref matchDiacritics, ref matchAlefHamza, ref matchControl);
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utilities.cs::replaceText()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }*/

        #endregion

        public static bool ficha_alumno_min(CLIENTES user)
        {
            bool error = false;

            /// Comprobar si le faltan datos en la ficha del alumno
            if (String.IsNullOrEmpty(user.telefono_contacto) || String.IsNullOrEmpty(user.Nacionalidad) || String.IsNullOrEmpty(user.Lugar_nac) || String.IsNullOrEmpty(user.localidad) || user.fecha_nac == null || String.IsNullOrEmpty(user.sexo) || String.IsNullOrEmpty(user.nif) || user.Nivel_Estudios == null || user.Experiencia == null || user.Situacion_Actual == null)
                error = true;

            return error;
        }

        private const string ConSignos = "áàäâéèëêíìïîóòöôúùuûñÁÀÄÂÉÈËÊÍÌÏÎÓÒÖÔÚÙÜÛçÇ";
        private const string SinSignos = "aaaaeeeeiiiioooouuuunAAAAEEEEIIIIOOOOUUUUcC";
        public static string RemoverSignosAcentos(string texto)
        {
            var textoSinAcentos = string.Empty;

            foreach (var caracter in texto)
            {
                var indexConAcento = ConSignos.IndexOf(caracter);
                if (indexConAcento > -1)
                    textoSinAcentos = textoSinAcentos + (SinSignos.Substring(indexConAcento, 1));
                else
                    textoSinAcentos = textoSinAcentos + (caracter);
            }
            return textoSinAcentos;
        }
        public static string obtenerNombreEstadoNewsletter(int? status)
        {
            string estado = string.Empty;
            if (status == (int)Constantes.status_newsletter.Pendiente)
                estado = "Pendiente";
            else if (status == (int)Constantes.status_newsletter.Enviando)
                estado = "Enviando";
            else if (status == (int)Constantes.status_newsletter.Enviado)
                estado = "Enviado";
            else if (status == (int)Constantes.status_newsletter.Cerrado)
                estado = "Cerrado";
            return estado;
        }
        public static string quitarAcentos(string Texto)
        {
            try
            {
                //Replazamos los acentos de la a
                Texto = Texto.Replace("á", "&aacute;");
                //Replazamos los acentos de la e
                Texto = Texto.Replace("é", "&eacute;");
                //Replazamos los acentos de la i
                Texto = Texto.Replace("í", "&iacute;");
                //Replazamos los acentos de la o
                Texto = Texto.Replace("ó", "&oacute;");
                //Replazamos los acentos de la u
                Texto = Texto.Replace("ú", "&uacute;");
                //Replazamos los acentos de la A
                Texto = Texto.Replace("Á", "&Aacute;");
                //Replazamos los acentos de la E
                Texto = Texto.Replace("É", "&Eacute;");
                //Replazamos los acentos de la I
                Texto = Texto.Replace("Í", "&Iacute;");
                //Replazamos los acentos de la O
                Texto = Texto.Replace("Ó", "&Oacute;");
                //Replazamos los acentos de la U
                Texto = Texto.Replace("Ú", "&Uacute;");
                //Replazamos la ñ
                Texto = Texto.Replace("ñ", "&ntilde;");
                //Replazamos la Ñ
                Texto = Texto.Replace("Ñ", "&Ntilde;");
                //Replazamos las dieresis de la a
                Texto = Texto.Replace("ä", "&auml;");
                //Replazamos las dieresis de la e
                Texto = Texto.Replace("ë", "&euml;");
                //Replazamos las dieresis de la i
                Texto = Texto.Replace("ï", "&iuml;");
                //Replazamos las dieresis de la o
                Texto = Texto.Replace("ö", "&ouml;");
                //Replazamos las dieresis de la u
                Texto = Texto.Replace("ü", "&uuml;");
                //Replazamos las dieresis de la A
                Texto = Texto.Replace("Ä", "&Auml;");
                //Replazamos las dieresis de la E
                Texto = Texto.Replace("Ë", "&Euml;");
                //Replazamos las dieresis de la I
                Texto = Texto.Replace("Ï", "&Iuml;");
                //Replazamos las dieresis de la O
                Texto = Texto.Replace("Ö", "&Ouml;");
                //Replazamos las dieresis de la U
                Texto = Texto.Replace("Ü", "&Uuml;");

                /// Interrogación
                Texto = Texto.Replace("¿", "&iquest;");
                /// Exclamación
                Texto = Texto.Replace("¡", "&iexcl;");

                return Texto;
            }
            catch
            {
                return "";
            }
        }

        public static bool validarEmail(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static string recortarCadena(string cadena, int numCaracteresRecorte)
        {
            string cadenaRecortada = "";
            if (cadena.Length > numCaracteresRecorte)
                //Nos quedamos con los x primeros caracteres y para no partir una palabra justo hasta el espacio anterior.
                cadenaRecortada = cadena.Substring(0, cadena.Substring(0, numCaracteresRecorte).LastIndexOf(" ")) + " ...";
            else
                cadenaRecortada = cadena;

            return cadenaRecortada;
        }

        #region Tipo de estados seguimiento

        public static string obtenerEstadoSeguimiento(int _status)
        {
            string _type = string.Empty;
            if (_status == (int)Constantes.type_status_action.status_cerrar)
                _type = "Cerrado";
            else if (_status == (int)Constantes.type_status_action.status_rechazado)
                _type = "Rechazado";
            else if (_status == (int)Constantes.type_status_action.status_duplicado)
                _type = "Duplicado";
            else if (_status == (int)Constantes.type_status_action.status_futuro)
                _type = "Futuro";
            else if (_status == (int)Constantes.type_status_action.status_matriculado)
                _type = "Matriculado";
            else
                _type = " - ";

            return _type;
        }

        #endregion

        #region Generar Password nuevo o login nuevo

        public static string generate_password(CLIENTES _user)
        {
            string _password = string.Empty;
            try
            {
                DataAccess da = new DataAccess();

                /// 1.- Limpiar el nombre y los apellidos
                string _name = RemoverSignosAcentos(Utils.cleanString(_user.nombre).Replace(" ", "").ToLower());
                string _surname = string.Empty;
                if (!String.IsNullOrEmpty(_user.apellidos) && _user.apellidos != ".")
                    _surname = RemoverSignosAcentos(Utils.cleanString(_user.apellidos).Replace(" ", "").ToLower());

                /// 2.- Generar el Password
                string part1 = _name.Substring(0, 2);
                string part3 = string.Empty;
                if (!String.IsNullOrEmpty(_surname) && _surname.Length > 2)
                    part3 = _surname.Substring(0, 2);
                else
                    part3 = "_";

                Random rand = new Random();
                string part2 = rand.Next(10, 99).ToString();
                string part4 = rand.Next(10, 99).ToString();

                _password = part1 + part2 + part3 + part4 + "sbs";
                _password = Utils.toCodeString(_password);
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utils.cs::generate_password()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _password = null;
            }
            return _password;
        }
        public static string generate_login(CLIENTES _user)
        {
            string _login = string.Empty;
            try
            {
                DataAccess da = new DataAccess();

                if (!String.IsNullOrEmpty(_user.apellidos) && _user.apellidos != ".")
                {
                    /// 1.- Limpiar el nombre y los apellidos
                    string _name = RemoverSignosAcentos(Utils.cleanString(_user.nombre).Replace(" ", "").ToLower());
                    string _surname = RemoverSignosAcentos(Utils.cleanString(_user.apellidos).Replace(" ", "").ToLower());

                    /// 2.- Generar el login
                    _login = comprobar_login(_name + "." + _surname);
                }
                else
                {
                    /// 1.- Limpiar el nombre
                    string _name = RemoverSignosAcentos(Utils.cleanString(_user.nombre).Replace(" ", "").ToLower());

                    /// 2.- Generar el login
                    _login = comprobar_login(_name + "_sbs");
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utilities.cs::generate_login()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _login = null;
            }
            return _login;
        }
        private static string comprobar_login(string login)
        {
            string _login = login;
            try
            {
                DataAccess da = new DataAccess();

                /// 1.- Comprobar si el login existe
                bool _exists = true;
                int c = 0;

                /// 2.- Comprueba que no existe el login en la tabla clientes, si existe concatena login + "." + contador de 1 en 1.
                while (_exists)
                {
                    List<CLIENTES> list_clients = da.getUserForMailOrLogin(_login);
                    if (list_clients.Count > 0)
                    {
                        /// Existe el cliente
                        _exists = true;
                        c++;
                        if (c == 1)
                            _login = _login + "." + c;
                        else
                        {
                            string[] asSplit = _login.Split('.');
                            string sNumAnterior = asSplit[2];
                            _login = _login.Replace(sNumAnterior, c.ToString());
                        }
                    }
                    else
                        _exists = false;
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utilities.cs::comprobar_login()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _login = string.Empty;
            }
            return _login;
        }

        #endregion

        /*public static string LimpiarTexto(string Texto)
        {
            try
            {
                Texto = Texto.Replace("á", "a");
                Texto = Texto.Replace("à", "a");
                Texto = Texto.Replace("ä", "a");
                Texto = Texto.Replace("Á", "a");
                Texto = Texto.Replace("À", "a");
                Texto = Texto.Replace("Ä", "a");
                Texto = Texto.Replace("Â", "a");
                Texto = Texto.Replace("Ã", "a");
                Texto = Texto.Replace("Å", "a");
                Texto = Texto.Replace("ã", "a");
                Texto = Texto.Replace("â", "a");
                Texto = Texto.Replace("å", "a");

                Texto = Texto.Replace("é", "e");
                Texto = Texto.Replace("è", "e");
                Texto = Texto.Replace("ë", "e");
                Texto = Texto.Replace("É", "e");
                Texto = Texto.Replace("È", "e");
                Texto = Texto.Replace("Ë", "e");
                Texto = Texto.Replace("Ê", "e");
                Texto = Texto.Replace("ê", "e");

                Texto = Texto.Replace("í", "i");
                Texto = Texto.Replace("ì", "i");
                Texto = Texto.Replace("ï", "i");
                Texto = Texto.Replace("Í", "i");
                Texto = Texto.Replace("Ì", "i");
                Texto = Texto.Replace("Ï", "i");
                Texto = Texto.Replace("Î", "i");
                Texto = Texto.Replace("î", "i");

                Texto = Texto.Replace("ó", "o");
                Texto = Texto.Replace("ò", "o");
                Texto = Texto.Replace("ö", "o");
                Texto = Texto.Replace("Ó", "o");
                Texto = Texto.Replace("Ò", "o");
                Texto = Texto.Replace("Ö", "o");
                Texto = Texto.Replace("Ô", "o");
                Texto = Texto.Replace("Õ", "o");
                Texto = Texto.Replace("õ", "o");
                Texto = Texto.Replace("ô", "o");

                Texto = Texto.Replace("ú", "u");
                Texto = Texto.Replace("ù", "u");
                Texto = Texto.Replace("ü", "u");
                Texto = Texto.Replace("Ú", "u");
                Texto = Texto.Replace("Ù", "u");
                Texto = Texto.Replace("Ü", "u");
                Texto = Texto.Replace("Û", "u");
                Texto = Texto.Replace("û", "u");

                //Texto = Texto.Replace("ñ", "n");
                Texto = Texto.Replace("ç", "c");
                //Texto = Texto.Replace("Ñ", "n");
                Texto = Texto.Replace("Ç", "c");

                Texto = Texto.Replace("=", "");
                Texto = Texto.Replace(">", "");
                Texto = Texto.Replace("<", "");
                Texto = Texto.Replace(";", "");
                Texto = Texto.Replace("+", "");
                Texto = Texto.Replace("-", "");
                Texto = Texto.Replace("(", "");
                Texto = Texto.Replace(")", "");
                Texto = Texto.Replace(",", "");
                Texto = Texto.Replace(".", "");
                Texto = Texto.Replace(":", "");
                Texto = Texto.Replace("%", "");
                Texto = Texto.Replace("ª", "");
                Texto = Texto.Replace("º", "");
                Texto = Texto.Replace("¡", "");
                Texto = Texto.Replace("!", "");
                Texto = Texto.Replace("?", "");
                Texto = Texto.Replace("¿", "");
                Texto = Texto.Replace("/", "");
                Texto = Texto.Replace("\n", "");
                Texto = Texto.Replace("\r", "");
                Texto = Texto.Replace("\t", "");
                Texto = Texto.Replace("»", "");
                Texto = Texto.Replace("«", "");
                Texto = Texto.Replace("&", "");
                Texto = Texto.Replace("'", "");
                Texto = Texto.Replace("^", "");
                Texto = Texto.Replace("*", "");
                Texto = Texto.Replace("{", "");
                Texto = Texto.Replace("}", "");
                Texto = Texto.Replace("[", "");
                Texto = Texto.Replace("]", "");
                Texto = Texto.Replace("|", "");
                Texto = Texto.Replace("´", "");
                Texto = Texto.Replace("`", "");
                Texto = Texto.Replace("\"", "");
                Texto = Texto.Replace("“", "");
                Texto = Texto.Replace("”", "");
                Texto = Texto.Replace("’", "");
                Texto = Texto.Replace("’", "");
                Texto = Texto.Replace("\"", "");

                //****************************
                Texto = Texto.Replace(Convert.ToChar(145).ToString(), "");
                Texto = Texto.Replace(Convert.ToChar(146).ToString(), "");
                Texto = Texto.Replace(Convert.ToChar(147).ToString(), "");
                Texto = Texto.Replace(Convert.ToChar(148).ToString(), "");
                Texto = Texto.Replace(Convert.ToChar(180).ToString(), "");
                Texto = Texto.Replace("     ", " ");
                Texto = Texto.Replace("    ", " ");
                Texto = Texto.Replace("   ", " ");
                Texto = Texto.Replace("  ", " ");
                //***************************
                Texto = Texto.Replace("µ", "");
                Texto = Texto.Replace("®", "");
                Texto = Texto.Replace("©", "");
                Texto = Texto.Replace("§", "");
                Texto = Texto.Replace("¤", "");
                Texto = Texto.Replace("±", "");

                Texto = Texto.Replace("$", "");
                Texto = Texto.Replace("@", "");

                return Texto;
            }
            catch
            {
                return string.Empty;
            }
        }*/
        
        #region Registrar usuario en SBS Life

        public static bool RegisterUser(long id_Persona)
        {
            bool _register = false;

            Uri url = new Uri(ConfigurationManager.AppSettings["url_register_sbslife"] + id_Persona);

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                
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

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion

        public static bool SendEmailUser(CLIENTES _user, EDU_Suscriptores _suscripcion, bool tpv)
        {
            DataAccess da = new DataAccess();

            bool _send = false;
            
            /// 1.- Mandar mail al usuario
            string template = Utilities.getPlantillaMail("suscripcion-sbs-life", ConfigurationManager.AppSettings["urlTemplate"]);
            if (!String.IsNullOrEmpty(template))
            {
                template = template.Replace("###Nombre###", _user.Nombre_Completo);
                if (tpv)
                    template = template.Replace("###Pago_TPV###", "Ya hemos verificado tu pago y hemos procedido a activar tu suscripción en SBS life.");
                else
                    template = template.Replace("###Pago_TPV###", "Hemos procedido a activar tu suscripción en SBS life.");
                template = template.Replace("###url###", "https://life.spainbs.com/");
                template = template.Replace("###fecha_fin###", _suscripcion.Fecha_Fin.Value.ToShortDateString());
                template = template.Replace("###password###", Utils.toDecodeString(_user.password));                
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
                _send = true;

                /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                long idLog = da.insertLog(_user.id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                if (idLog < 1)
                    LogUtils.InsertarLog(" ERROR - Error al guardar la entrada en el log del usuario " + _user.id_cliente);
            }
            else
                LogUtils.InsertarLog(" ERROR - Se ha producido un error al mandar el mail de activación al usuario " + _user.id_cliente);

            return _send;
        }
    }
}