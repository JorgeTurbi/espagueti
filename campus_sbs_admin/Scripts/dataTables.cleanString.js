/// Función para limpiar acentos
jQuery.fn.dataTableExt.oSort['clear-string-asc'] = function (x, y) {
    return clearString(x) > clearString(y) ? 1 : -1;
};

jQuery.fn.dataTableExt.oSort['clear-string-desc'] = function (x, y) {
    return clearString(x) < clearString(y) ? 1 : -1;
};