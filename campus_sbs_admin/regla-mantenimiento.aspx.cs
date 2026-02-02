using sbs_DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class regla_mantenimiento : System.Web.UI.Page
    {
        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            /// 0.- Sacar los datos del usuario 
            List<CLIENTES> list_user = new List<CLIENTES>();
            if (!String.IsNullOrEmpty(Request.QueryString["k"]))
            {
                list_user = da.getUserByKey(Request.QueryString["k"]);
                if (list_user.Count > 0)
                    Session.Add("usuario", list_user[0]);
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
                    /// 1.- Cargar combos
                    cargar_tipos();

                    /// 2.- Pintar datos 
                    long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
                    if (id_rule > 0)
                        load_rule(id_rule, list_user[0]);
                }
            }
        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /// 1.- Borrar los errores
            txt_error.InnerHtml = string.Empty;
            
            /// 2.- Sacar los datos del formulario
            int tipo = !String.IsNullOrEmpty(ddl_lista_tipo_automatizacion.Value) ? int.Parse(ddl_lista_tipo_automatizacion.Value) : -1;
            int orden = !String.IsNullOrEmpty(txt_orden.Value) ? int.Parse(txt_orden.Value) : -1;
            string nombre_regla = txt_name_rule.Value.Trim();
            string descripcion_regla = txt_descripcion.Value.Trim();
            
            /// 3.- Comprobar si hay errores
            if (tipo==-1 || orden == -1 || String.IsNullOrEmpty(nombre_regla))
                txt_error.InnerHtml = "Todos los campos son obligatorios menos la descripción";
            else
            {
                bool process = true;

                /// Sacar la regla si existe
                long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
                
                /// 4.- Comprobar si existe una regla con el mismo orden
                List<campus_REGLAS_AUTOMATIZACION> lst_rules = da.getRuleAutomationByIdType(tipo);
                if (lst_rules.Count > 0)
                {
                    if (id_rule > 0)
                        lst_rules = lst_rules.Where(r => r.id_regla != id_rule).ToList();

                    int num_rules = lst_rules.Where(r => r.orden == orden && r.activo).Count();
                    if (num_rules == 1)
                        process = false;
                }

                if (!process)
                    txt_error.InnerHtml = "El orden introducido ya existe en otra regla de ese tipo";
                else
                {
                    if (id_rule > 0)
                    {
                        List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                        if (lst_rule.Count == 1)
                        {
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.id_tipo = tipo;
                            regla.orden = orden;
                            regla.nombre = nombre_regla;
                            regla.descripcion = descripcion_regla;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                Response.Redirect("regla-mantenimiento.aspx?idr=" + id_rule);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                    else
                    {
                        /// 5.- Crear la regla
                        campus_REGLAS_AUTOMATIZACION rule = new campus_REGLAS_AUTOMATIZACION();
                        rule.id_tipo = tipo;
                        rule.orden = orden;
                        rule.nombre = nombre_regla;
                        rule.descripcion = descripcion_regla;
                        rule.activo = false;

                        long insert_rule = da.insertRuleAutomation(rule);
                        if (insert_rule > 0)
                            Response.Redirect("regla-mantenimiento.aspx?idr=" + insert_rule);
                        else
                            txt_error.InnerHtml = "Se ha producido un error al guardar la regla";
                    }
                }
            }
        }

        #region Estados

        protected void btn_sel_status_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar todos los origenes
                List<Estados_Lead> lst_status = getStatus();
                if (lst_status.Count > 0)
                {
                    string list_status = string.Empty;
                    foreach (var status in lst_status)
                    {
                        if (String.IsNullOrEmpty(list_status))
                            list_status = status.id_estado.ToString();
                        else
                            list_status = list_status + "," + status.id_estado.ToString();
                    }

                    if (!String.IsNullOrEmpty(list_status))
                    {
                        /// 3.- Actualizar la regla
                        List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                        if (lst_rule.Count == 1)
                        {
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.estados = list_status;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_status(list_status);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_sel_status_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string estados = hidStatus.Value;
                if (!String.IsNullOrEmpty(estados))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los origenes
                        List<long> lst_id_status = new List<long>();
                        if (!String.IsNullOrEmpty(estados))
                        {
                            List<string> lst_ids = estados.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var status in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(status) && !lst_id_status.Contains(long.Parse(status)))
                                        lst_id_status.Add(long.Parse(status));
                                }
                            }
                        }
                        string sStatus = lst_rule[0].estados;
                        if (!String.IsNullOrEmpty(sStatus))
                        {
                            List<string> lst_ids = sStatus.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var estado in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(estado) && !lst_id_status.Contains(long.Parse(estado)))
                                        lst_id_status.Add(long.Parse(estado));
                                }
                            }
                        }

                        string list_status = string.Empty;
                        foreach (var status in lst_id_status)
                        {
                            if (String.IsNullOrEmpty(list_status))
                                list_status = status.ToString();
                            else
                                list_status = list_status + "," + status.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_status))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.estados = list_status;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_status(list_status);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_status_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string estados = hidStatusSel.Value;
                if (!String.IsNullOrEmpty(estados))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los origenes
                        List<long> lst_id_status = new List<long>();
                        string sStatus = lst_rule[0].estados;
                        if (!String.IsNullOrEmpty(sStatus))
                        {
                            List<string> lst_ids = sStatus.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var estado in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(estado) && !lst_id_status.Contains(long.Parse(estado)))
                                        lst_id_status.Add(long.Parse(estado));
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(estados))
                        {
                            List<string> lst_ids = estados.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var estado in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(estado) && lst_id_status.Contains(long.Parse(estado)))
                                        lst_id_status.Remove(long.Parse(estado));
                                }
                            }
                        }

                        string list_status = string.Empty;
                        foreach (var estado in lst_id_status)
                        {
                            if (String.IsNullOrEmpty(list_status))
                                list_status = estado.ToString();
                            else
                                list_status = list_status + "," + estado.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_status))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.estados = list_status;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_status(list_status);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_status_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_estado.Attributes["class"] = card_estado.Attributes["class"].Replace("collapsed", "");
            card_estado.Attributes["aria-expanded"] = card_estado.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_1.Attributes["class"] = collapse_bbdd_1.Attributes["class"].Insert(collapse_bbdd_1.Attributes["class"].Length, " show");
            
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Quitar todos los origenes                
                string list_status = null;

                /// 3.- Actualizar la regla
                List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                if (lst_rule.Count == 1)
                {
                    campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                    regla.estados = list_status;

                    bool update_rule = da.updateRuleAutomation(regla);
                    if (update_rule)
                        load_status(list_status);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
            }
        }

        private void load_status(string status)
        {
            /// 1.- Sacar los estados seleccionados
            List<Estados_Lead> lst_status = getStatus();
            if (lst_status.Count > 0)
            {
                List<long> lst_id_status = new List<long>();
                if (!String.IsNullOrEmpty(status))
                {
                    List<string> lst_ids = status.Split(',').ToList();
                    if (lst_ids.Count > 0)
                    {
                        foreach (var estado in lst_ids)
                        {
                            if (!String.IsNullOrEmpty(estado) && !lst_id_status.Contains(long.Parse(estado)))
                                lst_id_status.Add(long.Parse(estado));
                        }
                    }
                }

                /// 2.- Pintar la tabla de origenes
                List<Estados_Lead> list_status = lst_status.Where(s => !lst_id_status.Contains(s.id_estado)).ToList();
                tabla_all_status.InnerHtml = paint_table_status(list_status, "table_all_status", true);

                /// 3.- Pintar la tabla de origenes seleccionados
                List<Estados_Lead> list_status_sel = lst_status.Where(s => lst_id_status.Contains(s.id_estado)).ToList();
                tabla_sel_status.InnerHtml = paint_table_status(list_status_sel, "table_status", false);

                /// 4.- Ocultar los botones de deseleccionar si no hay origenes seleccionados
                if (list_status.Count == 0)
                {
                    btn_sel_status.Visible = false;
                    btn_sel_status_all.Visible = false;
                    btn_desel_status.Visible = true;
                    btn_desel_status_all.Visible = true;
                }
                else if (list_status_sel.Count == 0)
                {
                    btn_sel_status.Visible = true;
                    btn_sel_status_all.Visible = true;
                    btn_desel_status.Visible = false;
                    btn_desel_status_all.Visible = false;
                }
                else
                {
                    btn_sel_status.Visible = true;
                    btn_sel_status_all.Visible = true;
                    btn_desel_status.Visible = true;
                    btn_desel_status_all.Visible = true;
                }

                /// 5.- Limpiar los campos
                hidStatus.Value = string.Empty;
                hidStatusSel.Value = string.Empty;
            }
        }
        private List<Estados_Lead> getStatus()
        {
            List<Estados_Lead> list = new List<Estados_Lead>();
            list.Add(getStatus("Nuevo", 0));
            list.Add(getStatus("Rechazado", 1));
            list.Add(getStatus("Duplicado", 2));
            list.Add(getStatus("Futuro", 3));
            list.Add(getStatus("Cerrar", 4));
            list.Add(getStatus("Sin Contactar", 5));
            list.Add(getStatus("Indeciso", 6));
            list.Add(getStatus("Interesado", 7));
            list.Add(getStatus("Enviar Contrato", 8));
            list.Add(getStatus("Recibir Contrato", 9));
            list.Add(getStatus("Pago", 10));
            list.Add(getStatus("Matriculado", 11));            
            return list;
        }
        private Estados_Lead getStatus(string status, int id)
        {
            Estados_Lead lead = new Estados_Lead();
            lead.id_estado = id;
            lead.nombre = status;
            return lead;
        }

        private string paint_table_status(List<Estados_Lead> list_status, string table, bool all)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id=\"" + table + "\" class=\"display compact\" style =\"width:100%\">");
            sbuild.Append("<thead>");
            //Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            if (all)
                sbuild.Append("<th>Estado</th>");
            else
                sbuild.Append("<th>Estado Seleccionado</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var status in list_status)
            {
                sbuild.Append("<tr>");
                if (all)
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + status.id_estado + "\" onclick=\"chk_mark_status(this,1)\"></td>");
                else
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + status.id_estado + "\" onclick=\"chk_mark_status(this,0)\"></td>");
                sbuild.Append("<td>" + status.nombre + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody>");
            sbuild.Append("</table>");

            return sbuild.ToString();
        }

        #endregion

        #region Origen

        protected void btn_sel_origin_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_origen.Attributes["class"] = card_origen.Attributes["class"].Replace("collapsed", "");
            card_origen.Attributes["aria-expanded"] = card_origen.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_2.Attributes["class"] = collapse_bbdd_2.Attributes["class"].Insert(collapse_bbdd_2.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar todos los origenes
                List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");
                if (lst_origins.Count > 0)
                {
                    string list_origins = string.Empty;
                    foreach (var origin in lst_origins)
                    {
                        if (String.IsNullOrEmpty(list_origins))
                            list_origins = origin.ID_Aux.ToString();
                        else
                            list_origins = list_origins + "," + origin.ID_Aux.ToString();
                    }

                    if (!String.IsNullOrEmpty(list_origins))
                    {
                        /// 3.- Actualizar la regla
                        List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                        if (lst_rule.Count == 1)
                        {
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.origenes = list_origins;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_origins(list_origins);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_sel_origin_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_origen.Attributes["class"] = card_origen.Attributes["class"].Replace("collapsed", "");
            card_origen.Attributes["aria-expanded"] = card_origen.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_2.Attributes["class"] = collapse_bbdd_2.Attributes["class"].Insert(collapse_bbdd_2.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string origen = hidOrigenes.Value;
                if (!String.IsNullOrEmpty(origen))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los origenes
                        List<long> lst_id_origins = new List<long>();
                        if (!String.IsNullOrEmpty(origen))
                        {
                            List<string> lst_ids = origen.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var origin in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(origin) && !lst_id_origins.Contains(long.Parse(origin)))
                                        lst_id_origins.Add(long.Parse(origin));
                                }
                            }
                        }
                        string sOrigin = lst_rule[0].origenes;
                        if (!String.IsNullOrEmpty(sOrigin))
                        {
                            List<string> lst_ids = sOrigin.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var origin in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(origin) && !lst_id_origins.Contains(long.Parse(origin)))
                                        lst_id_origins.Add(long.Parse(origin));
                                }
                            }
                        }

                        string list_origins = string.Empty;
                        foreach (var origin in lst_id_origins)
                        {
                            if (String.IsNullOrEmpty(list_origins))
                                list_origins = origin.ToString();
                            else
                                list_origins = list_origins + "," + origin.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_origins))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.origenes = list_origins;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_origins(list_origins);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_origin_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_origen.Attributes["class"] = card_origen.Attributes["class"].Replace("collapsed", "");
            card_origen.Attributes["aria-expanded"] = card_origen.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_2.Attributes["class"] = collapse_bbdd_2.Attributes["class"].Insert(collapse_bbdd_2.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string origen = hidOrigenesSel.Value;
                if (!String.IsNullOrEmpty(origen))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los origenes
                        List<long> lst_id_origins = new List<long>();
                        string sOrigin = lst_rule[0].origenes;
                        if (!String.IsNullOrEmpty(sOrigin))
                        {
                            List<string> lst_ids = sOrigin.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var origin in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(origin) && !lst_id_origins.Contains(long.Parse(origin)))
                                        lst_id_origins.Add(long.Parse(origin));
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(origen))
                        {
                            List<string> lst_ids = origen.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var origin in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(origin) && lst_id_origins.Contains(long.Parse(origin)))
                                        lst_id_origins.Remove(long.Parse(origin));
                                }
                            }
                        }                        

                        string list_origins = string.Empty;
                        foreach (var origin in lst_id_origins)
                        {
                            if (String.IsNullOrEmpty(list_origins))
                                list_origins = origin.ToString();
                            else
                                list_origins = list_origins + "," + origin.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_origins))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.origenes = list_origins;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_origins(list_origins);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_origin_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_origen.Attributes["class"] = card_origen.Attributes["class"].Replace("collapsed", "");
            card_origen.Attributes["aria-expanded"] = card_origen.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_2.Attributes["class"] = collapse_bbdd_2.Attributes["class"].Insert(collapse_bbdd_2.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Quitar todos los origenes                
                string list_origins = null;

                /// 3.- Actualizar la regla
                List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                if (lst_rule.Count == 1)
                {
                    campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                    regla.origenes = list_origins;

                    bool update_rule = da.updateRuleAutomation(regla);
                    if (update_rule)
                        load_origins(list_origins);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
            }
        }

        private void load_origins(string origen)
        {
            /// 1.- Sacar los origenes seleccionados
            List<campus_AUX> lst_origins = da.getAuxiliars("ORIGEN");
            if (lst_origins.Count > 0)
            {
                List<long> lst_id_origins = new List<long>();
                if (!String.IsNullOrEmpty(origen))
                {
                    List<string> lst_ids = origen.Split(',').ToList();
                    if (lst_ids.Count > 0)
                    {
                        foreach (var origin in lst_ids)
                        {
                            if (!String.IsNullOrEmpty(origin) && !lst_id_origins.Contains(long.Parse(origin)))
                                lst_id_origins.Add(long.Parse(origin));
                        }
                    }
                }

                /// 2.- Pintar la tabla de origenes
                List<campus_AUX> list_origins = lst_origins.Where(o => !lst_id_origins.Contains(o.ID_Aux)).ToList();
                tabla_all_origins.InnerHtml = paint_table_origins(list_origins, "table_all_origins", true);

                /// 3.- Pintar la tabla de origenes seleccionados
                List<campus_AUX> list_origins_sel = lst_origins.Where(o => lst_id_origins.Contains(o.ID_Aux)).ToList();
                tabla_sel_origins.InnerHtml = paint_table_origins(list_origins_sel, "table_origins", false);

                /// 4.- Ocultar los botones de deseleccionar si no hay origenes seleccionados
                if (list_origins.Count == 0)
                {
                    btn_sel_origin.Visible = false;
                    btn_sel_origin_all.Visible = false;
                    btn_desel_origin.Visible = true;
                    btn_desel_origin_all.Visible = true;
                }
                else if (list_origins_sel.Count == 0)
                {
                    btn_sel_origin.Visible = true;
                    btn_sel_origin_all.Visible = true;
                    btn_desel_origin.Visible = false;
                    btn_desel_origin_all.Visible = false;
                }
                else
                {
                    btn_sel_origin.Visible = true;
                    btn_sel_origin_all.Visible = true;
                    btn_desel_origin.Visible = true;
                    btn_desel_origin_all.Visible = true;
                }

                /// 5.- Limpiar los campos
                hidOrigenes.Value = string.Empty;
                hidOrigenesSel.Value = string.Empty;
            }
        }
        private string paint_table_origins(List<campus_AUX> list_origins, string table, bool all)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id=\"" + table + "\" class=\"display compact\" style =\"width:100%\">");
            sbuild.Append("<thead>");
            //Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            if (all)
                sbuild.Append("<th>Origen</th>");
            else
                sbuild.Append("<th>Origen Seleccionado</th>");
            sbuild.Append("<th>ID</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var origin in list_origins)
            {
                sbuild.Append("<tr>");
                if (all)
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + origin.ID_Aux + "\" onclick=\"chk_mark_origin(this,1)\"></td>");
                else
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + origin.ID_Aux + "\" onclick=\"chk_mark_origin(this,0)\"></td>");
                sbuild.Append("<td>" + origin.Nombre + "</td>");
                sbuild.Append("<td>" + origin.ID_Aux + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody>");
            sbuild.Append("</table>");

            return sbuild.ToString();
        }

        #endregion

        #region Curso

        protected void btn_sel_course_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_curso.Attributes["class"] = card_curso.Attributes["class"].Replace("collapsed", "");
            card_curso.Attributes["aria-expanded"] = card_curso.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_3.Attributes["class"] = collapse_bbdd_3.Attributes["class"].Insert(collapse_bbdd_3.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar todos los cursos
                List<campus_CURSO> lst_courses = da.getCourses(true);
                if (lst_courses.Count > 0)
                {
                    string list_courses = string.Empty;
                    foreach (var course in lst_courses)
                    {
                        if (String.IsNullOrEmpty(list_courses))
                            list_courses = course.ID_Curso.ToString();
                        else
                            list_courses = list_courses + "," + course.ID_Curso.ToString();
                    }

                    if (!String.IsNullOrEmpty(list_courses))
                    {
                        /// 3.- Actualizar la regla
                        List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                        if (lst_rule.Count == 1)
                        {
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.cursos = list_courses;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_courses(list_courses);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_sel_course_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_curso.Attributes["class"] = card_curso.Attributes["class"].Replace("collapsed", "");
            card_curso.Attributes["aria-expanded"] = card_curso.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_3.Attributes["class"] = collapse_bbdd_3.Attributes["class"].Insert(collapse_bbdd_3.Attributes["class"].Length, " show");
                        
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string curso = hidCursos.Value;
                if (!String.IsNullOrEmpty(curso))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los cursos
                        List<long> lst_id_courses = new List<long>();
                        if (!String.IsNullOrEmpty(curso))
                        {
                            List<string> lst_ids = curso.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var course in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(course) && !lst_id_courses.Contains(long.Parse(course)))
                                        lst_id_courses.Add(long.Parse(course));
                                }
                            }
                        }

                        string sCourse = lst_rule[0].cursos;
                        if (!String.IsNullOrEmpty(sCourse))
                        {
                            List<string> lst_ids = sCourse.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var course in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(course) && !lst_id_courses.Contains(long.Parse(course)))
                                        lst_id_courses.Add(long.Parse(course));
                                }
                            }
                        }

                        string list_courses = string.Empty;
                        foreach (var course in lst_id_courses)
                        {
                            if (String.IsNullOrEmpty(list_courses))
                                list_courses = course.ToString();
                            else
                                list_courses = list_courses + "," + course.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_courses))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.cursos = list_courses;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_courses(list_courses);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_course_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_curso.Attributes["class"] = card_curso.Attributes["class"].Replace("collapsed", "");
            card_curso.Attributes["aria-expanded"] = card_curso.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_3.Attributes["class"] = collapse_bbdd_3.Attributes["class"].Insert(collapse_bbdd_3.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string curso = hidCursosSel.Value;
                if (!String.IsNullOrEmpty(curso))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los cursos
                        List<long> lst_id_courses = new List<long>();
                        string sCourse = lst_rule[0].cursos;
                        if (!String.IsNullOrEmpty(sCourse))
                        {
                            List<string> lst_ids = sCourse.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var course in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(course) && !lst_id_courses.Contains(long.Parse(course)))
                                        lst_id_courses.Add(long.Parse(course));
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(curso))
                        {
                            List<string> lst_ids = curso.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var course in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(course) && lst_id_courses.Contains(long.Parse(course)))
                                        lst_id_courses.Remove(long.Parse(course));
                                }
                            }
                        }

                        string list_courses = string.Empty;
                        foreach (var course in lst_id_courses)
                        {
                            if (String.IsNullOrEmpty(list_courses))
                                list_courses = course.ToString();
                            else
                                list_courses = list_courses + "," + course.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_courses))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.cursos = list_courses;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_courses(list_courses);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_course_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_curso.Attributes["class"] = card_curso.Attributes["class"].Replace("collapsed", "");
            card_curso.Attributes["aria-expanded"] = card_curso.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_3.Attributes["class"] = collapse_bbdd_3.Attributes["class"].Insert(collapse_bbdd_3.Attributes["class"].Length, " show");
            
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Quitar todos los origenes                
                string list_courses = null;

                /// 3.- Actualizar la regla
                List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                if (lst_rule.Count == 1)
                {
                    campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                    regla.cursos = list_courses;

                    bool update_rule = da.updateRuleAutomation(regla);
                    if (update_rule)
                        load_courses(list_courses);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
            }
        }

        private void load_courses(string cursos)
        {
            /// 1.- Sacar los cursos seleccionados
            List<campus_CURSO> lst_courses = da.getCourses(true);
            if (lst_courses.Count > 0)
            {
                List<long> lst_id_courses = new List<long>();
                if (!String.IsNullOrEmpty(cursos))
                {
                    List<string> lst_ids = cursos.Split(',').ToList();
                    if (lst_ids.Count > 0)
                    {
                        foreach (var curso in lst_ids)
                        {
                            if (!String.IsNullOrEmpty(curso) && !lst_id_courses.Contains(long.Parse(curso)))
                                lst_id_courses.Add(long.Parse(curso));
                        }
                    }
                }

                /// 2.- Pintar la tabla de cursos
                List<campus_CURSO> list_courses = lst_courses.Where(o => !lst_id_courses.Contains(o.ID_Curso)).ToList();
                tabla_all_courses.InnerHtml = paint_table_courses(list_courses, "table_all_courses", true);

                /// 3.- Pintar la tabla de origenes seleccionados
                List<campus_CURSO> list_courses_sel = lst_courses.Where(o => lst_id_courses.Contains(o.ID_Curso)).ToList();
                tabla_sel_courses.InnerHtml = paint_table_courses(list_courses_sel, "table_courses", false);

                /// 4.- Ocultar los botones de deseleccionar si no hay cursos seleccionados
                if (list_courses.Count == 0)
                {
                    btn_sel_course.Visible = false;
                    btn_sel_course_all.Visible = false;
                    btn_desel_course.Visible = true;
                    btn_desel_course_all.Visible = true;
                }
                else if (list_courses_sel.Count == 0)
                {
                    btn_sel_course.Visible = true;
                    btn_sel_course_all.Visible = true;
                    btn_desel_course.Visible = false;
                    btn_desel_course_all.Visible = false;
                }
                else
                {
                    btn_sel_course.Visible = true;
                    btn_sel_course_all.Visible = true;
                    btn_desel_course.Visible = true;
                    btn_desel_course_all.Visible = true;
                }

                /// 5.- Limpiar los campos
                hidCursos.Value = string.Empty;
                hidCursosSel.Value = string.Empty;
            }
        }
        private string paint_table_courses(List<campus_CURSO> list_courses, string table, bool all)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id=\"" + table + "\" class=\"display compact\" style =\"width:100%\">");
            sbuild.Append("<thead>");
            //Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            if (all)
                sbuild.Append("<th>Curso</th>");
            else
                sbuild.Append("<th>Curso Seleccionado</th>");
            sbuild.Append("<th>ID</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var course in list_courses)
            {
                sbuild.Append("<tr>");
                if (all)
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + course.ID_Curso + "\" onclick=\"chk_mark_course(this,1)\"></td>");
                else
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + course.ID_Curso + "\" onclick=\"chk_mark_course(this,0)\"></td>");
                sbuild.Append("<td>" + course.Nombre + "</td>");
                sbuild.Append("<td>" + course.ID_Curso + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody>");
            sbuild.Append("</table>");

            return sbuild.ToString();
        }

        #endregion

        #region País

        protected void btn_sel_paises_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_country.Attributes["class"] = card_country.Attributes["class"].Replace("collapsed", "");
            card_country.Attributes["aria-expanded"] = card_country.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_4.Attributes["class"] = collapse_bbdd_4.Attributes["class"].Insert(collapse_bbdd_4.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar todos los países
                List<Paises> lst_paises = da.getCountries();
                if (lst_paises.Count > 0)
                {
                    string list_paises = string.Empty;
                    foreach (var country in lst_paises)
                    {
                        if (String.IsNullOrEmpty(list_paises))
                            list_paises = country.id_pais.ToString();
                        else
                            list_paises = list_paises + "," + country.id_pais.ToString();
                    }

                    if (!String.IsNullOrEmpty(list_paises))
                    {
                        /// 3.- Actualizar la regla
                        List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                        if (lst_rule.Count == 1)
                        {
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.paises = list_paises;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_paises(list_paises);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_sel_paises_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_country.Attributes["class"] = card_country.Attributes["class"].Replace("collapsed", "");
            card_country.Attributes["aria-expanded"] = card_country.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_4.Attributes["class"] = collapse_bbdd_4.Attributes["class"].Insert(collapse_bbdd_4.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string paises = hidPais.Value;
                if (!String.IsNullOrEmpty(paises))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los cursos
                        List<long> lst_id_paises = new List<long>();
                        if (!String.IsNullOrEmpty(paises))
                        {
                            List<string> lst_ids = paises.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var country in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(country) && !lst_id_paises.Contains(long.Parse(country)))
                                        lst_id_paises.Add(long.Parse(country));
                                }
                            }
                        }

                        string sCountry = lst_rule[0].paises;
                        if (!String.IsNullOrEmpty(sCountry))
                        {
                            List<string> lst_ids = sCountry.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var country in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(country) && !lst_id_paises.Contains(long.Parse(country)))
                                        lst_id_paises.Add(long.Parse(country));
                                }
                            }
                        }

                        string list_paises = string.Empty;
                        foreach (var country in lst_id_paises)
                        {
                            if (String.IsNullOrEmpty(list_paises))
                                list_paises = country.ToString();
                            else
                                list_paises = list_paises + "," + country.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_paises))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.paises = list_paises;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_paises(list_paises);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_paises_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_country.Attributes["class"] = card_country.Attributes["class"].Replace("collapsed", "");
            card_country.Attributes["aria-expanded"] = card_country.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_4.Attributes["class"] = collapse_bbdd_4.Attributes["class"].Insert(collapse_bbdd_4.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los ids del campo oculto
                string paises = hidPaisSel.Value;
                if (!String.IsNullOrEmpty(paises))
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de los cursos
                        List<long> lst_id_paises = new List<long>();
                        string sCountry = lst_rule[0].paises;
                        if (!String.IsNullOrEmpty(sCountry))
                        {
                            List<string> lst_ids = sCountry.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var country in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(country) && !lst_id_paises.Contains(long.Parse(country)))
                                        lst_id_paises.Add(long.Parse(country));
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(paises))
                        {
                            List<string> lst_ids = paises.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var country in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(country) && lst_id_paises.Contains(long.Parse(country)))
                                        lst_id_paises.Remove(long.Parse(country));
                                }
                            }
                        }

                        string list_paises = string.Empty;
                        foreach (var country in lst_id_paises)
                        {
                            if (String.IsNullOrEmpty(list_paises))
                                list_paises = country.ToString();
                            else
                                list_paises = list_paises + "," + country.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_paises))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.paises = list_paises;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_paises(list_paises);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }
        protected void btn_desel_paises_all_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_country.Attributes["class"] = card_country.Attributes["class"].Replace("collapsed", "");
            card_country.Attributes["aria-expanded"] = card_country.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_4.Attributes["class"] = collapse_bbdd_4.Attributes["class"].Insert(collapse_bbdd_4.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Quitar todos los origenes                
                string list_paises = null;

                /// 3.- Actualizar la regla
                List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                if (lst_rule.Count == 1)
                {
                    campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                    regla.paises = list_paises;

                    bool update_rule = da.updateRuleAutomation(regla);
                    if (update_rule)
                        load_paises(list_paises);
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
            }
        }

        private void load_paises(string paises)
        {
            /// 1.- Sacar los cursos seleccionados
            List<Paises> lst_paises = da.getCountries();
            if (lst_paises.Count > 0)
            {
                List<long> lst_id_paises = new List<long>();
                if (!String.IsNullOrEmpty(paises))
                {
                    List<string> lst_ids = paises.Split(',').ToList();
                    if (lst_ids.Count > 0)
                    {
                        foreach (var pais in lst_ids)
                        {
                            if (!String.IsNullOrEmpty(pais) && !lst_id_paises.Contains(long.Parse(pais)))
                                lst_id_paises.Add(long.Parse(pais));
                        }
                    }
                }

                /// 2.- Pintar la tabla de cursos
                List<Paises> list_paises = lst_paises.Where(p => !lst_id_paises.Contains(p.id_pais)).ToList();
                tabla_all_paises.InnerHtml = paint_table_paises(list_paises, "table_all_paises", true);

                /// 3.- Pintar la tabla de origenes seleccionados
                List<Paises> list_paises_sel = lst_paises.Where(p => lst_id_paises.Contains(p.id_pais)).ToList();
                tabla_sel_paises.InnerHtml = paint_table_paises(list_paises_sel, "table_paises", false);

                /// 4.- Ocultar los botones de deseleccionar si no hay cursos seleccionados
                if (list_paises.Count == 0)
                {
                    btn_sel_paises.Visible = false;
                    btn_sel_paises_all.Visible = false;
                    btn_desel_paises.Visible = true;
                    btn_desel_paises_all.Visible = true;
                }
                else if (list_paises_sel.Count == 0)
                {
                    btn_sel_paises.Visible = true;
                    btn_sel_paises_all.Visible = true;
                    btn_desel_paises.Visible = false;
                    btn_desel_paises_all.Visible = false;
                }
                else
                {
                    btn_sel_paises.Visible = true;
                    btn_sel_paises_all.Visible = true;
                    btn_desel_paises.Visible = true;
                    btn_desel_paises_all.Visible = true;
                }

                /// 5.- Limpiar los campos
                hidPais.Value = string.Empty;
                hidPaisSel.Value = string.Empty;
            }
        }
        private string paint_table_paises(List<Paises> list_paises, string table, bool all)
        {
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id=\"" + table + "\" class=\"display compact\" style =\"width:100%\">");
            sbuild.Append("<thead>");
            //Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th></th>");
            if (all)
                sbuild.Append("<th>País</th>");
            else
                sbuild.Append("<th>País Seleccionado</th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var pais in list_paises)
            {
                sbuild.Append("<tr>");
                if (all)
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + pais.id_pais + "\" onclick=\"chk_mark_country(this,1)\"></td>");
                else
                    sbuild.Append("<td><input type=\"checkbox\" value=\"" + pais.id_pais + "\" onclick=\"chk_mark_country(this,0)\"></td>");
                sbuild.Append("<td>" + pais.nombre + "</td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody>");
            sbuild.Append("</table>");

            return sbuild.ToString();
        }

        #endregion

        #region Fecha

        protected void btnGuardarFecha_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los datos del formulario
                bool date_all = chk_date_all.Checked;
                DateTime? date_start = null;
                DateTime? date_end = null;
                if (!date_all)
                {
                    date_start = DateTime.Parse(txt_fecha_inicio.Value);
                    date_end = DateTime.Parse(txt_fecha_fin.Value);
                }

                /// 3.- Actualizar la regla
                List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                if (lst_rule.Count == 1)
                {
                    campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                    regla.fecha_all = date_all;
                    regla.fecha_inicio = date_start;
                    regla.fecha_fin = date_end;

                    bool update_rule = da.updateRuleAutomation(regla);
                    if (update_rule)
                    {
                        card_date.Attributes["class"] = card_date.Attributes["class"].Replace("collapsed", "");
                        card_date.Attributes["aria-expanded"] = card_date.Attributes["aria-expanded"].Replace("false", "true");
                        collapse_bbdd_5.Attributes["class"] = collapse_bbdd_5.Attributes["class"].Insert(collapse_bbdd_5.Attributes["class"].Length, " show");
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
            }
        }

        #endregion

        #region Horas

        protected void btnGuardarHoras_Click(object sender, EventArgs e)
        {
            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Sacar los datos del formulario
                bool hour_all = chk_hour_all.Checked;
                string _hour_start = null;
                string _hour_end = null;
                if (!hour_all)
                {
                    _hour_start = DateTime.Today.AddHours(int.Parse(hour_start.Value)).AddMinutes(int.Parse(min_start.Value)).ToShortTimeString();
                    _hour_end = DateTime.Today.AddHours(int.Parse(hour_end.Value)).AddMinutes(int.Parse(min_end.Value)).ToShortTimeString();
                }

                /// 3.- Actualizar la regla
                List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                if (lst_rule.Count == 1)
                {
                    campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                    regla.hora_all = hour_all;
                    regla.hora_inicio = _hour_start;
                    regla.hora_fin = _hour_end;

                    bool update_rule = da.updateRuleAutomation(regla);
                    if (update_rule)
                    {
                        card_hour.Attributes["class"] = card_hour.Attributes["class"].Replace("collapsed", "");
                        card_hour.Attributes["aria-expanded"] = card_hour.Attributes["aria-expanded"].Replace("false", "true");
                        collapse_bbdd_6.Attributes["class"] = collapse_bbdd_6.Attributes["class"].Insert(collapse_bbdd_6.Attributes["class"].Length, " show");
                    }
                    else
                        txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                }
            }
        }

        #endregion

        #region Acciones

        protected void btn_tipo_accion_Click(object sender, EventArgs e)
        {
            /// 0.- Abrir la pestaña
            card_actions.Attributes["class"] = card_actions.Attributes["class"].Replace("collapsed", "");
            card_actions.Attributes["aria-expanded"] = card_actions.Attributes["aria-expanded"].Replace("false", "true");
            collapse_bbdd_7.Attributes["class"] = collapse_bbdd_7.Attributes["class"].Insert(collapse_bbdd_7.Attributes["class"].Length, " show");

            /// 1.- Sacar el id de la regla
            long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
            if (id_rule > 0)
            {
                /// 2.- Borrar los errores
                txt_error.InnerHtml = string.Empty;

                /// 3.- Sacar los datos del formulario
                long tipo = !String.IsNullOrEmpty(ddl_lista_acciones.Value) ? int.Parse(ddl_lista_acciones.Value) : -1;

                /// 3.- Comprobar si hay errores
                if (tipo == -1)
                    txt_error.InnerHtml = "Todos los campos son obligatorios";
                else
                {
                    /// 3.- Recuperar los datos de la regla
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 4.- Pintar los ids de las acciones
                        List<long> lst_id_acciones = new List<long>();
                        lst_id_acciones.Add(tipo);

                        string sAccion = lst_rule[0].acciones;
                        if (!String.IsNullOrEmpty(sAccion))
                        {
                            List<string> lst_ids = sAccion.Split(',').ToList();
                            if (lst_ids.Count > 0)
                            {
                                foreach (var action in lst_ids)
                                {
                                    if (!String.IsNullOrEmpty(action) && !lst_id_acciones.Contains(long.Parse(action)))
                                        lst_id_acciones.Add(long.Parse(action));
                                }
                            }
                        }

                        string list_acciones = string.Empty;
                        foreach (var action in lst_id_acciones)
                        {
                            if (String.IsNullOrEmpty(list_acciones))
                                list_acciones = action.ToString();
                            else
                                list_acciones = list_acciones + "," + action.ToString();
                        }

                        if (!String.IsNullOrEmpty(list_acciones))
                        {
                            /// 3.- Actualizar la regla
                            campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                            regla.acciones = list_acciones;

                            bool update_rule = da.updateRuleAutomation(regla);
                            if (update_rule)
                                load_actions(id_rule, list_acciones);
                            else
                                txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                        }
                    }
                }
            }
        }

        protected void btnBorrarAccion_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_rule = false;

            try
            {
                /// 0.- Abrir la pestaña
                card_actions.Attributes["class"] = card_actions.Attributes["class"].Replace("collapsed", "");
                card_actions.Attributes["aria-expanded"] = card_actions.Attributes["aria-expanded"].Replace("false", "true");
                collapse_bbdd_7.Attributes["class"] = collapse_bbdd_7.Attributes["class"].Insert(collapse_bbdd_7.Attributes["class"].Length, " show");

                /// 1.- Sacar el id de la regla
                long id_rule = !String.IsNullOrEmpty(Request.QueryString["idr"]) ? long.Parse(Request.QueryString["idr"]) : -1;
                if (id_rule > 0)
                {
                    int tipo = !String.IsNullOrEmpty(hidIdAccion.Value) ? int.Parse(hidIdAccion.Value) : -1;
                    if (tipo > 0)
                    {
                        delete_rule_action(id_rule, tipo);

                        /// 2.- Recuperar los datos de la regla
                        List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                        if (lst_rule.Count == 1)
                        {
                            /// 3.- Pintar los ids de los cursos
                            List<long> lst_id_acciones = new List<long>();
                            string sAction = lst_rule[0].acciones;
                            if (!String.IsNullOrEmpty(sAction))
                            {
                                List<string> lst_ids = sAction.Split(',').ToList();
                                if (lst_ids.Count > 0)
                                {
                                    foreach (var action in lst_ids)
                                    {
                                        if (!String.IsNullOrEmpty(action) && !lst_id_acciones.Contains(long.Parse(action)))
                                            lst_id_acciones.Add(long.Parse(action));
                                    }
                                }
                            }

                            /// 4.- Eliminar el tipo
                            lst_id_acciones.Remove(tipo);

                            /// 5.- Pintar las acciones
                            string list_acciones = string.Empty;
                            foreach (var action in lst_id_acciones)
                            {
                                if (String.IsNullOrEmpty(list_acciones))
                                    list_acciones = action.ToString();
                                else
                                    list_acciones = list_acciones + "," + action.ToString();
                            }

                            if (!String.IsNullOrEmpty(list_acciones))
                            {
                                /// 3.- Actualizar la regla
                                campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                                regla.acciones = list_acciones;

                                bool update_rule = da.updateRuleAutomation(regla);
                                if (update_rule)
                                    load_actions(id_rule, list_acciones);
                                else
                                    txt_error.InnerHtml = "Se ha producido un error al actualizar la regla";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al eliminar la regla');</script>");

                LogUtils.InsertarLog(" ERROR - empresas.cs::btnBorrarRegla_Click()");
                LogUtils.InsertarLog("- MSG:" + ex.Message);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message));
            }

            /// Si no hay errores recargar la página
            if (delete_rule)
                Response.Redirect("lista-reglas.aspx");
        }

        private void delete_rule_action(long id_rule, int tipo)
        {
            List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_rule, tipo);
            if (lst_actions.Count == 1)
            {
                if (tipo == (int)Constantes.type_action_rule.Mail)
                {
                    /// 1.- Eliminar los ficheros adjuntos

                    if (!String.IsNullOrEmpty(lst_actions[0].adjuntos))
                    {
                        List<string> lst_adjuntos = lst_actions[0].adjuntos.Split(',').ToList();
                        if (lst_adjuntos.Count > 0)
                        {
                            foreach (var adjunto in lst_adjuntos)
                            {
                                File.SetAttributes(adjunto, FileAttributes.Normal);
                                File.Delete(adjunto);
                            }
                        }
                    }
                }

                /// 2.- Eliminar la regla
                da.deleteRuleAction(lst_actions[0].idReglaAccion);
            }
        }

        private void load_actions(long id_regla, string acciones)
        {
            /// 1.- Sacar los tipos de acción
            List<campus_REGLAS_TIPO_ACCION> lst_types = da.getRuleTypeActionById(-1);
            if (lst_types.Count > 0)
            {
                List<long> lst_id_tipos = new List<long>();
                if (!String.IsNullOrEmpty(acciones))
                {
                    List<string> lst_ids = acciones.Split(',').ToList();
                    if (lst_ids.Count > 0)
                    {
                        foreach (var accion in lst_ids)
                        {
                            if (!String.IsNullOrEmpty(accion) && !lst_id_tipos.Contains(long.Parse(accion)))
                                lst_id_tipos.Add(long.Parse(accion));
                        }
                    }
                }

                /// 2.- Pintar el combo de acciones
                List<campus_REGLAS_TIPO_ACCION> list_acciones = lst_types.Where(p => !lst_id_tipos.Contains(p.id_type)).ToList();
                load_actions_ddl(list_acciones);

                /// 3.- Pintar la tabla de acciones
                List<campus_REGLAS_TIPO_ACCION> list_acciones_sel = lst_types.Where(p => lst_id_tipos.Contains(p.id_type)).ToList();
                tabla_listado_tipos.InnerHtml = paint_table_acciones(list_acciones_sel, "table_listado_tipos", id_regla);
            }
        }

        private void load_actions_ddl(List<campus_REGLAS_TIPO_ACCION> list_acciones)
        {
            ddl_lista_acciones.DataSource = list_acciones;
            ddl_lista_acciones.DataTextField = "nombre";
            ddl_lista_acciones.DataValueField = "id_type";
            ddl_lista_acciones.DataBind();
            ddl_lista_acciones.Items.Add(new ListItem("Selecciona una acción", "-1"));
            ddl_lista_acciones.Value = "-1";
        }

        private string paint_table_acciones(List<campus_REGLAS_TIPO_ACCION> list_acciones, string table, long id_regla)
        {
            List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(id_regla, -1);
            
            StringBuilder sbuild = new StringBuilder();

            sbuild.Append("<table id=\"" + table + "\" class=\"display compact\" style =\"width:100%\">");
            sbuild.Append("<thead>");
            //Cabecera
            sbuild.Append("<tr>");            
            sbuild.Append("<th>Acciones</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            foreach (var accion in list_acciones)
            {
                sbuild.Append("<tr>");
                sbuild.Append("<td>" + accion.nombre + "</td>");

                List<campus_REGLAS_ACCIONES> lst_actions_filter = lst_actions.Where(ra => ra.idTipo == accion.id_type).ToList();
                if (lst_actions_filter.Count == 1)
                    sbuild.Append("<td><a href='regla-accion.aspx?idr=" + id_regla + "&idta=" + accion.id_type + "' title='Editar'><i class='fas fa-cogs text-color-green fa-1-6x'></i></a></td>");
                else
                    sbuild.Append("<td><a href='regla-accion.aspx?idr=" + id_regla + "&idta=" + accion.id_type + "' title='Editar'><i class='fas fa-cogs fa-1-6x'></i></a></td>");
                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la accion " + accion.nombre + "?\")){eliminarAccion(" + accion.id_type + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody>");
            sbuild.Append("</table>");

            return sbuild.ToString();
        }

        #endregion

        private void cargar_tipos()
        {
            /// 1.- Cargar los tipos
            List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(-1);
            if (lst_types.Count > 0)
            {
                ddl_lista_tipo_automatizacion.DataSource = lst_types;
                ddl_lista_tipo_automatizacion.DataTextField = "nombre";
                ddl_lista_tipo_automatizacion.DataValueField = "id_type";
                ddl_lista_tipo_automatizacion.DataBind();
                ddl_lista_tipo_automatizacion.Items.Add(new ListItem("Seleccione", "-1"));
                ddl_lista_tipo_automatizacion.Value = "-1";
            }
        }

        private void load_rule(long id_rule, CLIENTES user)
        {
            /// Cargar los datos de la regla
            List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
            if (lst_rule.Count == 1)
            {
                /// 1.- Desbloquear el bloque de la campaña
                block_all.Attributes["class"] = block_all.Attributes["class"].Replace("hidden", string.Empty);

                /// 2.- Ocultar el bloque de los botones de la campaña inicial
                block_save.Attributes["class"] = block_save.Attributes["class"].Insert(block_save.Attributes["class"].Length, " hidden");

                /// 3.- Cargar los datos de la regla inicial

                /// 3.1.- Tipo de automatización
                ddl_lista_tipo_automatizacion.Value = lst_rule[0].id_tipo.ToString();

                /// 3.2.- Orden
                txt_orden.Value = lst_rule[0].orden.ToString();

                /// 3.3.- Nombre
                txt_name_rule.Value = lst_rule[0].nombre;

                /// 3.4.- Descripción
                txt_descripcion.Value = lst_rule[0].descripcion;

                /// 4.- Cargar los estados
                load_status(lst_rule[0].estados);

                /// 5.- Cargar los origenes
                load_origins(lst_rule[0].origenes);

                /// 6.- Cargar los cursos
                load_courses(lst_rule[0].cursos);

                /// 7.- Cargar los paises
                load_paises(lst_rule[0].paises);

                /// 8.- Cargar las fechas
                chk_date_all.Checked = lst_rule[0].fecha_all != null ? lst_rule[0].fecha_all.Value : false;
                txt_fecha_inicio.Value = lst_rule[0].fecha_inicio != null ? lst_rule[0].fecha_inicio.Value.ToShortDateString() : string.Empty;
                txt_fecha_fin.Value = lst_rule[0].fecha_fin != null ? lst_rule[0].fecha_fin.Value.ToShortDateString() : string.Empty;

                /// 9.- Cargar las horas
                load_hours();
                load_minutes();
                chk_hour_all.Checked = lst_rule[0].hora_all != null ? lst_rule[0].hora_all.Value : false;
                hour_start.Value = !String.IsNullOrEmpty(lst_rule[0].hora_inicio) ? DateTime.Today.AddHours(int.Parse(lst_rule[0].hora_inicio.Split(':')[0])).Hour.ToString("00") : string.Empty;
                min_start.Value = !String.IsNullOrEmpty(lst_rule[0].hora_inicio) ? DateTime.Today.AddHours(int.Parse(lst_rule[0].hora_inicio.Split(':')[1])).Minute.ToString("00") : string.Empty;
                hour_end.Value = !String.IsNullOrEmpty(lst_rule[0].hora_fin) ? DateTime.Today.AddHours(int.Parse(lst_rule[0].hora_fin.Split(':')[0])).Hour.ToString("00") : string.Empty;
                min_end.Value = !String.IsNullOrEmpty(lst_rule[0].hora_fin) ? DateTime.Today.AddHours(int.Parse(lst_rule[0].hora_fin.Split(':')[1])).Minute.ToString("00") : string.Empty;

                /// 10.- Cargar las acciones
                load_actions(lst_rule[0].id_regla, lst_rule[0].acciones);
            }
        }
        
        private void load_hours()
        {
            /// 1.- Cargar las horas
            hour_start.Items.Clear();
            hour_end.Items.Clear();

            hour_start.Items.Add(new ListItem("Selecciona una hora", string.Empty));
            hour_end.Items.Add(new ListItem("Selecciona una hora", string.Empty));

            for (int hour = 0; hour < 24; hour++)
            {
                hour_start.Items.Add(new ListItem(hour.ToString("00"), hour.ToString("00")));
                hour_end.Items.Add(new ListItem(hour.ToString("00"), hour.ToString("00")));
            }
        }
        private void load_minutes()
        {
            /// 1.- Cargar las horas
            min_start.Items.Clear();
            min_end.Items.Clear();

            min_start.Items.Add(new ListItem("Selecciona minutos", string.Empty));
            min_end.Items.Add(new ListItem("Selecciona minutos", string.Empty));

            for (int min = 0; min < 60; min++)
            {
                min_start.Items.Add(new ListItem(min.ToString("00"), min.ToString("00")));
                min_end.Items.Add(new ListItem(min.ToString("00"), min.ToString("00")));
            }
        }
    }
}