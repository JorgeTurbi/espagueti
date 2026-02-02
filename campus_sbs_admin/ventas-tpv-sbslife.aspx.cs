using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class ventas_tpv_sbslife : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el parámetro 'k' en la url
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                {
                    Session.Add("usuario", list_user[0]);
                }
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
                    loadSales(list_user[0]);
            }
        }

        protected void btnConfirmarPedido_Click(object sender, ImageClickEventArgs e)
        {
            bool confirm_sale = false;

            try
            {
                long idPedido = !String.IsNullOrEmpty(hidIdPedido.Value) ? long.Parse(hidIdPedido.Value) : -1;
                if (idPedido > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<Pagos> lst_pedidos = da.getPaymentByIdOrder(idPedido);
                    if (lst_pedidos.Count == 1)
                    {
                        Pagos pay = lst_pedidos[0];
                        pay.aceptado = ((int)Constantes.activo.Activo).ToString();
                        pay.procesado = true;

                        bool update_pay = da.updatePayment(pay);
                        if (update_pay)
                            confirm_sale = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ventas-tpv-sbslife.cs::btnConfirmarPedido_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                confirm_sale = false;
            }

            /// Si no hay errores recargar la página
            if (confirm_sale)
                Response.Redirect("ventas-tpv-sbslife.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al confirmar el pedido');</script>");
        }

        protected void btnProcesarPedido_Click(object sender, ImageClickEventArgs e)
        {
            bool process_sale = false;

            try
            {
                long idPedido = !String.IsNullOrEmpty(hidIdPedido.Value) ? long.Parse(hidIdPedido.Value) : -1;
                if (idPedido > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<Pagos> lst_pedidos = da.getPaymentByIdOrder(idPedido);
                    if (lst_pedidos.Count == 1)
                    {
                        Pagos pay = lst_pedidos[0];
                        pay.procesado = true;

                        bool update_pay = da.updatePayment(pay);
                        if (update_pay)
                            process_sale = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ventas-tpv-sbslife.cs::btnProcesarPedido_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                process_sale = false;
            }

            /// Si no hay errores recargar la página
            if (process_sale)
                Response.Redirect("ventas-tpv-sbslife.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al procesar el pedido');</script>");
        }

        protected void btnProcesarSuscripcion_Click(object sender, ImageClickEventArgs e)
        {
            bool process_subscription = false;

            try
            {
                long idPedido = !String.IsNullOrEmpty(hidIdPedido.Value) ? long.Parse(hidIdPedido.Value) : -1;
                if (idPedido > 0)
                {
                    /// 1.- Sacar los datos del pedido
                    List<Pedidos> _pedidos = da.getPedidosById(idPedido);
                    if (_pedidos.Count == 1)
                    {
                        /// 1.1.- Sacar el pago asociado al pedido
                        List<Pagos> _pago = da.getPaymentByIdOrder(idPedido);
                        if (_pago.Count == 1)
                        {
                            /// 1.2.- Sacar los datos del usuario
                            List<CLIENTES> _users = da.getUserById(_pedidos[0].id_cliente);

                            ///2.- Comprobar si el usuario tiene una suscripción
                            List<EDU_Suscriptores> _suscripciones_user = da.getEduSuscriptoresByUser(_pedidos[0].id_cliente);
                            if (_suscripciones_user.Count == 0)
                            {
                                /// 5.1.- Registro en SBS Life
                                bool _register_user = Utilities.RegisterUser(_pedidos[0].id_cliente);
                                if (_register_user)
                                {
                                    /// 5.1.- Añadir una suscripción
                                    EDU_Suscriptores _suscripcion_user = new EDU_Suscriptores();
                                    _suscripcion_user.Id_Edu_Producto = (long)Constantes.producto.SBS_Life;
                                    _suscripcion_user.Id_Persona = _pedidos[0].id_cliente;
                                    _suscripcion_user.Fecha_Alta = DateTime.Today;
                                    _suscripcion_user.Fecha_Fin = DateTime.Today.AddDays(int.Parse(ConfigurationManager.AppSettings["num_dias_sbslife"]));
                                    _suscripcion_user.Importe = _pago[0].importe.HasValue ? _pago[0].importe.Value : 0;
                                    _suscripcion_user.Comentarios = "Suscripción anual a SBS Life";
                                    _suscripcion_user.IdPedido = idPedido;
                                    _suscripcion_user.Activo = true;

                                    int _insert_suscripcion = da.insertEduSuscriptor(_suscripcion_user);
                                    if (_insert_suscripcion > 0)
                                    {
                                        bool _send = Utilities.SendEmailUser(_users[0], _suscripcion_user, true);
                                        process_subscription = true;
                                    }
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona);
                                        process_subscription = false;
                                    }
                                }
                                else
                                {
                                    LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _pedidos[0].id_cliente);
                                    process_subscription = false;
                                }
                            }
                            else
                            {
                                DateTime _max_date = _suscripciones_user.Where(_ => _.Fecha_Fin.HasValue).Select(_ => _.Fecha_Fin.Value).Max();
                                DateTime _fecha_inicio = DateTime.Today;
                                if (_max_date >= _fecha_inicio)
                                {
                                    /// 5.1.- Añadir una suscripción
                                    EDU_Suscriptores _suscripcion_user = new EDU_Suscriptores();
                                    _suscripcion_user.Id_Edu_Producto = (long)Constantes.producto.SBS_Life;
                                    _suscripcion_user.Id_Persona = _pedidos[0].id_cliente;
                                    _suscripcion_user.Fecha_Alta = _max_date.AddDays(1);
                                    _suscripcion_user.Fecha_Fin = _max_date.AddDays(int.Parse(ConfigurationManager.AppSettings["num_dias_sbslife"]) + 1);
                                    _suscripcion_user.Importe = _pago[0].importe.HasValue ? _pago[0].importe.Value : 0;
                                    _suscripcion_user.Comentarios = "Suscripción anual a SBS Life";
                                    _suscripcion_user.IdPedido = idPedido;
                                    _suscripcion_user.Activo = true;

                                    int _insert_suscripcion = da.insertEduSuscriptor(_suscripcion_user);
                                    if (_insert_suscripcion > 0)
                                    {
                                        bool _send = Utilities.SendEmailUser(_users[0], _suscripcion_user, true);
                                        process_subscription = true;
                                    }
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona);
                                        process_subscription = false;
                                    }
                                }
                                else
                                {
                                    /// 5.1.- Añadir una suscripción
                                    EDU_Suscriptores _suscripcion_user = new EDU_Suscriptores();
                                    _suscripcion_user.Id_Edu_Producto = (long)Constantes.producto.SBS_Life;
                                    _suscripcion_user.Id_Persona = _pedidos[0].id_cliente;
                                    _suscripcion_user.Fecha_Alta = DateTime.Today;
                                    _suscripcion_user.Fecha_Fin = DateTime.Today.AddDays(int.Parse(ConfigurationManager.AppSettings["num_dias_sbslife"]));
                                    _suscripcion_user.Importe = _pago[0].importe.HasValue ? _pago[0].importe.Value : 0;
                                    _suscripcion_user.Comentarios = "Suscripción anual a SBS Life";
                                    _suscripcion_user.IdPedido = idPedido;
                                    _suscripcion_user.Activo = true;

                                    int _insert_suscripcion = da.insertEduSuscriptor(_suscripcion_user);
                                    if (_insert_suscripcion > 0)
                                    {
                                        bool _send = Utilities.SendEmailUser(_users[0], _suscripcion_user, true);
                                        process_subscription = true;
                                    }
                                    else
                                    {
                                        LogUtils.InsertarLog(" ERROR - Se ha producido un error al añadir la suscripción al usuario " + _suscripcion_user.Id_Persona);
                                        process_subscription = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - ventas-tpv-sbslife.cs::btnProcesarSuscripcion_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                process_subscription = false;
            }

            /// Si no hay errores recargar la página
            if (process_subscription)
                Response.Redirect("ventas-tpv-sbslife.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al procesar la suscripción');</script>");
        }

        protected void btnTodasVentas_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Sacar los datos de la BBDD
            List<VentasTPV> lst_ventas = da.getListSales(-1);

            /// 1.1.- Sacar los datos de ventas de sbs Life
            List<Lineas_pedido> _pedidos_sbslife = da.getPedidosByProducto(long.Parse(ConfigurationManager.AppSettings["curso_sbslife"]));

            /// 1.2.- Sacar los ids de los pedidos de sbs life
            List<long> _ids = _pedidos_sbslife.Select(_ => _.id_pedido).Distinct().ToList();

            /// 1.3.- Filtrar las ventas
            lst_ventas = lst_ventas.Where(_ => _ids.Contains(_.idPedido)).ToList();
            
            /// 2.- Pintar la tabla
            table_listado_ventas_tpv.InnerHtml = paint_table(lst_ventas);

            /// 3.- Pintar el título
            txt_ventas_tpv.InnerHtml = "<i class='far fa-credit-card'></i> Listado de ventas por TPV<a href='javascript:void(0);' onclick='all_sales()' title='Todas las ventas por TPV' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i>  Todas las ventas</small></a>";
        }

        private void loadSales(CLIENTES user)
        {
            /// 1.- Sacar los datos de la BBDD
            int days = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["dias_practicas"]) ? int.Parse(ConfigurationManager.AppSettings["dias_practicas"]) : 0;
            List<VentasTPV> lst_ventas = da.getListSales(days);

            /// 1.1.- Sacar los datos de ventas de sbs Life
            List<Lineas_pedido> _pedidos_sbslife = da.getPedidosByProducto(long.Parse(ConfigurationManager.AppSettings["curso_sbslife"]));

            /// 1.2.- Sacar los ids de los pedidos de sbs life
            List<long> _ids = _pedidos_sbslife.Select(_ => _.id_pedido).Distinct().ToList();

            /// 1.3.- Filtrar las ventas
            lst_ventas = lst_ventas.Where(_ => _ids.Contains(_.idPedido)).ToList();

            /// 2.- Pintar la tabla
            table_listado_ventas_tpv.InnerHtml = paint_table(lst_ventas);

            /// 3.- Pintar el título
            txt_ventas_tpv.InnerHtml = "<i class='far fa-credit-card'></i> Listado de ventas por TPV<a href='javascript:void(0);' onclick='all_sales()' title='Todas las ventas por TPV' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i>  Todas las ventas</small></a>";
        }

        private string paint_table(List<VentasTPV> lst_ventas)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Sacar las suscripciones con pedido
            List<EDU_Suscriptores> _suscripciones = da.getEduSuscriptoresPedidos(true);

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Ventas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Mail / Tlf</th>");
            sbuild.Append("<th>Pedido</th>");
            sbuild.Append("<th>Fecha Pedido</th>");
            sbuild.Append("<th>Importe</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Confirmar</th>");
            sbuild.Append("<th>Procesado</th>");
            sbuild.Append("<th>Alta</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 3.- Pintar las ventas por TPV
            foreach (var sale in lst_ventas)
            {
                if (sale.aceptado == ((int)Constantes.activo.Activo).ToString())
                {
                    sbuild.Append("<tr class='text-color-primary'>");
                    sbuild.Append("<td><span class='hidden'>" + sale.nombreCliente + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + sale.idCliente + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + sale.nombreCliente + " (" + sale.idCliente + ")</a></td>");
                }
                else if (sale.procesado != null && sale.procesado.Value)
                {
                    sbuild.Append("<tr class='text-color-red'>");
                    sbuild.Append("<td><span class='hidden'>" + sale.nombreCliente + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + sale.idCliente + "\" class=\"text-color-red\"   title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + sale.nombreCliente + " (" + sale.idCliente + ")</a></td>");
                }
                else
                {
                    sbuild.Append("<tr class='text-color-black bold'>");
                    sbuild.Append("<td><span class='hidden'>" + sale.nombreCliente + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + sale.idCliente + "\" class=\"text-color-black bold\"   title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + sale.nombreCliente + " (" + sale.idCliente + ")</a></td>");
                }

                sbuild.Append("<td>" + sale.mailCliente + "<br />(" + sale.tlfCliente + ")</td>");
                sbuild.Append("<td>" + sale.numPedido + "</td>");
                sbuild.Append("<td>" + (sale.fechaPedido != null ? sale.fechaPedido.Value.ToString() : string.Empty) + "</td>");
                sbuild.Append("<td>" + Utilities.PonerPuntoMiles(Math.Round((sale.importe != null ? sale.importe.Value : new decimal(0)), 2)) + "€</td>");
                sbuild.Append("<td>" + sale.tipo + "</td>");
                if (sale.aceptado == ((int)Constantes.activo.Activo).ToString())
                {
                    sbuild.Append("<td>" + (sale.aceptado == ((int)Constantes.activo.Activo).ToString() ? "<i class='far fa-check-square fa-1-6x'></i>" : "<a href='javascript:void(0)' onclick='confirmar_pedido(" + sale.idPedido + ")'>Confirmar</a>") + "</td>");
                    sbuild.Append("<td>" + (sale.procesado != null && sale.procesado.Value ? "<i class='far fa-check-square fa-1-6x'></i>" : "<a href='javascript:void(0)' onclick='procesar_pedido(" + sale.idPedido + ")'>Procesar</a>") + "</td>");
                }
                else if (sale.procesado != null && sale.procesado.Value)
                {
                    sbuild.Append("<td>" + (sale.aceptado == ((int)Constantes.activo.Activo).ToString() ? "<i class='far fa-check-square fa-1-6x'></i>" : "<a href='javascript:void(0)' class=\"text-color-red\" onclick='confirmar_pedido(" + sale.idPedido + ")'>Confirmar</a>") + "</td>");
                    sbuild.Append("<td>" + (sale.procesado != null && sale.procesado.Value ? "<i class='far fa-check-square fa-1-6x'></i>" : "<a href='javascript:void(0)' class=\"text-color-red\" onclick='procesar_pedido(" + sale.idPedido + ")'>Procesar</a>") + "</td>");
                }
                else
                {
                    sbuild.Append("<td>" + (sale.aceptado == ((int)Constantes.activo.Activo).ToString() ? "<i class='far fa-check-square fa-1-6x'></i>" : "<a href='javascript:void(0)' class=\"text-color-black bold\" onclick='confirmar_pedido(" + sale.idPedido + ")'>Confirmar</a>") + "</td>");
                    sbuild.Append("<td>" + (sale.procesado != null && sale.procesado.Value ? "<i class='far fa-check-square fa-1-6x'></i>" : "<a href='javascript:void(0)' class=\"text-color-black bold\" onclick='procesar_pedido(" + sale.idPedido + ")'>Procesar</a>") + "</td>");
                }

                if (sale.aceptado == ((int)Constantes.activo.Activo).ToString() && sale.procesado.HasValue)
                {
                    if (_suscripciones.Where(_ => _.IdPedido == sale.idPedido).Count() == 0)
                        sbuild.Append("<td><a href='javascript:void(0)' class=\"text-color-black bold\" onclick='procesar_suscripcion(" + sale.idPedido + ")' title='Añadir suscripción'><i class='fas fa-user-plus fa-1-6x'></i></a></td>");
                    else
                        sbuild.Append("<td></td>");
                }
                else
                    sbuild.Append("<td></td>");
                
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 4.- Añadimos la tabla al div
            return sbuild.ToString();
        }
    }
}