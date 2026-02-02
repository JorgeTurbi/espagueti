using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class banner_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 0.- Cargar los paises
                cargar_combos();

                /// 1.- Sacar los datos del recurso directo
                long idBanner = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                if (idBanner > 0)
                    cargar_datos(idBanner);
                else
                    txtFecha.Value = DateTime.Today.ToShortDateString();

                /// 2.- Datos de los fileuploads
                file_banner.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre la imagen del banner con tamaño 1500px X 300px en el área seleccionada</span><input id='fileupload_banner' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idb\": \"" + idBanner + "\", \"type\": \"img_banner\", \"accion\": \"update\" }' />";
                file_banner2.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre la imagen del banner con tamaño 3000px X 600px en el área seleccionada</span><input id='fileupload_banner2' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idb\": \"" + idBanner + "\", \"type\": \"img_banner\", \"accion\": \"update\" }' />";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idRecDirecto
            long idBanner = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"]) : -1;
            long idResultado = -1;

            try
            {
                /// 2.- Sacar los datos del formulario
                string _nombre = txt_nombre.Value;
                int _producto = int.Parse(ddlProducto.Value);
                int _orden = int.Parse(txt_orden.Value);
                string _link = txt_link.Value;
                DateTime _fecha = DateTime.Parse(txtFecha.Value);
                DateTime? _fecha_fin = null;
                if (!String.IsNullOrEmpty(txtFechaFin.Value))
                    _fecha_fin = DateTime.Parse(txtFechaFin.Value);
                string _banner = !String.IsNullOrEmpty(txt_banner.Value) ? txt_banner.Value : null;
                string _banner2 = !String.IsNullOrEmpty(txt_banner2.Value) ? txt_banner2.Value : null;
                
                /// 3.- Modificar o Insertar
                if (idBanner > 0)
                {
                    /// 3.0.- Sacar los datos del banner
                    List<EDU_Banners> _banners = da.getEduBanners(idBanner);
                    if (_banners.Count == 1)
                    {
                        bool _update = false;
                        string ruta = ConfigurationManager.AppSettings["ruta_banner"];

                        /// 3.1.- Comprobar banner
                        if (!String.IsNullOrEmpty(_banner) && _banner != _banners[0].Banner)
                        {
                            /// 3.1.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(_banners[0].Banner))
                                File.Delete(ruta + _banners[0].Id_Edu_Producto + "\\" + _banners[0].Banner);

                            /// 3.1.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + _banner;
                            string ruta_destino = ruta + _banners[0].Id_Edu_Producto + "\\";
                            
                            /// 3.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + _banner;

                            /// 3.1.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 3.1.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                            _update = true;
                        }

                        /// 3.1.- Comprobar banner2
                        if (!String.IsNullOrEmpty(_banner2) && _banner2 != _banners[0].Banner_2x)
                        {
                            /// 3.1.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(_banners[0].Banner_2x))
                                File.Delete(ruta + _banners[0].Id_Edu_Producto + "\\" + _banners[0].Banner_2x);

                            /// 3.1.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + _banner2;
                            string ruta_destino = ruta + _banners[0].Id_Edu_Producto + "\\";

                            /// 3.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + _banner2;

                            /// 3.1.3.- Copiar el fichero
                            File.Copy(ruta_origen, ruta_destino, true);

                            /// 3.1.4.- Borramos el fichero de la carpeta origen
                            File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

                            _update = true;
                        }

                        if (_update)
                        {
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

                        /// 3.2.- Actualizar los datos del recurso directo
                        EDU_Banners _banner_up = _banners[0];
                        _banner_up.Nombre = _nombre;
                        _banner_up.Id_Edu_Producto = _producto;
                        _banner_up.Banner = _banner;
                        _banner_up.Banner_2x = _banner2;
                        _banner_up.Orden = _orden;
                        _banner_up.Fecha_Inicio = _fecha;
                        _banner_up.Fecha_Fin = _fecha_fin;
                        _banner_up.Link = _link;

                        bool _update_banner = da.updateEduBanner(_banner_up);
                        if (_update_banner)
                            idResultado = idBanner;
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el banner";
                    }
                }
                else
                {
                    /// 3.1.- Añadir un nuevo banner
                    EDU_Banners _banners = new EDU_Banners();
                    _banners.Nombre = _nombre;
                    _banners.Id_Edu_Producto = _producto;
                    _banners.Banner = _banner;
                    _banners.Banner_2x = _banner2;
                    _banners.Orden = _orden;
                    _banners.Fecha_Inicio = _fecha;
                    _banners.Fecha_Fin = _fecha_fin;
                    _banners.Link = _link;

                    idResultado = da.insertEduBanner(_banners);
                    if (idResultado > 0)
                    {
                        /// 3.2.1.- Guardar la foto en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_banner"];

                        /// 3.2.2.- Rutas nuevas
                        string ruta_origen = ruta + "temp\\";
                        string ruta_destino = ruta + _producto + "\\";

                        /// 3.2.3.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        /// 3.2.4.- Copiar los documentos
                        if (!String.IsNullOrEmpty(_banner))
                        {
                            string _ruta_origen = ruta_origen + _banner;
                            string _ruta_destino = ruta_destino + _banner;

                            /// 3.2.4.1.- Copiar el fichero
                            File.Copy(_ruta_origen, _ruta_destino, true);

                            /// 3.2.4.2.- Borramos el fichero de la carpeta origen
                            File.Delete(_ruta_origen); // Eliminamos el fichero de la carpeta temporal
                        }

                        /// 3.2.4.- Copiar los documentos
                        if (!String.IsNullOrEmpty(_banner2))
                        {
                            string _ruta_origen = ruta_origen + _banner2;
                            string _ruta_destino = ruta_destino + _banner2;

                            /// 3.2.4.1.- Copiar el fichero
                            File.Copy(_ruta_origen, _ruta_destino, true);

                            /// 3.2.4.2.- Borramos el fichero de la carpeta origen
                            File.Delete(_ruta_origen); // Eliminamos el fichero de la carpeta temporal
                        }

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
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir el banner";
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - banner-mantenimiento.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
                Response.Redirect("lista-banners.aspx");
        }

        private void cargar_combos()
        {
            /// 0.- Sacar datos de la BBDD
            List<EDU_Productos> _productos = da.getEduProductos(null);

            /// 1.- Cargar los productos
            if (_productos.Count > 0)
            {
                ddlProducto.DataSource = _productos;
                ddlProducto.DataTextField = "Nombre";
                ddlProducto.DataValueField = "Id_Edu_Producto";
                ddlProducto.DataBind();
                ddlProducto.Items.Add(new ListItem("Seleccione", "-1"));
                ddlProducto.Value = "-1";
            }
        }

        private void cargar_datos(long idBanner)
        {
            /// 1.- Sacar los datos del banner
            List<EDU_Banners> _banner = da.getEduBanners(idBanner);
            if (_banner.Count == 1)
            {
                txt_nombre.Value = _banner[0].Nombre;
                ddlProducto.Value = _banner[0].Id_Edu_Producto.ToString();
                txt_orden.Value = _banner[0].Orden.ToString();
                txt_link.Value = _banner[0].Link;
                txtFecha.Value = _banner[0].Fecha_Inicio.ToShortDateString();
                if (_banner[0].Fecha_Fin.HasValue)
                    txtFechaFin.Value = _banner[0].Fecha_Fin.Value.ToShortDateString();
                txt_banner.Value = _banner[0].Banner;
                txt_banner2.Value = _banner[0].Banner_2x;
                blk_search.Attributes["class"] = blk_search.Attributes["class"].Insert(blk_search.Attributes["class"].Length, " hidden");

                if (!String.IsNullOrEmpty(txt_banner.Value))
                {
                    block_delete_banner.Attributes["class"] = block_delete_banner.Attributes["class"].Replace("hidden", string.Empty);
                    block_see.InnerHtml = "<label class='full-width'>&nbsp;</label><a href='" + ConfigurationManager.AppSettings["url_banner"] + _banner[0].Id_Edu_Producto + "/" + txt_banner.Value + "' target='_blank' title='Ver foto' class='fas fa-eye fa-3x' runat='server'></a>";
                    block_see.Attributes["class"] = block_see.Attributes["class"].Replace("hidden", string.Empty);
                    block_upload_banner.Attributes["class"] = block_upload_banner.Attributes["class"].Insert(block_upload_banner.Attributes["class"].Length, " hidden");
                }

                if (!String.IsNullOrEmpty(txt_banner2.Value))
                {
                    block_delete_banner2.Attributes["class"] = block_delete_banner2.Attributes["class"].Replace("hidden", string.Empty);
                    block_see_banner.InnerHtml = "<label class='full-width'>&nbsp;</label><a href='" + ConfigurationManager.AppSettings["url_banner"] + _banner[0].Id_Edu_Producto + "/" + txt_banner2.Value + "' target='_blank' title='Ver foto' class='fas fa-eye fa-3x' runat='server'></a>";
                    block_see_banner.Attributes["class"] = block_see_banner.Attributes["class"].Replace("hidden", string.Empty);
                    block_upload_banner2.Attributes["class"] = block_upload_banner2.Attributes["class"].Insert(block_upload_banner2.Attributes["class"].Length, " hidden");
                }
            }
        }

        [WebMethod(Description = "Busca el siguiente valor de orden de un producto")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int search_order(long idProducto)
        {
            DataAccess da = new DataAccess();

            /// 1.- Sacar los banners
            List<EDU_Banners> _banners = da.getEduBannersByProducto(idProducto);
            int orden = _banners.Select(_ => _.Orden).Max();

            return orden;
        }

        [WebMethod(Description = "Eliminar un fichero")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool delete_img(long id_banner, string photo, bool first)
        {
            bool _delete = false;
            try
            {
                if (id_banner > 0)
                {
                    DataAccess da = new DataAccess();

                    /// 1.0.- Sacar los datos del banner
                    List<EDU_Banners> _banners = da.getEduBanners(id_banner);
                    if (_banners.Count == 1)
                    {
                        /// 1.1.- Sacar la ruta de la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_banner"] + _banners[0].Id_Edu_Producto + "\\";

                        if (first)
                        {
                            /// 1.2.- Eliminar el fichero
                            if (!String.IsNullOrEmpty(_banners[0].Banner))
                                File.Delete(ruta + _banners[0].Banner);

                            /// 1.3.- Actualizar los datos del usuario                
                            _banners[0].Banner = null;
                        }
                        else
                        {
                            /// 1.2.- Eliminar el fichero
                            if (!String.IsNullOrEmpty(_banners[0].Banner_2x))
                                File.Delete(ruta + _banners[0].Banner_2x);

                            /// 1.3.- Actualizar los datos del usuario                
                            _banners[0].Banner_2x = null;
                        }

                        bool _update = da.updateEduBanner(_banners[0]);
                        if (_update)
                            _delete = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - banner-mantenimiento.cs::delete_img()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _delete = false;
            }
            return _delete;
        }
    }
}