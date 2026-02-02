using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class remember_password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Cookie_Page.ComprobarCookie(this.Page, null, null, null, null);
        }

        protected void btn_forget_Click(object sender, EventArgs e)
        {
            DataAccess da = new DataAccess();
            bool _redirect = false;

            /// 1.- Recuperar el usuario
            string login_mail = login_user.Value.Trim();

            /// 2.- Pasar por el control de seguridad
            if (!Utils.esStringCorrecto(login_mail) || String.IsNullOrEmpty(login_mail))
            {
                LogUtils.InsertarLog(" ERROR - String Incorrecto en remember-password.aspx::btn_forget_Click()");
                LogUtils.InsertarLog("- STRING:" + login_mail);

                block_error.Attributes["class"] = block_error.Attributes["class"].Replace(" hidden", string.Empty);
                txt_error.InnerHtml = "<label>El mail o login introducido no es correcto.</label>";
            }
            else
            {
                List<CLIENTES> list_user = da.getUserForMailOrLogin(login_mail);
                if (list_user.Count == 1)
                {
                    if ((!String.IsNullOrEmpty(list_user[0].Profesor) && list_user[0].Profesor == ((int)Constantes.activo.Activo).ToString()) || (!String.IsNullOrEmpty(list_user[0].Administrador) && list_user[0].Administrador == ((int)Constantes.activo.Activo).ToString()) || (list_user[0].Comercial != null && list_user[0].Comercial.Value))
                    {

                        try
                        {
                            /// 1.- Cuerpo del mail
                            string template = Utilities.getPlantillaMail("remember-password", ConfigurationManager.AppSettings["urlTemplate"]);
                            if (!String.IsNullOrEmpty(template))
                            {
                                template = template.Replace("###Nombre###", list_user[0].Nombre_Completo);
                                template = template.Replace("###Usuario###", list_user[0].login);
                                template = template.Replace("###Clave###", Utils.toDecodeString(list_user[0].password));
                                template = template.Replace("###IDC###", ((long)Constantes.course.Sin_determinar).ToString());

                                template = template.Replace("###KEY###", list_user[0].Key);
                            }

                            /// 2.- Resto de datos necesarios para el mail
                            string asunto = Utilities.getAsuntoMail("remember-password", ConfigurationManager.AppSettings["urlAsunto"]);
                            int priority = 1;
                            string mailTo = list_user[0].email;
                            string nameTo = list_user[0].Nombre_Completo;

                            /// 3.- Añadir los datos de envío del mail
                            EMAIL_CONTENT email_data = new EMAIL_CONTENT();
                            email_data.nombreTo = nameTo;
                            email_data.mailTo = mailTo;
                            email_data.priority = priority;
                            email_data.asunto = asunto;
                            email_data.body = template;

                            long insert_mail = da.insertEmailContent(email_data);
                            if (insert_mail > 0)
                            {
                                /// Escribimos en el Log la hora a la cual se ha mandado el mensaje             
                                long idLog = da.insertLog(list_user[0].id_cliente, Utils.GetStringValue(Constantes.log.Mandar_mensaje));
                                if (idLog < 1)
                                {
                                    LogUtils.InsertarLog(" ERROR - remember-password.cs::btn_forget_Click()");
                                    LogUtils.InsertarLog("- MSG: Error al guardar la entrada en el log.");
                                }
                                _redirect = true;
                            }
                            else
                            {
                                LogUtils.InsertarLog(" ERROR - remember-password.cs::btn_forget_Click()");
                                LogUtils.InsertarLog("- MSG: Error al guardar el mail del usuario.");
                                LogUtils.InsertarLog("- Asunto:" + asunto);
                                LogUtils.InsertarLog("- Cuerpo:" + template);
                                LogUtils.InsertarLog("- Usuario:" + list_user[0].Nombre_Completo + " (" + list_user[0].id_cliente + ")");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogUtils.InsertarLog(" ERROR - remember-password.cs::btn_forget_Click()");
                            LogUtils.InsertarLog("- MSG:" + ex.Message);
                            LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
                        }
                    }
                    else
                    {
                        block_error.Attributes["class"] = block_error.Attributes["class"].Replace(" hidden", string.Empty);
                        txt_error.InnerHtml = "<label>El Usuario introducido no es un profesor.<br /> Por favor contacte con la escuela <a href='mailto:info@spainbs.com'>info@spainbs.com</a> o en el Telf. 91 719 10 00.</label>";
                    }
                }
                else if (list_user.Count > 1)
                {
                    block_error.Attributes["class"] = block_error.Attributes["class"].Replace(" hidden", string.Empty);
                    txt_error.InnerHtml = "<label>Hay mas de un usuario o email en nuestra base de datos.<br /> Por favor contacte con la escuela <a href='mailto:info@spainbs.com'>info@spainbs.com</a> o en el Telf. 91 719 10 00.</label>";
                }
                else
                {
                    block_error.Attributes["class"] = block_error.Attributes["class"].Replace(" hidden", string.Empty);
                    txt_error.InnerHtml = "<label>Su usuario o email no han sido encontrados.<br /> Por favor contacte con la escuela <a href='mailto:info@spainbs.com'>info@spainbs.com</a> o en el Telf. 91 719 10 00.</label>";
                }
            }
            if (_redirect)
                Response.Redirect("login.aspx");
        }
    }
}