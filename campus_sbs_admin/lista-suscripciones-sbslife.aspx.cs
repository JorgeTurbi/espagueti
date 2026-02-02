using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_suscripciones_sbslife : System.Web.UI.Page
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
                    /// 1.- Pintar el título
                    txt_suscripciones.InnerHtml = "<i class='far fa-list-alt'></i> Listado de suscripciones <a href='suscripcion-sbslife-mantenimiento.aspx' title='Nueva suscripción' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva suscripción</small></a><a href='javascript:void(0);' onclick='mostrarTodasSuscripciones()' title='Mostrar todas las suscripciones' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-eye fa-2x'></i> Mostrar todas las suscripciones</small></a>";

                    /// 2.- Cargar los programas
                    cargar_suscripciones(true);
                }
            }
        }

        protected void btnActivarSuscripcion_Click(object sender, ImageClickEventArgs e)
        {
            bool process = true;
            
            try
            {
                long idProducto = !String.IsNullOrEmpty(hidIdProducto.Value) ? long.Parse(hidIdProducto.Value) : -1;
                long idUsuario = !String.IsNullOrEmpty(hidIdUsuario.Value) ? long.Parse(hidIdUsuario.Value) : -1;
                long ticks = !String.IsNullOrEmpty(hidFecha.Value) ? long.Parse(hidFecha.Value) : -1;

                if (idUsuario != -1 && idProducto != -1 && ticks != -1)
                {
                    DateTime _fecha = new DateTime(ticks);

                    /// 1.- Activar / Desactivar suscripción y confirmar email
                    List<EDU_Suscriptores> _suscripciones = da.getEduSuscriptoresByUser(idUsuario);
                    _suscripciones = _suscripciones.Where(_ => _.Id_Edu_Producto == idProducto && _.Fecha_Alta == _fecha).ToList();
                    if (_suscripciones.Count == 1)
                    {
                        if (!_suscripciones[0].Activo)
                        {
                            /// 2.- Registro en SBS Life
                            bool _register_user = Utilities.RegisterUser(idUsuario);
                            if (_register_user)
                            {
                                EDU_Suscriptores _suscripcion = _suscripciones[0];
                                _suscripcion.Activo = true;

                                bool update_suscripcion = da.updateEduSuscriptor(_suscripcion);
                                if (update_suscripcion)
                                {
                                    List<CLIENTES> _users = da.getUserById(idUsuario);
                                    process = Utilities.SendEmailUser(_users[0], _suscripcion, false);                                    
                                }
                                else
                                {
                                    process = false;
                                    ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar la suscripción');</script>");
                                }
                            }
                            else
                            {
                                process = false;
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar la suscripción');</script>");
                            }
                        }
                        else
                        {
                            EDU_Suscriptores _suscripcion = _suscripciones[0];
                            _suscripcion.Activo = false;

                            bool update_suscripcion = da.updateEduSuscriptor(_suscripcion);
                            if (update_suscripcion)
                                process = true;
                            else
                            {
                                process = false;
                                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar la suscripción');</script>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar la suscripción');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscripciones-sbslife.cs::btnActivarSuscripcion_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                process = false;
            }

            /// Si no hay errores recargar la página
            if (process)
                Response.Redirect("lista-suscripciones-sbslife.aspx");
        }
        protected void btnBorrarSuscripcion_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_suscripcion = false;

            try
            {
                long idProducto = !String.IsNullOrEmpty(hidIdProducto.Value) ? long.Parse(hidIdProducto.Value) : -1;
                long idUsuario = !String.IsNullOrEmpty(hidIdUsuario.Value) ? long.Parse(hidIdUsuario.Value) : -1;
                long ticks = !String.IsNullOrEmpty(hidFecha.Value) ? long.Parse(hidFecha.Value) : -1;

                if (idUsuario != -1 && idProducto != -1 && ticks != -1)
                {
                    DateTime _fecha = new DateTime(ticks);

                    /// 1.- Sacar los datos de la BBDD
                    List<EDU_Suscriptores> _suscripciones = da.getEduSuscriptoresByUser(idUsuario);
                    _suscripciones = _suscripciones.Where(_ => _.Id_Edu_Producto == idProducto && _.Fecha_Alta == _fecha).ToList();
                    if (_suscripciones.Count == 1)
                    {
                        /// 2.- Eliminar la suscripción
                        delete_suscripcion = da.deleteEduSuscriptor(_suscripciones[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la suscripción');</script>");

                LogUtils.InsertarLog(" ERROR - lista-suscripciones-sbslife.cs::btnBorrarSuscripcion_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_suscripcion)
                Response.Redirect("lista-suscripciones-sbslife.aspx");
        }
        protected void btnAllSuscripciones_Click(object sender, ImageClickEventArgs e)
        {
            cargar_suscripciones(false);
        }

        private void cargar_suscripciones(bool active)
        {
            /// 1.- Sacar los suscriptores activos
            List<EDU_Suscriptores> _suscripciones = da.getEduSuscriptoresActivos(active);
            _suscripciones = _suscripciones.Where(_ => _.Id_Edu_Producto == (long)Constantes.producto.SBS_Life).ToList();
            if (_suscripciones.Count > 0)
            {
                /// 1.1.- Sacar los pedidos
                List<long> _id_pedidos = _suscripciones.Where(_ => _.IdPedido.HasValue).Select(_ => _.IdPedido.Value).Distinct().ToList();
                List<Pedidos> _pedidos = da.getPedidosByList(_id_pedidos);

                /// 2.- Sacar los datos de los usuarios
                List<long> _id_users = _suscripciones.Select(_ => _.Id_Persona).Distinct().ToList();
                List<CLIENTES> _users = da.getUserByList(_id_users);

                /// 3.- Pintar la tabla
                StringBuilder sbuild = new StringBuilder();

                /// 3.1.- Inicio tabla
                sbuild.Append("<table id =\"tabla_Suscripciones\" class=\"display compact\" style =\"width:100%\"><thead>");

                /// 3.2.- Cabecera
                sbuild.Append("<tr>");                
                sbuild.Append("<th>Alumno</th>");
                sbuild.Append("<th>Fecha Inicio</th>");
                sbuild.Append("<th>Fecha Fin</th>");
                sbuild.Append("<th>Importe</th>");
                sbuild.Append("<th>Comentarios</th>");
                sbuild.Append("<th>Pedido</th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("</tr>");
                sbuild.Append("</thead><tbody>");

                /// 3.3.- Pintar las suscripciones
                foreach (var _suscripcion in _suscripciones)
                {
                    Pedidos _pedido = _pedidos.Where(_ => _.id_pedido == _suscripcion.IdPedido).FirstOrDefault();

                    if (_suscripcion.Fecha_Fin.HasValue && _suscripcion.Fecha_Fin.Value < DateTime.Today)
                    {
                        sbuild.Append("<tr class='text-color-red'>");
                        sbuild.Append($"<td><a href=\"ficha-alumno-crm.aspx?idu={_suscripcion.Id_Persona}\" title=\"Datos del alumno\" target=\"_blank\" class=\"text-color-red\"><i class=\"fas fa-user\"></i> {(_users.Where(_ => _.id_cliente == _suscripcion.Id_Persona).Select(_ => _.Nombre_Completo).FirstOrDefault())} ({_suscripcion.Id_Persona})</a></td>");
                    }
                    else
                    {
                        sbuild.Append("<tr>");

                        if (!_suscripcion.Activo)
                            sbuild.Append($"<td><a href=\"ficha-alumno-crm.aspx?idu={_suscripcion.Id_Persona}\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-orange'></i> {(_users.Where(_ => _.id_cliente == _suscripcion.Id_Persona).Select(_ => _.Nombre_Completo).FirstOrDefault())} ({_suscripcion.Id_Persona})</a></td>");
                        else if (_users.Where(_ => _.id_cliente == _suscripcion.Id_Persona).Select(_ => _.fecha_baja.HasValue).FirstOrDefault())
                            sbuild.Append($"<td><a href=\"ficha-alumno-crm.aspx?idu={_suscripcion.Id_Persona}\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-red'></i> {(_users.Where(_ => _.id_cliente == _suscripcion.Id_Persona).Select(_ => _.Nombre_Completo).FirstOrDefault())} ({_suscripcion.Id_Persona})</a></td>");
                        else
                            sbuild.Append($"<td><a href=\"ficha-alumno-crm.aspx?idu={_suscripcion.Id_Persona}\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> {(_users.Where(_ => _.id_cliente == _suscripcion.Id_Persona).Select(_ => _.Nombre_Completo).FirstOrDefault())} ({_suscripcion.Id_Persona})</a></td>");
                    }
                    sbuild.Append($"<td>{_suscripcion.Fecha_Alta.ToShortDateString()}</td>");
                    sbuild.Append($"<td>{(_suscripcion.Fecha_Fin.HasValue ? _suscripcion.Fecha_Fin.Value.ToShortDateString() : null)}</td>");
                    sbuild.Append($"<td>{_suscripcion.Importe}</td>");
                    sbuild.Append($"<td>{_suscripcion.Comentarios}</td>");
                    sbuild.Append($"<td>{(_pedido != null ? _pedido.num_pedido : string.Empty)}</td>");

                    sbuild.Append($"<td><a href='suscripcion-sbslife-mantenimiento.aspx?idp={_suscripcion.Id_Edu_Producto}&idu={_suscripcion.Id_Persona}&date={_suscripcion.Fecha_Alta.Ticks}' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                    if (_suscripcion.Activo)
                        sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea desactivar la suscripción?\")){activarSuscripcion(" + _suscripcion.Id_Edu_Producto + "," + _suscripcion.Id_Persona + "," + _suscripcion.Fecha_Alta.Ticks + ")}'><i class=\"fas fa-power-off text-color-green fa-1-6x\" style=\"cursor: pointer\" title=\"Desactivar suscripción.\"></i></a></td>");
                    else
                        sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea activar la suscripción?\")){activarSuscripcion(" + _suscripcion.Id_Edu_Producto + "," + _suscripcion.Id_Persona + "," + _suscripcion.Fecha_Alta.Ticks + ")}'><i class=\"fas fa-power-off text-color-red fa-1-6x\" style=\"cursor: pointer\" title=\"Activar suscripción.\"></i></a></td>");
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la suscripción?\")){eliminarSuscripcion(" + _suscripcion.Id_Edu_Producto + "," + _suscripcion.Id_Persona + "," + _suscripcion.Fecha_Alta.Ticks + ")}' title='Eliminar suscripción'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");                    
                    sbuild.Append("</tr>");
                }
                sbuild.Append("</tbody></table>");

                table_listado_suscripciones.InnerHtml = sbuild.ToString();
            }
        }
    }
}