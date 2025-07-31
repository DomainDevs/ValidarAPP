var timeReportAccountInventory = window.setInterval(getMassiveReportAccountInventory, 6000);
var isRunningAccountInventoryReport;
var processTableRunninReportAccountInventory;
var ReportTypeId = 0;
var oParameterModel = {
    Id: 0,
    Name: "",
    Description: "",
    IsObject: false,
    Branch: 0,
    DateFrom: null,
    DateTo: null,
    Month: null,
    Year: null,
    MonthTo: null,
    Accumulated: null,
    All: null,
    Assets: null,
    Liabilities: null,
    Patrimony: null,
    MemorandumAccounts: null,
    Income: null,
    Expenses: null,
    ContingentAccount: null,
    ContingentAccountTwo: null,
    MemorandumAccountsTwo: null,
    Operation: 0
};

getMassiveReportAccountInventory();

$("#GenerateInventory").on("click", function () {
    $("#alertAccountInventory").UifAlert('hide');

    $("#ReportFormAccountInventory").validate();
    if ($("#ReportFormAccountInventory").valid()) {
        if (validateUnChekAllAccountInventory()) {
            $("#alertAccountInventory").UifAlert('show', Resources.WarningSelectOneAccount, "warning");
        } else {
            getTotalRecordAccountInventory();
        }
    }
});

$("#CancelInventory").on("click", function () {
    $('#YearAccountInventory').val("");
    $('#MonthFromAccountInventory').val("");
    $('#MonthToAccountInventory').val("");
    $("#alertAccountInventory").UifAlert('hide');
});

$("#btnModalAccountInventory").on("click", function () {
    $('#modalMassiveTotRecords').modal('hide');

    processTableRunninReportAccountInventory = undefined;
    clearInterval(timeReportAccountInventory);

    var paramterModel = SetModelReportAccountInventory();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/AccountInventoryReports",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportAccountInventory();

                if (!isRunningAccountInventoryReport) {
                    timeReportAccountInventory = window.setInterval(getMassiveReportAccountInventory, 6000);
                }
            } else {
                $("#alertAccountInventory").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#MonthFromAccountInventory").on('itemSelected', function (event, selectedItem) {
    $("#alertAccountInventory").UifAlert('hide');
    if ($("#MonthToAccountInventory").val() != '') {
        if (parseInt($("#MonthFromAccountInventory").val()) != parseInt($("#MonthToAccountInventory").val())) {
            if (parseInt($("#MonthFromAccountInventory").val()) > parseInt($("#MonthToAccountInventory").val())) {
                $("#alertAccountInventory").UifAlert('show', Resources.MessageValidateMonthFrom, "warning");
                $("#MonthFromAccountInventory").val('');
            }
        }
    }
});

$("#MonthToAccountInventory").on('itemSelected', function (event, selectedItem) {
    $("#alertAccountInventory").UifAlert('hide');
    if ($("#MonthFromAccountInventory").val() != '') {
        if (parseInt($("#MonthToAccountInventory").val()) != parseInt($("#MonthFromAccountInventory").val())) {
            if (parseInt($("#MonthToAccountInventory").val()) < parseInt($("#MonthFromAccountInventory").val())) {
                $("#alertAccountInventory").UifAlert('show', Resources.MessageValidateMonthTo, "warning");
                $("#MonthToAccountInventory").val('');
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertAccountInventory").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunninReportAccountInventory = undefined;
            clearInterval(timeReportAccountInventory);

            generateFileAccountInventory(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkAccountInventory(data.UrlFile);
    } else {
        $("#alertAccountInventory").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

$('#checkAccumulatedAccountInventory').click(function () {
    $("#alertAccountInventory").UifAlert('hide');
    if ($('#checkAccumulatedAccountInventory').hasClass('glyphicon glyphicon-check')) {
        uncheckAccountInventory('#checkAccumulatedAccountInventory');
    }
    else {
        checkAccountInventory('#checkAccumulatedAccountInventory');
    }
});

$('#checkAllAccountInventory').click(function () {
    $("#alertAccountInventory").UifAlert('hide');
    if ($('#checkAllAccountInventory').hasClass('glyphicon glyphicon-check')) {
        uncheckAccountInventory('#checkAllAccountInventory');

        if ($('#checkAssetsAccountInventory').hasClass('glyphicon glyphicon-check')) {
            uncheckAccountInventory('#checkAssetsAccountInventory');
        }
        if ($('#checkLiabilitiesAccountInventory').hasClass('glyphicon glyphicon-check')) {
            uncheckAccountInventory('#checkLiabilitiesAccountInventory');
        }
        if ($('#checkPatrimonyAccountInventory').hasClass('glyphicon glyphicon-check')) {
            uncheckAccountInventory('#checkPatrimonyAccountInventory');
        }
        if ($('#checkIncomeAccountInventory').hasClass('glyphicon glyphicon-check')) {
            uncheckAccountInventory('#checkIncomeAccountInventory');
        }
        if ($('#checkExpensesAccountInventory').hasClass('glyphicon glyphicon-check')) {
            uncheckAccountInventory('#checkExpensesAccountInventory');
        }
    }
    else {
        checkAccountInventory('#checkAllAccountInventory');
        if ($('#checkAssetsAccountInventory').hasClass('glyphicon glyphicon-unchecked')) {
            checkAccountInventory('#checkAssetsAccountInventory');
        }
        if ($('#checkLiabilitiesAccountInventory').hasClass('glyphicon glyphicon-unchecked')) {
            checkAccountInventory('#checkLiabilitiesAccountInventory');
        }
        if ($('#checkPatrimonyAccountInventory').hasClass('glyphicon glyphicon-unchecked')) {
            checkAccountInventory('#checkPatrimonyAccountInventory');
        }
        if ($('#checkIncomeAccountInventory').hasClass('glyphicon glyphicon-unchecked')) {
            checkAccountInventory('#checkIncomeAccountInventory');
        }
        if ($('#checkExpensesAccountInventory').hasClass('glyphicon glyphicon-unchecked')) {
            checkAccountInventory('#checkExpensesAccountInventory');
        }
    }
});

$('#checkAssetsAccountInventory').click(function () {
    $("#alertAccountInventory").UifAlert('hide');
    if ($('#checkAssetsAccountInventory').hasClass('glyphicon glyphicon-check')) {
        uncheckAccountInventory('#checkAssetsAccountInventory');
        uncheckAccountInventory('#checkAllAccountInventory');
    }
    else {
        checkAccountInventory('#checkAssetsAccountInventory');
        if (validateChekAllAccountInventory()) {
            checkAccountInventory('#checkAllAccountInventory');
        }
    }
});

$('#checkLiabilitiesAccountInventory').click(function () {
    $("#alertAccountInventory").UifAlert('hide');
    if ($('#checkLiabilitiesAccountInventory').hasClass('glyphicon glyphicon-check')) {
        uncheckAccountInventory('#checkLiabilitiesAccountInventory');
        uncheckAccountInventory('#checkAllAccountInventory');
    }
    else {
        checkAccountInventory('#checkLiabilitiesAccountInventory');
        if (validateChekAllAccountInventory()) {
            checkAccountInventory('#checkAllAccountInventory');
        }
    }
});

$('#checkPatrimonyAccountInventory').click(function () {
    $("#alertAccountInventory").UifAlert('hide');
    if ($('#checkPatrimonyAccountInventory').hasClass('glyphicon glyphicon-check')) {
        uncheckAccountInventory('#checkPatrimonyAccountInventory');
        uncheckAccountInventory('#checkAllAccountInventory');
    }
    else {
        checkAccountInventory('#checkPatrimonyAccountInventory');
        if (validateChekAllAccountInventory()) {
            checkAccountInventory('#checkAllAccountInventory');
        }
    }
});

$('#checkIncomeAccountInventory').click(function () {
    $("#alertAccountInventory").UifAlert('hide');
    if ($('#checkIncomeAccountInventory').hasClass('glyphicon glyphicon-check')) {
        uncheckAccountInventory('#checkIncomeAccountInventory');
        uncheckAccountInventory('#checkAllAccountInventory');
    }
    else {
        checkAccountInventory('#checkIncomeAccountInventory');
        if (validateChekAllAccountInventory()) {
            checkAccountInventory('#checkAllAccountInventory');
        }
    }
});

$('#checkExpensesAccountInventory').click(function () {
    $("#alertAccountInventory").UifAlert('hide');
    if ($('#checkExpensesAccountInventory').hasClass('glyphicon glyphicon-check')) {
        uncheckAccountInventory('#checkExpensesAccountInventory');
        uncheckAccountInventory('#checkAllAccountInventory');
    }
    else {
        checkAccountInventory('#checkExpensesAccountInventory');
        if (validateChekAllAccountInventory()) {
            checkAccountInventory('#checkAllAccountInventory');
        }
    }
});


function getTotalRecordAccountInventory() {
    var paramterModel = SetModelReportAccountInventory();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/GetTotalRecordsAccountInventory",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertAccountInventory").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertAccountInventory").UifAlert('show', Resources.MessageInternalError, "danger");
            }
        }
    });
}

function getMassiveReportAccountInventory() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.AccountInventory.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunninReportAccountInventory = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunninReportAccountInventory != undefined) {
        if (validateProcessReport(processTableRunninReportAccountInventory, processTableRunninReportAccountInventory.length)) {
            clearInterval(timeReportAccountInventory);
            isRunningAccountInventoryReport = false;
            $("#alertAccountInventory").UifAlert('hide');
        } else {
            isRunningAccountInventoryReport = true;
        }
    }
};

function generateFileAccountInventory(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertAccountInventory").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportAccountInventory();
                    if (!isRunningAccountInventoryReport) {
                        timeReportAccountInventory = window.setInterval(getMassiveReportAccountInventory, 6000);
                    }
                }
            } else {
                $("#alertAccountInventory").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkAccountInventory(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertAccountInventory", "selectFileTypePartial");
}


function checkAccountInventory(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-unchecked");
    $(checkAccount).addClass("glyphicon glyphicon-check");
}

function uncheckAccountInventory(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-check");
    $(checkAccount).addClass("glyphicon glyphicon-unchecked");
}

function validateChekAllAccountInventory() {
    if ($('#checkAssetsAccountInventory').hasClass('glyphicon glyphicon-check') && $('#checkLiabilitiesAccountInventory').hasClass('glyphicon glyphicon-check') &&
        $('#checkPatrimonyAccountInventory').hasClass('glyphicon glyphicon-check') && $('#checkIncomeAccountInventory').hasClass('glyphicon glyphicon-check') &&
        $('#checkExpensesAccountInventory').hasClass('glyphicon glyphicon-check')) {
        return true;
    }
    else {
        return false
    }
}

function validateUnChekAllAccountInventory() {
    if ($('#checkAssetsAccountInventory').hasClass('glyphicon glyphicon-unchecked') && $('#checkLiabilitiesAccountInventory').hasClass('glyphicon glyphicon-unchecked') &&
        $('#checkPatrimonyAccountInventory').hasClass('glyphicon glyphicon-unchecked') && $('#checkIncomeAccountInventory').hasClass('glyphicon glyphicon-unchecked') &&
        $('#checkExpensesAccountInventory').hasClass('glyphicon glyphicon-unchecked')) {
        return true;
    }
    else {
        return false
    }
}

function SetModelReportAccountInventory() {
    return {
        Branch: $("#BranchAccountInventory").val(),
        Month: $("#MonthFromAccountInventory").val(),
        Year: $("#YearAccountInventory").val(),
        MonthTo: $("#MonthToAccountInventory").val(),
        Assets: ($('#checkAssetsAccountInventory').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagAssets").val() : 0,
        Liabilities: ($('#checkLiabilitiesAccountInventory').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagLiabilities").val() : 0,
        Patrimony: ($('#checkPatrimonyAccountInventory').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagPatrimony").val() : 0,
        Income: ($('#checkIncomeAccountInventory').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagIncome").val() : 0,
        Expenses: ($('#checkExpensesAccountInventory').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagExpenses").val() : 0,
        Operation: $('input:radio[name=options]:checked').val() == 'D' ? 0 : 1,
        Accumulated: ($('#checkAccumulatedAccountInventory').hasClass('glyphicon glyphicon-check')) ? 1 : 0,
        Name: Resources.AccountInventory
    }
}
