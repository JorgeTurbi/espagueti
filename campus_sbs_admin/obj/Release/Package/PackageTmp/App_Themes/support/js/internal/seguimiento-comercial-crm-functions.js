// --------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -------------------------------------------------------
// --------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_adjunto');

    /// 2.- Cargar los datepicker
    $(".input-group.date").datepicker({
        language: "es",
        autoclose: true,
        todayHighlight: true
    });

    /// 3.- Mostrar u ocultar el bloque de mail
    $('input[name="action_method"]').change(function () {
        if ($(this).val() === '1') {
            $('#blk_mail').removeClass('hidden');
        }
        else {
            if (!$("#blk_mail").hasClass("hidden"))
                $('#blk_mail').addClass(' hidden');
        }
    });
    $('input[name="status_method"]').change(function () {
        if ($(this).val() === '1') {
            $('#blk_rejected').removeClass('hidden');
        }
        else {
            if (!$("#blk_rejected").hasClass("hidden"))
                $('#blk_rejected').addClass(' hidden');

            if ($(this).val() === '3') {
                $('#chkProgRec').prop("checked", true);

                if ($("#blk_program").hasClass("hidden"))
                    $('#blk_program').removeClass('hidden');
            }
            else {
                $('#chkProgRec').prop("checked", false);

                if (!$("#blk_program").hasClass("hidden"))
                    $('#blk_program').addClass(' hidden');
            }
        }
    });
    $('#chkProgRec').change(function () {
        if ($('#chkProgRec').is(':checked'))
            $('#blk_program').removeClass('hidden');
        else {
            if (!$("#blk_program").hasClass("hidden"))
                $('#blk_program').addClass(' hidden');
        }
    });
    $('#chkReasignarC').change(function () {
        if ($('#chkReasignarC').is(':checked'))
            $('#blk_comercial').removeClass('hidden');
        else {
            if (!$("#blk_comercial").hasClass("hidden"))
                $('#blk_comercial').addClass(' hidden');
        }
    });
    $('#ddlPlantilla').change(function () {
        var idPlantilla = $('#ddlPlantilla').val();
        search_plantilla_mail(idPlantilla);
    });

    /// 4.- Pintar la tabla de los adjuntos
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
// Funcion para buscar la plantilla de mail --------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_plantilla_mail(idPlantilla) {
    if (idPlantilla !== null && idPlantilla !== '' && idPlantilla !== undefined) {
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: 'seguimiento-comercial-crm.aspx/search_plantilla',
            data: "{'idPlantilla' : '" + idPlantilla + "'}",
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                $.each(data.d, function (index, _plantilla) {
                    $('#txt_asunto').val(_plantilla.asunto);
                    CKEDITOR.instances['txt_cuerpo'].setData(_plantilla.cuerpo);
                    if (_plantilla.adjuntos !== '') {
                        $.each(_plantilla.adjuntos.split(','), function (index, file) {
                            save_file(file, false);
                        });
                        $('#hidAdjuntos').val(_plantilla.adjuntos);
                        
                        var row = '';
                        $.each(_plantilla.adjuntos.split(','), function (index, file) {
                            row += "<tr class='template-download'><td>";
                            row += "<a href='https://media.spainbs.com/recursos_www/adjuntos_mail/adjuntos/" + _plantilla.idPlantillaMail + "/" + file.trim() + "' title='" + file + "' target='_blank'>" + file + "</a>";
                            row += "</td><td><span class='size'> - </span></td><td>";                            
                            row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file.trim() + "','', " + _plantilla.idPlantillaMail + ")\"><i class='fas fa-trash-alt'></i></a>";
                            row += "</td></tr>";
                        });
                        $('#tbl_adjuntos').append(row);
                    }
                });
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

// --------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function start_fileupload(name) {
    /// 1.- Eliminar los datos de la sesión
    sessionStorage.clear();
    var idPlantilla = $('#ddlPlantilla').val();

    /// 2.- Función que sube un fichero al servidor
    $('#' + name + '').fileupload({
        dataType: 'json',
        dropZone: $('#file_adjunto'),
        maxNumberOfFiles: 1,
        done: function (e, data) {
            save_file(data, false);

            var row = '';
            $.each(data.result.files, function (index, file) {
                row = "<tr class='template-download'><td>";
                if (file.url)
                    row += "<a href='" + file.url + "' title='" + file.name + "' download='" + file.name + "' target='_blank'>" + file.name + "</a>";
                else
                    row += "<a href='https://media.spainbs.com/recursos_www/adjuntos_mail/adjuntos/" + idPlantilla + "/" + file + "' title='" + file + "' target='_blank'>" + file + "</a>";
                if (file.error)
                    row += "<div><span class='label label-danger'>Error</span> " + file.error + "</div>";

                if(file.size)
                    row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                else
                    row += "</td><td><span class='size'> - </span></td><td>";

                if (file.deleteUrl)
                    row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','file_mail', '" + idPlantilla + "')\"><i class='fas fa-trash-alt'></i></a>";
                else
                    row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file + "','', " + idPlantilla + ")\"><i class='fas fa-trash-alt'></i></a>";
                row += "</td></tr>";
            });
            
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
function delete_file(name, type, idPlantilla) {
    if (type !== '') {
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
                    var _files_mail = getfile('');
                    var _files = getNames(_files_mail);

                    var _files_filter = [];
                    $.each(_files.split(','), function (index, _file) {
                        if (_file !== null && _file !== undefined && _file !== name)
                            _files_filter.push(_file);
                    });

                    var row = '';
                    var _files_process = [];
                    if (_files_filter.length > 0) {
                        $.each(_files_mail, function (index, file) {
                            $.each(_files_filter, function (index, file_mail) {
                                if (file.name === file_mail || file === file_mail) {
                                    row += "<tr class='template-download'><td>";
                                    if (file.url)
                                        row += "<a href='" + file.url + "' title='" + file.name + "' target='_blank'>" + file.name + "</a>";
                                    else
                                        row += "<a href='https://media.spainbs.com/recursos_www/adjuntos_mail/adjuntos/" + idPlantilla + "/" + file + "' title='" + file + "' target='_blank'>" + file + "</a>";
                                    if (file.error)
                                        row += "<div><span class='label label-danger'>Error</span> " + file.error + "</div>";

                                    if (file.size)
                                        row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                                    else
                                        row += "</td><td><span class='size'> - </span></td><td>";
                                    if (file.deleteUrl)
                                        row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','file_mail', '" + idPlantilla + "')\"><i class='fas fa-trash-alt'></i></a>";
                                    else
                                        row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file + "','', " + idPlantilla + ")\"><i class='fas fa-trash-alt'></i></a>";
                                    row += "</td></tr>";

                                    _files_process.push(file);
                                }
                            });
                        });

                        save_file(_files_process, true);
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
    else {
        var _files_mail = getfile('');
        var _files = getNames(_files_mail);

        var _files_filter = [];
        $.each(_files.split(','), function (index, _file) {
            if (_file !== null && _file !== undefined && _file !== name)
                _files_filter.push(_file);
        });

        var row = '';
        var _files_process = [];
        if (_files_filter.length > 0) {
            $.each(_files_mail, function (index, file) {
                $.each(_files_filter, function (index, file_mail) {
                    if (file.name === file_mail || file === file_mail) {
                        row += "<tr class='template-download'><td>";
                        if (file.url)
                            row += "<a href='" + file.url + "' title='" + file.name + "' target='_blank'>" + file.name + "</a>";
                        else
                            row += "<a href='https://media.spainbs.com/recursos_www/adjuntos_mail/adjuntos/" + idPlantilla + "/" + file + "' title='" + file + "' target='_blank'>" + file + "</a>";
                        if (file.error)
                            row += "<div><span class='label label-danger'>Error</span> " + file.error + "</div>";

                        if (file.size)
                            row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                        else
                            row += "</td><td><span class='size'> - </span></td><td>";
                        if (file.deleteUrl)
                            row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','file_mail', '" + idPlantilla + "')\"><i class='fas fa-trash-alt'></i></a>";
                        else
                            row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"delete_file('" + file + "','', " + idPlantilla + ")\"><i class='fas fa-trash-alt'></i></a>";
                        row += "</td></tr>";

                        _files_process.push(file);
                    }
                });
            });

            save_file(_files_process, true);
            $('#tbl_adjuntos').html('');
            $('#tbl_adjuntos').append(row);
        }
        else {
            deleteFileData();
            $('#tbl_adjuntos').html('');
            $('#tbl_adjuntos').append(row);
        }
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
        else if (file !== null && file !== undefined)
            list_files.push(file);
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
        else if (file !== null && file !== undefined) {
            $.each(file, function (index, _file) {
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
        if (_index === 0) {
            if (_file.name !== null && _file.name !== undefined)
                names = _file.name;
            else
                names = _file;
        }
        else {
            if (_file.name !== null && _file.name !== undefined)
                names += "," + _file.name;
            else
                names += "," + _file;
        }
        _index++;
    });
    return names;
}
function getfilesize(files) {
    var size = 0;
    $.each(files, function (index, _file) {
        if (_file.size)
            size = size + _file.size;
    });
    return size;
}

/*

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
function eliminar_fichero(name, idPlantillaMail) {
    var _files_mail = getfile('');
    var _files = getNames(_files_mail);

    var _files_filter = [];
    $.each(_files.split(','), function (index, _file) {
        if (_file !== null && _file !== undefined && _file !== name)
            _files_filter.push(_file);
    });

    var row = '';
    if (_files_filter.length > 0) {
        $.each(_files_filter, function (index, file) {
            row = "<tr class='template-download'><td>";
            row += "<a href='https://media.spainbs.com/recursos_www/adjuntos_mail/adjuntos/" + idPlantillaMail + "/" + file + "' title='" + file + "' target='_blank'>" + file + "</a>";
            row += "</td><td><span class='size'> - </span></td><td>";
            row += "<a href=\"javascript: void(0)\" class='btn btn-danger delete' onclick=\"eliminar_fichero('" + file + "', " + idPlantillaMail + ")\"><i class='fas fa-trash-alt'></i></a>";
            row += "</td></tr>";
        });

        save_file(_files_filter, true);
        $('#tbl_adjuntos').html('');
        $('#tbl_adjuntos').append(row);        
    }
    else {
        deleteFileData();
        $('#tbl_adjuntos').html('');
        $('#tbl_adjuntos').append(row);
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
        else if (file !== null && file !== undefined)
            list_files.push(file);
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
        else if (file !== null && file !== undefined) {
            $.each(file, function (index, _file) {
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
        if (_index === 0) {
            if (_file.name !== null && _file.name !== undefined)
                names = _file.name;
            else
                names = _file;
        }
        else {
            if (_file.name !== null && _file.name !== undefined)
                names += "," + _file.name;
            else
                names += "," + _file;
        }
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
*/

// --------------------------------------------------------------------------------------------------
// Funciones para validar el formulario -------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 0.- Deshabilitar el botón
    $('#btn_save').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var fecha = $('#txtFechaSeg').val();
    var accion_comercial = $('input:radio[name="action_method"]:checked').val();
    var idSeg = getParams('ids');
    var estado = $('input:radio[name="status_method"]:checked').val();
    var program_rec = $('#chkProgRec').is(':checked');
    var fecha_rec = $('#txtDateProg').val();
    var hora_rec = $('#ddlHour').val();
    var reasignar_comercial = $('#chkReasignarC').is(':checked');
    var comercial = $('#ddlComerciales').val();
    var mandar_mail = $('#chkEnviarMail').is(':checked');
    var asunto_mail = $('#txt_asunto').val();
    var cuerpo_mail = CKEDITOR.instances['txt_cuerpo'].getData();
    var motivo = $('input:radio[name="motive_method"]:checked').val();
    var _files = getfile('');
    var file_size = getfilesize(_files);
    $('#hidAdjuntos').val(getNames(_files));
    var comentarios = $('#txt_comentarios').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    limpiar_campos();

    /// 3.- Validar los datos
    if (fecha === "undefined" || fecha === undefined || fecha === "null" || fecha === null || fecha === '') {
        $('#txt_error').html('El campo Fecha es obligatorio');
        $('#txtFechaSeg').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if ((idSeg === -1 || idSeg === '') && (accion_comercial === "undefined" || accion_comercial === undefined || accion_comercial === "null" || accion_comercial === null || accion_comercial === '')) {
        $('#txt_error').html('Hay que seleccionar una acción comercial');
        $('#action_mail').addClass(' is-invalid');
        $('#action_phone').addClass(' is-invalid');
        $('#action_whatsapp').addClass(' is-invalid');
        $('#action_advise').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if ((idSeg === -1 || idSeg === '') && (estado === "undefined" || estado === undefined || estado === "null" || estado === null || estado === '')) {
        $('#txt_error').html('Hay que seleccionar un estado');
        $('#status_sin_contactar').addClass(' is-invalid');
        $('#status_indeciso').addClass(' is-invalid');
        $('#status_interesado').addClass(' is-invalid');
        $('#status_send').addClass(' is-invalid');
        $('#status_receive').addClass(' is-invalid');
        $('#status_pago').addClass(' is-invalid');
        $('#status_futuro').addClass(' is-invalid');
        $('#status_duplicado').addClass(' is-invalid');
        $('#status_rechazado').addClass(' is-invalid');
        $('#status_cerrar').addClass(' is-invalid');
        $('#status_matriculado').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (program_rec && (fecha_rec === "undefined" || fecha_rec === undefined || fecha_rec === "null" || fecha_rec === null || fecha_rec === '')) {
        $('#txt_error').html('El campo Fecha del recordatorio es obligatoria');
        $('#txtDateProg').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (program_rec && (hora_rec === "undefined" || hora_rec === undefined || hora_rec === "null" || hora_rec === null || hora_rec === '')) {
        $('#txt_error').html('El campo Hora del recordatorio es obligatoria');
        $('#hora_recordatorio').addClass(' has-error');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (reasignar_comercial && (comercial === '-1')) {
        $('#txt_error').html('Hay que seleccionar un comercial');
        $('#ddlComerciales').addClass(' has-error');
        $('#ddlComerciales').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
        /// Rechazado + Motivo
    else if (estado === '1' && (motivo === "undefined" || motivo === undefined || motivo === "null" || motivo === null || motivo === '')) {
        $('#txt_error').html('Hay que seleccionar un motivo del rechazo');
        $('#motive1').addClass(' is-invalid');
        $('#motive2').addClass(' is-invalid');
        $('#motive3').addClass(' is-invalid');
        $('#motive4').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
        /// Futuro + programar recordatorio
    else if ((idSeg === -1 || idSeg === '') && estado === '3' && !program_rec) {
        $('#txt_error').html('Hay que programar un recordatorio');
        $('#chkProgRec').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
        /// No permite explicar + comentario
    else if (motivo === '3' && (comentarios === "undefined" || comentarios === undefined || comentarios === "null" || comentarios === null || comentarios === '')) {
        $('#txt_error').html('Hay que seleccionar un comentario');
        $('#txt_comentarios').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    /*else if (accion_comercial === "1" && mandar_mail && (asunto_mail === "undefined" || asunto_mail === undefined || asunto_mail === "null" || asunto_mail === null || asunto_mail === '' || cuerpo_mail === "undefined" || cuerpo_mail === undefined || cuerpo_mail === "null" || cuerpo_mail === null || cuerpo_mail === '')) {
        $('#txt_error').html('Hay que rellenar el asunto y el cuerpo del mail');
        $('#txt_asunto').addClass(' is-invalid');
        $('#txt_cuerpo').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }*/
    else if (accion_comercial === "1" && (asunto_mail === "undefined" || asunto_mail === undefined || asunto_mail === "null" || asunto_mail === null || asunto_mail === '' || cuerpo_mail === "undefined" || cuerpo_mail === undefined || cuerpo_mail === "null" || cuerpo_mail === null || cuerpo_mail === '')) {
        $('#txt_error').html('Hay que rellenar el asunto y el cuerpo del mail');
        $('#txt_asunto').addClass(' is-invalid');
        $('#txt_cuerpo').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
        /// No programar con Rechazado o Duplicado
    else if (program_rec && (estado === '1' || estado === '2')) {
        $('#txt_error').html('No se puede programar un recordatorio con el estado Rechazado o Duplicado.');
        $('#chkProgRec').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (comentarios === "undefined" || comentarios === undefined || comentarios === "null" || comentarios === null || comentarios === '') {
        $('#txt_error').html('El campo comentarios es obligatorio');
        $('#txt_comentarios').addClass(' is-invalid');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (accion_comercial === "1" && file_size >= 15000000) {
        $('#txt_error').html('Todos los adjuntos no pueden superar los 15MB(megas)');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else {
        $('#btnGuardar').click();
        return true;
    }
}

function limpiar_campos() {
    if ($('#txtFechaSeg').hasClass("is-invalid"))
        $('#txtFechaSeg').removeClass("is-invalid");
    if ($('#action_mail').hasClass("is-invalid"))
        $('#action_mail').removeClass("is-invalid");
    if ($('#action_phone').hasClass("is-invalid"))
        $('#action_phone').removeClass("is-invalid");
    if ($('#action_whatsapp').hasClass("is-invalid"))
        $('#action_whatsapp').removeClass("is-invalid");
    if ($('#action_advise').hasClass("is-invalid"))
        $('#action_advise').removeClass("is-invalid");
    if ($('#status_sin_contactar').hasClass("is-invalid"))
        $('#status_sin_contactar').removeClass("is-invalid");
    if ($('#status_indeciso').hasClass("is-invalid"))
        $('#status_indeciso').removeClass("is-invalid");
    if ($('#status_interesado').hasClass("is-invalid"))
        $('#status_interesado').removeClass("is-invalid");
    if ($('#status_send').hasClass("is-invalid"))
        $('#status_send').removeClass("is-invalid");
    if ($('#status_receive').hasClass("is-invalid"))
        $('#status_receive').removeClass("is-invalid");
    if ($('#status_pago').hasClass("is-invalid"))
        $('#status_pago').removeClass("is-invalid");
    if ($('#status_futuro').hasClass("is-invalid"))
        $('#status_futuro').removeClass("is-invalid");
    if ($('#status_duplicado').hasClass("is-invalid"))
        $('#status_duplicado').removeClass("is-invalid");
    if ($('#status_rechazado').hasClass("is-invalid"))
        $('#status_rechazado').removeClass("is-invalid");
    if ($('#status_cerrar').hasClass("is-invalid"))
        $('#status_cerrar').removeClass("is-invalid");
    if ($('#status_matriculado').hasClass("is-invalid"))
        $('#status_matriculado').removeClass("is-invalid");
    if ($('#txtDateProg').hasClass("is-invalid"))
        $('#txtDateProg').removeClass("is-invalid");
    if ($('#hora_recordatorio').hasClass("has-error"))
        $('#hora_recordatorio').removeClass("has-error");
    if ($('#ddlComerciales').hasClass("has-error"))
        $('#ddlComerciales').removeClass("has-error");
    if ($('#motive1').hasClass("is-invalid"))
        $('#motive1').removeClass("is-invalid");
    if ($('#motive2').hasClass("is-invalid"))
        $('#motive2').removeClass("is-invalid");
    if ($('#motive3').hasClass("is-invalid"))
        $('#motive3').removeClass("is-invalid");
    if ($('#motive4').hasClass("is-invalid"))
        $('#motive4').removeClass("is-invalid");
    if ($('#chkProgRec').hasClass("is-invalid"))
        $('#chkProgRec').removeClass("is-invalid");
    if ($('#txt_comentarios').hasClass("is-invalid"))
        $('#txt_comentarios').removeClass("is-invalid");
    if ($('#txt_asunto').hasClass("is-invalid"))
        $('#txt_asunto').removeClass("is-invalid");
    if ($('#txt_cuerpo').hasClass("is-invalid"))
        $('#txt_cuerpo').removeClass("is-invalid");
}

// --------------------------------------------------------------------------------------------------
// Funcion para eliminar adjunto --------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function eliminarAdjunto(route) {
    $('#hidAdjuntos').val(route);
    $('#btnEliminar').click();
}