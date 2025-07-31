var DateFormat = new String("dd/mm/yyyy");
var DateFullFormat = new String("dd/mm/yyyy HH:mm:ss");
var MinCurrentDate = new String("01/01/1900");
var DateSplit = "/";
var RoundDecimal = 2;
var nameControl = "_newControl";
var decimals = undefined;
var MaxDecimalQuota = 9999999999999999.99;
var currencyObject = {
    exchangeRate: 0,
    currencyType: 0
}

var toType = function (obj) {
    return ({}).toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase();
}

function Isnull(data) {
    if (data == null || data == undefined) {
        return true;
    }
    {
        return false;
    }

}
function CalculateDays(from, to) {
    if (from.indexOf("Date") > -1) {
        from = FormatDate(from);
    }
    if (to.indexOf("Date") > -1) {
        to = FormatDate(to);
    }
    var date1 = from.toString().split(DateSplit);
    var date2 = to.toString().split(DateSplit);
    var fdate1 = Date.UTC(date1[2], date1[1] - 1, date1[0]);
    var fdate2 = Date.UTC(date2[2], date2[1] - 1, date2[0]);
    var dif = fdate2 - fdate1;
    var days = Math.floor(dif / (1000 * 60 * 60 * 24));
    if (isNaN(days)) {
        return 0;
    }
    else {
        return days;
    }
}

function ReplaceCharacter(value) {
    if (value != null) {

        value.toString();
        if (typeof value === "String" || typeof value === "string") {
            var symbols = {
                '@': "%40",
                '&amp;': "%26",
                '*': "%2A",
                '+': "%2B",
                '/': "%2F",
                '&lt;': "%3C",
                '&gt;': "%3E",
                '#': "%23",
                '&': "&",
                '~': "7E%",

            };
            value = value.replace(/([@*+/#&]|&(amp|lt|gt);)/g, function (m) { return symbols[m]; });

        }
    }
    return value;
}

//Summary: Comparar dos fechas
//Return: 1 = La fecha 1 es mayor a la fecha 2 | 0 = La fecha 1 es menor  a la fecha 2 | 2 las 2 fechas son iguales
/**

 *Comparar dos fechas

 * @param  {datetime,datetime}

 * @return  {boolean}

 */
function CompareDates(date1, date2) {
    var resultCompare = 0;
    if (date1 != null && date2 != null) {
        if (date1.indexOf("Date") > -1) {
            date1 = FormatDate(date1);
        }
        if (date2.indexOf("Date") > -1) {
            date2 = FormatDate(date2);
        }
        var datePart1 = date1.split(DateSplit);
        var datePart2 = date2.split(DateSplit);
        var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
        var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);

        if (compare1 > compare2) {
            resultCompare = 1;
        }
        else if (compare1 < compare2) {
            resultCompare = 0;
        }
        else {
            resultCompare = 2;
        }
    }

    return resultCompare;
}

//Summary: Comparar dos fechas
//Return: 1 = La fecha 1 es mayor a la fecha 2 | 0 = La fecha 1 es menor o igual a la fecha 2
function CompareClaimDates(date1, date2) {
    var resultCompare = 0;
    if (date1 != null && date2 != null) {
        if (date1.indexOf("Date") > -1) {
            date1 = FormatDate(date1);
        }
        if (date2.indexOf("Date") > -1) {
            date2 = FormatDate(date2);
        }
        var datePart1 = date1.split(DateSplit);
        var datePart2 = date2.split(DateSplit);
        var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
        var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);

        if (compare1 > compare2) {
            resultCompare = 1;
        }
    }

    return resultCompare;
}

//Summary: Comparar dos fechas
//Return: 1 = La fecha 1 es mayor o igual a la fecha 2 | 0 = La fecha 1 es menor o igual a la fecha 2
function CompareDateEquals(date1, date2) {

    var resultCompare = 0;

    if (date1 != null && date2 != null) {
        if (date1.indexOf("Date") > -1) {
            date1 = FormatDate(date1);
        }
        if (date2.indexOf("Date") > -1) {
            date2 = FormatDate(date2);
        }
        var datePart1 = date1.split(DateSplit);
        var datePart2 = date2.split(DateSplit);
        var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
        var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);

        if (compare1 >= compare2) {
            resultCompare = 1;
        }
    }

    return resultCompare;
}
function FormatMoney(input, decimals) {
    if (input != null) {
      if(!ValidateMoney(input.toString()))
      {
        return input;
      }
        if (separatorDecimal == ",") {
            input = input.toString().replace(/\./g, ",");
        }
        else {
            input = input.toString().replace(/\,/g, ".");
        }

        var decimal = input.toString().split(separatorDecimal);
        input = decimal[0].toString();
        var pattern = /(-?\d+)(\d{3})/;
        while (pattern.test(input)) {
            input = input.replace(pattern, "$1" + separatorThousands + "$2");
        }
        if (decimal.length > 1) {
            if (decimals === undefined) {
                input = input + separatorDecimal + decimal[1].substring(0, 6);
            }
            else {
                input = input + separatorDecimal + decimal[1].substring(0, decimals);
            }
        }
      else
        {
          input = input + separatorDecimal + "00"
        }
    }
    return input;
}

function FormatMoneyCurrency(currencyType, value, inputId) {

    var newValor = 0.0;

    if ((currencyObject.local == currencyObject.currencyType || (currencyObject.currencyType == undefined && currencyType != undefined)) &&
        (currencyObject.exchangeRate > 0 && value != null && value > 0)) {
        newValor = value;
    }
    if (currencyObject.local != currencyObject.currencyType &&
        (currencyObject.exchangeRate > 0 && value != null && value > 0)) {
        if ((currencyObject.local == EnumCurrency.Pesos && currencyObject.currencyType == EnumCurrency.Dolares) ||
            (currencyObject.local == EnumCurrency.Pesos && currencyObject.currencyType == EnumCurrency.Euros)) {
            newValor = (value * currencyObject.exchangeRate);
        }
        else if ((currencyObject.local == EnumCurrency.Dolares && currencyObject.currencyType == EnumCurrency.Pesos) ||
            (currencyObject.local == EnumCurrency.Euros && currencyObject.currencyType == EnumCurrency.Pesos)) {
            newValor = (value / currencyObject.exchangeRate);
        }
    }
    if (value != null && newValor == 0) {
        if (separatorDecimal == ",") {
            value = value.toString().replace(/\./g, ",");
        }
        else {
            value = value.toString().replace(/\,/g, ".");
        }

        var decimal = value.toString().split(separatorDecimal);
        value = decimal[0].toString();
        var pattern = /(-?\d+)(\d{3})/;
        while (pattern.test(value)) {
            value = value.replace(pattern, "$1" + separatorThousands + "$2");
        }
        if (decimal.length > 1) {
            if (decimals === undefined) {
                value = value + separatorDecimal + decimal[1].substring(0, 6);
            }
            else {
                value = value + separatorDecimal + decimal[1].substring(0, decimals);
            }
        }
        $("#" + inputId).show();
        $("#" + inputId + nameControl).remove();

    } else {

        if (separatorDecimal == ",") {
            newValor = newValor.toString().replace(/\./g, ",");
        }
        else {
            newValor = newValor.toString().replace(/\,/g, ".");
        }

        var decimal = newValor.toString().split(separatorDecimal);
        newValor = decimal[0].toString();
        var pattern = /(-?\d+)(\d{3})/;
        while (pattern.test(newValor)) {
            newValor = newValor.replace(pattern, "$1" + separatorThousands + "$2");
        }
        if (decimal.length > 1) {
            if (decimals === undefined) {
                newValor = newValor + separatorDecimal + decimal[1].substring(0, 2);
            }
            else {
                newValor = newValor + separatorDecimal + decimal[1].substring(0, decimals);
            }
        }

        if (inputId != null && newValor.toString() != "0.0") {
            $("div").remove("#" + inputId + nameControl);
            $("#" + inputId).after($("#" + inputId).prop('outerHTML').replace(inputId, inputId + nameControl));
            $("#" + inputId).hide();
            $("#" + inputId + nameControl).show();
            $("#" + inputId + nameControl).html(newValor);
        }
    }
}



function NotFormatMoney(input) {
    if (input != null) {
      input = input.toString();      
      if(!ValidateNotMoney(input))
      {
        return input;
      }          
      if (separatorThousands == ",") {
        input = input.replace(/\,/g, "");
      }
      else {
        input = input.replace(/\./g, "");
      }
            
      input = input.replace(/\$/g, "");
    }    
    return input;  
}

function GetDateIssue() {
    $.ajax({
        type: "POST",
        url: rootPath + "Underwriting/Underwriting/GetModuleDateIssue",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#inputIssueDate").text(FormatFullDate(data.result));
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        resultSave = false;
        $.UifNotify("show", { 'type': "danger", 'message': "Error al consultar fecha emisión", 'autoclose': true });
    });
}

function GetCurrentDateFrom() {
    $.ajax({
        type: "POST",
        url: rootPath + "Underwriting/Underwriting/GetDate",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.success) {
            $("#inputFrom").val(FormatDate(data.result));
            $("#inputTo").UifDatepicker('setValue', AddToDate($("#inputFrom").val(), 0, 0, 1));
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        resultSave = false;
        $.UifNotify("show", { 'type': "danger", 'message': "Error al consultar fecha emisión", 'autoclose': true });
    });
}

function GetAccessButtonsByPath(pathName) {
    if (pathName != "") {
        $.ajax({
            type: "POST",
            url: rootPath + "Account/GetAccessButtonsByPath",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ path: pathName })
        }).done(function (data) {
            if (data.success) {
                $.each(data.result, function (key, control) {
                    if (document.getElementById(control.Description) != null) {
                        $("#" + control.Description).show();
                    }
                });
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            resultSave = false;
            $.UifNotify("show", { 'type': "danger", 'message': "Error al consultar accesos a nivel de botón", 'autoclose': true });
        });
    }

}

function setInitialDate(tag, date) {
    if (date != null) {
        $(tag).UifDatepicker({
            onLoaded: function () {
                $(tag).UifDatepicker("setValue", date);
            }
        });
    }
}

function clearProperties(data) {
    $.each(data, function (id, item) {
        switch (item.component) {
            case "val":
                $("#" + id).val("");
                break;
            case "UifSelect":
                $("#" + id).UifSelect("setSelected", null);
                break;
            case "UifDatepicker":
                $("#" + id).UifDatepicker("clear");
                break;
            case "UifDataTable":
                $("#" + id).UifDataTable("unselect");
                break;
            case "UifListView":
                $("#" + id).UifListView("clear");
                break;
            case "Check":
                $("#" + id).prop("checked", false);
                break;
            default:
                break;
        }
    });
}

function assignProperties(data, objectProperties) {
    if (data != null) {
        $.each(objectProperties, function (id, item) {
            var valItem = data[id];
            switch (item.component) {
                case "val":
                    $("#" + id).val(valItem);
                    break;
                case "UifSelect":
                    $("#" + id).UifSelect("setSelected", valItem);
                    break;
                case "UifDatepicker":
                    if (valItem != null && valItem != "") {
                        $("#" + id).UifDatepicker("setValue", FormatDate(valItem));
                    }
                    else {
                        $("#" + id).UifDatepicker("clear");
                    }

                    break;
                case "UifDataTable":
                    var tablaGetData = $("#" + id).UifDataTable("getData");
                    $.each(tablaGetData, function (idTabla, itemTabla) {
                        $.each(data[id.replace("table", "")], function (index, data) {
                            if (itemTabla.Id == data.SpecialityId) {
                                $("#" + id + " tbody tr:eq(" + idTabla + ")").removeClass("row-selected").addClass("row-selected");
                                $("#" + id + " tbody tr:eq(" + idTabla + ") td button span").removeClass("glyphicon glyphicon-unchecked").addClass("glyphicon glyphicon-check");
                            }
                        })

                    });
                    break;
                case "UifListView":
                    $("#" + id).UifListView("clear");
                    $.each(data[id.replace("listV", "")], function (index, data) {
                        $("#" + id).UifListView("addItem", data);
                    });
                    break;
                case "Check":
                    $("#" + id).prop("checked", false);
                    break;
                default:
                    break;
            }
        });
    }
    else {
        clearProperties(objectProperties);
    }
}

function addItemListView() {

}

function savePromise(data, url, msjError, dataPropierties) {
    var promise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            resolve(data);
        }).fail(function (response) {
            reject(response);
        });
    });
    promise.then(function (data) {
        if (data.success) {
            clearProperties(dataPropierties);
            assignProperties(dataPropierties, data.result);
        }
        else {
            $.UifNotify("show", { 'type': "danger", 'message': msjError, 'autoclose': true });
        }
    }, function (response) {
        $.UifNotify("show", { 'type': "danger", 'message': msjError, 'autoclose': true });
    });
}

function FormatDate(date) {
    if (date != null) {
        date = date.toString();
        var dateString = date.substr(6);
        if (date.length > 10) {
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var dayFormat = day < 10 ? "0" + day : "" + day;
            var monthFormat = month < 10 ? "0" + month : "" + month;
            var yearFormat = year < 1000 ? "0" + year : "" + year;
            yearFormat = year < 100 ? "00" + year : "" + year;
            yearFormat = year < 10 ? "000" + year : "" + year;
            date = dayFormat + DateSplit + monthFormat + DateSplit + yearFormat;
            
            if (year <1900) {
                date = null;
            }
        }
    }
    return date;
}
function FormatFullDate(date, dateShort) {
    if (date != null) {
        if (toType(date) == TypeObjects.string) {
            var d = /\/Date\(/.exec(date);
            var currentTime = (d) ? new Date(parseInt(date.substr(6))) : date;
            var date = currentTime;
            if (toType(currentTime) == TypeObjects.date && dateShort == undefined) {
                date = currentTime.getFromFormat("dd" + DateSplit + "MM" + DateSplit + "yyyy hh:ii:ss");
            }
            else if (toType(currentTime) == TypeObjects.date && dateShort) {
                date = currentTime.getFromFormat("dd" + DateSplit + "MM" + DateSplit + "yyyy");
            }
            return date;
        }
    }
    return date;
}
function FormatDateFromUTC(date) {
    if (date != null) {
        date = date.toLocaleString();
        if (date.length > 10) {
            var date = date.substr(0, 10);
        }
    }
    return date;
}
function GetCurrentFromDate() {
    var currentTime = new Date();
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = day + DateSplit + month + DateSplit + year;
    return date;
}

function AddToDate(date, days, months, years) {
    var newDate = new Date();
    if (date != null && date.toString() != "") {
        if (date.indexOf("Date") > -1) {
            date = FormatDate(date);
        }
        var dateArray = date.toString().split(DateSplit);
        days = parseInt(days);
        if (dateArray == null) {
            throw { toString: function () { return "Error Fecha Invalida"; } };
        }
        else if (days != 0) {

            newDate = new Date(parseInt(dateArray[2]) + DateSplit + parseInt(dateArray[1]) + DateSplit + parseInt(dateArray[0]));
            newDate = AddDayToDate(newDate, days);
        }
        else if (parseInt(months) != 0) {
            newDate = new Date(parseInt(dateArray[2]) + DateSplit + parseInt(dateArray[1]) + DateSplit + parseInt(dateArray[0]));
            newDate = AddMonthToDate(newDate, parseInt(months));
        }
        else if (parseInt(years) != 0) {
            newDate = new Date(parseInt(dateArray[2]) + DateSplit + parseInt(dateArray[1]) + DateSplit + parseInt(dateArray[0]));
            newDate = AddYearToDate(newDate, parseInt(years));
        }
    }
    return newDate.getFromFormat(DateFormat);
}

function DownloadFile(urlFile, forceDownload, getFilename) {
    var link = document.createElement("a");
    document.body.appendChild(link);
    link.href = urlFile;
    link.download = urlFile.split('\\').pop();
    document.body.appendChild(link);

    if (forceDownload) {
        link.download = getFilename(urlFile);
        link.target = "_blank";
    }
    link.click();
    document.body.removeChild(link);
}

function AddDayToDate(date, days) {
    //Validar si es una Fecha
    if (!(date instanceof Date)) {
        console.log(date);
    }
    date.setDate(date.getDate() + days)
    return date;
}
function AddMonthToDate(date, month) {
    //Validar si es una Fecha
    if (!(date instanceof Date)) {
        console.log(date);
    }
    var m, d = (date = new Date(+date)).getDate();
    date.setMonth(date.getMonth() + month, 1)
    m = date.getMonth()
    date.setDate(d)
    if (date.getMonth() !== m) date.setDate(0)

    return date;
}
function AddYearToDate(date, year) {

    if (!(date instanceof Date)) {
        console.log(date);
    }
    date.setFullYear(date.getFullYear() + year);
    return date;
}
Date.prototype.getFromFormat = function (format) {
    if (toType(this) === TypeObjects.string) {
        var dateArray = this.split(/[-,./ ]/);
        if (dateArray == null) {
            throw { toString: function () { return "Error Fecha Invalida"; } };
        }
    }
    if (!(this instanceof Date)) {
        throw { toString: function () { return "Error Fecha Invalida"; } };
        console.log(date);
    }
    var yyyy = this.getFullYear().toString();
    format = format.replace(/yyyy/gi, yyyy)
    var mm = (this.getMonth() + 1).toString();
    format = format.replace(/mm/gi, (mm[1] ? mm : "0" + mm[0]));
    var dd = this.getDate().toString();
    format = format.replace(/dd/gi, (dd[1] ? dd : "0" + dd[0]));
    var hh = this.getHours().toString();
    format = format.replace(/hh/gi, (hh[1] ? hh : "0" + hh[0]));
    var ii = this.getMinutes().toString();
    format = format.replace(/ii/gi, (ii[1] ? ii : "0" + ii[0]));
    var ss = this.getSeconds().toString();
    format = format.replace(/ss/gi, (ss[1] ? ss : "0" + ss[0]));
    return format;
};

function ClearValidation(formName) {
    const form = $(formName);
    const validator = $(form).validate();
    $("[name]", form).each(function () {
        validator.successList.push(this);//mark as error free
        validator.showErrors();//remove error messages if present
    });
    validator.resetForm();//remove error class on name elements and clear history
    validator.reset();//remove all error and success data  
}

function LoadTime(selectId, amountItems) {
    $('#' + selectId).prop('disabled', false)
    var selectedTime = 0;
    for (var i = 0; i < amountItems; i++) {
        if (i < 10) {
            if (i == selectedTime) {
                $('#' + selectId).append($('<option>', {
                    value: i < 10 ? '0' + i : '' + i,
                    text: i < 10 ? '0' + i + ':00' : '' + i + ':00',
                    selected: true
                }));
            }
            else {
                $('#' + selectId).append($('<option>', {
                    value: i < 10 ? '0' + i : '' + i,
                    text: i < 10 ? '0' + i + ':00' : '' + i + ':00'
                }));
            }
        }
        else {
            $('#' + selectId).append($('<option>', {
                value: i,
                text: i + ':00'
            }));
        }
    }
}
/// Compara fecha entre dos fechas
function CompareBetweenDates(date, startDate, endDate) {
    if (date != null && startDate != null && endDate != null) {
        if (date.indexOf("Date") > -1) {
            date = FormatDate(date);
        }
        if (startDate.indexOf("Date") > -1) {
            startDate = FormatDate(startDate);
        }
        if (endDate.indexOf("Date") > -1) {
            endDate = FormatDate(endDate);
        }

        var datePart = date.split('/');
        var startDatePart = startDate.split('/');
        var endDatePart = endDate.split('/');

        var compareDate = new Date(datePart[2], datePart[1], datePart[0]);
        var compareStartDate = new Date(startDatePart[2], startDatePart[1], startDatePart[0]);
        var compareEndDate = new Date(endDatePart[2], endDatePart[1], endDatePart[0]);

        if (compareDate >= compareStartDate && compareDate <= compareEndDate) {
            return true;
        }

        return false;
    }

}
function ValidateMoney(input)
{ 
 var valid=true
 var decimals = (input.match(new RegExp("["+separatorDecimal+"]","g")) || []).length;
 var separator = (input.match(new RegExp("[" +separatorThousands + "]","g"))|| []).length;
 if(decimals>0)
  {  
    valid=false;
  }
  else if(separator>1)
  {
    valid=false;
  }
  return valid;
}
function ValidateNotMoney(input)
{ 
 var valid=false;
 var decimals = (input.match(new RegExp("[" + separatorDecimal  + "]","g")) || []).length;
 var separator = (input.match(new RegExp("[" +separatorThousands + "]","g"))|| []).length;
if(decimals>0)
{
valid = true;
}    
return valid;
}
