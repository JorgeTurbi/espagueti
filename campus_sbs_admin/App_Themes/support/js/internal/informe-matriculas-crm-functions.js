// --------------------------------------------------------------------------------------------------
// Funciones que carga al arrancar la página --------------------------------------------------------
// --------------------------------------------------------------------------------------------------
$(function () {
    /// 1.- Cargar los datepicker
    $(".input-group.date").datepicker({
        language: "es",
        autoclose: true,
        todayHighlight: true
    });

    /// 2.- Pintar las tablas
    var table_leads = $('#tabla_List_Matriculas').DataTable({
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
        lengthMenu: [[-1, 20, 50], ["All", 20, 50]],
        "columnDefs": [
          {
              "targets": [0],
              "className": 'details-control'
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
        "order": [[1, "asc"]],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            /// Nº matriculas
            number = api.column(2)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString());
                }, 0);

            /// Update footer
            $(api.column(2).footer()).html(number);

            /// Venta total
            total = api.column(3)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);

            /// Update footer
            $(api.column(3).footer()).html(
                PonerPuntoMil(total.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Ventas programa
            program = api.column(4)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);

            /// Update footer
            $(api.column(4).footer()).html(
                PonerPuntoMil(program.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Fundación
            fundation = api.column(5)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);
            /// Update footer
            $(api.column(5).footer()).html(
                PonerPuntoMil(fundation.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Universidad
            university = api.column(6)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);
            /// Update footer
            $(api.column(6).footer()).html(
                PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Restante
            university = api.column(7)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);
            /// Update footer
            $(api.column(7).footer()).html(
                PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
            );
        }
    });
    $('#tabla_List_Matriculas tbody').on('click', 'td.details-control', function () {
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

    $('#tabla_List_Matriculas_All').DataTable({
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
        lengthMenu: [[-1, 20, 50], ["All", 20, 50]],
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
              "targets": [4]
          },
          {
              "targets": [5],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [6],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [7],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [8],
              "class": "text-center"
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
          }
        ],
        "order": [[0, "asc"]],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            /// Venta total
            total = api.column(9)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);

            /// Update footer
            $(api.column(9).footer()).html(
                PonerPuntoMil(total.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Ventas programa
            program = api.column(10)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);

            /// Update footer
            $(api.column(10).footer()).html(
                PonerPuntoMil(program.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Fundación
            fundation = api.column(11)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);
            /// Update footer
            $(api.column(11).footer()).html(
                PonerPuntoMil(fundation.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Universidad
            university = api.column(12)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);
            /// Update footer
            $(api.column(12).footer()).html(
                PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
            );

            /// Restante
            university = api.column(13)
                .data()
                .reduce(function (a, b) {
                    return parseFloat(a) + parseFloat(b.toString().replace('.', '').replace(',', '.'));
                }, 0);
            /// Update footer
            $(api.column(13).footer()).html(
                PonerPuntoMil(university.toFixed(2).toString().replace('.', ',')) + '€'
            );
        }
    });

    /// 3.- Recuperar la tecla pulsada
    if ($("#txt_search").val() === "") {
        $("#txt_search").focus();
    }
    $("#txt_search").on('keypress', $(this), function (e) {
        if (e.which === 13 && $(this).val() === "") {
            return false;
        }
    });

    /// 4.- Autocompletar
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
// Función para buscar los datos de las subtablas ---------------------------------------------------
// --------------------------------------------------------------------------------------------------            
function search_subtable(id, row, tr) {
    var _date_start = $('#date_start').val();
    var _date_end = $('#date_end').val();
    var _type = $('input:radio[name=radTipo]:checked').val();
    $('#wait_modal').modal('show');

    $.ajax({
        url: 'informe-matriculas-crm.aspx/search_subtable',
        data: "{ 'id': '" + id + "', '_date_start': '" + _date_start + "', '_date_end': '" + _date_end + "', '_type': '" + _type + "'}",
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
            alert(response.responseText);
            $('#wait_modal').modal('hide');
        },
        failure: function (response) {
            alert(response.responseText);
            $('#wait_modal').modal('hide');
        }
    });
}
function paintTableLevel2() {
    $('#tabla_List_Matriculas_level2').DataTable({
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
        lengthMenu: [[-1, 10, 25], ["All", 10, 25]],
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
              "type": "eu_date"
          },
          {
              "targets": [5],
              "class": "text-center",
              "type": "eu_date"
          },
          {
              "targets": [6],
              "class": "text-center",
              "type": "eu_date"
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
          }
        ],
        "order": [[0, "asc"]]
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