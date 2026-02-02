using campus_sbs_admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class Admin_Gastos : System.Web.UI.Page
    {
        private string RouteGastos => (ConfigurationManager.AppSettings["routeGastos"] ?? "").Trim();
        private string UrlGastos => (ConfigurationManager.AppSettings["urlGastos"] ?? "").Trim();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                load_data();

                // ✅ EDIT MODE por QueryString
                long idg = 0;
                long.TryParse(Request.QueryString["idg"], out idg);

                if (idg > 0)
                {
                    load_expense(idg);

                    // mostrar botón eliminar (solo en edición)
                    // el botón es HTML, lo mostramos por JS leyendo basic_id,
                    // pero por si acaso puedes setear un flag:
                    // btnDeleteAjax.Visible no existe (no es server control)
                }
                else
                {
                    // default: set fecha emision al 01/01 del año elegido
                    var y = ddlyear.Value;
                    if (!string.IsNullOrWhiteSpace(y))
                        date_emision.Value = "01/01/" + y;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin_Gastos.aspx");
        }

        protected void btn_del_file_Click(object sender, EventArgs e)
        {
            // si quieres eliminar archivo con postback (opcional).
        }

        #region Load inicial UI

        private void load_data()
        {
            load_company();
            loadYears();          // ✅ basado en COSTES
            loadTeachers();
            load_area();
            load_empty_subareas();

            basic_LH_year.Value = DateTime.Today.ToString("yy");

            if (string.IsNullOrWhiteSpace(date_emision.Value))
                date_emision.Value = DateTime.Today.ToString("dd/MM/yyyy");

            basic_LH.Attributes["class"] = AddHidden(basic_LH.Attributes["class"]);
            basic_tax.Attributes["class"] = AddHidden(basic_tax.Attributes["class"]);

            lnkFile.Attributes["class"] = AddHidden(lnkFile.Attributes["class"]);
            btn_del_file.CssClass = AddHidden(btn_del_file.CssClass);
        }

        private void load_company()
        {
            basic_sociedad.Items.Clear();
            using (var db = new SpainBS_Connection())
            {
                var partnerList = db.Socio.AsNoTracking()
                    .Select(a => a.Nombre)
                    .ToList();

                basic_sociedad.Items.Add(new ListItem("Selecciona Sociedad", ""));
                foreach (var y in partnerList)
                    basic_sociedad.Items.Add(new ListItem(y, y));
            }
            basic_sociedad.Value = "";
        }

        private void loadYears()
        {
            ddlyear.Items.Clear();
            List<int> yearList;

            using (var db = new SpainBS_Connection())
            {
                yearList = db.INF_FIN_COSTES
                    .Where(x => x.FEmision != null)
                    .Select(x => x.FEmision.Year)
                    .Distinct()
                    .ToList();
            }

            ddlyear.Items.Add(new ListItem("Selecciona el año", ""));

            int currentYear = DateTime.Today.Year;
            yearList.Add(currentYear);
            yearList.Add(currentYear + 1);

            yearList = yearList.Distinct().OrderByDescending(y => y).ToList();

            foreach (var y in yearList)
                ddlyear.Items.Add(new ListItem(y.ToString(), y.ToString()));

            ddlyear.Value = currentYear.ToString();
        }

        private void loadTeachers()
        {
            ddlTeacher.Items.Clear();
            ddlTeacher.Items.Add(new ListItem("Seleccione un profesor", ""));

            using (var db = new SpainBS_Connection())
            {
                // Si tu campus_LH usa idPersona (long) real, lo correcto es cargar ID + Nombre
                // Aquí mantengo tu patrón simple para no romper diseño: value = nombre
                var lista = db.EXPERTOS
                    .Where(x => !string.IsNullOrEmpty(x.Nombre))
                    .Select(x => x.Nombre)
                    .Distinct()
                    .ToList();

                foreach (var item in lista)
                    ddlTeacher.Items.Add(new ListItem(item, item));
            }

            ddlTeacher.Value = "";
        }

        private void load_area()
        {
            ddlArea.Items.Clear();
            ddlArea.Items.Add(new ListItem("Seleccione un Área", ""));

            using (var db = new SpainBS_Connection())
            {
                var list = db.INF_AUX
                    .Where(x => x.Tabla == "AREA")
                    .OrderBy(x => x.Valor)
                    .Select(x => new { x.id_inf_aux, x.Valor })
                    .ToList();

                foreach (var a in list)
                    ddlArea.Items.Add(new ListItem(a.Valor, a.id_inf_aux.ToString()));
            }
        }

        private void load_empty_subareas()
        {
            ddlSubArea.Items.Clear();
            ddlSubArea.Items.Add(new ListItem("Seleccione un Subárea", ""));

            ddlSubArea2.Items.Clear();
            ddlSubArea2.Items.Add(new ListItem("Seleccione un Subárea 2", ""));
        }

        private void load_expense(long idGasto)
        {
            using (var db = new SpainBS_Connection())
            {
                var e = db.INF_FIN_COSTES.FirstOrDefault(x => x.IdInfFinCostes == idGasto);
                if (e == null) return;

                basic_id.Value = e.IdInfFinCostes.ToString();
                basic_sociedad.Value = e.Sociedad ?? "";
                ddlyear.Value = e.FEmision.Year.ToString();

                basic_secuence.Value = e.NumSecuencia.ToString();
                date_emision.Value = e.FEmision.ToString("dd/MM/yyyy");

                basic_company.Value = e.EmpresaNombre ?? "";
                basic_cif.Value = e.EmpresaCIF ?? "";
                basic_description.Value = e.Descripcion ?? "";

                basic_subtotal.Value = e.Subtotal.ToString("0.##");
                basic_iva.Value = e.IVA.HasValue ? e.IVA.Value.ToString("0.##") : "";
                basic_irpf.Value = e.IRPF.HasValue ? e.IRPF.Value.ToString("0.##") : "";
                basic_total.Value = e.Total.HasValue ? e.Total.Value.ToString("0.##") : "";

                ddlArea.Value = e.Area.HasValue ? e.Area.Value.ToString() : "";
                ddlSubArea.Value = e.SubArea.HasValue ? e.SubArea.Value.ToString() : "";
                ddlSubArea2.Value = e.SubArea2.HasValue ? e.SubArea2.Value.ToString() : "";

                date_pay.Value = e.FPago.HasValue ? e.FPago.Value.ToString("dd/MM/yyyy") : "";
                basic_banc.Value = e.ApunteBanco ?? "";

                basic_chk_fact.Checked = e.NoFactura == true;
                basic_chk_provision.Checked = e.Provision == true;

                basic_tax_chk.Checked = e.Tax == true;
                ddlTax.Value = e.Tax_type ?? "";

                basic_comments.Value = e.Comentarios ?? "";

                if (!string.IsNullOrEmpty(e.Fichero))
                {
                    fuFile.Visible = false;

                    lnkFile.InnerText = "Ver archivo";
                    lnkFile.HRef = UrlGastos + e.Fichero;
                    lnkFile.Attributes["class"] = RemoveHidden(lnkFile.Attributes["class"]);

                    btn_del_file.CssClass = RemoveHidden(btn_del_file.CssClass);
                }

                var lh = db.campus_LH.FirstOrDefault(x => x.idCoste == e.IdInfFinCostes);
                if (lh != null)
                {
                    basic_chk.Checked = true;
                    basic_LH.Attributes["class"] = RemoveHidden(basic_LH.Attributes["class"]);

                    // Si tu ddlTeacher usa nombre, aquí debes mapearlo.
                    // Si usa ID real, cambia a lh.idPersona.ToString();
                    ddlTeacher.Value = lh.idPersona.ToString();

                    tipo_liquidacion.Value = lh.Online.ToString();
                    basic_LH_comment.Value = lh.Comentarios ?? "";

                    if (!string.IsNullOrEmpty(lh.LH) && lh.LH.Contains("-"))
                    {
                        var parts = lh.LH.Split('-');
                        basic_LH_year.Value = parts[0];
                        basic_LH_number.Value = parts.Length > 1 ? parts[1] : "";
                    }
                }

                if (basic_tax_chk.Checked)
                    basic_tax.Attributes["class"] = RemoveHidden(basic_tax.Attributes["class"]);
            }
        }

        #endregion

        #region DTO + AJAX Save

        public class SaveGastoRequest
        {
            public long id { get; set; }

            public string sociedad { get; set; }
            public int secuencia { get; set; }
            public string fecha_emision { get; set; }

            public string empresa { get; set; }
            public string cif { get; set; }
            public string descripcion { get; set; }

            public string subtotal { get; set; }
            public string iva { get; set; }
            public string irpf { get; set; }
            public string total { get; set; }

            public string area { get; set; }
            public string subarea { get; set; }
            public string subarea2 { get; set; }

            public string fecha_pago { get; set; }
            public string apunte_banco { get; set; }

            public bool no_factura { get; set; }
            public bool provision { get; set; }

            public bool tax { get; set; }
            public string tax_type { get; set; }

            public bool is_lh { get; set; }
            public string teacher { get; set; }
            public string lh_year { get; set; }
            public string lh_number { get; set; }
            public string lh_comment { get; set; }
            public string tipo_liquidacion { get; set; }

            public string comentarios { get; set; }

            public string fichero { get; set; }
        }

        public class AjaxResult
        {
            public bool ok { get; set; }
            public string message { get; set; }
            public long id { get; set; }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static AjaxResult SaveGasto(SaveGastoRequest r)
        {
            try
            {
                if (r == null) return new AjaxResult { ok = false, message = "Solicitud inválida." };

                var society = (r.sociedad ?? "").Trim();
                var company = (r.empresa ?? "").Trim();
                var cif = (r.cif ?? "").Trim();
                var description = (r.descripcion ?? "").Trim();

                var fEmision = ParseDateStatic(r.fecha_emision);
                var subtotal = ParseDecimalStatic(r.subtotal, 0m);
                var iva = ParseDecimalStatic(r.iva, 0m);
                var irpf = ParseDecimalStatic(r.irpf, 0m);
                var total = ParseDecimalStatic(r.total, 0m);

                if (string.IsNullOrEmpty(society) || r.secuencia <= 0 || fEmision == default(DateTime) ||
                    string.IsNullOrEmpty(company) || string.IsNullOrEmpty(cif) ||
                    string.IsNullOrEmpty(description) || subtotal <= 0m)
                {
                    return new AjaxResult { ok = false, message = "Todos los campos marcados con * son obligatorios." };
                }

                // ✅ tolerancia para decimales
                var expected = (subtotal + iva) - irpf;
                if (Math.Abs(total - expected) > 0.01m)
                    total = expected;

                if (r.tax && string.IsNullOrWhiteSpace(r.tax_type))
                    return new AjaxResult { ok = false, message = "Al marcar un impuesto hay que seleccionar el tipo." };

                int area = TryInt(r.area);
                int subarea = TryInt(r.subarea);
                int subarea2 = TryInt(r.subarea2);

                var fPago = ParseDateStatic(r.fecha_pago);
                var bank = (r.apunte_banco ?? "").Trim();

                using (var db = new SpainBS_Connection())
                {
                    INF_FIN_COSTES entity;

                    if (r.id > 0)
                    {
                        entity = db.INF_FIN_COSTES.FirstOrDefault(x => x.IdInfFinCostes == r.id);
                        if (entity == null) return new AjaxResult { ok = false, message = "No se encontró el gasto a actualizar." };
                    }
                    else
                    {
                        entity = new INF_FIN_COSTES();
                        db.INF_FIN_COSTES.Add(entity);
                    }

                    entity.Sociedad = society;
                    entity.NumSecuencia = r.secuencia;
                    entity.FEmision = fEmision;

                    entity.EmpresaNombre = company;
                    entity.EmpresaCIF = cif;
                    entity.Descripcion = description;

                    entity.Subtotal = subtotal;
                    entity.IVA = (iva == 0m ? (decimal?)null : iva);
                    entity.IRPF = (irpf == 0m ? (decimal?)null : irpf);
                    entity.Total = (decimal?)total;

                    entity.Area = (area == 0 ? (int?)null : area);
                    entity.SubArea = (subarea == 0 ? (int?)null : subarea);
                    entity.SubArea2 = (subarea2 == 0 ? (int?)null : subarea2);

                    entity.FPago = (fPago == default(DateTime) ? (DateTime?)null : fPago);
                    entity.ApunteBanco = bank;

                    entity.NoFactura = r.no_factura;
                    entity.Provision = r.provision;

                    entity.Tax = r.tax;
                    entity.Tax_type = (r.tax ? (r.tax_type ?? "").Trim() : null);

                    entity.Comentarios = (r.comentarios ?? "").Trim();

                    if (!string.IsNullOrWhiteSpace(r.fichero))
                        entity.Fichero = r.fichero.Trim();

                    db.SaveChanges();

                    return new AjaxResult { ok = true, message = "Gasto guardado correctamente.", id = entity.IdInfFinCostes };
                }
            }
            catch (Exception ex)
            {
                return new AjaxResult { ok = false, message = "Ha ocurrido un error guardando el gasto: " + ex.Message };
            }
        }

        // ✅ DELETE desde el formulario
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static AjaxResult DeleteGasto(long id)
        {
            try
            {
                if (id <= 0) return new AjaxResult { ok = false, message = "ID inválido." };

                string route = (ConfigurationManager.AppSettings["routeGastos"] ?? "").Trim();

                using (var db = new SpainBS_Connection())
                {
                    var e = db.INF_FIN_COSTES.FirstOrDefault(x => x.IdInfFinCostes == id);
                    if (e == null) return new AjaxResult { ok = false, message = "No se encontró el gasto." };

                    if (!string.IsNullOrEmpty(e.Fichero) && !string.IsNullOrEmpty(route))
                    {
                        var full = Path.Combine(route, e.Fichero);
                        if (File.Exists(full)) File.Delete(full);
                    }

                    var lh = db.campus_LH.FirstOrDefault(x => x.idCoste == id);
                    if (lh != null) db.campus_LH.Remove(lh);

                    db.INF_FIN_COSTES.Remove(e);
                    db.SaveChanges();
                }

                return new AjaxResult { ok = true, message = "Gasto eliminado correctamente.", id = id };
            }
            catch (Exception ex)
            {
                return new AjaxResult { ok = false, message = "Error eliminando: " + ex.Message };
            }
        }

        private static int TryInt(string s)
        {
            if (int.TryParse((s ?? "").Trim(), out int v)) return v;
            return 0;
        }

        private static DateTime ParseDateStatic(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return default(DateTime);

            if (DateTime.TryParseExact(input.Trim(),
                new[] { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy" },
                new CultureInfo("es-ES"),
                DateTimeStyles.None,
                out DateTime d))
                return d;

            if (DateTime.TryParse(input.Trim(), new CultureInfo("es-ES"), DateTimeStyles.None, out d))
                return d;

            return default(DateTime);
        }

        private static decimal ParseDecimalStatic(string input, decimal def)
        {
            if (string.IsNullOrWhiteSpace(input)) return def;

            var s = input.Trim();
            if (decimal.TryParse(s, NumberStyles.Any, new CultureInfo("es-ES"), out decimal v))
                return v;

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                return v;

            s = s.Replace(".", "").Replace(",", ".");
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                return v;

            return def;
        }

        #endregion

        #region WebMethods existentes (subareas + secuencia)

        [WebMethod(Description = "Carga Subáreas/Subáreas2 por ID padre")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> searchSubArea(string table, string idArea)
        {
            var result = new List<object>();
            if (string.IsNullOrWhiteSpace(table) || string.IsNullOrWhiteSpace(idArea))
                return result;

            if (!int.TryParse(idArea, out int parentId))
                return result;

            using (var db = new SpainBS_Connection())
            {
                var list = db.INF_AUX
                    .Where(x => x.Tabla == table && x.ID_Padre == parentId)
                    .OrderBy(x => x.Valor)
                    .Select(x => new { x.id_inf_aux, x.Valor })
                    .ToList();

                foreach (var a in list)
                    result.Add(new { id_inf_aux = a.id_inf_aux, Valor = a.Valor });
            }

            return result;
        }

        [WebMethod(Description = "Devuelve el siguiente número de secuencia (000) por sociedad y año")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string searchSecuence(string sociedad, string _year)
        {
            if (string.IsNullOrEmpty(sociedad) || string.IsNullOrEmpty(_year))
                return "000";

            int year = int.Parse(_year);

            using (var db = new SpainBS_Connection())
            {
                int last = db.INF_FIN_COSTES
                    .Where(x => x.Sociedad == sociedad && x.FEmision.Year == year)
                    .Select(x => x.NumSecuencia)
                    .DefaultIfEmpty(0)
                    .Max();

                return (last + 1).ToString("000");
            }
        }

        #endregion

        #region Helpers UI

        private static string AddHidden(string css)
        {
            css = (css ?? "").Trim();
            if (!css.Contains("d-none-imp")) css = (css + " d-none-imp").Trim();
            return css;
        }

        private static string RemoveHidden(string css)
        {
            css = (css ?? "");
            css = css.Replace("d-none-imp", "").Trim();
            return css;
        }

        #endregion
    }
}
