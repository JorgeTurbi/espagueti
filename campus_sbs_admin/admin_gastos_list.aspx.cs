using campus_sbs_admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class admin_gastos_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYears();
            }
        }

        private void LoadYears()
        {
            ddlyear.Items.Clear();

            var years = new List<int>();
            using (var db = new SpainBS_Connection())
            {
                years = db.INF_FIN_COSTES
                    .Select(x => x.FEmision.Year)
                    .Distinct()
                    .ToList();
            }

            ddlyear.Items.Add(new ListItem("Año", ""));

            int currentYear = DateTime.Today.Year;
            years.Add(currentYear);
            years.Add(currentYear + 1);

            years = years.Distinct().OrderByDescending(x => x).ToList();

            foreach (var y in years)
                ddlyear.Items.Add(new ListItem(y.ToString(), y.ToString()));

            ddlyear.SelectedValue = currentYear.ToString();
        }

        // ==========================
        // DTOs
        // ==========================
        public class GastosFilter
        {
            public string sociedad { get; set; }
            public string desde { get; set; }
            public string hasta { get; set; }
            public string year { get; set; }

            public bool noFactura { get; set; }
            public bool noCatalog { get; set; }
            public bool impuestos { get; set; }
            public bool noPago { get; set; }
            public bool noBanco { get; set; }
            public bool provision { get; set; }
        }

        public class GastoRow
        {
            public long idInfFinCostes { get; set; }
            public string sociedad { get; set; }
            public int num { get; set; }
            public string datos_empresa { get; set; }
            public string f_emision { get; set; }
            public string descripcion { get; set; }

            public string eur_subtotal { get; set; }
            public string eur_iva { get; set; }
            public string eur_irpf { get; set; }
            public string eur_total { get; set; }

            public string f_pago { get; set; }
            public string banco { get; set; }

            public string no_factura { get; set; }
            public string catalog { get; set; }
        }

        public class Result
        {
            public bool ok { get; set; }
            public string message { get; set; }
        }

        // ==========================
        // WebMethods
        // ==========================
        [WebMethod]
        public static object GetGastos(GastosFilter f)
        {
            try
            {
                f = f ?? new GastosFilter();
                var es = new CultureInfo("es-ES");

                DateTime? desde = ParseNullableDate(f.desde);
                DateTime? hasta = ParseNullableDate(f.hasta);

                int? year = null;
                if (!string.IsNullOrWhiteSpace(f.year) && int.TryParse(f.year, out int yy))
                    year = yy;

                using (var db = new SpainBS_Connection())
                {
                    var q = db.INF_FIN_COSTES.AsNoTracking().AsQueryable();

                    if (!string.IsNullOrWhiteSpace(f.sociedad))
                        q = q.Where(x => x.Sociedad == f.sociedad);

                    if (year.HasValue)
                        q = q.Where(x => x.FEmision.Year == year.Value);

                    if (desde.HasValue)
                        q = q.Where(x => x.FEmision >= desde.Value);

                    if (hasta.HasValue)
                        q = q.Where(x => x.FEmision <= hasta.Value);

                    if (f.noFactura)
                        q = q.Where(x => x.NoFactura == true);

                    if (f.provision)
                        q = q.Where(x => x.Provision == true);

                    if (f.impuestos)
                        q = q.Where(x => x.Tax == true);

                    if (f.noPago)
                        q = q.Where(x => x.FPago == null);

                    if (f.noBanco)
                        q = q.Where(x => x.ApunteBanco == null || x.ApunteBanco.Trim() == "");

                    if (f.noCatalog)
                        q = q.Where(x => x.Area == null || x.SubArea == null || x.SubArea2 == null);

                    var list = q
                        .OrderByDescending(x => x.FEmision)
                        .ThenByDescending(x => x.IdInfFinCostes)
                        .Take(5000)
                        .ToList();

                    var rows = list.Select(x =>
                    {
                        var empresa = (x.EmpresaNombre ?? "").Trim();
                        var cif = (x.EmpresaCIF ?? "").Trim();
                        var datos = string.IsNullOrEmpty(cif) ? empresa : $"{empresa} [{cif}]";

                        var catalog = "";
                        if (x.Area.HasValue && x.SubArea.HasValue && x.SubArea2.HasValue)
                            catalog = $"{x.Area.Value}/{x.SubArea.Value}/{x.SubArea2.Value}";

                        return new GastoRow
                        {
                            idInfFinCostes = x.IdInfFinCostes,
                            sociedad = x.Sociedad ?? "",
                            num = x.NumSecuencia,

                            datos_empresa = datos,
                            f_emision = x.FEmision.ToString("dd/MM/yyyy"),
                            descripcion = x.Descripcion ?? "",

                            eur_subtotal = FormatEur(x.Subtotal, es),
                            eur_iva = FormatEur(x.IVA, es),
                            eur_irpf = FormatEur(x.IRPF, es),
                            eur_total = FormatEur(x.Total, es),

                            f_pago = x.FPago.HasValue ? x.FPago.Value.ToString("dd/MM/yyyy") : "",
                            banco = x.ApunteBanco ?? "",

                            no_factura = (x.NoFactura == true) ? "SI" : "",
                            catalog = catalog
                        };
                    }).ToList();

                    return rows;
                }
            }
            catch (Exception ex)
            {
                return new { ok = false, message = "Error cargando gastos: " + ex.Message };
            }
        }

        [WebMethod]
        public static Result DeleteGasto(long id)
        {
            try
            {
                if (id <= 0) return new Result { ok = false, message = "ID inválido." };

                string route = (ConfigurationManager.AppSettings["routeGastos"] ?? "").Trim();

                using (var db = new SpainBS_Connection())
                {
                    var e = db.INF_FIN_COSTES.FirstOrDefault(x => x.IdInfFinCostes == id);
                    if (e == null) return new Result { ok = false, message = "No se encontró el gasto." };

                    // borrar archivo
                    if (!string.IsNullOrEmpty(e.Fichero) && !string.IsNullOrEmpty(route))
                    {
                        var full = Path.Combine(route, e.Fichero);
                        if (File.Exists(full))
                            File.Delete(full);
                    }

                    // borrar LH
                    var lh = db.campus_LH.FirstOrDefault(x => x.idCoste == id);
                    if (lh != null) db.campus_LH.Remove(lh);

                    db.INF_FIN_COSTES.Remove(e);
                    db.SaveChanges();
                }

                return new Result { ok = true, message = "Gasto eliminado correctamente." };
            }
            catch (Exception ex)
            {
                return new Result { ok = false, message = "Error eliminando: " + ex.Message };
            }
        }

        // ==========================
        // Helpers
        // ==========================
        private static DateTime? ParseNullableDate(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;

            if (DateTime.TryParseExact(
                s.Trim(),
                new[] { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy" },
                new CultureInfo("es-ES"),
                DateTimeStyles.None,
                out DateTime d))
                return d;

            if (DateTime.TryParse(s.Trim(), new CultureInfo("es-ES"), DateTimeStyles.None, out d))
                return d;

            return null;
        }

        private static string FormatEur(decimal? n, CultureInfo es)
        {
            if (!n.HasValue) return "";
            return n.Value.ToString("N2", es);
        }
    }
}
