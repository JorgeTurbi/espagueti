<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newsletter-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.newsletter_mantenimiento" %>
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
        
        #txt_text_1, #txt_text_2 {max-height: 350px;}
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
							    <legend class="text-color-primary"><i class='fas fa-envelope'></i> Manteniento de Newsletter</legend>
                                <div id="blk_step_1" class="">
                                    <div id="block_error" class="form-group has-error" runat="server">
                                        <span id="txt_error" class="help-block text-center" runat="server"></span>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Noticia principal</label>
                                        <div id="noticia_principal_form" class="form-group" runat="server">
                                            <input type="text" placeholder="Noticia principal" id="txt_noticia_principal" autocomplete="off" class="form-control" runat="server" />
                                            <input id="id_noticia_principal" type="hidden" value="-1" runat="server" />
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Noticia secundaria 1</label>
                                        <div id="noticia_secundaria_1_form" class="form-group" runat="server">
                                            <input type="text" placeholder="Noticia secundaria 1" id="txt_noticia_secundaria_1" autocomplete="off" class="form-control" runat="server" />
                                            <input id="id_noticia_secundaria_1" type="hidden" value="-1" runat="server" />
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Noticia secundaria 2</label>
                                        <div id="noticia_secundaria_2_form" class="form-group" runat="server">
                                            <input type="text" placeholder="Noticia secundaria 2" id="txt_noticia_secundaria_2" autocomplete="off" class="form-control" runat="server" />
                                            <input id="id_noticia_secundaria_2" type="hidden" value="-1" runat="server" />
								        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label>Noticia secundaria 3</label>
                                        <div id="noticia_secundaria_3_form" class="form-group" runat="server">
                                            <input type="text" placeholder="Noticia secundaria 3" id="txt_noticia_secundaria_3" autocomplete="off" class="form-control" runat="server" />
                                            <input id="id_noticia_secundaria_3" type="hidden" value="-1" runat="server" />
								        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Texto 1</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_text_1">Texto 1</label>
									        <textarea id="txt_text_1" placeholder="Texto 1" class="form-control" cols='2' rows='5' runat="server"></textarea>										
								        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Url texto 1</label>
                                        <div class="form-group" runat="server">
                                            <input type="text" placeholder="Url texto 1" id="txt_url_1" class="form-control" runat="server" />
								        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Texto 2</label>
                                        <div class="form-group" runat="server">
								            <label class="sr-only" for="txt_text_2">Texto 2</label>
									        <textarea id="txt_text_2" placeholder="Texto 2" class="form-control" cols='2' rows='5' runat="server"></textarea>										
								        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <label>Url texto 2</label>
                                        <div class="form-group" runat="server">
                                            <input type="text" placeholder="Url texto 2" id="txt_url_2" class="form-control" runat="server" />
								        </div>
                                    </div>
                                    <div id="block_save" class="col-sm-12" runat="server">
                                        <a href="javascript:void(0);" onclick="next_step()" class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right">Siguiente</a>
                                    </div>
                                </div>

                                <div id="block_all" class="hidden" runat="server">                                    
                                    <div id="preview_newsletter" class="col-sm-12"></div>
                                    <div class="col-sm-12 margin-t-20" runat="server">
                                        <a href="javascript:void(0);" onclick="previous_step()" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>

                                        <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-xs-b-15 pull-right" runat="server"
                                            Text="Guardar" OnClick="btnGuardar_Click" />
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
    <script type="text/javascript" src="/App_Themes/support/js/jquery-ui.js"></script>  
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>
    <script type="text/javascript">
        $(function () {
            /// 1.- Cargar los textarea
            autosize($('#txt_text_1'));
            autosize($('#txt_text_2'));
                        
            /// 2.- Recuperar la tecla pulsada
            $("#txt_noticia_principal").on('keypress', $(this), function (e) {
                if (e.which === 13 && $(this).val() === "") {
                    return false;
                }
            });
            $("#txt_noticia_secundaria_1").on('keypress', $(this), function (e) {
                if (e.which === 13 && $(this).val() === "") {
                    return false;
                }
            });
            $("#txt_noticia_secundaria_2").on('keypress', $(this), function (e) {
                if (e.which === 13 && $(this).val() === "") {
                    return false;
                }
            });
            $("#txt_noticia_secundaria_3").on('keypress', $(this), function (e) {
                if (e.which === 13 && $(this).val() === "") {
                    return false;
                }
            });
            
            /// 3.- Autocompletar
            $("#txt_noticia_principal").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'newsletter-mantenimiento.aspx/search_news',
                        data: "{ 'name': '" + request.term + "','noticia1': '" + $('#id_noticia_principal').val() + "','noticia2': '" + $('#id_noticia_secundaria_1').val() + "','noticia3': '" + $('#id_noticia_secundaria_2').val() + "','noticia4': '" + $('#id_noticia_secundaria_3').val() + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.titulo + " (" + item.id_noticia + ")",
                                    val: item.id_noticia
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
                    $('#id_noticia_principal').val(ui.item.val);
                    $("#txt_noticia_secundaria_1").focus();
                },
                minLength: 2
            });
            $("#txt_noticia_secundaria_1").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'newsletter-mantenimiento.aspx/search_news',
                        data: "{ 'name': '" + request.term + "','noticia1': '" + $('#id_noticia_principal').val() + "','noticia2': '" + $('#id_noticia_secundaria_1').val() + "','noticia3': '" + $('#id_noticia_secundaria_2').val() + "','noticia4': '" + $('#id_noticia_secundaria_3').val() + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.titulo + " (" + item.id_noticia + ")",
                                    val: item.id_noticia
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
                    $('#id_noticia_secundaria_1').val(ui.item.val);
                    $("#txt_noticia_secundaria_2").focus();
                },
                minLength: 2
            });
            $("#txt_noticia_secundaria_2").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'newsletter-mantenimiento.aspx/search_news',
                        data: "{ 'name': '" + request.term + "','noticia1': '" + $('#id_noticia_principal').val() + "','noticia2': '" + $('#id_noticia_secundaria_1').val() + "','noticia3': '" + $('#id_noticia_secundaria_2').val() + "','noticia4': '" + $('#id_noticia_secundaria_3').val() + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.titulo + " (" + item.id_noticia + ")",
                                    val: item.id_noticia
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
                    $('#id_noticia_secundaria_2').val(ui.item.val);
                    $("#txt_noticia_secundaria_3").focus();
                },
                minLength: 2
            });
            $("#txt_noticia_secundaria_3").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'newsletter-mantenimiento.aspx/search_news',
                        data: "{ 'name': '" + request.term + "','noticia1': '" + $('#id_noticia_principal').val() + "','noticia2': '" + $('#id_noticia_secundaria_1').val() + "','noticia3': '" + $('#id_noticia_secundaria_2').val() + "','noticia4': '" + $('#id_noticia_secundaria_3').val() + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.titulo + " (" + item.id_noticia + ")",
                                    val: item.id_noticia
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
                    $('#id_noticia_secundaria_3').val(ui.item.val);
                    $("#txt_text_1").focus();
                },
                minLength: 2
            });
        });

        function next_step() {
            /// 1.- Borrar el error
            $('#txt_error').html('');

            /// 2.- Sacar datos del formulario
            var noticia_principal = $('#id_noticia_principal').val();
            var noticia_secundaria = $('#id_noticia_secundaria_1').val();
            var noticia_secundaria_2 = $('#id_noticia_secundaria_2').val();
            var noticia_secundaria_3 = $('#id_noticia_secundaria_3').val();
            var texto_1 = $('#txt_text_1').val();
            var url_1 = $('#txt_url_1').val();
            var texto_2 = $('#txt_text_2').val();
            var url_2 = $('#txt_url_2').val();

            /// 3.- Comprobar que la noticia principal está rellena
            if (noticia_principal == -1) {
                $('#noticia_principal_form').addClass(' has-error');
                $('#txt_error').html('La noticia principal es obligatoria');
                subirArribaPagina();
            }
            else {
                $.ajax({
                    type: "POST",
                    crossDomain: true,
                    url: 'newsletter-mantenimiento.aspx/cargarNewsletter',
                    data: "{ 'noticia_principal': '" + noticia_principal + "','noticia_secundaria': '" + noticia_secundaria + "','noticia_secundaria_2': '" + noticia_secundaria_2 + "','noticia_secundaria_3': '" + noticia_secundaria_3 + "','texto_1': '" + texto_1 + "','url_1': '" + url_1 + "','texto_2': '" + texto_2 + "','url_2': '" + url_2 + "'}",
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.d !== null && data.d !== "") {
                            $('#blk_step_1').addClass("hidden");
                            $('#block_all').removeClass("hidden");

                            $('#preview_newsletter').html(data.d);
                        }
                    },
                    error: function (response) {
                        return false;
                    },
                    failure: function (response) {
                        return false;
                    }
                });
            }
        }

        function previous_step() {
            /// 1.- Ocultar paso 2
            $('#block_all').addClass("hidden");
            $('#preview_newsletter').html('');

            /// 2.- Mostrar paso 1
            $('#blk_step_1').removeClass("hidden");
        }
    </script>
</body>
</html>