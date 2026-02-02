// --------------------------------------------------------------------------------------------------
// Funciones que carga al arrancar la página --------------------------------------------------------
// --------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Pintar las tablas
    paint_table_day();
    paint_table_month();

    /// 2.- Recuperar la tecla pulsada
    if ($("#txt_search").val() === "") {
        $("#txt_search").focus();
    }
    $("#txt_search").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 3.- Autocompletar
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
});

// --------------------------------------------------------------------------------------------------
// Funciones que carga de las tablas ----------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function paint_table_day() {
    var table_leads = $('#tabla_List_Leads').DataTable({
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
              "className": 'details-control',
          },
          {
              "targets": [1]
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
          }
        ],
        "order": [[1, "asc"]],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            /// Nº leads
            number = api.column(2)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString());
                }, 0);

            /// Update footer
            $(api.column(2).footer()).html(PonerPuntoMil(number));

            /// Nº leads procesados
            procesados = api.column(3)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString());
                }, 0);

            /// Update footer
            $(api.column(3).footer()).html(PonerPuntoMil(procesados));

            /// Nº leads sin procesar
            sin_procesar = api.column(4)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString());
                }, 0);

            /// Update footer
            $(api.column(4).footer()).html(PonerPuntoMil(sin_procesar));
        }
    });
    $('#tabla_List_Leads tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table_leads.row(tr);

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
function paintTableLevel2() {
    $('#tabla_List_Leads_Origin').DataTable({
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
              "class": "text-center"
          },
          {
              "targets": [5],
              "class": "text-center"
          }
        ],
        "order": [[3, "asc"], [1, "asc"]]
    });
}
function paint_table_month() {
    var table_leads_month = $('#tabla_List_Leads_Month').DataTable({
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
              "className": 'details-control',
          },
          {
              "targets": [1]
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
          }
        ],
        "order": [[1, "asc"]],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            /// Nº leads
            number = api.column(2)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString());
                }, 0);

            /// Update footer
            $(api.column(2).footer()).html(PonerPuntoMil(number));

            /// Nº leads procesados
            procesados = api.column(3)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString());
                }, 0);

            /// Update footer
            $(api.column(3).footer()).html(PonerPuntoMil(procesados));

            /// Nº leads sin procesar
            sin_procesar = api.column(4)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString());
                }, 0);

            /// Update footer
            $(api.column(4).footer()).html(PonerPuntoMil(sin_procesar));
        }
    });
    $('#tabla_List_Leads_Month tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table_leads_month.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var id = tr.find('td').find('span').html();
            if (id.indexOf(',') === -1)
                var _subTable = search_subtable_month(id, row, tr);
        }
    });
}
function paintTableMonthLevel2() {
    $('#tabla_List_Leads_Month_Origin').DataTable({
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
              "class": "text-center"
          },
          {
              "targets": [5],
              "class": "text-center"
          }
        ],
        "order": [[3, "asc"], [1, "asc"]]
    });
}

// --------------------------------------------------------------------------------------------------
// Funciones que buscar las subtablas ---------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_subtable(id, row, tr) {
    var day = $('#hid_day').val();
    var month = $('#hid_day_month').val();
    var year = $('#hid_day_year').val();
    $('#wait_modal').modal('show');

    $.ajax({
        url: 'informe-leads-crm.aspx/search_subtable',
        data: "{ 'id': '" + id + "', 'day': '" + day + "', 'month': '" + month + "', 'year': '" + year + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            row.child(data.d);
            row.child.show();
            tr.addClass('shown');
            $('#wait_modal').modal('hide');
            paintTableLevel2();
        },
        error: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        },
        failure: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        }
    });
}
function search_subtable_month(id, row, tr) {
    var month = $('#hid_month').val();
    var year = $('#hid_year').val();
    $('#wait_modal').modal('show');

    $.ajax({
        url: 'informe-leads-crm.aspx/search_subtable_month',
        data: "{ 'id': '" + id + "', 'month': '" + month + "', 'year': '" + year + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            row.child(data.d);
            row.child.show();
            tr.addClass('shown');
            $('#wait_modal').modal('hide');
            paintTableMonthLevel2();
        },
        error: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        },
        failure: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        }
    });
}

// --------------------------------------------------------------------------------------------------
// Funciones para avanazar día a día ----------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_day_ant() {
    var day = $('#hid_day').val();
    var month = $('#hid_day_month').val();
    var year = $('#hid_day_year').val();

    var fecha = new Date(year, month - 1, day);
    var yesterday = new Date(fecha.getTime() - 24 * 60 * 60 * 1000);

    $('#hid_day').val(yesterday.getDate());
    $('#hid_day_month').val(yesterday.getMonth() + 1);
    $('#hid_day_year').val(yesterday.getFullYear());

    var meses = new Array("Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
    $('#lnk_day').html("<div class='fc-button-group'><button type='button' onclick='search_day_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_day_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + yesterday.getDate() + " " + meses[yesterday.getMonth()] + " " + yesterday.getFullYear() + "</span>");
    search_table_day(yesterday.getDate(), yesterday.getMonth() + 1, yesterday.getFullYear());
}
function search_day_sig() {
    var day = $('#hid_day').val();
    var month = $('#hid_day_month').val();
    var year = $('#hid_day_year').val();

    var fecha = new Date(year, month - 1, day);
    var tomorrow = new Date(fecha.getTime() + 24 * 60 * 60 * 1000);

    $('#hid_day').val(tomorrow.getDate());
    $('#hid_day_month').val(tomorrow.getMonth() + 1);
    $('#hid_day_year').val(tomorrow.getFullYear());

    var meses = new Array("Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
    $('#lnk_day').html("<div class='fc-button-group'><button type='button' onclick='search_day_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_day_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + tomorrow.getDate() + " " + meses[tomorrow.getMonth()] + " " + tomorrow.getFullYear() + "</span>");
    search_table_day(tomorrow.getDate(), tomorrow.getMonth() + 1, tomorrow.getFullYear());
}
function search_table_day(day, month, year) {
    $('#wait_modal').modal('show');

    $.ajax({
        url: 'informe-leads-crm.aspx/search_table_day',
        data: "{ 'day': '" + day + "', 'month': '" + month + "', 'year': '" + year + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#table_listado_leads_day').html('');
            $('#table_listado_leads_day').html(data.d);
            $('#wait_modal').modal('hide');
            paint_table_day();
        },
        error: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        },
        failure: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        }
    });
}

// --------------------------------------------------------------------------------------------------
// Funciones para avanazar mes a mes ----------------------------------------------------------------
// --------------------------------------------------------------------------------------------------
function search_month_ant() {
    var month = $('#hid_month').val();
    var year = $('#hid_year').val();

    if (month === '1') {
        $('#hid_month').val(12);
        $('#hid_year').val(parseInt(year) - 1);
    }
    else {
        $('#hid_month').val(parseInt(month) - 1);
        $('#hid_year').val(parseInt(year));
    }

    var meses = new Array("", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
    var _month = parseInt(month) - 1;
    var _year = parseInt(year);
    if (month === '1') {
        _month = 12;
        _year = parseInt(year) - 1;
    }
    $('#lnk_month').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
    search_table_month(_month, _year);
}
function search_month_sig() {
    var month = $('#hid_month').val();
    var year = $('#hid_year').val();

    if (month === '12') {
        $('#hid_month').val(1);
        $('#hid_year').val(parseInt(year) + 1);
    }
    else {
        $('#hid_month').val(parseInt(month) + 1);
        $('#hid_year').val(parseInt(year));
    }

    var meses = new Array("", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
    var _month = parseInt(month) + 1;
    var _year = parseInt(year);
    if (month === '12') {
        _month = 1;
        _year = parseInt(year) + 1;
    }
    $('#lnk_month').html("<div class='fc-button-group'><button type='button' onclick='search_month_ant()' class='fc-prev-button fc-button fc-state-default fc-corner-left' aria-label='prev'><span class='fc-icon fc-icon-left-single-arrow'></span></button><button type='button' onclick='search_month_sig()' class='fc -next-button fc-button fc-state-default fc-corner-right' aria-label='next'><span class='fc-icon fc-icon-right-single-arrow'></span></button></div> <span>" + meses[_month] + " " + _year + "</span>");
    search_table_month(_month, _year);
}
function search_table_month(month, year) {
    $('#wait_modal').modal('show');

    $.ajax({
        url: 'informe-leads-crm.aspx/search_table_month',
        data: "{ 'month': '" + month + "', 'year': '" + year + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#table_listado_leads_month').html('');
            $('#table_listado_leads_month').html(data.d);
            $('#wait_modal').modal('hide');
            paint_table_month();
        },
        error: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        },
        failure: function (response) {
            $('#wait_modal').modal('hide');
            alert(response.responseText);
        }
    });
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