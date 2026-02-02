using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class cursos_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();
        private int _id;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                {
                    Session.Add("usuario", list_user[0]);
                }
            }

            if (list_user.Count == 0)
            {
                if (list_user.Count == 0 && Session["usuario"] != null)
                    list_user.Add((CLIENTES)Session["usuario"]);
                else
                    Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                LoadCombos();
                _id = !String.IsNullOrEmpty(Request.QueryString["id"]) ? int.Parse(Request.QueryString["id"].ToString()) : -1;
                txtidcurso.Value = _id.ToString();
                if (_id > 0)
                {
                    var campuscurso = da.getCourseById(_id).FirstOrDefault();
                    if (campuscurso != null)
                    {
                        txt_codigo.Value = campuscurso.COD_Curso;
                        txt_nombre.Value = campuscurso.Nombre;
                        txt_FechaAlta.Value = campuscurso.FAlta.ToString();
                        txtFechaBaja.Value = campuscurso.FBaja.ToString();
                        chkActivo.Checked = campuscurso.Activo;
                        txt_descripcion.Value = campuscurso.Descripcion;
                        version.Value = campuscurso.Version.ToString();
                        sesiones.Value = campuscurso.Num_Sesiones.ToString();
                        horas.Value = campuscurso.numHoras.ToString();
                        duracion.Value = campuscurso.duracion.ToString();
                        dias.Value = campuscurso.plan_num_dias.ToString();
                        chk_universitario.Checked = campuscurso.universitario.HasValue && campuscurso.universitario.Value;
                        titulo_oficial.Value = campuscurso.TIT_Oficial;
                        contenido_oficial.Value = campuscurso.CONT_Oficial;
                        ddltipos.Value = campuscurso.idTipo_Curso != null ? campuscurso.idTipo_Curso.Value.ToString() : "0";
                        ddlmetodologia.Value = campuscurso.idMetodologia != null ? campuscurso.idMetodologia.Value.ToString() : "0"; ;
                        ddldificultad.Value = campuscurso.idDificultad != null ? campuscurso.idDificultad.Value.ToString() : "0"; ;
                        PintarAutores();
                        LoadContenidos();
                    }
                    else
                    {
                        txt_error.InnerHtml = "Curso no encontrado";
                    }
                }
                PintarTablasAreas();
                PintarTablasTematicas();
            }
        }

        [WebMethod]
        public static long SaveCurso(long id, string codigo, string nombre, DateTime fechaAlta, DateTime? fechaBaja, string descripcion, int version, int sesiones, int horas, int duracion,
            int dias, string titulooficial, string contenidooficial, int ddltipos, int ddlmetodologia, int ddldificultad, int[] autores, decimal[] autorespct, bool chkpublicar,
            int[] idsareas, int[] idstematicas, bool chkactivo, bool chkuniversitario, string programapdf, string programapdfcompleto, string programapdfweb)
        {
            DataAccess localda = new DataAccess();
            campus_CURSO curso = new campus_CURSO();
            if (id > 0)
                curso = localda.getCourseById(id).FirstOrDefault();

            curso.COD_Curso = codigo;
            curso.Nombre = nombre;
            curso.FAlta = fechaAlta;
            curso.FBaja = fechaBaja;
            curso.Descripcion = descripcion;
            curso.Version = version;
            curso.Num_Sesiones = sesiones;
            curso.numHoras = horas;
            curso.duracion = duracion;
            curso.plan_num_dias = dias;
            curso.TIT_Oficial = titulooficial;
            curso.CONT_Oficial = contenidooficial;
            curso.idTipo_Curso = ddltipos;
            curso.idMetodologia = ddlmetodologia;
            curso.idDificultad = ddldificultad;
            curso.Activo = chkactivo;
            curso.universitario = chkuniversitario;

            if (!string.IsNullOrEmpty(programapdf))
            {
                curso.ProgramaPDF = curso.COD_Curso + ".pdf";
                File.WriteAllBytes(ConfigurationManager.AppSettings["ruta_programas"] + curso.ProgramaPDF, Convert.FromBase64String(programapdf.Split(';')[1].Split(',')[1]));
            }

            if (!string.IsNullOrEmpty(programapdfcompleto))
            {
                curso.ProgramaPDFCompleto = curso.COD_Curso + "completo.pdf";
                File.WriteAllBytes(ConfigurationManager.AppSettings["ruta_programas_web"] + curso.ProgramaPDFCompleto, Convert.FromBase64String(programapdfcompleto.Split(';')[1].Split(',')[1]));
            }

            if (!string.IsNullOrEmpty(programapdfweb))
            {
                curso.ProgramaPDFWeb = curso.COD_Curso + "web.pdf";
                File.WriteAllBytes(ConfigurationManager.AppSettings["url_programas_web"] + curso.ProgramaPDFWeb, Convert.FromBase64String(programapdfweb.Split(';')[1].Split(',')[1]));
            }

            if (id <= 0)
                localda.insertCurso(curso);
            else
                localda.updateCurso(curso);

            bool diferents = false;
            IEnumerable<int> currentareas = localda.getAreaFuncionalByIdCurso(curso.ID_Curso).Select(_ => _.idArea);
            foreach (var item in idsareas)
            {
                if (!currentareas.Any(_ => _ == item))
                {
                    diferents = true;
                    break;
                }
            }

            foreach (var item in currentareas)
            {
                if (!idsareas.Any(_ => _ == item))
                {
                    diferents = true;
                    break;
                }
            }

            if (diferents)
            {
                localda.DeleteCursoAreaByCursoId(curso.ID_Curso);
                localda.AddCursoAreaRange(curso.ID_Curso, idsareas);
            }

            //tematicas
            diferents = false;
            IEnumerable<int> currenttematicas = localda.getTematicaByIdCurso(curso.ID_Curso).Select(_ => _.idTematica);
            foreach (var item in idstematicas)
            {
                if (!currenttematicas.Any(_ => _ == item))
                {
                    diferents = true;
                    break;
                }
            }

            foreach (var item in currenttematicas)
            {
                if (!idstematicas.Any(_ => _ == item))
                {
                    diferents = true;
                    break;
                }
            }

            if (diferents)
            {
                localda.DeleteCursoTematicaByCursoId(curso.ID_Curso);
                localda.AddCursoTematicaRange(curso.ID_Curso, idstematicas);
            }

            //autorias
            diferents = false;
            var currentautorias = localda.getAutorias(curso.ID_Curso);
            for (int i = 0; i < autores.Length; i++)
            {
                if (!currentautorias.Any(_ => _.idAutor == autores[i] && _.Pct == autorespct[i]))
                {
                    diferents = true;
                    break;
                }
            }

            foreach (var item in currentautorias)
            {
                if (!autores.Any(_ => _ == item.idAutor))
                {
                    diferents = true;
                    break;
                }
            }

            if (diferents)
            {
                localda.DeleteCursoAutoriaByCursoId(curso.ID_Curso);
                localda.AddCursoAutoriaRange(curso.ID_Curso, autores, autorespct);
            }

            return curso.ID_Curso;
        }

        [WebMethod]
        public static string GetLiterales(long id)
        {
            var result = new List<string>();
            DataAccess localda = new DataAccess();
            var literales = localda.getLiterals(id).OrderBy(_ => _.Valor1).ToList();
            result.AddRange(literales.Select(_ => _.Descripcion));
            var curso = localda.getCourseById(id).FirstOrDefault();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < literales.Count(); i++)
            {
                string html = "<div id='#idcontainer#' class='col-sm-12 form-group'><div class='col-sm-3 from-group'><label>#label#</label></div><div class='col-sm-8 #has-error#'><input id='#idimput#' class='form-control' value='#inputvalue#' /></div>#btn#</div>";
                html = html.Replace("#idcontainer#", $"literal{i + 1}");
                html = html.Replace("#label#", $"Sesión {i + 1} : ");
                html = html.Replace("#idimput#", $"iliteral{i + 1}");
                html = html.Replace("#inputvalue#", literales[i].Descripcion);
                if (i >= (curso.Num_Sesiones))
                {
                    html = html.Replace("#btn#", $"<div class='col-sm-1'><button type='button' class='btn btn-danger' onclick='onremoveliteral({literales[i].ID_Literales},\"literal{i + 1}\")'><i class='fas fa-trash'></i></button></div>");
                    html = html.Replace("#has-error#", "has-error");
                }
                else
                {
                    html = html.Replace("#btn#", "");
                    html = html.Replace("#has-error#", "");
                }
                sb.Append(html);
            }
            if (curso.Num_Sesiones > literales.Count())
            {
                var counter = curso.Num_Sesiones - result.Count;
                for (int i = literales.Count(); i < curso.Num_Sesiones; i++)
                {
                    string html = "<div id='#idcontainer#' class='col-sm-12 form-group'><div class='col-sm-3 from-group'><label>#label#</label></div><div class='col-sm-8 #has-error#'><input id='#idimput#' class='form-control' value='#inputvalue#' /></div></div>";
                    html = html.Replace("#idcontainer#", $"literal{i + 1}");
                    html = html.Replace("#label#", $"Sesión {i + 1} : ");
                    html = html.Replace("#idimput#", $"iliteral{i + 1}");
                    html = html.Replace("#inputvalue#", "");

                    sb.Append(html);
                }
            }

            return sb.ToString();
        }

        [WebMethod]
        public static void SaveLiterales(long id, string[] literales)
        {
            DataAccess localda = new DataAccess();
            for (int i = 0; i < literales.Length; i++)
            {
                var literal = localda.getLiterals(id, i + 1);
                if (literal != null)
                {
                    literal.Descripcion = literales[i];
                    localda.updateCampusLiterales(literal);
                }
                else
                {
                    campus_LITERALES nueva = new campus_LITERALES
                    {
                        ID_Padre = id,
                        Descripcion = literales[i],
                        Valor1 = i + 1
                    };

                    localda.insertCampusLiterales(nueva);
                }
            }
        }

        [WebMethod]
        public static void AddContenidoRecurso(long idcurso, int sesion, bool lectura, long[] recursos)
        {
            DataAccess localda = new DataAccess();
            for (int i = 0; i < recursos.Length; i++)
            {
                campus_CONTENIDO_CURSO contenido = new campus_CONTENIDO_CURSO
                {
                    ID_Curso = idcurso,
                    ID_Recurso = recursos[i],
                    Sesion = sesion,
                    Lectura = lectura ? 1 : 0,
                    activo = true
                };

                localda.insertContentCurso(contenido);
            }
        }

        [WebMethod]
        public static void DeleteLiterales(long idcurso, decimal id)
        {
            DataAccess localda = new DataAccess();

            localda.removeLiteral(idcurso, id, true);
        }

        [WebMethod]
        public static string DeleteContenidoRecurso(long idcurso, long idcontenido)
        {
            DataAccess localda = new DataAccess();
            localda.deleteContenidoCurso(idcontenido);

            return cursos_mantenimiento.RenderHtmlContenidos(idcurso);
        }

        private void LoadCombos()
        {

            var autores = da.getProfesores().OrderBy(_ => _.Nombre_Completo).ToList();
            autores.Insert(0, new CLIENTES { Nombre_Completo = "Seleccione un autor" });

            this.ddlautor.DataSource = autores;
            this.ddlautor.DataTextField = "Nombre_Completo";
            this.ddlautor.DataValueField = "id_cliente";
            this.ddlautor.DataBind();
            this.ddlautor.Value = "0";

        }

        private void LoadContenidos()
        {
            contenidoscontainer.InnerHtml = cursos_mantenimiento.RenderHtmlContenidos(_id);
        }

        private void PintarTablasAreas()
        {
            List<sbs2_area_funcional> list = da.getAreasFuncionales().OrderBy(_ => _.nombre).ToList();
            var current = da.getAreaFuncionalByIdCurso(_id);
            PaintAreas(list, current);
        }

        private void PaintTableAllAreas(List<sbs2_area_funcional> list)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id='table_all_areas' class=\"display compact\" style =\"width:100%\">");
            sbuild.Append("<thead>");
            //Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th>Área</th>");

            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var item in list)
            {
                sbuild.Append($"<tr data-id='{item.idArea}'>");
                sbuild.Append("<td><input type=\"checkbox\" value=\"" + item.idArea + "\" onclick=\"\"></td>");
                sbuild.Append("<td>" + item.nombre + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody>");
            sbuild.Append("</table>");

            //tabla_all_areas.InnerHtml = sbuild.ToString();
        }

        private void PintarTablasTematicas()
        {
            List<sbs2_tematica> list = da.getTematicas().OrderBy(_ => _.nombre).ToList();
            var current = da.getTematicaByIdCurso(_id);
            PaintTematicas(list, current);
        }

        private void PintarAutores()
        {
            List<campus_AUTORIA> list = da.getAutorias(_id);
            foreach (var item in list)
            {
                StringBuilder sbuild = new StringBuilder();
                var profesor = da.getUserById(item.idAutor).FirstOrDefault();
                var nombre = profesor != null ? profesor.Nombre_Completo : string.Empty;
                sbuild.Append($"<span class='badge badge-primary badgeareas' id='{item.idAutor}' data-pc='{item.Pct.Value}' >{nombre} ({(int)item.Pct} %)<i class='fa fa-trash ml-5px' onclick='deleteautor({item.idAutor})'></i></span>");

                autorescontainers.InnerHtml = sbuild.ToString();
            }

        }

        private void PaintAreas(List<sbs2_area_funcional> list, List<sbs2_area_funcional> current)
        {
            //<span class='badge badge-primary badgeareas' data-id=''></span>
            StringBuilder sbuild = new StringBuilder();
            var primarys = list.Intersect(current).Select(_ => _.idArea);
            foreach (var item in list)
            {
                string primary = primarys.Contains(item.idArea) ? "badge-primary" : "";

                sbuild.Append($"<span class='badge {primary} badgeareas' id='{item.idArea}' onclick='clickarea({item.idArea})'>{item.nombre}</span>");
            }
            areascontainer.InnerHtml = sbuild.ToString();
        }

        private void PaintTematicas(List<sbs2_tematica> list, List<sbs2_tematica> current)
        {
            StringBuilder sbuild = new StringBuilder();
            var primarys = list.Intersect(current).Select(_ => _.idTematica);
            foreach (var item in list)
            {
                string primary = primarys.Contains(item.idTematica) ? "badge-primary" : "";
                sbuild.Append($"<span class='badge {primary} badgeareas' id='{item.idTematica}' onclick='clicktematica({item.idTematica})'>{item.nombre}</span>");

            }
            tematicascontainer.InnerHtml = sbuild.ToString();
        }

        [WebMethod]
        public static string RenderHtmlContenidos(long idcurso)
        {
            DataAccess localda = new DataAccess();
            StringBuilder sb = new StringBuilder();
            var curso = localda.getCourseById(idcurso).FirstOrDefault();
            var sesiones = localda.getLiterals(idcurso).OrderBy(_ => _.Valor1).ToList();
            var contenidos = localda.getContentCourse(idcurso);
            for (int i = 0; i < sesiones.Count; i++)
            {
                string html = "";
                if (i >= (curso.Num_Sesiones))
                {
                    html += $"<span style='font-size:24px; color:red'>Sesión {sesiones[i].Valor1}: {sesiones[i].Descripcion}";
                    html += $"<div class='contenidolistcontainer' style='color:red'>";
                }
                else
                {
                    html += $"<span style='font-size:24px'>Sesión {sesiones[i].Valor1}: {sesiones[i].Descripcion} <div class='btn-group pull-right' style='padding-right: 30px;' role='group' aria-label='...'>";
                    html += $"<a><button style='margin-right: 5px;' type = 'button' class='btn' title='Seleccionar recurso existente' onclick='addexistingcontent({sesiones[i].Valor1})'>Seleccionar recurso existente</button></a>";
                    html += $"<a href='recurso-mantenimiento.aspx?idcr={curso.ID_Curso}&crsesion={sesiones[i].Valor1}&crlectura=0&returnto=cursos-mantenimiento.aspx?id={curso.ID_Curso}'><button style='margin-right: 5px;' type = 'button' class='btn' title='Añadir nuevo recurso'>Añadir nuevo recurso</button></a>";
                    html += $"<a href='recurso-mantenimiento.aspx?idcr={curso.ID_Curso}&crsesion={sesiones[i].Valor1}&crlectura=1&returnto=cursos-mantenimiento.aspx?id={curso.ID_Curso}'><button style='margin-right: 5px;' type = 'button' class='btn' title='Añadir nueva lectura'>Añadir nueva lectura</button></a></div></span></span>";
                    html += $"<div class='contenidolistcontainer'>";
                }
                html += "<ul class='list-group nobullets'>";
                foreach (var contenido in contenidos.Where(_ => _.Sesion == sesiones[i].Valor1 && _.Lectura == 0))
                {
                    var recurso = localda.getResourcesById(contenido.ID_Recurso).FirstOrDefault();

                    html += $"<li class='list-group-item'>{recurso.Titulo} <div class='btn-group pull-right' role='group' aria-label='...'>";
                    switch (recurso.ID_Tipo_Recurso)
                    {
                        case 1:
                            html += "<button type = 'button' class='btn' title='NOTA TECNICA'><i class='far fa-file-pdf'></i></button>";
                            break;
                        case 2:
                            html += "<button type = 'button' class='btn' title='CASO PRACTICO'><i class='far fa-question-circle'></i></button>";
                            break;
                        case 3:
                            html += "<button type = 'button' class='btn' title='MULTIMEDIA'><i class='fa fa-film'></i></button>";
                            break;
                        case 4:
                            html += "<button type = 'button' class='btn' title='PRUEBA/EXAMEN'><i class='far fa-question-square'></i></button>";
                            break;
                        case 5:
                            html += "<button type = 'button' class='btn' title='WEBINAR'><i class='fa fa-webcam'></i></button>";
                            break;
                        case 6:
                            html += "<button type = 'button' class='btn' title='MASTERCLASS'><i class='fa fa-chalkboard-teacher'></i></button>";
                            break;
                        case 7:
                            html += "<button type = 'button' class='btn' title='CASO PRACTICO SOLUCION'><i class='far fa-tools'></i></button>";
                            break;
                        default:
                            break;
                    }

                    if (recurso.Activo == "1")
                        html += "<button type = 'button' class='btn' title='Editar recurso (Recurso activo)'><i class='fa fa-pen text-success'></i></button>";
                    else
                        html += "<button type = 'button' class='btn' title='Editar recurso (Recurso inactivo)'><i class='fa fa-pen text-danger'></i></button>";
                    

                    if (contenido.activo == true)
                        html += "<button type = 'button' class='btn' title='Editar sesión (Contenido Activo)'><i class='fa fa-marker text-success'></i></button>";
                    else
                        html += "<button type = 'button' class='btn' title='Editar sesión (Contenido Inactivo)'><i class='fa fa-marker text-danger'></i></button>";
                    

                    html += $"<button type = 'button' class='btn' title='Eliminar recurso' onclick='ondeletecontenido({contenido.ID_Contenido_Curso})'><i class='fa fa-trash text-danger'></i></button>";

                    html += "</div></li>";
                }
                html += "</ul>";
                if (contenidos.Any(_ => _.Sesion == sesiones[i].Valor1 && _.Lectura == 1))
                {
                    html += $"<h3 style='font-size:20px'>Lecturas recomendadas</h3>";
                    html += $"<div class=''>";
                    html += "<ul class='list-group nobullets'>";

                    foreach (var lectura in contenidos.Where(_ => _.Sesion == sesiones[i].Valor1 && _.Lectura == 1))
                    {
                        var recurso = localda.getResourcesById(lectura.ID_Recurso).FirstOrDefault();
                        html += $"<li class='list-group-item'>{recurso.Titulo} <div class='btn-group pull-right' role='group' aria-label='...'>";
                        switch (recurso.ID_Tipo_Recurso)
                        {
                            case 1:
                                html += "<button type = 'button' class='btn' title='NOTA TECNICA'><i class='far fa-file-pdf'></i></button>";
                                break;
                            case 2:
                                html += "<button type = 'button' class='btn' title='CASO PRACTICO'><i class='far fa-question-circle'></i></button>";
                                break;
                            case 3:
                                html += "<button type = 'button' class='btn' title='MULTIMEDIA'><i class='fa fa-film'></i></button>";
                                break;
                            case 4:
                                html += "<button type = 'button' class='btn' title='PRUEBA/EXAMEN'><i class='far fa-question-square'></i></button>";
                                break;
                            case 5:
                                html += "<button type = 'button' class='btn' title='WEBINAR'><i class='fa fa-webcam'></i></button>";
                                break;
                            case 6:
                                html += "<button type = 'button' class='btn' title='MASTERCLASS'><i class='fa fa-chalkboard-teacher'></i></button>";
                                break;
                            case 7:
                                html += "<button type = 'button' class='btn' title='CASO PRACTICO SOLUCION'><i class='far fa-tools'></i></button>";
                                break;
                            default:
                                break;
                        }

                        if (recurso.Activo == "1")
                            html += "<button type = 'button' class='btn' title='Editar recurso (Recurso activo)'><i class='fa fa-pen text-success'></i></button>";
                        else
                            html += "<button type = 'button' class='btn' title='Editar recurso (Recurso inactivo)'><i class='fa fa-pen text-danger'></i></button>";


                        if (lectura.activo == true)
                            html += "<button type = 'button' class='btn' title='Editar sesión (Contenido Activo)'><i class='fa fa-marker text-success'></i></button>";
                        else
                            html += "<button type = 'button' class='btn' title='Editar sesión (Contenido Inactivo)'><i class='fa fa-marker text-danger'></i></button>";


                        html += $"<button type = 'button' class='btn' title='Eliminar recurso' onclick='ondeletecontenido({lectura.ID_Contenido_Curso})'><i class='fa fa-trash text-danger'></i></button>";
                        html += "</div></li>";

                    }
                    html += "</ul>";
                    html += "</div>";
                }
                html += "</div>";
                sb.Append(html);
            }

            return sb.ToString();
        }

        [WebMethod]
        public static string RenderHtmlRecursos(long idcurso)
        {
            DataAccess localda = new DataAccess();
            var recursos = localda.getResources();
            var contenidos = localda.getContentCourse(idcurso);
            var contentsid = contenidos.Select(_ => _.ID_Recurso);
            recursos = recursos.Where(_ => !contentsid.Contains(_.ID_Recurso)).ToList();
            var areas = localda.getAreasFuncionales();
            var tipos = localda.getTypeResource();

            StringBuilder sb = new StringBuilder("<table id =\"tabla_resources\" class=\"display compact\" style =\"width:100%\"><thead>");
            sb.Append("<tr>");
            sb.Append("<th></th>");
            sb.Append("<th>Cod. Recurso</th>");
            sb.Append("<th>Titulo</th>");
            sb.Append("<th>Tipo</th>");
            sb.Append("<th>Área Funcional</th>");
            sb.Append("<th>Activo</th>");
            sb.Append("<th>Versión</th>");
            sb.Append("</tr></thead><tbody>");

            foreach (var item in recursos)
            {
                var tiporecurso = tipos.FirstOrDefault(_ => _.ID_Tipo_Recurso == item.ID_Tipo_Recurso) != null ? tipos.FirstOrDefault(_ => _.ID_Tipo_Recurso == item.ID_Tipo_Recurso).Nombre : "";
                var area = areas.FirstOrDefault(_ => _.idArea == item.ID_Area_Funcional) != null ? areas.FirstOrDefault(_ => _.idArea == item.ID_Area_Funcional).nombre : "";
                var activo = (item.Activo == "1") ? "Si" : "No";
                sb.Append("<tr>");
                sb.Append($"<td><input type='checkbox' data-id='{item.ID_Recurso}' onchange='onchangeresourcechk({item.ID_Recurso})'></input></td>");
                sb.Append($"<td>{item.COD_Recurso}</td>");
                sb.Append($"<td>{item.Titulo}</td>");
                sb.Append($"<td>{tiporecurso}</td>");
                sb.Append($"<td>{area}</td>");
                sb.Append($"<td>{activo}</td>");
                sb.Append($"<td>{item.Version}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody></table>");
            return sb.ToString();
        }

        [WebMethod]
        public static void Deleteprograma(long idcurso, string tipo)
        {
            DataAccess localda = new DataAccess();

            var curso = localda.getCourseById(idcurso).FirstOrDefault();

            if (curso != null)
            {
                switch (tipo)
                {
                    case "pdf":
                        if (!string.IsNullOrEmpty(curso.ProgramaPDF))
                        {
                            var ruta = ConfigurationManager.AppSettings["ruta_programas"];
                            File.Delete(Path.Combine(ruta, curso.ProgramaPDF));
                            curso.ProgramaPDF = null;
                            localda.updateCurso(curso);
                        }
                        break;
                    case "pdfcompleto":
                        if (!string.IsNullOrEmpty(curso.ProgramaPDFCompleto))
                        {
                            var ruta = ConfigurationManager.AppSettings["ruta_programas_web"];
                            File.Delete(Path.Combine(ruta, curso.ProgramaPDFCompleto));
                            curso.ProgramaPDFCompleto = null;
                            localda.updateCurso(curso);
                        }
                        break;
                    case "web":
                        if (!string.IsNullOrEmpty(curso.ProgramaPDFWeb))
                        {
                            var ruta = ConfigurationManager.AppSettings["ruta_programas_web"];
                            File.Delete(Path.Combine(ruta, curso.ProgramaPDFWeb));
                            curso.ProgramaPDFWeb = null;
                            localda.updateCurso(curso);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                throw new Exception("curso no encontrado");
            }
        }
    }
}