using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class regla_accion : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Comprobar si viene el parámetro 'k' en la url
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                {
                    Session.Add("usuario", list_user[0]);
                }
            }

            if (list_user.Count == 0)
            {
                if (list_user.Count == 0 && Session["usuario"] != null)
                    list_user.Add((CLIENTES)Session["usuario"]);
                else
                    Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                /// Comprobar al usuario
                bool comprobate_user = Utilities.comprobate_user(list_user[0]);
                if (!comprobate_user)
                    Response.Redirect("login.aspx");
                else
                {
                    /// Sacar los datos de la url
                    long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
                    long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;

                    if (id_rule > 0 && id_type_action > 0)
                        load_action(id_rule, id_type_action);
                    else
                        Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                }
            }
        }

        #region Características Comunes

        protected void btnComun_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;
            if (id_rule > 0 && id_type_action > 0)
            {
                /// 2.- Sacar los datos del formulario
                string tags = txt_tags.Value;
                int? score = null;
                if (!String.IsNullOrEmpty(txt_score.Value))
                    score = int.Parse(txt_score.Value);
                                
                /// 3.- Actualizar la regla
                List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                if (lst_actions.Count == 1)
                {
                    campus_REGLAS_ACCIONES action_rule = lst_actions[0];
                    action_rule.tags = tags;
                    action_rule.score = score;

                    bool update_rule = da.updateRuleAction(action_rule);
                    if (update_rule)
                        Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
                else
                {
                    campus_REGLAS_ACCIONES action_rule = new campus_REGLAS_ACCIONES();
                    action_rule.idRegla = id_rule;
                    action_rule.idTipo = id_type_action;
                    action_rule.tags = tags;
                    action_rule.score = score;

                    long insert_rule = da.insertRuleAction(action_rule);
                    if (insert_rule > 0)
                        Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir la regla";
                }
            }
        }

        #endregion
        
        #region Reasignar comercial

        protected void btnReasignarComercial_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;
            if (id_rule > 0 && id_type_action > 0)
            {
                /// 2.- Sacar los datos del formulario
                long idComercial = long.Parse(ddlComercial.Value);
                string name_from = txt_nombre_from.Value;
                string mail_from = txt_mail_from.Value;
                string reply_to = txt_reply_to.Value;
                string mail_asunto = txt_asunto.Value;
                string mail_body = txt_cuerpo.Value;
                string mail_cco = txt_cco.Value;
                string adjuntos = string.Empty;                
                bool validate_files = true;

                #region Adjuntos

                string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                if (!(Directory.Exists(route)))
                    Directory.CreateDirectory(route);

                string adjunto1 = string.Empty;
                string adjunto2 = string.Empty;
                string adjunto3 = string.Empty;
                string adjunto4 = string.Empty;
                string adjunto5 = string.Empty;

                /// Adjunto 1
                if (fuAdjunto1.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto1.PostedFile.FileName);
                        adjunto1 = fuAdjunto1.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto1.PostedFile.SaveAs(route + adjunto1);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto1 = lnkAdjunto1.InnerText;

                /// Adjunto 2
                if (fuAdjunto2.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto2.PostedFile.FileName);
                        adjunto2 = fuAdjunto2.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto2.PostedFile.SaveAs(route + adjunto2);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto2 = lnkAdjunto2.InnerText;

                /// Adjunto 3
                if (fuAdjunto3.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto3.PostedFile.FileName);
                        adjunto3 = fuAdjunto3.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto3.PostedFile.SaveAs(route + adjunto3);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto3 = lnkAdjunto3.InnerText;

                /// Adjunto 4
                if (fuAdjunto4.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto4.PostedFile.FileName);
                        adjunto4 = fuAdjunto4.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto4.PostedFile.SaveAs(route + adjunto4);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto4 = lnkAdjunto4.InnerText;

                /// Adjunto 5
                if (fuAdjunto5.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto5.PostedFile.FileName);
                        adjunto5 = fuAdjunto5.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto5.PostedFile.SaveAs(route + adjunto5);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto5 = lnkAdjunto5.InnerText;

                List<string> list_adjuntos = new List<string>();
                if (!String.IsNullOrEmpty(adjunto1))
                    list_adjuntos.Add(adjunto1);
                if (!String.IsNullOrEmpty(adjunto2))
                    list_adjuntos.Add(adjunto2);
                if (!String.IsNullOrEmpty(adjunto3))
                    list_adjuntos.Add(adjunto3);
                if (!String.IsNullOrEmpty(adjunto4))
                    list_adjuntos.Add(adjunto4);
                if (!String.IsNullOrEmpty(adjunto5))
                    list_adjuntos.Add(adjunto5);

                int cont = 0;
                if (list_adjuntos.Count > 0)
                {
                    foreach (string _adjunto in list_adjuntos)
                    {
                        if (cont == 0)
                            adjuntos = route + _adjunto;
                        else
                            adjuntos += "," + route + _adjunto;
                        cont++;
                    }
                }

                #endregion

                if (validate_files)
                {
                    /// 3.- Actualizar la regla
                    List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                    if (lst_actions.Count == 1)
                    {
                        campus_REGLAS_ACCIONES action_rule = lst_actions[0];
                        action_rule.idComercial = idComercial;
                        if (!String.IsNullOrEmpty(name_from))
                            action_rule.nombreFrom = name_from;
                        if (!String.IsNullOrEmpty(mail_from))
                            action_rule.mailFrom = mail_from;
                        if (!String.IsNullOrEmpty(reply_to))
                            action_rule.replyTo = reply_to;
                        if (!String.IsNullOrEmpty(mail_cco))
                            action_rule.cco = mail_cco;
                        if (!String.IsNullOrEmpty(mail_asunto))
                            action_rule.asunto = mail_asunto;
                        if (!String.IsNullOrEmpty(mail_body))
                            action_rule.body = mail_body;
                        if (!String.IsNullOrEmpty(adjuntos))
                            action_rule.adjuntos = adjuntos;

                        bool update_rule = da.updateRuleAction(action_rule);
                        if (update_rule)
                            Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                    }
                    else
                    {
                        campus_REGLAS_ACCIONES action_rule = new campus_REGLAS_ACCIONES();
                        action_rule.idRegla = id_rule;
                        action_rule.idTipo = id_type_action;
                        action_rule.idComercial = idComercial;
                        if (!String.IsNullOrEmpty(name_from))
                            action_rule.nombreFrom = name_from;
                        if (!String.IsNullOrEmpty(mail_from))
                            action_rule.mailFrom = mail_from;
                        if (!String.IsNullOrEmpty(reply_to))
                            action_rule.replyTo = reply_to;
                        if (!String.IsNullOrEmpty(mail_cco))
                            action_rule.cco = mail_cco;
                        if (!String.IsNullOrEmpty(mail_asunto))
                            action_rule.asunto = mail_asunto;
                        if (!String.IsNullOrEmpty(mail_body))
                            action_rule.body = mail_body;
                        if (!String.IsNullOrEmpty(adjuntos))
                            action_rule.adjuntos = adjuntos;

                        long insert_rule = da.insertRuleAction(action_rule);
                        if (insert_rule > 0)
                            Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al añadir la regla";
                    }
                }
            }
        }

        #endregion

        #region Mail

        protected void btnGuardarMail_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;
            if (id_rule > 0 && id_type_action > 0)
            {
                /// 2.- Sacar el resto de parámetros            
                string name_from = txt_nombre_from.Value;
                string mail_from = txt_mail_from.Value;
                string reply_to = txt_reply_to.Value;
                string mail_asunto = txt_asunto.Value;
                string mail_body = txt_cuerpo.Value;
                string mail_cco = txt_cco.Value;
                string adjuntos = string.Empty;
                bool validate_files = true;

                #region Adjuntos

                string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                if (!(Directory.Exists(route)))
                    Directory.CreateDirectory(route);

                string adjunto1 = string.Empty;
                string adjunto2 = string.Empty;
                string adjunto3 = string.Empty;
                string adjunto4 = string.Empty;
                string adjunto5 = string.Empty;

                /// Adjunto 1
                if (fuAdjunto1.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto1.PostedFile.FileName);
                        adjunto1 = fuAdjunto1.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto1.PostedFile.SaveAs(route + adjunto1);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 1');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto1 = lnkAdjunto1.InnerText;

                /// Adjunto 2
                if (fuAdjunto2.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto2.PostedFile.FileName);
                        adjunto2 = fuAdjunto2.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto2.PostedFile.SaveAs(route + adjunto2);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 2');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto2 = lnkAdjunto2.InnerText;

                /// Adjunto 3
                if (fuAdjunto3.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto3.PostedFile.FileName);
                        adjunto3 = fuAdjunto3.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto3.PostedFile.SaveAs(route + adjunto3);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 3');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto3 = lnkAdjunto3.InnerText;

                /// Adjunto 4
                if (fuAdjunto4.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto4.PostedFile.FileName);
                        adjunto4 = fuAdjunto4.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto4.PostedFile.SaveAs(route + adjunto4);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 4');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto4 = lnkAdjunto4.InnerText;

                /// Adjunto 5
                if (fuAdjunto5.HasFile)
                {
                    if (Directory.Exists(route))
                    {
                        FileInfo archivo = new FileInfo(fuAdjunto5.PostedFile.FileName);
                        adjunto5 = fuAdjunto5.PostedFile.FileName.Replace(" ", "-");
                        try
                        {
                            fuAdjunto5.PostedFile.SaveAs(route + adjunto5);
                        }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al guardar el adjunto 5');</script>");
                            validate_files = false;
                        }
                    }
                }
                else
                    adjunto5 = lnkAdjunto5.InnerText;

                List<string> list_adjuntos = new List<string>();
                if (!String.IsNullOrEmpty(adjunto1))
                    list_adjuntos.Add(adjunto1);
                if (!String.IsNullOrEmpty(adjunto2))
                    list_adjuntos.Add(adjunto2);
                if (!String.IsNullOrEmpty(adjunto3))
                    list_adjuntos.Add(adjunto3);
                if (!String.IsNullOrEmpty(adjunto4))
                    list_adjuntos.Add(adjunto4);
                if (!String.IsNullOrEmpty(adjunto5))
                    list_adjuntos.Add(adjunto5);

                int cont = 0;
                if (list_adjuntos.Count > 0)
                {
                    foreach (string _adjunto in list_adjuntos)
                    {
                        if (cont == 0)
                            adjuntos = route + _adjunto;
                        else
                            adjuntos += "," + route + _adjunto;
                        cont++;
                    }
                }

                #endregion

                if (validate_files)
                {
                    /// 3.- Actualizar la regla
                    List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                    if (lst_actions.Count == 1)
                    {
                        campus_REGLAS_ACCIONES action_rule = lst_actions[0];
                        action_rule.nombreFrom = name_from;
                        action_rule.mailFrom = mail_from;
                        action_rule.replyTo = reply_to;
                        action_rule.cco = mail_cco;
                        action_rule.asunto = mail_asunto;
                        action_rule.body = mail_body;
                        action_rule.adjuntos = adjuntos;

                        bool update_rule = da.updateRuleAction(action_rule);
                        if (update_rule)
                            Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                    }
                    else
                    {
                        campus_REGLAS_ACCIONES action_rule = new campus_REGLAS_ACCIONES();
                        action_rule.idRegla = id_rule;
                        action_rule.idTipo = id_type_action;
                        action_rule.nombreFrom = name_from;
                        action_rule.mailFrom = mail_from;
                        action_rule.replyTo = reply_to;
                        action_rule.cco = mail_cco;
                        action_rule.asunto = mail_asunto;
                        action_rule.body = mail_body;
                        action_rule.adjuntos = adjuntos;

                        long insert_rule = da.insertRuleAction(action_rule);
                        if (insert_rule > 0)
                            Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al añadir la regla";
                    }
                }
            }
        }

        protected void btn_del_Adjunto1_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_1 = false;
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;

            try
            {
                if (id_rule > 0 && id_type_action > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                    if (lst_actions.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto1.InnerText))
                        {
                            string file_delete = route + lnkAdjunto1.InnerText;
                            File.SetAttributes(file_delete, FileAttributes.Normal);
                            File.Delete(file_delete);
                            lnkAdjunto1.InnerText = string.Empty;
                            fuAdjunto1.Visible = true;
                            blk_del_1.Attributes["class"] = blk_del_1.Attributes["class"].Insert(blk_del_1.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst_actions[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_1 = da.updateRuleAction(lst_actions[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - regla-accion.cs::btn_del_Adjunto1_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_1)
                Response.Redirect("regla-accion.aspx?idr=" + id_rule + "&idta=" + id_type_action);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto2_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_2 = false;
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;

            try
            {
                if (id_rule > 0 && id_type_action > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                    if (lst_actions.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto2.InnerText))
                        {
                            string file_delete = route + lnkAdjunto2.InnerText;
                            File.SetAttributes(file_delete, FileAttributes.Normal);
                            File.Delete(file_delete);
                            lnkAdjunto2.InnerText = string.Empty;
                            fuAdjunto2.Visible = true;
                            blk_del_2.Attributes["class"] = blk_del_2.Attributes["class"].Insert(blk_del_2.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst_actions[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_2 = da.updateRuleAction(lst_actions[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - regla-accion.cs::btn_del_Adjunto2_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_2)
                Response.Redirect("regla-accion.aspx?idr=" + id_rule + "&idta=" + id_type_action);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto3_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_3 = false;
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;

            try
            {
                if (id_rule > 0 && id_type_action > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                    if (lst_actions.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto3.InnerText))
                        {
                            string file_delete = route + lnkAdjunto3.InnerText;
                            File.SetAttributes(file_delete, FileAttributes.Normal);
                            File.Delete(file_delete);
                            lnkAdjunto3.InnerText = string.Empty;
                            fuAdjunto3.Visible = true;
                            blk_del_3.Attributes["class"] = blk_del_3.Attributes["class"].Insert(blk_del_3.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst_actions[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_3 = da.updateRuleAction(lst_actions[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - regla-accion.cs::btn_del_Adjunto3_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_3)
                Response.Redirect("regla-accion.aspx?idr=" + id_rule + "&idta=" + id_type_action);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto4_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_4 = false;
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;

            try
            {
                if (id_rule > 0 && id_type_action > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                    if (lst_actions.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto4.InnerText))
                        {
                            string file_delete = route + lnkAdjunto4.InnerText;
                            File.SetAttributes(file_delete, FileAttributes.Normal);
                            File.Delete(file_delete);
                            lnkAdjunto4.InnerText = string.Empty;
                            fuAdjunto4.Visible = true;
                            blk_del_4.Attributes["class"] = blk_del_4.Attributes["class"].Insert(blk_del_4.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst_actions[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_4 = da.updateRuleAction(lst_actions[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - regla-accion.cs::btn_del_Adjunto4_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_4)
                Response.Redirect("regla-accion.aspx?idr=" + id_rule + "&idta=" + id_type_action);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        protected void btn_del_Adjunto5_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_adjunto_5 = false;
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;

            try
            {
                if (id_rule > 0 && id_type_action > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                    if (lst_actions.Count == 1)
                    {
                        string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                        if (!String.IsNullOrEmpty(lnkAdjunto5.InnerText))
                        {
                            string file_delete = route + lnkAdjunto5.InnerText;
                            File.SetAttributes(file_delete, FileAttributes.Normal);
                            File.Delete(file_delete);
                            lnkAdjunto5.InnerText = string.Empty;
                            fuAdjunto5.Visible = true;
                            blk_del_5.Attributes["class"] = blk_del_5.Attributes["class"].Insert(blk_del_5.Attributes["class"].Length, " hidden");

                            string adjuntos = string.Empty;
                            string mail_adjuntos = lst_actions[0].adjuntos;
                            if (!String.IsNullOrEmpty(mail_adjuntos))
                            {
                                mail_adjuntos = mail_adjuntos.Replace(file_delete, string.Empty);
                                string[] list_adjuntos = mail_adjuntos.Split(',');

                                int cont = 0;
                                if (list_adjuntos.Length > 0)
                                {
                                    foreach (string _adjunto in list_adjuntos)
                                    {
                                        if (!String.IsNullOrEmpty(_adjunto))
                                        {
                                            if (cont == 0)
                                                adjuntos = _adjunto;
                                            else
                                                adjuntos += "," + _adjunto;
                                            cont++;
                                        }
                                    }
                                }
                            }

                            delete_adjunto_5 = da.updateRuleAction(lst_actions[0], adjuntos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");

                LogUtils.InsertarLog(" ERROR - regla-accion.cs::btn_del_Adjunto5_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_adjunto_5)
                Response.Redirect("regla-accion.aspx?idr=" + id_rule + "&idta=" + id_type_action);
            else
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar el adjunto');</script>");
        }

        #endregion

        #region Características Seguimiento

        protected void btn_seguimiento_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            long id_type_action = !String.IsNullOrEmpty(Request.QueryString["idta"]) ? long.Parse(Request.QueryString["idta"]) : -1;
            if (id_rule > 0 && id_type_action > 0)
            {
                /// 2.- Sacar los datos del formulario
                int? canal = null;
                if (!String.IsNullOrEmpty(ddlCanal.Value))
                    canal = int.Parse(ddlCanal.Value);
                int? estado = null;
                if (!String.IsNullOrEmpty(ddlEstado.Value))
                    estado = int.Parse(ddlEstado.Value);
                string comentarios = txt_comentario.Value;

                /// 3.- Actualizar la regla
                List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                if (lst_actions.Count == 1)
                {
                    campus_REGLAS_ACCIONES action_rule = lst_actions[0];
                    action_rule.canal = canal;
                    action_rule.estado = estado;
                    action_rule.comentario = comentarios;

                    bool update_rule = da.updateRuleAction(action_rule);
                    if (update_rule)
                        Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
                else
                {
                    campus_REGLAS_ACCIONES action_rule = new campus_REGLAS_ACCIONES();
                    action_rule.idRegla = id_rule;
                    action_rule.idTipo = id_type_action;
                    action_rule.canal = canal;
                    action_rule.estado = estado;
                    action_rule.comentario = comentarios;

                    long insert_rule = da.insertRuleAction(action_rule);
                    if (insert_rule > 0)
                        Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al añadir la regla";
                }
            }
        }

        #endregion

        private void load_action(long id_rule, long id_type_action)
        {
            /// 0.- Sacar los datos de las acciones
            if (id_type_action == (long)Constantes.type_action_rule.Comun)
            {
                /// 1.- Desbloquear el bloque comun
                block_comun.Attributes["class"] = block_comun.Attributes["class"].Replace("hidden", string.Empty);

                /// 2.- Poner el botón de volver
                btn_back_comun.HRef = "regla-mantenimiento.aspx?idr=" + id_rule;

                /// 4.- Pintar los datos en el formulario
                List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                if (lst_actions.Count == 1)
                {
                    txt_tags.Value = lst_actions[0].tags;
                    txt_score.Value = lst_actions[0].score != null ? lst_actions[0].score.Value.ToString() : string.Empty;
                }
            }
            else if (id_type_action == (long)Constantes.type_action_rule.Seguimiento)
            {
                /// 1.- Desbloquear el bloque seguimiento
                block_seguimiento.Attributes["class"] = block_seguimiento.Attributes["class"].Replace("hidden", string.Empty);

                /// 2.- Poner el botón de volver
                btn_back_seguimiento.HRef = "regla-mantenimiento.aspx?idr=" + id_rule;

                /// 3.- Pintar los datos en el formulario
                List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                if (lst_actions.Count == 1)
                {
                    ddlCanal.Value = lst_actions[0].canal != null ? lst_actions[0].canal.Value.ToString() : string.Empty;
                    ddlEstado.Value = lst_actions[0].estado != null ? lst_actions[0].estado.Value.ToString() : string.Empty;
                    txt_comentario.Value = lst_actions[0].comentario;
                }
            }
            else if (id_type_action == (long)Constantes.type_action_rule.Reasignar)
            {
                /// 1.- Desbloquear el bloque reasignar comercial
                block_reasignar.Attributes["class"] = block_reasignar.Attributes["class"].Replace("hidden", string.Empty);
                btn_reasignar.Attributes["class"] = btn_reasignar.Attributes["class"].Replace("hidden", string.Empty);

                /// 2.- Poner el botón de volver
                btn_back.HRef = "regla-mantenimiento.aspx?idr=" + id_rule;

                /// 3.- Rellenar el combo de comerciales
                List<CLIENTES> lst_users = da.getCommercialToReassign();
                if (lst_users.Count > 0)
                {
                    ddlComercial.DataSource = lst_users;
                    ddlComercial.DataTextField = "Nombre_Completo";
                    ddlComercial.DataValueField = "id_cliente";
                    ddlComercial.DataBind();
                    ddlComercial.Items.Add(new ListItem("Seleccione un comercial", "-1"));
                    ddlComercial.Value = "-1";
                }

                /// 4.- Pintar los datos en el formulario
                List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                if (lst_actions.Count == 1)
                {
                    ddlComercial.Value = lst_actions[0].idComercial.Value.ToString();

                    /// 4.1.- Cargar datos de los mails
                    txt_nombre_from.Value = !String.IsNullOrEmpty(lst_actions[0].nombreFrom) ? lst_actions[0].nombreFrom : string.Empty;
                    txt_mail_from.Value = !String.IsNullOrEmpty(lst_actions[0].mailFrom) ? lst_actions[0].mailFrom : string.Empty;
                    txt_reply_to.Value = !String.IsNullOrEmpty(lst_actions[0].replyTo) ? lst_actions[0].replyTo : string.Empty;
                    txt_asunto.Value = !String.IsNullOrEmpty(lst_actions[0].asunto) ? lst_actions[0].asunto : string.Empty;
                    txt_cuerpo.Value = !String.IsNullOrEmpty(lst_actions[0].body) ? lst_actions[0].body : string.Empty;
                    txt_cco.Value = !String.IsNullOrEmpty(lst_actions[0].cco) ? lst_actions[0].cco : string.Empty;

                    /// 4.2.- Cargar los adjuntos del mail
                    string mail_adjuntos = !String.IsNullOrEmpty(lst_actions[0].adjuntos) ? lst_actions[0].adjuntos : string.Empty;
                    if (!String.IsNullOrEmpty(mail_adjuntos))
                    {
                        string[] list_adjuntos = mail_adjuntos.Split(',');
                        if (list_adjuntos.Length > 0)
                        {
                            int index = 1;
                            string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                            string route_see = ConfigurationManager.AppSettings["urlTemplateMailRule"] + id_rule + "/";

                            foreach (string _adjunto in list_adjuntos)
                            {
                                string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                if (index == 1)
                                {
                                    fuAdjunto1.Visible = false;
                                    lnkAdjunto1.Visible = true;
                                    lnkAdjunto1.HRef = route_see + ajunto_clean;
                                    lnkAdjunto1.InnerText = ajunto_clean;
                                    blk_del_1.Attributes["class"] = blk_del_1.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else if (index == 2)
                                {
                                    fuAdjunto2.Visible = false;
                                    lnkAdjunto2.Visible = true;
                                    lnkAdjunto2.HRef = route_see + ajunto_clean;
                                    lnkAdjunto2.InnerText = ajunto_clean;
                                    blk_del_2.Attributes["class"] = blk_del_2.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else if (index == 3)
                                {
                                    fuAdjunto3.Visible = false;
                                    lnkAdjunto3.Visible = true;
                                    lnkAdjunto3.HRef = route_see + ajunto_clean;
                                    lnkAdjunto3.InnerText = ajunto_clean;
                                    blk_del_3.Attributes["class"] = blk_del_3.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else if (index == 4)
                                {
                                    fuAdjunto4.Visible = false;
                                    lnkAdjunto4.Visible = true;
                                    lnkAdjunto4.HRef = route_see + ajunto_clean;
                                    lnkAdjunto4.InnerText = ajunto_clean;
                                    blk_del_4.Attributes["class"] = blk_del_4.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else
                                {
                                    fuAdjunto5.Visible = false;
                                    lnkAdjunto5.Visible = true;
                                    lnkAdjunto5.HRef = route_see + ajunto_clean;
                                    lnkAdjunto5.InnerText = ajunto_clean;
                                    blk_del_5.Attributes["class"] = blk_del_5.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                index++;
                            }
                        }
                    }
                }
            }
            else if (id_type_action == (long)Constantes.type_action_rule.Mail)
            {
                /// 1.- Desbloquear el bloque mail
                block_reasignar.Attributes["class"] = block_reasignar.Attributes["class"].Replace("hidden", string.Empty);
                btn_mail.Attributes["class"] = btn_mail.Attributes["class"].Replace("hidden", string.Empty);

                /// 2.- Poner el botón de volver
                btn_back.HRef = "regla-mantenimiento.aspx?idr=" + id_rule;

                /// 2.1.- Ocultar al comercial
                div_comercial.Visible = false;

                /// 3.- Pintar los datos en el formulario
                List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, id_type_action);
                if (lst_actions.Count == 1)
                {
                    /// 3.1.- Cargar datos de los mails
                    txt_nombre_from.Value = !String.IsNullOrEmpty(lst_actions[0].nombreFrom) ? lst_actions[0].nombreFrom : string.Empty;
                    txt_mail_from.Value = !String.IsNullOrEmpty(lst_actions[0].mailFrom) ? lst_actions[0].mailFrom : string.Empty;
                    txt_reply_to.Value = !String.IsNullOrEmpty(lst_actions[0].replyTo) ? lst_actions[0].replyTo : string.Empty;
                    txt_asunto.Value = !String.IsNullOrEmpty(lst_actions[0].asunto) ? lst_actions[0].asunto : string.Empty;
                    txt_cuerpo.Value = !String.IsNullOrEmpty(lst_actions[0].body) ? lst_actions[0].body : string.Empty;
                    txt_cco.Value = !String.IsNullOrEmpty(lst_actions[0].cco) ? lst_actions[0].cco : string.Empty;

                    /// 3.2.- Cargar los adjuntos del mail
                    string mail_adjuntos = !String.IsNullOrEmpty(lst_actions[0].adjuntos) ? lst_actions[0].adjuntos : string.Empty;
                    if (!String.IsNullOrEmpty(mail_adjuntos))
                    {
                        string[] list_adjuntos = mail_adjuntos.Split(',');
                        if (list_adjuntos.Length > 0)
                        {
                            int index = 1;
                            string route = ConfigurationManager.AppSettings["routeTemplateMailRule"] + id_rule + "\\";
                            string route_see = ConfigurationManager.AppSettings["urlTemplateMailRule"] + id_rule + "/";

                            foreach (string _adjunto in list_adjuntos)
                            {
                                string ajunto_clean = _adjunto.Replace(route, string.Empty).Trim();

                                if (index == 1)
                                {
                                    fuAdjunto1.Visible = false;
                                    lnkAdjunto1.Visible = true;
                                    lnkAdjunto1.HRef = route_see + ajunto_clean;
                                    lnkAdjunto1.InnerText = ajunto_clean;
                                    blk_del_1.Attributes["class"] = blk_del_1.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else if (index == 2)
                                {
                                    fuAdjunto2.Visible = false;
                                    lnkAdjunto2.Visible = true;
                                    lnkAdjunto2.HRef = route_see + ajunto_clean;
                                    lnkAdjunto2.InnerText = ajunto_clean;
                                    blk_del_2.Attributes["class"] = blk_del_2.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else if (index == 3)
                                {
                                    fuAdjunto3.Visible = false;
                                    lnkAdjunto3.Visible = true;
                                    lnkAdjunto3.HRef = route_see + ajunto_clean;
                                    lnkAdjunto3.InnerText = ajunto_clean;
                                    blk_del_3.Attributes["class"] = blk_del_3.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else if (index == 4)
                                {
                                    fuAdjunto4.Visible = false;
                                    lnkAdjunto4.Visible = true;
                                    lnkAdjunto4.HRef = route_see + ajunto_clean;
                                    lnkAdjunto4.InnerText = ajunto_clean;
                                    blk_del_4.Attributes["class"] = blk_del_4.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                else
                                {
                                    fuAdjunto5.Visible = false;
                                    lnkAdjunto5.Visible = true;
                                    lnkAdjunto5.HRef = route_see + ajunto_clean;
                                    lnkAdjunto5.InnerText = ajunto_clean;
                                    blk_del_5.Attributes["class"] = blk_del_5.Attributes["class"].Replace("hidden", string.Empty);
                                }
                                index++;
                            }
                        }
                    }
                }
            }
        }
    }
}