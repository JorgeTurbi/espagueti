<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ficha-alumno-crm-aux.aspx.cs" Inherits="campus_sbs_admin.ficha_alumno_crm_aux" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header-crm.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Ficha alumno Aux</title>

    <!-- CSS 
    =================================================== -->	    
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/datepicker_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>
        <%: Styles.Render("~/bundles/datatables_css") %>
    </asp:PlaceHolder>

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

    <section class="wrapper">
        <div class="padding-nav">
            <div class="col pt-1">
                <form id="form1" runat="server"> 
                    <div>
                        <div id="block_error" class="form-group has-error" runat="server">
                            <span id="txt_error" class="help-block text-center" runat="server"></span>
                        </div>
                        <div id="blk_AP" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Petición de información</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <label>Fecha Lead</label>
							    <div id="fecha_lead_form" class="input-group date" runat="server">
								    <label class="sr-only" for="txtFechaLead">Fecha Lead</label>
								    <input type="text" id="txtFechaLead" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-4">
                                <label>Origen</label>													
							    <div id="origen_ap_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlOrigenAP">Origen</label>
                                    <select id="ddlOrigenAP" class="selectpicker w-100" title="Seleccione un origen" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-4">
                                <label>Pertenencia</label>													
							    <div id="pertenencia_ap_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlertenenciaAP">Pertenencia</label>
                                    <select id="ddlPertenenciaAP" class="selectpicker w-100" title="Seleccione una pertenencia" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>    
                            <div class="col-sm-12">
                                <label>Curso</label>											
							    <div id="curso_ap_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlCursoAP">País de residencia</label>
								    <select id="ddlCursoAP" class="selectpicker w-100" title="Seleccione un curso" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>                        
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save" href="javascript: void(0);" onclick="validarFormularioAP()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardar" CssClass="hidden" runat="server" OnClick="btnGuardar_Click" />
                            </div>
                        </div>
                        <div id="blk_origen" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Origen</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-6">
                                <label>Fecha Origen</label>
							    <div id="fecha_origen_form" class="input-group date" runat="server">
								    <label class="sr-only" for="txtFechaOrigen">Fecha Origen</label>
								    <input type="text" id="txtFechaOrigen" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Origen</label>													
							    <div id="origen_or_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlOrigenOR">Origen</label>
                                    <select id="ddlOrigenOR" class="selectpicker w-100" title="Seleccione un origen" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>                     
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_origen" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save_origen" href="javascript: void(0);" onclick="validarFormularioOR()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardarOrigen" CssClass="hidden" runat="server" OnClick="btnGuardarOrigen_Click" />
                            </div>
                        </div>
                        <div id="blk_links" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-link'></i> Links</legend>
                                </fieldset>
                            </div>                            
                            <div class="col-sm-6">
                                <label>Tipo link</label>													
							    <div id="lnk_tipo_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlLinks">Tipo link</label>
                                    <select id="ddlLinks" class="selectpicker w-100" title="Seleccione un link" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Url</label>											
							    <div id="lnk_url_form" class="form-group" runat="server">
								    <label class="sr-only" for="txt_url_link">Url</label>
								    <input type="text" placeholder="Url" id="txt_url_link" class="form-control" runat="server" />
							    </div>
                            </div>                     
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_link" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save_link" href="javascript: void(0);" onclick="validarFormularioLnk()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardarLink" CssClass="hidden" runat="server" OnClick="btnGuardarLink_Click" />
                            </div>
                        </div>
                        <div id="blk_fundacion" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Fundación y descuentos</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <label>Fecha Seguimiento Fundación</label>
							    <div id="fecha_fund_form" class="input-group date" runat="server">
								    <label class="sr-only" for="txtFechaSeguimientoFund">Fecha Seguimiento Fundación</label>
								    <input type="text" id="txtFechaSeguimientoFund" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-4">
                                <label>Beca</label>											
							    <div id="beca_fund_form" class="form-group" runat="server">
								    <label class="sr-only" for="txt_beca_fund">Beca</label>
								    <input type="text" id="txt_beca_fund" class="form-control" runat="server" />
							    </div>
                            </div>  
                            <div class="col-sm-4">
                                <label>Descuento</label>											
							    <div id="desc_fund_form" class="form-group" runat="server">
								    <label class="sr-only" for="txt_desc_fund">Descuento</label>
								    <input type="text" id="txt_desc_fund" class="form-control" runat="server" />
							    </div>
                            </div>    
                            <div class="col-sm-12">
                                <label>Comentarios</label>
                                <div id="comentarios_fund_form" class="form-group" runat="server">
                                    <label class="sr-only" for="txt_comentarios_fund">Comentarios</label>
								    <textarea class="form-control" id="txt_comentarios_fund" rows="3" runat="server"></textarea>
                                </div>
                            </div>                  
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_fund" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save_fund" href="javascript: void(0);" onclick="validarFormularioFund()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardarFund" CssClass="hidden" runat="server" OnClick="btnGuardarFund_Click" />
                            </div>
                        </div>
                        <div id="blk_cambio_edicion" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Cambio de edición</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-6">
                                <label>Edición actual</label>													
							    <div class="form-group">
								    <label class="sr-only" for="ddlEdicionActual">Edición actual</label>
                                    <select id="ddlEdicionActual" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" disabled="disabled" runat="server"></select>
							    </div>
                            </div> 
                            <div class="col-sm-6">
                                <label>Nueva edición</label>													
							    <div class="form-group">
								    <label class="sr-only" for="ddlNuevaEdicion">Nueva edición</label>
                                    <select id="ddlNuevaEdicion" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>                
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_edicion" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <asp:Button ID="btn_save_edicion" CssClass="btn btn-primary bg-green text-color-white btn-block-xs pull-right" runat="server" Text="Guardar" OnClick="btnGuardarEdicion_Click" />
                            </div>
                        </div>
                        <div id="blk_precio" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Precio y Fecha fin acceso</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <label>Precio</label>											
							    <div id="precio_form" class="form-group" runat="server">
								    <label class="sr-only" for="txt_precio">Precio</label>
								    <input type="text" id="txt_precio" class="form-control" runat="server" />
							    </div>
                            </div>  
                            <div class="col-sm-4">
                                <label>Fecha Fin Acceso</label>
							    <div class="input-group date col-sm-10 p-0">
								    <label class="sr-only" for="txtFechaFinAcceso">Fecha Fin Acceso</label>
								    <input type="text" id="txtFechaFinAcceso" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                                <div class="col-sm-2 text-center pt-1 pl-1">
                                    <a onclick="clean_input('txtFechaFinAcceso')" class="fas fa-times-circle fa-1-6x text-color-red" href="javascript:void(0);"></a>
                                </div> 
                            </div>                                     
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_precio" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <asp:Button ID="btn_save_precio" CssClass="btn btn-primary bg-green text-color-white btn-block-xs pull-right" runat="server" Text="Guardar" OnClick="btnGuardarPrecio_Click" />
                            </div>
                        </div>
                        <div id="blk_asignacion_comercial" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Asignación comercial</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <label>Fecha Asignación comercial</label>
							    <div id="fecha_asig_comercial_form" class="input-group date" runat="server">
								    <label class="sr-only" for="txtFechaAsignacionComercial">Fecha Asignación comercial</label>
								    <input type="text" id="txtFechaAsignacionComercial" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-4">
                                <label>Comercial</label>													
							    <div id="comercial_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlComerciales">Comercial</label>
                                    <select id="ddlComerciales" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-4">
                                <label>Precio</label>											
							    <div id="precio_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="txt_precio_asig">Precio</label>
								    <input type="text" id="txt_precio_asig" class="form-control" runat="server" />
							    </div>
                            </div>  
                            <div class="col-sm-6">
                                <label>Docencia</label>													
							    <div id="doc_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlDocenciaAsig">Docencia</label>
                                    <select id="ddlDocenciaAsig" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Curso</label>													
							    <div id="curso_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlCursoAsig">Curso</label>
                                    <select id="ddlCursoAsig" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Matriculado en</label>													
							    <div id="pertenencia_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlPertenenciaAsig">Matriculado en</label>
                                    <select id="ddlPertenenciaAsig" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Metodología</label>													
							    <div id="metodologia_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlMetodologiaAsig">Metodología</label>
                                    <select id="ddlMetodologiaAsig" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Captación tipo</label>													
							    <div id="captacion_tipo_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlCaptacionTipoAsig">Captación tipo</label>
                                    <select id="ddlCaptacionTipoAsig" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Captación origen</label>													
							    <div id="captacion_origen_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlCaptacionOrigenAsig">Captación origen</label>
                                    <select id="ddlCaptacionOrigenAsig" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-2">
                                <label>Acceso por</label>													
							    <div id="acceso_asig_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlAcceso">Acceso por</label>
                                    <select id="ddlAcceso" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-2 pt-2">
                                <label>&nbsp;</label>
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkTitPropio" runat="server" />
                                    <label class="custom-control-label" for="chkTitPropio">Título Propio</label>
                                </div>
                            </div>
                            <div class="col-sm-2 pt-2">
                                <label>&nbsp;</label>
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkTitUcam" runat="server" />
                                    <label class="custom-control-label" for="chkTitUcam">Título UCAM</label>
                                </div>
                            </div>
                            <div class="col-sm-2 pt-2">
                                <label>&nbsp;</label>
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkTitSbsAd" runat="server" />
                                    <label class="custom-control-label" for="chkTitSbsAd">Título SBS AD</label>
                                </div>
                            </div>
                            <div class="col-sm-2 pt-2">
                                <label>&nbsp;</label>
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkTitSbsDo" runat="server" />
                                    <label class="custom-control-label" for="chkTitSbsDo">Título SBS DO</label>
                                </div>
                            </div>
                            <div class="col-sm-2 pt-2">
                                <label>&nbsp;</label>
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkTitCualificam" runat="server" />
                                    <label class="custom-control-label" for="chkTitCualificam">Título Cualificam</label>
                                </div>
                            </div>    
                            <div class="col-sm-12 pt-2">
                                <label>Comentarios</label>
                                <div id="comentarios_asig_form" class="form-group" runat="server">
                                    <label class="sr-only" for="txt_comentarios_asig">Comentarios</label>
								    <textarea class="form-control" id="txt_comentarios_asig" rows="3" runat="server"></textarea>
                                </div>
                            </div>                  
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_asig" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save_asig" href="javascript: void(0);" onclick="validarFormularioAsig()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardarAsig" CssClass="hidden" runat="server" OnClick="btnGuardarAsig_Click" />
                            </div>
                        </div>
                        <div id="blk_asignacion_comercial_all" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Asignación comercial</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <label>Fecha Pago matrícula</label>
							    <div class="input-group date">
								    <label class="sr-only" for="txtFechaPagoMatricula">Fecha Pago matrícula</label>
								    <input type="text" id="txtFechaPagoMatricula" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-4">
                                <label>Fecha Envío contrato</label>
							    <div class="input-group date">
								    <label class="sr-only" for="txtFechaEnvContrato">Fecha Envío contrato</label>
								    <input type="text" id="txtFechaEnvContrato" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-4">
                                <label>Fecha Recepción contrato</label>
							    <div class="input-group date">
								    <label class="sr-only" for="txtFechaRecepContrato">Fecha Recepción contrato</label>
								    <input type="text" id="txtFechaRecepContrato" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-2">
                                <label>PVP</label>											
							    <div class="form-group">
								    <label class="sr-only" for="txt_pvp">PVP</label>
								    <input type="text" id="txt_pvp" class="form-control" runat="server" />
							    </div>
                            </div>
                            <div class="col-sm-2">
                                <label>PVP (Sin aportaciones)</label>											
							    <div class="form-group">
								    <label class="sr-only" for="txt_pvp_becado">PVP Becado (Sin aportaciones)</label>
								    <input type="text" id="txt_pvp_becado" class="form-control" runat="server" />
							    </div>
                            </div>
                            <div class="col-sm-2">
                                <label>Beca Fund. %</label>											
							    <div class="form-group">
								    <label class="sr-only" for="txt_beca_fundacion">Beca Fund. %</label>
								    <input type="text" id="txt_beca_fundacion" class="form-control" runat="server" />
							    </div>
                            </div>
                            <div class="col-sm-2">
                                <label>Otros descuentos %</label>											
							    <div class="form-group">
								    <label class="sr-only" for="txt_otros_descuentos">Otros descuentos %</label>
								    <input type="text" id="txt_otros_descuentos" class="form-control" runat="server" />
							    </div>
                            </div>
                                <div class="col-sm-2">
                                <label>Aportación Fundación</label>											
							    <div class="form-group">
								    <label class="sr-only" for="txt_fundacion">Aportación Fundación</label>
								    <input type="text" id="txt_fundacion" class="form-control" runat="server" />
							    </div>
                            </div>
                                <div class="col-sm-2">
                                <label>Ap. Universidad</label>											
							    <div class="form-group">
								    <label class="sr-only" for="txt_universidad">Aportación Universidad</label>
								    <input type="text" id="txt_universidad" class="form-control" runat="server" />
							    </div>
                            </div> 
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_asig_comercial" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <asp:Button ID="btnGuardarAsigAll" CssClass="btn btn-primary bg-green text-color-white btn-block-xs pull-right" runat="server" OnClick="btnGuardarAsigAll_Click" Text="Guardar" />
                            </div>
                        </div>
                        <div id="blk_pago" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Pagos</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-3">
                                <label>Fecha Estimada</label>
							    <div id="fecha_estimada_form" class="input-group date">
								    <label class="sr-only" for="txtFechaEstimada">Fecha Estimada</label>
								    <input type="text" id="txtFechaEstimada" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Euros Estimados</label>											
							    <div id="euros_estimados_form" class="form-group" runat="server">
								    <label class="sr-only" for="txt_euros_est">Euros Estimados</label>
								    <input type="text" id="txt_euros_est" class="form-control" runat="server" />
							    </div>
                            </div>  
                            <div class="col-sm-3">
                                <label>Fecha Real</label>
							    <div class="input-group date">
								    <label class="sr-only" for="txtFechaEstimada">Fecha Real</label>
								    <input type="text" id="txtFechaReal" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Euros Reales</label>											
							    <div class="form-group">
								    <label class="sr-only" for="txt_euros_real">Euros Reales</label>
								    <input type="text" id="txt_euros_real" class="form-control" runat="server" />
							    </div>
                            </div>
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_pagos" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save_pay" href="javascript: void(0);" onclick="validarFormularioPay()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardarPago" CssClass="hidden" runat="server" OnClick="btnGuardarPago_Click" />
                            </div>
                        </div>
                        <div id="blk_documentacion" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Documentación del alumno</legend>
                                </fieldset>
                            </div>                            
                            <div class="col-sm-3">
                                <label>Tipo Documentación</label>													
							    <div id="type_doc_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlTipoDoc">Tipo Documentación</label>
                                    <select id="ddlTipoDoc" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server">
                                        <option value="">Seleccione un tipo</option>
                                        <option value="Cedula Identidad">Cedula Identidad</option>
                                        <option value="Contrato">Contrato</option>
                                        <option value="CV">CV</option>
                                        <option value="Factura">Factura</option>
                                        <option value="NIF/NIE">NIF/NIE</option>
                                        <option value="Otros">Otros</option>
                                        <option value="Pasaporte">Pasaporte</option>                                        
                                        <option value="Título">Título</option>
                                        <option value="Url">Url</option>
                                    </select>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Fecha Caducidad</label>
							    <div id="fecha_caducidad_form" class="input-group date col-sm-9 p-0" runat="server">
								    <label class="sr-only" for="txtFechaCaducidad">Fecha Caducidad</label>
								    <input type="text" id="txtFechaCaducidad" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                                <div class="col-sm-3 text-center pt-2 pl-1">
                                    <a onclick="clean_input('txtFechaCaducidad')" class="fas fa-times-circle fa-1-6x text-color-red" href="javascript:void(0);"></a>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Docencia</label>													
							    <div id="docencias_doc_form" class="form-group" runat="server">
								    <label class="sr-only" for="ddlDocenciaDocumento">Docencia</label>
                                    <select id="ddlDocenciaDocumento" class="selectpicker w-100" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label class="w-100">Fichero</label>	
                                <input type="hidden" id="documento_usuario" value="" runat="server" />
                                <div id="block_upload_documento" class="col-sm-4 p-0" runat="server">
                                    <a id="upload_documento" class='far fa-image fa-2x' href='javascript:void(0);' data-toggle='modal' data-target='#modal_documento' title="Añadir documento" runat="server" />
                                </div>
                                <div id="block_see" class="col-sm-4 p-0 hidden" runat="server">
                                    <a id='lnk_documento' href='#' target='_blank' title='Ver documento' class='fas fa-eye fa-2x' runat='server'></a>
                                </div>
                                <div id="block_delete_documento" class="col-sm-4 p-0 hidden" runat="server">
                                    <a id="delete_doc" onclick='delete_documento()' class='fas fa-times-circle fa-2x text-red' href='javascript:void(0);' title="Eliminar documento" runat="server" />
                                    <div id="delete_doc_user"></div>
                                </div>
                            </div>  
                            <div class="col-sm-12">
                                <label>Descripción</label>
                                <div id="decripcion_doc_form" class="form-group" runat="server">
                                    <label class="sr-only" for="txt_descripcion_doc">Descripción</label>
								    <textarea class="form-control" id="txt_descripcion_doc" rows="3" runat="server"></textarea>
                                </div>
                            </div>                  
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_doc" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save_doc" href="javascript: void(0);" onclick="validarFormularioDoc()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardarDoc" CssClass="hidden" runat="server" OnClick="btnGuardarDoc_Click" />
                            </div>
                        </div>
                        <div id="blk_inf_tripartita" class="hidden" runat="server">
                            <div id="table_list_tripartita" class="col-sm-12 py-2" runat="server"></div>
                        </div>
                        <div id="blk_comentarios" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i> Comentarios</legend>
                                </fieldset>
                            </div>
                            <div class="col-sm-12">
                                <label>Comentarios</label>
                                <div id="comentarios_user_form" class="form-group" runat="server">
                                    <label class="sr-only" for="txt_comentarios_user">Comentarios</label>
								    <textarea class="form-control" id="txt_comentarios_user" rows="3" runat="server"></textarea>
                                </div>
                            </div>                  
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_comentarios" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_save_comentarios" href="javascript: void(0);" onclick="validarFormularioComentario()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardarComentario" CssClass="hidden" runat="server" OnClick="btnGuardarComentario_Click" />
                            </div>
                        </div>
                        <div id="blk_avance" class="hidden" runat="server">
                            <div id="table_list_avance" class="col-sm-12 py-2" runat="server"></div>
                        </div>
                        <div id="blk_unificar_usuarios" class="hidden" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend><i class='fas fa-info-circle'></i>Comentarios</legend>
                                </fieldset>
                            </div>
                            <div id="blk_search_user" runat="server">
                                <div class="col-sm-6">
                                    <label>Usuario correcto</label>
                                    <div class="form-group">
                                        <label class="sr-only" for="txt_user">Usuario correcto</label>
                                        <input type="text" placeholder="Usuario correcto" id="txt_user" class="form-control" disabled="disabled" runat="server" />
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Usuario a eliminar</label>
                                    <div class="form-group">
                                        <label class="sr-only" for="txt_user_old">Usuario a eliminar</label>
                                        <input type="text" placeholder="Usuario a eliminar" id="txt_user_old" class="form-control" runat="server" maxlength="100" />
                                    </div>
                                </div>
                            </div>
                            <div id="blk_data_user" runat="server">
                                <div class="col-sm-6 px-0">
                                    <div class="col-sm-12">
                                        <label>Usuario correcto</label>
                                        <div class="form-group">
                                            <span id="txt_user_correcto" class="text-color-green" runat="server"></span>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <label>Nombre</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txt_nombre">Nombre</label>
                                            <input type="text" placeholder="Nombre" id="txt_nombre" class="form-control" runat="server" maxlength="100" />
                                        </div>
                                    </div>
                                    <div class="col-sm-8">
                                        <label>Apellidos</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txt_apellidos">Apellidos</label>
                                            <input type="text" placeholder="Apellidos" id="txt_apellidos" class="form-control" runat="server" maxlength="200" />
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Mail</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txt_mail">Mail</label>
                                            <input type="text" placeholder="Mail" id="txt_mail" class="form-control" runat="server" maxlength="250" />
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Mail 2</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txt_mail2">Mail 2</label>
                                            <input type="text" placeholder="Mail 2" id="txt_mail2" class="form-control" runat="server" maxlength="250" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Teléfono</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txt_telefono">Teléfono</label>
                                            <input type="text" placeholder="Teléfono" id="txt_telefono" class="form-control" runat="server" maxlength="50" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Teléfono 2</label>
                                        <div class="form-group" runat="server">
                                            <label class="sr-only" for="txt_telefono2">Teléfono 2</label>
                                            <input type="text" placeholder="Teléfono 2" id="txt_telefono2" class="form-control" runat="server" maxlength="50" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Fecha Nacimiento</label>
                                        <div id="birthdate_form" class="input-group date" runat="server">
                                            <label class="sr-only" for="txtFechaNacimiento">Fecha Nacimiento</label>
                                            <input type="text" id="txtFechaNacimiento" class="form-control" runat="server" readonly="readonly" />
                                            <span class="input-group-addon">
                                                <i class="fas fa-th"></i>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 pb-2">
                                        <label class="w-100 pb-2">Sexo</label>
                                        <div class="custom-control custom-radio d-inline">
                                            <input type="radio" class="custom-control-input" id="sex_V" name="sex_method" value="V" runat="server" />
                                            <label class="custom-control-label" for="sex_V">Hombre</label>
                                        </div>
                                        <div class="custom-control custom-radio d-inline">
                                            <input type="radio" class="custom-control-input" id="sex_M" name="sex_method" value="M" runat="server" />
                                            <label class="custom-control-label" for="sex_M">Mujer</label>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>País de nacimiento</label>
                                        <div id="country_birth_form" class="form-group" runat="server">
                                            <label class="sr-only" for="ddlPaisNac">País de nacimiento</label>
                                            <select class="selectpicker w-100" id="ddlPaisNac" title="Seleccione un País de nacimiento" data-live-search="true" data-hide-disabled="true" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Ciudad de nacimiento</label>
                                        <div id="city_birth_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_ciudad_nac">Ciudad de nacimiento</label>
                                            <input type="text" placeholder="Ciudad de nacimiento" id="txt_ciudad_nac" class="form-control" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>País de residencia</label>
                                        <div id="country_form" class="form-group" runat="server">
                                            <label class="sr-only" for="ddlPaisResidencia">País de residencia</label>
                                            <select class="selectpicker w-100" id="ddlPaisResidencia" title="Seleccione un País de residencia" onchange="searchProvinces(this.value, -1)" data-live-search="true" data-hide-disabled="true" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Provincia de residencia</label>
                                        <div id="province_form" class="form-group" runat="server">
                                            <label class="sr-only" for="ddlProvResidencia">Provincia de residencia</label>
                                            <select class="selectpicker w-100" id="ddlProvResidencia" title="Seleccione una Provincia de residencia" data-live-search="true" data-hide-disabled="true" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Ciudad de residencia</label>
                                        <div id="city_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_ciudad_residencia">Ciudad de residencia</label>
                                            <input type="text" placeholder="Ciudad de residencia" id="txt_ciudad_residencia" class="form-control" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>DNI / NIF / Pasaporte</label>
                                        <div id="dni_form" class="form-group" runat="server">
                                            <label class="sr-only" for="dni_user">DNI / NIF / Pasaporte</label>
                                            <input type="text" placeholder="DNI / NIF / Pasaporte" id="dni_user" class="form-control" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-sm-2 pb-2">
                                        <label>Fecha Alta</label>
                                        <div id="fecha_alta_form" class="input-group date" runat="server">
                                            <label class="sr-only" for="txtFechaAlta">Fecha Alta</label>
                                            <input type="text" id="txtFechaAlta" class="form-control" runat="server" readonly="readonly" />
                                            <span class="input-group-addon">
                                                <i class="fas fa-th"></i>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-sm-2 pb-2 pr-0">
                                        <label>Fecha Baja</label>
                                        <div id="fechaBaja_form" class="input-group date col-sm-10 p-0" runat="server">
                                            <label class="sr-only" for="txtFechaBaja">Fecha Baja</label>
                                            <input type="text" id="txtFechaBaja" class="form-control" runat="server" readonly="readonly" />
                                            <span class="input-group-addon">
                                                <i class="fas fa-th"></i>
                                            </span>
                                        </div>
                                        <div class="col-sm-2 text-center pt-2 pl-1">
                                            <a onclick="clean_input('txtFechaBaja')" class="fas fa-times-circle text-color-red" href="javascript:void(0);"></a>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="clearfix"></div>
                                    <div class="col-sm-12 hidden">
                                        <label>Foto</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txt_foto">Foto</label>
                                            <input type="text" placeholder="Foto" id="txt_foto" class="form-control" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label>Nivel de estudios</label>
                                        <div id="level_form" class="form-group" runat="server">
                                            <label class="sr-only" for="ddlNivelEstudios">Nivel de estudios</label>
                                            <select id="ddlNivelEstudios" class="form-control" title="Seleccione tu Nivel de estudios" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label>Experiencia Laboral</label>
                                        <div id="exp_form" class="form-group" runat="server">
                                            <label for="ddlExperiencia" class="sr-only">Experiencia Laboral</label>
                                            <select id="ddlExperiencia" class="form-control" title="Seleccione tu Experiencia laboral" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <label>Situación laboral</label>
                                        <div id="job_form" class="form-group" runat="server">
                                            <label for="ddlSituacion" class="sr-only">Situación laboral</label>
                                            <select id="ddlSituacion" class="form-control" title="Seleccione tu Situación laboral" onchange="searchJob(this.value)" runat="server"></select>
                                        </div>
                                    </div>
                                    <div id="sector_form_all" class="col-sm-3 hidden" runat="server">
                                        <label>Sector profesional</label>
                                        <div id="sector_form" class="form-group" runat="server">
                                            <label class="sr-only" for="sector_user">Sector profesional</label>
                                            <input type="text" placeholder="Sector profesional" id="sector_user" class="form-control" runat="server" />
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-sm-6">
                                        <label>Descripción ficha base</label>
                                        <div id="descripcion_form" class="form-group" runat="server">
                                            <label class="sr-only" for="descripcion_user">Descripción ficha base</label>
                                            <textarea class="form-control" id="descripcion_user" rows="3" runat="server"></textarea>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Comentarios Internos</label>
                                        <div id="comentarios_form" class="form-group" runat="server">
                                            <label class="sr-only" for="comentarios_user">Comentarios</label>
                                            <textarea class="form-control" id="comentarios_user" rows="3" runat="server"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 px-0">
                                    <%-- <div id="block_user">
                        <div class="col-sm-2">
                            <img id="foto_user" src="https://media.spainbs.com/recursos_www/img_alumnos/generic/sin_foto.png" alt="Foto alumno" class="img-fluid rounded-circle" runat="server" />
                            <div class="col-sm-12 text-center">
                                <a id="upload_photo" class='fas fa-edit hidden' href='javascript:void(0);' data-toggle='modal' data-target='#modal_photo' title="Añadir foto" runat="server" />
                            </div>
                            <div class="col-sm-12 text-center">
                                <a id="delete_photo" class="fas fa-trash-alt hidden" href="javascript:void(0);" onclick="delete_foto()" title="Eliminar foto" runat="server" />
                            </div>
                            <span class="col small text-center p-0 pt-1">* Tamaño: 400x400 pixeles</span>
                        </div>
                        <div class="col-sm-3">
                            <label>Nombre</label>
                            <div id="nombre_form" class="form-group" runat="server">
								<label class="sr-only" for="txt_nombre">Nombre</label>
								<input type="text" placeholder="Nombre" id="txt_nombre" class="form-control" runat="server" maxlength="100" />												
							</div>
                        </div>
                        <div class="col-sm-3">
                            <label>Apellidos</label>
                            <div id="apellidos_form" class="form-group" runat="server">
								<label class="sr-only" for="txt_apellidos">Apellidos</label>
								<input type="text" placeholder="Apellidos" id="txt_apellidos" class="form-control" runat="server" maxlength="200" />											
							</div>
                        </div>
                        <div class="col-sm-2">
                            <label class="w-100">Usuario <a id="lnk_mail_activacion" href="javascript:void(0);" onclick="send_mail()" title="Mandar mail activación" class="align-middle" runat="server"><small><i class="fas fa-envelope fa-1-4x"></i></small></a></label>
                            <div class="form-group col-sm-3 px-0 py-2 text-center">
                                <span id="lblStatus" data-toggle='tooltip' data-placement='top' title="" class="bg-green text-white p-1 c-pointer" runat="server">&nbsp;</span>
                            </div>
                            <div class="form-group col-sm-3 px-0 pt-2 text-center">
                                <a id="login_user" data-toggle='tooltip' data-placement='top' data-html='true' class="py-2" href="#" target="_blank" runat="server"><i class="fas fa-user-check fa-1-6x"></i></a>
                                <asp:Button ID="btn_send_mail" CssClass="hidden" runat="server" Text="Mandar mail" OnClick="btn_send_mail_Click" />
                            </div>
                            <div class="form-group col-sm-3 px-0 pt-2 text-center">
                                <a id="campus_user" href="#" title="Ir al campus" target="_blank" runat="server"><i class="fas fa-globe fa-1-6x"></i></a>
                            </div>
                            <div class="form-group col-sm-3 px-0 pt-2 text-center">
                                <a id="lnk_change_password" href="javascript:void(0);" onclick="change_password()" title="Cambiar contraseña" runat="server"><i class="fas fa-user-shield fa-1-6x"></i></a>
                                <asp:Button ID="btn_Password" CssClass="hidden" runat="server" Text="Cambiar contraseña" OnClick="btn_Password_Click" />
                            </div>
                        </div>
                        <div class="col-sm-1 text-center">
                            <label class="pb-2">Cookies</label>
                            <span id="lblCookies" class="border-primary" runat="server">&nbsp;</span>
                        </div>
                        <div class="col-sm-1 text-center">
                            <label class="pb-2">Scoring</label>
                            <span id="lblScoring" class="border-primary" runat="server">&nbsp;</span>
                        </div>
                        <div class="col-sm-3">
                            <label class="w-100">Mail</label>
                            <div id="mail_form" class="form-group col-sm-10 p-0" runat="server">
								<label class="sr-only" for="txt_mail">Mail</label>
								<input type="text" placeholder="Mail" id="txt_mail" class="form-control" runat="server" maxlength="250" />												
							</div>
                            <div class="col-sm-2 text-center pt-1 p-0">
                                <a id="lnk_mail" href="#" target="_blank" runat="server"><i class="fas fa-envelope fa-1-6x"></i></a> 
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label class="w-100">Teléfono</label>
                            <div id="telf_form" class="form-group col-sm-10 p-0" runat="server">
								<label class="sr-only" for="txt_telefono">Teléfono</label>
								<input type="text" placeholder="Teléfono" id="txt_telefono" class="form-control" runat="server" maxlength="50" />												
							</div>
                            <div class="col-sm-2 text-center pt-1 p-0">
                                <a id="ws_user" href="#" rel="noreferrer noopener" target="_blank" runat="server"><i class="fab fa-whatsapp text-color-green fa-1-6x"></i></a> 
                            </div>
                        </div>
                        <div class="col-sm-2">                            
                            <label>DNI / NIF / Pasaporte</label>
                            <div id="dni_form" class="form-group" runat="server">
								<label class="sr-only" for="dni_user">DNI / NIF / Pasaporte</label>
								<input type="text" placeholder="DNI / NIF / Pasaporte" id="dni_user" class="form-control" runat="server" />
							</div>
                        </div>                            
                        <div class="col-sm-2 pb-2">
                            <label class="w-100 pb-2">Sexo</label>
                            <div class="custom-control custom-radio d-inline">
                                <input type="radio" class="custom-control-input" id="sex_V" name="sex_method" value="V" runat="server" />
                                <label class="custom-control-label" for="sex_V">Hombre</label>
                            </div>
                            <div class="custom-control custom-radio d-inline">
                                <input type="radio" class="custom-control-input" id="sex_M" name="sex_method" value="M" runat="server" />
                                <label class="custom-control-label" for="sex_M">Mujer</label>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label class="w-100">Mail 2</label>
                            <div id="mail2_form" class="form-group col-sm-10 p-0" runat="server">
								<label class="sr-only" for="txt_mail2">Mail 2</label>
								<input type="text" placeholder="Mail 2" id="txt_mail2" class="form-control" runat="server" maxlength="250" />												
							</div>
                            <div class="col-sm-2 text-center pt-1 p-0">
                                <a id="lnk_mail2" href="#" target="_blank" runat="server"><i class="fas fa-envelope fa-1-6x"></i></a> 
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label class="w-100">Teléfono 2</label>
                            <div id="telf2_form" class="form-group col-sm-10 p-0" runat="server">
								<label class="sr-only" for="txt_telefono2">Teléfono 2</label>
								<input type="text" placeholder="Teléfono 2" id="txt_telefono2" class="form-control" runat="server" maxlength="50" />												
							</div>
                            <div class="col-sm-2 text-center pt-1 p-0">
                                <a id="ws2_user" href="#" rel="noreferrer noopener" target="_blank" runat="server"><i class="fab fa-whatsapp text-color-green fa-1-6x"></i></a> 
                            </div>
                        </div>
                        <div class="col-sm-2 pb-2">
                            <label>Fecha Alta</label>
						    <div id="fecha_alta_form" class="input-group date" runat="server">
							    <label class="sr-only" for="txtFechaAlta">Fecha Alta</label>
							    <input type="text" id="txtFechaAlta" class="form-control" runat="server" readonly="readonly" />
                                <span class="input-group-addon">
                                    <i class="fas fa-th"></i>
                                </span>
						    </div>
                        </div>
                        <div class="col-sm-2 pb-2 pr-0">
                            <label>Fecha Baja</label>
						    <div id="fechaBaja_form" class="input-group date col-sm-10 p-0" runat="server">
							    <label class="sr-only" for="txtFechaBaja">Fecha Baja</label>
							    <input type="text" id="txtFechaBaja" class="form-control" runat="server" readonly="readonly" />
                                <span class="input-group-addon">
                                    <i class="fas fa-th"></i>
                                </span>
						    </div>
                            <div class="col-sm-2 text-center pt-2 pl-1">
                                <a onclick="clean_input('txtFechaBaja')" class="fas fa-times-circle text-color-red" href="javascript:void(0);"></a>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <label>País de nacimiento</label>											
							<div id="country_birth_form" class="form-group" runat="server">
								<label class="sr-only" for="ddlPaisNac">País de nacimiento</label>
								<select class="selectpicker w-100" id="ddlPaisNac" title="Seleccione un País de nacimiento" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							</div>
                        </div>
                        <div class="col-sm-3">
                            <label>Ciudad de nacimiento</label>
                            <div id="city_birth_form" class="form-group" runat="server">
								<label class="sr-only" for="txt_ciudad_nac">Ciudad de nacimiento</label>
								<input type="text" placeholder="Ciudad de nacimiento" id="txt_ciudad_nac" class="form-control" runat="server" />
							</div>
                        </div>
                        <div class="col-sm-2">
                            <label>Fecha Nacimiento</label>	
                            <div id="birthdate_form" class="input-group date" runat="server">
							    <label class="sr-only" for="txtFechaNacimiento">Fecha Nacimiento</label>
							    <input type="text" id="txtFechaNacimiento" class="form-control" runat="server" readonly="readonly" />
                                <span class="input-group-addon">
                                    <i class="fas fa-th"></i>
                                </span>
						    </div>
                        </div>
                        <div class="col-sm-2">
                            <label class="w-100">GEO IP </label>
                            <div id="txt_geo_ip" class="form-group text-color-primary bold pt-2" runat="server">&nbsp;</div>
                        </div>
                        <div class="col-sm-2">
                            <label class="w-100">Fecha GEO IP </label>
                            <div id="txt_date_geo_ip" class="form-group text-color-primary bold pt-2" runat="server">&nbsp;</div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="col-sm-3">
                            <label>País de residencia</label>											
							<div id="country_form" class="form-group" runat="server">
								<label class="sr-only" for="ddlPaisResidencia">País de residencia</label>
								<select class="selectpicker w-100" id="ddlPaisResidencia" title="Seleccione un País de residencia" onchange="searchProvinces(this.value, -1)" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							</div>
                        </div>
                        <div class="col-sm-3">
                            <label>Provincia de residencia</label>											
							<div id="province_form" class="form-group" runat="server">
								<label class="sr-only" for="ddlProvResidencia">Provincia de residencia</label>
								<select class="selectpicker w-100" id="ddlProvResidencia" title="Seleccione una Provincia de residencia" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							</div>
                        </div>
                        <div class="col-sm-2">                            
                            <label>Ciudad de residencia</label>
                            <div id="city_form" class="form-group" runat="server">
								<label class="sr-only" for="txt_ciudad_residencia">Ciudad de residencia</label>
								<input type="text" placeholder="Ciudad de residencia" id="txt_ciudad_residencia" class="form-control" runat="server" />
							</div>
                        </div>
                        <div class="col-sm-4 pb-1">
                            <div id="blk_links" runat="server"></div>

                            <input type="hidden" id="hid_link" value="" runat="server" />
                            <asp:Button ID="btn_delete_link" CssClass="hidden" runat="server" Text="Eliminar link" OnClick="btn_delete_link_Click" />
                        </div>
                        <div class="clearfix"></div>
                        <div class="col-sm-12 hidden">                            
                            <label>Foto</label>
                            <div class="form-group">
								<label class="sr-only" for="txt_foto">Foto</label>
								<input type="text" placeholder="Foto" id="txt_foto" class="form-control" runat="server" />
							</div>
                        </div>                     
                        <div class="col-sm-3">
                            <label>Nivel de estudios</label>
                            <div id="level_form" class="form-group" runat="server">
								<label class="sr-only" for="ddlNivelEstudios">Nivel de estudios</label>
								<select id="ddlNivelEstudios" class="form-control" title="Seleccione tu Nivel de estudios" runat="server"></select>
							</div>
                        </div>
                        <div class="col-sm-3">
                            <label>Experiencia Laboral</label>
                            <div id="exp_form" class="form-group" runat="server">
								<label for="ddlExperiencia" class="sr-only">Experiencia Laboral</label>
                                <select id="ddlExperiencia" class="form-control" title="Seleccione tu Experiencia laboral"  runat="server"></select>
                            </div> 
                        </div>
                        <div class="col-sm-3">
                            <label>Situación laboral</label>
                            <div id="job_form" class="form-group" runat="server">
								<label for="ddlSituacion" class="sr-only">Situación laboral</label>
                                <select id="ddlSituacion" class="form-control" title="Seleccione tu Situación laboral" onchange="searchJob(this.value)" runat="server"></select>	
                            </div> 
                        </div>
                        <div id="sector_form_all" class="col-sm-3 hidden" runat="server">
                            <label>Sector profesional</label>											
							<div id="sector_form" class="form-group" runat="server">
								<label class="sr-only" for="sector_user">Sector profesional</label>
								<input type="text" placeholder="Sector profesional" id="sector_user" class="form-control" runat="server" />
							</div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="col-sm-6">
                            <label>Descripción ficha base</label>
                            <div id="descripcion_form" class="form-group" runat="server">
                                <label class="sr-only" for="descripcion_user">Descripción ficha base</label>
								<textarea class="form-control" id="descripcion_user" rows="3" runat="server"></textarea>
                            </div>
                        </div>                        
                        <div class="col-sm-6">
                            <label>Comentarios Internos</label>
                            <div id="comentarios_form" class="form-group" runat="server">
                                <label class="sr-only" for="comentarios_user">Comentarios</label>
								<textarea class="form-control" id="comentarios_user" rows="3" runat="server"></textarea>
                            </div>
                        </div>
                        <div id="blk_origins" class="col-sm-8 pb-1" runat="server"></div>
                        <div id="blk_tags" class="col-sm-12 pb-2" runat="server"></div>
                        <div id="blk_comentarios" class="col-sm-12 pb-2 px-0" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend id="txt_title_comentarios" class="text-color-primary fa-1x" runat="server" />
                                </fieldset>
                            </div>
                            <div id="table_comentarios" class="col-sm-12 py-2" runat="server"></div>

                            <input type="hidden" id="hid_comentario" value="" runat="server" />
                            <asp:Button ID="btn_delete_comentario" CssClass="hidden" runat="server" Text="Eliminar comentario" OnClick="btn_delete_comentario_Click" />
                        </div>
                        <div class="col-sm-12 hidden">
                            <input type="hidden" id="hid_tag" value="" runat="server" />
                            <asp:Button ID="btn_delete_tag" CssClass="hidden" runat="server" Text="Eliminar tag" OnClick="btn_delete_tag_Click" />
                        </div>
                        <div class="col-sm-12 pt-3">
                            <a id="lnk_baja" class='btn btn-primary bg-red text-color-white btn-block-xs pull-left' href='javascript:void(0);' data-toggle='modal' data-target='#modal_baja' title="Dar de baja" runat="server">Dar de baja</a>
                            

                            <asp:Button ID="btn_baja" CssClass="hidden" runat="server" Text="Dar de baja" OnClick="btn_baja_Click" />
                            <asp:Button ID="btn_guardar" CssClass="btn btn-primary bg-green text-color-white btn-block-xs pull-right" runat="server"
                                Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btn_guardar_Click" />
                        </div>
                    </div> --%>
                                </div>
                            </div>
                            <div class="col-sm-12 pt-4">
                                <asp:Button ID="btn_back_unificar" CssClass="btn btn-primary btn-block-xs pull-left margin-xs-b-15" Text="Volver" runat="server" OnClick="btn_back_Click" />
                                <a id="btn_search" href="javascript: void(0);" onclick="validarUsuario()" class="btn btn-primary bg-green text-color-white btn-block-xs pull-right" runat="server">Buscar</a>
                                <asp:Button ID="btnUnificarUsuario" CssClass="btn btn-primary bg-green text-color-white btn-block-xs pull-right" Text="Guardar" runat="server" OnClick="btnUnificarUsuario_Click" />
                            </div>
                        </div>
                    </div>
                </form>

                <div class="modal fade" id="modal_documento" tabindex="-1" role="dialog" aria-labelledby="modal_documento_title" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="modal_documento_title">Documento</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="row fileupload-buttonbar">
                                    <div class="col-sm-12">
                                        <span id="file_documento" class="btn fileinput-button" runat="server"></span>
                                        <div id='progress_documento' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>                                        
                                    </div>
                                </div>
                                <table id="tbl_documento" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
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

    <!-- Scripts
    =================================================== --> 
    <asp:PlaceHolder runat="server">        
        <%: Scripts.Render("~/bundles/general_admin_js") %>
        <%: Scripts.Render("~/bundles/jquery_ui_js") %>
        <%: Scripts.Render("~/bundles/menu_nav_js") %>
        <%: Scripts.Render("~/bundles/datepicker_js") %>
        <%: Scripts.Render("~/bundles/datatables_js") %>
        <%: Scripts.Render("~/bundles/upload_js") %>
        <%: Scripts.Render("~/bundles/ficha_usuario_aux_js") %>
    </asp:PlaceHolder>
</body>
</html>
