using campus_sbs_admin.Models;
using campus_sbs_admin.Models.DTOS;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static campus_sbs_admin.admin_facturas_edit;

namespace campus_sbs_admin
{
    public partial class admin_facturas : System.Web.UI.Page
    {
        // ⚠️ ELIMINADO: private static readonly DateTime fe;  (esto causaba 0001-01-01)

        protected void Page_Load(object sender, EventArgs e)
        {
            femision.Value = DateTime.UtcNow.ToString("yyyy-MM-dd");
          
         
            if (!IsPostBack)
            {
                loadYears();
                loadPartners();
                loadAtribucciones();
            }

            
        }


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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/admin_facturas_list.aspx");
        }

        protected void btn_del_file_Click(object sender, ImageClickEventArgs e) { }

        protected void btnCancel_Click(object sender, EventArgs e) { }
      

        protected void img_filter_Click(object sender, ImageClickEventArgs e) { }

        // ======================================================
        //  Registrar Facturas
        // ======================================================
        #region Registro de Facturas

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object SaveFactura(FacturaDto dto)
        {
            try
            {
                var errors = new List<string>();

               


                if (dto == null)
                    return new { ok = false, message = "Errores de validación.", errors = new[] { "Datos vacíos." } };

                // --------------------------
                // Validaciones simples
                // --------------------------
                if (string.IsNullOrWhiteSpace(dto.sociedad)) errors.Add("Sociedad es obligatoria.");
                else if (dto.sociedad.Length > 5) errors.Add("Sociedad excede 5 caracteres.");

                if (dto.numero <= 0) errors.Add("Número de factura inválido.");

                if (string.IsNullOrWhiteSpace(dto.cliente_nombre)) errors.Add("Cliente es obligatorio.");
                else if (dto.cliente_nombre.Length > 250) errors.Add("Cliente excede 250 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.cliente_nif)) errors.Add("NIF es obligatorio.");
                else if (dto.cliente_nif.Length > 20) errors.Add("NIF excede 20 caracteres.");

                if (string.IsNullOrWhiteSpace(dto.descripcion)) errors.Add("Descripción es obligatoria.");
                else if (dto.descripcion.Length > 500) errors.Add("Descripción excede 500 caracteres.");

                if (!string.IsNullOrWhiteSpace(dto.comentarios) && dto.comentarios.Length > 1000)
                    errors.Add("Comentarios excede 1000 caracteres.");

                if (!string.IsNullOrWhiteSpace(dto.atribucion) && dto.atribucion.Length > 9)
                    errors.Add("Atribución excede 9 caracteres.");

                if (!string.IsNullOrWhiteSpace(dto.fichero) && dto.fichero.Length > 250)
                    errors.Add("Nombre del fichero excede 250 caracteres.");

                // --------------------------
                // Fechas: PARSE ESTRICTO ISO
                // --------------------------
                // SQL datetime => mínimo 1753-01-01
                DateTime minSqlDate = new DateTime(1753, 1, 1);

                DateTime fe;
                if (!TryParseIsoDate(dto.fecha_emision, out fe))
                    errors.Add("Fecha de emisión inválida (formato esperado yyyy-MM-dd).");
                else if (fe < minSqlDate)
                    errors.Add("Fecha de emisión fuera de rango para SQL datetime.");
                fe = fe.Date;

                DateTime? fvDb = ParseIsoDateOrNull(dto.fecha_vencimiento);
                DateTime? fcDb = ParseIsoDateOrNull(dto.fecha_cobro);

                if (fvDb.HasValue && fvDb.Value < minSqlDate) fvDb = null;
                if (fcDb.HasValue && fcDb.Value < minSqlDate) fcDb = null;

                if (fvDb.HasValue) fvDb = fvDb.Value.Date;
                if (fcDb.HasValue) fcDb = fcDb.Value.Date;

                // relación de fechas
                if (fvDb.HasValue && fvDb.Value < fe) errors.Add("Fecha vencimiento no puede ser menor que la fecha de emisión.");
                if (fcDb.HasValue && fcDb.Value < fe) errors.Add("Fecha cobro no puede ser menor que la fecha de emisión.");

                // --------------------------
                // Validaciones numéricas
                // --------------------------
                if (dto.eur_pvp < 0) errors.Add("PVP no puede ser negativo.");
                if (dto.eur_precio < 0) errors.Add("Precio no puede ser negativo.");
                if (dto.eur_tripartita < 0) errors.Add("Tripartita no puede ser negativo.");
                if (dto.eur_total < 0) errors.Add("Total no puede ser negativo.");

                // Redondeo a 2 decimales (tabla decimal(18,2))
                decimal R2(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);

                var eur_pvp = R2(dto.eur_pvp);
                var eur_beca = R2(dto.eur_beca);
                var eur_dto = R2(dto.eur_dto);
                var eur_precio = R2(dto.eur_precio);
                var eur_fund = R2(dto.eur_fundacion);
                var eur_uni = R2(dto.eur_universidad);
                var eur_trip = R2(dto.eur_tripartita);
                var eur_iva = R2(dto.eur_iva);
                var eur_irpf = R2(dto.eur_irpf);
                var eur_total = R2(dto.eur_total);

                if (errors.Count > 0)
                    return new { ok = false, message = "Errores de validación.", errors };

                // --------------------------
                // Guardado en BD
                // --------------------------
                using (var db = new SpainBS_Connection())
                {
                    var year = fe.Year;

                    var exists = db.INF_FIN_FACTURAS.Any(x =>
                        x.sociedad.ToUpper() == dto.sociedad.ToUpper() &&
                        x.numero == dto.numero &&
                        x.fecha_emision.Year == year);

                    if (exists)
                        return new
                        {
                            ok = false,
                            message = "Ya existe una factura con esa sociedad y número en ese año.",
                            errors = new[] { "Factura duplicada." }
                        };

                    var entity = new INF_FIN_FACTURAS
                    {
                        sociedad = dto.sociedad,
                        numero = dto.numero,
                        fecha_emision = fe,

                        cliente_nombre = dto.cliente_nombre,
                        cliente_nif = dto.cliente_nif,
                        descripcion = dto.descripcion,

                        eur_pvp = eur_pvp,
                        eur_beca = eur_beca,
                        eur_dto = eur_dto,
                        eur_precio = eur_precio,
                        eur_fundacion = eur_fund,
                        eur_universidad = eur_uni,
                        eur_tripartita = eur_trip,
                        eur_iva = eur_iva,
                        eur_irpf = eur_irpf,
                        eur_total = eur_total,

                        fecha_vencimiento = fvDb,
                        fecha_cobro = fcDb,

                        comentarios = string.IsNullOrWhiteSpace(dto.comentarios) ? null : dto.comentarios,
                        atribucion = string.IsNullOrWhiteSpace(dto.atribucion) ? null : dto.atribucion,
                        fichero = string.IsNullOrWhiteSpace(dto.fichero) ? null : dto.fichero
                    };

                    db.INF_FIN_FACTURAS.Add(entity);
                    db.SaveChanges();

                    return new { ok = true, message = "Factura registrada correctamente.", id = entity.idInfFinFacturas };
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                var msgs = new List<string>();
                Exception e = ex;
                while (e != null)
                {
                    msgs.Add(e.Message);
                    e = e.InnerException;
                }

                var sqlEx = ex.GetBaseException() as System.Data.SqlClient.SqlException;
                if (sqlEx != null)
                {
                    msgs.Add($"SQL Number: {sqlEx.Number}, State: {sqlEx.State}, Line: {sqlEx.LineNumber}");
                    msgs.Add($"SQL Server: {sqlEx.Server}");
                    msgs.Add($"SQL Procedure: {sqlEx.Procedure}");
                }

                return new { ok = false, message = "Error al guardar en base de datos.", errors = msgs };
            }
            catch (Exception ex)
            {
                return new { ok = false, message = "Error inesperado.", errors = new[] { ex.Message, ex.InnerException?.Message } };
            }
        }

        #endregion

        // ======================================================
        // Helpers de fecha ISO
        // ======================================================

        private static bool TryParseIsoDate(string value, out DateTime dt)
        {
            dt = default;

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
            if (TryParseIsoDate(value, out dt))
                return dt.Date;

            return null;
        }

        // ======================================================
        // WebMethods existentes
        // ======================================================

        [WebMethod]
        public static List<string> SearchClients(string term)
        {
            using (var db = new SpainBS_Connection())
            {
                return db.INF_FIN_FACTURAS
                    .Where(c => c.cliente_nombre.ToUpper().Contains(term.ToUpper()))
                    .OrderBy(c => c.cliente_nombre)
                    .Select(c => c.cliente_nombre)
                    .Distinct()
                    .Take(10)
                    .ToList();
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int ObtenerSecuencia(string valorSociedad, int year)
        {
            if (year == 0) return 0;

            using (var db = new SpainBS_Connection())
            {
                var lista = db.INF_FIN_FACTURAS
                    .Where(a => a.sociedad.ToUpper() == valorSociedad.ToUpper() && a.fecha_emision.Year == year)
                    .ToList();

                if (lista.Count == 0) return 1;

                return lista.Max(a => a.numero) + 1;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object LoadYearsRead(int year)
        {
            using (var db = new SpainBS_Connection())
            {
                var lista = db.INF_FIN_FACTURAS
                    .Where(x => x.fecha_emision != null && x.fecha_emision.Year == year)
                    .GroupBy(x => x.sociedad)
                    .Select(g => new FacturabySociedad
                    {
                        NombreSocio = g.Key,
                        Total = g.Max(a => a.numero)
                    })
                    .OrderByDescending(x => x.Total)
                    .ToList();

                return new { Objeto = lista };
            }
        }

        // ======================================================
        // Cargas dropdowns
        // ======================================================

        private void loadPartners()
        {
            string mensaje = "Selecciona  Sociedad";
            slSociedad.Items.Clear();

            List<string> partnerList;
            using (var db = new SpainBS_Connection())
            {
                partnerList = db.Socio.AsNoTracking().Select(a => a.Nombre).ToList();
            }

            slSociedad.Items.Add(new System.Web.UI.WebControls.ListItem(mensaje, ""));
            foreach (var y in partnerList)
                slSociedad.Items.Add(new System.Web.UI.WebControls.ListItem(y, y));

            slSociedad.Value = mensaje;
        }

        private void loadAtribucciones()
        {
            ddlAtribucion.Items.Clear();

            using (var db = new SpainBS_Connection())
            {
                var lst = db.Atribuciones.Where(a => a.Activo == true).ToList();

                ddlAtribucion.Items.Add(new System.Web.UI.WebControls.ListItem("Selecciona una atribución", string.Empty));

                foreach (var item in lst)
                {
                    // ✅ Texto = Nombre / Value = Código
                    ddlAtribucion.Items.Add(new System.Web.UI.WebControls.ListItem(item.Nombre, item.Codigo));
                }
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object UpdateFactura(FacturaUpdateDto dto)
        {
            try
            {
                if (dto == null)
                    return new { ok = false, message = "DTO inválido." };

                if (dto.idInfFinFacturas <= 0)
                    return new { ok = false, message = "ID inválido para actualizar." };

                DateTime ParseDateRequired(string iso, string fieldName)
                {
                    if (string.IsNullOrWhiteSpace(iso))
                        throw new Exception($"El campo {fieldName} es obligatorio.");

                    if (!DateTime.TryParseExact(iso, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                        throw new Exception($"Fecha inválida en {fieldName}.");

                    return dt;
                }

                DateTime? ParseDateOptional(string iso, string fieldName)
                {
                    if (string.IsNullOrWhiteSpace(iso)) return null;

                    if (!DateTime.TryParseExact(iso, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                        throw new Exception($"Fecha inválida en {fieldName}.");

                    return dt;
                }

                using (var db = new SpainBS_Connection())
                {
                    var e = db.INF_FIN_FACTURAS.FirstOrDefault(x => x.idInfFinFacturas == dto.idInfFinFacturas);
                    if (e == null)
                        return new { ok = false, message = "Factura no encontrada." };

                    // ===== Campos básicos =====
                    e.sociedad = dto.sociedad;
                    e.numero = dto.numero;

                    e.fecha_emision = ParseDateRequired(dto.fecha_emision, "fecha_emision");
                    e.fecha_vencimiento = ParseDateOptional(dto.fecha_vencimiento, "fecha_vencimiento");
                    e.fecha_cobro = ParseDateOptional(dto.fecha_cobro, "fecha_cobro");

                    e.cliente_nombre = dto.cliente_nombre;
                    e.cliente_nif = dto.cliente_nif;

                    e.descripcion = dto.descripcion;
                    e.comentarios = dto.comentarios;
                    e.atribucion = dto.atribucion;

                    // Importante: NO borrar fichero si viene vacío/null
                    if (!string.IsNullOrWhiteSpace(dto.fichero))
                        e.fichero = dto.fichero;

                    // ===== Importes =====
                    e.eur_pvp = dto.eur_pvp;
                    e.eur_beca = dto.eur_beca;
                    e.eur_dto = dto.eur_dto;
                    e.eur_precio = dto.eur_precio;
                    e.eur_fundacion = dto.eur_fundacion;
                    e.eur_universidad = dto.eur_universidad;
                    e.eur_tripartita = dto.eur_tripartita;
                    e.eur_iva = dto.eur_iva;
                    e.eur_irpf = dto.eur_irpf;
                    e.eur_total = dto.eur_total;

                    db.SaveChanges();

                    return new { ok = true, message = "Factura actualizada correctamente." };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    ok = false,
                    message = "Error actualizando factura.",
                    errors = new[] { ex.Message, ex.InnerException?.Message }
                };
            }
        }
        private void loadYears()
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

            ddlyear.Items.Add(new System.Web.UI.WebControls.ListItem("Selecciona el año", ""));

            int currentYear = DateTime.Today.Year;
            yearList.Add(currentYear);
            yearList.Add(currentYear + 1);

            yearList = yearList.Distinct().OrderByDescending(y => y).ToList();

            foreach (var y in yearList)
                ddlyear.Items.Add(new System.Web.UI.WebControls.ListItem(y.ToString(), y.ToString()));

            ddlyear.Value = currentYear.ToString();
        }
    }
}

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

public class FacturaUpdateDto
{
    public long idInfFinFacturas { get; set; } 

    public string sociedad { get; set; }
    public int numero { get; set; }

    public string fecha_emision { get; set; }
    public string fecha_vencimiento { get; set; }
    public string fecha_cobro { get; set; }

    public string cliente_nombre { get; set; }
    public string cliente_nif { get; set; }

    public string descripcion { get; set; }
    public string comentarios { get; set; }
    public string atribucion { get; set; }
    public string fichero { get; set; }

    public decimal eur_pvp { get; set; }
    public decimal eur_beca { get; set; }
    public decimal eur_dto { get; set; }
    public decimal eur_precio { get; set; }
    public decimal eur_fundacion { get; set; }
    public decimal eur_universidad { get; set; }
    public decimal eur_tripartita { get; set; }
    public decimal eur_iva { get; set; }
    public decimal eur_irpf { get; set; }
    public decimal eur_total { get; set; }
}
