<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="remember-password.aspx.cs" Inherits="campus_sbs_admin.remember_password" %>
<%@ Register TagPrefix="uc_cookie" TagName="cookie" Src="~/controls/control_cookie.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Recordar contraseña Campus</title>

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
<body class="bg-color-primary">
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
    		
	<main class="wrapper public bg-color-white" role="main">	    
        <section class="padding-tb-50">
		    <div class="block-primary">
			    <div class="container">	
                    <div class="row">
                        <div class="col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3 bg-color-white-opacity-m padding20">
                            <form id="Form1" class="bg-color-white padding10" accept-charset="utf-8" runat="server">
                                <fieldset id="block_error" class="form-group has-error hidden" runat="server">
                                    <legend id="txt_error" class="help-block" runat="server"></legend>
                                </fieldset>                                
                                <fieldset>
							        <legend class="text-color-primary">RECORDAR CONTRASEÑA</legend>
                                    <label>Usuario / Mail</label>
                                    <div id="user_login" class="form-group" runat="server">
								        <label class="sr-only" for="login_user">Usuario / Mail</label>
									    <input type="text" placeholder="Usuario / Mail" id="login_user" autocomplete="off" class="form-control" required="" runat="server" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
									    <div class="help-block with-errors">Usuario / Mail no válido</div>	
								    </div>
                                </fieldset>
                                <fieldset>
                                    <a href="login.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                    <asp:Button ID="btn_forget" CssClass="btn pull-right btn-primary btn-block-xs" Text="Solicitar" 
                                        runat="server" onclick="btn_forget_Click" />		    
                                </fieldset>
                            </form>
                        </div>
					</div>	
                </div>							
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
