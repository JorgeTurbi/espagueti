using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace campus_sbs_admin
{
    /// <summary>
    /// Descripción breve de UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        private readonly JavaScriptSerializer js;
        private string StorageRoot
        {
            get
            {
                /// 1.- Sacar los parámetros
                //long idEmpresa = !String.IsNullOrEmpty(HttpContext.Current.Request.Params["idb"]) ? long.Parse(HttpContext.Current.Request.Params["idb"]) : -1;
                string type = !String.IsNullOrEmpty(HttpContext.Current.Request.Params["type"]) ? HttpContext.Current.Request.Params["type"] : string.Empty;

                if (type == "file_conv")
                {
                    string route = ConfigurationManager.AppSettings["ruta_convenio"] + "temp\\";
                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "img_logo")
                {
                    string route = ConfigurationManager.AppSettings["ruta_logo_empresa"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "file_anexo")
                {
                    string route = ConfigurationManager.AppSettings["ruta_practicas"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "img_opinion")
                {
                    string route = ConfigurationManager.AppSettings["ruta_foto_opinion"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "img_ficha")
                {
                    string route = ConfigurationManager.AppSettings["routeUserPhoto"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "auto")
                {
                    string route = ConfigurationManager.AppSettings["ruta_automatizacion"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "img_foto")
                {
                    string route = ConfigurationManager.AppSettings["route_rec_directo"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "file_rec_int")
                {
                    string route = ConfigurationManager.AppSettings["route_nota_tecnica"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "file_mail")
                {
                    string route = ConfigurationManager.AppSettings["routeTemplateMailFree"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "file_doc")
                {
                    string route = ConfigurationManager.AppSettings["ruta_documentos"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else if (type == "img_banner")
                {
                    string route = ConfigurationManager.AppSettings["ruta_banner"] + "temp\\";

                    if (!(Directory.Exists(route)))
                        Directory.CreateDirectory(route);

                    return Path.Combine(route);
                }
                else
                    return string.Empty;
            }
        }

        public UploadHandler()
        {
            js = new JavaScriptSerializer();
            js.MaxJsonLength = 41943040;
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    AnalizarAccion(context);
                    break;

                case "POST":
                case "PUT":
                    AnalizarAccion(context);
                    break;

                case "DELETE":
                    DeleteFile(context);
                    ListCurrentFiles(context);
                    break;

                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        /// <summary>
        ///  Subir fichero al servidor
        /// </summary>
        /// <param name="context"></param>
        private void AnalizarAccion(HttpContext context)
        {
            string accion = context.Request["accion"];
            if (accion == "update")
                UploadFile(context);
            else if (accion == "delete")
            {
                DeleteFile(context);
                ListCurrentFiles(context);
            }
        }

        /// <summary>
        ///  Subir fichero al servidor
        /// </summary>
        /// <param name="context"></param>
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            UploadWholeFile(context, statuses);
            WriteJsonIframeSafe(context, statuses);
        }

        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {           
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                var file = context.Request.Files[i];
                string sFileName = string.Empty;
                if (!String.IsNullOrEmpty(context.Request.Params["type"]) && (context.Request.Params["type"] == "img_opinion" || context.Request.Params["type"] == "auto" || context.Request.Params["type"] == "img_foto" || context.Request.Params["type"] == "file_rec_int" || context.Request.Params["type"] == "file_mail" || context.Request.Params["type"] == "file_doc" || context.Request.Params["type"] == "img_banner"))
                    sFileName = Utilities.RemoverSignosAcentos(Utils.cleanString(file.FileName)).Replace(",", string.Empty).Replace(" ", "-");
                else
                    sFileName = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + Utilities.RemoverSignosAcentos(Utils.cleanString(file.FileName)).Replace(",", string.Empty).Replace(" ", "-");
                var fullPath = StorageRoot + Path.GetFileName(sFileName);
                file.SaveAs(fullPath);

                string fullName = Path.GetFileName(sFileName);
                statuses.Add(new FilesStatus(fullName, file.ContentLength, fullPath));
            }
        }

        private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                    context.Response.ContentType = "application/json";
                else
                    context.Response.ContentType = "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            var jsonObj = "{\"files\":" + js.Serialize(statuses.ToArray()) + "}";
            context.Response.Write(jsonObj);
        }

        /// <summary>
        /// Eliminar fichero del servidor
        /// </summary>
        /// <param name="context"></param>
        private void DeleteFile(HttpContext context)
        {
            var filePath = StorageRoot + context.Request["f"];
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private void ListCurrentFiles(HttpContext context)
        {
            var files =
                new DirectoryInfo(StorageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(f))
                    .ToArray();

            string jsonObj = "{\"files\":" + js.Serialize(files) + "}";
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}