var DateFormat = new String("dd/mm/yyyy");
var DateSplit = "/";

function CalculateDays(from, to) {
    var date1 = from.toString().split('/');
    var date2 = to.toString().split('/');
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

function DownloadFile(urlFile) {
    var link = document.createElement("a");
    link.href = urlFile;
    link.target = "_blank";
    link.click();
}

//Summary: Comparar dos fechas
//Return: 1 = La fecha 1 es mayor a la fecha 2 | 0 = La fecha 1 es menor
function CompareDates(date1, date2) {
    var resultCompare = 0;

    if (date1 != null && date2 != null) {
        var datePart1 = date1.split('/');
        var datePart2 = date2.split('/');
        var compare1 = new Date(datePart1[2], datePart1[1] - 1, datePart1[0]);
        var compare2 = new Date(datePart2[2], datePart2[1] - 1, datePart2[0]);
        var compare3 = new Date(1930, 01, 01);

        if (compare1 > compare2 || compare1 < compare3) {
            resultCompare = 1;
        }
    }
    return resultCompare;
}
//Summary: Comparar dos años
//Return: 1 = La fecha 1 es mayor a la fecha 2 | 0 = La fecha 1 es menor
function CompareYear(date1, date2) {
    var resultCompare = 0;

    if (date1 != null && date2 != null) {  
        var datePart2 = date2.split('/');
        var compare2 = datePart2[2];

        if (parseInt(date1) > parseInt(compare2))  {
            resultCompare = 1;
        }
    }
    return resultCompare;
}

function AddToDate(date, months) {
    var newDate = new Date();
    if (date != null && date.toString() != "") {
        var dateArray = date.toString().split(DateSplit);       
        if (dateArray == null) {
            throw { toString: function () { return "Error Fecha Invalida"; } };
        }      
        else if(parseInt(months) != 0) {
            newDate = new Date(parseInt(dateArray[2]) + DateSplit + parseInt(dateArray[1]) + DateSplit + parseInt(dateArray[0]));
            newDate = AddMonthToDate(newDate, parseInt(months));
        }
       
    }
    return newDate.getFromFormat(DateFormat);
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
Date.prototype.getFromFormat = function (format) {
    var yyyy = this.getFullYear().toString();
    format = format.replace(/yyyy/g, yyyy)
    var mm = (this.getMonth() + 1).toString();
    format = format.replace(/mm/g, (mm[1] ? mm : "0" + mm[0]));
    var dd = this.getDate().toString();
    format = format.replace(/dd/g, (dd[1] ? dd : "0" + dd[0]));
    var hh = this.getHours().toString();
    format = format.replace(/hh/g, (hh[1] ? hh : "0" + hh[0]));
    var ii = this.getMinutes().toString();
    format = format.replace(/ii/g, (ii[1] ? ii : "0" + ii[0]));
    var ss = this.getSeconds().toString();
    format = format.replace(/ss/g, (ss[1] ? ss : "0" + ss[0]));
    return format;
};
