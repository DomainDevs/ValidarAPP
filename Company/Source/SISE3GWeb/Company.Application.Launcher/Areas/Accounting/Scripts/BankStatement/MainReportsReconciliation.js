$('#selectAccountTypeReports').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "BankStatement/GetAccountNumbers?bankId=" + $('#selectBankReports').val() + "&accountTypeId=" + selectedItem.Id;
        $("#selectAccountNumberReports").UifSelect({ source: controller });

    }
    else {
        $("#selectAccountNumberReports").UifSelect();
    }
});

$('#selectBankReports').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        $("#selectAccountTypeReports").val('');
        $("#selectAccountTypeReports").trigger('change');
        $("#selectAccountNumberReports").val('');
        $("#selectAccountNumberReports").trigger('change');
    }
});

//////////////////////////////////////
// Botón Cancelar                   //
//////////////////////////////////////
$('#btnCancelReports').click(function () {
    $("#selectBankReports").val("");
    $("#selectAccountTypeReports").val("");
    $("#selectAccountNumberReports").val("");
    $("#DateToReports").val("");
});

/// Reporte de Pendiente de Bancos 
$("#btnPrintReports").click(function () {

    $("#formPendingBanks").validate();

    if ($("#formPendingBanks").valid()) {

        if ($("#selectBankReports").val != "") {
            var accountType = ($("#selectAccountTypeReports").val() == "") ? -1 : $("#selectAccountTypeReports").val();
            if (accountType == null) {
                accountType = -1;
            }
            ShowReconciliationReports($("#selectOptionsReports").val(), $("#selectBankReports").val(), accountType, $("#selectAccountNumberReports").val(), $("#DateToReports").val());
            showReportReports();
        }
    }
});


// Visualiza el reporte de Pendiente de Bancos en pantalla
function ShowReconciliationReports(optionReports, bankIdReports, accountTypeReports, accountNumber, dateToReports) {

    if (optionReports == 1) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "BankStatement/GetConciliatedMovements",
            data: { "option": optionReports, "bankId": bankIdReports, "accountType": accountTypeReports, "accountNumber": $("#selectAccountNumberReports option:selected").html(), "dateTo": dateToReports },
            success: function (data) { }
        });
    }
    else {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "BankStatement/GetPendingBanks",
            data: { "option": optionReports, "bankId": bankIdReports, "accountType": accountTypeReports, "accountNumber": $("#selectAccountNumberReports option:selected").html(), "dateTo": dateToReports },
            success: function (data) { }
        });
    }
}

function showReportReports() {
    window.open(ACC_ROOT + "BankStatement/ShowPendingBanks", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}