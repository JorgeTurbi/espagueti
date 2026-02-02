<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="banner-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.banner_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Mantenimiento banner</title>

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
        #fileinput > span {white-space: initial;}
        .fileinput-button span {white-space: initial;}
        #txt_comentarios, #txt_descripcion {max-height: 350px;}
        .btn.fileinput-button {border: 1px solid #ccc; height: 150px; margin-bottom: 5px; overflow: hidden; text-align: left; width: 100%;}
        .fileinput-button input {cursor: pointer; direction: ltr; height: 150px; left: -4px; margin: 0; opacity: 0; position: absolute; right: 0; top: 0; width: 100%;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}   
        
        .modal.in .modal-dialog {transform: translate(0px, 25%);}
        @media (min-width:576px){.modal-dialog{max-width:500px;margin:1.75rem auto}.modal-dialog-large{max-width: calc(100vw - 30%);}.modal-dialog-centered{min-height:calc(100% - 3.5rem)}.modal-dialog-centered::before{height:calc(100vh - 3.5rem)}}    
               
        .form-control-sm {border-radius: 0.2rem; font-size: 0.875rem; height: calc(1.5em + 1rem + 2px); line-height: 1.5; padding: 0.25rem 0.5rem;}
        .d-inline-block {display: inline-block !important;}
        .align-middle {vertical-align: middle !important;}
        .pl-2, .px-2 {padding-left: 0.5rem !important;}
        .pb-5,.py-5{padding-bottom:5rem!important}
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
        <section class="padding-tb-50 vh-100">
		    <div class="row no-margin padding-nav">	
                <div class="col-sm-12 padding-b-50">                         
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div>
                            <fieldset>
							    <legend id="txt_title" class="text-color-primary" runat="server"><i class='fas fa-ad'></i> Mantenimiento banner</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-6">
                                    <label>Nombre *</label>
                                    <div id="nombre_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_nombre">Nombre</label>
									    <input type="text" placeholder="Nombre" id="txt_nombre" class="form-control" runat="server" maxlength="250" />												
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Producto *</label>													
								    <div id="producto_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlProducto">Producto</label>
                                        <select id="ddlProducto" class="selectpicker" data-live-search="true" data-hide-disabled="true" runat="server"></select>									    
								    </div>
                                </div>
                                <div class="col-sm-2">
                                    <label>Orden</label>
                                    <div id="orden_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_orden">Orden</label>
									    <input type="text" placeholder="Orden" id="txt_orden" class="form-control" value="0" runat="server" />														
								    </div>
                                </div>
                                <div id="blk_search" class="col-sm-1 padding-tb-5 text-center" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a onclick='search_order()' class='fas fa-search fa-2x padding-t-10' href='javascript:void(0);' title="Buscar siguiente valor de orden"></a>
                                </div>
                                <div class="col-sm-6">
                                    <label>Link</label>
                                    <div id="link_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_link">Link</label>
									    <input type="text" placeholder="Link" id="txt_link" class="form-control" runat="server" maxlength="250" />												
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha Inicio*</label>
								    <div id="fecha_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFecha">Fecha Inicio</label>
									    <input type="text" id="txtFecha" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha Fin</label>
								    <div id="fecha_fin_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFechaFin">Fecha Fin</label>
									    <input type="text" id="txtFechaFin" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="col-sm-3">
                                    <label>Banner</label>
                                    <div id="banner_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_banner">Banner</label>
									    <input type="text" placeholder="Banner" id="txt_banner" class="form-control" runat="server" readonly="readonly" />												
								    </div>
                                </div>
                                <div id="block_delete_banner" class="col-sm-1 padding-tb-5 text-center hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a id="delete_banner" onclick='delete_banner_img()' class='fas fa-times-circle fa-3x text-color-red' href='javascript:void(0);' title="Eliminar banner"></a>
                                </div>
                                <div id="block_upload_banner" class="col-sm-1 padding-tb-5 text-center" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a class='far fa-image fa-3x' href='javascript:void(0);' data-toggle='modal' data-target='#modal_banner' title="Añadir banner"></a>
                                </div>
                                <div id="block_see" class="col-sm-1 padding-tb-5 hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a id='lnk_banner' class='fas fa-eye fa-3x' href="#" target='_blank' title='Ver banner' runat='server'></a>
                                </div>
                                <div class="col-sm-3">
                                    <label>Banner 2x</label>
                                    <div id="banner2_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_banner2">Banner 2x</label>
									    <input type="text" placeholder="Banner 2x" id="txt_banner2" class="form-control" runat="server" readonly="readonly" />												
								    </div>
                                </div>
                                <div id="block_delete_banner2" class="col-sm-1 padding-tb-5 text-center hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a id="delete_banner2" onclick='delete_banner_img2()' class='fas fa-times-circle fa-3x text-color-red' href='javascript:void(0);' title="Eliminar banner"></a>
                                </div>
                                <div id="block_upload_banner2" class="col-sm-1 padding-tb-5 text-center" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a class='far fa-image fa-3x' href='javascript:void(0);' data-toggle='modal' data-target='#modal_banner2' title="Añadir banner"></a>
                                </div>
                                <div id="block_see_banner" class="col-sm-1 padding-tb-5 hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a id='lnk_banner2' class='fas fa-eye fa-3x' href="#" target='_blank' title='Ver banner' runat='server'></a>
                                </div>
                                <div class="col-sm-12 pb-5">
                                    <a href="lista-banners.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>	

                                    <a id="btn_save" href="javascript: void(0);" onclick="validarFormulario()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right">Guardar</a>
                                    <asp:Button ID="btnGuardar" CssClass="hidden" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>
                    
                    <div class="modal fade" id="modal_banner" tabindex="-1" role="dialog" aria-labelledby="modal_banner_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_banner_title">Banner (1500px × 300px)</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="row fileupload-buttonbar">
                                        <div class="col-sm-12">
                                            <span id="file_banner" class="btn fileinput-button" runat="server"></span>
                                            <div id='progress_banner' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                            <input type="hidden" id="banner_img" value="" runat="server" />
                                        </div>
                                    </div>
                                    <table id="tbl_banner" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
                                </div>
                                <div class="modal-footer">
                                    <button id="modal-close-banner" type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="modal_banner2" tabindex="-1" role="dialog" aria-labelledby="modal_banner2_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_banner2_title">Banner (3000px × 600px)</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="row fileupload-buttonbar">
                                        <div class="col-sm-12">
                                            <span id="file_banner2" class="btn fileinput-button" runat="server"></span>
                                            <div id='progress_banner2' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                            <input type="hidden" id="banner2_img" value="" runat="server" />
                                        </div>
                                    </div>
                                    <table id="tbl_banner2" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
                                </div>
                                <div class="modal-footer">
                                    <button id="modal-close-banner2" type="button" class="btn btn-primary" data-dismiss="modal">Cerrar</button>
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
    <script type="text/javascript" src="/App_Themes/support/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>    
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.dateFormat.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/dataTables.cleanString.js"></script>
    
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.iframe-transport.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.fileupload.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/banner-functions.js"></script>
</body>
</html>