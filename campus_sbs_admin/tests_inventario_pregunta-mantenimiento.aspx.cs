using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class tests_inventario_pregunta_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCombos();
                int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;

                if (id > 0)
                {
                    Test_Inventario_Pregunta pregunta = da.getTest_Inventario_Pregunta(id);

                    txt_texto.Value = pregunta.Pregunta_Texto;
                    txt_adjunto.Value = pregunta.Pregunta_Adjunto;
                    txt_pregunta_url.Value = pregunta.Pregunta_Url;
                    txt_interpretacion.Value = pregunta.Interpretacion;
                    txt_respuesta1.Value = pregunta.Respuesta_1;
                    txt_respuesta2.Value = pregunta.Respuesta_2;
                    txt_respuesta3.Value = pregunta.Respuesta_3;
                    txt_respuesta4.Value = pregunta.Respuesta_4;
                    txt_respuesta5.Value = pregunta.Respuesta_5;
                    txt_respuesta6.Value = pregunta.Respuesta_6;
                    txt_respuesta7.Value = pregunta.Respuesta_7;
                    txt_respuesta8.Value = pregunta.Respuesta_8;
                    txt_respuesta9.Value = pregunta.Respuesta_9;
                    txt_respuesta10.Value = pregunta.Respuesta_10;
                    text_respuesta_correcta_.Value = pregunta.Respuesta_Correcta.ToString();
                    txt_dificultad.Value = pregunta.Nivel_Dificultad.ToString();
                    ddlCurso.Value = pregunta.id_Curso != null ? pregunta.id_Curso.ToString() : "0";
                    ddlCategoria.Value = pregunta.id_Categoria.ToString();
                    ddlSubcategoria.Value = pregunta.id_Subcategoria != null ? pregunta.id_Subcategoria.ToString() : "0";
                    txt_fecha_creacion.Value = pregunta.Fecha_Creacion.ToShortDateString();
                    txt_fecha_ult_mod.Value = pregunta.Fecha_Ult_Modificacion.ToShortDateString();
                    chk_baja.Checked = pregunta.Baja;
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
            if (ddlCurso.Value == "0")
            {
                txt_error.InnerHtml = "Seleccione un Curso";
                return;
            }
            if (ddlCategoria.Value == "0")
            {
                txt_error.InnerHtml = "Seleccione una Categoría";
                return;
            }
            int id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"]) : -1;

            if (id > 0)
            {
                Test_Inventario_Pregunta pregunta = da.getTest_Inventario_Pregunta(id);

                pregunta.Pregunta_Texto = txt_texto.Value;
                pregunta.Pregunta_Adjunto = txt_adjunto.Value;
                pregunta.Pregunta_Url = txt_pregunta_url.Value;
                pregunta.Interpretacion = txt_interpretacion.Value;
                pregunta.Respuesta_1 = txt_respuesta1.Value;
                pregunta.Respuesta_2 = txt_respuesta2.Value;
                pregunta.Respuesta_3 = txt_respuesta3.Value;
                pregunta.Respuesta_4 = txt_respuesta4.Value;
                pregunta.Respuesta_5 = txt_respuesta5.Value;
                pregunta.Respuesta_6 = txt_respuesta6.Value;
                pregunta.Respuesta_7 = txt_respuesta7.Value;
                pregunta.Respuesta_8 = txt_respuesta8.Value;
                pregunta.Respuesta_9 = txt_respuesta9.Value;
                pregunta.Respuesta_10 = txt_respuesta10.Value;
                pregunta.Respuesta_Correcta = short.Parse(text_respuesta_correcta_.Value);
                pregunta.Nivel_Dificultad = short.Parse(txt_dificultad.Value);
                pregunta.id_Curso = long.Parse(ddlCurso.Value);
                pregunta.id_Categoria = int.Parse(ddlCategoria.Value);
                pregunta.Fecha_Ult_Modificacion = DateTime.Now;
                pregunta.Baja = chk_baja.Checked;

                if (ddlSubcategoria.Value != "0")
                    pregunta.id_Subcategoria = int.Parse(ddlSubcategoria.Value);
                else
                    pregunta.id_Subcategoria = null;

                da.updateTest_Inventario_Pregunta(pregunta);
            }
            else
            {
                Test_Inventario_Pregunta pregunta = new Test_Inventario_Pregunta();

                pregunta.Pregunta_Texto = txt_texto.Value;
                pregunta.Pregunta_Adjunto = txt_adjunto.Value;
                pregunta.Pregunta_Url = txt_pregunta_url.Value;
                pregunta.Interpretacion = txt_interpretacion.Value;
                pregunta.Respuesta_1 = txt_respuesta1.Value;
                pregunta.Respuesta_2 = txt_respuesta2.Value;
                pregunta.Respuesta_3 = txt_respuesta3.Value;
                pregunta.Respuesta_4 = txt_respuesta4.Value;
                pregunta.Respuesta_5 = txt_respuesta5.Value;
                pregunta.Respuesta_6 = txt_respuesta6.Value;
                pregunta.Respuesta_7 = txt_respuesta7.Value;
                pregunta.Respuesta_8 = txt_respuesta8.Value;
                pregunta.Respuesta_9 = txt_respuesta9.Value;
                pregunta.Respuesta_10 = txt_respuesta10.Value;
                pregunta.Respuesta_Correcta = short.Parse(text_respuesta_correcta_.Value);
                pregunta.Nivel_Dificultad = short.Parse(txt_dificultad.Value);
                pregunta.id_Curso = long.Parse(ddlCurso.Value);
                pregunta.id_Categoria = int.Parse(ddlCategoria.Value);
                pregunta.Baja = chk_baja.Checked;

                if (ddlSubcategoria.Value != "0")
                    pregunta.id_Subcategoria = int.Parse(ddlSubcategoria.Value);
                else
                    pregunta.id_Subcategoria = null;

                da.insertTest_Inventario_Pregunta(pregunta);

            }
            Response.Redirect("test_inventario_pregunta.aspx");
        }

        protected void loadCombos()
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

            List<Test_Categoria> lst_cat = new List<Test_Categoria>();
            lst_cat.Add(new Test_Categoria { Id = 0, Nombre = "Seleccione" });
            lst_cat.AddRange(da.getTest_Categorias());
            if (lst_cat.Count > 0)
            {
                this.ddlCategoria.DataSource = lst_cat;
                this.ddlCategoria.DataTextField = "Nombre";
                this.ddlCategoria.DataValueField = "Id";
                this.ddlCategoria.DataBind();
                this.ddlCategoria.Value = "0";
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
                this.ddlSubcategoria.Value = "0";
            }
        }
    }
}