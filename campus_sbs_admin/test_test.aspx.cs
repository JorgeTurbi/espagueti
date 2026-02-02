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
    public partial class test_test : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (list_user.Count == 0 && Session["usuario"] != null)
                list_user.Add((CLIENTES)Session["usuario"]);
            else
                Response.Redirect("login.aspx");

            if (!IsPostBack)
            {
                List<Test_Test> items = da.getTest_Tests();
                List<campus_CURSO> cursos = da.getAllCourses();

                StringBuilder sbuild = new StringBuilder();

                /// 2.1.- Inicio tabla
                sbuild.Append("<table id =\"tabla\" class=\"display compact\" style =\"width:100%\"><thead>");

                /// 2.2.- Cabecera
                sbuild.Append("<tr>");
                sbuild.Append("<th>Id</th>");
                sbuild.Append("<th>Nombre</th>");
                sbuild.Append("<th>Curso</th>");
                sbuild.Append("<th title='Tiempo en minutos'>Min.</th>");
                sbuild.Append("<th title='Preguntas'>Preg.</th>");
                sbuild.Append("<th>Apto</th>");
                sbuild.Append("<th title='Dificultad'>Dif.</th>");
                sbuild.Append("<th title='Intentos'>Int.</th>");
                sbuild.Append("<th title='Nº veces realizado'>Num</th>");
                sbuild.Append("<th>Aptos</th>");
                sbuild.Append("<th>Nota Media</th>");
                sbuild.Append("<th title='Dificultad media'>Dif. Media</th>");
                sbuild.Append("<th>Abierto</th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("<th></th>");
                sbuild.Append("</tr>");
                sbuild.Append("</thead>");
                sbuild.Append("<tbody>");

                foreach (Test_Test item in items)
                {
                    campus_CURSO curso = cursos.FirstOrDefault(_ => _.ID_Curso == item.Id_Curso);
                    string cssclass = $"{(item.Baja ? "table-danger" : string.Empty)}";
                    sbuild.Append($"<tr class='{cssclass}'>");
                    sbuild.Append($"<td><span class='badge badge-primary test-id' title='Clic para copiar al portapapeles' onclick='copycb({item.Id})'>{item.Id}</span></td>");
                    sbuild.Append("<td>" + item.Nombre + "</td>");
                    sbuild.Append("<td>" + (curso != null ? curso.Nombre : "No Disponible") + "</td>");
                    sbuild.Append($"<td>{item.Tiempo}</td>");
                    sbuild.Append($"<td>{item.Num_Preguntas}</td>");
                    sbuild.Append($"<td>{item.Apto_Puntos}</td>");
                    sbuild.Append($"<td>{item.Nivel_Dificultad}</td>");
                    sbuild.Append($"<td>{item.Num_Intentos}</td>");
                    sbuild.Append($"<td>{item.Info_Num_Participantes}</td>");
                    sbuild.Append($"<td>{item.Info_Num_Aptos}</td>");
                    sbuild.Append($"<td>{item.Info_Puntuacion_Media}</td>");
                    sbuild.Append($"<td>{item.Info_Nivel_Dificultad_Medio}</td>");
                    if (item.Abierto)
                        sbuild.Append("<td><i class='far fa-check-square text-color-primary fa-1-6x'></i></td>");
                    else
                        sbuild.Append("<td></td>");
                    sbuild.Append("<td><a href=\"test-reglas.aspx?id=" + item.Id + "\" title=\"Reglas\"><i class='fa fa-clipboard-check fa-1-6x'></i></a></td>");
                    sbuild.Append("<td><a href=\"test_test_mantenimiento.aspx?id=" + item.Id + "\" title=\"Editar\"><i class='fa fa-edit fa-1-6x'></i></a></td>");
                    sbuild.Append("<td><a href=\"test-listado.aspx?id=" + item.Id + "\" title=\"Listado Test\"><i class='far fa-list-alt fa-1-6x'></i></a></td>");
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el test: " + item.Nombre + "?\")){eliminar(" + item.Id + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                    sbuild.Append("</tr>");
                }

                sbuild.Append("</tbody></table>");

                /// 2.4.- Añadimos la tabla al div
                table.InnerHtml = sbuild.ToString();

                /// 3.- Pintar el título
                title.InnerHtml = "<i class='fa fa-clipboard-list'></i> Listado de Tests <a href='test_test_mantenimiento.aspx' title='Nuevo test' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nuevo test</small></a><a href='javascript:void(0);' onclick='asignar_dificultad()' title='Asignar dificultad' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-sync-alt fa-2x'></i> Asignar dificultad</small></a><a href='https://campus.spainbs.com/examen-smart.aspx?idt=1&idd=404&idc=180&k=F464D3EB-D005-4BF2-BDE9-2FBC15929606' target='_blank' title='Autocorregir examenes' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-tasks fa-2x'></i> Autocorregir examenes</small></a>";
            }
        }

        protected void btnBorrar_Click(object sender, ImageClickEventArgs e)
        {
            bool delete = false;

            try
            {
                int id = !String.IsNullOrEmpty(hidId.Value) ? int.Parse(hidId.Value) : -1;
                if (id > 0)
                {
                    da.DeleteTest_Test(id);
                    delete = true;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el Test');</script>");

                LogUtils.InsertarLog(" ERROR - test_test.cs::btnBorrar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete)
                Response.Redirect("test_test.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el Test');</script>");
        }

        protected void btn_asignar_Click(object sender, ImageClickEventArgs e)
        {
            bool _update = true;

            try
            {
                /// 1.- Sacar el nº de apariciones mínimo para asignar la dificultad media
                int _num_asignaciones = int.Parse(ConfigurationManager.AppSettings["num_realizaciones_examen"]);

                /// 2.- Sacar los tests
                List<Test_Test> _tests = da.getTest_Tests();

                /// 2.1.- Filtrar las que tienen el nº de apariciones correcto
                _tests = _tests.Where(_ => _.Info_Num_Participantes >= _num_asignaciones).ToList();

                /// 3.- Recorrer los tests
                foreach (var _test in _tests)
                {
                    if (_test.Nivel_Dificultad != _test.Info_Nivel_Dificultad_Medio)
                    {
                        Test_Test _test_act = _test;
                        _test_act.Nivel_Dificultad = _test.Info_Nivel_Dificultad_Medio;
                        da.updateTest_Test(_test_act);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - test_test.cs::btn_asignar_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                _update = false;
            }

            if (_update)
                Response.Redirect("test_test.aspx");
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al actualizar los tests');</script>");
        }
    }
}