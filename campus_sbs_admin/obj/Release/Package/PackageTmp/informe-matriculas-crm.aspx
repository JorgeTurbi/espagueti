<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-matriculas-crm.aspx.cs" Inherits="campus_sbs_admin.informe_matriculas_crm" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header-crm.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Informe de matrículas</title>

    <!-- CSS 
    =================================================== -->	  
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/datepicker_css") %>
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
                    <legend class="text-color-primary" runat="server"><i class="fas fa-search"></i> Búsqueda <a href='regla-mantenimiento.aspx' title='Nueva regla' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-plus-circle'></i>  Nuevo</small></a><a href='informe-leads-crm.aspx' title='Informe de leads' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-list'></i> Inf. Leads</small></a><a href='listado-leads-crm.aspx' title='Peticiones de información' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-list'></i> Leads</small></a></legend>                    
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
            <div class="col">
                <form id="Form1" accept-charset="utf-8" runat="server">
                    <fieldset>
                        <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de matrículas</legend>
                    </fieldset>
                    <div class="col">
                        <div id="block_error" class="form-group has-error" runat="server">
                            <span id="txt_error" class="help-block text-center" runat="server"></span>
                        </div>
                    </div>
                    <div class="col-sm-5">
                        <label>Tipo</label>
                        <div class="form-group padding-t-10">													
							<div class="radio">
                                <asp:RadioButtonList ID="radTipo" RepeatDirection="Horizontal" runat="server">
                                    <asp:ListItem Text="&nbsp;Por Edición" Value="E" />
                                    <asp:ListItem Text="&nbsp;Por Venta" Value="V" Selected="True" />
                                    <asp:ListItem Text="&nbsp;Todas" Value="T" />
                                </asp:RadioButtonList>
							</div>
						</div>
                    </div>
                    <div class="col-sm-3">
                        <label>Fecha Inicio</label>
						<div id="fechaAlta_form" class="input-group date" runat="server">
							<label class="sr-only" for="date_start">Fecha Inicio</label>
							<input type="text" id="date_start" class="form-control" runat="server" readonly="readonly" />
                            <span class="input-group-addon">
                                <i class="fas fa-th"></i>
                            </span>
						</div>
                    </div>
                    <div class="col-sm-3">
                        <label>Fecha Fin</label>
						<div id="fechaBaja_form" class="input-group date" runat="server">
							<label class="sr-only" for="date_end">Fecha Fin</label>
							<input type="text" id="date_end" class="form-control" runat="server" readonly="readonly" />
                            <span class="input-group-addon">
                                <i class="fas fa-th"></i>
                            </span>
						</div>
                    </div>
                    <div class="col-sm-1 text-center">
                        <label class="w-100">&nbsp;</label>
                        <asp:ImageButton ID="img_filter" ImageUrl="/App_Themes/support/img/icons/icon_search.png" runat="server" CssClass="padding5" ToolTip="Buscar" OnClick="img_filter_Click" />
                    </div>
                </form>
            </div>
            <div id="tabla_matriculas" class="col-sm-12 margin-b-15 padding-t-20" runat="server"></div>
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
        <%: Scripts.Render("~/bundles/datepicker_js") %>
        <%: Scripts.Render("~/bundles/datatables_js") %>
        <%: Scripts.Render("~/bundles/inf_matriculas_js") %>         
    </asp:PlaceHolder>
</body>
</html>