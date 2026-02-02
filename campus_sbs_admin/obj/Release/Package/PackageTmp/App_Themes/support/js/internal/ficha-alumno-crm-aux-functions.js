// --------------------------------------------------------------------------------------------------
// Funciones carga inicial --------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Inicializar los datepicker
    $(".input-group.date").datepicker({
        language: "es",
        autoclose: true,
        todayHighlight: true
    });

    /// 2.- Inicializa las tablas
    paint_table_inf_tripartita();
    paint_table_avance();

    /// 3.- Llamar a la función de fileupload
    start_fileupload('fileupload_documento');
});

// --------------------------------------------------------------------------------------------------
// Funciones para validar el formulario -------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function validarFormularioAP() {
    /// 0.- Deshabilitar el botón
    $('#btn_save').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var origen = $('#ddlOrigenAP').val();
    var curso = $('#ddlCursoAP').val();
    var fecha_lead = $('#txtFechaLead').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#origen_ap_form').hasClass("has-error"))
        $('#origen_ap_form').removeClass("has-error");
    if ($('#curso_ap_form').hasClass("has-error"))
        $('#curso_ap_form').removeClass("has-error");
    if ($('#txtFechaLead').hasClass("is-invalid"))
        $('#txtFechaLead').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (origen === "-1") {
        $('#origen_ap_form').addClass(' has-error');
        $('#txt_error').html('El origen es obligatorio');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (curso === "-1") {
        $('#curso_ap_form').addClass(' has-error');
        $('#txt_error').html('El curso es obligatorio');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (fecha_lead === "undefined" || fecha_lead === undefined || fecha_lead === "null" || fecha_lead === null || fecha_lead === '') {
        $('#txtFechaLead').addClass(' is-invalid');
        $('#txt_error').html('La fecha del lead es obligatoria');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardar').click();
        return true;
    }
}
function validarFormularioOR() {
    /// 0.- Deshabilitar el botón
    $('#btn_save_origen').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var origen = $('#ddlOrigenOR').val();
    var fecha_origen = $('#txtFechaOrigen').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#origen_or_form').hasClass("has-error"))
        $('#origen_or_form').removeClass("has-error");
    if ($('#txtFechaOrigen').hasClass("is-invalid"))
        $('#txtFechaOrigen').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (origen === "-1") {
        $('#origen_or_form').addClass(' has-error');
        $('#txt_error').html('El origen es obligatorio');
        $('#btn_save_origen').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (fecha_origen === "undefined" || fecha_origen === undefined || fecha_origen === "null" || fecha_origen === null || fecha_origen === '') {
        $('#txtFechaOrigen').addClass(' is-invalid');
        $('#txt_error').html('La fecha del lead es obligatoria');
        $('#btn_save_origen').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardarOrigen').click();
        return true;
    }
}
function validarFormularioLnk() {
    /// 0.- Deshabilitar el botón
    $('#btn_save_link').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var tipo_link = $('#ddlOrigenOR').val();
    var url_link = $('#txt_url_link').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#lnk_tipo_form').hasClass("has-error"))
        $('#lnk_tipo_form').removeClass("has-error");
    if ($('#txt_url_link').hasClass("is-invalid"))
        $('#txt_url_link').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (tipo_link === "-1") {
        $('#lnk_tipo_form').addClass(' has-error');
        $('#txt_error').html('El tipo de link es obligatorio');
        $('#btn_save_link').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (url_link === "undefined" || url_link === undefined || url_link === "null" || url_link === null || url_link === '') {
        $('#txt_url_link').addClass(' is-invalid');
        $('#txt_error').html('La Url del link es obligatoria');
        $('#btn_save_link').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardarLink').click();
        return true;
    }
}
function validarFormularioFund() {
    /// 0.- Deshabilitar el botón
    $('#btn_save_fund').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var fecha_fundacion = $('#txtFechaSeguimientoFund').val();
    var beca_fundacion = $('#txt_beca_fund').val();
    var descuento_fundacion = $('#txt_desc_fund').val();
    var comentarios = $('#txt_comentarios_fund').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#txtFechaSeguimientoFund').hasClass("is-invalid"))
        $('#txtFechaSeguimientoFund').removeClass("is-invalid");
    if ($('#txt_beca_fund').hasClass("is-invalid"))
        $('#txt_beca_fund').removeClass("is-invalid");
    if ($('#txt_desc_fund').hasClass("is-invalid"))
        $('#txt_desc_fund').removeClass("is-invalid");
    if ($('#txt_comentarios_fund').hasClass("is-invalid"))
        $('#txt_comentarios_fund').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (fecha_fundacion === "undefined" || fecha_fundacion === undefined || fecha_fundacion === "null" || fecha_fundacion === null || fecha_fundacion === '') {
        $('#txtFechaSeguimientoFund').addClass(' is-invalid');
        $('#txt_error').html('La fecha de la fundación es obligatoria');
        $('#btn_save_fund').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (beca_fundacion === "undefined" || beca_fundacion === undefined || beca_fundacion === "null" || beca_fundacion === null || beca_fundacion === '' || !validarTelefono(beca_fundacion)) {
        $('#txt_beca_fund').addClass(' is-invalid');
        $('#txt_error').html('La Beca es obligatoria');
        $('#btn_save_fund').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (descuento_fundacion === "undefined" || descuento_fundacion === undefined || descuento_fundacion === "null" || descuento_fundacion === null || descuento_fundacion === '' || !validarTelefono(descuento_fundacion)) {
        $('#txt_desc_fund').addClass(' is-invalid');
        $('#txt_error').html('El descuento es obligatorio');
        $('#btn_save_fund').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (comentarios === "undefined" || comentarios === undefined || comentarios === "null" || comentarios === null || comentarios === '') {
        $('#txt_error').html('El campo comentarios es obligatorio');
        $('#txt_comentarios_fund').addClass(' is-invalid');
        $('#btn_save_fund').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardarFund').click();
        return true;
    }
}
function validarFormularioAsig() {
    /// 0.- Deshabilitar el botón
    $('#btn_save_asig').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var fecha_asignacion = $('#txtFechaAsignacionComercial').val();
    var comercial = $('#ddlComerciales').val();
    var precio = $('#txt_precio_asig').val();
    var docencia = $('#ddlDocenciaAsig').val();
    var curso = $('#ddlCursoAsig').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#txtFechaAsignacionComercial').hasClass("is-invalid"))
        $('#txtFechaAsignacionComercial').removeClass("is-invalid");
    if ($('#comercial_asig_form').hasClass("has-error"))
        $('#comercial_asig_form').removeClass("has-error");
    if ($('#txt_precio_asig').hasClass("is-invalid"))
        $('#txt_precio_asig').removeClass("is-invalid");
    if ($('#doc_asig_form').hasClass("has-error"))
        $('#doc_asig_form').removeClass("has-error");
    if ($('#curso_asig_form').hasClass("has-error"))
        $('#curso_asig_form').removeClass("has-error");

    /// 3.- Validar los datos
    if (fecha_asignacion === "undefined" || fecha_asignacion === undefined || fecha_asignacion === "null" || fecha_asignacion === null || fecha_asignacion === '') {
        $('#txtFechaAsignacionComercial').addClass(' is-invalid');
        $('#txt_error').html('La fecha de la asignación comercial es obligatoria');
        $('#btn_save_asig').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    if (comercial === "-1") {
        $('#comercial_asig_form').addClass(' has-error');
        $('#txt_error').html('El comercial es obligatorio');
        $('#btn_save_asig').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    if (precio === "undefined" || precio === undefined || precio === "null" || precio === null || precio === '') {
        $('#txt_precio_asig').addClass(' is-invalid');
        $('#txt_error').html('El precio de la asignación comercial es obligatoria');
        $('#btn_save_asig').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    if (docencia === "-1") {
        $('#doc_asig_form').addClass(' has-error');
        $('#txt_error').html('La docencia es obligatoria');
        $('#btn_save_asig').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (curso === "-1") {
        $('#curso_asig_form').addClass(' has-error');
        $('#txt_error').html('El curso es obligatorio');
        $('#btn_save_asig').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardarAsig').click();
        return true;
    }
}
function validarFormularioPay() {
    /// 0.- Deshabilitar el botón
    $('#btn_save_pay').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var fecha_estimada = $('#txtFechaEstimada').val();
    var precio_estimado = $('#txt_euros_est').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#txtFechaEstimada').hasClass("is-invalid"))
        $('#txtFechaEstimada').removeClass("is-invalid");
    if ($('#txt_euros_est').hasClass("is-invalid"))
        $('#txt_euros_est').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (fecha_estimada === "undefined" || fecha_estimada === undefined || fecha_estimada === "null" || fecha_estimada === null || fecha_estimada === '') {
        $('#txtFechaEstimada').addClass(' is-invalid');
        $('#txt_error').html('La fecha estimada es obligatoria');
        $('#btn_save_pay').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    if (precio_estimado === "undefined" || precio_estimado === undefined || precio_estimado === "null" || precio_estimado === null || precio_estimado === '') {
        $('#txt_euros_est').addClass(' is-invalid');
        $('#txt_error').html('Los euros estimados son obligatorios');
        $('#btn_save_pay').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardarPago').click();
        return true;
    }
}
function validarFormularioDoc() {
    /// 0.- Deshabilitar el botón
    $('#btn_save_doc').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var tipo = $('#ddlTipoDoc').val();
    var fichero = $('#documento_usuario').val();
    var descripcion = $('#txt_descripcion_doc').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#type_doc_form').hasClass("has-error"))
        $('#type_doc_form').removeClass("has-error");
    if ($('#documento_usuario').hasClass("is-invalid"))
        $('#documento_usuario').removeClass("is-invalid");
    if ($('#txt_descripcion_doc').hasClass("is-invalid"))
        $('#txt_descripcion_doc').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (tipo === "undefined" || tipo === undefined || tipo === "null" || tipo === null || tipo === '') {
        $('#txt_error').html('El campo Tipo de documento es obligatorio');
        $('#type_doc_form').addClass(' has-error');
        $('#btn_save_doc').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (tipo !== 'Url' && (fichero === "undefined" || fichero === undefined || fichero === "null" || fichero === null || fichero === '')) {
        $('#txt_error').html('Hay que subir un documento');
        $('#btn_save_doc').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (descripcion === "undefined" || descripcion === undefined || descripcion === "null" || descripcion === null || descripcion === '') {
        $('#txt_error').html('El campo descripción es obligatorio');
        $('#txt_descripcion_doc').addClass(' is-invalid');
        $('#btn_save_doc').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardarDoc').click();
        return true;
    }
}
function validarFormularioComentario() {
    /// 0.- Deshabilitar el botón
    $('#btn_save_comentarios').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var comentarios = $('#txt_comentarios_user').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#txt_comentarios_user').hasClass("is-invalid"))
        $('#txt_comentarios_user').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (comentarios === "undefined" || comentarios === undefined || comentarios === "null" || comentarios === null || comentarios === '') {
        $('#txt_error').html('El campo comentarios es obligatorio');
        $('#txt_comentarios_user').addClass(' is-invalid');
        $('#btn_save_comentarios').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardarComentario').click();
        return true;
    }
}
function validarUsuario() {
    /// 0.- Deshabilitar el botón
    $('#btn_search').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var idUsuario = getParams('idu');
    var idUsuarioOld = $('#txt_user_old').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#txt_user_old').hasClass("is-invalid"))
        $('#txt_user_old').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (idUsuarioOld === "undefined" || idUsuarioOld === undefined || idUsuarioOld === "null" || idUsuarioOld === null || idUsuarioOld === '' || !validarTelefono(idUsuarioOld)) {
        $('#txt_error').html('El campo Usuario a eliminar es obligatorio');
        $('#txt_user_old').addClass(' is-invalid');
        $('#btn_search').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        var url = "/ficha-alumno-crm-aux.aspx?idu=" + idUsuario + "&ido=" + idUsuarioOld + "&idta=14";
        location.href = url;
    }
}

// --------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function start_fileupload(name) {
    /// 1.- Eliminar los datos de la sesión
    sessionStorage.clear();

    /// 2.- Función que sube un fichero al servidor
    $('#' + name + '').fileupload({
        dataType: 'json',
        dropZone: $('#file_documento'),
        maxNumberOfFiles: 1,
        done: function (e, data) {
            var file_doc = '';

            var row = '';
            $.each(data.result.files, function (index, file) {
                row = "<tr class='template-download'><td>";
                if (file.url)
                    row += "<a href='" + file.url + "' title='" + file.name + "' download='" + file.name + "'>" + file.name + "</a>";
                else
                    row += "<span>" + file.name + "</span>";
                if (file.error)
                    row += "<div><span class='label label-danger'>Error</span> " + file.error + "</div>";
                else
                    file_doc = file.name;
                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl)
                    row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','file_doc')\"><i class='fas fa-trash-alt'></i></button>";
                else
                    row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');

            if (file_doc !== '') {
                $('#modal-close').click();
                $('#documento_usuario').val(file_doc);
                if ($('#block_delete_documento').hasClass("hidden"))
                    $('#block_delete_documento').removeClass("hidden");
                if (!$('#block_upload_documento').hasClass("hidden"))
                    $('#block_upload_documento').addClass(" hidden");
                if ($('#block_see').hasClass("hidden"))
                    $('#block_see').removeClass("hidden");

                var link = "http://media.spainbs.com/alumnos/ficha/temp/" + file_doc;
                $('#lnk_documento').attr('href', link);

                if (!$('#delete_doc').hasClass("hidden"))
                    $('#delete_doc').addClass(" hidden");

                $('#delete_doc_user').html("<a class='fas fa-times-circle fa-2x text-red' onclick=\"delete_file('" + file_doc + "','file_doc')\" href='javascript:void(0);' title='Eliminar documento' />");
            }
        },
        progressall: function (e, data) {
            $('#progress').removeClass('hidden');
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css('width', progress + '%');
        }
    });
}
function delete_file(name, type) {
    var _file = getfile(name);
    if (_file !== null && _file !== undefined) {
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: _file[0].deleteUrl + "&type=" + type + "&accion=delete",
            data: "",
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                var row = '';
                if (data.files.length === 0) {
                    deleteFileData();
                    $('#documento_usuario').val('');
                    if ($('#block_upload_documento').hasClass("hidden"))
                        $('#block_upload_documento').removeClass(" hidden");
                    if (!$('#block_see').hasClass("hidden"))
                        $('#block_see').addClass(" hidden");
                    if (!$('#block_delete_documento').hasClass("hidden"))
                        $('#block_delete_documento').addClass(" hidden");  

                    if ($('#delete_doc').hasClass("hidden"))
                        $('#delete_doc').removeClass("hidden");

                    $('#delete_doc_user').html('');
                }
            },
            error: function (response) {
                return false;
            },
            failure: function (response) {
                return false;
            }
        });
    }
}
function getfile(name) {
    var file_data = [];
    var _datos = getFileData() !== "" ? JSON.parse(getFileData()) : [];
    if (_datos.length > 0) {
        var list_files = [];

        $.each(_datos, function (index, file) {
            list_files.push(file);
        });

        if (name !== '') {
            $.each(list_files, function (index, _file) {
                if (_file.name === name)
                    file_data.push(_file);
            });
        }
        else {
            $.each(list_files, function (index, _file) {
                file_data.push(_file);
            });
        }
    }
    return file_data;
}
function save_file(file, del) {
    var list_files = [];
    _files = getFileData() !== "" ? JSON.parse(getFileData()) : [];
    $.each(_files, function (index, _file) {
        list_files.push(_file);
    });
    if (!del) {
        if (file.result !== null && file.result !== undefined)
            list_files.push(file.result.files[0]);
        else if (file.files !== null && file.files !== undefined)
            list_files.push(file.files[0]);
    }
    else {
        list_files = [];
        if (file.result !== null && file.result !== undefined)
            list_files.push(file.result.files[0]);
        else if (file.files !== null && file.files !== undefined) {
            $.each(file.files, function (index, _file) {
                list_files.push(_file);
            });
        }
    }

    /// Guardar en sesion los datos del fichero subido
    setFileData(JSON.stringify(list_files));
}

// --------------------------------------------------------------------------------------------------
// Funciones para los documentos --------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function delete_documento() {
    /// 1.- Sacar los parámetros
    var id_document = getParams('idd');
    var documento = $('#documento_usuario').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Eliminar la foto
    var urlWS = "ficha-alumno-crm-aux.aspx/eliminar_documento";
    var data = "{'id_document' : '" + id_document + "', 'document' : '" + documento + "'}";
    $.ajax({
        type: "POST",
        crossDomain: true,
        url: urlWS,
        data: data,
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            var _delete = data.d;
            if (_delete) {
                if ($('#upload_documento').hasClass("hidden"))
                    $('#upload_documento').removeClass("hidden");
                if (!$('#delete_documento').hasClass("hidden"))
                    $('#delete_documento').addClass(" hidden");
                if (!$('#block_see').hasClass("hidden"))
                    $('#block_see').addClass(" hidden");

                $('#documento_usuario').val('');
                return true;
            }
            else {
                alert('Se ha producido un error al eliminar el documento del usuario');
                return false;
            }
        },
        error: function (response) {
            alert('Se ha producido un error al eliminar el documento del usuario');
            return false;
        },
        failure: function (response) {
            alert('Se ha producido un error al eliminar el documento del usuario');
            return false;
        }
    });
}

// --------------------------------------------------------------------------------------------------
// Tablas de los bloques ----------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function paint_table_inf_tripartita() {
    $('#tabla_inf_tripartita').DataTable({
        responsive: true,
        language: {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"

            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        lengthMenu: [[20, 50, -1], [20, 50, "All"]],
        "columnDefs": [
          {
              "targets": [0],
              "className": "text-center",
              "type": "euro_date"
          },
          {
              "targets": [1],
              "className": "text-center"
          },
          {
              "targets": [2]
          },
          {
              "targets": [3]
          },
          {
              "targets": [4]
          }
        ],
        "order": [[0, "desc"]]
    });
}
function paint_table_avance() {
    $('#tabla_avance').DataTable({
        responsive: true,
        language: {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"

            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        lengthMenu: [[20, 50, -1], [20, 50, "All"]],
        "columnDefs": [
          {
              "targets": [0]
          },
          {
              "targets": [1]
          },
          {
              "targets": [2],
              "className": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [3],
              "className": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [4],
              "className": "text-center"
          },
          {
              "targets": [5]
          },
          {
              "targets": [6],
              "className": "text-center"
          }
        ],
        "order": [[0, "asc"]]
    });
}