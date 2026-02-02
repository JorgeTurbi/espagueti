// --------------------------------------------------------------------------------------------------
// Funciones carga inicial --------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Inicializar los datepicker y los dropdown
    $(".input-group.date").datepicker({
        language: "es",
        autoclose: true,
        todayHighlight: true
    });
    $('[data-toggle="tooltip"]').tooltip();
    $('.dropdown-toggle').dropdown();

    /// 2.- Inicializa las tablas
    paint_table_cursos();
    paint_table_ap();
    paint_table_ventas();
    paint_table_documentos();
    paint_table_acciones();
    paint_table_empleos();
    paint_table_comentarios();
    paint_table_examenes();

    /// 3.- Llamar a la función de fileupload
    start_fileupload('fileupload_logo');

    /// 4.- Poner las provincias
    var _pais = $('#ddlPaisResidencia').val();
    if (_pais > 0) {
        var _provincia = $('#ddlProvResidencia').val();
        searchProvinces(_pais, _provincia);
    }

    /// 5.- Función para quitar el ancla de la url y posicionar el ancla dento de la página
    $('a[href*="#"]').click(function (e) {
        window.location.hash = ''; // for older browsers, leaves a # behind
        history.pushState('', document.title, window.location.href); // nice and clean
        var query = e.currentTarget.href.split('#')[1];
        var height_header = $('header').height();
        $("html,body").animate({ scrollTop: $('#' + query).offset().top - height_header }, "slow");
        e.preventDefault(); // no page reload
    });
});

// --------------------------------------------------------------------------------------------------
// Funciones para buscar provincias a partir de un país y mostrar el sector profesional -------------
// --------------------------------------------------------------------------------------------------
function searchProvinces(idCountry, idProvincia) {
    if (idCountry !== null && idCountry !== '' && idCountry !== undefined) {
        $.ajax({
            type: "POST",
            crossDomain: true,
            url: 'ficha-alumno-crm.aspx/searchProvinces',
            data: "{'country' : '" + idCountry + "'}",
            contentType: "application/json; charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                $('#ddlProvResidencia').find('option').remove().end();
                $.each(data.d, function (index, _provincia) {
                    $('#ddlProvResidencia').append("<option value='" + _provincia.valor + "'>" + _provincia.nombre + "</option>");
                });
                $('#ddlProvResidencia').selectpicker('refresh');
                $('#ddlProvResidencia').selectpicker('val', idProvincia);
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
function searchJob(idSituation) {
    if (idSituation !== '-1' && (idSituation === '61' || idSituation === '62' || idSituation === '271'))
        $('#sector_form_all').removeClass('hidden');
    else if (!$('#sector_form_all').hasClass('hidden'))
        $('#sector_form_all').addClass(' hidden');
}

// --------------------------------------------------------------------------------------------------
// Tablas de los bloques ----------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function paint_table_cursos() {
    $('#tabla_cursos').DataTable({
        //responsive: true,
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
              "className": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [1],
              "className": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [2]
          },
          {
              "targets": [3],
              "className": "text-center",
              "type": "eu_date"
          }
        ],
        "order": [[0, "desc"]]
    });
}

function paint_table_ap() {
    var table_ap = $('#tabla_AP').DataTable({
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
              "className": 'details-control'

          },
          {
              "targets": [1],
              "class": "text-center",
              "type": "euro_date"
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
              "targets": [6]
          },
          {
              "targets": [7],
              "class": "text-center"
          },
          {
              "targets": [8],
              "class": "text-center"
          },
          {
              "targets": [9],
              "class": "text-center",
              "type": "euro_date"
          },
          {
              "targets": [10],
              "class": "text-center"
          },
          {
              "targets": [11],
              "class": "text-center"
          },
          {
              "targets": [12],
              "class": "text-center"
          },
          {
              "targets": [13],
              "class": "text-center"
          },
          {
              "targets": [14],
              "class": "text-center"
          }
        ],
        "order": [[9, "desc"]]
    });
    $('#tabla_AP tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table_ap.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var id = tr.find('td').find('span').html();
            if (id.indexOf(',') === -1)
                var _subTable = search_subtable(id, row, tr);
        }
    });
}
function paint_table_seg() {
    $('#tabla_AP_Seg').DataTable({
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
        lengthMenu: [[10, 25, -1], [10, 25, "All"]],
        "columnDefs": [
          {
              "targets": [0],
              "class": "text-center",
              "type": "euro_date"
          },
          {
              "targets": [1],
              "class": "text-center"
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
          }
        ],
        "order": [0, "desc"]
    });
}
function search_subtable(id, row, tr) {
    $.ajax({
        url: 'ficha-alumno-crm.aspx/search_subtable_ap',
        data: "{ 'id': '" + id + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            row.child(data.d);
            row.child.show();
            tr.addClass('shown');
            paint_table_seg();
            $('[data-toggle="tooltip"]').tooltip();
        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });
}

function paint_table_ventas() {
    var table_ventas = $('#tabla_Matriculas').DataTable({
        //responsive: true,
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
              "className": 'details-control'

          },
          {
              "targets": [1],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [2],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [3],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [4],
              "class": "text-center"
          },
          {
              "targets": [5]
          },
          {
              "targets": [6]
          },
          {
              "targets": [7]
          },
          {
              "targets": [8]
          },
          {
              "targets": [9],
              "class": "text-center"
          },
          {
              "targets": [10],
              "class": "text-center"
          },
          {
              "targets": [11],
              "class": "text-center"
          },
          {
              "targets": [12],
              "class": "text-center"
          },
          {
              "targets": [13],
              "class": "text-center"
          },
          {
              "targets": [14]
          },
          {
              "targets": [15],
              "class": "text-center"
          },
          {
              "targets": [16],
              "class": "text-center"
          },
          {
              "targets": [17],
              "class": "text-center"
          }
        ],
        "order": [[1, "desc"]]
    });
    $('#tabla_Matriculas tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table_ventas.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var id = tr.find('td').find('span').html();
            if (id.indexOf(',') !== -1)
                var _subTable = search_subtable_ventas(id, row, tr);
        }
    });
}
function paint_table_pays() {
    $('#tabla_pays').DataTable({
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
        lengthMenu: [[-1], ["All"]],
        "columnDefs": [
          {
              "targets": [0],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [1],
              "class": "text-center",
              "type": "eu_date"
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
              "targets": [4],
              "class": "text-center"
          },
          {
              "targets": [5],
              "class": "text-center"
          },
          {
              "targets": [6],
              "class": "text-center"
          }
        ],
        "order": [0, "desc"]
    });
}
function search_subtable_ventas(id, row, tr) {
    $.ajax({
        url: 'ficha-alumno-crm.aspx/search_subtable_ventas',
        data: "{ 'id': '" + id + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            row.child(data.d);
            row.child.show();
            tr.addClass('shown');
            paint_table_pays();
            $('[data-toggle="tooltip"]').tooltip();
        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });
}

function paint_table_documentos() {
    $('#tabla_documentos').DataTable({
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
              "targets": [1]
          },
          {
              "targets": [2],
              "className": "text-center"
          },
          {
              "targets": [3],
              "className": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [4]
          },
          {
              "targets": [5],
              "className": "text-center"
          },
          {
              "targets": [6],
              "className": "text-center"
          }
        ],
        "order": [[0, "desc"]]
    });
}
function paint_table_acciones() {
    $('#tabla_acciones').DataTable({
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
              "className": "text-center",
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
              "targets": [5]
          },
          {
              "targets": [6]
          },
          {
              "targets": [7]
          },
          {
              "targets": [8]
          }
        ],
        "order": [[0, "desc"]]
    });
}
function paint_table_empleos() {
    $('#tabla_empleos').DataTable({
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
              "className": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [1],
              "className": "text-center",
              "type": "eu_date"
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
              "targets": [5]
          },
          {
              "targets": [6],
              "className": "text-center"
          }
        ],
        "order": [[0, "asc"]]
    });
}
function paint_table_comentarios() {
    $('#tabla_comentarios').DataTable({
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
              "className": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [1]
          },
          {
              "targets": [2]
          },
          {
              "targets": [3],
              "className": "text-center"
          },
          {
              "targets": [4],
              "className": "text-center"
          }
        ],
        "order": [[0, "asc"]]
    });
}
function paint_table_examenes() {
    $('#tabla_examenes').DataTable({
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
              "className": "text-center",
              "type": "eu_date"
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
              "className": "text-center",
              "type": "euro_date"
          },
          {
              "targets": [5],
              "className": "text-center",
              "type": "euro_date"
          },
          {
              "targets": [6],
              "className": "text-center"
          },
          {
              "targets": [7],
              "className": "text-center"
          },
          {
              "targets": [8],
              "className": "text-center"
          }
        ],
        "order": [[0, "desc"], [4, "desc"]]
    });
}

// --------------------------------------------------------------------------------------------------
// Funciones para la foto ---------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function delete_foto() {
    /// 1.- Sacar los parámetros
    var id_user = getParams('idu');
    var foto = $('#txt_foto').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');

    /// 3.- Eliminar la foto
    var urlWS = "ficha-alumno-crm.aspx/eliminar_photo";
    var data = "{'id_user' : '" + id_user + "', 'photo' : '" + foto + "'}";
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
                if ($('#upload_photo').hasClass("hidden"))
                    $('#upload_photo').removeClass("hidden");
                if (!$('#delete_photo').hasClass("hidden"))
                    $('#delete_photo').addClass(" hidden");

                $('#txt_foto').val('');
                $('#foto_user').attr('src', 'https://media.spainbs.com/recursos_www/img_alumnos/generic/sin_foto.png');
                return true;
            }
            else {
                alert('Se ha producido un error al eliminar la foto del usuario');
                return false;
            }
        },
        error: function (response) {
            alert('Se ha producido un error al eliminar la foto del usuario');
            return false;
        },
        failure: function (response) {
            alert('Se ha producido un error al eliminar la foto del usuario');
            return false;
        }
    });
}
function save_photo(photo) {
    var id_user = getParams('idu');

    var urlWS = "ficha-alumno-crm.aspx/save_photo";
    var data = "{'id_user' : '" + id_user + "', 'photo' : '" + photo + "'}";
    $.ajax({
        type: "POST",
        crossDomain: true,
        url: urlWS,
        data: data,
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            var save = data.d;
            if (save) {
                $('#modal-close').click();
                $('#txt_foto').val(photo);
                if ($('#delete_photo').hasClass("hidden"))
                    $('#delete_photo').removeClass("hidden");
                if (!$('#upload_photo').hasClass("hidden"))
                    $('#upload_photo').addClass(" hidden");

                var link = "https://media.spainbs.com/alumnos/fotos/" + photo;
                $('#foto_user').attr('src', link);
                return true;
            }
            else {
                alert('Se ha producido un error al guardar la foto del usuario');
                return false;
            }
        },
        error: function (response) {
            alert('Se ha producido un error al guardar la foto del usuario');
            return false;
        },
        failure: function (response) {
            alert('Se ha producido un error al guardar la foto del usuario');
            return false;
        }
    });
}

// --------------------------------------------------------------------------------------------------
// Funciones para subir y eliminar un fichero -------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function start_fileupload(name) {
    /// 1.- Eliminar los datos de la sesión
    sessionStorage.clear();

    /// 2.- Función que sube un fichero al servidor
    $('#' + name + '').fileupload({
        dataType: 'json',
        dropZone: $('#file_foto'),
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
                //$('#txt_foto').val(file.name);

                row += "</td><td><span class='size'>" + formatFileSize(file.size) + "</span></td><td>";
                if (file.deleteUrl)
                    row += "<button class='btn btn-danger delete' onclick=\"delete_file('" + file.name + "','img_ficha')\"><i class='fas fa-trash-alt'></i></button>";
                else
                    row += "<button class='btn btn-warning cancel'><i class='fas fa-times-circle'></i></button>";
                row += "</td></tr>";
            });
            save_file(data, false);
            $('#progress').addClass(' hidden');

            //$('#tbl_foto .files').append(row);

            if (file_photo !== '')
                save_photo(file_photo);
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

                    $('#txt_foto').val('');
                }
                else {
                    deleteFileData();
                    $('table .files').html('');
                    $('table .files').append(row);

                    $('#txt_foto').val('');
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

// --------------------------------------------------------------------------------------------------
// Funciones ----------------------------------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function change_password() {
    $('#btn_Password').click();
}
function baja_user() {
    /// 1.- Sacar los parámetros
    var motivo_baja = $('#motivo_baja').val();

    /// 2.- Validar los datos
    if (motivo_baja === "undefined" || motivo_baja === undefined || motivo_baja === "null" || motivo_baja === null || motivo_baja === '') {
        alert("Hay que poner un motivo para la baja");
        return false;
    }
    else {
        //$('#hid_motivo_baja').val(motivo_baja);
        $('#btn_baja').click();
    }
}
function send_mail() {
    $('#btn_send_mail').click();
}
function eliminar_link(id) {
    $("#hid_link").val(id);
    $('#btn_delete_link').click();
}
function eliminarPeticionInfo(id) {
    $("#hid_peticion_info").val(id);
    $('#btn_delete_pet_info').click();
}
function eliminar_seguimiento(id) {
    $("#hid_seguimiento").val(id);
    $('#btn_delete_seguimiento').click();
}
function eliminar_asignacion_comercial(curso, docencia) {
    $("#hid_curso").val(curso);
    $("#hid_docencia").val(docencia);
    $('#btn_delete_asig_comercial').click();
}
function eliminar_pago(id) {
    $("#hid_pago").val(id);
    $('#btn_delete_pago').click();
}
function enviar_pago(id) {
    $("#hid_pago").val(id);
    $('#btn_send_pago').click();
}
function eliminar_documento(id) {
    $("#hid_documento").val(id);
    $('#btn_delete_documento').click();
}
function procesar_pet_info(id) {
    $("#hid_peticion_info").val(id);
    $('#btn_procesar_peticion').click();
}
function add_cuerpo_mail(body, adjuntos) {
    /// 1.- Cuerpo
    $('#modal_body_mail').html(body);

    /// 2.- Adjuntos
    $('#modal_adjuntos_mail').html('');
    var _adjuntos_html = '';    
    if (adjuntos != '') {
        adjuntos.split(',').forEach(function (item) {
            _adjuntos_html += "<a class='px-2' href='" + item + "' target='_blank'><i class='far fa-file-archive fa-1-6x'></i></a>";
        });
    }
    $('#modal_adjuntos_mail').html(_adjuntos_html);

    /// 3.- Mostrar el modal
    $('#modal_mail').modal('show');
}
function show_tag() {
    if ($('#blk_add_tag').hasClass("hidden"))
        $('#blk_add_tag').removeClass("hidden");

    $('#tag_user').val('');
}
function close_tag() {
    if (!$('#blk_add_tag').hasClass("hidden"))
        $('#blk_add_tag').addClass(" hidden");

    $('#tag_user').val('');
}
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
        var id_user = getParams('idu');

        /// 3.1.- Guardar el tag
        var urlWS = "ficha-alumno-crm.aspx/add_tag_user";
        var data = "{'idUsuario' : '" + id_user + "', 'tag' : '" + tag + "'}";
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
                    close_tag();
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
function eliminar_comentario(id) {
    $("#hid_comentario").val(id);
    $('#btn_delete_comentario').click();
}
function procesar_pet_diploma(idd, idc) {
    $("#hid_idDocencia").val(idd);
    $("#hid_idCurso").val(idc);
    $('#btn_solicitar_diploma').click();
}

// --------------------------------------------------------------------------------------------------
// Funciones para validar el formulario -------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function validarFormulario() {
    /// 0.- Deshabilitar el botón
    $('#btn_guardar').attr('disabled', 'disabled');

    /// 1.- Sacar los parámetros
    var nombre = $('#txt_nombre').val();
    var apellidos = $('#txt_apellidos').val();
    var mail = $('#txt_mail').val();
    var telefono = $('#txt_telefono').val();
    var mail2 = $('#txt_mail2').val();
    var fecha_alta = $('#txtFechaAlta').val();
    var pais = $('#ddlPaisResidencia').val();    
    var provincia = $('#ddlProvResidencia').val();

    /// 2.- Limpiar el error
    $('#txt_error').html('');
    if ($('#txt_nombre').hasClass("is-invalid"))
        $('#txt_nombre').removeClass("is-invalid");
    if ($('#txt_apellidos').hasClass("is-invalid"))
        $('#txt_apellidos').removeClass("is-invalid");
    if ($('#txt_mail').hasClass("is-invalid"))
        $('#txt_mail').removeClass("is-invalid");
    if ($('#txt_telefono').hasClass("is-invalid"))
        $('#txt_telefono').removeClass("is-invalid");
    if ($('#txt_mail2').hasClass("is-invalid"))
        $('#txt_mail2').removeClass("is-invalid");
    if ($('#txtFechaAlta').hasClass("is-invalid"))
        $('#txtFechaAlta').removeClass("is-invalid");
    if ($('#country_form').hasClass("has-error"))
        $('#country_form').removeClass("has-error");
    if ($('#province_form').hasClass("has-error"))
        $('#province_form').removeClass("has-error");
   
    /// 3.- Validar los datos
    if (nombre === "undefined" || nombre === undefined || nombre === "null" || nombre === null || nombre === '') {
        $('#txt_nombre').addClass(' is-invalid');
        $('#txt_error').html('El campo Nombre es obligatorio');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (!validarCarateresEspeciales(nombre)) {
        $('#txt_nombre').addClass(' is-invalid');
        $('#txt_error').html('El campo Nombre contiene carácteres no válidos');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (apellidos === "undefined" || apellidos === undefined || apellidos === "null" || apellidos === null || apellidos === '') {
        $('#txt_apellidos').addClass(' is-invalid');
        $('#txt_error').html('El campo Apellidos es obligatorio');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (!validarCarateresEspeciales(apellidos)) {
        $('#txt_apellidos').addClass(' is-invalid');
        $('#txt_error').html('El campo Apellidos contiene carácteres no válidos');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (mail === "undefined" || mail === undefined || mail === "null" || mail === null || mail === '') {
        $('#txt_mail').addClass(' is-invalid');
        $('#txt_error').html('El campo Mail es obligatorio');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (!validarMail(mail)) {
        $('#txt_mail').addClass(' is-invalid');
        $('#txt_error').html('El campo Mail contiene carácteres no válidos');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (telefono === "undefined" || telefono === undefined || telefono === "null" || telefono === null || telefono === '') {
        $('#txt_telefono').addClass(' is-invalid');
        $('#txt_error').html('El campo Teléfono es obligatorio');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (!validarCarateresEspeciales(telefono)) {
        $('#txt_telefono').addClass(' is-invalid');
        $('#txt_error').html('El campo Teléfono contiene carácteres no válidos');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }    
    else if (fecha_alta === "undefined" || fecha_alta === undefined || fecha_alta === "null" || fecha_alta === null || fecha_alta === '') {
        $('#txtFechaAlta').addClass(' is-invalid');
        $('#txt_error').html('La fecha de alta es obligatoria');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (pais === "-1") {
        $('#country_form').addClass(' has-error');
        $('#txt_error').html('El país de residencia es obligatorio');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (provincia === "-1") {
        $('#province_form').addClass(' has-error');
        $('#txt_error').html('Hay que rellenar la provincia de residencia');
        $('#btn_guardar').removeAttr('disabled');
        subirArribaPagina();
        return false;
    }
    else if (mail2 !== '') {
        if (!validarMail(mail2)) {
            $('#txt_mail2').addClass(' is-invalid');
            $('#txt_error').html('El campo Mail2 contiene carácteres no válidos');
            $('#btn_guardar').removeAttr('disabled');
            subirArribaPagina();
            return false;
        }
        else if (mail === mail2) {
            $('#txt_mail2').addClass(' is-invalid');
            $('#txt_error').html('No puede haber dos mails iguales');
            $('#btn_guardar').removeAttr('disabled');
            subirArribaPagina();
            return false;
        }
        else {
            $('#btn_guardar').removeAttr('disabled');
            return true;
        }
    }
    else {
        $('#btn_guardar').removeAttr('disabled');
        return true;
    }
}