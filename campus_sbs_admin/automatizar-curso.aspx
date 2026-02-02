<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="automatizar-curso.aspx.cs" Inherits="campus_sbs_admin.automatizar_curso" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Automatización de un curso</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <style type="text/css">
         .checkbox .form-control {display: inline-block; height: 30px; margin: 0 5px; padding: 6px; vertical-align: middle; width: 5%;}
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
    		
	<section class="wrapper">
		<div class="padding-nav">	
            <div class="col-sm-12 padding-t-10">                         
                <form id="Form1" accept-charset="utf-8" runat="server">
                    <div>
                        <fieldset>
							<legend id="txt_title" class="text-color-primary" runat="server"></legend>
                        </fieldset>
                        <fieldset>
							<legend id="txt_tasks" class="text-color-primary" runat="server"><i class="fas fa-cogs"></i>  Tareas planificadas</legend>
                            <div id="table_listado_tasks" class="col-sm-12" runat="server"></div>
                        </fieldset>
                        <fieldset class="margin-t-20">
                            <legend id="txt_title_foro" class="text-color-primary" runat="server"><i class="fas fa-comments"></i> Foro <a id="lnk_foro" href="#" title="Nuevo Foro" class="pull-right bold padding-r-5" runat="server"><small class="text-color-primary"><i class="fas fa-plus-circle fa-2x"></i> Foro</small></a></legend>
							<div id="table_list_foros" class="col-sm-12" runat="server"></div>
                        </fieldset>
                        <fieldset class="margin-t-20">
							<legend class="text-color-primary"><i class="fas fa-comment-dots"></i> Mensaje Foro <a id="link_msg_foro" href="#" title="Nuevo Mensaje foro" class="pull-right bold padding-r-5" runat="server"><small class="text-color-primary"><i class="fas fa-plus-circle fa-2x"></i> Mensaje foro</small></a></legend>
                            <div id="table_list_msg_foro" class="col-sm-12" runat="server"></div>
                        </fieldset>
                        <fieldset class="margin-t-20">
							<legend class="text-color-primary"><i class="fas fa-envelope"></i> Mensaje <a id="link_msg" href="#" title="Nuevo Mensaje" class="pull-right bold padding-r-5" runat="server"><small class="text-color-primary"><i class="fas fa-plus-circle fa-2x"></i> Mensaje</small></a></legend>
                            <div id="table_list_msg" class="col-sm-12" runat="server"></div>
                        </fieldset>
                        <fieldset id="block_content" class="margin-t-20">
							<legend class="text-color-primary"><i class="fas fa-book-open"></i> Contenido <a href="javascript:void(0)" title="Ver contenido del curso" class="pull-right bold padding-r-5" onclick="mostrar_bloque()" runat="server"><small class="text-color-primary"><i id="lbl_content" class="fas fa-eye fa-2x"></i></small></a></legend>
                            <div id="block_contenido" class="hidden">                            
                                <div id="block_contents" class="col-sm-12" runat="server"></div>
                                <div class="col-sm-12">
                                    <asp:Button ID="btnGuardarContenido" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClick="btnGuardarContenido_Click" />
                                </div>
                            </div>
                        </fieldset>
                        <fieldset class="margin-t-20">
							<legend class="text-color-primary"><i class="fas fa-file"></i> Caso Práctico <a id="link_cp" href="#" title="Nuevo Caso Práctico" class="pull-right bold padding-r-5" runat="server"><small class="text-color-primary"><i class="fas fa-plus-circle fa-2x"></i> Caso Práctico</small></a></legend>
                            <div id="table_list_cp" class="col-sm-12" runat="server"></div>
                        </fieldset>
                        <div class="col-sm-12">
                            <asp:HiddenField ID="hidTask" Value="" runat="server" />
                            <asp:Button ID="btnEliminar" CssClass="hidden" runat="server" OnClick="btnEliminar_Click" />
                        </div>
                    </div>
                </form>
            </div>							
		</div>
    </section>

   <!-- Scripts
    =================================================== --> 
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.dateFormat.js"></script>

    <script type="text/javascript">    
        $(document).ready(function () {
            $('#tabla_Tareas_Auto').DataTable({
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
                lengthMenu: [[10, 25, -1], [10, 25, "Todos"]],
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
                  },
                  {
                      "targets": [3]
                  },
                  {
                      "targets": [4],
                      "class": "text-center"
                  },
                  {
                      "targets": [5],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"]]
            });
            $('#tabla_Tareas_Auto_Foro').DataTable({
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
                lengthMenu: [[10, 25, -1], [10, 25, "Todos"]],
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
                "order": [[0, "asc"]]
            });
            $('#tabla_Tareas_Auto_MSG_Foro').DataTable({
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
                lengthMenu: [[10, 25, -1], [10, 25, "Todos"]],
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
                  },
                  {
                      "targets": [3]
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
                "order": [[0, "asc"]]
            });
            $('#tabla_Tareas_Auto_MSG').DataTable({
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
                lengthMenu: [[10, 25, -1], [10, 25, "Todos"]],
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
                  }
                ],
                "order": [[0, "asc"]]
            });
            $('#tabla_Tareas_Auto_CP').DataTable({
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
                lengthMenu: [[10, 25, -1], [10, 25, "Todos"]],
                "columnDefs": [
                  {
                      "targets": [0],
                      "class": "text-center"
                  },
                  {
                      "targets": [1],
                      "class": "text-center"
                  },
                  {
                      "targets": [2],
                      "type": "eu_date"
                  },
                  {
                      "targets": [3]
                  },
                  {
                      "targets": [4],
                      "class": "text-center"
                  },
                  {
                      "targets": [5],
                      "class": "text-center"
                  }
                ],
                "order": [[0, "asc"]]
            });
        });
        function eliminarAccion(id) {
            $('#hidTask').val(id);
            $('#btnEliminar').click();
        }        
        function mostrar_bloque() {
            if ($('#lbl_content').hasClass('fas fa-eye fa-2x')) {
                $('#block_contenido').removeClass("hidden");
                $('#lbl_content').removeClass("fas fa-eye fa-2x");
                $('#lbl_content').addClass("fas fa-eye-slash fa-2x");
            }
            else {
                $('#block_contenido').addClass("hidden");
                $('#lbl_content').removeClass("fas fa-eye-slash fa-2x");
                $('#lbl_content').addClass("fas fa-eye fa-2x");
            }
        }
    </script>
</body>
</html>