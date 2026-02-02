using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace campus_sbs_admin.Models.DTOS
{
    public class FacturaDto
    {
        public string sociedad { get; set; }
        public int numero { get; set; }
        public string fecha_emision { get; set; } // yyyy-MM-dd desde el input date
        public string cliente_nombre { get; set; }
        public string cliente_nif { get; set; }
        public string descripcion { get; set; }

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

        public string fecha_vencimiento { get; set; } // yyyy-MM-dd o null
        public string fecha_cobro { get; set; }       // yyyy-MM-dd o null

        public string comentarios { get; set; }
        public string atribucion { get; set; }
        public string fichero { get; set; } // opcional
    }

}