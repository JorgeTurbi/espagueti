using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class test_test_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private int _id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCombos();
                _id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;

                if (_id > 0)
                {
                    Test_Test test = da.getTest_TestById(_id);

                    txt_nombre.Value = test.Nombre;
                    txt_normas.Value = test.Normas;
                    txt_comentarios_iniciales.Value = test.Comentarios_Iniciales;
                    txt_comentarios_interno.Value = test.Comentarios_Internos;
                    txt_comentarios_finales.Value = test.Comentarios_Finales;
                    txt_tiempo.Value = test.Tiempo.ToString();
                    txt_nopreguntas.Value = test.Num_Preguntas.ToString();
                    txt_acierto_suma.Value = test.Acierto_Suma.ToString();
                    txt_error_resta.Value = test.Error_Resta.ToString("0.##");
                    txt_apto_puntos.Value = test.Apto_Puntos.ToString();
                    txt_dificultad.Value = test.Nivel_Dificultad.ToString();
                    ddlCurso.Value = test.Id_Curso != null ? test.Id_Curso.ToString() : "0";
                    txt_nointentos.Value = test.Num_Intentos.ToString();
                    chk_hacerlo_en_partes.Checked = test.Hacerlo_En_Partes;
                    chk_interpretado.Checked = test.Interpretado;
                    ddlClienteTest.Value = test.idClienteTest != null ? test.idClienteTest.ToString() : string.Empty;
                    chk_abierto.Checked = test.Abierto;

                    txt_descripcion_publica.Value = test.Descripcion_Publica;
                    txtMetaTitle.Value = test.Meta_Title;
                    txtMetaKeywords.Value = test.Meta_Keywords;
                    txtMetaUrl.Value = test.Meta_Url;
                    txtMetaAuthor.Value = test.Meta_Author;
                    txtMetaDescripcion.Value = test.Meta_Description;

                    txt_fecha_creacion.Value = test.Fecha_Creacion.ToShortDateString();
                    txt_fecha_ult_mod.Value = test.Fecha_Ult_Modificacion.ToShortDateString();

                    if (da.Test_Realizado(!String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1))
                    {
                        txt_nopreguntas.Disabled = true;
                        txt_acierto_suma.Disabled = true;
                        txt_error_resta.Disabled = true;
                        txt_apto_puntos.Disabled = true;
                        alert_realizado.Visible = true;
                    }
                    else
                        alert_realizado.Visible = false;
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
                txt_error.InnerText = string.Empty;

                int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"]) : -1;

                if (da.ExistsTest_Test(id,txt_nombre.Value.Trim(), long.Parse(ddlCurso.Value)))
                {
                    txt_error.InnerHtml = "Ya existe un test con ese nombre dentro de ese curso";
                    return;
                }

                if (id > 0)
                {
                    Test_Test item = da.getTest_TestById(id);

                    UpdateModel(item);

                    da.updateTest_Test(item);
                }
                else
                {
                    Test_Test item = new Test_Test();

                    UpdateModel(item);

                    da.insertTest_Test(item);
                }

                Response.Redirect("test_test.aspx");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el Test');</script>");

                LogUtils.InsertarLog(" ERROR - tests_test-mantenimiento.cs::btnGuardar_Click()");
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
                var _cursos = da.getAllCourses().OrderBy(_ => _.Nombre);
                lst_cursos.AddRange(_cursos.Select(_ => new campus_CURSO { ID_Curso = _.ID_Curso, Nombre = _.Nombre + " (" + _.ID_Curso + ") [" + _.Version + "]" }).ToList());
                if (lst_cursos.Count > 0)
                {
                    this.ddlCurso.DataSource = lst_cursos;
                    this.ddlCurso.DataTextField = "Nombre";
                    this.ddlCurso.DataValueField = "ID_Curso";
                    this.ddlCurso.DataBind();
                    this.ddlCurso.Value = "0";

                    List<long> _inactives = _cursos.Where(_ => _.ID_Curso > 0 && !_.Activo).Select(_ => _.ID_Curso).ToList();
                    foreach (ListItem item in ddlCurso.Items)
                    {
                        if (_inactives.Contains(long.Parse(item.Value)))
                            item.Attributes.Add("style", "color: red");
                    }
                }

                /// Cargar los clientes de Test_Clientes
                List<Test_Clientes> lst_clientes = da.getTest_Clientes();
                ddlClienteTest.DataSource = lst_clientes;
                ddlClienteTest.DataTextField = "nombre";
                ddlClienteTest.DataValueField = "idClienteTest";
                ddlClienteTest.DataBind();
                ddlClienteTest.Items.Insert(0, new ListItem("Seleccione un cliente", ""));
                ddlClienteTest.Value = "";
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al cargar los combos');</script>");

                LogUtils.InsertarLog(" ERROR - tests_test-mantenimiento.cs::loadCombos()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }

        private void UpdateModel(Test_Test item)
        {
            item.Nombre = txt_nombre.Value.Trim();
            item.Id_Curso = long.Parse(ddlCurso.Value);
            if (item.Id_Curso == 0) item.Id_Curso = null;
            item.Normas = txt_normas.Value;
            item.Comentarios_Iniciales = txt_comentarios_iniciales.Value;
            item.Comentarios_Internos = txt_comentarios_interno.Value;
            item.Comentarios_Finales = txt_comentarios_finales.Value;
            item.Tiempo = int.Parse(txt_tiempo.Value);
            item.Num_Preguntas = int.Parse(txt_nopreguntas.Value);
            item.Acierto_Suma = int.Parse(txt_acierto_suma.Value);
            item.Error_Resta = decimal.Parse(txt_error_resta.Value);
            item.Apto_Puntos = int.Parse(txt_apto_puntos.Value);
            item.Nivel_Dificultad = short.Parse(txt_dificultad.Value);
            item.Num_Intentos = short.Parse(txt_nointentos.Value);
            item.Hacerlo_En_Partes = chk_hacerlo_en_partes.Checked;
            item.Interpretado = chk_interpretado.Checked;
            if (!String.IsNullOrEmpty(ddlClienteTest.Value))
                item.idClienteTest = long.Parse(ddlClienteTest.Value);
            item.Abierto = chk_abierto.Checked;

            if (!String.IsNullOrEmpty(txt_descripcion_publica.Value))
                item.Descripcion_Publica = txt_descripcion_publica.Value;
            if (!String.IsNullOrEmpty(txtMetaTitle.Value))
                item.Meta_Title = txtMetaTitle.Value;
            if (!String.IsNullOrEmpty(txtMetaKeywords.Value))
                item.Meta_Keywords = txtMetaKeywords.Value;
            if (!String.IsNullOrEmpty(txtMetaUrl.Value))
                item.Meta_Url = txtMetaUrl.Value;
            if (!String.IsNullOrEmpty(txtMetaAuthor.Value))
                item.Meta_Author = txtMetaAuthor.Value;
            if (!String.IsNullOrEmpty(txtMetaDescripcion.Value))
                item.Meta_Description = txtMetaDescripcion.Value;
        }
    }
}