using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace campus_sbs_admin
{
    public class FilesStatus
    {
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string deleteUrl { get; set; }
        public string deleteType { get; set; }
        public string error { get; set; }

        public FilesStatus() { }
        public FilesStatus(FileInfo fileInfo) { SetValues(fileInfo.Name, (int)fileInfo.Length, fileInfo.FullName); }
        public FilesStatus(string fileName, int fileLength, string fullPath) { SetValues(fileName, fileLength, fullPath); }

        private void SetValues(string fileName, int fileLength, string fullPath)
        {
            string extension = Path.GetExtension(fullPath);
            //long idEmpresa = !String.IsNullOrEmpty(HttpContext.Current.Request.Params["idb"]) ? long.Parse(HttpContext.Current.Request.Params["idb"]) : -1;
            string type = !String.IsNullOrEmpty(HttpContext.Current.Request.Params["type"]) ? HttpContext.Current.Request.Params["type"] : string.Empty;
            
            /// Sacar los datos del fichero
            name = fileName;
            size = fileLength;
            if (type == "file_conv")
                url = ConfigurationManager.AppSettings["url_convenio"] + "temp/" + fileName;
            else if (type == "img_logo")
                url = ConfigurationManager.AppSettings["url_logo_empresa"] + "temp/" + fileName;
            else if (type == "file_anexo")
                url = ConfigurationManager.AppSettings["url_practicas"] + "temp/" + fileName;
            else if (type == "img_opinion")
                url = ConfigurationManager.AppSettings["url_foto_opinion"] + "temp/" + fileName;
            else if (type == "auto")
                url = ConfigurationManager.AppSettings["url_automatizacion"] + "temp/" + fileName;
            else if (type == "img_foto")
                url = ConfigurationManager.AppSettings["url_rec_directo"] + "temp/" + fileName;
            else if (type == "file_rec_int")
                url = ConfigurationManager.AppSettings["nota_tecnica"] + "temp/" + fileName;
            else if (type == "img_ficha")
                url = ConfigurationManager.AppSettings["urlUserPhoto"] + "temp/" + fileName;
            else if (type == "file_mail")
                url = ConfigurationManager.AppSettings["urlTemplateMailFree"] + "temp/" + fileName;
            else if (type == "file_doc")
                url = ConfigurationManager.AppSettings["url_documentos"] + "temp/" + fileName;
            else if (type == "img_banner")
                url = ConfigurationManager.AppSettings["url_banner"] + "temp/" + fileName;
            //deleteUrl = "/controls/UploadHandler.ashx?f=" + fileName;

            if (type == "auto")
                deleteUrl = "/controls/UploadHandler.ashx?f=" + fileName;
            else
            {
                deleteUrl = "../controls/UploadHandler.ashx?f=" + fileName;
                deleteType = "DELETE";
            }
            var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
            if (fileSize > 3 || !IsImage(extension))
                thumbnailUrl = string.Empty;
            else
                thumbnailUrl = @"data:image/png;base64," + EncodeFile(fullPath);
        }
        private bool IsImage(string ext)
        {
            return ext == ".gif" || ext == ".jpg" || ext == ".png";
        }
        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(File.ReadAllBytes(fileName));
        }
        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}