<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="listado-leads-crm.aspx.cs" Inherits="campus_sbs_admin.listado_leads_crm" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header-crm.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Listado de leads</title>

    <!-- CSS 
    =================================================== -->
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>        
        <%: Styles.Render("~/bundles/jquery_ui_css") %>
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
                    <legend class="text-color-primary" runat="server"><i class="fas fa-search"></i> Búsqueda <a href='ficha-alumno-crm.aspx' title='Nuevo usuario' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-user-plus'></i>  Nuevo</small></a><a href='informe-matriculas-crm.aspx' title='Informe de matriculas' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-tasks'></i> Inf. Matriculas</small></a><a href='informe-leads-crm.aspx' title='Informe de leads' class='pull-right bold padding-r-5'><small class='text-color-primary'><i class='fas fa-list'></i> Inf. Leads</small></a></legend>                    
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
                <fieldset>
                    <legend class="text-color-primary" runat="server"><i class="far fa-list-alt"></i> Lista de peticiones de información</legend>
                </fieldset>            
                <ul class="nav nav-tabs" id="myTab" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" id="nuevos-tab" data-toggle="tab" href="#nuevos" role="tab" aria-controls="nuevos" aria-selected="true">Nuevos <span id="count_nuevos" class="border-primary bold text-color-black" title="Leads" runat="server">0</span> <span id="count_nuevos_all" class="border-primary bold text-color-primary" title="Leads totales" runat="server">0</span></a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="avisos-tab" data-toggle="tab" href="#avisos" role="tab" aria-controls="avisos" aria-selected="true">Avisos <span id="count_avisos" class="border-primary bold text-color-black" runat="server">0</span></a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="sin-contactar-tab" data-toggle="tab" href="#sin-contactar" role="tab" aria-controls="sin-contactar" aria-selected="false">Sin contactar <span id="count_sin_contactar" class="border-primary bold text-color-black" runat="server">0</span></a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="proceso-tab" data-toggle="tab" href="#proceso" role="tab" aria-controls="proceso" aria-selected="false">Proceso <span id="count_proceso" class="border-primary bold text-color-black" runat="server">0</span></a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="futuro-tab" data-toggle="tab" href="#futuro" role="tab" aria-controls="futuro" aria-selected="false">Futuro <span id="count_futuro" class="border-primary bold text-color-black" runat="server">0</span></a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="todos-tab" data-toggle="tab" href="#todos" role="tab" aria-controls="todos" aria-selected="false">Todos <span id="count_todos" class="border-primary bold text-color-black" runat="server">0</span></a>
                    </li>
                </ul>
                <div class="tab-content" id="myTabContent">
                    <div class="tab-pane fade show active" id="nuevos" role="tabpanel" aria-labelledby="nuevos-tab">
                        <div id="table_list_leads" class="col-sm-12 padding-tb-20" runat="server">Nuevos</div>
                        <input id="hid_comercial" type="hidden" value="-1" runat="server" />
                    </div>
                    <div class="tab-pane fade" id="avisos" role="tabpanel" aria-labelledby="avisos-tab">
                        <div id="table_list_avisos" class="col-sm-12 padding-tb-20" runat="server">Avisos</div>
                        <input id="hid_comercial_avisos" type="hidden" value="-1" runat="server" />
                    </div>
                    <div class="tab-pane fade" id="sin-contactar" role="tabpanel" aria-labelledby="sin-contactar-tab">
                        <div class="col-sm-5 offset-sm-2 pt-2">
                            <label>Seleccionar comercial</label>
				            <div class="form-group">
                                <label class="sr-only" for="txt_search">Seleccionar comercial</label>
				                <select id="contact_person" class="form-control" runat="server" title="Comercial"></select>	
				            </div>
				        </div>
				        <div class="col-sm-2 pt-2">
                            <label>&nbsp;</label>
                            <div class="form-group">
				                <a href="javascript:void(0);" onclick="search_sin_contactar()" title="Buscar"><i class="fas fa-search fa-2x"></i></a>
                            </div>
				        </div>
                        <div id="table_list_sin_contactar" class="col-sm-12 padding-tb-20" runat="server">Sin contactar</div>
                    </div>
                    <div class="tab-pane fade" id="proceso" role="tabpanel" aria-labelledby="proceso-tab">
                        <div class="col-sm-5 offset-sm-2 pt-2">
                            <label>Seleccionar comercial</label>
				            <div class="form-group">
                                <label class="sr-only" for="txt_search">Seleccionar comercial</label>
				                <select id="process_person" class="form-control" runat="server" title="Comercial"></select>	
				            </div>
				        </div>
				        <div class="col-sm-2 pt-2">
                            <label>&nbsp;</label>
                            <div class="form-group">
				                <a href="javascript:void(0);" onclick="search_proceso()" title="Buscar"><i class="fas fa-search fa-2x"></i></a>
                            </div>
				        </div>
                        <div id="table_list_proceso" class="col-sm-12 padding-tb-20" runat="server">En proceso</div>
                    </div>
                    <div class="tab-pane fade" id="futuro" role="tabpanel" aria-labelledby="futuro-tab">
                        <div class="col-sm-5 offset-sm-2 pt-2">
                            <label>Seleccionar comercial</label>
				            <div class="form-group">
                                <label class="sr-only" for="txt_search">Seleccionar comercial</label>
				                <select id="future_person" class="form-control" runat="server" title="Comercial"></select>	
				            </div>
				        </div>
				        <div class="col-sm-2 pt-2">
                            <label>&nbsp;</label>
                            <div class="form-group">
				                <a href="javascript:void(0);" onclick="search_futuro()" title="Buscar"><i class="fas fa-search fa-2x"></i></a>
                            </div>
				        </div>                        
                        <div id="table_list_futuro" class="col-sm-12 padding-tb-20" runat="server">Futuro</div>
                    </div>
                    <div class="tab-pane fade" id="todos" role="tabpanel" aria-labelledby="todos-tab">
                        <div class="col-sm-3 offset-sm-2 pt-2">
                            <label>Seleccionar comercial</label>
				            <div class="form-group">
                                <label class="sr-only" for="txt_search">Seleccionar comercial</label>
				                <select id="all_person" class="form-control" runat="server" title="Comercial"></select>	
				            </div>
				        </div>
                        <div class="col-sm-3 pt-2">
                            <label>Seleccionar estado</label>
				            <div class="form-group">
                                <label class="sr-only" for="txt_search">Seleccionar estado</label>
				                <select id="all_status" class="form-control" runat="server" title="Estado"></select>	
				            </div>
				        </div>
				        <div class="col-sm-2 pt-2">
                            <label>&nbsp;</label>
                            <div class="form-group">
				                <a href="javascript:void(0);" onclick="search_todos()" title="Buscar"><i class="fas fa-search fa-2x"></i></a>
                            </div>
				        </div>   
                        
                        <div id="table_list_todos" class="col-sm-12 padding-tb-20" runat="server">Todos</div>
                    </div>
                </div>
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
        <%: Scripts.Render("~/bundles/bootstrap_bundle_js") %>
        <%: Scripts.Render("~/bundles/datatables_js") %>
        <%: Scripts.Render("~/bundles/listado_leads_js") %>         
    </asp:PlaceHolder>
</body>
</html>