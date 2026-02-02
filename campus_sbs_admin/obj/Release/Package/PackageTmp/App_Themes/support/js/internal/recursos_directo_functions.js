// ------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -----------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_foto');

    /// 2.- Cargar el textarea
    autosize($('#txt_comentarios'));
    autosize($('#txt_descripcion'));

    /// 3.- Recuperar la tecla pulsada
    if ($("#txt_profesor").val() === "") {
        $("#txt_profesor").focus();
    }
    $("#txt_profesor").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 4.- Autocompletar
    $("#txt_profesor").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'recurso-directo.aspx/search_teacher',
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
            $('#idProfesor').val(ui.item.val);
        },
        minLength: 2
    });

    /// 5.- Cargar la tabla de valoraciones
    $('#tabla_Valoraciones').DataTable({
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
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [1]
          },
          {
              "targets": [2],
              "class": "text-center"
          },
          {
              "targets": [3],
              "class": "text-center"
          },
          {
              "targets": [4]
          }
        ],
        "order": [[0, "desc"]]
    });

    /// 6.- Cargar las docencias de ese recurso
    $('#tabla_doc_all').DataTable({
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
        lengthMenu: [[10, 20, -1], [10, 20, "All"]],
        "columnDefs": [
          {
              "targets": [0],
              "class": "text-center"
          },
          {
              "targets": [1]
          }
        ],
        "order": [[1, "asc"]]
    });

    $('#table_doc').DataTable({
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
        lengthMenu: [[10, 20, -1], [10, 20, "All"]],
        "columnDefs": [
          {
              "targets": [0],
              "class": "text-center"
          },
          {
              "targets": [1]
          }
        ],
        "order": [[1, "asc"]]
    });
});

// ------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -----------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
function start_fileupload(name) {
    /// 1.- Eliminar los datos de la sesión
    sessionStorage.clear();

    /// 2.- Función que sube un fichero al servidor
    $('#' + name + '').fileupload({
        dataType: 'json',
        dropZone: $('.fileinput-button'),
        maxNumberOfFiles: 1,
        done: function (e, data) {
            var file_photo = '';

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
                    file_photo = file.name;                    
                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl)
                    row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','img_foto')\"><i class='fas fa-trash-alt'></i></button>";
                else
                    row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');
            
            //$('#tbl_foto .files').append(row);

            if (file_photo !== '') {
                $('#modal-close').click();
                $('#txt_foto').val(file_photo);
                if ($('#block_delete_photo').hasClass("hidden"))
                    $('#block_delete_photo').removeClass("hidden");
                if (!$('#block_upload_photo').hasClass("hidden"))
                    $('#block_upload_photo').addClass(" hidden");
                if ($('#block_see').hasClass("hidden"))
                    $('#block_see').removeClass("hidden");

                var link = "https://media.spainbs.com/academico/recursos_directo/temp/" + file_photo;
                $('#lnk_photo').attr('href', link);
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
function delete_foto() {
    /// 1.- Sacar los parámetros
    var id_rec_directo = getParams('idrd');
    var foto = $('#txt_foto').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Eliminar la foto
    var urlWS = "recurso-directo.aspx/delete_photo";
    var data = "{'id_rec_directo' : '" + id_rec_directo + "', 'photo' : '" + foto + "'}";
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
                $('#txt_foto').val('');

                if (!$('#block_delete_photo').hasClass("hidden"))
                    $('#block_delete_photo').addClass(" hidden");
                if ($('#block_upload_photo').hasClass("hidden"))
                    $('#block_upload_photo').removeClass("hidden");
                if (!$('#block_see').hasClass("hidden"))
                    $('#block_see').addClass("hidden");

                $('#lnk_photo').attr('href', '');
                return true;
            }
            else {
                alert('Se ha producido un error al eliminar la foto del recurso');
                return false;
            }
        },
        error: function (response) {
            alert('Se ha producido un error al eliminar la foto del recurso');
            return false;
        },
        failure: function (response) {
            alert('Se ha producido un error al eliminar la foto del recurso');
            return false;
        }
    });
}

// ------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario -----------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    $('#btn_save').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var titulo = $('#txt_titulo').val();
    var tipo = $('#ddlTipo').val();
    var fecha = $('#txtFecha').val();
    var idProfesor = $('#idProfesor').val();
    var area = $('#ddlArea').val();
    var tematica = $('#ddlTematica').val();

    var meta_url = $('#txtMetaUrl').val();
    var idRecursoDirecto = getParams('idrd');

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Comprobar los datos
    if (titulo === "undefined" || titulo === undefined || titulo === "null" || titulo === null || titulo === '') {
        $('#titulo_form').addClass(' has-error');
        $('#txt_error').html('El campo Título es obligatorio');
        $('#txt_nombre').attr("placeholder", "El campo Título es obligatorio");
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (tipo === "undefined" || tipo === undefined || tipo === "null" || tipo === null || tipo === '') {
        $('#tipo_form').addClass(' has-error');
        $('#txt_error').html('El campo Tipo es obligatorio');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (fecha === "undefined" || fecha === undefined || fecha === "null" || fecha === null || fecha === '') {
        $('#fecha_form').addClass(' has-error');
        $('#txt_error').html('La fecha es obligatoria');
        $('#txtFecha').attr("placeholder", "La fecha es obligatoria");
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (idProfesor === "undefined" || idProfesor === undefined || idProfesor === "null" || idProfesor === null || idProfesor === '') {
        $('#profesor_form').addClass(' has-error');
        $('#txt_profesor').html('El profesor es obligatorio');
        $('#txt_error').html('El profesor es obligatorio');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (area === "-1") {
        $('#area_form').addClass(' has-error');
        $('#txt_error').html('El área es obligatorio');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (tematica === "-1") {
        $('#tematica_form').addClass(' has-error');
        $('#txt_error').html('La temática es obligatorio');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (!(meta_url === "undefined" || meta_url === undefined || meta_url === "null" || meta_url === null || meta_url === '')) {
        /// Comprobar el meta_url
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: 'recurso-directo.aspx/comprobar_meta_url',
            data: "{'meta_url' : '" + meta_url + "','idRecursoDirecto' : '" + idRecursoDirecto + "'}",
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                var validate = data.d;
                if (!validate) {
                    $('#btnGuardar').click();
                    return true;
                }
                else {
                    $('#metaUrl_form').addClass(' has-error');
                    $('#txt_error').html('El Meta Url introducido ya existe');
                    $('#btn_save').removeAttr('disabled');
                    subirArribaPagina();
                    return false;
                }
            },
            error: function (response) {
                alert("Se ha producido un error al actualizar los datos");
                $('#btn_save').removeAttr('disabled');
                return false;
            },
            failure: function (response) {
                alert("Se ha producido un error al actualizar los datos");
                $('#btn_save').removeAttr('disabled');
                return false;
            }
        });
    }
    else {
        $('#btnGuardar').click();
        return true;
    }
}

// ------------------------------------------------------------------------------------------------------------
// Funciones para transformar una fecha -----------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
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

// ------------------------------------------------------------------------------------------------------------
// Funciones para tags ----------------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
function add_tag() {
    /// 0.- Deshabilitar el botón
    $('#lnk_add_tag').attr('disabled', 'disabled');
    $('#wait_modal').modal('show');

    /// 1.- Sacar los parámetros
    var tag = $('#tag_user').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#tag_user').hasClass("is-invalid"))
        $('#tag_user').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (tag === "undefined" || tag === undefined || tag === "null" || tag === null || tag === '') {
        $('#wait_modal').modal('hide');
        $('#tag_user').addClass(' is-invalid');
        $('#txt_error').html('El campo Tag es obligatorio');
        $('#lnk_add_tag').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#lnk_add_tag').removeAttr('disabled');
        var idRecursoDirecto = getParams('idrd');

        /// 3.1.- Guardar el tag
        var urlWS = "recurso-directo.aspx/add_tag_user";
        var data = "{'idRecursoDirecto' : '" + idRecursoDirecto + "', 'tag' : '" + tag + "'}";
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: urlWS,
            data: data,
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                $('#wait_modal').modal('hide');

                var _tags = data.d;
                if (_tags.length > 0) {
                    $('#blk_tags').html('');
                    $('#blk_tags').append(_tags);
                    return true;
                }
                else {
                    alert('Se ha producido un error al guardar el tag');
                    return false;
                }
            },
            error: function (response) {
                $('#wait_modal').modal('hide');
                alert('Se ha producido un error al guardar el tag');
                return false;
            },
            failure: function (response) {
                $('#wait_modal').modal('hide');
                alert('Se ha producido un error al guardar el tag');
                return false;
            }
        });
    }
}
function eliminar_tag(id) {
    $("#hid_tag").val(id);
    $('#btn_delete_tag').click();
}

/// -----------------------------------------------------------------------------------------------------------
// Funciones para guardar en la misma página ------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
function save_data() {
    $('#btn_save_all').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var titulo = $('#txt_titulo').val();
    var tipo = $('#ddlTipo').val();
    var fecha = $('#txtFecha').val();
    var idProfesor = $('#idProfesor').val();
    var area = $('#ddlArea').val();
    var tematica = $('#ddlTematica').val();

    var meta_url = $('#txtMetaUrl').val();
    var idRecursoDirecto = getParams('idrd');

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Comprobar los datos
    if (titulo === "undefined" || titulo === undefined || titulo === "null" || titulo === null || titulo === '') {
        $('#titulo_form').addClass(' has-error');
        $('#txt_error').html('El campo Título es obligatorio');
        $('#txt_nombre').attr("placeholder", "El campo Título es obligatorio");
        $('#btn_save_all').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (tipo === "undefined" || tipo === undefined || tipo === "null" || tipo === null || tipo === '') {
        $('#tipo_form').addClass(' has-error');
        $('#txt_error').html('El campo Tipo es obligatorio');
        $('#btn_save_all').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (fecha === "undefined" || fecha === undefined || fecha === "null" || fecha === null || fecha === '') {
        $('#fecha_form').addClass(' has-error');
        $('#txt_error').html('La fecha es obligatoria');
        $('#txtFecha').attr("placeholder", "La fecha es obligatoria");
        $('#btn_save_all').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (idProfesor === "undefined" || idProfesor === undefined || idProfesor === "null" || idProfesor === null || idProfesor === '') {
        $('#profesor_form').addClass(' has-error');
        $('#txt_profesor').html('El profesor es obligatorio');
        $('#txt_error').html('El profesor es obligatorio');
        $('#btn_save_all').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (area === "-1") {
        $('#area_form').addClass(' has-error');
        $('#txt_error').html('El área es obligatorio');
        $('#btn_save_all').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (tematica === "-1") {
        $('#tematica_form').addClass(' has-error');
        $('#txt_error').html('La temática es obligatorio');
        $('#btn_save_all').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (!(meta_url === "undefined" || meta_url === undefined || meta_url === "null" || meta_url === null || meta_url === '')) {
        /// Comprobar el meta_url
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: 'recurso-directo.aspx/comprobar_meta_url',
            data: "{'meta_url' : '" + meta_url + "','idRecursoDirecto' : '" + idRecursoDirecto + "'}",
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                var validate = data.d;
                if (!validate) {
                    $('#btnGuardarAll').click();
                    return true;
                }
                else {
                    $('#metaUrl_form').addClass(' has-error');
                    $('#txt_error').html('El Meta Url introducido ya existe');
                    $('#btn_save_all').removeAttr('disabled');
                    subirArribaPagina();
                    return false;
                }
            },
            error: function (response) {
                alert("Se ha producido un error al actualizar los datos");
                $('#btn_save_all').removeAttr('disabled');
                return false;
            },
            failure: function (response) {
                alert("Se ha producido un error al actualizar los datos");
                $('#btn_save_all').removeAttr('disabled');
                return false;
            }
        });
    }
    else {
        $('#btnGuardarAll').click();
        return true;
    }
}

/// -----------------------------------------------------------------------------------------------------------
// Funciones para generar los metadatos -----------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
function copiarMetadatos() {
    $('#txtMetaTitle').val($('#txt_titulo').val().trim());
    $('#txtMetaUrl').val(RemoveAccents($('#txt_titulo').val().trim()).toLowerCase());
    $('#txtMetaDescripcion').val($('#txt_descripcion').val().trim());
    $('#txtMetaAuthor').val($("#txt_profesor").val());
    /// Recuperar los tags
    var idRecursoDirecto = getParams('idrd');
    var urlWS = "recurso-directo.aspx/search_tag";
    var data = "{'idRecursoDirecto' : '" + idRecursoDirecto + "'}";
    $.ajax({
        type: "POST",
        crossDomain: true,
        url: urlWS,
        data: data,
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            var _tags = data.d;
            if (_tags.length > 0) {
                $('#txtMetaKeywords').val(_tags);
                return true;
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

function RemoveCaracteres(str) {
    str = str.replace(',', '');
    str = str.replace('.', '');
    str = str.replace('?', '');
    str = str.replace('¿', '');
    str = str.replace('!', '');
    str = str.replace('¡', '');
    str = str.replace('_', '');
    str = str.replace('"', '');
    str = str.replace(';', '');
    str = str.replace('&', '');
    str = str.replace('@', '');
    str = str.replace('(', '');
    str = str.replace(')', '');
    str = str.replace('[', '');
    str = str.replace(']', '');
    str = str.replace('{', '');
    str = str.replace('}', '');
    str = str.replace('*', '');
    str = str.replace('<', '');
    str = str.replace('>', '');
    str = str.replace(':', '');
    str = str.replace('´', '');
    str = str.replace('     ', '');
    str = str.replace('    ', '');
    str = str.replace('   ', '');
    str = str.replace('  ', '');
    str = str.replace('     ', '');
    str = str.replace('    ', '');
    str = str.replace('   ', '');
    str = str.replace('  ', '');
    str = str.replace(' ', '-');

    return str;
}
function RemoveAccents(str) {
    var accents = 'ÀÁÂÃÄÅàáâãäåÒÓÔÕÕÖØòóôõöøÈÉÊËèéêëðÇçÐÌÍÎÏìíîïÙÚÛÜùúûüÑñŠšŸÿýŽž';
    var accentsOut = "AAAAAAaaaaaaOOOOOOOooooooEEEEeeeeeCcDIIIIiiiiUUUUuuuuNnSsYyyZz";
    str = str.split('');
    var strLen = str.length;
    var i, x;
    for (i = 0; i < strLen; i++) {
        if ((x = accents.indexOf(str[i])) != -1) {
            str[i] = accentsOut[x];
        }
        str[i] = RemoveCaracteres(str[i]);
    }
    var texto = str.join('');
    texto = texto.replace('-----', '-');
    texto = texto.replace('----', '-');
    texto = texto.replace('---', '-');
    texto = texto.replace('--', '-');
    return texto;
}

// --------------------------------------------------------------------------------------------------------------------------------
// Funciones para marcar y desmarcar checks ---------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------------------------------------
function chk_mark_doc(control, campo) {
    var cadena = '';
    if (campo === 1)
        cadena = $('#hidDocs').val();
    else
        cadena = $('#hidDocsSel').val();
    var checked = control.checked;
    var valor = control.value + ",";

    if (!checked) {
        if (cadena !== "") {
            cadena = cadena.replace(valor, "");
        }
    }
    else {
        cadena = cadena + valor;
    }

    if (campo === 1)
        $('#hidDocs').val(cadena);
    else
        $('#hidDocsSel').val(cadena);
}

// --------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar los checks ----------------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------------------------------------
function validarChkDocAll() {
    /// 1.- Sacar los parametros
    var status = $('#hidDocs').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (status === "undefined" || status === undefined || status === "null" || status === null || status === '') {
        alert('Hay que seleccionar al menos una docencia en la tabla docencias.');
        return false;
    }
    else
        return true;
}
function validarChkDocSel() {
    /// 1.- Sacar los parametros
    var status = $('#hidDocsSel').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (status === "undefined" || status === undefined || status === "null" || status === null || status === '') {
        alert('Hay que seleccionar al menos una docencia en la tabla docencias seleccionadas.');
        return false;
    }
    else
        return true;
}
