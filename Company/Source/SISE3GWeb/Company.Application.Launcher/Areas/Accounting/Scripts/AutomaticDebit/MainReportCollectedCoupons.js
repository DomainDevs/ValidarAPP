var collectedCouponsAgentId = 0;

//////////////////////////////////////
/// Fecha Proceso Desde            ///
//////////////////////////////////////
$('#collectedCouponsDateProcessFrom').on("datepicker.change", function (event, date) {
    $("#alert").UifAlert('hide');
    if (IsDate($('#collectedCouponsDateProcessFrom').val())) {
        if ($("#collectedCouponsDateProcessTo").val() != "")
        {
            if (CompareDates($('#collectedCouponsDateProcessFrom').val(), $("#collectedCouponsDateProcessTo").val()) == true) {
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

$("#collectedCouponsDateProcessFrom").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#collectedCouponsDateProcessFrom").val() != '') {

        if (IsDate($("#collectedCouponsDateProcessFrom").val()) == true) {
            if ($("#collectedCouponsDateProcessTo").val() != '') {
                if (CompareDates($("#collectedCouponsDateProcessFrom").val(), $("#collectedCouponsDateProcessTo").val())) {
                    $("#alert").UifAlert('show', Resources.MessageValidateProcessDateFrom, "warning");
                    $("#collectedCouponsDateProcessFrom").val("");
                    return true;
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessFrom, "warning");
            $("#collectedCouponsDateProcessFrom").val(date);
        }
    }
});

//////////////////////////////////////
/// Fecha Proceso Hasta            ///
//////////////////////////////////////
$('#collectedCouponsDateProcessTo').on("datepicker.change", function (event, date) {
    $("#alert").UifAlert('hide');
    if (IsDate($('#collectedCouponsDateProcessTo').val())) {
        if ($('#collectedCouponsDateProcessFrom').val() != "")
        {
            if (CompareDates($('#collectedCouponsDateProcessFrom').val(), $("#collectedCouponsDateProcessTo").val()) == true) {
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

$("#collectedCouponsDateProcessTo").blur(function () {
    $("#alert").UifAlert('hide');

    if ($("#collectedCouponsDateProcessTo").val() != '') {

        if (IsDate($("#collectedCouponsDateProcessTo").val()) == true) {
            if ($("#collectedCouponsDateProcessFrom").val() != '') {
                if (CompareDates($("#collectedCouponsDateProcessFrom").val(), $("#collectedCouponsDateProcessTo").val())) {
                    $("#alert").UifAlert('show', Resources.MessageValidateProcessDateTo, "warning");
                    $("#collectedCouponsDateProcessTo").val("");
                    return true;
                }
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.EntryDateProcessTo, "warning");
            $("#collectedCouponsDateProcessTo").val("");
        }
    }
});

////////////////////////////////////
/// Botón Generar Reporte        ///
////////////////////////////////////
$('#collectedCouponsGenerate').click(function () {
    $("#formCollectedCoupons").validate();

    if ($("#formCollectedCoupons").valid()) {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        var reportType = $('input:radio[name=options]:checked').val();

        if ($('#collectedCouponsDateProcessFrom').val() != "") {
            if ($('#collectedCouponsDateProcessTo').val() != "") {
                if (CompareDates($('#collectedCouponsDateProcessFrom').val(), $('#collectedCouponsDateProcessTo').val()) == false) {
                    CollectedCouponsExportReportToExcel();
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
$('#collectedCouponsCancel').click(function () {
    $('#collectedCouponsPrefix').val("");
    $('#collectedCouponsAgent').val("");
    $("#collectedCouponsCollected").prop("checked", true);
    $("#collectedCouponsUncollected").prop("checked", false);
    $("#collectedCouponsAll").prop("checked", false);
});

////////////////////////////////////
/// Autocomplete agente          ///
////////////////////////////////////
$('#collectedCouponsAgent').on('itemSelected', function (event, selectedItem) {
    collectedCouponsAgentId = selectedItem.IndividualId;
});

//////////////////////////////////////////////
/// Funciones                              ///     
//////////////////////////////////////////////
function CollectedCouponsExportReportToExcel() {
    var reportType = $('input:radio[name=options]:checked').val();
    var prefixId = $("#collectedCouponsPrefix").val();
    if (prefixId == "") {
        prefixId = "0";
    }

    if ($('#collectedCouponsAgent').val() == "") {
        collectedCouponsAgentId = 0;
    }

    var url = ACC_ROOT + "AutomaticDebit/GenerateReportCollectedCoupons?dateFrom=" + $('#collectedCouponsDateProcessFrom').val() + "&dateTo=" +
          $("#collectedCouponsDateProcessTo").val() + "&prefixId=" + prefixId + "&agentId=" + collectedCouponsAgentId + "&reportType=" + reportType;
    var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
    setTimeout(function () {
        newPage.open('', '_self', '');
    }, 100);
}