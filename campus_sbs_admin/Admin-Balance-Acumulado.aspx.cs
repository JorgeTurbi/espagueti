using campus_sbs_admin.Models;
using sbs_DAL;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using INF_FIN_COSTES = sbs_DAL.INF_FIN_COSTES;
using INF_FIN_FACTURAS = sbs_DAL.INF_FIN_FACTURAS;

namespace campus_sbs_admin
{
    public partial class Admin_Balance_Acumulado : System.Web.UI.Page
    {
        // Contexto de Entity Framework
        private SpainBS_CampusEntities db = new SpainBS_CampusEntities();

        // Cache de estructura (se carga una vez por request)
        private EstructuraFinanciera _estructura;
        private EstructuraFinanciera Estructura
        {
            get
            {
                if (_estructura == null)
                {
                    _estructura = CargarEstructura();
                }
                return _estructura;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar sesión
            if (Session["usuario"] == null)
                Response.Redirect("Admin_Login.aspx");

            if (!IsPostBack)
            {
                LoadYears();
                loadPartners();
                lblLastUpdate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                lblYear.InnerText = DateTime.Today.Year.ToString();
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            // 1.- Recuperar las fechas del formulario
            int year = int.Parse(ddlYear.SelectedValue);
            DateTime start_date = new DateTime(year, 1, 1);
            DateTime end_date = new DateTime(year, 12, 31);

            // 2.- Sacar la sociedad del formulario
            string society = ddlSociety.Value;

            // 3.- Calcular el bloque de ingresos
            List<INF_FIN_FACTURAS> list_ingresos = GetResultIngresos(society, start_date, end_date);

            // 4.- Calcular el bloque de gastos
            List<INF_FIN_COSTES> list_gastos = GetResultGastos(society, start_date, end_date);

            // 5.- Obtener los datos por meses de los ingresos
            List<Tax> lista_ingresos = GetResultIng(list_ingresos, start_date);

            // 6.- Obtener los datos por meses de los gastos 
            Dictionary<int, Dictionary<int, Dictionary<int, decimal>>> gastosPorMes = GetResultGasDynamic(list_gastos, start_date);

            // 7.- Pintar la tabla
            block_balance.InnerHtml = PaintTableDynamic(lista_ingresos, gastosPorMes, year);

            // 8.- Actualizar etiquetas
            lblYear.InnerText = year.ToString();
            lblLastUpdate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        protected void btn_excel_Click(object sender, EventArgs e)
        {
            // 1.- Recuperar las fechas del formulario
            int year = int.Parse(ddlYear.SelectedValue);
            DateTime start_date = new DateTime(year, 1, 1);
            DateTime end_date = new DateTime(year, 12, 31);

            // 2.- Sacar la sociedad del formulario
            string society = ddlSociety.Value;

            // 3.- Calcular el bloque de ingresos
            List<INF_FIN_FACTURAS> list_ingresos = GetResultIngresos(society, start_date, end_date);

            // 4.- Calcular el bloque de gastos
            List<INF_FIN_COSTES> list_gastos = GetResultGastos(society, start_date, end_date);

            // 5.- Obtener los datos por meses de los ingresos
            List<Tax> lista_ingresos = GetResultIng(list_ingresos, start_date);

            // 6.- Obtener los datos por meses de los gastos 
            Dictionary<int, Dictionary<int, Dictionary<int, decimal>>> gastosPorMes = GetResultGasDynamic(list_gastos, start_date);

            // 7.- Crear Excel
            CreateExcelDynamic(lista_ingresos, gastosPorMes, year);
        }

        #region Carga de Estructura Dinámica usando Entity Framework

        /// <summary>
        /// Carga la estructura financiera desde la base de datos usando EF
        /// </summary>
        private EstructuraFinanciera CargarEstructura()
        {
            var estructura = new EstructuraFinanciera();

            try
            {
                // USAR SpainBS_Connection porque es donde están los DbSets
                using (var dbEstructura = new SpainBS_Connection())
                {
                    // Obtener la versión activa
                    var versionActiva = dbEstructura.INF_FIN_ESTRUCTURAS_VERSION
                        .Where(v => v.Activa == true)  // CORREGIDO: comparar con == true
                        .OrderByDescending(v => v.FechaInicio)
                        .FirstOrDefault();

                    if (versionActiva == null)
                    {
                        // Si no hay versión activa, usar versión 1 por defecto
                        versionActiva = dbEstructura.INF_FIN_ESTRUCTURAS_VERSION.Find(1);
                    }

                    if (versionActiva == null)
                    {
                        // No hay estructura configurada
                        estructura.AreasIngresos = new List<AreaFinanciera>();
                        estructura.AreasGastos = new List<AreaFinanciera>();
                        return estructura;
                    }

                    estructura.VersionID = versionActiva.ID;

                    // Cargar áreas de ingresos
                    var areasIngresosEF = dbEstructura.INF_FIN_AREAS
                        .Where(a => a.VersionID == versionActiva.ID && a.TipoCategoria == "INGRESO" && a.Activo == true)  // CORREGIDO
                        .OrderBy(a => a.Orden)
                        .ToList();

                    estructura.AreasIngresos = areasIngresosEF.Select(a => new AreaFinanciera
                    {
                        ID = a.ID,
                        Nombre = a.Nombre,
                        NombreCorto = a.NombreCorto,
                        Orden = a.Orden,
                        TipoCategoria = a.TipoCategoria,
                        Subareas = new List<SubareaFinanciera>()
                    }).ToList();

                    // Cargar áreas de gastos con subareas
                    var areasGastosEF = dbEstructura.INF_FIN_AREAS
                        .Where(a => a.VersionID == versionActiva.ID && a.TipoCategoria == "GASTO" && a.Activo == true)  // CORREGIDO
                        .OrderBy(a => a.Orden)
                        .ToList();

                    estructura.AreasGastos = new List<AreaFinanciera>();

                    foreach (var areaEF in areasGastosEF)
                    {
                        var area = new AreaFinanciera
                        {
                            ID = areaEF.ID,
                            Nombre = areaEF.Nombre,
                            NombreCorto = areaEF.NombreCorto,
                            Orden = areaEF.Orden,
                            TipoCategoria = areaEF.TipoCategoria,
                            Subareas = new List<SubareaFinanciera>()
                        };

                        // Cargar subareas
                        var subareasEF = dbEstructura.INF_FIN_SUBAREAS
                            .Where(s => s.AreaID == areaEF.ID && s.Activo == true)  // CORREGIDO
                            .OrderBy(s => s.Orden)
                            .ToList();

                        foreach (var subareaEF in subareasEF)
                        {
                            var subarea = new SubareaFinanciera
                            {
                                ID = subareaEF.ID,
                                AreaID = subareaEF.AreaID,
                                Nombre = subareaEF.Nombre,
                                NombreCorto = subareaEF.NombreCorto,
                                Orden = subareaEF.Orden,
                                TieneHijos = subareaEF.TieneHijos == true,  // CORREGIDO: convertir a bool
                                Subareas2 = new List<Subarea2Financiera>()
                            };

                            if (subareaEF.TieneHijos == true)  // CORREGIDO
                            {
                                // Cargar subareas nivel 2
                                var subareas2EF = dbEstructura.INF_FIN_SUBAREAS2
                                    .Where(s2 => s2.SubareaID == subareaEF.ID && s2.Activo == true)  // CORREGIDO
                                    .OrderBy(s2 => s2.Orden)
                                    .ToList();

                                subarea.Subareas2 = subareas2EF.Select(s2 => new Subarea2Financiera
                                {
                                    ID = s2.ID,
                                    SubareaID = s2.SubareaID,
                                    Nombre = s2.Nombre,
                                    NombreCorto = s2.NombreCorto,
                                    Orden = s2.Orden
                                }).ToList();
                            }

                            area.Subareas.Add(subarea);
                        }

                        estructura.AreasGastos.Add(area);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log del error
                System.Diagnostics.Debug.WriteLine($"Error cargando estructura: {ex.Message}");

                // Retornar estructura vacía pero válida
                estructura.AreasIngresos = new List<AreaFinanciera>();
                estructura.AreasGastos = new List<AreaFinanciera>();
            }

            return estructura;
        }

        #endregion

        #region Métodos para el bloque de ingresos

        private List<INF_FIN_FACTURAS> GetResultIngresos(string society, DateTime start_date, DateTime end_date)
        {
            var query = db.INF_FIN_FACTURAS.AsQueryable();

            if (!string.IsNullOrEmpty(society))
            {
                query = query.Where(f => f.sociedad == society);
            }

            query = query.Where(f => f.fecha_emision >= start_date && f.fecha_emision <= end_date);

            return query.ToList();
        }

        private List<Tax> GetResultIng(List<INF_FIN_FACTURAS> list_ingresos, DateTime start_date)
        {
            List<Tax> list = new List<Tax>();

            var areasIngresos = Estructura.AreasIngresos.OrderBy(a => a.Orden).ToList();

            for (int month = 1; month <= 12; month++)
            {
                DateTime date_start = new DateTime(start_date.Year, month, 1);
                DateTime date_end = date_start.AddMonths(1).AddDays(-1);

                foreach (var area in areasIngresos)
                {
                    Tax tax = new Tax
                    {
                        month = month,
                        type = area.Nombre,
                        tax_value = GetIngresosByTipo(list_ingresos, area.Nombre, date_start, date_end)
                    };
                    list.Add(tax);
                }
            }

            return list;
        }

        private decimal GetIngresosByTipo(List<INF_FIN_FACTURAS> list_ingresos, string tipo, DateTime date_start, DateTime date_end)
        {
            var facturasFiltradas = list_ingresos
                .Where(f => f.fecha_emision >= date_start && f.fecha_emision <= date_end);

            decimal price = 0;

            switch (tipo)
            {
                case "INCOMPANY":
                case "FORMACION":
                case "POSTGRADO":
                    price = facturasFiltradas
                        .Where(f => f.atribucion == tipo)
                        .Sum(f => f.eur_precio);
                    break;
                case "FUNDACION":
                    price = facturasFiltradas.Sum(f => f.eur_fundacion);
                    break;
                case "UNIVERSIDAD":
                    price = facturasFiltradas.Sum(f => f.eur_universidad);
                    break;
                case "TRIPARTITA":
                    price = facturasFiltradas.Sum(f => f.eur_tripartita);
                    break;
            }

            return price;
        }

        private void loadPartners()
        {
            string mensaje = "Todas";
            ddlSociety.Items.Clear();

            List<string> partnerList;
            using (var dbConn = new SpainBS_Connection())
            {
                partnerList = dbConn.Socio.AsNoTracking().Select(a => a.Nombre).ToList();
            }

            ddlSociety.Items.Add(new ListItem(mensaje, ""));
            foreach (var y in partnerList)
                ddlSociety.Items.Add(new ListItem(y, y));
        }

        #endregion

        #region Métodos para el bloque de gastos - DINÁMICO

        private List<INF_FIN_COSTES> GetResultGastos(string society, DateTime start_date, DateTime end_date)
        {
            var query = db.INF_FIN_COSTES.AsQueryable();

            if (!string.IsNullOrEmpty(society))
            {
                query = query.Where(c => c.Sociedad == society);
            }

            query = query.Where(c => c.FEmision >= start_date && c.FEmision <= end_date);

            return query.ToList();
        }

        /// <summary>
        /// Calcula los gastos de forma dinámica basándose en la estructura cargada
        /// Retorna: Dictionary[mes, Dictionary[areaID, Dictionary[subareaID, valor]]]
        /// </summary>
        private Dictionary<int, Dictionary<int, Dictionary<int, decimal>>> GetResultGasDynamic(List<INF_FIN_COSTES> list_gastos, DateTime start_date)
        {
            var resultado = new Dictionary<int, Dictionary<int, Dictionary<int, decimal>>>();

            for (int month = 1; month <= 12; month++)
            {
                DateTime date_start = new DateTime(start_date.Year, month, 1);
                DateTime date_end = date_start.AddMonths(1).AddDays(-1);

                var gastosMes = list_gastos
                    .Where(c => c.FEmision >= date_start && c.FEmision <= date_end)
                    .ToList();

                var dictAreas = new Dictionary<int, Dictionary<int, decimal>>();

                foreach (var area in Estructura.AreasGastos)
                {
                    var gastosArea = gastosMes.Where(c => c.Area == area.ID).ToList();
                    var dictSubareas = new Dictionary<int, decimal>();

                    foreach (var subarea in area.Subareas)
                    {
                        decimal total = gastosArea
                            .Where(c => c.SubArea == subarea.ID)
                            .Sum(c => c.Subtotal);

                        dictSubareas[subarea.ID] = total;

                        // Si tiene hijos (subareas2), también calcularlos
                        if (subarea.TieneHijos && subarea.Subareas2 != null)
                        {
                            foreach (var subarea2 in subarea.Subareas2)
                            {
                                decimal totalSub2 = gastosArea
                                    .Where(c => c.SubArea == subarea.ID && c.SubArea2 == subarea2.ID)
                                    .Sum(c => c.Subtotal);

                                // Guardamos con ID negativo para diferenciar nivel 3
                                dictSubareas[-subarea2.ID] = totalSub2;
                            }
                        }
                    }

                    dictAreas[area.ID] = dictSubareas;
                }

                resultado[month] = dictAreas;
            }

            return resultado;
        }

        #endregion

        #region Métodos generales - Pintar tabla DINÁMICA con Toggle

        private void LoadYears()
        {
            ddlYear.Items.Clear();
            for (int _year = 2016; _year <= DateTime.Today.Year; _year++)
            {
                ddlYear.Items.Add(new ListItem("Año " + _year, _year.ToString()));
            }
            ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        }

        private string PaintTableDynamic(List<Tax> lista_ingresos, Dictionary<int, Dictionary<int, Dictionary<int, decimal>>> gastosPorMes, int year)
        {
            StringBuilder sbuild = new StringBuilder();

            // Cabecera
            sbuild.Append("<table><thead><tr><th></th><th>ENE</th><th>FEB</th><th>MAR</th><th>ABR</th><th>MAY</th><th>JUN</th><th>JUL</th><th>AGO</th><th>SEP</th><th>OCT</th><th>NOV</th><th>DIC</th><th>AÑO</th></tr></thead>");
            sbuild.Append("<tbody>");

            // Calcular totales mensuales
            List<decimal> list_ingresos = new List<decimal>();
            List<decimal> list_gastos = new List<decimal>();

            for (int month = 1; month <= 12; month++)
            {
                decimal ingMes = lista_ingresos.Where(t => t.month == month).Sum(t => t.tax_value);
                list_ingresos.Add(ingMes);

                decimal gasMes = 0;
                if (gastosPorMes.ContainsKey(month))
                {
                    foreach (var area in gastosPorMes[month].Values)
                    {
                        gasMes += area.Where(kv => kv.Key > 0).Sum(kv => kv.Value); // Solo subareas principales (ID positivos)
                    }
                }
                list_gastos.Add(gasMes);
            }

            // 1.- Fila RESULTADO (sin toggle, sin hijos)
            sbuild.Append(GetRowResultado(list_ingresos, list_gastos));

            // 2.- Bloque INGRESOS
            sbuild.Append(GetBlockIngresosDynamic(lista_ingresos, list_ingresos, year));

            // 3.- Bloque GASTOS DINÁMICO
            sbuild.Append(GetBlockGastosDynamic(gastosPorMes, list_gastos, year));

            sbuild.Append("</tbody></table>");

            // 4.- Actualizar tarjetas de resumen
            UpdateSummaryCards(list_ingresos, list_gastos);

            return sbuild.ToString();
        }

        private void UpdateSummaryCards(List<decimal> list_ingresos, List<decimal> list_gastos)
        {
            decimal totalIngresos = list_ingresos.Sum();
            decimal totalGastos = list_gastos.Sum();
            decimal resultado = totalIngresos - totalGastos;

            lblIngresos.InnerText = FormatCurrency(totalIngresos);
            lblGastos.InnerText = FormatCurrency(totalGastos);
            lblResultado.InnerText = FormatCurrency(resultado);

            if (resultado < 0)
                lblResultado.Attributes["class"] = "summary-card-value negative";
            else
                lblResultado.Attributes["class"] = "summary-card-value gold";
        }

        private string FormatCurrency(decimal value)
        {
            return string.Format("{0:N2}€", value);
        }

        private string GetRowResultado(List<decimal> list_ingresos, List<decimal> list_gastos)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<tr class='row-level-0 resultado'><td><b>[ = ] RESULTADO</b></td>");
            decimal price_total = 0;

            for (int index = 0; index < 12; index++)
            {
                decimal _price = list_ingresos[index] - list_gastos[index];
                string cssClass = _price >= 0 ? "" : "red";
                sbuild.Append($"<td class='{cssClass}'><b>{FormatCurrency(_price)}</b></td>");
                price_total += _price;
            }

            string totalCssClass = price_total >= 0 ? "" : "red";
            sbuild.Append($"<td class='{totalCssClass}'><b>{FormatCurrency(price_total)}</b></td></tr>");

            return sbuild.ToString();
        }

        private string GetBlockIngresosDynamic(List<Tax> lista_ingresos, List<decimal> list_ingresos, int year)
        {
            StringBuilder sbuild = new StringBuilder();
            DateTime date_start_year = new DateTime(year, 1, 1);
            DateTime date_end_year = new DateTime(year, 12, 31);

            // Fila principal INGRESOS con toggle
            sbuild.Append("<tr class='row-level-0' data-row-id='ingresos'>");
            sbuild.Append("<td><span class='toggle-icon'><i class='fas fa-chevron-down'></i></span><b>[ + ] INGRESOS</b></td>");

            decimal price_total = 0;
            foreach (decimal _price in list_ingresos)
            {
                string cssClass = _price >= 0 ? "" : "red";
                sbuild.Append($"<td class='{cssClass}'><b>{FormatCurrency(_price)}</b></td>");
                price_total += _price;
            }
            string totalCssClass = price_total >= 0 ? "" : "red";
            sbuild.Append($"<td class='{totalCssClass}'><b>{FormatCurrency(price_total)}</b></td></tr>");

            // Subcategorías de ingresos desde estructura dinámica
            foreach (var area in Estructura.AreasIngresos.OrderBy(a => a.Orden))
            {
                sbuild.Append($"<tr class='row-level-2' data-parent-id='ingresos'>");
                sbuild.Append($"<td>{area.Nombre}</td>");
                price_total = 0;

                for (int month = 1; month <= 12; month++)
                {
                    DateTime date_start = new DateTime(year, month, 1);
                    DateTime date_end = date_start.AddMonths(1).AddDays(-1);

                    decimal _price = lista_ingresos
                        .Where(t => t.month == month && t.type == area.Nombre)
                        .Sum(t => t.tax_value);

                    string cssClass = _price >= 0 ? "" : "red";
                    string link = $"Admin_Facturas.aspx?ida={area.Nombre}&ds={date_start.ToShortDateString()}&de={date_end.ToShortDateString()}";
                    sbuild.Append($"<td class='{cssClass}'><a href='{link}' target='_blank'>{FormatCurrency(_price)}</a></td>");
                    price_total += _price;
                }

                totalCssClass = price_total >= 0 ? "" : "red";
                string linkTotal = $"Admin_Facturas.aspx?ida={area.Nombre}&ds={date_start_year.ToShortDateString()}&de={date_end_year.ToShortDateString()}";
                sbuild.Append($"<td class='{totalCssClass}'><a href='{linkTotal}' target='_blank'>{FormatCurrency(price_total)}</a></td></tr>");
            }

            return sbuild.ToString();
        }

        private string GetBlockGastosDynamic(Dictionary<int, Dictionary<int, Dictionary<int, decimal>>> gastosPorMes, List<decimal> list_gastos, int year)
        {
            StringBuilder sbuild = new StringBuilder();
            DateTime date_start_year = new DateTime(year, 1, 1);
            DateTime date_end_year = new DateTime(year, 12, 31);

            // Fila principal GASTOS con toggle
            sbuild.Append("<tr class='row-level-0' data-row-id='gastos'>");
            sbuild.Append("<td><span class='toggle-icon'><i class='fas fa-chevron-down'></i></span><b>[ - ] GASTOS</b></td>");

            decimal price_total = 0;
            foreach (decimal _price in list_gastos)
            {
                string cssClass = _price >= 0 ? "" : "red";
                sbuild.Append($"<td class='{cssClass}'><b>{FormatCurrency(_price)}</b></td>");
                price_total += _price;
            }
            string totalCssClass = price_total >= 0 ? "" : "red";
            sbuild.Append($"<td class='{totalCssClass}'><b>{FormatCurrency(price_total)}</b></td></tr>");

            // Iterar sobre cada área de gastos dinámicamente
            foreach (var area in Estructura.AreasGastos.OrderBy(a => a.Orden))
            {
                sbuild.Append(GetGastoSectionDynamic(gastosPorMes, year, area, "gastos"));
            }

            return sbuild.ToString();
        }

        private string GetGastoSectionDynamic(Dictionary<int, Dictionary<int, Dictionary<int, decimal>>> gastosPorMes, int year, AreaFinanciera area, string parentId)
        {
            StringBuilder sbuild = new StringBuilder();
            DateTime date_start_year = new DateTime(year, 1, 1);
            DateTime date_end_year = new DateTime(year, 12, 31);

            string rowId = $"gas_{area.ID}";
            bool hasChildren = area.Subareas != null && area.Subareas.Count > 0;

            // Fila principal de categoría (Nivel 1)
            sbuild.Append($"<tr class='row-level-1' data-row-id='{rowId}' data-parent-id='{parentId}'>");

            if (hasChildren)
                sbuild.Append($"<td><span class='toggle-icon'><i class='fas fa-chevron-down'></i></span><b>{area.Nombre}</b></td>");
            else
                sbuild.Append($"<td><b>{area.Nombre}</b></td>");

            decimal price_total = 0;
            for (int month = 1; month <= 12; month++)
            {
                decimal _price = 0;
                if (gastosPorMes.ContainsKey(month) && gastosPorMes[month].ContainsKey(area.ID))
                {
                    _price = gastosPorMes[month][area.ID].Where(kv => kv.Key > 0).Sum(kv => kv.Value);
                }

                string cssClass = _price >= 0 ? "" : "red";
                sbuild.Append($"<td class='{cssClass}'><b>{FormatCurrency(_price)}</b></td>");
                price_total += _price;
            }
            string totalCssClass = price_total >= 0 ? "" : "red";
            sbuild.Append($"<td class='{totalCssClass}'><b>{FormatCurrency(price_total)}</b></td></tr>");

            // Subareas (Nivel 2)
            if (hasChildren)
            {
                foreach (var subarea in area.Subareas.OrderBy(s => s.Orden))
                {
                    string subareaRowId = $"gas_{area.ID}_{subarea.ID}";
                    bool hasSubChildren = subarea.TieneHijos && subarea.Subareas2 != null && subarea.Subareas2.Count > 0;

                    sbuild.Append($"<tr class='row-level-2' data-parent-id='{rowId}'");
                    if (hasSubChildren)
                        sbuild.Append($" data-row-id='{subareaRowId}'");
                    sbuild.Append(">");

                    if (hasSubChildren)
                        sbuild.Append($"<td><span class='toggle-icon'><i class='fas fa-chevron-down'></i></span>{subarea.Nombre}</td>");
                    else
                        sbuild.Append($"<td>{subarea.Nombre}</td>");

                    price_total = 0;
                    for (int month = 1; month <= 12; month++)
                    {
                        DateTime date_start = new DateTime(year, month, 1);
                        DateTime date_end = date_start.AddMonths(1).AddDays(-1);

                        decimal _price = 0;
                        if (gastosPorMes.ContainsKey(month) &&
                            gastosPorMes[month].ContainsKey(area.ID) &&
                            gastosPorMes[month][area.ID].ContainsKey(subarea.ID))
                        {
                            _price = gastosPorMes[month][area.ID][subarea.ID];
                        }

                        string cssClass = _price >= 0 ? "" : "red";
                        string link = $"Admin_Gastos.aspx?ida={area.ID}&idsa={subarea.ID}&ds={date_start.ToShortDateString()}&de={date_end.ToShortDateString()}";
                        sbuild.Append($"<td class='{cssClass}'><a href='{link}' target='_blank'>{FormatCurrency(_price)}</a></td>");
                        price_total += _price;
                    }

                    totalCssClass = price_total >= 0 ? "" : "red";
                    string linkTotal = $"Admin_Gastos.aspx?ida={area.ID}&idsa={subarea.ID}&ds={date_start_year.ToShortDateString()}&de={date_end_year.ToShortDateString()}";
                    sbuild.Append($"<td class='{totalCssClass}'><a href='{linkTotal}' target='_blank'>{FormatCurrency(price_total)}</a></td></tr>");

                    // Subarea2 items (Nivel 3)
                    if (hasSubChildren)
                    {
                        foreach (var subarea2 in subarea.Subareas2.OrderBy(s => s.Orden))
                        {
                            sbuild.Append($"<tr class='row-level-3' data-parent-id='{subareaRowId}'>");
                            sbuild.Append($"<td>{subarea2.Nombre}</td>");
                            price_total = 0;

                            for (int month = 1; month <= 12; month++)
                            {
                                DateTime date_start = new DateTime(year, month, 1);
                                DateTime date_end = date_start.AddMonths(1).AddDays(-1);

                                decimal _price = 0;
                                int negativeId = -subarea2.ID;
                                if (gastosPorMes.ContainsKey(month) &&
                                    gastosPorMes[month].ContainsKey(area.ID) &&
                                    gastosPorMes[month][area.ID].ContainsKey(negativeId))
                                {
                                    _price = gastosPorMes[month][area.ID][negativeId];
                                }

                                string cssClass = _price >= 0 ? "" : "red";
                                string link = $"Admin_Gastos.aspx?ida={area.ID}&idsa={subarea.ID}&idsa2={subarea2.ID}&ds={date_start.ToShortDateString()}&de={date_end.ToShortDateString()}";
                                sbuild.Append($"<td class='{cssClass}'><a href='{link}' target='_blank'>{FormatCurrency(_price)}</a></td>");
                                price_total += _price;
                            }

                            totalCssClass = price_total >= 0 ? "" : "red";
                            string linkTotal2 = $"Admin_Gastos.aspx?ida={area.ID}&idsa={subarea.ID}&idsa2={subarea2.ID}&ds={date_start_year.ToShortDateString()}&de={date_end_year.ToShortDateString()}";
                            sbuild.Append($"<td class='{totalCssClass}'><a href='{linkTotal2}' target='_blank'>{FormatCurrency(price_total)}</a></td></tr>");
                        }
                    }
                }
            }

            return sbuild.ToString();
        }

        #endregion

        #region Excel Dinámico

        private void CreateExcelDynamic(List<Tax> lista_ingresos, Dictionary<int, Dictionary<int, Dictionary<int, decimal>>> gastosPorMes, int year)
        {
            string file_name = $"Balance-Acumulado-{year}.xlsx";

            SLDocument _excel = new SLDocument();
            _excel.DocumentProperties.Title = "Balance Acumulado";
            _excel.DocumentProperties.Creator = "Spain Business School";
            _excel.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Balance Acumulado");

            // Estilos
            SLStyle style_lv1 = _excel.CreateStyle();
            style_lv1.SetFontBold(true);

            SLStyle style_lv2 = _excel.CreateStyle();
            style_lv2.SetFontBold(true);

            SLStyle style_number = _excel.CreateStyle();
            style_number.FormatCode = "#,##0.00€;[Red]-#,##0.00€";

            // Cabecera
            _excel.SetCellValue(1, 1, "");
            _excel.SetCellValue(1, 2, "ENERO");
            _excel.SetCellValue(1, 3, "FEBRERO");
            _excel.SetCellValue(1, 4, "MARZO");
            _excel.SetCellValue(1, 5, "ABRIL");
            _excel.SetCellValue(1, 6, "MAYO");
            _excel.SetCellValue(1, 7, "JUNIO");
            _excel.SetCellValue(1, 8, "JULIO");
            _excel.SetCellValue(1, 9, "AGOSTO");
            _excel.SetCellValue(1, 10, "SEPTIEMBRE");
            _excel.SetCellValue(1, 11, "OCTUBRE");
            _excel.SetCellValue(1, 12, "NOVIEMBRE");
            _excel.SetCellValue(1, 13, "DICIEMBRE");
            _excel.SetCellValue(1, 14, "AÑO");
            _excel.SetRowStyle(1, style_lv2);

            int row = 2;

            // Calcular totales
            List<decimal> list_ingresos = new List<decimal>();
            List<decimal> list_gastos = new List<decimal>();

            for (int month = 1; month <= 12; month++)
            {
                decimal ingMes = lista_ingresos.Where(t => t.month == month).Sum(t => t.tax_value);
                list_ingresos.Add(ingMes);

                decimal gasMes = 0;
                if (gastosPorMes.ContainsKey(month))
                {
                    foreach (var area in gastosPorMes[month].Values)
                    {
                        gasMes += area.Where(kv => kv.Key > 0).Sum(kv => kv.Value);
                    }
                }
                list_gastos.Add(gasMes);
            }

            // Resultado
            _excel.SetCellValue(row, 1, "[ = ] RESULTADO");
            decimal price_total = 0;
            for (int index = 0; index < 12; index++)
            {
                decimal _price = list_ingresos[index] - list_gastos[index];
                _excel.SetCellValue(row, index + 2, Math.Round(_price, 2));
                price_total += _price;
            }
            _excel.SetCellValue(row, 14, Math.Round(price_total, 2));
            _excel.SetRowStyle(row, style_lv1);
            row++;

            // Ingresos
            _excel.SetCellValue(row, 1, "[ + ] INGRESOS");
            price_total = 0;
            for (int index = 0; index < 12; index++)
            {
                _excel.SetCellValue(row, index + 2, Math.Round(list_ingresos[index], 2));
                price_total += list_ingresos[index];
            }
            _excel.SetCellValue(row, 14, Math.Round(price_total, 2));
            _excel.SetRowStyle(row, style_lv1);
            row++;

            // Subcategorías de ingresos
            foreach (var area in Estructura.AreasIngresos.OrderBy(a => a.Orden))
            {
                _excel.SetCellValue(row, 1, "     " + area.Nombre);
                price_total = 0;
                for (int month = 1; month <= 12; month++)
                {
                    decimal _price = lista_ingresos
                        .Where(t => t.month == month && t.type == area.Nombre)
                        .Sum(t => t.tax_value);
                    _excel.SetCellValue(row, month + 1, Math.Round(_price, 2));
                    price_total += _price;
                }
                _excel.SetCellValue(row, 14, Math.Round(price_total, 2));
                row++;
            }

            // Gastos
            _excel.SetCellValue(row, 1, "[ - ] GASTOS");
            price_total = 0;
            for (int index = 0; index < 12; index++)
            {
                _excel.SetCellValue(row, index + 2, Math.Round(list_gastos[index], 2));
                price_total += list_gastos[index];
            }
            _excel.SetCellValue(row, 14, Math.Round(price_total, 2));
            _excel.SetRowStyle(row, style_lv1);
            row++;

            // Aplicar formato de números
            _excel.SetCellStyle(2, 2, row, 14, style_number);
            _excel.AutoFitColumn(1, 14);

            // Guardar y enviar
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file_name);
            _excel.SaveAs(Response.OutputStream);
            Response.End();
        }

        #endregion
    }

    #region Clases auxiliares

    public class Tax
    {
        public int month { get; set; }
        public string type { get; set; }
        public decimal tax_value { get; set; }
    }

    /// <summary>
    /// Clase para la estructura financiera completa
    /// </summary>
    public class EstructuraFinanciera
    {
        public int VersionID { get; set; }
        public List<AreaFinanciera> AreasIngresos { get; set; }
        public List<AreaFinanciera> AreasGastos { get; set; }

        public EstructuraFinanciera()
        {
            AreasIngresos = new List<AreaFinanciera>();
            AreasGastos = new List<AreaFinanciera>();
        }
    }

    /// <summary>
    /// Clase para áreas financieras (nivel 1)
    /// </summary>
    public class AreaFinanciera
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public int Orden { get; set; }
        public string TipoCategoria { get; set; }
        public List<SubareaFinanciera> Subareas { get; set; }

        public AreaFinanciera()
        {
            Subareas = new List<SubareaFinanciera>();
        }
    }

    /// <summary>
    /// Clase para subareas financieras (nivel 2)
    /// </summary>
    public class SubareaFinanciera
    {
        public int ID { get; set; }
        public int AreaID { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public int Orden { get; set; }
        public bool TieneHijos { get; set; }
        public List<Subarea2Financiera> Subareas2 { get; set; }

        public SubareaFinanciera()
        {
            Subareas2 = new List<Subarea2Financiera>();
        }
    }

    /// <summary>
    /// Clase para subareas nivel 2 (nivel 3)
    /// </summary>
    public class Subarea2Financiera
    {
        public int ID { get; set; }
        public int SubareaID { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public int Orden { get; set; }
    }

    #endregion
}