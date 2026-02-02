using DocumentFormat.OpenXml.Spreadsheet;
using sbs_DAL;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class practicas : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el parámetro 'k' en la url
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
                /// Comprobar al usuario
                bool comprobate_user = Utilities.comprobate_user(list_user[0]);
                if (!comprobate_user)
                    Response.Redirect("login.aspx");
                else
                    loadPractices(list_user[0]);
            }
        }
                
        protected void btnBorrarPractica_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_practice = false;

            try
            {
                long idPractice = !String.IsNullOrEmpty(hidIdPractica.Value) ? long.Parse(hidIdPractica.Value) : -1;
                if (idPractice > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<PRA_PRACTICAS> lst_practicas = da.getListPracticesById(idPractice);
                    if (lst_practicas.Count == 1)
                    {
                        /// 2.- Eliminar el fichero 
                        if (!String.IsNullOrEmpty(lst_practicas[0].FicheroAnexo))
                            File.Delete(ConfigurationManager.AppSettings["ruta_practicas"] + lst_practicas[0].FicheroAnexo);

                        /// 3.- Eliminar el contacto
                        delete_practice = da.deletePractice(idPractice);

                        /// 4.- Modificar en empresa el nº de contactos
                        List<PRA_EMPRESA> lst_empresas = da.getBusinessById(lst_practicas[0].idEmpresa);
                        if (lst_empresas.Count == 1)
                        {
                            PRA_EMPRESA empresa = lst_empresas[0];
                            empresa.num_practicas = empresa.num_practicas - 1;

                            bool update_company = da.updateCompany(empresa);
                            if (!update_company)
                            {
                                delete_practice = false;
                                LogUtils.InsertarLog(" ERROR - practicas.cs::btnBorrarPractica_Click()");
                                LogUtils.InsertarLog("- MSG: Se ha producido un error al modificar en empresa el nº de prácticas");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la práctica');</script>");

                LogUtils.InsertarLog(" ERROR - contactos.cs::btnBorrarPractica_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_practice)
                Response.Redirect("practicas.aspx" + (!String.IsNullOrEmpty(Request.QueryString["idb"]) ? "?idb=" + Request.QueryString["idb"] : string.Empty));
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la práctica');</script>");
        }

        protected void btnCrearExcel_Click(object sender, ImageClickEventArgs e)
        {
            /// 0.- Recuperar datos de la url
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;

            /// 1.- Sacar los datos de la BBDD
            List<PRA_PRACTICAS> lst_practicas = da.getListPracticesById(-1);
            List<PRA_CONTACTO> lst_contactos = da.getContactsById(-1);
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(-1);
            List<CLIENTES> lst_users = da.getActiveUsers(true);
            List<campus_CURSO> lst_courses = da.getCourses(null);

            /// 2.- Filtrar los contactos por empresa
            if (idEmpresa > 0)
                lst_practicas = lst_practicas.Where(c => c.idEmpresa == idEmpresa).ToList();

            /// 3.- Recorrer los origenes y crear el excel
            SLDocument sl = new SLDocument();

            SLStyle style = sl.CreateStyle();
            style.Font.FontSize = 14;
            style.Font.FontColor = System.Drawing.Color.White;
            style.Font.Bold = true;
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Black, System.Drawing.Color.White);
            style.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(1, 1, 1, 21, style);

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Prácticas");
            sl.SetCellValue(1, 1, "Fecha Alta");
            sl.SetCellValue(1, 2, "Fecha Baja");
            sl.SetCellValue(1, 3, "Nombre Alumno");
            sl.SetCellValue(1, 4, "Mail Alumno");
            sl.SetCellValue(1, 5, "Tutor escuela");
            sl.SetCellValue(1, 6, "Curso");
            sl.SetCellValue(1, 7, "Tipo");
            sl.SetCellValue(1, 8, "Razón Social Empresa");
            sl.SetCellValue(1, 9, "Tutor Empresa");
            sl.SetCellValue(1, 10, "Duración");
            sl.SetCellValue(1, 11, "Ayuda");
            sl.SetCellValue(1, 12, "Horas semana");
            sl.SetCellValue(1, 13, "Fichero anexo");
            sl.SetCellValue(1, 14, "Comentarios");
            sl.SetCellValue(1, 15, "PDP NumFactura");
            sl.SetCellValue(1, 16, "PDP NumPedido");
            sl.SetCellValue(1, 17, "PDP Precio");
            sl.SetCellValue(1, 18, "PDP Matriculado");
            sl.SetCellValue(1, 19, "PDP Activado");
            sl.SetCellValue(1, 20, "PDP Comentarios");
            sl.SetCellValue(1, 21, "Curriculares");

            int _index = 2;
            foreach (var practica in lst_practicas)
            {
                /// 2.3.1.- Sacar los datos de la empresa y el contacto
                List<PRA_EMPRESA> lst_empresa = lst_empresas.Where(emp => emp.idEmpresa == practica.idEmpresa).ToList();
                List<PRA_CONTACTO> lst_contacto = lst_contactos.Where(cont => cont.idContacto == practica.idTutorEmpresa).ToList();
                List<CLIENTES> lst_teacher = lst_users.Where(c => c.id_cliente == practica.idTutorEscuela).ToList();
                List<CLIENTES> lst_student = lst_users.Where(c => c.id_cliente == practica.idAlumno).ToList();
                List<campus_CURSO> lst_course = lst_courses.Where(c => c.ID_Curso == practica.idCurso).ToList();
                
                sl.SetCellValue(_index, 1, practica.FechaAlta.ToShortDateString());
                sl.SetCellValue(_index, 2, (practica.FechaBaja != null ? practica.FechaBaja.Value.ToShortDateString() : string.Empty));
                
                if (lst_student.Count > 0)
                {
                    sl.SetCellValue(_index, 3, lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "]");
                    sl.SetCellValue(_index, 4, lst_student[0].email);
                }
                else
                {
                    sl.SetCellValue(_index, 3, practica.idAlumno);
                    sl.SetCellValue(_index, 4, string.Empty);
                }

                if (lst_teacher.Count > 0)
                    sl.SetCellValue(_index, 5, lst_teacher[0].Nombre_Completo + " (" + practica.idTutorEscuela + ")");
                else
                    sl.SetCellValue(_index, 5, practica.idTutorEscuela);

                if (lst_course.Count > 0)
                    sl.SetCellValue(_index, 6, lst_course[0].Nombre + " (" + practica.idCurso + ")");
                else
                    sl.SetCellValue(_index, 6, practica.idCurso);

                sl.SetCellValue(_index, 7, practica.Tipo);

                if (lst_empresa.Count > 0)
                    sl.SetCellValue(_index, 8, lst_empresa[0].RazonSocial + " [" + lst_empresa[0].idEmpresa + "]");
                else
                    sl.SetCellValue(_index, 8, lst_empresa[0].idEmpresa);

                if (lst_contacto.Count > 0)
                    sl.SetCellValue(_index, 9, lst_contacto[0].Nombre + " " + lst_contacto[0].Apellidos + " (" + practica.idTutorEmpresa + ")");
                else
                    sl.SetCellValue(_index, 9, practica.idTutorEmpresa);

                sl.SetCellValue(_index, 10, (practica.Duracion == 1 ? practica.Duracion + " mes " : practica.Duracion + " meses "));
                sl.SetCellValue(_index, 11, practica.AyudaEstudioMes + "€ ");
                sl.SetCellValue(_index, 12, practica.HoraSemana + " horas");
                sl.SetCellValue(_index, 13, practica.FicheroAnexo);
                sl.SetCellValue(_index, 14, practica.Comentarios);
                sl.SetCellValue(_index, 15, practica.PDP_NumFactura);
                sl.SetCellValue(_index, 16, practica.PDP_NumPedido);
                sl.SetCellValue(_index, 17, (practica.PDP_Precio != null ? practica.PDP_Precio.Value : 0));
                sl.SetCellValue(_index, 18, (practica.PDP_Matriculado != null ? practica.PDP_Matriculado.Value : false));
                sl.SetCellValue(_index, 19, (practica.PDP_Activado != null ? practica.PDP_Activado.Value : false));
                sl.SetCellValue(_index, 20, practica.PDP_Comentarios);
                sl.SetCellValue(_index, 21, (practica.Curriculares != null ? practica.Curriculares.Value : false));

                _index++;
            }

            sl.Sort(2, 1, _index, 21, 1, true);
            sl.AutoFitColumn(1, 21);

            string filename = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_Practicas.xlsx";

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            sl.SaveAs(Response.OutputStream);
            Response.End();
        }

        protected void btnTodasPracticas_Click(object sender, ImageClickEventArgs e)
        {
            /// 0.- Recuperar datos de la url
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
            int days = 0;

            /// 1.- Sacar los datos de la BBDD
            List<PRA_PRACTICAS> lst_practicas = new List<PRA_PRACTICAS>();
            List<PRA_EMPRESA> lst_empresas = new List<PRA_EMPRESA>();
            if (idEmpresa > 0)
            {
                lst_practicas = da.getListPracticesByIdBusiness(idEmpresa, days);
                lst_empresas = da.getBusinessById(idEmpresa);
            }
            else
            {
                lst_practicas = da.getListPracticesByIdBusiness(-1, days);
                lst_empresas = da.getBusinessById(-1);
            }
            List<PRA_CONTACTO> lst_contactos = da.getContactsById(-1);

            /// 1.1.- Sacar los alumnos
            List<long> _users = lst_practicas.Select(_ => _.idAlumno).Distinct().ToList();
            List<CLIENTES> lst_users = da.getUserByList(_users);
            //List<CLIENTES> lst_users = da.getActiveUsers(true);

            /// 1.2.- Pintar la tabla
            paint_table(lst_practicas, lst_empresas, lst_contactos, lst_users);

            /// 2.- Pintar el título
            if (idEmpresa > 0)
                txt_practicas.InnerHtml = "<i class='fas fa-toolbox'></i> Listado de Prácticas <a href='practica-mantenimiento.aspx?idb=" + idEmpresa + "' title='Nueva práctica' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva práctica</small></a><a href='javascript:void(0);' onclick='create_excel()' title='Crear excel' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='far fa-file-excel fa-2x text-color-green'></i>  Crear excel</small><a href='javascript:void(0);' onclick='all_practices()' title='Todas las prácticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i> Todas las prácticas</small></a>" + paintMonth(string.Empty) + "<a href='javascript:void(0);' onclick='end_practices()' title='Prácticas que finalizan' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-calendar-times fa-2x'></i> Prácticas que finalizan</small></a>";
            else
                txt_practicas.InnerHtml = "<i class='fas fa-toolbox'></i> Listado de Prácticas <a href='practica-mantenimiento.aspx' title='Nueva práctica' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva práctica</small></a><a href='javascript:void(0);' onclick='create_excel()' title='Crear excel' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='far fa-file-excel fa-2x text-color-green'></i>  Crear excel</small></a><a href='javascript:void(0);' onclick='all_practices()' title='Todas las prácticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i> Todas las prácticas</small></a>" + paintMonth(string.Empty) + "<a href='javascript:void(0);' onclick='end_practices()' title='Prácticas que finalizan' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-calendar-times fa-2x'></i> Prácticas que finalizan</small></a>";
        }

        protected void btnFinPracticas_Click(object sender, ImageClickEventArgs e)
        {
            /// 0.- Recuperar datos de la url
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
            int days = 0;

            /// 1.- Sacar los datos de la BBDD
            List<PRA_PRACTICAS> lst_practicas = new List<PRA_PRACTICAS>();
            List<PRA_EMPRESA> lst_empresas = new List<PRA_EMPRESA>();
            if (idEmpresa > 0)
            {
                lst_practicas = da.getListPracticesByIdBusiness(idEmpresa, days);
                lst_empresas = da.getBusinessById(idEmpresa);
            }
            else
            {
                lst_practicas = da.getListPracticesByIdBusiness(-1, days);
                lst_empresas = da.getBusinessById(-1);
            }
            List<PRA_CONTACTO> lst_contactos = da.getContactsById(-1);
            List<CLIENTES> lst_users = da.getActiveUsers(true);

            /// 1.1.- Filtrar las prácticas que finalizan en el mes en curso
            DateTime fecha_ini = new DateTime();
            if (!String.IsNullOrEmpty(hidMonth.Value))
                fecha_ini = new DateTime(DateTime.Today.Year, int.Parse(hidMonth.Value), 1);
            else
                fecha_ini = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            lst_practicas = lst_practicas.Where(pra => pra.FechaBaja != null && pra.FechaBaja.Value >= fecha_ini && pra.FechaBaja.Value < fecha_ini.AddMonths(1)).ToList();
            
            paint_table(lst_practicas, lst_empresas, lst_contactos, lst_users);

            /// 2.- Pintar el título
            if (idEmpresa > 0)
                txt_practicas.InnerHtml = "<i class='fas fa-toolbox'></i> Listado de Prácticas <a href='practica-mantenimiento.aspx?idb=" + idEmpresa + "' title='Nueva práctica' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva práctica</small></a><a href='javascript:void(0);' onclick='create_excel()' title='Crear excel' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='far fa-file-excel fa-2x text-color-green'></i>  Crear excel</small><a href='javascript:void(0);' onclick='all_practices()' title='Todas las prácticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i> Todas las prácticas</small></a>" + paintMonth(hidMonth.Value) + "<a href='javascript:void(0);' onclick='end_practices()' title='Prácticas que finalizan' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-calendar-times fa-2x'></i> Prácticas que finalizan</small></a>";
            else
                txt_practicas.InnerHtml = "<i class='fas fa-toolbox'></i> Listado de Prácticas <a href='practica-mantenimiento.aspx' title='Nueva práctica' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva práctica</small></a><a href='javascript:void(0);' onclick='create_excel()' title='Crear excel' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='far fa-file-excel fa-2x text-color-green'></i>  Crear excel</small></a><a href='javascript:void(0);' onclick='all_practices()' title='Todas las prácticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i> Todas las prácticas</small></a>" + paintMonth(hidMonth.Value) + "<a href='javascript:void(0);' onclick='end_practices()' title='Prácticas que finalizan' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-calendar-times fa-2x'></i> Prácticas que finalizan</small></a>";
        }

        private void loadPractices(CLIENTES user)
        {
            /// 0.- Recuperar datos de la url
            long idEmpresa = !String.IsNullOrEmpty(Request.QueryString["idb"]) ? long.Parse(Request.QueryString["idb"].ToString()) : -1;
            int days = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["dias_practicas"]) ? int.Parse(ConfigurationManager.AppSettings["dias_practicas"]) : 0;

            /// 1.- Sacar los datos de la BBDD
            List<PRA_PRACTICAS> lst_practicas = new List<PRA_PRACTICAS>();
            List<PRA_EMPRESA> lst_empresas = new List<PRA_EMPRESA>();
            if (idEmpresa > 0)
            {
                lst_practicas = da.getListPracticesByIdBusiness(idEmpresa, -1);
                lst_empresas = da.getBusinessById(idEmpresa);
            }
            else
            {
                /// Sacar las 200 últimas
                lst_practicas = da.getListPracticesByIdBusiness(-1, days);
                lst_practicas = lst_practicas.OrderByDescending(p => p.FechaBaja ?? DateTime.MaxValue).Take(200).ToList();
                lst_empresas = da.getBusinessById(-1);
            }
            List<PRA_CONTACTO> lst_contactos = da.getContactsById(-1);

            /// 1.1.- Sacar los alumnos
            List<long> _users = lst_practicas.Select(_ => _.idAlumno).Distinct().ToList();
            List<CLIENTES> lst_users = da.getUserByList(_users);
            //List<CLIENTES> lst_users = da.getActiveUsers(true);

            paint_table(lst_practicas, lst_empresas, lst_contactos, lst_users);

            /// 2.- Pintar el título
            if (idEmpresa > 0)
                txt_practicas.InnerHtml = "<i class='fas fa-toolbox'></i> Listado de Prácticas <a href='practica-mantenimiento.aspx?idb=" + idEmpresa + "' title='Nueva práctica' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva práctica</small></a><a href='javascript:void(0);' onclick='create_excel()' title='Crear excel' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='far fa-file-excel fa-2x text-color-green'></i>  Crear excel</small><a href='javascript:void(0);' onclick='all_practices()' title='Todas las prácticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i> Todas las prácticas</small></a>" + paintMonth(string.Empty) + "<a href='javascript:void(0);' onclick='end_practices()' title='Prácticas que finalizan' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-calendar-times fa-2x'></i> Prácticas que finalizan</small></a>";
            else
                txt_practicas.InnerHtml = "<i class='fas fa-toolbox'></i> Listado de Prácticas <a href='practica-mantenimiento.aspx' title='Nueva práctica' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva práctica</small></a><a href='javascript:void(0);' onclick='create_excel()' title='Crear excel' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='far fa-file-excel fa-2x text-color-green'></i>  Crear excel</small></a><a href='javascript:void(0);' onclick='all_practices()' title='Todas las prácticas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-history fa-2x'></i> Todas las prácticas</small></a>" + paintMonth(string.Empty) + "<a href='javascript:void(0);' onclick='end_practices()' title='Prácticas que finalizan' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-calendar-times fa-2x'></i> Prácticas que finalizan</small></a>";
        }
        
        private void paint_table(List<PRA_PRACTICAS> lst_practicas, List<PRA_EMPRESA> lst_empresas, List<PRA_CONTACTO> lst_contactos, List<CLIENTES> lst_users)
        {
            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Practicas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Fecha Alta</th>");
            sbuild.Append("<th>Fecha Baja</th>");
            sbuild.Append("<th>Datos Alumno</th>");
            //sbuild.Append("<th>Tutor</th>");
            //sbuild.Append("<th>Curso</th>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Datos Empresa</th>");
            sbuild.Append("<th>Anexo</th>");
            sbuild.Append("<th>Duración / Ayudas / Horas</th>");
            sbuild.Append("<th>Factura</th>");
            sbuild.Append("<th>Pedido</th>");
            sbuild.Append("<th>Curriculares</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las prácticas
            foreach (var practica in lst_practicas)
            {
                /// 2.3.1.- Sacar los datos de la empresa y el contacto
                List<PRA_EMPRESA> lst_empresa = lst_empresas.Where(emp => emp.idEmpresa == practica.idEmpresa).ToList();
                List<PRA_CONTACTO> lst_contacto = lst_contactos.Where(cont => cont.idContacto == practica.idTutorEmpresa).ToList();
                //List<CLIENTES> lst_teacher = lst_users.Where(c => c.id_cliente == practica.idTutorEscuela).ToList();
                List<CLIENTES> lst_student = lst_users.Where(c => c.id_cliente == practica.idAlumno).ToList();
                //List<campus_CURSO> lst_course = lst_courses.Where(c => c.ID_Curso == practica.idCurso).ToList();

                sbuild.Append("<tr>");
                sbuild.Append("<td>" + practica.FechaAlta.ToShortDateString() + "</td>");
                sbuild.Append("<td>" + (practica.FechaBaja != null ? practica.FechaBaja.Value.ToShortDateString() : string.Empty) + "</td>");
                if (lst_student.Count > 0)
                {
                    if (lst_student[0].fecha_baja != null)
                        sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(lst_student[0].Nombre_Completo) + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + practica.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-red'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "]<br />(" + lst_student[0].email + ")</a></td>");
                    else if (!String.IsNullOrEmpty(lst_student[0].activo) && lst_student[0].activo == ((int)Constantes.activo.NoActivo).ToString())
                        sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(lst_student[0].Nombre_Completo) + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + practica.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user text-color-orange'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "]<br />(" + lst_student[0].email + ")</a></td>");
                    else
                        sbuild.Append("<td><span class='hidden'>" + Utils.cleanString(lst_student[0].Nombre_Completo) + "</span><a href=\"ficha-alumno-crm.aspx?idu=" + practica.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + lst_student[0].Nombre_Completo + " [" + lst_student[0].id_cliente + "]<br />(" + lst_student[0].email + ")</a></td>");
                }
                else
                    sbuild.Append("<td><a href=\"ficha-alumno-crm.aspx?idu=" + practica.idAlumno + "\" title=\"Datos del alumno\" target=\"_blank\"><i class='fas fa-user'></i> " + practica.idAlumno + "</a></td>");

                /*if (lst_teacher.Count > 0)
                    sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"" + lst_teacher[0].Nombre_Completo + "\">" + lst_teacher[0].Nombre_Completo + " (" + practica.idTutorEscuela + ")</a></td>");
                else
                    sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"Datos del tutor\">" + practica.idTutorEscuela + "</a></td>");*/

                /*if (lst_course.Count > 0)
                    sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"" + lst_course[0].Nombre + "\">" + lst_course[0].Nombre + " (" + practica.idCurso + ")</a></td>");
                else
                    sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"Curso\">" + practica.idCurso + "</a></td>");*/

                sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"Tipo\">" + practica.Tipo + "</a></td>");

                if (lst_empresa.Count > 0)
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + practica.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + lst_empresa[0].RazonSocial + " [" + lst_empresa[0].idEmpresa + "] </a>");
                else
                    sbuild.Append("<td><a href=\"empresa-mantenimiento.aspx?idb=" + practica.idEmpresa + "\" title=\"Datos de la Empresa\" target=\"_blank\"><i class='fas fa-building'></i> " + practica.idEmpresa + "</a>");

                if (lst_contacto.Count > 0)
                    sbuild.Append("<br /><a href=\"contacto-mantenimiento.aspx?idc=" + practica.idTutorEmpresa + "\" title=\"" + lst_contacto[0].Nombre + "\" target=\"_blank\"><i class='fas fa-user-plus'></i> " + lst_contacto[0].Nombre + " " + lst_contacto[0].Apellidos + " (" + practica.idTutorEmpresa + ")</a></td>");
                else
                    sbuild.Append("<br /><a href=\"contacto-mantenimiento.aspx?idc=" + practica.idTutorEmpresa + "\" title=\"Datos de contacto\" target=\"_blank\"><i class='fas fa-user-plus'></i> " + practica.idTutorEmpresa + "</a></td>");

                sbuild.Append("<td>" + (String.IsNullOrEmpty(practica.FicheroAnexo) ? string.Empty : "<a href='" + ConfigurationManager.AppSettings["url_practicas"] + practica.FicheroAnexo + "' runat='server' target='_blank'><i class='far fa-file-pdf fa-1-6x text-color-red'></i></a>") + "</td>");
                sbuild.Append("<td>" + (practica.Duracion == 1 ? practica.Duracion + " mes " : practica.Duracion + " meses ") + " / " + practica.AyudaEstudioMes + "€ " + " / " + practica.HoraSemana + " horas" + "</td>");

                sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"Factura\">" + practica.PDP_NumFactura + "</a></td>");
                sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"Pedido\">" + practica.PDP_NumPedido + "</a></td>");
                sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"Curriculares\">" + (practica.Curriculares != null && practica.Curriculares.Value ? 1 : 0) + "</a></td>");

                sbuild.Append("<td><a href=\"practica-mantenimiento.aspx?idp=" + practica.idPractica + "\" title=\"Editar\"><i class=\"fas fa-edit fa-1-6x\"></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la práctica: " + practica.idPractica + "?\")){eliminarPractica(" + practica.idPractica + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_practicas.InnerHtml = sbuild.ToString();
        }

        private string paintMonth(string _month)
        {
            StringBuilder sbuild = new StringBuilder();
            sbuild.Append("<select id='ddlMonth' class='selectpicker pull-right col-sm-2' data-live-search='true' data-hide-disabled='true'>");
            for (var month = 1; month <= 12; month++)
            {
                if (!String.IsNullOrEmpty(_month) && int.Parse(_month) == month)
                    sbuild.Append("<option selected='selected' value=" + month + ">" + Utilities.MonthName(month) + "</ option >");
                else if (String.IsNullOrEmpty(_month) && DateTime.Today.Month == month)
                    sbuild.Append("<option selected='selected' value=" + month + ">" + Utilities.MonthName(month) + "</ option >");
                else
                    sbuild.Append("<option value=" + month + ">" + Utilities.MonthName(month) + "</ option >");
            }
            sbuild.Append("</select>");
            return sbuild.ToString();
        }
    }
}