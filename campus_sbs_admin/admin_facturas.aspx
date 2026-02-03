<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_facturas.aspx.cs" Inherits="campus_sbs_admin.admin_facturas" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Registro de Facturas</title>

    <!-- CSS 
    =================================================== -->
    <asp:PlaceHolder runat="server">
        <%: Styles.Render("~/bundles/bootstrap_css") %>
        <%: Styles.Render("~/bundles/fonts_css") %>
        <%: Styles.Render("~/bundles/general_admin_css") %>        
        <%: Styles.Render("~/bundles/jquery_ui_css") %>
        <%: Styles.Render("~/bundles/datatables_css") %>
    </asp:PlaceHolder>

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
                     CABECERA: AÑO + TOTALES
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-file-invoice-dollar"></i> Registro de Facturas
                            <a href='consultar_facturas.aspx' title='Consultar facturas' class='pull-right bold padding-r-5'>
                                <small class='text-color-primary'><i class='fas fa-search'></i> Consultar</small>
                            </a>
                        </legend>                    
                    </fieldset>

                    <div class="col-12 pt-2">
                        <div class="col-3">
                            <label>Año Fiscal</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlyear">Año Fiscal</label>
                                <select id="ddlyear" runat="server" title="Año Fiscal" class="form-control"></select>
                            </div>
                        </div>
                        <div class="col-3">
                            <label>&nbsp;</label>
                            <div class="form-group">
                                <span class="bold text-color-primary">SBS: </span>
                                <span id="sbs_total" class="border-primary bold text-color-black" title="Total SBS">0</span>
                            </div>
                        </div>
                        <div class="col-3">
                            <label>&nbsp;</label>
                            <div class="form-group">
                                <span class="bold text-color-primary">SBSCS: </span>
                                <span id="sbscs_total" class="border-primary bold text-color-black" title="Total SBSCS">0</span>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     FORMULARIO PRINCIPAL
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-edit"></i> Datos de la Factura
                        </legend>
                    </fieldset>

                    <!-- Fila: Sociedad, Secuencia, Fecha Emisión -->
                    <div class="col-12 pt-2">
                        <div class="col-4">
                            <label>Sociedad *</label>
                            <div class="form-group">
                                <label class="sr-only" for="slSociedad">Sociedad</label>
                                <select id="slSociedad" runat="server" title="Sociedad" class="form-control"></select>
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Secuencia *</label>
                            <div class="form-group">
                                <label class="sr-only" for="secuencia">Secuencia</label>
                                <input type="number" id="secuencia" runat="server" title="Secuencia Siguiente" class="form-control" placeholder="Secuencia siguiente" />
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Fecha Emisión *</label>
                            <div class="form-group">
                                <label class="sr-only" for="femision">Fecha Emisión</label>
                                <input type="date" id="femision" runat="server" title="Fecha Emisión" class="form-control" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Cliente, NIF -->
                    <div class="col-12 pt-2">
                        <div class="col-6">
                            <label>Cliente *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_client">Cliente</label>
                                <input type="text" runat="server" id="basic_client" placeholder="Nombre del cliente" title="Nombre Cliente" class="form-control" />
                                <span id="viewclient" class="text-muted small"></span>
                            </div>
                        </div>
                        <div class="col-6">
                            <label>NIF *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_nif">NIF</label>
                                <input type="text" runat="server" id="basic_nif" placeholder="NIF" title="Número de Identificación Fiscal" class="form-control" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Descripción -->
                    <div class="col-12 pt-2">
                        <div class="col-12 px-0">
                            <label>Descripción *</label>
                            <div class="form-group">
                                <label class="sr-only" for="descripcion">Descripción</label>
                                <textarea placeholder="Descripción de la factura" runat="server" id="descripcion" class="form-control" title="Descripción" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     SECCIÓN: IMPORTES
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-euro-sign"></i> Importes
                        </legend>
                    </fieldset>

                    <!-- Fila: PVP, Beca, Descuento -->
                    <div class="col-12 pt-2">
                        <div class="col-4">
                            <label>PVP *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_pvp">PVP</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_pvp" placeholder="0,00" title="PVP" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Beca</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_beca">Beca</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_beca" placeholder="0,00" title="Beca" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Descuento</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_dto">Descuento</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_dto" placeholder="0,00" title="Descuento" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Precio, Fundación, Universidad -->
                    <div class="col-12 pt-2">
                        <div class="col-4">
                            <label>Precio (calculado)</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_precio">Precio</label>
                                <input type="text" class="form-control calc-readonly" runat="server" id="basic_precio" placeholder="0,00" title="Precio" readonly="readonly" />
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Fundación</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_fund">Fundación</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_fund" placeholder="0,00" title="Fundación" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Universidad</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_uni">Universidad</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_uni" placeholder="0,00" title="Universidad" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Tripartita, IVA, IRPF, Total -->
                    <div class="col-12 pt-2">
                        <div class="col-3">
                            <label>Tripartita</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_trip">Tripartita</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_trip" placeholder="0,00" title="Tripartita" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>IVA</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_iva">IVA</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_iva" placeholder="0,00" title="IVA" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>IRPF</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_irpf">IRPF</label>
                                <input type="text" class="form-control calc-src" runat="server" id="basic_irpf" placeholder="0,00" title="IRPF" oninput="validatePriceLocale(this)" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Total (calculado)</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_total">Total</label>
                                <input type="text" class="form-control calc-readonly bold text-color-primary" runat="server" id="basic_total" placeholder="0,00" title="Total" readonly="readonly" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     SECCIÓN: FECHAS Y ADICIONALES
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-calendar-alt"></i> Fechas y Datos Adicionales
                        </legend>
                    </fieldset>

                    <!-- Fila: Fecha Vencimiento, Fecha Cobro -->
                    <div class="col-12 pt-2">
                        <div class="col-6">
                            <label>Fecha Vencimiento</label>
                            <div class="form-group">
                                <label class="sr-only" for="date_venc">Fecha Vencimiento</label>
                                <input type="date" class="form-control" id="date_venc" runat="server" title="Fecha Vencimiento" />
                            </div>
                        </div>
                        <div class="col-6">
                            <label>Fecha Cobro</label>
                            <div class="form-group">
                                <label class="sr-only" for="date_payment">Fecha Cobro</label>
                                <input type="date" class="form-control" id="date_payment" runat="server" title="Fecha Cobro" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Archivo, Atribución -->
                    <div class="col-12 pt-2">
                        <div class="col-6">
                            <label>Archivo Factura</label>
                            <div class="form-group">
                                <asp:FileUpload ID="fuFactura" runat="server" Style="display: none;" />
                                <a href="javascript:void(0);" onclick="document.getElementById('<%= fuFactura.ClientID %>').click();" title="Cargar archivo" class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-upload"></i> Cargar archivo
                                </a>
                                <span id="fileName" class="text-muted small ml-2"></span>
                            </div>
                        </div>
                        <div class="col-6">
                            <label>Atribución</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlAtribucion">Atribución</label>
                                <select id="ddlAtribucion" runat="server" title="Atribución" class="form-control"></select>
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Comentarios -->
                    <div class="col-12 pt-2">
                        <div class="col-12 px-0">
                            <label>Comentarios</label>
                            <div class="form-group">
                                <label class="sr-only" for="comentario">Comentarios</label>
                                <textarea runat="server" id="comentario" placeholder="Comentarios adicionales" class="form-control" title="Comentarios" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     BOTONES DE ACCIÓN
                     =========================== -->
                <div class="col pt-4 pb-4">
                    <div class="col-12 text-center">
                        <button type="button" runat="server" id="btnCancel" class="btn btn-danger" title="Cancelar">
                            <i class="fas fa-times"></i> Cancelar
                        </button>
                        <button id="btnSave" type="button" class="btn btn-success ml-3" title="Registrar">
                            <i class="fas fa-save"></i> Registrar
                        </button>
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary ml-3" Text="Consultar" OnClick="btnSearch_Click" />
                    </div>
                </div>

            </div>       
        </section>

        <!-- Modal: Guardando -->
        <div class="modal fade" id="wait_modal" tabindex="-1" role="dialog" aria-labelledby="wait_modal" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-body text-center">
                        <i class="fas fa-spinner fa-pulse fa-5x text-color-primary"></i>
                        <p class="mt-3 bold">Guardando...</p>
                        <small class="text-muted">Por favor espere</small>
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal: Info (éxito/error/validación) -->
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

    <script type="text/javascript">
        // ===========================
        // Helpers UI (Modal)
        // ===========================
        function showSaving() {
            if ($.fn.modal) $('#wait_modal').modal('show');
        }

        function hideSaving() {
            if ($.fn.modal) $('#wait_modal').modal('hide');
        }

        function showInfo(title, htmlOrText) {
            $('#info_modal_title').text(title || 'Mensaje');
            $('#info_modal_body').html(htmlOrText || '');
            if ($.fn.modal) $('#info_modal').modal('show');
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

            var dec = getDecimalSeparator();
            val = (val + '').trim();
            if (!val) return 0;

            var thousands = (dec === ',') ? '.' : ',';
            val = val.split(thousands).join('');
            if (dec !== '.') val = val.replace(dec, '.');

            var num = Number(val);
            return isNaN(num) ? NaN : num;
        }

        function formatLocaleNumber(num) {
            var n = Number(num);
            if (isNaN(n)) return '';
            return n.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        }

        function validatePriceLocale(input) {
            var decimalSeparator = getDecimalSeparator();
            var regexNotAllowed = new RegExp('[^0-9' + decimalSeparator + ']', 'g');
            var regexExtraDecimal = new RegExp('\\' + decimalSeparator + '.*\\' + decimalSeparator, 'g');

            input.value = input.value
                .replace(regexNotAllowed, '')
                .replace(regexExtraDecimal, function (match) { return match.slice(0, -1); });
        }

        // ===========================
        // CÁLCULOS AUTOMÁTICOS
        // ===========================
        function recalcFactura() {
            var $pvp = $('#<%= basic_pvp.ClientID %>');
            var $beca = $('#<%= basic_beca.ClientID %>');
            var $dto = $('#<%= basic_dto.ClientID %>');
            var $precio = $('#<%= basic_precio.ClientID %>');

            var $fund = $('#<%= basic_fund.ClientID %>');
            var $uni = $('#<%= basic_uni.ClientID %>');
            var $iva = $('#<%= basic_iva.ClientID %>');
            var $irpf = $('#<%= basic_irpf.ClientID %>');
            var $total = $('#<%= basic_total.ClientID %>');

            var pvp = parseLocaleNumber($pvp.val());
            var beca = parseLocaleNumber($beca.val());
            var dto = parseLocaleNumber($dto.val());

            var fund = parseLocaleNumber($fund.val());
            var uni = parseLocaleNumber($uni.val());
            var iva = parseLocaleNumber($iva.val());
            var irpf = parseLocaleNumber($irpf.val());

            var safe = function (x) { return isNaN(x) ? 0 : x; };

            var precioCalc = safe(pvp) - safe(beca) - safe(dto);
            $precio.val(formatLocaleNumber(precioCalc));

            var totalCalc = safe(precioCalc) + safe(fund) + safe(uni) + safe(iva) + safe(irpf);
            $total.val(formatLocaleNumber(totalCalc));
        }

        // ===========================
        // Validación
        // ===========================
        function showValidationErrors(errors) {
            var html = '<ul style="text-align:left;margin:0;padding-left:18px;">';
            for (var i = 0; i < errors.length; i++) {
                html += '<li>' + errors[i] + '</li>';
            }
            html += '</ul>';
            showInfo('Revisa el formulario', html);
        }

        function validateFacturaForm() {
            var errors = [];

            var sociedad = $('#<%= slSociedad.ClientID %>').val();
            var numero = $('#<%= secuencia.ClientID %>').val();
            var femision = $('#<%= femision.ClientID %>').val();
            var cliente = $('#<%= basic_client.ClientID %>').val();
            var nif = $('#<%= basic_nif.ClientID %>').val();
            var descripcion = $('#<%= descripcion.ClientID %>').val();

            recalcFactura();

            var pvp = parseLocaleNumber($('#<%= basic_pvp.ClientID %>').val());
            var precio = parseLocaleNumber($('#<%= basic_precio.ClientID %>').val());
            var trip = parseLocaleNumber($('#<%= basic_trip.ClientID %>').val());
            var total = parseLocaleNumber($('#<%= basic_total.ClientID %>').val());

            var beca = parseLocaleNumber($('#<%= basic_beca.ClientID %>').val());
            var dto = parseLocaleNumber($('#<%= basic_dto.ClientID %>').val());
            var fund = parseLocaleNumber($('#<%= basic_fund.ClientID %>').val());
            var uni = parseLocaleNumber($('#<%= basic_uni.ClientID %>').val());
            var iva = parseLocaleNumber($('#<%= basic_iva.ClientID %>').val());
            var irpf = parseLocaleNumber($('#<%= basic_irpf.ClientID %>').val());

            var venc = $('#<%= date_venc.ClientID %>').val();
            var cobro = $('#<%= date_payment.ClientID %>').val();
            var atribucion = $('#<%= ddlAtribucion.ClientID %>').val();
            var comentarios = $('#<%= comentario.ClientID %>').val();

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

            var pairs = [
                { n: 'PVP', v: pvp }, { n: 'Precio', v: precio }, { n: 'Tripartita', v: trip }, { n: 'Total', v: total },
                { n: 'Beca', v: beca }, { n: 'Descuento', v: dto }, { n: 'Fundación', v: fund }, { n: 'Universidad', v: uni },
                { n: 'IVA', v: iva }, { n: 'IRPF', v: irpf }
            ];
            
            for (var i = 0; i < pairs.length; i++) {
                if (!isNaN(pairs[i].v) && pairs[i].v < 0) {
                    errors.push(pairs[i].n + ' no puede ser negativo.');
                }
            }

            if (venc && femision && venc < femision) errors.push('La fecha de vencimiento no puede ser menor que la de emisión.');
            if (cobro && femision && cobro < femision) errors.push('La fecha de cobro no puede ser menor que la de emisión.');

            if (cliente && cliente.length > 250) errors.push('Cliente excede 250 caracteres.');
            if (nif && nif.length > 20) errors.push('NIF excede 20 caracteres.');
            if (descripcion && descripcion.length > 500) errors.push('Descripción excede 500 caracteres.');
            if (comentarios && comentarios.length > 1000) errors.push('Comentarios excede 1000 caracteres.');

            return {
                ok: errors.length === 0,
                errors: errors,
                data: {
                    sociedad: sociedad,
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
        // Upload + Save
        // ===========================
        function uploadFacturaIfAny() {
            var fileInput = document.getElementById('<%= fuFactura.ClientID %>');
            var hasFile = fileInput && fileInput.files && fileInput.files.length > 0;

            if (!hasFile) {
                return $.Deferred().resolve({ ok: true, fileName: null, relativePath: null }).promise();
            }

            var fd = new FormData();
            fd.append('file', fileInput.files[0]);
            fd.append('sociedad', $('#<%= slSociedad.ClientID %>').val() || '');
            fd.append('year', ($('#<%= femision.ClientID %>').val() || '').substring(0, 4));

            return $.ajax({
                url: '<%= ResolveUrl("~/Admin/UploadFactura.ashx") %>',
                type: 'POST',
                data: fd,
                processData: false,
                contentType: false
            }).then(function(resp) {
                var r = resp;
                if (typeof resp === 'string') {
                    try { r = JSON.parse(resp); } catch (e) { r = null; }
                }
                if (!r || !r.ok) {
                    return { ok: false, message: (r && r.message) ? r.message : 'No se pudo subir el archivo.' };
                }
                return { ok: true, fileName: r.fileName || null, relativePath: r.relativePath || null };
            }).fail(function() {
                return { ok: false, message: 'No se pudo subir el archivo.' };
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
                    var r = res && res.d ? res.d : null;
                    hideSaving();

                    if (!r) {
                        showInfo('Error', 'Respuesta inválida del servidor.');
                        return;
                    }

                    if (r.ok) {
                        showInfo('Éxito', r.message || 'Factura registrada correctamente.');
                        $('#info_modal').one('hidden.bs.modal', function () { location.reload(); });
                    } else {
                        var errs = r.errors || [r.message || 'No se pudo registrar.'];
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

        // ===========================
        // Eventos
        // ===========================
        function bindAutoCalc() {
            $('.calc-src').on('input', function () {
                recalcFactura();
            });
            recalcFactura();
        }

        $(document).ready(function () {
            bindAutoCalc();

            var selectedYear = $('#<%= ddlyear.ClientID %>').val();
            if (selectedYear) loadYears(selectedYear);

            // File input change
            var fileInput = document.getElementById('<%= fuFactura.ClientID %>');
            var fileNameSpan = document.getElementById('fileName');

            if (fileInput) {
                fileInput.addEventListener('change', function () {
                    if (this.files && this.files.length > 0) {
                        fileNameSpan.textContent = this.files[0].name;
                    } else {
                        fileNameSpan.textContent = '';
                    }
                });
            }

            // Botón guardar
            $('#btnSave').on('click', function () {
                var v = validateFacturaForm();
                if (!v.ok) {
                    showValidationErrors(v.errors);
                    return;
                }

                showSaving();

                uploadFacturaIfAny().then(function(up) {
                    if (!up.ok) {
                        hideSaving();
                        showInfo('Error', up.message || 'No se pudo subir el archivo.');
                        return;
                    }

                    v.data.fichero = up.fileName || null;
                    saveFacturaAjax(v.data);
                }).fail(function(e) {
                    console.error(e);
                    hideSaving();
                    showInfo('Error', 'Error inesperado.');
                });
            });

            // Cliente change
            $('#<%= basic_client.ClientID %>').on('change', function () {
                var cliente = $(this).val();
                document.getElementById('viewclient').innerText = cliente || '';
            });

            // Año change
            $('#<%= ddlyear.ClientID %>').on('change', function () {
                var year = $(this).val();
                if (!year) return;

                setFemisionYear(year);
                var valorSociedad = $('#<%= slSociedad.ClientID %>').val();
                loadYears(year);

                if (valorSociedad) {
                    GetSecuencia(valorSociedad, year);
                }
            });

            // Sociedad change
            $('#<%= slSociedad.ClientID %>').on('change', function () {
                var valorSociedad = $(this).val();
                if (!valorSociedad) return;

                var year = $('#<%= ddlyear.ClientID %>').val();
                if (!year) {
                    showInfo('Atención', 'Seleccione el año primero.');
                    return;
                }
                GetSecuencia(valorSociedad, year);
            });

            // Autocomplete cliente
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
        });

        // ===========================
        // Funciones auxiliares
        // ===========================
        function pad2(n) { 
            return String(n).length < 2 ? '0' + n : String(n); 
        }

        function setFemisionYear(newYear) {
            var $fe = $('#<%= femision.ClientID %>');
            var cur = ($fe.val() || '').trim();

            if (!cur) {
                $fe.val(newYear + '-01-01');
                return;
            }

            var parts = cur.split('-');
            if (parts.length !== 3) {
                $fe.val(newYear + '-01-01');
                return;
            }

            var month = Number(parts[1]);
            var day = Number(parts[2]);

            if (!month || month < 1 || month > 12) month = 1;
            if (!day || day < 1 || day > 31) day = 1;

            if (month === 2 && day === 29) {
                var y = Number(newYear);
                var isLeap = (y % 4 === 0 && y % 100 !== 0) || (y % 400 === 0);
                if (!isLeap) day = 28;
            }

            $fe.val(newYear + '-' + pad2(month) + '-' + pad2(day));
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

                    var sbs = null, sbscs = null;
                    for (var i = 0; i < lista.length; i++) {
                        var nombre = (lista[i].NombreSocio || '').toUpperCase();
                        if (nombre === 'SBS') sbs = lista[i];
                        if (nombre === 'SBSCS') sbscs = lista[i];
                    }

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
                    $('#<%= secuencia.ClientID %>').val(sec);
                },
                error: function (err) { console.error("Error obteniendo secuencia", err); }
            });
        }
    </script>
</body>
</html>