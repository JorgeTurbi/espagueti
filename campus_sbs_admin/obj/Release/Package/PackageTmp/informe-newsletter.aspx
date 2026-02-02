<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-newsletter.aspx.cs" Inherits="campus_sbs_admin.informe_newsletter" %>
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
                            <legend id="txt_informe_newsletter" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="nombre_newsletter" class="col-sm-12 margin-b-2" runat="server"></div>
                    <div id="asunto_newsletter" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div id="block_informe_status" class="row col-sm-12" runat="server"></div>
                    <div id="block_otros_datos_status" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_24_horas" class="text-color-primary" runat="server">24 HORAS PERFORMANCE OPENS</legend>
                        </fieldset>
                    </div>
                    <div id="txt_envio_24" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div id="chart_24_hours" class="col-sm-12" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_week" class="text-color-primary" runat="server">WEEK PERFORMANCE OPENS</legend>
                        </fieldset>
                    </div>
                    <div id="txt_envio_week" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div id="chart_week" class="col-sm-12" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_clicks" class="text-color-primary" runat="server">CLICS</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_clics" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_opens" class="text-color-primary" runat="server">OPENS</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_opens" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_bounced" class="text-color-primary" runat="server">BOUNCED</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_bounced" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_bajas" class="text-color-primary" runat="server">BAJAS (UNSUBSCRIBED)</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_bajas" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_top_locations" class="text-color-primary" runat="server">TOP LOCATIONS PERFORMANCE OPENS</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_locations" class="col-sm-12 margin-b-15" runat="server"></div>
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
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.cleanString.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tabla_Bajas').DataTable({
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
                      "targets": [1]
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  },
                  {
                      "targets": [3],
                      "class": "text-center",
                      "type": "eu_date"
                  }
                ],
                "order": [[0, "asc"]]
            });

            $('#tabla_locations').DataTable({
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
                      "targets": [1]
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  },
                  {
                      "targets": [3],
                      "class": "text-center"
                  }
                ],
                "order": [[2, "desc"], [1, 'asc']]
            });

            $('#tabla_Clics').DataTable({
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
                      "type": "clear-string"
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"]]
            });

            $('#tabla_Opens').DataTable({
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
                      "type": "clear-string"
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"]]
            });

            $('#tabla_Bounced').DataTable({
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
                      "type": "clear-string"
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"]]
            });
        });
    </script>

    <!-- Google CHARTS
     ================================================= -->
     <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
     
    <!-- Line Chart
     ================================================= -->
     <script type="text/javascript">
         google.charts.load('current', { packages: ['corechart'] });
         
         /// Gráfico 24 horas
         google.charts.setOnLoadCallback(arrayLine24hours);
         function arrayLine24hours() {
             $.ajax({
                 url: 'informe-newsletter.aspx/line24hours',
                 data: "{'id_newsletter': '" + getParams('idc') + "'}",
                 dataType: "json",
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     drawLineChart(data);
                 },
                 error: function (response) {
                     alert(response.responseText);
                 },
                 failure: function (response) {
                     alert(response.responseText);
                 }
             });
         }
         /*function drawLineChart(array) {
             var data = new google.visualization.DataTable();
             data.addColumn('string', 'Hora');
             data.addColumn('number', 'Nº opens');

             $.each(array.d, function (index, value) {
                 data.addRow([value.hours, value.num_opens]);
             });

             var options = {
                 title: '24 horas performance opens',
                 legend: { position: 'bottom' },
                 pointSize: 5,
                 height: 300
             };

             var chart = new google.visualization.LineChart(document.getElementById('chart_24_hours'));
             chart.draw(data, options);
         }*/
         function drawLineChart(array) {
             var columns = 0;
             $.each(array.d, function (index, value) {
                 if (value.envio > columns)
                     columns = value.envio;
             });

             var data = new google.visualization.DataTable();
             data.addColumn('string', 'Hora');

             for (var i = 1; i < columns + 1; i++) {
                 data.addColumn('number', 'Nº opens Envio ' + i);
             };
                          
             var _hours = ['00', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23'];
             for (var i = 0; i < _hours.length; i++) {
                 var _row = [];
                 _row.push(_hours[i]);

                 var _max = 0;
                 for (var j = 1; j < columns + 1; j++) {
                     $.each(array.d, function (index, value) {
                         if (value.hours == _hours[i]) {
                             if (value.envio == j)
                                 _max = value.num_opens;
                         }
                     });

                     _row.push(parseInt(_max));
                 }

                 data.addRow(_row);
             };

             var options = {
                 title: '24 horas performance opens',
                 legend: { position: 'bottom' },
                 pointSize: 5,
                 height: 300
             };

             var chart = new google.visualization.LineChart(document.getElementById('chart_24_hours'));
             chart.draw(data, options);
         }

         /// Gráfico week
         google.charts.setOnLoadCallback(arrayLineWeek);
         function arrayLineWeek() {
             $.ajax({
                 url: 'informe-newsletter.aspx/line_week',
                 data: "{'id_newsletter': '" + getParams('idc') + "'}",
                 dataType: "json",
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     drawLineChartWeek(data);
                 },
                 error: function (response) {
                     alert(response.responseText);
                 },
                 failure: function (response) {
                     alert(response.responseText);
                 }
             });
         }
         /*function drawLineChartWeek(array) {
             var data = new google.visualization.DataTable();
             data.addColumn('string', 'Día');
             data.addColumn('number', 'Nº opens');

             $.each(array.d, function (index, value) {
                 data.addRow([value.days, value.num_opens]);
             });

             var options = {
                 title: 'week performance opens',
                 legend: { position: 'bottom' },
                 pointSize: 5,
                 height: 300
             };

             var chart = new google.visualization.ColumnChart(document.getElementById('chart_week'));
             chart.draw(data, options);
         }*/
         function drawLineChartWeek(array) {
             var columns = 0;
             $.each(array.d, function (index, value) {
                 if (value.envio > columns)
                     columns = value.envio;
             });

             var data = new google.visualization.DataTable();
             data.addColumn('string', 'Día');

             for (var i = 1; i < columns + 1; i++) {
                 data.addColumn('number', 'Nº opens Envio ' + i);
             };

             var _days = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];
             for (var i = 0; i < _days.length; i++) {
                 var _row = [];
                 _row.push(_days[i]);

                 var _max = 0;
                 for (var j = 1; j < columns + 1; j++) {
                     $.each(array.d, function (index, value) {
                         if (value.days == _days[i]) {
                             if (value.envio == j)
                                 _max = value.num_opens;
                         }
                     });

                     _row.push(parseInt(_max));
                 }

                 data.addRow(_row);
             };
             

             /*$.each(array.d, function (index, value) {
                 data.addRow([value.days, value.num_opens]);
             });*/

             var options = {
                 title: 'week performance opens',
                 legend: { position: 'bottom' },
                 pointSize: 5,
                 height: 300
             };

             var chart = new google.visualization.ColumnChart(document.getElementById('chart_week'));
             chart.draw(data, options);
         }
     </script>
</body>
</html>