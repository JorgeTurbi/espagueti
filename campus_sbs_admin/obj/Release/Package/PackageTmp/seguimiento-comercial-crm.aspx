<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="seguimiento-comercial-crm.aspx.cs" Inherits="campus_sbs_admin.seguimiento_comercial_crm" ValidateRequest="false" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header-crm.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Seguimiento comercial</title>

    <!-- CSS 
    =================================================== -->	    
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/datepicker_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>
        <%: Styles.Render("~/bundles/datatables_css") %>
    </asp:PlaceHolder>

    <!-- CKeditor -->
    <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>

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
                        <div class="col">
                            <fieldset>
                                <legend><i class='fas fa-info-circle'></i> Seguimiento comercial</legend>
                            </fieldset>
                        </div>
                        <div id="block_error" class="form-group has-error" runat="server">
                            <span id="txt_error" class="help-block text-center" runat="server"></span>
                        </div>
                        <div>
                            <div class="col-sm-6">
                                <label class="w-100">Usuario</label>
                                <div id="txt_user" class="form-group text-color-black bold pt-2" runat="server">&nbsp;</div>
                            </div>
                            <div class="col-sm-6">
                                <label>Curso</label>											
							    <div class="form-group">
								    <label class="sr-only" for="ddlCurso">Curso</label>
								    <select class="selectpicker w-100" id="ddlCurso" title="Seleccione un curso" data-live-search="true" data-hide-disabled="true" disabled="disabled" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Origen</label>											
							    <div class="form-group">
								    <label class="sr-only" for="ddlOrigen">Origen</label>
								    <select class="selectpicker w-100" id="ddlOrigen" title="Seleccione un origen" data-live-search="true" data-hide-disabled="true" disabled="disabled" runat="server"></select>
							    </div>
                            </div>
                            <div class="col-sm-3">
                                <label>Fecha Seguimiento</label>
							    <div id="fecha_seg_form" class="input-group date" runat="server">
								    <label class="sr-only" for="txtFechaSeg">Fecha Seguimiento</label>
								    <input type="text" id="txtFechaSeg" class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="fas fa-th"></i>
                                    </span>
							    </div>
                            </div>
                            <div id="blk_accion_comercial" class="col-sm-3" runat="server">
                                <label class="w-100 pb-2">Acción Comercial</label>
                                <div class="custom-control custom-radio d-inline pr-2">
                                    <input type="radio" class="custom-control-input" id="action_mail" name="action_method" value="1" runat="server" />
                                    <label class="custom-control-label" for="action_mail"><i class='fas fa-envelope fa-1-6x'></i></label>
                                </div>
                                <div class="custom-control custom-radio d-inline pr-2">
                                    <input type="radio" class="custom-control-input" id="action_phone" name="action_method" value="2" runat="server" />
                                    <label class="custom-control-label" for="action_phone"><i class='fas fa-phone-square fa-1-6x'></i></label>
                                </div>
                                <div class="custom-control custom-radio d-inline pr-2">
                                    <input type="radio" class="custom-control-input" id="action_whatsapp" name="action_method" value="3" runat="server" />
                                    <label class="custom-control-label" for="action_whatsapp"><i class='fab fa-whatsapp fa-1-6x text-color-green'></i></label>
                                </div>
                                <div class="custom-control custom-radio d-inline pr-2">
                                    <input type="radio" class="custom-control-input" id="action_advise" name="action_method" value="4" runat="server" />
                                    <label class="custom-control-label" for="action_advise"><i class='fas fa-bell fa-1-6x text-color-secondary'></i></label>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-sm-2">
                                <label class="w-100 pb-2">Metodologia</label>
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="method_online" name="metodologia_method" value="Online" runat="server" />
                                    <label class="custom-control-label" for="method_online">Online</label>
                                </div>
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="method_semi" name="metodologia_method" value="Semipresencial" runat="server" />
                                    <label class="custom-control-label" for="method_semi">Semipresencial</label>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <label class="w-100 pb-2">Cuándo</label>
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="cuando_menor_7" name="cuando_method" value="De 0 a 6 meses" runat="server" />
                                    <label class="custom-control-label" for="cuando_menor_7">De 0 a 6 meses</label>
                                </div>
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="cuando_menor_12" name="cuando_method" value="De 7 a 12 meses" runat="server" />
                                    <label class="custom-control-label" for="cuando_menor_12">De 7 a 12 meses</label>
                                </div>
                                <div class="custom-control custom-radio">
                                    <input type="radio" class="custom-control-input" id="cuando_mayor_12" name="cuando_method" value="+ 12 meses" runat="server" />
                                    <label class="custom-control-label" for="cuando_mayor_12">+ 12 meses</label>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Comentarios Resumen</label>
                                <div class="form-group">
                                    <label class="sr-only" for="resumen_comentarios_user">Comentarios Resumen</label>
								    <textarea class="form-control" id="resumen_comentarios_user" rows="3" runat="server"></textarea>
                                </div>
                            </div>  
                            <div class="col-sm-2">                            
                                <label>Beca</label>
                                <div class="form-group mb-0">
								    <label class="sr-only" for="beca_user">Beca</label>
								    <input type="text" id="beca_user" class="form-control" readonly="readonly" runat="server" />
							    </div>
                            </div>  
                            <div class="col-sm-2">                            
                                <label>Descuento</label>
                                <div class="form-group">
								    <label class="sr-only" for="descuento_user">Descuento</label>
								    <input type="text" id="descuento_user" class="form-control" readonly="readonly" runat="server" />
							    </div>
                            </div>                              
                            <div id="blk_estado" class="col-sm-12 px-0" runat="server">
                                <label class="col-sm-12 w-100 pb-2">Estado</label>
                                <div class="col-sm-3">
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_sin_contactar" name="status_method" value="5" runat="server" />
                                        <label class="custom-control-label" for="status_sin_contactar">Sin Contactar</label>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_indeciso" name="status_method" value="6" runat="server" />
                                        <label class="custom-control-label" for="status_indeciso">Indeciso</label>
                                    </div>
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_interesado" name="status_method" value="7" runat="server" />
                                        <label class="custom-control-label" for="status_interesado">Interesado</label>
                                    </div>
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_send" name="status_method" value="8" runat="server" />
                                        <label class="custom-control-label" for="status_send">Enviar Contrato</label>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_receive" name="status_method" value="9" runat="server" />
                                        <label class="custom-control-label" for="status_receive">Recibir Contrato</label>
                                    </div>
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_pago" name="status_method" value="10" runat="server" />
                                        <label class="custom-control-label" for="status_pago">Pago</label>
                                    </div>
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_futuro" name="status_method" value="3" runat="server" />
                                        <label class="custom-control-label" for="status_futuro">Futuro</label>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_duplicado" name="status_method" value="2" runat="server" />
                                        <label class="custom-control-label" for="status_duplicado">Duplicado</label>
                                    </div>
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_rechazado" name="status_method" value="1" runat="server" />
                                        <label class="custom-control-label" for="status_rechazado">Rechazado</label>
                                    </div>
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_cerrar" name="status_method" value="4" runat="server" />
                                        <label class="custom-control-label" for="status_cerrar">Cerrar</label>
                                    </div>
                                    <div class="custom-control custom-radio">
                                        <input type="radio" class="custom-control-input" id="status_matriculado" name="status_method" value="11" runat="server" />
                                        <label class="custom-control-label" for="status_matriculado">Matriculado</label>
                                    </div>
                                </div>
                            </div>
                            <div id="blk_prog_rec" class="col-sm-6 px-0" runat="server">
				                <label class="col-sm-12 w-100 pb-2">Programar recordatorio</label>
                                <div class="col-sm-3 pt-2">
                                    <div class="custom-control custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkProgRec" runat="server" />
                                        <label class="custom-control-label" for="chkProgRec">Programar</label>
                                    </div>
                                </div>
                                <div id="blk_program" class="col-sm-9 px-0 hidden" runat="server">
                                    <div class="col-sm-7">
                                        <div class="input-group date">
								            <label class="sr-only" for="txtDateProg">Fecha recordatorio</label>
								            <input type="text" id="txtDateProg" class="form-control" runat="server" readonly="readonly" />
                                            <span class="input-group-addon">
                                                <i class="fas fa-th"></i>
                                            </span>
							            </div>
                                    </div>
                                    <div class="col-sm-5">
                                        <div id="hora_recordatorio" class="form-group">
								            <label class="sr-only" for="ddlHour">Hora</label>
								            <select class="selectpicker w-100" id="ddlHour" title="Hora" data-live-search="true" data-hide-disabled="true" runat="server">
                                                <option value="08:00">08:00</option>
									            <option value="08:30">08:30</option>
									            <option value="09:00">09:00</option>
									            <option value="09:30">09:30</option>
									            <option value="10:00">10:00</option>
									            <option value="10:30">10:30</option>
									            <option value="11:00">11:00</option>
									            <option value="11:30">11:30</option>
									            <option value="12:00">12:00</option>
									            <option value="12:30">12:30</option>
									            <option value="13:00">13:00</option>
									            <option value="13:30">13:30</option>
									            <option value="14:00">14:00</option>
									            <option value="14:30">14:30</option>
									            <option value="15:00">15:00</option>
									            <option value="15:30">15:30</option>
									            <option value="16:00">16:00</option>
									            <option value="16:30">16:30</option>
									            <option value="17:00">17:00</option>
									            <option value="17:30">17:30</option>
									            <option value="18:00">18:00</option>
									            <option value="18:30">18:30</option>
									            <option value="19:00">19:00</option>
									            <option value="19:30">19:30</option>
									            <option value="20:00">20:00</option>
									            <option value="20:30">20:30</option>
									            <option value="21:00">21:00</option>
								            </select>
							            </div>
                                    </div>
                                </div>
				            </div>
                            <div id="blk_change_user" class="col-sm-6 px-0" runat="server">
                                <label class="col-sm-12 w-100 pb-2">Reasignar comercial</label>
				                <div class="col-sm-5 pt-2">
                                    <div class="custom-control custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkReasignarC" runat="server" />
                                        <label class="custom-control-label" for="chkReasignarC">Reasignar comercial</label>
                                    </div>
			                    </div>	
			                    <div id="blk_comercial" class="col-sm-7 hidden">
                                    <div class="form-group">
								        <label class="sr-only" for="ddlComerciales">Origen</label>
								        <select class="selectpicker w-100" id="ddlComerciales" title="Seleccione un comercial para reasignar" data-live-search="true" data-hide-disabled="true" runat="server"></select>
							        </div>
                                </div>
                            </div>
                            <div id="blk_mail" class="col-sm-12 pt-2 px-0 hidden" runat="server">
                                <div class="col-sm-3">
                                    <label>Enviar mail</label>
                                    <div id="send_mail_form" class="custom-control custom-checkbox text-color-red pt-2" runat="server">
                                        <input type="checkbox" class="custom-control-input" id="chkEnviarMail" runat="server" />
                                        <label class="custom-control-label" for="chkEnviarMail">Enviar Mail</label>
						            </div>
                                </div>  
                                <div class="col-sm-9">
                                    <label>Plantilla mail</label>											
							        <div class="form-group">
								        <label class="sr-only" for="ddlPlantilla">Plantilla mail</label>
								        <select class="selectpicker w-100" id="ddlPlantilla" title="Seleccione una plantilla" data-live-search="true" data-hide-disabled="true" runat="server"></select>
                                        <asp:HiddenField ID="mail_id" runat="server" />
							        </div>
                                </div>                                
                                <div class="col-sm-12">
                                    <label>Asunto</label>
                                    <div id="asunto_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_asunto">Asunto</label>
									    <input type="text" placeholder="Asunto" id="txt_asunto" class="form-control" runat="server" maxlength="250" />							
								    </div>
                                </div>                        
                                <div class="col-sm-12 ">
                                    <label>Cuerpo</label>											
								    <div id='cuerpo_form' class="form-group">
							            <label class="sr-only" for="txt_cuerpo">Cuerpo</label>
                                        <textarea id="txt_cuerpo" placeholder="Cuerpo" class="form-control" cols='2' rows='5' runat="server"></textarea>
                                        <script type="text/javascript">
                                            CKEDITOR.replace('txt_cuerpo',
                                            {
                                                placeholder: 'Cuerpo'
	                                        });
                                        </script>                            
						            </div>   
                                </div>
                                <div id="blk_adjuntos" class="col-sm-12 no-padding" runat="server">       
                                    <label class="col-sm-12 padding-t-20">Adjuntos  <a id="lbl_adjuntos" class="fas fa-file-upload fa-1-4x pull-right text-color-primary" href="javascript:void(0);" data-toggle="modal" data-target="#modal_adjunto" title="Añadir Adjunto" runat="server"> Añadir adjunto</a></label>
                                    <div class="padding-t-20">
                                        <div id="block_adjuntos_lst" class="col-sm-12" runat="server"></div>
                                        <div class="col-sm-12 padding-t-20">
                                            <table id="tbl_adjuntos" role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
                                            <input type="hidden" id="hidAdjuntos" value="" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>  
                            <div id="blk_rejected" class="col-sm-12 pt-2 hidden" runat="server">
                                <label class="w-100 pb-2">Indique el motivo de rechazo</label>
                                <div class="custom-control custom-radio col-sm-3">
                                    <input type="radio" class="custom-control-input" id="motive1" name="motive_method" value="1" runat="server" />
                                    <label class="custom-control-label" for="motive1">Usuario falso (mail/telf no pertenece a ese lead)</label>
                                </div>
                                <div class="custom-control custom-radio col-sm-3">
                                    <input type="radio" class="custom-control-input" id="motive2" name="motive_method" value="2" runat="server" />
                                    <label class="custom-control-label" for="motive2">Usuario no contactable</label>
                                </div>
                                <div class="custom-control custom-radio col-sm-3">
                                    <input type="radio" class="custom-control-input" id="motive3" name="motive_method" value="3" runat="server" />
                                    <label class="custom-control-label" for="motive3">Usuario no permite explicar el producto</label>
                                </div>
                                <div class="custom-control custom-radio col-sm-3">
                                    <input type="radio" class="custom-control-input" id="motive4" name="motive_method" value="4" runat="server" />
                                    <label class="custom-control-label" for="motive4">Usuario no pide info de este máster ni parecido</label>
                                </div>
                            </div>
                            <div class="col-sm-12 pt-2">
                                <label>Comentarios del seguimiento comercial</label>
                                <div class="form-group">
                                    <label class="sr-only" for="txt_comentarios">Comentarios</label>
								    <textarea class="form-control" id="txt_comentarios" rows="3" runat="server"></textarea>
                                </div>
                            </div>
                            <div class="col-sm-12 py-4">
                                <a id="btn_back" href="listado-cursos.aspx" class="btn btn-primary btn-block-xs pull-left margin-xs-b-15" runat="server">Volver</a>
                                <a id="btn_save" href="javascript: void(0);" onclick="validarFormulario()" class="btn btn-primary bg-green btn-block-xs pull-right">Guardar</a>
                                
                                <asp:Button ID="btnGuardar" CssClass="hidden" runat="server" OnClick="btnGuardar_Click" />
                                <asp:Button ID="btn_reenviar" CssClass="btn btn-warning text-white btn-block-xs pull-right mx-4" Visible="false" Text="Reenviar" runat="server" OnClick="btn_reenviar_Click" />
                            </div>
                        </div>
                    </div>
                </form>

                <div class="modal fade" id="modal_adjunto" tabindex="-1" role="dialog" aria-labelledby="modal_adjunto_title" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-large modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="modal_adjunto_title">Adjunto</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="row fileupload-buttonbar">
                                    <div class="col-sm-12">
                                        <span id="file_adjunto" class="btn fileinput-button" runat="server"></span>
                                        <div id='progress_foto' class='progress progress-striped active hidden' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'><div class='progress-bar progress-bar-success' style='width:0%;'></div></div>
                                    </div>
                                </div>
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
        <%: Scripts.Render("~/bundles/seguimiento_comercial_js") %>
    </asp:PlaceHolder>
</body>
</html>
