using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace campus_sbs_admin
{
    public class CsvBajaUsuarios
    {
        [CsvColumn(FieldIndex = 1)]
        public string Email { get; set; }
    }
}