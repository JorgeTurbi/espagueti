function calculate_date(fecha) {
    var date = fecha.replace(" ", "");
    if (date === "") {
        var f = new Date(2000, 1, 1);
        date = f.getDate() + "/" + f.getMonth() + "/" + f.getFullYear();
    }

    var eu_date = '';
    if (date.indexOf('.') > 0) {
        /*date a, format dd.mn.(yyyy) ; (year is optional)*/
        eu_date = date.split('.');
    } else {
        /*date a, format dd/mn/(yyyy) ; (year is optional)*/
        eu_date = date.split('/');
    }

    /*year (optional)*/
    var year = 0;
    if (eu_date[2])
        year = eu_date[2];

    /*month*/
    var month = eu_date[1];
    if (month.length === 1)
        month = 0 + month;

    /*day*/
    var day = eu_date[0];
    if (day.length === 1)
        day = 0 + day;

    return (year + month + day) * 1;
}
function calculate_date_eu(date) {
    var x;

    if (date !== '') {
        var frDatea = date.split(' ');
        var frTimea = frDatea[1].split(':');
        var frDatea2 = frDatea[0].split('/');
        x = (frDatea2[2] + (frDatea2[1].length === 1 ? 0 + frDatea2[1] : frDatea2[1]) + (frDatea2[0].length === 1 ? 0 + frDatea2[0] : frDatea2[0]) + (frTimea[0].length === 1 ? 0 + frTimea[0] : frTimea[0]) + (frTimea[1] === 1 ? 0 + frTimea[1] : frTimea[1]) + (frTimea[2].length === 1 ? 0 + frTimea[2] : frTimea[2])) * 1;
    }
    else {
        x = Infinity;
    }

    return x;
}

jQuery.fn.dataTableExt.oSort['eu_date-asc'] = function(a, b) {
	x = calculate_date(a);
	y = calculate_date(b);
	
	return x < y ? -1 : x > y ?  1 : 0;
};

jQuery.fn.dataTableExt.oSort['eu_date-desc'] = function(a, b) {
	x = calculate_date(a);
	y = calculate_date(b);
	
	return x < y ? 1 : x > y ?  -1 : 0;
};

jQuery.fn.dataTableExt.oSort['euro_date-asc'] = function (a, b) {
    x = calculate_date_eu(a);
    y = calculate_date_eu(b);

    return x < y ? -1 : x > y ? 1 : 0;
};

jQuery.fn.dataTableExt.oSort['euro_date-desc'] = function (a, b) {
    x = calculate_date_eu(a);
    y = calculate_date_eu(b);

    return x < y ? 1 : x > y ? -1 : 0;
};