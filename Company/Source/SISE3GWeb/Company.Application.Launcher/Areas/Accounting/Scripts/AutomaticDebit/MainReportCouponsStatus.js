//////////////////////////////////////
// Fecha Proceso Desde            ///
//////////////////////////////////////
$('#couponStatusDateProcessFrom').on("datepicker.change", function (event, date) {
    $("#alert").UifAlert('hide');
    if (IsDate($('#couponStatusDateProcessFrom').val())) {
        if ($("#couponStatusDateProcessTo").val() != "") {
            if (CompareDates($('#couponStatusDateProcessFrom').val(), $("#couponStatusDateProcessTo").val()) == true) {
                $("#alert").UifAlert('show', Resources.MessageValidateProcessDateFrom, "warning");
                return true;
            }
        }
    }
    else {
        $("#alert").UifAlert('show', Resources.EntryDateProcessFrom, "warning");
    }

    setTimeout(function () {
        $("#alert").UifAlert('hide');
    }, 3000);
});

$("#couponStatusDateProcessFrom").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#couponStatusDateProcessFrom").val() != '') {

        if (IsDate($("#couponStatusDateProcessFrom").val()) == true) {
            if ($("#couponStatusDateProcessTo").val() != '') {
                if (CompareDates($("#couponStatusDateProcessFrom").val(), $("#couponStatusDateProcessTo").val())) {
                    $("#alert").UifAlert('show', Resources.MessageValidateProcessDateFrom, "warning");
                    $("#couponStatusDateProcessFrom").val("");
                    return true;
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessFrom, "warning");
            $("#couponStatusDateProcessFrom").val(date);
        }
    }
});

//////////////////////////////////////
/// Fecha Proceso Hasta            ///
//////////////////////////////////////
$('#couponStatusDateProcessTo').on("datepicker.change", function (event, date) {
    $("#alert").UifAlert('hide');
    if (IsDate($('#couponStatusDateProcessTo').val())) {
        if ($('#couponStatusDateProcessFrom').val() != "") {
            if (CompareDates($('#couponStatusDateProcessFrom').val(), $("#couponStatusDateProcessTo").val()) == true) {
                $("#alert").UifAlert('show', Resources.MessageValidateProcessDateTo, "warning");
                return true;
            }
        }
    }
    else {
        $("#alert").UifAlert('show', Resources.EntryDateProcessTo, "warning");
    }

    setTimeout(function () {
        $("#alert").UifAlert('hide');
    }, 3000);
});

$("#couponStatusDateProcessTo").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#couponStatusDateProcessTo").val() != '') {

        if (IsDate($("#couponStatusDateProcessTo").val()) == true) {
            if ($("#couponStatusDateProcessFrom").val() != '') {
                if (CompareDates($("#couponStatusDateProcessFrom").val(), $("#couponStatusDateProcessTo").val())) {
                    $("#alert").UifAlert('show', Resources.MessageValidateProcessDateTo, "warning");
                    $("#couponStatusDateProcessTo").val("");
                    return true;
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessTo, "warning");
            $("#couponStatusDateProcessTo").val("");
        }
    }
});

////////////////////////////////////
/// Botón Generar Reporte        ///
////////////////////////////////////
$('#couponStatusGenerate').click(function () {
    $("#formCouponStatus").validate();

    if ($("#formCouponStatus").valid()) {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        var reportType = $('input:radio[name=options]:checked').val();

        if ($('#couponStatusDateProcessFrom').val() != "") {
            if ($('#couponStatusDateProcessTo').val() != "") {
                if (CompareDates($('#couponStatusDateProcessFrom').val(), $('#couponStatusDateProcessTo').val()) == false) {
                    CouponStatusExportReportToExcel();
                }
                else {
                    $("#alert").UifAlert('show', Resources.MessageValidateProcessDateFrom, "danger");
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.EntryDateProcessTo, "danger");
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessFrom, "danger");
        }

        setTimeout(function () {
            $("#alert").UifAlert('hide');
        }, 3000);
    }
});

////////////////////////////////////
/// Botón Cancelar               ///
////////////////////////////////////
$('#couponStatusCancel').click(function () {
    $('#couponStatusDateProcessFrom').val("");
    $('#couponStatusDateProcessTo').val("");
});

//////////////////////////////////////////////
/// Funciones                              ///
//////////////////////////////////////////////
function CouponStatusExportReportToExcel() {
    var url = ACC_ROOT + "AutomaticDebit/GenerateReportCouponsStatus?dateFrom=" + $('#couponStatusDateProcessFrom').val() + "&dateTo=" +
          $("#couponStatusDateProcessTo").val();
    var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
}