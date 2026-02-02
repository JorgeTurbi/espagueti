<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tests_inventario_pregunta-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.tests_inventario_pregunta_mantenimiento" validateRequest="false" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Test Preguntas mantenimiento</title>

    <!-- CSS 
     =================================================== -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    <style type="text/css">
        .input-group .form-control { background-color: white; border: 1px solid #bdbdbd; color: black;}
        .input-group.date.js-datepicker {width: 100%;}
        .input-group.has-error .form-control {background: #fbf2f1 none repeat scroll 0 0; border: 1px solid #a94442; color: #f2958d;}
        .input-group.has-error .form-control::-moz-placeholder {color: #f2958d; opacity: 1;}
        .checkbox img {height: 25px; width: 25px;}
        #btn_upload > img {cursor: pointer;}
        #fileinput > span {white-space: normal;}
        #txt_comentarios {max-height: 350px;}
        .btn.fileinput-button {border: 1px solid #ccc; height: 150px; margin-bottom: 5px; overflow: hidden; text-align: left; width: 100%;}
        .fileinput-button input {cursor: pointer; direction: ltr; height: 150px; left: -4px; margin: 0; opacity: 0; position: absolute; right: 0; top: 0; width: 100%;}
        .bootstrap-select > .btn.btn-default {padding: 12px;}
        .form-group.has-error .help-block {background-color: red; border-radius: 5px; color: white;}
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
                                <legend class="text-color-primary"><i class='fas fa-question-circle'></i> Mantenimiento de Test Preguntas</legend>
                                <div id="block_error" class="col-sm-12 form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-12" id="preguntacontainer">
                                    <label>Pregunta *</label>
                                    <div id='texto_form' class="form-group">
                                        <label class="sr-only" for="txt_texto"></label>
                                        <textarea id="txt_texto" placeholder="Texto" class="" cols='2' rows='5' runat="server"></textarea>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label>Adjunto URL</label>
                                    <div id="adjunto_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_adjunto">Adjunto URL</label>
                                        <input type="text" placeholder="Adjunto URL" id="txt_adjunto" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label>Pregunta URL</label>
                                    <div id="pregunta_url_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_pregunta_url">Pregunta URL</label>
                                        <input type="text" placeholder="Adjunto URL" id="txt_pregunta_url" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-12" id="interpretacioncontainer">
                                    <label>Interpretación</label>
                                    <div id="interpretacion_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_interpretacion">Interpretación</label>
                                        <textarea id="txt_interpretacion" class="" cols='2' rows='5' runat="server"></textarea>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 1 *</label>
                                    <div id="respuesta1_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta1">Respuesta 1</label>
                                        <input type="text" placeholder="Respuesta 1" id="txt_respuesta1" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 2 *</label>
                                    <div id="respuesta2_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta2">Respuesta 2</label>
                                        <input type="text" placeholder="Respuesta 2" id="txt_respuesta2" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 3</label>
                                    <div id="respuesta3_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta3">Respuesta 3</label>
                                        <input type="text" placeholder="Respuesta 3" id="txt_respuesta3" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 4</label>
                                    <div id="respuesta4_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta4">Respuesta 4</label>
                                        <input type="text" placeholder="Respuesta 4" id="txt_respuesta4" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 5</label>
                                    <div id="respuesta5_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta5">Respuesta 5</label>
                                        <input type="text" placeholder="Respuesta 5" id="txt_respuesta5" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 6</label>
                                    <div id="respuesta6_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta6">Respuesta 6</label>
                                        <input type="text" placeholder="Respuesta 6" id="txt_respuesta6" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 7</label>
                                    <div id="respuesta7_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta7">Respuesta 7</label>
                                        <input type="text" placeholder="Respuesta 7" id="txt_respuesta7" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 8</label>
                                    <div id="respuesta8_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta8">Respuesta 8</label>
                                        <input type="text" placeholder="Respuesta 8" id="txt_respuesta8" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 9</label>
                                    <div id="respuesta9_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta9">Respuesta 9</label>
                                        <input type="text" placeholder="Respuesta 9" id="txt_respuesta9" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Respuesta 10</label>
                                    <div id="respuesta10_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta10">Respuesta 10</label>
                                        <input type="text" placeholder="Respuesta 10" id="txt_respuesta10" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Respuesta Correcta *</label>
                                    <div id="respuesta_correcta_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_respuesta_correcta">Respuesta Correcta</label>
                                        <input type="number" id="text_respuesta_correcta_" class="form-control" runat="server" min="1" max="10" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Dificultad *</label>
                                    <div id="dificultad_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_dificultad">Dificultad</label>
                                        <input type="number" id="txt_dificultad" class="form-control" runat="server"/>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Curso *</label>
                                    <div id="curso_form" class="form-group" runat="server">
                                        <select ID="ddlCurso" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true" data-live-search-normalize="true"></select>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Categoría *</label>
                                    <div id="categoria_form" class="form-group" runat="server">
                                        <select ID="ddlCategoria" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true" data-live-search-normalize="true"></select>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Subcategoría</label>
                                    <div id="subcategoria_form" class="form-group" runat="server">
                                        <select ID="ddlSubcategoria" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true" data-live-search-normalize="true"></select>
                                    </div>
                                </div>                            
                                <div class="col-sm-3">
                                    <label>Fecha Creación</label>
                                    <div id="fecha_creacion_form" class="input-group date" runat="server">
                                        <label class="sr-only" for="txt_fecha_creacion">Fecha Creación</label>
                                        <input type="text" id="txt_fecha_creacion" disabled class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
                                            <span class="icon-calendar xs"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>Fecha Última Modificación</label>
                                    <div id="fecha_ult_mod_form" class="input-group date" runat="server">
                                        <label class="sr-only" for="txt_fecha_ult_mod">Fecha Última Modificación</label>
                                        <input type="text" id="txt_fecha_ult_mod" disabled class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
                                            <span class="icon-calendar xs"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <label>&nbsp;</label>
                                    <div id="baja_form" class="form-group padding-t-10" runat="server">
                                        <div class="checkbox">
                                            <asp:CheckBox ID="chk_baja" runat="server" Text="Baja" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <a href="test_inventario_pregunta.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" 
                                        runat="server" Text="Guardar" OnClientClick="return validarFormularioM();" OnClick="btnGuardar_Click" />
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

    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.iframe-transport.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.fileupload.js"></script>

    <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        CKEDITOR.replace('txt_texto',
        {
            /*toolbar: 'SBSToolbar'*/
        });
        CKEDITOR.replace('txt_interpretacion',
        {
            /*toolbar: 'SBSToolbar'*/
        });
        
        function validarFormularioM() {
            /// 1.- Sacar los parametros
            var texto = CKEDITOR.instances['txt_texto'].getData();
            var adjunto = $('#txt_adjunto').val();
            var url = $('#txt_pregunta_url').val();
            var interpretacion = CKEDITOR.instances['txt_interpretacion'].getData();
            var respuesta1 = $('#txt_respuesta1').val();
            var respuesta2 = $('#txt_respuesta2').val();
            var respuestacorrecta = $('#text_respuesta_correcta_').val();
            var dificultad = $('#txt_dificultad').val();
            var curso = $('#ddlCurso').val();
            var categoria = $('#ddlCategoria').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos

                // Pregunta
            if (texto === "undefined" || texto === undefined || texto === "null" || texto === null || texto === '') {
                $('#texto_form').addClass(' has-error');
                $('#txt_error').html('El campo Pregunta es obligatorio');
                $('#txt_texto').attr("placeholder", "El campo Pregunta es obligatorio");
                subirArribaPagina();
                return false;
            }
            /*else if (!validarCarateresEspeciales(texto)) {
                $('#texto_form').addClass(' has-error');
                $('#txt_error').html('El campo Pregunta contiene carácteres no válidos');
                subirArribaPagina();
                return false;
            }*/
                // respuesta1
            else if (respuesta1 === "undefined" || respuesta1 === undefined || respuesta1 === "null" || respuesta1 === null || respuesta1 === '') {
                $('#respuesta1_form').addClass(' has-error');
                $('#txt_error').html('El campo Respuesta 1 es obligatorio');
                $('#txt_respuesta1').attr("placeholder", "El campo Respuesta 1 es obligatorio");
                subirArribaPagina();
                return false;
            }
            /*else if (!validarCarateresEspeciales(respuesta1)) {
                $('#respuesta1_form').addClass(' has-error');
                $('#txt_error').html('El campo Respuesta 1 contiene carácteres no válidos');
                subirArribaPagina();
                return false;
            }*/
                // respuesta2
            else if (respuesta2 === "undefined" || respuesta2 === undefined || respuesta2 === "null" || respuesta2 === null || respuesta2 === '') {
                $('#respuesta2_form').addClass(' has-error');
                $('#txt_error').html('El campo Respuesta 2 es obligatorio');
                $('#txt_respuesta2').attr("placeholder", "El campo Respuesta 2 es obligatorio");
                subirArribaPagina();
                return false;
            }
            /*else if (!validarCarateresEspeciales(respuesta2)) {
                $('#respuesta1_form').addClass(' has-error');
                $('#txt_error').html('El campo Respuesta 2 contiene carácteres no válidos');
                subirArribaPagina();
                return false;
            }*/
                // resp correcta
            else if (respuestacorrecta === "undefined" || respuestacorrecta === undefined || respuestacorrecta === "null" || respuestacorrecta === null || respuestacorrecta === '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('El campo Respuesta Correcta es obligatorio');
                $('#text_respuesta_correcta_').attr("placeholder", "El campo Respuesta Correcta es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta < 1 || respuestacorrecta > 10) {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('La Respuesta Correcta debe ser un número entre 1 y 10');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 3 && $('#txt_respuesta3').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 4 && $('#txt_respuesta4').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 5 && $('#txt_respuesta5').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 6 && $('#txt_respuesta6').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 7 && $('#txt_respuesta7').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 8 && $('#txt_respuesta8').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 9 && $('#txt_respuesta9').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
            else if (respuestacorrecta == 10 && $('#txt_respuesta10').val() == '') {
                $('#respuesta_correcta_form').addClass(' has-error');
                $('#txt_error').html('No puede establecer como Respuesta Correcta una respuesta vacía');
                subirArribaPagina();
                return false;
            }
                // dificultad
            else if (dificultad === "undefined" || dificultad === undefined || dificultad === "null" || dificultad === null || dificultad === '') {
                $('#dificultad_form').addClass(' has-error');
                $('#txt_error').html('El campo Dificultad es obligatorio');
                $('#txt_dificultad').attr("placeholder", "El campo Dificultad es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (dificultad < 1 || dificultad > 5) {
                $('#dificultad_form').addClass(' has-error');
                $('#txt_error').html('La Dificultad debe ser un número entre 1 y 5');
                subirArribaPagina();
                return false;
            }
                // curso
            else if (curso === "0") {
                $('#curso_form').addClass(' has-error');
                $('#txt_error').html('El curso es obligatorio');
                $('#btn_guardar').removeAttr('disabled');
                subirArribaPagina();
                return false;
            }
                // categoría
            else if (categoria === "0") {
                $('#categoria_form').addClass(' has-error');
                $('#txt_error').html('La categoría es obligatoria');
                $('#btn_guardar').removeAttr('disabled');
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }
    </script>
</body>
</html>
