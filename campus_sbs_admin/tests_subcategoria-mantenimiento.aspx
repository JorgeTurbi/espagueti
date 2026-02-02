<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tests_subcategoria-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.tests_subcategoria_mantenimiento" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SBS | Test Subcategoría mantenimiento</title>

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
        .bootstrap-select > .btn.btn-default {padding: 12px;}
        .lesspadding{padding-right: 10px!important;}
        /*.bootstrap-select.btn-group .dropdown-menu {z-index: 2147483647;}       */
    </style>

    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700,900%7COpen+Sans:400,600,700" rel="stylesheet" type="text/css" />

    <!-- Modernizr -->
    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>
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
                                <legend class="text-color-primary"><i class='fa fa-tags'></i> Mantenimiento de Test Subcategoría</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="col-sm-4">
                                    <label>Nombre *</label>
                                    <div id="nombre_form" class="form-group" runat="server">
                                        <label class="sr-only" for="txt_cont_nombre">Nombre</label>
                                        <input type="text" placeholder="Nombre" id="txt_cont_nombre" class="form-control" runat="server" maxlength="200" />
                                        <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                        <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                    </div>
                                </div>
                            </fieldset>

                            <div class="col-sm-12">
                                <a href="tests_subcategoria.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                <asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server"
                                    Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />
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

    <script type="text/javascript">
        $(function () {
            /// 1.- Cargar el textarea
            autosize($('#txt_comentarios'));
        });


        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        // Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------------------------------------------------
        function validarFormulario() {
            /// 1.- Sacar los parametros
            var nombre = $('#txt_cont_nombre').val();

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
            else
                return true;
        }
    </script>
</body>
</html>
