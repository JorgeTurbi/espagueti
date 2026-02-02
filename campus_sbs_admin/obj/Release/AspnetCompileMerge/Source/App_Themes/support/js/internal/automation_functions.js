// --------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -------------------------------------------------------
// --------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_adjunto');

    $('#tabla_Adjuntos').DataTable({
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
        lengthMenu: [[10, 25, -1], [10, 25, "Todos"]],
        "columnDefs": [
          {
              "targets": [0]
          },
          {
              "targets": [1],
              "class": "text-center"
          },
          {
              "targets": [2],
              "class": "text-center"
          }
        ],
        "order": [[0, "asc"]]
    });
});

// --------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function start_fileupload(name) {
    /// 1.- Eliminar los datos de la sesión
    sessionStorage.clear();

    /// 2.- Función que sube un fichero al servidor
    $('#' + name + '').fileupload({
        dataType: 'json',
        dropZone: $('#file_adjunto'),
        maxNumberOfFiles: 1,
        done: function (e, data) {
            var row = '';
            $.each(data.result.files, function (index, file) {
                row = "<tr class='template-download'><td>";
                if (file.url)
                    row += "<a href='" + file.url + "' title='" + file.name + "' download='" + file.name + "' target='_blank'>" + file.name + "</a>";
                else
                    row += "<span>" + file.name + "</span>";
                if (file.error)
                    row += "<div><span class='label label-danger'>Error</span> " + file.error + "</div>";

                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl)
                    row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','auto')\"><i class='fas fa-trash-alt'></i></a>";
                else
                    row += "<a href=\"javascript: void(0)\" class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></a>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');
            $('#tbl_adjuntos').append(row);
            $('#modal-close').click();
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
    if (_file !== null && _file !== undefined && _file.length > 0) {
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
                            row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','" + type + "')\"><i class='fas fa-trash-alt'></i></a>";
                        else
                            row += "<a href=\"javascript: void(0)\" class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></a>";
                        row += "</td></tr>";
                    });
                    save_file(data, true);

                    $('#tbl_adjuntos').html('');
                    $('#tbl_adjuntos').append(row);
                }
                else {
                    deleteFileData();
                    $('#tbl_adjuntos').html('');
                    $('#tbl_adjuntos').append(row);
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
function getNames(files) {
    var names = '';
    var _index = 0;
    $.each(files, function (index, _file) {
        if (_index === 0)
            names = _file.name;
        else
            names += "," + _file.name;
        _index++;
    });
    return names;
}
function getfilesize(files) {
    var size = 0;
    $.each(files, function (index, _file) {
        size = size + _file.size;
    });
    return size;
}

// --------------------------------------------------------------------------------------------------
// Funciones para validar el formulario -------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function validarFormularioForo() {
    /// 0.- Deshabilitar el botón
    $('#btn_save').attr('disabled', 'disabled');

    /// 1.- Sacar los parametros
    var Titulo = $('#txt_asunto').val();
    var Descripcion = CKEDITOR.instances['txt_cuerpo'].getData();
    var Num_dias = $('#txt_dias_foro').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (Titulo === "undefined" || Titulo === undefined || Titulo === "null" || Titulo === null || Titulo === '') {
        $('#txt_error').html('El campo Título del foro es obligatorio');
        $('#txt_asunto').attr("placeholder", "El campo Título del foro es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (Descripcion === "undefined" || Descripcion === undefined || Descripcion === "null" || Descripcion === null || Descripcion === '') {
        $('#txt_error').html('El campo Descripción del foro es obligatorio');
        $('#txt_cuerpo').attr("placeholder", "El campo Descripción del foro es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (Num_dias === "undefined" || Num_dias === undefined || Num_dias === "null" || Num_dias === null || Num_dias === '') {
        $('#txt_error').html('El nº de días es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (!validarTelefono(Num_dias)) {
        $('#txt_error').html('El nº de días es un campo númerico');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardar').click();
        return true;
    }
}
function validarFormularioMsgForo() {
    /// 0.- Deshabilitar el botón
    $('#btn_save').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var Titulo = $('#txt_asunto').val();
    var Num_dias = $('#txt_dias_foro').val();
    var idforo = $('#ddlForo').val();
    var Descripcion = CKEDITOR.instances['txt_cuerpo'].getData();
    var _files = getfile('');
    $('#hidAdjuntos').val(getNames(_files));

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (Titulo === "undefined" || Titulo === undefined || Titulo === "null" || Titulo === null || Titulo === '') {
        $('#txt_error').html('El campo Título del foro es obligatorio');
        $('#txt_asunto').attr("placeholder", "El campo Título del foro es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (Descripcion === "undefined" || Descripcion === undefined || Descripcion === "null" || Descripcion === null || Descripcion === '') {
        $('#txt_error').html('El campo Descripción del foro es obligatorio');
        $('#txt_cuerpo').attr("placeholder", "El campo Descripción del foro es obligatorio");
        subirArribaPagina();
        return false;
    }            
    else if (Num_dias === "undefined" || Num_dias === undefined || Num_dias === "null" || Num_dias === null || Num_dias === '') {
        $('#txt_error').html('El nº de días es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (!validarTelefono(Num_dias)) {
        $('#txt_error').html('El nº de días es un campo númerico');
        subirArribaPagina();
        return false;
    }
    else if (idforo === "-1") {
        $('#txt_error').html('El Foro es obligatorio');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardar').click();
        return true;
    }
}
function validarFormularioMsg() {
    /// 0.- Deshabilitar el botón
    $('#btn_save').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var Titulo = $('#txt_asunto').val();
    var Num_dias = $('#txt_dias_foro').val();
    var Descripcion = CKEDITOR.instances['txt_cuerpo'].getData();
    var _files = getfile('');
    var file_size = getfilesize(_files);
    $('#hidAdjuntos').val(getNames(_files));

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (Titulo === "undefined" || Titulo === undefined || Titulo === "null" || Titulo === null || Titulo === '') {
        $('#txt_error').html('El campo Título del foro es obligatorio');
        $('#txt_asunto').attr("placeholder", "El campo Título del foro es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (Descripcion === "undefined" || Descripcion === undefined || Descripcion === "null" || Descripcion === null || Descripcion === '') {
        $('#txt_error').html('El campo Descripción del foro es obligatorio');
        $('#txt_cuerpo').attr("placeholder", "El campo Descripción del foro es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (Num_dias === "undefined" || Num_dias === undefined || Num_dias === "null" || Num_dias === null || Num_dias === '') {
        $('#txt_error').html('El nº de días es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (Num_dias !== 'N' && !validarTelefono(Num_dias)) {
        $('#txt_error').html('El nº de días es un campo númerico');
        subirArribaPagina();
        return false;
    }
    else if (file_size >= 15000000) {
        $('#txt_error').html('Todos los adjuntos no pueden superar los 15MB(megas)');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardar').click();
        return true;
    }
}
function validarFormularioCP() {
    /// 0.- Deshabilitar el botón
    $('#btn_save').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    //var Fecha = $('#date_limit').val();
    var Num_lim_dias = $('#txt_dias_lim_cp').val();
    var Num_dias = $('#txt_dias_cp').val();
    var Descripcion = CKEDITOR.instances['txt_cuerpo'].getData();
    var _files = getfile('');
    $('#hidAdjuntos').val(getNames(_files));

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    /*if (Fecha === "undefined" || Fecha === undefined || Fecha === "null" || Fecha === null || Fecha === '') {
        $('#txt_error').html('El campo Título del foro es obligatorio');
        $('#date_limit').attr("placeholder", "El campo Título del foro es obligatorio");
        subirArribaPagina();
        return false;
    }*/

    if (Num_lim_dias === "undefined" || Num_lim_dias === undefined || Num_lim_dias === "null" || Num_lim_dias === null || Num_lim_dias === '') {
        $('#txt_error').html('El nº de días de límite es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (Num_lim_dias !== 'N' && !validarTelefono(Num_lim_dias)) {
        $('#txt_error').html('El nº de días de límite es un campo númerico');
        subirArribaPagina();
        return false;
    }    
    else if (Descripcion === "undefined" || Descripcion === undefined || Descripcion === "null" || Descripcion === null || Descripcion === '') {
        $('#txt_error').html('El campo Descripción del foro es obligatorio');
        $('#txt_cuerpo').attr("placeholder", "El campo Descripción del foro es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (Num_dias === "undefined" || Num_dias === undefined || Num_dias === "null" || Num_dias === null || Num_dias === '') {
        $('#txt_error').html('El nº de días es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (!validarTelefono(Num_dias)) {
        $('#txt_error').html('El nº de días es un campo númerico');
        subirArribaPagina();
        return false;
    }
    else if (_files === "undefined" || _files === undefined || _files === "null" || _files === null || _files === '') {
        $('#txt_error').html('Hay que subir un caso práctico');
        subirArribaPagina();
        return false;
    }
    else if (_files.length > 1) {
        $('#txt_error').html('Sólo hay que subir un caso práctico');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardar').click();
        return true;
    }
}

// --------------------------------------------------------------------------------------------------
// Funcion para eliminar adjunto --------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function eliminarAdjunto(route) {
    $('#hidAdjuntos').val(route);
    $('#btnEliminar').click();
}