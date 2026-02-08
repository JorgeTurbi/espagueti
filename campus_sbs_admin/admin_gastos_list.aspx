<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="admin_gastos_list.aspx.cs"
    Inherits="campus_sbs_admin.admin_gastos_list" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Consulta de Gastos</title>

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

                <!-- ===========================
                     SECCIÓN: FILTROS DE BÚSQUEDA
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-search"></i> Búsqueda de Gastos
                            <a href='Admin_Gastos.aspx' title='Registrar gasto' class='pull-right bold padding-r-5'>
                                <small class='text-color-primary'><i class='fas fa-plus'></i> Registrar gasto</small>
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
                        <div class="col-2">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkNoFactura" />
                                    <label class="custom-control-label" for="chkNoFactura">No Factura</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkNoCatalog" />
                                    <label class="custom-control-label" for="chkNoCatalog">No Catalogación</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkImpuestos" />
                                    <label class="custom-control-label" for="chkImpuestos">Impuestos</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkNoPago" />
                                    <label class="custom-control-label" for="chkNoPago">No Pago</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkNoBanco" />
                                    <label class="custom-control-label" for="chkNoBanco">No Banco</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkProvision" />
                                    <label class="custom-control-label" for="chkProvision">Provisión</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Fila 3: Botones buscar/limpiar -->
                    <div class="col-12 pt-2">
                        <div class="col-12 px-0">
                            <div class="form-group">
                                <a href="javascript:void(0);" id="btnBuscar" title="Buscar"><i class="fas fa-search fa-2x text-color-primary"></i></a>
                                <a href="javascript:void(0);" id="btnLimpiar" title="Limpiar filtros" class="ml-3"><i class="fas fa-undo fa-2x text-color-primary"></i></a>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     SECCIÓN: LISTADO DE GASTOS
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="far fa-list-alt"></i> Listado de Gastos
                        </legend>
                    </fieldset>

                    <div id="table_list_gastos" class="col-sm-12 padding-tb-20">
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
                        ¿Seguro que deseas eliminar el gasto? (Se eliminará también el archivo adjunto si existe)
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-danger" id="btnConfirmDelete" data-id="">
                            <i class="fas fa-trash"></i> Sí, eliminar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Info -->
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

    <!-- Datepicker -->
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/bootstrap-datepicker.es.js"></script>

    <!-- DataTables Buttons -->
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js"></script>

    <script type="text/javascript">
        var dt = null;

        // ===========================
        // Helpers UI (Modal)
        // ===========================
        function showLoading() {
            if ($.fn.modal) $('#wait_modal').modal('show');
        }

        function hideLoading() {
            if ($.fn.modal) $('#wait_modal').modal('hide');
        }

        function hideLoadingForce() {
            try { $('#wait_modal').modal('hide'); } catch (e) { }

            // Por si Bootstrap deja backdrop pegado
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open').css('padding-right', '');
        }


        function showInfo(title, msg) {
            $('#info_modal_title').text(title || 'Mensaje');
            $('#info_modal_body').html(msg || '');
            if ($.fn.modal) $('#info_modal').modal('show');
            else alert((title ? title + ': ' : '') + msg);
        }

        function openConfirmDelete(id) {
            $('#btnConfirmDelete').attr('data-id', id);
            $('#confirm_modal').modal('show');
        }

        // ===========================
        // Construcción de acciones
        // ===========================
        function buildActions(row) {
            var id = row.idInfFinCostes;
            var html = '<div class="text-center">';
            html += '<a class="btn btn-sm btn-outline-primary" title="Editar" href="Admin_Gastos.aspx?idg=' + id + '&edit=1">';
            html += '<i class="fas fa-eye"></i>';
            html += '</a>';
            html += ' <button type="button" class="btn btn-sm btn-outline-danger btn-del" title="Eliminar" data-id="' + id + '">';
            html += '<i class="fas fa-trash"></i>';
            html += '</button>';
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
                noFactura: $('#chkNoFactura').is(':checked'),
                noCatalog: $('#chkNoCatalog').is(':checked'),
                impuestos: $('#chkImpuestos').is(':checked'),
                noPago: $('#chkNoPago').is(':checked'),
                noBanco: $('#chkNoBanco').is(':checked'),
                provision: $('#chkProvision').is(':checked')
            };
        }

        // ===========================
        // Cargar tabla
        // ===========================
        function loadTable(keepPage) {
            var filtros = getFiltros();
            var currentPage = (dt && keepPage) ? dt.page() : 0;

            showLoading();

            $.ajax({
                url: 'admin_gastos_list.aspx/GetGastos',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ f: filtros }),
                timeout: 30000, // ✅ evita que se quede infinito (30s)

                success: function (res) {
                    var payload = (res && res.d) ? res.d : null;

                    // Si el server devuelve {ok:false,...}
                    if (payload && payload.ok === false) {
                        showInfo('Error', payload.message || 'No se pudo cargar el listado.');
                        renderDataTable([], currentPage);
                        return;
                    }

                    var rows = Array.isArray(payload) ? payload : [];
                    renderDataTable(rows, currentPage);
                },

                error: function (xhr) {
                    console.error('GetGastos error:', xhr.status, xhr.responseText);
                    showInfo('Error', 'No se pudo cargar el listado.');
                    renderDataTable([], currentPage);
                },

                complete: function () {
                    // ✅ SIEMPRE se ejecuta (success, error, timeout, abort)
                    hideLoadingForce();
                }
            });
        }

        // ===========================
        // Renderizar DataTable
        // ===========================
        function renderDataTable(rows, goToPage) {
            if (dt) {
                dt.clear().destroy();
                $('#tblGastos tbody').empty();
            }

            dt = $('#tblGastos').DataTable({
                data: rows,
                columns: [
                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        width: '45px',
                        className: 'text-center',
                        render: function () { return ''; }
                    },
                    { data: 'sociedad' },
                    { data: 'num', className: 'text-center' },
                    { data: 'datos_empresa' },
                    { data: 'f_emision', className: 'text-nowrap' },
                    { data: 'descripcion' },
                    { data: 'eur_subtotal', className: 'text-right text-nowrap' },
                    { data: 'eur_iva', className: 'text-right text-nowrap' },
                    { data: 'eur_irpf', className: 'text-right text-nowrap' },
                    { data: 'eur_total', className: 'text-right text-nowrap bold' },
                    { data: 'f_pago', className: 'text-nowrap' },
                    { data: 'banco' },
                    { data: 'no_factura', className: 'text-center' },
                    { data: 'catalog', className: 'text-nowrap text-center' },
                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        className: 'text-nowrap',
                        render: function (data, type, row) { return buildActions(row); }
                    }
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

            if (typeof goToPage === 'number' && goToPage > 0) {
                dt.page(goToPage).draw('page');
            }
        }

        // ===========================
        // Eliminar gasto
        // ===========================
        function doDeleteGasto(id) {
            if (!id || Number(id) <= 0) {
                return showInfo('Error', 'ID inválido para eliminar.');
            }

            showLoading();

            $.ajax({
                url: 'admin_gastos_list.aspx/DeleteGasto',
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

                    loadTable(true);
                    showInfo('Éxito', rr.message || 'Gasto eliminado correctamente.');
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
            var id = Number($(this).attr('data-id') || 0);
            openConfirmDelete(id);
        });

        $('#btnConfirmDelete').on('click', function () {
            var id = Number($(this).attr('data-id') || 0);

            if (document.activeElement) document.activeElement.blur();

            $('#confirm_modal').modal('hide');

            if (id > 0) {
                doDeleteGasto(id);
            } else {
                showInfo('Error', 'No se encontró el ID del registro a eliminar.');
            }
        });

        $('#btnBuscar').on('click', function (e) {
            e.preventDefault();
            loadTable(false);
        });

        $('#btnLimpiar').on('click', function (e) {
            e.preventDefault();
            $('#fSociedad').val('');
            $('#fDesde').val('');
            $('#fHasta').val('');
            $('#chkNoFactura,#chkNoCatalog,#chkImpuestos,#chkNoPago,#chkNoBanco,#chkProvision').prop('checked', false);

            var currentYear = new Date().getFullYear();
            $('#<%= ddlyear.ClientID %>').val(String(currentYear));
            $('#fDesde').val('01/01/' + currentYear);
            $('#fHasta').val('31/12/' + currentYear);

            loadTable(false);
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

            loadTable(false);
        });
    </script>
</body>
</html>