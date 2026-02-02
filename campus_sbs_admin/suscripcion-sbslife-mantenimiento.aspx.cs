using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class suscripcion_sbslife_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la url
                long idProducto = !String.IsNullOrEmpty(Request.QueryString["idp"]) ? long.Parse(Request.QueryString["idp"]) : -1;
                long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
                long ticks = !String.IsNullOrEmpty(Request.QueryString["date"]) ? long.Parse(Request.QueryString["date"]) : -1;

                /// 2.- Cargar datos
                cargar_suscripcion(idProducto, idUsuario, ticks);                
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos de la url
            long idProducto = !String.IsNullOrEmpty(Request.QueryString["idp"]) ? long.Parse(Request.QueryString["idp"]) : -1;
            long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"]) : -1;
            long ticks = !String.IsNullOrEmpty(Request.QueryString["date"]) ? long.Parse(Request.QueryString["date"]) : -1;

            if (idProducto > 0 && idUsuario > 0 && ticks > 0)
            {
                DateTime _fecha = new DateTime(ticks);

                /// 2.- Sacar los datos del formulario
                long idUsuarioForm = long.Parse(idAlumno.Value);
                long idProductoForm = long.Parse(ddlProducto.SelectedValue);
                DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
                DateTime fecha_fin = DateTime.Parse(txtFechaFin.Value);
                decimal importe = decimal.Parse(txt_importe.Value.Replace(".", ","));
                string comentarios = txt_comentarios.Value;

                if (idUsuario != idUsuarioForm || idProducto != idProductoForm || _fecha != fecha_alta)
                {
                    /// 3.- Buscar y eliminar la suscripción
                    List<EDU_Suscriptores> _suscripciones = da.getEduSuscriptoresByUser(idUsuario);
                    _suscripciones = _suscripciones.Where(_ => _.Id_Edu_Producto == idProducto && _.Fecha_Alta == _fecha).ToList();
                    if (_suscripciones.Count == 1)
                    {
                        bool _delete_suscriptor = da.deleteEduSuscriptor(_suscripciones[0]);
                        if (_delete_suscriptor)
                        {
                            EDU_Suscriptores _suscriptor = new EDU_Suscriptores();
                            _suscriptor.Id_Edu_Producto = idProductoForm;
                            _suscriptor.Id_Persona = idUsuarioForm;
                            _suscriptor.Fecha_Alta = fecha_alta;
                            _suscriptor.Fecha_Fin = fecha_fin;
                            _suscriptor.Importe = importe;
                            _suscriptor.Comentarios = comentarios;

                            int insert = da.insertEduSuscriptor(_suscriptor);
                            if (insert > 0)
                                Response.Redirect("lista-suscripciones-sbslife.aspx");
                            else
                                txt_error.InnerHtml = "Se ha producido un error al añadir los datos de la suscripción";
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al eliminar la suscripción";
                    }
                }
                else
                {
                    List<EDU_Suscriptores> _suscripciones = da.getEduSuscriptoresByUser(idUsuario);
                    _suscripciones = _suscripciones.Where(_ => _.Id_Edu_Producto == idProducto && _.Fecha_Alta == _fecha).ToList();
                    if (_suscripciones.Count == 1)
                    {
                        EDU_Suscriptores _suscriptor = _suscripciones[0];
                        _suscriptor.Fecha_Fin = fecha_fin;
                        _suscriptor.Importe = importe;
                        _suscriptor.Comentarios = comentarios;

                        bool _update_suscriptor = da.updateEduSuscriptor(_suscriptor);
                        if (_update_suscriptor)
                            Response.Redirect("lista-suscripciones-sbslife.aspx");
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar los datos de la suscripción";
                    }
                }
            }
            else
            {
                /// 2.- Sacar los datos del formulario
                idUsuario = long.Parse(idAlumno.Value);
                idProducto = long.Parse(ddlProducto.SelectedValue);
                DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
                DateTime fecha_fin = DateTime.Parse(txtFechaFin.Value);
                decimal importe = decimal.Parse(txt_importe.Value.Replace(".", ","));
                string comentarios = txt_comentarios.Value;

                EDU_Suscriptores _suscriptor = new EDU_Suscriptores();
                _suscriptor.Id_Edu_Producto = idProducto;
                _suscriptor.Id_Persona = idUsuario;
                _suscriptor.Fecha_Alta = fecha_alta;
                _suscriptor.Fecha_Fin = fecha_fin;
                _suscriptor.Importe = importe;
                _suscriptor.Comentarios = comentarios;

                int insert = da.insertEduSuscriptor(_suscriptor);
                if (insert > 0)
                    Response.Redirect("lista-suscripciones-sbslife.aspx");
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir los datos de la suscripción";
            }
        }

        [WebMethod(Description = "Busca alumnos a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> search_student(string name)
        {
            DataAccess da = new DataAccess();

            List<Usuarios> list_users = new List<Usuarios>();
            List<CLIENTES> lst_users = da.getUserBySearch(name, null);
            if (lst_users.Count > 0)
                list_users = lst_users.Select(user => new Usuarios { id_usuario = user.id_cliente, nombre_completo = user.Nombre_Completo }).ToList();
            return list_users;
        }

        private void cargar_suscripcion(long idProducto, long idUsuario, long ticks)
        {
            /// 1.- Cargar productos
            cargar_productos();

            /// 2.- Cargar datos de la sucripción
            if (idProducto > 0 && idUsuario > 0 && ticks > 0)
            {
                DateTime _fecha = new DateTime(ticks);

                /// 1.- Activar / Desactivar suscripción y confirmar email
                List<EDU_Suscriptores> _suscripciones = da.getEduSuscriptoresByUser(idUsuario);
                _suscripciones = _suscripciones.Where(_ => _.Id_Edu_Producto == idProducto && _.Fecha_Alta == _fecha).ToList();
                if (_suscripciones.Count == 1)
                {
                    List<CLIENTES> _user = da.getUserById(idUsuario);
                    if (_user.Count == 1)
                        txt_alumno.Value = _user[0].Nombre_Completo;
                    else
                        txt_alumno.Value = idUsuario.ToString();
                    idAlumno.Value = idUsuario.ToString();

                    ddlProducto.SelectedValue = _suscripciones[0].Id_Edu_Producto.ToString();
                    txtFechaAlta.Value = _suscripciones[0].Fecha_Alta.ToShortDateString();
                    txtFechaFin.Value = _suscripciones[0].Fecha_Fin.HasValue ? _suscripciones[0].Fecha_Fin.Value.ToShortDateString() : string.Empty;
                    txt_importe.Value = _suscripciones[0].Importe.ToString();
                    txt_comentarios.Value = _suscripciones[0].Comentarios;
                }
            }
            else
                txtFechaAlta.Value = DateTime.Today.ToShortDateString();
        }

        private void cargar_productos()
        {
            /// 1.- Cargar los productos
            List<EDU_Productos> _productos = da.getEduProductos(null);
            if (_productos.Count > 0)
            {
                this.ddlProducto.DataSource = _productos;
                this.ddlProducto.DataTextField = "Nombre";
                this.ddlProducto.DataValueField = "Id_Edu_Producto";
                this.ddlProducto.DataBind();
                this.ddlProducto.Items.Add(new ListItem("Seleccione", "-1"));
                ddlProducto.SelectedValue = "-1";
            }
        }
    }
}