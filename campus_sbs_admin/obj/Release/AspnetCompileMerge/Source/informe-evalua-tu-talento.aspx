<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="informe-evalua-tu-talento.aspx.cs" Inherits="campus_sbs_admin.informe_evalua_tu_talento" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Informe Evalúa tu talento</title>

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

    <style>
        /*.summary {
            border: dashed 1px green;
            width: 97%;
            color: black;
            margin-bottom: 15px;
            padding-top: 5px;
            padding-bottom: 5px;
            margin-left: 15px;
            margin-right: 15px;
        }

        .summary-changed {
            border: dashed 2px orange;
            width: 97%;
            margin-bottom: 15px;
            padding-top: 5px;
            padding-bottom: 5px;
            margin-left: 15px;
            color: black;
            margin-right: 15px;
        }*/

        .lblresumen {
            font-size: 20px;
            color: green;
        }

        /*.card {
            width: 14%;
            float: left;
            position: relative;
            min-height: 70px;
            padding-left: 15px;
            padding-right: 15px;
            background-color: whitesmoke;
            margin: 1px;
        }*/

        .card {background-color: whitesmoke; border: 1px solid white;}

        .card-value {
            height: 60px;
        }

        .labelcenter {
            margin-top: 9px;
        }

        .help {
            cursor: help;
        }
    </style>

    <style type="text/css">
        .input-group .form-control {
            background-color: white;
            border: 1px solid #bdbdbd;
            color: black;
        }

        .input-group.date.js-datepicker {
            width: 100%;
        }

        .input-group.has-error .form-control {
            background: #fbf2f1 none repeat scroll 0 0;
            border: 1px solid #a94442;
            color: #f2958d;
        }

            .input-group.has-error .form-control::-moz-placeholder {
                color: #f2958d;
                opacity: 1;
            }

        .checkbox img {
            height: 25px;
            width: 25px;
        }

        #btn_upload > img {
            cursor: pointer;
        }

        #fileinput > span {
            white-space: normal;
        }

        #txt_comentarios {
            max-height: 350px;
        }

        .btn.fileinput-button {
            border: 1px solid #ccc;
            height: 150px;
            margin-bottom: 5px;
            overflow: hidden;
            text-align: left;
            width: 100%;
        }

        .fileinput-button input {
            cursor: pointer;
            direction: ltr;
            height: 150px;
            left: -4px;
            margin: 0;
            opacity: 0;
            position: absolute;
            right: 0;
            top: 0;
            width: 100%;
        }

        .bootstrap-select > .btn.btn-default {
            padding: 12px;
        }
    </style>
</head>
<body>
    <uc_menu:menu ID="menu" runat="server" />
    <header id="header" class="bg-color-primary affix">
        <uc_header:cabecera ID="cabecera" runat="server" />
    </header>

    <main class="wrapper public bg-color-white" role="main">
        <section class="padding-tb-40 padding-xs-tb-30">
            <div class="block-primary">
                <div class="row no-margin padding-nav" style="min-height: 100vh;">
                    <div class="col-sm-12">
                        <fieldset>
                            <legend id="title" class="text-color-primary" runat="server"><i class='fas fa-chart-line'></i> Informe evalúa tu talento</legend>
                        </fieldset>
                    </div>                    
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div class="col-sm-6 col-sm-offset-3 margin-b-10">
                            <div class="col-sm-4 no-padding">
                                <div class="col-sm-12 card">
                                    <label class="col-sm-12 text-center padding-t-10" runat="server">Ficha Test</label>
                                    <div class="col-sm-12 card-value text-center">
                                        <span id="test_number" style="font-size: 30px; width: 100%;" runat="server" class="help" title="Cantidad de visitas ficha test."></span>
                                    </div>                                
                                </div>
                            </div>
                            <div class="col-sm-4 no-padding ">
                                <div class="col-sm-12 card">
                                    <label class="col-sm-12 text-center padding-t-10" runat="server">Test iniciados</label>
                                    <div class="col-sm-12 card-value text-center">
                                        <span id="test_start_number" style="font-size: 30px; width: 100%;" runat="server" class="help" title="Cantidad de test iniciados."></span>
                                        <br />
                                        <span id="test_start_percent" runat="server" class="help" title="Porciento que representan los test iniciados de los que han llegado a la ficha."></span>
                                    </div>                                
                                </div>
                            </div>
                            <div class="col-sm-4 no-padding ">
                                <div class="col-sm-12 card">
                                    <label class="col-sm-12 text-center padding-t-10" runat="server">Test finalizados</label>
                                    <div class="col-sm-12 card-value text-center">
                                        <span id="test_end_number" style="font-size: 30px; width: 100%;" runat="server" class="help" title="Cantidad de test finalizados."></span>
                                        <br />
                                        <span id="test_end_percent" runat="server" class="help" title="Porciento que representan los test finalizados sobre los test iniciados."></span>
                                    </div>                                
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <fieldset>
                                <legend id="title_search" class="text-color-primary" runat="server"><i class='far fa-list-alt'></i> Listado de test</legend>
                            </fieldset>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-3">
                                <label>Fecha Inicio</label>
								<div id="fechaAlta_form" class="input-group date js-datepicker" runat="server">
								    <label class="sr-only" for="date_start">Fecha</label>
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
                                <label class="w-100">&nbsp;</label>
                                <asp:ImageButton ID="img_filter" ImageUrl="/App_Themes/support/img/icons/icon_search.png" runat="server" CssClass="padding5" ToolTip="Buscar" OnClick="img_filter_Click" />
                            </div>
                        </div>
                    </form>
                    <div id="table_listado_examenes" class="col-sm-12" runat="server"></div>
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
            $('#tabla_List_Test').DataTable({
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
                lengthMenu: [[50, -1], [50, "All"]],
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

                    /// Alcance
                    alcance = api.column(1)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(1).footer()).html(PonerPuntoMil(alcance));

                    /// Test iniciados
                    test_iniciados = api.column(2)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(2).footer()).html(PonerPuntoMil(test_iniciados));

                    /// Test finalizados
                    test_finalizados = api.column(4)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(4).footer()).html(PonerPuntoMil(test_finalizados));

                    /// Leads
                    leads = api.column(6)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    /// Update footer
                    $(api.column(6).footer()).html(PonerPuntoMil(leads));
                }
            });
        });
    </script>
</body>
</html>