<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rrss-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.rrss_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Link RRSS mantenimiento</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     	    
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
							    <legend class="text-color-primary"><i class='fas fa-rss'></i> Manteniento de link RRSS</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-12">
                                    <label>Título *</label>
                                    <div id="title_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_title">Título</label>
									    <input type="text" placeholder="Título" id="txt_title" class="form-control" runat="server" maxlength="250" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label>Url *</label>
                                    <div id="url_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_url">Url</label>
									    <input type="text" placeholder="Url" id="txt_url" class="form-control" runat="server" maxlength="250" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Orden *</label>
                                    <div id="orden_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_orden">Orden</label>
									    <input type="text" placeholder="Orden" id="txt_orden" class="form-control" runat="server" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4 col-sm-offset-1">
                                    <label>Activo</label>
                                    <div id="activo_form" class="form-group padding-t-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-12">
                                    <a href="comunidad-social.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
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

    <script type="text/javascript">
        function validarFormulario() {
            /// 1.- Sacar los parametros
            var titulo = $('#txt_title').val();
            var url = $('#txt_url').val();
            var orden = $('#txt_orden').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (titulo === "undefined" || titulo === undefined || titulo === "null" || titulo === null || titulo === '') {
                $('#title_form').addClass(' has-error');
                $('#txt_error').html('El campo título es obligatorio');
                $('#txt_title').attr("placeholder", "El campo título es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (url === "undefined" || url === undefined || url === "null" || url === null || url === '') {
                $('#url_form').addClass(' has-error');
                $('#txt_error').html('El campo url es obligatorio');
                $('#txt_url').attr("placeholder", "El campo url es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (orden === "undefined" || orden === undefined || orden === "null" || orden === null || orden === '' || !validarTelefono(orden)) {
                $('#orden_form').addClass(' has-error');
                $('#txt_error').html('El campo orden es un campo numérico');
                $('#txt_orden').attr("placeholder", "El campo orden es obligatorio");
                subirArribaPagina();
                return false;
            }            
            else
                return true;
        }
    </script>
</body>
</html>
