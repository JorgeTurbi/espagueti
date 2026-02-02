using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class peticion_info : System.Web.UI.Page
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
                bool comprobate_user = Utilities.comprobate_users(list_user[0]);
                if (!comprobate_user)
                    Response.Redirect("login.aspx");
                else
                {
                    /// 0.- Cargar los paises
                    cargar_combos();

                    /// 1.- Sacar los datos del usuario
                    long idUsuario = !String.IsNullOrEmpty(Request.QueryString["idu"]) ? long.Parse(Request.QueryString["idu"].ToString()) : -1;
                    long idPeticion = !String.IsNullOrEmpty(Request.QueryString["idap"]) ? long.Parse(Request.QueryString["idap"].ToString()) : -1;
                    if (idUsuario > 0 && idPeticion > 0)
                        cargar_datos(idUsuario, idPeticion);
                    else
                        txtFechaLead.Value = DateTime.Today.ToShortDateString();

                    /// 2.- Botón de volver
                    btn_back.HRef = "ficha-usuario.aspx?idu=" + idUsuario;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        private void cargar_combos()
        {
            /// 1.- Cargar los origenes
            List<campus_AUX> _origenes = da.getAuxiliars(Constantes.aux.origen.GetStringValue());
            if (_origenes.Count > 0)
            {
                ddlOrigen.DataSource = _origenes;
                ddlOrigen.DataTextField = "Nombre";
                ddlOrigen.DataValueField = "ID_Aux";
                ddlOrigen.DataBind();
                ddlOrigen.Items.Insert(0, new ListItem("Seleccione un origen", "-1"));
                ddlOrigen.Value = "-1";
            }

            /// 2.- Cargar los cursos
            List<campus_CURSO> _cursos = da.getAllCourses();
            if (_cursos.Count > 0)
            {
                /// Ordenar los cursos
                _cursos = _cursos.OrderBy(_ => _.idTipo_Curso == (int)Constantes.tipo_curso.Master_presencial).ToList();
            }
        }
        private void cargar_datos(long idUsuario, long idPeticion)
        {
            /*Accion_Persona ap = new Accion_Persona(idPeticion);
            ddlIdioma.Value = ap.entAP.Idioma.ToString();
            ddlNivel.Value = ap.entAP.Nivel.ToString();
            txt_frecuencia.Value = ap.entAP.Frecuencia_Clases;
            txt_comentarios.Value = ap.entAP.Comentarios;
            txtFechaInicio.Value = ap.entAP.Fecha.ToShortDateString();
            ddlOrigen.Value = ap.entAP.IdOrigen.ToString();*/
        }
    }
}