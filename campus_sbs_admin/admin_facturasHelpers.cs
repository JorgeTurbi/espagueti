using campus_sbs_admin.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Services;
using System.Web.Services;

namespace campus_sbs_admin
{
    internal static class admin_facturasHelpers
    {


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public async static Task<int> ObtenerSecuencia(string valorSociedad)
        {

            int secuencia = 0;

            if (string.IsNullOrEmpty(valorSociedad))
                return 0;

            using (var db = new SpainBS_Connection())
            {

                var listaSecuencia = await db.INF_FIN_FACTURAS.Where(a => a.sociedad.ToUpper() == valorSociedad.ToUpper()).ToListAsync();
                if (listaSecuencia.Count() == 0)
                    return 0;

                secuencia = listaSecuencia.Max(a => a.numero);
                if (secuencia == 0)
                    return 0;
                secuencia++;




            }

            return secuencia;


        }
    }
}