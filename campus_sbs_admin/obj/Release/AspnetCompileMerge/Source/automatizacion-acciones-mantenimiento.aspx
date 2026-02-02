<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="automatizacion-acciones-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.automatizacion_acciones_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Acciones del tipo de automatización</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <style type="text/css">input[type=checkbox] {-webkit-appearance: checkbox}</style>

    <!-- CSS para el control DataTable -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />
	    
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
        <section class="padding-tb-40 padding-xs-tb-30">
            <div class="block-primary">
                <div class="row no-margin padding-nav">
                    <div class="col-sm-12">
                        <fieldset>
                            <legend id="txt_lista_tipos" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="block_error" class="col-sm-12 form-group has-error" runat="server">
                        <span id="txt_error" class="help-block text-center" runat="server"></span>
                    </div>
                    <div id="table_listado_tipos" class="col-sm-12 margin-b-30" runat="server"></div>

                    <!-- Hidden y botones ocultos -->
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div class="col-sm-12">
                            <asp:HiddenField ID="hidAccion" runat="server" />
                            <a id="lnk_back" href="#" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>
                            <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                Text="Asociar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                            <%--<asp:Button ID="btn_guardar_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15 hidden" runat="server"
                                Text=">>" ToolTip="Añadir todos los acciones" OnClick="btn_guardar_all_Click" />--%>
                        </div>
                    </form>
                    <!-- Fin hidden y botones ocultos -->
                </div>
            </div>
        </section>
    </main>

   <!-- Scripts
    =================================================== --> 
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>

    <script type="text/javascript">    
        $(document).ready(function () {
            $('#tabla_Listados').DataTable({
                responsive: true,
                language: {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"

                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                lengthMenu: [[10, 25, -1], [10, 25, "All"]],
                "columnDefs": [
                  {
                      "targets": [0],
                      "class": "text-center"
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  }
                ],
                "order": [[1, "asc"]]
            });
        });

        function chk_mark(control) {
            var cadena = $('#hidAccion').val();
            var checked = control.checked;
            var valor = control.value + ",";

            if (!checked) {
                if (cadena !== "") {
                    cadena = cadena.replace(valor, "");
                }
            }
            else {
                cadena = cadena + valor;
            }

            $('#hidAccion').val(cadena);
        }

    </script>
</body>
</html>