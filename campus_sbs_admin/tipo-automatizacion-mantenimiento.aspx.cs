using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class tipo_automatizacion_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Sacar los datos del tipo de automatización
                int idType = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;
                if (idType > 0)
                    cargar_datos(idType);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar los datos del tipo de automatización
            int idType = !String.IsNullOrEmpty(Request.QueryString["idt"]) ? int.Parse(Request.QueryString["idt"].ToString()) : -1;

            /// 2.- Sacar los datos del formulario
            string nombre_tipo = txt_name.Value;

            if (String.IsNullOrEmpty(nombre_tipo))
            {
                name_form.Attributes["class"] = name_form.Attributes["class"].Replace("has-error", string.Empty);
                name_form.Attributes["class"] = name_form.Attributes["class"].Insert(name_form.Attributes["class"].Length, " has-error");
                
                txt_error.InnerHtml = "El campo marcado es obligatorio";
            }
            else
            {
                /// 3.- Modificar o Insertar
                if (idType > 0)
                {
                    /// 3.1.- Sacar los datos del tipo
                    List<campus_TIPO_AUTOMATIZACION> lst_type = da.getTypeAutomation(idType);
                    if (lst_type.Count == 1)
                    {
                        /// 3.2.- Actualizar los datos del tipo
                        campus_TIPO_AUTOMATIZACION type_Automation = lst_type[0];
                        type_Automation.nombre = nombre_tipo;

                        bool update_type = da.updateTypeAutomation(type_Automation);
                        if (update_type)
                            Response.Redirect("lista-tipos-automatizacion.aspx");
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar el tipo de automatización";
                    }
                }
                else
                {
                    /// 3.1.- Añadir un nuevo tipo de automatización
                    campus_TIPO_AUTOMATIZACION type_Automation = new campus_TIPO_AUTOMATIZACION();
                    type_Automation.nombre = nombre_tipo;

                    long insert_type = da.insertTypeAutomation(type_Automation);
                    if (insert_type > 0)
                        Response.Redirect("lista-tipos-automatizacion.aspx");
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir el tipo de automatización";
                }
            }
        }

        private void cargar_datos(int idType)
        {
            /// 1.- Obtener los datos de la empresa de la BBDD
            List<campus_TIPO_AUTOMATIZACION> lst_type = da.getTypeAutomation(idType);
            if (lst_type.Count == 1)
                txt_name.Value = lst_type[0].nombre;
        }
    }
}