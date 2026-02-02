using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class recurso_directo : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 0.- Cargar los paises
                cargar_combos();

                /// 0.1.- Poner el botón volver
                btn_back.HRef = "lista-recursos-directo.aspx" + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "?lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty);

                /// 1.- Sacar los datos del recurso directo
                long idRecursoDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"].ToString()) : -1;
                if (idRecursoDirecto > 0)
                    cargar_datos(idRecursoDirecto);
                else
                    txt_title.InnerHtml = "<i class='fas fa-photo-video'></i> Mantenimiento recurso directo";

                /// 2.- Datos de los fileuploads
                file_foto.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre la foto del usuario en el área seleccionada</span><input id='fileupload_foto' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idrd\": \"" + idRecursoDirecto + "\", \"type\": \"img_foto\", \"accion\": \"update\" }' />";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idRecDirecto
            long idRecDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            long idResultado = -1;

            try
            {
                /// 2.- Sacar los datos del formulario
                string _titulo = txt_titulo.Value;
                string _tipo = ddlTipo.SelectedValue;
                DateTime _fecha = DateTime.Parse(txtFecha.Value);
                string _descripcion = !String.IsNullOrEmpty(txt_descripcion.Value) ? txt_descripcion.Value : null;
                long _profesor = long.Parse(idProfesor.Value);
                int _area = int.Parse(ddlArea.Value);
                int _tematica = int.Parse(ddlTematica.Value);
                string _foto = !String.IsNullOrEmpty(txt_foto.Value) ? txt_foto.Value : null;
                decimal _val_clase = new decimal(0);
                decimal _val_profesor = new decimal(0);
                if (!String.IsNullOrEmpty(txt_clase.Value))
                    _val_clase = decimal.Parse(txt_clase.Value.Replace('.', ','));
                if (!String.IsNullOrEmpty(txt_val_profesor.Value))
                    _val_profesor = decimal.Parse(txt_val_profesor.Value.Replace('.', ','));
                bool _visible = chkVisible.Checked;
                bool _interno = chkInterno.Checked;
                string _comentarios_internos = !String.IsNullOrEmpty(txt_comentarios.Value) ? txt_comentarios.Value : null;
                string invitacion_directo = !String.IsNullOrEmpty(txt_invitacion.Value) ? txt_invitacion.Value : null;
                string _meta_title = !String.IsNullOrEmpty(txtMetaTitle.Value) ? txtMetaTitle.Value : null;
                string _meta_description = !String.IsNullOrEmpty(txtMetaDescripcion.Value) ? txtMetaDescripcion.Value : null;
                string _meta_keywords = !String.IsNullOrEmpty(txtMetaKeywords.Value) ? txtMetaKeywords.Value : null;
                string _meta_url = !String.IsNullOrEmpty(txtMetaUrl.Value) ? txtMetaUrl.Value : null;
                string _meta_author = !String.IsNullOrEmpty(txtMetaAuthor.Value) ? txtMetaAuthor.Value : null;

                /// 3.- Modificar o Insertar
                if (idRecDirecto > 0)
                {
                    /// 3.0.- Sacar los datos del recurso en directo
                    List<campus_RECURSO_DIRECTO> lst_rec = da.getRecursoDirectoByIdRD(idRecDirecto);
                    if (lst_rec.Count == 1)
                    {
                        /// 3.1.- Comprobar foto
                        if (!String.IsNullOrEmpty(_foto) && _foto != lst_rec[0].foto)
                        {
                            /// 3.1.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["route_rec_directo"];

                            /// 3.1.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(lst_rec[0].foto))
                                File.Delete(ruta + idRecDirecto + "\\" + lst_rec[0].foto);

                            /// 3.1.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + _foto;
                            string ruta_destino = ruta + idRecDirecto + "\\";

                            /// 3.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + _foto;

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

                        /// 3.2.- Actualizar los datos del recurso directo
                        campus_RECURSO_DIRECTO _rec_directo = lst_rec[0];
                        _rec_directo.tipo = _tipo;
                        _rec_directo.titulo = _titulo;
                        _rec_directo.fecha = _fecha;
                        _rec_directo.descripcion = _descripcion;
                        _rec_directo.foto = _foto;
                        _rec_directo.idProfesor = _profesor;
                        _rec_directo.idArea = _area;
                        _rec_directo.idTematica = _tematica;
                        _rec_directo.comentario_interno = _comentarios_internos;
                        _rec_directo.val_clase = _val_clase;
                        _rec_directo.val_profesor = _val_profesor;
                        _rec_directo.visible = _visible;
                        _rec_directo.interno = _interno;
                        _rec_directo.invitacion_directo = invitacion_directo;
                        _rec_directo.Meta_Title = _meta_title;
                        _rec_directo.Meta_Description = _meta_description;
                        _rec_directo.Meta_Keywords = _meta_keywords;
                        _rec_directo.Meta_Url = _meta_url;
                        _rec_directo.Meta_Author = _meta_author;                        

                        bool _update = da.updateRecursoDirecto(_rec_directo);
                        if (_update)
                            idResultado = idRecDirecto;
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el recurso en directo";
                    }
                }
                else
                {
                    /// 3.1.- Insertar un nuevo recurso
                    campus_RECURSO_DIRECTO _rec_directo = new campus_RECURSO_DIRECTO();
                    _rec_directo.tipo = _tipo;
                    _rec_directo.titulo = _titulo;
                    _rec_directo.fecha = _fecha;
                    _rec_directo.descripcion = _descripcion;
                    _rec_directo.foto = _foto;
                    _rec_directo.idProfesor = _profesor;
                    _rec_directo.idArea = _area;
                    _rec_directo.idTematica = _tematica;
                    _rec_directo.comentario_interno = _comentarios_internos;
                    _rec_directo.num_alumnos = 0;
                    _rec_directo.num_valoraciones = 0; 
                    _rec_directo.val_clase = _val_clase;
                    _rec_directo.val_profesor = _val_profesor;
                    _rec_directo.visible = _visible;
                    _rec_directo.interno = _interno;
                    _rec_directo.invitacion_directo = invitacion_directo;
                    _rec_directo.Meta_Title = _meta_title;
                    _rec_directo.Meta_Description = _meta_description;
                    _rec_directo.Meta_Keywords = _meta_keywords;
                    _rec_directo.Meta_Url = _meta_url;
                    _rec_directo.Meta_Author = _meta_author;

                    idResultado = da.insertRecursoDirecto(_rec_directo);
                    if (idResultado > 0)
                    {
                        /// 3.2.1.- Guardar la foto en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["route_rec_directo"];

                        /// 3.2.2.- Rutas nuevas
                        string ruta_origen = ruta + "temp\\" + _foto;
                        string ruta_destino = ruta + idResultado + "\\";

                        /// 3.2.3.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        ruta_destino = ruta_destino + _foto;

                        /// 3.2.4.- Copiar el fichero
                        File.Copy(ruta_origen, ruta_destino, true);

                        /// 3.2.5.- Borramos el fichero de la carpeta origen
                        File.Delete(ruta_origen); // Eliminamos el fichero de la carpeta temporal

                        /// 3.2.6.- Borramos el directorio temp
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
                        txt_error.InnerHtml = "Se ha producido un error al añadir el recurso en directo";
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - recurso-directo.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
                Response.Redirect("lista-tipo-recursos-directo.aspx?idrd=" + idResultado + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
        }
        protected void btnGuardarAll_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idRecDirecto
            long idRecDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            long idResultado = -1;

            try
            {
                /// 2.- Sacar los datos del formulario
                string _titulo = txt_titulo.Value;
                string _tipo = ddlTipo.SelectedValue;
                DateTime _fecha = DateTime.Parse(txtFecha.Value);
                string _descripcion = !String.IsNullOrEmpty(txt_descripcion.Value) ? txt_descripcion.Value : null;
                long _profesor = long.Parse(idProfesor.Value);
                int _area = int.Parse(ddlArea.Value);
                int _tematica = int.Parse(ddlTematica.Value);
                string _foto = !String.IsNullOrEmpty(txt_foto.Value) ? txt_foto.Value : null;
                decimal _val_clase = new decimal(0);
                decimal _val_profesor = new decimal(0);
                if (!String.IsNullOrEmpty(txt_clase.Value))
                    _val_clase = decimal.Parse(txt_clase.Value.Replace('.', ','));
                if (!String.IsNullOrEmpty(txt_val_profesor.Value))
                    _val_profesor = decimal.Parse(txt_val_profesor.Value.Replace('.', ','));
                bool _visible = chkVisible.Checked;
                bool _interno = chkInterno.Checked;
                string _comentarios_internos = !String.IsNullOrEmpty(txt_comentarios.Value) ? txt_comentarios.Value : null;
                string invitacion_directo = !String.IsNullOrEmpty(txt_invitacion.Value) ? txt_invitacion.Value : null;
                string _meta_title = !String.IsNullOrEmpty(txtMetaTitle.Value) ? txtMetaTitle.Value : null;
                string _meta_description = !String.IsNullOrEmpty(txtMetaDescripcion.Value) ? txtMetaDescripcion.Value : null;
                string _meta_keywords = !String.IsNullOrEmpty(txtMetaKeywords.Value) ? txtMetaKeywords.Value : null;
                string _meta_url = !String.IsNullOrEmpty(txtMetaUrl.Value) ? txtMetaUrl.Value : null;
                string _meta_author = !String.IsNullOrEmpty(txtMetaAuthor.Value) ? txtMetaAuthor.Value : null;

                /// 3.- Modificar o Insertar
                if (idRecDirecto > 0)
                {
                    /// 3.0.- Sacar los datos del recurso en directo
                    List<campus_RECURSO_DIRECTO> lst_rec = da.getRecursoDirectoByIdRD(idRecDirecto);
                    if (lst_rec.Count == 1)
                    {
                        /// 3.1.- Comprobar foto
                        if (!String.IsNullOrEmpty(_foto) && _foto != lst_rec[0].foto)
                        {
                            /// 3.1.1.- Guardar el fichero en la carpeta correcta
                            string ruta = ConfigurationManager.AppSettings["route_rec_directo"];

                            /// 3.1.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(lst_rec[0].foto))
                                File.Delete(ruta + idRecDirecto + "\\" + lst_rec[0].foto);

                            /// 3.1.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + _foto;
                            string ruta_destino = ruta + idRecDirecto + "\\";

                            /// 3.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + _foto;

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

                        /// 3.2.- Actualizar los datos del recurso directo
                        campus_RECURSO_DIRECTO _rec_directo = lst_rec[0];
                        _rec_directo.tipo = _tipo;
                        _rec_directo.titulo = _titulo;
                        _rec_directo.fecha = _fecha;
                        _rec_directo.descripcion = _descripcion;
                        _rec_directo.foto = _foto;
                        _rec_directo.idProfesor = _profesor;
                        _rec_directo.idArea = _area;
                        _rec_directo.idTematica = _tematica;
                        _rec_directo.comentario_interno = _comentarios_internos;
                        _rec_directo.val_clase = _val_clase;
                        _rec_directo.val_profesor = _val_profesor;
                        _rec_directo.visible = _visible;
                        _rec_directo.interno = _interno;
                        _rec_directo.invitacion_directo = invitacion_directo;
                        _rec_directo.Meta_Title = _meta_title;
                        _rec_directo.Meta_Description = _meta_description;
                        _rec_directo.Meta_Keywords = _meta_keywords;
                        _rec_directo.Meta_Url = _meta_url;
                        _rec_directo.Meta_Author = _meta_author;

                        bool _update = da.updateRecursoDirecto(_rec_directo);
                        if (_update)
                            idResultado = idRecDirecto;
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el recurso en directo";
                    }
                }
                else
                {
                    /// 3.1.- Insertar un nuevo recurso
                    campus_RECURSO_DIRECTO _rec_directo = new campus_RECURSO_DIRECTO();
                    _rec_directo.tipo = _tipo;
                    _rec_directo.titulo = _titulo;
                    _rec_directo.fecha = _fecha;
                    _rec_directo.descripcion = _descripcion;
                    _rec_directo.foto = _foto;
                    _rec_directo.idProfesor = _profesor;
                    _rec_directo.idArea = _area;
                    _rec_directo.idTematica = _tematica;
                    _rec_directo.comentario_interno = _comentarios_internos;
                    _rec_directo.num_alumnos = 0;
                    _rec_directo.num_valoraciones = 0;
                    _rec_directo.val_clase = _val_clase;
                    _rec_directo.val_profesor = _val_profesor;
                    _rec_directo.visible = _visible;
                    _rec_directo.interno = _interno;
                    _rec_directo.invitacion_directo = invitacion_directo;
                    _rec_directo.Meta_Title = _meta_title;
                    _rec_directo.Meta_Description = _meta_description;
                    _rec_directo.Meta_Keywords = _meta_keywords;
                    _rec_directo.Meta_Url = _meta_url;
                    _rec_directo.Meta_Author = _meta_author;

                    idResultado = da.insertRecursoDirecto(_rec_directo);
                    if (idResultado > 0)
                    {
                        /// 3.2.1.- Guardar la foto en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["route_rec_directo"];

                        /// 3.2.2.- Rutas nuevas
                        string ruta_origen = ruta + "temp\\" + _foto;
                        string ruta_destino = ruta + idResultado + "\\";

                        /// 3.2.3.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        ruta_destino = ruta_destino + _foto;

                        /// 3.2.4.- Copiar el fichero
                        File.Copy(ruta_origen, ruta_destino, true);

                        /// 3.2.5.- Borramos el fichero de la carpeta origen
                        File.Delete(ruta_origen); // Eliminamos el fichero de la carpeta temporal

                        /// 3.2.6.- Borramos el directorio temp
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
                        txt_error.InnerHtml = "Se ha producido un error al añadir el recurso en directo";
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - recurso-directo.cs::btnGuardarAll_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
                Response.Redirect("recurso-directo.aspx?idrd=" + idResultado + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
        }
        protected void btn_delete_tag_Click(object sender, EventArgs e)
        {
            bool update_tag = false;
            long idRecursoDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"].ToString()) : -1;

            try
            {
                if (!String.IsNullOrEmpty(hid_tag.Value))
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_RECURSO_DIRECTO> _recurso = da.getRecursoDirectoByIdRD(idRecursoDirecto);
                    if (_recurso.Count == 1)
                    {
                        /// 2.- Sacar los tags
                        List<string> _tags = new List<string>();
                        if (!String.IsNullOrEmpty(_recurso[0].tags))
                            _tags = _recurso[0].tags.Split(';').ToList();

                        /// 3.- Eliminar los tags
                        _tags = _tags.Where(_ => !_.Equals(hid_tag.Value)).ToList();

                        /// 4.- Actualizar los tags
                        string _tags_recurso = null;
                        int _index = 0;
                        foreach (var _tag in _tags)
                        {
                            if (_index == 0)
                                _tags_recurso = _tag.Trim();
                            else
                                _tags_recurso = _tags_recurso + ";" + _tag.Trim();
                            _index++;
                        }

                        campus_RECURSO_DIRECTO _recurso_directo = _recurso[0];
                        _recurso_directo.tags = _tags_recurso;

                        update_tag = da.updateRecursoDirecto(_recurso_directo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - recurso-directo.cs::btn_delete_tag_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (update_tag)
                Response.Redirect("recurso-directo.aspx?idrd=" + idRecursoDirecto);
            else
                txt_error.InnerHtml = "Se ha producido un error al eliminar el tag";
        }

        [WebMethod(Description = "Busca profesores a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> search_teacher(string name)
        {
            DataAccess da = new DataAccess();

            /// 1.- Sacar los usuarios
            List<Usuarios> _teachers = new List<Usuarios>();
            List<CLIENTES> _users = da.getUserBySearch(name, null);

            /// 2.- Filtrar los profesores
            _users = _users.Where(_ => !String.IsNullOrEmpty(_.Profesor) && _.Profesor == ((int)Constantes.activo.Activo).ToString()).ToList();

            /// 3.- Sacar los profesores
            if (_users.Count > 0)
                _teachers = _users.Select(user => new Usuarios { id_usuario = user.id_cliente, nombre_completo = user.Nombre_Completo }).ToList();
            return _teachers;
        }

        [WebMethod(Description = "Eliminar un fichero")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool delete_photo(long id_rec_directo, string photo)
        {
            bool _delete = false;
            try
            {
                if (id_rec_directo > 0)
                {
                    DataAccess da = new DataAccess();

                    /// 1.0.- Sacar los datos del recurso
                    List<campus_RECURSO_DIRECTO> _recurso = da.getRecursoDirectoByIdRD(id_rec_directo);
                    if (_recurso.Count == 1)
                    {
                        /// 1.1.- Sacar la ruta de la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["route_rec_directo"] + id_rec_directo + "\\";

                        /// 1.2.- Eliminar el fichero
                        if (!String.IsNullOrEmpty(_recurso[0].foto))
                            File.Delete(ruta + _recurso[0].foto);

                        /// 1.3.- Actualizar los datos del usuario                
                        _recurso[0].foto = null;

                        bool _update = da.updateRecursoDirecto(_recurso[0]);
                        if (_update)
                            _delete = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - recurso-directo.cs::delete_photo()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _delete = false;
            }
            return _delete;
        }

        [WebMethod(Description = "Comprobar si existe el meta url")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool comprobar_meta_url(string meta_url, string idRecursoDirecto)
        {
            bool url_exists = true;

            try
            {
                DataAccess da = new DataAccess();

                /// 1.- Sacar los recursos en directo
                List<campus_RECURSO_DIRECTO> _recursos = da.getRecursoDirectoByIdRD(-1);

                /// 2.- Comprobar si el meta_url existe
                if (!String.IsNullOrEmpty(idRecursoDirecto))
                    _recursos = _recursos.Where(_ => _.Meta_Url == meta_url && _.id_RecursoDirecto != long.Parse(idRecursoDirecto)).ToList();
                else
                    _recursos = _recursos.Where(_ => _.Meta_Url == meta_url).ToList();

                if (_recursos.Count > 0)
                    url_exists = true;
                else
                    url_exists = false;
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - recurso-directo.cs::comprobar_meta_url()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                url_exists = true;
            }

            return url_exists;
        }

        private void cargar_combos()
        {
            /// 0.- Sacar datos de la BBDD
            List<sbs2_area_funcional> _areas = da.getAreaFuncionalByIdArea(-1);
            List<sbs2_tematica> _tematicas = da.getTematicaByIdTematica(-1);

            /// 1.- Cargar Áreas
            if (_areas.Count > 0)
            {
                ddlArea.DataSource = _areas;
                ddlArea.DataTextField = "nombre";
                ddlArea.DataValueField = "idArea";
                ddlArea.DataBind();
                ddlArea.Items.Add(new ListItem("Seleccione", "-1"));
                ddlArea.Value = "-1";
            }

            /// 2.- Cargar temáticas
            if (_tematicas.Count > 0)
            {
                ddlTematica.DataSource = _tematicas;
                ddlTematica.DataTextField = "nombre";
                ddlTematica.DataValueField = "idTematica";
                ddlTematica.DataBind();
                ddlTematica.Items.Add(new ListItem("Seleccione", "-1"));
                ddlTematica.Value = "-1";
            }
        }

        private void cargar_datos(long idRecursoDirecto)
        {
            /// 1.- Sacar los datos del recurso en directo
            List<campus_RECURSO_DIRECTO> _recurso = da.getRecursoDirectoByIdRD(idRecursoDirecto);
            if (_recurso.Count == 1)
            {
                /// 1.1.- Desbloquear el bloque de docencias
                block_Docencias.Attributes["class"] = block_Docencias.Attributes["class"].Replace("hidden", string.Empty);

                txt_titulo.Value = _recurso[0].titulo;
                ddlTipo.SelectedValue = _recurso[0].tipo;
                txtFecha.Value = _recurso[0].fecha.ToShortDateString();
                txt_descripcion.Value = _recurso[0].descripcion;

                List<CLIENTES> _user = da.getUserById(_recurso[0].idProfesor);
                if (_user.Count == 1)
                    txt_profesor.Value = _user[0].Nombre_Completo;
                else
                    txt_profesor.Value = _recurso[0].idProfesor.ToString();
                idProfesor.Value = _recurso[0].idProfesor.ToString();

                ddlArea.Value = _recurso[0].idArea.ToString();
                ddlTematica.Value = _recurso[0].idTematica.ToString();
                txt_clase.Value = _recurso[0].val_clase.ToString();
                txt_val_profesor.Value = _recurso[0].val_profesor.ToString();
                chkVisible.Checked = _recurso[0].visible;
                chkInterno.Checked = _recurso[0].interno;
                txt_comentarios.Value = _recurso[0].comentario_interno;
                txt_foto.Value = _recurso[0].foto;
                txt_invitacion.Value = _recurso[0].invitacion_directo;

                txtMetaTitle.Value = _recurso[0].Meta_Title;
                txtMetaDescripcion.Value = _recurso[0].Meta_Description;
                txtMetaKeywords.Value = _recurso[0].Meta_Keywords;
                txtMetaUrl.Value = _recurso[0].Meta_Url;
                txtMetaAuthor.Value = _recurso[0].Meta_Author;

                if (!String.IsNullOrEmpty(txt_foto.Value))
                {
                    block_delete_photo.Attributes["class"] = block_delete_photo.Attributes["class"].Replace("hidden", string.Empty);
                    block_see.InnerHtml = "<label class='full-width'>&nbsp;</label><a id='lnk_photo' href='" + ConfigurationManager.AppSettings["url_rec_directo"] + idRecursoDirecto + "/" + txt_foto.Value + "' target='_blank' title='Ver foto' class='fas fa-eye fa-3x' runat='server'></a>";
                    block_see.Attributes["class"] = block_see.Attributes["class"].Replace("hidden", string.Empty);
                    block_upload_photo.Attributes["class"] = block_upload_photo.Attributes["class"].Insert(block_upload_photo.Attributes["class"].Length, " hidden");
                }

                /// 2.- Cargar las valoraciones de la clase
                List<campus_RD_Valoracion> _valoraciones = da.getRecDirectosValoracion(idRecursoDirecto);
                if (_valoraciones.Count > 0)
                    table_listado_valoraciones.InnerHtml = paint_table(_valoraciones);

                /// 3.- Pintar los tags
                blk_tags.InnerHtml = paint_tags(_recurso[0].tags);

                /// 4.- Pintar el título
                List<campus_RECURSO_DIRECTO> _recursos = da.getRecursoDirectoByIdRD(-1);
                txt_title.InnerHtml = "<i class='fas fa-photo-video'></i> Mantenimiento recurso directo";

                /// 4.0.- Poner el botón de ver recursos
                txt_title.InnerHtml += "<a id='btn_save_rec' onclick='validarFormulario()' title='Ir a recursos' class='pull-right bold padding-lr-5 c-pointer'><i class='far fa-eye fa-1-6x'></i></a>";
                
                /// 4.1.- Poner el botón de guardar
                txt_title.InnerHtml += "<a id='btn_save_all' onclick='save_data()' title='Guardar' class='pull-right bold padding-lr-5 c-pointer'><i class='far fa-save fa-1-6x'></i></a>";

                /// 4.2.- Sacar el siguiente
                var _next = _recursos.Where(_ => _.id_RecursoDirecto > idRecursoDirecto).FirstOrDefault();
                if (_next != null)
                    txt_title.InnerHtml += "<a href='recurso-directo.aspx?idrd=" + _next.id_RecursoDirecto + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty) + "' title='Siguiente recurso directo' class='pull-right bold padding-lr-5'><i class='fas fa-caret-right fa-1-6x'></i></a>";

                /// 4.3.- Sacar el anterior
                var _previous = _recursos.Where(_ => _.id_RecursoDirecto < idRecursoDirecto).OrderByDescending(_ => _.id_RecursoDirecto).FirstOrDefault();
                if(_previous != null)
                    txt_title.InnerHtml += "<a href='recurso-directo.aspx?idrd=" + _previous.id_RecursoDirecto + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty) + "' title='Anterior recurso directo' class='pull-right bold padding-lr-5'><i class='fas fa-caret-left fa-1-6x'></i></a>";

                /// 5. Pintar el bloque de docencias
                cargar_docencias_activas(idRecursoDirecto);
            }
        }

        //private void cargar_docencias_activas(string docencias)
        //{
        //    /// 1.- Sacar las docencias
        //    List<campus_DOCENCIA> list_docencias = da.getDocenciaById(-1);
        //    List<campus_DOCENCIA> list_docencias_actives = list_docencias.Where(d => (d.FInicio == null || d.FInicio <= DateTime.Today) && (d.FFin == null || d.FFin >= DateTime.Today)).OrderBy(d => d.Nombre).ToList();
        //    if (list_docencias_actives.Count > 0)
        //    {
        //        List<long> lst_id_docs = new List<long>();
        //        if (!String.IsNullOrEmpty(docencias))
        //        {
        //            List<string> lst_ids = docencias.Split(',').ToList();
        //            if (lst_ids.Count > 0)
        //            {
        //                foreach (var docencia in lst_ids)
        //                {
        //                    if (!String.IsNullOrEmpty(docencia) && !lst_id_docs.Contains(long.Parse(docencia)))
        //                        lst_id_docs.Add(long.Parse(docencia));
        //                }
        //            }
        //        }

        //        /// 2.- Pintar la tabla de docencias
        //        List<campus_DOCENCIA> list_docs = list_docencias_actives.Where(_ => !lst_id_docs.Contains(_.ID_Docencia)).ToList();
        //        tabla_all_doc.InnerHtml = paint_table_docs(list_docs, "tabla_doc_all", true);

        //        /// 3.- Pintar la tabla de docencias seleccionadas
        //        List<campus_DOCENCIA> list_docs_sel = list_docencias_actives.Where(_ => lst_id_docs.Contains(_.ID_Docencia)).ToList();
        //        tabla_sel_doc.InnerHtml = paint_table_docs(list_docs_sel, "table_doc", false);

        //        /// 4.- Ocultar los botones de deseleccionar si no hay origenes seleccionados
        //        if (list_docs.Count == 0)
        //        {
        //            btn_sel_doc.Visible = false;
        //            btn_sel_doc_all.Visible = false;
        //            btn_desel_doc.Visible = true;
        //            btn_desel_status_all.Visible = true;
        //        }
        //        else if (list_docs_sel.Count == 0)
        //        {
        //            btn_sel_doc.Visible = true;
        //            btn_sel_doc_all.Visible = true;
        //            btn_desel_doc.Visible = false;
        //            btn_desel_status_all.Visible = false;
        //        }
        //        else
        //        {
        //            btn_sel_doc.Visible = true;
        //            btn_sel_doc_all.Visible = true;
        //            btn_desel_doc.Visible = true;
        //            btn_desel_status_all.Visible = true;
        //        }

        //        /// 5.- Limpiar los campos
        //        hidDocs.Value = string.Empty;
        //        hidDocsSel.Value = string.Empty;
        //    }
        //}

        private void cargar_docencias_activas(long id_rd)
        {
            /// 1.- Sacar las docencias
            List<campus_DOCENCIA> list_docencias = da.getDocenciaById(-1);
            //List<campus_DOCENCIA> list_docencias_actives = list_docencias.Where(d => (d.FInicio == null || d.FInicio <= DateTime.Today) && (d.FFin == null || d.FFin >= DateTime.Today)).OrderBy(d => d.Nombre).ToList();
            List<campus_DOCENCIA> list_docencias_actives = list_docencias.Where(d => (d.FFin == null || d.FFin >= DateTime.Today)).OrderBy(d => d.Nombre).ToList();
            List<campus_RD_Doc> lst_doc_rec_dir = da.getDocenciasByRecDirecto(id_rd);
            if (list_docencias_actives.Count > 0)
            {
                List<long> lst_id_docs = new List<long>();
                foreach (var docencia in lst_doc_rec_dir)
                {
                    if (!lst_id_docs.Contains(docencia.ID_Docencia))
                        lst_id_docs.Add(docencia.ID_Docencia);
                }            

                /// 2.- Pintar la tabla de docencias
                List<campus_DOCENCIA> list_docs = list_docencias_actives.Where(_ => !lst_id_docs.Contains(_.ID_Docencia)).ToList();
                tabla_all_doc.InnerHtml = paint_table_docs(list_docs, "tabla_doc_all", true);

                /// 3.- Pintar la tabla de docencias seleccionadas
                List<campus_DOCENCIA> list_docs_sel = list_docencias_actives.Where(_ => lst_id_docs.Contains(_.ID_Docencia)).ToList();
                tabla_sel_doc.InnerHtml = paint_table_docs(list_docs_sel, "table_doc", false);

                /// 4.- Ocultar los botones de deseleccionar si no hay origenes seleccionados
                if (list_docs.Count == 0)
                {
                    btn_sel_doc.Visible = false;
                    btn_sel_doc_all.Visible = false;
                    btn_desel_doc.Visible = true;
                    btn_desel_status_all.Visible = true;
                }
                else if (list_docs_sel.Count == 0)
                {
                    btn_sel_doc.Visible = true;
                    btn_sel_doc_all.Visible = true;
                    btn_desel_doc.Visible = false;
                    btn_desel_status_all.Visible = false;
                }
                else
                {
                    btn_sel_doc.Visible = true;
                    btn_sel_doc_all.Visible = true;
                    btn_desel_doc.Visible = true;
                    btn_desel_status_all.Visible = true;
                }

                /// 5.- Limpiar los campos
                hidDocs.Value = string.Empty;
                hidDocsSel.Value = string.Empty;
            }
        }

        private string paint_table(List<campus_RD_Valoracion> _valoraciones)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Sacar los alumnos
            List<long> _ids = _valoraciones.Select(_ => _.idPersona).Distinct().ToList();
            List<CLIENTES> _users = da.getUserByList(_ids);

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Valoraciones\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Val. Clase</th>");
            sbuild.Append("<th>Val.Profesor</th>");
            sbuild.Append("<th>Comentario</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las valoraciones
            foreach (var _valoracion in _valoraciones)
            {
                sbuild.Append("<tr>");
                sbuild.Append($"<td>{_valoracion.fecha.ToShortDateString()}</td>");
                sbuild.Append($"<td>{_users.Where(_ => _.id_cliente == _valoracion.idPersona).Select(_ => _.Nombre_Completo).FirstOrDefault()}</td>");
                sbuild.Append($"<td>{_valoracion.val_clase}</td>");
                sbuild.Append($"<td>{_valoracion.val_profesor}</td>");
                sbuild.Append($"<td>{_valoracion.comentario}</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }

        private string paint_table_docs(List<campus_DOCENCIA> list_docs, string table, bool all)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id=\"" + table + "\" class=\"display compact\" style =\"width:100%\">");
            sbuild.Append("<thead>");
            //Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            if (all)
                sbuild.Append("<th>Docencia</th>");
            else
                sbuild.Append("<th>Docencia Seleccionada</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var docencia in list_docs)
            {
                sbuild.Append("<tr>");
                if (all)
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + docencia.ID_Docencia + "\" onclick=\"chk_mark_doc(this,1)\"></td>");
                else
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + docencia.ID_Docencia + "\" onclick=\"chk_mark_doc(this,0)\"></td>");
                sbuild.Append("<td>" + docencia.Nombre + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody>");
            sbuild.Append("</table>");

            return sbuild.ToString();
        }

        #region Métodos de los tags

        [WebMethod(Description = "Añade un tag al usuario")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> add_tag_user(long idRecursoDirecto, string tag)
        {
            DataAccess da = new DataAccess();

            /// 1.- Inicializar el objeto
            List<string> _lst_tags = new List<string>();

            /// 2.- Sacar los tags
            List<campus_RECURSO_DIRECTO> _recurso = da.getRecursoDirectoByIdRD(idRecursoDirecto);
            if (_recurso.Count == 1)
            {
                /// 2.1.- Sacar los tags
                List<string> _tags = new List<string>();
                if (!String.IsNullOrEmpty(_recurso[0].tags))
                    _tags = _recurso[0].tags.Split(';').ToList();

                /// 2.2.- Añadir el tag
                if (!_tags.Contains(tag))
                    _tags.Add(tag);

                /// 2.3.- Actualizar los tags
                string _tags_recurso = null;
                int _index = 0;
                foreach (var _tag in _tags)
                {
                    if (_index == 0)
                        _tags_recurso = _tag;
                    else
                        _tags_recurso = _tags_recurso + ";" + _tag;
                    _index++;
                }

                campus_RECURSO_DIRECTO _recurso_directo = _recurso[0];
                _recurso_directo.tags = _tags_recurso;

                bool _update_recurso = da.updateRecursoDirecto(_recurso_directo);
                if (_update_recurso)
                    _lst_tags.Add(paint_tags(_tags_recurso));
            }

            return _lst_tags;
        }

        private static string paint_tags(string tags)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Recorrer los tags
            if (!String.IsNullOrEmpty(tags))
            {
                /// 1.1.- Pintar el título del bloque de origenes
                sbuild.Append("<label class='w-100'>Tags <span class='d-inline-block align-middle px-2'><input type='text' id='tag_user' class='form-control form-control-sm' runat='server' /></span><span class='d-inline-block align-middle px-2'><a id='lnk_add_tag' href='javascript:void(0);' class='fas fa-check text-color-green fa-1-4x' onclick='add_tag()'></a></span></label>");

                /// 1.2.- Recorrer los tags
                foreach (var _tag in tags.Split(';'))
                {
                    sbuild.Append("<a href='javascript:void(0)' onclick='if (confirm_message(\"¿Desea eliminar el tag: " + _tag + "?\")){eliminar_tag(\"" + _tag + "\")}' class='badge badge-primary'>" + _tag + " <i class='fas fa-times-circle text-color-red align-middle fa-1-6x'></i></a> ");
                }
            }
            else
                sbuild.Append("<label class='w-100'>Tags <span class='d-inline-block align-middle px-2'><input type='text' id='tag_user' class='form-control form-control-sm' runat='server' /></span><span class='d-inline-block align-middle px-2'><a id='lnk_add_tag' href='javascript:void(0);' class='fas fa-check text-color-green fa-1-4x' onclick='add_tag()'></a></span></label>");

            return sbuild.ToString();
        }

        [WebMethod(Description = "Añade un tag al usuario")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> search_tag(long idRecursoDirecto)
        {
            DataAccess da = new DataAccess();

            /// 1.- Inicializar el objeto
            List<string> _lst_tags = new List<string>();

            /// 2.- Sacar los tags
            List<campus_RECURSO_DIRECTO> _recurso = da.getRecursoDirectoByIdRD(idRecursoDirecto);
            if (_recurso.Count == 1)
            {
                /// 2.1.- Sacar los tags
                List<string> _tags = new List<string>();
                if (!String.IsNullOrEmpty(_recurso[0].tags))
                    _tags = _recurso[0].tags.Split(';').ToList();

                /// 2.2.- Pintar los tags
                string _tags_recurso = null;
                int _index = 0;
                foreach (var _tag in _tags)
                {
                    if (_index == 0)
                        _tags_recurso = _tag.Trim();
                    else
                        _tags_recurso = _tags_recurso + ", " + _tag.Trim();
                    _index++;
                }

                _lst_tags.Add(_tags_recurso);
            }

            return _lst_tags;
        }

        #endregion

        #region Docencias

        protected void btn_sel_doc_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            //card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            //card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            //collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rd = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            if (id_rd > 0)
            {
                bool _error = false;

                /// 2.- Sacar todas las docencias
                List<campus_DOCENCIA> list_docencias = da.getDocenciaById(-1);
                List<campus_DOCENCIA> list_docencias_actives = list_docencias.Where(d => (d.FInicio == null || d.FInicio <= DateTime.Today) && (d.FFin == null || d.FFin >= DateTime.Today)).OrderBy(d => d.Nombre).ToList();
                if (list_docencias_actives.Count > 0)
                {
                    foreach (var _docencia in list_docencias_actives)
                    {
                        /// 3.3.4.- Añadir
                        campus_RD_Doc _rd = new campus_RD_Doc();
                        _rd.id_RecursoDirecto = id_rd;
                        _rd.ID_Docencia = _docencia.ID_Docencia;

                        long insert_rd = da.insertRD_Doc(_rd);
                        if (insert_rd < 1)
                            _error = true;
                    }
                }

                if (!_error)
                    cargar_docencias_activas(id_rd);
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir las docencias al recurso en directo";                
            }
        }

        protected void btn_sel_doc_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            //card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            //card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            //collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rd = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            if (id_rd > 0)
            {
                /// 2.- Sacar las docencias seleccionadas
                string _docencias = hidDocs.Value;
                if (!String.IsNullOrEmpty(_docencias))
                {
                    List<long> lst_id_docs = new List<long>();
                    foreach (var docencia in _docencias.Split(',').ToList())
                    {
                        if (!String.IsNullOrEmpty(docencia) && !lst_id_docs.Contains(long.Parse(docencia)))
                            lst_id_docs.Add(long.Parse(docencia));
                    }

                    if (lst_id_docs.Count > 0)
                    {
                        bool _error = false;

                        /// 3.- Sacar las docencias seleccionadas
                        foreach (var _docencia in lst_id_docs)
                        {
                            /// 4.- Añadir
                            campus_RD_Doc _rd = new campus_RD_Doc();
                            _rd.id_RecursoDirecto = id_rd;
                            _rd.ID_Docencia = _docencia;

                            long insert_rd = da.insertRD_Doc(_rd);
                            if (insert_rd < 1)
                                _error = true;
                        }

                        if (!_error)
                            cargar_docencias_activas(id_rd);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al añadir las docencias seleccionadas al recurso en directo";
                    }
                }
            }
        }

        protected void btn_desel_doc_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            //card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            //card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            //collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rd = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            if (id_rd > 0)
            {
                /// 2.- Sacar las docencias seleccionadas
                string _docencias = hidDocsSel.Value;
                if (!String.IsNullOrEmpty(_docencias))
                {
                    List<long> lst_id_docs = new List<long>();
                    foreach (var docencia in _docencias.Split(',').ToList())
                    {
                        if (!String.IsNullOrEmpty(docencia) && !lst_id_docs.Contains(long.Parse(docencia)))
                            lst_id_docs.Add(long.Parse(docencia));
                    }

                    if (lst_id_docs.Count > 0)
                    {
                        bool _error = false;

                        /// 3.- Sacar las docencias seleccionadas
                        foreach (var _docencia in lst_id_docs)
                        {
                            bool _delete_rd = da.deleteRD_Doc(_docencia, id_rd);
                            if (!_delete_rd)
                                _error = true;
                        }

                        if (!_error)
                            cargar_docencias_activas(id_rd);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al añadir las docencias seleccionadas al recurso en directo";
                    }
                }
            }
        }

        protected void btn_desel_doc_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            //card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            //card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            //collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rd = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            if (id_rd > 0)
            {
                bool _error = false;

                /// 2.- Sacar todas las docencias
                List<campus_RD_Doc> lst_doc_rec_dir = da.getDocenciasByRecDirecto(id_rd);
                if (lst_doc_rec_dir.Count > 0)
                {
                    List<long> lst_id_docs = new List<long>();
                    foreach (var docencia in lst_doc_rec_dir)
                    {
                        if (!lst_id_docs.Contains(docencia.ID_Docencia))
                            lst_id_docs.Add(docencia.ID_Docencia);
                    }

                    List<campus_DOCENCIA> list_docencias = da.getDocenciaById(-1);
                    List<campus_DOCENCIA> list_docs_sel = list_docencias.Where(_ => lst_id_docs.Contains(_.ID_Docencia)).ToList();

                    foreach (var _docencia in list_docs_sel)
                    {
                        /// 3.3.4.- Añadir
                        bool _delete_rd = da.deleteRD_Doc(_docencia.ID_Docencia, id_rd);
                        if (!_delete_rd)
                            _error = true;
                    }
                }

                if (!_error)
                    cargar_docencias_activas(id_rd);
                else
                    txt_error.InnerHtml = "Se ha producido un error al eliminar las docencias al recurso directo";
            }
        }

        #endregion
    }
} 