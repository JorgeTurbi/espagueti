<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recurso-directo.aspx.cs" Inherits="campus_sbs_admin.recurso_directo" ValidateRequest="false" %>
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
               
        .form-control-sm {border-radius: 0.2rem; font-size: 0.875rem; height: calc(1.5em + 1rem + 2px); line-height: 1.5; padding: 0.25rem 0.5rem;}
        .d-inline-block {display: inline-block !important;}
        .align-middle {vertical-align: middle !important;}
        .pl-2, .px-2 {padding-left: 0.5rem !important;}

        .card {background-clip: border-box; background-color: #fff; border-radius: 0.25rem; display: flex; flex-direction: column; min-width: 0; overflow-wrap: break-word; position: relative;}
        .card h4 {cursor: pointer;}
        .card-header {border-bottom: 1px solid #223266; margin-bottom: 0; padding: 1.5rem 1.25rem;}
        .card-body {flex: 1 1 auto; padding: 1.25rem;}        
        .card .card-header .btn-link[aria-expanded="false"]::before {border-color: #edab3a transparent transparent; border-style: solid; border-width: 7px 5px 0; content: ""; display: block; height: 0; position: absolute; right: 5px; top: 15px; width: 0;}
        .card .card-header .btn-link[aria-expanded="true"]::before {border-color: transparent transparent #edab3a; border-style: solid; border-width: 0 5px 7px; content: ""; display: block; height: 0; position: absolute; right: 5px; top: 15px; width: 0;}
        input[type=checkbox] {-webkit-appearance: checkbox}
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
							    <legend id="txt_title" class="text-color-primary" runat="server"><i class='fas fa-photo-video'></i> Mantenimiento recurso directo</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-6">
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
                                            <asp:ListItem Text="Seleccione" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Bootcamp" Value="BO"></asp:ListItem>
                                            <asp:ListItem Text="Clase online" Value="OC"></asp:ListItem>
                                            <asp:ListItem Text="Clase presencial" Value="MC"></asp:ListItem>
                                            <asp:ListItem Text="Webinar" Value="WB"></asp:ListItem>
									    </asp:DropDownList>
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha *</label>
								    <div id="fecha_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFecha">Fecha</label>
									    <input type="text" id="txtFecha" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label>Descripción</label>											
								    <div id='descripcion_form' class="form-group">
							            <label class="sr-only" for="txt_descripcion">Descripción</label>
                                        <textarea id="txt_descripcion" placeholder="Descripción" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div class="col-sm-6">
                                    <label>Profesor *</label>
								    <div id="profesor_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_profesor">Profesor</label>
									    <input type="text" placeholder="Profesor" id="txt_profesor" autocomplete="off" class="form-control" runat="server" />
                                        <input id="idProfesor" type="hidden" runat="server" />
								    </div>
                                </div>                                
                                <div class="col-sm-3">
                                    <label>Área *</label>
                                    <div id="area_form" class="form-group" runat="server">                                       
                                        <select id="ddlArea" class="selectpicker" data-live-search="true" data-hide-disabled="true" runat="server"></select>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Temática *</label>													
								    <div id="tematica_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlTematica">Temática</label>
                                        <select id="ddlTematica" class="selectpicker" data-live-search="true" data-hide-disabled="true" runat="server"></select>
								    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="col-sm-3">
                                    <label>Foto</label>
                                    <div id="foto_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_foto">Foto</label>
									    <input type="text" placeholder="Foto" id="txt_foto" class="form-control" runat="server" readonly="readonly" />												
								    </div>
                                </div>
                                <div id="block_delete_photo" class="col-sm-1 padding-tb-5 text-center hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a onclick='delete_foto()' class='fas fa-times-circle fa-3x text-red' href='javascript:void(0);' title="Eliminar foto"></a>
                                </div>
                                <div id="block_upload_photo" class="col-sm-1 padding-tb-5 text-center" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a class='far fa-image fa-3x' href='javascript:void(0);' data-toggle='modal' data-target='#modal_photo' title="Añadir foto"></a>
                                </div>
                                <div id="block_see" class="col-sm-1 padding-tb-5 hidden" runat="server">
                                    <label class="full-width">&nbsp;</label>
                                    <a id='lnk_photo' class='fas fa-eye fa-3x' href="#" target='_blank' title='Ver foto' runat='server'></a>
                                </div>
                                <div class="col-sm-2">
                                    <label>Valoración clase</label>
                                    <div id="clase_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_clase">Valoración clase</label>
									    <input type="text" placeholder="Valoración clase" id="txt_clase" class="form-control" value="0" runat="server" />														
								    </div>
                                </div>
                                <div class="col-sm-2">
                                    <label>Valoración profesor</label>
                                    <div id="val_profesor_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_val_profesor">Valoración profesor</label>
									    <input type="text" placeholder="Valoración profesor" id="txt_val_profesor" class="form-control" value="0" runat="server" />														
								    </div>
                                </div>
                                <div class="col-sm-2">
                                    <label>Interno</label>
                                    <div id="interno_form" class="form-group padding-t-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkInterno" runat="server" Text="Interno" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-2 hidden">
                                    <label>Visible</label>
                                    <div id="visible_form" class="form-group padding-t-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkVisible" runat="server" Text="Visible" /> 
							            </div>
						            </div>
                                </div>
                                <div id="blk_tags" class="col-sm-12 margin-b-10" runat="server"></div>
                                <div class="col-sm-12">
                                    <label>Comentarios Internos</label>											
								    <div id='comentarios_form' class="form-group">
							            <label class="sr-only" for="txt_comentarios">Comentarios Internos</label>
                                        <textarea id="txt_comentarios" placeholder="Comentarios Internos" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div id="block_Docencias" class="col-sm-12 hidden" runat="server">
                                    <div id="accordion-bbdd">
                                        <div class="card">
                                            <div class="card-header" id="heading-bbdd-1">                                
                                                <a id="card_estado" class="btn-link" data-toggle="collapse" href="#collapse_bbdd_1" aria-expanded="true" aria-controls="collapse_bbdd_1" runat="server"><h4 class="text-color-primary">Docencias</h4></a>
                                            </div>
                                            <div id="collapse_bbdd_1" class="border-blue collapse in" aria-labelledby="heading-bbdd-1" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_all_doc" runat="server"></div>
                                                    </div>
                                                    <div class="col-sm-2 padding-tb-70">
                                                        <asp:Button ID="btn_sel_doc_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">>" OnClick="btn_sel_doc_all_Click" ToolTip="Añadir todas las docencias" />
                                                        <asp:Button ID="btn_sel_doc" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">" OnClientClick="return validarChkDocAll();" OnClick="btn_sel_doc_Click"  ToolTip="Añadir las docencias seleccionadas" />
                                                        <asp:Button ID="btn_desel_doc" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<" OnClientClick="return validarChkDocSel();" OnClick="btn_desel_doc_Click" ToolTip="Quitar las docencias seleccionadas" />
                                                        <asp:Button ID="btn_desel_status_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<<" OnClick="btn_desel_doc_all_Click"  ToolTip="Quitar todas las docencias" />
                                                        <asp:HiddenField runat="server" ID="hidDocs" />
                                                        <asp:HiddenField runat="server" ID="hidDocsSel" />
                                                    </div>
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_sel_doc" runat="server"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label></label>											
								    <div id="copiar_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetadatosAuotomaticos">Metadatos Automáticos</label>
                                        <a href="javascript:void(0)" class="btn btn-primary btn-block-xs margin-xs-b-15" onclick="return copiarMetadatos()">Generar Metadatos</a>	
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Title</label>													
								    <div id="metaTitle_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaTitle">Meta Title</label>
                                        <input type="text" placeholder="Meta Title" id="txtMetaTitle" class="form-control"  runat="server" maxlength="600" />
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Keywords</label>													
								    <div id="metaKey_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaKeywords">Meta Keywords</label>
									    <input type="text" placeholder="Meta Keywords" id="txtMetaKeywords" class="form-control"  runat="server" />
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Url (Sólo caracteres alfanuméricos, unidos por guiones medios y sin acentos)</label>													
								    <div id="metaUrl_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaUrl">Meta Url</label>
									    <input type="text" placeholder="Meta Url" id="txtMetaUrl" class="form-control"  runat="server" maxlength="500" />
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Author</label>													
								    <div id="metaAuthor_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaAuthor">Meta Author</label>
									    <input type="text" placeholder="Meta Author" id="txtMetaAuthor" class="form-control"  runat="server" maxlength="100" />
								    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label>Meta Descripción</label>													
								    <div id="metaDesc_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaDescripcion">Meta Descripción</label>
                                        <textarea rows="4" cols="50" class="form-control" id="txtMetaDescripcion" placeholder="Meta Descripción" runat="server" ></textarea>
								    </div>
                                </div>   
                                <div class="col-sm-12">
                                    <label>Invitación a directo</label>											
								    <div class="form-group">
							            <label class="sr-only" for="txt_invitacion">Invitación a directo</label>
                                        <textarea id="txt_invitacion" placeholder="Invitación a directo" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div class="col-sm-12">
                                    <a id="btn_back" href="lista-recursos-directo.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>	

                                    <a id="btn_save" href="javascript: void(0);" onclick="validarFormulario()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right">Guardar</a>
                                    <asp:Button ID="btnGuardar" CssClass="hidden" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnGuardarAll" CssClass="hidden" runat="server" Text="Guardar" OnClick="btnGuardarAll_Click" />

                                    <input type="hidden" id="hid_tag" value="" runat="server" />
                                    <asp:Button ID="btn_delete_tag" CssClass="hidden" runat="server" Text="Eliminar tag" OnClick="btn_delete_tag_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>

                    <div>
                        <fieldset>
                            <legend class="text-color-primary"><i class="fas fa-sliders-h"></i> Valoraciones de los alumnos</legend>
                        </fieldset>
                    </div>
                    <div id="table_listado_valoraciones" class="col-sm-12" runat="server"></div>

                    <div class="modal fade" id="modal_photo" tabindex="-1" role="dialog" aria-labelledby="modal_photo_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_photo_title">Foto (400px × 400px)</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="row fileupload-buttonbar">
                                        <div class="col-sm-12">
                                            <span id="file_foto" class="btn fileinput-button" runat="server"></span>
                                            <div id='progress_foto' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                            <input type="hidden" id="foto_usuario" value="" runat="server" />
                                        </div>
                                    </div>
                                    <table id="tbl_foto" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
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

        <!-- Modal -->
        <div class="modal fade" id="wait_modal" tabindex="-1" role="dialog" aria-labelledby="wait_modal" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body text-center">
                        <i class="fas fa-spinner fa-pulse fa-5x"></i>
                    </div>
                </div>
            </div>
        </div>
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
    <script type="text/javascript" src="/App_Themes/support/js/internal/recursos_directo_functions.js"></script>

    <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        CKEDITOR.replace('txt_invitacion',
        {
            placeholder: 'Invitación a directo'
	    });
    </script>
</body>
</html>