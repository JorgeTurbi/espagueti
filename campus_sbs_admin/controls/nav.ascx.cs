using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin.controls
{
    public partial class nav : System.Web.UI.UserControl
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Sacar los datos del usuario         
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (Session["usuario"] != null)
                list_user.Add((CLIENTES)Session["usuario"]);
            if (list_user.Count > 0)
            {
                string page = Request.Path.Replace("/", "");
                paint_menu(list_user[0], page);
            }
            else
                Response.Redirect("login.aspx");
        }

        private void paint_menu(CLIENTES client, string page)
        {
            DataAccess da = new DataAccess();
            StringBuilder sbuild = new StringBuilder();

            /// 0.- Comprobar si es un usuario especial
            List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
            long id_user_special = -1;

            if (list_users_mails.Contains(client.id_cliente.ToString()))
                id_user_special = client.id_cliente;

            if (id_user_special == -1)
            {
                if (client.Comercial != null && client.Comercial.Value)
                {
                    /// 1.1.- Página leads
                    if (page == "listado-leads-crm.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Leads</span></a></li>");

                    /// 1.2.- Página de informe leads
                    if (page == "informe-leads-crm.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Leads</span></a></li>");

                    /// 1.3.- Página de informe matrículas
                    if (page == "informe-matriculas-crm.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Matrículas</span></a></li>");

                    /// 1.- Menu CRM
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_crm' aria-expanded='false'><i class='fas fa-globe fa-2x' aria-hidden='true'></i><span> CRM</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_crm'>");
                    sbuild.Append("<li><a href='listado-leads-crm.aspx'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Leads</span></a></li>");
                    sbuild.Append("<li><a href='informe-leads-crm.aspx'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Leads</span></a></li>");
                    sbuild.Append("<li><a href='informe-matriculas-crm.aspx'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Matrículas</span></a></li>");
                    sbuild.Append("</ul></div></li>");
                }
                else
                {
                    /// 1.1.- Página empresas
                    if (page == "empresas.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-building fa-2x' aria-hidden='true'></i><span> Empresas</span></a></li>");

                    /// 1.2.- Página contactos
                    if (page == "contactos.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-address-card fa-2x' aria-hidden='true'></i><span> Contactos</span></a></li>");

                    /// 1.3.- Página prácticas
                    if (page == "practicas.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-toolbox fa-2x' aria-hidden='true'></i><span> Prácticas</span></a></li>");

                    /// 1.4.- Página empleos
                    if (page == "empleos.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-business-time fa-2x' aria-hidden='true'></i><span> Empleos</span></a></li>");

                    /// 1.5.- Página informe prácticas
                    if (page == "informe-practicas.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-chart-bar fa-2x' aria-hidden='true'></i><span> Informe de Prácticas</span></a></li>");

                    /// 1.6.- Página listado de solicitudes
                    if (page == "solicitud-practica.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-file-signature fa-2x' aria-hidden='true'></i><span> Solicitudes prácticas</span></a></li>");

                    /// 1.7.- Página listado de candidatos
                    if (page == "candidatos.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-users fa-2x' aria-hidden='true'></i><span> Candidatos prácticas</span></a></li>");

                    /// 1.8.- Página matriculación PDP
                    if (page == "matricula-pdp.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-user-plus fa-2x' aria-hidden='true'></i><span> Matriculación PDP</span></a></li>");

                    /// 2.1.- Página ventas tpv
                    if (page == "ventas-tpv.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span> Ventas por TPV</span></a></li>");

                    /// 2.2.- Página ventas tpv sbs Life
                    if (page == "ventas-tpv-sbslife.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span> Suscripciones SBS Life</span></a></li>");

                    /// 3.1.- Página lista suscriptores
                    if (page == "lista-suscriptores.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de suscriptores</span></a></li>");

                    /// 3.2.- Página lista campañas
                    if (page == "lista-newsletter.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de campañas</span></a></li>");

                    /// 4.1.- Página leads
                    if (page == "listado-leads-crm.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Leads</span></a></li>");

                    /// 4.2.- Página de informe leads
                    if (page == "informe-leads-crm.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Leads</span></a></li>");

                    /// 4.3.- Página de informe matrículas
                    if (page == "informe-matriculas-crm.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Matrículas</span></a></li>");
                    
                    /// 5.1.- Página de recursos en directo
                    if (page == "lista-recursos-directo.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-graduation-cap fa-2x' aria-hidden='true'></i><span> Recursos en directo</span></a></li>");

                    /// 5.2.- Página de cursos
                    if (page == "cursos.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-graduation-cap fa-2x' aria-hidden='true'></i><span> Cursos</span></a></li>");

                    /// 6.1.- Página solicitar opinión
                    if (page == "solicitar-opinion.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-comment-dots fa-2x' aria-hidden='true'></i><span> Solicitud Opinión</span></a></li>");

                    /// 6.2.- Página lista opiniones
                    if (page == "lista-opiniones.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de opiniones</span></a></li>");

                    /// 6.3.- Página comunidad social
                    if (page == "comunidad-social.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-rss fa-2x' aria-hidden='true'></i><span> Comunidad Social</span></a></li>");

                    /// 8.1.- Página tipos automatización
                    if (page == "lista-tipos-automatizacion.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-tools fa-2x' aria-hidden='true'></i><span> Lista de tipos Automatización</span></a></li>");

                    /// 8.2.- Página reglas automatización
                    if (page == "lista-reglas.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-pencil-ruler fa-2x' aria-hidden='true'></i><span> Lista de reglas Automatización</span></a></li>");

                    /// 9.1.- Página control leads
                    if (page == "control-lead-automation.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Control leads</span></a></li>");

                    /// 9.2.- Página informe de matrículas
                    if (page == "informe-matriculas.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Informe matrículas</span></a></li>");

                    /// 9.3.- Página informe de leads
                    if (page == "informe-leads.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Informe leads</span></a></li>");

                    /// 10.1.- Test_Test
                    if (page == "test_test.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-clipboard-list fa-2x' aria-hidden='true'></i><span> Tests</span></a></li>");

                    /// 10.2.- Test_Preguntas
                    if (page == "test_inventario_pregunta.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-question-circle fa-2x' aria-hidden='true'></i><span> Test Preguntas</span></a></li>");

                    /// 10.3.- Test_Categorias
                    if (page == "tests_categoria.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-tag fa-2x' aria-hidden='true'></i><span> Test Categorías</span></a></li>");

                    /// 10.4.- Test_Subcategorias
                    if (page == "tests_subcategoria.aspx")
                        sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='fas fa-tags fa-2x' aria-hidden='true'></i><span> Test Subcategoría</span></a></li>");

                    /// 1.- Menu común COMPANY
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_company' aria-expanded='false'><i class='fas fa-industry fa-2x' aria-hidden='true'></i><span> Company</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_company'>");
                    sbuild.Append("<li><a href='empresas.aspx'><i class='fas fa-building fa-2x' aria-hidden='true'></i><span> Empresas</span></a></li>");
                    sbuild.Append("<li><a href='contactos.aspx'><i class='far fa-address-card fa-2x' aria-hidden='true'></i><span> Contactos</span></a></li>");
                    sbuild.Append("<li><a href='practicas.aspx'><i class='fas fa-toolbox fa-2x' aria-hidden='true'></i><span> Prácticas</span></a></li>");
                    sbuild.Append("<li><a href='empleos.aspx'><i class='fas fa-business-time fa-2x' aria-hidden='true'></i><span> Empleos</span></a></li>");
                    sbuild.Append("<li><a href='informe-practicas.aspx'><i class='fas fa-chart-bar fa-2x' aria-hidden='true'></i><span> Informe de Prácticas</span></a></li>");
                    sbuild.Append("<li><a href='solicitud-practica.aspx'><i class='fas fa-file-signature fa-2x' aria-hidden='true'></i><span> Solicitudes de Prácticas</span></a></li>");
                    sbuild.Append("<li><a href='candidatos.aspx'><i class='fas fa-users fa-2x' aria-hidden='true'></i><span> Candidatos prácticas</span></a></li>");
                    sbuild.Append("<li><a href='matricula-pdp.aspx'><i class='fas fa-user-plus fa-2x' aria-hidden='true'></i><span> Matriculación PDP</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 2.- Menu común FINANCIERO
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_finance' aria-expanded='false'><i class='fas fa-landmark fa-2x' aria-hidden='true'></i><span> Financiero</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_finance'>");


                    sbuild.Append("<li><a href='admin_facturas.aspx'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span>Facturas</span></a></li>");
                    sbuild.Append("<li><a href='admin_facturas_list.aspx'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span>Listar Facturas</span></a></li>");
                    sbuild.Append("<li><a href='admin_Gastos.aspx'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span>Gastos</span></a></li>");
                    sbuild.Append("<li><a href='Admin-Balance-Acumulado.aspx'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span>Balac. Acumulado</span></a></li>");
                    sbuild.Append("<li><a href='ventas-tpv.aspx'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span> Ventas por TPV</span></a></li>");
                    sbuild.Append("<li><a href='ventas-tpv-sbslife.aspx'><i class='far fa-credit-card fa-2x' aria-hidden='true'></i><span> Suscripciones SBS Life</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 3.- Menu común COMERCIAL
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_comercial' aria-expanded='false'><i class='fas fa-mail-bulk fa-2x' aria-hidden='true'></i><span> Comercial</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_comercial'>");
                    sbuild.Append("<li><a href='lista-suscriptores.aspx'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de suscriptores</span></a></li>");
                    sbuild.Append("<li><a href='lista-newsletter.aspx'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de campañas</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 4.- Menu CRM
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_crm' aria-expanded='false'><i class='fas fa-globe fa-2x' aria-hidden='true'></i><span> CRM</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_crm'>");
                    sbuild.Append("<li><a href='listado-leads-crm.aspx'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Leads</span></a></li>");
                    sbuild.Append("<li><a href='informe-leads-crm.aspx'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Leads</span></a></li>");
                    sbuild.Append("<li><a href='informe-matriculas-crm.aspx'><i class='fas fa-list fa-2x' aria-hidden='true'></i><span> Inf. Matrículas</span></a></li>");
                    sbuild.Append("</ul></div></li>");
                    
                    /// 5.- Menu común Académico
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_academico' aria-expanded='false'><i class='fas fa-graduation-cap fa-2x' aria-hidden='true'></i><span> Académico</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_academico'>");
                    sbuild.Append("<li><a href='lista-recursos-directo.aspx'><i class='fas fa-photo-video fa-2x' aria-hidden='true'></i><span> Recursos en directo</span></a></li>");
                    sbuild.Append("<li><a href='cursos.aspx'><i class='fas fa-folder-open fa-2x' aria-hidden='true'></i><span> Cursos</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 6.- Menu común WEB
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_web' aria-expanded='false'><i class='fas fa-globe-europe fa-2x' aria-hidden='true'></i><span> WEB</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_web'>");
                    sbuild.Append("<li><a href='lista-opiniones.aspx'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de opiniones</span></a></li>");
                    sbuild.Append("<li><a href='comunidad-social.aspx'><i class='fas fa-rss fa-2x' aria-hidden='true'></i><span> Comunidad Social</span></a></li>");
                    sbuild.Append("<li></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 7.- Menu común Usuarios
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_usuarios' aria-expanded='false'><i class='fas fa-users fa-2x'></i><span> Usuarios</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_usuarios'>");
                    sbuild.Append("<li><a href='cargar-usuarios.aspx'><i class='fas fa-file-upload fa-2x'></i><span> Cargar Usuarios</span></a></li>");
                    sbuild.Append("<li><a href='dar-baja-usuarios.aspx'><i class='fas fa-file-download fa-2x'></i><span> Dar de baja Usuarios</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 8.- Menu común Automatización
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_automation' aria-expanded='false'><i class='fas fa-tools fa-2x' aria-hidden='true'></i><span> Automatización</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_automation'>");
                    sbuild.Append("<li><a href='lista-tipos-automatizacion.aspx'><i class='fas fa-tools fa-2x' aria-hidden='true'></i><span> Lista de tipos Automatización</span></a></li>");
                    sbuild.Append("<li><a href='lista-reglas.aspx'><i class='fas fa-pencil-ruler fa-2x' aria-hidden='true'></i><span> Lista de reglas Automatización</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 9.- Menu informes
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_informes' aria-expanded='false'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Informes</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_informes'>");
                    sbuild.Append("<li><a href='control-lead-automation.aspx'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Control leads</span></a></li>");
                    sbuild.Append("<li><a href='informe-matriculas.aspx'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Ventas</span></a></li>");
                    sbuild.Append("<li><a href='informe-leads.aspx'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Leads</span></a></li>");
                    sbuild.Append("<li><a href='informe-universidad.aspx'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Matrículas</span></a></li>");
                    sbuild.Append("<li><a href='informe-evalua-tu-talento.aspx'><i class='fas fa-chart-line fa-2x' aria-hidden='true'></i><span> Evalúa tu talento</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 10.- Menu Tests
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_tests' aria-expanded='false'><i class='fas fa-clipboard-list fa-2x' aria-hidden='true'></i><span> Tests</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_tests'>");
                    sbuild.Append("<li><a href='test_test.aspx'><i class='fas fa-clipboard-list fa-2x' aria-hidden='true'></i><span> Tests</span></a></li>");
                    sbuild.Append("<li><a href='test_inventario_pregunta.aspx'><i class='fas fa-question-circle fa-2x' aria-hidden='true'></i><span> Preguntas</span></a></li>");
                    sbuild.Append("<li><a href='tests_categoria.aspx'><i class='fas fa-tag fa-2x' aria-hidden='true'></i><span> Categorías</span></a></li>");
                    sbuild.Append("<li><a href='tests_subcategoria.aspx'><i class='fas fa-tags fa-2x' aria-hidden='true'></i><span> Subcategorías</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    /// 11.- Menu SBS Life
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_sbs_life' aria-expanded='false'><i class='fas fa-globe-americas fa-2x' aria-hidden='true'></i><span> SBS Life</span><i class='fas fa-angle-down fa-2x'></i></a>");
                    sbuild.Append("<div class='nav-level'><ul id='menu_sbs_life'>");
                    sbuild.Append("<li><a href='lista-banners.aspx'><i class='fas fa-ad fa-2x'></i><span> Banners</span></a></li>");
                    sbuild.Append("<li><a href='registro-sbslife.aspx'><i class='fas fa-file-upload fa-2x'></i><span> Registrar Usuarios</span></a></li>");
                    sbuild.Append("<li><a href='lista-suscripciones-sbslife.aspx'><i class='far fa-list-alt fa-2x'></i><span> Lista de suscripciones</span></a></li>");
                    sbuild.Append("</ul></div></li>");

                    sbuild.Append("<li class='nav-item'><br /><br /><br /><br /><br /><br /></li>");
                }
            }
            else
            {
                /// 1.1.- Página lista suscriptores
                if (page == "lista-suscriptores.aspx")
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de suscriptores</span></a></li>");

                /// 1.2.- Página lista campañas
                if (page == "lista-newsletter.aspx")
                    sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' class='active'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de campañas</span></a></li>");

                /// 1.- Menu común COMERCIAL
                sbuild.Append("<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_comercial' aria-expanded='false'><i class='fas fa-mail-bulk fa-2x' aria-hidden='true'></i><span> Comercial</span><i class='fas fa-angle-down fa-2x'></i></a>");
                sbuild.Append("<div class='nav-level'><ul id='menu_comercial'>");
                sbuild.Append("<li><a href='lista-suscriptores.aspx'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de suscriptores</span></a></li>");
                sbuild.Append("<li><a href='lista-newsletter.aspx'><i class='far fa-list-alt fa-2x' aria-hidden='true'></i><span> Lista de campañas</span></a></li>");
                sbuild.Append("</ul></div></li>");
            }

            menu_nav.InnerHtml = sbuild.ToString();
        }
    }
}