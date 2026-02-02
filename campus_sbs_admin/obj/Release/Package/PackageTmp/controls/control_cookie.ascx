<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="control_cookie.ascx.cs" Inherits="campus_sbs_admin.controls.control_cookie" %>

<div id="sliding-popup">
    <div class="fluid-container inner"> 
        <div class="container padding-tb-10">    
            <div class="col-sm-10 col-xs-10">       
		        Desde <strong>Spain Business School</strong> te informamos que al navegar por nuestra página web se te instalarán temporalmente cookies que servirán para el buen funcionamiento de la web y nos ayudarán a personalizar nuestros servicios y a seguir mejorando. Si continúas navegando por nuestra página web, consideraremos que prestas tu consentimiento a la instalación de cookies. Puedes obtener más información en nuestra <a onclick="setCookieRedirect();">Política de Cookies</a>.
            </div>
            <div class="col-sm-2 col-xs-2">
                <a onclick="PonerCookie();" class="cdp-cookies-boton-cerrar"><i class="fa fa-times-circle fa-3x text-color-text-soft"></i></a>
            </div>
        </div>
	</div>
</div>