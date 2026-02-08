<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_Gastos.aspx.cs" Inherits="campus_sbs_admin.Admin_Gastos" %>
<%@ Import Namespace="System.Web.Optimization" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>

<!--[if IE 8]>    <html class="ie8 legacy-ie no-js" lang="es"> <![endif]-->
<!--[if IE 9]>    <html class="ie9 legacy-ie no-js" lang="es"><![endif]-->
<!--[if !IE]><!--> <html class="no-legacy-ie no-js" lang="es"> <!--<![endif]-->
<head runat="server">
    <title>SBS | Gastos</title>

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

    <style type="text/css">
        /* jQuery UI Datepicker - forzar z-index alto */
        .ui-datepicker {
            z-index: 99999 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <uc_menu:menu ID="menu" runat="server" />
        <header id="header" class="bg-color-primary affix">        
            <uc_header:cabecera ID="cabecera" runat="server" />
        </header>

        <section class="wrapper">
            <div class="padding-nav">

                <!-- ===========================
                     SECCIÓN: DATOS BÁSICOS
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-file-invoice"></i> Registro de Gastos
                            <a href='admin_gastos_list.aspx' title='Ver consulta' class='pull-right bold padding-r-5'>
                                <small class='text-color-primary'><i class='fas fa-list'></i> Ver consulta</small>
                            </a>
                        </legend>
                    </fieldset>

                    <!-- Mensaje de error -->
                    <div id="block_result" runat="server" style="display:none;" class="col-12 pt-2">
                        <div class="alert alert-danger mb-0 py-2 px-3">
                            <span id="lbl_result" runat="server"></span>
                        </div>
                    </div>

                    <!-- Fila: Año, Sociedad, Último Nº, Secuencia, Fecha Emisión -->
                    <div class="col-12 pt-2">
                        <div class="col-2">
                            <label>Año *</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlyear">Año</label>
                                <select id="ddlyear" runat="server" class="form-control" title="Año"></select>
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Sociedad *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_sociedad">Sociedad</label>
                                <select id="basic_sociedad" runat="server" class="form-control" title="Sociedad"></select>
                            </div>
                        </div>
                        <div class="col-2">
                            <label>Último Nº</label>
                            <div class="form-group">
                                <span class="bold text-color-primary">Nº </span>
                                <span id="txt_num" runat="server" class="border-primary bold text-color-black">000</span>
                            </div>
                        </div>
                        <div class="col-2">
                            <label>Secuencia *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_secuence">Secuencia</label>
                                <input type="text" id="basic_secuence" runat="server" class="form-control" placeholder="Nº Secuencia" title="Secuencia" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Fecha Emisión *</label>
                            <div class="form-group">
                                <label class="sr-only" for="date_emision">Fecha Emisión</label>
                                <input type="text" id="date_emision" runat="server" class="form-control" placeholder="dd/mm/aaaa" title="Fecha Emisión" autocomplete="off" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Empresa, CIF -->
                    <div class="col-12 pt-2">
                        <div class="col-6">
                            <label>Empresa *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_company">Empresa</label>
                                <input type="text" id="basic_company" runat="server" class="form-control" placeholder="Empresa" title="Empresa" maxlength="200" />
                            </div>
                        </div>
                        <div class="col-6">
                            <label>CIF *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_cif">CIF</label>
                                <input type="text" id="basic_cif" runat="server" class="form-control" placeholder="CIF" title="CIF" maxlength="20" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Descripción -->
                    <div class="col-12 pt-2">
                        <div class="col-12 px-0">
                            <label>Descripción *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_description">Descripción</label>
                                <textarea id="basic_description" runat="server" class="form-control" placeholder="Descripción" title="Descripción" rows="3"></textarea>
                            </div>
                        </div>
                    </div>

                    <!-- Fila: SubTotal, IVA, IRPF, Total -->
                    <div class="col-12 pt-2">
                        <div class="col-3">
                            <label>SubTotal *</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_subtotal">SubTotal</label>
                                <input type="text" id="basic_subtotal" runat="server" class="form-control calc-src" placeholder="0,00" title="SubTotal" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>IVA</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_iva">IVA</label>
                                <input type="text" id="basic_iva" runat="server" class="form-control calc-src" placeholder="0,00" title="IVA" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>IRPF</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_irpf">IRPF</label>
                                <input type="text" id="basic_irpf" runat="server" class="form-control calc-src" placeholder="0,00" title="IRPF" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Total (calculado)</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_total">Total</label>
                                <input type="text" id="basic_total" runat="server" class="form-control calc-readonly bold text-color-primary" placeholder="0,00" title="Total" readonly="readonly" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Checkbox LH -->
                    <div class="col-12 pt-2">
                        <div class="col-12 px-0">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="basic_chk" runat="server" />
                                    <label class="custom-control-label" for="basic_chk">¿Es una liquidación de honorarios (LH)?</label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     SECCIÓN: LIQUIDACIÓN HONORARIOS
                     =========================== -->
                <div id="basic_LH" runat="server" class="col pt-2" style="display:none;">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-chalkboard-teacher"></i> Liquidación de Honorarios
                        </legend>
                    </fieldset>

                    <!-- Fila: Profesor, LH Año, LH Nº, Tipo -->
                    <div class="col-12 pt-2">
                        <div class="col-5">
                            <label>Profesor</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlTeacher">Profesor</label>
                                <select id="ddlTeacher" runat="server" class="form-control" title="Profesor"></select>
                            </div>
                        </div>
                        <div class="col-2">
                            <label>LH Año</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_LH_year">LH Año</label>
                                <input type="text" id="basic_LH_year" runat="server" class="form-control" title="LH Año" />
                            </div>
                        </div>
                        <div class="col-2">
                            <label>LH Nº</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_LH_number">LH Nº</label>
                                <input type="text" id="basic_LH_number" runat="server" class="form-control" title="LH Nº" />
                            </div>
                        </div>
                        <div class="col-3">
                            <label>Tipo liquidación</label>
                            <div class="form-group">
                                <label class="sr-only" for="tipo_liquidacion">Tipo liquidación</label>
                                <select id="tipo_liquidacion" runat="server" class="form-control" title="Tipo liquidación">
                                    <option value="0">Seleccione un tipo</option>
                                    <option value="1">Online</option>
                                    <option value="2">Clase</option>
                                    <option value="3">Otros</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Comentarios LH -->
                    <div class="col-12 pt-2">
                        <div class="col-12 px-0">
                            <label>Comentarios LH</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_LH_comment">Comentarios LH</label>
                                <textarea id="basic_LH_comment" runat="server" class="form-control" placeholder="Comentarios LH" title="Comentarios LH" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     SECCIÓN: CATALOGACIÓN
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-tags"></i> Catalogación
                        </legend>
                    </fieldset>

                    <div class="col-12 pt-2">
                        <div class="col-4">
                            <label>Área</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlArea">Área</label>
                                <select id="ddlArea" runat="server" class="form-control" title="Área"></select>
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Subárea</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlSubArea">Subárea</label>
                                <select id="ddlSubArea" runat="server" class="form-control" title="Subárea"></select>
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Subárea 2</label>
                            <div class="form-group">
                                <label class="sr-only" for="ddlSubArea2">Subárea 2</label>
                                <select id="ddlSubArea2" runat="server" class="form-control" title="Subárea 2"></select>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     SECCIÓN: CONTABLE
                     =========================== -->
                <div class="col pt-2">
                    <fieldset>
                        <legend class="text-color-primary">
                            <i class="fas fa-calculator"></i> Contable
                        </legend>
                    </fieldset>

                    <!-- Fila: Fecha Pago, Apunte Banco, No Factura, Provisión -->
                    <div class="col-12 pt-2">
                        <div class="col-4">
                            <label>Fecha Pago</label>
                            <div class="form-group">
                                <label class="sr-only" for="date_pay">Fecha Pago</label>
                                <input type="text" id="date_pay" runat="server" class="form-control" placeholder="dd/mm/aaaa" title="Fecha Pago" autocomplete="off" />
                            </div>
                        </div>
                        <div class="col-4">
                            <label>Apunte Banco</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_banc">Apunte Banco</label>
                                <input type="text" id="basic_banc" runat="server" class="form-control" placeholder="Apunte Banco" title="Apunte Banco" maxlength="200" />
                            </div>
                        </div>
                        <div class="col-2">
                            <label>&nbsp;</label>
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="basic_chk_fact" runat="server" />
                                    <label class="custom-control-label" for="basic_chk_fact">¿No Factura?</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <label>&nbsp;</label>
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="basic_chk_provision" runat="server" />
                                    <label class="custom-control-label" for="basic_chk_provision">Provisión</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Archivo -->
                    <div class="col-12 pt-2">
                        <div class="col-6">
                            <label>Archivo</label>
                            <div class="form-group">
                                <asp:FileUpload ID="fuFile" runat="server" Style="display: none;" />
                                <a href="javascript:void(0);" onclick="document.getElementById('<%= fuFile.ClientID %>').click();" title="Cargar archivo" class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-upload"></i> Cargar archivo
                                </a>
                                <span id="fileNameDisplay" class="text-muted small ml-2"></span>
                            </div>
                        </div>
                        <div class="col-6 text-right">
                            <label>&nbsp;</label>
                            <div class="form-group">
                                <a id="lnkFile" runat="server" target="_blank" class="btn btn-sm btn-outline-secondary" style="display:none;">
                                    <i class="fas fa-eye"></i> Ver archivo
                                </a>
                                <asp:Button ID="btn_del_file" runat="server" CssClass="btn btn-sm btn-outline-danger" style="display:none;"
                                    Text="Eliminar archivo" OnClick="btn_del_file_Click" OnClientClick="showSaving();" />
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Impuesto -->
                    <div class="col-12 pt-2">
                        <div class="col-3">
                            <div class="form-group">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="basic_tax_chk" runat="server" />
                                    <label class="custom-control-label" for="basic_tax_chk">Impuesto</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-3" id="basic_tax" runat="server" style="display:none;">
                            <div class="form-group">
                                <label class="sr-only" for="ddlTax">Impuesto</label>
                                <select id="ddlTax" runat="server" class="form-control" title="Impuesto">
                                    <option value="">Seleccione un impuesto</option>
                                    <option value="111-190">111-190</option>
                                    <option value="303-390">303-390</option>
                                    <option value="202-200">202-200</option>
                                    <option value="115-190">115-190</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    <!-- Fila: Comentarios -->
                    <div class="col-12 pt-2">
                        <div class="col-12 px-0">
                            <label>Comentarios</label>
                            <div class="form-group">
                                <label class="sr-only" for="basic_comments">Comentarios</label>
                                <textarea id="basic_comments" runat="server" class="form-control" placeholder="Comentarios" title="Comentarios" rows="3"></textarea>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ===========================
                     BOTONES DE ACCIÓN
                     =========================== -->
                <div class="col pt-4 pb-4">
                    <div class="col-12 text-center">
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancel_Click" />

                        <button type="button" id="btnSaveAjax" class="btn btn-success ml-3">
                            <i class="fas fa-save"></i> Guardar
                        </button>

                        <button type="button" id="btnDeleteAjax" class="btn btn-outline-danger ml-3" style="display:none;">
                            <i class="fas fa-trash"></i> Eliminar
                        </button>
                    </div>
                </div>

                <input id="basic_id" type="hidden" runat="server" />

            </div>       
        </section>

        <!-- Modal: Guardando -->
        <div class="modal fade" id="wait_modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
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

        <!-- Modal: Confirmar eliminar -->
        <div class="modal fade" id="confirm_modal" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-color-primary">Confirmar eliminación</h5>
                        <button type="button" class="close" data-dismiss="modal"><span>&times;</span></button>
                    </div>
                    <div class="modal-body">
                        ¿Seguro que deseas eliminar este gasto? (Se eliminará el archivo adjunto si existe)
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
        function showSaving() {
            if (window.jQuery && $.fn.modal) $('#wait_modal').modal('show');
        }

        function hideSaving() {
            if (window.jQuery && $.fn.modal) $('#wait_modal').modal('hide');
        }

        function showInfo(title, htmlOrText, reloadOnClose) {
            $('#info_modal_title').text(title || 'Mensaje');
            $('#info_modal_body').html(htmlOrText || '');
            if ($.fn.modal) $('#info_modal').modal('show');

            $('#info_modal').off('hidden.bs.modal');
            if (reloadOnClose) {
                $('#info_modal').one('hidden.bs.modal', function () { window.location.reload(); });
            }
        }

        function hideLoadingForce() {
            try { $('#wait_modal').modal('hide'); } catch (e) { }
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open').css('padding-right', '');
        }

        // ===========================
        // Parseo / formato decimal
        // ===========================
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

        // ===========================
        // Cálculo Total
        // ===========================
        function recalcTotalGasto() {
            var sub = parseDec($('#<%= basic_subtotal.ClientID %>').val());
            var iva = parseDec($('#<%= basic_iva.ClientID %>').val());
            var irpf = parseDec($('#<%= basic_irpf.ClientID %>').val());
            var t = sub + iva - irpf;
            $('#<%= basic_total.ClientID %>').val(formatDec(t));
        }

        // ===========================
        // Toggle paneles
        // ===========================
        function toggleLH() {
            var chk = $('#<%= basic_chk.ClientID %>').is(':checked');
            var $b = $('#<%= basic_LH.ClientID %>');
            if (chk) { $b.show(); } else { $b.hide(); }
        }

        function toggleTax() {
            var chk = $('#<%= basic_tax_chk.ClientID %>').is(':checked');
            var $b = $('#<%= basic_tax.ClientID %>');
            if (chk) { $b.show(); } else { $b.hide(); }
        }

        // ===========================
        // Validación
        // ===========================
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
                var html = '<ul style="margin:0;padding-left:18px;text-align:left;">';
                for (var i = 0; i < errors.length; i++) {
                    html += '<li>' + errors[i] + '</li>';
                }
                html += '</ul>';
                showInfo('Revisa el formulario', html, false);
                return false;
            }
            return true;
        }

        // ===========================
        // Secuencia
        // ===========================
        function loadSecuence() {
            var soc = $('#<%= basic_sociedad.ClientID %>').val();
            var y = $('#<%= ddlyear.ClientID %>').val();
            if (!soc || !y) return;

            PageMethods.searchSecuence(soc, y, function (next) {
                $('#<%= basic_secuence.ClientID %>').val(next || '000');

                var n = parseInt(next || '0', 10);
                var last = (n > 0) ? String('000' + (n - 1)).slice(-3) : '000';
                $('#<%= txt_num.ClientID %>').text(last);

            }, function (err) {
                console.error(err);
                showInfo('Error', 'No se pudo obtener la secuencia.', false);
            });
        }

        // ===========================
        // Subáreas
        // ===========================
        function loadSub(table, parentId, ddlClientId) {
            if (!parentId) {
                var $empty = $('#' + ddlClientId);
                $empty.empty();
                var ph = (ddlClientId === '<%= ddlSubArea2.ClientID %>') ? 'Seleccione un Subárea 2' : 'Seleccione un Subárea';
                $empty.append($('<option/>', { value: '', text: ph }));
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

                    var items = list || [];
                    for (var i = 0; i < items.length; i++) {
                        $ddl.append($('<option/>', { value: items[i].id_inf_aux, text: items[i].Valor }));
                    }
                },
                function (err) {
                    console.error(err);
                    showInfo('Error', 'No se pudieron cargar las subáreas.', false);
                }
            );
        }

        // ===========================
        // Build request + Save
        // ===========================
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
                    hideLoadingForce();
                    var payload = (res && res.d) ? res.d : null;

                    if (!payload || payload.ok !== true) {
                        showInfo('Error', (payload && payload.message) ? payload.message : 'No se pudo guardar.', false);
                        return;
                    }

                    $('#<%= basic_id.ClientID %>').val(payload.id || 0);
                    setSaveButtonMode();

                    if ((payload.id || 0) > 0) {
                        $('#btnDeleteAjax').show();
                    }

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
                $('#btnSaveAjax').html('<i class="fas fa-save"></i> Actualizar');
            } else {
                $('#btnSaveAjax').html('<i class="fas fa-save"></i> Guardar');
            }
        }

        // ===========================
        // Delete
        // ===========================
        function openDeleteModal() {
            $('#confirm_modal').modal('show');
        }

        function deleteGastoAjax() {
            var id = Number($('#<%= basic_id.ClientID %>').val() || 0);
            if (!id) return showInfo('Error', 'No hay un gasto cargado para eliminar.', false);

            showSaving();
            PageMethods.DeleteGasto(id,
                function (r) {
                    hideSaving();
                    if (!r || r.ok !== true) {
                        return showInfo('Error', (r && r.message) ? r.message : 'No se pudo eliminar.', false);
                    }
                    showInfo('OK', r.message || 'Eliminado.', true);
                    setTimeout(function () { window.location.href = 'admin_gastos_list.aspx'; }, 900);
                },
                function (err) {
                    hideSaving();
                    console.error(err);
                    showInfo('Error', 'No se pudo eliminar.', false);
                }
            );
        }

        // ===========================
        // Eventos
        // ===========================
        $(document).ready(function () {
            
            // =============================================
            // DATEPICKER - Usando jQuery UI
            // =============================================
            $.datepicker.regional['es'] = {
                closeText: 'Cerrar',
                prevText: '&#x3C;Ant',
                nextText: 'Sig&#x3E;',
                currentText: 'Hoy',
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                    'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                    'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            };
            $.datepicker.setDefaults($.datepicker.regional['es']);

            // Inicializar ambos datepickers con jQuery UI
            $('#<%= date_emision.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '2020:2030'
            });

            $('#<%= date_pay.ClientID %>').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '2020:2030'
            });

            // =============================================
            // File input display
            // =============================================
            var fileInput = document.getElementById('<%= fuFile.ClientID %>');
            if (fileInput) {
                fileInput.addEventListener('change', function () {
                    var span = document.getElementById('fileNameDisplay');
                    if (this.files && this.files.length > 0) {
                        span.textContent = this.files[0].name;
                    } else {
                        span.textContent = '';
                    }
                });
            }

            // =============================================
            // Toggles
            // =============================================
            $('#<%= basic_chk.ClientID %>').on('change', toggleLH);
            $('#<%= basic_tax_chk.ClientID %>').on('change', toggleTax);

            // =============================================
            // Cálculo automático
            // =============================================
            $('#<%= basic_subtotal.ClientID %>,#<%= basic_iva.ClientID %>,#<%= basic_irpf.ClientID %>')
                .on('input', recalcTotalGasto);

            // =============================================
            // Sociedad change
            // =============================================
            $('#<%= basic_sociedad.ClientID %>').on('change', loadSecuence);

            // =============================================
            // Año change
            // =============================================
            $('#<%= ddlyear.ClientID %>').on('change', function () {
                var y = $(this).val();
                if (!y) return;

                var newDate = '01/01/' + y;
                var $emision = $('#<%= date_emision.ClientID %>');
                $emision.val(newDate);

                loadSecuence();
            });

            // =============================================
            // Áreas cascada
            // =============================================
            $('#<%= ddlArea.ClientID %>').on('change', function () {
                loadSub('SUBAREA', $(this).val(), '<%= ddlSubArea.ClientID %>');
                loadSub('SUBAREA2', '', '<%= ddlSubArea2.ClientID %>');
            });

            $('#<%= ddlSubArea.ClientID %>').on('change', function () {
                loadSub('SUBAREA2', $(this).val(), '<%= ddlSubArea2.ClientID %>');
            });

            // =============================================
            // Guardar
            // =============================================
            $('#btnSaveAjax').on('click', saveGastoAjax);

            // =============================================
            // Eliminar
            // =============================================
            $('#btnDeleteAjax').on('click', openDeleteModal);
            $('#btnConfirmDelete').on('click', function () {
                $('#confirm_modal').modal('hide');
                deleteGastoAjax();
            });

            // =============================================
            // Init
            // =============================================
            toggleLH();
            toggleTax();
            recalcTotalGasto();

            var currentId = Number($('#<%= basic_id.ClientID %>').val() || 0);
            if (currentId > 0) {
                $('#btnDeleteAjax').show();
            }

            setSaveButtonMode();
        });
    </script>
</body>
</html>