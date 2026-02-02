using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class empresa_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                if (idEmpresa > 0)
                    cargar_datos(idEmpresa);
                else
                    txtFechaAlta.Value = DateTime.Today.ToShortDateString();

                /// 2.- Datos de los fileuploads
                file_convenio.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre el fichero del convenio en el área seleccionada</span><input id='fileupload_conv' type='file' data-url='controls/UploadHandler.ashx' data-form-data='{\"idb\": \"" + idEmpresa + "\", \"type\": \"file_conv\", \"accion\": \"update\" }' />";
                file_logo.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre el logo de la empresa en el área seleccionada</span><input id='fileupload_logo' type='file' data-url='controls/UploadHandler.ashx' data-form-data='{\"idb\": \"" + idEmpresa + "\", \"type\": \"img_logo\", \"accion\": \"update\" }' />";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idEmpresa
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"]) : -1;

            /// 2.- Sacar los datos del formulario
            string razon_social = txt_emp_razon.Value;
            string cif = txt_emp_cif.Value;
            string direccion = txt_emp_direccion.Value;
            string mail = txt_emp_mail.Value;
            string telefono = txt_emp_telefono.Value;
            DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
            DateTime? fecha_baja = null;
            if (!String.IsNullOrEmpty(txtFechaBaja.Value))
                fecha_baja = DateTime.Parse(txtFechaBaja.Value);
            DateTime? fecha_convenio = null;
            if (!String.IsNullOrEmpty(txtFechaConvenio.Value))
                fecha_convenio = DateTime.Parse(txtFechaConvenio.Value);
            string fichero_convenio = txtFicheroConvenio.Value;
            string logo = txtLogo.Value;
            string comentarios = txt_comentarios.Value;

            if (String.IsNullOrEmpty(razon_social) || String.IsNullOrEmpty(cif))
            {
                razon_form.Attributes["class"] = razon_form.Attributes["class"].Replace("has-error", string.Empty);
                if (String.IsNullOrEmpty(razon_social))
                    razon_form.Attributes["class"] = razon_form.Attributes["class"].Insert(razon_form.Attributes["class"].Length, " has-error");

                cif_form.Attributes["class"] = cif_form.Attributes["class"].Replace("has-error", string.Empty);
                if (String.IsNullOrEmpty(cif))
                    cif_form.Attributes["class"] = cif_form.Attributes["class"].Insert(cif_form.Attributes["class"].Length, " has-error");

                txt_error.InnerHtml = "Los campos marcados son obligatorios";
            }
            else
            {
                /// 3.- Modificar o Insertar
                if (idEmpresa > 0)
                {
                    /// 3.1.- Sacar los datos de la empresa
                    List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idEmpresa);
                    if (lst_empresas.Count == 1)
                    {
                        /// 3.2.- Comprobar fichero convenio nuevo
                        if (!String.IsNullOrEmpty(fichero_convenio) && fichero_convenio != lst_empresas[0].FicheroConvenio)
                        {
                            /// 3.2.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["ruta_convenio"];

                            /// 3.2.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(lst_empresas[0].FicheroConvenio))
                                File.Delete(ruta + lst_empresas[0].FicheroConvenio);

                            /// 3.2.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + fichero_convenio;
                            string ruta_destino = ruta;

                            /// 3.2.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + fichero_convenio;

                            /// 3.2.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 3.2.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                            /// 3.2.5.- Borramos el directorio temp
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

                        /// 3.3.- Comprobar logo
                        if (!String.IsNullOrEmpty(logo) && logo != lst_empresas[0].Logo)
                        {
                            /// 3.3.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["ruta_logo_empresa"];

                            /// 3.2.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(lst_empresas[0].Logo))
                                File.Delete(ruta + lst_empresas[0].Logo);

                            /// 3.2.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + logo;
                            string ruta_destino = ruta;

                            /// 3.3.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + logo;

                            /// 3.3.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 3.3.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                            /// 3.3.5.- Borramos el directorio temp
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

                        /// 3.4.- Actualizar los datos de la empresa
                        PRA_EMPRESA empresa = lst_empresas[0];

                        empresa.RazonSocial = razon_social;
                        empresa.Cif = cif;
                        empresa.Direccion = direccion;
                        empresa.Mail = mail;
                        empresa.Telefono = telefono;
                        empresa.FechaAlta = fecha_alta;
                        empresa.FechaBaja = fecha_baja;
                        empresa.FechaConvenio = fecha_convenio;
                        empresa.FicheroConvenio = fichero_convenio;
                        empresa.Logo = logo;
                        empresa.Comentarios = comentarios;

                        bool update_company = da.updateCompany(empresa);
                        if (update_company)
                            Response.Redirect("empresas.aspx");
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar los datos de la empresa";
                    }
                }
                else
                {
                    /// 3.1.- Comprobar fichero convenio nuevo
                    if (!String.IsNullOrEmpty(fichero_convenio))
                    {
                        /// 3.1.1.- Guardar el fichero en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_convenio"];

                        string ruta_origen = ruta + "temp\\" + fichero_convenio;
                        string ruta_destino = ruta;

                        /// 3.1.2.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        ruta_destino = ruta_destino + fichero_convenio;

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

                    /// 3.2.- Comprobar logo
                    if (!String.IsNullOrEmpty(logo))
                    {
                        /// 3.2.1.- Guardar el fichero en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_logo_empresa"];

                        string ruta_origen = ruta + "temp\\" + logo;
                        string ruta_destino = ruta;

                        /// 3.2.2.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        ruta_destino = ruta_destino + logo;

                        /// 3.2.3.- Copiar el fichero
                        File.Copy(ruta_origen, ruta_destino, true);

                        /// 3.2.4.- Borramos el fichero de la carpeta origen
                        File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                        /// 3.2.5.- Borramos el directorio temp
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

                    /// 3.3.- Actualizar los datos de la empresa
                    PRA_EMPRESA empresa = new PRA_EMPRESA();

                    empresa.RazonSocial = razon_social;
                    empresa.Cif = cif;
                    empresa.Direccion = direccion;
                    empresa.Mail = mail;
                    empresa.Telefono = telefono;
                    empresa.FechaAlta = fecha_alta;
                    empresa.FechaBaja = fecha_baja;
                    empresa.FechaConvenio = fecha_convenio;
                    empresa.FicheroConvenio = fichero_convenio;
                    empresa.Logo = logo;
                    empresa.Comentarios = comentarios;
                    empresa.num_contactos = 0;
                    empresa.num_empleos = 0;
                    empresa.num_practicas = 0;
                    empresa.num_solicitudes = 0;

                    long insert_company = da.insertCompany(empresa);
                    if (insert_company > 0)
                        Response.Redirect("empresas.aspx");
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir los datos de la empresa";
                }
            }
        }

        private void cargar_datos(long idEmpresa)
        {
            /// 1.- Obtener los datos de la empresa de la BBDD
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idEmpresa);
            if (lst_empresas.Count == 1)
            {
                txt_emp_razon.Value = lst_empresas[0].RazonSocial;
                txt_emp_cif.Value = lst_empresas[0].Cif;
                txt_emp_direccion.Value = lst_empresas[0].Direccion;
                txt_emp_mail.Value = lst_empresas[0].Mail;
                txt_emp_telefono.Value = lst_empresas[0].Telefono;
                txtFechaAlta.Value = lst_empresas[0].FechaAlta.ToShortDateString();
                txtFechaBaja.Value = lst_empresas[0].FechaBaja != null ? lst_empresas[0].FechaBaja.Value.ToShortDateString() : string.Empty;
                txtFechaConvenio.Value = lst_empresas[0].FechaConvenio != null ? lst_empresas[0].FechaConvenio.Value.ToShortDateString() : string.Empty;
                txtFicheroConvenio.Value = lst_empresas[0].FicheroConvenio;
                txtLogo.Value = lst_empresas[0].Logo;
                txt_comentarios.Value = lst_empresas[0].Comentarios;
            }
        }
    }
}