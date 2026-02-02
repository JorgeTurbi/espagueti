using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class lista_reglas : System.Web.UI.Page
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
                    loadRules();
            }
        }

        private void loadRules()
        {
            /// 1.- Sacar datos de la BBDD
            List<campus_TIPO_AUTOMATIZACION> lst_types = da.getTypeAutomation(-1);
            List<campus_REGLAS_AUTOMATIZACION> lst_reglas = da.getRuleAutomationById(-1);
            List<campus_REGLAS_ACCIONES> lst_actions = da.getRulesActions(-1, -1);
            List<campus_AUX> lst_aux = da.getAuxForId(-1);
            List<Paises> lst_paises = da.getCountries();
            List<campus_CURSO> lst_courses = da.getCourses(null);

            /// 2.- Pintar la tabla
            StringBuilder sbuild = new StringBuilder();

            /// 2.1.- Inicio tabla
            sbuild.Append("<table id =\"tabla_Reglas\" class=\"display compact\" style =\"width:100%\"><thead>");

            /// 2.2.- Cabecera
            sbuild.Append("<tr>");
            sbuild.Append("<th>Tipo</th>");
            sbuild.Append("<th>Orden</th>");
            sbuild.Append("<th>Nombre</th>");
            sbuild.Append("<th>Fecha</th>");
            sbuild.Append("<th>Hora</th>");
            sbuild.Append("<th>Estados</th>");
            sbuild.Append("<th>Origen</th>");
            sbuild.Append("<th>Programa</th>");
            sbuild.Append("<th>País</th>");
            sbuild.Append("<th>Acciones</th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("<th></th>");
            sbuild.Append("</tr>");
            sbuild.Append("<tbody>");

            /// 2.3.- Pintar las reglas
            foreach (var regla in lst_reglas)
            {
                sbuild.Append("<tr>");

                /// 2.3.0.- Tipo de regla
                sbuild.Append("<td>" + lst_types.Where(t => t.id_type == regla.id_tipo).Select(t => t.nombre).FirstOrDefault() + "</td>");

                /// 2.3.1.- Orden
                sbuild.Append("<td>" + regla.orden + "</td>");

                /// 2.3.2.- Nombre
                sbuild.Append("<td>" + regla.nombre + "</td>");

                /// 2.3.3.- Fechas
                if ((regla.fecha_all != null && regla.fecha_all.Value))
                    sbuild.Append("<td><i class='far fa-calendar-alt text-color-green fa-1-6x'></i></td>");
                else if ((regla.fecha_all == null || !regla.fecha_all.Value) && regla.fecha_inicio != null && regla.fecha_fin != null)
                    sbuild.Append("<td>" + regla.fecha_inicio.Value.ToShortDateString() + " - " + regla.fecha_fin.Value.ToShortDateString() + "</td>");
                else
                    sbuild.Append("<td></td>");

                /// 2.3.4.- Horas
                if (regla.hora_all != null && regla.hora_all.Value)
                    sbuild.Append("<td><i class='far fa-calendar-alt text-color-green fa-1-6x'></i></td>");
                else if ((regla.hora_all == null || !regla.hora_all.Value) && !String.IsNullOrEmpty(regla.hora_inicio) && !String.IsNullOrEmpty(regla.hora_fin))
                    sbuild.Append("<td>" + regla.hora_inicio + " - " + regla.hora_fin + "</td>");
                else
                    sbuild.Append("<td></td>");

                /// 2.3.5.- Estados
                if (!String.IsNullOrEmpty(regla.estados))
                {
                    string lst_estados = string.Empty; 

                    List<long> lst_id = new List<long>();
                    List<string> lst_st_id = regla.estados.Split(',').ToList();
                    foreach (var id in lst_st_id)
                    {
                        if (!String.IsNullOrEmpty(id) && !lst_id.Contains(long.Parse(id)))
                            lst_id.Add(long.Parse(id));
                    }

                    List<Estados_Lead> lst_status = getStatus();
                    lst_status = lst_status.Where(s => lst_id.Contains(s.id_estado)).ToList();
                    if (lst_status.Count > 0)
                    {                        
                        foreach (var _status in lst_status)
                        {
                            if (String.IsNullOrEmpty(lst_estados))
                                lst_estados = _status.nombre;
                            else
                                lst_estados = lst_estados + "," + _status.nombre;
                        }                        
                    }

                    sbuild.Append("<td>" + lst_estados + "</td>");
                }
                else
                    sbuild.Append("<td></td>");

                /// 2.3.6.- Origenes
                if (!String.IsNullOrEmpty(regla.origenes))
                {
                    string lst_origenes = string.Empty;

                    List<long> lst_id = new List<long>();
                    List<string> lst_st_id = regla.origenes.Split(',').ToList();
                    foreach (var id in lst_st_id)
                    {
                        if (!String.IsNullOrEmpty(id) && !lst_id.Contains(long.Parse(id)))
                            lst_id.Add(long.Parse(id));
                    }

                    List<campus_AUX> lst_origins = lst_aux.Where(s => lst_id.Contains(s.ID_Aux)).ToList();
                    if (lst_origins.Count > 0)
                    {
                        foreach (var origin in lst_origins)
                        {
                            if (String.IsNullOrEmpty(lst_origenes))
                                lst_origenes = origin.Nombre;
                            else
                                lst_origenes = lst_origenes + "," + origin.Nombre;
                        }
                    }

                    sbuild.Append("<td>" + lst_origenes + "</td>");
                }
                else
                    sbuild.Append("<td></td>");

                /// 2.3.7.- Cursos
                if (!String.IsNullOrEmpty(regla.cursos))
                {
                    string lst_cursos = string.Empty;

                    List<long> lst_id = new List<long>();
                    List<string> lst_st_id = regla.cursos.Split(',').ToList();
                    foreach (var id in lst_st_id)
                    {
                        if (!String.IsNullOrEmpty(id) && !lst_id.Contains(long.Parse(id)))
                            lst_id.Add(long.Parse(id));
                    }

                    List<campus_CURSO> lst_courses_filter = lst_courses.Where(c => lst_id.Contains(c.ID_Curso)).ToList();
                    if (lst_courses_filter.Count > 0)
                    {
                        foreach (var course in lst_courses_filter)
                        {
                            if (String.IsNullOrEmpty(lst_cursos))
                                lst_cursos = course.Nombre;
                            else
                                lst_cursos = lst_cursos + "," + course.Nombre;
                        }
                    }

                    sbuild.Append("<td>" + lst_cursos + "</td>");
                }
                else
                    sbuild.Append("<td></td>");

                /// 2.3.8.- Cursos
                if (!String.IsNullOrEmpty(regla.paises))
                {
                    string lst_countries = string.Empty;

                    List<long> lst_id = new List<long>();
                    List<string> lst_st_id = regla.paises.Split(',').ToList();
                    foreach (var id in lst_st_id)
                    {
                        if (!String.IsNullOrEmpty(id) && !lst_id.Contains(long.Parse(id)))
                            lst_id.Add(long.Parse(id));
                    }

                    List<Paises> lst_paises_filter = lst_paises.Where(p => lst_id.Contains(p.id_pais)).ToList();
                    if (lst_paises_filter.Count > 0)
                    {
                        foreach (var country in lst_paises_filter)
                        {
                            if (String.IsNullOrEmpty(lst_countries))
                                lst_countries = "<img alt='" + country.nombre + "' title='" + country.nombre + "' src='/App_Themes/support/img/flags/" + country.descripcion + ".png' style='width: 32px;' />";
                            else
                                lst_countries = lst_countries + "<img alt='" + country.nombre + "' title='" + country.nombre + "' src='/App_Themes/support/img/flags/" + country.descripcion + ".png' style='width: 32px;' />";
                        }
                    }

                    sbuild.Append("<td>" + lst_countries + "</td>");
                }
                else
                    sbuild.Append("<td></td>");

                /// 2.3.9.- Acciones
                if (!String.IsNullOrEmpty(regla.acciones))
                {
                    List<campus_REGLAS_ACCIONES> lst_actions_filter = lst_actions.Where(ra => ra.idRegla == regla.id_regla).OrderBy(ra => ra.idTipo).ToList();
                    if (lst_actions_filter.Count > 0)
                    {
                        string lst_acciones = string.Empty;

                        foreach (var action in lst_actions_filter)
                        {
                            if (action.idTipo == (long)Constantes.type_action_rule.Comun)
                            {
                                if (String.IsNullOrEmpty(lst_acciones))
                                    lst_acciones = "<i class='fas fa-cogs text-color-green fa-1-6x pointer' title='Común'></i>";
                                else
                                    lst_acciones = lst_acciones + " " + "<i class='fas fa-cogs text-color-green fa-1-6x pointer' title='Común'></i>";
                            }
                            else if (action.idTipo == (long)Constantes.type_action_rule.Reasignar)
                            {
                                if (String.IsNullOrEmpty(lst_acciones))
                                    lst_acciones = "<i class='fas fa-user-cog text-color-green fa-1-6x pointer' title='Reasignar comercial'></i>";
                                else
                                    lst_acciones = lst_acciones + " " + "<i class='fas fa-user-cog text-color-green fa-1-6x pointer' title='Reasignar comercial'></i>";
                            }
                            else if (action.idTipo == (long)Constantes.type_action_rule.Mail)
                            {
                                if (String.IsNullOrEmpty(lst_acciones))
                                    lst_acciones = "<i class='fas fa-envelope text-color-green fa-1-6x pointer' title='Enviar mail'></i>";
                                else
                                    lst_acciones = lst_acciones + " " + "<i class='fas fa-envelope text-color-green fa-1-6x pointer' title='Enviar mail'></i>";
                            }
                            else if(action.idTipo == (long)Constantes.type_action_rule.Seguimiento)
                            {
                                if (String.IsNullOrEmpty(lst_acciones))
                                    lst_acciones = "<i class='fas fa-cog text-color-green fa-1-6x pointer' title='Añadir seguimiento'></i>";
                                else
                                    lst_acciones = lst_acciones + " " + "<i class='fas fa-cog text-color-green fa-1-6x pointer' title='Añadir seguimiento'></i>";
                            }
                        }

                        sbuild.Append("<td>" + lst_acciones + "</td>");
                    }
                    else
                        sbuild.Append("<td></td>");
                }
                else
                    sbuild.Append("<td></td>");
                
                sbuild.Append("<td><a href='regla-mantenimiento.aspx?idr=" + regla.id_regla + "' title='Editar'><i class='fas fa-edit fa-1-6x'></i></a></td>");
                if (regla.activo)
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea desactivar la regla: " + regla.id_regla + "?\")){activarRegla(" + regla.id_regla + ")}'><i class=\"fas fa-power-off text-color-green fa-1-6x\" style=\"cursor: pointer\" title=\"Desactivar regla.\"></i></a></td>");
                else
                    sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea activar la regla: " + regla.id_regla + "?\")){activarRegla(" + regla.id_regla + ")}'><i class=\"fas fa-power-off text-color-red fa-1-6x\" style=\"cursor: pointer\" title=\"Activar regla.\"></i></a></td>");

                sbuild.Append("<td><a href='javascript:void(0)' onclick='if(confirm_message(\"¿Desea eliminar la regla " + regla.id_regla + "?\")){eliminarRegla(" + regla.id_regla + ")}'><i class=\"fas fa-trash-alt fa-1-6x\"></i></a></td>");
                sbuild.Append("</tr>");
            }

            sbuild.Append("</tbody></table>");

            /// 2.4.- Añadimos la tabla al div
            table_listado_reglas.InnerHtml = sbuild.ToString();

            /// 3.- Pintar el título
            txt_reglas.InnerHtml = "<i class='fas fa-pencil-ruler'></i> Listado de reglas de automatización <a href='regla-mantenimiento.aspx' title='Nueva regla' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle fa-2x'></i>  Nueva regla</small></a>";
        }

        protected void btnActivarRegla_Click(object sender, ImageClickEventArgs e)
        {
            /// 1.- Recuperar la regla
            long id_rule = !String.IsNullOrEmpty(hidIdRegla.Value) ? long.Parse(hidIdRegla.Value) : -1;
            if (id_rule > 0)
            {
                List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                if (lst_rule.Count == 1)
                {
                    bool process = true;

                    /// 2.- Comprobar si existe una regla con el mismo orden
                    List<campus_REGLAS_AUTOMATIZACION> lst_rules = da.getRuleAutomationByIdType(lst_rule[0].id_tipo);
                    if (lst_rules.Count > 0)
                    {
                        if (id_rule > 0)
                            lst_rules = lst_rules.Where(r => r.id_regla != id_rule).ToList();

                        int num_rules = lst_rules.Where(r => r.orden == lst_rule[0].orden && r.activo).Count();
                        if (num_rules == 1)
                            process = false;
                    }

                    if (process)
                    {
                        campus_REGLAS_AUTOMATIZACION regla = lst_rule[0];
                        if (lst_rule[0].activo)
                            regla.activo = false;
                        else
                            regla.activo = true;

                        bool update_rule = da.updateRuleAutomation(regla);
                        if (update_rule)
                            Response.Redirect("lista-reglas.aspx");
                        else
                            ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar la regla');</script>");
                    }
                    else
                        ClientScript.RegisterClientScriptBlock(Page.GetType(), string.Empty, "<script>alert('Se ha producido un error al activar/desactivar la regla porque ya existe una regla con ese orden en ese tipo');</script>");
                }
            }
        }

        protected void btnBorrarRegla_Click(object sender, ImageClickEventArgs e)
        {
            bool delete_rule = false;

            try
            {
                long id_rule = !String.IsNullOrEmpty(hidIdRegla.Value) ? long.Parse(hidIdRegla.Value) : -1;
                if (id_rule > 0)
                {
                    /// 1.- Sacar los datos de la BBDD
                    List<campus_REGLAS_AUTOMATIZACION> lst_rule = da.getRuleAutomationById(id_rule);
                    if (lst_rule.Count == 1)
                    {
                        /// 2.- Eliminar la regla 
                        delete_rule = da.deleteRuleAutomation(id_rule);
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
    }
}