<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="automatizar-curso-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.automatizar_curso_mantenimiento" ValidateRequest="false" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Mantenimiento de automatización de curso</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    
    <!-- CSS para el control DataTable -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />

     <style>
         #btn_upload > img {cursor: pointer;}
         #fileinput > span {white-space: normal;}
         #txt_body {max-height: 350px;}
         .btn.fileinput-button {border: 1px solid #ccc; height: 150px; margin-bottom: 5px; overflow: hidden; text-align: left; width: 100%;}
         .fileinput-button input {cursor: pointer; direction: ltr; height: 150px; left: -4px; margin: 0; opacity: 0; position: absolute; right: 0; top: 0; width: 100%;}

         .modal-dialog.modal-dialog-large.modal-dialog-centered {top: 20vh;}
         .bootstrap-select > .btn.btn-default {font-size: 13px; padding: 13px;}
         #modal_adjunto_title {float: left; width: 90%;}
         .date {background: #cdcdcd none repeat scroll 0 0;}
         .input-group-addon {width: 1%;}
         .input-group-addon::before {top: 15px;}
     </style>

	 <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700,900%7COpen+Sans:400,600,700" rel="stylesheet" type="text/css" />

    <!-- Modernizr -->	
     <script type="text/javascript" src="App_Themes/support/js/modernizr.js" async></script>

     <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>

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
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div id="block_upload" class="col-sm-12" runat="server">
                            <fieldset>
						        <legend id="txt_title" class="text-color-primary" runat="server"></legend>
                                <div class="has-error">
                                    <h3 id="txt_error" class="h4 help-block text-center" runat="server"></h3>
                                </div>
                                <div id="block_msg" class="hidden" runat="server">
                                    <div class="col-sm-6">       
                                        <label>Asunto del mensaje</label>
                                        <div id="title_foro_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_asunto">Asunto del mensaje</label>
                                            <input type="text" placeholder="Asunto del mensaje" id="txt_asunto" class="form-control" runat="server" />
								        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label>Nº días desde F. Apertura</label>
                                        <div id="tipo_dias_form" class="form-group" runat="server">  
                                            <label class="sr-only" for="txt_dias_foro">Nº días desde F. Apertura</label>                                     
                                            <input type="text" id="txt_dias_foro" class="form-control" runat="server" />
                                        </div>
                                    </div>
                                    <div id="blk_foro" class="col-sm-3 hidden" runat="server">
                                        <label>Foro</label>
                                        <div class="form-group">
                                            <select id="ddlForo" class="selectpicker margin-b-15" data-hide-disabled="true" data-live-search="true" title="Seleccione una opción" runat="server" />
                                        </div>
                                    </div>    
                                </div>                            
                                <div id="blk_cp" class="hidden" runat="server">
                                    <%--<div class="col-sm-4">
                                        <label>Fecha límite</label>	
                                        <div id="date_form" class="input-group date js-datepicker" runat="server">
                                            <label for="date_limit" class="sr-only">Fecha límite</label>
								            <input type="text" class="form-control" id="date_limit" placeholder="Fecha límite" runat="server" />
                                            <span class="input-group-addon glyphicon">
			                                    <span class="icon-calendar xs"></span>
			                                </span>								
							            </div>
                                    </div>--%> 
                                    <div class="col-sm-4">
                                        <label>Nº días desde F. Límite</label>
                                        <div id="Div1" class="form-group" runat="server">  
                                            <label class="sr-only" for="txt_dias_lim_cp">Nº días desde F. Límite</label>                                     
                                            <input type="text" id="txt_dias_lim_cp" class="form-control" runat="server" />
                                        </div>
                                    </div>  
                                    <div class="col-sm-4">
                                        <label>Nº días desde F. Apertura</label>
                                        <div id="tipo_dias_cp_form" class="form-group" runat="server">  
                                            <label class="sr-only" for="txt_dias_cp">Nº días desde F. Apertura</label>                                     
                                            <input type="text" id="txt_dias_cp" class="form-control" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label>Cuerpo del mensaje / Comentario CP</label>
                                    <div id='block_body' class="form-group">
							            <label class="sr-only" for="txt_cuerpo">Cuerpo del mensaje / Comentario CP</label>
                                        <textarea id="txt_cuerpo" runat="server" placeholder="Cuerpo del mensaje / Comentario CP" name="txtCuerpo" cols="80" rows="10" />
									    <script type="text/javascript">
									        CKEDITOR.replace('txt_cuerpo',
                                            {
                                                placeholder: 'Cuerpo del mensaje / Comentario CP'
	                                        });
                                        </script>
						            </div>                 
                                </div>
                                <div id="blk_adjuntos" class="col-sm-12" runat="server">       
                                    <label class="full-width padding-t-20">Adjuntos / Caso Práctico  <a id="lbl_adjuntos" class="fas fa-file-upload fa-1-6x text-blue pull-right" href="javascript:void(0);" data-toggle="modal" data-target="#modal_adjunto" title="Añadir Adjunto / Caso Práctico" runat="server"> Añadir adjunto / Caso Práctico</a></label>
                                    <div class="padding-t-20">
                                        <div id="block_adjuntos_lst" class="col-sm-12" runat="server"></div>
                                        <div class="col-sm-12 padding-t-20">
                                            <table id="tbl_adjuntos" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
                                            <input type="hidden" id="hidAdjuntos" value="" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <a href="#" id="btn_back" class="btn pull-left btn-primary btn-block-xs margin-b-15" runat="server">Volver</a>
                                    <a id="btn_save" href="javascript: void(0);" onclick="validarFormularioMsgForo()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right hidden" runat="server">Guardar</a>
                                    <a id="btn_save_msg" href="javascript: void(0);" onclick="validarFormularioMsg()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right hidden" runat="server">Guardar</a>
                                    <a id="btn_save_cp" href="javascript: void(0);" onclick="validarFormularioCP()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right hidden" runat="server">Guardar</a>
                                    <a id="btn_save_foro" href="javascript: void(0);" onclick="validarFormularioForo()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right hidden" runat="server">Guardar</a>
                                
                                    <asp:Button ID="btnGuardar" CssClass="hidden" runat="server" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnEliminar" CssClass="hidden" runat="server" OnClick="btnEliminar_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>
                    
                    <div class="modal fade" id="modal_adjunto" tabindex="-1" role="dialog" aria-labelledby="modal_adjunto_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_adjunto_title">Adjunto / Caso Práctico</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="row fileupload-buttonbar">
                                        <div class="col-sm-12">
                                            <span id="file_adjunto" class="btn fileinput-button" runat="server"></span>
                                            <div id='progress_foto' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button id="modal-close" type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
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
     <%--<script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>--%>
     <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>
     
     <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.ui.widget.js"></script>
     <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.iframe-transport.js"></script>
     <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.fileupload.js"></script>
     <script type="text/javascript" src="/App_Themes/support/js/internal/automation_functions.js"></script>
</body>
</html>