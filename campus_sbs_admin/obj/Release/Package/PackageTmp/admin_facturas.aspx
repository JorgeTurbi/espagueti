<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_facturas.aspx.cs" Inherits="campus_sbs_admin.admin_facturas" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!-->
<html class="no-legacy-ie no-js" lang="es">
<!--<![endif]-->
<head runat="server">
    <title>SBS | Facturas</title>

    <link rel="stylesheet" href="App_Themes/support/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    <link href="App_Themes/support/css/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/datatables.min.css" />
    <link href="https://fonts.googleapis.com/css?family=Lato:300,400,700,900%7COpen+Sans:400,600,700" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js" async></script>

    <style type="text/css">
        .content-wrapper { margin-left: 260px; padding: 20px; }
        @media (max-width: 768px) { .content-wrapper { margin-left: 0; padding: 15px; } }

        .card { border: none; }
        input, select, textarea { border-radius: 10px !important; }
        .textolabel18 { font-size: 18px; }

        .box { resize: both; overflow: auto; width: 100%; min-height: 1% !important; }

        /* inputs calculados */
        .calc-readonly {
            background: #f8f9fa !important;
            cursor: not-allowed;
        }

        /* Modal stacking */
        .modal { z-index: 10550; }
        .modal-backdrop { z-index: 10540; }
    </style>
</head>
<body>
    <uc_menu:menu ID="menu" runat="server" />
    <header id="header" class="bg-color-primary affix">
        <uc_header:cabecera ID="cabecera" runat="server" />
    </header>

    <div class="content-wrapper">
        <main class="wrapper public bg-color-white" role="main">
            <h3 class="display-4 text-center">Registro de Facturas</h3>

            <form class="form-row" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />

                <section class="padding-tb-40 padding-xs-tb-30">
                    <div class="container-fluid">
                        <div class="card">
                            <div class="card-header">
                                <div class="d-flex justify-content-around">
                                    <div class="d-flex align-items-center mr-4">
                                        <h5 class="mb-0 mr-2">Año</h5>
                                        <select id="ddlyear" runat="server" title="Año Fiscal" class="custom-select custom-select-sm" style="width: 270px; height: 40px; font-size: 18px;"></select>
                                    </div>

                                    <div class="d-flex align-items-center mr-4">
                                        <h5 class="mb-0 mr-1 textolabel18">SBS:</h5>
                                        <h5 id="sbs_total" class="mb-0 mr-0 font-weight-bold textolabel18">0</h5>
                                    </div>

                                    <div class="d-flex align-items-center">
                                        <h5 class="mb-0 mr-1 textolabel18">SBSCS:</h5>
                                        <h5 id="sbscs_total" class="mb-0 mr-0 font-weight-bold textolabel18">0</h5>
                                    </div>
                                </div>
                            </div>

                            <div class="card-body">

                                <div class="row my-2">
                                    <div class="d-flex justify-content-around">
                                        <div class="d-flex align-items-center mr-4 ml-4 py-3">
                                            <select id="slSociedad" runat="server" title="Socio" class="custom-select custom-select-sm" style="width: 270px; height: 40px; font-size: 18px;"></select>
                                        </div>

                                        <div class="d-flex align-items-center mr-4">
                                            <input type="number" id="secuencia" runat="server" title="Secuencia Siguiente" class="form-control textolabel18" placeholder="Secuencia siguiente" />
                                        </div>

                                        <div class="d-flex align-items-center mr-4">
                                            <h5 class="mb-0 mr-2">Fecha Emisión</h5>
                                            <input type="date" id="femision" runat="server" title="Fecha Emsión" class="form-control textolabel18 " placeholder="Fecha Emisión siguiente" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="d-flex justify-content-around w-100">
                                        <div class="col-md">
                                            <div class="form-group">
                                                <input type="text" runat="server" id="basic_client" placeholder="Cliente *" title="Nombre Cliente" class="form-control textolabel18" />
                                                <span id="viewclient" class="text-muted"></span>
                                            </div>
                                        </div>

                                        <div class="col-md">
                                            <div class="form-group">
                                                <input type="text" runat="server" id="basic_nif" placeholder="NIF *" title="Número de Identificación Fiscal" class="form-control textolabel18" />
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-md">
                                        <textarea placeholder="Descripción" runat="server" id="descripcion" class="form-control box py-3 textolabel18" title="Descripción" cols="3" rows="2"></textarea>
                                    </div>
                                </div>

                                <hr class="my-5" />

                                <!-- PVP - BECA - DTO -->
                                <div class="row my-3">
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_pvp" placeholder="PVP *" title="PVP" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_beca" placeholder="Beca" title="Beca" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_dto" placeholder="Descuento" title="Descuento" oninput="validatePriceLocale(this)" />
                                    </div>
                                </div>

                                <!-- PRECIO - FUND - UNI -->
                                <div class="row my-3">
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-readonly" runat="server" id="basic_precio" placeholder="Precio *" title="Precio" />
                                    </div>
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_fund" placeholder="Fundación" title="Fundación" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_uni" placeholder="Universidad" title="Universidad" oninput="validatePriceLocale(this)" />
                                    </div>
                                </div>

                                <!-- TRIP - IVA - IRPF - TOTAL -->
                                <div class="row my-3">
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_trip" placeholder="Tripartita" title="Tripartita" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_iva" placeholder="IVA" title="IVA" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-src" runat="server" id="basic_irpf" placeholder="IRPF" title="Impuesto sobre la Renta a Personas Físicas" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md">
                                        <input type="text" class="form-control textolabel18 calc-readonly" runat="server" id="basic_total" placeholder="Total *" title="Total *" />
                                    </div>
                                </div>

                                <div class="row my-2">
                                    <div class="col-md">
                                        <div class="form-group">
                                            <label class="col-form-label-sm textolabel18">Fecha Vencimiento</label>
                                            <input type="date" class="form-control textolabel18" id="date_venc" placeholder="Vencimiento" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md">
                                        <div class="form-group">
                                            <label class="col-form-label-sm textolabel18">Fecha Cobro</label>
                                            <input type="date" class="form-control textolabel18" id="date_payment" placeholder="Fecha Cobro" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row my-2">
                                    <div class="col-md-6">
                                        <asp:FileUpload ID="fuFactura" runat="server" Style="display: none;" />
                                        <button type="button" class="btn btn-primary btn-sm"
                                            onclick="document.getElementById('<%= fuFactura.ClientID %>').click();">
                                            <i class="fas fa-upload mr-2"></i>
                                            Cargar archivo
                                        </button>
                                        <span id="fileName" class="ml-2 text-muted small"></span>
                                    </div>

                                    <div class="col-md">
                                        <select id="ddlAtribucion" runat="server" title="Atribución" class="custom-select custom-select-md" style="height: 40px; font-size: 18px;"></select>
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-md">
                                        <textarea runat="server" id="comentario" placeholder="Comentarios" class="form-control box py-3 textolabel18" title="Comentarios" cols="3" rows="2"></textarea>
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-12 d-flex justify-content-center">
                                        <button type="button" runat="server" id="btnCancel" class="btn btn-danger mr-3">
                                            Cancelar
                                        </button>

                                        <button id="btnSave" type="button" class="btn btn-success ml-3">
                                            Registrar
                                        </button>

                                        <div>
                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary ml-3"
                                                Text="Consultar" OnClick="btnSearch_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </section>

                <!-- ===========================
                     MODALES BOOTSTRAP 4
                     =========================== -->

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

                <!-- Modal Info (éxito/error/validación) -->
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
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="App_Themes/support/js/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="App_Themes/support/js/bootstrap-datepicker.es.js"></script>

    <script type="text/javascript">

        // ===========================
        // Helpers UI (Modal)
        // ===========================
        function showSaving() { if ($.fn.modal) $('#mdSaving').modal('show'); }
        function hideSaving() { if ($.fn.modal) $('#mdSaving').modal('hide'); }
        function showInfo(title, htmlOrText) {
            $('#mdInfoTitle').text(title || 'Mensaje');
            $('#mdInfoBody').html(htmlOrText || '');
            if ($.fn.modal) $('#mdInfo').modal('show');
            else alert((title ? title + ': ' : '') + $(htmlOrText).text());
        }

        // ===========================
        // Locale number parse/format
        // ===========================
        function getDecimalSeparator() {
            return (1.1).toLocaleString().substring(1, 2);
        }

        function parseLocaleNumber(val) {
            if (val === null || val === undefined) return 0;

            const dec = getDecimalSeparator();
            val = (val + '').trim();
            if (!val) return 0;

            const thousands = (dec === ',') ? '.' : ',';
            val = val.split(thousands).join('');
            if (dec !== '.') val = val.replace(dec, '.');

            const num = Number(val);
            return isNaN(num) ? NaN : num;
        }

        function formatLocaleNumber(num) {
            const n = Number(num);
            if (isNaN(n)) return '';
            // España: 2 decimales
            return n.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        }

        function validatePriceLocale(input) {
            const decimalSeparator = getDecimalSeparator();
            const regexNotAllowed = new RegExp(`[^0-9${decimalSeparator}]`, 'g');
            const regexExtraDecimal = new RegExp(`\\${decimalSeparator}.*\\${decimalSeparator}`, 'g');

            input.value = input.value
                .replace(regexNotAllowed, '')
                .replace(regexExtraDecimal, match => match.slice(0, -1));
        }

        // ===========================
        // ✅ CÁLCULOS AUTOMÁTICOS
        // precio = pvp - beca - dto
        // total  = precio + fund + uni + iva + irpf
        // ===========================
        function recalcFactura() {
            const $pvp = $('#<%= basic_pvp.ClientID %>');
            const $beca = $('#<%= basic_beca.ClientID %>');
            const $dto = $('#<%= basic_dto.ClientID %>');
            const $precio = $('#<%= basic_precio.ClientID %>');

            const $fund = $('#<%= basic_fund.ClientID %>');
            const $uni = $('#<%= basic_uni.ClientID %>');
            const $iva = $('#<%= basic_iva.ClientID %>');
            const $irpf = $('#<%= basic_irpf.ClientID %>');
            const $total = $('#<%= basic_total.ClientID %>');

            const pvp = parseLocaleNumber($pvp.val());
            const beca = parseLocaleNumber($beca.val());
            const dto = parseLocaleNumber($dto.val());

            const fund = parseLocaleNumber($fund.val());
            const uni = parseLocaleNumber($uni.val());
            const iva = parseLocaleNumber($iva.val());
            const irpf = parseLocaleNumber($irpf.val());

            // Si algún campo es NaN, tratamos como 0 para no romper el cálculo
            const safe = (x) => isNaN(x) ? 0 : x;

            const precioCalc = safe(pvp) - safe(beca) - safe(dto);
            $precio.val(formatLocaleNumber(precioCalc));

            const totalCalc = safe(precioCalc) + safe(fund) + safe(uni) + safe(iva) + safe(irpf);
            $total.val(formatLocaleNumber(totalCalc));
        }

        // ===========================
        // Validación
        // ===========================
        function showValidationErrors(errors) {
            const html = `<ul style="text-align:left;margin:0;padding-left:18px;">${errors.map(e => `<li>${e}</li>`).join('')}</ul>`;
            showInfo('Revisa el formulario', html);
        }

        function validateFacturaForm() {
            const errors = [];

            const sociedad = $('#<%= slSociedad.ClientID %>').val();
            const numero = $('#<%= secuencia.ClientID %>').val();
            const femision = $('#<%= femision.ClientID %>').val();
            const cliente = $('#<%= basic_client.ClientID %>').val();
            const nif = $('#<%= basic_nif.ClientID %>').val();
            const descripcion = $('#<%= descripcion.ClientID %>').val();

            // recalcular antes de leer
            recalcFactura();

            const pvp = parseLocaleNumber($('#<%= basic_pvp.ClientID %>').val());
            const precio = parseLocaleNumber($('#<%= basic_precio.ClientID %>').val());
            const trip = parseLocaleNumber($('#<%= basic_trip.ClientID %>').val());
            const total = parseLocaleNumber($('#<%= basic_total.ClientID %>').val());

            const beca = parseLocaleNumber($('#<%= basic_beca.ClientID %>').val());
            const dto = parseLocaleNumber($('#<%= basic_dto.ClientID %>').val());
            const fund = parseLocaleNumber($('#<%= basic_fund.ClientID %>').val());
            const uni = parseLocaleNumber($('#<%= basic_uni.ClientID %>').val());
            const iva = parseLocaleNumber($('#<%= basic_iva.ClientID %>').val());
            const irpf = parseLocaleNumber($('#<%= basic_irpf.ClientID %>').val());

            const venc = $('#<%= date_venc.ClientID %>').val();
            const cobro = $('#<%= date_payment.ClientID %>').val();
            const atribucion = $('#<%= ddlAtribucion.ClientID %>').val();
            const comentarios = $('#<%= comentario.ClientID %>').val();

            if (!sociedad) errors.push('Seleccione la sociedad.');
            if (!numero || Number(numero) <= 0) errors.push('La secuencia/número debe ser mayor que 0.');
            if (!femision) errors.push('Seleccione la fecha de emisión.');
            if (!cliente || cliente.trim().length < 2) errors.push('El nombre del cliente es obligatorio.');
            if (!nif || nif.trim().length < 3) errors.push('El NIF es obligatorio.');
            if (!descripcion || descripcion.trim().length < 3) errors.push('La descripción es obligatoria.');

            if (isNaN(pvp)) errors.push('PVP inválido.');
            if (isNaN(precio)) errors.push('Precio inválido.');
            if (isNaN(trip)) errors.push('Tripartita inválida.');
            if (isNaN(total)) errors.push('Total inválido.');

            const pairs = [
                { n: 'PVP', v: pvp }, { n: 'Precio', v: precio }, { n: 'Tripartita', v: trip }, { n: 'Total', v: total },
                { n: 'Beca', v: beca }, { n: 'Descuento', v: dto }, { n: 'Fundación', v: fund }, { n: 'Universidad', v: uni },
                { n: 'IVA', v: iva }, { n: 'IRPF', v: irpf },
            ];
            pairs.forEach(x => { if (!isNaN(x.v) && x.v < 0) errors.push(`${x.n} no puede ser negativo.`); });

            if (venc && femision && venc < femision) errors.push('La fecha de vencimiento no puede ser menor que la de emisión.');
            if (cobro && femision && cobro < femision) errors.push('La fecha de cobro no puede ser menor que la de emisión.');

            if (cliente && cliente.length > 250) errors.push('Cliente excede 250 caracteres.');
            if (nif && nif.length > 20) errors.push('NIF excede 20 caracteres.');
            if (descripcion && descripcion.length > 500) errors.push('Descripción excede 500 caracteres.');
            if (comentarios && comentarios.length > 1000) errors.push('Comentarios excede 1000 caracteres.');
            if (atribucion && atribucion.length > 9) errors.push('Atribución excede 9 caracteres.');
            if (sociedad && sociedad.length > 5) errors.push('Sociedad excede 5 caracteres.');

            return {
                ok: errors.length === 0,
                errors,
                data: {
                    sociedad,
                    numero: Number(numero),
                    fecha_emision: femision,
                    cliente_nombre: cliente,
                    cliente_nif: nif,
                    descripcion: descripcion,
                    eur_pvp: isNaN(pvp) ? 0 : pvp,
                    eur_beca: isNaN(beca) ? 0 : beca,
                    eur_dto: isNaN(dto) ? 0 : dto,
                    eur_precio: isNaN(precio) ? 0 : precio,
                    eur_fundacion: isNaN(fund) ? 0 : fund,
                    eur_universidad: isNaN(uni) ? 0 : uni,
                    eur_tripartita: isNaN(trip) ? 0 : trip,
                    eur_iva: isNaN(iva) ? 0 : iva,
                    eur_irpf: isNaN(irpf) ? 0 : irpf,
                    eur_total: isNaN(total) ? 0 : total,
                    fecha_vencimiento: venc || null,
                    fecha_cobro: cobro || null,
                    comentarios: comentarios || null,
                    atribucion: atribucion || null,
                    fichero: null
                }
            };
        }

        // ===========================
        // Upload + Save (igual lógica que tenías)
        // ===========================
        function uploadFacturaIfAny() {
            const fileInput = document.getElementById('<%= fuFactura.ClientID %>');
            const hasFile = fileInput && fileInput.files && fileInput.files.length > 0;

            if (!hasFile) return Promise.resolve({ ok: true, fileName: null, relativePath: null });

            const fd = new FormData();
            fd.append('file', fileInput.files[0]);
            fd.append('sociedad', $('#<%= slSociedad.ClientID %>').val() || '');
            fd.append('year', ($('#<%= femision.ClientID %>').val() || '').substring(0, 4));

            return new Promise((resolve, reject) => {
                $.ajax({
                    url: '<%= ResolveUrl("~/Admin/UploadFactura.ashx") %>',
                    type: 'POST',
                    data: fd,
                    processData: false,
                    contentType: false,
                    success: function (resp) {
                        let r = resp;
                        if (typeof resp === 'string') {
                            try { r = JSON.parse(resp); } catch (e) { r = null; }
                        }
                        if (!r || !r.ok) {
                            return resolve({ ok: false, message: (r && r.message) ? r.message : 'No se pudo subir el archivo.' });
                        }
                        resolve({ ok: true, fileName: r.fileName || null, relativePath: r.relativePath || null });
                    },
                    error: function (xhr) {
                        console.error(xhr);
                        reject(new Error('No se pudo subir el archivo.'));
                    }
                });
            });
        }

        function saveFacturaAjax(dto) {
            $.ajax({
                url: 'admin_facturas.aspx/SaveFactura',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ dto: dto }),
                success: function (res) {
                    const r = res && res.d ? res.d : null;
                    hideSaving();

                    if (!r) return showInfo('Error', 'Respuesta inválida del servidor.');

                    if (r.ok) {
                        showInfo('OK', r.message || 'Factura registrada.');
                        // recargar al cerrar
                        $('#mdInfo').one('hidden.bs.modal', function () { location.reload(); });
                    } else {
                        const errs = r.errors || [r.message || 'No se pudo registrar.'];
                        showValidationErrors(errs);
                    }
                },
                error: function (xhr) {
                    console.error(xhr);
                    hideSaving();
                    showInfo('Error', 'No se pudo conectar con el servidor.');
                }
            });
        }

        $('#btnSave').on('click', async function () {
            const v = validateFacturaForm();
            if (!v.ok) return showValidationErrors(v.errors);

            showSaving();

            try {
                const up = await uploadFacturaIfAny();
                if (!up.ok) {
                    hideSaving();
                    return showInfo('Error', up.message || 'No se pudo subir el archivo.');
                }

                v.data.fichero = up.fileName || null;
                saveFacturaAjax(v.data);

            } catch (e) {
                console.error(e);
                hideSaving();
                showInfo('Error', e.message || 'Error inesperado.');
            }
        });

        // ===========================
        // Eventos para recálculo
        // ===========================
        function bindAutoCalc() {
            // cada vez que el usuario escriba en cualquier campo fuente
            $('.calc-src').on('input', function () {
                // validatePriceLocale ya corre por atributo, pero recalculamos después:
                recalcFactura();
            });

            // al cargar (por si hay valores iniciales)
            recalcFactura();
        }

        // ===========================
        // Resto de tu lógica (sin Swal)
        // ===========================
        $(document).ready(function () {
            bindAutoCalc();

            var selectedYear = $('#<%= ddlyear.ClientID %>').val();
            if (selectedYear) loadYears(selectedYear);

            $('.input-group.date').datepicker({
                language: "es",
                autoclose: true,
                todayHighlight: true
            });
        });

        document.addEventListener('DOMContentLoaded', function () {
            const fileInput = document.getElementById('<%= fuFactura.ClientID %>');
            const fileName = document.getElementById('fileName');

            fileInput.addEventListener('change', function () {
                if (this.files && this.files.length > 0) fileName.textContent = this.files[0].name;
                else fileName.textContent = '';
            });
        });

        $('#<%= basic_client.ClientID %>').on('change', function () {
            var cliente = $(this).val();
            document.getElementById('viewclient').innerText = cliente || '';
        });

        $('#<%= ddlyear.ClientID %>').on('change', function () {
    var year = $(this).val();
    if (!year) return;

    // ✅ Cambiar el año en la fecha de emisión
    setFemisionYear(year);

         var valorSociedad = $('#<%= slSociedad.ClientID %>').val();

         loadYears(year);

         if (valorSociedad) {
             GetSecuencia(valorSociedad, year);
         }
     });


        $('#<%= slSociedad.ClientID %>').on('change', function () {
            var valorSociedad = $(this).val();
            if (!valorSociedad) return;

            var year = $('#<%= ddlyear.ClientID %>').val();
            if (!year) {
                showInfo('Atención', 'Seleccione el año');
                return;
            }
            GetSecuencia(valorSociedad, year);
        });

        $('#<%= basic_client.ClientID %>').autocomplete({
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: 'admin_facturas.aspx/SearchClients',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ term: request.term }),
                    success: function (res) {
                        response(res.d || []);
                        if (!res.d || res.d.length === 0) {
                            document.getElementById('viewclient').innerText = 'cliente no encontrado';
                            return;
                        }
                        document.getElementById('viewclient').innerText = res.d[0];
                    }
                });
            },
            select: function (event, ui) {
                $('#<%= basic_client.ClientID %>').val(ui.item.value);
                return false;
            }
        });

        function pad2(n) { return String(n).padStart(2, '0'); }

        function setFemisionYear(newYear) {
            const $fe = $('#<%= femision.ClientID %>');
            const cur = ($fe.val() || '').trim(); // formato input date: yyyy-MM-dd

            // si está vacío: poner 01-01 del año seleccionado
            if (!cur) {
                $fe.val(`${newYear}-01-01`);
                return;
            }

            // parsea yyyy-MM-dd
            const parts = cur.split('-');
            if (parts.length !== 3) {
                // si viene raro, fuerza 01-01
                $fe.val(`${newYear}-01-01`);
                return;
            }

            let month = Number(parts[1]);
            let day = Number(parts[2]);

            
            if (!month || month < 1 || month > 12) month = 1;
            if (!day || day < 1 || day > 31) day = 1;

          
            if (month === 2 && day === 29) {
                const y = Number(newYear);
                const isLeap = (y % 4 === 0 && y % 100 !== 0) || (y % 400 === 0);
                if (!isLeap) day = 28;
            }

            $fe.val(`${newYear}-${pad2(month)}-${pad2(day)}`);
        }




        function loadYears(year) {
            $.ajax({
                type: "POST",
                url: "admin_facturas.aspx/LoadYearsRead",
                data: JSON.stringify({ year: parseInt(year, 10) }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var data = (res && res.d) ? res.d : null;
                    var lista = data ? data.Objeto : null;

                    $('#sbs_total').text('0');
                    $('#sbscs_total').text('0');

                    if (!lista || !lista.length) return;

                    var sbs = lista.find(function (x) { return (x.NombreSocio || '').toUpperCase() === 'SBS'; });
                    var sbscs = lista.find(function (x) { return (x.NombreSocio || '').toUpperCase() === 'SBSCS'; });

                    if (sbs) $('#sbs_total').text(sbs.Total);
                    if (sbscs) $('#sbscs_total').text(sbscs.Total);
                },
                error: function (err) { console.error(err); }
            });
        }

        function GetSecuencia(valorSociedad, year) {
            $.ajax({
                type: "POST",
                url: "admin_facturas.aspx/ObtenerSecuencia",
                data: JSON.stringify({ valorSociedad: valorSociedad, year: parseInt(year, 10) }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    var sec = (res && res.d != null) ? res.d : '';
                    $('#secuencia').val(sec);
                },
                error: function (err) { console.error("otro error", err); }
            });
        }

    </script>
</body>
</html>
