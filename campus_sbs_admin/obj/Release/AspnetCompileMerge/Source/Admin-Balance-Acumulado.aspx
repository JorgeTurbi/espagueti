<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin-Balance-Acumulado.aspx.cs" Inherits="campus_sbs_admin.Admin_Balance_Acumulado" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>
<!DOCTYPE html>
<html class="no-legacy-ie no-js" lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Balance Acumulado | Spain Business School</title>
    <!-- Bootstrap / estilos base -->
    <link rel="stylesheet" href="/App_Themes/support/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="stylesheet" href="/App_Themes/support/css/sbs.css" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@400;500;600;700&family=Roboto+Mono:wght@400;500&display=swap" rel="stylesheet" />

    <style type="text/css">
        :root {
            --sbs-navy-dark: #1a2744;
            --sbs-navy: #1e3a5f;
            --sbs-navy-light: #2d4a6f;
            --sbs-gold: #d4a726;
            --sbs-gold-light: #e8bf4a;
            --sbs-gold-dark: #b8911f;
            --bg-primary: #f5f6fa;
            --bg-white: #ffffff;
            --text-dark: #2c3e50;
            --text-secondary: #5a6a7a;
            --text-muted: #8a9aaa;
            --border-color: #dde2e8;
            --border-light: #e8ecf0;
            --positive: #27ae60;
            --negative: #e74c3c;
            --row-hover: rgba(212, 167, 38, 0.08);
            --sidebar-width: 220px;
            --sidebar-collapsed-width: 60px;
            --header-height: 70px;
            --transition-speed: 0.3s;
        }


        /* Obliga a colpsar el sidebar */
        /* Estilos para el sidebar del sistema (nav.ascx) */

        .nav-container,
        .sidebar,
        .main-nav,
        nav.nav,
        #nav,
        .valkyrie-nav,
        aside.sidebar,
        .left-sidebar,
        .side-menu {
            width: var(--sidebar-width);
            transition: width var(--transition-speed) ease, transform var(--transition-speed) ease, left var(--transition-speed) ease;
        }

        /* Cuando el sidebar está colapsado */
        body.sidebar-collapsed .nav-container,
        body.sidebar-collapsed .sidebar,
        body.sidebar-collapsed .main-nav,
        body.sidebar-collapsed nav.nav,
        body.sidebar-collapsed #nav,
        body.sidebar-collapsed .valkyrie-nav,
        body.sidebar-collapsed aside.sidebar,
        body.sidebar-collapsed .left-sidebar,
        body.sidebar-collapsed .side-menu {
            width: var(--sidebar-collapsed-width);
            overflow: hidden;
        }

            /* Ocultar texto del menú cuando está colapsado */
            body.sidebar-collapsed .nav-container span,
            body.sidebar-collapsed .sidebar span,
            body.sidebar-collapsed .main-nav span,
            body.sidebar-collapsed .valkyrie-nav span,
            body.sidebar-collapsed .nav-text,
            body.sidebar-collapsed .menu-text,
            body.sidebar-collapsed .nav-label {
                opacity: 0;
                width: 0;
                overflow: hidden;
                white-space: nowrap;
            }

            /* Centrar iconos cuando está colapsado */
            body.sidebar-collapsed .nav-container a,
            body.sidebar-collapsed .sidebar a,
            body.sidebar-collapsed .main-nav a,
            body.sidebar-collapsed .valkyrie-nav a {
                justify-content: center;
                padding-left: 0;
                padding-right: 0;
            }

            /* Ocultar flechas de submenú cuando está colapsado */
            body.sidebar-collapsed .nav-container .fa-chevron-down,
            body.sidebar-collapsed .nav-container .fa-chevron-right,
            body.sidebar-collapsed .nav-container .fa-angle-down,
            body.sidebar-collapsed .nav-container .submenu-arrow,
            body.sidebar-collapsed .sidebar .fa-chevron-down,
            body.sidebar-collapsed .valkyrie-nav .fa-chevron-down {
                display: none;
            }

        /* Botón toggle del sidebar */
        .sidebar-toggle {
            position: fixed;
            top: calc(var(--header-height) + 15px);
            left: calc(var(--sidebar-width) - 15px);
            z-index: 1050;
            width: 30px;
            height: 30px;
            background: var(--sbs-gold);
            border: none;
            border-radius: 50%;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            box-shadow: 0 2px 8px rgba(0,0,0,0.3);
            transition: left var(--transition-speed) ease, transform 0.2s ease;
        }

            .sidebar-toggle:hover {
                transform: scale(1.1);
                background: var(--sbs-gold-light);
            }

            .sidebar-toggle i {
                color: var(--sbs-navy-dark);
                font-size: 0.75rem;
                transition: transform var(--transition-speed) ease;
            }

        /* Estado colapsado - botón */
        body.sidebar-collapsed .sidebar-toggle {
            left: calc(var(--sidebar-collapsed-width) - 15px);
        }

            body.sidebar-collapsed .sidebar-toggle i {
                transform: rotate(180deg);
            }

        /*
           CONTENEDOR PRINCIPAL
          */
        .balance-container {
            padding: 30px 2rem 2rem 2rem;
            background: var(--bg-primary);
            min-height: 100vh;
            margin-left: var(--sidebar-width);
            padding-top: calc(var(--header-height) + 30px);
            box-sizing: border-box;
            transition: margin-left var(--transition-speed) ease;
        }

        body.sidebar-collapsed .balance-container {
            margin-left: var(--sidebar-collapsed-width);
        }

        /* 
           PAGE HEADER
           */
        .page-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 1.5rem;
            padding-bottom: 1rem;
            border-bottom: 3px solid var(--sbs-gold);
        }

        .page-header-left {
            display: flex;
            align-items: center;
            gap: 1rem;
        }

        .page-icon {
            width: 48px;
            height: 48px;
            background: linear-gradient(135deg, var(--sbs-navy), var(--sbs-navy-light));
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.25rem;
            color: var(--sbs-gold);
            box-shadow: 0 4px 12px rgba(30, 58, 95, 0.3);
        }

        .page-title h1 {
            font-size: 1.5rem;
            font-weight: 700;
            color: var(--sbs-navy-dark);
            margin: 0;
        }

        .page-title p {
            color: var(--text-secondary);
            font-size: 0.8125rem;
            margin: 0;
        }

        /* 
           FILTROS
          */
        .filters-card {
            display: flex;
            gap: 1rem;
            margin-bottom: 1.5rem;
            flex-wrap: wrap;
            align-items: flex-end;
            background: var(--bg-white);
            padding: 1rem 1.25rem;
            border-radius: 10px;
            border: 1px solid var(--border-color);
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
        }

        .filter-group {
            display: flex;
            flex-direction: column;
            gap: 0.375rem;
        }

            .filter-group label {
                font-size: 0.6875rem;
                font-weight: 600;
                text-transform: uppercase;
                letter-spacing: 0.5px;
                color: var(--text-secondary);
            }

            .filter-group select,
            .filter-group .form-control {
                background: var(--bg-white);
                border: 1px solid var(--border-color);
                border-radius: 6px;
                padding: 0.5rem 0.875rem;
                color: var(--text-dark);
                font-family: inherit;
                font-size: 0.875rem;
                min-width: 150px;
                height: 38px;
            }

                .filter-group select:focus,
                .filter-group .form-control:focus {
                    outline: none;
                    border-color: var(--sbs-gold);
                    box-shadow: 0 0 0 3px rgba(212, 167, 38, 0.15);
                }

        .btn-sbs {
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            padding: 0.5rem 1rem;
            border-radius: 6px;
            font-size: 0.8125rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
            border: none;
            height: 38px;
        }

        .btn-sbs-primary {
            background: var(--sbs-gold);
            color: var(--sbs-navy-dark);
        }

            .btn-sbs-primary:hover {
                background: var(--sbs-gold-light);
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(212, 167, 38, 0.4);
            }

        .btn-sbs-secondary {
            background: var(--sbs-navy);
            color: #fff;
        }

            .btn-sbs-secondary:hover {
                background: var(--sbs-navy-light);
            }

        /* 
           TARJETAS RESUMEN
          */
        .summary-cards {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
            gap: 1rem;
            margin-bottom: 1.5rem;
        }

        .summary-card {
            background: var(--bg-white);
            border: 1px solid var(--border-color);
            border-radius: 10px;
            padding: 1rem 1.25rem;
            position: relative;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.04);
        }

            .summary-card::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                height: 4px;
            }

            .summary-card.resultado::before {
                background: linear-gradient(90deg, var(--sbs-gold), var(--sbs-gold-light));
            }

            .summary-card.ingresos::before {
                background: linear-gradient(90deg, var(--positive), #2ecc71);
            }

            .summary-card.gastos::before {
                background: linear-gradient(90deg, var(--negative), #e67e73);
            }

            .summary-card.info::before {
                background: linear-gradient(90deg, var(--sbs-navy), var(--sbs-navy-light));
            }

        .summary-card-label {
            font-size: 0.6875rem;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: var(--text-secondary);
            margin-bottom: 0.375rem;
        }

        .summary-card-value {
            font-size: 1.375rem;
            font-weight: 700;
            font-family: 'Roboto Mono', monospace;
        }

            .summary-card-value.positive {
                color: var(--positive);
            }

            .summary-card-value.negative {
                color: var(--negative);
            }

            .summary-card-value.gold {
                color: var(--sbs-gold-dark);
            }

        /* 
           TABLA - CONTENEDOR
          */
        .table-container {
            background: var(--bg-white);
            border: 1px solid var(--border-color);
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 2px 12px rgba(0, 0, 0, 0.06);
        }

        .table-wrapper {
            overflow-x: auto;
        }

        /* 
           TABLA - ESTILOS BASE
           */
        #block_balance table {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.8125rem;
        }

        #block_balance thead th {
            background: var(--sbs-navy-dark);
            padding: 0.875rem 0.625rem;
            text-align: center;
            font-weight: 600;
            font-size: 0.6875rem;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: #fff;
            border-bottom: 3px solid var(--sbs-gold);
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
                background: var(--sbs-navy);
                color: var(--sbs-gold);
            }

        #block_balance td {
            padding: 0.625rem;
            text-align: right;
            border-bottom: 1px solid var(--border-light);
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

        #block_balance tbody tr {
            transition: background 0.15s ease;
        }

            #block_balance tbody tr:hover {
                background: var(--row-hover);
            }

        /* 
           TABLA - FILAS COLAPSABLES (TOGGLE)
            */

        /* Icono de toggle */
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

            .toggle-icon:hover {
                background: rgba(255,255,255,0.2);
            }

            .toggle-icon i {
                font-size: 0.65rem;
                transition: transform 0.2s ease;
            }

        /* Estado colapsado */
        tr.collapsed .toggle-icon i {
            transform: rotate(-90deg);
        }

        /* Filas hijas ocultas cuando el padre está colapsado */
        tr.child-hidden {
            display: none !important;
        }

        /* Animación suave para mostrar/ocultar */
        #block_balance tbody tr {
            transition: background 0.15s ease, opacity 0.2s ease;
        }

        /* 
           TABLA - NIVELES DE FILA
            */

        /* Row Level 0 - Categorías principales (RESULTADO, INGRESOS, GASTOS) */
        #block_balance tr.row-level-0 {
            background: var(--sbs-navy-dark) !important;
        }

            #block_balance tr.row-level-0 td {
                font-weight: 700 !important;
                font-size: 0.875rem !important;
                padding: 0.875rem 0.625rem !important;
                border-bottom: 2px solid var(--border-color) !important;
                color: #fff;
            }

                #block_balance tr.row-level-0 td:last-child {
                    background: var(--sbs-navy) !important;
                }

            #block_balance tr.row-level-0.resultado td {
                color: var(--sbs-gold);
            }

            #block_balance tr.row-level-0 .toggle-icon {
                background: rgba(212, 167, 38, 0.3);
            }

                #block_balance tr.row-level-0 .toggle-icon:hover {
                    background: rgba(212, 167, 38, 0.5);
                }

        /* Row Level 1 - Subcategorías principales */
        #block_balance tr.row-level-1 {
            background: rgba(30, 58, 95, 0.06);
        }

            #block_balance tr.row-level-1 td {
                font-weight: 600;
                color: var(--sbs-navy-dark);
            }

            #block_balance tr.row-level-1 .toggle-icon {
                background: rgba(30, 58, 95, 0.15);
                margin-left: 20px;
            }

                #block_balance tr.row-level-1 .toggle-icon:hover {
                    background: rgba(30, 58, 95, 0.25);
                }

                #block_balance tr.row-level-1 .toggle-icon i {
                    color: var(--sbs-navy);
                }

        /* Row Level 2 */
        #block_balance tr.row-level-2 td:first-child {
            color: var(--text-secondary);
            padding-left: 3.5rem;
        }

        #block_balance tr.row-level-2 .toggle-icon {
            background: rgba(90, 106, 122, 0.15);
            margin-left: 0;
        }

            #block_balance tr.row-level-2 .toggle-icon i {
                color: var(--text-secondary);
            }

        /* Row Level 3 - Detalle */
        #block_balance tr.row-level-3 td:first-child {
            color: var(--text-muted);
            font-style: italic;
            padding-left: 5rem;
        }

        /* 
           TABLA - COLORES DE VALORES
            */
        #block_balance .value-positive {
            color: var(--positive);
        }

        #block_balance .value-negative {
            color: var(--negative);
        }

        #block_balance .value-zero {
            color: var(--text-muted);
        }

        #block_balance tr.row-level-0 .value-positive,
        #block_balance tr.row-level-0 .value-negative,
        #block_balance tr.row-level-0 .value-zero {
            color: inherit;
        }

        #block_balance .red {
            color: var(--negative) !important;
        }

        #block_balance tr.row-level-0 .red {
            color: #ff8a80 !important;
        }

        /* 
           TABLA - LINKS
           */
        #block_balance td a {
            color: inherit;
            text-decoration: none;
            transition: color 0.2s;
        }

            #block_balance td a:hover {
                color: var(--sbs-gold);
                text-decoration: underline;
            }

        #block_balance tr.row-level-0 td a:hover {
            color: var(--sbs-gold-light);
        }

        /* 
           TABLA - FOOTER
           */
        .table-footer {
            padding: 0.875rem 1.25rem;
            background: var(--bg-primary);
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-top: 1px solid var(--border-color);
        }

        .table-footer-info {
            font-size: 0.75rem;
            color: var(--text-muted);
        }

            .table-footer-info i {
                color: var(--sbs-gold);
                margin-right: 0.375rem;
            }

        /* Botones de expandir/colapsar todo */
        .table-actions {
            display: flex;
            gap: 0.5rem;
        }

        .btn-table-action {
            padding: 0.375rem 0.75rem;
            font-size: 0.7rem;
            background: var(--bg-white);
            border: 1px solid var(--border-color);
            border-radius: 4px;
            cursor: pointer;
            transition: all 0.2s ease;
            color: var(--text-secondary);
        }

            .btn-table-action:hover {
                background: var(--sbs-gold);
                border-color: var(--sbs-gold);
                color: var(--sbs-navy-dark);
            }

            .btn-table-action i {
                margin-right: 0.375rem;
            }

        /* 
           RESPONSIVE
           */
        @media (max-width: 992px) {
            .balance-container {
                margin-left: var(--sidebar-collapsed-width);
            }

            .sidebar-toggle {
                left: calc(var(--sidebar-collapsed-width) - 15px);
            }

            body.sidebar-collapsed .sidebar-toggle {
                left: 15px;
            }

            body.sidebar-collapsed .balance-container {
                margin-left: 0;
            }
        }

        @media (max-width: 768px) {
            .balance-container {
                margin-left: 0;
                padding-top: calc(var(--header-height) + 20px);
                padding-left: 1rem;
                padding-right: 1rem;
            }

            .sidebar-toggle {
                display: none;
            }

            .page-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 1rem;
            }

            .filters-card {
                flex-direction: column;
            }

            .filter-group select,
            .filter-group .form-control {
                width: 100%;
            }

            .btn-sbs {
                width: 100%;
                justify-content: center;
            }

            .summary-cards {
                grid-template-columns: 1fr 1fr;
            }

            .summary-card-value {
                font-size: 1.1rem;
            }

            .table-footer {
                flex-direction: column;
                gap: 0.75rem;
            }
        }

        @media (max-width: 480px) {
            .summary-cards {
                grid-template-columns: 1fr;
            }
        }

        /* 
           PRINT
           */
        @media print {
            .filters-card, .page-header .header-actions, .sidebar-toggle, .table-actions {
                display: none;
            }

            .balance-container {
                margin-left: 0;
                padding-top: 0;
            }

            #block_balance thead th {
                background: var(--sbs-navy-dark) !important;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }

            #block_balance tr.row-level-0 {
                background: var(--sbs-navy-dark) !important;
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }

            /* Mostrar todo al imprimir */
            tr.child-hidden {
                display: table-row !important;
            }
        }
    </style>
</head>
<body>
    <uc_menu:menu ID="menu" runat="server" />
    <header id="header" class="bg-color-primary affix">
        <uc_header:cabecera ID="cabecera" runat="server" />
    </header>

    <!-- Botón toggle sidebar -->
    <button type="button" class="sidebar-toggle" onclick="toggleSidebar()" title="Colapsar/Expandir menú">
        <i class="fas fa-chevron-left"></i>
    </button>

    <form id="form1" runat="server">
        <div class="balance-container">
            <!-- Page Header -->
            <header class="page-header">
                <div class="page-header-left">
                    <div class="page-icon">
                        <i class="fas fa-chart-bar"></i>
                    </div>
                    <div class="page-title">
                        <h1>Balance Acumulado</h1>
                        <p>Resumen financiero por período</p>
                    </div>
                </div>
            </header>

            <!-- Filters -->
            <div class="filters-card">
                <div class="filter-group">
                    <label>Sociedad</label>
                     <%-- <asp:DropDownList ID="ddlSociety" CssClass="form-control" ToolTip="Sociedad" runat="server">
                      <asp:ListItem Text="Todas" Value="" />
                        <asp:ListItem Text="SBS" Value="SBS" />
                        <asp:ListItem Text="SBSCS" Value="SBSCS" />
                    </asp:DropDownList>--%>
                    <select id="ddlSociety" class="form-control" title="Sociedad" runat="server">
                        
                    </select>
                </div>
                <div class="filter-group">
                    <label>Año</label>
                    <asp:DropDownList ID="ddlYear" CssClass="form-control" ToolTip="Año" runat="server" />
                </div>
                <asp:Button ID="btn_search" runat="server" Text="Buscar" CssClass="btn-sbs btn-sbs-secondary" OnClick="btn_search_Click" />
                <asp:Button ID="btn_excel" runat="server" Text="Exportar Excel" CssClass="btn-sbs btn-sbs-primary" OnClick="btn_excel_Click" />
            </div>

            <!-- Summary Cards -->
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

            <!-- Table -->
            <div class="table-container">
                <div class="table-wrapper">
                    <div id="block_balance" runat="server"></div>
                </div>
                <div class="table-footer">
                    <div class="table-footer-info">
                        <i class="fas fa-clock"></i>Última actualización:
                        <asp:Label ID="lblLastUpdate" runat="server" />
                    </div>
                    <div class="table-actions">
                        <button type="button" class="btn-table-action" onclick="expandAll()">
                            <i class="fas fa-expand-alt"></i>Expandir todo
                        </button>
                        <button type="button" class="btn-table-action" onclick="collapseAll()">
                            <i class="fas fa-compress-alt"></i>Colapsar todo
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- JS base -->
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>

    <script type="text/javascript">
        // ============================================
        // SIDEBAR TOGGLE
        // ============================================

        // Función para encontrar el elemento sidebar
        function getSidebarElement() {
            // Intenta varios selectores comunes
            var selectors = [
                '.valkyrie-nav',
                '.nav-container',
                '.sidebar',
                '.main-nav',
                'nav.nav',
                '#nav',
                'aside.sidebar',
                '.left-sidebar',
                '.side-menu',
                'nav[class*="nav"]',
                'aside[class*="side"]',
                'div[class*="sidebar"]'
            ];

            for (var i = 0; i < selectors.length; i++) {
                var element = document.querySelector(selectors[i]);
                if (element && element.offsetHeight > 100) { // Debe ser un elemento visible grande
                    return element;
                }
            }

            // Buscar por posición (elemento fijo a la izquierda)
            var allElements = document.querySelectorAll('nav, aside, div');
            for (var j = 0; j < allElements.length; j++) {
                var el = allElements[j];
                var style = window.getComputedStyle(el);
                if ((style.position === 'fixed' || style.position === 'absolute') &&
                    parseInt(style.left) < 50 &&
                    parseInt(style.width) > 150 &&
                    parseInt(style.height) > 300) {
                    return el;
                }
            }

            return null;
        }

        function toggleSidebar() {
            var body = document.body;
            var sidebar = getSidebarElement();

            body.classList.toggle('sidebar-collapsed');

            // Aplicar estilos directamente al sidebar si se encontró
            if (sidebar) {
                if (body.classList.contains('sidebar-collapsed')) {
                    sidebar.style.width = '60px';
                    sidebar.style.overflow = 'hidden';

                    // Ocultar textos del menú
                    var texts = sidebar.querySelectorAll('span, .nav-text, .menu-text');
                    texts.forEach(function (text) {
                        text.style.opacity = '0';
                        text.style.width = '0';
                        text.style.overflow = 'hidden';
                    });
                } else {
                    sidebar.style.width = '';
                    sidebar.style.overflow = '';

                    // Mostrar textos del menú
                    var texts = sidebar.querySelectorAll('span, .nav-text, .menu-text');
                    texts.forEach(function (text) {
                        text.style.opacity = '';
                        text.style.width = '';
                        text.style.overflow = '';
                    });
                }
            }

            // Guardar preferencia
            localStorage.setItem('sidebarCollapsed', body.classList.contains('sidebar-collapsed'));

            // Log para debug
            console.log('Sidebar element:', sidebar);
            console.log('Sidebar collapsed:', body.classList.contains('sidebar-collapsed'));
        }

        // Restaurar estado del sidebar al cargar
        document.addEventListener('DOMContentLoaded', function () {
            var isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
            var sidebar = getSidebarElement();

            if (isCollapsed) {
                document.body.classList.add('sidebar-collapsed');

                if (sidebar) {
                    sidebar.style.width = '60px';
                    sidebar.style.overflow = 'hidden';

                    var texts = sidebar.querySelectorAll('span, .nav-text, .menu-text');
                    texts.forEach(function (text) {
                        text.style.opacity = '0';
                        text.style.width = '0';
                        text.style.overflow = 'hidden';
                    });
                }
            }

            // Log para debug - muestra en consola qué elemento es el sidebar
            console.log('Detected sidebar element:', sidebar);
            if (sidebar) {
                console.log('Sidebar classes:', sidebar.className);
                console.log('Sidebar ID:', sidebar.id);
            }

            // Inicializar toggles de la tabla
            initTableToggles();
        });

        // ============================================
        // TABLA - TOGGLE DE CATEGORÍAS
        // ============================================
        function initTableToggles() {
            // Buscar todas las filas con toggle
            var toggleIcons = document.querySelectorAll('#block_balance .toggle-icon');

            toggleIcons.forEach(function (icon) {
                icon.addEventListener('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var row = this.closest('tr');
                    toggleRow(row);
                });
            });
        }

        function toggleRow(row) {
            var isCollapsed = row.classList.contains('collapsed');
            var rowId = row.getAttribute('data-row-id');

            if (isCollapsed) {
                // Expandir
                row.classList.remove('collapsed');
                showChildren(rowId);
            } else {
                // Colapsar
                row.classList.add('collapsed');
                hideChildren(rowId);
            }
        }

        function showChildren(parentId) {
            var children = document.querySelectorAll('tr[data-parent-id="' + parentId + '"]');

            children.forEach(function (child) {
                child.classList.remove('child-hidden');

                // Si el hijo tiene sus propios hijos y NO está colapsado, mostrarlos también
                var childId = child.getAttribute('data-row-id');
                if (childId && !child.classList.contains('collapsed')) {
                    showChildren(childId);
                }
            });
        }

        function hideChildren(parentId) {
            var children = document.querySelectorAll('tr[data-parent-id="' + parentId + '"]');

            children.forEach(function (child) {
                child.classList.add('child-hidden');

                // También ocultar los hijos de este hijo (recursivo)
                var childId = child.getAttribute('data-row-id');
                if (childId) {
                    hideChildren(childId);
                }
            });
        }

        function expandAll() {
            var rows = document.querySelectorAll('#block_balance tr[data-row-id]');

            rows.forEach(function (row) {
                row.classList.remove('collapsed');
            });

            var hiddenRows = document.querySelectorAll('#block_balance tr.child-hidden');
            hiddenRows.forEach(function (row) {
                row.classList.remove('child-hidden');
            });
        }

        function collapseAll() {
            // Colapsar todas las filas de nivel 0 y 1
            var parentRows = document.querySelectorAll('#block_balance tr[data-row-id]');

            parentRows.forEach(function (row) {
                row.classList.add('collapsed');
                var rowId = row.getAttribute('data-row-id');
                hideChildren(rowId);
            });
        }

        // Re-inicializar después de un postback (búsqueda)
        if (typeof Sys !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initTableToggles();
            });
        }
    </script>
</body>
</html>
