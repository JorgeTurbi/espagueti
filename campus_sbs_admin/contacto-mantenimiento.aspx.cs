using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class contacto_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa y del contacto
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                long idContacto = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;

                /// 2.- Cargar las empresas
                cargar_empresas(idEmpresa);

                /// 3.- Cargar los datos del contacto
                if (idContacto > 0)
                    cargar_datos(idContacto);
                else
                    txtFechaAlta.Value = DateTime.Today.ToShortDateString();
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idContacto
            long idContacto = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"]) : -1;
            long idCompany = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"]) : -1;

            /// 2.- Sacar los datos del formulario
            string nombre = txt_cont_nombre.Value;
            string apellidos = txt_cont_apellidos.Value;
            string nif = txt_cont_nif.Value;
            string mail = txt_cont_mail.Value;
            string telefono = txt_cont_telefono.Value;
            string telefono_movil = txt_cont_telefono_movil.Value;
            long idEmpresa = long.Parse(ddlEmpresa.SelectedValue);
            string cargo = txt_cont_cargo.Value;
            DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
            DateTime? fecha_baja = null;
            if (!String.IsNullOrEmpty(txtFechaBaja.Value))
                fecha_baja = DateTime.Parse(txtFechaBaja.Value);
            string comentarios = txt_comentarios.Value;

            /// 3.- Modificar o Insertar
            if (idContacto > 0)
            {
                /// 3.1.- Sacar los datos del contacto
                List<PRA_CONTACTO> lst_contactos = da.getContactsById(idContacto);
                if (lst_contactos.Count == 1)
                {
                    /// 3.2.- Actualizar los datos del contacto
                    PRA_CONTACTO contacto = lst_contactos[0];
                    contacto.Nombre = nombre;
                    contacto.Apellidos = apellidos;
                    contacto.Nif = nif;
                    contacto.Mail = mail;
                    contacto.Telefono = telefono;
                    contacto.TelefonoMovil = telefono_movil;
                    contacto.idEmpresa = idEmpresa;
                    contacto.Cargo = cargo;
                    contacto.FechaAlta = fecha_alta;
                    contacto.FechaBaja = fecha_baja;
                    contacto.Comentarios = comentarios;

                    bool update_contact = da.updateContact(contacto);
                    if (update_contact)
                    {
                        if (idCompany > 0)
                            Response.Redirect("empresas.aspx");
                        else
                            Response.Redirect("contactos.aspx");
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar los datos del contacto";
                }
            }
            else
            {
                /// 3.1.- Actualizar los datos del contacto
                PRA_CONTACTO contacto = new PRA_CONTACTO();
                contacto.Nombre = nombre;
                contacto.Apellidos = apellidos;
                contacto.Nif = nif;
                contacto.Mail = mail;
                contacto.Telefono = telefono;
                contacto.TelefonoMovil = telefono_movil;
                contacto.idEmpresa = idEmpresa;
                contacto.Cargo = cargo;
                contacto.FechaAlta = fecha_alta;
                contacto.FechaBaja = fecha_baja;
                contacto.Comentarios = comentarios;
                                
                long insert_contact = da.insertContact(contacto);
                if (insert_contact > 0)
                {
                    /// 3.1.- Autoincrementar los datos de contactos de una empresa
                    List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idEmpresa);
                    if (lst_empresas.Count == 1)
                    {
                        PRA_EMPRESA empresa = lst_empresas[0];
                        empresa.num_contactos = empresa.num_contactos + 1;

                        bool update_company = da.updateCompany(empresa);
                        if (update_company)
                        {
                            if (idCompany > 0)
                                Response.Redirect("empresas.aspx");
                            else
                                Response.Redirect("contactos.aspx");
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el nº de contactos de una empresa";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al calcular el nº de contactos de una empresa";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir los datos del contacto";
            }
        }

        private void cargar_datos(long idContacto)
        {
            /// 1.- Obtener los datos del contacto  de la BBDD
            List<PRA_CONTACTO> lst_contactos = da.getContactsById(idContacto);
            if (lst_contactos.Count == 1)
            {
                txt_cont_nombre.Value = lst_contactos[0].Nombre;
                txt_cont_apellidos.Value = lst_contactos[0].Apellidos;
                txt_cont_nif.Value = lst_contactos[0].Nif;
                txt_cont_mail.Value = lst_contactos[0].Mail;
                txt_cont_telefono.Value = lst_contactos[0].Telefono;
                txt_cont_telefono_movil.Value = lst_contactos[0].TelefonoMovil;
                ddlEmpresa.SelectedValue = lst_contactos[0].idEmpresa.ToString();
                txt_cont_cargo.Value = lst_contactos[0].Cargo;
                txtFechaAlta.Value = lst_contactos[0].FechaAlta.ToShortDateString();
                txtFechaBaja.Value = lst_contactos[0].FechaBaja != null ? lst_contactos[0].FechaBaja.Value.ToShortDateString() : string.Empty;
                txt_comentarios.Value = lst_contactos[0].Comentarios;
            }
        }

        private void cargar_empresas(long idEmpresa)
        {
            /// 1.- Cargar las empresas
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(-1);

            if (lst_empresas.Count > 0)
            {
                this.ddlEmpresa.DataSource = lst_empresas;
                this.ddlEmpresa.DataTextField = "RazonSocial";
                this.ddlEmpresa.DataValueField = "idEmpresa";
                this.ddlEmpresa.DataBind();
                this.ddlEmpresa.Items.Add(new ListItem("Seleccione", "-1"));
                ddlEmpresa.SelectedValue = idEmpresa.ToString();
            }
        }
    }
}