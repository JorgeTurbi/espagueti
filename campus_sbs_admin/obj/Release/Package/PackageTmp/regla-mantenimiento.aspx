<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="regla-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.regla_mantenimiento" %>
<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Regla automatización mantenimiento</title>

    <!-- CSS 
     =================================================== -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
	 <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
     <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
     <!-- CSS para el control DataTable -->
     <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />
     <style type="text/css">
        .input-group .form-control {background-color: white; border: 1px solid #bdbdbd; color: black;}
        .input-group.date.js-datepicker { width: 100%;}
        .input-group.has-error .form-control {background: #fbf2f1 none repeat scroll 0 0; border: 1px solid #a94442; color: #f2958d;}
        .input-group.has-error .form-control::-moz-placeholder {color: #f2958d; opacity: 1;}
        .checkbox img {height: 25px; width: 25px;}
        #txt_comentarios {max-height: 350px;}
        .bootstrap-select > .btn.btn-default { padding: 12px;}

        .card {background-clip: border-box; background-color: #fff; border-radius: 0.25rem; display: flex; flex-direction: column; min-width: 0; overflow-wrap: break-word; position: relative;}
        .card h4 {cursor: pointer;}
        .card-header {border-bottom: 1px solid #223266; margin-bottom: 0; padding: 1.5rem 1.25rem;}
        .card-body {flex: 1 1 auto; padding: 1.25rem;}        
        .card .card-header .btn-link[aria-expanded="false"]::before {border-color: #edab3a transparent transparent; border-style: solid; border-width: 7px 5px 0; content: ""; display: block; height: 0; position: absolute; right: 5px; top: 15px; width: 0;}
        .card .card-header .btn-link[aria-expanded="true"]::before {border-color: transparent transparent #edab3a; border-style: solid; border-width: 0 5px 7px; content: ""; display: block; height: 0; position: absolute; right: 5px; top: 15px; width: 0;}
        
        #collapse_bbdd_6 .dropdown-menu.open.show {top: auto !important; transform: none !important;}
        input[type=checkbox] {-webkit-appearance: checkbox}
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
                    <form id="Form1" accept-charset="utf-8" onKeyPress="if(event.keyCode == 13) event.returnValue = false;" runat="server">
                        <div>
                            <fieldset>
							    <legend class="text-color-primary"><i class='fas fa-envelope'></i> Manteniento de regla automatización</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-5">       
                                    <label>Nombre Regla</label>
                                    <div id="name_rule_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_name_rule">Nombre Regla</label>
                                        <input type="text" placeholder="Nombre Regla" id="txt_name_rule" class="form-control" maxlength="250" runat="server" />
								    </div>
                                </div>
                                <div class="col-sm-5">
                                    <label>Seleccione el tipo de automatización</label>
                                    <div id="tipo_form" class="form-group" runat="server">                                       
                                        <select id="ddl_lista_tipo_automatizacion" runat="server" class="selectpicker" data-live-search="false" data-hide-disabled="false"></select>
                                    </div>
                                </div>
                                <div class="col-sm-2">       
                                    <label>Orden</label>
                                    <div id="orden_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_orden">Orden</label>
                                        <input type="text" placeholder="Orden" id="txt_orden" class="form-control" maxlength="5" onkeyup="this.value = this.value.replace(/[^0-9.]/g, '');" runat="server" />
								    </div>
                                </div>  
                                <div class="col-sm-12">
                                    <label>Descripción</label>											
								    <div id="descripcion_form" class="form-group" runat="server">
								        <label class="sr-only" for="txt_descripcion">Descripción</label>
									    <textarea id="txt_descripcion" runat="server" placeholder="Descripción" name="txt_comentario" class="form-control" cols='2' rows='3' />
								    </div>
                                </div>                              
                                <div class="col-sm-12">
                                    <a href="lista-reglas.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>	
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                        Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                                    <div id="block_save" class="" style="height: 40vh;" runat="server"></div>
                                </div>
                                <div id="block_all" class="col-sm-12 hidden" runat="server">
                                    <div id="accordion-bbdd">
                                        <div class="card">
                                            <div class="card-header" id="heading-bbdd-1">                                
                                                <a id="card_estado" class="btn-link collapsed" data-toggle="collapse" href="#collapse_bbdd_1" aria-expanded="false" aria-controls="collapse_bbdd_1" runat="server"><h4 class="text-color-primary">Estado</h4></a>
                                            </div>
                                            <div id="collapse_bbdd_1" class="border-blue collapse" aria-labelledby="heading-bbdd-1" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_all_status" runat="server"></div>
                                                    </div>
                                                    <div class="col-sm-2 padding-tb-70">
                                                        <asp:Button ID="btn_sel_status_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">>" OnClick="btn_sel_status_all_Click" ToolTip="Añadir todos los estados" />
                                                        <asp:Button ID="btn_sel_status" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">" OnClientClick="return validarChkStatusAll();" OnClick="btn_sel_status_Click"  ToolTip="Añadir los estados seleccionados" />
                                                        <asp:Button ID="btn_desel_status" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<" OnClientClick="return validarChkStatusSel();" OnClick="btn_desel_status_Click" ToolTip="Quitar los estados seleccionados" />
                                                        <asp:Button ID="btn_desel_status_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<<" OnClick="btn_desel_status_all_Click"  ToolTip="Quitar todos los estados" />
                                                        <asp:HiddenField runat="server" ID="hidStatus" />
                                                        <asp:HiddenField runat="server" ID="hidStatusSel" />
                                                    </div>
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_sel_status" runat="server"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>     
                                        <div class="card">    
                                            <div class="card-header border-blue" id="heading-bbdd-2">
                                                <a id="card_origen" class="btn-link collapsed" data-toggle="collapse" href="#collapse_bbdd_2" aria-expanded="false" aria-controls="collapse_bbdd_2" runat="server"><h4 class="text-color-primary">Origen</h4></a>
                                            </div>                                    
                                            <div id="collapse_bbdd_2" class="border-blue collapse" aria-labelledby="heading-bbdd-2" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_all_origins" runat="server"></div>
                                                    </div>
                                                    <div class="col-sm-2 padding-tb-70">
                                                        <asp:Button ID="btn_sel_origin_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">>" OnClick="btn_sel_origin_all_Click" ToolTip="Añadir todos los origenes" />
                                                        <asp:Button ID="btn_sel_origin" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">" OnClientClick="return validarChkOriginAll();" OnClick="btn_sel_origin_Click"  ToolTip="Añadir los origenes seleccionados" />
                                                        <asp:Button ID="btn_desel_origin" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<" OnClientClick="return validarChkOriginSel();" OnClick="btn_desel_origin_Click" ToolTip="Quitar los origenes seleccionados" />
                                                        <asp:Button ID="btn_desel_origin_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<<" OnClick="btn_desel_origin_all_Click"  ToolTip="Quitar todos los origenes" />
                                                        <asp:HiddenField runat="server" ID="hidOrigenes" />
                                                        <asp:HiddenField runat="server" ID="hidOrigenesSel" />
                                                    </div>
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_sel_origins" runat="server"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card">    
                                            <div class="card-header border-blue" id="heading-bbdd-3">
                                                <a id="card_curso" class="btn-link collapsed" data-toggle="collapse" href="#collapse_bbdd_3" aria-expanded="false" aria-controls="collapse_bbdd_3" runat="server"><h4 class="text-color-primary">Curso</h4></a>
                                            </div>                                    
                                            <div id="collapse_bbdd_3" class="border-blue collapse" aria-labelledby="heading-bbdd-3" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_all_courses" runat="server"></div>
                                                    </div>
                                                    <div class="col-sm-2 padding-tb-70">
                                                        <asp:Button ID="btn_sel_course_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">>" ToolTip="Añadir todos los cursos" OnClick="btn_sel_course_all_Click" />
                                                        <asp:Button ID="btn_sel_course" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">" OnClientClick="return validarChkCourseAll();" ToolTip="Añadir los cursos seleccionados" OnClick="btn_sel_course_Click" />
                                                        <asp:Button ID="btn_desel_course" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<" OnClientClick="return validarChkCourseSel();" ToolTip="Quitar los cursos seleccionados" OnClick="btn_desel_course_Click" />
                                                        <asp:Button ID="btn_desel_course_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<<" ToolTip="Quitar todos los cursos" OnClick="btn_desel_course_all_Click" />
                                                        <asp:HiddenField runat="server" ID="hidCursos" />
                                                        <asp:HiddenField runat="server" ID="hidCursosSel" />
                                                    </div>
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_sel_courses" runat="server"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card">    
                                            <div class="card-header border-blue" id="heading-bbdd-4">
                                                <a id="card_country" class="btn-link collapsed" data-toggle="collapse" href="#collapse_bbdd_4" aria-expanded="false" aria-controls="collapse_bbdd_4" runat="server"><h4 class="text-color-primary">País</h4></a>
                                            </div>                                    
                                            <div id="collapse_bbdd_4" class="border-blue collapse" aria-labelledby="heading-bbdd-4" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_all_paises" runat="server"></div>
                                                    </div>
                                                    <div class="col-sm-2 padding-tb-70">
                                                        <asp:Button ID="btn_sel_paises_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">>" OnClick="btn_sel_paises_all_Click" ToolTip="Añadir todos los países" />
                                                        <asp:Button ID="btn_sel_paises" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text=">" OnClientClick="return validarChkCountryAll();" OnClick="btn_sel_paises_Click"  ToolTip="Añadir los países seleccionados" />
                                                        <asp:Button ID="btn_desel_paises" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<" OnClientClick="return validarChkCountrySel();" OnClick="btn_desel_paises_Click" ToolTip="Quitar los países seleccionados" />
                                                        <asp:Button ID="btn_desel_paises_all" CssClass="btn btn-primary btn-block-xs bold margin-b-15" runat="server"
                                                            Text="<<" OnClick="btn_desel_paises_all_Click"  ToolTip="Quitar todos los países" />
                                                        <asp:HiddenField runat="server" ID="hidPais" />
                                                        <asp:HiddenField runat="server" ID="hidPaisSel" />
                                                    </div>
                                                    <div class="col-sm-5 no-padding">
                                                        <div id="tabla_sel_paises" runat="server"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card">    
                                            <div class="card-header border-blue" id="heading-bbdd-5">
                                                <a id="card_date" class="btn-link collapsed" data-toggle="collapse" href="#collapse_bbdd_5" aria-expanded="false" aria-controls="collapse_bbdd_5" runat="server"><h4 class="text-color-primary">Fecha</h4></a>
                                            </div>                                    
                                            <div id="collapse_bbdd_5" class="border-blue collapse" aria-labelledby="heading-bbdd-5" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-2">
                                                        <label>Siempre</label>
                                                        <div id="all_date_form" class="form-group padding-tb-10" runat="server">													
							                                <div class="checkbox">
                                                                <asp:CheckBox ID="chk_date_all" runat="server" Text="Siempre" /> 
							                                </div>
						                                </div>
                                                    </div>
                                                    <div class="col-sm-5">
                                                        <div class="col-xs-10 col-sm-10 no-padding">
                                                            <label>Fecha Inicio</label>
								                            <div id="fecha_inicio_form" class="input-group date js-datepicker" runat="server">
								                                <label class="sr-only" for="txt_fecha_inicio">Fecha Inicio</label>
									                            <input type="text" id="txt_fecha_inicio" class="form-control" runat="server" readonly="readonly" />
                                                                <span class="input-group-addon glyphicon">
			                                                        <span class="icon-calendar xs"></span>
                                                                </span>
								                            </div>
                                                        </div>
                                                        <div class="col-xs-2 col-sm-2 text-center margin-t-20">
                                                            <a href="javascript:void(0);" onclick="clean_input('txt_fecha_inicio')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-5">
                                                        <div class="col-xs-10 col-sm-10 no-padding">
                                                            <label>Fecha Fin</label>
								                            <div id="fecha_fin_form" class="input-group date js-datepicker" runat="server">
								                                <label class="sr-only" for="txt_fecha_fin">Fecha Fin</label>
									                            <input type="text" id="txt_fecha_fin" class="form-control" runat="server" readonly="readonly" />
                                                                <span class="input-group-addon glyphicon">
			                                                        <span class="icon-calendar xs"></span>
                                                                </span>
								                            </div>
                                                        </div>
                                                        <div class="col-xs-2 col-sm-2 text-center margin-t-20">
                                                            <a href="javascript:void(0);" onclick="clean_input('txt_fecha_fin')" class="pointer"><i class="fas fa-times-circle fa-2x padding-t-10 text-color-red"></i></a>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12">
                                                        <asp:Button ID="btnGuardarFecha" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                                            Text="Guardar Fechas" OnClientClick="return validateDate();" OnClick="btnGuardarFecha_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card">    
                                            <div class="card-header border-blue" id="heading-bbdd-6">
                                                <a id="card_hour" class="btn-link collapsed" data-toggle="collapse" href="#collapse_bbdd_6" aria-expanded="false" aria-controls="collapse_bbdd_6" runat="server"><h4 class="text-color-primary">Horas</h4></a>
                                            </div>                                    
                                            <div id="collapse_bbdd_6" class="border-blue collapse" aria-labelledby="heading-bbdd-6" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-2">
                                                        <label>Siempre</label>
                                                        <div id="all_hour_form" class="form-group padding-tb-10" runat="server">													
							                                <div class="checkbox">
                                                                <asp:CheckBox ID="chk_hour_all" runat="server" Text="Siempre" /> 
							                                </div>
						                                </div>
                                                    </div>
                                                    <div class="col-sm-5 no-padding">                                                        
                                                        <div class="col-sm-6">
                                                            <label>Hora Inicio</label>
                                                            <div id="hora_inicio_form" class="form-group" runat="server"> 
                                                                <select id="hour_start" runat="server" class="selectpicker" data-live-search="false" data-hide-disabled="false"></select>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <label>Minutos inicio</label>
                                                            <div id="min_inicio_form" class="form-group" runat="server"> 
                                                                <select id="min_start" runat="server" class="selectpicker" data-live-search="false" data-hide-disabled="false"></select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-5 no-padding">
                                                        <div class="col-sm-6">
                                                            <label>Hora Fin</label>
                                                            <div id="hora_fin_form" class="form-group" runat="server">
                                                                <select id="hour_end" runat="server" class="selectpicker" data-live-search="false" data-hide-disabled="false"></select>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <label>Minutos fin</label>
                                                            <div id="min_fin_form" class="form-group" runat="server">
                                                                <select id="min_end" runat="server" class="selectpicker" data-live-search="false" data-hide-disabled="false"></select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12">
                                                        <asp:Button ID="btnGuardarHoras" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                                            Text="Guardar Fechas" OnClientClick="return validateHours();" OnClick="btnGuardarHoras_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card">
                                            <div class="card-header" id="heading-bbdd-7">                                
                                                <a id="card_actions" class="btn-link collapsed" data-toggle="collapse" href="#collapse_bbdd_7" aria-expanded="false" aria-controls="collapse_bbdd_7" runat="server"><h4 class="text-color-primary">Acciones</h4></a>
                                            </div>
                                            <div id="collapse_bbdd_7" class="border-blue collapse" aria-labelledby="heading-bbdd-7" data-parent="#accordion-bbdd" runat="server">
                                                <div class="card-body">
                                                    <div class="col-sm-4 col-sm-offset-3">
                                                        <label>Seleccione el tipo de acción</label>
                                                        <div id="tipo_accion_form" class="form-group" runat="server">                                       
                                                            <select id="ddl_lista_acciones" runat="server" class="selectpicker" data-live-search="false" data-hide-disabled="false"></select>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2 padding-t-15">
                                                        <asp:Button ID="btn_tipo_accion" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs pull-right" runat="server"
                                                            Text="Añadir" OnClientClick="return validarTipoAccion();" OnClick="btn_tipo_accion_Click" />
                                                    </div>
                                                    <div id="tabla_listado_tipos" class="col-sm-12 padding-t-15" runat="server"></div>
                                                    <div>
                                                        <asp:HiddenField ID="hidIdAccion" runat="server" />
                                                        <asp:ImageButton ID="btnBorrarAccion" runat="server" ImageUrl="~/App_Themes/support/img/pixel.png" CssClass="hidden" OnClick="btnBorrarAccion_Click" />
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
    <script type="text/javascript" src="/App_Themes/support/js/jquery.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/selectpicker.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/popper.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap-select.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap-select-es.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>    
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/rules_functions.js"></script>
    
    <script type="text/javascript">
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para eliminar un tipo de acción -----------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function eliminarAccion(id) {
            var hidId = document.getElementById('<%=hidIdAccion.ClientID %>');
            hidId.value = id;

            var boton = document.getElementById('<%=btnBorrarAccion.ClientID %>');
            boton.click();
        }
    </script>  
</body>
</html>
