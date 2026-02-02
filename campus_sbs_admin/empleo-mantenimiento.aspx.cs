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
    public partial class empleo_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa y del contacto
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                long idEmpleo = !String.IsNullOrEmpty(Request.QueryString["ide"]) ? long.Parse(Request.QueryString["ide"].ToString()) : -1;

                /// 2.- Cargar combos
                cargar_empresas(idEmpresa);

                /// 3.- Cargar los datos del contacto
                if (idEmpleo > 0)
                    cargar_datos(idEmpleo);
                else
                    txtFechaAlta.Value = DateTime.Today.ToShortDateString();

                /// 4.- Pintar el botón volver
                btn_back.HRef = "empleos.aspx" + (idEmpresa > 0 ? "?idb=" + idEmpresa : string.Empty);
            }
        }        

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idPractica
            long idEmpleo = !String.IsNullOrEmpty(Request.QueryString["ide"]) ? long.Parse(Request.QueryString["ide"]) : -1;
            long idCompany = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"]) : -1;

            /// 2.- Sacar los datos del formulario
            long idStudent = long.Parse(idAlumno.Value);
            long idEmpresa = long.Parse(ddlEmpresa.SelectedValue);
            DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
            string contrato = txt_emp_contrato.Value;
            string comentarios = txt_comentarios.Value;

            /// 3.- Modificar o Insertar
            if (idEmpleo > 0)
            {
                /// 3.1.- Sacar los datos del empleo
                List<PRA_EMPLEO> lst_empleos = da.getWorksById(idEmpleo);
                if (lst_empleos.Count == 1)
                {
                    /// 3.2.- Actualizar los datos del empleo
                    PRA_EMPLEO empleo = lst_empleos[0];
                    empleo.idAlumno = idStudent;
                    empleo.idEmpresa = idEmpresa;
                    empleo.FechaAlta = fecha_alta;
                    empleo.Contrato = contrato;
                    empleo.Comentarios = comentarios;

                    bool update_job = da.updateJob(empleo);
                    if (update_job)
                        Response.Redirect("empleos.aspx" + (idCompany > 0 ? "?idb=" + idCompany : string.Empty));
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar los datos del empleo";
                }
            }
            else
            {
                /// 3.1.- Añadir los datos del empleo
                PRA_EMPLEO empleo = new PRA_EMPLEO();
                empleo.idAlumno = idStudent;
                empleo.idEmpresa = idEmpresa;
                empleo.FechaAlta = fecha_alta;
                empleo.Contrato = contrato;
                empleo.Comentarios = comentarios;

                long insert_job = da.insertJob(empleo);
                if (insert_job > 0)
                {
                    /// 3.1.- Autoincrementar los datos de contactos de una empresa
                    List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idEmpresa);
                    if (lst_empresas.Count == 1)
                    {
                        PRA_EMPRESA empresa = lst_empresas[0];
                        empresa.num_empleos = empresa.num_empleos + 1;

                        bool update_company = da.updateCompany(empresa);
                        if (update_company)
                            Response.Redirect("empleos.aspx" + (idCompany > 0 ? "?idb=" + idCompany : string.Empty));
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el nº de empleos de una empresa";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al calcular el nº de empleos de una empresa";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir los datos del empleo";
            }
        }

        [WebMethod(Description = "Busca alumnos a partir de un texto dado")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> search_student(string name)
        {
            DataAccess da = new DataAccess();

            List<Usuarios> list_users = new List<Usuarios>();
            List<CLIENTES> lst_users = da.getUserBySearch(name);
            if (lst_users.Count > 0)
                list_users = lst_users.Select(user => new Usuarios { id_usuario = user.id_cliente, nombre_completo = user.Nombre_Completo }).ToList();
            return list_users;
        }

        private void cargar_datos(long idEmpleo)
        {
            /// 1.- Obtener los datos del empleo de la BBDD
            List<PRA_EMPLEO> lst_empleos = da.getWorksById(idEmpleo);
            if (lst_empleos.Count == 1)
            {
                List<CLIENTES> lst_user = da.getUserById(lst_empleos[0].idAlumno);
                if (lst_user.Count == 1)
                    txt_prac_alumno.Value = lst_user[0].Nombre_Completo;
                else
                    txt_prac_alumno.Value = lst_empleos[0].idAlumno.ToString();
                idAlumno.Value = lst_empleos[0].idAlumno.ToString();

                ddlEmpresa.SelectedValue = lst_empleos[0].idEmpresa.ToString();
                txtFechaAlta.Value = lst_empleos[0].FechaAlta.ToShortDateString();
                txt_emp_contrato.Value = lst_empleos[0].Contrato;
                txt_comentarios.Value = lst_empleos[0].Comentarios;
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