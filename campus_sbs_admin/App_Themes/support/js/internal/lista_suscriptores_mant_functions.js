// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero ----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Cargar el textarea
    autosize($('#txt_sql'));

    /// 2.- Recuperar la tecla pulsada
    if ($("#txt_name_list").val() === "") {
        $("#txt_name_list").focus();
    }
    $("#txt_name_list").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 3.- Autocompletar
    $("#txt_name_list").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'lista-suscriptores-mantenimiento.aspx/search_list',
                data: "{ 'name': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.nombre,
                            val: item.id_els
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
        /*select: function (e, ui) {
            $('#idListSuscriptores').val(ui.item.val);
        },*/
        minLength: 2
    });
});

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parametros
    var nombre = $('#txt_name_list').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (nombre === "undefined" || nombre === undefined || nombre === "null" || nombre === null || nombre === '') {
        $('#lista_form').addClass(' has-error');
        $('#txt_name_list').html('El nombre es obligatorio');
        $('#txt_error').html('El nombre es obligatorio');
        subirArribaPagina();
        return false;
    }
    else
        return true;
}