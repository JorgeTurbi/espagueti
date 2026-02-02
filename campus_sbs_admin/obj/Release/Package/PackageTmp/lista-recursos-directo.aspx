<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lista-recursos-directo.aspx.cs" Inherits="campus_sbs_admin.lista_recursos_directo" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Listas de recursos en directo</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <style type="text/css">
         .bootstrap-select > .btn.btn-default { font-size: 15px;padding: 13px;}         
     </style>

    <style type="text/css">
        .input-group .form-control {background-color: white; border: 1px solid #bdbdbd; color: black;}
        .input-group.date.js-datepicker { width: 100%;}
        .input-group.has-error .form-control {background: #fbf2f1 none repeat scroll 0 0; border: 1px solid #a94442; color: #f2958d;}
        .input-group.has-error .form-control::-moz-placeholder {color: #f2958d; opacity: 1;}
        .checkbox img {height: 25px; width: 25px;}
        
        #btn_upload > img {cursor: pointer;}
        #fileinput > span {white-space: normal;}
        #txt_comentarios, #txt_descripcion {max-height: 350px;}
        .btn.fileinput-button {border: 1px solid #ccc; height: 150px; margin-bottom: 5px; overflow: hidden; text-align: left; width: 100%;}
        .fileinput-button input {cursor: pointer; direction: ltr; height: 150px; left: -4px; margin: 0; opacity: 0; position: absolute; right: 0; top: 0; width: 100%;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}   
        
        .modal.in .modal-dialog {transform: translate(0px, 15%);}
        @media (min-width:576px){.modal-dialog{max-width:500px;margin:1.75rem auto}.modal-dialog-large{max-width: calc(100vw - 10%);}.modal-dialog-centered{min-height:calc(100% - 3.5rem)}.modal-dialog-centered::before{height:calc(100vh - 3.5rem)}}
        @media (min-width: 768px) {
            .modal-dialog {
                width: calc(100vw - 10%);
                margin: 30px auto;
            }
        }
        .d-flex {
            display: flex;
        }
        .d-inline-flex {
            display: inline-flex;
        }
        .modal-header .btn-close {
            padding: calc(var(--bs-modal-header-padding-y) * .5) calc(var(--bs-modal-header-padding-x) * .5);
            margin: calc(-.5 * var(--bs-modal-header-padding-y)) calc(-.5 * var(--bs-modal-header-padding-x)) calc(-.5 * var(--bs-modal-header-padding-y)) auto;
        }
        .modal-open .modal {overflow-y: hidden;}

        /* Fondo oscuro */
        #loader {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.8); /* Fondo oscuro con transparencia */
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 1000;
        }

        /* Círculo giratorio */
        #spinner {
            width: 50px;
            height: 50px;
            border: 5px solid #f3f3f3; /* Color claro del círculo */
            border-top: 5px solid #3498db; /* Color de la animación */
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        /* Animación de giro */
        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

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
            <div class="block-primary">
                <div class="row no-margin padding-nav">
                    <div class="col-sm-12">
                        <fieldset>
                            <legend id="txt_recursos" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_recursos" class="col-sm-12" runat="server"></div>

                    <!-- Hidden y botones ocultos -->
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <asp:HiddenField ID="hidIdRecurso" runat="server" />
                        <asp:ImageButton ID="btnActivarRecurso" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnActivarRecurso_Click" />
                        <asp:ImageButton ID="btnBorrarRecurso" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnBorrarRecurso_Click" />
                    </form>
                    <!-- Fin hidden y botones ocultos -->
                    
                    <div class="modal fade" id="modal_rec_int" tabindex="-1" role="dialog" aria-labelledby="modal_rec_int_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title d-flex d-inline-flex" id="modal_rec_int_title">Listado Alumnos</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">                                    
                                    <table id="tbl_rec_int" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="loader" style="display: none;">
                        <div id="spinner"></div>
                    </div>

                </div>
            </div>
        </section>
    </main>

   <!-- Scripts
    =================================================== --> 
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/jquery-ui.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.dateFormat.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.cleanString.js"></script>
    
    <script type="text/javascript">    
        $(document).ready(function () {
            $('#tabla_Recursos').DataTable({
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
                      "type": "eu_date"
                  },
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
                      "targets": [5]
                  },
                  {
                      "targets": [6],
                      "class": "text-center c-pointer"
                  },
                  {
                      "targets": [7],
                      "class": "text-center c-pointer"
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
                  },
                  {
                      "targets": [14],
                      "class": "text-center"
                  },
                  {
                      "targets": [15],
                      "class": "text-center"
                  },
                  {
                      "targets": [16],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "desc"]]
            });
        });

        function activarRecurso(id) {
            $('#hidIdRecurso').val(id);
            $('#btnActivarRecurso').click();
        }
        function eliminarRecurso(id) {
            $('#hidIdRecurso').val(id);
            $('#btnBorrarRecurso').click();
        }
        function MostrarListadoAlumnos(idRecursoDirecto, all) {

            /// Mostrar cargando
            showLoader();

            $.ajax({
                url: 'lista-recursos-directo.aspx/search_subtable',
                data: "{'idRecursoDirecto':" + idRecursoDirecto + ", 'todos': " + all + "}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#tbl_rec_int').html(data.d);
                    paint_table();
                    hideLoader();
                    $('#modal_rec_int').modal('show');
                },
                error: function (response) {
                    hideLoader();
                    alert(response.responseText);
                },
                failure: function (response) {
                    hideLoader();
                    alert(response.responseText);
                }
            });
        }
        function paint_table() {
            $('#tabla_Usuarios').DataTable({
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
                      "class": "text-center"
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2],
                      "class": "text-center",
                      "type": "euro_date"
                  }
                ],
                "order": [1, "asc"]
            });
        }

        function showLoader() {
            document.getElementById("loader").style.display = "flex";
        }
        function hideLoader() {
            document.getElementById("loader").style.display = "none";
        }
    </script>
</body>
</html>