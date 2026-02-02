var recursosseleccionados = [];
// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_conv');
    start_fileupload('fileupload_logo');

    var id = $('#txtidcurso').val();
    
    if (!id || id == undefined || id < 1) {
        showmessage('literalesalert', 'No se ha guardado el curso, cuando guarde el curso podrá editar las literales.', 'primary');
        $('#literalesbtns').hide();
        showmessage('contenidosalert', 'No se ha guardado el curso, cuando guarde el curso podrá editar los contenidos.', 'primary');
        $('#contenidobtns').hide();
    } else {
        refreshliterales();
        refreshcontenidos();
    }
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
// Funciones para guardar ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function guardarcurso() {
    
    if (!validarFormulario())
        return;
    var autores = [];
    var autorespct = [];
    var autoreslist = $('#autorescontainers span');

    for (var i = 0; i < autoreslist.length; i++) {
        autores.push(autoreslist[i].id);
        autorespct.push(autoreslist[i].dataset.pc);
    }
    var areas = $('#areascontainer .badge-primary');
    var idsareas = [];
    for (var i = 0; i < areas.length; i++) {
        idsareas.push(areas[i].id);
    }

    var tematicas = $('#tematicascontainer .badge-primary');
    var idstematicas = [];
    for (var i = 0; i < tematicas.length; i++) {
        idstematicas.push(tematicas[i].id);
    }
    var data = "{";
    data += "'id':" + $('#txtidcurso').val() + ",";
    data += "'codigo': '" + $('#txt_codigo').val() + "',";
    data += "'nombre': '" + $('#txt_nombre').val() + "',";
    data += "'fechaAlta': '" + $('#txt_FechaAlta').val() + "',";
    data += "'fechaBaja': '" + $('#txtFechaBaja').val() + "',";
    data += "'descripcion': '" + $('#txt_descripcion').val() + "',";
    data += "'version': " + $('#version').val() + ",";
    data += "'sesiones': " + $('#sesiones').val() + ",";
    data += "'horas': " + $('#horas').val() + ",";
    data += "'duracion': " + $('#duracion').val() + ",";
    data += "'dias': " + $('#dias').val() + ",";
    data += "'titulooficial': '" + $('#titulo_oficial').val() + "',";
    data += "'contenidooficial': '" + $('#contenido_oficial').val() + "',";
    data += "'ddltipos': " + $('#ddltipos').val() + ",";
    data += "'ddlmetodologia': " + $('#ddlmetodologia').val() + ",";
    data += "'ddldificultad': " + $('#ddldificultad').val() + ",";
    data += "'autores': [" + autores + "],";
    data += "'autorespct': [" + autorespct + "],";
    data += "'chkpublicar': " + $('#chk_publicar').prop("checked") + ",";
    data += "'idsareas': [" + idsareas + "],";
    data += "'idstematicas': [" + idstematicas + "],";
    data += "'chkactivo': " + $('#chkActivo').prop("checked") + ",";
    data += "'chkuniversitario': " + $('#chk_universitario').prop("checked") + ",";
    data += "'programapdf': '" + $('#programapdfh').val() + "',";
    data += "'programapdfcompleto': '" + $('#programapdfcompletoh').val() + "',";
    data += "'programapdfweb': '" + $('#programapdfwebh').val() + "'";
    data += "}"
    console.log(data);
    //return;
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/SaveCurso',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('messagealert', "Procesando petición ...", "primary");
            subirArribaPagina();
        },
        success: function (response) {
            showmessage('messagealert', "Cambios guardados", "success");
            subirArribaPagina();
            console.log(response);
            $('#txtidcurso').val(response.d);
            console.log($('#txtidcurso').val());
            hidemessage('literalesalert');
            refreshliterales();
            $('#literalesbtns').show();
            refreshcontenidos();
            $('#contenidobtns').show();
        },
        error: function (error) {
            showmessage('messagealert', "Se ha producido un error al guardar los cambios", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}
// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parametros
    var codigo = $('#txt_codigo').val();
    var nombre = $('#txt_nombre').val();
    var fechaAlta = $('#txt_FechaAlta').val();
    var descripcion = $('#txt_descripcion').val();
    var version = $('#version').val();
    var sesiones = $('#sesiones').val();
    var horas = $('#horas').val();
    var duracion = $('#duracion').val();
    var dias = $('#dias').val();
    var titulo_oficial = $('#titulo_oficial').val();
    var contenido_oficial = $('#contenido_oficial').val();
    var ddltipos = $('#ddltipos').val();
    var ddlmetodologia = $('#ddlmetodologia').val();
    var ddldificultad = $('#ddldificultad').val();

    var areas = $('#areascontainer .badge-primary');
    var idsareas = [];
    for (var i = 0; i < areas.length; i++) {
        idsareas.push(areas[i].id);
    }

    var tematicas = $('#tematicascontainer .badge-primary');
    var idstematicas = [];
    for (var i = 0; i < tematicas.length; i++) {
        idstematicas.push(tematicas[i].id);
    }

    var autoreslist = $('#autorescontainers span');
    var autores = [];
    var duplicatedautor = false;
    for (var i = 0; i < autoreslist.length; i++) {
        for (var j = 0; j < autores.length; j++) {
            if (autoreslist[i].id == autores[j].id) {
                duplicatedautor = true;
                break;
            }
        }
        if (!duplicatedautor) {
            autores.push({
                "id": autoreslist[i].id,
                "pc": autoreslist[i].dataset.pc
            });
        }
    }

    var pc = 0;
    for (var i = 0; i < autores.length; i++) {
        pc = pc + parseInt(autores[i].pc);
    }

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    //codigo
    if (codigo === "undefined" || codigo === undefined || codigo === "null" || codigo === null || codigo === '') {
        $('#codigo_form').addClass(' has-error');
        $('#txt_error').html('El campo Código es obligatorio');
        $('#txt_codigo').attr("placeholder", "El campo Código es obligatorio");
        subirArribaPagina();
        return false;
    }
        //nombre
    else if (nombre === "undefined" || nombre === undefined || nombre === "null" || nombre === null || nombre === '') {
        $('#nombre_form').addClass(' has-error');
        $('#txt_error').html('El campo Nombre  es obligatorio');
        $('#txt_nombre').attr("placeholder", "El campo Nombre es obligatorio");
        subirArribaPagina();
        return false;
    }
        //fechaAlta
    else if (fechaAlta === "undefined" || fechaAlta === undefined || fechaAlta === "null" || fechaAlta === null || fechaAlta === '') {
        $('#fechaAlta_form').addClass(' has-error');
        $('#txt_error').html('El campo Fecha Alta es obligatorio');
        $('#txt_FechaAlta').attr("placeholder", "El campo Fecha Alta es obligatorio");
        subirArribaPagina();
        return false;
    }
        //descripcion
    else if (descripcion === "undefined" || descripcion === undefined || descripcion === "null" || descripcion === null || descripcion === '') {
        $('#descripcion_form').addClass(' has-error');
        $('#txt_error').html('El campo Descripción es obligatorio');
        $('#txt_descripcion').attr("placeholder", "El campo Descripción es obligatorio");
        subirArribaPagina();
        return false;
    }
        //version
    else if (version === "undefined" || version === undefined || version === "null" || version === null || version === '') {
        $('#version_form').addClass(' has-error');
        $('#txt_error').html('El campo Versión es obligatorio');
        $('#version').attr("placeholder", "El campo Versión es obligatorio");
        subirArribaPagina();
        return false;
    }
        //sesiones
    else if (sesiones === "undefined" || sesiones === undefined || sesiones === "null" || sesiones === null || sesiones === '') {
        $('#sesiones_form').addClass(' has-error');
        $('#txt_error').html('El campo Número Sesiones es obligatorio');
        $('#sesiones').attr("placeholder", "El campo Número Sesiones es obligatorio");
        subirArribaPagina();
        return false;
    }
        //horas
    else if (horas === "undefined" || horas === undefined || horas === "null" || horas === null || horas === '') {
        $('#horas_form').addClass(' has-error');
        $('#txt_error').html('El campo Número de horas es obligatorio');
        $('#horas').attr("placeholder", "El campo Número de horas es obligatorio");
        subirArribaPagina();
        return false;
    }
        //duracion
    else if (duracion === "undefined" || duracion === undefined || duracion === "null" || duracion === null || duracion === '') {
        $('#duracion_form').addClass(' has-error');
        $('#txt_error').html('El campo Duración es obligatorio');
        $('#duracion').attr("placeholder", "El campo Duración es obligatorio");
        subirArribaPagina();
        return false;
    }
        //dias
    else if (dias === "undefined" || dias === undefined || dias === "null" || dias === null || dias === '') {
        $('#dias_form').addClass(' has-error');
        $('#txt_error').html('El campo Días planificados es obligatorio');
        $('#dias').attr("placeholder", "El campo Días planificados es obligatorio");
        subirArribaPagina();
        return false;
    }
        //titulo_oficial
    else if (titulo_oficial === "undefined" || titulo_oficial === undefined || titulo_oficial === "null" || titulo_oficial === null || titulo_oficial === '') {
        $('#titulo_oficial_form').addClass(' has-error');
        $('#txt_error').html('El campo Título oficial es obligatorio');
        $('#titulo_oficial').attr("placeholder", "El campo Título oficial es obligatorio");
        subirArribaPagina();
        return false;
    }
        //contenido oficial
    else if (contenido_oficial === "undefined" || contenido_oficial === undefined || contenido_oficial === "null" || contenido_oficial === null || contenido_oficial === '') {
        $('#contenido_oficial_form').addClass(' has-error');
        $('#txt_error').html('El campo Contenido oficial es obligatorio');
        $('#contenido_oficial').attr("placeholder", "El campo Contenido oficial es obligatorio");
        subirArribaPagina();
        return false;
    }
        //ddltipos
    else if (ddltipos === "undefined" || ddltipos === undefined || ddltipos === "null" || ddltipos === null || ddltipos === '') {
        $('#tipología_form').addClass(' has-error');
        $('#txt_error').html('El campo Tipología es obligatorio');
        $('#ddltipos').attr("placeholder", "El campo Tipología es obligatorio");
        subirArribaPagina();
        return false;
    }   //ddlmetodologia
    else if (ddlmetodologia === "undefined" || ddlmetodologia === undefined || ddlmetodologia === "null" || ddlmetodologia === null || ddlmetodologia === '') {
        $('#metodologia_form').addClass(' has-error');
        $('#txt_error').html('El campo Metodología es obligatorio');
        $('#ddlmetodologia').attr("placeholder", "El campo Metodología es obligatorio");
        subirArribaPagina();
        return false;
    }
        //dificultad
    else if (ddldificultad === "undefined" || ddldificultad === undefined || ddldificultad === "null" || ddldificultad === null || ddldificultad === '') {
        $('#dificultad_form').addClass(' has-error');
        $('#txt_error').html('El campo Dificultad es obligatorio');
        $('#ddldificultad').attr("placeholder", "El campo Dificultad es obligatorio");
        subirArribaPagina();
        return false;
    }
        //autores
    else if (duplicatedautor) {
        $('#autoresmanagement').addClass(' has-error');
        $('#txt_error').html('No deben haber autores repetidos.');
        subirArribaPagina();
    }
        //area funcional
    else if (areas.length == 0) {
        $('#areascontainer').addClass(' has-error');
        $('#txt_error').html('Debe seleccionar al menos un área funcional');
        subirArribaPagina();
        return false;
    }
        //ddltematica
    else if (idstematicas.length == 0) {
        $('#tematicascontainer').addClass(' has-error');
        $('#txt_error').html('Debe seleccionar al menos una temática');
        subirArribaPagina();
        return false;
    }
    else
        return true;
}
// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para seleccionar y deseleccionar areas y tematicas ----------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------

function clickarea(id) {
    if ($('#areascontainer #' + id).hasClass('badge-primary')) {
        $('#areascontainer #' + id).removeClass('badge-primary');
    } else {
        $('#areascontainer #' + id).addClass('badge-primary');
    }
}

function clicktematica(id) {
    if ($('#tematicascontainer #' + id).hasClass('badge-primary')) {
        $('#tematicascontainer #' + id).removeClass('badge-primary');
    } else {
        $('#tematicascontainer #' + id).addClass('badge-primary');
    }
}

function addautor() {
    var autor = $('#ddlautor').val();
    if (!autor || autor == 0) {
        $('#autor_form').addClass(' has-error');
        $('#txt_error').html('Seleccione un autor por favor.');
        subirArribaPagina();
        return;
    }
    var autornombre = $('#ddlautor option:selected').text();
    var pc = $('#newautorpercent').val();
    if (!pc || pc == 0) {
        $('#autorpct').addClass(' has-error');
        $('#txt_error').html('Especifique el % de la autoría.');
        subirArribaPagina();
        return;
    }
    var html = "<span class='badge badge-primary badgeareas' id='#id#' data-pc='#pc#'>#nombre# (#pc# %)<i class='fa fa-trash ml-5px' onclick='deleteautor(#id#)'></i></span>"
    html = html.replace('#id#', autor);
    html = html.replace('#id#', autor);
    html = html.replace('#pc#', pc);
    html = html.replace('#pc#', pc);
    html = html.replace('#nombre#', autornombre);
    $('#autorescontainers').append(html);
}

function deleteautor(id) {
    $('#autoresmanagement #' + id).remove();
}

function showmessage(id, message, type) {
    $('#' + id).html(message);

    $('#' + id).removeClass('alert-success');
    $('#' + id).removeClass('alert-primary');
    $('#' + id).removeClass('alert-danger');

    if (type == 'success') {
        $('#' + id).addClass('alert-success');
    }
    if (type == 'primary') {
        $('#' + id).addClass('alert-primary');
    }
    if (type == 'danger') {
        $('#' + id).addClass('alert-danger');
    }

    //$('#' + id).show();
}

function hidemessage(id) {
    $('#' + id).html("");

    $('#' + id).removeClass('alert-success');
    $('#' + id).removeClass('alert-primary');
    $('#' + id).removeClass('alert-danger');
}

function refreshliterales() {
    var data = "{";
    data += "'id':" + $('#txtidcurso').val() + "}";
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/GetLiterales',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('literalesalert', "Refrescando literales ...", "primary");
        },
        success: function (response) {
            console.log(response);

            $('#literalescontainer').html(response.d);

            hidemessage('literalesalert');
        },
        error: function (error) {
            showmessage('literalesalert', "Se ha producido un error al refrescar las literales", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}

function guardarliterales() {
    var literales = [];
    var literaleslist = $('#literalescontainer input');
    console.log(literaleslist);
    for (var i = 0; i < literaleslist.length; i++) {
        literales.push('"' + literaleslist[i].value + '"');
        console.log(literaleslist[i].value);
    }

    var data = "{";
    data += "'id':" + $('#txtidcurso').val() + ",";
    data += "'literales': [" + literales + "]";
    data += "}"
    console.log(data);
    //return;
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/SaveLiterales',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('literalesalert', "Procesando petición ...", "primary");
            subirArribaPagina();
        },
        success: function (response) {
            showmessage('literalesalert', "Cambios guardados", "success");
            subirArribaPagina();
            refreshcontenidos();
            console.log(response);
            console.log($('#txtidcurso').val());
        },
        error: function (error) {
            showmessage('literalesalert', "Se ha producido un error al guardar los cambios", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}

function onremoveliteral(id, selector) {

    var r = confirm("¿Está seguro de que desea eliminar la literal?");
    if (r == true) {
        removeliteral(id, selector);
    }

}

function removeliteral(id, selector) {

    var data = "{";
    data += "'idcurso':" + $('#txtidcurso').val() + ",";
    data += "'id':" + id + "";
    data += "}"

    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/DeleteLiterales',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('literalesalert', "Procesando petición ...", "primary");
            subirArribaPagina();
        },
        success: function (response) {
            //refreshliterales();
            $("#" + selector).remove();
            refreshcontenidos();
            showmessage('literalesalert', "Cambios guardados", "success");
        },
        error: function (error) {
            showmessage('literalesalert', "Se ha producido un error al guardar los cambios", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}

function refreshcontenidos() {
    var data = "{";
    data += "'idcurso':" + $('#txtidcurso').val() + "}";
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/RenderHtmlContenidos',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('contenidosalert', "Refrescando contenidos ...", "primary");
        },
        success: function (response) {
            console.log(response);
            $('#contenidoscontainer').html(response.d);
            hidemessage('contenidosalert');
        },
        error: function (error) {
            showmessage('contenidosalert', "Se ha producido un error al refrescar los contenidos", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}

function ondeletecontenido(id) {
    var r = confirm("¿Está seguro de que desea eliminar el contenido?");
    if (r == true) {
        deletecontenido(id);
    }
}

function deletecontenido(id) {
    var data = "{";
    data += "'idcurso':" + $('#txtidcurso').val() + ",";
    data += "'idcontenido':" + id + "";
    data += "}"
    console.log(data);
    //return;
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/DeleteContenidoRecurso',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('contenidosalert', "Procesando petición ...", "primary");
            subirArribaPagina();
        },
        success: function (response) {
            showmessage('contenidosalert', "Cambios guardados", "success");
            subirArribaPagina();
            $('#contenidoscontainer').html(response.d);
        },
        error: function (error) {
            showmessage('contenidosalert', "Se ha producido un error al guardar los cambios", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}

function loadfile(id, idh) {
    console.log('IDS');
    console.log(id);
    console.log(idh);
    getBase64($('#' + id)[0].files[0], '#' + idh);
}

function getBase64(file, inputcontrolselector) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        $(inputcontrolselector).val(reader.result);
    };
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };
}

function onremoveprograma(tipo) {
    var r = confirm("¿Está seguro de que desea eliminar el archivo?");
    if (r == true) {
        removeprograma(tipo);
    }
}

function removeprograma(tipo) {

    var data = "{";
    data += "'idcurso':" + $('#txtidcurso').val() + ",";
    data += "'tipo':'" + tipo + "'";
    data += "}"
    console.log(data);
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/Deleteprograma',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('messagealert', "Procesando petición ...", "primary");
            subirArribaPagina();
        },
        success: function (response) {
            showmessage('messagealert', "Cambios guardados", "success");
            subirArribaPagina();
        },
        error: function (error) {
            showmessage('messagealert', "Se ha producido un error al guardar los cambios", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}

function addexistingcontent(sesion) {
    subirArribaPagina();
    recursosseleccionados = [];
    $('#addcontentsesion').val(sesion);
    $("#addcontentlectura").prop("checked", false);
    var data = "{";
    data += "'idcurso':" + $('#txtidcurso').val() + "}";
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/RenderHtmlRecursos',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            subirArribaPagina();
            showmessage('contenidosalert', "Obteniendo recursos ...", "primary");
        },
        success: function (response) {
            hidemessage('contenidosalert');
            $('#addcontentresources').html(response.d);

            $('#tabla_resources').DataTable({
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
                lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                "columnDefs": [
                  {
                      "targets": [0]
                  },
                  {
                      "targets": [1]
                  },
                  {
                      "targets": [2]
                  },
                  {
                      "targets": [3]
                  },
                  {
                      "targets": [4],
                      "class": "text-center",
                  },
                  {
                      "targets": [5],
                      "class": "text-center",
                  }
                ],
                "order": [[0, "desc"]]
            });

            $('#contenidoscontainer').hide(500);
            $('#addcontent').show(500);
        },
        error: function (error) {
            showmessage('contenidosalert', "Se ha producido un error obtener los recursos", "danger");
            subirArribaPagina();
            console.log(error)

        },
        complete: function () {

        }
    });

}

function onchangeresourcechk(idrecurso) {
    for (var i = 0; i < recursosseleccionados.length; i++) {
        if (recursosseleccionados[i] == idrecurso) {
            recursosseleccionados.splice(i, 1);
            return;
        }
    }
    recursosseleccionados.push(idrecurso);
}

function addrecursos() {
    if (recursosseleccionados.length == 0) {
        showmessage('contenidosalert', "Debe seleccionar al menos un recurso", "danger");
        subirArribaPagina();
        return;
    }
    var sesion = $('#addcontentsesion').val();
    var lectura = $('#addcontentlectura').prop('checked');
    var data = "{";
    data += "'idcurso':" + $('#txtidcurso').val() + ",";
    data += "'sesion':" + sesion + ",";
    data += "'lectura':" + lectura + ",";
    data += "'recursos': [" + recursosseleccionados + "]";
    data += "}"
    console.log(data);
    //return;
    $.ajax({
        type: "POST",
        url: 'cursos-mantenimiento.aspx/AddContenidoRecurso',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showmessage('contenidosalert', "Procesando petición ...", "primary");
            subirArribaPagina();
        },
        success: function (response) {
            showmessage('contenidosalert', "Cambios guardados", "success");
            subirArribaPagina();
            $('#contenidoscontainer').show(500);
            $('#addcontent').hide(500);
            refreshcontenidos();
        },
        error: function (error) {
            showmessage('contenidosalert', "Se ha producido un error al añadir", "danger");
            subirArribaPagina();
            console.log(error)
        }
    });
}

function cancelaraddrecurso() {
    $('#contenidoscontainer').show(500);
    $('#addcontent').hide(500);
}