<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_facturas_list.aspx.cs" Inherits="campus_sbs_admin.admin_facturas_list" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Consulta de Facturas</title>

    <!-- CSS 
    =================================================== -->
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>        
        <%: Styles.Render("~/bundles/jquery_ui_css") %>
        <%: Styles.Render("~/bundles/datatables_css") %>
    </asp:PlaceHolder>

    <!-- DataTables Buttons -->
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" />

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
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <uc_menu:menu ID="menu" runat="server" />
        <header id="header" class="bg-color-primary affix">        
            <uc_header:cabecera ID="cabecera" runat="server" />
        </header>

        <section class="wrapper">
            <div class="padding-nav">

                <!-- ===========================
                     SECCIÓN: FILTROS DE BÚSQUEDA
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-search"></i> Búsqueda de Facturas
                            <a href='admin_facturas.aspx' title='Registrar factura' class='pull-right bold padding-r-5'>
                                <small class='text-color-primary'><i class='fas fa-plus'></i> Registrar factura</small>
                            </a>
                        </legend>                    
                    </fieldset>

                    <!-- Fila 1: Sociedad, Desde, Hasta, Año -->
                    <div class="col-12 pt-2">
                        <div class="col-3">
                            <label>Sociedad</label>
                            <div class="form-group">
                                <label class="sr-only" for="fSociedad">Sociedad</label>
                                <select id="fSociedad" class="form-control" title="Sociedad">
                                    <option value="">Todas</option>
                                    <option value="SBS">SBS</option>
                                    <option value="SBSCS">SBSCS</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Desde</label>
                            <div class="form-group">
                                <label class="sr-only" for="fDesde">Desde</label>
                                <input type="text" id="fDesde" class="form-control" placeholder="01/01/2026" autocomplete="off" title="Fecha desde" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Hasta</label>
                            <div class="form-group">
                                <label class="sr-only" for="fHasta">Hasta</label>
                                <input type="text" id="fHasta" class="form-control" placeholder="31/12/2026" autocomplete="off" title="Fecha hasta" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Año Fiscal</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlyear">Año Fiscal</label>
                                <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-control" title="Año Fiscal"></asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <!-- Fila 2: Checkboxes -->
                    <div class="col-12 pt-2">
                        <div class="col-3">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkPendCobro" />
                                    <label class="custom-control-label" for="chkPendCobro">Pendiente cobro</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-3">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkPendAtrib" />
                                    <label class="custom-control-label" for="chkPendAtrib">Pendiente atribución</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-3">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkPendVenc" />
                                    <label class="custom-control-label" for="chkPendVenc">Pendiente vencimiento</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-3">
                            <div class="form-group">
                                <a href="javascript:void(0);" id="btnBuscar" title="Buscar"><i class="fas fa-search fa-2x text-color-primary"></i></a>
                                <a href="javascript:void(0);" id="btnLimpiar" title="Limpiar filtros" class="ml-3"><i class="fas fa-undo fa-2x text-color-primary"></i></a>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     SECCIÓN: LISTADO DE FACTURAS
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="far fa-list-alt"></i> Listado de Facturas
                        </legend>
                    </fieldset>

                    <div id="table_list_facturas" class="col-sm-12 padding-tb-20">
                        <table id="tblFacturas" class="table table-striped table-bordered" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>No.</th>
                                    <th>Sociedad</th>
                                    <th>Descripción</th>
                                    <th>Atribución</th>
                                    <th>Precio</th>
                                    <th>Fundación</th>
                                    <th>Universidad</th>
                                    <th>Tripartita</th>
                                    <th>IVA</th>
                                    <th>IRPF</th>
                                    <th>Total</th>
                                    <th>F. Venc.</th>
                                    <th>F. Cobro</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>

            </div>       
        </section>

        <!-- Modal: Cargando -->
        <div class="modal fade" id="wait_modal" tabindex="-1" role="dialog" aria-labelledby="wait_modal" aria-hidden="true" data-backdrop="static" data-keyboard="false">
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

        <!-- Modal: Confirmar eliminar -->
        <div class="modal fade" id="confirm_modal" tabindex="-1" role="dialog" aria-labelledby="confirm_modal" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-color-primary">Confirmar eliminación</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        ¿Seguro que deseas eliminar la factura? (Se eliminará también el archivo adjunto si existe)
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-danger" id="btnConfirmDelete">
                            <i class="fas fa-trash"></i> Sí, eliminar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Mensajes -->
        <div class="modal fade" id="info_modal" tabindex="-1" role="dialog" aria-labelledby="info_modal" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-color-primary" id="info_modal_title">Mensaje</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" id="info_modal_body"></div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Aceptar</button>
                    </div>
                </div>
            </div>
        </div>

    </form>

    <!-- Scripts
    =================================================== --> 
    <asp:PlaceHolder runat="server">        
        <%: Scripts.Render("~/bundles/general_admin_js") %>
        <%: Scripts.Render("~/bundles/jquery_ui_js") %>
        <%: Scripts.Render("~/bundles/menu_nav_js") %>
        <%: Scripts.Render("~/bundles/bootstrap_bundle_js") %>
        <%: Scripts.Render("~/bundles/datatables_js") %>
    </asp:PlaceHolder>

    <!-- DataTables Buttons -->
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>

    <script type="text/javascript">
        var dt = null;
        var deleteId = null;

        // ===========================
        // Helpers UI (Modal)
        // ===========================
        function showLoading() {
            if ($.fn.modal) $('#wait_modal').modal('show');
        }

        function hideLoading() {
            if ($.fn.modal) $('#wait_modal').modal('hide');
        }

        function showInfo(title, msg) {
            $('#info_modal_title').text(title || 'Mensaje');
            $('#info_modal_body').html(msg || '');
            if ($.fn.modal) $('#info_modal').modal('show');
            else alert((title ? title + ': ' : '') + msg);
        }

        function openConfirmDelete(id) {
            deleteId = id;
            $('#confirm_modal').modal('show');
        }

        // ===========================
        // Construcción de acciones
        // ===========================
        function buildActions(row) {
            var id = row.idInfFinFacturas;
            var html = '<div class="text-center">';
            html += '<a class="btn btn-sm btn-outline-primary" title="Ver/Editar" href="admin_facturas_edit.aspx?id=' + id + '">';
            html += '<i class="fas fa-eye"></i>';
            html += '</a>';
            html += ' <a href="javascript:void(0);" class="btn btn-sm btn-outline-danger btn-del" data-id="' + id + '" title="Eliminar">';
            html += '<i class="fas fa-trash"></i>';
            html += '</a>';
            html += '</div>';
            return html;
        }

        // ===========================
        // Obtener filtros
        // ===========================
        function getFiltros() {
            return {
                sociedad: $('#fSociedad').val() || null,
                desde: $('#fDesde').val() || null,
                hasta: $('#fHasta').val() || null,
                year: $('#<%= ddlyear.ClientID %>').val() || null,
                pendCobro: $('#chkPendCobro').is(':checked'),
                pendAtrib: $('#chkPendAtrib').is(':checked'),
                pendVenc: $('#chkPendVenc').is(':checked')
            };
        }

        // ===========================
        // Cargar tabla
        // ===========================
        function loadTable() {
            var filtros = getFiltros();
            showLoading();

            $.ajax({
                url: 'admin_facturas_list.aspx/GetFacturas',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ f: filtros }),
                success: function (res) {
                    hideLoading();
                    var payload = (res && res.d) ? res.d : null;

                    if (payload && payload.ok === false) {
                        console.error(payload);
                        return showInfo('Error', payload.message || 'No se pudo cargar el listado.');
                    }

                    var rows = Array.isArray(payload) ? payload : [];
                    renderDataTable(rows);
                },
                error: function (xhr) {
                    hideLoading();
                    console.error(xhr);
                    showInfo('Error', 'No se pudo cargar el listado.');
                }
            });
        }

        // ===========================
        // Renderizar DataTable
        // ===========================
        function renderDataTable(rows) {
            if (dt) {
                dt.clear().destroy();
                $('#tblFacturas tbody').empty();
            }

            dt = $('#tblFacturas').DataTable({
                data: rows,
                columns: [
                    {
                        data: null,
                        title: 'No.',
                        orderable: false,
                        searchable: false,
                        width: '45px',
                        className: 'text-center',
                        render: function () { return ''; }
                    },
                    { data: 'sociedad', title: 'Sociedad' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'atribucion', title: 'Atribución' },
                    { data: 'eur_precio_str', title: 'Precio', className: 'text-right' },
                    { data: 'eur_fundacion_str', title: 'Fundación', className: 'text-right' },
                    { data: 'eur_universidad_str', title: 'Universidad', className: 'text-right' },
                    { data: 'eur_tripartita_str', title: 'Tripartita', className: 'text-right' },
                    { data: 'eur_iva_str', title: 'IVA', className: 'text-right' },
                    { data: 'eur_irpf_str', title: 'IRPF', className: 'text-right' },
                    { data: 'eur_total_str', title: 'Total', className: 'text-right bold' },
                    { data: 'fecha_vencimiento_str', title: 'F. Venc.', className: 'text-nowrap' },
                    { data: 'fecha_cobro_str', title: 'F. Cobro', className: 'text-nowrap' },
                    {
                        data: null,
                        title: 'Acciones',
                        orderable: false,
                        searchable: false,
                        className: 'text-nowrap',
                        render: function (data, type, row) { return buildActions(row); }
                    }
                ],
                pageLength: 10,
                order: [],
                dom: 'Bfrtip',
                buttons: [
                    { extend: 'excelHtml5', title: 'Facturas', exportOptions: { columns: ':visible:not(:last-child)' } },
                    { extend: 'pdfHtml5', title: 'Facturas', exportOptions: { columns: ':visible:not(:last-child)' } },
                    { extend: 'print', title: 'Facturas', exportOptions: { columns: ':visible:not(:last-child)' } }
                ],
                autoWidth: false,
                responsive: true,
                deferRender: true,
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                },
                drawCallback: function () {
                    var api = this.api();
                    var start = api.page.info().start;

                    api.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                        cell.innerHTML = start + i + 1;
                    });
                }
            });
        }

        // ===========================
        // Eliminar factura
        // ===========================
        function doDeleteFactura(id) {
            showLoading();

            $.ajax({
                url: 'admin_facturas_list.aspx/DeleteFactura',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ id: Number(id) }),
                success: function (res) {
                    hideLoading();
                    var rr = (res && res.d) ? res.d : null;

                    if (!rr || rr.ok === false) {
                        return showInfo('Error', (rr && rr.message) ? rr.message : 'No se pudo eliminar.');
                    }

                    showInfo('Éxito', rr.message || 'Factura eliminada correctamente.');
                    loadTable();
                },
                error: function (xhr) {
                    hideLoading();
                    console.error(xhr);
                    showInfo('Error', 'No se pudo eliminar.');
                }
            });
        }

        // ===========================
        // Eventos
        // ===========================
        $(document).on('click', '.btn-del', function (e) {
            e.preventDefault();
            e.stopPropagation();
            openConfirmDelete($(this).data('id'));
        });

        $('#btnConfirmDelete').on('click', function () {
            $('#confirm_modal').modal('hide');
            if (deleteId) doDeleteFactura(deleteId);
        });

        $('#btnBuscar').on('click', function (e) {
            e.preventDefault();
            loadTable();
        });

        $('#btnLimpiar').on('click', function (e) {
            e.preventDefault();
            $('#fSociedad').val('');
            $('#fDesde').val('');
            $('#fHasta').val('');
            $('#chkPendCobro').prop('checked', false);
            $('#chkPendAtrib').prop('checked', false);
            $('#chkPendVenc').prop('checked', false);

            var currentYear = new Date().getFullYear();
            $('#<%= ddlyear.ClientID %>').val(String(currentYear));
            $('#fDesde').val('01/01/' + currentYear);
            $('#fHasta').val('31/12/' + currentYear);

            loadTable();
        });

        $('#<%= ddlyear.ClientID %>').on('change', function () {
            var y = $(this).val();
            if (!y) return;
            $('#fDesde').val('01/01/' + y);
            $('#fHasta').val('31/12/' + y);
        });

        // ===========================
        // Inicialización
        // ===========================
        function initDatepickers() {
            if (!$.fn.datepicker) return;

            $('#fDesde, #fHasta').datepicker({
                format: 'dd/mm/yyyy',
                language: 'es',
                autoclose: true,
                todayHighlight: true
            });
        }

        $(document).ready(function () {
            initDatepickers();

            var y = $('#<%= ddlyear.ClientID %>').val();
            if (y) {
                $('#fDesde').val('01/01/' + y);
                $('#fHasta').val('31/12/' + y);
            }

            loadTable();
        });
    </script>
</body>
</html>