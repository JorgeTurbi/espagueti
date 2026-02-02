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
    public partial class candidatos_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos de la empresa y del contacto
                long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
                long idSolicitud = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;
                long idCandidato = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;

                if (idEmpresa > 0 && idSolicitud > 0)
                {
                    /// 2.- Cargar los datos del candidato
                    if (idCandidato > 0)
                        cargar_datos(idCandidato);

                    /// 3.- Pintar el botón volver
                    btn_back.HRef = "candidatos.aspx?idb=" + idEmpresa + "&ids=" + idSolicitud;
                }
                else
                    Response.Redirect("empresas.aspx");
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar datos de la url
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
            long idSolicitud = !String.IsNullOrEmpty(Request.QueryString["ids"]) ? long.Parse(Request.QueryString["ids"].ToString()) : -1;
            long idCandidato = !String.IsNullOrEmpty(Request.QueryString["idc"]) ? long.Parse(Request.QueryString["idc"].ToString()) : -1;

            /// 2.- Sacar los datos del formulario
            long idStudent = long.Parse(idAlumno.Value);
            string comentarios = txt_comentarios.Value;

            /// 3.- Modificar o Insertar
            if (idCandidato > 0)
            {
                /// 3.1.- Obtener los datos del candidato de la BBDD
                List<PRA_CANDIDATOS> lst_candidatos = da.getCandidatesById(idCandidato);
                if (lst_candidatos.Count == 1)
                {
                    /// 3.2.- Actualizar los datos del candidato
                    PRA_CANDIDATOS candidato = lst_candidatos[0];
                    candidato.idPersona = idStudent;
                    candidato.idSolicitud = idSolicitud;
                    candidato.comentarios = comentarios;

                    bool update_candidate = da.updateCandidate(candidato);
                    if (update_candidate)
                        Response.Redirect("candidatos.aspx?idb=" + idEmpresa + "&ids=" + idSolicitud);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar los datos del candidato";
                }
            }
            else
            {
                /// 3.1.- Añadir los datos del candidato
                PRA_CANDIDATOS candidato = new PRA_CANDIDATOS();
                candidato.idPersona = idStudent;
                candidato.idSolicitud = idSolicitud;
                candidato.comentarios = comentarios;
                candidato.fecha = DateTime.Today;

                long insert_candidate = da.insertCandidate(candidato);
                if (insert_candidate > 0)
                    Response.Redirect("candidatos.aspx?idb=" + idEmpresa + "&ids=" + idSolicitud);
                else
                    txt_error.InnerHtml = "Se ha producido un error al añadir los datos del candidato";
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

        private void cargar_datos(long idCandidato)
        {
            /// 1.- Obtener los datos del candidato de la BBDD
            List<PRA_CANDIDATOS> lst_candidatos = da.getCandidatesById(idCandidato);
            if (lst_candidatos.Count == 1)
            {
                List<CLIENTES> lst_user = da.getUserById(lst_candidatos[0].idPersona);
                if (lst_user.Count == 1)
                    txt_prac_alumno.Value = lst_user[0].Nombre_Completo;
                else
                    txt_prac_alumno.Value = lst_candidatos[0].idPersona.ToString();
                idAlumno.Value = lst_candidatos[0].idPersona.ToString();

                txt_comentarios.Value = lst_candidatos[0].comentarios;
            }
        }
    }
}