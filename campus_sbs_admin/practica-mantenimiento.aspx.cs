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
    public partial class practica_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa y del contacto
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                long idPractica = !String.IsNullOrEmpty(Request.QueryString["idp"]) ? long.Parse(Request.QueryString["idp"].ToString()) : -1;

                /// 2.- Cargar combos
                cargar_empresas(idEmpresa);
                //cargar_tutores_empresa(idEmpresa);
                cargar_tutores_escuela();
                cargar_cursos();

                /// 3.- Cargar los datos del contacto
                if (idPractica > 0)
                    cargar_datos(idPractica);
                else
                {
                    txtFechaAlta.Value = DateTime.Today.ToShortDateString();
                    cargar_tutores_empresa(idEmpresa);
                }

                /// 4.- Datos del fileupload
                file_anexo.InnerHtml = "<i class='far fa-image fa-2x'></i><span> Pulse o arrastre el fichero del anexo en el área seleccionada</span><input id='fileupload_anexo' type='file' data-url='controls/UploadHandler.ashx' data-form-data='{\"idp\": \"" + idPractica + "\", \"type\": \"file_anexo\", \"accion\": \"update\" }' />";
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el idPractica
            long idPractica = !String.IsNullOrEmpty(Request.QueryString["idp"]) ? long.Parse(Request.QueryString["idp"]) : -1;
            long idCompany = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"]) : -1;

            /// 2.- Sacar los datos del formulario
            long idStudent = long.Parse(idAlumno.Value);
            long idCurso = long.Parse(ddlCurso.SelectedValue);
            long idEmpresa = long.Parse(ddlEmpresa.SelectedValue);
            long idTutorEmpresa = long.Parse(ddlTutorEmpresa.Value);
            long idTutorEscuela = long.Parse(ddlTutorEscuela.SelectedValue);
            DateTime fecha_alta = DateTime.Parse(txtFechaAlta.Value);
            DateTime? fecha_baja = null;
            if (!String.IsNullOrEmpty(txtFechaBaja.Value))
                fecha_baja = DateTime.Parse(txtFechaBaja.Value);
            decimal duracion = decimal.Parse(txt_pra_duracion.Value.Replace(".", ","));
            int ayuda = int.Parse(txt_pra_ayuda.Value);
            int horas = int.Parse(txt_pra_horas.Value);
            string tipo = ddlTipo.SelectedValue;
            string fichero_anexo = txtFicheroAnexo.Value;
            string comentarios = txt_comentarios.Value;
            string factura = txt_pra_factura.Value;
            string pedido = txt_pra_pedido.Value;
            decimal? precio = null;
            if (!String.IsNullOrEmpty(txt_pra_precio.Value))
                precio = decimal.Parse(txt_pra_precio.Value.Replace(".", ","));
            bool matriculado = chkMatriculado.Checked;
            bool activado = chkActivado.Checked;
            string pdp_comentarios = txt_pdp_comentarios.Value;
            bool curriculares = chkCurriculares.Checked;

            /// 3.- Modificar o Insertar
            if (idPractica > 0)
            {
                /// 3.1.- Sacar los datos de la práctica
                List<PRA_PRACTICAS> lst_practicas = da.getListPracticesById(idPractica);
                if (lst_practicas.Count == 1)
                {
                    long idCompanyOld = lst_practicas[0].idEmpresa;

                    /// 3.2.- Comprobar fichero anexo nuevo
                    if (!String.IsNullOrEmpty(fichero_anexo) && fichero_anexo != lst_practicas[0].FicheroAnexo)
                    {
                        /// 3.2.1.- Guardar el fichero en la carpeta correcta
                        string ruta = ConfigurationManager.AppSettings["ruta_practicas"];

                        /// 3.2.1.0.- Eliminar el fichero anterior
                        if (!String.IsNullOrEmpty(lst_practicas[0].FicheroAnexo))
                            File.Delete(ruta + lst_practicas[0].FicheroAnexo);

                        /// 3.2.1.1.- Rutas nuevas
                        string ruta_origen = ruta + "temp\\" + fichero_anexo;
                        string ruta_destino = ruta;

                        /// 3.2.2.- Si no existe el directorio lo creamos.
                        if (!(Directory.Exists(ruta_destino)))
                            Directory.CreateDirectory(ruta_destino);

                        ruta_destino = ruta_destino + fichero_anexo;

                        /// 3.2.3.- Copiar el fichero
                        File.Copy(ruta_origen, ruta_destino, true);

                        /// 3.2.4.- Borramos el fichero de la carpeta origen
                        File.Delete(ruta_origen); //Eliminamos el fichero de la carpeta temporal

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

                    /// 3.3.- Actualizar los datos de la práctica
                    PRA_PRACTICAS practica = lst_practicas[0];
                    practica.idAlumno = idStudent;
                    practica.idCurso = idCurso;
                    practica.idEmpresa = idEmpresa;
                    practica.idTutorEmpresa = idTutorEmpresa;
                    practica.idTutorEscuela = idTutorEscuela;
                    practica.FechaAlta = fecha_alta;
                    practica.FechaBaja = fecha_baja;
                    practica.Duracion = duracion;
                    practica.AyudaEstudioMes = ayuda;
                    practica.HoraSemana = horas;
                    practica.Tipo = tipo;
                    practica.FicheroAnexo = fichero_anexo;
                    practica.Comentarios = comentarios;
                    practica.PDP_NumFactura = factura;
                    practica.PDP_NumPedido = pedido;
                    practica.PDP_Precio = precio;
                    practica.PDP_Matriculado = matriculado;
                    practica.PDP_Activado = activado;
                    practica.PDP_Comentarios = pdp_comentarios;
                    practica.Curriculares = curriculares;

                    bool update_practice = da.updatePractice(practica);
                    if (update_practice)
                    {
                        if (idCompanyOld != idCompany)
                        {
                            bool send = true;

                            /// 3.1.- Autoincrementar los datos de contactos de una empresa
                            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idEmpresa);
                            if (lst_empresas.Count == 1)
                            {
                                PRA_EMPRESA empresa = lst_empresas[0];
                                empresa.num_practicas = empresa.num_practicas + 1;

                                bool update_company = da.updateCompany(empresa);
                                if (!update_company)
                                {
                                    send = false;
                                    txt_error.InnerHtml = "Se ha producido un error al actualizar el nº de prácticas de una empresa";
                                }
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al calcular el nº de contactos de una empresa";

                            /// 3.2.- Restar el nº de practicas a la empresa original
                            List<PRA_EMPRESA> lst_empresas_old = da.getBusinessById(idCompanyOld);
                            if (lst_empresas_old.Count == 1)
                            {
                                PRA_EMPRESA empresa = lst_empresas_old[0];
                                empresa.num_practicas = empresa.num_practicas - 1;

                                bool update_company = da.updateCompany(empresa);
                                if (!update_company)
                                {
                                    send = false;
                                    txt_error.InnerHtml = "Se ha producido un error al actualizar el nº de prácticas de una empresa";
                                }
                            }
                            else
                                txt_error.InnerHtml = "Se ha producido un error al calcular el nº de contactos de una empresa";

                            if (send)
                                Response.Redirect("practicas.aspx" + (idCompany > 0 ? "?idb=" + idCompany : string.Empty));
                        }
                        else
                            Response.Redirect("practicas.aspx" + (idCompany > 0 ? "?idb=" + idCompany : string.Empty));
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar los datos de la práctica";
                }
            }
            else
            {
                /// 3.1.- Comprobar fichero convenio nuevo
                if (!String.IsNullOrEmpty(fichero_anexo))
                {
                    /// 3.1.1.- Guardar el fichero en la carpeta correcta
                    string ruta = ConfigurationManager.AppSettings["ruta_practicas"];

                    string ruta_origen = ruta + "temp\\" + fichero_anexo;
                    string ruta_destino = ruta;

                    /// 3.1.2.- Si no existe el directorio lo creamos.
                    if (!(Directory.Exists(ruta_destino)))
                        Directory.CreateDirectory(ruta_destino);

                    ruta_destino = ruta_destino + fichero_anexo;

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
                        foreach(var file in Directory.GetFiles(ruta + "temp\\"))
                        {
                            File.Delete(file);
                        }

                        if ((Directory.Exists(ruta + "temp\\")))
                            Directory.Delete(ruta + "temp\\");
                    }
                }

                /// 3.2.- Añadir los datos de la práctica
                PRA_PRACTICAS practica = new PRA_PRACTICAS();
                practica.idAlumno = idStudent;
                practica.idCurso = idCurso;
                practica.idEmpresa = idEmpresa;
                practica.idTutorEmpresa = idTutorEmpresa;
                practica.idTutorEscuela = idTutorEscuela;
                practica.FechaAlta = fecha_alta;
                practica.FechaBaja = fecha_baja;
                practica.Duracion = duracion;
                practica.AyudaEstudioMes = ayuda;
                practica.HoraSemana = horas;
                practica.Tipo = tipo;
                practica.FicheroAnexo = fichero_anexo;
                practica.Comentarios = comentarios;
                practica.PDP_NumFactura = factura;
                practica.PDP_NumPedido = pedido;
                practica.PDP_Precio = precio;
                practica.PDP_Matriculado = matriculado;
                practica.PDP_Activado = activado;
                practica.PDP_Comentarios = pdp_comentarios;
                practica.Curriculares = curriculares;

                long insert_practice = da.insertPractice(practica);
                if (insert_practice > 0)
                {
                    /// 3.1.- Autoincrementar los datos de contactos de una empresa
                    List<PRA_EMPRESA> lst_empresas = da.getBusinessById(idEmpresa);
                    if (lst_empresas.Count == 1)
                    {
                        PRA_EMPRESA empresa = lst_empresas[0];
                        empresa.num_practicas = empresa.num_practicas + 1;

                        bool update_company = da.updateCompany(empresa);
                        if (update_company)
                        {
                            if (idCompany > 0)
                                Response.Redirect("empresas.aspx");
                            else
                                Response.Redirect("practicas.aspx");
                        }
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el nº de prácticas de una empresa";
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al calcular el nº de contactos de una empresa";
                }
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir los datos de la práctica";
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

        [WebMethod(Description = "Busca Tutor de empresa a partir de un id de empresa")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Usuarios> cargarTutoresEmpresaWS(long idEmpresa)
        {
            DataAccess da = new DataAccess();

            List<Usuarios> list_users = new List<Usuarios>();
            List<PRA_CONTACTO> lst_contacts = da.getContactsByIdCompany(idEmpresa);
            if (lst_contacts.Count > 0)
                list_users = lst_contacts.Select(user => new Usuarios { id_usuario = user.idContacto, nombre_completo = user.Nombre + " " + user.Apellidos }).ToList();
            return list_users;
        }
        
        private void cargar_datos(long idPractica)
        {
            /// 1.- Obtener los datos de la práctica de la BBDD
            List<PRA_PRACTICAS> lst_practicas = da.getListPracticesById(idPractica);
            if (lst_practicas.Count == 1)
            {
                List<CLIENTES> lst_user = da.getUserById(lst_practicas[0].idAlumno);
                if (lst_user.Count == 1)
                    txt_prac_alumno.Value = lst_user[0].Nombre_Completo;
                else
                    txt_prac_alumno.Value = lst_practicas[0].idAlumno.ToString();
                idAlumno.Value = lst_practicas[0].idAlumno.ToString();

                ddlCurso.SelectedValue = lst_practicas[0].idCurso.ToString();
                ddlEmpresa.SelectedValue = lst_practicas[0].idEmpresa.ToString();
                
                cargar_tutores_empresa(lst_practicas[0].idEmpresa);

                ddlTutorEmpresa.Value = lst_practicas[0].idTutorEmpresa.ToString();
                ddlTutorEscuela.SelectedValue = lst_practicas[0].idTutorEscuela.ToString();
                id_Tutor_Empresa.Value = lst_practicas[0].idTutorEmpresa.ToString();

                txtFechaAlta.Value = lst_practicas[0].FechaAlta.ToShortDateString();
                txtFechaBaja.Value = lst_practicas[0].FechaBaja != null ? lst_practicas[0].FechaBaja.Value.ToShortDateString() : string.Empty;
                txt_pra_duracion.Value = lst_practicas[0].Duracion.ToString();
                txt_pra_ayuda.Value = lst_practicas[0].AyudaEstudioMes.ToString();
                txt_pra_horas.Value = lst_practicas[0].HoraSemana.ToString();
                ddlTipo.SelectedValue = lst_practicas[0].Tipo;
                txtFicheroAnexo.Value = lst_practicas[0].FicheroAnexo;
                if (!String.IsNullOrEmpty(lst_practicas[0].FicheroAnexo))
                    txt_file_anexo.HRef = ConfigurationManager.AppSettings["url_practicas"] + lst_practicas[0].FicheroAnexo;
                else
                    txt_file_anexo.Attributes["class"] = "hidden";

                txt_comentarios.Value = lst_practicas[0].Comentarios;
                txt_pra_factura.Value = lst_practicas[0].PDP_NumFactura;
                txt_pra_pedido.Value = lst_practicas[0].PDP_NumPedido;
                txt_pra_precio.Value = lst_practicas[0].PDP_Precio != null ? lst_practicas[0].PDP_Precio.Value.ToString() : string.Empty;
                chkMatriculado.Checked = lst_practicas[0].PDP_Matriculado != null ? lst_practicas[0].PDP_Matriculado.Value : false;
                chkActivado.Checked = lst_practicas[0].PDP_Activado != null ? lst_practicas[0].PDP_Activado.Value : false;
                txt_pdp_comentarios.Value = lst_practicas[0].PDP_Comentarios;

                chkCurriculares.Checked = lst_practicas[0].Curriculares != null ? lst_practicas[0].Curriculares.Value : false;
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

        private void cargar_tutores_empresa(long idEmpresa)
        {
            /// 1.- Cargar los tutores de las empresas
            List<PRA_CONTACTO> lst_contacts = da.getContactsByIdCompany(-1);
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

            
            /*/// 1.- Cargar los tutores de las empresas
            List<PRA_CONTACTO> lst_contacts = da.getContactsByIdCompany(-1);
            if (lst_contacts.Count > 0)
            {
                this.ddlTutorEmpresa.Items.Add(new ListItem("Seleccione", "-1"));

                if (idEmpresa > 0)
                {
                    List<PRA_CONTACTO> lst_contacts_filter = lst_contacts.Where(c => c.idEmpresa == idEmpresa).ToList();
                    List<Usuarios> lst_users = lst_contacts_filter.Select(user => new Usuarios { id_usuario = user.idContacto, nombre_completo = (user.Nombre + " " + user.Apellidos) }).ToList();
                    foreach (var user in lst_users)
                    {
                        this.ddlTutorEmpresa.Items.Add(new ListItem(user.nombre_completo, user.id_usuario.ToString()));
                    }
                }

                List<PRA_CONTACTO> lst_contacts_filters = lst_contacts.Where(c => c.idEmpresa != idEmpresa).ToList();
                List<Usuarios> lst_users_filter = lst_contacts_filters.Select(user => new Usuarios { id_usuario = user.idContacto, nombre_completo = (user.Nombre + " " + user.Apellidos) }).ToList();
                foreach (var user in lst_users_filter)
                {
                    ListItem list_item = new ListItem(user.nombre_completo, user.id_usuario.ToString());
                    //list_item.Attributes.Add("class", "hidden");
                    list_item.Attributes.Add("style", "display:none");

                    this.ddlTutorEmpresa.Items.Add(list_item);
                }

                //ddlTutorEmpresa.SelectedValue = idEmpresa.ToString();
                ddlTutorEmpresa.Value = idEmpresa.ToString();
            }*/
        }

        private void cargar_tutores_escuela()
        {
            List<CLIENTES> lst_users = da.getUser("1").Where(c => c.Administrador == ((int)Constantes.activo.Activo).ToString()).ToList();
            if (lst_users.Count > 0)
            {
                this.ddlTutorEscuela.DataSource = lst_users;
                this.ddlTutorEscuela.DataTextField = "Nombre_Completo";
                this.ddlTutorEscuela.DataValueField = "id_cliente";
                this.ddlTutorEscuela.DataBind();
                this.ddlTutorEscuela.Items.Add(new ListItem("Seleccione", "-1"));
                this.ddlTutorEscuela.SelectedValue = ConfigurationManager.AppSettings["tutor_escuela"];
            }
        }

        private void cargar_cursos()
        {
            /// 1.- Cargar los cursos
            List<campus_CURSO> lst_courses = da.getCourses(null).OrderBy(c => c.Nombre).ToList();
            if (lst_courses.Count > 0)
            {
                this.ddlCurso.DataSource = lst_courses;
                this.ddlCurso.DataTextField = "Nombre";
                this.ddlCurso.DataValueField = "ID_Curso";
                this.ddlCurso.DataBind();
                this.ddlCurso.Items.Add(new ListItem("Seleccione", "-1"));
                this.ddlCurso.SelectedValue = "-1";
            }
        }
    }
}