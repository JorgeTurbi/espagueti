<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lista-opiniones.aspx.cs" Inherits="campus_sbs_admin.lista_opiniones" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Listas de opiniones</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <style type="text/css">
         .bootstrap-select > .btn.btn-default { font-size: 15px;padding: 13px;}
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
                    <div class="col-sm-12 margin-b-15">
                        <form id="Form1" accept-charset="utf-8" runat="server">
                            <fieldset>
                                <legend id="txt_buscar_opiniones" class="text-color-primary" runat="server"></legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-5">
                                    <label>Alumno</label>
								    <div id="alumno_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_alumno">Alumno</label>
									    <input type="text" placeholder="Alumno" id="txt_alumno" autocomplete="off" class="form-control" runat="server" />
                                        <input id="idAlumno" type="hidden" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
								    </div>
                                </div>
                                <div class="col-sm-5">
                                    <label>Programa</label>
                                    <div id="programa_form" class="form-group" runat="server">                                       
                                        <asp:DropDownList ID="ddlPrograma" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-2">
                                    <label></label>
                                    <div>
                                        <asp:Button ID="btnBuscar" CssClass="btn btn-primary btn-block-xs margin-b-10" runat="server"
                                            Text="Buscar" OnClick="btnBuscar_Click" />
                                    </div>
                                </div>

                                <!-- Hidden y botones ocultos -->
                                <asp:HiddenField ID="hidIdOpinion" runat="server" />
                                <asp:HiddenField ID="hidPendientes" runat="server" />
                                <asp:ImageButton ID="btnActivarOpinion" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnActivarOpinion_Click" />
                                <asp:ImageButton ID="btnDesactivarOpinion" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnDesactivarOpinion_Click" />
                                <asp:ImageButton ID="btnOpinionesPendientes" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnOpinionesPendientes_Click" />
                                <asp:ImageButton ID="btnOpinionesActivas" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnOpinionesActivas_Click" />
                                <!-- Fin hidden y botones ocultos -->
                            </fieldset>
                        </form>
                    </div>
                    <div class="col-sm-12">
                        <fieldset>
                            <legend id="txt_lista_opiniones" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_opiniones" class="col-sm-12" runat="server"></div>
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
            $('#tabla_Opiniones').DataTable({
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
                      "type": "clear-string"
                  },
                  {
                      "targets": [2]
                  },
                  {
                      "targets": [3],
                      "class": "text-center"
                  },
                  {
                      "targets": [4]
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
                "order": [[0, "desc"]]
            });

            /// 3.- Recuperar la tecla pulsada
            if ($("#txt_alumno").val() === "") {
                $("#txt_alumno").focus();
            }
            $("#txt_alumno").on('keypress', $(this), function (e) {
                if (e.which === 13 && $(this).val() === "") {
                    return false;
                }
            });

            /// 4.- Autocompletar
            $("#txt_alumno").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'lista-opiniones.aspx/search_student',
                        data: "{ 'name': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.nombre_completo,
                                    val: item.id_usuario
                                }
                            }))
                        },
                        error: function (response) {
                            alert("La consulta es poco restrictiva, escriba más caracteres o restringe más su búsqueda");
                        },
                        failure: function (response) {
                            alert("La consulta es poco restrictiva, escriba más caracteres o restringe más su búsqueda");
                        }
                    });
                },
                select: function (e, ui) {
                    $('#idAlumno').val(ui.item.val);
                },
                minLength: 3
            });
        });

        function activarOpinion(id) {
            var hidId = document.getElementById('<%=hidIdOpinion.ClientID %>');
            hidId.value = id;

            var boton = document.getElementById('<%=btnActivarOpinion.ClientID %>');
            boton.click();
        }

        function desactivarOpinion(id) {
            var hidId = document.getElementById('<%=hidIdOpinion.ClientID %>');
            hidId.value = id;

            var boton = document.getElementById('<%=btnDesactivarOpinion.ClientID %>');
            boton.click();
        }

        function opiniones_pendientes() {
            var hidId = document.getElementById('<%=hidPendientes.ClientID %>');
            hidId.value = 1;

            var boton = document.getElementById('<%=btnOpinionesPendientes.ClientID %>');
            boton.click();
        }

        function opiniones_activas() {
            var hidId = document.getElementById('<%=hidPendientes.ClientID %>');
            hidId.value = 0;

            var boton = document.getElementById('<%=btnOpinionesActivas.ClientID %>');
            boton.click();
        }        
    </script>
</body>
</html>