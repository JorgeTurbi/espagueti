// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Cargar el textarea
    autosize($('#txt_comentarios'));

    /// 2.- Recuperar la tecla pulsada
    if ($("#txt_prac_alumno").val() === "") {
        $("#txt_prac_alumno").focus();
    }
    $("#txt_prac_alumno").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 3.- Autocompletar
    $("#txt_prac_alumno").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'candidatos-mantenimiento.aspx/search_student',
                data: "{ 'name': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.nombre_completo,
                            val: item.id_usuario
                        }
                    }))
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
});

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parametros
    var idAlumno = $('#idAlumno').val();

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
    else
        return true;
}