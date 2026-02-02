<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="preview-mail.aspx.cs" Inherits="campus_sbs_admin.preview_mail" %>

<!DOCTYPE html>

<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Preview mail</title>
     <!-- Scripts
    =================================================== --> 
    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                url: 'preview-mail.aspx/getData',
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d.length == 1) {
                        var _html = data.d[0];
                        var _body = getFileData();
                        var html_all = _html.replace("###BODY###", _body);
                        $('#block_mail').html(html_all);
                        deleteFileData();
                    }
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        });
	    
	    function getFileData() {
            var file_data = window.sessionStorage.getItem("file_data");
            if ((file_data == "undefined") || (file_data == undefined) || (file_data == "null") || (file_data == null))
                file_data = "";
            return file_data;
        }
	    function deleteFileData() {
	        window.sessionStorage.removeItem("file_data");
	    }
	 </script>

     <!-- HTML5 IE8 -->
		<!--[if lt IE 9]>
			<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js" async></script>
			<script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js" async></script>
		<![endif]-->
	<!-- /HTML5 IE8 -->
</head>
<body>
    <form id="form1" runat="server">
        <div id="block_mail" runat="server"></div>
    </form>
</body>
</html>
