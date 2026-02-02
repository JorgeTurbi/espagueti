<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-servidor-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.informe_servidor_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Lista de suscriptores mantenimiento</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <style type="text/css">
        .input-group .form-control {background-color: white; border: 1px solid #bdbdbd; color: black;}
        .input-group.date.js-datepicker { width: 100%;}
        .input-group.has-error .form-control {background: #fbf2f1 none repeat scroll 0 0; border: 1px solid #a94442; color: #f2958d;}
        .input-group.has-error .form-control::-moz-placeholder {color: #f2958d; opacity: 1;}
        .checkbox img {height: 25px; width: 25px;}
        
        #txt_comentarios {max-height: 350px;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}       
     </style>
	    
	 <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700,900%7COpen+Sans:400,600,700" rel="stylesheet" type="text/css" />

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
    		
	<main class="wrapper public bg-color-white" role="main">     	    
        <section class="padding-tb-50">
		    <div class="row no-margin padding-nav">	
                <div class="col-sm-12">                         
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div>
                            <fieldset>
							    <legend class="text-color-primary"><i class='far fa-list-alt'></i> Manteniento de Lista de suscriptores</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-6">
                                    <label>Identificador</label>
								    <div id="identificador_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_identificador">Identificador</label>
									    <input type="text" placeholder="Identificador" id="txt_identificador" class="form-control" runat="server" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Prioritario</label>
                                    <div id="priority_form" class="form-group padding-tb-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkPriority" runat="server" Text="Prioritario" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Visible</label>
                                    <div id="visible_form" class="form-group padding-tb-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkVisible" runat="server" Text="Visible" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Límite día</label>											
								    <div id='limit_day_form' class="form-group">
							            <label class="sr-only" for="txt_limit_day">Límite día</label>
                                        <input type="text" placeholder="Límite día" id="txt_limit_day" class="form-control" runat="server" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
						            </div>   
                                </div>
                                <div class="col-sm-4">
                                    <label>Límite mes</label>											
								    <div id='limit_month_form' class="form-group">
							            <label class="sr-only" for="txt_limit_month">Límite mes</label>
                                        <input type="text" placeholder="Límite mes" id="txt_limit_month" class="form-control" runat="server" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
						            </div>   
                                </div>
                                <div class="col-sm-4">
                                    <label>Límite año</label>											
								    <div id='limit_year_form' class="form-group">
							            <label class="sr-only" for="txt_limit_year">Límite año</label>
                                        <input type="text" placeholder="Límite año" id="txt_limit_year" class="form-control" runat="server" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
						            </div>   
                                </div>
                                <div class="col-sm-6">
                                    <label>Día inicio mes</label>											
								    <div id='start_day_form' class="form-group">
							            <label class="sr-only" for="txt_start_day">Día inicio mes</label>
                                        <input type="text" placeholder="Día inicio mes" id="txt_start_day" class="form-control" runat="server" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
						            </div>   
                                </div>
                                <div class="col-sm-6">
                                    <label>Mes inicio año</label>
                                    <div id="start_month_form" class="form-group" runat="server">                                       
                                        <asp:DropDownList ID="ddlStartMonth" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                            <asp:ListItem Selected="True" Value="-1" Text="Seleccione mes"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Enero"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                            <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                                            <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                            <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <a id="btn_back" href="informe-servidor.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClick="btnGuardar_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>
                </div>							
			</div>
        </section>
    </main>

   <!-- Scripts
    =================================================== -->    
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>    
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
</body>
</html>