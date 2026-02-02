// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_anexo');

    /// 2.- Cargar el textarea
    autosize($('#txt_comentarios'));
    autosize($('#txt_pdp_comentarios'));

    /// 3.- Recuperar la tecla pulsada
    if ($("#txt_prac_alumno").val() === "") {
        $("#txt_prac_alumno").focus();
    }
    $("#txt_prac_alumno").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 4.- Autocompletar
    $("#txt_prac_alumno").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'practica-mantenimiento.aspx/search_student',
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

    /// 5.- Si el tipo es POSTGRADO poner o en Nº Pedido y Nº de factura
    $('#ddlTipo').change(function () {
        if ($(this).val() === 'POSTGRADO') {
            $('#txt_pra_factura').val(0);
            $('#txt_pra_pedido').val(0);
        }
        else
        {
            if ($('#txt_pra_factura').val() === 0)
                $('#txt_pra_factura').val('');
            if ($('#txt_pra_pedido').val() === 0)
                $('#txt_pra_pedido').val('');
        }
    });

    /// 6.- Automatizar la duración de las prácticas
    $('#txtFechaBaja').change(function () {
        var fecha_alta = $('#txtFechaAlta').val();
        var fecha_baja = $('#txtFechaBaja').val();

        if (!(fecha_alta === "undefined" || fecha_alta === undefined || fecha_alta === "null" || fecha_alta === null || fecha_alta === '' || fecha_baja === "undefined" || fecha_baja === undefined || fecha_baja === "null" || fecha_baja === null || fecha_baja === '')) {
            var date_up = new Date(transform_date(fecha_alta));
            var date_down = new Date(transform_date(fecha_baja));

            var diff = date_down - date_up;
            if (diff > 0) {
                var days = diff / (1000 * 60 * 60 * 24);
                var month = (days / 30).toFixed(2);

                $('#txt_pra_duracion').val(month);
            }
            else
                $('#txt_pra_duracion').val(0);
        }
        else
            $('#txt_pra_duracion').val(-1);
    });

    /*$('#txtFechaBaja').datepicker({ autoclose: true }).on('changeDate', function (ev) {
        var fecha_alta = $('#txtFechaAlta').val();
        var fecha_baja = $('#txtFechaBaja').val();

        if (!(fecha_alta === "undefined" || fecha_alta === undefined || fecha_alta === "null" || fecha_alta === null || fecha_alta === '' || fecha_baja === "undefined" || fecha_baja === undefined || fecha_baja === "null" || fecha_baja === null || fecha_baja === '')) {
            var date_up = new Date(transform_date(fecha_alta));
            var date_down = new Date(transform_date(fecha_baja));

            var diff = date_down - date_up;
            if (diff > 0) {
                var days = diff / (1000 * 60 * 60 * 24);
                var month = (days / 30).toFixed(2);

                $('#txt_pra_duracion').val(month);
            }
            else
                $('#txt_pra_duracion').val(0);
        }
        else
            $('#txt_pra_duracion').val(-1);
    });*/

    /// 7.- Cargar los tutores de la empresa
    cargarTutoresEmpresa();
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
                    $('#txtFicheroAnexo').val(file.name);

                    if ($('#txt_file_anexo').hasClass('hidden'))
                        $('#txt_file_anexo').removeClass('hidden');

                    $('#txt_file_anexo').attr('href', 'http://campusadmin.spainbs.com/Contents/Practicas/Practicas/' + file.name);
                }
                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl)
                    row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','file_anexo')\"><i class='fas fa-trash-alt'></i></button>";
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
                    $('#txtFicheroAnexo').val('');
                    $('#txt_file_anexo').addClass('hidden');
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
function cargarTutoresEmpresa() {

    var empresa = $('#ddlEmpresa').find('option:selected').val();
    var idTutorEmp = $('#id_Tutor_Empresa').val();

    $.ajax({
        type: "POST",
        crossDomain: true,
        url: 'practica-mantenimiento.aspx/cargarTutoresEmpresaWS',
        data: "{'idEmpresa' : '" + empresa + "'}",
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data !== null) {
                if (data.d.length > 0) {
                    $('#ddlTutorEmpresa').html('');

                    if (empresa === "-1" && (idTutorEmp === '' || idTutorEmp === '-1'))
                        $('#ddlTutorEmpresa').append("<option selected='selected' value='-1'>Seleccione</option>");
                    else {
                        $.each(data.d, function (index, tutor) {
                            if (idTutorEmp !== '' && (tutor.id_usuario === parseInt(idTutorEmp) || tutor.id_usuario === idTutorEmp))
                                $('#ddlTutorEmpresa').append("<option selected='selected' value='" + tutor.id_usuario + "'>" + tutor.nombre_completo + "</option>");
                            else
                                $('#ddlTutorEmpresa').append("<option value='" + tutor.id_usuario + "'>" + tutor.nombre_completo + "</option>");
                        });
                    }
                    $('#ddlTutorEmpresa').selectpicker('refresh');
                }
                else {
                    $('#ddlTutorEmpresa').html('');
                    $('#ddlTutorEmpresa').selectpicker('refresh');
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
    var idEmpresa = $('#ddlEmpresa').val();
    var idTutorEmpresa = $('#ddlTutorEmpresa').val();
    var idTutorEscuela = $('#ddlTutorEscuela').val();
    var fecha_alta = $('#txtFechaAlta').val();
    var duracion = $('#txt_pra_duracion').val();
    var ayuda = $('#txt_pra_ayuda').val();
    var horas = $('#txt_pra_horas').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (idAlumno === "undefined" || idAlumno === undefined || idAlumno === "null" || idAlumno === null || idAlumno === '') {
        $('#alumno_form').addClass(' has-error');
        $('#txt_prac_alumno').html('El alumno es obligatorio');
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
    else if (idEmpresa === "-1") {
        $('#empresa_form').addClass(' has-error');
        $('#txt_error').html('La empresa es obligatoria');
        subirArribaPagina();
        return false;
    }
    else if (idTutorEmpresa === "-1") {
        $('#tutor_empresa_form').addClass(' has-error');
        $('#txt_error').html('El tutor de la empresa es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (idTutorEscuela === "-1") {
        $('#tutor_escuela_form').addClass(' has-error');
        $('#txt_error').html('El tutor de la escuela es obligatorio');
        subirArribaPagina();
        return false;
    }
    else if (fecha_alta === "undefined" || fecha_alta === undefined || fecha_alta === "null" || fecha_alta === null || fecha_alta === '') {
        $('#fechaAlta_form').addClass(' has-error');
        $('#txt_error').html('La fecha de alta es obligatoria');
        $('#txtFechaAlta').attr("placeholder", "La fecha de alta es obligatoria");
        subirArribaPagina();
        return false;
    }
    else if (duracion === "undefined" || duracion === undefined || duracion === "null" || duracion === null || duracion === '' || !validarTelefono(duracion.replace(',', '.'))) {
        $('#duracion_form').addClass(' has-error');
        $('#txt_error').html('El campo Duración es un campo numérico');
        $('#txt_emp_razon').attr("placeholder", "El campo Duración es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (ayuda === "undefined" || ayuda === undefined || ayuda === "null" || ayuda === null || ayuda === '' || !validarTelefono(ayuda)) {
        $('#ayuda_form').addClass(' has-error');
        $('#txt_error').html('El campo Ayuda al estudio es un campo numérico');
        $('#txt_pra_ayuda').attr("placeholder", "El campo Ayuda al estudio es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (horas === "undefined" || horas === undefined || horas === "null" || horas === null || horas === '' || !validarTelefono(horas)) {
        $('#horas_form').addClass(' has-error');
        $('#txt_error').html('El campo Horas es un campo numérico');
        $('#txt_pra_horas').attr("placeholder", "El campo Horas es obligatorio");
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