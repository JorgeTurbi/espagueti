// --------------------------------------------------------------------------------------------------------------------------------
// Funciones de carga al inicio de la pagina --------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------------------------------------
$(document).ready(function () {
    $('#table_all_status').DataTable({
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
        lengthMenu: [[11, 20, -1], [11, 20, "All"]],
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

    $('#table_status').DataTable({
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
        lengthMenu: [[11, 20, -1], [11, 20, "All"]],
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

    $('#table_all_origins').DataTable({
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
          },
          {
              "targets": [2],
              "class": "text-center"
          }
        ],
        "order": [[1, "asc"]]
    });

    $('#table_origins').DataTable({
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
          },
          {
              "targets": [2],
              "class": "text-center"
          }
        ],
        "order": [[1, "asc"]]
    });

    $('#table_all_courses').DataTable({
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
          },
          {
              "targets": [2],
              "class": "text-center"
          }
        ],
        "order": [[1, "asc"]]
    });

    $('#table_courses').DataTable({
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
          },
          {
              "targets": [2],
              "class": "text-center"
          }
        ],
        "order": [[1, "asc"]]
    });

    $('#table_all_paises').DataTable({
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

    $('#table_paises').DataTable({
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

    $('#table_listado_tipos').DataTable({
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
$(document).on('keypress', function (e) {
    if (13 === e.which) { // 13 es la asignacion de la tecla enter
        e.preventDefault();
    }
});

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar el formulario ----------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 1.- Sacar los parametros
    var tipo = $('#ddl_lista_tipo_automatizacion').val();
    var orden = $('#txt_orden').val();
    var nombre_regla = $('#txt_name_rule').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (nombre_regla === "undefined" || nombre_regla === undefined || nombre_regla === "null" || nombre_regla === null || nombre_regla === '') {
        $('#name_rule_form').addClass(' has-error');
        $('#txt_error').html('El campo Nombre es obligatorio');
        $('#txt_name_rule').attr("placeholder", "El campo Nombre es obligatorio");
        return false;
    }
    else if (tipo === "-1") {
        $('#tipo_form').addClass(' has-error');
        $('#txt_error').html('El tipo de automatización es obligatorio');
        return false;
    }
    else if (orden === "undefined" || orden === undefined || orden === "null" || orden === null || orden === '') {
        $('#orden_form').addClass(' has-error');
        $('#txt_error').html('El campo Orden es obligatorio');
        $('#txt_orden').attr("placeholder", "El campo Orden es obligatorio");
        return false;
    }
    else
        return true;
}
function validarTipoAccion() {
    /// 1.- Sacar los parametros
    var tipo = $('#ddl_lista_acciones').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (tipo === "-1") {
        $('#tipo_accion_form').addClass(' has-error');
        $('#txt_error').html('El tipo de acción es obligatorio');
        return false;
    }
    else
        return true;
}

// --------------------------------------------------------------------------------------------------------------------------------
// Funciones para marcar y desmarcar checks ---------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------------------------------------
function chk_mark_status(control, campo) {
    var cadena = '';
    if (campo === 1)
        cadena = $('#hidStatus').val();
    else
        cadena = $('#hidStatusSel').val();
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
        $('#hidStatus').val(cadena);
    else
        $('#hidStatusSel').val(cadena);
}
function chk_mark_origin(control, campo) {
    var cadena = '';
    if (campo === 1)
        cadena = $('#hidOrigenes').val();
    else
        cadena = $('#hidOrigenesSel').val();
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
        $('#hidOrigenes').val(cadena);
    else
        $('#hidOrigenesSel').val(cadena);
}
function chk_mark_course(control, campo) {
    var cadena = '';
    if (campo === 1)
        cadena = $('#hidCursos').val();
    else
        cadena = $('#hidCursosSel').val();
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
        $('#hidCursos').val(cadena);
    else
        $('#hidCursosSel').val(cadena);
}
function chk_mark_country(control, campo) {
    var cadena = '';
    if (campo === 1)
        cadena = $('#hidPais').val();
    else
        cadena = $('#hidPaisSel').val();
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
        $('#hidPais').val(cadena);
    else
        $('#hidPaisSel').val(cadena);
}

// --------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar los checks ----------------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------------------------------------
function validarChkStatusAll() {
    /// 1.- Sacar los parametros
    var status = $('#hidStatus').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (status === "undefined" || status === undefined || status === "null" || status === null || status === '') {
        alert('Hay que seleccionar al menos un estado en la tabla estados.');
        return false;
    }
    else
        return true;
}
function validarChkStatusSel() {
    /// 1.- Sacar los parametros
    var status = $('#hidStatusSel').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (status === "undefined" || status === undefined || status === "null" || status === null || status === '') {
        alert('Hay que seleccionar al menos un estado en la tabla estados seleccionados.');
        return false;
    }
    else
        return true;
}
function validarChkOriginAll() {
    /// 1.- Sacar los parametros
    var origins = $('#hidOrigenes').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (origins === "undefined" || origins === undefined || origins === "null" || origins === null || origins === '') {
        alert('Hay que seleccionar al menos un origen en la tabla origenes.');
        return false;
    }
    else
        return true;
}
function validarChkOriginSel() {
    /// 1.- Sacar los parametros
    var origins = $('#hidOrigenesSel').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (origins === "undefined" || origins === undefined || origins === "null" || origins === null || origins === '') {
        alert('Hay que seleccionar al menos un origen en la tabla origenes seleccionados.');
        return false;
    }
    else
        return true;
}
function validarChkCourseAll() {
    /// 1.- Sacar los parametros
    var courses = $('#hidCursos').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (courses === "undefined" || courses === undefined || courses === "null" || courses === null || courses === '') {
        alert('Hay que seleccionar al menos un curso en la tabla cursos.');
        return false;
    }
    else
        return true;
}
function validarChkCourseSel() {
    /// 1.- Sacar los parametros
    var courses = $('#hidCursosSel').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (courses === "undefined" || courses === undefined || courses === "null" || courses === null || courses === '') {
        alert('Hay que seleccionar al menos un curso en la tabla cursos seleccionados.');
        return false;
    }
    else
        return true;
}
function validarChkCountryAll() {
    /// 1.- Sacar los parametros
    var paises = $('#hidPais').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (paises === "undefined" || paises === undefined || paises === "null" || paises === null || paises === '') {
        alert('Hay que seleccionar al menos un país en la tabla países.');
        return false;
    }
    else
        return true;
}
function validarChkCountrySel() {
    /// 1.- Sacar los parametros
    var paises = $('#hidPaisSel').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (paises === "undefined" || paises === undefined || paises === "null" || paises === null || paises === '') {
        alert('Hay que seleccionar al menos un país en la tabla países seleccionados.');
        return false;
    }
    else
        return true;
}

// --------------------------------------------------------------------------------------------------------------------------------
// Funciones para validar las fechas ----------------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------------------------------------
function validateDate() {
    /// 1.- Sacar los parametros
    var date_all = $('#chk_date_all').is(':checked');
    var fecha_inicio = $('#txt_fecha_inicio').val();
    var fecha_fin = $('#txt_fecha_fin').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Validar los datos
    if (!date_all) {
        if (fecha_inicio === "undefined" || fecha_inicio === undefined || fecha_inicio === "null" || fecha_inicio === null || fecha_inicio === '') {
            $('#fecha_inicio_form').addClass(' has-error');
            $('#txt_error').html('El campo Fecha Inicio es obligatorio');
            $('#txt_fecha_inicio').attr("placeholder", "El campo Fecha Inicio es obligatorio");
            return false;
        }
        else if (fecha_fin === "undefined" || fecha_fin === undefined || fecha_fin === "null" || fecha_fin === null || fecha_fin === '') {
            $('#fecha_fin_form').addClass(' has-error');
            $('#txt_error').html('El campo Fecha Fin es obligatorio');
            $('#txt_fecha_fin').attr("placeholder", "El campo Fecha Fin es obligatorio");
            return false;
        }
        else
            return true;
    }
    else
        return true;
}
function validateHours() {
    /// 1.- Sacar los parametros
    var hour_all = $('#chk_hour_all').is(':checked');
    var hora_inicio = $('#hour_start').val();
    var minutos_inicio = $('#min_start').val();
    var hora_fin = $('#hour_end').val();
    var minutos_fin = $('#min_end').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    $('#hora_inicio_form').removeClass('has-error');
    $('#min_inicio_form').removeClass('has-error');
    $('#hora_fin_form').removeClass('has-error');
    $('#min_fin_form').removeClass('has-error');

    /// 3.- Validar los datos
    if (!hour_all) {
        if (hora_inicio === "undefined" || hora_inicio === undefined || hora_inicio === "null" || hora_inicio === null || hora_inicio === '') {
            $('#hora_inicio_form').addClass(' has-error');
            $('#txt_error').html('El campo Hora Inicio es obligatorio');
            return false;
        }
        else if (minutos_inicio === "undefined" || minutos_inicio === undefined || minutos_inicio === "null" || minutos_inicio === null || minutos_inicio === '') {
            $('#min_inicio_form').addClass(' has-error');
            $('#txt_error').html('El campo Minutos inicio es obligatorio');
            return false;
        }
        else if (hora_fin === "undefined" || hora_fin === undefined || hora_fin === "null" || hora_fin === null || hora_fin === '') {
            $('#hora_fin_form').addClass(' has-error');
            $('#txt_error').html('El campo Hora fin es obligatorio');
            return false;
        }
        else if (minutos_fin === "undefined" || minutos_fin === undefined || minutos_fin === "null" || minutos_fin === null || minutos_fin === '') {
            $('#min_fin_form').addClass(' has-error');
            $('#txt_error').html('El campo Minutos fin es obligatorio');
            return false;
        }
        else
            return true;
    }
    else
        return true;
}