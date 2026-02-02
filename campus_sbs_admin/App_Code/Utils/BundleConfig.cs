using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace campus_sbs_admin
{
    public class BundleConfig
    {
        public BundleConfig()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            /// JavaScripts
            
            bundles.Add(new ScriptBundle("~/bundles/general_admin_js").Include(
                            "~/App_Themes/support/js/jquery.min.js",
                            "~/App_Themes/support/js/popper.min.js",
                            "~/App_Themes/support/js/bootstrap.min.js",                            
                            "~/App_Themes/support/js/bootstrap-select.min.js",
                            "~/App_Themes/support/js/bootstrap-select-es.min.js",
                            "~/App_Themes/support/js/internal/functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/menu_nav_js").Include(
                            "~/App_Themes/support/js/valkyrie-nav.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables_js").Include(
                            "~/App_Themes/support/js/datatables.min.js",
                            "~/App_Themes/support/js/dataTables.dateFormat.js",
                            "~/App_Themes/support/js/dataTables.cleanString.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery_ui_js").Include(
                            "~/App_Themes/support/js/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker_js").Include(
                            "~/App_Themes/support/js/bootstrap-datepicker.min.js",
                            "~/App_Themes/support/js/bootstrap-datepicker.es.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap_bundle_js").Include(
                            "~/App_Themes/support/js/bootstrap.bundle.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/upload_js").Include(
                            "~/App_Themes/support/js/upload/jquery.ui.widget.js",
                            "~/App_Themes/support/js/upload/jquery.iframe-transport.js",
                            "~/App_Themes/support/js/upload/jquery.fileupload.js"));

            bundles.Add(new ScriptBundle("~/bundles/inf_matriculas_js").Include(
                            "~/App_Themes/support/js/internal/informe-matriculas-crm-functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/listado_leads_js").Include(
                            "~/App_Themes/support/js/internal/listado-leads-crm-funcions.js"));

            bundles.Add(new ScriptBundle("~/bundles/inf_leads_js").Include(
                            "~/App_Themes/support/js/internal/informe-leads-crm-functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/ficha_usuario_js").Include(
                            "~/App_Themes/support/js/internal/ficha-alumno-crm-functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/ficha_usuario_aux_js").Include(
                            "~/App_Themes/support/js/internal/ficha-alumno-crm-aux-functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/seguimiento_comercial_js").Include(
                            "~/App_Themes/support/js/internal/seguimiento-comercial-crm-functions.js"));

            /// Hojas de estilos

            bundles.Add(new StyleBundle("~/bundles/bootstrap_css").Include(
                            "~/App_Themes/support/css/bootstrap.min.css",
                            "~/App_Themes/support/css/bootstrap-select.min.css"));

            bundles.UseCdn = true;
            var cdn_path = "https://fonts.googleapis.com/css?family=Lato:300,400,700,900%7COpen+Sans:400,600,700";
            bundles.Add(new StyleBundle("~/bundles/fonts_css", cdn_path));

            bundles.Add(new StyleBundle("~/bundles/datatables_css").Include(
                            "~/App_Themes/support/css/datatables.min.css"));

            bundles.Add(new StyleBundle("~/bundles/jquery_ui_css").Include(
                            "~/App_Themes/support/css/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/bundles/datepicker_css").Include(
                            "~/App_Themes/support/css/bootstrap-datepicker.min.css",
                            "~/App_Themes/support/css/bootstrap-datepicker.standalone.min.css"));

            bundles.Add(new StyleBundle("~/bundles/calendar_css").Include(
                            "~/App_Themes/support/css/fullcalendar.css"));

            bundles.Add(new StyleBundle("~/bundles/general_admin_css").Include(
                            "~/App_Themes/support/css/crm.css",
                            "~/App_Themes/support/css/fonts.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}