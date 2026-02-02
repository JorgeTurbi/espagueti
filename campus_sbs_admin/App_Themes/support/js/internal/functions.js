// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para desconectar ----------------------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function exit_user() {
    $.ajax({
        type: "POST",
        crossDomain: true,
        url: 'functions.aspx/exit',
        data: "",
        contentType: "application/json; charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            var validate = data.d;
            if (validate)
                location.href = "login.aspx";
            else
                alert("Se ha producido un error al desconectarse.");
        },
        error: function (response) {
            return false;
        },
        failure: function (response) {
            return false;
        }
    });
}
// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para buscar parámetros en la url ------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function getParams(param) {
    var regexS = "[\\?&]" + param + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var tmpURL = window.location.href;
    var results = regex.exec(tmpURL);
    if (results === null)
        return "";
    else
        return results[1];
}
/// -------------------------------------------------------------------------------------------------------------------------------------
/// Función para guardar los datos de la subida de un fichero ---------------------------------------------------------------------------
/// -------------------------------------------------------------------------------------------------------------------------------------
function getFileData() {
    var file_data = window.sessionStorage.getItem("file_data");
    if (file_data === "undefined" || file_data === undefined || file_data === "null" || file_data === null)
        file_data = "";
    return file_data;
}
function setFileData(file_data) {
    window.sessionStorage.setItem("file_data", file_data);
}
function deleteFileData() {
    window.sessionStorage.removeItem("file_data");
}
// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para transformar el tamaño de un fichero ----------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function formatFileSize(bytes) {
    if (typeof bytes !== 'number')
        return '';
    if (bytes >= 1000000000)
        return (bytes / 1000000000).toFixed(2) + ' GB';
    if (bytes >= 1000000)
        return (bytes / 1000000).toFixed(2) + ' MB';
    return (bytes / 1000).toFixed(2) + ' KB';
}
// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para validar campos -------------------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function validarCarateresEspeciales(textoValidar) {
    //var characterReg = /[`~!#$%^&*()°¬|+\-=?¿¡;:'",.<>\{\}\[\]\\\/]/gi;
    var characterReg = /[`~$%^*()°¬|\=<>\{\}\[\]\\\/]/gi;
    if (characterReg.test(textoValidar))
        return false;
    else
        return true;
}
function validarMail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!expr.test(email))
        return false;
    else
        return true;
}
function validarTelefono(phone) {
    if (isNaN(phone))
        return false;
    else
        return true;
}
// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para confirmar algo -------------------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function confirm_message(message) {
    return confirm(message);
}

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para poner una elipsis en un texto grande ---------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function ellipsis(selector) {
    var nodeList = document.querySelectorAll(selector);
    arrNodes = [].slice.call(nodeList);
    for (var i in arrNodes) {
        var n = arrNodes[i];
        while (n.scrollHeight - n.offsetHeight > 0) {
            var text = n.innerText !== undefined ? n.innerText : n.textContent;
            if (n.innerText !== undefined) {
                n.innerText = text.replace(/\W*\s(\S)*$/, '...');
            }
            else {
                // Para Firefox
                n.textContent = text.replace(/\W*\s(\S)*$/, '...');
            }
        }
    }
}

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para poner una subir a la cabecera de una página --------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function subirArribaPagina() {
    var body = $("html, body");
    body.stop().animate({ scrollTop: 0 }, 500, 'swing', function () {
        //alert("Hecho");
    });
}

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para limpiar acentos ------------------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function clearString(str) {
    return str.toLowerCase()
        .replace(/[áãà]/g, 'a')
        .replace(/é/g, 'e')
        .replace(/í/g, 'i')
        .replace(/[óõô]/g, 'o')
        .replace(/[úü]/g, 'u')
        .replace(/ç/g, 'c');
}

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Función para poner el punto de los miles en las tablas ----------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function PonerPuntoMil(valor) {
    var PuntoMil = "";
    var PuntoMil1 = "";
    var PuntoMil2 = "";
    var PuntoMil3 = "";
    var val = valor.toString();

    if (val.indexOf(',') != -1) {
        var Punto = val.split(',')[0];
        var num = Punto.length;
        if (num >= 4) {
            if (num >= 7) {
                PuntoMil1 = Punto.substring(0, (num - 6));
                PuntoMil2 = Punto.substring((num - 6), 4);
                PuntoMil3 = Punto.substring((num - 3));
                PuntoMil = PuntoMil1 + "." + PuntoMil2 + "." + PuntoMil3 + "," + val.split(',')[1];
            }
            else {
                PuntoMil1 = Punto.substring(0, (num - 3));
                PuntoMil2 = Punto.substring((num - 3));
                PuntoMil = PuntoMil1 + "." + PuntoMil2 + "," + val.split(',')[1];
            }
        }
        else
            PuntoMil = valor;
    }
    else {
        var num2 = val.length;
        if (num2 >= 4) {
            if (num2 >= 7) {
                PuntoMil1 = Punto.substring(0, (num2 - 6));
                PuntoMil2 = Punto.substring((num2 - 6), 4);
                PuntoMil3 = Punto.substring((num2 - 3));
                PuntoMil = PuntoMil1 + "." + PuntoMil2 + "." + PuntoMil3;
            }
            else {
                PuntMil1 = val.substring(0, (num2 - 3));
                PuntMil2 = val.substring((num2 - 3));
                PuntoMil = PuntMil1 + "." + PuntMil2;
            }
        }
        else
            PuntoMil = val;
    }
    return PuntoMil;
}

// -----------------------------------------------------------------------------------------------------------------------------------------------------
// Funciones para borrar la fecha ----------------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------------------------------------------------
function clean_input(data) {
    $("#" + data).val('');
}