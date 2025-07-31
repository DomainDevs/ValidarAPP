$.validator.methods.number = function (value, element) {
    var TypeReturn = true;
    valueInitial = element.value;
    value = NotFormatMoney(element.value);

    value = replaceCode(value);
    //element.value = value;
    if (this.optional(element) || !isNaN(value)) {
        value = valueInitial;
        TypeReturn = true;
    }
    else {
        TypeReturn = false;
    }
    return TypeReturn;
}

$.validator.methods.range = function (value, element, param) {
    var TypeReturn = true;
    valueInitial = element.value;
    value = NotFormatMoney(value);
    value = replaceCode(value);
    //element.value = value;
    if (this.optional(element) || (value >= param[0] && value <= param[1])) {
        TypeReturn = true;
        value = valueInitial;
    }
    else {
        TypeReturn = false;
    }
    return TypeReturn;
};

function replaceCode(value) {
    return value.replace(",", ".");
}
// Funcion Permitir solo numeros o letras
//Validar Email

var classAlerts = ['success', 'warning', 'info', 'danger'];
var separatorDecimal = ',';
var separatorThousands = '.';
var patternDecimal = GetPatternDecimal();
var numberDecimal = 2;
var numberDecimalPremium = 2;
var numberDecimalSumInsured = 2;
function GetPatternDecimal() {
    if (separatorDecimal == '.') {
        return /[0-9]|[.]/;
    }
    else {
        return /[0-9]|[,]/;
    }
}

function replaceChar(char, chain) {
    if (char.indexOf("\\") === -1) {
        char = "\\" + char;
    }
    var re = char.replace(/"/, "");
    var newChain = String(chain);
    newChain = newChain.replace(/char/gi, "");
    return newChain;
}

(function ($) {
    var isShift = false;
    /// <summary>
    /// Validar caracteres permitidos del teclado y acciones de pegar
    /// Parametros de entrada:
    /// keyDefault: caracteres permitidos = 1. solo numeros | 2. solo letras | 3. direcciones | 
    /// 4. emails | 5. fechas | 6. polizas externas | 7.Solo letras y numeros
    /// keyblock: bloquear copiar, cortar, pegar = 1. si | 0. no
    /// rightclick: bloquear clic derecho = 1. si | 0. no
    /// </summary>
    $.fn.ValidatorKey = function (keyInput, keyBlock, rightClick) {

        var param = {
            keyDefault: keyInput,
            keyblock: keyBlock,
            rightclick: rightClick
        };

        var defaults = {
            keyDefault: 1,
            keyblock: 1,
            rightclick: 1,
            EnabledControl: true
        };

        $.extend(defaults, param);

        this.each(function () {
            //variables locales al plugin
            var myControl = $(this);

            var keyVal = defaults.keyDefault
            myControl.data("keyDefault", defaults.keyDefault);
            myControl.data("keyblock", defaults.keyblock);
            myControl.keyup(function (e) {
                //Permitir alt+26=→ 39 alt+27=←  37  $=52 . = 46
                if (e.keyCode == 37 || e.keyCode == 39 || e.keyCode == 52 || e.keyCode == 46 || e.keyCode == 110) {
                    e.preventDefault();
                    return true;
                }
            });
            myControl.keypress(function (e) {

                //Blolquear controles especiales
                var allow = false;
                var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                switch ($(this).data("keyblock")) {
                    case ValidatorType.Number:
                        //Habilita o deshabilita alt+27=← 
                        if (e.altKey && key == 27 /* firefox */) return defaults.EnabledControl;
                        // allow Ctrl+A
                        if ((e.ctrlKey && key == 97 /* firefox */) || (e.ctrlKey && key == 65) /* opera */) return true;
                        // allow Ctrl+X (Cortar)
                        if ((e.ctrlKey && key == 120 /* firefox */) || (e.ctrlKey && key == 88) /* opera */) return true;
                        // allow Ctrl+C (Pegar)
                        if ((e.ctrlKey && key == 99 /* firefox */) || (e.ctrlKey && key == 67) /* opera */) return true;
                        // allow Ctrl+Z (Deshacer)
                        if ((e.ctrlKey && key == 122 /* firefox */) || (e.ctrlKey && key == 90) /* opera */) return true;
                        // allow or deny Ctrl+V (Pegar), Shift+Ins
                        if ((e.ctrlKey && key == 118 /* firefox */) || (e.ctrlKey && key == 86) /* opera */
                        || (e.shiftKey && key == 45)) return allow;

                        break;
                }
                //Permitir teclas retroceso y borrar

                //Validar datos de entrada permitidos
                var valtec = $(this).data("keyDefault");
                switch (valtec) {
                    case ValidatorType.Number:
                        if (e.which >= 48 && e.which <= 57) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.Letter:
                        if ((e.which >= 65 && e.which <= 90) || (e.which >= 97 && e.which <= 122) || e.which == 209 || e.which == 241 || e.which == 32) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.Addresses:
                        if ((e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) ||
                            (e.which >= 97 && e.which <= 122) || (e.which >= 160 && e.which <= 163) ||
                            e.which == 164 || e.which == 241 || e.which == 46 || e.which == 165 ||
                            e.which == 130 || e.which == 32 || e.which == 164 || e.which == 225 ||
                            e.which == 223 || e.which == 237 || e.which == 250 || e.which == 243) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.Emails:
                        if ((e.which >= 48 && e.which <= 57) || (e.which >= 64 && e.which <= 90) ||
                            (e.which >= 97 && e.which <= 122) || e.which == 45 || e.which == 95 || e.which == 46) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.Dates:
                        if (e.which >= 47 && e.which <= 57) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.ExternalPolicies:
                        if ((e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) ||
                            (e.which >= 97 && e.which <= 122) || e.which == 45) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.Onlylettersandnumbers:
                        if ((e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) ||
                            (e.which >= 97 && e.which <= 122)) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.lettersandnumbersSpecial:
                        if ((e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) ||
                           (e.which >= 97 && e.which <= 122) || (e.which == 32) || (e.which == 241)
                            || (e.which == 45) || (e.which == 95) || (e.which == 193) || (e.which == 201) || (e.which == 205) || (e.which == 211) || (e.which == 218)
                            ) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;

                }
            });

            //bloquear click derecho
            switch (defaults.rightclick) {
                case 1:
                    myControl.bind("contextmenu", function (e) {
                        return false;
                    });
            }

            switch (defaults.keyblock) {
                case 1:
                    //bloquear cortar Pegar
                    myControl.bind("cut copy paste", function (e) {
                        e.preventDefault();
                    });
            }
        });
    }
})(jQuery);

$.fn.LoadCss = function (param) {
    var defaults = {
        column: 0,
        color: "yellow",
        text: ""
    }
    $.extend(defaults, param);
    var $this = $('tbody tr', this);
    $this.each(function (index) {
        if ($('td', this).eq(defaults.column).text() == defaults.text) {
            $(this).css("background-color", defaults.color);
        }
    });
    return this;
}

$.fn.GetDataSelected = function () {
    var $this = $(this).DataTable();
    var data = $this.rows('.row-selected').data();
    return data;
}

$.fn.OnlyPercentage = function () {
    $(this).keypress(function (e) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        var keyCode = e.keyCode ? e.keyCode : e.which;
        var text = $(this).val();
        var input = text + String.fromCharCode(keyCode);
        var number = parseInt(input, 10);
        var point = text.indexOf(separatorDecimal);

        if (text == '0' && keyCode == 48) {
            return false;
        }

        if (text == '' && keyCode == separatorDecimal.charCodeAt()) {
            return false;
        }

        if (!patternDecimal.test(String.fromCharCode(keyCode))) {
            return false;
        }

        if (number > 100 || (number == 100 && keyCode == separatorDecimal.charCodeAt())) {
            return false;
        }

        if (point > 0) {
            if (keyCode == separatorDecimal.charCodeAt()) {
                return false;
            }
            else {
                var decimals = text.split(separatorDecimal);
                if (decimals[1].length == 2) {
                    return false;
                }
            }
        }
        return true;
    });

    var myControl = $(this);
    myControl.bind("contextmenu", function (e) {
        return false;
    });

    myControl.bind("paste", function (e) {
        e.preventDefault();
    });
}

$.fn.OnlyDecimals = function (decimalCount) {
    $(this).keypress(function (e) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        var keyCode = e.keyCode ? e.keyCode : e.which;

        if (keyCode == 46) {
            keyCode = 44;
        }

        var text = $(this).val();
        var input = text + String.fromCharCode(keyCode);
        var number = parseInt(input, 10);
        var point = text.indexOf(separatorDecimal);

        if (text == '0' && keyCode == 48) {
            return false;
        }

        if (!patternDecimal.test(String.fromCharCode(keyCode))) {
            return false;
        }

        if (point > 0) {
            if (keyCode == separatorDecimal.charCodeAt()) {
                return false;
            }
            else {
                var decimals = text.split(separatorDecimal);
                if (decimals[1].length == decimalCount) {
                    return false;
                }
            }
        }
        return true;
    });

    $(this).keyup(function (e) {
        $(this).val($(this).val().toString().replace(/\./g, ','));
    });

    var myControl = $(this);
    myControl.bind("contextmenu", function (e) {
        return false;
    });

    myControl.bind("paste", function (e) {
        e.preventDefault();
    });
}

String.prototype.sistranReplaceAll = function () {
    var find = ['&#225;', '&#233;', '&#237;', '&#243;', '&#250;', '\xe1', '\xe9', '\xed', '\xf3', '\xfa'];
    var replace = ['á', 'é', 'í', 'ó', 'ú', 'á', 'é', 'í', 'ó', 'ú'];
    var replaceString = this;
    var regex;
    for (var i = 0; i < find.length; i++) {
        regex = new RegExp(find[i], "g");
        replaceString = replaceString.replace(regex, replace[i]);
    }
    return replaceString;
};

function showAlerts(objName, textMessage, colorClass, stateVisible) {
    if (stateVisible) {
        $(objName).UifAlert('show', textMessage.sistranReplaceAll(), classAlerts[colorClass]);
    }
}

function hideAlerts() {
    $('#alert').hide();
    $('#alert2').hide();
    $('#alertSeeLoad').hide();
    $('#alertFile').hide();
    $('#alertInquiryDetail').hide();
    $('#alertLoadInquiry').hide();
    $('#alert_modal').hide();
}

//Validar formato Email
function ValidateEmail(input) {
    var email = input.toLowerCase();
    var valid = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i.test(email);
    return valid;
}

(function ($) {
    $.fn.HideColums = function (option) {
        var defaults = {
            colums: [],
            control: ""
        }
        var defaults = $.extend(defaults, option)
        var elem = $(defaults.control);
        var oID = $(elem).attr("id");
        $(this).data('defaults', defaults);
        for (var i = 0; i < $(this).data('defaults').colums.length; i++) {
            var k = $(this).data('defaults').colums[i] + 1;
            $('#' + oID + ' thead th:eq(' + $(this).data('defaults').colums[i] + ')').hide();
            $('#' + oID + ' tbody td:nth-child(' + k + ')').hide();
        }
        this.each(function () {

            if (defaults.length === 0) {
                var colum = $(elem).DataTable().columns;
                $(elem).DataTable().on('draw', function () {
                    var i = 0;
                    for (i = 1; i <= defaults.colums.length + 1; i++) {
                        $('#table' + oID + ' tbody td:nth-child(' + i + ')').hide();
                    };
                    for (i = 0; i <= defaults.colums.length; i++) {
                        $('#table' + oID + ' thead th:eq(' + i + ')').hide();
                    }
                });


            }
            else {
                $(this).DataTable().on('draw', function () {
                    for (var i = 0; i < $(this).data('defaults').colums.length; i++) {
                        var k = defaults.colums[i] + 1;
                        $('#' + oID + ' thead th:eq(' + defaults.colums[i] + ')').hide();
                        $('#' + oID + ' tbody td:nth-child(' + k + ')').hide();
                    }
                });
            }

        });
        //return this;
    }
})(jQuery)

function FormatDate(date) {
    if (date != null) {
        date = date.toString();
        if (date.length > 10) {
            var dateString = date.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var dayFormat = day < 10 ? '0' + day : '' + day;
            var monthFormat = month < 10 ? '0' + month : '' + month;
            var date = dayFormat + "/" + monthFormat + "/" + year;
        }
    }
    return date;
}

function GetCurrentFromDate() {
    var currentTime = new Date();
    var month = '0' + (currentTime.getMonth() + 1);
    var day = '0' + currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = day.substring(day.length - 2, day.length) + "/" + month.substring(month.length - 2, month.length) + "/" + year;
    return date;
}

function GetCurrentToDate(dateCurrent) {
    if (dateCurrent != null) {
        var cDate1 = dateCurrent.toString().split('/');
        var toTime = new Date(cDate1[2], cDate1[1] - 1, cDate1[0]);
        var month = toTime.getMonth() + 1;
        var day = toTime.getDate();
        var year = toTime.getFullYear() + 1;
        date = day + "/" + month + "/" + year;
    }
    return date;
}

function FormatMoney(input, decimals) {
    if (input != null) {
        if (separatorDecimal == ',') {
            input = input.toString().replace(/\./g, ',');
        }
        else {
            input = input.toString().replace(/\,/g, '.');
        }

        var decimal = input.toString().split(separatorDecimal);
        input = decimal[0].toString();
        var pattern = /(-?\d+)(\d{3})/;
        while (pattern.test(input)) {
            input = input.replace(pattern, "$1" + separatorThousands + "$2");
        }
        if (decimal.length > 1) {
            if (decimals === undefined) {
                input = input + separatorDecimal + decimal[1].substring(0, 3);
            }
            else {
                input = input + separatorDecimal + decimal[1].substring(0, decimals);
            }

        }
    }
    return input;
}

function NotFormatMoney(input) {
    if (input != null) {
        input = input.toString();
        //Si el separador decimal es . o , y es solo uno no se debe borrar
        var count = input.split(separatorDecimal).length - 1;
        if (!count == 1) {
            if (separatorThousands == ',') {
                input = input.replace(/\,/g, '');
            }
            else {
                input = input.replace(/\./g, '');
            }
        }
        else {
            input = input.replace(/\./g, '');
        }
        input = input.replace(/\$/g, '');
    }
    return input;
}

/// <summary>
/// Calcula la Edad de una persona
/// </summary>
/// <param name="birthDate">Fecha de nacimiento</param>
/// <returns></returns>
function CalculateAge(birthDate) {
    if (birthDate != null && birthDate != "") {
        birthDate=birthDate.replace('-','/');
        var values = birthDate.split("/");
        var day = values[2];
        var month = values[1];
        var year = values[0];

        var date_day = new Date();
        var now_year = date_day.getYear();
        var now_month = date_day.getMonth() + 1;
        var now_day = date_day.getDate();

        var age = (now_year + 1900) - year;
        if (now_month < month) {
            age--;
        }
        if ((month == now_month) && (date_day < day)) {
            age--;
        }
        if (age > 1900) {
            age -= 1900;
        }
        return age;
    }
    return birthDate;
}

function stringToBoolean(stringBool) {
    if (stringBool !== undefined && stringBool !== null) {
        stringBool || (stringBool = 'false');
        if (!(typeof stringBool === "string")) {
            stringBool = stringBool.toString();
        }
        switch (stringBool.toLowerCase().trim()) {
            case "true": case "yes": case "1": return true;
            case "false": case "no": case "0": case null: return false;
            default: return Boolean(string);
        }
    }
    else {
        return false;
    }
}

$.fn.TextTransform = function (option) {
    $(this).keyup(function (e) {
        if (e.which < 48 || e.which > 57) {
            var position = $(this).prop('selectionStart');
            if (typeof option != 'undefined' && option == ValidatorType.UpperCase) {
                $(this).val($(this).val().toUpperCase());
            }
            $(this).prop('selectionStart', position);
            $(this).prop('selectionEnd', position);
        }
    });

}