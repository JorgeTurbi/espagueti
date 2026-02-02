using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace campus_sbs_admin
{
    public class CsvUsuarios
    {
        [CsvColumn(FieldIndex = 1)]
        public long? ID { get; set; }

        [CsvColumn(FieldIndex = 2)]
        public string Nombre { get; set; }

        [CsvColumn(FieldIndex = 3)]
        public string Apellidos { get; set; }

        [CsvColumn(FieldIndex = 4)]
        public string Telefono { get; set; }

        [CsvColumn(FieldIndex = 5)]
        public string Mail { get; set; }

        [CsvColumn(FieldIndex = 6)]
        public string Pais { get; set; }

        [CsvColumn(FieldIndex = 7)]
        public string Provincia { get; set; }

        [CsvColumn(FieldIndex = 8)]
        public string Ciudad { get; set; }

        [CsvColumn(FieldIndex = 9)]
        public long? Origen { get; set; }

        [CsvColumn(FieldIndex = 10)]
        public long? Curso { get; set; }

        [CsvColumn(FieldIndex = 11)]
        public long? Docencia { get; set; }

        [CsvColumn(FieldIndex = 12)]
        public long? Campaign { get; set; }

        [CsvColumn(FieldIndex = 13)]
        public long? idLanding { get; set; }

        [CsvColumn(FieldIndex = 14)]
        public DateTime? Fecha { get; set; }

        [CsvColumn(FieldIndex = 15)]
        public string utm_source { get; set; }

        [CsvColumn(FieldIndex = 16)]
        public string utm_medium { get; set; }

        [CsvColumn(FieldIndex = 17)]
        public string utm_campaign { get; set; }

        [CsvColumn(FieldIndex = 18)]
        public string utm_content { get; set; }

        [CsvColumn(FieldIndex = 19)]
        public string utm_term { get; set; }

        [CsvColumn(FieldIndex = 20)]
        public string Tags { get; set; }

        [CsvColumn(FieldIndex = 21)]
        public int? procesado { get; set; }
    }
}