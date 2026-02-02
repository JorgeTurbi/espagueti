<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lista-newsletter-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.lista_newsletter_mantenimiento" ValidateRequest="false" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Campaña mantenimiento</title>

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

        .card {background-clip: border-box; background-color: #fff; border-radius: 0.25rem; display: flex; flex-direction: column; min-width: 0; overflow-wrap: break-word; position: relative;}
        .card h4 {cursor: pointer;}
        .card-header {border-bottom: 1px solid #223266; margin-bottom: 0; padding: 1.5rem 1.25rem;}
        .card-body {flex: 1 1 auto; padding: 1.25rem;}        
        .card .card-header .btn-link[aria-expanded="false"]::before {border-color: #edab3a transparent transparent; border-style: solid; border-width: 7px 5px 0; content: ""; display: block; height: 0; position: absolute; right: 5px; top: 15px; width: 0;}
        .card .card-header .btn-link[aria-expanded="true"]::before {border-color: transparent transparent #edab3a; border-style: solid; border-width: 0 5px 7px; content: ""; display: block; height: 0; position: absolute; right: 5px; top: 15px; width: 0;}
        
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
							    <legend class="text-color-primary"><i class='fas fa-envelope'></i> Manteniento de Campaña</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-7">
                                    <label>Nombre campaña</label>
                                    <div id="nombre_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_nombre">Nombre campaña</label>
									    <input type="text" placeholder="Nombre campaña" id="txt_nombre" class="form-control" runat="server" maxlength="250" />
									    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
									    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        
                                        <asp:HiddenField ID="type_newsletter" Value="0" runat="server" />																		
								    </div>
                                </div>
                                <div class="col-sm-5">       
                                    <label>Prioridad</label>											
								    <div id="prioridad_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddl_prioridad">Prioridad</label>
									    <asp:DropDownList ID="ddl_prioridad" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                            <asp:ListItem Text="Seleccione" Selected="True" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="1 Máxima" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="10 Mínima" Value="10"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="col-xs-10 col-sm-10 no-padding">
                                        <label>Fecha Envío</label>
								        <div id="fecha_form" class="input-group date js-datepicker" runat="server">
								            <label class="sr-only" for="txt_fecha_envio">Fecha Envío</label>
									        <input type="text" id="txt_fecha_envio" class="form-control" runat="server" readonly="readonly" />
                                            <span class="input-group-addon glyphicon">
			                                    <span class="icon-calendar xs"></span>
                                            </span>
								        </div>
                                    </div>
                                    <div class="col-xs-2 col-sm-2 margin-t-20">
                                        <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                    </div>
                                </div>
                                <div class="clearfix visible-xs-block hidden"></div>
                                <div class="col-sm-3">       
                                    <label>Hora</label>											
								    <div id="hora_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddl_hora">Hora</label>
									    <asp:DropDownList ID="ddl_hora" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                            <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                            <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                            <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                            <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3">       
                                    <label>Minutos</label>											
								    <div id="minutos_form" class="form-group" runat="server">
								        <label class="sr-only" for="ddl_minutos">Minutos</label>
									    <asp:DropDownList ID="ddl_minutos" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                            <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                            <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                            <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                            <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                            <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                            <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                            <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                            <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                            <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                            <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                            <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                            <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                            <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                            <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                            <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                            <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                            <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                            <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                            <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                            <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                            <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                            <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                            <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                            <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                            <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                            <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                            <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                            <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                            <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                            <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                            <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                            <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                            <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                            <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                            <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                            <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                            <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                            <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                            <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                            <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                            <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                            <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                            <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                            <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                            <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="block_save" class="col-sm-12" runat="server">
                                    <a href="lista-newsletter.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                                    <div style="height: 40vh;"></div>
                                </div>
                                <div id="block_all" class="hidden" runat="server">
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
                                        <label>Asunto</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_asunto">Asunto</label>
									        <input type="text" placeholder="Asunto" id="txt_asunto" class="form-control" runat="server" maxlength="500" />										
								        </div>
                                    </div>
                                    <div id="blk_body" class="col-sm-12" runat="server">
                                        <label>Cuerpo Mail</label>											
								        <div id="cuerpo_form2" class="form-group" runat="server">
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
                                            <span class="h4 text-color-primary margin-tb-5">1.-</span>
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
                                            <span class="h4 text-color-primary margin-tb-5">2.-</span>
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
                                            <span class="h4 text-color-primary margin-tb-5">3.-</span>
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
                                            <span class="h4 text-color-primary margin-tb-5">4.-</span>
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
                                            <span class="h4 text-color-primary margin-tb-5">5.-</span>
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
                                    <div class="col-sm-6">
                                        <label>Marque la campaña principal a la que pertenece y queda asociada</label>
                                        <div class="form-group" runat="server">                                       
                                            <asp:DropDownList ID="ddl_newsletter_principal" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Seleccione la lista de suscriptores</label>
                                        <div class="form-group" runat="server">                                       
                                            <asp:DropDownList ID="dll_lista_suscriptores" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Landing asociada a la campaña</label>
                                        <div class="form-group" runat="server">                                       
                                            <asp:DropDownList ID="ddl_lista_landings" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Tags Open</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_tags">Tags open</label>
									        <input type="text" placeholder="Tags" id="txt_tags" class="form-control" runat="server" maxlength="500" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Tags Clic</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_tags_clics">Tags clic</label>
									        <input type="text" placeholder="Tags" id="txt_tags_clics" class="form-control" runat="server" maxlength="500" />										
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Usuario que envía</label>
                                        <div class="form-group" runat="server">                                       
                                            <asp:DropDownList ID="ddlUsuarioEspecial" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="blk_send_mail" class="col-sm-12 hidden" runat="server">
                                        <label>Mail para envío de prueba</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_mail_prueba">Mail para envío de prueba</label>
									        <input type="text" placeholder="Mail para envío de prueba" id="txt_mail_prueba" class="form-control" runat="server" />										
								        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <a href="lista-newsletter.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                    </div>
                                    <div class="col-sm-2">	
                                        <a href="javascript:void(0);" onclick="preview_mail()" class="btn btn-secondary padding15 btn-block-xs margin-xs-b-15">Preview</a>
                                    </div>
                                    <div class="col-sm-2">	
                                        <asp:Button ID="btnGuardarAll" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-xs-b-15" runat="server"
                                            Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardarAll_Click" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnMailPrueba" CssClass="btn btn-primary bg-color-text-soft text-color-white btn-block-xs margin-xs-b-15" runat="server"
                                            Text="Envío prueba" OnClick="btnMailPrueba_Click" Visible="false" />
                                    </div>
                                    <div class="col-sm-2">
                                        <a id="btn_envio" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15 hidden" runat="server">Planificar Envío</a>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnEnvioNoOpens" CssClass="btn btn-primary bg-color-info text-color-white btn-block-xs margin-xs-b-15" runat="server"
                                            Text="Reenviar No OPENS" OnClick="btnEnvioNoOpens_Click" Visible="false" />
                                    </div>
                                    <div>
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto1" runat="server" OnClick="btn_del_Adjunto1_Click" />
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto2" runat="server" OnClick="btn_del_Adjunto2_Click" />                                        
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto3" runat="server" OnClick="btn_del_Adjunto3_Click" />
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto4" runat="server" OnClick="btn_del_Adjunto4_Click" />
                                        <asp:ImageButton ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" ID="btn_del_Adjunto5" runat="server" OnClick="btn_del_Adjunto5_Click" />
                                        <asp:Button ID="btnEnvio" CssClass="hidden" Text="Planificar Envío" OnClick="btnEnvio_Click" runat="server" />
                                        
                                        <input type="hidden" id="hid_tipo" value="" runat="server" />
                                        <input type="hidden" id="hid_reintento" value="" runat="server" />
                                        <asp:Button ID="btn_copiar_datos" CssClass="hidden" Text="Copiar datos" OnClick="btnCopiar_Datos_Click" runat="server" />  
                                        <asp:Button ID="btn_reenvio" CssClass="hidden" Text="Planificar Envío" OnClick="btn_reenvio_Click" runat="server" />
                                        <input type="hidden" id="hid_adj_1" value="" runat="server" />
                                        <input type="hidden" id="hid_adj_2" value="" runat="server" />
                                        <input type="hidden" id="hid_adj_3" value="" runat="server" />
                                        <input type="hidden" id="hid_adj_4" value="" runat="server" />
                                        <input type="hidden" id="hid_adj_5" value="" runat="server" /> 
                                    </div>
                                </div>
                                <div id="block_no_opens" class="hidden" runat="server">
                                    <div class="clearfix"></div>
                                    <div class="col-sm-12 fa-1-6x text-color-primary padding-tb-20"><i class='fas fa-envelope'></i> Reintentos no opens</div>
                                    <div class="col-sm-12" runat="server">
                                        <div id="accordion-no-opens">
                                            <div class="card">
                                                <div class="card-header" id="heading-no-opens-1">                                
                                                    <a id="card_no_open_1" class="btn-link collapsed" data-toggle="collapse" href="#collapse_no_opens_1" aria-expanded="false" aria-controls="collapse_no_opens_1" runat="server"><h4 class="text-color-primary">Reintento 1</h4></a>
                                                </div>
                                                <div id="collapse_no_opens_1" class="border-blue collapse" aria-labelledby="heading-no-opens-1" data-parent="#accordion-no-opens" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_open_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_open_1">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_open_1" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_open_1')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_open_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_open_1">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_open_1" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_open_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_open_1">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_open_1" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_open" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_open">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_open" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_open">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_open" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_open">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_open" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_open">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_open" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_open_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_open_1">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_open_1" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_open_1" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_open_1',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_open" runat="server" ToolTip="Adjunto 1" TabIndex="9" CssClass="" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_open" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_open" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_open" runat="server" ToolTip="Adjunto 2" TabIndex="10" CssClass="" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_open" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_open" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_open" runat="server" ToolTip="Adjunto 3" TabIndex="11" CssClass="" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_open" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_open" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_open" runat="server" ToolTip="Adjunto 4" TabIndex="12" CssClass="" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_open" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_open" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_open" runat="server" ToolTip="Adjunto 5" TabIndex="13" CssClass="visible" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_open" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_open" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_no_open" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>     
                                            <div class="card">    
                                                <div class="card-header border-blue" id="heading-no-opens-2">
                                                    <a id="card_no_open_2" class="btn-link collapsed" data-toggle="collapse" href="#collapse_no_opens_2" aria-expanded="false" aria-controls="collapse_no_opens_2" runat="server"><h4 class="text-color-primary">Reintento 2</h4></a>
                                                </div>                                    
                                                <div id="collapse_no_opens_2" class="border-blue collapse" aria-labelledby="heading-no-opens-2" data-parent="#accordion-no-opens" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_open2_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_open_2">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_open_2" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_open_2')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_open2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_open_2">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_open_2" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_open2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_open_2">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_open_2" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_open2" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_open2">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_open2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_open2">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_open2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_open2">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_open2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_open2">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_open2" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_open2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_open_2">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_open_2" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_open_2" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_open_2',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_open2" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_open2" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_open2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_open2" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_open2" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_open2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_open2" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_open2" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_open2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_open2" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_open2" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_open2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_open2" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_open2" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_open2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_no_open2" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card">    
                                                <div class="card-header border-blue" id="heading-no-opens-3">
                                                    <a id="card_no_open_3" class="btn-link collapsed" data-toggle="collapse" href="#collapse_no_opens_3" aria-expanded="false" aria-controls="collapse_no_opens_3" runat="server"><h4 class="text-color-primary">Reintento 3</h4></a>
                                                </div>                                    
                                                <div id="collapse_no_opens_3" class="border-blue collapse" aria-labelledby="heading-no-opens-3" data-parent="#accordion-no-opens" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_open3_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_open_3">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_open_3" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_open_3')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_open3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_open_3">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_open_3" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_open3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_open_3">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_open_3" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_open3" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_open3">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_open3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_open3">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_open3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_open3">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_open3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_open3">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_open3" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_open3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_open_3">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_open_3" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_open_3" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_open_3',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_open3" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_open3" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_open3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_open3" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_open3" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_open3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_open3" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_open3" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_open3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_open3" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_open3" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_open3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_open3" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_open3" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_open3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_no_open3" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="block_no_clics" class="hidden" runat="server">
                                    <div class="clearfix"></div>
                                    <div class="col-sm-12 fa-1-6x text-color-primary padding-tb-20"><i class='fas fa-envelope'></i> Reintentos no clics</div>
                                    <div class="col-sm-12" runat="server">
                                        <div id="accordion-no-clics">
                                            <div class="card">
                                                <div class="card-header" id="heading-no-clics-1">                                
                                                    <a id="card_no_clic_1" class="btn-link collapsed" data-toggle="collapse" href="#collapse_no_clics_1" aria-expanded="false" aria-controls="collapse_no_clics_1" runat="server"><h4 class="text-color-primary">Reintento 1</h4></a>
                                                </div>
                                                <div id="collapse_no_clics_1" class="border-blue collapse" aria-labelledby="heading-no-clics-1" data-parent="#accordion-no-clics" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_no_clic_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_no_clic_1">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_no_clic_1" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_no_clic_1')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_no_clic_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_no_clic_1">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_no_clic_1" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_no_clic_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_no_clic_1">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_no_clic_1" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_no_clic" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_no_clic">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_no_clic" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_no_clic">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_no_clic" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_no_clic">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_no_clic" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_no_clic">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_no_clic" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_no_clic_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_no_clic_1">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_no_clic_1" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_no_clic_1" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_no_clic_1',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_no_clic" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_no_clic" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_no_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(1)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_no_clic" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_no_clic" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_no_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(2)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_no_clic" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_no_clic" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_no_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(3)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_no_clic" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_no_clic" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_no_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(4)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_no_clic" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_no_clic" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_no_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(5)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_no_clic" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>     
                                            <div class="card">    
                                                <div class="card-header border-blue" id="heading-no-clics-2">
                                                    <a id="card_no_clic_2" class="btn-link collapsed" data-toggle="collapse" href="#collapse_no_clics_2" aria-expanded="false" aria-controls="collapse_no_clics_2" runat="server"><h4 class="text-color-primary">Reintento 2</h4></a>
                                                </div>                                    
                                                <div id="collapse_no_clics_2" class="border-blue collapse" aria-labelledby="heading-no-clics-2" data-parent="#accordion-no-clics" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_no_clic2_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_no_clic_2">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_no_clic_2" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_no_clic_2')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_no_clic2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_no_clic_2">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_no_clic_2" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_no_clic2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_no_clic_2">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_no_clic_2" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_no_clic2" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_no_clic2">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_no_clic2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_no_clic2">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_no_clic2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_no_clic2">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_no_clic2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_no_clic2">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_no_clic2" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_no_clic2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_no_clic_2">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_no_clic_2" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_no_clic_2" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_no_clic_2',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_no_clic2" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_no_clic2" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_no_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(1)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_no_clic2" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_no_clic2" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_no_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(2)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_no_clic2" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_no_clic2" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_no_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(3)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_no_clic2" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_no_clic2" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_no_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(4)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_no_clic2" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_no_clic2" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_no_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(5)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_no_clic2" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card">    
                                                <div class="card-header border-blue" id="heading-no-clics-3">
                                                    <a id="card_no_clic_3" class="btn-link collapsed" data-toggle="collapse" href="#collapse_no_clics_3" aria-expanded="false" aria-controls="collapse_no_clics_3" runat="server"><h4 class="text-color-primary">Reintento 3</h4></a>
                                                </div>                                    
                                                <div id="collapse_no_clics_3" class="border-blue collapse" aria-labelledby="heading-no-clics-3" data-parent="#accordion-no-clics" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_no_clic3_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_no_clic_3">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_no_clic_3" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_no_clic_3')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_no_clic3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_no_clic_3">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_no_clic_3" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_no_clic3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_no_clic_3">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_no_clic_3" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_no_clic3" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_no_clic3">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_no_clic3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_no_clic3">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_no_clic3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_no_clic3">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_no_clic3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_no_clic3">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_no_clic3" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_no_clic3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_no_clic_3">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_no_clic_3" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_no_clic_3" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_no_clic_3',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_no_clic3" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_no_clic3" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_no_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(1)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_no_clic3" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_no_clic3" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_no_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(2)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_no_clic3" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_no_clic3" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_no_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(3)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_no_clic3" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_no_clic3" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_no_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(4)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_no_clic3" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_no_clic3" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_no_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(5)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_no_clic3" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="block_clics" class="hidden" runat="server">
                                    <div class="clearfix"></div>
                                    <div class="col-sm-12 fa-1-6x text-color-primary padding-tb-20"><i class='fas fa-envelope'></i> Reintentos clics</div>
                                    <div class="col-sm-12" runat="server">
                                        <div id="accordion-clics">
                                            <div class="card">
                                                <div class="card-header" id="heading-clics-1">                                
                                                    <a id="card_clic_1" class="btn-link collapsed" data-toggle="collapse" href="#collapse_clics_1" aria-expanded="false" aria-controls="collapse_clics_1" runat="server"><h4 class="text-color-primary">Reintento 1</h4></a>
                                                </div>
                                                <div id="collapse_clics_1" class="border-blue collapse" aria-labelledby="heading-clics-1" data-parent="#accordion-clics" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenvio_clic_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_clic_1">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_clic_1" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_clic_1')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_clic_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_clic_1">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_clic_1" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_clic_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_clic_1">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_clic_1" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_clic" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_clic">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_clic" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_clic">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_clic" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_clic">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_clic" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_clic">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_clic" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_clic_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_clic_1">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_clic_1" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_clic_1" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_clic_1',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_clic" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_clic" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(1)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_clic" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_clic" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(2)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_clic" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_clic" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(3)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_clic" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_clic" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(4)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_clic" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_clic" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_clic" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(5)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_clic" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>     
                                            <div class="card">    
                                                <div class="card-header border-blue" id="heading-clics-2">
                                                    <a id="card_clic_2" class="btn-link collapsed" data-toggle="collapse" href="#collapse_clics_2" aria-expanded="false" aria-controls="collapse_clics_2" runat="server"><h4 class="text-color-primary">Reintento 2</h4></a>
                                                </div>                                    
                                                <div id="collapse_clics_2" class="border-blue collapse" aria-labelledby="heading-clics-2" data-parent="#accordion-clics" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_clic2_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_clic_2">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_clic_2" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_open_2')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_clic2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_clic_2">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_clic_2" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_clic2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_clic_2">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_clic_2" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_clic2" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_clic2">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_clic2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_clic2">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_clic2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_clic2">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_clic2" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_clic2">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_clic2" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_clic2_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_clic_2">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_clic_2" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_clic_2" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_clic_2',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_clic2" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_clic2" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(1)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_clic2" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_clic2" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(2)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_clic2" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_clic2" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(3)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_clic2" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_clic2" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(4)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_clic2" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_clic2" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_clic2" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(5)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_clic2" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card">    
                                                <div class="card-header border-blue" id="heading-clics-3">
                                                    <a id="card_clic_3" class="btn-link collapsed" data-toggle="collapse" href="#collapse_clics_3" aria-expanded="false" aria-controls="collapse_clics_3" runat="server"><h4 class="text-color-primary">Reintento 3</h4></a>
                                                </div>                                    
                                                <div id="collapse_clics_3" class="border-blue collapse" aria-labelledby="heading-clics-3" data-parent="#accordion-clics" runat="server">
                                                    <div class="card-body clearfix">
                                                        <div class="col-sm-12 no-padding">
                                                            <div class="col-sm-5">
                                                                <div class="col-xs-10 col-sm-10 no-padding">
                                                                    <label>Fecha Envío</label>
								                                    <div id="fecha_reenv_clic3_form" class="input-group date js-datepicker" runat="server">
								                                        <label class="sr-only" for="txt_fecha_envio_clic_3">Fecha Envío</label>
									                                    <input type="text" id="txt_fecha_envio_clic_3" class="form-control" runat="server" readonly="readonly" />
                                                                        <span class="input-group-addon glyphicon">
			                                                                <span class="icon-calendar xs"></span>
                                                                        </span>
								                                    </div>
                                                                </div>
                                                                <div class="col-xs-2 col-sm-2 margin-t-20">
                                                                    <a href="javascript:void(0);" onclick="clean_input('txt_fecha_envio_clic_3')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="clearfix visible-xs-block hidden"></div>
                                                            <div class="col-sm-2">       
                                                                <label>Hora</label>											
								                                <div id="hora_reenv_clic3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_hora_clic_3">Hora</label>
									                                <asp:DropDownList ID="ddl_hora_clic_3" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">       
                                                                <label>Minutos</label>											
								                                <div id="min_reenv_clic3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="ddl_minutos_clic_3">Minutos</label>
									                                <asp:DropDownList ID="ddl_minutos_clic_3" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                                        <asp:ListItem Text="00" Selected="True" Value="00"></asp:ListItem>
                                                                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                                                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                                                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                                                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                                                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                                                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                                                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                                                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                                                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-3">
                                                                <label>&nbsp;</label>
                                                                <div>
                                                                    <a id="btn_copy_clic3" href="javascript:void(0);" onclick="" class="pull-right bold padding-r-5" title="Copiar datos de la campaña" runat="server"><small class='text-color-primary'><i class='fas fa-copy fa-2x'></i> Copiar datos campaña</small></a>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-6">
                                                                <label>Nombre From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_nombre_from_clic3">Nombre From</label>
									                                <input type="text" placeholder="Nombre From" id="txt_nombre_from_clic3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Mail From</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_mail_from_clic3">Mail From</label>
									                                <input type="text" placeholder="Mail From" id="txt_mail_from_clic3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Reply To</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_reply_to_clic3">Reply To</label>
									                                <input type="text" placeholder="Reply To" id="txt_reply_to_clic3" class="form-control" runat="server" maxlength="250" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <label>Asunto</label>
                                                                <div class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_asunto_clic3">Asunto</label>
									                                <input type="text" placeholder="Asunto" id="txt_asunto_clic3" class="form-control" runat="server" maxlength="500" />										
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12" runat="server">
                                                                <label>Cuerpo Mail</label>											
								                                <div id="cuerpo_clic3_form" class="form-group" runat="server">
								                                    <label class="sr-only" for="txt_cuerpo_clic_3">Cuerpo Mail</label>
									                                <!-- CONTROL DE TEXTO ENRIQUECIDO -->
                                                                    <textarea id="txt_cuerpo_clic_3" runat="server" placeholder="Cuerpo Mail" name="txt_cuerpo_clic_3" cols="80" rows="10" />
									                                <script type="text/javascript">
									                                    CKEDITOR.replace('txt_cuerpo_clic_3',
                                                                        {
                                                                            placeholder: 'Cuerpo Mail'
	                                                                    });
                                                                    </script>
								                                </div>
                                                            </div>
                                                            <div class="col-sm-12">
                                                                <label>Adjuntos</label>
                                                            </div>	
                                                            <div class="col-sm-12 col-lg-6">
				                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">1.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto1_clic3" runat="server" ToolTip="Adjunto 1" TabIndex="9" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto1_clic3" runat="server" target="_blank" title="Adjunto 1" tabindex="9" />
                                                                        </div>
                                                                        <div id="blk_del_1_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(1)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div> 
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">2.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto2_clic3" runat="server" ToolTip="Adjunto 2" TabIndex="10" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto2_clic3" runat="server" target="_blank" title="Adjunto 2" tabindex="10" />
                                                                        </div>
                                                                        <div id="blk_del_2_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(2)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">3.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto3_clic3" runat="server" ToolTip="Adjunto 3" TabIndex="11" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto3_clic3" runat="server" target="_blank" title="Adjunto 3" tabindex="11" />
                                                                        </div>
                                                                        <div id="blk_del_3_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(3)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">4.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto4_clic3" runat="server" ToolTip="Adjunto 4" TabIndex="12" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto4_clic3" runat="server" target="_blank" title="Adjunto 4" tabindex="12" />
                                                                        </div>
                                                                        <div id="blk_del_4_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(4)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-12 col-lg-6">
                                                                <div class="col-sm-2">
                                                                    <span class="h4 text-color-primary margin-tb-5">5.-</span>
                                                                </div>
                                                                <div class="col-sm-10 margin-b-10">
                                                                    <div class="form-group">
		                                                                <div class="col-sm-12">
                                                                            <asp:FileUpload ID="fuAdjunto5_clic3" runat="server" ToolTip="Adjunto 5" TabIndex="13" />
                                                                        </div>
                                                                        <div class="col-sm-10 margin-tb-5">
                                                                            <a id="lnkAdjunto5_clic3" runat="server" target="_blank" title="Adjunto 5" tabindex="13" />
                                                                        </div>
                                                                        <div id="blk_del_5_clic3" class="col-sm-2 hidden" runat="server">
                                                                            <a href="javascript:void(0);" onclick="eliminar_adjunto(5)" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                                        </div>    
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="clear-both"></div>
                                                            <div class="col-sm-2 col-sm-offset-10">
                                                                <a id="btn_envio_clic3" href="javascript:void(0);" onclick="" class="btn btn-primary bg-color-primary text-color-white btn-block-xs margin-xs-b-15" runat="server">Planificar Envío</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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
        // Funciones para borrar la fecha ----------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function clean_input(data) {
            $("#" + data).val('');
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function validarFormulario() {
            /// 1.- Sacar los parametros
            var nombre = $('#txt_nombre').val();
            var prioridad = $('#ddl_prioridad').val();
            var fecha_envio = $('#txt_fecha_envio').val();

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
            else if (fecha_envio == "undefined" || fecha_envio == undefined || fecha_envio == "null" || fecha_envio == null || fecha_envio == '') {
                $('#fecha_form').addClass(' has-error');
                $('#txt_error').html('La fecha de envío es obligatoria');
                $('#txt_fecha_envio').attr("placeholder", "La fecha de envío es obligatoria");
                subirArribaPagina();
                return false;
            }
            else if (prioridad == "-1") {
                $('#prioridad_form').addClass(' has-error');
                $('#txt_error').html('La prioridad es obligatoria');
                return false;
            }            
            else
                return true;
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para recuperar los datos del cuerpo del mail ----------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function preview_mail() {
            if ($('#type_newsletter').val() === "1")
                window.open('preview-newsletter.aspx?idc=' + getParams('idc'), '_blank');
            else {
                var texto = CKEDITOR.instances['txt_cuerpo'].getData();
                if (texto != "undefined" && texto != undefined && texto != "null" && texto != null && texto != '') {
                    setFileData(texto);
                    window.open('preview-mail.aspx', '_blank');
                }
            }
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para eliminar los adjuntos de los mails ---------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function eliminar_adjunto(index) {
            if (index == 1) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 1?');
                if (confirm)
                    $('#btn_del_Adjunto1').click();
            }
            else if (index == 2) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 2?');
                if (confirm)
                    $('#btn_del_Adjunto2').click();
            }
            else if (index == 3) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 3?');
                if (confirm)
                    $('#btn_del_Adjunto3').click();
            }
            else if (index == 4) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 4?');
                if (confirm) $('#btn_del_Adjunto4').click();
            }
            else if (index == 5) {
                var confirm = confirm_message('¿Seguro que desea eliminar el adjunto 5?');
                if (confirm) $('#btn_del_Adjunto5').click();
            }
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para planificar el envío ------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function planificar_envio() {
            $('#btnEnvio').click();
        }
        
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para copiar datos -------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function copiar_datos(tipo, reintento) {
            /// 1.- Poner datos  
            $('#hid_tipo').val(tipo);
            $('#hid_reintento').val(reintento);

            /// 2.- Dar al botón
            $('#btn_copiar_datos').click();
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para planificar el reenvío ----------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function planificar_reenvio(tipo, reintento) {
            /// 1.- Poner datos  
            $('#hid_tipo').val(tipo);
            $('#hid_reintento').val(reintento);

            /// 2.- Dar al botón
            $('#btn_reenvio').click();
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para quitar adjuntos para el reenvío ------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function eliminar_adjunto_reenvio(tipo, reintento, adjunto) {
            if (tipo == 5) {
                if (reintento == 1) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_open').removeClass('hidden');
                        $('#lnkAdjunto1_open').html('');
                        $('#blk_del_1_open').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_open').removeClass('hidden');
                        $('#lnkAdjunto2_open').html('');
                        $('#blk_del_2_open').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_open').removeClass('hidden');
                        $('#lnkAdjunto3_open').html('');
                        $('#blk_del_3_open').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_open').removeClass('hidden');
                        $('#lnkAdjunto4_open').html('');
                        $('#blk_del_4_open').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_open').removeClass('hidden');
                        $('#lnkAdjunto5_open').html('');
                        $('#blk_del_5_open').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
                else if (reintento == 2) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_open2').removeClass('hidden');
                        $('#lnkAdjunto1_open2').html('');
                        $('#blk_del_1_open2').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_open2').removeClass('hidden');
                        $('#lnkAdjunto2_open2').html('');
                        $('#blk_del_2_open2').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_open2').removeClass('hidden');
                        $('#lnkAdjunto3_open2').html('');
                        $('#blk_del_3_open2').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_open2').removeClass('hidden');
                        $('#lnkAdjunto4_open2').html('');
                        $('#blk_del_4_open2').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_open2').removeClass('hidden');
                        $('#lnkAdjunto5_open2').html('');
                        $('#blk_del_5_open2').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
                else if (reintento == 3) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_open3').removeClass('hidden');
                        $('#lnkAdjunto1_open3').html('');
                        $('#blk_del_1_open3').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_open3').removeClass('hidden');
                        $('#lnkAdjunto2_open3').html('');
                        $('#blk_del_2_open3').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_open3').removeClass('hidden');
                        $('#lnkAdjunto3_open3').html('');
                        $('#blk_del_3_open3').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_open3').removeClass('hidden');
                        $('#lnkAdjunto4_open3').html('');
                        $('#blk_del_4_open3').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_open3').removeClass('hidden');
                        $('#lnkAdjunto5_open3').html('');
                        $('#blk_del_5_open3').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
            }
            else if (tipo == 4) {
                if (reintento == 1) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_no_clic').removeClass('hidden');
                        $('#lnkAdjunto1_no_clic').html('');
                        $('#blk_del_1_no_clic').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_no_clic').removeClass('hidden');
                        $('#lnkAdjunto2_no_clic').html('');
                        $('#blk_del_2_no_clic').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_no_clic').removeClass('hidden');
                        $('#lnkAdjunto3_no_clic').html('');
                        $('#blk_del_3_no_clic').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_no_clic').removeClass('hidden');
                        $('#lnkAdjunto4_no_clic').html('');
                        $('#blk_del_4_no_clic').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_no_clic').removeClass('hidden');
                        $('#lnkAdjunto5_no_clic').html('');
                        $('#blk_del_5_no_clic').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
                else if (reintento == 2) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_no_clic2').removeClass('hidden');
                        $('#lnkAdjunto1_no_clic2').html('');
                        $('#blk_del_1_no_clic2').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_no_clic2').removeClass('hidden');
                        $('#lnkAdjunto2_no_clic2').html('');
                        $('#blk_del_2_no_clic2').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_no_clic2').removeClass('hidden');
                        $('#lnkAdjunto3_no_clic2').html('');
                        $('#blk_del_3_no_clic2').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_no_clic2').removeClass('hidden');
                        $('#lnkAdjunto4_no_clic2').html('');
                        $('#blk_del_4_no_clic2').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_no_clic2').removeClass('hidden');
                        $('#lnkAdjunto5_no_clic2').html('');
                        $('#blk_del_5_no_clic2').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
                else if (reintento == 3) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_no_clic3').removeClass('hidden');
                        $('#lnkAdjunto1_no_clic3').html('');
                        $('#blk_del_1_no_clic3').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_no_clic3').removeClass('hidden');
                        $('#lnkAdjunto2_no_clic3').html('');
                        $('#blk_del_2_no_clic3').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_no_clic3').removeClass('hidden');
                        $('#lnkAdjunto3_no_clic3').html('');
                        $('#blk_del_3_no_clic3').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_no_clic3').removeClass('hidden');
                        $('#lnkAdjunto4_no_clic3').html('');
                        $('#blk_del_4_no_clic3').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_no_clic3').removeClass('hidden');
                        $('#lnkAdjunto5_no_clic3').html('');
                        $('#blk_del_5_no_clic3').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
            }
            else if (tipo == 6) {
                if (reintento == 1) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_clic').removeClass('hidden');
                        $('#lnkAdjunto1_clic').html('');
                        $('#blk_del_1_clic').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_clic').removeClass('hidden');
                        $('#lnkAdjunto2_clic').html('');
                        $('#blk_del_2_clic').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_clic').removeClass('hidden');
                        $('#lnkAdjunto3_clic').html('');
                        $('#blk_del_3_clic').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_clic').removeClass('hidden');
                        $('#lnkAdjunto4_clic').html('');
                        $('#blk_del_4_clic').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_clic').removeClass('hidden');
                        $('#lnkAdjunto5_clic').html('');
                        $('#blk_del_5_clic').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
                else if (reintento == 2) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_clic2').removeClass('hidden');
                        $('#lnkAdjunto1_clic2').html('');
                        $('#blk_del_1_clic2').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_clic2').removeClass('hidden');
                        $('#lnkAdjunto2_clic2').html('');
                        $('#blk_del_2_clic2').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_clic2').removeClass('hidden');
                        $('#lnkAdjunto3_clic2').html('');
                        $('#blk_del_3_clic2').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_clic2').removeClass('hidden');
                        $('#lnkAdjunto4_clic2').html('');
                        $('#blk_del_4_clic2').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_clic2').removeClass('hidden');
                        $('#lnkAdjunto5_clic2').html('');
                        $('#blk_del_5_clic2').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
                else if (reintento == 3) {
                    if (adjunto == 1) {
                        $('#fuAdjunto1_clic3').removeClass('hidden');
                        $('#lnkAdjunto1_clic3').html('');
                        $('#blk_del_1_clic3').addClass(' hidden');
                        $('#hid_adj_1').val(1);
                    }
                    else if (adjunto == 2) {
                        $('#fuAdjunto2_clic3').removeClass('hidden');
                        $('#lnkAdjunto2_clic3').html('');
                        $('#blk_del_2_clic3').addClass(' hidden');
                        $('#hid_adj_2').val(1);
                    }
                    else if (adjunto == 3) {
                        $('#fuAdjunto3_clic3').removeClass('hidden');
                        $('#lnkAdjunto3_clic3').html('');
                        $('#blk_del_3_clic3').addClass(' hidden');
                        $('#hid_adj_3').val(1);
                    }
                    else if (adjunto == 4) {
                        $('#fuAdjunto4_clic3').removeClass('hidden');
                        $('#lnkAdjunto4_clic3').html('');
                        $('#blk_del_4_clic3').addClass(' hidden');
                        $('#hid_adj_4').val(1);
                    }
                    else if (adjunto == 5) {
                        $('#fuAdjunto5_clic3').removeClass('hidden');
                        $('#lnkAdjunto5_clic3').html('');
                        $('#blk_del_5_clic3').addClass(' hidden');
                        $('#hid_adj_5').val(1);
                    }
                }
            }
        }
    </script>  
</body>
</html>