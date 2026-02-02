<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_test_mantenimiento.aspx.cs" Inherits="campus_sbs_admin.test_test_mantenimiento" ValidateRequest="false" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Test mantenimiento</title>

    <!-- CSS 
     =================================================== -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    <style type="text/css">
        .input-group .form-control {background-color: white; border: 1px solid #bdbdbd; color: black;}
        .input-group.date.js-datepicker {width: 100%;}
        .input-group.has-error .form-control {background: #fbf2f1 none repeat scroll 0 0; border: 1px solid #a94442; color: #f2958d;}
        .input-group.has-error .form-control::-moz-placeholder {color: #f2958d; opacity: 1;}
        .checkbox img {height: 25px; width: 25px;}
        #btn_upload > img {cursor: pointer;}
        #fileinput > span {white-space: normal;}
        #txt_comentarios {max-height: 350px;}
        .btn.fileinput-button {border: 1px solid #ccc; height: 150px; margin-bottom: 5px; overflow: hidden; text-align: left; width: 100%;}
        .fileinput-button input {cursor: pointer; direction: ltr; height: 150px; left: -4px; margin: 0; opacity: 0; position: absolute; right: 0; top: 0; width: 100%;}
        .lesspadding {padding-right: 10px !important;}
        .bootstrap-select > .btn.btn-default {padding: 12px;}
        .alert {position: relative; padding: .75rem 1.25rem; margin-bottom: 1rem; border: 1px solid transparent; border-radius: .25rem;}
        .alert-primary {color: #004085; background-color: #cce5ff; border-color: #b8daff;}
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
                            <div class="alert alert-primary text-center" role="alert">
                                Recuerda que los test internos no pueden ser usados como tests abiertos.
                            </div>
                            <div class="alert alert-primary text-center" role="alert" id="alert_realizado" runat="server">
                                Este test ya fue realizado por un alumno y no se pueden modificar los campos deshabilidatos.
                            </div>
                            <fieldset>
                                <legend class="text-color-primary"><i class='fas fa-clipboard-list'></i>Mantenimiento de Test</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="row">

                                    <div class="col-sm-6">
                                        <label>Nombre *</label>
                                        <div id="nombre_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_nombre">Nombre *</label>
                                            <input type="text" placeholder="Nombre" id="txt_nombre" class="form-control" runat="server" />
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

                                    <div class="col-sm-12" id="normascontainer">
                                        <label>Normas *</label>
                                        <div id='normas_form' class="form-group">
                                            <label class="sr-only" for="txt_normas"></label>
                                            <textarea id="txt_normas" cols='2' rows='5' runat="server" hidden></textarea>                                            
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <label>Comentarios Iniciales *</label>
                                        <div id='comentarios_iniciales_form' class="form-group">
                                            <label class="sr-only" for="txt_comentarios_iniciales"></label>
                                            <textarea id="txt_comentarios_iniciales" placeholder="Comentarios Iniciales" class="form-control" cols='2' rows='5' runat="server"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <label>Comentarios Internos </label>
                                        <div id='comentarios_interno_form' class="form-group">
                                            <label class="sr-only" for="txt_comentarios_iniciales"></label>
                                            <textarea id="txt_comentarios_interno" placeholder="Comentarios Internos" class="form-control" cols='2' rows='5' runat="server"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <label>Comentarios Finales </label>
                                        <div id='comentarios_finales_form' class="form-group">
                                            <label class="sr-only" for="txt_comentarios_finales"></label>
                                            <textarea id="txt_comentarios_finales" placeholder="Comentarios Finales" class="form-control" cols='2' rows='5' runat="server"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label>Tiempo *</label>
                                        <div id="tiempo_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_tiempo"></label>
                                            <input type="number" id="txt_tiempo" class="form-control lesspadding text-right" runat="server" />
                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label>N°. Preguntas *</label>
                                        <div id="nopreguntas_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_nopreguntas"></label>
                                            <input type="number" id="txt_nopreguntas" class="form-control lesspadding text-right" runat="server" />
                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label>Acierto Suma *</label>
                                        <div id="acierto_suma_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_acierto_suma"></label>
                                            <input type="number" id="txt_acierto_suma" class="form-control lesspadding text-right" runat="server" />
                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label>Error Resta *</label>
                                        <div id="error_resta_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_error_resta"></label>
                                            <input type="text" id="txt_error_resta" class="form-control lesspadding text-right" runat="server" />
                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label>Apto Puntos *</label>
                                        <div id="apto_puntos_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_apto_puntos"></label>
                                            <input type="number" id="txt_apto_puntos" class="form-control lesspadding text-right" runat="server" />
                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label>Dificultad *</label>
                                        <div id="dificultad_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_dificultad"></label>
                                            <input type="number" id="txt_dificultad" class="form-control lesspadding text-right" runat="server" />
                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>

                                    <div class="col-sm-2">
                                        <label>N°. Intentos</label>
                                        <div id="nointentos_form" class="form-group" runat="server">
                                            <label class="sr-only" for="txt_nointentos"></label>
                                            <input type="number" id="txt_nointentos" class="form-control lesspadding text-right" runat="server" />
                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label>&nbsp;</label>
                                        <div id="hacerlo_en_partes_form" class="form-group padding-t-10" runat="server">
                                            <div class="checkbox">
                                                <asp:CheckBox ID="chk_hacerlo_en_partes" runat="server" Text="Hacerlo en partes" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <label>&nbsp;</label>
                                        <div id="interpretado_form" class="form-group padding-t-10" runat="server">
                                            <div class="checkbox">
                                                <asp:CheckBox ID="chk_interpretado" runat="server" Text="Interpretado" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <label>&nbsp;</label>
                                        <div id="abierto_form" class="form-group padding-t-10" runat="server">
                                            <div class="checkbox">
                                                <asp:CheckBox ID="chk_abierto" runat="server" Text="Abierto" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <label>Cliente Test</label>
                                        <div id="test_cliente_form" class="form-group" runat="server">
                                            <select ID="ddlClienteTest" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true" data-live-search-normalize="true"></select>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                            <div class="row">

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
                                    <label>Fecha Modificación</label>
                                    <div id="fecha_ult_mod_form" class="input-group date" runat="server">
                                        <label class="sr-only" for="txt_fecha_ult_mod">Fecha Última Modificación</label>
                                        <input type="text" id="txt_fecha_ult_mod" disabled class="form-control" runat="server" readonly="readonly" />
                                        <span class="input-group-addon glyphicon">
                                            <span class="icon-calendar xs"></span>
                                        </span>
                                    </div>
                                </div>

                                <div class="col-sm-12">
                                    <label>Descripción Pública</label>
                                    <div class="form-group">
                                        <label class="sr-only" for="txt_descripcion_publica"></label>
                                        <textarea id="txt_descripcion_publica" cols='2' rows='5' runat="server"></textarea>                                            
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Title</label>													
								    <div id="metaTitle_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaTitle">Meta Title</label>
                                        <input type="text" placeholder="Meta Title" id="txtMetaTitle" class="form-control" runat="server" maxlength="600" />
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Keywords</label>													
								    <div id="metaKey_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaKeywords">Meta Keywords</label>
									    <input type="text" placeholder="Meta Keywords" id="txtMetaKeywords" class="form-control" runat="server" />
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Url (Sólo caracteres alfanuméricos, unidos por guiones medios y sin acentos)</label>													
								    <div id="metaUrl_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaUrl">Meta Url</label>
									    <input type="text" placeholder="Meta Url" id="txtMetaUrl" class="form-control" runat="server" maxlength="500" />
								    </div>
                                </div>
                                <div class="col-sm-6">
                                    <label>Meta Author</label>													
								    <div id="metaAuthor_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaAuthor">Meta Author</label>
									    <input type="text" placeholder="Meta Author" id="txtMetaAuthor" class="form-control" runat="server" maxlength="100" />
								    </div>
                                </div>
                                <div class="col-sm-12">
                                    <label>Meta Descripción</label>													
								    <div id="metaDesc_form" class="form-group" runat="server">
								        <label class="sr-only" for="txtMetaDescripcion">Meta Descripción</label>
                                        <textarea rows="4" cols="50" class="form-control" id="txtMetaDescripcion" placeholder="Meta Descripción" runat="server"></textarea>
								    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-sm-12">
                                <a href="test_test.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server" Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
                            </div>
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
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>

    <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        CKEDITOR.replace('txt_normas',
        {
            /*toolbar: 'SBSToolbar'*/
        });
        
        CKEDITOR.replace('txt_descripcion_publica',
        {
            /*toolbar: 'SBSToolbar'*/
        });

        function validarFormularioM() {
            /// 1.- Sacar los parametros
            var nombre = $('#txt_nombre').val();
            var normas = CKEDITOR.instances['txt_normas'].getData();
            var comentarios_iniciales = $('#txt_comentarios_iniciales').val();
            var comentarios_interno = $('#txt_comentarios_interno').val();
            var comentarios_finales = $('#txt_comentarios_finales').val();
            var tiempo = $('#txt_tiempo').val();
            var nopreguntas = $('#txt_nopreguntas').val();
            var acierto_suma = $('#txt_acierto_suma').val();
            var error_resta = $('#txt_error_resta').val();
            var apto_puntos = $('#txt_apto_puntos').val();
            var dificultad = $('#txt_dificultad').val();
            var nointentos = $('#txt_nointentos').val();
            
            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
                /// Nombre
            if (nombre === "undefined" || nombre === undefined || nombre === "null" || nombre === null || nombre === '') {
                $('#nombre_form').addClass(' has-error');
                $('#txt_error').html('El campo Nombre es obligatorio');
                $('#txt_nombre').attr("placeholder", "El campo Nombre es obligatorio");
                subirArribaPagina();
                return false;
            }
                /// Normas
            else if (normas === "undefined" || normas === undefined || normas === "null" || normas === null || normas === '' || normas == '<p><br data-cke-filler="true"></p>') {
                $('#normas_form').addClass(' has-error');
                $('#txt_error').html('El campo Normas es obligatorio');
                $('#txt_normas').attr("placeholder", "El campo Normas es obligatorio");
                subirArribaPagina();
                return false;
            }
                /// comentarios_iniciales
            else if (comentarios_iniciales === "undefined" || comentarios_iniciales === undefined || comentarios_iniciales === "null" || comentarios_iniciales === null || comentarios_iniciales === '') {
                $('#comentarios_iniciales_form').addClass(' has-error');
                $('#txt_error').html('El campo Comentarios Iniciales es obligatorio');
                $('#txt_comentarios_iniciales').attr("placeholder", "El campo Comentarios Iniciales es obligatorio");
                subirArribaPagina();
                return false;
            }
                /// tiempo
            else if (tiempo === "undefined" || tiempo === undefined || tiempo === "null" || tiempo === null || tiempo === '') {
                $('#tiempo_form').addClass(' has-error');
                $('#txt_error').html('El campo Tiempo es obligatorio');
                $('#txt_tiempo').attr("placeholder", "El campo Tiempo es obligatorio");
                subirArribaPagina();
                return false;
            }
                /// nopreguntas
            else if (nopreguntas === "undefined" || nopreguntas === undefined || nopreguntas === "null" || nopreguntas === null || nopreguntas === '') {
                $('#nopreguntas_form').addClass(' has-error');
                $('#txt_error').html('El campo N°. Preguntas es obligatorio');
                $('#text_nopreguntas').attr("placeholder", "El campo N°. Preguntas es obligatorio");
                subirArribaPagina();
                return false;
            }
                /// acierto_suma
            else if (acierto_suma === "undefined" || acierto_suma === undefined || acierto_suma === "null" || acierto_suma === null || acierto_suma === '') {
                $('#acierto_suma_form').addClass(' has-error');
                $('#txt_error').html('El campo Acierto Suma es obligatorio');
                $('#txt_acierto_suma').attr("placeholder", "El campo Acierto Suma es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (acierto_suma < 0) {
                $('#acierto_suma_form').addClass(' has-error');
                $('#txt_error').html('El campo Acierto Suma debe ser un número mayor o igual que 0');
                subirArribaPagina();
                return false;
            }
                /// error_resta
            else if (error_resta === "undefined" || error_resta === undefined || error_resta === "null" || error_resta === null || error_resta === '') {
                $('#error_resta_form').addClass(' has-error');
                $('#txt_error').html('El campo Error Resta es obligatorio');
                $('#txt_error_resta').attr("placeholder", "El campo Error Resta es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (error_resta < 0) {
                $('#error_resta_form').addClass(' has-error');
                $('#txt_error').html('El campo Error resta debe ser un número mayor o igual que 0');
                subirArribaPagina();
                return false;
            }
            else if (isNaN(error_resta)) {
                $('#error_resta_form').addClass(' has-error');
                $('#txt_error').html('El campo Error resta debe ser un número mayor o igual que 0');
                subirArribaPagina();
                return false;
            }
                /// apto_puntos
            else if (apto_puntos === "undefined" || apto_puntos === undefined || apto_puntos === "null" || apto_puntos === null || apto_puntos === '') {
                $('#apto_puntos_form').addClass(' has-error');
                $('#txt_error').html('El campo Apto Puntos es obligatorio');
                $('#txt_apto_puntos').attr("placeholder", "El campo Apto Puntos es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (apto_puntos < 0) {
                $('#apto_puntos_form').addClass(' has-error');
                $('#txt_error').html('El campo Apto Puntos debe ser un número mayor o igual que 0');
                subirArribaPagina();
                return false;
            }
                /// dificultad
            else if (dificultad === "undefined" || dificultad === undefined || dificultad === "null" || dificultad === null || dificultad === '') {
                $('#dificultad_form').addClass(' has-error');
                $('#txt_error').html('El campo Dificultad es obligatorio');
                $('#txt_dificultad').attr("placeholder", "El campo Dificultad es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (dificultad < 1 || dificultad > 5) {
                $('#dificultad_form').addClass(' has-error');
                $('#txt_error').html('El campo Dificultad debe ser un número entre 1 y 5');
                subirArribaPagina();
                return false;
            }
                /// nointentos
            else if (nointentos === "undefined" || nointentos === undefined || nointentos === "null" || nointentos === null || nointentos === '') {
                $('#nointentos_form').addClass(' has-error');
                $('#txt_error').html('El campo N°. Intentos es obligatorio');
                $('#txt_nointentos').attr("placeholder", "El campo N°. Intentos es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (nointentos < 1) {
                $('#nointentos_form').addClass(' has-error');
                $('#txt_error').html('El campo N°. Intentos debe ser un número mayor que 0');
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }
    </script>
</body>
</html>

