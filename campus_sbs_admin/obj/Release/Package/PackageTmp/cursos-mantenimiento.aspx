<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cursos-mantenimiento.aspx.cs" Inherits="campus_sbs_admin.cursos_mantenimiento" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Cursos mantenimiento</title>

    <!-- CSS =================================================== -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    <!-- CSS para el control DataTable -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />
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

        #txt_comentarios {
            max-height: 350px;
        }

        .bootstrap-select > .btn.btn-default {
            padding: 12px;
        }

        .card {
            background-clip: border-box;
            background-color: #fff;
            border-radius: 0.25rem;
            display: flex;
            flex-direction: column;
            min-width: 0;
            overflow-wrap: break-word;
            position: relative;
        }

            .card h4 {
                cursor: pointer;
            }

        .card-header {
            border-bottom: 1px solid #223266;
            margin-bottom: 0;
            padding: 1.5rem 1.25rem;
        }

        .card-body {
            flex: 1 1 auto;
            padding: 1.25rem;
        }

        .card .card-header .btn-link[aria-expanded="false"]::before {
            border-color: #edab3a transparent transparent;
            border-style: solid;
            border-width: 7px 5px 0;
            content: "";
            display: block;
            height: 0;
            position: absolute;
            right: 5px;
            top: 15px;
            width: 0;
        }

        .card .card-header .btn-link[aria-expanded="true"]::before {
            border-color: transparent transparent #edab3a;
            border-style: solid;
            border-width: 0 5px 7px;
            content: "";
            display: block;
            height: 0;
            position: absolute;
            right: 5px;
            top: 15px;
            width: 0;
        }

        #collapse_bbdd_6 .dropdown-menu.open.show {
            top: auto !important;
            transform: none !important;
        }

        input[type=checkbox] {
            -webkit-appearance: checkbox;
        }
    </style>
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

        .lesspadding {
            padding-right: 10px !important;
        }

        .bootstrap-select > .btn.btn-default {
            padding: 12px;
        }

        .alert {
            position: relative;
            padding: .75rem 1.25rem;
            margin-bottom: 1rem;
            border: 1px solid transparent;
            border-radius: .25rem;
        }

        .alert-primary {
            color: #000;
            background-color: #cce5ff;
            border-color: #b8daff;
        }

        .alert-success {
            color: #000;
        }

        .alert-danger {
            color: #000;
        }

        .badgeareas {
            margin-right: 5px;
            padding: 8px;
            margin-bottom: 8px;
            cursor: pointer;
        }

        .badgecontainer {
            border: solid black 1px;
            padding: 10px;
        }

        .ml-5px {
            margin-left: 5px;
        }

        .nobullets {
            list-style-type: none !important;
        }

        .contenidolistcontainer {
            padding-left: 30px;
            padding-right: 30px;
        }

            .contenidolistcontainer ul li:before {
                content: "";
            }

            .contenidolistcontainer ul li {
                height: 55px;
                vertical-align: central;
            }

        .d-flex {
            display: flex;
        }
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
        <section class="padding-tb-20">
            <div class="row no-margin padding-nav">
                <div class="col-sm-12">
                    <form id="Form1" accept-charset="utf-8" runat="server">
                        <div>
                            <fieldset>
                                <legend class="text-color-primary" style="margin-bottom: 0px"><i class='fas fa-clipboard-list'></i>Mantenimiento de Curso</legend>
                                <div id="block_error" class="form-group has-error" runat="server">
                                    <span id="txt_error" class="help-block text-center" runat="server"></span>
                                </div>
                                <div class="row px-4">
                                    <ul class="nav nav-tabs" style="margin-bottom: 0px;">
                                        <li class="active" style="width: auto"><a data-toggle="tab" href="#curso">Curso</a></li>
                                        <li style="width: auto"><a data-toggle="tab" href="#literales">Literales</a></li>
                                        <li style="width: auto"><a data-toggle="tab" href="#contenidos">Contenidos</a></li>
                                    </ul>

                                    <div class="tab-content">
                                        <div id="curso" class="tab-pane fade in active padding10">
                                            <div id="messagealert" class="alert text-center" role="alert">
                                            </div>
                                            <input id="txtidcurso" hidden runat="server" />
                                            <%--codigo--%>
                                            <div class="col-sm-2">
                                                <label>Código *</label>
                                                <div id="codigo_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="txt_codigo">Código *</label>
                                                    <input type="text" placeholder="Código" id="txt_codigo" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--nombre--%>
                                            <div class="col-sm-4">
                                                <label>Nombre *</label>
                                                <div id="nombre_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="txt_nombre">Nombre *</label>
                                                    <input type="text" placeholder="Nombre" id="txt_nombre" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--Fecha inicio--%>
                                            <div class="col-sm-2">
                                                <label>Fecha Alta *</label>
                                                <div id="fechaAlta_form" class="input-group date js-datepicker" runat="server">
                                                    <label class="sr-only" for="txt_FechaAlta">Fecha Alta</label>
                                                    <input type="text" id="txt_FechaAlta" class="form-control" runat="server" disabled />
                                                    <span class="input-group-addon glyphicon">
                                                        <span class="icon-calendar xs"></span>
                                                    </span>
                                                </div>
                                            </div>
                                            <%--fecha baja--%>
                                            <div class="col-sm-2">
                                                <label>Fecha Baja</label>
                                                <div id="fechaBaja_form" class="input-group date js-datepicker" runat="server">
                                                    <label class="sr-only" for="txtFechaBaja">Fecha Baja</label>
                                                    <input type="text" id="txtFechaBaja" class="form-control" runat="server" />
                                                    <span class="input-group-addon glyphicon">
                                                        <span class="icon-calendar xs"></span>
                                                    </span>
                                                </div>
                                            </div>
                                            <%--activo--%>
                                            <div class="col-sm-2">
                                                <label>Activo</label>
                                                <div id="activo_form" class="form-group padding-t-10" runat="server">
                                                    <div class="checkbox">
                                                        <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                                                    </div>
                                                </div>
                                            </div>
                                            <%--descripcion--%>
                                            <div class="col-sm-12">
                                                <label>Descripción *</label>
                                                <div id="descripcion_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="txt_descripcion">Descripción *</label>
                                                    <textarea placeholder="Descripción" id="txt_descripcion" class="form-control" runat="server" rows="5" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--version--%>
                                            <div class="col-sm-2">
                                                <label>Versión *</label>
                                                <div id="version_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="version">*</label>
                                                    <input placeholder="Versión" type="number" id="version" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--numero de sesiones--%>
                                            <div class="col-sm-2">
                                                <label>Número Sesiones *</label>
                                                <div id="sesiones_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="sesiones">*</label>
                                                    <input placeholder="Número de sesiones" type="number" id="sesiones" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--numero de horas--%>
                                            <div class="col-sm-2">
                                                <label>Número de horas *</label>
                                                <div id="horas_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="horas">*</label>
                                                    <input placeholder="Número de horas" type="number" id="horas" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--duracion--%>
                                            <div class="col-sm-2">
                                                <label>Duración *</label>
                                                <div id="duracion_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="duracion">*</label>
                                                    <input placeholder="Duración" type="number" id="duracion" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--dias planificados--%>
                                            <div class="col-sm-2">
                                                <label>Días planificados *</label>
                                                <div id="dias_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="dias">*</label>
                                                    <input placeholder="Nº días planificado" type="number" id="dias" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--universitario--%>
                                            <div class="col-sm-2">
                                                <label>Universitario</label>
                                                <div id="universitario_form" class="form-group padding-t-10" runat="server">
                                                    <div class="checkbox">
                                                        <asp:CheckBox ID="chk_universitario" runat="server" Text="Universitario" />
                                                    </div>
                                                </div>
                                            </div>
                                            <%--titulo oficial--%>
                                            <div class="col-sm-12">
                                                <label>Título oficial*</label>
                                                <div id="titulo_oficial_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="txt_titulo_oficial">Título oficial *</label>
                                                    <input type="text" placeholder="Título oficial" id="titulo_oficial" class="form-control" runat="server" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--contenido oficial--%>
                                            <div class="col-sm-12">
                                                <label>Contenido oficial *</label>
                                                <div id="contenido_oficial_form" class="form-group" runat="server">
                                                    <label class="sr-only" for="txt_contenido_oficial">Descripción *</label>
                                                    <textarea placeholder="Contenido oficial" id="contenido_oficial" class="form-control" runat="server" rows="5" />
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                    <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                </div>
                                            </div>
                                            <%--tipologia--%>
                                            <div class="col-sm-4">
                                                <label>Tipología *</label>
                                                <div id="tipología_form" class="form-group" runat="server">
                                                    <select id="ddltipos" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                        <option selected="selected" value="" id="type">Seleccione una tipología</option>
                                                        <option value="7" id="type_7">Master presencial</option>
                                                        <option value="8" id="type_8">Master semipresencial</option>
                                                        <option value="9" id="type_9">Master online</option>
                                                        <option value="10" id="type_10">Master Fundamentals</option>
                                                        <option value="3" id="type_3">P. Superior</option>
                                                        <option value="6" id="type_6">Experto Universitario</option>
                                                        <option value="2" id="type_2">Programa</option>
                                                        <option value="1" id="type_1">Curso</option>
                                                        <option value="0" id="type_0">Gratuito</option>
                                                        <option value="5" id="type_5">Seminario</option>
                                                        <option value="4" id="type_4">Master original</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <%--metodologia--%>
                                            <div class="col-sm-4">
                                                <label>Metodología *</label>
                                                <div id="metodologia_form" class="form-group" runat="server">
                                                    <select id="ddlmetodologia" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                        <option value="" id="method">Seleccione una metodología</option>
                                                        <option value="1" id="method_1">Online</option>
                                                        <option value="2" id="method_2">Semi-presencial</option>
                                                        <option value="3" id="method_3">Presencial</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <%--dificultad--%>
                                            <div class="col-sm-4">
                                                <label>Dificultad *</label>
                                                <div id="dificultad_form" class="form-group" runat="server">
                                                    <select id="ddldificultad" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true">
                                                        <option selected="selected" value="" id="dificulty">Seleccione una dificultad</option>
                                                        <option value="1" id="dificulty_1">Básico</option>
                                                        <option value="2" id="dificulty_2">Medio</option>
                                                        <option value="3" id="dificulty_3">Avanzado</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <%--programapdf--%>
                                            <div class="col-sm-4">
                                                <label>Programa PDF</label>
                                                <div id="programapdf_form" class="form-group d-flex" runat="server">
                                                    <input type="file" id="programapdf" class="form-control" runat="server" onchange="loadfile('programapdf','programapdfh')" />
                                                    <button type="button" class="btn btn-danger" onclick="onremoveprograma('pdf')"><i class="fa fa-trash"></i></button>
                                                    <input type="text" id="programapdfh" hidden runat="server" />
                                                </div>
                                            </div>
                                            <%--programapdfcompleto--%>
                                            <div class="col-sm-4">
                                                <label>Programa PDF Completo</label>
                                                <div id="programapdfcompleto_form" class="form-group d-flex" runat="server">
                                                    <input type="file" id="programapdfcompleto" class="form-control" runat="server" onchange="loadfile('programapdfcompleto','programapdfcompletoh')" />
                                                    <button type="button" class="btn btn-danger" onclick="onremoveprograma('pdfcompleto')"><i class="fa fa-trash"></i></button>
                                                    <input type="text" id="programapdfcompletoh" hidden runat="server" />
                                                </div>
                                            </div>
                                            <%--programapdfweb--%>
                                            <div class="col-sm-4">
                                                <label>Programa PDF Web</label>
                                                <div id="programapdfweb_form" class="form-group d-flex" runat="server">
                                                    <input type="file" id="programapdfweb" class="form-control" runat="server" onchange="loadfile('programapdfweb','programapdfwebh')" />
                                                    <button type="button" class="btn btn-danger" onclick="onremoveprograma('web')"><i class="fa fa-trash"></i></button>
                                                    <input type="text" id="programapdfwebh" hidden runat="server" />
                                                </div>
                                            </div>
                                            <%--autores--%>
                                            <div class="col-sm-8" id="autoresmanagement">
                                                <div class="col-sm-8" style="padding-left: 0px">
                                                    <label>Autor</label>
                                                </div>
                                                <div class="col-sm-3">
                                                    <label>% de autoría</label>
                                                </div>
                                                <div class="col-sm-8" style="padding-left: 0px">
                                                    <div id="autor_form" class="form-group" runat="server">
                                                        <select id="ddlautor" runat="server" class="selectpicker" data-live-search="true" data-hide-disabled="true"></select>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div id="autorpct" class="form-group" runat="server">
                                                        <input type="number" id="newautorpercent" class="form-control" min="1" max="100" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-1">
                                                    <button type="button" class="btn btn-success" onclick="addautor()"><i class="far fa-plus-square"></i></button>
                                                </div>
                                                <div class="col-sm-12 badgecontainer" id="autorescontainers" runat="server">
                                                </div>
                                            </div>
                                            <%--publicar--%>
                                            <div class="col-sm-4">
                                                <label>Publicar</label>
                                                <div id="publicar_form" class="form-group padding-t-10" runat="server">
                                                    <div class="checkbox">
                                                        <asp:CheckBox ID="chk_publicar" runat="server" Text="Publicar" />
                                                    </div>
                                                </div>
                                            </div>
                                            <%--area funcional--%>
                                            <div class="col-sm-6 card-body">
                                                <div class="">
                                                    <label>Área funcional *</label>
                                                </div>
                                                <div class="badgecontainer" id="areascontainer" runat="server">
                                                </div>
                                            </div>
                                            <%--tematica--%>
                                            <div class="col-sm-6 card-body">
                                                <div class="">
                                                    <label>Temática *</label>
                                                </div>
                                                <div class="badgecontainer" id="tematicascontainer" runat="server">
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <a href="cursos.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                                <button class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" type="button" onclick="guardarcurso()">Guardar curso</button>
                                            </div>
                                        </div>
                                        <div id="literales" class="tab-pane fade padding10">
                                            <div id="literalesalert" class="alert text-center" role="alert" runat="server">
                                            </div>
                                            <div id='literalescontainer' class='col-sm-12' runat='server'>
                                            </div>
                                            <div class="col-sm-12" id="literalesbtns">
                                                <a href="cursos.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                                <button class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" type="button" onclick="guardarliterales()">Guardar literales</button>
                                            </div>
                                        </div>
                                        <div id="contenidos" class="tab-pane fade padding10">
                                            <div id="contenidosalert" class="alert text-center" role="alert" runat="server">
                                            </div>
                                            <div id="addcontent" style="display: none" runat="server">
                                                <div class="col-sm-12">
                                                    <div class="col-sm-12">
                                                        <span style="font-size: 24px">Añadir recurso existente</span>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <label>Sesión *</label>
                                                        <div id="addcontentsesion_form" class="form-group" runat="server">
                                                            <label class="sr-only" for="addcontentsesion">*</label>
                                                            <input placeholder="Sesión" type="number" id="addcontentsesion" readonly class="form-control" runat="server" />
                                                            <span aria-hidden="true" class="glyphicon glyphicon-ko xs form-control-feedback"></span>
                                                            <span aria-hidden="true" class="glyphicon glyphicon-ok xs form-control-feedback"></span>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <label>Lectura</label>
                                                        <div id="addcontentlectura_form" class="form-group padding-t-10" runat="server">
                                                            <div class="checkbox">
                                                                <asp:CheckBox ID="addcontentlectura" runat="server" Text="Lectura" />
                                                            </div>
                                                        </div>
                                                    </div>                                                    
                                                    <div id="addcontentresources" class="col-sm-12"></div>
                                                    <div class="col-sm-12">
                                                        <button class="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" type="button" onclick="addrecursos()">Añadir recurso</button>
                                                        <button class="btn btn-primary bg-color-danger text-color-white btn-block-xs margin-b-15" type="button" onclick="cancelaraddrecurso()">Cancelar</button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="contenidoscontainer" runat="server">
                                            </div>
                                            <div class="col-sm-12" id="contenidobtns">
                                                <a href="cursos.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>

                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <%--<a href="cursos.aspx" class="btn btn-primary btn-block-xs margin-xs-b-15">Volver</a>--%>
                                <%--<asp:Button ID="btnGuardar" CssClass="btn btn-primary bg-color-success text-color-white btn-block-xs margin-b-15 pull-right" runat="server" Text="Guardar" OnClientClick="return validarFormulario();" OnClick="btnGuardar_Click" />--%>
                            </div>
                        </div>
                        <asp:ScriptManager ID="sm" EnablePageMethods="true" runat="server" />
                    </form>

                </div>
            </div>
        </section>
    </main>
    <!-- Scripts=================================================== -->
    <script type="text/javascript" src="/App_Themes/support/js/jquery.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/autosize.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/datatables.min.js"></script>

    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.iframe-transport.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/upload/jquery.fileupload.js"></script>
    <%--<script src="App_Themes/support/js/bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="/App_Themes/support/js/internal/curso_functions.js"></script>




</body>
</html>


