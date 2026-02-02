<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="email-click.aspx.cs" Inherits="campus_sbs_admin.email_click" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>SBS | Email click</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
	 
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
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btn_redireccionar" CssClass="hidden" runat="server" OnClick="btn_redireccionar_Click" />
        </div>
    </form>

    <!-- Scripts
    =================================================== --> 
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript">
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para geolocalizar al usuario --------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        $(function () {
            $.getJSON("https://api.ipify.org?format=jsonp&callback=?", function (json) {
                getIp(json.ip);
            });
        });

        function getIp(ip) {
            if (ip != "undefined" && ip != undefined && ip != "null" && ip != null && ip != '') {
                var urlWS = "functions.aspx/getIp";
                var data = "{'ip' : '" + ip + "'}";
                var promise = callWebService(urlWS, data);

                promise.success(function (json) {
                    var boton = document.getElementById('<%=btn_redireccionar.ClientID %>');
                    boton.click();
                });

                promise.fail(function (json) {
                    console.log(json.d);
                });
            }
        }

        /// -----------------------------------------------------------------------------------------------------------------------------------
        /// Función para llamar al WS ---------------------------------------------------------------------------------------------------------
        /// -----------------------------------------------------------------------------------------------------------------------------------
        function callWebService(url, data) {
            var promise = $.ajax({
                type: "POST",
                crossDomain: true,
                url: '' + url + '',
                data: data,
                contentType: "application/json; charset=utf-8",
                cache: false,
                dataType: 'json'
            });

            return promise;
        }
    </script>
</body>
</html>
