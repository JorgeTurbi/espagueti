using sbs_DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Campaign_plan_process
{
    public class Utilities
    {
        #region Plantillas Mail

        public static string getPlantillaMail(string page, string route)
        {
            string template = string.Empty;
            try
            {
                /// Obtenemos la ruta donde se encuentran los ficheros
                string rutaBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()).Remove(0, 6) + "\\" + route;
                StreamReader sr = new StreamReader(rutaBase + "/" + page + ".html", System.Text.Encoding.Default);
                template = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Utilities.cs::getPlantillaMail()", true);
                LogUtils.InsertarLog("- MSG:" + ex.Message, true);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message), true);
                template = string.Empty;
            }

            return template;
        }

        #endregion
    }
}
