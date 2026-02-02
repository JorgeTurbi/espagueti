<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="peticion-info.aspx.cs" Inherits="campus_sbs_admin.peticion_info" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header-crm.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Ficha alumno</title>

    <!-- CSS 
    =================================================== -->	    
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/datepicker_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>
    </asp:PlaceHolder>

    <!-- Modernizr -->	
    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>

    <!-- HTML5 IE8 -->
		<!--[if lt IE 9]>
			<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js" async></script>
			<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js" async></script>
		<![endif]-->
	<!-- /HTML5 IE8 -->
</head>
<body>
    <uc_menu:menu ID="menu" runat="server" />
    <header id="header" class="bg-color-primary affix">        
        <uc_header:cabecera ID="cabecera" runat="server" />
    </header>

    <section class="wrapper">
        <div class="padding-nav">
            <div class="col pt-1">
                <form id="form1" runat="server">                    
                    <div class="col">
                        <fieldset>
                            <legend><i class='fas fa-info-circle'></i> Petición de información</legend>
                        </fieldset>
                    </div>
                    <div>
                        <div id="block_error" class="form-group has-error" runat="server">
                            <span id="txt_error" class="help-block text-center" runat="server"></span>
                        </div>
                        <div class="col-sm-6">
                            <label>Fecha Lead</label>
							<div id="fecha_lead_form" class="input-group date" runat="server">
								<label class="sr-only" for="txtFechaLead">Fecha Lead</label>
								<input type="text" id="txtFechaLead" class="form-control" runat="server" readonly="readonly" />
                                <span class="input-group-addon">
                                    <i class="fas fa-th"></i>
                                </span>
							</div>
                        </div>
                        <div class="col-sm-6">
                            <label>Origen</label>													
							<div id="origen_form" class="form-group" runat="server">
								<label class="sr-only" for="ddlOrigen">Origen</label>
                                <select id="ddlOrigen" class="selectpicker w-100" title="Seleccione un origen" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							</div>
                        </div>  
                        <div class="col-sm-12">
                            <label>Curso</label>											
							<div id="curso_form" class="form-group" runat="server">
								<label class="sr-only" for="ddlCurso">País de residencia</label>
								<select id="ddlCurso" class="selectpicker w-100" title="Seleccione un curso" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							</div>
                        </div>                        
                        <div class="col-sm-12">
                            <a id="btn_back" href="#" class="btn btn-primary btn-block-xs pull-left margin-xs-b-15" runat="server">Volver</a>
                            <a id="btn_save" href="javascript: void(0);" onclick="validarFormulario()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right">Guardar</a>
                                
                            <asp:Button ID="btnGuardar" CssClass="hidden" runat="server" OnClick="btnGuardar_Click" />
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </section>

    <!-- Scripts
    =================================================== --> 
    <asp:PlaceHolder runat="server">        
        <%: Scripts.Render("~/bundles/general_admin_js") %>
        <%: Scripts.Render("~/bundles/jquery_ui_js") %>
        <%: Scripts.Render("~/bundles/menu_nav_js") %>
        <%: Scripts.Render("~/bundles/datepicker_js") %>
    </asp:PlaceHolder>

    <script type="text/javascript">
        $(function () {
            /// 1.- Inicializar los datepicker
            $(".input-group.date").datepicker({
                language: "es",
                autoclose: true,
                todayHighlight: true
            });
        });
    </script>
</body>
</html>