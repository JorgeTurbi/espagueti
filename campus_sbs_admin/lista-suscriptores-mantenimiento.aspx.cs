using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_suscriptores_mantenimiento : System.Web.UI.Page
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
                    /// 0.- Cargar el combo con los usuarios especiales
                    cargar_combo(list_user[0]);

                    /// 1.- Sacar los datos de la empresa y del contacto
                    long idListado = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"].ToString()) : -1;
                    if (idListado > 0)
                        /// 2.- Cargar los datos del listado
                        cargar_datos(idListado);

                    /// 3.- Pintar el botón volver
                    btn_back.HRef = "lista-suscriptores.aspx";
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 0.- Sacar los datos del formulario
            string name_list = txt_name_list.Value;
            bool check_auto = chkAuto.Checked;
            string sql_list = txt_sql.Value;
            long? idUsuarioEspecial = null;
            if (!String.IsNullOrEmpty(ddlUsuarioEspecial.SelectedValue))
                idUsuarioEspecial = long.Parse(ddlUsuarioEspecial.SelectedValue);

            /// 1.- Comprobar si viene la lista
            long idListado = !String.IsNullOrEmpty(Request.QueryString["idl"]) ? long.Parse(Request.QueryString["idl"].ToString()) : -1;
            if (idListado > 0)
            {
                /// 2.- Actualizar la lista
                List<EMAIL_LISTADO_SUSCRIPCIONES> lst = da.getEmailListSubscriptionsById(idListado);
                if (lst.Count == 1)
                {
                    /// 2.1.- Actualizar los datos de la lista
                    EMAIL_LISTADO_SUSCRIPCIONES suscripcion = lst[0];
                    suscripcion.nombre = name_list;
                    suscripcion.auto = check_auto;
                    suscripcion.sql = sql_list;
                    suscripcion.id_usuario = idUsuarioEspecial;

                    bool update_list = da.updateEmailListSubscription(suscripcion);
                    if (update_list)
                        Response.Redirect("lista-suscriptores.aspx");
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar los datos de la lista";
                }
            }
            else
            {
                /// 3.- Comprobar si existe la lista 
                List<EMAIL_LISTADO_SUSCRIPCIONES> listado = da.getEmailListSubscriptionsByName(name_list);
                if (listado.Count == 0)
                {
                    /// 3.1.- Añadir la lista
                    EMAIL_LISTADO_SUSCRIPCIONES suscripcion = new EMAIL_LISTADO_SUSCRIPCIONES();
                    suscripcion.nombre = name_list;
                    suscripcion.fecha_alta = DateTime.Today;
                    suscripcion.num_total = 0;
                    suscripcion.num_actual = 0;
                    suscripcion.num_bajas = 0;
                    suscripcion.auto = check_auto;
                    suscripcion.sql = sql_list;
                    suscripcion.id_usuario = idUsuarioEspecial;

                    /// 3.2.- Comprobar si es un usuario especial
                    /*List<CLIENTES> list_user = new List<CLIENTES>();
                    if (list_user.Count == 0 && Session["usuario"] != null)
                        list_user.Add((CLIENTES)Session["usuario"]);

                    List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                    if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                        suscripcion.id_usuario = list_user[0].id_cliente;*/

                    long insert_list = da.insertEmailListSubscription(suscripcion);
                    if (insert_list > 0)
                        Response.Redirect("suscriptores-mantenimiento.aspx?idl=" + insert_list);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir la lista";
                }
                else
                    txt_error.InnerHtml = "Ya existe una lista de suscriptores con ese nombre";
            }
        }

        [WebMethod(Description = "Busca nombres de listas")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Listado_Suscripciones> search_list(string name)
        {
            DataAccess da = new DataAccess();

            List<Listado_Suscripciones> listado = new List<Listado_Suscripciones>();
            List<EMAIL_LISTADO_SUSCRIPCIONES> list = da.getEmailListSubscriptionsBySearch(name);
            if (list.Count > 0)
                listado = list.Select(suscripcion => new Listado_Suscripciones { nombre = suscripcion.nombre, id_els = suscripcion.id_els }).ToList();
            return listado;
        }

        private void cargar_combo(CLIENTES user)
        {
            /// 1.- Listado de usuarios especiales
            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
            List<long> _users_specials = new List<long>();
            foreach (var _id_user_special in list_users_mails)
            {
                _users_specials.Add(long.Parse(_id_user_special));
            }
            if (list_users_mails.Contains(user.id_cliente.ToString()))
                _users_specials = _users_specials.Where(_ => _ == user.id_cliente).ToList();
            List<CLIENTES> _usuarios_especiales = da.getUserByList(_users_specials);
            if (_usuarios_especiales.Count > 0)
            {
                ddlUsuarioEspecial.DataSource = _usuarios_especiales.OrderBy(_ => _.Nombre_Completo).ToList();
                ddlUsuarioEspecial.DataTextField = "Nombre_Completo";
                ddlUsuarioEspecial.DataValueField = "id_cliente";
                ddlUsuarioEspecial.DataBind();

                if (_usuarios_especiales.Count > 1)
                {
                    ddlUsuarioEspecial.Items.Add(new ListItem("Seleccione", string.Empty));
                    ddlUsuarioEspecial.SelectedValue = string.Empty;
                }
            }
        }
        private void cargar_datos(long idListado)
        {
            List<EMAIL_LISTADO_SUSCRIPCIONES> lst = da.getEmailListSubscriptionsById(idListado);
            if (lst.Count == 1)
            {
                txt_name_list.Value = lst[0].nombre;
                chkAuto.Checked = lst[0].auto != null ? lst[0].auto.Value : false;
                txt_sql.Value = lst[0].sql;

                if (lst[0].id_usuario.HasValue)
                    ddlUsuarioEspecial.SelectedValue = lst[0].id_usuario.ToString();                
            }
        }
    }
}