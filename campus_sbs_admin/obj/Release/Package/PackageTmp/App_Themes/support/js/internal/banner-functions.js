// ------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -----------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Llamar a la función de fileupload
    start_fileupload('fileupload_banner');
    start_fileupload('fileupload_banner2');
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
        dropZone: (name === "fileupload_banner" ? $('#file_banner') : $('#file_banner2')),
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
                if (file.deleteUrl) {
                    if (name === 'fileupload_banner')
                        $('#delete_banner').attr("onclick", "delete_banner('" + file.name + "','img_banner')");
                    else if(name === 'fileupload_banner2')
                        $('#delete_banner2').attr("onclick", "delete_banner2('" + file.name + "','img_banner')");

                    row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','img_banner')\"><i class='fas fa-trash-alt'></i></button>";
                }
                else
                    row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');

            if (file_photo !== '') {
                if (name === 'fileupload_banner') {
                    $('#modal-close-banner').click();
                    $('#txt_banner').val(file_photo);
                    if ($('#block_delete_banner').hasClass("hidden"))
                        $('#block_delete_banner').removeClass("hidden");
                    if (!$('#block_upload_banner').hasClass("hidden"))
                        $('#block_upload_banner').addClass(" hidden");
                    if ($('#block_see').hasClass("hidden"))
                        $('#block_see').removeClass("hidden");
                    
                    var link = "https://media.spainbs.com/recursos_www/recursos_productos/banners/temp/" + file_photo;
                    $('#lnk_banner').attr('href', link);
                }
                else if (name === 'fileupload_banner2') {
                    $('#modal-close-banner2').click();
                    $('#txt_banner2').val(file_photo);
                    if ($('#block_delete_banner2').hasClass("hidden"))
                        $('#block_delete_banner2').removeClass("hidden");
                    if (!$('#block_upload_banner2').hasClass("hidden"))
                        $('#block_upload_banner2').addClass(" hidden");
                    if ($('#block_see_banner').hasClass("hidden"))
                        $('#block_see_banner').removeClass("hidden");

                    var _link = "https://media.spainbs.com/recursos_www/recursos_productos/banners/temp/" + file_photo;
                    $('#lnk_banner2').attr('href', _link);
                }
            }
        },
        progressall: function (e, data) {
            $('#progress').removeClass('hidden');
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css('width', progress + '%');
        }
    });
}
function delete_banner(name, type) {
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
                if (data.files.length === 0)
                    deleteFileData();
                
                $('table .files').html('');
                $('#txt_banner').val('');

                if (!$('#block_delete_banner').hasClass("hidden"))
                    $('#block_delete_banner').addClass(" hidden");
                if ($('#block_upload_banner').hasClass("hidden"))
                    $('#block_upload_banner').removeClass("hidden");
                if (!$('#block_see').hasClass("hidden"))
                    $('#block_see').addClass(" hidden");

                $('#lnk_banner').attr('href', '');

                /*
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
                    $('#txt_banner').val('');
                    
                    if (!$('#block_delete_banner').hasClass("hidden"))
                        $('#block_delete_banner').addClass(" hidden");
                    if ($('#block_upload_banner').hasClass("hidden"))
                        $('#block_upload_banner').removeClass("hidden");
                    if (!$('#block_see').hasClass("hidden"))
                        $('#block_see').addClass(" hidden");

                    $('#lnk_banner').attr('href', '');
                }*/
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
function delete_banner2(name, type) {
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
                if(data.files.length === 0)
                    deleteFileData();

                $('table .files').html('');
                $('#txt_banner2').val('');

                if (!$('#block_delete_banner2').hasClass("hidden"))
                    $('#block_delete_banner2').addClass(" hidden");
                if ($('#block_upload_banner2').hasClass("hidden"))
                    $('#block_upload_banner2').removeClass("hidden");
                if (!$('#block_see_banner').hasClass("hidden"))
                    $('#block_see_banner').addClass(" hidden");

                $('#lnk_banner2').attr('href', '');

                /*var row = '';
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
                    
                }*/
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
function delete_banner_img() {
    /// 1.- Sacar los parámetros
    var id_banner = getParams('idb');
    var foto = $('#txt_banner').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Eliminar la foto
    var urlWS = "banner-mantenimiento.aspx/delete_img";
    var data = "{'id_banner' : '" + id_banner + "', 'photo' : '" + foto + "', 'first' : 'true'}";
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
                $('#txt_error').val('');

                if (!$('#block_delete_banner').hasClass("hidden"))
                    $('#block_delete_banner').addClass(" hidden");
                if ($('#block_upload_banner').hasClass("hidden"))
                    $('#block_upload_banner').removeClass("hidden");
                if (!$('#block_see').hasClass("hidden"))
                    $('#block_see').addClass("hidden");

                $('#txt_banner').val('');
                $('#lnk_banner').attr('href', '');
                return true;
            }
            else {
                alert('Se ha producido un error al eliminar la imagen del banner');
                return false;
            }
        },
        error: function (response) {
            alert('Se ha producido un error al eliminar la imagen del banner');
            return false;
        },
        failure: function (response) {
            alert('Se ha producido un error al eliminar la imagen del banner');
            return false;
        }
    });
}
function delete_banner_img2() {
    /// 1.- Sacar los parámetros
    var id_banner = getParams('idb');
    var foto = $('#txt_banner2').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Eliminar la foto
    var urlWS = "banner-mantenimiento.aspx/delete_img";
    var data = "{'id_banner' : '" + id_banner + "', 'photo' : '" + foto + "', 'first' : 'false'}";
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
                $('#txt_error').val('');

                if (!$('#block_delete_banner2').hasClass("hidden"))
                    $('#block_delete_banner2').addClass(" hidden");
                if ($('#block_upload_banner2').hasClass("hidden"))
                    $('#block_upload_banner2').removeClass("hidden");
                if (!$('#block_see_banner').hasClass("hidden"))
                    $('#block_see_banner').addClass("hidden");

                $('#txt_banner2').val('');
                $('#lnk_banner2').attr('href', '');
                return true;
            }
            else {
                alert('Se ha producido un error al eliminar la imagen del banner');
                return false;
            }
        },
        error: function (response) {
            alert('Se ha producido un error al eliminar la imagen del banner');
            return false;
        },
        failure: function (response) {
            alert('Se ha producido un error al eliminar la imagen del banner');
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
    var nombre = $('#txt_nombre').val();
    var producto = $('#ddlProducto').val();
    var orden = $('#txt_orden').val();
    var fecha = $('#txtFecha').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Comprobar los datos
    if (nombre === "undefined" || nombre === undefined || nombre === "null" || nombre === null || nombre === '') {
        $('#nombre_form').addClass(' has-error');
        $('#txt_error').html('El campo Nombre es obligatorio');
        $('#txt_nombre').attr("placeholder", "El campo Nombre es obligatorio");
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (producto === "-1") {
        $('#producto_form').addClass(' has-error');
        $('#txt_error').html('El Producto es obligatorio');
        $('#btn_save').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (orden === "undefined" || orden === undefined || orden === "null" || orden === null || orden === '') {
        $('#orden_form').addClass(' has-error');
        $('#txt_error').html('El orden es obligatorio');
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
    else {
        $('#btnGuardar').click();
        return true;
    }
}

// --------------------------------------------------------------------------------------------------
// Funciones ----------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_order() {
    var idProducto = $('#ddlProducto').val();
    if (idProducto > 0) {
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: 'banner-mantenimiento.aspx/search_order',
            data: "{'idProducto' : '" + idProducto + "'}",
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                $('#txt_orden').val((data.d + 1));
            },
            error: function (response) {
                return false;
            },
            failure: function (response) {
                return false;
            }
        });
    }
    else
        alert("Para buscar el siguiente valor de orden hay que seleccionar primero el proyecto al que pertenece");
}