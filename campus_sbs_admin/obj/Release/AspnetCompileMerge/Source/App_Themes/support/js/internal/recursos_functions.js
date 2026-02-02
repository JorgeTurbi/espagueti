// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_rec_int');
});

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function start_fileupload(name) {
    /// 1.- Eliminar los datos de la sesión
    sessionStorage.clear();

    /// 2.- Función que sube un fichero al servidor
    $('#' + name + '').fileupload({
        dataType: 'json',
        dropZone: $('.fileinput-button'),
        maxNumberOfFiles: 1,
        done: function (e, data) {
            var file_rect_int = '';

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
                    file_rect_int = file.name;
                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl)
                    row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','file_rec_int')\"><i class='fas fa-trash-alt'></i></button>";
                else
                    row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');
            
            //$('#tbl_foto .files').append(row);

            if (file_rect_int !== '') {
                $('#modal-close').click();
                $('#txt_rec_int').val(file_rect_int);
                if ($('#block_delete_rec_int').hasClass("hidden"))
                    $('#block_delete_rec_int').removeClass("hidden");
                if (!$('#block_upload_rec_int').hasClass("hidden"))
                    $('#block_upload_rec_int').addClass(" hidden");
                if ($('#block_see').hasClass("hidden"))
                    $('#block_see').removeClass("hidden");

                var link = "https://media.spainbs.com/academico/NT/temp/" + file_rect_int;
                $('#lnk_rec_int').attr('href', link);
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
                if (data.files.length > 0) {
                    $.each(data.files, function (index, file) {
                        row += "<tr class='template-download'><td>";
                        if (file.url)
                            row += "<a href='" + file.url + "' title='" + file.name + "' target='_blank' download='" + file.name + "'>" + file.name + "</a>";
                        else
                            row += "<span>" + file.name + "</span>";
                        if (file.error)
                            row += "<div><span class='label label-danger'>Error</span> " + file.error + "</div>";
                        row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                        if (file.deleteUrl)
                            row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + ", " + type + "')\"><i class='fas fa-trash-alt'></i></button>";
                        else
                            row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                        row += "</td></tr>";
                    });
                    save_file(data, true);

                    $('table .files').html('');
                    $('table .files').append(row);
                }
                else {
                    deleteFileData();
                    $('table .files').html('');
                    $('table .files').append(row);
                    $('#txt_rec_int').val('');
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
function delete_rec_int() {
    /// 1.- Sacar los parámetros
    var id_recurso = getParams('idr');
    var recurso_interno = $('#txt_rec_int').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Eliminar la foto
    var urlWS = "recurso-mantenimiento.aspx/delete_rec_int";
    var data = "{'id_recurso' : '" + id_recurso + "', 'recurso_interno' : '" + recurso_interno + "'}";
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
                $('#modal-close').click();
                $('#txt_rec_int').val('');

                if (!$('#block_delete_rec_int').hasClass("hidden"))
                    $('#block_delete_rec_int').addClass(" hidden");
                if ($('#block_upload_rec_int').hasClass("hidden"))
                    $('#block_upload_rec_int').removeClass("hidden");
                if (!$('#block_see').hasClass("hidden"))
                    $('#block_see').addClass("hidden");

                $('#lnk_rec_int').attr('href', '');
                return true;
            }
            else {
                alert('Se ha producido un error al eliminar el recurso interno');
                return false;
            }
        },
        error: function (response) {
            alert('Se ha producido un error al eliminar el recurso interno');
            return false;
        },
        failure: function (response) {
            alert('Se ha producido un error al eliminar el recurso interno');
            return false;
        }
    });
}

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parámetros
    var titulo = $('#txt_titulo').val();
    var tipo = $('#ddlTipo').val();
    var area = $('#ddlArea').val();
    var version = $('#resource_version').val();
    var derechos = $('#resource_derechos').val();
    var recurso_int = $('#txt_rec_int').val();
    var recurso_ext = $('#txt_rec_ext').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Comprobar los datos
    if (titulo === "undefined" || titulo === undefined || titulo === "null" || titulo === null || titulo === '') {
        $('#titulo_form').addClass(' has-error');
        $('#txt_error').html('El campo Título es obligatorio');
        $('#txt_nombre').attr("placeholder", "El campo Título es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (tipo === "-1") {
        $('#tipo_form').addClass(' has-error');
        $('#txt_error').html('El campo Tipo es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (area === "-1") {
        $('#area_form').addClass(' has-error');
        $('#txt_error').html('El área es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (version === "undefined" || version === undefined || version === "null" || version === null || version === '' || !validarTelefono(version)) {
        $('#resource_version_form').addClass(' has-error');
        $('#txt_error').html('El campo versión es obligatorio');
        $('#resource_version').attr("placeholder", "El campo versión es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (derechos === "undefined" || derechos === undefined || derechos === "null" || derechos === null || derechos === '') {
        $('#resource_derechos_form').addClass(' has-error');
        $('#txt_error').html('El campo derechos es obligatorio');
        $('#resource_derechos').attr("placeholder", "El campo derechos es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if ((recurso_int === "undefined" || recurso_int === undefined || recurso_int === "null" || recurso_int === null || recurso_int === '') && (recurso_ext === "undefined" || recurso_ext === undefined || recurso_ext === "null" || recurso_ext === null || recurso_ext === '')) {
        $('#txt_error').html('Hay que subir un recurso interno o externo');
        subirArribaPagina();
        return false;
    }
    else
        return true;
}

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para transformar una fecha ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function transform_date(fecha) {
    var date = fecha.replace(" ", "");
    
    var eu_date = '';
    if (date.indexOf('.') > 0)
        eu_date = date.split('.');
    else
        eu_date = date.split('/');

    /*year (optional)*/
    var year = 0;
    if (eu_date[2])
        year = eu_date[2];

    /*month*/
    var month = eu_date[1];
    if (month.length === 1)
        month = 0 + month;

    /*day*/
    var day = eu_date[0];
    if (day.length === 1)
        day = 0 + day;

    return year + "-" + month + "-" + day;
}