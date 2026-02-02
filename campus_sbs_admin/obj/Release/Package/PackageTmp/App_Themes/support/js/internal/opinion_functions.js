// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_foto');

    /// 2.- Cargar el textarea
    autosize($('#txt_comentarios'));
    autosize($('#txt_huella'));

    /// 3.- Recuperar la tecla pulsada
    if ($("#txt_alumno").val() === "") {
        $("#txt_alumno").focus();
    }
    $("#txt_alumno").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 4.- Autocompletar
    $("#txt_alumno").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'opinion-mantenimiento.aspx/search_student',
                data: "{ 'name': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.nombre_completo,
                            val: item.id_usuario
                        };
                    }));
                },
                error: function (response) {
                    alert("La consulta es poco restrictiva, escriba más caracteres o restringe más su búsqueda");
                },
                failure: function (response) {
                    alert("La consulta es poco restrictiva, escriba más caracteres o restringe más su búsqueda");
                }
            });
        },
        select: function (e, ui) {
            $('#idAlumno').val(ui.item.val);
        },
        minLength: 3
    });

    /// 5.- Cargar docencias
    //cargarDocencias();
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
                    $('#txtFoto').val(file.name);

                    if ($('#txt_img_foto').hasClass('hidden'))
                        $('#txt_img_foto').removeClass('hidden');

                    $('#txt_img_foto').attr('href', 'https://media.spainbs.com/recursos_www/img_alumnos/temp/' + file.name);
                }
                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl)
                    row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','img_opinion')\"><i class='fas fa-trash-alt'></i></button>";
                else
                    row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');
            $('#tbl_anexo .files').append(row);
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
                    $('#txtFoto').val('');
                    $('#txt_img_foto').addClass('hidden');
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
// Funciones para cargar los tutores de la empresa -----------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function cargarDocencias() {

    var curso = $('#ddlCurso').find('option:selected').val();
    var idDocencia = $('#id_Docencia').val();

    $.ajax({
        type: "POST",
        crossDomain: true,
        url: 'opinion-mantenimiento.aspx/cargarDocenciasWS',
        data: "{'idCurso' : '" + curso + "'}",
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data !== null) {
                if (data.d.length > 0) {
                    $('#ddlDocencia').html('');

                    if (curso === "-1" && (idDocencia === '' || idDocencia === '-1'))
                        $('#ddlDocencia').append("<option selected='selected' value='-1'>Seleccione</option>");
                    else {
                        $.each(data.d, function (index, docencia) {
                            if (idDocencia !== '' && docencia.id_docencia === idDocencia)
                                $('#ddlDocencia').append("<option selected='selected' value='" + docencia.id_docencia + "'>" + docencia.nombre + "</option>");
                            else
                                $('#ddlDocencia').append("<option value='" + docencia.id_docencia + "'>" + docencia.nombre + "</option>");
                        });
                    }
                    $('#ddlDocencia').selectpicker('refresh');
                }
                else {
                    $('#ddlDocencia').html('');
                    $('#ddlDocencia').selectpicker('refresh');
                }
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

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parametros
    var idAlumno = $('#idAlumno').val();
    var idCurso = $('#ddlCurso').val();
    //var idDocencia = $('#ddlDocencia').val();
    var comentarios = $('#txt_comentarios').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (idAlumno === "undefined" || idAlumno === undefined || idAlumno === "null" || idAlumno === null || idAlumno === '') {
        $('#alumno_form').addClass(' has-error');
        $('#txt_alumno').html('El alumno es obligatorio');
        $('#txt_error').html('El alumno es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (idCurso === "-1") {
        $('#curso_form').addClass(' has-error');
        $('#txt_error').html('El curso es obligatorio');
        subirArribaPagina();
        return false;
    }
    /*else if (idDocencia === "-1") {
        $('#docencia_form').addClass(' has-error');
        $('#txt_error').html('La docencia es obligatoria');
        subirArribaPagina();
        return false;
    }*/
    else if (comentarios === "undefined" || comentarios === undefined || comentarios === "null" || comentarios === null || comentarios === '') {
        $('#comentarios_form').addClass(' has-error');
        $('#txt_error').html('El campo comentarios es obligatorio');
        $('#txt_comentarios').attr("placeholder", "El campo comentarios es obligatorio");
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