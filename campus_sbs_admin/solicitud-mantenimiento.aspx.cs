using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class solicitud_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa y del contacto
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                long idSolicitud = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;

                if (idEmpresa > 0)
                {
                    /// 2.- Cargar combo contactos
                    cargar_contactos_empresa(idEmpresa);

                    /// 3.- Cargar los datos de la solicitud
                    if (idSolicitud > 0)
                        cargar_datos(idSolicitud);
                    else
                    {
                        txtFechaAlta.Value = DateTime.Today.ToShortDateString();
                        txt_pra_horas.Value = "0";
                        txt_pra_cantidad.Value = "0";
                    }

                    /// 4.- Pintar el botón volver
                    btn_back.HRef = "solicitud-practica.aspx?idb=" + idEmpresa;
                }
                else
                    Response.Redirect("empresas.aspx");
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idSolicitud
            long idSolicitud = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"]) : -1;
            long idCompany = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"]) : -1;

            /// 2.- Sacar los datos del formulario            
            DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
            /*DateTime? fecha_baja = null;
            if (!String.IsNullOrEmpty(txtFechaBaja.Value))
                fecha_baja = DateTime.Parse(txtFechaBaja.Value);*/
            int horas = int.Parse(txt_pra_horas.Value);
            decimal cantidad = decimal.Parse(txt_pra_cantidad.Value.Replace(".", ","));
            string descripcion = txt_descripcion.Value;
            string comentarios = txt_comentarios.Value;
            long idContacto = long.Parse(ddlTutorEmpresa.Value);

            /// 3.- Modificar o Insertar
            if (idSolicitud > 0)
            {
                /// 3.1.- Sacar los datos de la solicitud de prácticas
                List<PRA_SOLICITUD_PRACTICA> lst_solicitud = da.getListPracticesRequestsById(idSolicitud);
                if (lst_solicitud.Count == 1)
                {
                    /// 3.2.- Actualizar los datos de la práctica
                    PRA_SOLICITUD_PRACTICA solicitud = lst_solicitud[0];
                    solicitud.idEmpresa = idCompany;
                    solicitud.fecha = fecha_alta;
                    //solicitud.fecha_cierre = fecha_baja;
                    solicitud.num_horas = horas;
                    solicitud.cantidad = cantidad;
                    solicitud.descripcion_puesto = descripcion;
                    solicitud.comentarios = comentarios;
                    solicitud.idContacto = idContacto;

                    bool update_request = da.updateRequest(solicitud);
                    if (update_request)
                        Response.Redirect("solicitud-practica.aspx?idb=" + idCompany);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar los datos de la solicitud de la práctica";
                }
            }
            else
            {
                /// 3.1.- Actualizar los datos de la práctica
                PRA_SOLICITUD_PRACTICA solicitud = new PRA_SOLICITUD_PRACTICA();
                solicitud.idEmpresa = idCompany;
                solicitud.fecha = fecha_alta;
                //solicitud.fecha_cierre = fecha_baja;
                solicitud.num_horas = horas;
                solicitud.cantidad = cantidad;
                solicitud.descripcion_puesto = descripcion;
                solicitud.comentarios = comentarios;
                solicitud.idContacto = idContacto;

                long insert_request = da.insertRequest(solicitud);
                if (insert_request > 0)
                {
                    /// 3.1.- Autoincrementar los datos de contactos de una empresa
                    List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idCompany);
                    if (lst_empresas.Count == 1)
                    {
                        PRA_EMPRESA empresa = lst_empresas[0];
                        empresa.num_solicitudes = empresa.num_solicitudes + 1;

                        bool update_company = da.updateCompany(empresa);
                        if (update_company)
                            Response.Redirect("solicitud-practica.aspx?idb=" + idCompany);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el nº de solicitudes de prácticas de una empresa";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al calcular el nº de solicitudes de prácticas de una empresa";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir los datos de la solicitud de prácticas";
            }
        }

        private void cargar_datos(long idSolicitud)
        {
            /// 1.- Obtener los datos de la solicitud de la BBDD
            List<PRA_SOLICITUD_PRACTICA> lst_solicitudes = da.getListPracticesRequestsById(idSolicitud);
            if (lst_solicitudes.Count == 1)
            {
                txtFechaAlta.Value = lst_solicitudes[0].fecha.ToShortDateString();
                //txtFechaBaja.Value = lst_solicitudes[0].fecha_cierre != null ? lst_solicitudes[0].fecha_cierre.Value.ToShortDateString() : string.Empty;
                txt_pra_horas.Value = lst_solicitudes[0].num_horas.ToString();
                txt_pra_cantidad.Value = lst_solicitudes[0].cantidad.ToString().Replace(",", ".");
                txt_descripcion.Value = lst_solicitudes[0].descripcion_puesto;
                txt_comentarios.Value = lst_solicitudes[0].comentarios;
                ddlTutorEmpresa.Value = lst_solicitudes[0].idContacto.ToString();
            }
        }

        private void cargar_contactos_empresa(long idEmpresa)
        {
            /// 1.- Cargar los tutores de las empresas
            List<PRA_CONTACTO> lst_contacts = da.getContactsByIdCompany(idEmpresa);
            if (lst_contacts.Count > 0)
            {
                List<Usuarios> lst_users = lst_contacts.Select(user => new Usuarios { id_usuario = user.idContacto, nombre_completo = (user.Nombre + " " + user.Apellidos) }).ToList();
                this.ddlTutorEmpresa.DataSource = lst_users;
                this.ddlTutorEmpresa.DataTextField = "nombre_completo";
                this.ddlTutorEmpresa.DataValueField = "id_usuario";
                this.ddlTutorEmpresa.DataBind();
                this.ddlTutorEmpresa.Items.Add(new ListItem("Seleccione", "-1"));
                ddlTutorEmpresa.Value = idEmpresa.ToString();
            }
        }
    }
}