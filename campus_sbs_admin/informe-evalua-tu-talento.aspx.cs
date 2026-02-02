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
    public partial class informe_evalua_tu_talento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Poner fechas
                date_end.Value = DateTime.Today.ToShortDateString();
                date_start.Value = DateTime.Today.AddDays(-30).ToShortDateString();

                /// 2.- Cargar el resumen
                cargar_resumen();
            }
        }
        
        protected void img_filter_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Paramétros del formulario
            DateTime _fecha_inicio = DateTime.Parse(date_start.Value);
            DateTime _fecha_fin = DateTime.Parse(date_end.Value).AddDays(1);

            /// 2.- Sacar datos
            
            /// 2.1.- Sacar las ap
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByTest("evalua-tu-talento");

            /// 2.2.- Sacar los examenes
            List<Test_Test> _test_abiertos = da.getTest_TestsOpen();
            List<int> _id_test = _test_abiertos.Select(_ => _.Id).Distinct().ToList();
            List<Test_Examen> _test_iniciados = da.getTest_ExamenByListIdTest(_id_test);

            /// 3.- Filtrar las acciones entre las fechas
            _actions = _actions.Where(_ => !String.IsNullOrEmpty(_.utm_content) && !String.IsNullOrEmpty(_.Cookie) && _.Fecha >= _fecha_inicio && _.Fecha < _fecha_fin).ToList();

            /// 4.- Filtrar los test entre las fechas
            _test_iniciados = _test_iniciados.Where(_ => _.Fecha >= _fecha_inicio && _.Fecha < _fecha_fin).ToList();
            
            /// 5.- Pintar los datos
            table_listado_examenes.InnerHtml = load_data(_actions, _test_iniciados, _test_abiertos);
        }

        private void cargar_resumen()
        {
            /// 1.- Sacar las entradas en ficha-test
            List<campus_ACCIONES_PERSONA> _actions = da.getActionsByTest("evalua-tu-talento");

            /// 1.1.- Filtrar los que tengan utm_content
            _actions = _actions.Where(_ => !String.IsNullOrEmpty(_.utm_content)).ToList();

            /// 1.2.- Sacar las cookies distintas
            int _num_cookies = _actions.Where(_ => !String.IsNullOrEmpty(_.Cookie)).Select(_ => _.Cookie).Distinct().Count();
            test_number.InnerHtml = _num_cookies.ToString();

            /// 2.- Sacar los test abiertos
            List<Test_Test> _test_abiertos = da.getTest_TestsOpen();

            /// 2.1.- Sacar los test iniciados que sean test abiertos
            List<int> _id_test = _test_abiertos.Select(_ => _.Id).Distinct().ToList();
            List<Test_Examen> _test_iniciados = da.getTest_ExamenByListIdTest(_id_test);

            /// 2.2.- Pintar los datos
            test_start_number.InnerHtml = _test_iniciados.Count.ToString();

            decimal _porcentaje_iniciados = Math.Round((((decimal)_test_iniciados.Count / _num_cookies) * 100), 2);
            test_start_percent.InnerHtml = _porcentaje_iniciados + "%";

            /// 3.- Sacar los test finalizados
            List<Test_Examen> _test_finalizados = _test_iniciados.Where(_ => _.Estado == "FINALIZADO").ToList();
            _test_finalizados = _test_finalizados.Where(_ => !_.Huella_Digital.Contains("Examen finalizado por autocorrección")).ToList();

            /// 3.1.- Pintar los datos
            test_end_number.InnerHtml = _test_finalizados.Count.ToString();

            decimal _procentaje_finalizados = new decimal(0);
            if (_test_iniciados.Count > 0)
                _procentaje_finalizados = Math.Round((((decimal)_test_finalizados.Count / _test_iniciados.Count) * 100), 2);
            test_end_percent.InnerHtml = _procentaje_finalizados + "%";
        }

        private string load_data(List<campus_ACCIONES_PERSONA> _actions, List<Test_Examen> _test_realizados, List<Test_Test> _tests)
        {
            /// 0.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Test\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>Nombre Test</th>");
            sbuild.Append("<th>Alcance</th>");
            sbuild.Append("<th>Test Ini.</th>");
            sbuild.Append("<th>CR</th>");
            sbuild.Append("<th>Test Fin</th>");
            sbuild.Append("<th>CR</th>");
            sbuild.Append("<th>Leads</th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");
            sbuild.Append("<th class='center'>Total:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer los test
            if (_test_realizados.Count > 0)
            {
                /// 3.1.- Alumnos
                List<long> _id_users = _test_realizados.Select(_ => _.Id_Alumno).Distinct().ToList();
                List<CLIENTES> _users = da.getUserByList(_id_users);

                /// 3.2.- Recorrer los test
                foreach (var _test in _tests)
                {
                    int _alcance = _actions.Where(_ => _.utm_content.Equals(_test.Id.ToString())).Count();
                    List<Test_Examen> _test_iniciados = _test_realizados.Where(_ => _.Id_Test == _test.Id).ToList();
                    List<Test_Examen> _test_finalizados = _test_iniciados.Where(_ => _.Estado == "FINALIZADO" && !_.Huella_Digital.Contains("Examen finalizado por autocorrección")).ToList();

                    decimal _porcentaje_iniciados = new decimal(0);
                    if (_alcance > 0)
                        _porcentaje_iniciados = Math.Round((((decimal)_test_iniciados.Count / _alcance) * 100), 2);
                    decimal _porcentaje_finalizados = new decimal(0);
                    if (_test_iniciados.Count > 0)
                        _porcentaje_finalizados = Math.Round((((decimal)_test_finalizados.Count / _test_iniciados.Count) * 100), 2);

                    List<long> _id_users_filter = _test_iniciados.Select(_ => _.Id_Alumno).Distinct().ToList();
                    DateTime _fecha_min = DateTime.Parse(ConfigurationManager.AppSettings["fecha_evalua"]);
                    int _leads = _users.Where(_ => _id_users_filter.Contains(_.id_cliente) && _.email != "testdigital@test.es" && _.fecha_alta >= _fecha_min).Count();

                    sbuild.Append("<tr>");
                    sbuild.Append($"<td>{_test.Nombre}</td>");
                    sbuild.Append($"<td>{_alcance}</td>");
                    sbuild.Append($"<td>{_test_iniciados.Count}</td>");
                    sbuild.Append($"<td>{_porcentaje_iniciados}</td>");
                    sbuild.Append($"<td>{_test_finalizados.Count}</td>");
                    sbuild.Append($"<td>{_porcentaje_finalizados}</td>");
                    sbuild.Append($"<td>{_leads}</td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
    }
}