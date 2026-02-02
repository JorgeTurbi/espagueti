using sbs_DAL;
using sbs_DAL.App_Code.BLL.ReturnClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class test_reglas : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private Test_Test _test;
        private List<Test_Test_Reglas> _reglas;
        private List<campus_CURSO> _cursos;
        private List<Test_Categoria> _categorias;
        private List<Test_Subcategoria> _subcategorias;
        private List<Test_Pregunta_Wrapper> _preguntas;
        private int _id;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;
                _test = da.getTest_TestById(_id);

                if (_test != null)
                {
                    _reglas = da.getTest_Test_Reglas(_id);
                    _cursos = da.getActiveCourses().OrderBy(_ => _.Nombre).ToList();
                    _categorias = da.getTest_Categorias();
                    _subcategorias = da.getTest_Subcategorias();
                    _preguntas = da.getPreguntasWrappers();

                    for (int i = 0; i < _test.Num_Preguntas; i++)
                    {
                        HtmlGenericControl div_container = new HtmlGenericControl("DIV");
                        div_container.Attributes.Add("class", "col-sm-12");

                        //Orden
                        Label txt_orden = new Label();
                        txt_orden.Text = $"Orden {i + 1}";
                        txt_orden.CssClass = "col-sm-1 labelcenter";
                        div_container.Controls.Add(txt_orden);

                        //Curso
                        HtmlGenericControl div_cursos = new HtmlGenericControl("DIV");
                        div_cursos.Attributes.Add("class", "col-sm-3");

                        HtmlGenericControl div_ddl_curso = new HtmlGenericControl("DIV");
                        div_ddl_curso.Attributes.Add("class", "form-group");

                        DropDownList cmb_cursos = new DropDownList();
                        cmb_cursos.CssClass = "selectpicker selectcurso";
                        cmb_cursos.Attributes.Add("data-live-search", "true");
                        cmb_cursos.Attributes.Add("data-hide-disabled", "true");
                        cmb_cursos.Attributes.Add("data-live-search-normalize", "true");
                        cmb_cursos.ID = $"cmb_curso_o_{i + 1}";

                        cmb_cursos.AutoPostBack = true;
                        cmb_cursos.SelectedIndexChanged += OnSetSelectedCurso;

                        cmb_cursos.Items.Add(new ListItem("Seleccione", "0"));
                        cmb_cursos.Items.AddRange(GetCursosListItemCollection());
                        cmb_cursos.SelectedValue = i < _reglas.Count ? _reglas[i].Id_Curso.ToString() : "0";

                        div_ddl_curso.Controls.Add(cmb_cursos);

                        div_cursos.Controls.Add(div_ddl_curso);

                        div_container.Controls.Add(div_cursos);

                        //Categorias
                        HtmlGenericControl div_cat = new HtmlGenericControl("DIV");
                        div_cat.Attributes.Add("class", "col-sm-3");

                        HtmlGenericControl div_ddl_cat = new HtmlGenericControl("DIV");
                        div_ddl_cat.Attributes.Add("class", "form-group");

                        DropDownList cmb_cat = new DropDownList();
                        cmb_cat.CssClass = "selectpicker selectcat";
                        cmb_cat.Attributes.Add("data-live-search", "true");
                        cmb_cat.Attributes.Add("data-hide-disabled", "true");
                        cmb_cat.Attributes.Add("data-live-search-normalize", "true");
                        cmb_cat.ID = $"cmb_cat_o_{i + 1}";

                        cmb_cat.AutoPostBack = true;
                        cmb_cat.SelectedIndexChanged += OnSetSelectedCAtegoria;

                        cmb_cat.Items.Add(new ListItem("Seleccione", "0"));
                        cmb_cat.Items.AddRange(GetCategoriasListItemCollection());
                        cmb_cat.SelectedValue = i < _reglas.Count ? _reglas[i].Id_Categoria.ToString() : "0";

                        div_ddl_cat.Controls.Add(cmb_cat);

                        div_cat.Controls.Add(div_ddl_cat);

                        div_container.Controls.Add(div_cat);

                        //Subcategorias
                        HtmlGenericControl div_subcat = new HtmlGenericControl("DIV");
                        div_subcat.Attributes.Add("class", "col-sm-3");

                        HtmlGenericControl div_ddl_subcat = new HtmlGenericControl("DIV");
                        div_ddl_subcat.Attributes.Add("class", "form-group");

                        DropDownList cmb_subcat = new DropDownList();
                        cmb_subcat.CssClass = "selectpicker selectsubcat";
                        cmb_subcat.Attributes.Add("data-live-search", "true");
                        cmb_subcat.Attributes.Add("data-hide-disabled", "true");
                        cmb_subcat.Attributes.Add("data-live-search-normalize", "true");
                        cmb_subcat.ID = $"cmb_subcat_o_{i + 1}";

                        cmb_subcat.AutoPostBack = true;
                        cmb_subcat.SelectedIndexChanged += OnSetSelectedSubcat;

                        cmb_subcat.Items.Add(new ListItem("Seleccione", "0"));
                        cmb_subcat.Items.AddRange(GetSubcategoriasListItemCollection());
                        cmb_subcat.SelectedValue = i < _reglas.Count ? _reglas[i].Id_Subcategoria.ToString() : "0";

                        div_ddl_subcat.Controls.Add(cmb_subcat);

                        div_subcat.Controls.Add(div_ddl_subcat);

                        div_container.Controls.Add(div_subcat);

                        //Preguntas
                        Label txt_preguntas = new Label();
                        int preguntas = CalculateAvailableQuestions(i < _reglas.Count ? _reglas[i].Id_Curso : 0, i < _reglas.Count ? _reglas[i].Id_Categoria : 0, i < _reglas.Count ? _reglas[i].Id_Subcategoria : 0);
                        txt_preguntas.Text = $"Disponible: {preguntas} preguntas";
                        txt_preguntas.ID = $"preg_{i + 1}";
                        txt_preguntas.CssClass = "col-sm-2 labelcenter";
                        if (preguntas == 0)
                        {
                            txt_preguntas.Attributes.Add("style", "color:#ff3d00");
                        }
                        div_container.Controls.Add(txt_preguntas);

                        lab.Controls.Add(div_container);
                    }

                    /// 3.- Pintar el título
                    title.InnerHtml = "<i class='fa fa-clipboard-check'></i> Listado de Reglas del Test: " + _test.Nombre;

                    UpdateSummary();

                    PaintCards();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al cargar el Test');</script>");

                LogUtils.InsertarLog(" ERROR - tests-reglas.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                txt_error.InnerText = string.Empty;
                //if (da.Test_Realizado(_id))
                //{
                //    txt_error.InnerText = "Este test ya ha sido realizado y no puede ser modificado.";
                //    return;
                //}
                List<Test_Test_Reglas> reglas = new List<Test_Test_Reglas>();
                for (int i = 1; i <= _test.Num_Preguntas; i++)
                {
                    if (CalculateAvailableQuestions((short)i) == 0)
                    {
                        txt_error.InnerText = "No se puede guardar un Test si al menos una regla no tiene preguntas disponibles.";
                        return;
                    }
                    DropDownList cmb_curso = (DropDownList)lab.FindControl($"cmb_curso_o_{i}");
                    long curso = long.Parse(cmb_curso.SelectedValue);

                    DropDownList cmb_cat = (DropDownList)lab.FindControl($"cmb_cat_o_{i}");
                    int cat = int.Parse(cmb_cat.SelectedValue);

                    DropDownList cmb_subcat = (DropDownList)lab.FindControl($"cmb_subcat_o_{i}");
                    int subcat = int.Parse(cmb_subcat.SelectedValue);

                    reglas.Add(new Test_Test_Reglas
                    {
                        Id_Test = _test.Id,
                        Orden = (short)i,
                        Id_Curso = curso,
                        Id_Categoria = cat,
                        Id_Subcategoria = subcat,
                        Fecha_Ult_Modificacion = DateTime.Now,
                        Fecha_Creacion = i < _reglas.Count ? _reglas[i - 1].Fecha_Creacion : DateTime.Now
                    });

                }
                da.updateTest_Test_Reglas(reglas);

                if (_test.Num_Preguntas < _reglas.Count)
                {
                    for (int i = _test.Num_Preguntas; i < reglas.Count; i++)
                    {
                        da.Delete_Test_Regla(_reglas[i].Id_Test, reglas[i].Orden);
                    }

                }
                //_reglas = da.getTest_Test_Reglas(_id);
                Response.Redirect("test_test.aspx");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el Test');</script>");

                LogUtils.InsertarLog(" ERROR - tests-reglas.cs::btnGuardar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }
        }

        private void OnSetSelectedCurso(object sender, EventArgs e)
        {

            DropDownList cmb_curso = (DropDownList)sender;

            short orden = short.Parse(cmb_curso.ID.Split('_')[3]);

            int preguntas = CalculateAvailableQuestions(orden);

            Label txt_pregunntas = (Label)lab.FindControl($"preg_{orden}");

            txt_pregunntas.Text = $"Disponible: {preguntas} preguntas";

            txt_pregunntas.Attributes.Remove("style");
            if (preguntas == 0)
            {
                txt_pregunntas.Attributes.Add("style", "color:#ff3d00");
            }

            UpdateSummary();

            MarkSummaryAsModified();
        }

        private void OnSetSelectedCAtegoria(object sender, EventArgs e)
        {
            DropDownList cmb = (DropDownList)sender;

            short orden = short.Parse(cmb.ID.Split('_')[3]);

            int preguntas = CalculateAvailableQuestions(orden);

            Label txt_pregunntas = (Label)lab.FindControl($"preg_{orden}");

            txt_pregunntas.Text = $"Disponible: {preguntas} preguntas";

            UpdateSummary();

            MarkSummaryAsModified();
        }

        private void OnSetSelectedSubcat(object sender, EventArgs e)
        {
            DropDownList cmb = (DropDownList)sender;

            short orden = short.Parse(cmb.ID.Split('_')[3]);

            int preguntas = CalculateAvailableQuestions(orden);

            Label txt_pregunntas = (Label)lab.FindControl($"preg_{orden}");

            txt_pregunntas.Text = $"Disponible: {preguntas} preguntas";

            UpdateSummary();

            MarkSummaryAsModified();
        }

        private ListItem[] GetCursosListItemCollection()
        {
            ListItem[] collection = new ListItem[_cursos.Count()];
            for (int i = 0; i < _cursos.Count; i++)
            {
                collection[i] = new ListItem(_cursos[i].Nombre + " (" + _cursos[i].ID_Curso + ") [" + _cursos[i].Version + "]", _cursos[i].ID_Curso.ToString());
            }
            return collection;
        }

        private ListItem[] GetCategoriasListItemCollection()
        {
            ListItem[] collection = new ListItem[_categorias.Count()];
            for (int i = 0; i < _categorias.Count; i++)
            {
                collection[i] = new ListItem(_categorias[i].Nombre, _categorias[i].Id.ToString());
            }
            return collection;
        }

        private ListItem[] GetSubcategoriasListItemCollection()
        {
            ListItem[] collection = new ListItem[_subcategorias.Count()];
            for (int i = 0; i < _subcategorias.Count; i++)
            {
                collection[i] = new ListItem(_subcategorias[i].Nombre, _subcategorias[i].Id.ToString());
            }
            return collection;
        }

        private int CalculateAvailableQuestions(short orden)
        {
            long id_curso = long.Parse(((DropDownList)(lab.FindControl($"cmb_curso_o_{orden}"))).SelectedValue);
            int id_categoria = int.Parse(((DropDownList)(lab.FindControl($"cmb_cat_o_{orden}"))).SelectedValue);
            int id_subcat = int.Parse(((DropDownList)(lab.FindControl($"cmb_subcat_o_{orden}"))).SelectedValue);
            return _preguntas.Count(_ => _.CumpleRegla(id_curso, id_categoria, id_subcat));
        }

        private List<Test_Pregunta_Wrapper> GetAvailableQuestionsIds(int orden)
        {
            long id_curso = long.Parse(((DropDownList)(lab.FindControl($"cmb_curso_o_{orden}"))).SelectedValue);
            int id_categoria = int.Parse(((DropDownList)(lab.FindControl($"cmb_cat_o_{orden}"))).SelectedValue);
            int id_subcat = int.Parse(((DropDownList)(lab.FindControl($"cmb_subcat_o_{orden}"))).SelectedValue);
            return _preguntas.Where(_ => _.CumpleRegla(id_curso, id_categoria, id_subcat)).ToList();
        }

        private int CalculateAvailableQuestions(long? id_Curso, int? id_Categoria, int? id_Subcategoria)
        {
            return _preguntas.Count(_ => _.CumpleRegla(id_Curso, id_Categoria, id_Subcategoria));
        }

        private void UpdateSummary()
        {
            List<Test_Pregunta_Wrapper> preguntas = new List<Test_Pregunta_Wrapper>();

            for (int i = 1; i <= _test.Num_Preguntas; i++)
            {
                foreach (var pregunta in GetAvailableQuestionsIds(i))
                {
                    if (!preguntas.Exists(_ => _.id == pregunta.id))
                    {
                        preguntas.Add(pregunta);
                    }
                }
            }

            int sum_dif = 0;
            int preguntasvalidas = 0;
            int dif1 = 0;
            int dif2 = 0;
            int dif3 = 0;
            int dif4 = 0;
            int dif5 = 0;

            foreach (var pregunta in preguntas)
            {
                if (pregunta.Nivel_Dificultad >= _test.Nivel_Dificultad - 1 && pregunta.Nivel_Dificultad <= _test.Nivel_Dificultad)
                {
                    sum_dif += pregunta.Nivel_Dificultad;
                    preguntasvalidas++;
                }

                switch (pregunta.Nivel_Dificultad)
                {
                    case 1:
                        dif1++;
                        break;
                    case 2:
                        dif2++;
                        break;
                    case 3:
                        dif3++;
                        break;
                    case 4:
                        dif4++;
                        break;
                    case 5:
                        dif5++;
                        break;
                    default:
                        break;
                }
            }

            decimal dif_avrg = preguntas.Count > 0 && preguntasvalidas > 0 ? Decimal.Divide(sum_dif, preguntasvalidas) : 0;

            int dif1_percent = preguntas.Count > 0 ? (dif1 * 100) / preguntas.Count : 0;
            int dif2_percent = preguntas.Count > 0 ? (dif2 * 100) / preguntas.Count : 0;
            int dif3_percent = preguntas.Count > 0 ? (dif3 * 100) / preguntas.Count : 0;
            int dif4_percent = preguntas.Count > 0 ? (dif4 * 100) / preguntas.Count : 0;
            int dif5_percent = preguntas.Count > 0 ? (dif5 * 100) / preguntas.Count : 0;

            smy_preguntas.InnerText = preguntas.Count.ToString();
            smy_preguntas_necesarias.InnerText = (_test.Num_Preguntas * 3).ToString();
            int preguntas_razon = (preguntas.Count * 100) / (_test.Num_Preguntas * 3);
            if (preguntas_razon < 25)
            {
                smy_preguntas_value_container.Attributes.Remove("style");
                smy_preguntas_value_container.Attributes.Add("style", "background-color:#ff3d00");
            }
            else if (preguntas_razon >= 25 && preguntas_razon < 50)
            {
                smy_preguntas_value_container.Attributes.Remove("style");
                smy_preguntas_value_container.Attributes.Add("style", "background-color:#ff9100");
            }
            else if (preguntas_razon >= 50 && preguntas_razon < 75)
            {
                smy_preguntas_value_container.Attributes.Remove("style");
                smy_preguntas_value_container.Attributes.Add("style", "background-color:#c6ff00");
            }
            else
            {
                smy_preguntas_value_container.Attributes.Remove("style");
                smy_preguntas_value_container.Attributes.Add("style", "background-color:#1de9b6");
            }

            smy_preguntas_difpromedio.InnerText = dif_avrg != 0 ? dif_avrg.ToString("#.##") : "0";
            smy_test_dificultad.InnerText = _test.Nivel_Dificultad.ToString();
            smy_dif_avr_value_container.Attributes.Remove("style");
            if (Math.Abs((_test.Nivel_Dificultad) - dif_avrg) <= (decimal)0.5)
            {
                smy_dif_avr_value_container.Attributes.Add("style", "background-color:#1de9b6");
            }
            else
            {
                smy_dif_avr_value_container.Attributes.Add("style", "background-color:#ff3d00");
            }

            smy_dif1.InnerText = dif1.ToString();
            smy_dif1_percent.InnerText = $"{dif1_percent}%";

            smy_dif2.InnerText = dif2.ToString();
            smy_dif2_percent.InnerText = $"{dif2_percent}%";

            smy_dif3.InnerText = dif3.ToString();
            smy_dif3_percent.InnerText = $"{dif3_percent}%";

            smy_dif4.InnerText = dif4.ToString();
            smy_dif4_percent.InnerText = $"{dif4_percent}%";

            smy_dif5.InnerText = dif5.ToString();
            smy_dif5_percent.InnerText = $"{dif5_percent}%";
        }

        private void MarkSummaryAsModified()
        {
            divsummary.Attributes.Remove("class");
            divsummary.Attributes.Add("class", "col-sm-12 summary-changed");
        }

        private void PaintCards()
        {
            switch (_test.Nivel_Dificultad)
            {
                case 1:
                    //#dcdcdc
                    dif3_container.Attributes.Remove("style");
                    dif3_container.Attributes.Add("style", "background-color: #dcdcdc");
                    dif4_container.Attributes.Remove("style");
                    dif4_container.Attributes.Add("style", "background-color: #dcdcdc");
                    dif5_container.Attributes.Remove("style");
                    dif5_container.Attributes.Add("style", "background-color: #dcdcdc");
                    break;
                case 2:
                    dif4_container.Attributes.Remove("style");
                    dif4_container.Attributes.Add("style", "background-color: #dcdcdc");
                    dif5_container.Attributes.Remove("style");
                    dif5_container.Attributes.Add("style", "background-color: #dcdcdc");
                    break;
                case 3:
                    dif1_container.Attributes.Remove("style");
                    dif1_container.Attributes.Add("style", "background-color: #dcdcdc");
                    dif5_container.Attributes.Remove("style");
                    dif5_container.Attributes.Add("style", "background-color: #dcdcdc");
                    break;
                case 4:
                    dif1_container.Attributes.Remove("style");
                    dif1_container.Attributes.Add("style", "background-color: #dcdcdc");
                    dif2_container.Attributes.Remove("style");
                    dif2_container.Attributes.Add("style", "background-color: #dcdcdc");
                    break;
                case 5:
                    dif1_container.Attributes.Remove("style");
                    dif1_container.Attributes.Add("style", "background-color: #dcdcdc");
                    dif2_container.Attributes.Remove("style");
                    dif2_container.Attributes.Add("style", "background-color: #dcdcdc");
                    dif3_container.Attributes.Remove("style");
                    dif3_container.Attributes.Add("style", "background-color: #dcdcdc");
                    break;
                default:
                    break;
            }
        }
    }

}