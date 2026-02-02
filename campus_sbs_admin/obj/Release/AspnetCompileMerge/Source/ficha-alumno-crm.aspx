<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ficha-alumno-crm.aspx.cs" Inherits="campus_sbs_admin.ficha_alumno_crm" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header-crm.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Ficha alumno</title>

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
            <%--<div class="col pt-2">
                <fieldset>
                    <legend class="text-color-primary" runat="server"><i class="fas fa-user-edit"></i> Datos del usuario</legend>                    
                </fieldset>
            </div>--%>
            <div class="col pt-1 pb-3">
                <form id="form1" runat="server">
                    <div id="block_error" class="form-group has-error" runat="server">
                        <span id="txt_error" class="help-block text-center" runat="server"></span>
                    </div>
                    <div class="col">
                        <fieldset>
                            <legend id="txt_title" class="text-color-primary px-2" runat="server" />                    
                        </fieldset>
                    </div>
                    <div id="block_user">
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
                            <%--<div class="form-group col-sm-3 px-0 py-2 text-center">
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
                            </div>--%>
                            <div class="form-group pt-2 pb-1 text-center">
                                <span id="lblStatus" data-toggle='tooltip' data-placement='top' title="" class="bg-green text-white px-1 c-pointer" runat="server">&nbsp;</span>
                                <a id="login_user" data-toggle='tooltip' data-placement='top' data-html='true' class="pt-1 px-1" href="#" target="_blank" runat="server"><i class="fas fa-user-check fa-1-4x"></i></a>
                                <a id="campus_user" href="#" data-toggle='tooltip' data-placement='top' data-html='true' title="Ir al campus" target="_blank" class="pt-1" runat="server"><i class="fas fa-globe fa-1-4x"></i></a>
                                <a id="lnk_change_password" href="javascript:void(0);" data-toggle='tooltip' data-placement='top' data-html='true' class="pt-1 px-1" onclick="change_password()" title="Cambiar contraseña" runat="server"><i class="fas fa-user-shield fa-1-4x"></i></a>
                                <a id="campus_pro" href="#" data-toggle='tooltip' data-placement='top' data-html='true' title="Ir al campus profesor" class="pt-1" target="_blank" runat="server"><i class="fas fa-globe text-warning fa-1-4x"></i></a>
                                <asp:Button ID="btn_Password" CssClass="hidden" runat="server" Text="Cambiar contraseña" OnClick="btn_Password_Click" />
                                <asp:Button ID="btn_send_mail" CssClass="hidden" runat="server" Text="Mandar mail" OnClick="btn_send_mail_Click" />
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
                            <label>Identificación</label>
                            <div id="dni_form" class="form-group" runat="server">
								<label class="sr-only" for="dni_user">Identificación</label>
								<input type="text" placeholder="Identificación" id="dni_user" class="form-control" runat="server" />
							</div>
                        </div>                            
                        <div class="col-sm-2">                            
                            <label>Pasaporte</label>
                            <div id="pasaporte_form" class="form-group" runat="server">
								<label class="sr-only" for="passport_user">Pasaporte</label>
								<input type="text" placeholder="Pasaporte" id="passport_user" class="form-control" runat="server" />
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
                        <div class="clearfix"></div>
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
                        <div class="col-sm-3">
                            <label>Fecha Nacimiento</label>	
                            <div id="birthdate_form" class="input-group date" runat="server">
							    <label class="sr-only" for="txtFechaNacimiento">Fecha Nacimiento</label>
							    <input type="text" id="txtFechaNacimiento" class="form-control" runat="server" readonly="readonly" />
                                <span class="input-group-addon">
                                    <i class="fas fa-th"></i>
                                </span>
						    </div>
                        </div>
                        <div class="col-sm-3 pb-2">
                            <label class="w-100 pb-2">Sexo</label>
                            <div class="custom-control custom-radio d-inline mr-2">
                                <input type="radio" class="custom-control-input" id="sex_V" name="sex_method" value="V" runat="server" />
                                <label class="custom-control-label" for="sex_V">Hombre</label>
                            </div>
                            <div class="custom-control custom-radio d-inline">
                                <input type="radio" class="custom-control-input" id="sex_M" name="sex_method" value="M" runat="server" />
                                <label class="custom-control-label" for="sex_M">Mujer</label>
                            </div>
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
                        <div class="col-sm-2">
                            <label class="w-100">GEO IP </label>
                            <div id="txt_geo_ip" class="form-group text-color-primary bold pt-2" runat="server">&nbsp;</div>
                        </div>
                        <div class="col-sm-2">
                            <label class="w-100">Fecha GEO IP </label>
                            <div id="txt_date_geo_ip" class="form-group text-color-primary bold pt-2" runat="server">&nbsp;</div>
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
                        <div class="col-sm-6 pb-1">
                            <div id="blk_links" runat="server"></div>

                            <input type="hidden" id="hid_link" value="" runat="server" />
                            <asp:Button ID="btn_delete_link" CssClass="hidden" runat="server" Text="Eliminar link" OnClick="btn_delete_link_Click" />
                        </div>
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
                        <div class="col-sm-6">
                            <label>Comentario Publico Alumno</label>
                            <div id="comentario_publico_form" class="form-group" runat="server">
                                <label class="sr-only" for="comentario_publico">Comentarios</label>
								<textarea class="form-control" id="comentario_publico" rows="3" runat="server"></textarea>
                            </div>
                        </div>
                        <div class="col-sm-12 py-2">
                            <a id="lnk_baja" class='btn btn-primary bg-red text-color-white btn-block-xs pull-left' href='javascript:void(0);' data-toggle='modal' data-target='#modal_baja' title="Dar de baja" runat="server">Dar de baja</a>
                            
                            <asp:Button ID="btn_baja" CssClass="hidden" runat="server" Text="Dar de baja" OnClick="btn_baja_Click" />
                            <asp:Button ID="btn_guardar" CssClass="btn btn-primary bg-green text-color-white btn-block-xs pull-right" runat="server"
                                Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btn_guardar_Click" />
                        </div>
                        <div id="blk_origins" class="col-sm-12 pb-1" runat="server"></div>
                        <div id="blk_tags" class="col-sm-12 pb-2" runat="server"></div>
                        <div id="blk_admin" class="col-sm-12 pb-2 px-0" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend id="txt_administracion" class="text-color-primary fa-1x" runat="server"><i class="fas fa-user-cog"></i> Datos del alumno</legend>                    
                                </fieldset>
                            </div>
                            <div class="col">
                                <div class="form-group padding-t-10 d-inline-block" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" /> 
							        </div>
						        </div>
                                <div class="form-group padding-t-10 d-inline-block px-4" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkProfesor" runat="server" Text="Profesor" /> 
							        </div>
						        </div>
                                <div class="form-group padding-t-10 d-inline-block" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkComercial" runat="server" Text="Comercial" /> 
							        </div>
						        </div>
                                <div class="form-group padding-t-10 d-inline-block px-4" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkAdministrador" runat="server" Text="Administrador" /> 
							        </div>
						        </div>
                                <div class="form-group padding-t-10 d-inline-block" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkNoEncuesta" runat="server" Text="No Encuesta" /> 
							        </div>
						        </div>
                                <div class="form-group padding-t-10 d-inline-block px-4" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkOnline" runat="server" Text="Online" /> 
							        </div>
						        </div>
                                <div class="form-group padding-t-10 d-inline-block px-4" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkPracticas" runat="server" Text="Prácticas" /> 
							        </div>
						        </div>
                                <div class="form-group padding-t-10 d-inline-block" runat="server">													
							        <div class="checkbox">
                                        <asp:CheckBox ID="chkBajaImpago" runat="server" Text="Baja por impago" /> 
							        </div>
						        </div>
                                <asp:Button ID="btn_save_admin" CssClass="btn btn-primary bg-green text-color-white btn-block-xs pull-right" runat="server"
                                    Text="Guardar" OnClick="btn_save_admin_Click"  />
                            </div>
                        </div>
                        <div id="blk_comentarios" class="col-sm-12 pb-2 px-0" runat="server">
                            <div class="col">
                                <fieldset>
                                    <legend id="txt_title_comentarios" class="bg-primary-soft-soft text-color-primary px-2" runat="server" />
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
                    </div>
                                        
                    <div id="blk_pet_inf" runat="server">
                        <div class="col pt-2">
                            <fieldset>
                                <legend id="txt_peticion_info" class="bg-primary-soft-soft text-color-primary px-2" runat="server" />
                            </fieldset>
                        </div>
                        <div id="block_pet_inf" runat="server">
                            <div id="table_pet_info" class="col-sm-12 py-2" runat="server"></div>

                            <input type="hidden" id="hid_peticion_info" value="" runat="server" />
                            <input type="hidden" id="hid_seguimiento" value="" runat="server" />
                            <asp:Button ID="btn_delete_pet_info" CssClass="hidden" runat="server" Text="Eliminar petición" OnClick="btn_delete_pet_info_Click" />
                            <asp:Button ID="btn_delete_seguimiento" CssClass="hidden" runat="server" Text="Eliminar seguimiento" OnClick="btn_delete_seguimiento_Click" />
                            <asp:Button ID="btn_procesar_peticion" CssClass="hidden" runat="server" Text="Procesar petición" OnClick="btn_procesar_peticion_Click" />
                        </div>
                    </div>

                    <div id="blk_cursos" runat="server">
                        <div class="col pt-2">
                            <fieldset>
                                <legend id="txt_cursos"  class="bg-primary-soft-soft text-color-primary px-2" runat="server"><i class="fas fa-university"></i> Programas realizados en la escuela <a id="inf_acceso" href='#' title='Informe Acceso' class='pull-right' runat="server"><small class='text-color-primary'><i class='fas fa-user-clock'></i>  Inf. Acceso</small></a><span id="last_access" title="Fecha último acceso" class="pull-right text-color-secondary border-primary small mr-2" runat="server">&nbsp;</span></legend>                    
                            </fieldset>
                        </div>
                        <div id="block_cursos">
                            <div id="table_list_cursos" class="col-sm-12 py-2" runat="server"></div>

                            <input type="hidden" id="hid_idDocencia" value="" runat="server" />
                            <input type="hidden" id="hid_idCurso" value="" runat="server" />
                            <input type="hidden" id="hid_idUsuario" value="" runat="server" />
                            <asp:Button ID="btn_solicitar_diploma" CssClass="hidden" runat="server" Text="Solicitar Diploma" OnClick="btn_solicitar_diploma_Click" />
                        </div>
                    </div>
                    
                    <div id="blk_ventas" runat="server">
                        <div class="col pt-2">
                            <fieldset>
                                <legend id="txt_ventas" class="bg-primary-soft-soft text-color-primary px-2" runat="server"><i class="fas fa-euro-sign"></i> Ventas</legend>                    
                            </fieldset>
                        </div>
                        <div id="block_ventas">
                            <div id="table_list_ventas" class="col-sm-12 py-2" runat="server"></div>

                            <input type="hidden" id="hid_curso" value="" runat="server" />
                            <input type="hidden" id="hid_docencia" value="" runat="server" />
                            <input type="hidden" id="hid_pago" value="" runat="server" />
                            <asp:Button ID="btn_delete_asig_comercial" CssClass="hidden" runat="server" Text="Eliminar asignación comercial" OnClick="btn_delete_asig_comercial_Click" />
                            <asp:Button ID="btn_delete_pago" CssClass="hidden" runat="server" Text="Eliminar pago" OnClick="btn_delete_pago_Click" />
                            <asp:Button ID="btn_send_pago" CssClass="hidden" runat="server" Text="Mandar mail" OnClick="btn_send_pago_Click" />
                        </div>
                    </div>

                    <div id="blk_documentos" runat="server">
                        <div class="col pt-2">
                            <fieldset>
                                <legend id="txt_documentacion" class="bg-primary-soft-soft text-color-primary px-2" runat="server" />
                            </fieldset>
                        </div>
                        <div id="block_documentos" runat="server">
                            <div id="table_documentos" class="col-sm-12 py-2" runat="server"></div>

                            <input type="hidden" id="hid_documento" value="" runat="server" />
                            <asp:Button ID="btn_delete_documento" CssClass="hidden" runat="server" Text="Eliminar documento" OnClick="btn_delete_documento_Click" />
                        </div>
                    </div>

                    <div id="blk_acciones" runat="server">
                        <div class="col pt-2">
                            <fieldset>
                                <legend id="txt_acciones" class="bg-primary-soft-soft text-color-primary px-2" runat="server"><i class="fas fa-tasks"></i> Acciones del usuario</legend>                    
                            </fieldset>
                        </div>
                        <div id="block_acciones">
                            <div id="table_list_acciones" class="col-sm-12 py-2" runat="server"></div>
                        </div>
                    </div>

                    <div id="blk_empleos" runat="server">
                        <div class="col pt-2">
                            <fieldset>
                                <legend id="txt_title_empleos" class="bg-primary-soft-soft text-color-primary px-2" runat="server"></legend>                    
                            </fieldset>
                        </div>
                        <div id="table_empleos" class="col-sm-12 py-2" runat="server"></div>
                    </div>

                    <div id="blk_examenes" runat="server">
                        <div class="col pt-2">
                            <fieldset>
                                <legend id="txt_title_examenes" class="bg-primary-soft-soft text-color-primary px-2" runat="server"></legend>                    
                            </fieldset>
                        </div>
                        <div id="table_examenes" class="col-sm-12 py-2" runat="server"></div>
                    </div>

                    <div class="modal fade" id="modal_baja" tabindex="-1" role="dialog" aria-labelledby="modal_baja_title" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_baja_title">Dar de baja</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">                    
                                    <div class="mb-3">
                                        Al pulsar en Guardar, se procederá a dar de baja al usuario. Para que se realice la acción debe de introducir el motivo por el cual se realiza la baja.
                                    </div>
                                    <div class="mb-3">
                                        <label for="motivo_baja" class="col-form-label">Motivo baja</label>
                                        <input type="text" class="form-control" id="motivo_baja" runat="server" />
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-primary col-3" data-dismiss="modal">Cerrar</button>
                                    <div class="col-3"></div>
                                    <a href="javascript:void(0)" onclick="baja_user()" class="btn btn-primary bg-green text-color-white col-3">Guardar</a>
                                    <div class="col-1"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="modal_mail" tabindex="-1" aria-labelledby="modal_mail_title" aria-hidden="true">
                        <div class="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="modal_mail_title">Contenido del mail</h5>
                                    <div id="modal_adjuntos_mail" class="m-auto"></div>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">×</span>
                                    </button>
                                </div>
                                <div id="modal_body_mail" class="modal-body"></div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </section>

    <!-- Modal -->
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

    <!-- Scripts
    =================================================== -->     
    <asp:PlaceHolder runat="server">        
        <%: Scripts.Render("~/bundles/general_admin_js") %>
        <%: Scripts.Render("~/bundles/jquery_ui_js") %>
        <%: Scripts.Render("~/bundles/menu_nav_js") %>
        <%: Scripts.Render("~/bundles/datepicker_js") %>
        <%: Scripts.Render("~/bundles/bootstrap_bundle_js") %>
        <%: Scripts.Render("~/bundles/datatables_js") %>
        <%: Scripts.Render("~/bundles/upload_js") %>
        <%: Scripts.Render("~/bundles/ficha_usuario_js") %>
    </asp:PlaceHolder>
</body>
</html>