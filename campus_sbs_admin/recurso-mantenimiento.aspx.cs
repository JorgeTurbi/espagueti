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
    public partial class recurso_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 0.- Cargar los paises
                cargar_combos();

                /// 1.- Sacar los datos del recurso directo
                long idRecursoDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"].ToString()) : -1;
                long idRecurso = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"].ToString()) : -1;
                if (idRecurso > 0)
                    cargar_datos(idRecurso);
                else
                {
                    chkActivo.Checked = true;
                    resource_version.Value = "1";
                    resource_derechos.Value = "SBS";
                }

                /// 2.- Datos de los fileuploads
                file_rec_int.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre el recurso interno en el área seleccionada</span><input id='fileupload_rec_int' type='file' data-url='/controls/UploadHandler.ashx' data-form-data='{\"idrd\": \"" + idRecursoDirecto + "\", \"type\": \"file_rec_int\", \"accion\": \"update\" }' />";

                /// 3.- Botón de volver
                btn_back.HRef = "lista-tipo-recursos-directo.aspx?idrd=" + idRecursoDirecto + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty);
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idRecDirecto
            long idRecDirecto = !String.IsNullOrEmpty(Request.QueryString["idrd"]) ? long.Parse(Request.QueryString["idrd"]) : -1;
            long idRecurso = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long idResultado = -1;

            try
            {
                /// 2.- Sacar los datos del formulario
                string _titulo = txt_titulo.Value;
                int _tipo = int.Parse(ddlTipo.SelectedValue);
                int _area = int.Parse(ddlArea.Value);
                bool _activo = chkActivo.Checked;
                int _version = int.Parse(resource_version.Value);
                string _derechos = resource_derechos.Value;
                string _recurso_int = !String.IsNullOrEmpty(txt_rec_int.Value) ? txt_rec_int.Value : null;
                string _recurso_ext = !String.IsNullOrEmpty(txt_rec_ext.Value) ? txt_rec_ext.Value : null;

                /// 3.- Modificar o Insertar
                if (idRecurso > 0)
                {
                    /// 3.0.- Sacar los datos del recurso en directo
                    List<campus_RECURSO> _recurso = da.getResourcesById(idRecurso);
                    if (_recurso.Count == 1)
                    {
                        /// 3.1.- Comprobar foto
                        if ((!String.IsNullOrEmpty(_recurso[0].Rec_Interno) && _recurso_int != _recurso[0].Rec_Interno) || (String.IsNullOrEmpty(_recurso[0].Rec_Interno) && !String.IsNullOrEmpty(_recurso_int)))
                        {
                            /// 3.1.1.- Guardar el fichero en la carpeta correcta
                            string ruta = string.Empty;
                            if (_recurso[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Multimedia)
                                ruta = ConfigurationManager.AppSettings["route_multimedia"];
                            else
                                ruta = ConfigurationManager.AppSettings["route_nota_tecnica"];

                            /// 3.1.1.0.- Eliminar el fichero anterior
                            if (!String.IsNullOrEmpty(_recurso[0].Rec_Interno))
                                File.Delete(ruta + _recurso[0].Rec_Interno);

                            /// 3.1.1.1.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + _recurso_int;
                            string ruta_destino = ruta;

                            /// 3.1.2.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + _recurso_int;

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

                        /// 3.2.- Actualizar los datos del recurso
                        campus_RECURSO _rec_directo = _recurso[0];
                        _rec_directo.Titulo = _titulo;
                        _rec_directo.ID_Tipo_Recurso = _tipo;
                        _rec_directo.ID_Area_Funcional = _area;
                        _rec_directo.Activo = (_activo ? ((int)Constantes.activo.Activo).ToString() : ((int)Constantes.activo.NoActivo).ToString());
                        _rec_directo.Version = _version;
                        _rec_directo.Derechos = _derechos;
                        _rec_directo.Rec_Interno = _recurso_int;
                        _rec_directo.Rec_Externo = _recurso_ext;

                        bool _update = da.updateRecurso(_rec_directo);
                        if (_update)
                        {
                            idResultado = idRecurso;
                            /*
                            /// 3.3.1.- Comprobar si hay que añadir la entrada a campus_RD_R
                            List<campus_RD_R> _recursos_rd = da.getRecDirectosByRecurso(idRecDirecto);

                            /// 3.3.2.- Filtrar por el recurso
                            _recursos_rd = _recursos_rd.Where(_ => _.ID_Recurso == idRecurso).ToList();
                            if (_recursos_rd.Count == 1 && !_activo)
                            {
                                /// 3.3.3.- Eliminar
                                bool _delete_rd = da.deleteRD_R(_recursos_rd[0].id_RecursoDirecto);
                                if (!_delete_rd)
                                    txt_error.InnerHtml = "Se ha producido un error al eliminar el recurso directo " + _recursos_rd[0].id_RecursoDirecto + " de campus_RD_R";
                            }
                            else if (_recursos_rd.Count == 0 && _activo)
                            {
                                /// 3.3.4.- Añadir
                                campus_RD_R _rd = new campus_RD_R();
                                _rd.id_RecursoDirecto = idRecDirecto;
                                _rd.ID_Recurso = idResultado;

                                long insert_rd = da.insertRD_R(_rd);
                                if (insert_rd < 1)
                                    txt_error.InnerHtml = "Se ha producido un error al añadir el recurso en directo";
                            }*/
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el recurso";
                    }
                }
                else
                {
                    /// 3.0.- Sacar el Código de los recursos
                    string _cod_recurso = string.Empty;
                    List<campus_RECURSO_DIRECTO> lst_rec = da.getRecursoDirectoByIdRD(idRecDirecto);
                    if (lst_rec.Count == 1)
                    {
                        string _type = lst_rec[0].tipo;

                        List<campus_RECURSO> _recursos = da.getResources().Where(r => r.COD_Recurso.Contains(_type)).OrderByDescending(r => r.ID_Recurso).ToList();
                        if (_recursos.Count > 0)
                        {
                            string num_resource = _recursos[0].COD_Recurso.Split('-')[1];
                            _cod_recurso = _type + "-" + (int.Parse(num_resource) + 1).ToString("00000");
                        }
                        else
                            _cod_recurso = _type + "-00000";
                    }

                    /// 3.1.- Insertar un nuevo recurso
                    campus_RECURSO _rec_directo = new campus_RECURSO();
                    _rec_directo.COD_Recurso = _cod_recurso;
                    _rec_directo.Titulo = _titulo;
                    _rec_directo.ID_Tipo_Recurso = _tipo;
                    _rec_directo.ID_Area_Funcional = _area;
                    _rec_directo.Activo = (_activo ? ((int)Constantes.activo.Activo).ToString() : ((int)Constantes.activo.NoActivo).ToString());
                    _rec_directo.Version = _version;
                    _rec_directo.Derechos = _derechos;
                    _rec_directo.Rec_Interno = _recurso_int;
                    _rec_directo.Rec_Externo = _recurso_ext;
                    _rec_directo.Key = Guid.NewGuid().ToString().ToUpper();

                    idResultado = da.insertResource(_rec_directo);
                    if (idResultado > 0)
                    {
                        if (!String.IsNullOrEmpty(_rec_directo.Rec_Interno))
                        {
                            /// 3.2.1.- Guardar la foto en la carpeta correcta
                            string ruta = string.Empty;
                            if (_rec_directo.ID_Tipo_Recurso == (int)Constantes.type_recurso.Multimedia)
                                ruta = ConfigurationManager.AppSettings["route_multimedia"];
                            else
                                ruta = ConfigurationManager.AppSettings["route_nota_tecnica"];

                            /// 3.2.2.- Rutas nuevas
                            string ruta_origen = ruta + "temp\\" + _rec_directo.Rec_Interno;
                            string ruta_destino = ruta;

                            /// 3.2.3.- Si no existe el directorio lo creamos.
                            if (!(Directory.Exists(ruta_destino)))
                                Directory.CreateDirectory(ruta_destino);

                            ruta_destino = ruta_destino + _rec_directo.Rec_Interno;

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

                        /// 3.3.- Añadir el recurso al recurso directo
                        campus_RD_R _rd = new campus_RD_R();
                        _rd.id_RecursoDirecto = idRecDirecto;
                        _rd.ID_Recurso = idResultado;

                        long insert_rd = da.insertRD_R(_rd);
                        if (insert_rd < 1)
                            txt_error.InnerHtml = "Se ha producido un error al añadir el recurso en directo";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir el recurso";
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - recurso-mantenimiento.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            if (idResultado > 0)
                Response.Redirect("lista-tipo-recursos-directo.aspx?idrd=" + idRecDirecto + (!String.IsNullOrEmpty(Request.QueryString["lrc"]) ? "&lrc=" + int.Parse(Request.QueryString["lrc"].ToString()) : string.Empty));
        }

        [WebMethod(Description = "Eliminar un fichero")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static bool delete_rec_int(long? id_recurso, string recurso_interno)
        {
            bool _delete = false;
            try
            {
                if (id_recurso != null && id_recurso > 0)
                {
                    DataAccess da = new DataAccess();

                    /// 1.0.- Sacar los datos del recurso
                    List<campus_RECURSO> _recurso = da.getResourcesById(id_recurso.Value);
                    if (_recurso.Count == 1)
                    {
                        /// 1.1.- Sacar la ruta de la carpeta correcta
                        string ruta = string.Empty;

                        if (_recurso[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Multimedia)
                            ruta = ConfigurationManager.AppSettings["route_multimedia"];
                        else
                            ruta = ConfigurationManager.AppSettings["route_nota_tecnica"];

                        /// 1.2.- Eliminar el fichero
                        if (!String.IsNullOrEmpty(_recurso[0].Rec_Interno))
                            File.Delete(ruta + _recurso[0].Rec_Interno);

                        /// 1.3.- Actualizar los datos del usuario                
                        _recurso[0].Rec_Interno = null;

                        bool _update = da.updateRecurso(_recurso[0]);
                        if (_update)
                            _delete = true;
                    }
                }
                else
                {
                    /// 1.1.- Sacar la ruta de la carpeta correcta
                    string ruta = ConfigurationManager.AppSettings["route_nota_tecnica"] + "temp\\";

                    /// 1.2.- Eliminar el fichero
                    if (!String.IsNullOrEmpty(recurso_interno))
                        File.Delete(ruta + recurso_interno);

                    _delete = true;
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - recurso-mantenimiento.cs::delete_rec_int()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _delete = false;
            }
            return _delete;
        }

        private void cargar_combos()
        {
            /// 0.- Sacar datos de la BBDD
            List<sbs2_area_funcional> _areas = da.getAreaFuncionalByIdArea(-1);

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
        }

        private void cargar_datos(long idRecurso)
        {
            /// 1.- Sacar los datos del recurso
            List<campus_RECURSO> _recursos = da.getResourcesById(idRecurso);
            if (_recursos.Count == 1)
            {
                txt_titulo.Value = _recursos[0].Titulo;
                ddlTipo.SelectedValue = _recursos[0].ID_Tipo_Recurso.ToString();
                ddlArea.Value = _recursos[0].ID_Area_Funcional.ToString();
                chkActivo.Checked = _recursos[0].Activo == ((int)Constantes.activo.Activo).ToString() ? true : false;
                resource_version.Value = _recursos[0].Version.ToString();
                resource_derechos.Value = _recursos[0].Derechos;
                txt_rec_int.Value = _recursos[0].Rec_Interno;
                txt_rec_ext.Value = _recursos[0].Rec_Externo;

                if (!String.IsNullOrEmpty(txt_rec_int.Value))
                {
                    block_delete_rec_int.Attributes["class"] = block_delete_rec_int.Attributes["class"].Replace("hidden", string.Empty);
                    block_see.Attributes["class"] = block_see.Attributes["class"].Replace("hidden", string.Empty);
                    if (_recursos[0].ID_Tipo_Recurso == (int)Constantes.type_recurso.Multimedia)
                        lnk_rec_int.HRef = ConfigurationManager.AppSettings["multimedia"] + _recursos[0].Rec_Interno;
                    else
                        lnk_rec_int.HRef = ConfigurationManager.AppSettings["nota_tecnica"] + _recursos[0].Rec_Interno;
                    block_upload_rec_int.Attributes["class"] = block_upload_rec_int.Attributes["class"].Insert(block_upload_rec_int.Attributes["class"].Length, " hidden");
                }
            }
        }
    }
}