<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="admin_gastos_list.aspx.cs"
    Inherits="campus_sbs_admin.admin_gastos_list" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>
<html class="no-legacy-ie no-js" lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Gastos</title>

    <link rel="stylesheet" href="/App_Themes/support/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/sbs.css" />

    <link href="/App_Themes/support/css/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />

    <link href="App_Themes/DataTable/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="App_Themes/DataTable/css/dataTables.bootstrap.min.css" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.5.2/css/all.min.css" />

    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js"></script>

    <style>
        .content-wrapper { margin-left: 260px; padding: 20px; }
        @media (max-width: 768px) { .content-wrapper { margin-left: 0; padding: 15px; } }

        .card { border: none; }
        .card-body { padding: 1.25rem; }

        .textolabel18 { font-size: 18px; }
        input, select { border-radius: 10px !important; }

        .card-header-tools {
            display: flex; justify-content: space-between; align-items: center;
            gap: 10px; flex-wrap: wrap;
        }
        .card-header-tools .left-tools { display: flex; align-items: center; gap: .5rem; flex-wrap: wrap; }

        .filter-row .form-control { height: 56px; font-size: 28px; }

        .filter-checks {
            display: flex; gap: 2rem; align-items: center; flex-wrap: wrap; margin-top: 1rem;
        }
        .filter-checks label { margin: 0; font-size: 24px; color: #6b6b6b; font-weight: 600; }
        .filter-checks .custom-control { padding-left: 2rem; }

        .gastos-actions {
            display: flex; align-items: center; justify-content: center;
            gap: .35rem; white-space: nowrap;
        }
        .btn-icon {
            width: 36px; height: 36px; padding: 0;
            display: inline-flex; align-items: center; justify-content: center;
            border-radius: 8px !important;
        }

        .table-responsive { overflow-x: auto; }
        table.dataTable td, table.dataTable th { vertical-align: middle !important; }
        .text-nowrap { white-space: nowrap; }

        .modal { z-index: 20000 !important; }
        .modal-backdrop { z-index: 19999 !important; }
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
                            <div class="left-tools">
                                <button type="button" id="btnBuscar" class="btn btn-outline-warning btn-lg">
                                    <i class="fas fa-search mr-2"></i>Buscar
                                </button>
                                <button type="button" id="btnLimpiar" class="btn btn-warning btn-lg">
                                    <i class="fas fa-undo mr-2"></i>Limpiar
                                </button>
                            </div>

                            <div>
                                <a href="Admin_Gastos.aspx" class="btn btn-success btn-lg">
                                    <i class="fas fa-plus mr-2"></i>Registrar gasto
                                </a>
                            </div>
                        </div>
                    </div>

                    <div class="card-body">
                        <div class="row filter-row">
                            <div class="col-lg-4 col-md-6 mb-4">
                                <select id="fSociedad" class="form-control">
                                    <option value="">Seleccione una sociedad</option>
                                    <option value="SBS">SBS</option>
                                    <option value="SBSCS">SBSCS</option>
                                </select>
                            </div>

                            <div class="col-md mb-4">
                                <input type="text" id="fDesde" class="form-control textolabel18 bg-white" placeholder="01/01/2026" autocomplete="off" />
                            </div>

                            <div class="col-md mb-4">
                                <input type="text" id="fHasta" class="form-control textolabel18 bg-white" placeholder="31/12/2026" autocomplete="off" />
                            </div>

                            <!-- Año -->
                            <div class="col-lg-2 col-md-6 mb-4">
                                <asp:DropDownList ID="ddlyear" runat="server" CssClass="form-control textolabel18"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="filter-checks">
                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkNoFactura" />
                                <label class="custom-control-label" for="chkNoFactura">No Factura</label>
                            </div>

                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkNoCatalog" />
                                <label class="custom-control-label" for="chkNoCatalog">No Catalogación</label>
                            </div>

                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkImpuestos" />
                                <label class="custom-control-label" for="chkImpuestos">Impuestos</label>
                            </div>

                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkNoPago" />
                                <label class="custom-control-label" for="chkNoPago">No Pago</label>
                            </div>

                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkNoBanco" />
                                <label class="custom-control-label" for="chkNoBanco">No Banco</label>
                            </div>

                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="chkProvision" />
                                <label class="custom-control-label" for="chkProvision">Provisión</label>
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
                            <table id="tblGastos" class="table table-striped table-bordered" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>No.</th>
                                        <th>Sociedad</th>
                                        <th>Núm</th>
                                        <th>Datos Empresa / CIF</th>
                                        <th>F. Emisión</th>
                                        <th>Descripción</th>
                                        <th>SubTotal</th>
                                        <th>IVA</th>
                                        <th>IRPF</th>
                                        <th>Total</th>
                                        <th>F. Pago</th>
                                        <th>Banco</th>
                                        <th>No Factura</th>
                                        <th>Catalog.</th>
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
                                ¿Seguro que deseas eliminar el gasto? (Se eliminará también el archivo adjunto si existe)
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>

                                <!-- ✅ aquí guardamos el ID -->
                                <button type="button" class="btn btn-danger" id="btnConfirmDelete" data-id="">
                                    Sí, eliminar
                                </button>
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

    <!-- DataTables -->
    <script src="App_Themes/DataTable/js/jquery.dataTables.min.js"></script>
    <script src="App_Themes/DataTable/js/dataTables.bootstrap.min.js"></script>

    <!-- Buttons -->
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>

    <script>
        let dt = null;

        function showInfo(title, msg) {
            $('#mdInfoTitle').text(title || 'Mensaje');
            $('#mdInfoBody').text(msg || '');
            $('#mdInfo').modal('show');
        }

        function openConfirmDelete(id) {
            // ✅ guardo el id en el botón confirmar (NO en variable global)
            $('#btnConfirmDelete').attr('data-id', id);
            $('#mdConfirmDelete').modal('show');
        }

        function buildActions(row) {
            const id = row.idInfFinCostes;
            return `
                <div class="gastos-actions">
                    <a class="btn btn-outline-primary btn-sm btn-icon" title="Editar" href="Admin_Gastos.aspx?idg=${id}&edit=1">
                        <i class="fa-solid fa-pen-to-square"></i>
                    </a>
                    <button type="button" class="btn btn-outline-danger btn-sm btn-icon btn-del" title="Eliminar" data-id="${id}">
                        <i class="fa-solid fa-xmark"></i>
                    </button>
                </div>
            `;
        }

        function getFiltros() {
            return {
                sociedad: $('#fSociedad').val() || null,
                desde: $('#fDesde').val() || null,
                hasta: $('#fHasta').val() || null,
                year: $('#<%= ddlyear.ClientID %>').val() || null,

                noFactura: $('#chkNoFactura').is(':checked'),
                noCatalog: $('#chkNoCatalog').is(':checked'),
                impuestos: $('#chkImpuestos').is(':checked'),
                noPago: $('#chkNoPago').is(':checked'),
                noBanco: $('#chkNoBanco').is(':checked'),
                provision: $('#chkProvision').is(':checked')
            };
        }

        function loadTable(keepPage) {
            const filtros = getFiltros();
            const currentPage = (dt && keepPage) ? dt.page() : 0;

            $.ajax({
                url: 'admin_gastos_list.aspx/GetGastos',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ f: filtros }),
                success: function (res) {
                    const payload = (res && res.d) ? res.d : null;
                    if (payload && payload.ok === false) return showInfo('Error', payload.message || 'No se pudo cargar el listado.');

                    const rows = Array.isArray(payload) ? payload : [];
                    renderDataTable(rows, currentPage);
                },
                error: function (xhr) {
                    console.error(xhr);
                    showInfo('Error', 'No se pudo cargar el listado.');
                }
            });
        }

        function renderDataTable(rows, goToPage) {
            if (dt) {
                dt.clear().destroy();
                $('#tblGastos tbody').empty();
            }

            dt = $('#tblGastos').DataTable({
                data: rows,
                columns: [
                    { data: null, orderable: false, searchable: false, width: '45px', className: 'text-center', render: () => '' },
                    { data: 'sociedad' },
                    { data: 'num', className: 'text-center' },
                    { data: 'datos_empresa' },
                    { data: 'f_emision', className: 'text-nowrap' },
                    { data: 'descripcion' },
                    { data: 'eur_subtotal', className: 'text-right text-nowrap' },
                    { data: 'eur_iva', className: 'text-right text-nowrap' },
                    { data: 'eur_irpf', className: 'text-right text-nowrap' },
                    { data: 'eur_total', className: 'text-right text-nowrap' },
                    { data: 'f_pago', className: 'text-nowrap' },
                    { data: 'banco' },
                    { data: 'no_factura', className: 'text-center' },
                    { data: 'catalog', className: 'text-nowrap text-center' },
                    { data: null, orderable: false, searchable: false, className: 'text-nowrap', render: (d, t, r) => buildActions(r) }
                ],
                pageLength: 15,
                order: [],
                dom: 'Bfrtip',
                buttons: [
                    { extend: 'excelHtml5', title: 'Gastos', exportOptions: { columns: ':visible:not(:last-child)' } },
                    { extend: 'pdfHtml5', title: 'Gastos', exportOptions: { columns: ':visible:not(:last-child)' } },
                    { extend: 'print', title: 'Gastos', exportOptions: { columns: ':visible:not(:last-child)' } }
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

            if (typeof goToPage === 'number' && goToPage > 0) {
                dt.page(goToPage).draw('page');
            }
        }

        function doDeleteGasto(id) {
            if (!id || Number(id) <= 0) {
                return showInfo('Error', 'ID inválido para eliminar.');
            }

            $.ajax({
                url: 'admin_gastos_list.aspx/DeleteGasto',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ id: Number(id) }),
                success: function (res) {
                    const rr = (res && res.d) ? res.d : null;

                    if (!rr || rr.ok === false) {
                        return showInfo('Error', (rr && rr.message) ? rr.message : 'No se pudo eliminar.');
                    }

                    // ✅ recarga manteniendo paginación actual
                    loadTable(true);

                    showInfo('OK', rr.message || 'Gasto eliminado.');
                },
                error: function (xhr) {
                    console.error(xhr);
                    showInfo('Error', 'No se pudo eliminar.');
                }
            });
        }

        // ✅ evento delete (delegado)
        $(document).on('click', '.btn-del', function (e) {
            e.preventDefault();
            e.stopPropagation();
            const id = Number($(this).attr('data-id') || 0);
            console.log('=====>>>',id)
            openConfirmDelete(id);
        });

        // ✅ confirmación: leer id del data-id
        $('#btnConfirmDelete').on('click', function () {
            const id = Number($(this).attr('data-id') || 0);
            console.log('encontrados',id)

            // evita warning aria-hidden
            if (document.activeElement) document.activeElement.blur();

            $('#mdConfirmDelete').modal('hide');

            if (id > 0) {
                console.log(`tenemos el id===>${id}`)
                doDeleteGasto(id);
            } else {
                console.log('Error', 'No se encontró el ID del registro a eliminar.')
                showInfo('Error', 'No se encontró el ID del registro a eliminar.');
            }
        });

        $('#btnBuscar').on('click', function (e) { e.preventDefault(); loadTable(false); });

        $('#btnLimpiar').on('click', function (e) {
            e.preventDefault();
            $('#fSociedad').val('');
            $('#fDesde').val('');
            $('#fHasta').val('');
            $('#chkNoFactura,#chkNoCatalog,#chkImpuestos,#chkNoPago,#chkNoBanco,#chkProvision').prop('checked', false);

            $('#<%= ddlyear.ClientID %>').val(String(new Date().getFullYear()));
            const y = $('#<%= ddlyear.ClientID %>').val();
            if (y) { $('#fDesde').val(`01/01/${y}`); $('#fHasta').val(`31/12/${y}`); }
            loadTable(false);
        });

        function initDatepickers() {
            if (!$.fn.datepicker) return;
            $('#fDesde, #fHasta').datepicker({
                format: 'dd/mm/yyyy',
                language: 'es',
                autoclose: true,
                todayHighlight: true
            });
        }

        $('#<%= ddlyear.ClientID %>').on('change', function () {
            const y = $(this).val();
            if (!y) return;
            $('#fDesde').val(`01/01/${y}`);
            $('#fHasta').val(`31/12/${y}`);
        });

        $(document).ready(function () {
            initDatepickers();
            const y = $('#<%= ddlyear.ClientID %>').val();
            if (y) { $('#fDesde').val(`01/01/${y}`); $('#fHasta').val(`31/12/${y}`); }
            loadTable(false);
        });
    </script>
</body>
</html>
