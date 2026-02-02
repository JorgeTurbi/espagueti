<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="solicitud-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.solicitud_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Solicitud Práctica mantenimiento</title>

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
							    <legend class="text-color-primary"><i class='fas fa-file-signature'></i> Manteniento de Solicitud Práctica</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha *</label>
								    <div id="fechaAlta_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFechaAlta">Fecha</label>
									    <input type="text" id="txtFechaAlta" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>
                                <%--<div class="col-sm-3">
                                    <label>Fecha Cierre</label>
								    <div id="fechaBaja_form" class="input-group date js-datepicker" runat="server">
								        <label class="sr-only" for="txtFechaBaja">Fecha Cierre</label>
									    <input type="text" id="txtFechaBaja" class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
			                                <span class="icon-calendar xs"></span>
                                        </span>
								    </div>
                                </div>--%>
                                <div class="col-sm-2">
                                    <label>Nº Horas *</label>
                                    <div id="horas_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_horas">Nº Horas</label>
									    <input type="text" placeholder="Nº Horas" id="txt_pra_horas" class="form-control" runat="server" maxlength="10" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>                                
                                <div class="col-sm-2">
                                    <label>Ayuda *</label>
                                    <div id="cantidad_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_pra_cantidad">Ayuda</label>
									    <input type="text" placeholder="Ayuda" id="txt_pra_cantidad" class="form-control" runat="server" maxlength="10" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-5">
                                    <label>Contacto Empresa</label>													
								    <div id="tutor_empresa_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlTutorEmpresa">Contacto Empresa</label>
                                        <select  id="ddlTutorEmpresa" class="selectpicker" data-live-search="true" data-hide-disabled="true" runat="server"></select>
								    </div>
                                </div>
                                <div class="col-sm-12 ">
                                    <label>Descripción puesto</label>											
								    <div id='descripcion_form' class="form-group">
							            <label class="sr-only" for="txt_descripcion">Descripción puesto</label>
                                        <textarea id="txt_descripcion" placeholder="Descripción puesto" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div class="col-sm-12 ">
                                    <label>Comentarios</label>											
								    <div id='comentarios_form' class="form-group">
							            <label class="sr-only" for="txt_comentarios">Comentarios</label>
                                        <textarea id="txt_comentarios" placeholder="Comentarios" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>                                
                                <div class="col-sm-12">
                                    <a id="btn_back" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                                </div>
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
    <script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>

    <script type="text/javascript">
        $(function () {
            /// 1.- Cargar los textareas
            autosize($('#txt_descripcion'));
            autosize($('#txt_comentarios'));
        });

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function validarFormulario() {
            /// 1.- Sacar los parametros
            var fecha_alta = $('#txtFechaAlta').val();
            var horas = $('#txt_pra_horas').val();
            var cantidad = $('#txt_pra_cantidad').val();
            var descripcion = $('#txt_descripcion').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (fecha_alta == "undefined" || fecha_alta == undefined || fecha_alta == "null" || fecha_alta == null || fecha_alta == '') {
                $('#fechaAlta_form').addClass(' has-error');
                $('#txt_error').html('La fecha de alta es obligatoria');
                $('#txtFechaAlta').attr("placeholder", "La fecha de alta es obligatoria");
                subirArribaPagina();
                return false;
            }
            else if (horas == "undefined" || horas == undefined || horas == "null" || horas == null || horas == '' || !validarTelefono(horas)) {
                $('#horas_form').addClass(' has-error');
                $('#txt_error').html('El campo Nº Horas es un campo numérico');
                $('#txt_pra_horas').attr("placeholder", "El campo Nº Horas es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (cantidad == "undefined" || cantidad == undefined || cantidad == "null" || cantidad == null || cantidad == '' || !validarTelefono(cantidad)) {
                $('#cantidad_form').addClass(' has-error');
                $('#txt_error').html('El campo Cantidad es obligatorio');
                $('#txt_pra_cantidad').attr("placeholder", "El campo Cantidad es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (descripcion == "undefined" || descripcion == undefined || descripcion == "null" || descripcion == null || descripcion == '') {
                $('#descripcion_form').addClass(' has-error');
                $('#txt_error').html('El campo Descripción puesto es obligatorio');
                $('#txt_descripcion').attr("placeholder", "El campo Descripción puesto es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (!validarCarateresEspeciales(descripcion)) {
                $('#descripcion_form').addClass(' has-error');
                $('#txt_descripcion').html('El campo Descripción puesto contiene carácteres no válidos');
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }
    </script> 
</body>
</html>
