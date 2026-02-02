<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="campus_sbs_admin.login" %>
<%@ Register TagPrefix="uc_cookie" TagName="cookie" Src="~/controls/control_cookie.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Login Campus</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />

     <style>
        body#login {display: flex; flex-direction: column; min-height: 100vh; height:100vh;}
        body#login main#container {display: flex; flex: 1 0 auto;}
        section { width:100%;}
        .middle {position: relative; top: 50%; transform: translateY(-50%);}
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
<body id="login" class="bg-color-primary">
    <header id="header" class="bg-color-primary">
        <div class="fluid-container hidden-xs">
            <div class="margin-t-5"></div>
        </div>
        <div class="container">			        		
            <div class="row">
                <div class="col-sm-6 col-xs-6">
                    <img src="App_Themes/support/img/logo-sbs.png" alt="logo-sbs" class="logo" />
			    </div>                
			</div><!-- / row -->
        </div>
        <div class="fluid-container">
		    <div class="bg-secundary padding-t-10"></div>
		</div>
    </header> 
    		
	<main id="container" class="wrapper public bg-color-white no-padding" role="main">	    
        <section class="bg-cover bg-cover-top-xs js-same relative block-primary padding-tb-50" style="background-image: url('/App_Themes/support/img/pantallas/fondo_login.jpg');">
            <div class="container middle">
                <div class="row">
                    <div class="col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3 bg-color-white-opacity-m padding20">
                        <form id="Form1" class="bg-color-white padding10" accept-charset="utf-8" runat="server">
                            <div id="block_error" class="form-group has-error" runat="server">
                                <span id="txt_error" class="help-block text-center" runat="server"></span>
                            </div>                                
                            <fieldset>
							    <legend class="text-color-primary">BIENVENIDO AL CAMPUS</legend>
                                <label>Usuario</label>
                                <div id="user_login" class="form-group" runat="server">
								    <label class="sr-only" for="login_user">Usuario</label>
									<input type="text" placeholder="Usuario" id="login_user" autocomplete="off" class="form-control" required="" runat="server" />
									<span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									<span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
									<div class="help-block with-errors">Usuario no válido</div>	
								</div>	
                                <label>Contraseña</label>
								<div id="user_password" class="form-group" runat="server">
								    <label class="sr-only" for="password_user">Contraseña</label>
									<input type="password" placeholder="Contraseña" id="password_user" autocomplete="off" class="form-control" required="" runat="server" />
									<span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									<span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
									<div class="help-block with-errors">Contraseña no válida</div>	
								</div>															
								<a href="remember-password.aspx" id="forget_password">¿Has olvidado tu contraseña?</a>	
                            </fieldset>
                            <fieldset>
                                <asp:Button ID="btn_Acceder" CssClass="btn pull-right btn-primary btn-block-xs" Text="Acceder" 
                                    runat="server" onclick="btn_Acceder_Click" />		    
                            </fieldset>
                        </form>
                    </div>
				</div>
                <div id="url_see_more" class="row" runat="server"></div>
			</div>
        </section>   

        <!--//BLOQUE COOKIES-->
        <uc_cookie:cookie ID="cookie_control" runat="server" />
        <!--//FIN BLOQUE COOKIES-->
    </main>	
		
    <footer class="hide-canvas-in bg-primary padding-tb-20">
        <div class="container">
            <div class="row">		
                <div class="col-md-12">
			        <div class="row">							
				        <div class="col-sm-6 col-sm-offset-3">
                            <label class="h3 display-block text-color-secondary text-center">
                                #DileHolaALaOportunidad
                            </label>
            			</div>
                    </div>
                </div>
		        <div class="col-md-12">
			        <div class="row">							
				        <div class="col-md-3 col-md-offset-2 col-sm-4">
					        <label class="display-block text-right text-center-xs">
                                <span class="h4 text-color-white bold">Spain Business School</span>
					            c/ Antonio Toledano 7<br />
					            28028 Madrid<br />
					            Copyright 2019
					        </label>	
				        </div>
                        <div class="col-md-2 col-sm-4 text-center">
                            <span class="sprite-social sprite-social-icon-phone margin-t-5"></span>

                            <label class="display-block text-center margin-t-20">
                                +34 91 719 10 00
                            </label>
                        </div>
                        <div class="col-md-2 col-sm-3 text-center no-padding">
                            <nav class="display-inline-block" id="social-links">
					            <ul>
                                    <li><a href="https://blog.spainbs.com/" title="Blog" target="_blank"><span class="sprite-social sprite-social-icon-book">Blog</span></a></li>
                                    <li><a href="https://twitter.com/spainbs" title="Síguenos en Twitter" target="_blank"><span class="sprite-social sprite-social-icon-twitter">Síguenos en Twitter</span></a></li>
                                    <li><a href="https://www.linkedin.com/company/spain-business-school/" title="Síguenos en Linkedin" target="_blank"><span class="sprite-social sprite-social-icon-linkedin">Síguenos en Linkedin</span></a></li>
                                    <li><a href="https://www.facebook.com/SpainBusinessSchool" title="Síguenos en Facebook" target="_blank"><span class="sprite-social sprite-social-icon-facebook">Síguenos en Facebook</span></a></li>
							        <li><a href="https://www.youtube.com/user/spainbs" title="Síguenos en YouTube" target="_blank"><span class="sprite-social sprite-social-icon-youtube">Síguenos en YouTube</span></a></li>
                                    <li><a href="https://www.instagram.com/spainbs_edu/" title="Síguenos en Instagram" target="_blank"><span class="sprite-social sprite-social-icon-instagram">Síguenos en Instagram</span></a></li>
				                </ul>
					        </nav>
                        </div>
			        </div>
		        </div>
	        </div>		
        </div>
    </footer> 

    <!-- Scripts
     =================================================== --> 
     <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>  
     <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>          

    <!-- Bloque Cookies
     =================================================== -->
     <script type="text/javascript" src="/App_Themes/support/js/internal/cookie_privacy.js"></script>
</body>
</html>
