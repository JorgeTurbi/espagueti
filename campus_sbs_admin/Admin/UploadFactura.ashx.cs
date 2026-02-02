
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace campus_sbs_admin.Admin
{
    /// <summary>
    /// Summary description for UploadFactura
    /// </summary>
    public class UploadFactura : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                var file = context.Request.Files["file"]; // debe llamarse "file" en FormData
                if (file == null || file.ContentLength == 0)
                    throw new Exception("No se recibió ningún archivo.");

                // 1) Ruta VIRTUAL desde Web.config (ej: ~/documentos/facturas/)
                var rutaVirtual = ConfigurationManager.AppSettings["routeFacturas"];
                if (string.IsNullOrWhiteSpace(rutaVirtual))
                    throw new Exception("No existe la key routeFacturas en web.config.");

                // 2) Convertir a RUTA FÍSICA (rooted)
                var rootFisico = context.Server.MapPath(rutaVirtual);

                if (!Directory.Exists(rootFisico))
                    Directory.CreateDirectory(rootFisico);

                // 3) Nombre seguro
                var original = Path.GetFileName(file.FileName);
                var safeName = MakeSafeFileName(original);

                // 4) Guardar
                var fullPath = Path.Combine(rootFisico, safeName);
                file.SaveAs(fullPath);

                // 5) Respuesta (devuelve nombre para guardarlo en BD)
                context.Response.Write("{\"ok\":true,\"fileName\":\"" + safeName.Replace("\"", "\\\"") + "\"}");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("{\"ok\":false,\"message\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
            }
        }

        public bool IsReusable => false;

        private static string MakeSafeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}