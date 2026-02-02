<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="regla-accion.aspx.cs" Inherits="campus_sbs_admin.regla_accion" ValidateRequest="false" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Acciones reglas autamización</title>

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

    <!-- CKeditor -->
     <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>

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
							    <legend class="text-color-primary"><i class='fas fa-cogs'></i> Manteniento de acciones de una regla</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div id="block_comun" class="hidden" runat="server">
                                    <div class="col-sm-10">
                                        <label>Tags</label>
                                        <div id="tags_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_tags">Tags</label>
									        <input type="text" placeholder="Tags" id="txt_tags" class="form-control" runat="server" maxlength="500" />										
								        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <label>Score</label>
                                        <div id="score_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_score">Score</label>
									        <input type="text" placeholder="Score" id="txt_score" class="form-control" runat="server" maxlength="5" />										
								        </div>
                                    </div>                                    
                                    <div class="col-sm-12">
                                        <a id="btn_back_comun" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>                                        
                                        <a href="javascript:void(0);" onclick="save_data_comun()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right">Guardar</a>
                                        	
                                        <asp:Button ID="btnComun" CssClass="hidden" runat="server" Enabled="false" 
                                            Text="Características común" OnClick="btnComun_Click"  />
                                    </div>
                                </div>
                                <div id="block_seguimiento" class="hidden" runat="server">
                                    <div class="col-sm-6">
                                        <label>Canal</label>													
								        <div id="canal_form" class="form-group" runat="server">
								            <label class="sr-only" for="ddlCanal">Canal</label>
									        <select id="ddlCanal" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                <option value="">Selecciona un canal</option>
                                                <option value="1">Mail</option>
                                                <option value="2">Teléfono</option>
                                                <option value="3">Whatsapp</option>
                                                <option value="4">Aviso</option>
									        </select>
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Estado</label>													
								        <div id="estado_form" class="form-group" runat="server">
								            <label class="sr-only" for="ddlEstado">Estado</label>
									        <select id="ddlEstado" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                <option value="">Selecciona un estado</option>
                                                <option value="1">Rechazado</option>
                                                <option value="2">Duplicado</option>
                                                <option value="3">Futuro</option>
                                                <option value="4">Cerrar</option>
                                                <option value="5">Sin Contactar</option>
                                                <option value="6">Indeciso</option>
                                                <option value="7">Interesado</option>
                                                <option value="8">Enviar Contrato</option>
                                                <option value="9">Recibir Contrato</option>
                                                <option value="10">Pago</option>
                                                <option value="11">Matriculado</option>
									        </select>
								        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Comentario</label>											
								        <div id="comentario_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_comentario">Comentario</label>
									        <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                            <textarea id="txt_comentario" runat="server" placeholder="Comentario" name="txt_comentario" cols="80" rows="10" />
									        <script type="text/javascript">
									            CKEDITOR.replace('txt_comentario',
                                                {
                                                    placeholder: 'Comentario'
	                                            });
                                            </script>
								        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <a id="btn_back_seguimiento" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>                                        
                                        <a href="javascript:void(0);" onclick="save_data_seguimiento()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right">Guardar</a>
                                        	
                                        <asp:Button ID="btn_seguimiento" CssClass="hidden" runat="server" Enabled="false" 
                                            Text="Características común" OnClick="btn_seguimiento_Click"  />
                                    </div>
                                </div>
                                <div id="block_reasignar" class="hidden" runat="server">
                                    <div id="div_comercial" class="col-sm-6" runat="server">
                                        <label>Reasignar Comercial</label>													
								        <div id="comercial_form" class="form-group" runat="server">
								            <label class="sr-only" for="ddlComercial">Reasignar Comercial</label>
									        <select id="ddlComercial" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true"></select>
								        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-sm-6">
                                        <label>Nombre From</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_nombre_from">Nombre From</label>
									        <input type="text" placeholder="Nombre From" id="txt_nombre_from" class="form-control" runat="server" maxlength="250" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Mail From</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_mail_from">Mail From</label>
									        <input type="text" placeholder="Mail From" id="txt_mail_from" class="form-control" runat="server" maxlength="250" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Reply To</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_reply_to">Reply To</label>
									        <input type="text" placeholder="Reply To" id="txt_reply_to" class="form-control" runat="server" maxlength="250" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>CCO</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_cco">CCO</label>
									        <input type="text" placeholder="CCO" id="txt_cco" class="form-control" runat="server" maxlength="500" />										
								        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Asunto</label>
                                        <div id="asunto_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_asunto">Asunto</label>
									        <input type="text" placeholder="Asunto" id="txt_asunto" class="form-control" runat="server" maxlength="500" />										
								        </div>
                                    </div>
                                    <div id="blk_body" class="col-sm-12" runat="server">
                                        <label>Cuerpo Mail</label>											
								        <div id="cuerpo_form" class="form-group" runat="server">
								            <label class="sr-only" for="txt_cuerpo">Cuerpo Mail</label>
									        <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                            <textarea id="txt_cuerpo" runat="server" placeholder="Cuerpo Mail" name="txtCuerpo" cols="80" rows="10" />
									        <script type="text/javascript">
									            CKEDITOR.replace('txt_cuerpo',
                                                {
                                                    placeholder: 'Cuerpo Mail'
	                                            });
                                            </script>
								        </div>
                                    </div>
                                    <div id="block_adjuntos" class="col-sm-12" runat="server">
                                        <label>Adjuntos</label>
                                    </div>	
                                    <div id="block_adj1" class="col-sm-12 col-lg-6" runat="server">
				                        <div class="col-sm-1">
                                            <h3 class="h4 text-color-primary margin-tb-5">1.-</h3>
                                        </div>
                                        <div class="col-sm-11 margin-b-10">
                                            <div class="form-group">
		                                        <div class="col-sm-12">
                                                    <asp:FileUpload ID="fuAdjunto1" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                </div>
                                                <div class="col-sm-10 margin-tb-5">
                                                    <a id="lnkAdjunto1" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                </div>
                                                <div id="blk_del_1" class="col-sm-2 hidden" runat="server">
                                                    <a href="javascript:void(0);" onclick="eliminar_adjunto(1)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                </div>    
                                            </div>
                                        </div>
                                    </div>
                                    <div id="block_adj2" class="col-sm-12 col-lg-6" runat="server">
                                        <div class="col-sm-1">
                                            <h3 class="h4 text-color-primary margin-tb-5">2.-</h3>
                                        </div>
                                        <div class="col-sm-11 margin-b-10">
                                            <div class="form-group">
		                                        <div class="col-sm-12">
                                                    <asp:FileUpload ID="fuAdjunto2" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                </div>
                                                <div class="col-sm-10 margin-tb-5">
                                                    <a id="lnkAdjunto2" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                </div>
                                                <div id="blk_del_2" class="col-sm-2 hidden" runat="server">
                                                    <a href="javascript:void(0);" onclick="eliminar_adjunto(2)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                </div>    
                                            </div>
                                        </div>
                                    </div>
                                    <div id="block_adj3" class="col-sm-12 col-lg-6" runat="server">
                                        <div class="col-sm-1">
                                            <h3 class="h4 text-color-primary margin-tb-5">3.-</h3>
                                        </div>
                                        <div class="col-sm-11 margin-b-10">
                                            <div class="form-group">
		                                        <div class="col-sm-12">
                                                    <asp:FileUpload ID="fuAdjunto3" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                </div>
                                                <div class="col-sm-10 margin-tb-5">
                                                    <a id="lnkAdjunto3" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                </div>
                                                <div id="blk_del_3" class="col-sm-2 hidden" runat="server">
                                                    <a href="javascript:void(0);" onclick="eliminar_adjunto(3)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                </div>    
                                            </div>
                                        </div>
                                    </div>
                                    <div id="block_adj4" class="col-sm-12 col-lg-6" runat="server">
                                        <div class="col-sm-1">
                                            <h3 class="h4 text-color-primary margin-tb-5">4.-</h3>
                                        </div>
                                        <div class="col-sm-11 margin-b-10">
                                            <div class="form-group">
		                                        <div class="col-sm-12">
                                                    <asp:FileUpload ID="fuAdjunto4" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                </div>
                                                <div class="col-sm-10 margin-tb-5">
                                                    <a id="lnkAdjunto4" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                </div>
                                                <div id="blk_del_4" class="col-sm-2 hidden" runat="server">
                                                    <a href="javascript:void(0);" onclick="eliminar_adjunto(4)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                </div>    
                                            </div>
                                        </div>
                                    </div>
                                    <div id="block_adj5" class="col-sm-12 col-lg-6" runat="server">
                                        <div class="col-sm-1">
                                            <h3 class="h4 text-color-primary margin-tb-5">5.-</h3>
                                        </div>
                                        <div class="col-sm-11 margin-b-10">
                                            <div class="form-group">
		                                        <div class="col-sm-12">
                                                    <asp:FileUpload ID="fuAdjunto5" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                </div>
                                                <div class="col-sm-10 margin-tb-5">
                                                    <a id="lnkAdjunto5" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                </div>
                                                <div id="blk_del_5" class="col-sm-2 hidden" runat="server">
                                                    <a href="javascript:void(0);" onclick="eliminar_adjunto(5)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                </div>    
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clear-both"></div>
                                    <div class="col-sm-12">
                                        <a id="btn_back" class="btn btn-primary btn-block-xs margin-xs-b-15" runat="server">Volver</a>
                                        <a id="btn_mail" href="javascript:void(0);" onclick="save_mail()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right hidden" runat="server">Guardar</a>
                                        <a id="btn_reasignar" href="javascript:void(0);" onclick="reasignar_comercial()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right hidden" runat="server">Reasignar Comercial</a>
                                        
                                        <asp:Button ID="btnGuardarMail" CssClass="hidden" runat="server" Enabled="false" Text="Guardar" OnClick="btnGuardarMail_Click" />
                                        <asp:Button ID="btnReasignarComercial" CssClass="hidden" runat="server" Enabled="false"
                                            Text="Reasignar Comercial" OnClick="btnReasignarComercial_Click" />
                                    </div>
                                    <div>
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto1" runat="server" Enabled="false" OnClick="btn_del_Adjunto1_Click" />
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto2" runat="server" Enabled="false" OnClick="btn_del_Adjunto2_Click" />                                        
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto3" runat="server" Enabled="false" OnClick="btn_del_Adjunto3_Click" />
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto4" runat="server" Enabled="false" OnClick="btn_del_Adjunto4_Click" />
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto5" runat="server" Enabled="false" OnClick="btn_del_Adjunto5_Click" />
                                    </div>
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
    
    <script type="text/javascript">
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para Reasignar al Comercial ---------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function reasignar_comercial() {
            var reasignar = validarReasignarComercial();
            if (reasignar) {
                var boton = document.getElementById('<%=btnReasignarComercial.ClientID %>');
                boton.removeAttribute("disabled");
                boton.click();
                return true;
            }
            else
                return false;
        }

        function validarReasignarComercial() {
            /// 1.- Sacar los parametros
            var comercial = $('#ddlComercial').val();
            var asunto = $('#txt_asunto').val();
            var cuerpo = CKEDITOR.instances['txt_cuerpo'].getData();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (comercial == "-1") {
                $('#comercial_form').addClass(' has-error');
                $('#txt_error').html('Hay que asignar un comercial');
                return false;
            }
            else if (asunto !== '' || cuerpo !== '') {
                if (asunto === "undefined" || asunto === undefined || asunto === "null" || asunto === null || asunto === '') {
                    $('#asunto_form').addClass(' has-error');
                    $('#txt_error').html('El campo Asunto es obligatorio');
                    $('#txt_asunto').attr("placeholder", "El campo Asunto es obligatorio");
                    subirArribaPagina();
                    return false;
                }
                else if (cuerpo === "undefined" || cuerpo === undefined || cuerpo === "null" || cuerpo === null || cuerpo === '') {
                    $('#cuerpo_form').addClass(' has-error');
                    $('#txt_error').html('El campo Cuerpo es obligatorio');
                    $('#txt_cuerpo').attr("placeholder", "El campo Cuerpo es obligatorio");
                    subirArribaPagina();
                    return false;
                }
                else
                    return true;
            }
            else
                return true;
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para las caracteristicas comunes ----------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function save_data_comun() {
            var data_comun = validarCaracteristicasComunes();
            if (data_comun) {
                var boton = document.getElementById('<%=btnComun.ClientID %>');
                boton.removeAttribute("disabled");
                boton.click();
                return true;
            }
            else
                return false;
        }

        function validarCaracteristicasComunes() {
            /// 1.- Sacar los parametros
            var tags = $('#txt_tags').val();
            var score = $('#txt_score').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (tags === "undefined" || tags === undefined || tags === "null" || tags === null || tags === '') {
                $('#tags_form').addClass(' has-error');
                $('#txt_error').html('El campo tags es obligatorio');
                subirArribaPagina();
                return false;
            }
            else if (score === "undefined" || score === undefined || score === "null" || score === null || score === '') {
                $('#score_form').addClass(' has-error');
                $('#txt_error').html('El campo score es obligatorio');
                subirArribaPagina();
                return false;
            }
            else if (score !== '' && !validarTelefono(score)) {
                $('#score_form').addClass(' has-error');
                $('#txt_error').html('El score es un campo numérico');
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para las Envios mail ----------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function save_mail() {
            var data_mail = validarEnviosMail();
            if (data_mail) {
                var boton = document.getElementById('<%=btnGuardarMail.ClientID %>');
                boton.removeAttribute("disabled");
                boton.click();
                return true;
            }
            else
                return false;
        }

        function validarEnviosMail() {
            /// 1.- Sacar los parametros
            var asunto = $('#txt_asunto').val();
            var cuerpo = CKEDITOR.instances['txt_cuerpo'].getData();

            /// 2.- Limpiar el error
            $('#txt_error').html('');
            
            /// 3.- Validar los datos
            if (asunto === "undefined" || asunto === undefined || asunto === "null" || asunto === null || asunto === '') {
                $('#asunto_form').addClass(' has-error');
                $('#txt_error').html('El campo Asunto es obligatorio');
                $('#txt_asunto').attr("placeholder", "El campo Asunto es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (!validarCarateresEspeciales(asunto)) {
                $('#asunto_form').addClass(' has-error');
                $('#txt_error').html('El campo Asunto contiene carácteres no válidos');
                subirArribaPagina();
                return false;
            }
            else if (cuerpo === "undefined" || cuerpo === undefined || cuerpo === "null" || cuerpo === null || cuerpo === '') {
                $('#cuerpo_form').addClass(' has-error');
                $('#txt_error').html('El campo Cuerpo es obligatorio');
                $('#txt_cuerpo').attr("placeholder", "El campo Cuerpo es obligatorio");
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para las caracteristicas añadir seguimiento ----------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function save_data_seguimiento() {
            var data_seguimiento = validarCaracteristicasSeguimiento();
            if (data_seguimiento) {
                var boton = document.getElementById('<%=btn_seguimiento.ClientID %>');
                boton.removeAttribute("disabled");
                boton.click();
                return true;
            }
            else
                return false;
        }

        function validarCaracteristicasSeguimiento() {
            /// 1.- Sacar los parametros
            var canal = $('#ddlCanal').val();
            var estado = $('#ddlEstado').val();
            var comentarios = CKEDITOR.instances['txt_comentario'].getData();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            if (canal == "") {
                $('#canal_form').addClass(' has-error');
                $('#txt_error').html('Hay que asignar un canal');
                subirArribaPagina();
                return false;
            }
            else if (estado == "") {
                $('#estado_form').addClass(' has-error');
                $('#txt_error').html('Hay que asignar un estado');
                subirArribaPagina();
                return false;
            }
            else if (comentarios === "undefined" || comentarios === undefined || comentarios === "null" || comentarios === null || comentarios === '') {
                $('#comentario_form').addClass(' has-error');
                $('#txt_error').html('Hay que rellenar el comentario');
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para eliminar los adjuntos de los mails ---------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function eliminar_adjunto(index) {
            if (index == 1) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 1?');
                if (confirm) {
                    var boton = document.getElementById('<%=btn_del_Adjunto1.ClientID %>');
                    boton.removeAttribute("disabled");
                    boton.click();
                }
            }
            else if (index == 2) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 2?');
                if (confirm) {
                    var boton = document.getElementById('<%=btn_del_Adjunto2.ClientID %>');
                    boton.removeAttribute("disabled");
                    boton.click();
                }
            }
            else if (index == 3) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 3?');
                if (confirm) {
                    var boton = document.getElementById('<%=btn_del_Adjunto3.ClientID %>');
                    boton.removeAttribute("disabled");
                    boton.click();
                }
            }
            else if (index == 4) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 4?');
                if (confirm) {
                    var boton = document.getElementById('<%=btn_del_Adjunto4.ClientID %>');
                    boton.removeAttribute("disabled");
                    boton.click();
                }
            }
            else if (index == 5) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 5?');
                if (confirm) {
                    var boton = document.getElementById('<%=btn_del_Adjunto5.ClientID %>');
                    boton.removeAttribute("disabled");
                    boton.click();
                }
            }
        }
    </script> 
</body>
</html>