<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Roles_Permisos.aspx.cs" Inherits="campus_sbs_admin.Roles_Permisos" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Roles y Permisos</title>

    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>        
        <%: Styles.Render("~/bundles/jquery_ui_css") %>
    </asp:PlaceHolder>

    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>

    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js" async></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js" async></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <uc_menu:menu ID="menu" runat="server" />
        <header id="header" class="bg-color-primary affix">        
            <uc_header:cabecera ID="cabecera" runat="server" />
        </header>

        <section class="wrapper">
            <div class="padding-nav">

                <!-- CABECERA -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-user-shield"></i> Roles y Permisos
                        </legend>
                    </fieldset>
                </div>

                <!-- TABS -->
                <div class="col pt-2">
                    <ul class="nav nav-tabs" id="tabRolesPermisos" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active bold text-color-primary" id="tab-roles" data-toggle="tab" href="#pnl_roles" role="tab">
                                <i class="fas fa-id-badge"></i> Roles
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link bold text-color-primary" id="tab-asignacion" data-toggle="tab" href="#pnl_asignacion" role="tab">
                                <i class="fas fa-users-cog"></i> Asignación
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link bold text-color-primary" id="tab-permisos" data-toggle="tab" href="#pnl_permisos" role="tab">
                                <i class="fas fa-key"></i> Permisos
                            </a>
                        </li>
                    </ul>

                    <div class="tab-content pt-3" id="tabRolesPermisosContent">

                        <!-- ===========================
                             TAB 1: ROLES
                             =========================== -->
                        <div class="tab-pane fade show active" id="pnl_roles" role="tabpanel">
                            <fieldset>
                                <legend class="text-color-primary">
                                    <i class="fas fa-id-badge"></i> Gestión de Roles
                                    <a href="javascript:void(0);" id="btnNuevoRol" class="pull-right bold padding-r-5">
                                        <small class="text-color-primary"><i class="fas fa-plus"></i> Nuevo Rol</small>
                                    </a>
                                </legend>
                            </fieldset>

                            <!-- Formulario inline nuevo/editar rol -->
                            <div id="pnlFormRol" class="col-12 pt-2" style="display:none;">
                                <div class="col-12 pt-2">
                                    <div class="col-3">
                                        <label>Nombre *</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txtRolNombre">Nombre</label>
                                            <input type="text" id="txtRolNombre" class="form-control" placeholder="Nombre del rol" title="Nombre" maxlength="100" />
                                        </div>
                                    </div>
                                    <div class="col-5">
                                        <label>Descripción</label>
                                        <div class="form-group">
                                            <label class="sr-only" for="txtRolDesc">Descripción</label>
                                            <input type="text" id="txtRolDesc" class="form-control" placeholder="Descripción del rol" title="Descripción" maxlength="250" />
                                        </div>
                                    </div>
                                    <div class="col-2">
                                        <label>&nbsp;</label>
                                        <div class="form-group">
                                            <div class="custom-control custom-checkbox" style="margin-top:8px;">
                                                <input type="checkbox" class="custom-control-input" id="chkRolActivo" checked="checked" />
                                                <label class="custom-control-label" for="chkRolActivo">Activo</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-2">
                                        <label>&nbsp;</label>
                                        <div class="form-group">
                                            <button type="button" id="btnGuardarRol" class="btn btn-success btn-sm">
                                                <i class="fas fa-save"></i> Guardar
                                            </button>
                                            <button type="button" id="btnCancelarRol" class="btn btn-secondary btn-sm ml-1">
                                                <i class="fas fa-times"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <input type="hidden" id="hdnRolId" value="0" />
                            </div>

                            <!-- Búsqueda + Tabla roles -->
                            <div class="col-sm-12 padding-tb-20">
                                <div class="col-12 pt-2 pb-2">
                                    <div class="col-4 px-0">
                                        <div class="form-group">
                                            <label class="sr-only" for="searchRoles">Buscar</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fas fa-search"></i></span>
                                                </div>
                                                <input type="text" id="searchRoles" class="form-control" placeholder="Buscar rol..." title="Buscar rol" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <table id="tblRoles" class="table table-striped table-bordered" style="width:100%">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Nombre</th>
                                            <th>Descripción</th>
                                            <th>Activo</th>
                                            <th>Usuarios</th>
                                            <th>Acciones</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>

                        <!-- ===========================
                             TAB 2: ASIGNACIÓN
                             =========================== -->
                        <div class="tab-pane fade" id="pnl_asignacion" role="tabpanel">
                            <fieldset>
                                <legend class="text-color-primary">
                                    <i class="fas fa-users-cog"></i> Asignación de Roles a Usuarios
                                </legend>
                            </fieldset>

                            <div class="col-sm-12 padding-tb-20">
                                <div class="col-12 pt-2 pb-2">
                                    <div class="col-4 px-0">
                                        <div class="form-group">
                                            <label class="sr-only" for="searchUsuarios">Buscar</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fas fa-search"></i></span>
                                                </div>
                                                <input type="text" id="searchUsuarios" class="form-control" placeholder="Buscar usuario..." title="Buscar usuario" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <table id="tblUsuarios" class="table table-striped table-bordered" style="width:100%">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Nombre</th>
                                            <th>Email</th>
                                            <th>Rol Actual</th>
                                            <th>Cambiar Rol</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>

                        <!-- ===========================
                             TAB 3: PERMISOS
                             =========================== -->
                        <div class="tab-pane fade" id="pnl_permisos" role="tabpanel">
                            <fieldset>
                                <legend class="text-color-primary">
                                    <i class="fas fa-key"></i> Permisos por Rol
                                </legend>
                            </fieldset>

                            <!-- Selector de rol -->
                            <div class="col-12 pt-2">
                                <div class="col-4">
                                    <label>Seleccionar Rol</label>
                                    <div class="form-group">
                                        <label class="sr-only" for="ddlRolPermisos">Rol</label>
                                        <select id="ddlRolPermisos" class="form-control" title="Rol"></select>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <label>&nbsp;</label>
                                    <div class="form-group">
                                        <button type="button" id="btnGuardarPermisos" class="btn btn-success btn-sm" style="display:none;">
                                            <i class="fas fa-save"></i> Guardar Permisos
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <!-- Búsqueda + Tabla permisos -->
                            <div class="col-sm-12 padding-tb-20">
                                <div class="col-12 pt-2 pb-2">
                                    <div class="col-4 px-0">
                                        <div class="form-group">
                                            <label class="sr-only" for="searchPermisos">Buscar</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fas fa-search"></i></span>
                                                </div>
                                                <input type="text" id="searchPermisos" class="form-control" placeholder="Buscar módulo..." title="Buscar módulo" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <table id="tblPermisos" class="table table-striped table-bordered" style="width:100%">
                                    <thead>
                                        <tr>
                                            <th>Módulo</th>
                                            <th class="text-center">Ver</th>
                                            <%--<th class="text-center">Crear</th>--%>
                                            <th class="text-center">Editar</th>
                                            <th class="text-center">Eliminar</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tblPermisosBody"></tbody>
                                </table>
                            </div>
                        </div>

                    </div>
                </div>

            </div>       
        </section>

        <!-- Modal: Cargando -->
        <div class="modal fade" id="wait_modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body text-center">
                        <i class="fas fa-spinner fa-pulse fa-5x text-color-primary"></i>
                        <p class="mt-3 bold">Cargando...</p>
                        <small class="text-muted">Por favor espere</small>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Confirmar -->
        <div class="modal fade" id="confirm_modal" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-color-primary">Confirmar</h5>
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                    </div>
                    <div class="modal-body" id="confirm_modal_body">¿Está seguro?</div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-danger" id="btnConfirmAction">
                            <i class="fas fa-check"></i> Confirmar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Info -->
        <div class="modal fade" id="info_modal" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-color-primary" id="info_modal_title">Mensaje</h5>
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                    </div>
                    <div class="modal-body" id="info_modal_body"></div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Aceptar</button>
                    </div>
                </div>
            </div>
        </div>

    </form>

    <!-- Scripts -->
    <asp:PlaceHolder runat="server">        
        <%: Scripts.Render("~/bundles/general_admin_js") %>
        <%: Scripts.Render("~/bundles/jquery_ui_js") %>
        <%: Scripts.Render("~/bundles/menu_nav_js") %>
        <%: Scripts.Render("~/bundles/bootstrap_bundle_js") %>
    </asp:PlaceHolder>

    <script type="text/javascript">
        var confirmCallback = null;
        var rolesCache = [];

        // ===========================
        // Helpers
        // ===========================
        function showLoading() { if ($.fn.modal) $('#wait_modal').modal('show'); }
        function hideLoading() { if ($.fn.modal) $('#wait_modal').modal('hide'); }

        function showInfo(title, html) {
            $('#info_modal_title').text(title || 'Mensaje');
            $('#info_modal_body').html(html || '');
            if ($.fn.modal) $('#info_modal').modal('show');
        }

        function showConfirm(msg, callback) {
            $('#confirm_modal_body').html(msg);
            confirmCallback = callback;
            $('#confirm_modal').modal('show');
        }

        function ajaxPost(url, data, onOk, onErr) {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(data),
                success: function (res) {
                    var d = (res && res.d) ? res.d : null;
                    if (d && d.ok === false) {
                        if (onErr) onErr(d.message || 'Error');
                        else showInfo('Error', d.message || 'Error');
                        return;
                    }
                    if (onOk) onOk(d);
                },
                error: function (xhr) {
                    console.error(xhr);
                    if (onErr) onErr('Error de comunicación.');
                    else showInfo('Error', 'Error de comunicación.');
                }
            });
        }

        // ===========================
        // Búsqueda en tablas
        // ===========================
        function bindTableSearch(inputId, tableId) {
            $('#' + inputId).on('keyup', function () {
                var term = $(this).val().toLowerCase();
                $('#' + tableId + ' tbody tr').each(function () {
                    var texto = $(this).text().toLowerCase();
                    if (texto.indexOf(term) >= 0) {
                        $(this).show();
                    } else {
                        $(this).hide();
                    }
                });
            });
        }

        // ===========================
        // TAB 1: ROLES
        // ===========================
        function loadRoles() {
            ajaxPost('Roles_Permisos.aspx/GetRoles', {}, function (data) {
                rolesCache = data || [];
                renderRolesTable(rolesCache);
                fillRolDropdown(rolesCache);
            });
        }

        function renderRolesTable(rows) {
            var $tbody = $('#tblRoles tbody');
            $tbody.empty();

            for (var i = 0; i < rows.length; i++) {
                var r = rows[i];
                var activo = r.Activo
                    ? '<span class="bold text-color-primary">Sí</span>'
                    : '<span class="text-muted">No</span>';

                var acciones = '<div class="text-center">'
                    + '<a href="javascript:void(0);" class="btn btn-sm btn-outline-primary btn-edit-rol" data-id="' + r.Id + '" title="Editar">'
                    + '<i class="fas fa-edit"></i>'
                    + '</a> '
                    + '<a href="javascript:void(0);" class="btn btn-sm btn-outline-danger btn-del-rol" data-id="' + r.Id + '" title="Eliminar">'
                    + '<i class="fas fa-trash"></i>'
                    + '</a>'
                    + '</div>';

                var tr = '<tr>'
                    + '<td class="text-center">' + r.Id + '</td>'
                    + '<td class="bold">' + r.Nombre + '</td>'
                    + '<td>' + r.Descripcion + '</td>'
                    + '<td class="text-center">' + activo + '</td>'
                    + '<td class="text-center">' + r.CantidadUsuario + '</td>'
                    + '<td>' + acciones + '</td>'
                    + '</tr>';

                $tbody.append(tr);
            }
        }

        function resetFormRol() {
            $('#hdnRolId').val('0');
            $('#txtRolNombre').val('');
            $('#txtRolDesc').val('');
            $('#chkRolActivo').prop('checked', true);
            $('#pnlFormRol').hide();
        }

        function saveRol() {
            var nombre = $('#txtRolNombre').val().trim();
            var desc = $('#txtRolDesc').val().trim();
            var activo = $('#chkRolActivo').is(':checked');
            var id = Number($('#hdnRolId').val() || 0);

            if (!nombre || nombre.length < 2) {
                return showInfo('Atención', 'El nombre del rol es obligatorio.');
            }

            showLoading();
            ajaxPost('Roles_Permisos.aspx/SaveRol',
                { id: id, nombre: nombre, descripcion: desc, activo: activo },
                function (d) {
                    hideLoading();
                    showInfo('Éxito', d.message || 'Guardado.');
                    resetFormRol();
                    loadRoles();
                },
                function (msg) {
                    hideLoading();
                    showInfo('Error', msg);
                }
            );
        }

        function deleteRol(id) {
            showConfirm('¿Seguro que deseas eliminar este rol?', function () {
                showLoading();
                ajaxPost('Roles_Permisos.aspx/DeleteRol', { Id: id },
                    function (d) {
                        hideLoading();
                        showInfo('Éxito', d.message || 'Eliminado.');
                        loadRoles();
                    },
                    function (msg) { hideLoading(); showInfo('Error', msg); }
                );
            });
        }

        // ===========================
        // TAB 2: ASIGNACIÓN
        // ===========================
        function loadUsuarios() {
            ajaxPost('Roles_Permisos.aspx/GetUsuarios', {}, function (data) {
                renderUsuariosTable(data || []);
            });
        }

        function renderUsuariosTable(rows) {
            var $tbody = $('#tblUsuarios tbody');
            $tbody.empty();

            for (var i = 0; i < rows.length; i++) {
                var u = rows[i];

                var options = '<option value="0">Sin asignar</option>';
                for (var j = 0; j < rolesCache.length; j++) {
                    var rc = rolesCache[j];
                    var sel = (rc.Id === u.rolId) ? ' selected' : '';
                    options += '<option value="' + rc.Id + '"' + sel + '>' + rc.Nombre + '</option>';
                }

                var select = '<select class="form-control ddl-asignar" data-uid="' + u.id + '" title="Rol">'
                    + options + '</select>';

                var rolBadge = u.rolId > 0
                    ? '<span class="bold text-color-primary">' + u.rol + '</span>'
                    : '<span class="text-muted">Sin asignar</span>';

                var tr = '<tr>'
                    + '<td class="text-center">' + u.id + '</td>'
                    + '<td class="bold">' + u.nombre + '</td>'
                    + '<td>' + u.email + '</td>'
                    + '<td class="text-center">' + rolBadge + '</td>'
                    + '<td>' + select + '</td>'
                    + '</tr>';

                $tbody.append(tr);
            }
        }

        function asignarRol(uid, rolId) {
            showLoading();
            ajaxPost('Roles_Permisos.aspx/AsignarRol',
                { usuarioId: uid, rolId: rolId },
                function (d) {
                    hideLoading();
                    showInfo('Éxito', d.message || 'Rol asignado.');
                    loadUsuarios();
                },
                function (msg) { hideLoading(); showInfo('Error', msg); }
            );
        }

        // ===========================
        // TAB 3: PERMISOS
        // ===========================
        function fillRolDropdown(roles) {
            var $ddl = $('#ddlRolPermisos');
            $ddl.empty();
            $ddl.append('<option value="">Seleccione un rol</option>');

            for (var i = 0; i < roles.length; i++) {
                $ddl.append('<option value="' + roles[i].Id + '">' + roles[i].Nombre + '</option>');
            }
        }

        function loadPermisos(rolId) {
            if (!rolId) {
                $('#tblPermisosBody').empty();
                $('#btnGuardarPermisos').hide();
                return;
            }

            ajaxPost('Roles_Permisos.aspx/GetPermisos', { rolId: Number(rolId) }, function (data) {
                renderPermisosTable(data || []);
                $('#btnGuardarPermisos').show();
            });
        }

        function renderPermisosTable(rows) {
            var $tbody = $('#tblPermisosBody');
            $tbody.empty();

            for (var i = 0; i < rows.length; i++) {
                var p = rows[i];

                var tr = '<tr data-pid="' + p.id + '">'
                    + '<td class="bold">' + p.modulo + '</td>'
                    + '<td class="text-center"><input type="checkbox" class="perm-chk" data-col="ver"' + (p.ver ? ' checked' : '') + ' /></td>'
                   /* + '<td class="text-center"><input type="checkbox" class="perm-chk" data-col="crear"' + (p.crear ? ' checked' : '') + ' /></td>'*/
                    + '<td class="text-center"><input type="checkbox" class="perm-chk" data-col="editar"' + (p.editar ? ' checked' : '') + ' /></td>'
                    + '<td class="text-center"><input type="checkbox" class="perm-chk" data-col="eliminar"' + (p.eliminar ? ' checked' : '') + ' /></td>'
                    + '</tr>';

                $tbody.append(tr);
            }
        }

        function savePermisos() {
            var rolId = Number($('#ddlRolPermisos').val() || 0);
            if (!rolId) return showInfo('Atención', 'Seleccione un rol primero.');

            var permisos = [];
            $('#tblPermisosBody tr').each(function () {
                var $tr = $(this);
                permisos.push({
                    id: Number($tr.attr('data-pid')),
                    ver: $tr.find('[data-col="ver"]').is(':checked'),
                 /*   crear: $tr.find('[data-col="crear"]').is(':checked'),*/
                    editar: $tr.find('[data-col="editar"]').is(':checked'),
                    eliminar: $tr.find('[data-col="eliminar"]').is(':checked')
                });
            });

            showLoading();
            ajaxPost('Roles_Permisos.aspx/SavePermisos',
                { rolId: rolId, permisos: permisos },
                function (d) {
                    hideLoading();
                    showInfo('Éxito', d.message || 'Permisos guardados.');
                },
                function (msg) { hideLoading(); showInfo('Error', msg); }
            );
        }

        // ===========================
        // EVENTOS
        // ===========================
        $(document).ready(function () {

            // --- BÚSQUEDA EN TABLAS ---
            bindTableSearch('searchRoles', 'tblRoles');
            bindTableSearch('searchUsuarios', 'tblUsuarios');
            bindTableSearch('searchPermisos', 'tblPermisos');

            // --- ROLES ---
            $('#btnNuevoRol').on('click', function () {
                resetFormRol();
                $('#pnlFormRol').show();
                $('#txtRolNombre').focus();
            });

            $('#btnCancelarRol').on('click', resetFormRol);
            $('#btnGuardarRol').on('click', saveRol);

            $(document).on('click', '.btn-edit-rol', function () {
                var id = Number($(this).data('id'));
                var rol = null;
                for (var i = 0; i < rolesCache.length; i++) {
                    if (rolesCache[i].Id === id) { rol = rolesCache[i]; break; }
                }
                if (!rol) return;

                $('#hdnRolId').val(rol.Id);
                $('#txtRolNombre').val(rol.Nombre);
                $('#txtRolDesc').val(rol.Descripcion);
                $('#chkRolActivo').prop('checked', rol.Activo);
                $('#pnlFormRol').show();
                $('#txtRolNombre').focus();
            });

            $(document).on('click', '.btn-del-rol', function () {
                deleteRol(Number($(this).data('id')));
            });

            // --- ASIGNACIÓN ---
            $(document).on('change', '.ddl-asignar', function () {
                var uid = Number($(this).data('uid'));
                var rolId = Number($(this).val());
                asignarRol(uid, rolId);
            });

            // --- PERMISOS ---
            $('#ddlRolPermisos').on('change', function () {
                loadPermisos($(this).val());
            });

            $('#btnGuardarPermisos').on('click', savePermisos);

            // --- CONFIRM MODAL ---
            $('#btnConfirmAction').on('click', function () {
                $('#confirm_modal').modal('hide');
                if (typeof confirmCallback === 'function') confirmCallback();
                confirmCallback = null;
            });

            // --- TAB CHANGE ---
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var target = $(e.target).attr('href');
                if (target === '#pnl_asignacion') loadUsuarios();
                if (target === '#pnl_permisos') loadPermisos($('#ddlRolPermisos').val());
            });

            // --- INIT ---
            loadRoles();
        });
    </script>
</body>
</html>