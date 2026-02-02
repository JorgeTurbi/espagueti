using sbs_DAL;
using SpreadsheetLight;
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
    public partial class suscriptores_mantenimiento : System.Web.UI.Page
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
                    long idList = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"]) : -1;
                    if (idList == -1)
                        Response.Redirect("lista-suscriptores.aspx");
                    else
                        /// 3.- Pintar el botón volver
                        btn_back.HRef = "suscriptores.aspx?idl=" + idList;
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
                if (extension == ".xlsx" || extension == ".xls")
                {
                    /// 2.2.- Buscar la ruta donde se van a dejar los ficheros
                    string rutaDoc = ConfigurationManager.AppSettings["routeTempSuscriptores"];
                    if (!(Directory.Exists(rutaDoc)))
                        Directory.CreateDirectory(rutaDoc);

                    /// 2.3.- Comprobar si ha subido un fichero, si ya había uno cargado, en este caso eliminar el cargado previamente
                    if (!String.IsNullOrEmpty(name_file.Value))
                        File.Delete(rutaDoc + name_file.Value);

                    /// 3.- Cargar el fichero excel            
                    string name_excel = string.Empty;
                    if (Directory.Exists(rutaDoc))
                    {
                        FileInfo archivo = new FileInfo(fuFile.PostedFile.FileName);
                        name_excel = "Excel_Suscriptores_" + DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + "_" + Utilities.RemoverSignosAcentos(archivo.Name).Replace(" ", "_");

                        try
                        {
                            fuFile.PostedFile.SaveAs(rutaDoc + name_excel);
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
                        string path = rutaDoc + name_excel;

                        int usuarios_correctos = 0;
                        int usuarios_errores = 0;

                        /// 4.1.- Cargamos los datos del excel
                        List<EMAIL_SUSCRIPCIONES> list_suscriptores = load_excel(path);
                        if (list_suscriptores.Count > 0)
                        {
                            /// 4.2.- Comprobar si tiene errores
                            bool _list_errors = false;
                            foreach (EMAIL_SUSCRIPCIONES sus in list_suscriptores)
                            {
                                if (sus.id_alumno == -1 || String.IsNullOrEmpty(sus.nombre_completo) || String.IsNullOrEmpty(sus.mail) || !Utils.esMailCorrecto(sus.mail))
                                {
                                    _list_errors = true;
                                    break;
                                }
                            }

                            /// 4.3.- Pintamos la tabla con los datos extraídos del Excel
                            StringBuilder sbuild = new StringBuilder();
                            sbuild.Append("<table id =\"tabla_suscriptores\" class=\"display compact\" style =\"width:100%\"><thead><th>id Alumno</th><th>Nombre</th><th>Mail</th></thead><tbody>");

                            if (_list_errors)
                            {
                                /// 4.4.- Pintamos la tabla de errores
                                foreach (EMAIL_SUSCRIPCIONES sus in list_suscriptores)
                                {
                                    if (sus.id_alumno == -1 || String.IsNullOrEmpty(sus.nombre_completo) || String.IsNullOrEmpty(sus.mail) || !Utils.esMailCorrecto(sus.mail))
                                    {
                                        sbuild.Append("<tr>");

                                        if (sus.id_alumno != -1)
                                            sbuild.Append("<td>" + sus.id_alumno + "</td>");
                                        else
                                            sbuild.Append("<td><span class='text-color-red'> Error </span></td>");

                                        if (!String.IsNullOrEmpty(sus.nombre_completo))
                                            sbuild.Append("<td>" + sus.nombre_completo + "</td>");
                                        else
                                            sbuild.Append("<td><span class='text-color-red'> Error </span></td>");

                                        if (!String.IsNullOrEmpty(sus.mail) && Utils.esMailCorrecto(sus.mail))
                                            sbuild.Append("<td>" + sus.mail + "</td>");
                                        else
                                        {
                                            if (!String.IsNullOrEmpty(sus.mail))
                                                sbuild.Append("<td><span class='text-color-red'>" + sus.mail + "</span></td>");
                                            else
                                                sbuild.Append("<td><span class='text-color-red'> Error </span></td>");
                                        }

                                        sbuild.Append("</tr>");
                                        usuarios_errores++;
                                    }
                                    else
                                        usuarios_correctos++;
                                }
                            }
                            else
                            {
                                /// 4.5.- Pintamos la tabla completa
                                foreach (EMAIL_SUSCRIPCIONES sus in list_suscriptores)
                                {
                                    sbuild.Append("<tr>");

                                    sbuild.Append("<td>" + sus.id_alumno + "</td>");
                                    sbuild.Append("<td>" + sus.nombre_completo + "</td>");
                                    sbuild.Append("<td>" + sus.mail + "</td>");

                                    sbuild.Append("</tr>");

                                    usuarios_correctos++;
                                }
                            }

                            sbuild.Append("</tbody></table>");
                            table_suscriptores.InnerHtml = sbuild.ToString();

                            /// 4.6.- Añadimos el nombre del excel al campo oculto y al texto
                            name_file.Value = name_excel;
                            txt_name_file.InnerHtml = name_excel;

                            /// 4.7.- Pintamos el resultado de la carga
                            table_resultados.InnerHtml = "<strong>Total:</strong> " + list_suscriptores.Count + "<br /><strong>Correctos:</strong> " + usuarios_correctos + "<br /><strong>Errores:</strong> " + usuarios_errores;

                            /// 4.8.- Desbloqueamos el bloque
                            block_suscriptores.Attributes["class"] = block_suscriptores.Attributes["class"].Replace("hidden", string.Empty);

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
                            txt_error.InnerHtml = "No se han encontrado datos en el excel";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al subir el fichero";
                }
                else
                    txt_error.InnerHtml = "El fichero subido no es un fichero tipo Excel";
            }
            else
                txt_error.InnerHtml = "Hay que subir un fichero tipo Excel";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Limpiar los errores
            txt_error.InnerHtml = string.Empty;

            /// 2.- Recuperar el id de la lista
            long idList = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"]) : -1;
            if (idList > 0)
            {
                string name_excel = string.Empty;

                /// 1.- Ruta de los ficheros
                string rutaDoc = ConfigurationManager.AppSettings["routeTempSuscriptores"];
                if (!(Directory.Exists(rutaDoc)))
                    Directory.CreateDirectory(rutaDoc);

                /// 2.- Comprobar si carga el fichero sin precargar
                if (fuFile.HasFile)
                {
                    /// 2.1.- Comprobar que ha subido un excel
                    string extension = Path.GetExtension(fuFile.FileName);
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        /// 2.2.- Cargar el fichero excel            
                        if (Directory.Exists(rutaDoc))
                        {
                            FileInfo archivo = new FileInfo(fuFile.PostedFile.FileName);
                            name_excel = "Excel_Suscriptores_" + DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + "_" + Utilities.RemoverSignosAcentos(archivo.Name).Replace(" ", "_");

                            try
                            {
                                fuFile.PostedFile.SaveAs(rutaDoc + name_excel);
                            }
                            catch
                            {
                                name_excel = string.Empty;
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el fichero');</script>");
                            }
                        }
                    }
                    else
                        txt_error.InnerHtml = "El fichero subido no es un fichero tipo Excel";
                }
                else
                    name_excel = name_file.Value;

                /// 3.- Sacar la ruta
                if (!String.IsNullOrEmpty(name_excel))
                {
                    string path = rutaDoc + name_excel;

                    /// 3.0.- Sacar de la base datos las suscriciones de una lista
                    List<EMAIL_SUSCRIPCIONES> list_suscripciones = da.getEmailSubscriptionsById(idList);

                    /// 3.1.- Cargamos los datos del excel
                    List<EMAIL_SUSCRIPCIONES> list_suscriptores = load_excel(path);
                    if (list_suscriptores.Count > 0)
                    {
                        int num_suscriptions = 0;
                        bool insert_correct = true;

                        /// 3.2.- Guardamos los datos en la tabla EMAIL_SUSCRIPCIONES
                        foreach (EMAIL_SUSCRIPCIONES suscripcion in list_suscriptores)
                        {
                            if (!(suscripcion.id_alumno == -1 || String.IsNullOrEmpty(suscripcion.nombre_completo) || String.IsNullOrEmpty(suscripcion.mail) || !Utils.esMailCorrecto(suscripcion.mail)))
                            {
                                List<EMAIL_SUSCRIPCIONES> list_filter = list_suscripciones.Where(sus => sus.nombre_completo == suscripcion.nombre_completo && sus.mail == suscripcion.mail).ToList();
                                if (list_filter.Count == 0)
                                {
                                    suscripcion.id_els = idList;
                                    suscripcion.fecha_alta = DateTime.Today;

                                    long insert_sus = da.insertEmailSubscription(suscripcion);
                                    if (insert_sus > 0)
                                        num_suscriptions++;
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - suscriptores-mantenimiento.cs::insertEmailSubscription()");
                                        LogUtils.InsertarLog("- MSG: Se ha producido un error al generar la suscripción del usuario " + suscripcion.nombre_completo + " (" + suscripcion.id_alumno + ") [" + suscripcion.mail + "]");
                                        insert_correct = false;
                                    }
                                }
                            }
                            else
                            {
                                LogUtils.InsertarLog(" ERROR - suscriptores-mantenimiento.cs:: Falta de datos obligatorios");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al generar la suscripción del usuario " + suscripcion.nombre_completo + " (" + suscripcion.id_alumno + ") [" + suscripcion.mail + "]");
                                insert_correct = false;
                            }
                        }

                        if (!insert_correct)
                            txt_error.InnerHtml = "Se han producido errores al generar las suscripciones de algunos alumnos. Por favor, revise el log";
                        else
                        {
                            File.Delete(path);

                            /// 4.- Actualizar la lista de suscripción con los nuevos datos
                            List<EMAIL_LISTADO_SUSCRIPCIONES> list_suscriptions = da.getEmailListSubscriptionsById(idList);
                            if (list_suscriptions.Count == 1)
                            {
                                EMAIL_LISTADO_SUSCRIPCIONES _suscripcion = list_suscriptions[0];
                                _suscripcion.num_total = (list_suscriptions[0].num_total != null ? list_suscriptions[0].num_total.Value : 0) + num_suscriptions;
                                _suscripcion.num_actual = (list_suscriptions[0].num_actual != null ? list_suscriptions[0].num_actual.Value : 0) + num_suscriptions;
                                _suscripcion.num_bajas = list_suscriptions[0].num_bajas != null ? list_suscriptions[0].num_bajas.Value : 0;
                                if (!String.IsNullOrEmpty(list_suscriptions[0].historico))
                                    _suscripcion.historico = list_suscriptions[0].historico + "<br />Lista actualizada el " + DateTime.Today.ToShortDateString() + " con " + num_suscriptions + " nuevos suscriptores.";
                                else
                                    _suscripcion.historico = "Lista actualizada el " + DateTime.Today.ToShortDateString() + " con " + num_suscriptions + " nuevos suscriptores.";

                                bool update_list = da.updateEmailListSubscription(_suscripcion);
                                if (update_list)
                                    Response.Redirect("suscriptores.aspx?idl=" + idList);
                                else
                                    txt_error.InnerHtml = "Se ha producido un error al actualizar la lista de suscriptores";
                            }
                        }
                    }
                    else
                        txt_error.InnerHtml = "No se han encontrado datos en el excel";
                }
                else
                    txt_error.InnerHtml = "No se ha encontrado el fichero";
            }
            else
                txt_error.InnerHtml = "El id no es correcto";
        }

        private List<EMAIL_SUSCRIPCIONES> load_excel(string path)
        {
            List<EMAIL_SUSCRIPCIONES> list = new List<EMAIL_SUSCRIPCIONES>();

            try
            {
                /// 1.- Cargar el excel
                SLDocument sl = new SLDocument(path);

                /// 2.- Recorrer todas las filas para sacar los datos
                int iRow = 2;
                while (!(string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 1).Trim()) && string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 2).Trim()) && string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 3).Trim())))
                {
                    /// 2.1.- Parametros del excel 
                    string idAlumno = sl.GetCellValueAsString(iRow, 1).Trim();
                    string nombre = sl.GetCellValueAsString(iRow, 2).Trim();
                    string mail = sl.GetCellValueAsString(iRow, 3).Trim();

                    /// 2.2.- Añadimos los datos en la estructura EMAIL_SUSCRIPCIONES
                    EMAIL_SUSCRIPCIONES suscripcion = new EMAIL_SUSCRIPCIONES();
                    suscripcion.id_alumno = !String.IsNullOrEmpty(idAlumno) ? long.Parse(idAlumno) : -1;
                    suscripcion.nombre_completo = nombre;
                    suscripcion.mail = mail;
                    list.Add(suscripcion);
                    iRow++;
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - suscriptores-mantenimiento.cs::load_excel()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            return list;
        }
    }
}