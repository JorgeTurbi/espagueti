<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recurso-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.recurso_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Mantenimiento recurso directo</title>

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
        
        .modal.in .modal-dialog {transform: translate(0px, 25%);}
        @media (min-width:576px){.modal-dialog{max-width:500px;margin:1.75rem auto}.modal-dialog-large{max-width: calc(100vw - 30%);}.modal-dialog-centered{min-height:calc(100% - 3.5rem)}.modal-dialog-centered::before{height:calc(100vh - 3.5rem)}}    
     </style>
	    
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
							    <legend class="text-color-primary"><i class='fas fa-file'></i> Mantenimiento recurso</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-9">
                                    <label>Título *</label>
                                    <div id="titulo_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_titulo">Título</label>
									    <input type="text" placeholder="Título" id="txt_titulo" class="form-control" runat="server" maxlength="250" />												
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Tipo *</label>													
								    <div id="tipo_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlTipo">Tipo</label>
									    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="selectpicker">
                                            <asp:ListItem Text="NOTA TECNICA" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="MULTIMEDIA" Value="3"></asp:ListItem>
									    </asp:DropDownList>
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Área *</label>
                                    <div id="area_form" class="form-group" runat="server">                                       
                                        <select id="ddlArea" class="selectpicker" data-live-search="true" data-hide-disabled="true" runat="server"></select>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Activo</label>
                                    <div id="activo_form" class="form-group padding-t-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Versión *</label>
                                    <div id="resource_version_form" class="form-group">
							            <label class="sr-only" for="resource_version">Versión</label>
                                        <input type="text" placeholder="Versión (Nº)" id="resource_version" class="form-control" runat="server" />
							        </div>                            
                                </div>
                                <div class="col-sm-3">
                                    <label>Derechos *</label>
                                    <div id="resource_derechos_form" class="form-group">
							            <label class="sr-only" for="resource_derechos">Derechos</label>
                                        <input type="text" placeholder="Derechos" id="resource_derechos" class="form-control" runat="server" />
							        </div>                            
                                </div>
                                <div class="col-sm-3">
                                    <label>Recurso interno</label>
                                    <div id="rec_int_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_rec_int">Recurso interno</label>
									    <input type="text" placeholder="Recurso interno" id="txt_rec_int" class="form-control" runat="server" readonly="readonly" />												
								    </div>
                                </div>
                                <div id="block_delete_rec_int" class="col-sm-1 padding-tb-5 text-center hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a onclick='delete_rec_int()' class='fas fa-times-circle fa-3x text-red' href='javascript:void(0);' title="Eliminar Recurso interno"></a>
                                </div>
                                <div id="block_upload_rec_int" class="col-sm-1 padding-tb-5 text-center" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a class='fas fa-file-upload fa-3x' href='javascript:void(0);' data-toggle='modal' data-target='#modal_rec_int' title="Añadir Recurso interno"></a>
                                </div>
                                <div id="block_see" class="col-sm-1 padding-tb-5 hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a id='lnk_rec_int' class='fas fa-eye fa-3x' href="#" target='_blank' title='Ver Recurso interno' runat='server'></a>
                                </div>
                                <div class="col-sm-6">
                                    <label>Recurso externo</label>
                                    <div id="rec_ext_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_rec_ext">Recurso externo</label>
									    <input type="text" placeholder="Recurso externo" id="txt_rec_ext" class="form-control" runat="server" />														
								    </div>
                                </div>                                
                                <div class="col-sm-12">
                                    <a id="btn_back" href="#" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>

                    <div class="modal fade" id="modal_rec_int" tabindex="-1" role="dialog" aria-labelledby="modal_rec_int_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_rec_int_title">Recurso interno</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="row fileupload-buttonbar">
                                        <div class="col-sm-12">
                                            <span id="file_rec_int" class="btn fileinput-button" runat="server"></span>
                                            <div id='progress_rec_int' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                            <input type="hidden" id="recurso_interno" value="" runat="server" />
                                        </div>
                                    </div>
                                    <table id="tbl_rec_int" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
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
    <script type="text/javascript" src="/App_Themes/support/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>    
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>
    
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.iframe-transport.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.fileupload.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/recursos_functions.js"></script>
</body>
</html>