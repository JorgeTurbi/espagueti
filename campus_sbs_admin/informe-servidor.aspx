<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-servidor.aspx.cs" Inherits="campus_sbs_admin.informe_servidor" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Informe de campaña</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <style type="text/css">.course-box::before {border-right: 0 none;}</style>

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
                            <legend id="txt_informe_servidor" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_servidores_activos" class="text-color-primary" runat="server">1.- SERVIDORES ACTIVOS</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_servidores_activos" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_historico_envios" class="text-color-primary" runat="server">2.- HISTÓRICO ENVÍOS</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_historico_envios" class="col-sm-12 margin-b-15" runat="server"></div>

                    <!-- Hidden y botones ocultos -->
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <asp:HiddenField ID="hidIdServer" runat="server" />
                        <asp:ImageButton ID="btnActivarServidor" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnActivarServidor_Click" />
                        <asp:ImageButton ID="btnDesactivarServidor" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnDesactivarServidor_Click"  />
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
            $('#tabla_Servidores').DataTable({
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
                lengthMenu: [[20, 50, -1], [20, 50, "All"]],
                "columnDefs": [
                  {
                      "targets": [0]
                  },
                  {
                      "targets": [1],
                      "class": "text-center"
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  },
                  {
                      "targets": [3],
                      "class": "text-center"
                  },
                  {
                      "targets": [4],
                      "class": "text-center"
                  },
                  {
                      "targets": [5],
                      "class": "text-center"
                  },
                  {
                      "targets": [6],
                      "class": "text-center"
                  },
                  {
                      "targets": [7],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Nº Hoy
                    hoy = api.column(1)
                        .data()
                        .reduce(function (a, b) {
                            return parseInt(a) + parseInt(b.toString().replace('.', ''));
                        }, 0);

                    /// Update footer
                    $(api.column(1).footer()).html(PonerPuntoMil(hoy));

                    /// Nº Ayer
                    ayer = api.column(2)
                        .data()
                        .reduce(function (a, b) {
                            return parseInt(a) + parseInt(b.toString().replace('.', ''));
                        }, 0);

                    /// Update footer
                    $(api.column(2).footer()).html(PonerPuntoMil(ayer));

                    /// Nº Semana
                    semana = api.column(3)
                        .data()
                        .reduce(function (a, b) {
                            return parseInt(a) + parseInt(b.toString().replace('.', ''));
                        }, 0);

                    /// Update footer
                    $(api.column(3).footer()).html(PonerPuntoMil(semana));

                    /// Nº Mes
                    mes = api.column(4)
                        .data()
                        .reduce(function (a, b) {
                            return parseInt(a) + parseInt(b.toString().replace('.', ''));
                        }, 0);

                    /// Update footer
                    $(api.column(4).footer()).html(PonerPuntoMil(mes));

                    /// Nº Mes anterior
                    mes_anterior = api.column(5)
                        .data()
                        .reduce(function (a, b) {
                            return parseInt(a) + parseInt(b.toString().replace('.', ''));
                        }, 0);

                    /// Update footer
                    $(api.column(5).footer()).html(PonerPuntoMil(mes_anterior));

                    /// Nº Año
                    anyo = api.column(6)
                        .data()
                        .reduce(function (a, b) {
                            return parseInt(a) + parseInt(b.toString().replace('.', ''));
                        }, 0);

                    /// Update footer
                    $(api.column(6).footer()).html(PonerPuntoMil(anyo));
                }
            });

            $('#tabla_Historico').DataTable({
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
                lengthMenu: [[20, 50, -1], [20, 50, "All"]],
                "columnDefs": [
                  {
                      "targets": [0],
                      "class": "text-center"
                  },
                  {
                      "targets": [1],
                      "class": "text-center"
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  },
                  {
                      "targets": [3],
                      "class": "text-center"
                  },
                  {
                      "targets": [4],
                      "class": "text-center"
                  },
                  {
                      "targets": [5],
                      "class": "text-center"
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
                  },
                  {
                      "targets": [12],
                      "class": "text-center"
                  },
                  {
                      "targets": [13],
                      "class": "text-center"
                  }
                ],
                "order": [[0, 'asc']]
            });
        });

        function desactivarServidor(id) {
            var hidId = document.getElementById('<%=hidIdServer.ClientID %>');
            hidId.value = id;

            var boton = document.getElementById('<%=btnDesactivarServidor.ClientID %>');
            boton.click();
        }

        function activarServidor(id) {
            var hidId = document.getElementById('<%=hidIdServer.ClientID %>');
            hidId.value = id;

            var boton = document.getElementById('<%=btnActivarServidor.ClientID %>');
            boton.click();
        }
    </script>
</body>
</html>
