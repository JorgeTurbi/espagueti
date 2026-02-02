<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contacto-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.contacto_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Contacto mantenimiento</title>

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
        /*.bootstrap-select.btn-group .dropdown-menu {z-index: 2147483647;}       */
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
							    <legend class="text-color-primary"><i class='far fa-address-card'></i> Manteniento de Contacto</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-4">
                                    <label>Nombre *</label>
                                    <div id="nombre_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_cont_nombre">Nombre</label>
									    <input type="text" placeholder="Nombre" id="txt_cont_nombre" class="form-control" runat="server" maxlength="200" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-5">
                                    <label>Apellidos *</label>
                                    <div id="apellidos_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_cont_apellidos">Apellidos</label>
									    <input type="text" placeholder="Apellidos" id="txt_cont_apellidos" class="form-control" runat="server" maxlength="250" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>NIF</label>
                                    <div id="nif_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_cont_nif">NIF</label>
									    <input type="text" placeholder="NIF" id="txt_cont_nif" class="form-control" runat="server" maxlength="20" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Mail</label>
                                    <div id="mail_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_cont_mail">Mail</label>
									    <input type="text" placeholder="Mail" id="txt_cont_mail" class="form-control" runat="server" maxlength="200" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Teléfono</label>
                                    <div id="telefono_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_cont_telefono">Teléfono</label>
									    <input type="text" placeholder="Teléfono" id="txt_cont_telefono" class="form-control" runat="server" maxlength="50" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Teléfono Móvil</label>
                                    <div id="telf_mov_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_cont_telefono_movil">Teléfono Móvil</label>
									    <input type="text" placeholder="Teléfono Móvil" id="txt_cont_telefono_movil" class="form-control" runat="server" maxlength="50" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
								    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Empresa *</label>
                                    <div id="empresa_form" class="form-group" runat="server">                                       
                                        <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Cargo</label>
                                    <div id="cargo_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_cont_cargo">Cargo</label>
									    <input type="text" placeholder="Cargo" id="txt_cont_cargo" class="form-control" runat="server" maxlength="200" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
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
                                <div class="col-sm-12 ">
                                    <label>Comentarios</label>											
								    <div id='comentarios_form' class="form-group">
							            <label class="sr-only" for="txt_comentarios">Comentarios</label>
                                        <textarea id="txt_comentarios" placeholder="Comentarios" class="form-control" cols='2' rows='5' runat="server"></textarea>                            
						            </div>   
                                </div>
                                <div class="col-sm-12">
                                    <a href="contactos.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>	
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
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>
    
    <script type="text/javascript">
        $(function () {
            /// 1.- Cargar el textarea
            autosize($('#txt_comentarios'));
        });


        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function validarFormulario() {
            /// 1.- Sacar los parametros
            var nombre = $('#txt_cont_nombre').val();
            var apellidos = $('#txt_cont_apellidos').val();
            //var nif = $('#txt_cont_nif').val();
            //var mail = $('#txt_cont_mail').val();
            var empresa = $('#ddlEmpresa').val();

            var fecha_alta = $('#txtFechaAlta').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (nombre == "undefined" || nombre == undefined || nombre == "null" || nombre == null || nombre == '') {
                $('#nombre_form').addClass(' has-error');
                $('#txt_error').html('El campo Nombre es obligatorio');
                $('#txt_cont_nombre').attr("placeholder", "El campo Nombre es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (!validarCarateresEspeciales(nombre)) {
                $('#nombre_form').addClass(' has-error');
                $('#txt_error').html('El campo Nombre contiene carácteres no válidos');
                return false;
            }
            else if (apellidos == "undefined" || apellidos == undefined || apellidos == "null" || apellidos == null || apellidos == '') {
                $('#apellidos_form').addClass(' has-error');
                $('#txt_error').html('El campo Apellidos es obligatorio');
                $('#txt_cont_apellidos').attr("placeholder", "El campo Apellidos es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (!validarCarateresEspeciales(apellidos)) {
                $('#apellidos_form').addClass(' has-error');
                $('#txt_error').html('El campo Apellidos contiene carácteres no válidos');
                return false;
            }
            /*else if (nif == "undefined" || nif == undefined || nif == "null" || nif == null || nif == '') {
                $('#nif_form').addClass(' has-error');
                $('#txt_error').html('El campo NIF es obligatorio');
                $('#txt_cont_nif').attr("placeholder", "El campo NIF es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (!validarCarateresEspeciales(nif)) {
                $('#nif_form').addClass(' has-error');
                $('#txt_error').html('El campo NIF contiene carácteres no válidos');
                return false;
            }
            else if (mail == "undefined" || mail == undefined || mail == "null" || mail == null || mail == '') {
                $('#mail_form').addClass(' has-error');
                $('#txt_error').html('El campo NIF es obligatorio');
                $('#txt_cont_mail').attr("placeholder", "El campo NIF es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (!validarMail(mail)) {
                $('#mail_form').addClass(' has-error');
                $('#txt_error').html('El campo NIF contiene carácteres no válidos');
                return false;
            }*/
            else if (empresa == "-1") {
                $('#empresa_form').addClass(' has-error');
                $('#txt_error').html('La empresa es obligatoria');
                return false;
            }
            else if (fecha_alta == "undefined" || fecha_alta == undefined || fecha_alta == "null" || fecha_alta == null || fecha_alta == '') {
                $('#fechaAlta_form').addClass(' has-error');
                $('#txt_error').html('La fecha de alta es obligatoria');
                $('#txtFechaAlta').attr("placeholder", "La fecha de alta es obligatoria");
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }
    </script>  
</body>
</html>
