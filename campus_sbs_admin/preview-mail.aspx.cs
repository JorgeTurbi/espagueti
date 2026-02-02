using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class preview_mail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(Description = "Busca nombres de listas")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> getData()
        {
            List<string> list = new List<string>();
            list.Add(Utilities.getPlantillaMail("email_campaign", ConfigurationManager.AppSettings["urlTemplate"]));
            return list;
        }
    }
}