<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="avisos-leads.aspx.cs" Inherits="campus_sbs_admin.avisos_leads" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" content="30"/>
    <title>Avisos Leads</title>
    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />

    <!-- CSS para el control DataTable -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />

    <style>.bg-color-red {background-color: red; height: calc(100vh - 61px);}</style>
</head>
<body>
    <header id="header" class="bg-color-primary affix">        
        <div>            		
            <div class="row">
                <div class="col-md-7 col-md-offset-0 col-sm-5 col-sm-offset-1 col-xs-6 col-xs-offset-1">
                    <img src="/App_Themes/support/img/logo-sbs.png" alt="logo-sbs" class="logo" />
	            </div>   
                <div class="col-md-5 col-sm-6"></div>             
	        </div>
        </div>
        <div class="fluid-container"><div class="bg-secundary padding-t-10"></div></div>    
    </header> 

    <main class="wrapper public bg-color-white" role="main">        	    
        <section id="section_leads" class="padding-tb-40 padding-xs-tb-30" runat="server">
            <div class="block-primary">
                <div class="row no-margin">
                    <div class="col-sm-12">
                        <fieldset>
                            <legend id="txt_ventas_tpv" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_leads" class="col-sm-12" runat="server"></div>
                </div>
            </div>
        </section>
    </main>

   <!-- Scripts
    =================================================== --> 
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.dateFormat.js"></script>

    <script type="text/javascript">    
        $(document).ready(function () {
            $('#tabla_Leads').DataTable({
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
                      "class": "text-center",
                      "type": "euro_date"
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2]
                  },
                  {
                      "targets": [3]
                  },
                  {
                      "targets": [4]
                  }
                ],
                "order": [[0, "desc"]]
            });
        });
    </script>
</body>
</html>
