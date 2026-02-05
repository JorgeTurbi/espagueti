using campus_sbs_admin.Models;
using campus_sbs_admin.Models.DTOS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class admin_facturas_list : System.Web.UI.Page
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
            List<int> yearList;

            using (var db = new SpainBS_Connection())
            {
                yearList = db.INF_FIN_FACTURAS
                    .Where(x => x.fecha_emision != null)
                    .Select(x => x.fecha_emision.Year)
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

            ddlyear.SelectedValue = currentYear.ToString();
        }

        public class FacturaFilterDto
        {
            public string desde { get; set; }     // dd/MM/yyyy
            public string hasta { get; set; }     // dd/MM/yyyy
            public string sociedad { get; set; }
            public string year { get; set; }      // "2026"

            public bool pendCobro { get; set; }
            public bool pendAtrib { get; set; }
            public bool pendVenc { get; set; }
        }

        public class FacturaRowDto
        {
            public long idInfFinFacturas { get; set; }
            public string sociedad { get; set; }
            public string cliente_nombre { get; set; }
            public long numero { get; set; }
            public string eur_precio_str { get; set; }
            public string eur_fundacion_str { get; set; }
            public string eur_universidad_str { get; set; }
            public string eur_tripartita_str { get; set; }
            public string eur_iva_str { get; set; }
            public string eur_irpf_str { get; set; }
            public string eur_total_str { get; set; }

            public string fecha_vencimiento_str { get; set; }
            public string fecha_cobro_str { get; set; }

            public string atribucion { get; set; }
            public string descripcion { get; set; }
            public string fichero { get; set; } // por si luego lo necesitas
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetFacturas(FacturaFilterDto f)
        {
            try
            {
                f = f ?? new FacturaFilterDto();

                // dd/MM/yyyy
                DateTime? d1 = TryEsDate(f.desde);
                DateTime? d2 = TryEsDate(f.hasta);

                // year
                int year = 0;
                if (!string.IsNullOrWhiteSpace(f.year))
                    int.TryParse(f.year.Trim(), out year);

                using (var db = new SpainBS_Connection())
                {
                    var q = db.INF_FIN_FACTURAS.AsQueryable();

                    // sociedad
                    if (!string.IsNullOrWhiteSpace(f.sociedad))
                    {
                        var soc = f.sociedad.Trim().ToUpper();
                        q = q.Where(x => x.sociedad != null && x.sociedad.ToUpper() == soc);
                    }

                    // año (si lo mandan)
                    if (year > 0)
                    {
                        q = q.Where(x => x.fecha_emision != null && x.fecha_emision.Year == year);
                    }

                    // rango fechas (SIN usar .Date en LINQ)
                    if (d1.HasValue)
                        q = q.Where(x => DbFunctions.TruncateTime(x.fecha_emision) >= DbFunctions.TruncateTime(d1.Value));

                    if (d2.HasValue)
                        q = q.Where(x => DbFunctions.TruncateTime(x.fecha_emision) <= DbFunctions.TruncateTime(d2.Value));

                    // checks
                    // Pendiente cobro => fecha_cobro == null
                    if (f.pendCobro)
                        q = q.Where(x => x.fecha_cobro == null);

                    // Pendiente atribución => atribucion null/vacía
                    if (f.pendAtrib)
                        q = q.Where(x => x.atribucion == null || x.atribucion.Trim() == "");

                    // Pendiente vencimiento => fecha_vencimiento < hoy y fecha_cobro == null
                    if (f.pendVenc)
                    {
                        var today = DateTime.Today;
                        q = q.Where(x => x.fecha_vencimiento != null
                                      && DbFunctions.TruncateTime(x.fecha_vencimiento) < DbFunctions.TruncateTime(today)
                                      && x.fecha_cobro == null);
                    }

                    var es = CultureInfo.GetCultureInfo("es-ES");

                    var list = q
                        .OrderByDescending(x => x.idInfFinFacturas)
                        .Take(5000)
                        .ToList()
                        .Select(x => new FacturaRowDto
                        {
                            idInfFinFacturas = x.idInfFinFacturas,
                            sociedad = x.sociedad,
                            cliente_nombre=x.cliente_nombre,
                            numero = x.numero,
                            eur_precio_str = (x.eur_precio).ToString("N2", es),
                            eur_fundacion_str = (x.eur_fundacion).ToString("N2", es),
                            eur_universidad_str = (x.eur_universidad).ToString("N2", es),
                            eur_tripartita_str = (x.eur_tripartita).ToString("N2", es),
                            eur_iva_str = (x.eur_iva).ToString("N2", es),
                            eur_irpf_str = (x.eur_irpf).ToString("N2", es),
                            eur_total_str = (x.eur_total).ToString("N2", es),

                            fecha_vencimiento_str = x.fecha_vencimiento.HasValue ? x.fecha_vencimiento.Value.ToString("dd/MM/yyyy") : "",
                            fecha_cobro_str = x.fecha_cobro.HasValue ? x.fecha_cobro.Value.ToString("dd/MM/yyyy") : "",

                            atribucion = x.atribucion,
                            descripcion = x.descripcion,
                            fichero = x.fichero
                        })
                        .ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    ok = false,
                    message = "Error en GetFacturas",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                };
            }
        }

        [WebMethod]
        public static object LoadSociedad()
        {
            try
            {
                using (var db = new SpainBS_Connection())
                {
                    // Ejemplo: tabla Sociedades
                    var data = db.Socio                        
                        .OrderBy(x => x.Nombre)
                        .Select(x => new {
                            Id = x.Id,
                            Nombre = x.Nombre
                        })
                        .ToList();

                    return new { Ok = true, Data = data };
                }
            }
            catch (Exception ex)
            {
                return new { Ok = false, Message = ex.Message };
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object DeleteFactura(long id)
        {
            try
            {
                using (var db = new SpainBS_Connection())
                {
                    var entity = db.INF_FIN_FACTURAS.FirstOrDefault(x => x.idInfFinFacturas == id);
                    if (entity == null)
                        return new { ok = false, message = "Factura no encontrada." };

                    if (!string.IsNullOrWhiteSpace(entity.fichero))
                    {
                        DeleteFacturaFilePhysical(entity.fichero);
                    }

                    db.INF_FIN_FACTURAS.Remove(entity);
                    db.SaveChanges();

                    return new { ok = true, message = "Factura eliminada correctamente." };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    ok = false,
                    message = "No se pudo eliminar.",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                };
            }
        }

        private static DateTime? TryEsDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            // dd/MM/yyyy
            if (DateTime.TryParseExact(value.Trim(), "dd/MM/yyyy",
                CultureInfo.GetCultureInfo("es-ES"), DateTimeStyles.None, out var d))
                return d.Date;

            return null;
        }

        private static void DeleteFacturaFilePhysical(string fileName)
        {
            var rel = ConfigurationManager.AppSettings["routeFacturas"] ?? "~/documentos/facturas/";
            var folder = HttpContext.Current.Server.MapPath(rel);

            var safe = Path.GetFileName(fileName);
            var fullPath = Path.Combine(folder, safe);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
