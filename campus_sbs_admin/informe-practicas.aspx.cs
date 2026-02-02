using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class informe_practicas : System.Web.UI.Page
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
                    loadReport(list_user[0]);
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = int.Parse(ddlYear.SelectedValue);
            load_report(year);
        }

        private void loadReport(CLIENTES user)
        {
            /// 1.- Pintar el combo con los años
            load_years();

            /// 2.- Pintar el informe
            load_report(DateTime.Today.Year);
        }
        
        private void load_years()
        {
            ddlYear.Items.Clear();
            for (int y = 2013; y <= DateTime.Today.Year; y++)
            {
                ddlYear.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }

            ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        }

        private void load_report(int year)
        {
            /// 1.- Sacar datos de la BBDD
            List<PRA_PRACTICAS> lst_practicas = da.getListPracticesById(-1);
            List<PRA_EMPRESA> lst_empresas = da.getBusinessById(-1);
            List<PRA_SOLICITUD_PRACTICA> lst_solicitudes = da.getListPracticesRequestsById(-1);

            /// 2.- Filtrar por año
            List<PRA_PRACTICAS> lst_practicas_year = new List<PRA_PRACTICAS>();
            List<PRA_EMPRESA> lst_empresas_year = new List<PRA_EMPRESA>();
            List<PRA_SOLICITUD_PRACTICA> lst_solicitudes_year = new List<PRA_SOLICITUD_PRACTICA>();
            if (year > 0)
            {
                DateTime fecha = new DateTime(year, 1, 1);
                lst_practicas_year = lst_practicas.Where(prac => prac.FechaAlta >= fecha && prac.FechaAlta < fecha.AddYears(1)).ToList();
                lst_empresas_year = lst_empresas.Where(emp => emp.FechaAlta >= fecha && emp.FechaAlta < fecha.AddYears(1)).ToList();
                lst_solicitudes_year = lst_solicitudes.Where(sol => sol.fecha >= fecha && sol.fecha < fecha.AddYears(1)).ToList();
            }
            else
            {
                lst_practicas_year = lst_practicas;
                lst_empresas_year = lst_empresas;
                lst_solicitudes_year = lst_solicitudes;
            }

            /// 3.- Pintar los bloques
            StringBuilder sbuild = new StringBuilder();
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;

            /// 3.1.- Bloque empresas
            sbuild.Append("<li class='col-sm-6 col-md-3'><div class='padding-lr-15'>");
            sbuild.Append("<div class='report-box margin-b-15'><div><p class='h3 padding-tb-10 text-center bold'>EMPRESAS</p></div><hr class='separator margin-tb-5'>");
            sbuild.Append("<div class='padding-tb-20'>");
            sbuild.Append("<div class='col-sm-6 text-right'>TOTALES:</div><div class='col-sm-6'>" + lst_empresas.Count + "</div>");
            sbuild.Append("<div class='col-sm-6 text-right'>NUEVAS:</div><div class='col-sm-6'><b>" + lst_empresas_year.Count + "</b></div>");
            sbuild.Append("</div></div></div></li>");

            /// 3.2.- Bloque prácticas
            sbuild.Append("<li class='col-sm-6 col-md-3'><div class='padding-lr-15'>");
            sbuild.Append("<div class='report-box margin-b-15'><div><p class='h3 padding-tb-10 text-center bold'>PRÁCTICAS</p></div><hr class='separator margin-tb-5'>");
            sbuild.Append("<div class='padding-tb-20'>");
            sbuild.Append("<div class='col-sm-6 text-right'>TOTALES:</div><div class='col-sm-6'>" + Utilities.PonerPuntoMiles(lst_practicas.Count) + "</div>");
            sbuild.Append("<div class='col-sm-6 text-right'>NUEVAS:</div><div class='col-sm-6'><b>" + lst_practicas_year.Count + "</b></div>");
            sbuild.Append("</div></div></div></li>");

            /// 3.3.- Bloque de solicitudes de prácticas
            sbuild.Append("<li class='col-sm-6 col-md-3'><div class='padding-lr-15'>");
            sbuild.Append("<div class='report-box margin-b-15'><div><p class='h3 padding-tb-10 text-center bold'>SOLICITUD PRÁC</p></div><hr class='separator margin-tb-5'>");
            sbuild.Append("<div class='padding-tb-20'>");
            sbuild.Append("<div class='col-sm-6 text-right'>TOTALES:</div><div class='col-sm-6'>" + Utilities.PonerPuntoMiles(lst_solicitudes.Count) + "</div>");
            sbuild.Append("<div class='col-sm-6 text-right'>NUEVAS:</div><div class='col-sm-6'><b>" + lst_solicitudes_year.Count + "</b></div>");
            sbuild.Append("</div></div></div></li>");

            /// 3.4.- Bloque valoración
            decimal meses = lst_practicas.Sum(pra => pra.Duracion);
            decimal meses_year = lst_practicas_year.Sum(pra => pra.Duracion);
            decimal euros = get_price(lst_practicas);
            decimal euros_year = get_price(lst_practicas_year);

            sbuild.Append("<li class='col-sm-6 col-md-3'><div class='padding-lr-15'>");
            sbuild.Append("<div class='report-box margin-b-15'><div><p class='h3 padding-tb-10 text-center bold'>VALORACIÓN</p></div><hr class='separator margin-tb-5'>");
            sbuild.Append("<div class='padding-tb-10'>");
            sbuild.Append("<div class='col-sm-6 text-right'>TOT. MESES:</div><div class='col-sm-6'>" + Utilities.PonerPuntoMiles(Math.Round(meses, 2)) + " meses</div>");
            sbuild.Append("<div class='col-sm-6 text-right'>TOT. EUROS:</div><div class='col-sm-6'>" + Utilities.PonerPuntoMiles(Math.Round(euros, 2)) + "€</div>");
            sbuild.Append("<div class='col-sm-6 text-right'>AÑO MESES:</div><div class='col-sm-6'><span class='bold'>" + Utilities.PonerPuntoMiles(Math.Round(meses_year, 2)) + " meses</span></div>");
            sbuild.Append("<div class='col-sm-6 text-right'>AÑO EUROS:</div><div class='col-sm-6'><span class='bold'>" + Utilities.PonerPuntoMiles(Math.Round(euros_year, 2)) + "€</span></div>");
            sbuild.Append("</div></div></div></li>");

            block_informe_global.InnerHtml = sbuild.ToString();

            /// 4.- Pintar el bloque detallado
            sbuild = new StringBuilder();

            if (year < 1)
            {
                year = DateTime.Today.Year;
                DateTime fecha = new DateTime(year, 1, 1);
                lst_practicas_year = lst_practicas.Where(prac => prac.FechaAlta >= fecha && prac.FechaAlta < fecha.AddYears(1)).ToList();
                //lst_empresas_year = lst_empresas.Where(emp => emp.FechaAlta >= fecha && emp.FechaAlta < fecha.AddYears(1)).ToList();
            }

            /// 4.1.- Título            
            sbuild.Append("<p class='fa-1-6x text-color-primary'><i class='fas fa-chart-bar'></i> Informe detallado de Prácticas" + (year > 0 ? " del año " + year : string.Empty) + "</p>");

            /// 4.2.- Pintar tabla
            sbuild.Append("<div class='table-reflow'><table class='table'><thead class='hidden-xs'><tr><th class='bg-primary'></th><th class='bg-primary'>ENE</th><th class='bg-primary'>FEB</th><th class='bg-primary'>MAR</th><th class='bg-primary'>ABR</th><th class='bg-primary'>MAY</th><th class='bg-primary'>JUN</th><th class='bg-primary'>JUL</th><th class='bg-primary'>AGO</th><th class='bg-primary'>SEP</th><th class='bg-primary'>OCT</th><th class='bg-primary'>NOV</th><th class='bg-primary'>DIC</th><th class='bg-primary'>AÑO</th></tr></thead><tbody>");

            /// 4.3.1.- Pintar fila prácticas
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block bold'>Nº Prácticas</span></td>");
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                List<PRA_PRACTICAS> lst_practicas_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1)).ToList();
                sbuild.Append("<td class='bold'><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + lst_practicas_mes.Count + "</td>");
            }
            sbuild.Append("<td class='bold'><span class='visible-xs visible-sm text-color-primary'>Total </span>" + lst_practicas_year.Count + "</td></tr>");

            /// 4.3.2.- Pintar fila PDP3
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block'>PDP3</span></td>");
            int num_pdp3 = 0;
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                List<PRA_PRACTICAS> lst_practicas_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1) && pra.Tipo == "PDP3").ToList();
                sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + lst_practicas_mes.Count + "</td>");
                num_pdp3 += lst_practicas_mes.Count;
            }
            sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>Total </span>" + num_pdp3 + "</td></tr>");

            /// 4.3.3.- Pintar fila PDP6
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block'>PDP6</span></td>");
            int num_pdp6 = 0;
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                List<PRA_PRACTICAS> lst_practicas_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1) && pra.Tipo == "PDP6").ToList();
                sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + lst_practicas_mes.Count + "</td>");
                num_pdp6 += lst_practicas_mes.Count;
            }
            sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>Total </span>" + num_pdp6 + "</td></tr>");

            /// 4.3.4.- Pintar fila PDP6
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block'>PDP</span></td>");
            int num_pdp = 0;
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                List<PRA_PRACTICAS> lst_practicas_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1) && pra.Tipo == "PDP").ToList();
                sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + lst_practicas_mes.Count + "</td>");
                num_pdp += lst_practicas_mes.Count;
            }
            sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>Total </span>" + num_pdp + "</td></tr>");

            /// 4.3.5.- Pintar fila POSTGRADO
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block'>POSTGRADO</span></td>");
            int num_postgrado = 0;
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                List<PRA_PRACTICAS> lst_practicas_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1) && pra.Tipo == "POSTGRADO").ToList();
                sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + lst_practicas_mes.Count + "</td>");
                num_postgrado += lst_practicas_mes.Count;
            }
            sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>Total </span>" + num_postgrado + "</td></tr>");

            /// 4.4.0.- Filtrar los que tengan PDP_Matriculacion=1 y PDP_Activo=1
            lst_practicas_year = lst_practicas_year.Where(pra => pra.PDP_Matriculado != null && pra.PDP_Matriculado.Value && pra.PDP_Activado != null && pra.PDP_Activado.Value).ToList(); 

            /// 4.4.1.- Pintar fila Ingresos
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block bold'>Ingresos</span></td>");
            decimal ingresos_year = new decimal(0);
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                decimal? ingresos_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1)).ToList().Sum(pra => pra.PDP_Precio);
                sbuild.Append("<td class='bold'><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + Utilities.PonerPuntoMiles(ingresos_mes.Value) + "</td>");
                ingresos_year += ingresos_mes.Value;
            }
            sbuild.Append("<td class='bold'><span class='visible-xs visible-sm text-color-primary'>Total </span>" + Utilities.PonerPuntoMiles(ingresos_year) + "</td></tr>");

            /// 4.4.2.- Pintar fila PDP3
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block'>PDP3</span></td>");
            decimal ingresos_pdp3_year = new decimal(0);
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                decimal? ingresos_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1) && pra.Tipo == "PDP3").ToList().Sum(pra => pra.PDP_Precio);
                sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + Utilities.PonerPuntoMiles(ingresos_mes.Value) + "</td>");
                ingresos_pdp3_year += (ingresos_mes > 0 ? ingresos_mes.Value : 0);
            }
            sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>Total </span>" + Utilities.PonerPuntoMiles(ingresos_pdp3_year) + "</td></tr>");

            /// 4.4.3.- Pintar fila PDP6
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block'>PDP6</span></td>");
            decimal ingresos_pdp6_year = new decimal(0);
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                decimal? ingresos_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1) && pra.Tipo == "PDP6").ToList().Sum(pra => pra.PDP_Precio);
                sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + Utilities.PonerPuntoMiles(ingresos_mes.Value) + "</td>");
                ingresos_pdp6_year += (ingresos_mes > 0 ? ingresos_mes.Value : 0);
            }
            sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>Total </span>" + Utilities.PonerPuntoMiles(ingresos_pdp6_year) + "</td></tr>");

            /// 4.4.4.- Pintar fila PDP
            sbuild.Append("<tr><td class='bg-primary'><span class='h4 text-color-white display-block'>PDP</span></td>");
            decimal ingresos_pdp_year = new decimal(0);
            for (var month = 1; month <= 12; month++)
            {
                DateTime fecha_mes = new DateTime(year, month, 1);
                decimal? ingresos_mes = lst_practicas_year.Where(pra => pra.FechaAlta >= fecha_mes && pra.FechaAlta < fecha_mes.AddMonths(1) && pra.Tipo == "PDP").ToList().Sum(pra => pra.PDP_Precio);
                sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>" + dtinfo.GetMonthName(month) + " </span>" + Utilities.PonerPuntoMiles(ingresos_mes.Value) + "</td>");
                ingresos_pdp_year += (ingresos_mes > 0 ? ingresos_mes.Value : 0);
            }
            sbuild.Append("<td><span class='visible-xs visible-sm text-color-primary'>Total </span>" + Utilities.PonerPuntoMiles(ingresos_pdp_year) + "</td></tr>");
            
            sbuild.Append("</tbody></table></div>");
            block_informe_detallado.InnerHtml = sbuild.ToString();
        }

        private decimal get_price(List<PRA_PRACTICAS> lst_practicas)
        {
            decimal price = new decimal(0);
            foreach(var practica in lst_practicas)
            {
                price += practica.Duracion * practica.AyudaEstudioMes; 
            }
            return price;
        }        
    }
}