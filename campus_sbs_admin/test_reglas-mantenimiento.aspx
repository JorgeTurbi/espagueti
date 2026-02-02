<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_reglas-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.test_reglas_mantenimiento" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Test Reglas mantenimiento</title>

    <!-- CSS 
     =================================================== -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    <style type="text/css">
        .input-group .form-control {
            background-color: white;
            border: 1px solid #bdbdbd;
            color: black;
        }

        .input-group.date.js-datepicker {
            width: 100%;
        }

        .input-group.has-error .form-control {
            background: #fbf2f1 none repeat scroll 0 0;
            border: 1px solid #a94442;
            color: #f2958d;
        }

            .input-group.has-error .form-control::-moz-placeholder {
                color: #f2958d;
                opacity: 1;
            }

        .checkbox img {
            height: 25px;
            width: 25px;
        }

        #btn_upload > img {
            cursor: pointer;
        }

        #fileinput > span {
            white-space: normal;
        }

        #txt_comentarios {
            max-height: 350px;
        }

        .btn.fileinput-button {
            border: 1px solid #ccc;
            height: 150px;
            margin-bottom: 5px;
            overflow: hidden;
            text-align: left;
            width: 100%;
        }

        .fileinput-button input {
            cursor: pointer;
            direction: ltr;
            height: 150px;
            left: -4px;
            margin: 0;
            opacity: 0;
            position: absolute;
            right: 0;
            top: 0;
            width: 100%;
        }

        .bootstrap-select > .btn.btn-default {
            padding: 12px;
        }
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
                                <legend class="text-color-primary"><i class='fas fa-clipboard-check'></i> Mantenimiento de Test Reglas</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>

                                <div class="col-sm-3">
                                    <label>Orden *</label>
                                    <div id="orden_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_orden">Orden *</label>
                                        <input type="number" placeholder="Orden" id="txt_orden" class="form-control" runat="server" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>Curso *</label>
                                    <div id="curso_form" class="form-group" runat="server">
                                        <asp:DropDownList ID="ddlCurso" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>Categoría *</label>
                                    <div id="categoria_form" class="form-group" runat="server">
                                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-sm-3">
                                    <label>Subcategoría</label>
                                    <div id="subcategoria_form" class="form-group" runat="server">
                                        <asp:DropDownList ID="ddlSubcategoria" runat="server" CssClass="selectpicker" data-live-search="true" data-hide-disabled="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </fieldset>

                            <div class="col-sm-3">
                                <label>Fecha Creación</label>
                                <div id="fecha_creacion_form" class="input-group date js-datepicker" runat="server">
                                    <label class="sr-only" for="txt_fecha_creacion">Fecha Creación</label>
                                    <input type="text" id="txt_fecha_creacion" disabled class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon glyphicon">
                                        <span class="icon-calendar xs"></span>
                                    </span>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <label>Fecha Última Modificación</label>
                                <div id="fecha_ult_mod_form" class="input-group date js-datepicker" runat="server">
                                    <label class="sr-only" for="txt_fecha_ult_mod">Fecha Última Modificación</label>
                                    <input type="text" id="txt_fecha_ult_mod" disabled class="form-control" runat="server" readonly="readonly" />
                                    <span class="input-group-addon glyphicon">
                                        <span class="icon-calendar xs"></span>
                                    </span>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <a id="a_volver" runat="server" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server" Text="Guardar" OnClientClick="return validarFormularioM();" OnClick="btnGuardar_Click" />
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

    <script type="text/javascript">
        function validarFormularioM() {
            /// 1.- Sacar los parametros            
            var orden = $('#txt_orden').val();

            /// 2.- Limpiar el error
            $('#txt_error').html('');

            /// 3.- Validar los datos
            //orden
            if (orden === "undefined" || orden === undefined || orden === "null" || orden === null || orden === '') {
                $('#orden_form').addClass(' has-error');
                $('#txt_error').html('El campo Orden es obligatorio');
                $('#txt_orden').attr("placeholder", "El campo Orden es obligatorio");
                subirArribaPagina();
                return false;
            }
            else if (orden < 1) {
                $('#orden_form').addClass(' has-error');
                $('#txt_error').html('El campo Orden debe ser un número mayor que 0');
                subirArribaPagina();
                return false;
            }
            else
                return true;
        }
    </script>
</body>
</html>
