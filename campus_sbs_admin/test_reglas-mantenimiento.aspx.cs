using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class test_reglas_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCombos();
                int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;
                short orden = !String.IsNullOrEmpty(Request.QueryString["orden"]) ? short.Parse(Request.QueryString["orden"].ToString()) : (short)-1;

                a_volver.HRef = $"test-reglas.aspx?id={id}";
                if (orden > 0)
                {
                    Test_Test_Reglas regla = da.getTest_Test_ReglasById(id, orden);

                    txt_orden.Value = regla.Orden.ToString();
                    ddlCurso.SelectedValue = regla.Id_Curso.ToString();
                    ddlCategoria.SelectedValue = regla.Id_Categoria.ToString();
                    ddlSubcategoria.SelectedValue = regla.Id_Subcategoria.ToString();
                    txt_fecha_creacion.Value = regla.Fecha_Creacion.ToShortDateString();
                    txt_fecha_ult_mod.Value = regla.Fecha_Ult_Modificacion.ToShortDateString();
                }
                else
                {
                    txt_fecha_creacion.Value = DateTime.Now.ToShortDateString();
                    txt_fecha_ult_mod.Value = DateTime.Now.ToShortDateString();
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCurso.SelectedValue == "0")
                {
                    txt_error.InnerHtml = "Seleccione un Curso";
                    return;
                }
                if (ddlCategoria.SelectedValue == "0")
                {
                    txt_error.InnerHtml = "Seleccione una Categoría";
                    return;
                }
                if (ddlSubcategoria.SelectedValue == "0")
                {
                    txt_error.InnerHtml = "Seleccione una Subcategoría";
                    return;
                }

                int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"]) : -1;
                short orden = !String.IsNullOrEmpty(Request.QueryString["orden"]) ? short.Parse(Request.QueryString["orden"].ToString()) : (short)-1;

                if (orden > 0)
                {
                    if (short.Parse(txt_orden.Value) != orden && da.Exists_Regla(id, short.Parse(txt_orden.Value)))
                    {
                        txt_error.InnerHtml = "Ya existe una Regla con ese orden dentro de este Test";
                        return;
                    }

                    Test_Test_Reglas currentregla = da.getTest_Test_ReglasById(id, orden);
                    Test_Test_Reglas regla = new Test_Test_Reglas();
                    regla.Id_Test = id;
                    regla.Orden = short.Parse(txt_orden.Value);
                    regla.Id_Curso = long.Parse(ddlCurso.SelectedValue);
                    regla.Id_Categoria = int.Parse(ddlCategoria.SelectedValue);
                    regla.Id_Subcategoria = int.Parse(ddlSubcategoria.SelectedValue);
                    regla.Fecha_Creacion = currentregla.Fecha_Creacion;
                    regla.Fecha_Ult_Modificacion = DateTime.Now;
                    da.Delete_Test_Regla(currentregla.Id_Test, currentregla.Orden);
                    da.insertTest_Test_Reglas(regla);
                }
                else
                {
                    if (da.Exists_Regla(id, short.Parse(txt_orden.Value)))
                    {
                        txt_error.InnerHtml = "Ya existe una Regla con ese orden dentro de este Test";
                        return;
                    }
                    Test_Test_Reglas regla = new Test_Test_Reglas();
                    regla.Id_Test = id;
                    regla.Orden = short.Parse(txt_orden.Value);
                    regla.Id_Curso = long.Parse(ddlCurso.SelectedValue);
                    regla.Id_Categoria = int.Parse(ddlCategoria.SelectedValue);
                    regla.Id_Subcategoria = int.Parse(ddlSubcategoria.SelectedValue);

                    regla.Fecha_Creacion = DateTime.Now;
                    regla.Fecha_Ult_Modificacion = DateTime.Now;
                    da.insertTest_Test_Reglas(regla);

                }
                Response.Redirect($"test-reglas.aspx?id={id}");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar la Regla');</script>");

                LogUtils.InsertarLog(" ERROR - tests_reglas-mantenimiento.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }

        protected void loadCombos()
        {
            try
            {
                List<campus_CURSO> lst_cursos = new List<campus_CURSO>();
                lst_cursos.Add(new campus_CURSO { ID_Curso = 0, Nombre = "Seleccione" });
                lst_cursos.AddRange(da.getActiveCourses());
                if (lst_cursos.Count > 0)
                {
                    this.ddlCurso.DataSource = lst_cursos;
                    this.ddlCurso.DataTextField = "Nombre";
                    this.ddlCurso.DataValueField = "ID_Curso";
                    this.ddlCurso.DataBind();
                    this.ddlCurso.SelectedValue = "0";

                }

                List<Test_Categoria> lst_cat = new List<Test_Categoria>();
                lst_cat.Add(new Test_Categoria { Id = 0, Nombre = "Seleccione" });
                lst_cat.AddRange(da.getTest_Categorias());
                if (lst_cat.Count > 0)
                {
                    this.ddlCategoria.DataSource = lst_cat;
                    this.ddlCategoria.DataTextField = "Nombre";
                    this.ddlCategoria.DataValueField = "Id";
                    this.ddlCategoria.DataBind();
                    this.ddlCategoria.SelectedValue = "0";
                }

                List<Test_Subcategoria> lst_subcat = new List<Test_Subcategoria>();
                lst_subcat.Add(new Test_Subcategoria { Id = 0, Nombre = "Seleccione" });
                lst_subcat.AddRange(da.getTest_Subcategorias());
                if (lst_subcat.Count > 0)
                {
                    this.ddlSubcategoria.DataSource = lst_subcat;
                    this.ddlSubcategoria.DataTextField = "Nombre";
                    this.ddlSubcategoria.DataValueField = "Id";
                    this.ddlSubcategoria.DataBind();
                    this.ddlSubcategoria.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al recuperar cursos, categorías o subcategorías');</script>");

                LogUtils.InsertarLog(" ERROR - tests_subcategoria.cs::loadCombos()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }
    }
}