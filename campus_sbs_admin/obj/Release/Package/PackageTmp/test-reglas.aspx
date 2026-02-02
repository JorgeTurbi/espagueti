<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test-reglas.aspx.cs" Inherits="campus_sbs_admin.test_reglas" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Tests Reglas mantenimiento</title>

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
        .summary {
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
        }

        .lblresumen {
            font-size: 20px;
            color: green;
        }

        .card {
            width: 14%;
            float: left;
            position: relative;
            min-height: 70px;
            padding-left: 15px;
            padding-right: 15px;
            background-color: whitesmoke;
            margin: 1px;
        }

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
                <div class="row no-margin padding-nav">
                    <div class="col-sm-12">
                        <fieldset>
                            <legend id="title" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="table" class="col-sm-12" runat="server"></div>
                    <!-- Hidden y botones ocultos -->
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div id="block_error" class="form-group has-error" runat="server">
                            <span id="txt_error" class="help-block text-center" runat="server"></span>
                        </div>
                        <div id="divsummary" runat="server" class="col-sm-12 summary">
                            <%--<h5 class="text-center lblresumen">Resumen</h5>--%>
                            <div class="card">
                                <div id="smy_preguntas_value_container" class="col-sm-12 card-value text-center"
                                    runat="server">
                                    <span id="smy_preguntas" style="font-size: 30px; width: 100%;" runat="server" class="help" title="Cantidad de preguntas que cumplen las reglas."></span>
                                    <br />
                                    <span id="smy_preguntas_necesarias" class="help" title="Cantidad de preguntas recomendadas." runat="server"></span>
                                </div>
                                <label class="col-sm-12 text-center" runat="server">Preguntas</label>
                            </div>

                            <div class="card">
                                <div id="smy_dif_avr_value_container" runat="server" class="col-sm-12 card-value text-center">
                                    <span id="smy_preguntas_difpromedio" style="font-size: 30px" runat="server" class="help" title="Dificultad promedio de las preguntas que cumplen las reglas."></span>
                                    <br />
                                    <span id="smy_test_dificultad" class="help" runat="server" title="Dificultad del Test."></span>
                                </div>
                                <label class="col-sm-12 text-center" runat="server">Difi. Promedio</label>
                            </div>

                            <div class="card">
                                <div id="dif1_container" runat="server" class="col-sm-12 card-value text-center" style="background-color: #1de9b6">
                                    <span id="smy_dif1" style="font-size: 30px" runat="server" class="help" title="Cantidad de preguntas con dificultad 1 que cumplen las reglas."></span>
                                    <br />
                                    <span id="smy_dif1_percent" runat="server" class="help" title="Porciento que representan las preguntas con dificultad 1 del total de preguntas que cumplen las reglas."></span>
                                </div>
                                <label class="col-sm-12 text-center" runat="server">Dificultad 1</label>
                            </div>

                            <div class="card">
                                <div id="dif2_container" runat="server" class="col-sm-12 card-value text-center" style="background-color: #c6ff00">
                                    <span id="smy_dif2" style="font-size: 30px" runat="server" class="help" title="Cantidad de preguntas con dificultad 2 que cumplen las reglas."></span>
                                    <br />
                                    <span id="smy_dif2_percent" runat="server" class="help" title="Porciento que representan las preguntas con dificultad 2 del total de preguntas que cumplen las reglas."></span>
                                </div>
                                <label class="col-sm-12 text-center" runat="server">Dificultad 2</label>
                            </div>

                            <div class="card">
                                <div id="dif3_container" runat="server" class="col-sm-12 card-value text-center" style="background-color: #ffea00">
                                    <span id="smy_dif3" style="font-size: 30px" runat="server" class="help" title="Cantidad de preguntas con dificultad 3 que cumplen las reglas."></span>
                                    <br />
                                    <span id="smy_dif3_percent" runat="server" class="help" title="Porciento que representan las preguntas con dificultad 3 del total de preguntas que cumplen las reglas."></span>
                                </div>
                                <label class="col-sm-12 text-center" runat="server">Dificultad 3</label>
                            </div>

                            <div class="card">
                                <div id="dif4_container" runat="server" class="col-sm-12 card-value text-center" style="background-color: #ff9100">
                                    <span id="smy_dif4" style="font-size: 30px" runat="server" class="help" title="Cantidad de preguntas con dificultad 4 que cumplen las reglas."></span>
                                    <br />
                                    <span id="smy_dif4_percent" runat="server" class="help" title="Porciento que representan las preguntas con dificultad 4 del total de preguntas que cumplen las reglas."></span>
                                </div>
                                <label class="col-sm-12 text-center" runat="server">Dificultad 4</label>
                            </div>

                            <div class="card">
                                <div id="dif5_container" runat="server" class="col-sm-12 card-value text-center" style="background-color: #ff3d00">
                                    <span id="smy_dif5" style="font-size: 30px" runat="server" class="help" title="Cantidad de preguntas con dificultad 5 que cumplen las reglas."></span>
                                    <br />
                                    <span id="smy_dif5_percent" runat="server" class="help" title="Porciento que representan las preguntas con dificultad 5 del total de preguntas que cumplen las reglas."></span>
                                </div>
                                <label class="col-sm-12 text-center" runat="server">Dificultad 5</label>
                            </div>
                        </div>
                        <div class="col-sm-12 form-group">
                            <div class="col-sm-1 text-center">
                                <button class="btn btn-primary" style="padding: 6px;" type="button" onclick="replicar()">Replicar</button>
                            </div>
                            <div class="col-sm-3 text-center">
                                <span style="font-size: 16px; font-weight: 600;">Curso
                                </span>
                            </div>
                            <div class="col-sm-3 text-center">
                                <span style="font-size: 16px; font-weight: 600;">Área
                                </span>
                            </div>
                            <div class="col-sm-3 text-center">
                                <span style="font-size: 16px; font-weight: 600;">Tema
                                </span>
                            </div>
                        </div>
                        <div id="lab" class="col-sm-12 form-group" runat="server"></div>
                        <asp:HiddenField ID="hidId" runat="server" />
                        <%--<asp:ImageButton ID="btnBorrar" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" OnClick="btnBorrar_Click" class="hidden" />--%>
                        <!-- Fin hidden y botones ocultos -->

                        <div class="col-sm-12">
                            <a href="test_test.aspx" runat="server" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                            <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right"
                                runat="server" Text="Guardar" OnClientClick="return validarFormularioM();" OnClick="btnGuardar_Click" />
                        </div>
                    </form>
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
            $('#tabla').DataTable({
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
                  },
                  {
                      "targets": [1],
                      "type": "clear-string"
                  },
                  {
                      "targets": [2]
                  },
                  {
                      "targets": [3]
                  }
                ],
                "order": [[0, "asc"]]
            });
        });

        function eliminar(orden) {
            <%--var hidId = document.getElementById('<%=hidId.ClientID %>');
            hidId.value = orden;

            var boton = document.getElementById('<%=btnBorrar.ClientID %>');
            boton.click();--%>
        }

        function replicar() {
            var cursosselect = $("select.selectcurso");
            var divcursosselect = $("div.selectcurso span.filter-option");
            var countpregunta = $("#preg_1").html();
            //console.log(valueref);
            //console.log($('#' + cursosselect[0].id + " option:selected").text());
            var textref = divcursosselect[0].innerHTML;
            var valueref = $("#" + cursosselect[0].id).val();
            for (var i = 1; i < cursosselect.length; i++) {
                $('#' + cursosselect[i].id).val(valueref);
                divcursosselect[i].innerHTML = textref;
                $("#preg_"+(i+1)).html(countpregunta);
            }

            var catselect = $("select.selectcat");
            var divcatselect = $("div.selectcat span.filter-option");

            var textref = divcatselect[0].innerHTML;
            var valueref = $("#" + catselect[0].id).val();
            for (var i = 1; i < catselect.length; i++) {
                $('#' + catselect[i].id).val(valueref);
                divcatselect[i].innerHTML = textref;
            }

            var subcatselect = $("select.selectsubcat");
            var divsubcatselect = $("div.selectsubcat span.filter-option");

            var textref = divsubcatselect[0].innerHTML;
            var valueref = $("#" + subcatselect[0].id).val();
            for (var i = 1; i < subcatselect.length; i++) {
                $('#' + subcatselect[i].id).val(valueref);
                divsubcatselect[i].innerHTML = textref;
            }
        }
    </script>
</body>
</html>


