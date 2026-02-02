using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin.controls
{
    public partial class header_crm : System.Web.UI.UserControl
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Sacar los datos del usuario
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (Session["usuario"] != null)
                list_user.Add((CLIENTES)Session["usuario"]);

            if (list_user.Count > 0)
            {
                if (list_user[0].fecha_baja == null || list_user[0].fecha_baja == new DateTime())
                {
                    if ((!String.IsNullOrEmpty(list_user[0].Administrador) && list_user[0].Administrador == ((int)Constantes.activo.Activo).ToString()) || (list_user[0].Comercial != null && list_user[0].Comercial.Value))
                    {
                        /// 1.- Pintar el nombre del usuario
                        teacher_perfil.InnerHtml = "<a href='https://www.spainbs.com/miperfil.aspx?idcl=" + list_user[0].Key + "' target='_blank'><span class='text-white bold'>" + list_user[0].Nombre_Completo + "</span></a>";

                        /// 2.- Pintar la imagen del usuario
                        teacher_user.InnerHtml = "<a href='https://www.spainbs.com/miperfil.aspx?idcl=" + list_user[0].Key + "' target='_blank'><img id='img_user' class='avatar' alt='" + list_user[0].Nombre_Completo + "' src='" + ConfigurationManager.AppSettings["urlUserPhoto"] + (!String.IsNullOrEmpty(list_user[0].Foto) ? list_user[0].Foto : "sin_foto.png") + "' title='" + list_user[0].Nombre_Completo + "' /></a>";
                    }
                    else
                    {
                        List<string> list_users_mails = ConfigurationManager.AppSettings["users_special_mail"].Split(',').ToList();
                        if (list_users_mails.Contains(list_user[0].id_cliente.ToString()))
                        {
                            /// 1.- Pintar el nombre del usuario
                            teacher_perfil.InnerHtml = "<a href='https://www.spainbs.com/miperfil.aspx?idcl=" + list_user[0].Key + "' target='_blank'><span class='text-white bold'>" + list_user[0].Nombre_Completo + "</span></a>";

                            /// 2.- Pintar la imagen del usuario
                            teacher_user.InnerHtml = "<a href='https://www.spainbs.com/miperfil.aspx?idcl=" + list_user[0].Key + "' target='_blank'><img id='img_user' class='avatar' alt='" + list_user[0].Nombre_Completo + "' src='" + ConfigurationManager.AppSettings["urlUserPhoto"] + (!String.IsNullOrEmpty(list_user[0].Foto) ? list_user[0].Foto : "sin_foto.png") + "' title='" + list_user[0].Nombre_Completo + "' /></a>";
                        }
                        else
                        {
                            Session.Remove("usuario");
                            Response.Redirect("login.aspx");
                        }
                    }
                }
                else
                {
                    Session.Remove("usuario");
                    Response.Redirect("login.aspx");
                }
            }
            else
                Response.Redirect("login.aspx");
        }
    }
}