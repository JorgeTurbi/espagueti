<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin-Balance-Acumulado.aspx.cs" Inherits="campus_sbs_admin.Admin_Balance_Acumulado" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Balance Acumulado</title>

    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>        
        <%: Styles.Render("~/bundles/jquery_ui_css") %>
    </asp:PlaceHolder>

    <!-- Google Fonts para tabla monospace -->
    <link href="https://fonts.googleapis.com/css2?family=Roboto+Mono:wght@400;500&display=swap" rel="stylesheet" />

    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>

    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js" async></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js" async></script>
    <![endif]-->

    <style type="text/css">
        /* Variables de color para el balance */
        :root {
            --balance-positive: #27ae60;
            --balance-negative: #e74c3c;
            --balance-gold: #d4a726;
            --balance-navy: #1e3a5f;
            --balance-navy-dark: #1a2744;
        }

        /* Tarjetas resumen */
        .summary-cards {
            display: flex;
            flex-wrap: wrap;
            gap: 1rem;
            margin-bottom: 1.5rem;
        }

        .summary-card {
            flex: 1;
            min-width: 180px;
            background: #fff;
            border: 1px solid #dde2e8;
            border-radius: 8px;
            padding: 1rem 1.25rem;
            position: relative;
            overflow: hidden;
        }

        .summary-card::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 4px;
        }

        .summary-card.resultado::before { background: linear-gradient(90deg, var(--balance-gold), #e8bf4a); }
        .summary-card.ingresos::before { background: linear-gradient(90deg, var(--balance-positive), #2ecc71); }
        .summary-card.gastos::before { background: linear-gradient(90deg, var(--balance-negative), #e67e73); }
        .summary-card.info::before { background: linear-gradient(90deg, var(--balance-navy), #2d4a6f); }

        .summary-card-label {
            font-size: 0.75rem;
            font-weight: 600;
            text-transform: uppercase;
            color: #5a6a7a;
            margin-bottom: 0.375rem;
        }

        .summary-card-value {
            font-size: 1.375rem;
            font-weight: 700;
            font-family: 'Roboto Mono', monospace;
        }

        .summary-card-value.positive { color: var(--balance-positive); }
        .summary-card-value.negative { color: var(--balance-negative); }
        .summary-card-value.gold { color: #b8911f; }

        /* Tabla Balance */
        .table-balance-container {
            background: #fff;
            border: 1px solid #dde2e8;
            border-radius: 8px;
            overflow: hidden;
        }

        .table-balance-wrapper {
            overflow-x: auto;
        }

        #block_balance table {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.8125rem;
        }

        #block_balance thead th {
            background: var(--balance-navy-dark);
            padding: 0.875rem 0.625rem;
            text-align: center;
            font-weight: 600;
            font-size: 0.6875rem;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: #fff;
            border-bottom: 3px solid var(--balance-gold);
            white-space: nowrap;
            position: sticky;
            top: 0;
            z-index: 10;
        }

        #block_balance thead th:first-child {
            text-align: left;
            padding-left: 1.25rem;
            min-width: 280px;
        }

        #block_balance thead th:last-child {
            background: var(--balance-navy);
            color: var(--balance-gold);
        }

        #block_balance td {
            padding: 0.625rem;
            text-align: right;
            border-bottom: 1px solid #e8ecf0;
            font-family: 'Roboto Mono', monospace;
            font-size: 0.8125rem;
        }

        #block_balance td:first-child {
            text-align: left;
            font-family: 'Open Sans', sans-serif;
            padding-left: 1.25rem;
        }

        #block_balance td:last-child {
            background: rgba(212, 167, 38, 0.06);
            font-weight: 600;
        }

        #block_balance tbody tr:hover {
            background: rgba(212, 167, 38, 0.08);
        }

        /* Toggle icon */
        .toggle-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 20px;
            height: 20px;
            margin-right: 8px;
            cursor: pointer;
            border-radius: 4px;
            transition: all 0.2s ease;
            background: rgba(255,255,255,0.1);
        }

        .toggle-icon:hover { background: rgba(255,255,255,0.2); }
        .toggle-icon i { font-size: 0.65rem; transition: transform 0.2s ease; }
        tr.collapsed .toggle-icon i { transform: rotate(-90deg); }
        tr.child-hidden { display: none !important; }

        /* Row levels */
        #block_balance tr.row-level-0 {
            background: var(--balance-navy-dark) !important;
        }

        #block_balance tr.row-level-0 td {
            font-weight: 700 !important;
            font-size: 0.875rem !important;
            padding: 0.875rem 0.625rem !important;
            border-bottom: 2px solid #dde2e8 !important;
            color: #fff;
        }

        #block_balance tr.row-level-0 td:last-child {
            background: var(--balance-navy) !important;
        }

        #block_balance tr.row-level-0.resultado td {
            color: var(--balance-gold);
        }

        #block_balance tr.row-level-0 .toggle-icon {
            background: rgba(212, 167, 38, 0.3);
        }

        #block_balance tr.row-level-1 {
            background: rgba(30, 58, 95, 0.06);
        }

        #block_balance tr.row-level-1 td {
            font-weight: 600;
            color: var(--balance-navy-dark);
        }

        #block_balance tr.row-level-1 .toggle-icon {
            background: rgba(30, 58, 95, 0.15);
            margin-left: 20px;
        }

        #block_balance tr.row-level-1 .toggle-icon i {
            color: var(--balance-navy);
        }

        #block_balance tr.row-level-2 td:first-child {
            color: #5a6a7a;
            padding-left: 3.5rem;
        }

        #block_balance tr.row-level-2 .toggle-icon {
            background: rgba(90, 106, 122, 0.15);
            margin-left: 0;
        }

        #block_balance tr.row-level-3 td:first-child {
            color: #8a9aaa;
            font-style: italic;
            padding-left: 5rem;
        }

        /* Colores valores */
        #block_balance .value-positive { color: var(--balance-positive); }
        #block_balance .value-negative { color: var(--balance-negative); }
        #block_balance .value-zero { color: #8a9aaa; }
        #block_balance .red { color: var(--balance-negative) !important; }
        #block_balance tr.row-level-0 .red { color: #ff8a80 !important; }

        #block_balance td a {
            color: inherit;
            text-decoration: none;
        }

        #block_balance td a:hover {
            color: var(--balance-gold);
            text-decoration: underline;
        }

        /* Footer tabla */
        .table-balance-footer {
            padding: 0.875rem 1.25rem;
            background: #f5f6fa;
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-top: 1px solid #dde2e8;
            flex-wrap: wrap;
            gap: 0.5rem;
        }

        .table-balance-footer-info {
            font-size: 0.75rem;
            color: #8a9aaa;
        }

        .table-balance-footer-info i {
            color: var(--balance-gold);
            margin-right: 0.375rem;
        }

        .table-balance-actions {
            display: flex;
            gap: 0.5rem;
        }

        .btn-table-action {
            padding: 0.375rem 0.75rem;
            font-size: 0.7rem;
            background: #fff;
            border: 1px solid #dde2e8;
            border-radius: 4px;
            cursor: pointer;
            transition: all 0.2s ease;
            color: #5a6a7a;
        }

        .btn-table-action:hover {
            background: var(--balance-gold);
            border-color: var(--balance-gold);
            color: var(--balance-navy-dark);
        }

        .btn-table-action i {
            margin-right: 0.375rem;
        }

        @media print {
            tr.child-hidden { display: table-row !important; }
            .table-balance-actions { display: none; }
        }
    </style>
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
                     CABECERA
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-chart-bar"></i> Balance Acumulado
                            <small class="pull-right text-muted">Resumen financiero por período</small>
                        </legend>
                    </fieldset>
                </div>

                <!-- ===========================
                     FILTROS
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-filter"></i> Filtros
                        </legend>
                    </fieldset>

                    <div class="col-12 pt-2">
                        <div class="col-3">
                            <label>Sociedad</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlSociety">Sociedad</label>
                                <select id="ddlSociety" class="form-control" title="Sociedad" runat="server"></select>
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Año</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlYear">Año</label>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" title="Año"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-3">
                            <label>&nbsp;</label>
                            <div class="form-group">
                                <asp:Button ID="btn_search" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btn_search_Click" />
                                <asp:Button ID="btn_excel" runat="server" Text="Exportar Excel" CssClass="btn btn-success ml-2" OnClick="btn_excel_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     TARJETAS RESUMEN
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-tachometer-alt"></i> Resumen
                        </legend>
                    </fieldset>

                    <div class="col-12 pt-2">
                        <div class="summary-cards">
                            <div class="summary-card resultado">
                                <div class="summary-card-label">Resultado Anual</div>
                                <div class="summary-card-value gold" id="lblResultado" runat="server">0€</div>
                            </div>
                            <div class="summary-card ingresos">
                                <div class="summary-card-label">Total Ingresos</div>
                                <div class="summary-card-value positive" id="lblIngresos" runat="server">0€</div>
                            </div>
                            <div class="summary-card gastos">
                                <div class="summary-card-label">Total Gastos</div>
                                <div class="summary-card-value negative" id="lblGastos" runat="server">0€</div>
                            </div>
                            <div class="summary-card info">
                                <div class="summary-card-label">Año Seleccionado</div>
                                <div class="summary-card-value" id="lblYear" runat="server" style="color: #1e3a5f;">2026</div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     TABLA BALANCE
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-table"></i> Detalle del Balance
                        </legend>
                    </fieldset>

                    <div class="col-12 pt-2">
                        <div class="table-balance-container">
                            <div class="table-balance-wrapper">
                                <div id="block_balance" runat="server"></div>
                            </div>
                            <div class="table-balance-footer">
                                <div class="table-balance-footer-info">
                                    <i class="fas fa-clock"></i>Última actualización:
                                    <asp:Label ID="lblLastUpdate" runat="server" />
                                </div>
                                <div class="table-balance-actions">
                                    <button type="button" class="btn-table-action" id="btnExpandAll">
                                        <i class="fas fa-expand-alt"></i>Expandir todo
                                    </button>
                                    <button type="button" class="btn-table-action" id="btnCollapseAll">
                                        <i class="fas fa-compress-alt"></i>Colapsar todo
                                    </button>
                                </div>
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
        // ===========================
        // Helpers UI
        // ===========================
        function showLoading() { if ($.fn.modal) $('#wait_modal').modal('show'); }
        function hideLoading() { if ($.fn.modal) $('#wait_modal').modal('hide'); }

        function showInfo(title, html) {
            $('#info_modal_title').text(title || 'Mensaje');
            $('#info_modal_body').html(html || '');
            if ($.fn.modal) $('#info_modal').modal('show');
        }

        // ===========================
        // Toggle de categorías tabla
        // ===========================
        function initTableToggles() {
            var toggleIcons = document.querySelectorAll('#block_balance .toggle-icon');

            for (var i = 0; i < toggleIcons.length; i++) {
                toggleIcons[i].addEventListener('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var row = this.closest('tr');
                    toggleRow(row);
                });
            }
        }

        function toggleRow(row) {
            var isCollapsed = row.classList.contains('collapsed');
            var rowId = row.getAttribute('data-row-id');

            if (isCollapsed) {
                row.classList.remove('collapsed');
                showChildren(rowId);
            } else {
                row.classList.add('collapsed');
                hideChildren(rowId);
            }
        }

        function showChildren(parentId) {
            var children = document.querySelectorAll('tr[data-parent-id="' + parentId + '"]');

            for (var i = 0; i < children.length; i++) {
                var child = children[i];
                child.classList.remove('child-hidden');

                var childId = child.getAttribute('data-row-id');
                if (childId && !child.classList.contains('collapsed')) {
                    showChildren(childId);
                }
            }
        }

        function hideChildren(parentId) {
            var children = document.querySelectorAll('tr[data-parent-id="' + parentId + '"]');

            for (var i = 0; i < children.length; i++) {
                var child = children[i];
                child.classList.add('child-hidden');

                var childId = child.getAttribute('data-row-id');
                if (childId) {
                    hideChildren(childId);
                }
            }
        }

        function expandAll() {
            var rows = document.querySelectorAll('#block_balance tr[data-row-id]');

            for (var i = 0; i < rows.length; i++) {
                rows[i].classList.remove('collapsed');
            }

            var hiddenRows = document.querySelectorAll('#block_balance tr.child-hidden');
            for (var j = 0; j < hiddenRows.length; j++) {
                hiddenRows[j].classList.remove('child-hidden');
            }
        }

        function collapseAll() {
            var parentRows = document.querySelectorAll('#block_balance tr[data-row-id]');

            for (var i = 0; i < parentRows.length; i++) {
                parentRows[i].classList.add('collapsed');
                var rowId = parentRows[i].getAttribute('data-row-id');
                hideChildren(rowId);
            }
        }

        // ===========================
        // Eventos
        // ===========================
        $(document).ready(function () {
            initTableToggles();

            $('#btnExpandAll').on('click', function (e) {
                e.preventDefault();
                expandAll();
            });

            $('#btnCollapseAll').on('click', function (e) {
                e.preventDefault();
                collapseAll();
            });
        });

        // Re-inicializar después de postback
        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initTableToggles();
            });
        }
    </script>
</body>
</html>