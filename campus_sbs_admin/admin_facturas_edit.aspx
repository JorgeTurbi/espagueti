<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_facturas_edit.aspx.cs" Inherits="campus_sbs_admin.admin_facturas_edit" %>

<%@ Register TagPrefix="uc_header" TagName="cabecera" Src="~/controls/header.ascx" %>
<%@ Register TagPrefix="uc_menu" TagName="menu" Src="~/controls/nav.ascx" %>

<!DOCTYPE html>
<html class="no-legacy-ie no-js" lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Editar Factura</title>

    <!-- Bootstrap / estilos base del proyecto -->
    <link rel="stylesheet" href="App_Themes/support/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/critical.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/async.min.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/support/css/fonts.min.css" />
    <link rel="Stylesheet" type="text/css" href="/App_Themes/support/css/sbs.css" />
    <link rel="stylesheet" href="App_Themes/support/css/sweetalert2.min.css" />

    <script type="text/javascript" src="/App_Themes/support/js/modernizr.js"></script>

    <style>
        .content-wrapper { margin-left: 260px; padding: 20px; }
        @media (max-width: 768px) { .content-wrapper { margin-left: 0; padding: 15px; } }

        input, select, textarea { border-radius: 10px !important; }
        .textolabel18 { font-size: 18px; }
        .box { resize: both; overflow:auto; width:100%; min-height:1% !important; }

        .file-pill {
            display:inline-flex; align-items:center; gap:10px;
            padding:8px 12px; border:1px solid #ddd; border-radius:10px;
            background:#fff;
        }

        /* SweetAlert2 fix */
        div.swal2-container { z-index: 99999 !important; }
        .swal-sbs { background:#fff !important; opacity:1 !important; }
    </style>
</head>

<body>
    <uc_menu:menu ID="menu" runat="server" />
    <header id="header" class="bg-color-primary affix">
        <uc_header:cabecera ID="cabecera" runat="server" />
    </header>

    <div class="content-wrapper">
        <main class="wrapper public bg-color-white" role="main">
            <h3 class="display-4 text-center">Editar Factura</h3>

            <form id="form1" runat="server" class="form-row">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />

                <section class="padding-tb-40 padding-xs-tb-30">
                    <div class="container-fluid">
                        <div class="card">
                            <div class="card-header d-flex justify-content-between align-items-center">
                                <div>
                                    <h5 class="mb-0">Detalle</h5>
                                    <small class="text-muted">ID: <span id="lblId">-</span></small>
                                </div>
                                <div>
                                    <a class="btn btn-secondary btn-sm" href="admin_facturas_list.aspx">
                                        Volver al listado
                                    </a>
                                    <a class="btn btn-success btn-sm" href="admin_facturas.aspx">
                                        Registrar nueva
                                    </a>
                                </div>
                            </div>

                            <div class="card-body">

                                <!-- Campos principales -->
                                <div class="row my-2">
                                    <div class="col-md-3">
                                        <label class="textolabel18">Sociedad</label>
                                        <select id="slSociedad" class="custom-select custom-select-md textolabel18">
                                            <option value="">Seleccione</option>
                                            <option value="SBS">SBS</option>
                                            <option value="SBSCS">SBSCS</option>
                                        </select>
                                    </div>

                                    <div class="col-md-3">
                                        <label class="textolabel18">Número</label>
                                        <input type="number" id="secuencia" class="form-control textolabel18" placeholder="Número" />
                                    </div>

                                    <div class="col-md-3">
                                        <label class="textolabel18">Fecha Emisión</label>
                                        <input type="date" id="femision" class="form-control textolabel18" />
                                    </div>

                                    <div class="col-md-3">
                                        <label class="textolabel18">Atribución</label>
                                        <select id="ddlAtribucion" class="custom-select custom-select-md textolabel18">
                                            <option value="">Selecciona una atribución</option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-md-6">
                                        <label class="textolabel18">Cliente</label>
                                        <input type="text" id="basic_client" class="form-control textolabel18" placeholder="Cliente *" />
                                    </div>

                                    <div class="col-md-6">
                                        <label class="textolabel18">NIF</label>
                                        <input type="text" id="basic_nif" class="form-control textolabel18" placeholder="NIF *" />
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-md-12">
                                        <label class="textolabel18">Descripción</label>
                                        <textarea id="descripcion" class="form-control box py-3 textolabel18" rows="2" placeholder="Descripción *"></textarea>
                                    </div>
                                </div>

                                <hr class="my-4" />

                                <!-- Importes -->
                                <div class="row my-3">
                                    <div class="col-md-3">
                                        <label class="textolabel18">PVP</label>
                                        <input type="text" id="basic_pvp" class="form-control textolabel18" placeholder="PVP *" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Beca</label>
                                        <input type="text" id="basic_beca" class="form-control textolabel18" placeholder="Beca" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Descuento</label>
                                        <input type="text" id="basic_dto" class="form-control textolabel18" placeholder="Descuento" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Precio</label>
                                        <input type="text" id="basic_precio" class="form-control textolabel18" placeholder="Precio *" oninput="validatePriceLocale(this)" />
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-md-3">
                                        <label class="textolabel18">Fundación</label>
                                        <input type="text" id="basic_fund" class="form-control textolabel18" placeholder="Fundación" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Universidad</label>
                                        <input type="text" id="basic_uni" class="form-control textolabel18" placeholder="Universidad" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Tripartita</label>
                                        <input type="text" id="basic_trip" class="form-control textolabel18" placeholder="Tripartita *" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Total</label>
                                        <input type="text" id="basic_total" class="form-control textolabel18" placeholder="Total *" oninput="validatePriceLocale(this)" />
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-md-3">
                                        <label class="textolabel18">IVA</label>
                                        <input type="text" id="basic_iva" class="form-control textolabel18" placeholder="IVA" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">IRPF</label>
                                        <input type="text" id="basic_irpf" class="form-control textolabel18" placeholder="IRPF" oninput="validatePriceLocale(this)" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Fecha Vencimiento</label>
                                        <input type="date" id="date_venc" class="form-control textolabel18" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="textolabel18">Fecha Cobro</label>
                                        <input type="date" id="date_payment" class="form-control textolabel18" />
                                    </div>
                                </div>

                                <hr class="my-4" />

                                <!-- Adjuntos -->
                                <div class="row my-2">
                                    <div class="col-md-12">
                                        <label class="textolabel18">Adjunto</label>

                                        <div class="d-flex flex-wrap align-items-center" style="gap:10px;">
                                            <!-- input file -->
                                            <input type="file" id="fuFactura" style="display:none;" />

                                            <button type="button" class="btn btn-primary btn-sm" id="btnSelectFile">
                                                <i class="fas fa-upload mr-2"></i> Seleccionar archivo
                                            </button>

                                            <span id="fileName" class="text-muted small"></span>

                                            <!-- actual file -->
                                            <span id="currentFileWrap" class="file-pill" style="display:none;">
                                                <a id="lnkCurrentFile" target="_blank" href="#">Archivo</a>
                                                <button type="button" class="btn btn-outline-danger btn-sm" id="btnRemoveFile">
                                                    Quitar
                                                </button>
                                            </span>
                                        </div>

                                        <small class="text-muted d-block mt-2">
                                            Si seleccionas un archivo nuevo, se subirá y reemplazará el nombre guardado en la factura.
                                        </small>
                                    </div>
                                </div>

                                <div class="row my-3">
                                    <div class="col-md-12">
                                        <label class="textolabel18">Comentarios</label>
                                        <textarea id="comentario" class="form-control box py-3 textolabel18" rows="2" placeholder="Comentarios"></textarea>
                                    </div>
                                </div>

                                <div class="row my-4">
                                    <div class="col-12 d-flex justify-content-center" style="gap:10px;">
                                        <button type="button" class="btn btn-danger" id="btnDeleteFactura">
                                            Eliminar factura
                                        </button>

                                        <button type="button" class="btn btn-success" id="btnUpdateFactura">
                                            Guardar cambios
                                        </button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </section>
            </form>
        </main>
    </div>

    <!-- JS base -->
    <script type="text/javascript" src="/App_Themes/support/js/critical.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/async.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/valkyrie-nav.js"></script>
    <script type="text/javascript" src="/App_Themes/support/js/internal/functions.js"></script>
    <script type="text/javascript" src="App_Themes/support/js/upload/sweetalert2011.js"></script>

    <script>
        // ========= Helpers =========
        function swalError(msg) {
            Swal.fire({ icon: 'error', title: 'Error', text: msg, customClass: { popup: 'swal-sbs' } });
        }
        function swalOk(msg) {
            Swal.fire({ icon: 'success', title: 'OK', text: msg, customClass: { popup: 'swal-sbs' } });
        }
        function getDecimalSeparator() {
            return (1.1).toLocaleString().substring(1, 2);
        }
        function validatePriceLocale(input) {
            const decimalSeparator = getDecimalSeparator();
            const regexNotAllowed = new RegExp(`[^0-9${decimalSeparator}]`, 'g');
            const regexExtraDecimal = new RegExp(`\\${decimalSeparator}.*\\${decimalSeparator}`, 'g');
            input.value = input.value.replace(regexNotAllowed, '').replace(regexExtraDecimal, match => match.slice(0, -1));
        }
        function parseLocaleNumber(val) {
            if (!val) return 0;
            const dec = getDecimalSeparator();
            val = (val + '').trim();
            const thousands = (dec === ',') ? '.' : ',';
            val = val.split(thousands).join('');
            if (dec !== '.') val = val.replace(dec, '.');
            const num = Number(val);
            return isNaN(num) ? NaN : num;
        }

        function getIdFromQuery() {
            const p = new URLSearchParams(window.location.search);
            const id = p.get('id');
            return id ? Number(id) : 0;
        }

        function setCurrentFile(fileName) {
            const wrap = document.getElementById('currentFileWrap');
            const link = document.getElementById('lnkCurrentFile');

            if (!fileName) {
                wrap.style.display = 'none';
                link.href = '#';
                link.textContent = '';
                return;
            }

            // OJO: tu ruta es ~/documentos/facturas/
            const safe = encodeURIComponent(fileName);
            link.href = '/documentos/facturas/' + safe;
            link.textContent = fileName;
            wrap.style.display = 'inline-flex';
        }

        // ========= Load (GetFactura) =========
        function loadFactura() {
            const id = getIdFromQuery();
            if (!id) return swalError('ID inválido en la URL.');

            Swal.fire({ title: 'Cargando...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

            $.ajax({
                url: 'admin_facturas_edit.aspx/GetFactura',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ id: id }),
                success: function (res) {
                    Swal.close();
                    const r = (res && res.d) ? res.d : null;
                    if (!r || !r.ok) return swalError((r && r.message) ? r.message : 'No se pudo cargar.');

                    const f = r.data;

                    $('#lblId').text(f.idInfFinFacturas);
                    $('#slSociedad').val(f.sociedad || '');
                    $('#secuencia').val(f.numero || '');
                    $('#femision').val(f.fecha_emision || '');
                    $('#basic_client').val(f.cliente_nombre || '');
                    $('#basic_nif').val(f.cliente_nif || '');
                    $('#descripcion').val(f.descripcion || '');

                    $('#basic_pvp').val(f.eur_pvp_str || '0');
                    $('#basic_beca').val(f.eur_beca_str || '0');
                    $('#basic_dto').val(f.eur_dto_str || '0');
                    $('#basic_precio').val(f.eur_precio_str || '0');

                    $('#basic_fund').val(f.eur_fundacion_str || '0');
                    $('#basic_uni').val(f.eur_universidad_str || '0');
                    $('#basic_trip').val(f.eur_tripartita_str || '0');

                    $('#basic_iva').val(f.eur_iva_str || '0');
                    $('#basic_irpf').val(f.eur_irpf_str || '0');
                    $('#basic_total').val(f.eur_total_str || '0');

                    $('#date_venc').val(f.fecha_vencimiento || '');
                    $('#date_payment').val(f.fecha_cobro || '');

                    $('#comentario').val(f.comentarios || '');
                    $('#ddlAtribucion').val(f.atribucion || '');

                    setCurrentFile(f.fichero || null);
                },
                error: function (xhr) {
                    console.error(xhr);
                    Swal.close();
                    swalError('No se pudo cargar la factura.');
                }
            });
        }

        // ========= Atribuciones =========
        function loadAtribuciones() {
            $.ajax({
                url: 'admin_facturas_edit.aspx/LoadAtribuciones',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({}),
                success: function (res) {
                    const r = (res && res.d) ? res.d : [];
                    const ddl = $('#ddlAtribucion');
                    ddl.empty();
                    ddl.append(`<option value="">Selecciona una atribución</option>`);
                    r.forEach(x => ddl.append(`<option value="${x.codigo}">${x.nombre}</option>`));
                }
            });
        }

        // ========= Build DTO =========
        function buildDtoForUpdate(fileNameFromUploadOrNull) {
            const id = getIdFromQuery();
            return {
                idInfFinFacturas: id,
                sociedad: $('#slSociedad').val(),
                numero: Number($('#secuencia').val()),
                fecha_emision: $('#femision').val(),

                cliente_nombre: $('#basic_client').val(),
                cliente_nif: $('#basic_nif').val(),
                descripcion: $('#descripcion').val(),

                eur_pvp: parseLocaleNumber($('#basic_pvp').val()),
                eur_beca: parseLocaleNumber($('#basic_beca').val()) || 0,
                eur_dto: parseLocaleNumber($('#basic_dto').val()) || 0,
                eur_precio: parseLocaleNumber($('#basic_precio').val()),

                eur_fundacion: parseLocaleNumber($('#basic_fund').val()) || 0,
                eur_universidad: parseLocaleNumber($('#basic_uni').val()) || 0,
                eur_tripartita: parseLocaleNumber($('#basic_trip').val()),
                eur_iva: parseLocaleNumber($('#basic_iva').val()) || 0,
                eur_irpf: parseLocaleNumber($('#basic_irpf').val()) || 0,
                eur_total: parseLocaleNumber($('#basic_total').val()),

                fecha_vencimiento: $('#date_venc').val() || null,
                fecha_cobro: $('#date_payment').val() || null,

                comentarios: $('#comentario').val() || null,
                atribucion: $('#ddlAtribucion').val() || null,

                // si subimos archivo nuevo => usar el nombre nuevo
                // si no => dejar null y el back mantendrá el actual (según implementación)
                fichero: fileNameFromUploadOrNull
            };
        }

        // ========= Update =========
        function updateFactura(dto) {
            $.ajax({
                url: 'admin_facturas_edit.aspx/UpdateFactura',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ id: getIdFromQuery(), dto: dto }),
                success: function (res) {
                    const r = (res && res.d) ? res.d : null;
                    if (!r || !r.ok) {
                        Swal.close();
                        return swalError((r && r.message) ? r.message : 'No se pudo actualizar.');
                    }

                    Swal.fire({
                        icon: 'success',
                        title: 'OK',
                        text: r.message || 'Actualizado.',
                        customClass: { popup: 'swal-sbs' }
                    }).then(() => {
                        // refrescar para ver cambios + link
                        location.reload();
                    });
                },
                error: function (xhr) {
                    console.error(xhr);
                    Swal.close();
                    swalError('No se pudo conectar con el servidor.');
                }
            });
        }

        // ========= Upload file first (optional) =========
        function uploadIfNeededThenSave() {
            Swal.fire({ title: 'Guardando...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

            const fileInput = document.getElementById('fuFactura');
            const hasFile = fileInput && fileInput.files && fileInput.files.length > 0;

            // no file => update sin cambiar fichero
            if (!hasFile) {
                const dto = buildDtoForUpdate(null);
                return updateFactura(dto);
            }

            // file => upload primero
            const fd = new FormData();
            fd.append('file', fileInput.files[0]);

            $.ajax({
                url: '/Admin/UploadFactura.ashx',  // ajusta si tu handler está en otra ruta
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
                        Swal.close();
                        return swalError((r && r.message) ? r.message : 'No se pudo subir el archivo.');
                    }

                    const newFileName = r.fileName;
                    const dto = buildDtoForUpdate(newFileName);
                    updateFactura(dto);
                },
                error: function (xhr) {
                    console.error(xhr);
                    Swal.close();
                    swalError('No se pudo subir el archivo.');
                }
            });
        }

        // ========= Remove file =========
        function removeFileFromFactura() {
            const id = getIdFromQuery();
            if (!id) return swalError('ID inválido.');

            Swal.fire({
                icon: 'warning',
                title: 'Quitar adjunto',
                text: '¿Seguro que deseas eliminar el archivo adjunto de esta factura?',
                showCancelButton: true,
                confirmButtonText: 'Sí, quitar',
                cancelButtonText: 'Cancelar',
                customClass: { popup: 'swal-sbs' }
            }).then((rr) => {
                if (!rr.isConfirmed) return;

                Swal.fire({ title: 'Eliminando...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

                $.ajax({
                    url: 'admin_facturas_edit.aspx/RemoveFile',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ id: id }),
                    success: function (res) {
                        const r = (res && res.d) ? res.d : null;
                        if (!r || !r.ok) {
                            Swal.close();
                            return swalError((r && r.message) ? r.message : 'No se pudo quitar el archivo.');
                        }
                        Swal.close();
                        setCurrentFile(null);
                        swalOk(r.message || 'Adjunto eliminado.');
                    },
                    error: function (xhr) {
                        console.error(xhr);
                        Swal.close();
                        swalError('No se pudo quitar el archivo.');
                    }
                });
            });
        }

        // ========= Delete factura =========
        function deleteFactura() {
            const id = getIdFromQuery();
            if (!id) return swalError('ID inválido.');

            Swal.fire({
                icon: 'warning',
                title: 'Eliminar factura',
                text: '¿Seguro que deseas eliminar la factura? (se eliminará también el archivo si existe)',
                showCancelButton: true,
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar',
                customClass: { popup: 'swal-sbs' }
            }).then((rr) => {
                if (!rr.isConfirmed) return;

                Swal.fire({ title: 'Eliminando...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

                $.ajax({
                    url: 'admin_facturas_edit.aspx/DeleteFactura',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ id: id }),
                    success: function (res) {
                        const r = (res && res.d) ? res.d : null;
                        if (!r || !r.ok) {
                            Swal.close();
                            return swalError((r && r.message) ? r.message : 'No se pudo eliminar.');
                        }
                        Swal.fire({ icon: 'success', title: 'OK', text: r.message || 'Eliminado.', customClass: { popup: 'swal-sbs' } })
                            .then(() => window.location.href = 'admin_facturas_list.aspx');
                    },
                    error: function (xhr) {
                        console.error(xhr);
                        Swal.close();
                        swalError('No se pudo eliminar.');
                    }
                });
            });
        }

        // ========= UI events =========
        $(document).ready(function () {
            loadAtribuciones();
            loadFactura();

            $('#btnSelectFile').on('click', function () {
                document.getElementById('fuFactura').click();
            });

            $('#fuFactura').on('change', function () {
                const el = document.getElementById('fuFactura');
                if (el.files && el.files.length > 0) {
                    $('#fileName').text(el.files[0].name);
                } else {
                    $('#fileName').text('');
                }
            });

            $('#btnRemoveFile').on('click', removeFileFromFactura);
            $('#btnUpdateFactura').on('click', uploadIfNeededThenSave);
            $('#btnDeleteFactura').on('click', deleteFactura);
        });
    </script>
</body>
</html>--%>
