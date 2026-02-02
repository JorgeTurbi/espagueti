using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class test_listado : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// 1.- Poner fechas
                date_end.Value = DateTime.Today.ToShortDateString();
                date_start.Value = DateTime.Today.AddDays(-30).ToShortDateString();

                /// 2.- Poner el nombre del test en el título
                int idTest = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;
                if (idTest > 0)
                {
                    Test_Test test = da.getTest_TestById(idTest);
                    if (test != null)
                        txt_title.InnerHtml = "<i class='far fa-list-alt'></i> Listado de test/exámenes realizados de " + test.Nombre + " <span class='badge badge-pill badge-dark pull-right'>" + test.Tiempo + " minutos.</span>";                        
                }
            }
        }

        protected void img_filter_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Paramétros del formulario
            int idTest = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;
            DateTime _fecha_inicio = DateTime.Parse(date_start.Value);
            DateTime _fecha_fin = DateTime.Parse(date_end.Value).AddDays(1);

            /// 2.- Sacar los test entre fechas
            List<Test_Examen> _examenes = da.getTest_Examenes(idTest, _fecha_inicio, _fecha_fin);

            /// 3.- Cargar los examenes
            table_listado_examenes.InnerHtml = load_test(_examenes);
        }
        protected void btnBorrar_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Paramétros del formulario
            int idTest = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;
            long idUsuario = long.Parse(hidIdUser.Value);
            short num_intento = short.Parse(hidIntento.Value);

            /// 2.- Buscar los examenes del usuario
            List<Test_Examen> _examenes = da.getTest_ExamenByIdTestAlumno(idUsuario, idTest);
            if (_examenes.Count > 0)
            {
                /// 2.1.- Filtrar los examenes por nº de intento
                _examenes = _examenes.Where(_ => _.Num_Intento >= num_intento).ToList();
                if (_examenes.Count == 1)
                {
                    /// 2.2.- Eliminar las preguntas del examen
                    List<Test_Examen_Contenido> _preguntas = da.getTest_Examen_Contenido(idUsuario, idTest, num_intento);
                    if (_preguntas.Count > 0)
                    {
                        foreach (var _pregunta in _preguntas)
                        {
                            da.Delete(_pregunta);
                        }
                    }

                    /// 2.3.- Eliminar el examen
                    da.DeleteTest_Examen(_examenes[0]);
                }
                else if (_examenes.Count > 1)
                {
                    /// 2.2.- Sacar el examen
                    List<Test_Examen> _examen_intento = _examenes.Where(_ => _.Num_Intento == num_intento).ToList();
                    if (_examen_intento.Count == 1)
                    {
                        /// 2.2.1.- Eliminar las preguntas del examen
                        List<Test_Examen_Contenido> _preguntas = da.getTest_Examen_Contenido(idUsuario, idTest, num_intento);
                        if (_preguntas.Count > 0)
                        {
                            foreach (var _pregunta in _preguntas)
                            {
                                da.Delete(_pregunta);
                            }
                        }

                        /// 2.2.2.- Eliminar el examen
                        da.DeleteTest_Examen(_examen_intento[0]);
                    }

                    /// 2.3.- Actualizar los examenes
                    _examenes = _examenes.Where(_ => _.Num_Intento != num_intento).OrderBy(_ => _.Num_Intento).ToList();
                    if (_examenes.Count > 0)
                    {
                        foreach (var _examen in _examenes)
                        {
                            /// 2.3.1.- Añadir preguntas
                            List<Test_Examen_Contenido> _preguntas = da.getTest_Examen_Contenido(idUsuario, idTest, _examen.Num_Intento);
                            if (_preguntas.Count > 0)
                            {
                                foreach (var _pregunta in _preguntas)
                                {
                                    Test_Examen_Contenido _pregunta_new = new Test_Examen_Contenido();
                                    _pregunta_new.Id_Alumno = _pregunta.Id_Alumno;
                                    _pregunta_new.Id_Test = _pregunta.Id_Test;
                                    _pregunta_new.Num_Intento = num_intento;
                                    _pregunta_new.Orden = _pregunta.Orden;
                                    _pregunta_new.Id_Pregunta = _pregunta.Id_Pregunta;
                                    _pregunta_new.Respuesta = _pregunta.Respuesta;
                                    _pregunta_new.FechaHora_Entrega = _pregunta.FechaHora_Entrega;
                                    _pregunta_new.Huella_Digital = _pregunta.Huella_Digital;
                                    _pregunta_new.Ok = _pregunta.Ok;
                                    da.insertTest_Examen_Contenido(_pregunta_new);
                                }
                            }

                            /// 2.3.2.- Añadir examenes
                            Test_Examen _examen_new = new Test_Examen();
                            _examen_new.Id_Alumno = _examen.Id_Alumno;
                            _examen_new.Id_Test = _examen.Id_Test;
                            _examen_new.Num_Intento = num_intento;
                            _examen_new.Fecha = _examen.Fecha;
                            _examen_new.Id_Docencia = _examen.Id_Docencia;
                            _examen_new.Id_Curso = _examen.Id_Curso;
                            _examen_new.FechaHora_Inicio = _examen.FechaHora_Inicio;
                            _examen_new.FechaHora_Fin = _examen.FechaHora_Fin;
                            _examen_new.Puntuacion = _examen.Puntuacion;
                            _examen_new.Nivel_Dificultad = _examen.Nivel_Dificultad;
                            _examen_new.Minutos = _examen.Minutos;
                            _examen_new.Apto = _examen.Apto;
                            _examen_new.Huella_Digital = _examen.Huella_Digital;
                            _examen_new.Estado = _examen.Estado;
                            _examen_new.Nota_Normalizada = _examen.Nota_Normalizada;
                            _examen_new.idMail = _examen.idMail;

                            da.insertTest_Examen(_examen_new);

                            /// 2.3.3.- Eliminar preguntas
                            List<Test_Examen_Contenido> _preguntas_old = da.getTest_Examen_Contenido(idUsuario, idTest, _examen.Num_Intento);
                            if (_preguntas_old.Count > 0)
                            {
                                foreach (var _pregunta_old in _preguntas_old)
                                {
                                    da.Delete(_pregunta_old);
                                }
                            }

                            /// 2.3.4.- Eliminar el examen
                            da.DeleteTest_Examen(_examen);

                            num_intento++;
                        }
                    }
                }
            }

            Response.Redirect("test-listado.aspx?id=" + idTest);
        }

        private string load_test(List<Test_Examen> _examenes)
        {
            /// 0.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_List_Test\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.- Cabecera y pie
            sbuild.Append("<tr>");
            sbuild.Append("<th>F. Examen</th>");
            sbuild.Append("<th>Alumno</th>");
            sbuild.Append("<th>Nota</th>");
            sbuild.Append("<th>Intento</th>");
            sbuild.Append("<th>F. Inicio</th>");
            sbuild.Append("<th>F. Fin</th>");
            sbuild.Append("<th>Min</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</thead><tfoot>");
            sbuild.Append("<tr>");            
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th class='center'>Media:</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("</tfoot><tbody>");

            /// 3.- Recorrer los examenes
            if (_examenes.Count > 0)
            {
                /// 3.1.- Alumnos
                List<long> _id_users = _examenes.Select(_ => _.Id_Alumno).Distinct().ToList();
                List<CLIENTES> _users = da.getUserByList(_id_users);

                /// 3.2.- Recorrer los test
                foreach (var _test in _examenes)
                {
                    string _nombre_completo = _users.Where(_ => _.id_cliente == _test.Id_Alumno).Select(_ => _.Nombre_Completo).FirstOrDefault();
                    if (String.IsNullOrEmpty(_nombre_completo))
                    {
                        List<CLIENTES> _usuario = _users.Where(_ => _.id_cliente == _test.Id_Alumno).ToList();
                        if (_usuario.Count == 1)
                            _nombre_completo = _usuario[0].nombre + " " + _usuario[0].apellidos;
                    }

                    sbuild.Append("<tr>");
                    sbuild.Append($"<td>{_test.Fecha.ToShortDateString()}</td>");
                    sbuild.Append($"<td><a href='ficha-alumno-crm.aspx?idu={_test.Id_Alumno}' target='_blank'><i class='fas fa-user fa-1-4x'></i>{_nombre_completo} ({_test.Id_Alumno})</a></td>");
                    sbuild.Append($"<td>{_test.Nota_Normalizada}</td>");
                    sbuild.Append($"<td>{_test.Num_Intento}</td>");
                    sbuild.Append($"<td>{_test.FechaHora_Inicio}</td>");
                    sbuild.Append($"<td>{_test.FechaHora_Fin}</td>");
                    sbuild.Append($"<td>{(_test.FechaHora_Fin != null ? _test.FechaHora_Fin.Value.Subtract(_test.FechaHora_Inicio.Value).Minutes : 0)}</td>");
                    sbuild.Append($"<td><a href='https://campus.spainbs.com/contenido-examen-smart.aspx?it={_test.Id_Test}&ni={_test.Num_Intento}&key=FBB14E99-6361-4A57-B325-526AAC88DE9D&idu={_test.Id_Alumno}' title='Ver test' target='_blank'><i class='fas fa-tasks fa-1-6x v-top text-color-green'></i></a></td>");

                    sbuild.Append("<td><a href='javascript:void(0)' title='Ver huella' onclick=\"add_huella(`" + huella_test(_test.Id_Alumno, _test.Id_Test, _test.Num_Intento, _nombre_completo).Replace('"', '\'') + "`)\"><i class='fas fa-clipboard-list fa-1-6x text-color-green'></i></a></td>");

                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar el test: " + _test.Id_Test + "?\")){eliminar_test(" + _test.Id_Alumno + "," + _test.Num_Intento + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                    sbuild.Append("</tr>");
                }
            }
            sbuild.Append("</tbody></table>");

            return sbuild.ToString();
        }
        
        private string huella_test(long id_Alumno, int idTest, short numintento, string _nombre_completo)
        {
            StringBuilder sbuild = new StringBuilder();

            /// 1.- Sacar el test
            Test_Examen _examen = da.getTest_ExamenById(id_Alumno, idTest, numintento);
            if (_examen != null)
            {
                /// Sacar los minutos de duración del examen
                Test_Test _test = da.getTest_TestById(idTest);
                int _tiempo_examen = _test != null ? _test.Tiempo : 0;

                DateTime _fecha_creacion = new DateTime();
                if (!String.IsNullOrEmpty(_examen.Huella_Digital))
                    _fecha_creacion = _examen.Huella_Digital.Split('|').Count() > 0 ? DateTime.Parse(_examen.Huella_Digital.Split('|')[0].Replace("Examen creado", string.Empty)) : DateTime.Parse(_examen.Huella_Digital.Replace("Examen creado", string.Empty));

                List<string> _list = _examen.Huella_Digital.Split('|').ToList();
                List<string> _list_sesions = new List<string>();

                foreach (var item in _list)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        if (item.Contains("Examen iniciado-session"))
                            _list_sesions.Add(item);
                    }
                }

                string _sesion = string.Empty;
                if (_list_sesions.Count == 1)
                {
                    foreach (var element in _list_sesions)
                    {
                        List<string> _elements = element.Split('&').ToList();
                        foreach (var _element in _elements)
                        {
                            if (_element.Contains("Examen iniciado-session="))
                                _sesion = _element.Substring((_element.IndexOf('=') + 1)).Trim();
                        }
                    }
                }

                sbuild.Append("<ul class='col-sm-12 list-unstyled'>");
                sbuild.Append($"<li><i class='fa fa-circle text-color-secondary margin-lr-5'></i><b>Creaci&oacute;n de examen:</b> {_fecha_creacion.ToString()}</li>");
                sbuild.Append($"<li><i class='fa fa-circle text-color-secondary margin-lr-5'></i><b>Examen iniciado:</b> {_examen.FechaHora_Inicio}</li>");
                sbuild.Append($"<li><i class='fa fa-circle text-color-secondary margin-lr-5'></i><b>Examen finalizado:</b> {_examen.FechaHora_Fin}</li>");
                sbuild.Append($"<li><i class='fa fa-circle text-color-secondary margin-lr-5'></i><b>Duración examen:</b> {_tiempo_examen} minutos</li>");
                sbuild.Append($"<li><i class='fa fa-circle text-color-secondary margin-lr-5'></i><b>Usuario:</b> {_nombre_completo} ({id_Alumno})</li>");
                sbuild.Append($"<li><i class='fa fa-circle text-color-secondary margin-lr-5'></i><b>Id Sesion:</b> {_sesion}</li>");
                sbuild.Append("</ul>");

                sbuild.Append("<p class='padding-tb-10 px-4'><span class='text-underline'>PREGUNTAS</span></p>");
                sbuild.Append("<div class='px-4 pb-3 clearfix'><ol class='col-sm-12 px-5'>");

                /// Sacar las respuestas del examen 
                List<Test_Examen_Contenido> _respuestas = da.getTest_Examen_Contenido(id_Alumno, idTest, numintento);
                if (_respuestas.Count > 0)
                {
                    foreach (var _respuesta in _respuestas)
                    {
                        List<string> _fechas_respuesta = new List<string>();
                        List<string> _fechas_respuestas = _respuesta.Huella_Digital.Split('|').ToList();
                        foreach (var _fecha_respuesta in _fechas_respuestas)
                        {
                            if (_fecha_respuesta.Contains("Establecida respuesta:"))
                                _fechas_respuesta.Add(_fecha_respuesta);
                        }

                        if (_fechas_respuesta.Count > 0)
                        {
                            sbuild.Append("<li class='w-100'>Pregunta:<br /><ul class='col-sm-12 px-5'>");
                            foreach (var _fecha in _fechas_respuesta)
                            {
                                DateTime _date = DateTime.Parse(_fecha.Substring(0, _fecha.IndexOf("Establecida respuesta:")));
                                string _respuesta_pregunta = _fecha.Substring(_fecha.IndexOf("Establecida respuesta:"));
                                int _minutos = _date.Subtract(_examen.FechaHora_Inicio.Value).Minutes;

                                if (_minutos > _tiempo_examen)
                                    sbuild.Append($"<li class='text-color-red'><b>Fecha respuesta:</b> {_date}&nbsp;&nbsp;&nbsp;({_minutos.ToString("00")} min)&nbsp;&nbsp;&nbsp;<b>Respuesta:</b> {_respuesta_pregunta}</li>");
                                else
                                    sbuild.Append($"<li><b>Fecha respuesta:</b> {_date}&nbsp;&nbsp;&nbsp;({_minutos.ToString("00")} min)&nbsp;&nbsp;&nbsp;<b>Respuesta:</b> {_respuesta_pregunta}</li>");
                            }
                            sbuild.Append("</ul></li>");
                        }
                        else
                            sbuild.Append($"<li><b>Fecha respuesta:</b> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Respuesta:</b></li>");
                    }

                }
                sbuild.Append("</ol></div>");
            }

            return sbuild.ToString();
        }
    }
}