<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="empresa-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.empresa_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Empresa mantenimiento</title>

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
        #txt_comentarios {max-height: 350px;}
        .btn.fileinput-button {border: 1px solid #ccc; height: 150px; margin-bottom: 5px; overflow: hidden; text-align: left; width: 100%;}
        .fileinput-button input {cursor: pointer; direction: ltr; height: 150px; left: -4px; margin: 0; opacity: 0; position: absolute; right: 0; top: 0; width: 100%;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}       
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
							    <legend class="text-color-primary"><i class='fas fa-building'></i> Manteniento de Empresa</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-8">
                                    <label>Razón Social *</label>
                                    <div id="razon_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_emp_razon">Razón Social</label>
									    <input type="text" placeholder="Razón Social" id="txt_emp_razon" class="form-control" runat="server" maxlength="255" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>CIF *</label>
                                    <div id="cif_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_emp_cif">CIF</label>
									    <input type="text" placeholder="CIF" id="txt_emp_cif" class="form-control" runat="server" maxlength="50" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-5">
                                    <label>Dirección</label>
                                    <div id="direccion_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_emp_direccion">Dirección</label>
									    <input type="text" placeholder="Dirección" id="txt_emp_direccion" class="form-control" runat="server" maxlength="499" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Mail</label>
                                    <div id="mail_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_emp_mail">Mail</label>
									    <input type="text" placeholder="Mail" id="txt_emp_mail" class="form-control" runat="server" maxlength="200" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Teléfono</label>
                                    <div id="telefono_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_emp_telefono">Teléfono</label>
									    <input type="text" placeholder="Teléfono" id="txt_emp_telefono" class="form-control" runat="server" maxlength="50" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Fecha Alta *</label>
								    <div id="fechaAlta_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFechaAlta">Fecha Alta</label>
									    <input type="text" id="txtFechaAlta" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Fecha Baja</label>
								    <div id="fechaBaja_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFechaBaja">Fecha Baja</label>
									    <input type="text" id="txtFechaBaja" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Fecha Convenio</label>
								    <div id="fechaConvenio_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFechaConvenio">Fecha Convenio</label>
									    <input type="text" id="txtFechaConvenio" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Fichero convenio</label>
                                    <div id="fichero_conv_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtFicheroConvenio">Fichero convenio</label>
                                        <input type="text" placeholder="Fichero convenio" id="txtFicheroConvenio" class="form-control" runat="server" readonly="readonly" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Logo Empresa</label>
                                    <div id="logo_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtLogo">Logo Empresa</label>
                                        <input type="text" placeholder="Logo Empresa" id="txtLogo" class="form-control" runat="server" readonly="readonly" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-12 "><p>Para modificar el Fichero de convenio o el logo Empresa, subir uno nuevo.</p></div>
                                <div class="col-sm-12 ">
                                    <label>Comentarios</label>											
								    <div id='comentarios_form' class="form-group">
							            <label class="sr-only" for="txt_comentarios">Comentarios</label>
                                        <textarea id="txt_comentarios" placeholder="Comentarios" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div class="col-sm-12">
                                    <a href="empresas.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>
                    
                    <!-- IMPORTANTE TIENE QUE ESTAR FUERA DE UN FORMULARIO -->
                    <div class="col-sm-12" runat="server">
                        <label>Fichero convenio</label>
                        <div class="row fileupload-buttonbar">
                            <div class="col-sm-12">
                                <span id="file_convenio" class="btn fileinput-button" runat="server"></span>
                                <div id='progress' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                <input type="hidden" id="fichero_convenio" value="" runat="server" />
                            </div>
                        </div>
                        <!-- Tabla donde va pintando los archivos que sube -->
                        <table id="tbl_convenio" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
                    </div>
                    <div class="col-sm-12" runat="server">
                        <label>Logo empresa (400px × 400px)</label>
                        <div class="row fileupload-buttonbar">
                            <div class="col-sm-12">
                                <span id="file_logo" class="btn fileinput-button" runat="server"></span>
                                <div id='progress_logo' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                <input type="hidden" id="logo_empresa" value="" runat="server" />
                            </div>
                        </div>
                        <table id="tbl_logo" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
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
    <script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>
    
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.iframe-transport.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.fileupload.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/company_functions.js"></script>
</body>
</html>
