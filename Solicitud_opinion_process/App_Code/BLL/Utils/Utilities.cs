using sbs_DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solicitud_opinion_process
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

        public static bool comprobar_alumno(List<campus_DOCENCIA_GRUPO> list_docencias_grupo, campus_DOCENCIA docencia, long id_docencia, long id_cliente)
        {
            bool student_active = true;
            List<campus_DOCENCIA_GRUPO> lst_doc = list_docencias_grupo.Where(dg => dg.ID_Docencia == id_docencia && dg.ID_Persona == id_cliente).ToList();
            if (lst_doc.Count == 1)
            {
                if (lst_doc[0].FFinAcceso != null && DateTime.Today <= lst_doc[0].FFinAcceso.Value)
                    student_active = true;
                else if (lst_doc[0].FFinAcceso != null && DateTime.Today > lst_doc[0].FFinAcceso.Value)
                    student_active = false;
                else if (docencia.FFinAcceso != null && DateTime.Today > docencia.FFinAcceso.Value)
                    student_active = false;
            }
            else
                student_active = false;

            return student_active;
        }
    }
}
