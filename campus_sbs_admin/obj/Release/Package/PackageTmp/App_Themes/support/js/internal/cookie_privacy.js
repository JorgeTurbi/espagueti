// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para comprobar la cookie --------------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
$(document).ready(function () {
    if (getCookie('AceptacionCookie') !== "1")
        $('#sliding-popup').show();
    else
        $('#sliding-popup').hide();
});

function getCookie(c_name) {
    var c_value = document.cookie;
    var c_start = c_value.indexOf(" " + c_name + "=");
    if (c_start === -1) {
        c_start = c_value.indexOf(c_name + "=");
    }
    if (c_start === -1) {
        c_value = null;
    } else {
        c_start = c_value.indexOf("=", c_start) + 1;
        var c_end = c_value.indexOf(";", c_start);
        if (c_end === -1) {
            c_end = c_value.length;
        }
        c_value = unescape(c_value.substring(c_start, c_end));
        if (c_value.indexOf('&') !== -1)
            c_value = c_value.split('&')[0];
    }
    return c_value;
}
function setCookie(c_name, value, exdays) {
    var sesion = getCookie('session');
    var fecha = getCookie('fecha');
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = "session=" + sesion + "&AceptacionCookie=" + escape(value) + "&fecha=" + fecha + "; domain=.spainbs.com" + (exdays === null ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}
function PonerCookie() {
    setCookie('ShopOnline', '1', 365);
    $('#sliding-popup').hide();
}
function setCookieRedirect() {
    setCookie('ShopOnline', '1', 365);
    location.href = "https://www.spainbs.com/politica-cookies";
}