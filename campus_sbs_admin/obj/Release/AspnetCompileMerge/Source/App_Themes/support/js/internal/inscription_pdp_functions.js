// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Cargar el textarea
    autosize($('#txt_comentarios'));

    /// 2.- Recuperar la tecla pulsada
    if ($("#txt_mat_alumno").val() === "") {
        $("#txt_mat_alumno").focus();
    }
    $("#txt_mat_alumno").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 3.- Autocompletar
    $("#txt_mat_alumno").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'matricula-pdp-mantenimiento.aspx/search_student',
                data: "{ 'name': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.nombre_completo,
                            val: item.id_usuario,
                            name: item.nombre,
                            surname: item.apellidos,
                            mail: item.email
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
            $('#txt_mat_nombre').val(ui.item.name);
            $('#txt_mat_apellidos').val(ui.item.surname);
            $('#txt_mat_mail').val(ui.item.mail);
        },
        minLength: 3
    });
});

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parametros
    var nombre = $('#txt_mat_nombre').val();
    var apellidos = $('#txt_mat_apellidos').val();
    var mail = $('#txt_mat_mail').val();
    var precio = $('#txt_mat_precio').val();
    var num_cursos = $('#txt_mat_cursos').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (nombre === "undefined" || nombre === undefined || nombre === "null" || nombre === null || nombre === '') {
        $('#nombre_form').addClass(' has-error');
        $('#txt_error').html('El campo Nombre es obligatorio');
        $('#txt_mat_nombre').attr("placeholder", "El campo Nombre es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (!validarCarateresEspeciales(nombre)) {
        $('#nombre_form').addClass(' has-error');
        $('#txt_error').html('El campo Nombre contiene carácteres no válidos');
        return false;
    }
    else if (apellidos === "undefined" || apellidos === undefined || apellidos === "null" || apellidos === null || apellidos === '') {
        $('#apellidos_form').addClass(' has-error');
        $('#txt_error').html('El campo Apellidos es obligatorio');
        $('#txt_mat_apellidos').attr("placeholder", "El campo Apellidos es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (!validarCarateresEspeciales(apellidos)) {
        $('#apellidos_form').addClass(' has-error');
        $('#txt_error').html('El campo Apellidos contiene carácteres no válidos');
        return false;
    }
    else if (mail === "undefined" || mail === undefined || mail === "null" || mail === null || mail === '') {
        $('#mail_form').addClass(' has-error');
        $('#txt_error').html('El campo Mail es obligatorio');
        $('#txt_mat_mail').attr("placeholder", "El campo Mail es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (!validarMail(mail)) {
        $('#mail_form').addClass(' has-error');
        $('#txt_error').html('El campo Mail contiene carácteres no válidos');
        return false;
    }
    else if (precio === "undefined" || precio === undefined || precio === "null" || precio === null || precio === '' || !validarTelefono(precio.replace(',','.'))) {
        $('#precio_form').addClass(' has-error');
        $('#txt_error').html('El campo Precio es un campo numérico');
        $('#txt_mat_precio').attr("placeholder", "El campo Horas es obligatorio");
        subirArribaPagina();
        return false;
    }
    else if (num_cursos === "undefined" || num_cursos === undefined || num_cursos === "null" || num_cursos === null || num_cursos === '' || !validarTelefono(num_cursos)) {
        $('#cursos_form').addClass(' has-error');
        $('#txt_error').html('El campo Nº de cursos es un campo numérico');
        $('#txt_mat_cursos').attr("placeholder", "El campo Nº de cursos es obligatorio");
        subirArribaPagina();
        return false;
    }
    else
        return true;
}