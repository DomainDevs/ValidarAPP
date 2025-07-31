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