using campus_sbs_admin.Models;
using campus_sbs_admin.Models.DTOS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace campus_sbs_admin
{
    public partial class admin_facturas_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No postback needed (AJAX)
        }

        // ============================
        //  DTO de salida para el Edit
        // ============================
        public class FacturaEditVm
        {
            public long idInfFinFacturas { get; set; }
            public string sociedad { get; set; }
            public int numero { get; set; }

            // ISO strings para input type="date"
            public string fecha_emision { get; set; }
            public string fecha_vencimiento { get; set; }
            public string fecha_cobro { get; set; }

            public string cliente_nombre { get; set; }
            public string cliente_nif { get; set; }
            public string descripcion { get; set; }
            public string comentarios { get; set; }
            public string atribucion { get; set; }
            public string fichero { get; set; }

            // strings para UI (puedes cambiar formato si quieres)
            public string eur_pvp_str { get; set; }
            public string eur_beca_str { get; set; }
            public string eur_dto_str { get; set; }
            public string eur_precio_str { get; set; }
            public string eur_fundacion_str { get; set; }
            public string eur_universidad_str { get; set; }
            public string eur_tripartita_str { get; set; }
            public string eur_iva_str { get; set; }
            public string eur_irpf_str { get; set; }
            public string eur_total_str { get; set; }
        }

        // ============================
        //  WebMethods
        // ============================

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetFactura(long id)
        {
            try
            {
                using (var db = new SpainBS_Connection())
                {
                    var e = db.INF_FIN_FACTURAS.AsNoTracking().FirstOrDefault(x => x.idInfFinFacturas == id);
                    if (e == null)
                        return new { ok = false, message = "Factura no encontrada." };

                    var ci = CultureInfo.GetCultureInfo("es-ES");

                    string Iso(DateTime dt) => dt.ToString("yyyy-MM-dd");
                    string Money(decimal v) => v.ToString("N2", ci);

                    var vm = new FacturaEditVm
                    {
                        idInfFinFacturas = e.idInfFinFacturas,
                        sociedad = e.sociedad,
                        numero = e.numero,

                        fecha_emision = Iso(e.fecha_emision),
                        fecha_vencimiento = e.fecha_vencimiento.HasValue ? Iso(e.fecha_vencimiento.Value) : null,
                        fecha_cobro = e.fecha_cobro.HasValue ? Iso(e.fecha_cobro.Value) : null,

                        cliente_nombre = e.cliente_nombre,
                        cliente_nif = e.cliente_nif,
                        descripcion = e.descripcion,
                        comentarios = e.comentarios,
                        atribucion = e.atribucion,
                        fichero = e.fichero,

                        eur_pvp_str = Money(e.eur_pvp),
                        eur_beca_str = Money(e.eur_beca),
                        eur_dto_str = Money(e.eur_dto),
                        eur_precio_str = Money(e.eur_precio),
                        eur_fundacion_str = Money(e.eur_fundacion),
                        eur_universidad_str = Money(e.eur_universidad),
                        eur_tripartita_str = Money(e.eur_tripartita),
                        eur_iva_str = Money(e.eur_iva),
                        eur_irpf_str = Money(e.eur_irpf),
                        eur_total_str = Money(e.eur_total)
                    };

                    return new { ok = true, data = vm };
                }
            }
            catch (Exception ex)
            {
                return new { ok = false, message = "Error cargando factura.", errors = new[] { ex.Message, ex.InnerException?.Message } };
            }
        }

        public class AtribVm { public string codigo { get; set; } public string nombre { get; set; } }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<AtribVm> LoadAtribuciones()
        {
            using (var db = new SpainBS_Connection())
            {
                return db.Atribuciones
                    .Where(x => x.Activo == true)
                    .OrderBy(x => x.Nombre)
                    .Select(x => new AtribVm { codigo = x.Codigo, nombre = x.Nombre })
                    .ToList();
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object UpdateFactura(long id, FacturaDto dto)
        {
            try
            {
                if (dto == null) return new { ok = false, message = "Datos vacíos." };
                if (id <= 0) return new { ok = false, message = "ID inválido." };

                // Parse ISO dates
                DateTime fe;
                if (!TryParseIsoDate(dto.fecha_emision, out fe))
                    return new { ok = false, message = "Fecha emisión inválida (yyyy-MM-dd)." };

                DateTime? fv = ParseIsoDateOrNull(dto.fecha_vencimiento);
                DateTime? fc = ParseIsoDateOrNull(dto.fecha_cobro);

                DateTime minSql = new DateTime(1753, 1, 1);
                fe = fe.Date;
                if (fe < minSql) return new { ok = false, message = "Fecha emisión fuera de rango SQL." };
                if (fv.HasValue && fv.Value < minSql) fv = null;
                if (fc.HasValue && fc.Value < minSql) fc = null;
                if (fv.HasValue) fv = fv.Value.Date;
                if (fc.HasValue) fc = fc.Value.Date;

                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(dto.sociedad)) errors.Add("Sociedad es obligatoria.");
                if (dto.numero <= 0) errors.Add("Número inválido.");
                if (string.IsNullOrWhiteSpace(dto.cliente_nombre)) errors.Add("Cliente obligatorio.");
                if (string.IsNullOrWhiteSpace(dto.cliente_nif)) errors.Add("NIF obligatorio.");
                if (string.IsNullOrWhiteSpace(dto.descripcion)) errors.Add("Descripción obligatoria.");

                if (fv.HasValue && fv.Value < fe) errors.Add("Vencimiento no puede ser menor que emisión.");
                if (fc.HasValue && fc.Value < fe) errors.Add("Cobro no puede ser menor que emisión.");

                if (dto.eur_pvp < 0) errors.Add("PVP no puede ser negativo.");
                if (dto.eur_precio < 0) errors.Add("Precio no puede ser negativo.");
                if (dto.eur_tripartita < 0) errors.Add("Tripartita no puede ser negativo.");
                if (dto.eur_total < 0) errors.Add("Total no puede ser negativo.");

                if (errors.Count > 0) return new { ok = false, message = "Errores de validación.", errors };

                decimal R2(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);

                using (var db = new SpainBS_Connection())
                {
                    var entity = db.INF_FIN_FACTURAS.FirstOrDefault(x => x.idInfFinFacturas == id);
                    if (entity == null) return new { ok = false, message = "Factura no encontrada." };

                    var year = fe.Year;
                    var dup = db.INF_FIN_FACTURAS.Any(x =>
                        x.idInfFinFacturas != id &&
                        x.sociedad.ToUpper() == dto.sociedad.ToUpper() &&
                        x.numero == dto.numero &&
                        x.fecha_emision.Year == year);

                    if (dup)
                        return new { ok = false, message = "Ya existe una factura con esa sociedad y número en ese año." };

                    entity.sociedad = dto.sociedad;
                    entity.numero = dto.numero;
                    entity.fecha_emision = fe;

                    entity.cliente_nombre = dto.cliente_nombre;
                    entity.cliente_nif = dto.cliente_nif;
                    entity.descripcion = dto.descripcion;

                    entity.eur_pvp = R2(dto.eur_pvp);
                    entity.eur_beca = R2(dto.eur_beca);
                    entity.eur_dto = R2(dto.eur_dto);
                    entity.eur_precio = R2(dto.eur_precio);

                    entity.eur_fundacion = R2(dto.eur_fundacion);
                    entity.eur_universidad = R2(dto.eur_universidad);
                    entity.eur_tripartita = R2(dto.eur_tripartita);
                    entity.eur_iva = R2(dto.eur_iva);
                    entity.eur_irpf = R2(dto.eur_irpf);
                    entity.eur_total = R2(dto.eur_total);

                    entity.fecha_vencimiento = fv;
                    entity.fecha_cobro = fc;

                    entity.comentarios = string.IsNullOrWhiteSpace(dto.comentarios) ? null : dto.comentarios;
                    entity.atribucion = string.IsNullOrWhiteSpace(dto.atribucion) ? null : dto.atribucion;

                    if (!string.IsNullOrWhiteSpace(dto.fichero))
                        entity.fichero = dto.fichero;

                    db.SaveChanges();
                    return new { ok = true, message = "Factura actualizada correctamente." };
                }
            }
            catch (Exception ex)
            {
                return new { ok = false, message = "Error actualizando.", errors = new[] { ex.Message, ex.InnerException?.Message } };
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object RemoveFile(long id)
        {
            try
            {
                using (var db = new SpainBS_Connection())
                {
                    var entity = db.INF_FIN_FACTURAS.FirstOrDefault(x => x.idInfFinFacturas == id);
                    if (entity == null) return new { ok = false, message = "Factura no encontrada." };

                    var current = entity.fichero;
                    if (string.IsNullOrWhiteSpace(current))
                        return new { ok = true, message = "No había archivo adjunto." };

                    // delete physical file
                    DeletePhysicalFile(current);

                    entity.fichero = null;
                    db.SaveChanges();

                    return new { ok = true, message = "Adjunto eliminado." };
                }
            }
            catch (Exception ex)
            {
                return new { ok = false, message = "Error quitando adjunto.", errors = new[] { ex.Message, ex.InnerException?.Message } };
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
                    if (entity == null) return new { ok = false, message = "Factura no encontrada." };

                    // delete file if exists
                    if (!string.IsNullOrWhiteSpace(entity.fichero))
                        DeletePhysicalFile(entity.fichero);

                    db.INF_FIN_FACTURAS.Remove(entity);
                    db.SaveChanges();

                    return new { ok = true, message = "Factura eliminada." };
                }
            }
            catch (Exception ex)
            {
                return new { ok = false, message = "Error eliminando.", errors = new[] { ex.Message, ex.InnerException?.Message } };
            }
        }

        // ============================
        // Helpers
        // ============================

        private static bool TryParseIsoDate(string value, out DateTime dt)
        {
            dt = default(DateTime);
            if (string.IsNullOrWhiteSpace(value)) return false;

            return DateTime.TryParseExact(
                value.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dt
            );
        }

        private static DateTime? ParseIsoDateOrNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            DateTime dt;
            if (TryParseIsoDate(value, out dt)) return dt.Date;
            return null;
        }

        private static string GetRouteFacturasPhysical()
        {
            // Web.config: <add key="routeFacturas" value="~/documentos/facturas/" />
            var rel = ConfigurationManager.AppSettings["routeFacturas"];
            if (string.IsNullOrWhiteSpace(rel))
                throw new Exception("No existe AppSetting 'routeFacturas' en Web.config.");

            // MapPath necesita contexto
            return HttpContext.Current.Server.MapPath(rel);
        }

        private static void DeletePhysicalFile(string fileName)
        {
            try
            {
                var folder = GetRouteFacturasPhysical();
                if (!Directory.Exists(folder)) return;

                var fullPath = Path.Combine(folder, fileName);
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
            catch
            {
                // si quieres loggear, hazlo aquí
            }
        }
    }
}
