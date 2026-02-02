using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class test_inventario_pregunta : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Test_Inventario_Pregunta> items = da.getTest_Inventario_PreguntasAll();
            List<campus_CURSO> cursos = da.getAllCourses();
            List<Test_Categoria> categorias = da.getTest_Categorias();
            List<Test_Subcategoria> subcategorias = da.getTest_Subcategorias();

            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            //sbuild.Append("<th style='width: 400px;'>Pregunta</th>");
            sbuild.Append("<th>Pregunta</th>");
            sbuild.Append("<th>F. Act.</th>");
            sbuild.Append("<th>Dif.</th>");
            sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>Cat.</th>");
            sbuild.Append("<th>Subcat.</th>");
            sbuild.Append("<th>Nº</th>");
            sbuild.Append("<th>OK</th>");
            sbuild.Append("<th>%</th>");
            sbuild.Append("<th>Dif. M</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead>");
            sbuild.Append("<tbody>");

            /// Ordenar por fecha última actualización
            items = items.OrderByDescending(_ => _.Fecha_Ult_Modificacion).ToList();

            foreach (Test_Inventario_Pregunta item in items)
            {
                campus_CURSO curso = cursos.FirstOrDefault(_ => _.ID_Curso == item.id_Curso);
                Test_Categoria categoria = categorias.FirstOrDefault(_ => _.Id == item.id_Categoria);
                Test_Subcategoria subcategoria = subcategorias.FirstOrDefault(_ => _.Id == item.id_Subcategoria);

                if (item.Baja)
                    sbuild.Append("<tr class='text-color-red'>");
                else
                    sbuild.Append("<tr>");
                sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(item.Pregunta_Texto) + "</span>" + item.Pregunta_Texto + "</td>");
                sbuild.Append($"<td>{item.Fecha_Ult_Modificacion}</td>");
                //sbuild.Append("<td style='text-align:right; padding-right:20px'>" + item.Nivel_Dificultad + "</td>");
                sbuild.Append("<td>" + item.Nivel_Dificultad + "</td>");
                sbuild.Append("<td>" + (curso != null ? curso.Nombre : "No Disponible") + "</td>");
                sbuild.Append("<td>" + (categoria != null ? categoria.Nombre : "No Disponible") + "</td>");
                sbuild.Append("<td>" + (subcategoria != null ? subcategoria.Nombre : "No Disponible") + "</td>");
                /*sbuild.Append("<td style='text-align:right; padding-right:20px'>" + item.Info_Num_Participantes + "</td>");
                sbuild.Append("<td style='text-align:right; padding-right:20px'>" + item.Info_Num_Aciertos + "</td>");*/
                sbuild.Append("<td>" + item.Info_Num_Participantes + "</td>");
                sbuild.Append("<td>" + item.Info_Num_Aciertos + "</td>");

                decimal _porcentaje = new decimal(0);
                if (item.Info_Num_Participantes > 0)
                    _porcentaje = Math.Round(((decimal)item.Info_Num_Aciertos / (decimal)item.Info_Num_Participantes) * 100, 2);

                //sbuild.Append("<td style='width:25px;'><a href=\"tests_inventario_pregunta-detalles.aspx?id=" + item.id + "\" title=\"Detalles\"><i class='fa fa-eye fa-1-6x' style='margin-right:15px'></i></a></td>");
                sbuild.Append($"<td>{Math.Round(_porcentaje, 2)}</td>");

                int _dificultad_media = 0;
                if (item.Info_Num_Participantes > 0)
                {
                    if (_porcentaje < new decimal(21))
                        _dificultad_media = 5;
                    else if (_porcentaje < new decimal(41))
                        _dificultad_media = 4;
                    else if (_porcentaje < new decimal(61))
                        _dificultad_media = 3;
                    else if (_porcentaje < new decimal(81))
                        _dificultad_media = 2;
                    else if (_porcentaje <= new decimal(100))
                        _dificultad_media = 1;
                }

                sbuild.Append($"<td>{_dificultad_media}</td>");
                sbuild.Append("<td><a href=\"tests_inventario_pregunta-mantenimiento.aspx?id=" + item.id + "\" title=\"Editar\"><i class='fa fa-edit fa-1-6x' style='margin-right:15px'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la pregunta: " + "" + "?\")){eliminar(" + item.id + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            title.InnerHtml = "<i class='fa fa-question-circle'></i> Listado de Preguntas <a href='tests_inventario_pregunta-mantenimiento.aspx' title='Nueva Pregunta' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva Pregunta</small></a><a href='javascript:void(0);' onclick='asignar_dificultad()' title='Asignar dificultad' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-sync-alt fa-2x'></i> Asignar dificultad</small></a>";
        }

        protected void btnBorrar_Click(object sender, ImageClickEventArgs e)
        {
            bool delete = false;

            try
            {
                int id = !String.IsNullOrEmpty(hidId.Value) ? int.Parse(hidId.Value) : -1;
                if (id > 0)
                {
                    da.DeleteTest_Inventario_Pregunta(id);
                    delete = true;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la Pregunta');</script>");

                LogUtils.InsertarLog(" ERROR - tests_inventario_pregunta.cs::btnBorrar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete)
                Response.Redirect("test_inventario_pregunta.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la Pregunta');</script>");
        }

        protected void btn_asignar_Click(object sender, ImageClickEventArgs e)
        {
            bool _update = true;

            try
            {
                /// 1.- Sacar el nº de apariciones mínimo para asignar la dificultad media
                int _num_asignaciones = int.Parse(ConfigurationManager.AppSettings["num_apariciones_examenes"]);

                /// 2.- Sacar la respuestas
                List<Test_Inventario_Pregunta> _preguntas = da.getTest_Inventario_Preguntas();

                /// 2.1.- Filtrar las que tienen el nº de apariciones correcto
                _preguntas = _preguntas.Where(_ => _.Info_Num_Participantes >= _num_asignaciones).ToList();

                /// 3.- Recorrer las preguntas
                foreach (var _pregunta in _preguntas)
                {
                    decimal _porcentaje = new decimal(0);
                    short _dificultad_media = 0;
                    if (_pregunta.Info_Num_Participantes > 0)
                    {
                        _porcentaje = Math.Round(((decimal)_pregunta.Info_Num_Aciertos / (decimal)_pregunta.Info_Num_Participantes) * 100, 2);
                        if (_porcentaje < new decimal(21))
                            _dificultad_media = 5;
                        else if (_porcentaje < new decimal(41))
                            _dificultad_media = 4;
                        else if (_porcentaje < new decimal(61))
                            _dificultad_media = 3;
                        else if (_porcentaje < new decimal(81))
                            _dificultad_media = 2;
                        else if (_porcentaje <= new decimal(100))
                            _dificultad_media = 1;
                    }

                    if (_pregunta.Nivel_Dificultad != _dificultad_media)
                    {
                        Test_Inventario_Pregunta _pregunta_act = _pregunta;
                        _pregunta_act.Nivel_Dificultad = _dificultad_media;
                        da.updateTest_Inventario_Pregunta(_pregunta_act);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - tests_inventario_pregunta.cs::btn_asignar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _update = false;
            }

            if (_update)
                Response.Redirect("test_inventario_pregunta.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al actualizar las preguntas');</script>");
        }
    }
}