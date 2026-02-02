<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_Gastos.aspx.cs" Inherits="campus_sbs_admin.Admin_Gastos" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>
<html class="no-legacy-ie no-js" lang="es">
<head runat="server">
    <title>SBS | Gastos</title>

    <link rel="stylesheet" href="App_Themes/support/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    <link href="App_Themes/support/css/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@6.5.2/css/all.min.css" />

    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>

    <style type="text/css">
        .content-wrapper { margin-left: 260px; padding: 20px; }
        @media (max-width: 768px) { .content-wrapper { margin-left: 0; padding: 15px; } }

        .card { border: none; box-shadow: 0 2px 12px rgba(0,0,0,.06); border-radius: 12px; }
        .card-header { background: #fff; border-bottom: 1px solid #eee; border-top-left-radius: 12px; border-top-right-radius: 12px; }

        .section-title { font-weight: 700; font-size: 22px; margin: 10px 0 14px; }
        input, select, textarea { border-radius: 10px !important; }
        .textolabel18 { font-size: 18px; }

        .calc-readonly { background: #f8f9fa !important; cursor: not-allowed; }
        .muted-small { font-size: 13px; color: #6c757d; }

        .modal { z-index: 10550; }
        .modal-backdrop { z-index: 10540; }

        .d-none-imp { display: none !important; }
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
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

                <div class="container-fluid">
                    <div class="card">
                        <div class="card-header">
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <h3 class="mb-0">Registro de Gastos</h3>
                                    <div class="muted-small">Formulario de alta / edición</div>
                                </div>

                                <div class="d-flex align-items-center" style="gap:10px;">
                                    <!-- Ir a consulta -->
                                    <a href="admin_gastos_list.aspx" class="btn btn-outline-warning textolabel18">
                                        <i class="fa-solid fa-list mr-2"></i>Ver consulta
                                    </a>

                                    <!-- Eliminar (solo en edición) -->
                                    <button type="button" id="btnDeleteAjax" class="btn btn-outline-danger textolabel18 d-none-imp">
                                        <i class="fa-solid fa-trash mr-2"></i>Eliminar
                                    </button>

                                    <div id="block_result" runat="server" class="d-none-imp">
                                        <div class="alert alert-danger mb-0 py-2 px-3">
                                            <span id="lbl_result" runat="server"></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card-body">

                            <div class="section-title">Datos Básicos</div>

                            <div class="form-row align-items-center">
                                <div class="form-group col-md-2">
                                    <select id="ddlyear" runat="server" class="form-control textolabel18" title="Año"></select>
                                </div>

                                <div class="form-group col-md-3">
                                    <select id="basic_sociedad" runat="server" class="form-control textolabel18" title="Sociedad"></select>
                                </div>

                                <div class="form-group col-md-2">
                                    <div class="muted-small">Último nº <span id="txt_num" runat="server">000</span></div>
                                </div>

                                <div class="form-group col-md-2">
                                    <input type="text" id="basic_secuence" runat="server" class="form-control textolabel18" placeholder="Nº Secuencia *" />
                                </div>

                                <div class="form-group col-md-3">
                                    <input type="text" id="date_emision" runat="server" class="form-control textolabel18 js-date" placeholder="Fecha Emisión *" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-md-6">
                                    <input type="text" id="basic_company" runat="server" class="form-control textolabel18" placeholder="Empresa *" maxlength="200" />
                                </div>

                                <div class="form-group col-md-6">
                                    <input type="text" id="basic_cif" runat="server" class="form-control textolabel18" placeholder="CIF *" maxlength="20" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-12">
                                    <textarea id="basic_description" runat="server" class="form-control textolabel18" placeholder="Descripción *" rows="3"></textarea>
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-md-3">
                                    <input type="text" id="basic_subtotal" runat="server" class="form-control textolabel18" placeholder="SubTotal *" />
                                </div>
                                <div class="form-group col-md-3">
                                    <input type="text" id="basic_iva" runat="server" class="form-control textolabel18" placeholder="IVA" />
                                </div>
                                <div class="form-group col-md-3">
                                    <input type="text" id="basic_irpf" runat="server" class="form-control textolabel18" placeholder="IRPF" />
                                </div>
                                <div class="form-group col-md-3">
                                    <input type="text" id="basic_total" runat="server" class="form-control textolabel18 calc-readonly" placeholder="Total" readonly="readonly" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-12">
                                    <div class="custom-control custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="basic_chk" runat="server" />
                                        <label class="custom-control-label textolabel18" for="basic_chk">¿Es una liquidación de honorarios (LH)?</label>
                                    </div>
                                </div>
                            </div>

                            <div id="basic_LH" runat="server" class="border rounded p-3 mb-3 d-none-imp">
                                <div class="form-row">
                                    <div class="form-group col-md-5">
                                        <label class="textolabel18 mb-1">Profesor</label>
                                        <select id="ddlTeacher" runat="server" class="form-control textolabel18"></select>
                                    </div>

                                    <div class="form-group col-md-2">
                                        <label class="textolabel18 mb-1">LH Año</label>
                                        <input type="text" id="basic_LH_year" runat="server" class="form-control textolabel18" />
                                    </div>

                                    <div class="form-group col-md-2">
                                        <label class="textolabel18 mb-1">LH Nº</label>
                                        <input type="text" id="basic_LH_number" runat="server" class="form-control textolabel18" />
                                    </div>

                                    <div class="form-group col-md-3">
                                        <label class="textolabel18 mb-1">Tipo liquidación</label>
                                        <select id="tipo_liquidacion" runat="server" class="form-control textolabel18">
                                            <option value="0">Seleccione un tipo</option>
                                            <option value="1">Online</option>
                                            <option value="2">Clase</option>
                                            <option value="3">Otros</option>
                                        </select>
                                    </div>
                                </div>

                                <div class="form-row">
                                    <div class="form-group col-12 mb-0">
                                        <textarea id="basic_LH_comment" runat="server" class="form-control textolabel18" placeholder="Comentarios LH" rows="2"></textarea>
                                    </div>
                                </div>
                            </div>

                            <hr class="my-4" />

                            <div class="section-title">Catalogación</div>

                            <div class="form-row">
                                <div class="form-group col-md-4">
                                    <select id="ddlArea" runat="server" class="form-control textolabel18"></select>
                                </div>
                                <div class="form-group col-md-4">
                                    <select id="ddlSubArea" runat="server" class="form-control textolabel18"></select>
                                </div>
                                <div class="form-group col-md-4">
                                    <select id="ddlSubArea2" runat="server" class="form-control textolabel18"></select>
                                </div>
                            </div>

                            <hr class="my-4" />

                            <div class="section-title">Contable</div>

                            <div class="form-row align-items-center">
                                <div class="form-group col-md-4">
                                    <input type="text" id="date_pay" runat="server" class="form-control textolabel18 js-date" placeholder="Fecha Pago" />
                                </div>

                                <div class="form-group col-md-4">
                                    <input type="text" id="basic_banc" runat="server" class="form-control textolabel18" placeholder="Apunte Banco" maxlength="200" />
                                </div>

                                <div class="form-group col-md-2">
                                    <div class="custom-control custom-checkbox mt-2">
                                        <input type="checkbox" class="custom-control-input" id="basic_chk_fact" runat="server" />
                                        <label class="custom-control-label textolabel18" for="basic_chk_fact">¿No Factura?</label>
                                    </div>
                                </div>

                                <div class="form-group col-md-2">
                                    <div class="custom-control custom-checkbox mt-2">
                                        <input type="checkbox" class="custom-control-input" id="basic_chk_provision" runat="server" />
                                        <label class="custom-control-label textolabel18" for="basic_chk_provision">Provisión</label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-row align-items-center">
                                <div class="form-group col-md-6">
                                    <asp:FileUpload ID="fuFile" runat="server" CssClass="form-control-file textolabel18" />
                                    <small class="text-muted">Seleccione un archivo (opcional)</small>
                                </div>

                                <div class="form-group col-md-6 text-right">
                                    <a id="lnkFile" runat="server" target="_blank" class="btn btn-outline-secondary btn-sm d-none-imp">Ver archivo</a>
                                    <asp:Button ID="btn_del_file" runat="server" CssClass="btn btn-outline-danger btn-sm d-none-imp"
                                        Text="Eliminar archivo" OnClick="btn_del_file_Click" OnClientClick="showSaving();" />
                                </div>
                            </div>

                            <div class="form-row align-items-center">
                                <div class="form-group col-md-3">
                                    <div class="custom-control custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="basic_tax_chk" runat="server" />
                                        <label class="custom-control-label textolabel18" for="basic_tax_chk">Impuesto</label>
                                    </div>
                                </div>

                                <div class="form-group col-md-3 d-none-imp" id="basic_tax" runat="server">
                                    <select id="ddlTax" runat="server" class="form-control textolabel18">
                                        <option value="">Seleccione un impuesto</option>
                                        <option value="111-190">111-190</option>
                                        <option value="303-390">303-390</option>
                                        <option value="202-200">202-200</option>
                                        <option value="115-190">115-190</option>
                                    </select>
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="form-group col-12">
                                    <textarea id="basic_comments" runat="server" class="form-control textolabel18" placeholder="Comentarios" rows="3"></textarea>
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col-md-6 mb-2">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary btn-block textolabel18"
                                        Text="Cancelar" OnClick="btnCancel_Click" />
                                </div>

                                <div class="col-md-6 mb-2">
                                    <button type="button" id="btnSaveAjax" class="btn btn-success btn-block textolabel18">
                                        Guardar
                                    </button>
                                </div>
                            </div>

                            <input id="basic_id" type="hidden" runat="server" />

                        </div>
                    </div>
                </div>

                <!-- Modal Guardando -->
                <div class="modal fade" id="mdSaving" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-body d-flex align-items-center">
                                <div class="spinner-border mr-3" role="status" aria-hidden="true"></div>
                                <div>
                                    <h5 class="mb-1">Guardando...</h5>
                                    <small class="text-muted">Por favor espere</small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal Confirm Delete -->
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
                                ¿Seguro que deseas eliminar este gasto? (Se eliminará el archivo adjunto si existe)
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                                <button type="button" class="btn btn-danger" id="btnConfirmDelete">
                                    Sí, eliminar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal Info -->
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
    <script type="text/javascript" src="App_Themes/support/js/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="App_Themes/support/js/bootstrap-datepicker.es.js"></script>

    <script type="text/javascript">
        function showSaving() { if (window.jQuery && $.fn.modal) $('#mdSaving').modal('show'); }
        function hideSaving() { if (window.jQuery && $.fn.modal) $('#mdSaving').modal('hide'); }

        function showInfo(title, htmlOrText, reloadOnClose) {
            $('#mdInfoTitle').text(title || 'Mensaje');
            $('#mdInfoBody').html(htmlOrText || '');
            if ($.fn.modal) $('#mdInfo').modal('show');

            $('#mdInfo').off('hidden.bs.modal');
            if (reloadOnClose) {
                $('#mdInfo').one('hidden.bs.modal', function () { window.location.reload(); });
            }
        }

        function parseDec(v) {
            v = (v || '').toString().trim();
            if (!v) return 0;
            v = v.replace(/\s/g, '');
            if (v.indexOf(',') >= 0) v = v.replace(/\./g, '').replace(',', '.');
            var n = Number(v);
            return isNaN(n) ? 0 : n;
        }

        function formatDec(n) {
            if (n === null || n === undefined) return '';
            return Number(n).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        }

        function recalcTotalGasto() {
            var sub = parseDec($('#<%= basic_subtotal.ClientID %>').val());
            var iva = parseDec($('#<%= basic_iva.ClientID %>').val());
            var irpf = parseDec($('#<%= basic_irpf.ClientID %>').val());
            var t = sub + iva - irpf;
            $('#<%= basic_total.ClientID %>').val(formatDec(t));
        }

        function toggleLH() {
            var chk = $('#<%= basic_chk.ClientID %>').is(':checked');
            var $b = $('#<%= basic_LH.ClientID %>');
            if (chk) $b.removeClass('d-none-imp'); else $b.addClass('d-none-imp');
        }

        function toggleTax() {
            var chk = $('#<%= basic_tax_chk.ClientID %>').is(':checked');
            var $b = $('#<%= basic_tax.ClientID %>');
            if (chk) $b.removeClass('d-none-imp'); else $b.addClass('d-none-imp');
        }

        function validateBeforeSubmitGasto() {
            var errors = [];
            var sociedad = $('#<%= basic_sociedad.ClientID %>').val();
            var secuencia = $('#<%= basic_secuence.ClientID %>').val();
            var femision = $('#<%= date_emision.ClientID %>').val();
            var empresa = $('#<%= basic_company.ClientID %>').val();
            var cif = $('#<%= basic_cif.ClientID %>').val();
            var desc = $('#<%= basic_description.ClientID %>').val();
            var subtotal = $('#<%= basic_subtotal.ClientID %>').val();

            if (!sociedad) errors.push('Seleccione la sociedad.');
            if (!secuencia || Number(secuencia) <= 0) errors.push('La secuencia es obligatoria y debe ser mayor que 0.');
            if (!femision) errors.push('La fecha de emisión es obligatoria.');
            if (!empresa || empresa.trim().length < 2) errors.push('La empresa es obligatoria.');
            if (!cif || cif.trim().length < 2) errors.push('El CIF es obligatorio.');
            if (!desc || desc.trim().length < 3) errors.push('La descripción es obligatoria.');
            if (!subtotal || subtotal.trim().length === 0) errors.push('El subtotal es obligatorio.');

            var sub = parseDec(subtotal);
            if (!isNaN(sub) && sub <= 0) errors.push('El subtotal debe ser mayor que 0.');

            var tax = $('#<%= basic_tax_chk.ClientID %>').is(':checked');
            var taxType = $('#<%= ddlTax.ClientID %>').val();
            if (tax && !taxType) errors.push('Al marcar impuesto, seleccione el tipo.');

            var isLH = $('#<%= basic_chk.ClientID %>').is(':checked');
            if (isLH) {
                var teacher = $('#<%= ddlTeacher.ClientID %>').val();
                var lhy = $('#<%= basic_LH_year.ClientID %>').val();
                var lhn = $('#<%= basic_LH_number.ClientID %>').val();
                if (!teacher) errors.push('Al marcar LH, seleccione profesor.');
                if (!lhy) errors.push('Al marcar LH, LH Año es obligatorio.');
                if (!lhn) errors.push('Al marcar LH, LH Nº es obligatorio.');
            }

            if (errors.length > 0) {
                hideSaving();
                var html = '<ul style="margin:0;padding-left:18px;text-align:left;">' +
                    errors.map(function (e) { return '<li>' + e + '</li>'; }).join('') +
                    '</ul>';
                showInfo('Revisa el formulario', html, false);
                return false;
            }
            return true;
        }

        function loadSecuence() {
            var soc = $('#<%= basic_sociedad.ClientID %>').val();
            var y = $('#<%= ddlyear.ClientID %>').val();
            if (!soc || !y) return;

            PageMethods.searchSecuence(soc, y, function (next) {
                // next ya viene en formato "000"
                $('#<%= basic_secuence.ClientID %>').val(next || '000');

                // mostrar "último" = next-1
                var n = parseInt(next || '0', 10);
                var last = (n > 0) ? (n - 1).toString().padStart(3, '0') : '000';
                $('#<%= txt_num.ClientID %>').text(last);

            }, function (err) {
                console.error(err);
                showInfo('Error', 'No se pudo obtener la secuencia.', false);
            });
        }

        function loadSub(table, parentId, ddlClientId) {
            if (!parentId) {
                $('#' + ddlClientId).empty().append($('<option/>', {
                    value: '',
                    text: (ddlClientId === '<%= ddlSubArea2.ClientID %>' ? 'Seleccione un Subárea 2' : 'Seleccione un Subárea')
                }));
                return;
            }

            PageMethods.searchSubArea(table, parentId.toString(),
                function (list) {
                    var $ddl = $('#' + ddlClientId);
                    $ddl.empty();

                    var placeholder = (ddlClientId === '<%= ddlSubArea2.ClientID %>')
                        ? 'Seleccione un Subárea 2'
                        : 'Seleccione un Subárea';

                    $ddl.append($('<option/>', { value: '', text: placeholder }));

                    (list || []).forEach(function (x) {
                        $ddl.append($('<option/>', { value: x.id_inf_aux, text: x.Valor }));
                    });
                },
                function (err) {
                    console.error(err);
                    showInfo('Error', 'No se pudieron cargar las subáreas.', false);
                }
            );
        }

        function buildSaveRequest() {
            return {
                id: Number($('#<%= basic_id.ClientID %>').val() || 0),

                sociedad: $('#<%= basic_sociedad.ClientID %>').val(),
                secuencia: Number($('#<%= basic_secuence.ClientID %>').val() || 0),
                fecha_emision: $('#<%= date_emision.ClientID %>').val(),

                empresa: $('#<%= basic_company.ClientID %>').val(),
                cif: $('#<%= basic_cif.ClientID %>').val(),
                descripcion: $('#<%= basic_description.ClientID %>').val(),

                subtotal: $('#<%= basic_subtotal.ClientID %>').val(),
                iva: $('#<%= basic_iva.ClientID %>').val(),
                irpf: $('#<%= basic_irpf.ClientID %>').val(),
                total: $('#<%= basic_total.ClientID %>').val(),

                area: $('#<%= ddlArea.ClientID %>').val(),
                subarea: $('#<%= ddlSubArea.ClientID %>').val(),
                subarea2: $('#<%= ddlSubArea2.ClientID %>').val(),

                fecha_pago: $('#<%= date_pay.ClientID %>').val(),
                apunte_banco: $('#<%= basic_banc.ClientID %>').val(),

                no_factura: $('#<%= basic_chk_fact.ClientID %>').is(':checked'),
                provision: $('#<%= basic_chk_provision.ClientID %>').is(':checked'),

                tax: $('#<%= basic_tax_chk.ClientID %>').is(':checked'),
                tax_type: $('#<%= ddlTax.ClientID %>').val(),

                is_lh: $('#<%= basic_chk.ClientID %>').is(':checked'),
                teacher: $('#<%= ddlTeacher.ClientID %>').val(),
                lh_year: $('#<%= basic_LH_year.ClientID %>').val(),
                lh_number: $('#<%= basic_LH_number.ClientID %>').val(),
                lh_comment: $('#<%= basic_LH_comment.ClientID %>').val(),
                tipo_liquidacion: $('#<%= tipo_liquidacion.ClientID %>').val(),

                comentarios: $('#<%= basic_comments.ClientID %>').val(),
                fichero: null
            };
        }

        function saveGastoAjax() {
            if (!validateBeforeSubmitGasto()) return;

            recalcTotalGasto();
            showSaving();

            var req = buildSaveRequest();

            $.ajax({
                url: 'Admin_Gastos.aspx/SaveGasto',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ r: req }),
                success: function (res) {
                    hideSaving();
                    var payload = (res && res.d) ? res.d : null;

                    if (!payload || payload.ok !== true) {
                        showInfo('Error', (payload && payload.message) ? payload.message : 'No se pudo guardar.', false);
                        return;
                    }

                    $('#<%= basic_id.ClientID %>').val(payload.id || 0);
                    setSaveButtonMode(); 


                    // mostrar botón eliminar cuando ya hay ID
                    if ((payload.id || 0) > 0) $('#btnDeleteAjax').removeClass('d-none-imp');

                    loadSecuence();
                    showInfo('OK', payload.message || 'Gasto guardado correctamente.', false);
                },
                error: function (xhr) {
                    hideSaving();
                    console.error(xhr);
                    showInfo('Error', 'Error de comunicación al guardar.', false);
                }
            });
        }
        function setSaveButtonMode() {
            var id = Number($('#<%= basic_id.ClientID %>').val() || 0);

            if (id > 0) {
                $('#btnSaveAjax').text('Actualizar');
            } else {
                $('#btnSaveAjax').text('Guardar');
            }
        }

        // ======= DELETE EN EDICIÓN (AJAX) =======
        function openDeleteModal() { $('#mdConfirmDelete').modal('show'); }

        function deleteGastoAjax() {
            var id = Number($('#<%= basic_id.ClientID %>').val() || 0);
            if (!id) return showInfo('Error', 'No hay un gasto cargado para eliminar.', false);

            showSaving();
            PageMethods.DeleteGasto(id,
                function (r) {
                    hideSaving();
                    if (!r || r.ok !== true) return showInfo('Error', (r && r.message) ? r.message : 'No se pudo eliminar.', false);
                    showInfo('OK', r.message || 'Eliminado.', true);
                    // al cerrar modal info recarga, o redirige:
                    setTimeout(function () { window.location.href = 'admin_gastos_list.aspx'; }, 900);
                },
                function (err) {
                    hideSaving();
                    console.error(err);
                    showInfo('Error', 'No se pudo eliminar.', false);
                }
            );
        }

        $(document).ready(function () {
            $('.js-date').datepicker({
                language: "es",
                autoclose: true,
                todayHighlight: true,
                format: "dd/mm/yyyy"
            });

            $('#<%= basic_chk.ClientID %>').on('change', toggleLH);
            $('#<%= basic_tax_chk.ClientID %>').on('change', toggleTax);

            $('#<%= basic_subtotal.ClientID %>,#<%= basic_iva.ClientID %>,#<%= basic_irpf.ClientID %>')
                .on('input', recalcTotalGasto);

            $('#<%= basic_sociedad.ClientID %>').on('change', loadSecuence);

            $('#<%= ddlyear.ClientID %>').on('change', function () {
                var y = $(this).val();
                if (!y) return;

                var newDate = '01/01/' + y;
                var $emision = $('#<%= date_emision.ClientID %>');
                $emision.val(newDate);
                if ($emision.data('datepicker')) $emision.datepicker('update', newDate);

                loadSecuence();
            });

            $('#<%= ddlArea.ClientID %>').on('change', function () {
                loadSub('SUBAREA', $(this).val(), '<%= ddlSubArea.ClientID %>');
                loadSub('SUBAREA2', '', '<%= ddlSubArea2.ClientID %>');
            });

            $('#<%= ddlSubArea.ClientID %>').on('change', function () {
                loadSub('SUBAREA2', $(this).val(), '<%= ddlSubArea2.ClientID %>');
            });

            $('#btnSaveAjax').on('click', saveGastoAjax);

            // delete
            $('#btnDeleteAjax').on('click', openDeleteModal);
            $('#btnConfirmDelete').on('click', function () {
                $('#mdConfirmDelete').modal('hide');
                deleteGastoAjax();
            });

            // init
            toggleLH();
            toggleTax();
            recalcTotalGasto();

            // mostrar eliminar si hay ID (edición)
            var currentId = Number($('#<%= basic_id.ClientID %>').val() || 0);
            if (currentId > 0) $('#btnDeleteAjax').removeClass('d-none-imp');

            setSaveButtonMode();
        });
    </script>

</body>
</html>
