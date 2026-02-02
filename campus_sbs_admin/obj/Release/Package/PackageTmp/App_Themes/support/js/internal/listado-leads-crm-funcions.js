// --------------------------------------------------------------------------------------------------
// Funciones que carga al arrancar la página --------------------------------------------------------
// --------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Recuperar la tecla pulsada
    if ($("#txt_search").val() === "") {
        $("#txt_search").focus();
    }
    $("#txt_search").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 2.- Autocompletar
    $("#txt_search").autocomplete({
        source: function (request, response) {
            // Fetch data
            $.ajax({
                url: 'functions.aspx/search_student',
                data: "{ 'name': '" + request.term + "'}",
                dataType: "json",
                type: 'post',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.nombre_completo,
                            val: item.id_usuario,
                            value: request.term
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
            var url = "/ficha-alumno-crm.aspx?idu=" + ui.item.val;
            location.href = url;
        },
        minLength: 3
    });

    /// 3.- Inicializa las tablas
    paint_table_nuevos();

    /// 4.- Controles del click del menu
    $('#nuevos-tab').on('click', function (e) {
        var _id_comercial = -1;
        if ($('#hid_comercial').val() !== '')
            _id_comercial = $('#hid_comercial').val();        
        $('#table_list_leads').html('');
        search_table(0, 0, _id_comercial, false);
        e.preventDefault();
        $(this).tab('show');
    });
    $('#avisos-tab').on('click', function (e) {
        var _id_comercial = -1;
        if ($('#hid_comercial_avisos').val() !== '')
            _id_comercial = $('#hid_comercial_avisos').val();
        $('#table_list_avisos').html('');
        search_table(1, 0, _id_comercial, true);
        e.preventDefault();
        $(this).tab('show');
    });
    $('#sin-contactar-tab').on('click', function (e) {
        $('#table_list_sin_contactar').html('');
        var _id_comercial = $('#contact_person').val();
        search_table(2, 5, _id_comercial, false);
        e.preventDefault();
        $(this).tab('show');
    });
    $('#proceso-tab').on('click', function (e) {
        $('#table_list_proceso').html('');
        var _id_comercial = $('#process_person').val();
        search_table(3, 12, _id_comercial, false);
        e.preventDefault();
        $(this).tab('show');
    });
    $('#futuro-tab').on('click', function (e) {
        $('#table_list_futuro').html('');
        var _id_comercial = $('#future_person').val();
        search_table(4, 3, _id_comercial, false);
        e.preventDefault();
        $(this).tab('show');
    });
    $('#todos-tab').on('click', function (e) {
        $('#table_list_todos').html('');        
        e.preventDefault();
        $(this).tab('show');
    });

    /// 5.- Inicializar los tooltip
    $('[data-toggle="tooltip"]').tooltip();
});

// --------------------------------------------------------------------------------------------------
// Tablas de los estados ----------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function paint_table_nuevos() {
    $('#tabla_Leads_Nuevos').DataTable({
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
              "type": "euro_date"
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
              "targets": [4]
          },
          {
              "targets": [5],
              "class": "text-center"
          },
          {
              "targets": [6],
              "class": "text-center"
          },
          {
              "targets": [7],
              "class": "text-center"
          }
        ],
        "order": [[0, "desc"]]
    });
}
function paint_table_avisos() {
    $('#tabla_Leads_Avisos').DataTable({
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
              "type": "euro_date"
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
              "targets": [4]
          },
          {
              "targets": [5],
              "class": "text-center"
          },
          {
              "targets": [6],
              "class": "text-center"
          },
          {
              "targets": [7]
          },
          {
              "targets": [8],
              "class": "text-center"
          }
        ],
        "order": [[0, "desc"]]
    });
}
function paint_table_sin_contactar() {
    $('#tabla_Leads_Sin_Contactar').DataTable({
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
              "type": "euro_date"
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
              "targets": [4]
          },
          {
              "targets": [5],
              "class": "text-center"
          },
          {
              "targets": [6],
              "class": "text-center"
          },
          {
              "targets": [7],
              "type": "euro_date"
          },
          {
              "targets": [8]
          },
          {
              "targets": [9],
              "class": "text-center"
          }
        ],
        "order": [[7, "desc"]]
    });
}
function paint_table_proceso() {
    $('#tabla_Leads_Proceso').DataTable({
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
              "type": "euro_date"
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
              "targets": [4]
          },
          {
              "targets": [5],
              "class": "text-center"
          },
          {
              "targets": [6],
              "class": "text-center"
          },
          {
              "targets": [7],
              "class": "text-center"
          },
          {
              "targets": [8],
              "type": "euro_date"
          },
          {
              "targets": [9]
          },
          {
              "targets": [10],
              "class": "text-center"
          }
        ],
        "order": [[8, "desc"]]
    });
}
function paint_table_futuro() {
    $('#tabla_Leads_Futuro').DataTable({
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
              "type": "euro_date"
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
              "targets": [4]
          },
          {
              "targets": [5],
              "class": "text-center"
          },
          {
              "targets": [6],
              "class": "text-center"
          },
          {
              "targets": [7],
              "type": "euro_date"
          },
          {
              "targets": [8]
          },
          {
              "targets": [9],
              "class": "text-center"
          }
        ],
        "order": [[7, "desc"]]
    });
}
function paint_table_todos() {
    $('#tabla_Leads_Todos').DataTable({
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
              "type": "euro_date"
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
              "targets": [4]
          },
          {
              "targets": [5],
              "class": "text-center"
          },
          {
              "targets": [6],
              "class": "text-center"
          },
          {
              "targets": [7],
              "class": "text-center"
          },
          {
              "targets": [8],
              "type": "euro_date"
          },
          {
              "targets": [9]
          },
          {
              "targets": [10],
              "class": "text-center"
          }
        ],
        "order": [[8, "desc"]]
    });
}

// --------------------------------------------------------------------------------------------------
// Función para buscar los datos de las tablas ------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_table(tab, status, comercial, aviso) {
    $('#wait_modal').modal('show');

    var urlWS = "listado-leads-crm.aspx/search_table";
    var data = "{'tab' : '" + tab + "', 'status' : '" + status + "', 'comercial' : '" + comercial + "', 'aviso' : '" + aviso + "'}";
    $.ajax({
        type: "POST",
        crossDomain: true,
        url: urlWS,
        data: data,
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {            
            var lst_leads = data.d;
            if (lst_leads.length > 0) {
                if (tab === 0) { /// Nuevos
                    $('#table_list_leads').append(lst_leads);                    
                    paint_table_nuevos();
                    $('#wait_modal').modal('hide');
                    return true;
                }
                else if (tab === 1) { /// Avisos
                    $('#table_list_avisos').append(lst_leads);
                    paint_table_avisos();
                    $('#wait_modal').modal('hide');
                    return true;
                }
                else if (tab === 2) { /// Sin contactar
                    $('#table_list_sin_contactar').append(lst_leads);
                    paint_table_sin_contactar();
                    $('[data-toggle="tooltip"]').tooltip();
                    $('#wait_modal').modal('hide');
                    return true;
                }
                else if (tab === 3) { /// En proceso
                    $('#table_list_proceso').append(lst_leads);
                    paint_table_proceso();
                    $('[data-toggle="tooltip"]').tooltip();
                    $('#wait_modal').modal('hide');
                    return true;
                }
                else if (tab === 4) { /// Futuro
                    $('#table_list_futuro').append(lst_leads);
                    paint_table_futuro();
                    $('[data-toggle="tooltip"]').tooltip();
                    $('#wait_modal').modal('hide');
                    return true;
                }
                else if (tab === 5) { /// Todos
                    $('#table_list_todos').append(lst_leads);
                    paint_table_todos();
                    $('[data-toggle="tooltip"]').tooltip();
                    $('#wait_modal').modal('hide');
                    return true;
                }
            }
        },
        error: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);            
            return false;
        },
        failure: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
            return false;
        }
    });
}

// --------------------------------------------------------------------------------------------------
// Función para buscar los leads por comerciales ----------------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_sin_contactar() {
    $('#table_list_sin_contactar').html('');
    var _id_comercial = $('#contact_person').val();
    search_table(2, 5, _id_comercial, false);
}
function search_proceso() {
    $('#table_list_proceso').html('');
    var _id_comercial = $('#process_person').val();
    search_table(3, 12, _id_comercial, false);
}
function search_futuro() {
    $('#table_list_futuro').html('');
    var _id_comercial = $('#future_person').val();
    search_table(4, 3, _id_comercial, false);
}
function search_todos() {
    $('#table_list_todos').html('');
    var _id_comercial = $('#all_person').val();
    var _status = $('#all_status').val();
    search_table(5, _status, _id_comercial, false);
}

// --------------------------------------------------------------------------------------------------
// Función para ir a la ficha de usuario a partir de su Id ------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_user() {
    /// 1.- Sacar al usuario
    var id_user = $('#text_user').val();

    /// 2.- Limpiar el error
    if ($('#text_user').hasClass("is-invalid"))
        $('#text_user').removeClass("is-invalid");

    /// 3.- Validar los datos
    if (id_user === "undefined" || id_user === undefined || id_user === "null" || id_user === null || id_user === '') {
        $('#text_user').addClass(' is-invalid');
        alert('Hay que seleccionar un id válido');
        return false;
    }
    else if (!validarTelefono(id_user)) {
        $('#text_user').addClass(' is-invalid');
        alert('El id del usuario introducido no es válido');
        return false;
    }
    else {
        /// 4.- Redirigir a la ficha del usuario
        var url = "/ficha-alumno-crm.aspx?idu=" + id_user;
        location.href = url;
    }
}