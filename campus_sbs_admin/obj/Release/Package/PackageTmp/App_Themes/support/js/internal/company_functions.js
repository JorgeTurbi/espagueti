// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_conv');
    start_fileupload('fileupload_logo');

    /// 2.- Cargar el textarea
    autosize($('#txt_comentarios'));
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
        dropZone: (name === "fileupload_conv" ? $('#file_convenio') : $('#logo_form')),
        maxNumberOfFiles: 1,
        done: function (e, data) {
            var row = '';
            $.each(data.result.files, function (index, file) {
                row = "<tr class='template-download'><td>";
                if (file.url)
                    row += "<a href='" + file.url + "' title='" + file.name + "' download='" + file.name + "'>" + file.name + "</a>";
                else
                    row += "<span>" + file.name + "</span>";
                if (file.error)
                    row += "<div><span class='label label-danger'>Error</span> " + file.error + "</div>";
                else {
                    if (name === "fileupload_conv")
                        $('#txtFicheroConvenio').val(file.name);
                    else
                        $('#txtLogo').val(file.name);
                }
                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl) {
                    if (name === "fileupload_conv")
                        row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','file_conv')\"><i class='fas fa-trash-alt'></i></button>";
                    else
                        row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','img_logo')\"><i class='fas fa-trash-alt'></i></button>";
                }
                else
                    row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');

            if (name === "fileupload_conv")
                $('#tbl_convenio .files').append(row);
            else
                $('#tbl_logo .files').append(row);
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
                            row += "<a href='" + file.url + "' title='" + file.name + "' download='" + file.name + "'>" + file.name + "</a>";
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

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parametros
    var razon_social = $('#txt_emp_razon').val();
    var cif = $('#txt_emp_cif').val();
    var fecha_alta = $('#txtFechaAlta').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (razon_social === "undefined" || razon_social === undefined || razon_social === "null" || razon_social === null || razon_social === '') {
        $('#razon_form').addClass(' has-error');
        $('#txt_error').html('El campo Razón Social es obligatorio');
        $('#txt_emp_razon').attr("placeholder", "El campo Razón Social es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (!validarCarateresEspeciales(razon_social)) {
        $('#razon_form').addClass(' has-error');
        $('#txt_error').html('El campo Razón Social contiene carácteres no válidos');
        return false;
    }
    else if (cif === "undefined" || cif === undefined || cif === "null" || cif === null || cif === '') {
        $('#cif_form').addClass(' has-error');
        $('#txt_error').html('El campo CIF es obligatorio');
        $('#txt_emp_cif').attr("placeholder", "El campo CIF es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (!validarCarateresEspeciales(cif)) {
        $('#cif_form').addClass(' has-error');
        $('#txt_error').html('El campo CIF contiene carácteres no válidos');
        return false;
    }
    else if (fecha_alta === "undefined" || fecha_alta === undefined || fecha_alta === "null" || fecha_alta === null || fecha_alta === '') {
        $('#fechaAlta_form').addClass(' has-error');
        $('#txt_error').html('La fecha de alta es obligatoria');
        $('#txtFechaAlta').attr("placeholder", "La fecha de alta es obligatoria");
        subirArribaPagina();
        return false;
    }
    else
        return true;
}