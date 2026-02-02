<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="practica-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.practica_mantenimiento" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Práctica mantenimiento</title>

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
        #txt_comentarios, #txt_pdp_comentarios {max-height: 350px;}
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
							    <legend class="text-color-primary"><i class='fas fa-toolbox'></i> Manteniento de Práctica</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-6">
                                    <label>Alumno *</label>
								    <div id="alumno_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_prac_alumno">Alumno</label>
									    <input type="text" placeholder="Alumno" id="txt_prac_alumno" autocomplete="off" class="form-control" runat="server" />
                                        <input id="idAlumno" type="hidden" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Curso *</label>
                                    <div id="curso_form" class="form-group" runat="server">                                       
                                        <asp:DropDownList ID="ddlCurso" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Empresa *</label>													
								    <div id="empresa_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlEmpresa">Empresa</label>
									    <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true" onchange="cargarTutoresEmpresa()"></asp:DropDownList>
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Tutor Empresa *</label>													
								    <div id="tutor_empresa_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlTutorEmpresa">Tutor Empresa</label>
                                        <select id="ddlTutorEmpresa" class="selectpicker" data-live-search="true" data-hide-disabled="true" runat="server"></select>
                                        <input type="hidden" id="id_Tutor_Empresa" runat="server" />
                                        <%--<select id="ddlTutorEmpresa" class="selectpicker" disabled="disabled" data-live-search="true" data-hide-disabled="true" runat="server"></select>--%>
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Tutor Escuela *</label>													
								    <div id="tutor_escuela_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlTutorEscuela">Tutor Escuela</label>
									    <asp:DropDownList ID="ddlTutorEscuela" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha Alta *</label>
								    <div id="fechaAlta_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFechaAlta">Fecha Alta</label>
									    <input type="text" id="txtFechaAlta" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <div class="col-sm-3">
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
                                    <label>Duración *</label>
                                    <div id="duracion_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_duracion">Duración</label>
									    <input type="text" placeholder="Duración" id="txt_pra_duracion" class="form-control" runat="server" maxlength="10" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Ayuda Estudio Mes *</label>
                                    <div id="ayuda_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_ayuda">Ayuda Estudio Mes</label>
									    <input type="text" placeholder="Ayuda Estudio Mes" id="txt_pra_ayuda" class="form-control" runat="server" maxlength="10" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Horas Semana *</label>
                                    <div id="horas_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_horas">Horas Semana</label>
									    <input type="text" placeholder="Horas Semana" id="txt_pra_horas" class="form-control" runat="server" maxlength="10" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-4">
                                    <label>Tipo</label>													
								    <div id="tipo_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlTipo">Tipo</label>
									    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="selectpicker">
                                            <asp:ListItem Text="PDP" Value="PDP"></asp:ListItem>
                                            <asp:ListItem Text="PDP3" Value="PDP3"></asp:ListItem>
                                            <asp:ListItem Text="PDP6" Value="PDP6"></asp:ListItem>
                                            <asp:ListItem Text="POSTGRADO" Value="POSTGRADO"></asp:ListItem>
									    </asp:DropDownList>
								    </div>
                                </div>
                                <div class="col-sm-2">
                                    <label>Curriculares</label>
                                    <div id="curriculares_for" class="form-group padding-t-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkCurriculares" runat="server" Text="Curriculares" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-5">
                                    <label>Fichero anexo</label>
                                    <div id="fichero_pra_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtFicheroAnexo">Fichero anexo</label>
                                        <input type="text" placeholder="Fichero anexo" id="txtFicheroAnexo" class="form-control" runat="server" readonly="readonly" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-1">
                                    <label>&nbsp;</label>
                                    <div class="form-group margin-tb-5 text-center">
                                        <a id="txt_file_anexo" href="#" target="_blank" runat="server"><i class='far fa-file-pdf fa-2x text-color-red'></i></a>
                                    </div>
                                </div>
                                <div class="col-sm-12 ">
                                    <label>Comentarios</label>											
								    <div id='comentarios_form' class="form-group">
							            <label class="sr-only" for="txt_comentarios">Comentarios</label>
                                        <textarea id="txt_comentarios" placeholder="Comentarios" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div class="col-sm-6">
                                    <label>PDP Nº Factura</label>
                                    <div id="factura_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_factura">PDP Nº Factura</label>
									    <input type="text" placeholder="PDP Nº Factura" id="txt_pra_factura" class="form-control" runat="server" maxlength="50" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>PDP Nº Pedido</label>
                                    <div id="pedido_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_pedido">PDP Nº Pedido</label>
									    <input type="text" placeholder="PDP Nº Pedido" id="txt_pra_pedido" class="form-control" runat="server" maxlength="50" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>PDP Precio</label>
                                    <div id="precio_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_precio">PDP Precio</label>
									    <input type="text" placeholder="PDP Precio" id="txt_pra_precio" class="form-control" runat="server" maxlength="21" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>PDP Matriculado</label>
                                    <div id="matriculado_form" class="form-group padding-t-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkMatriculado" runat="server" Text="PDP Matriculado" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>PDP Activado</label>
                                    <div id="activado_form" class="form-group padding-t-10" runat="server">													
							            <div class="checkbox">
                                            <asp:CheckBox ID="chkActivado" runat="server" Text="PDP Activado" /> 
							            </div>
						            </div>
                                </div>
                                <div class="col-sm-12 ">
                                    <label>PDP Comentarios</label>											
								    <div id='pdp_comentarios_form' class="form-group">
							            <label class="sr-only" for="txt_pdp_comentarios">PDP Comentarios</label>
                                        <textarea id="txt_pdp_comentarios" placeholder="PDP Comentarios" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div class="col-sm-12">
                                    <a href="practicas.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                                </div>
                            </fieldset>
                        </div>
                    </form>
                    
                    <!-- IMPORTANTE TIENE QUE ESTAR FUERA DE UN FORMULARIO -->
                    <div class="col-sm-12" runat="server">
                        <label>Fichero anexo</label>
                        <div class="row fileupload-buttonbar">
                            <div class="col-sm-12">
                                <span id="file_anexo" class="btn fileinput-button" runat="server"></span>
                                <div id='progress' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                <input type="hidden" id="fichero_anexo" value="" runat="server" />
                            </div>
                        </div>
                        <!-- Tabla donde va pintando los archivos que sube -->
                        <table id="tbl_anexo" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
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
    <script type="text/javascript" src="/App_Themes/support/js/internal/practice_functions.js"></script>
</body>
</html>
