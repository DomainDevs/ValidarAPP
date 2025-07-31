var separatorDecimal = ',';

//////////////////////////////////////////////////
/// Función para obtener la fecha del sistema ///
/////////////////////////////////////////////////
function GetCurrentDate() {

    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();

    if (month < 10) {
        month = '0' + month;
    }

    if (day < 10) {
        day = '0' + day;
    }

    return (day + "/" + month + "/" + year);
}

///////////////////////////////////////////////////////////////
/// Función para obtener la fecha y hora actual del sistema ///
///////////////////////////////////////////////////////////////
function GetCurrentDateTime() {
    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();
    var hour = currentDate.getHours();
    var minute = currentDate.getMinutes();
    var second = currentDate.getSeconds();

    if (month < 10) {
        month = '0' + month;
    }

    if (day < 10) {
        day = '0' + day;
    }

    if (hour < 10) {
        hour = '0' + hour;
    }

    if (minute < 10) {
        minute = '0' + minute;
    }

    return (day + "/" + month + "/" + year + " " + hour + ":" + minute);
}

/////////////////////////////////////////////////////////////////////////////
/// Función para convertir una fecha en milisegundos a formato dd/mm/yyyy ///
/////////////////////////////////////////////////////////////////////////////
function ChangeFormatDate(currentDate) {
    day = currentDate.getDate();
    if (day < 10) day = '0' + day;
    month = currentDate.getMonth();
    month = (parseInt(month) + 1);
    if (month < 10) month = '0' + month;
    year = currentDate.getFullYear();
    dateFormat = day + '/' + month + '/' + year;
    return dateFormat;
}

////////////////////////////////////
/// Función para comparar fechas ///
////////////////////////////////////
function CompareDates(dateFrom, dateTo) {
    var monthFrom = dateFrom.substring(3, 5);
    var dayFrom = dateFrom.substring(0, 2);
    var yearFrom = dateFrom.substring(6, 10);

    var monthTo = dateTo.substring(3, 5);
    var dayTo = dateTo.substring(0, 2);
    var yearTo = dateTo.substring(6, 10);

    if (yearFrom > yearTo) {
        return (true);
    }
    else {
        if (yearFrom == yearTo) {
            if (monthFrom > monthTo) {
                return (true);
            }
            else {
                if (monthFrom == monthTo) {
                    if (dayFrom > dayTo) {
                        return (true);
                    }
                    else {
                        return (false);
                    }
                }
                else {
                    return (false);
                }
            }
        }
        else {
            return (false);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
/// Función para validar caracteres especiales, puede ingresar el espacio ///
/////////////////////////////////////////////////////////////////////////////
function ValidateSpecialCharacter(event) {
    if (event.which != 32 && event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        //if (event.which < 48 || event.which > 57) {//Números
        if (event.which < 65 || event.which > 90) {
            if (event.which < 97 || event.which > 122) {
                if (event.which != 209 && event.which != 241)
                    event.preventDefault();
            }
        }
        //}
    }
}

///////////////////////////////////////////
/// Función para convertir a mayúsculas ///
///////////////////////////////////////////
function ToUpper(id) {
    try {
        if ($("#" + id).val() != undefined) {
            $("#" + id).val($("#" + id).val().toUpperCase());
        }
    } catch (e) {

    }
}

////////////////////////////////////////////////
/// Función para quitar el formato de moneda ///
////////////////////////////////////////////////
function ClearFormatCurrency(id) {
    var field = id;
    field = field.replace("$", "");
    field = field.replace(new RegExp(',', 'gi'), "");

    return field;
}

////////////////////////////////////////
/// Función para formatear la moneda ///
////////////////////////////////////////
function FormatCurrency(id) {
    var field;
    field = "$ " + id;
    return field;
}

/////////////////////////////////////////
/// Función para formatear un decimal ///
/////////////////////////////////////////
//function FormatDecimal(id) {

//    var number = new ObjectNumber(id);
//    return number.format(2, true);
//}

function FormatDecimal(id) {
    //var valor = $("#" + id).val();
    var numero = new oNumero(id);
    return numero.formato(2, true);
}

function oNumero(numero) {
    //Propiedades 
    this.valor = numero || 0;
    this.dec = -1;
    //Métodos 
    this.formato = numFormat;
    this.ponValor = ponValor;
    //Definición de los métodos 
    function ponValor(cad) {
        if (cad == '-' || cad == '+') { return; }
        if (cad.length == 0) { return; }
        if (cad.indexOf('.') >= 0) {
            this.valor = parseFloat(cad);
        } else {
            this.valor = parseInt(cad);
        }
    }

    function numFormat(dec, miles) {
        var num = this.valor, signo = 3, expr;
        var cad = "" + this.valor;
        var ceros = "", pos, pdec, i;
        for (i = 0; i < dec; i++) {
            ceros += '0';
        }
        pos = cad.indexOf('.');
        if (pos < 0) {
            cad = cad + "." + ceros;
        }
        else {
            pdec = cad.length - pos - 1;
            if (pdec <= dec) {
                for (i = 0; i < (dec - pdec); i++) {
                    cad += '0';
                }
            }
            else {
                num = num * Math.pow(10, dec);
                num = Math.round(num);
                num = num / Math.pow(10, dec);
                cad = new String(num);
            }
        }
        pos = cad.indexOf('.');
        if (pos < 0) { pos = cad.lentgh; }
        if (cad.substr(0, 1) == '-' || cad.substr(0, 1) == '+') {
            signo = 4;
        }
        if (miles && pos > signo) {
            do {
                expr = new RegExp('([+-]?\\d)(\\d{3}[\\.,]\\d*)');
                cad.match(expr);
                cad = cad.replace(expr, RegExp.$1 + ',' + RegExp.$2);
            }
            while (cad.indexOf(',') > signo);
        }
        if (dec < 0) { cad = cad.replace(new RegExp('\\.'), '') }
        return cad;
    }

}

/////////////////////////////////////////
/// Función para formatear un decimal ///
/////////////////////////////////////////
function ObjectNumber(number) {
    //Propiedades 
    this.value = number || 0
    this.dec = -1;
    //Métodos 
    this.format = numberFormat;
    this.setValue = setValue;
    //Definición de los métodos 
    function setValue(cad) {
        if (cad == '-' || cad == '+') return
        if (cad.length == 0) return
        if (cad.indexOf('.') >= 0) {
            this.value = parseFloat(cad);
        } else {
            this.value = parseInt(cad);
        }
    }


    function numberFormat(dec, miles) {
        var number = this.value, sign = 3, expr;
        var cad = "" + this.valor;
        var zeros = "", pos, pdec, i;
        for (i = 0; i < dec; i++) {
            zeros += '0';
        }
        pos = cad.indexOf('.');
        if (pos < 0) {
            cad = cad + "." + zeros;
        }
        else {
            pdec = cad.length - pos - 1;
            if (pdec <= dec) {
                for (i = 0; i < (dec - pdec); i++)
                    cad += '0';
            }
            else {
                number = number * Math.pow(10, dec);
                number = Math.round(num);
                number = number / Math.pow(10, dec);
                cad = new String(number);
            }
        }
        pos = cad.indexOf('.');
        if (pos < 0) { pos = cad.lentgh; }
        if (cad.substr(0, 1) == '-' || cad.substr(0, 1) == '+') {
            sign = 4;
        }
        if (miles && pos > sign) {
            do {
                expr = new RegExp('([+-]?\\d)(\\d{3}[\\.,]\\d*)');
                cad.match(expr);
                cad = cad.replace(expr, RegExp.$1 + ',' + RegExp.$2);
            }
            while (cad.indexOf(',') > sign);
        }
        if (dec < 0) { cad = cad.replace(new RegExp('\\.'), '') }
        return cad;
    }

}

///////////////////////////////////////////////////////////////////////////////
/// Función para verificar si es una fecha valida, en el formato dd/mm/yyyy ///
///////////////////////////////////////////////////////////////////////////////
function IsDate(currentDate) {


    var result;
    var currentDay;
    var currentMonth;
    var currentYear;
    var leapYear;

    if (currentDate.length < 10) { return (false); }

    if (currentDate.substring(0, 1) == "/" || currentDate.substring(1, 2) == "/" || currentDate.substring(3, 4) == "/"
        || currentDate.substring(4, 5) == "/" || currentDate.substring(6, 7) == "/" || currentDate.substring(7, 8) == "/" ||
        currentDate.substring(8, 9) == "/" || currentDate.substring(9, 10) == "/") {
        return (false);
    }

    if (currentDate.substring(2, 3) != "/" || currentDate.substring(1, 2) == "/" || currentDate.substring(5, 6) != "/") {
        return (false);
    }

    currentDay = currentDate.substring(0, 2);
    currentMonth = currentDate.substring(3, 5);
    currentYear = currentDate.substring(6, 10);

    result = ValidateDate(currentDay);
    if (result >= 1) { // Si existe error
        return false;
    }

    result = ValidateDate(currentMonth);
    if (result >= 1) { // Si existe error
        return false;
    }
    result = ValidateDate(currentYear);
    if (result >= 1) { // Si existe error
        return false;
    }

    currentDay = eval(currentDay);
    currentMonth = eval(currentMonth);
    currentYear = eval(currentYear);

    if (currentYear < 1900 || currentYear > 3000) {
        return (false);
    }

    if (currentYear % 4 == 0) {
        leapYear = 1;
    }
    else {
        leapYear = 0;
    }

    switch (currentMonth) {

        case 1:
            if (currentDay < 1 || currentDay > 31) { return (false); }
            break;
        case 2:
            if (currentDay < 1 || currentDay > (28 + leapYear)) { return (false); }
            break;
        case 3:
            if (currentDay < 1 || currentDay > 31) { return (false); }
            break;
        case 4:
            if (currentDay < 1 || currentDay > 30) { return (false); }
            break;
        case 5:
            if (currentDay < 1 || currentDay > 31) { return (false); }
            break;
        case 6:
            if (currentDay < 1 || currentDay > 30) { return (false); }
            break;
        case 7:
            if (currentDay < 1 || currentDay > 31) { return (false); }
            break;
        case 8:
            if (currentDay < 1 || currentDay > 31) { return (false); }
            break;
        case 9:
            if (currentDay < 1 || currentDay > 30) { return (false); }
            break;
        case 10:
            if (currentDay < 1 || currentDay > 31) { return (false); }
            break;
        case 11:
            if (currentDay < 1 || currentDay > 30) { return (false); }
            break;
        case 12:
            if (currentDay < 1 || currentDay > 31) { return (false); }
            break;
        default:
            if (currentMonth < 1 || currentMonth > 12) { return (false); }
    }

    return (true);
}

//////////////////////////////////////
/// Función para validar una fecha ///
//////////////////////////////////////
function ValidateDate(currentDate) {
    var valid = "0123456789";
    var temp;
    for (var i = 0; i < currentDate.length; i++) {
        temp = currentDate.substring(i, i + 1);
        if (valid.indexOf(temp) < 0) {
            return 1;
        }
    }
    return -1;
}

//////////////////////////////////////
/// String.Format                  ///
//////////////////////////////////////
String.prototype.format = function () {
    var pattern = new RegExp('\\{\\d+\\}', 'g');
    var args = arguments;
    return this.replace(pattern, function (capture) { return args[capture.match(new RegExp('\\d+'))]; });
}

/////////////////////////////////////////////////////////////////////
/// Kindly borrowed from http://phpjs.org/functions/number_format ///
/////////////////////////////////////////////////////////////////////
number_format = function (number, decimals, dec_point, thousands_sep) {
    number = (number + '').replace(new RegExp('[^0-9+\\-Ee.]', 'g'), '');
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return '' + Math.round(n * k) / k;
        };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(new RegExp('\\B(?=(?:\\d{3})+(?!\\d))', 'g'), sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}

/////////////////////////////////////////////////////
/// Función que permite ingresar solo dígitos 0-9 ///
/////////////////////////////////////////////////////
function JustNumbers(e) {

    var keynum = window.event ? window.event.keyCode : e.which;
    if ((keynum == 8)) {
        return true;
    }

    if ((keynum == 0) || (keynum == 13)) {
        $(tb).keypress(enterTab);
        return true;
    }
    return (new RegExp('\\d')).test(String.fromCharCode(keynum));
}

///////////////////////////////////////////////////
/// Función que permite capturar la tecla enter ///
///////////////////////////////////////////////////
function enterTab(e) {
    if (e.keyCode == 13) {
        cb = parseInt($(this).attr('tabindex'));

        if ($(':input[tabindex=\'' + (cb + 1) + '\']') != null) {
            $(':input[tabindex=\'' + (cb + 1) + '\']').focus();
            $(':input[tabindex=\'' + (cb + 1) + '\']').select();
            e.preventDefault();

            return false;
        }
    }
}

//////////////////////////////////////////////////////////
/// Función que permite solo números con dos decimales ///
//////////////////////////////////////////////////////////
function OnlyDecimalNumber(e, field) {
    key = e.keyCode ? e.keyCode : e.which;

    if (field.value == "") { return true; }

    var regex = new RegExp('^(-)?(\\d+)(\\.(\\d){0,2})?$');
    var selectedText = window.getSelection().toString();
    if (selectedText !== '') {
        if (field.value.includes(selectedText) === true) {
            return regex.test(e.key);
        }
    }
    
    // Recibe el punto, eliminar y números
    if (key == 46 || key == 8 || (key >= 48 && key <= 57)) {
        return regex.test(field.value + e.key);
    }
    return false;

    /*key = e.keyCode ? e.keyCode : e.which;

    if (key == 46) {
        if (field.value == "") { return false }
        regexp = new RegExp('^[0-9]+$');
        return regexp.test(field.value);
    }

    // 0-9 a partir del .decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida dos digitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") { return true; }
                //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/
                regexp = new RegExp('[0-9]{2}$');
                return !(regexp.test(field.value));
            }
        }
    }

    // backspace
    if (key == 8 || (key > 47 && key < 58)) {
        if (field.value == "") { return true; }
        regexp = new RegExp('[0-9]{18}');
        //regexp = /[0-9]{10}^[0-9]+$/
        return !(regexp.test(field.value));
    }

    // 0-9 a partir del .decimal  
    if (field.value != "") {
        if ((field.value.indexOf(".")) > 0) {
            //si tiene un punto valida dos digitos en la parte decimal
            if (key > 47 && key < 58) {
                if (field.value == "") { return true; }
                //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/
                regexp = new RegExp('[0-9]{2}$');
                return !(regexp.test(field.value));
            }
        }
    }

    // other key
    return false;*/
}

////////////////////////////////////////////////////////////////////////////////
/// Función que permite solo números positivos y negativos con dos decimales ///
////////////////////////////////////////////////////////////////////////////////
function OnlyNumbers(e, field) {
    // capturamos la tecla pulsada
    var keyPressed = window.event ? window.event.keyCode : e.which;

    // capturamos el contenido del input
    var value = field.value;

    // 45 = tecla simbolo menos (-)
    // Si el usuario pulsa la tecla menos, y no se ha pulsado anteriormente
    // Modificamos el contenido del mismo añadiendo el simbolo menos al
    // inicio
    if (keyPressed == 45 && value.indexOf("-") == -1) {
        field.value = "-" + value;
    }

    // 13 = tecla enter
    // 46 = tecla punto (.)
    // Si el usuario pulsa la tecla enter o el punto y no hay ningun otro
    // punto
    if (keyPressed == 13 || (keyPressed == 46 && value.indexOf(".") == -1)) {
        return true;
    }

    // devolvemos true o false dependiendo de si es numerico o no
    return (new RegExp('\\d')).test(String.fromCharCode(keyPressed));
}

/******************************************************************/
/** FUNCION PARA RESALTAR TEXTO ENCONTRADO EN LA BUSQUEDA        **     
 ** Utilizada en todos los parciales módulo de ayuda             **
******************************************************************/
//jQuery.fn.extend({

//    highlight: function (search, searchClassCss) {

//        var regex = new RegExp("(<[^>]*>)|(" + search.replace(/([-.*+?^${}()|[\]\/\\])/g, "\\$1") + ')', 'ig');
//        var newHtml = this.html(this.html().replace(regex, function (a, b, c) {
//            return (a.charAt(0) == "<") ? a : "<span class=\"" + searchClassCss + "\">" + c + "</span>";
//        }));
//        return newHtml;
//    }
//});

//////////////////////////////////////////////
///
//////////////////////////////////////////////
function highlightWord(text, div, style) {

    var a = "á";    //225
    var e = "é";    //233
    var i = "í";    //237
    var o = "ó";    //243 
    var enie = "ñ"; //241
    var u = "ú";    //250

    if (text.indexOf("&#225;") > 0) {
        text = text.replace("&#225;", a);
    }
    if (text.indexOf("&#233;") > 0) {
        text = text.replace("&#233;", e);
    }
    if (text.indexOf("&#237;") > 0) {
        text = text.replace("&#237;", i);
    }
    if (text.indexOf("&#243;") > 0) {
        text = text.replace("&#243;", o);
    }
    if (text.indexOf("&#250;") > 0) {
        text = text.replace("&#250;", u);
    }
    if (text.indexOf("&#241;") > 0) {
        text = text.replace("&#241;", enie);
    }

    $("#" + div).highlight(text, style);
}

////////////////////////////////////////////
/// Función que valida fecha desde/hasta ///
////////////////////////////////////////////
function CompareDateHour(dateFrom, dateTo) {

    //Fecha Desde
    var _dateFrom = new Date();
    var dayEnteredOcurrence = dateFrom.substr(0, 2);
    var monthEnteredOcurrence = dateFrom.substr(3, 2);
    var yearEnteredOcurrence = dateFrom.substr(6, 4);
    var hourFrom = dateFrom.substr(11, 2);
    var minFrom = dateFrom.substr(14, 2);
    _dateFrom.setMonth(monthEnteredOcurrence - 1);
    _dateFrom.setDate(dayEnteredOcurrence);
    _dateFrom.setYear(yearEnteredOcurrence);
    _dateFrom.setHours(hourFrom, minFrom);


    //Fecha To
    var _dateTo = new Date();
    var dayEntered = dateTo.substr(0, 2);
    var monthEntered = dateTo.substr(3, 2);
    var yearEntered = dateTo.substr(6, 4);
    var hourTo = dateTo.substr(11, 2);
    var minTo = dateTo.substr(14, 2);
    _dateTo.setMonth(monthEntered - 1);
    _dateTo.setDate(dayEntered);
    _dateTo.setYear(yearEntered);
    _dateTo.setHours(hourTo, minTo);

    if (_dateTo < _dateFrom) {
        return false;
    }
    else {
        return true;
    }
}

///////////////////////////////////////////////////////////////////////
/// Función que permite validar la fecha de reverso de conciliación ///
///////////////////////////////////////////////////////////////////////
function ValidateReverseDate(conciliationDate, newDate) {

    //Fecha contable
    var startAccountingDate = new Date();
    var accountingDate = new Date();
    var accountingDay = conciliationDate.substr(0, 2);
    var accountingMonth = conciliationDate.substr(3, 2);
    var accountingYear = conciliationDate.substr(6, 4);

    accountingDate.setMonth(accountingMonth - 1);
    accountingDate.setDate(accountingDay);
    accountingDate.setYear(accountingYear);

    startAccountingDate.setMonth(accountingMonth - 1);
    startAccountingDate.setDate("01");
    startAccountingDate.setYear(accountingYear);

    //Fecha conciliación
    var conciliatonDate = new Date();
    var conciliationDay = newDate.substr(0, 2);
    var conciliationMonth = newDate.substr(3, 2);
    var conciliationYear = newDate.substr(6, 4);

    conciliatonDate.setMonth(conciliationMonth - 1);
    conciliatonDate.setDate(conciliationDay);
    conciliatonDate.setYear(conciliationYear);

    //Fecha Hoy
    var dateNow = new Date();
    var dateSystem = GetCurrentDate();
    var dayNow = dateSystem.substr(0, 2);
    var monthNow = dateSystem.substr(3, 2);
    var yearNow = dateSystem.substr(6, 4);

    dateNow.setMonth(monthNow - 1);
    dateNow.setDate(dayNow);
    dateNow.setYear(yearNow);

    if (conciliatonDate.toISOString().slice(0, 10) == accountingDate.toISOString().slice(0, 10)) {
        return true;
    }
    else if (conciliatonDate.toISOString().slice(0, 10) > accountingDate.toISOString().slice(0, 10)) {
        return false;
    }
    else if (conciliatonDate.toISOString().slice(0, 10) < startAccountingDate.toISOString().slice(0, 10) ||
        conciliatonDate.toISOString().slice(0, 10) > accountingDate.toISOString().slice(0, 10)) {
        return false;
    }
    else if (conciliatonDate.toISOString().slice(0, 10) >= startAccountingDate.toISOString().slice(0, 10) &&
        conciliatonDate.toISOString().slice(0, 10) <= accountingDate.toISOString().slice(0, 10)) {
        return true;
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////
/// Función que permite validar que la fecha de conciliación este dentro de una fecha contable ///
//////////////////////////////////////////////////////////////////////////////////////////////////
function ValidateConciliationDate(conciliationDate, newDate) {

    //Fecha contable
    var startAccountingDate = new Date();
    var accountingDate = new Date();
    var accountingDay = conciliationDate.substr(0, 2);
    var accountingMonth = conciliationDate.substr(3, 2);
    var accountingYear = conciliationDate.substr(6, 4);

    accountingDate.setMonth(accountingMonth - 1);
    accountingDate.setDate(accountingDay);
    accountingDate.setYear(accountingYear);

    startAccountingDate.setMonth(accountingMonth - 1);
    startAccountingDate.setDate("01");
    startAccountingDate.setYear(accountingYear);

    //Fecha conciliación
    var conciliatonDate = new Date();
    var conciliationDay = newDate.substr(0, 2);
    var conciliationMonth = newDate.substr(3, 2);
    var conciliationYear = newDate.substr(6, 4);

    conciliatonDate.setMonth(conciliationMonth - 1);
    conciliatonDate.setDate(conciliationDay);
    conciliatonDate.setYear(conciliationYear);

    //Fecha Hoy
    var dateNow = new Date();
    var dateSystem = GetCurrentDate();
    var dayNow = dateSystem.substr(0, 2);
    var monthNow = dateSystem.substr(3, 2);
    var yearNow = dateSystem.substr(6, 4);

    dateNow.setMonth(monthNow - 1);
    dateNow.setDate(dayNow);
    dateNow.setYear(yearNow);

    if (conciliatonDate.toISOString().slice(0, 10) == accountingDate.toISOString().slice(0, 10)) {
        return true;
    }
    else if (conciliatonDate.toISOString().slice(0, 10) > accountingDate.toISOString().slice(0, 10)) {
        return false;
    }
    else if (conciliatonDate.toISOString().slice(0, 10) < startAccountingDate.toISOString().slice(0, 10) ||
        conciliatonDate.toISOString().slice(0, 10) > accountingDate.toISOString().slice(0, 10)) {
        return false;
    }
    else if (conciliatonDate.toISOString().slice(0, 10) >= startAccountingDate.toISOString().slice(0, 10) &&
        conciliatonDate.toISOString().slice(0, 10) <= accountingDate.toISOString().slice(0, 10)) {
        return true;
    }
}

/////////////////////////////////////////////////////////////////////////////////////
/// Función que permite validar que la fecha hasta no sea mayor a la fecha actual ///
/////////////////////////////////////////////////////////////////////////////////////
function ValidateDateTo(dateTo) {
    //Fecha hasta
    var dateToDate = new Date();
    var dateToDay = dateTo.substr(0, 2);
    var dateToMonth = dateTo.substr(3, 2);
    var dateToYear = dateTo.substr(6, 4);

    dateToDate.setMonth(dateToMonth - 1);
    dateToDate.setDate(dateToDay);
    dateToDate.setYear(dateToYear);

    //Fecha Hoy
    var dateNow = new Date();
    var dateSystem = GetCurrentDate();
    var dayNow = dateSystem.substr(0, 2);
    var monthNow = dateSystem.substr(3, 2);
    var yearNow = dateSystem.substr(6, 4);

    dateNow.setMonth(monthNow - 1);
    dateNow.setDate(dayNow);
    dateNow.setYear(yearNow);

    if (dateToDate > dateNow) {
        return false;
    }
    else {
        return true;
    }
}

////////////////////////////////////////////////////////////////
/// Función que permite redondear a dos decimales un importe ///
////////////////////////////////////////////////////////////////
function decimalAdjust(type, value, exp) {
    // Si el exp no está definido o es cero...
    if (typeof exp === 'undefined' || +exp === 0) {
        return Math[type](value);
    }
    value = +value;
    exp = +exp;
    // Si el valor no es un número o el exp no es un entero...
    if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
        return NaN;
    }
    // Shift
    value = value.toString().split('e');
    value = Math[type](+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
    // Shift back
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
}

/////////////////////////////////////////
// Formatear un número decimal         //
/////////////////////////////////////////
function NumberFormatDecimal(number, decimals, decimalSeparator, thousandsSeparator) {
    var parts, array;

    if (!isFinite(number) || isNaN(number = parseFloat(number))) {
        return "";
    }
    if (typeof decimalSeparator === "undefined") {
        decimalSeparator = ",";
    }
    if (typeof thousandsSeparator === "undefined") {
        thousandsSeparator = "";
    }

    //Redondeamos
    if (!isNaN(parseInt(decimals))) {
        if (decimals >= 0) {
            number = number.toFixed(decimals);
        }
        else {
            number = (
                Math.round(number / Math.pow(10, Math.abs(decimals))) * Math.pow(10, Math.abs(decimals))
            ).toFixed();
        }
    }
    else {
        number = number.toString();
    }

    //Damos formato
    parts = number.split(".", 2);
    array = parts[0].split("");
    for (var i = array.length - 3; i > 0 && array[i - 1] !== "-"; i -= 3) {
        array.splice(i, 0, thousandsSeparator);
    }
    number = array.join("");

    if (parts.length > 1) {
        number += decimalSeparator + parts[1];
    }

    return number;
}

////////////////////////////////////////////////
// Función para obtener la fecha del sistema ///
////////////////////////////////////////////////
function getCurrentDate() {

    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();

    if (month < 10)
        month = '0' + month;

    if (day < 10)
        day = '0' + day;

    return (day + "/" + month + "/" + year);
}

//////////////////////////////////
// Función para comparar fechas //
//////////////////////////////////
function compare_dates(fecha, fecha2) {

    var xMonth = fecha.substring(3, 5);
    var xDay = fecha.substring(0, 2);
    var xYear = fecha.substring(6, 10);
    var yMonth = fecha2.substring(3, 5);
    var yDay = fecha2.substring(0, 2);
    var yYear = fecha2.substring(6, 10);
    if (xYear > yYear) {
        return (true);
    }
    else {
        if (xYear == yYear) {
            if (xMonth > yMonth) {
                return (true);
            }
            else {
                if (xMonth == yMonth) {
                    if (xDay > yDay) {
                        return (true);
                    }
                    else {
                        return (false);
                    }
                }
                else {
                    return (false);
                }
            }
        }
        else {
            return (false);
        }
    }
}

//////////////////////////////////////////////////////////////////
// Permite calcular el total dependiendo del tipo de aplicación //
//////////////////////////////////////////////////////////////////
function SetMainTotalControl(amount, debit, credit, type) {

    var total = 0;

    debit = decimalAdjust('round', debit, -2);
    credit = decimalAdjust('round', credit, -2);

    if (type == 1) { //Orden de Pago
        //total = amount + credit - debit;
        total = (amount - debit) + credit; //Segun EF _CO_INT_0613_EF_REQ_NBIL_Orden de Pago.doc pag 15
    }
    else if (type == 2) { //Recibo de caja
        total = amount - credit + debit;
    }
    else if (type == 3) { //Asiento de diario y preliquidaciones
        total = debit - credit;
    }

    return total;
}

////////////////////////////////////////////////////////////
// Permite reemplazar la coma o el punto en campo importe //
////////////////////////////////////////////////////////////
function ReplaceDecimalPoint(amount) {
    if (separatorDecimal == ",") {
        amount = amount.toString().replace(new RegExp('\\.', 'g'), ",");
    }
    else {
        amount = amount.toString().replace(new RegExp('\\,', 'g'), ".");
    }

    return amount;
}

/**
    *  Añade o resta un número de días, a una fecha determinada
    *  mostrarFecha(-10) => restara 10 dias a la fecha actual
    *  mostrarFecha(30) => añadira 30 dias a la fecha actual
    * 
    * @param {datetime} dateStart - Fecha inicio.
    * @param {number} days        - Número de días.
    *
    * @returns {number} número de días
    */
function addDaysToDate(dateStart, days) {

    var dateResult = "";

    //se construye una fecha válida
    var xMonth = dateStart.substring(3, 5);
    var xDay = dateStart.substring(0, 2);
    var xYear = dateStart.substring(6, 10);

    var dateFormatValid = xYear + "-" + xMonth + "-" + xDay;

    var dateParse = Date.parse(dateFormatValid);
    dateParse = dateParse + 86400000;

    var fecha = new Date(dateParse);

    //Obtenemos los milisegundos desde media noche del 1/1/1970
    var tiempo = fecha.getTime();
    //Calculamos los milisegundos sobre la fecha que hay que sumar o restar...

    var milisegundos = parseInt(days * 24 * 60 * 60 * 1000);
    //Modificamos la fecha actual

    fecha.setTime(tiempo + milisegundos);

    var day = fecha.getDate();
    var month = fecha.getMonth() + 1;
    var year = fecha.getFullYear();

    //Completa los 10 caracteres de la fecha de ser necesario
    if (day < 10) {
        day = "0" + day;
    }

    if (month < 10) {
        month = "0" + month;
    }

    dateResult = day + "/" + month + "/" + year;

    return dateResult;

}

//////////////////////////////////////////////////
// Da formato a un número para su visualización //
//////////////////////////////////////////////////
function NumberFormatSearch(number, decimals, decimalSeparator, thousandsSeparator) {

    var parts, array;

    if (!isFinite(number) || isNaN(number = parseFloat(number))) {
        return "";
    }
    if (typeof decimalSeparator === "undefined") {
        decimalSeparator = ",";
    }
    if (typeof thousandsSeparator === "undefined") {
        thousandsSeparator = "";
    }

    // Redondeamos
    if (!isNaN(parseInt(decimals))) {
        if (decimals >= 0) {
            number = number.toFixed(decimals);
        } else {
            number = (
                Math.round(number / Math.pow(10, Math.abs(decimals))) * Math.pow(10, Math.abs(decimals))
            ).toFixed();
        }
    } else {
        number = number.toString();
    }

    // Damos formato
    parts = number.split(".", 2);
    array = parts[0].split("");
    for (var i = array.length - 3; i > 0 && array[i - 1] !== "-"; i -= 3) {
        array.splice(i, 0, thousandsSeparator);
    }
    number = array.join("");

    if (parts.length > 1) {
        number += decimalSeparator + parts[1];
    }

    return number;
}

//////////////////////////////////////////////////
// Da formato a un número para su visualización //
//////////////////////////////////////////////////
function NumberFormat(number, decimals, decimalSeparator, thousandsSeparator) {
    var parts, array;

    if (!isFinite(number) || isNaN(number = parseFloat(number))) {
        return "";
    }
    if (typeof decimalSeparator === "undefined") {
        decimalSeparator = ",";
    }
    if (typeof thousandsSeparator === "undefined") {
        thousandsSeparator = "";
    }

    // Redondeamos
    if (!isNaN(parseInt(decimals))) {
        if (decimals >= 0) {
            number = number.toFixed(decimals);
        }
        else {
            number = (
                Math.round(number / Math.pow(10, Math.abs(decimals))) * Math.pow(10, Math.abs(decimals))
            ).toFixed();
        }
    }
    else {
        number = number.toString();
    }

    // Damos formato
    parts = number.split(".", 2);
    array = parts[0].split("");
    for (var i = array.length - 3; i > 0 && array[i - 1] !== "-"; i -= 3) {
        array.splice(i, 0, thousandsSeparator);
    }
    number = array.join("");

    if (parts.length > 1) {
        number += decimalSeparator + parts[1];
    }

    return number;
}

//Bloquea totalmente la pantalla para que corra algún proceso en background
function lockScreen() {
    return new Promise(function (resolve, reject) {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            onBlock: function () {
                resolve();
            },
            message: "<h1>" + Resources.MessageWaiting + "</h1>"
        });
    })
}

//Desbloquea la pantalla 
function unlockScreen() {
    $.unblockUI();
}

function showInfoToast(message) {
    $.UifNotify('show', { 'type': 'info', 'message': message, 'autoclose': true });
}

function showErrorToast(message){
    $.UifNotify('show', { 'type': 'danger', 'message': message, 'autoclose': true });
}

function isBlankChar(text) {
    var textSplit = text.split(" ");
    if (textSplit.length == 1) {
        return (text.length <= 20)
    }
    else {
        if (textSplit.length > 1) {
            for (var i = 0; i < textSplit.length; i++) {
                if (textSplit[i].length > 20) {
                    return false;
                }
            }
        }
        return true;
    }
}

function RemoveFormatMoney(inpText) {
    if (inpText.indexOf('$') != -1) {
        inpText = inpText.replace('$', '').trim();
    }

    if ((new RegExp('^(\\d){1,3}(\\.(\\d){3})*(,(\\d){0,' + numberDecimal + '})?$')).test(inpText)) {
        inpText = inpText.replace(new RegExp('\\.', 'g'), '').replace(',', '.');
    }
    else if ((new RegExp('^(\\d){1,3}(,(\\d){3})*(.(\\d){0,' + numberDecimal + '})?$')).test(inpText)) {
        inpText = inpText.replace(new RegExp(',', 'g'), '');
    }
    else if ((new RegExp('^(\\d+)\\.(\\d){0,' + numberDecimal + '}$')).test(inpText)) {
        inpText = inpText.padEnd(inpText.indexOf('.') + (numberDecimal + 1), '0');
    }
    else if ((new RegExp('^(\\d+),(\\d){0,' + numberDecimal + '}$')).test(inpText)) {
        inpText = inpText.padEnd(inpText.indexOf(',') + (numberDecimal + 1), '0').replace(',', '.');
    }
    return 0;
}