<%@ Page Language="C#" Strict="true"%>
<script runat="server">
    protected void  Page_Load(object sender, EventArgs e)
    {
        Response.Write(processUpload());
    }
    //Ejemplo sacado de la siguiente página
    //https://www.pluginsforckeditor.com/Tutoriales/86/Subir-ficheros-con-CKEditor
    // Upload script for CKEditor.
    // Use at your own risk, no warranty provided. Be careful about who is able to access this file
    // The upload folder shouldn't be able to upload any kind of script, just in case.
    // If you're not sure, hire a professional that takes care of adjusting the server configuration as well as this script for you.
    // (I am not such professional)

    private String processUpload()
    {
        // Step 1: change the true for whatever condition you use in your environment to verify that the user
        // is logged in and is allowed to use the script
        //if (true)
        //    return sendError("You're not allowed to upload files");

        // Step 2: Put here the full absolute path of the folder where you want to save the files:
        // You must set the proper permissions on that folder
        String basePath = ConfigurationManager.AppSettings["basePath"];

        // Step 3: Put here the Url that should be used for the upload folder (it the URL to access the folder that you have set in $basePath
        // you can use a relative url "/images/", or a path including the host "http://example.com/images/"
        // ALWAYS put the final slash (/)
        String baseUrl = ConfigurationManager.AppSettings["baseUrl"];

        // Done. Now test it!

        // No need to modify anything below this line
        //----------------------------------------------------

        // ------------------------
        // Input parameters: optional means that you can ignore it, and required means that you
        // must use it to provide the data back to CKEditor.
        // ------------------------

        // Optional: instance name (might be used to adjust the server folders for example)
        String CKEditor = HttpContext.Current.Request["CKEditor"] ;

        // Required: Function number as indicated by CKEditor.
        String funcNum = HttpContext.Current.Request["CKEditorFuncNum"] ;

        // Optional: To provide localized messages
        String langCode = HttpContext.Current.Request["langCode"] ;

        // ------------------------
        // Data processing
        // ------------------------

        int total;
        try
        {
            total = HttpContext.Current.Request.Files.Count;
        }
        catch (Exception)
        {
            return  sendError("Error al subir el fichero.");
        }
        if (total==0)
            return sendError("No se ha enviado el archivo.");

        if (!System.IO.Directory.Exists(basePath))
            return sendError("El directorio basePath no existe.");

        //Grab the file name from its fully qualified path at client
        HttpPostedFile theFile = HttpContext.Current.Request.Files[0];

        String strFileName = theFile.FileName;
        if (strFileName=="")
            return sendError("El nombre del archivo está vacío.");

        String sFileName = System.IO.Path.GetFileName(strFileName);
        sFileName = DateTime.Today.Year + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + "_" + RemoverSignosAcentos(sFileName).Trim().Replace(" ", "_");

        String name = System.IO.Path.Combine(basePath, sFileName);
        theFile.SaveAs(name);

        String url = baseUrl + sFileName.Replace("'", "\'");

        // ------------------------
        // Write output
        // ------------------------
        return "<scr" + "ipt type='text/javascript'> window.parent.CKEDITOR.tools.callFunction(" + funcNum + ", '" + url + "', '')</scr" + "ipt>";
    }

    private String sendError(String msg)
    {
        String funcNum = HttpContext.Current.Request["CKEditorFuncNum"] ;
        return "<scr" + "ipt type='text/javascript'> window.parent.CKEDITOR.tools.callFunction(" + funcNum + ", '', '" + msg + "')</scr" + "ipt>";
    }

    private const string ConSignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇ";
    private const string SinSignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUcC";
    private string RemoverSignosAcentos(string texto)
    {
        var textoSinAcentos = string.Empty;
        foreach (var caracter in texto)
        {
            var indexConAcento = ConSignos.IndexOf(caracter);
            if (indexConAcento > -1)
                textoSinAcentos = textoSinAcentos + (SinSignos.Substring(indexConAcento, 1));
            else
                textoSinAcentos = textoSinAcentos + (caracter);
        }
        return textoSinAcentos;
    }
</script>