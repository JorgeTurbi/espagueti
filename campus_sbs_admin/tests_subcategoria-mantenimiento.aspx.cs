using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class tests_subcategoria_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;

                /// 3.- Cargar los datos
                if (id > 0)
                {
                    txt_cont_nombre.Value = da.getTest_SubcategoriaById(id).Nombre;
                }                
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            txt_error.InnerHtml = "";

            int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"]) : -1;

            /// 2.- Sacar los datos del formulario
            string nombre = txt_cont_nombre.Value.Trim();

            if (da.ExisteTest_Subcategoria(nombre))
            {
                txt_error.InnerHtml = "Ya existe una subcategoría con ese nombre.";
                return;
            }

            /// 3.- Modificar o Insertar
            if (id > 0)
            {
                try
                {
                    Test_Subcategoria item = da.getTest_SubcategoriaById(id);
                    if (item.Nombre == nombre)
                        Response.Redirect("tests_subcategoria.aspx");

                    if (da.ExisteTest_Subcategoria(nombre))
                    {
                        txt_error.InnerHtml = "Ya existe una Subcategoría con ese nombre.";
                        return;
                    }

                    item.Nombre = nombre;
                    da.updateSubcategoria(item);

                    Response.Redirect("tests_subcategoria.aspx");

                }
                catch (Exception)
                {
                    txt_error.InnerHtml = "Se ha producido un error al actualizar la Subcategoría";
                }
            }
            else
            {
                try
                {
                    if (da.ExisteTest_Subcategoria(nombre))
                    {
                        txt_error.InnerHtml = "Ya existe una Subcategoría con ese nombre.";
                        return;
                    }

                    Test_Subcategoria item = new Test_Subcategoria();
                    item.Nombre = nombre;

                    da.insertSubcategoria(item);

                    Response.Redirect("tests_subcategoria.aspx");

                }
                catch (Exception exc)
                {
                    txt_error.InnerHtml = "Se ha producido un error al insertar la Subcategoría";

                    LogUtils.InsertarLog(" ERROR - tests_subcategoria_mantenimiento.cs::btnGuardar_Click()");
                    LogUtils.InsertarLog("- MSG:" + exc.Message);
                    LogUtils.InsertarLog("- INNEREXC:" + ((exc.InnerException == null) ? "" : exc.InnerException.Message));
                }

            }
            txt_error.InnerHtml = string.Empty;

        }
    }
}