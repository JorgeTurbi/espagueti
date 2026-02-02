<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="control-lead-automation.aspx.cs" Inherits="campus_sbs_admin.control_lead_automation" %>
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
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fullcalendar.css" />
     <style type="text/css">
         .course-box::before {border-right: 0 none;}

         td.details-control {background: rgba(0, 0, 0, 0) url("/App_Themes/support/img/datatables/details_open.png") no-repeat scroll left center; cursor: pointer;}
        tr.shown td.details-control {background: rgba(0, 0, 0, 0) url("/App_Themes/support/img/datatables/details_close.png") no-repeat scroll left center;}
        .details-control > span {margin-left: 20px;}
        .wait {cursor: wait !important;}
     </style>

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
            <div id="blk_control" class="block-primary">
                <div class="row no-margin padding-nav">
                    <div class="col-sm-12">
                        <fieldset>
                            <legend id="txt_control_leads" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="lnk_day" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div id="table_listado_leads_day" class="col-sm-12 margin-b-15" runat="server"></div>
                    <input id="hid_day" type="hidden" value="" runat="server" />
                    <input id="hid_day_month" type="hidden" value="" runat="server" />
                    <input id="hid_day_year" type="hidden" value="" runat="server" />
                    <div class="col-sm-12 padding-t-20">
                        <fieldset>
                            <legend id="txt_control_leads_month" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="lnk_month" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div id="table_listado_leads_month" class="col-sm-12 margin-b-15" runat="server"></div>
                    <div id="chart_month" class="col-sm-12" runat="server"></div>
                    <input id="hid_month" type="hidden" value="" runat="server" />
                    <input id="hid_year" type="hidden" value="" runat="server" />
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
            paint_table_day();
            paint_table_month();
            google_chart();
        });
        
        function paint_table_day() {
            var table_leads = $('#tabla_List_Leads').DataTable({
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
                      "className": 'details-control',
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
                  },
                  {
                      "targets": [4],
                      "class": "text-center"
                  }
                ],
                "order": [[1, "asc"]]
            });
            $('#tabla_List_Leads tbody').on('click', 'td.details-control', function () {
                var tr = $(this).closest('tr');
                var row = table_leads.row(tr);

                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    // Open this row
                    var id = tr.find('td').find('span').html();
                    if (id.indexOf(',') == -1)
                        var _subTable = search_subtable(id, row, tr);
                }
            });
        }
        function paintTableLevel2() {
            $('#tabla_List_Leads_Origin').DataTable({
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
                      "class": "text-center",
                      "type": "eu_date"
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
                      "targets": [4],
                      "class": "text-center"
                  },
                  {
                      "targets": [5],
                      "class": "text-center"
                  }
                ],
                "order": [[3, "asc"], [1, "asc"]]
            });
        }
        function paint_table_month() {
            $('#tabla_List_Leads_Month').DataTable({
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
                      "targets": [0]
                  },
                  {
                      "targets": [1],
                      "class": "text-center"
                  },
                  {
                      "targets": [2],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"]]
            });
        }        

        function search_subtable(id, row, tr) {
            var day = $('#hid_day').val();
            var month = $('#hid_day_month').val();
            var year = $('#hid_day_year').val();

            $.ajax({
                url: 'control-lead-automation.aspx/search_subtable',
                data: "{ 'id': '" + id + "', 'day': '" + day + "', 'month': '" + month + "', 'year': '" + year + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    row.child(data.d);
                    row.child.show();
                    tr.addClass('shown');
                    paintTableLevel2();
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        }

        function search_day_ant() {
            var day = $('#hid_day').val();
            var month = $('#hid_day_month').val();
            var year = $('#hid_day_year').val();
            $('#blk_control').addClass(' wait');
            
            var fecha = new Date(year, month - 1, day);
            var yesterday = new Date(fecha.getTime() - 24 * 60 * 60 * 1000);
            
            $('#hid_day').val(yesterday.getDate());
            $('#hid_day_month').val(yesterday.getMonth() + 1);
            $('#hid_day_year').val(yesterday.getFullYear());

            var meses = new Array("Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
            $('#lnk_day').html("<div class='fc-button-group'><button type='button' onclick='search_day_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_day_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + yesterday.getDate() + " " + meses[yesterday.getMonth()] + " " + yesterday.getFullYear() + "</span>");
            search_table_day(yesterday.getDate(), yesterday.getMonth() + 1, yesterday.getFullYear());
        }
        function search_day_sig() {
            var day = $('#hid_day').val();
            var month = $('#hid_day_month').val();
            var year = $('#hid_day_year').val();
            $('#blk_control').addClass(' wait');

            var fecha = new Date(year, month - 1, day);
            var tomorrow = new Date(fecha.getTime() + 24 * 60 * 60 * 1000);

            $('#hid_day').val(tomorrow.getDate());
            $('#hid_day_month').val(tomorrow.getMonth() + 1);
            $('#hid_day_year').val(tomorrow.getFullYear());

            var meses = new Array("Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
            $('#lnk_day').html("<div class='fc-button-group'><button type='button' onclick='search_day_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_day_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + tomorrow.getDate() + " " + meses[tomorrow.getMonth()] + " " + tomorrow.getFullYear() + "</span>");
            search_table_day(tomorrow.getDate(), tomorrow.getMonth() + 1, tomorrow.getFullYear());
        }
        function search_table_day(day, month, year) {
            $.ajax({
                url: 'control-lead-automation.aspx/search_table_day',
                data: "{ 'day': '" + day + "', 'month': '" + month + "', 'year': '" + year + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#table_listado_leads_day').html('');
                    $('#table_listado_leads_day').html(data.d);
                    $('#blk_control').removeClass(' wait');
                    paint_table_day();
                },
                error: function (response) {
                    $('#blk_control').removeClass(' wait');
                    alert(response.responseText);
                },
                failure: function (response) {
                    $('#blk_control').removeClass(' wait');
                    alert(response.responseText);
                }
            });
        }

        function search_month_ant() {
            var month = $('#hid_month').val();
            var year = $('#hid_year').val();
            $('#blk_control').addClass(' wait');

            if (month == '1') {
                $('#hid_month').val(12);
                $('#hid_year').val(parseInt(year) - 1);
            }
            else {
                $('#hid_month').val(parseInt(month) - 1);
                $('#hid_year').val(parseInt(year));
            }

            var meses = new Array("", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
            var _month = parseInt(month) - 1;
            var _year = parseInt(year);
            if (month == '1') {
                _month = 12;
                _year = parseInt(year) - 1;
            }
            $('#lnk_month').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            search_table_month(_month, _year);
            google_chart();            
        }
        function search_month_sig() {
            var month = $('#hid_month').val();
            var year = $('#hid_year').val();
            $('#blk_control').addClass(' wait');
            
            if (month == '12') {
                $('#hid_month').val(1);
                $('#hid_year').val(parseInt(year) + 1);
            }
            else {
                $('#hid_month').val(parseInt(month) + 1);
                $('#hid_year').val(parseInt(year));
            }

            var meses = new Array("", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
            var _month = parseInt(month) + 1;
            var _year = parseInt(year);
            if (month == '12') {
                _month = 1;
                _year = parseInt(year) + 1;
            }
            $('#lnk_month').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            search_table_month(_month, _year);
            google_chart();
        }
        function search_table_month(month, year) {
            $.ajax({
                url: 'control-lead-automation.aspx/search_table_month',
                data: "{ 'month': '" + month + "', 'year': '" + year + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#table_listado_leads_month').html('');
                    $('#table_listado_leads_month').html(data.d);
                    paint_table_month();
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        }
    </script>

    <!-- Google CHARTS
     ================================================= -->
     <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
     
    <!-- Line Chart
     ================================================= -->
     <script type="text/javascript">
         function google_chart() {
             google.charts.load('current', { packages: ['corechart'] });

             /// Gráfico Mes
             google.charts.setOnLoadCallback(arrayLine);
         }
         function arrayLine() {
             $.ajax({
                 url: 'control-lead-automation.aspx/arrayLine',
                 data: "{'month': '" + $('#hid_month').val() + "', 'year': '" + $('#hid_year').val() + "'}",
                 dataType: "json",
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     drawLineChart(data);
                     $('#blk_control').removeClass(' wait');
                 },
                 error: function (response) {
                     alert(response.responseText);
                     $('#blk_control').removeClass(' wait');
                 },
                 failure: function (response) {
                     alert(response.responseText);
                     $('#blk_control').removeClass(' wait');
                 }
             });
         }
         function drawLineChart(array) {
             var columns = 0;
             $.each(array.d, function (index, value) {
                 if (value.envio > columns)
                     columns = value.envio;
             });

             var data = new google.visualization.DataTable();
             data.addColumn('number', 'Día');

             var _origins = [];
             $.each(array.d, function (index, value) {
                 if (!_origins.includes(value.days))
                     _origins.push(value.days);
             });

             for (var i = 0; i < _origins.length; i++) {
                 data.addColumn('number', _origins[i]);
             };

             for (var i = 1; i <= columns; i++) {
                 var _row = [];
                 _row.push(i);

                 for (var j = 0; j < _origins.length; j++) {
                     $.each(array.d, function (index, value) {
                         if (value.days == _origins[j] && value.envio == i)
                             _row.push(parseInt(value.num_opens));
                     });
                 }

                 data.addRow(_row);
             };

             var options = {
                 title: 'Nº de leads por origen',
                 legend: { position: 'bottom' },
                 pointSize: 5,
                 height: 450
             };

             var chart = new google.visualization.LineChart(document.getElementById('chart_month'));
             chart.draw(data, options);
         }         
     </script>
</body>
</html>
