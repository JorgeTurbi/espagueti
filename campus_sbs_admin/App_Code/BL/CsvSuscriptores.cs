using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace campus_sbs_admin
{
    public class CsvSuscriptores
    {
        [CsvColumn(FieldIndex = 1)]
        public long Id_Persona { get; set; }

        [CsvColumn(FieldIndex = 2)]
        public DateTime? Fecha_Inicio { get; set; }

        [CsvColumn(FieldIndex = 3)]
        public int Dias { get; set; }

        [CsvColumn(FieldIndex = 4)]
        public decimal? Importe { get; set; }

        [CsvColumn(FieldIndex = 5)]
        public string Comentarios { get; set; }
    }
}