<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-universidad.aspx.cs" Inherits="campus_sbs_admin.informe_universidad" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Informe Universidad</title>

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
							    <legend class="text-color-primary"><i class='fas fa-chart-line'></i> Informe de matrículas universidad</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
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
            $('#tabla_List_Matriculas').DataTable({
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
                      "targets": [4],
                      "class": "text-center"
                  },
                  {
                      "targets": [5]
                  },
                  {
                      "targets": [6]
                  },
                  {
                      "targets": [7]
                  },
                  {
                      "targets": [8]
                  },
                  {
                      "targets": [9]
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
    </script>
</body>
</html>