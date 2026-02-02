<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="empleo-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.empleo_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Empleo mantenimiento</title>

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
							    <legend class="text-color-primary"><i class='fas fa-business-time'></i> Manteniento de Empleo</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-6">
                                    <label>Empresa *</label>													
								    <div id="empresa_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddlEmpresa">Empresa</label>
									    <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
								    </div>
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
                                <div class="col-sm-8">
                                    <label>Contrato</label>
                                    <div id="contrato_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_emp_contrato">Contrato</label>
									    <input type="text" placeholder="Contrato" id="txt_emp_contrato" class="form-control" runat="server" maxlength="250" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>																		
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
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap-select-es.min.js"></script>
    
    <script type="text/javascript">
        $(function () {
            /// 1.- Cargar el textarea
            autosize($('#txt_comentarios'));

            /// 2.- Recuperar la tecla pulsada
            if ($("#txt_prac_alumno").val() == "") {
                $("#txt_prac_alumno").focus();
            }
            $("#txt_prac_alumno").on('keypress', $(this), function (e) {
                if (e.which == 13 && $(this).val() == "") {
                    return false;
                }
            });

            /// 3.- Autocompletar
            $("#txt_prac_alumno").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'empleo-mantenimiento.aspx/search_student',
                        data: "{ 'name': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.nombre_completo,
                                    val: item.id_usuario
                                }
                            }))
                        },
                        error: function (response) {
                            alert("La consulta es poco restrictiva, escriba más caracteres o restringe más su búsqueda");
                        },
                        failure: function (response) {
                            alert("La consulta es poco restrictiva, escriba más caracteres o restringe más su búsqueda");
                        }
                    });
                },
                select: function (e, ui) {
                    $('#idAlumno').val(ui.item.val);
                },
                minLength: 3
            });
        });
        
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function validarFormulario() {
            /// 1.- Sacar los parametros
            var idAlumno = $('#idAlumno').val();
            var idEmpresa = $('#ddlEmpresa').val();
            var fecha_alta = $('#txtFechaAlta').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (idAlumno == "undefined" || idAlumno == undefined || idAlumno == "null" || idAlumno == null || idAlumno == '') {
                $('#alumno_form').addClass(' has-error');
                $('#txt_prac_alumno').html('El alumno es obligatorio');
                $('#txt_error').html('El alumno es obligatorio');
                subirArribaPagina();
                return false;
            }
            else if (idEmpresa == "-1") {
                $('#empresa_form').addClass(' has-error');
                $('#txt_error').html('La empresa es obligatoria');
                subirArribaPagina();
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
