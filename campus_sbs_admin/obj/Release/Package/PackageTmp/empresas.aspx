<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="empresas.aspx.cs" Inherits="campus_sbs_admin.empresas" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Empresas</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />

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
                            <legend id="txt_empresas" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_empresas" class="col-sm-12" runat="server"></div>

                    <!-- Hidden y botones ocultos -->
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <asp:HiddenField ID="hidIdEmpresa" runat="server" />
                        <asp:ImageButton ID="btnBorrarEmpresa" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" OnClick="btnBorrarEmpresa_Click" CssClass="hidden" />
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
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.dateFormat.js"></script>

    <script type="text/javascript">    
        $(document).ready(function () {
            $('#tabla_Empresas').DataTable({
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
                lengthMenu: [[50, 20, -1], [50, 20, "Todos"]],
                "columnDefs": [
                  {
                      "targets": [0],
                      "class": "text-center",
                      "type": "eu_date"
                  },
                  /*{
                      "targets": [1],
                      "class": "text-center",
                      "type": "eu_date"
                  },*/
                  {
                      "targets": [1],
                      "class": "text-center"
                  },
                  {
                      "targets": [2]
                  },
                  {
                      "targets": [3]
                  },
                  {
                      "targets": [4]
                  },
                  {
                      "targets": [5],
                      "class": "text-center",
                      "type": "eu_date"
                  },
                  {
                      "targets": [6],
                      "class": "text-center"
                  },
                  {
                      "targets": [7],
                      "class": "text-center"
                  },
                  {
                      "targets": [8],
                      "class": "text-center"
                  },
                  {
                      "targets": [9],
                      "class": "text-center"
                  },
                  {
                      "targets": [10],
                      "class": "text-center"
                  },
                  {
                      "targets": [11],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "desc"]]
            });
        });

        function eliminarEmpresa(id) {
            var hidId = document.getElementById('<%=hidIdEmpresa.ClientID %>');
            hidId.value = id;

            var boton = document.getElementById('<%=btnBorrarEmpresa.ClientID %>');
            boton.click();
        }
    </script>
</body>
</html>
