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
    public partial class dar_baja_usuarios : System.Web.UI.Page
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

        protected void btnGuardar_Click(object sender, EventArgs e)
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
                        name_excel = "Excel_baja_usuario_" + DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + "_" + Utilities.RemoverSignosAcentos(archivo.Name).Replace(" ", "_");

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

                    /// 4.- Si ha guardado el excel, procesamos los datos
                    if (!String.IsNullOrEmpty(name_excel))
                    {
                        try
                        {
                            string path = ruta_fichero + name_excel;

                            int usuarios_correctos = 0;
                            int usuarios_errores = 0;
                            int usuarios_old = 0;

                            /// 4.1.- Cargamos los datos del excel                        
                            IEnumerable<CsvBajaUsuarios> items = LinqToCSV.ReadFileWithExceptionHandling<CsvBajaUsuarios>(path);
                            if (items.Count() > 0)
                            {
                                /// 4.2.- Comprobar si tiene errores
                                foreach (var _user in items)
                                {
                                    /// 5.- Buscar al usuario
                                    List<CLIENTES> _usuarios = da.getUserForMailOrLogin(_user.Email);
                                    if (_usuarios.Count == 1)
                                    {
                                        /// 5.1.- Comprobar si esta dado de baja
                                        if (!_usuarios[0].fecha_baja.HasValue)
                                        {
                                            /// 6.- Dar de baja
                                            CLIENTES _usuario = _usuarios[0];
                                            if (!String.IsNullOrEmpty(_usuario.Comentarios_Internos))
                                                _usuario.Comentarios_Internos = _usuario.Comentarios_Internos + "<br />Baja por rebote definitivo del mail";
                                            else
                                                _usuario.Comentarios_Internos = "Baja por rebote definitivo del mail";
                                            _usuario.fecha_baja = DateTime.Now;
                                            _usuario.activo = ((int)Constantes.activo.NoActivo).ToString();

                                            bool _update_user = da.updateClient(_usuario);
                                            if (_update_user)
                                            {
                                                /// 7.- Añadir entrada en AP
                                                campus_ACCIONES_PERSONA _ap = new campus_ACCIONES_PERSONA();
                                                _ap.idPersona = _usuario.id_cliente;
                                                _ap.idAccion = (long)Constantes.accion.Dar_baja;
                                                _ap.Fecha = DateTime.Now;
                                                _ap.IdOrigen = (long)Constantes.origen.SBS_Admin;
                                                _ap.Comentario = "Baja por rebote definitivo del mail";

                                                long _insert_ap = da.insertPersonAction(_ap);
                                                if (_insert_ap < 1)
                                                    LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir lead al usuario " + _usuario.id_cliente);

                                                usuarios_correctos++;
                                            }
                                            else
                                            {
                                                usuarios_errores++;
                                                LogUtils.InsertarLog("[Error] Se ha producido un error al dar de baja al usuario " + _usuario.id_cliente);                                                
                                            }
                                        }
                                        else
                                            usuarios_old++;
                                    }
                                    else
                                        LogUtils.InsertarLog("[Error] Usuario no encontrado con el mail " + _user.Email);
                                }

                                /// 8.- Pintamos el resultado de la carga
                                table_resultados.InnerHtml = "<strong>Total:</strong> " + items.Count() + "<br /><strong>Dados de baja:</strong> " + usuarios_correctos + "<br /><strong>Antiguos:</strong> " + usuarios_old + "<br /><strong>Errores:</strong> " + usuarios_errores;

                                /// 9.- Desbloqueamos el bloque
                                block_usuarios.Attributes["class"] = block_usuarios.Attributes["class"].Replace("hidden", string.Empty);
                                block_upload.Visible = false;
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
    }
}