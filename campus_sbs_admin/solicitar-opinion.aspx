<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="solicitar-opinion.aspx.cs" Inherits="campus_sbs_admin.solicitar_opinion" ValidateRequest="false" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Solicitud de opinión</title>

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
        
        #txt_comentarios, #txt_descripcion {max-height: 350px;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}       
     </style>

    <!-- CSS para el control DataTable -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />
	    
	 <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700,900%7COpen+Sans:400,600,700" rel="stylesheet" type="text/css" />

    <!-- Modernizr -->	
     <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>

    <!-- CKeditor -->
     <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>

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
        <section class="padding-tb-50">
		    <div class="row no-margin padding-nav">	
                <div class="col-sm-12">                         
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div>
                            <fieldset>
							    <legend id="title_solicitud" class="text-color-primary" runat="server"></legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-6">
                                    <label>Tipo de solicitud</label>													
								    <div id="tipo_solicitud_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlTipoSolicitud">Contacto Empresa</label>
                                        <select  id="ddlTipoSolicitud" class="selectpicker" data-live-search="true" data-hide-disabled="true" runat="server"></select>
								    </div>
                                </div>
                                <div class="col-sm-3 text-center">
                                    <a id="btn_send_mail" href="javascript:void(0);" onclick="mostrar_datos_mail(true);" class="btn btn-primary bg-color-text-soft text-color-white btn-block-xs margin-t-15">Enviar mail</a>
                                </div>
                                <div class="col-sm-3 text-center">
                                    <asp:Button ID="btn_link" CssClass="btn btn-primary bg-color-info text-color-white btn-block-xs margin-t-15" runat="server"
                                        Text="Ver link" OnClientClick="return validarFormulario();" OnClick="btn_link_Click" />
                                    <asp:Button ID="btn_ver" CssClass="hidden" runat="server" Text="Ver solicitudes" OnClick="btn_ver_Click" />
                                </div>
                                <div id="block_all" class="hidden" runat="server">
                                    <div class="col-sm-12 text-color-primary padding-tb-10 fa-1-6x"><i class="fas fa-envelope"></i> Datos del mail</div>
                                    <div class="clearfix"></div>
                                    <div class="col-sm-6">
                                        <label>Nombre From</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_nombre_from">Nombre From</label>
									        <input type="text" placeholder="Nombre From" id="txt_nombre_from" class="form-control" runat="server" maxlength="250" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Mail From</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_mail_from">Mail From</label>
									        <input type="text" placeholder="Mail From" id="txt_mail_from" class="form-control" runat="server" maxlength="250" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Reply To</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_reply_to">Reply To</label>
									        <input type="text" placeholder="Reply To" id="txt_reply_to" class="form-control" runat="server" maxlength="250" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Asunto *</label>
                                        <div id="asunto_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_asunto">Asunto *</label>
									        <input type="text" placeholder="Asunto *" id="txt_asunto" class="form-control" runat="server" maxlength="500" />										
								        </div>
                                    </div>
                                    <div id="blk_body" class="col-sm-12 margin-b-15" runat="server">
                                        <label>Cuerpo Mail *</label>											
								        <div id="cuerpo_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_cuerpo">Cuerpo Mail *</label>
									        <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                            <textarea id="txt_cuerpo" runat="server" placeholder="Cuerpo Mail *" name="txtCuerpo" cols="80" rows="10" />
									        <script type="text/javascript">
									            CKEDITOR.replace('txt_cuerpo',
                                                {
                                                    placeholder: 'Cuerpo Mail'
	                                            });
                                            </script>
								        </div>
                                    </div>
                                    <div class="clear-both"></div>                                    
                                    <div class="col-sm-12">
                                        <a href="javascript:void(0);" onclick="mostrar_datos_mail(false);" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                        <asp:Button ID="btn_send" CssClass="btn btn-primary bg-color-text-soft text-color-white btn-block-xs pull-right" runat="server"
                                            Text="Enviar mail" OnClientClick="return validarFormularioMail();" OnClick="btn_send_Click" />

                                        
                                    </div>
                                </div>
                                <div id="table_listado_links" class="col-sm-12 padding-tb-10" runat="server"></div>
                                <div class="col-sm-12">
                                    <a id="btn_back" href="lista-opiniones.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>
                                </div>
                                <div id="table_listado_solicitudes" class="col-sm-12 padding-tb-20" runat="server"></div>
                            </fieldset>
                        </div>
                    </form>
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
            $('#tabla_Listados').DataTable({
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
                      "targets": [2]
                  }
                ],
                "order": [[0, "asc"]]
            });

            $('#tabla_Listados_Solicitudes').DataTable({
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
                  }
                ],
                "order": [[0, "desc"]]
            });
        });

        function mostrar_datos_mail(ocultar_mostrar) {
            if (ocultar_mostrar) {
                if ($('#block_all').hasClass('hidden'))
                    $('#block_all').removeClass("hidden");

                if (!$('#btn_link').hasClass('hidden'))
                    $('#btn_link').addClass('hidden');
                if (!$('#btn_send_mail').hasClass('hidden'))
                    $('#btn_send_mail').addClass('hidden');
                if (!$('#btn_back').hasClass('hidden'))
                    $('#btn_back').addClass('hidden');                
            }
            else {
                if (!$('#txt_file_anexo').hasClass('hidden'))
                    $('#txt_file_anexo').addClass('hidden');

                if ($('#btn_link').hasClass('hidden'))
                    $('#btn_link').removeClass('hidden');
                if ($('#btn_send_mail').hasClass('hidden'))
                    $('#btn_send_mail').removeClass('hidden');
                if ($('#btn_back').hasClass('hidden'))
                    $('#btn_back').removeClass('hidden');
            }
        }

        function ver_solicitudes() {
            var ver_todas = document.getElementById('<%=btn_ver.ClientID %>');
            ver_todas.click();
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function validarFormulario() {
            /// 1.- Sacar los parametros
            var solicitud = $('#ddlTipoSolicitud').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (solicitud == '-1' || solicitud == -1) {
                $('#tipo_solicitud_form').addClass(' has-error');
                $('#txt_error').html('El tipo de solicitud es obligatoria');
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }
        function validarFormularioMail() {
            /// 1.- Sacar los parametros
            var solicitud = $('#ddlTipoSolicitud').val();
            var asunto = $('#txt_asunto').val();
            var cuerpo = $('#txt_cuerpo').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (solicitud == '-1' || solicitud == -1) {
                $('#tipo_solicitud_form').addClass(' has-error');
                $('#txt_error').html('El tipo de solicitud es obligatoria');
                subirArribaPagina();
                return false;
            }
            else if (asunto === "undefined" || asunto === undefined || asunto === "null" || asunto === null || asunto === '') {
                $('#asunto_form').addClass(' has-error');
                $('#txt_error').html('El campo asunto es obligatorio');
                $('#txt_asunto').attr("placeholder", "El campo asunto es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (cuerpo === "undefined" || cuerpo === undefined || cuerpo === "null" || cuerpo === null || cuerpo === '') {
                $('#cuerpo_form').addClass(' has-error');
                $('#txt_error').html('El campo cuerpo es obligatorio');
                $('#txt_comentarios').attr("placeholder", "El campo cuerpo es obligatorio");
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }
    </script> 
</body>
</html>