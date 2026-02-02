<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_facturas_list.aspx.cs" Inherits="campus_sbs_admin.admin_facturas_list" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>
<html class="no-legacy-ie no-js" lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Facturas</title>

    <!-- Bootstrap / estilos base -->
    <link rel="stylesheet" href="/App_Themes/support/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/sbs.css" />

    <!-- Datepicker -->
    <link href="/App_Themes/support/css/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />

    <!-- DataTables (local) -->
    <link href="App_Themes/DataTable/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="App_Themes/DataTable/css/dataTables.bootstrap.min.css" rel="stylesheet" />

    <!-- Buttons (CDN) -->
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" />

    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js"></script>

    <style>
        /* Layout para no meterse debajo del sidebar */
        .content-wrapper {
            margin-left: 260px;
            padding: 20px;
        }

        @media (max-width: 768px) {
            .content-wrapper {
                margin-left: 0;
                padding: 15px;
            }
        }

        /* Card cómodo */
        .card {
            border: none;
        }

        .card-body {
            padding: 1.25rem;
        }

        .textolabel18 {
            font-size: 18px;
        }

        input, select {
            border-radius: 10px !important;
        }

        /* Header del card: izquierda botones / derecha registrar */
        .card-header-tools {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }

            .card-header-tools .left-tools {
                display: flex;
                align-items: center;
                gap: .5rem;
                flex-wrap: wrap;
            }

        /* Filtros estilo imagen */
        .filter-row .form-control {
            height: 56px;
            font-size: 28px;
        }

        .filter-row .input-group-text {
            height: 56px;
        }

        .input-group-date {
            flex-wrap: nowrap;
        }

            .input-group-date .form-control {
                background: #fff !important;
                height: 56px;
                font-size: 28px;
                border-radius: 12px !important;
            }

            .input-group-date .input-group-text,
            .input-group-date .btn {
                height: 56px;
                border-radius: 0 !important;
            }

        .btn-clear-date {
            padding: 0 14px;
            background: #fff !important;
        }

        /* Checkboxes */
        .filter-checks {
            display: flex;
            gap: 2rem;
            align-items: center;
            flex-wrap: wrap;
            margin-top: 1rem;
        }

            .filter-checks label {
                margin: 0;
                font-size: 24px;
                color: #6b6b6b;
                font-weight: 600;
            }

            .filter-checks .custom-control {
                padding-left: 2rem;
            }

        /* Acciones icon-only alineadas */
        .fact-actions {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: .35rem;
            white-space: nowrap;
        }

        .btn-icon {
            width: 36px;
            height: 36px;
            padding: 0;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            border-radius: 8px !important;
        }

        /* Tabla */
        .table-responsive {
            overflow-x: auto;
        }

        table.dataTable td, table.dataTable th {
            vertical-align: middle !important;
        }

        .text-nowrap {
            white-space: nowrap;
        }

        /* Modales por encima de todo */
        .modal {
            z-index: 20000 !important;
        }

        .modal-backdrop {
            z-index: 19999 !important;
        }
    </style>
</head>

<body>
    <uc_menu:menu ID="menu" runat="server" />
    <header id="header" class="bg-color-primary affix">
        <uc_header:cabecera ID="cabecera" runat="server" />
    </header>

    <div class="content-wrapper">
        <main class="wrapper public bg-color-white" role="main">

            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />

                <!-- CARD: FILTROS -->
                <div class="card mt-4">
                    <div class="card-header">
                        <div class="card-header-tools">
                            <!-- IZQUIERDA: Buscar / Limpiar -->
                            <div class="left-tools">
                                <button type="button" id="btnBuscar" class="btn btn-outline-warning btn-lg">
                                    <i class="fas fa-search mr-2"></i>Buscar
                                </button>

                                <button type="button" id="btnLimpiar" class="btn btn-warning btn-lg">
                                    <i class="fas fa-undo mr-2"></i>Limpiar
                                </button>
                            </div>

                            <!-- DERECHA: Registrar -->
                            <div>
                                <a href="admin_facturas.aspx" class="btn btn-success btn-lg">
                                    <i class="fas fa-plus mr-2"></i>Registrar factura
                                </a>
                            </div>
                        </div>
                    </div>

                    <div class="card-body">
                        <!-- Fila 1 -->
                        <div class="row filter-row">
                            <div class="col-lg-4 col-md-6 mb-4">
                                <select id="fSociedad" class="form-control">
                                    <option value="">Seleccione una sociedad</option>
                                    <option value="SBS">SBS</option>
                                    <option value="SBSCS">SBSCS</option>
                                </select>
                            </div>

                            <!-- Desde -->
                            <div class="col-md">
                                <div class="">
                                    <input type="text" id="fDesde" class="form-control textolabel18 bg-white" placeholder="01/01/2026" autocomplete="off" />


                                </div>
                            </div>

                            <!-- Hasta -->
                            <div class="col-md">
                                <div class="">
                                    <input type="text" id="fHasta" class="form-control textolabel18 bg-white" placeholder="31/12/2026" autocomplete="off" />

                                </div>
                            </div>

                            <!-- Año (desde BD) -->
                            <div class="col-lg-2 col-md-6 mb-4">
                                <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-control textolabel18"></asp:DropDownList>
                            </div>
                        </div>

                        <!-- Fila 2: checks -->
                        <div class="filter-checks">
                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkPendCobro" />
                                <label class="custom-control-label" for="chkPendCobro">Pendiente cobro</label>
                            </div>

                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkPendAtrib" />
                                <label class="custom-control-label" for="chkPendAtrib">Pendiente atribución</label>
                            </div>

                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkPendVenc" />
                                <label class="custom-control-label" for="chkPendVenc">Pendiente vencimiento</label>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- CARD: TABLA -->
                <div class="card mt-4">
                    <div class="card-header">
                        <h5 class="mb-0">Listado</h5>
                    </div>

                    <div class="card-body">
                        <div class="table-responsive">
                            <table id="tblFacturas" class="table table-striped table-bordered" style="width: 100%">
                                <thead>
                                    <tr>
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
                                        <th class="text-nowrap">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- MODAL: Confirmar eliminar -->
                <div class="modal fade" id="mdConfirmDelete" tabindex="-1" role="dialog" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Confirmar</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                ¿Seguro que deseas eliminar la factura? (Se eliminará también el archivo adjunto si existe)
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                                <button type="button" class="btn btn-danger" id="btnConfirmDelete">Sí, eliminar</button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- MODAL: Mensajes -->
                <div class="modal fade" id="mdInfo" tabindex="-1" role="dialog" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="mdInfoTitle">Mensaje</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="mdInfoBody"></div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" data-dismiss="modal">OK</button>
                            </div>
                        </div>
                    </div>
                </div>

            </form>
        </main>
    </div>

    <!-- JS base -->
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>

    <!-- Datepicker -->
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap-datepicker.es.js"></script>

    <!-- DataTables local -->
    <script src="App_Themes/DataTable/js/jquery.dataTables.min.js"></script>
    <script src="App_Themes/DataTable/js/dataTables.bootstrap.min.js"></script>

    <!-- Buttons (CDN) -->
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>

    <script>
        let dt = null;
        let deleteId = null;

        function showInfo(title, msg) {
            $('#mdInfoTitle').text(title || 'Mensaje');
            $('#mdInfoBody').text(msg || '');
            $('#mdInfo').modal('show');
        }

        function openConfirmDelete(id) {
            deleteId = id;
            $('#mdConfirmDelete').modal('show');
        }

        function buildActions(row) {
            const id = row.idInfFinFacturas;
            return `
                <div class="fact-actions">
                    <a class="btn btn-outline-warning btn-sm btn-icon" title="Ver/Editar" href="admin_facturas_edit.aspx?id=${id}">
                        <i class="fas fa-eye"></i>
                    </a>
                  
                </div>
            `;
        }

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

        function loadTable() {
            const filtros = getFiltros();

            $.ajax({
                url: 'admin_facturas_list.aspx/GetFacturas',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ f: filtros }),
                success: function (res) {
                    const payload = (res && res.d) ? res.d : null;

                    if (payload && payload.ok === false) {
                        console.error(payload);
                        return showInfo('Error', payload.message || 'No se pudo cargar el listado.');
                    }

                    const rows = Array.isArray(payload) ? payload : [];
                    renderDataTable(rows);
                },
                error: function (xhr) {
                    console.error(xhr);
                    showInfo('Error', 'No se pudo cargar el listado.');
                }
            });
        }

        function renderDataTable(rows) {
            if (dt) {
                dt.clear().destroy();
                $('#tblFacturas tbody').empty();
            }

            dt = $('#tblFacturas').DataTable({
                data: rows,
                columns: [
                    // ✅ Columna de orden (No.)
                    {
                        data: null,
                        title: 'No.',
                        orderable: false,
                        searchable: false,
                        width: '45px',
                        className: 'text-center',
                        render: function () { return ''; } // se llena en drawCallback
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
                    { data: 'eur_total_str', title: 'Total', className: 'text-right' },
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


                drawCallback: function () {
                    const api = this.api();
                    const start = api.page.info().start;

                    api.column(0, { page: 'current' }).nodes().each(function (cell, i) {
                        cell.innerHTML = start + i + 1;
                    });
                }


            });
        }

        function doDeleteFactura(id) {
            $.ajax({
                url: 'admin_facturas_list.aspx/DeleteFactura',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ id: Number(id) }),
                success: function (res) {
                    const rr = (res && res.d) ? res.d : null;
                    if (!rr || rr.ok === false) return showInfo('Error', (rr && rr.message) ? rr.message : 'No se pudo eliminar.');
                    loadTable();
                    showInfo('OK', rr.message || 'Factura eliminada.');

                },
                error: function (xhr) {
                    console.error(xhr);
                    showInfo('Error', 'No se pudo eliminar.');
                }
            });
        }


        $(document).on('click', '.btn-del', function (e) {
            e.preventDefault();
            e.stopPropagation();
            openConfirmDelete($(this).data('id'));
        });


        $('#btnConfirmDelete').on('click', function () {
            $('#mdConfirmDelete').modal('hide');
            if (deleteId) doDeleteFactura(deleteId);
        });

        // Buscar / Limpiar
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

            // año actual por defecto
            $('#<%= ddlyear.ClientID %>').val(String(new Date().getFullYear()));

            loadTable();
        });

        // Datepicker dd/mm/yyyy
        function initDatepickers() {
            if (!$.fn.datepicker) return;

            $('#fDesde, #fHasta').datepicker({
                format: 'dd/mm/yyyy',
                language: 'es',
                autoclose: true,
                todayHighlight: true
            });
        }

        // Botones X
        $('#btnClearDesde').on('click', function () { $('#fDesde').val(''); });
        $('#btnClearHasta').on('click', function () { $('#fHasta').val(''); });

        // Al cambiar el año => pone rango (como antes)
        $('#<%= ddlyear.ClientID %>').on('change', function () {
            const y = $(this).val();
            if (!y) return;
            $('#fDesde').val(`01/01/${y}`);
            $('#fHasta').val(`31/12/${y}`);
        });

        $(document).ready(function () {
            initDatepickers();

            // rango inicial basado en año seleccionado
            const y = $('#<%= ddlyear.ClientID %>').val();
            if (y) {
                $('#fDesde').val(`01/01/${y}`);
                $('#fHasta').val(`31/12/${y}`);
            }

            loadTable();
        });
    </script>

</body>
</html>
