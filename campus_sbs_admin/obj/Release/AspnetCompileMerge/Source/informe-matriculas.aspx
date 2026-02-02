<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-matriculas.aspx.cs" Inherits="campus_sbs_admin.informe_matriculas" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Informe de matrículas</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />     
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
							    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de matrículas</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-5">
                                    <label>Tipo</label>
                                    <div class="form-group padding-t-10">													
							            <div class="radio">
                                            <asp:RadioButtonList ID="radTipo" RepeatDirection="Horizontal" runat="server">
                                                <asp:ListItem Text="&nbsp;Por Edición" Value="E" />
                                                <asp:ListItem Text="&nbsp;Por Venta" Value="V" Selected="True" />
                                                <asp:ListItem Text="&nbsp;Por Comercial" Value="C" />
                                                <asp:ListItem Text="&nbsp;Todas" Value="T" />
                                            </asp:RadioButtonList>
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha Inicio</label>
								    <div id="fechaAlta_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="date_start">Fecha Inicio</label>
									    <input type="text" id="date_start" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha Fin</label>
								    <div id="fechaBaja_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="date_end">Fecha Fin</label>
									    <input type="text" id="date_end" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
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
                <div id="tabla_matriculas" class="col-sm-12 margin-b-15 padding-t-20" runat="server"></div>							
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
            var table_leads = $('#tabla_List_Matriculas').DataTable({
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
                      "className": 'details-control'
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
                  }
                ],
                "order": [[1, "asc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Nº matriculas
                    number = api.column(2)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(2).footer()).html(number);

                    /// Venta total
                    total = api.column(3)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);

                    /// Update footer
                    $(api.column(3).footer()).html(
                        PonerPuntoMil(total.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Ventas programa
                    program = api.column(4)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);

                    /// Update footer
                    $(api.column(4).footer()).html(
                        PonerPuntoMil(program.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Fundación
                    fundation = api.column(5)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);
                    /// Update footer
                    $(api.column(5).footer()).html(
                        PonerPuntoMil(fundation.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Universidad
                    university = api.column(6)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);
                    /// Update footer
                    $(api.column(6).footer()).html(
                        PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Restante
                    university = api.column(7)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);
                    /// Update footer
                    $(api.column(7).footer()).html(
                        PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
                    );
                }
            });
            $('#tabla_List_Matriculas tbody').on('click', 'td.details-control', function () {
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

            $('#tabla_List_Matriculas_All').DataTable({
                //responsive: true,
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
                      "targets": [3]
                  },
                  {
                      "targets": [4]
                  },
                  {
                      "targets": [5]
                  },
                  {
                      "targets": [6],
                      "class": "text-center",
                      "type": "eu_date"
                  },
                  {
                      "targets": [7],
                      "class": "text-center",
                      "type": "eu_date"
                  },
                  {
                      "targets": [8],
                      "class": "text-center",
                      "type": "eu_date"
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
                  },
                  {
                      "targets": [14],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"],[1, "asc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Venta total
                    total = api.column(10)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);

                    /// Update footer
                    $(api.column(10).footer()).html(
                        PonerPuntoMil(total.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Ventas programa
                    program = api.column(11)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);

                    /// Update footer
                    $(api.column(11).footer()).html(
                        PonerPuntoMil(program.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Fundación
                    fundation = api.column(12)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);
                    /// Update footer
                    $(api.column(12).footer()).html(
                        PonerPuntoMil(fundation.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Universidad
                    university = api.column(13)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);
                    /// Update footer
                    $(api.column(13).footer()).html(
                        PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
                    );

                    /// Restante
                    university = api.column(14)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                        }, 0);
                    /// Update footer
                    $(api.column(14).footer()).html(
                        PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
                    );
                }
            });
        });
        
        function search_subtable(id, row, tr) {
            var _date_start = $('#date_start').val();
            var _date_end = $('#date_end').val();
            var _type = $('input:radio[name=radTipo]:checked').val();
            $('#wait_modal').modal('show');

            $.ajax({
                url: 'informe-matriculas.aspx/search_subtable',
                data: "{ 'id': '" + id + "', '_date_start': '" + _date_start + "', '_date_end': '" + _date_end + "', '_type': '" + _type + "'}",
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
            $('#tabla_List_Matriculas_level2').DataTable({
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
                      "targets": [0]
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
                  },
                  {
                      "targets": [5],
                      "class": "text-center",
                      "type": "eu_date"
                  },
                  {
                      "targets": [6],
                      "class": "text-center",
                      "type": "eu_date"
                  },
                  {
                      "targets": [7],
                      "class": "text-center",
                      "type": "eu_date"
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
                "order": [[0, "asc"]]
            });
        }
    </script>
</body>
</html>