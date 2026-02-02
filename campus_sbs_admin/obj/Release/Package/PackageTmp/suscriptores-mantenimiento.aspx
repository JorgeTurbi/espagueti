<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="suscriptores-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.suscriptores_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Lista de suscriptores mantenimiento</title>

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
        
        #txt_comentarios {max-height: 350px;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}       
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
        <section class="padding-tb-50">
		    <div class="row no-margin padding-nav">	
                <div class="col-sm-12">                         
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div>
                            <fieldset>
							    <legend class="text-color-primary"><i class='far fa-list-alt'></i> Manteniento de Lista de suscriptores <a href="https://media.spainbs.com/recursos_www/campus_upload/Plantilla_Excel_Suscripcion.xlsx" class='pull-right bold padding-r-5' target="_blank"><small class='text-color-primary'><i class="far fa-file-excel fa-2x text-color-green"></i> Plantilla Excel</small></a></legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-12">
                                    <label>Subir excel</label>
								    <div id="upload_form" class="form-group" runat="server">
                                        <asp:FileUpload ID="fuFile" ToolTip="Fichero" runat="server" />
                                        <label id="txt_name_file" runat="server" />
								    </div>
                                </div>                                
                                <div class="col-sm-12">
                                    <a id="btn_back" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>
                                    <asp:HiddenField ID="name_file" runat="server" />
                                    <asp:Button ID="btnCargar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Cargar" OnClick="btnCargar_Click" />	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" Visible="false" OnClick="btnGuardar_Click" />
                                </div>                                
                            </fieldset>
                        </div>
                    </form>
                </div>
                <div id="block_suscriptores" class="col-sm-12 hidden" runat="server">
                    <fieldset>
					    <legend class="text-color-primary"><i class='far fa-list-alt'></i> Lista de suscriptores</legend>
                    </fieldset>            
                </div>
                <div id="table_suscriptores" class="col-sm-12" runat="server"></div>
                <div id="table_resultados" class="col-sm-12" runat="server"></div>							
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

    <script type="text/javascript">    
        $(document).ready(function () {
            $('#tabla_suscriptores').DataTable({
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
                      "class": "text-center"
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2]
                  }
                ]
            });
        });
    </script>
</body>
</html>
