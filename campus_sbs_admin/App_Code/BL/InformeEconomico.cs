using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace campus_sbs_admin
{
    public class InformeEconomico
    {
        public long IdDocencia { get; set; }
        public string NameDocencia { get; set; }
        public long IdCurso { get; set; }
        public string NameCurso { get; set; }
        public long IdPrograma { get; set; }
        public long IdTutor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double PorcentajeTutor { get; set; }
        public decimal Venta { get; set; }

        public decimal TipoCurso;
    }
}