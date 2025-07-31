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
    if (valueInitial > 100) {
        value = NotFormatMoney(value);
    }
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
var currencySymbol = '$';
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
    /// keyblock: bloquear copiar, cortar, pegar = 1. si | 0. no,2 pegar=solo numeros ,3 pegar=solo alfanumericos
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

                    case ValidatorType.Decimal:
                        if (e.which >= 48 && e.which <= 57 || (e.which == 44) || (e.which == 46)) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.RealNumbers:

                        if (e.which >= 48 && e.which <= 57) {
                            return true;
                        } if (e.which == 45) {
                            var value = $(this).val();
                            if (value.includes('-') || value.length > 0)
                                return false;
                            return true
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
                            (e.which >= 97 && e.which <= 122) || e.which == 160 || e.which == 162 ||
                            e.which == 163 || e.which == 164 || e.which == 241 || e.which == 46 || e.which == 165 ||
                            e.which == 130 || e.which == 32 || e.which == 164 || e.which == 225 ||
                            e.which == 223 || e.which == 237 || e.which == 250 || e.which == 243 ||
                            e.which == 233 || e.which == 35 || e.which == 45) {
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
                            (e.which >= 97 && e.which <= 122) || e.which == 32 || e.which == 38 || e.which == 209 || e.which == 241) {
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
                    case ValidatorType.Password:
                        if ((e.which >= 128 || e.which <= 163) && (e.which != 181 && e.which != 182 && e.which != 183 && e.which != 193 && e.which != 198 && e.which != 199 &&
                            e.which != 201 && e.which != 205 && e.which != 210 && e.which != 211 && e.which != 212 && e.which != 218 && e.which != 223 && e.which != 224 &&
                            e.which != 225 && e.which != 226 && e.which != 227 && e.which != 228 && e.which != 229 && e.which != 233 && e.which != 237 && e.which != 243 && e.which != 250)
                        ) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.lettersandnumbersAccent:
                        if ((e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) ||
                            (e.which >= 97 && e.which <= 122) || e.which == 32 || e.which == 38 || e.which == 209 || e.which == 241
                            || (e.which == 243) || (e.which == 225) || (e.which == 233) || (e.which == 237) || (e.which == 180) || (e.which == 193) || (e.which == 250)
                            || (e.which == 201) || (e.which == 211) || (e.which == 205) || (e.which == 218)
                        ) {
                            return true;
                        }
                        else {
                            return false;
                        }
                        break;
                    case ValidatorType.OnlylettersandnumbersNIT:
                        if (((e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) ||
                            (e.which >= 97 && e.which <= 122) || e.which == 32 || e.which == 38 || e.which == 209 || e.which == 241) && this.value.length <= 8) {
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
                        var text = $(this).val();
                    });
                    break;
                case 2:
                    //permitir pegar solo numeros
                    myControl.bind("paste", function (e) {
                        e.preventDefault();
                        if (e.originalEvent.clipboardData != undefined) {
                            var regex = /^\d+$/;
                            var copyText = e.originalEvent.clipboardData.getData('text');
                            copyText = copyText.replace(' ', '');
                            if (regex.test(copyText)) {
                                $(this).val(copyText);
                            }
                        }
                    });
                    break;
                case 3:
                    //permitir pegar solo alfanumericos
                    myControl.bind("paste", function (e) {
                        e.preventDefault();
                        if (e.originalEvent.clipboardData != undefined) {
                            var regex = (/[A-Za-z0-9 ]+/g);
                            //var regex = (/[A-z\u00C0-\u00ff]+/g);
                            var copyText = e.originalEvent.clipboardData.getData('text');
                            var clearText = copyText.replace(/[`~\_\-.+*!@#$%^&|'<>\{\}\[\]\\\/]/gi, '');
                            if (regex.test(clearText)) {
                                $(this).val(clearText);
                            }
                        } else {
                            e.preventDefault();
                        }

                    });
                    break;
                case 4:
                    //permitir pegar alfanumericos y algunos caracteres especiales
                    myControl.bind("paste", function (e) {
                        e.preventDefault();
                        if (e.originalEvent.clipboardData != undefined) {
                            var regex = (/[A-Za-z0-9 _ - ]+/g);
                            //var regex = (/[A-z]+/g);
                            var copyText = e.originalEvent.clipboardData.getData('text');
                            var clearText = copyText.replace(/[`~+.*!@#$%^&|'<>\{\}\[\]\\\/]/gi, '');
                            if (regex.test(clearText)) {
                                $(this).val(clearText);
                            }
                        } else {
                            e.preventDefault();
                        }

                    });
                    break;
                case 5:
                    //Permitir solo pegar letras
                    myControl.bind("cut copy paste", function (e) {
                        if (e.originalEvent.clipboardData != undefined) {
                            var regex = /[A-Za-z ñ]+/;
                            var copyText = e.originalEvent.clipboardData.getData('text');
                            copyText = copyText.replace(' ', '');
                            for (var i = 0; i < copyText.length; i++) {
                                if (!regex.test(copyText.charAt(i))) {
                                    e.preventDefault();
                                    var text = $(this).val();
                                    break;
                                }
                            }
                        }
                    });
                    break;
                case 6:
                    //Permitir solo cadenas de caracteres menores o iguales a nueve
                    myControl.bind("cut copy paste", function (e) {
                        if (e.originalEvent.clipboardData != undefined) {
                            var copyText = e.originalEvent.clipboardData.getData('text'); 
                            if (copyText.length > 9) {
                                e.preventDefault();
                                $(this).val("");
                            }
                        }
                    });
                    break;
                case 7:
                    //Permitir pegar copiar cortar
                    myControl.unbind("cut copy paste");
                    break;
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
        if (e.originalEvent.clipboardData != undefined) {
            var regex = /^[0-9,.]+$/;
            var copyText = e.originalEvent.clipboardData.getData('text');
            copyText = copyText.replace(' ', '');
            if (regex.test(copyText)) {
                $(this).val(copyText);
            }
        }
    });
}

$.fn.OnlyDecimals1 = function (decimalCount) {
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
$.fn.OnlyGeographicCoordinates = function (coordinate, errorMessage) {
    $(this).focusout(function (e) {
        var regExp = "";
        var value = $(this).val() + (e.key != undefined ? e.key : "");
        switch (coordinate) {
            case "Latitude":
                regExp = "^(\\+|-)?(?:90(?:(?:\\" + separatorDecimal + "0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\\" + separatorDecimal + "[0-9]{1,6})?))$";
                break;
            case "Longitude":
                regExp = "^(\\+|-)?(?:180(?:(?:\\" + separatorDecimal + "0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\\" + separatorDecimal + "[0-9]{1,6})?))$";
                break;
        }
        var exp = new RegExp(regExp);
        if (value != "" && !exp.test(value)) {
            $(this).val('');
            $.UifNotify('show', { 'type': 'info', 'message': errorMessage, 'autoclose': true });
        }

    });
    $(this).keyup(function (e) {
        $(this).val($(this).val().toString().replace(/\./g, ','));
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
String.prototype.sistranReplaceAccentMark = function () {
    var find = ['á', 'é', 'í', 'ó', 'ú'];
    var replace = ['a', 'e', 'i', 'o', 'u'];
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


//Validar formato Email
function ValidateEmail(input) {
    var email = input.toLowerCase();
    var valid = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,8}(?:\.[a-z]{2})?)$/i.test(email);
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

/// <summary>
/// Calcula la Edad de una persona
/// </summary>
/// <param name="birthDate">Fecha de nacimiento</param>
/// <returns></returns>
function CalculateAge(birthDate) {
    if (birthDate != null && birthDate != "") {
        birthDate = birthDate.replace('-', '/');
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
var isoDateRegex = /^(\d\d\d\d)-(\d\d)-(\d\d)T(\d\d):(\d\d):(\d\d)(\.\d\d?\d?)?([\+-]\d\d:\d\d|Z)?$/;
function looksLikeIsoDate(s) {
    return isoDateRegex.test(s);
}
function DateValidate(date) {
    return date.indexOf("/Date");
}
jQuery.throughObject = function (obj) {
    for (var attr in obj) {
        if (typeof obj[attr] === 'string') {
            if (DateValidate(obj[attr]) > -1) {
                obj[attr] = FormatDate(obj[attr]);
            }
        }
        if (typeof obj[attr] === 'object') {
            jQuery.throughObject(obj[attr]);
        }
        return obj;
    }
}

function isExpirationDate(fecha) {

    var today = new Date()
    var date = fecha[1] + "/" + fecha[0] + "/" + fecha[2]
    if ((Date.parse(date)) > (Date.parse(today))) {
        return true;
    }
    return false;
}

$.fn.OnlyDecimalsWithDot = function (decimalCount) {
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
        $(this).val($(this).val().toString().replace(/\,/g, '.'));
    });

    var myControl = $(this);
    myControl.bind("contextmenu", function (e) {
        return false;
    });

    myControl.bind("paste", function (e) {
        e.preventDefault();
    });
}

/**
*@summary Actualiza el color de los items del list view de acuerdo al status asignado en el ultimo div del template
*@param {id} * ID del listview
*@param {child} * Linea en el item que debe ser ajustada int; si son todas string "all"; undefined deja la primera
*/
function listViewColors(id, child) {
    var list = document.getElementById(id);
    var itemsList = list.getElementsByClassName("template");

    for (i = 0; i < itemsList.length; i++) {
        var status = itemsList[i].lastElementChild.textContent.replace(/\D/g, '');
        switch (status) {
            //Original
            case "1":
                if (child == undefined) {
                    itemsList[i].firstElementChild.className.replace(/\bsuccess\b/g, "");
                    itemsList[i].firstElementChild.className.replace(/\bwarning\b/g, "");
                }
                else {
                    if (child == "all") {
                        itemsList[i].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].className.replace(/\bwarning\b/g, "");
                    } else {
                        itemsList[i].children[child - 1].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].children[child - 1].className.replace(/\bwarning\b/g, "");
                    }
                }
                break;
            //Create
            case "2":
                if (child == undefined) {
                    itemsList[i].firstElementChild.className.replace(/\bsuccess\b/g, "");
                    itemsList[i].firstElementChild.className.replace(/\bwarning\b/g, "");
                    itemsList[i].firstElementChild.classList.add("success");
                } else {
                    if (child == "all") {
                        itemsList[i].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].className.replace(/\bwarning\b/g, "");
                        itemsList[i].classList.add("success");
                    } else {
                        itemsList[i].children[child - 1].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].children[child - 1].className.replace(/\bwarning\b/g, "");
                        itemsList[i].children[child - 1].classList.add("success");
                    }
                }
                break;
            //Update
            case "3":
                if (child == undefined) {
                    itemsList[i].firstElementChild.className.replace(/\bsuccess\b/g, "");
                    itemsList[i].firstElementChild.className.replace(/\bwarning\b/g, "");
                    itemsList[i].firstElementChild.classList.add('warning');
                } else {
                    if (child == "all") {
                        itemsList[i].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].className.replace(/\bwarning\b/g, "");
                        itemsList[i].classList.add('warning');
                    } else {
                        itemsList[i].children[child - 1].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].children[child - 1].className.replace(/\bwarning\b/g, "");
                        itemsList[i].children[child - 1].classList.add('warning');
                    }

                }
                break;
            //Delete
            case "4":
                break;
            default:
                if (child == undefined) {
                    itemsList[i].firstElementChild.className.replace(/\bsuccess\b/g, "");
                    itemsList[i].firstElementChild.className.replace(/\bwarning\b/g, "");
                } else {
                    if (child == "all") {
                        itemsList[i].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].className.replace(/\bwarning\b/g, "");
                    } else {
                        itemsList[i].children[child - 1].className.replace(/\bsuccess\b/g, "");
                        itemsList[i].children[child - 1].className.replace(/\bwarning\b/g, "");
                    }
                }
                break;
        }
    };
}