<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-leads-crm.aspx.cs" Inherits="campus_sbs_admin.informe_leads_crm" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header-crm.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Informe de leads</title>

    <!-- CSS 
    =================================================== -->	    
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>        
        <%: Styles.Render("~/bundles/jquery_ui_css") %>
        <%: Styles.Render("~/bundles/calendar_css") %>
        <%: Styles.Render("~/bundles/datatables_css") %>
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
            <div class="col pt-2">
                <fieldset>
                    <legend class="text-color-primary" runat="server"><i class="fas fa-search"></i> Búsqueda <a href='regla-mantenimiento.aspx' title='Nueva regla' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle'></i>  Nuevo</small></a><a href='informe-matriculas-crm.aspx' title='Informe de matriculas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-tasks'></i> Inf. Matriculas</small></a><a href='listado-leads-crm.aspx' title='Peticiones de información' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-list'></i> Leads</small></a></legend>                    
                </fieldset>
                <div class="col-6">
                    <div class="col-8 px-0">
                        <label>Buscar usuario Id</label>
				        <div class="form-group">
                            <label class="sr-only" for="text_user">Buscar usuario Id</label>
				            <input type="text" placeholder="Buscar usuario Id" id="text_user" class="form-control" runat="server" />	
				        </div>
				    </div>
				    <div class="col-4">
                        <label>&nbsp;</label>
                        <div class="form-group">
				            <a href="javascript:void(0);" onclick="search_user()" title="Buscar"><i class="fas fa-search fa-1-6x pt-2"></i></a>
                        </div>
				    </div>
                </div>
                <div class="col-6">
                    <label>Buscar usuario</label>
					<div class="form-group">
						<label class="sr-only" for="txt_search">Buscar usuario</label>
						<input type="text" placeholder="Buscar usuario" id="txt_search" autocomplete="off" class="form-control" runat="server" />
					</div>
                </div>
            </div>
            <div class="col-sm-6">
                <fieldset>
                    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Control de leads por día</legend>
                </fieldset>
                <div id="lnk_day" class="col-sm-12 margin-b-15" runat="server"></div>
                <div id="table_listado_leads_day" class="col-sm-12 margin-b-15" runat="server"></div>
                <input id="hid_day" type="hidden" value="" runat="server" />
                <input id="hid_day_month" type="hidden" value="" runat="server" />
                <input id="hid_day_year" type="hidden" value="" runat="server" />        
            </div>
            <div class="col-sm-6">
                <fieldset>
                    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Control de leads por mes</legend>
                </fieldset>
                <div id="lnk_month" class="col-sm-12 margin-b-15" runat="server"></div>
                <div id="table_listado_leads_month" class="col-sm-12 margin-b-15" runat="server"></div>
                <input id="hid_month" type="hidden" value="" runat="server" />
                <input id="hid_year" type="hidden" value="" runat="server" />
            </div>
        </div>        
    </section>

    <!-- Modal -->
    <div class="modal fade" id="wait_modal" tabindex="-1" role="dialog" aria-labelledby="wait_modal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-body text-center">
                    <i class="fas fa-spinner fa-pulse fa-5x"></i>
                </div>
            </div>
        </div>
    </div>

    <!-- Scripts
    =================================================== --> 
    <asp:PlaceHolder runat="server">        
        <%: Scripts.Render("~/bundles/general_admin_js") %>
        <%: Scripts.Render("~/bundles/jquery_ui_js") %>
        <%: Scripts.Render("~/bundles/menu_nav_js") %>
        <%: Scripts.Render("~/bundles/datatables_js") %>
        <%: Scripts.Render("~/bundles/inf_leads_js") %>         
    </asp:PlaceHolder>



<%--    <script type="text/javascript" src="/App_Themes/support/js/jquery.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/popper.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.dateFormat.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.cleanString.js"></script>
      
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>    
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/informe-leads-crm-functions.js"></script>--%>
</body>
</html>