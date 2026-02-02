using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class tests_categoria_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;

                /// 3.- Cargar los datos del contacto
                if (id > 0)
                {
                    txt_cont_nombre.Value = da.getTestCategoriaById(id).Nombre;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                txt_error.InnerHtml = "";

                int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"]) : -1;

                /// 2.- Sacar los datos del formulario
                string nombre = txt_cont_nombre.Value.Trim();

                /// 3.- Modificar o Insertar
                if (id > 0)
                {

                    Test_Categoria item = da.getTestCategoriaById(id);
                    if (item.Nombre == nombre)
                        Response.Redirect("tests_categoria.aspx");

                    if (da.ExisteTest_Categoria(nombre))
                    {
                        txt_error.InnerHtml = "Ya existe una Categoría con ese nombre.";
                        return;
                    }

                    item.Nombre = nombre;
                    da.updateTest_Categoria(item);

                    Response.Redirect("tests_categoria.aspx");

                }
                else
                {

                    if (da.ExisteTest_Categoria(nombre))
                    {
                        txt_error.InnerHtml = "Ya existe una Categoría con ese nombre.";
                        return;
                    }

                    Test_Categoria item = new Test_Categoria();
                    item.Nombre = nombre;

                    da.insertTest_Categoria(item);

                    Response.Redirect("tests_categoria.aspx");


                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al gurdar la Categoría');</script>");

                LogUtils.InsertarLog(" ERROR - tests_categoria-mantenimiento.cs::btnBorrar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }
    }
}