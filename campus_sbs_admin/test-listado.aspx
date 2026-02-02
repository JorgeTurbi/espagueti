<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test-listado.aspx.cs" Inherits="campus_sbs_admin.test_listado" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Listado Tests</title>

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
        
        #btn_upload > img {cursor: pointer;}
        #fileinput > span {white-space: normal;}
        #txt_comentarios, #txt_descripcion {max-height: 350px;}
        .btn.fileinput-button {border: 1px solid #ccc; height: 150px; margin-bottom: 5px; overflow: hidden; text-align: left; width: 100%;}
        .fileinput-button input {cursor: pointer; direction: ltr; height: 150px; left: -4px; margin: 0; opacity: 0; position: absolute; right: 0; top: 0; width: 100%;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}   
        
        .modal-dialog-scrollable{display:-ms-flexbox;display:flex;max-height:calc(100% - 1rem); width: auto;}
        .modal-dialog-scrollable .modal-content{max-height:calc(100vh - 10rem);overflow:hidden; overflow-y: auto;}
        .modal-dialog-scrollable .modal-footer,.modal-dialog-scrollable .modal-header{-ms-flex-negative:0;flex-shrink:0}
        .modal-dialog-scrollable .modal-body{overflow-y:auto}
        .modal-dialog-centered{display:-ms-flexbox;display:flex;-ms-flex-align:center;align-items:center;min-height:calc(100% - 1rem)}
        .modal-dialog-centered::before{display:block;height:calc(100vh - 1rem);content:""}
        .modal-dialog-centered.modal-dialog-scrollable{-ms-flex-direction:column;flex-direction:column;-ms-flex-pack:center;justify-content:center;height:100%;}
        .modal-dialog-centered.modal-dialog-scrollable::before{content:none}
        
        .modal-header{display:-ms-flexbox;display:flex;-ms-flex-align:start;align-items:flex-start;-ms-flex-pack:justify;justify-content:space-between;padding:1rem 1rem;border-bottom:1px solid #dee2e6;border-top-left-radius:.3rem;border-top-right-radius:.3rem}
        .modal-header .close{padding:1rem 1rem;margin:-1rem -1rem -1rem auto}
        .modal-title{margin-bottom:0;line-height:1.5}
        .modal-body{position:relative;-ms-flex:1 1 auto;flex:1 1 auto;padding:1rem}
        .modal-footer{display:-ms-flexbox;display:flex;-ms-flex-align:center;align-items:center;-ms-flex-pack:end;justify-content:flex-end;padding:1rem;border-top:1px solid #dee2e6;border-bottom-right-radius:.3rem;border-bottom-left-radius:.3rem}
               
        .form-control-sm {border-radius: 0.2rem; font-size: 0.875rem; height: calc(1.5em + 1rem + 2px); line-height: 1.5; padding: 0.25rem 0.5rem;}
        .d-inline-block {display: inline-block !important;}
        .align-middle {vertical-align: middle !important;}
        .pl-2, .px-2 {padding-left: 0.5rem !important;}
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
    <style>
        .badge-primary {
            color: #fff;
            background-color: #007bff;
        }

        .badge {
            display: inline-block;
            padding: .25em .4em;
            font-weight: 600;
            line-height: 1;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: .25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }
        .badge-dark {background-color: rgb(52, 58, 64); color: rgb(255, 255, 255);}
        .test-id {
            cursor: pointer;
        }

        .table-danger, .table-danger > td, .table-danger > th {
            background-color: #f5c6cb;
        }
    </style>
</head>
<body>
    <uc_menu:menu ID="menu" runat="server" />
    <header id="header" class="bg-color-primary affix">
        <uc_header:cabecera ID="cabecera" runat="server" />
    </header>

    <main class="wrapper public bg-color-white" role="main">     	    
        <section class="padding-tb-50">
		    <div class="row no-margin padding-nav" style="min-height: 100vh;">	
                <div class="col-sm-12">                         
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div>
                            <fieldset>
							    <legend id="txt_title" class="text-color-primary" runat="server"><i class='far fa-list-alt'></i> Listado de test <span id="lbl_min" class="badge badge-pill badge-secondary badge-comments" data-toggle="tooltip" data-placement="top" data-html="true" title="" data-original-title="Minutos" runat="server" /></legend>
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
                            </fieldset>
                        </div>
                        <div>
                            <asp:HiddenField ID="hidIdUser" runat="server" />
                            <asp:HiddenField ID="hidIntento" runat="server" />
                            <asp:ImageButton ID="btnBorrar" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" OnClick="btnBorrar_Click" CssClass="hidden" />
                        </div>
                    </form>
                    <div id="table_listado_examenes" class="col-sm-12" runat="server"></div>
                    <div class="modal fade" id="modal_mail" tabindex="-1" aria-labelledby="modal_mail_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-large modal-dialog-centered modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_mail_title">Huella del test</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">×</span>
                                    </button>
                                </div>
                                <div id="modal_body_mail" class="modal-body"></div>
                                <div class="modal-footer">
                                    <button type="button" class="btn" data-dismiss="modal">Cerrar</button>
                                </div>
                            </div>
                        </div>
                    </div>
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
                      "targets": [0],
                      "class": "text-center",
                      "type": "eu_date"
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
                      "class": "text-center",
                      "type": "euro_date"
                  },
                  {
                      "targets": [5],
                      "class": "text-center",
                      "type": "euro_date"
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
                  }
                ],
                "order": [[0, "desc"], [4, "desc"]],
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    /// Media 
                    media = api.column(6)
                        .data()
                        .reduce(function (a, b) {
                            return parseFloat(a) + parseFloat(b.toString());
                        }, 0);

                    var _media = media / data.length;

                    /// Update footer
                    $(api.column(6).footer()).html(_media.toFixed());                    
                }
            });
        });

        function eliminar_test(usuario, intento) {
            $('#hidIdUser').val(usuario);
            $('#hidIntento').val(intento);
            $('#btnBorrar').click();
        }
        function add_huella(huella) {
            /// 1.- Cuerpo
            $('#modal_body_mail').html(huella);

            /// 2.- Mostrar el modal
            $('#modal_mail').modal('show');
        }
    </script>
</body>
</html>