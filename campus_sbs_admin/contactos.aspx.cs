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
    public partial class contactos : System.Web.UI.Page
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
                    loadContacts(list_user[0]);
            }
        }

        protected void btnBorrarContacto_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_contact = false;

            try
            {
                long idContact = !String.IsNullOrEmpty(hidIdContacto.Value) ? long.Parse(hidIdContacto.Value) : -1;
                if (idContact > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<PRA_CONTACTO> lst_contactos = da.getContactsById(idContact);
                    if (lst_contactos.Count == 1)
                    {
                        /// 2.- Eliminar el contacto
                        delete_contact = da.deleteContact(idContact);

                        /// 3.- Modificar en empresa el nº de contactos
                        List<PRA_EMPRESA> lst_empresas = da.getBusinessById(lst_contactos[0].idEmpresa);
                        if (lst_empresas.Count == 1)
                        {
                            PRA_EMPRESA empresa = lst_empresas[0];
                            empresa.num_contactos = empresa.num_contactos - 1;

                            bool update_company = da.updateCompany(empresa);
                            if (!update_company)
                            {
                                delete_contact = false;
                                LogUtils.InsertarLog(" ERROR - contactos.cs::btnBorrarContacto_Click()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al modificar en empresa el nº de contactos");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el contacto');</script>");

                LogUtils.InsertarLog(" ERROR - contactos.cs::btnBorrarContacto_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_contact)
                Response.Redirect("contactos.aspx" + (!String.IsNullOrEmpty(Request.QueryString["idb"]) ? "?idb=" + Request.QueryString["idb"] : string.Empty));
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el contacto');</script>");
        }

        private void loadContacts(CLIENTES user)
        {
            /// 0.- Recuperar datos de la url
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;

            /// 1.- Sacar los datos de la BBDD
            List<PRA_CONTACTO> lst_contactos = da.getContactsById(-1);
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(-1);

            /// 1.5.- Filtrar los contactos por empresa
            if (idEmpresa > 0)
                lst_contactos = lst_contactos.Where(c => c.idEmpresa == idEmpresa).ToList();

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Contactos\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha Alta</th>");
            sbuild.Append("<th>Fecha Baja</th>");
            sbuild.Append("<th>Contacto</th>");
            sbuild.Append("<th>Mail</th>");
            sbuild.Append("<th>Teléfono</th>");
            sbuild.Append("<th>Cargo</th>");
            sbuild.Append("<th>Empresa</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar los contactos
            foreach (var contact in lst_contactos)
            {
                /// 2.3.1.- Sacar los datos de la empresa
                List<PRA_EMPRESA> lst_empresa = lst_empresas.Where(emp => emp.idEmpresa == contact.idEmpresa).ToList();

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + contact.FechaAlta.ToShortDateString() + "</td>");
                sbuild.Append("<td>" + (contact.FechaBaja != null ? contact.FechaBaja.Value.ToShortDateString() : string.Empty) + "</td>");
                sbuild.Append("<td><span class='hidden'>" + contact.Nombre + " " + contact.Apellidos + "</span><a href=\"contacto-mantenimiento.aspx?idc=" + contact.idContacto + "\" title=\"Datos de contacto\">" + contact.Nombre + " " + contact.Apellidos + (!String.IsNullOrEmpty(contact.Nif) ? " (" + contact.Nif + ")" : string.Empty) + "</a></td>");
                sbuild.Append("<td><a href=\"contacto-mantenimiento.aspx?idc=" + contact.idContacto + "\" title=\"" + contact.Mail + "\">" + contact.Mail + " </a></td>");
                sbuild.Append("<td><a href=\"contacto-mantenimiento.aspx?idc=" + contact.idContacto + "\" title=\"Teléfonos\">" + (!String.IsNullOrEmpty(contact.Telefono) ? (!String.IsNullOrEmpty(contact.TelefonoMovil) ? contact.Telefono + " / " + contact.TelefonoMovil : contact.Telefono) : (!String.IsNullOrEmpty(contact.TelefonoMovil) ? contact.TelefonoMovil : string.Empty)) + " </a></td>");
                sbuild.Append("<td><a href=\"contacto-mantenimiento.aspx?idc=" + contact.idContacto + "\" title=\"" + contact.Cargo + "\">" + contact.Cargo + " </a></td>");

                if (lst_empresa.Count > 0)
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + contact.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + lst_empresa[0].RazonSocial + " [" + lst_empresa[0].idEmpresa + "] (" + lst_empresa[0].Mail + ")</a></td>");
                else
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + contact.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + contact.idEmpresa + "</a></td>");

                sbuild.Append("<td><a href=\"contacto-mantenimiento.aspx?idc=" + contact.idContacto + "\" title=\"Editar\"><i class='fas fa-edit fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el contacto: " + contact.Nombre + " " + contact.Apellidos + "?\")){eliminarContacto(" + contact.idContacto + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_contactos.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            txt_contactos.InnerHtml = "<i class='far fa-address-card'></i> Listado de Contactos <a href='contacto-mantenimiento.aspx' title='Nuevo contacto' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nuevo contacto</small></a>";
        }
    }
}