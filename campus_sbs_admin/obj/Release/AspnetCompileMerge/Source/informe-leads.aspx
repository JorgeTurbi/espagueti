<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-leads.aspx.cs" Inherits="campus_sbs_admin.informe_leads" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Informe de leads</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fullcalendar.css" />     
     <style type="text/css">
         .input-group .form-control {background-color: white; border: 1px solid #bdbdbd; color: black;}
         .input-group.date.js-datepicker { width: 100%;}
         .input-group.has-error .form-control {background: #fbf2f1 none repeat scroll 0 0; border: 1px solid #a94442; color: #f2958d;}
         .input-group.has-error .form-control::-moz-placeholder {color: #f2958d; opacity: 1;}
         .checkbox img {height: 25px; width: 25px;}

         .vh-100 {height: 100vh;}
         .modal-dialog {top: 33%;}        
         .block_head {background-color: #ddd; border: 1px solid #ddd; color: #696969; font-weight: bold; padding: 3% 0;}
         .block_body {border: 1px solid #ddd; min-height: 110px; text-align: center; }
         #wait_modal .modal-dialog {width: 9em;}   

         .course-box::before {border-right: 0 none;}
         td.details-control {background: rgba(0, 0, 0, 0) url("/App_Themes/support/img/datatables/details_open.png") no-repeat scroll left center; cursor: pointer;}
         tr.shown td.details-control {background: rgba(0, 0, 0, 0) url("/App_Themes/support/img/datatables/details_close.png") no-repeat scroll left center;}
         .details-control > span {margin-left: 20px;}
         .wait {cursor: wait !important;}
     </style>    
	    
	 <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700,900%7COpen+Sans:400,600,700" rel="stylesheet" type="text/css" />

    <!-- CSS para el control DataTable -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />

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
    
    <main class="wrapper public bg-color-white vh-100" role="main">     	    
        <section class="padding-tb-50">
		    <div class="row no-margin padding-nav">	
                <div class="col-sm-12">                         
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div>
                            <fieldset>
							    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de leads</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-5">
                                    <label>Tipo</label>
                                    <div class="form-group padding-t-10">													
							            <div class="radio">
                                            <asp:RadioButtonList ID="radTipo" RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Text="&nbsp;Por Origen" Value="O" Selected="True" />
                                                <asp:ListItem Text="&nbsp;Por Comercial" Value="C" />
                                                <asp:ListItem Text="&nbsp;Por Programa" Value="P" />
                                                <asp:ListItem Text="&nbsp;Todos" Value="T" />
                                            </asp:RadioButtonList>
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-2">
                                    <label>Fecha Inicio</label>
								    <div id="fechaAlta_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="date_start">Fecha Inicio</label>
									    <input type="text" id="date_start" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-2">
                                    <label>Fecha Fin</label>
								    <div id="fechaBaja_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="date_end">Fecha Fin</label>
									    <input type="text" id="date_end" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-2">
                                    <label>Mes</label>
                                    <div id="month_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlMes">Mes</label>
                                        <select id="ddlMes" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true"></select>
								    </div>
                                </div>
                                <div class="col-sm-1 text-center">
                                    <label class="full-width">&nbsp;</label>
                                    <asp:ImageButton ID="img_filter" ImageUrl="/App_Themes/support/img/icons/icon_search.png" runat="server" CssClass="padding5" ToolTip="Buscar" OnClick="img_filter_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>
                </div>
                <div id="blk_origen" class="col-sm-12 padding-t-20" visible="false" runat="server">
                    <fieldset>
					    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de leads por origenes</legend>
                        <div id="lnk_month_origenes" class="col-sm-12 margin-b-15" runat="server"></div>
                        <div id="tabla_leads_origenes" class="col-sm-12 margin-b-15 padding-t-20" runat="server"></div>
                    </fieldset>
                </div>
                <div id="blk_comercial" class="col-sm-12 padding-t-20" visible="false" runat="server">
                    <fieldset>
					    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de leads por comerciales</legend>
                        <div id="lnk_month_comerciales" class="col-sm-12 margin-b-15" runat="server"></div>
                        <div id="tabla_leads_comerciales" class="col-sm-12 margin-b-15 padding-t-20" runat="server"></div>
                    </fieldset>
                </div>
                <div id="blk_programa" class="col-sm-12 padding-t-20" visible="false" runat="server">
                    <fieldset>
					    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de leads por programas</legend>
                        <div id="lnk_month_programas" class="col-sm-12 margin-b-15" runat="server"></div>
                        <div id="tabla_leads_programas" class="col-sm-12 margin-b-15 padding-t-20" runat="server"></div>
                    </fieldset>
                </div>
                <div id="blk_todos" class="col-sm-12 padding-t-20" visible="false" runat="server">
                    <fieldset>
					    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de leads</legend>
                        <div id="lnk_month_todos" class="col-sm-12 margin-b-15" runat="server"></div>
                        <div id="tabla_leads_todos" class="col-sm-12 margin-b-15 padding-t-20" runat="server"></div>
                    </fieldset>
                </div>
                <input id="hid_month" type="hidden" value="" runat="server" />
                <input id="hid_year" type="hidden" value="" runat="server" />						
			</div>
        </section>
    </main>

    <!-- Modal -->
    <div class="modal fade" id="wait_modal" tabindex="-1" role="dialog" aria-labelledby="wait_modal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-body text-center">
                    <i class="fas fa-spinner fa-pulse fa-5x"></i>
                </div>
            </div>
        </div>
    </div>

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
            paint_table_origin();
            paint_table_commercial();
            paint_table_programa();
            paint_table_all();
        });
        
        function paint_table_origin() {
            $('#tabla_List_Origenes').DataTable({
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
                lengthMenu: [[-1, 20, 50], ["All", 20, 50]],
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
                  }
                ],
                "order": [[0, "asc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Nº leads
                    number = api.column(1)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(1).footer()).html(PonerPuntoMil(number));

                    /// Nº leads válidos
                    validos = api.column(2)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(2).footer()).html(PonerPuntoMil(validos));

                    /// Porcentaje de Nº leads entre Nºlead válidos
                    var porcentaje_leads = 0;
                    if (number > 0)
                        porcentaje_leads = (validos / number) * 100;

                    /// Update footer
                    $(api.column(3).footer()).html(porcentaje_leads.toFixed(2) + "%");

                    /// Nº matriculas
                    matriculas = api.column(4)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(4).footer()).html(PonerPuntoMil(matriculas));

                    /// Porcentaje de Ratio M/L
                    var porcentaje_matriculas = 0;
                    if (number > 0)
                        porcentaje_matriculas = (matriculas / number) * 100;

                    /// Update footer
                    $(api.column(5).footer()).html(porcentaje_matriculas.toFixed(2) + "%");

                    /// Porcentaje de Nº leads entre Nºlead válidos
                    var porcentaje_matriculas_val = 0;
                    if (validos > 0)
                        porcentaje_matriculas_val = (matriculas / validos) * 100;

                    /// Update footer
                    $(api.column(6).footer()).html(porcentaje_matriculas_val.toFixed(2) + "%");
                }
            });
        }

        function paint_table_commercial() {
            var table_leads = $('#tabla_List_Comerciales').DataTable({
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
                lengthMenu: [[-1, 20, 50], ["All", 20, 50]],
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
                "order": [[1, "asc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Nº leads
                    number = api.column(2)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(2).footer()).html(PonerPuntoMil(number));

                    /// Nº leads válidos
                    validos = api.column(3)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(3).footer()).html(PonerPuntoMil(validos));

                    /// Nº matriculas
                    matriculas = api.column(5)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(5).footer()).html(PonerPuntoMil(matriculas));
                }
            });
            $('#tabla_List_Comerciales tbody').on('click', 'td.details-control', function () {
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
        function search_subtable(id, row, tr) {
            var _date_start = $('#date_start').val();
            var _date_end = $('#date_end').val();
            $('#wait_modal').modal('show');

            $.ajax({
                url: 'informe-leads.aspx/search_subtable',
                data: "{ 'id': '" + id + "', '_date_start': '" + _date_start + "', '_date_end': '" + _date_end + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    row.child(data.d);
                    row.child.show();
                    tr.addClass('shown');                    
                    $('#wait_modal').modal('hide');
                    paintTableLevel2();
                },
                error: function (response) {
                    alert(response.responseText);
                    $('#wait_modal').modal('hide');
                },
                failure: function (response) {
                    alert(response.responseText);
                    $('#wait_modal').modal('hide');
                }
            });
        }
        function paintTableLevel2() {
            $('#tabla_List_Comerciales_level2').DataTable({
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
                  }
                ],
                "order": [[0, "asc"]],
            });
        }

        function paint_table_programa() {
            var table_programs = $('#tabla_List_Programas').DataTable({
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
                lengthMenu: [[-1, 20, 50], ["All", 20, 50]],
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
                      "targets": [8],
                      "class": "text-center"
                  }
                ],
                "order": [[1, "asc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Nº leads
                    visitas = api.column(2)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(2).footer()).html(PonerPuntoMil(visitas));
                    
                    /// Nº leads
                    number = api.column(3)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(3).footer()).html(PonerPuntoMil(number));

                    /// CTR
                    var porcentaje_visitas = 0;
                    if (visitas > 0)
                        porcentaje_visitas = (number / visitas) * 100;

                    /// Update footer
                    $(api.column(4).footer()).html(porcentaje_visitas.toFixed(2) + "%");
                    
                    /// Nº leads válidos
                    validos = api.column(5)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(5).footer()).html(PonerPuntoMil(validos));

                    /// Porcentaje de Nº leads entre Nºlead válidos
                    var porcentaje_leads = 0;
                    if (number > 0)
                        porcentaje_leads = (validos / number) * 100;

                    /// Update footer
                    $(api.column(6).footer()).html(porcentaje_leads.toFixed(2) + "%");

                    /// Nº matriculas
                    matriculas = api.column(7)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(7).footer()).html(PonerPuntoMil(matriculas));

                    /// Porcentaje de Ratio M/L
                    var porcentaje_matriculas = 0;
                    if (number > 0)
                        porcentaje_matriculas = (matriculas / number) * 100;

                    /// Update footer
                    $(api.column(8).footer()).html(porcentaje_matriculas.toFixed(2) + "%");

                    /// Porcentaje de Nº leads entre Nºlead válidos
                    var porcentaje_matriculas_val = 0;
                    if (validos > 0)
                        porcentaje_matriculas_val = (matriculas / validos) * 100;

                    /// Update footer
                    $(api.column(9).footer()).html(porcentaje_matriculas_val.toFixed(2) + "%");
                }
            });
            $('#tabla_List_Programas tbody').on('click', 'td.details-control', function () {
                var tr = $(this).closest('tr');
                var row = table_programs.row(tr);

                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                }
                else {
                    // Open this row
                    var id = tr.find('td').find('span').html();
                    if (id.indexOf(',') == -1)
                        var _subTable = search_subtable_program(id, row, tr);
                }
            });
        }
        function search_subtable_program(id, row, tr) {
            var _date_start = $('#date_start').val();
            var _date_end = $('#date_end').val();
            $('#wait_modal').modal('show');

            $.ajax({
                url: 'informe-leads.aspx/search_subtable_programs',
                data: "{ 'id': '" + id + "', '_date_start': '" + _date_start + "', '_date_end': '" + _date_end + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    row.child(data.d);
                    row.child.show();
                    tr.addClass('shown');
                    $('#wait_modal').modal('hide');
                    paintTableProgramLevel2();
                },
                error: function (response) {
                    alert(response.responseText);
                    $('#wait_modal').modal('hide');
                },
                failure: function (response) {
                    alert(response.responseText);
                    $('#wait_modal').modal('hide');
                }
            });
        }
        function paintTableProgramLevel2() {
            $('#tabla_List_Programs_level2').DataTable({
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
                lengthMenu: [[-1, 10, 25], ["All", 10, 25]],
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
                  }
                ],
                "order": [[1, "asc"]],
            });
        }

        function paint_table_all() {
            $('#tabla_List_Todos').DataTable({
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
                lengthMenu: [[-1, 20, 50], ["All", 20, 50]],
                "columnDefs": [
                  {
                      "targets": [0]
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2]
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
                  }
                ],
                "order": [[0, "asc"], [1, "asc"], [2, "asc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Nº leads
                    number = api.column(3)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(3).footer()).html(PonerPuntoMil(number));

                    /// Nº leads válidos
                    validos = api.column(4)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(4).footer()).html(PonerPuntoMil(validos));

                    /// Porcentaje de Nº leads entre Nºlead válidos
                    var porcentaje_leads = 0;
                    if (number > 0)
                        porcentaje_leads = (validos / number) * 100;

                    /// Update footer
                    $(api.column(5).footer()).html(porcentaje_leads.toFixed(2) + "%");

                    /// Nº matriculas
                    matriculas = api.column(6)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(6).footer()).html(PonerPuntoMil(matriculas));

                    /// Porcentaje de Ratio M/L
                    var porcentaje_matriculas = 0;
                    if (number > 0)
                        porcentaje_matriculas = (matriculas / number) * 100;

                    /// Update footer
                    $(api.column(7).footer()).html(porcentaje_matriculas.toFixed(2) + "%");

                    /// Porcentaje de Nº leads entre Nºlead válidos
                    var porcentaje_matriculas_val = 0;
                    if (validos > 0)
                        porcentaje_matriculas_val = (matriculas / validos) * 100;

                    /// Update footer
                    $(api.column(8).footer()).html(porcentaje_matriculas_val.toFixed(2) + "%");
                }
            });
        }

        function search_month_ant() {
            var month = $('#hid_month').val();
            var year = $('#hid_year').val();
            var _type = $('input:radio[name=radTipo]:checked').val();
            $('#wait_modal').modal('show');

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

            if (_type == "O")
                $('#lnk_month_origenes').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            else if (_type == "C")
                $('#lnk_month_comerciales').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            else if (_type == "P")
                $('#lnk_month_programas').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            else if (_type == "T")
                $('#lnk_month_todos').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");

            search_table_month(_month, _year, _type);
        }
        function search_month_sig() {
            var month = $('#hid_month').val();
            var year = $('#hid_year').val();
            var _type = $('input:radio[name=radTipo]:checked').val();
            $('#wait_modal').modal('show');

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

            if (_type == "O")
                $('#lnk_month_origenes').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            else if (_type == "C")
                $('#lnk_month_comerciales').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            else if (_type == "P")
                $('#lnk_month_programas').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
            else if (_type == "T")
                $('#lnk_month_todos').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");

            search_table_month(_month, _year, _type);
        }
        function search_table_month(month, year, _type) {
            $.ajax({
                url: 'informe-leads.aspx/search_table_month',
                data: "{ 'month': '" + month + "', 'year': '" + year + "', 'type': '" + _type + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (_type == "O") {
                        $('#tabla_leads_origenes').html('');
                        $('#tabla_leads_origenes').html(data.d);
                        paint_table_origin();
                    }
                    else if (_type == "C") {
                        $('#tabla_leads_comerciales').html('');
                        $('#tabla_leads_comerciales').html(data.d);
                        paint_table_commercial();
                    }
                    else if (_type == "P") {
                        $('#tabla_leads_programas').html('');
                        $('#tabla_leads_programas').html(data.d);
                        paint_table_programa();
                    }
                    else if (_type == "T") {
                        $('#tabla_leads_todos').html('');
                        $('#tabla_leads_todos').html(data.d);
                        paint_table_all();
                    }
                    $('#wait_modal').modal('hide');
                },
                error: function (response) {
                    alert(response.responseText);
                    $('#wait_modal').modal('hide');
                },
                failure: function (response) {
                    alert(response.responseText);
                    $('#wait_modal').modal('hide');
                }
            });
        }
    </script>
</body>
</html>