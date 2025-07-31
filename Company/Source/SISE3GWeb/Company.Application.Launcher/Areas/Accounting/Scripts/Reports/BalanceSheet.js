var timeReportBs = window.setInterval(getMassiveReportBS, 6000);
var isRunningTBReport;
var processTableRunninReportBs;
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

getMassiveReportBS();

$("#BSReportGenerate").on("click", function () {
    $("#alertBs").UifAlert('hide');

    $("#BSReportForm").validate();
    if ($("#BSReportForm").valid()) {
        if (validateUnChekAllBS()) {
            $("#alertBs").UifAlert('show', Resources.WarningSelectOneAccount, "warning");
        } else {

            lockScreen();

            setTimeout(function () {
                getTotalRecordBS();
            }, 300);
        }
    }
});

$("#BSReportCancel").on("click", function () {
    $('#BSReportYear').val("");
    $('#BSReportMonthFrom').val("");
    $('#BSReportMonthTo').val("");
    $("#alertBs").UifAlert('hide');
});

$("#BSReportModal").on("click", function () {
    $('#modalMassiveTotRecordsBS').modal('hide');

    processTableRunninReportBs = undefined;
    clearInterval(timeReportBs);

    var paramterModel = SetModelReportBs();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/BalanceSheetReports",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportBS();

                if (!isRunningTBReport) {
                    timeReportBs = window.setInterval(getMassiveReportBS, 6000);
                }
            } else {
                $("#alertBs").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#BSReportMonthFrom").on('itemSelected', function (event, selectedItem) {
    $("#alertBs").UifAlert('hide');
    if ($("#BSReportMonthTo").val() != '') {
        if (parseInt($("#BSReportMonthFrom").val()) != parseInt($("#BSReportMonthTo").val())) {
            if (parseInt($("#BSReportMonthFrom").val()) > parseInt($("#BSReportMonthTo").val())) {
                $("#alertBs").UifAlert('show', Resources.MessageValidateMonthFrom, "warning");
                $("#BSReportMonthFrom").val('');
            }
        }
    }
});

$("#BSReportMonthTo").on('itemSelected', function (event, selectedItem) {
    $("#alertBs").UifAlert('hide');
    if ($("#BSReportMonthFrom").val() != '') {
        if (parseInt($("#BSReportMonthTo").val()) != parseInt($("#BSReportMonthFrom").val())) {
            if (parseInt($("#BSReportMonthTo").val()) < parseInt($("#BSReportMonthFrom").val())) {
                $("#alertBs").UifAlert('show', Resources.MessageValidateMonthTo, "warning");
                $("#BSReportMonthTo").val('');
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertBs").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunninReportBs = undefined;
            clearInterval(timeReportBs);

            generateFileBS(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkBS(data.UrlFile);
    } else {
        $("#alertBs").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

$('#checkAccumulatedBS').click(function () {
    $("#alertBs").UifAlert('hide');
    if ($('#checkAccumulatedBS').hasClass('glyphicon glyphicon-check')) {
        uncheckBs('#checkAccumulatedBS');
    }
    else {
        checkBs('#checkAccumulatedBS');
    }
});

$('#checkAllBS').click(function () {
    $("#alertBs").UifAlert('hide');
    if ($('#checkAllBS').hasClass('glyphicon glyphicon-check')) {
        uncheckBs('#checkAllBS');

        if ($('#checkAssetsBS').hasClass('glyphicon glyphicon-check')) {
            uncheckBs('#checkAssetsBS');
        }
        if ($('#checkLiabilitiesBS').hasClass('glyphicon glyphicon-check')) {
            uncheckBs('#checkLiabilitiesBS');
        }
        if ($('#checkPatrimonyBS').hasClass('glyphicon glyphicon-check')) {
            uncheckBs('#checkPatrimonyBS');
        }
        if ($('#checkIncomeBS').hasClass('glyphicon glyphicon-check')) {
            uncheckBs('#checkIncomeBS');
        }
        if ($('#checkExpensesBS').hasClass('glyphicon glyphicon-check')) {
            uncheckBs('#checkExpensesBS');
        }
    }
    else {
        checkBs('#checkAllBS');
        if ($('#checkAssetsBS').hasClass('glyphicon glyphicon-unchecked')) {
            checkBs('#checkAssetsBS');
        }
        if ($('#checkLiabilitiesBS').hasClass('glyphicon glyphicon-unchecked')) {
            checkBs('#checkLiabilitiesBS');
        }
        if ($('#checkPatrimonyBS').hasClass('glyphicon glyphicon-unchecked')) {
            checkBs('#checkPatrimonyBS');
        }
        if ($('#checkIncomeBS').hasClass('glyphicon glyphicon-unchecked')) {
            checkBs('#checkIncomeBS');
        }
        if ($('#checkExpensesBS').hasClass('glyphicon glyphicon-unchecked')) {
            checkBs('#checkExpensesBS');
        }
    }
});

$('#checkAssetsBS').click(function () {
    $("#alertBs").UifAlert('hide');
    if ($('#checkAssetsBS').hasClass('glyphicon glyphicon-check')) {
        uncheckBs('#checkAssetsBS');
        uncheckBs('#checkAllBS');
    }
    else {
        checkBs('#checkAssetsBS');
        if (validateChekAllBS()) {
            checkBs('#checkAllBS');
        }
    }
});

$('#checkLiabilitiesBS').click(function () {
    $("#alertBs").UifAlert('hide');
    if ($('#checkLiabilitiesBS').hasClass('glyphicon glyphicon-check')) {
        uncheckBs('#checkLiabilitiesBS');
        uncheckBs('#checkAllBS');
    }
    else {
        checkBs('#checkLiabilitiesBS');
        if (validateChekAllBS()) {
            checkBs('#checkAllBS');
        }
    }
});

$('#checkPatrimonyBS').click(function () {
    $("#alertBs").UifAlert('hide');
    if ($('#checkPatrimonyBS').hasClass('glyphicon glyphicon-check')) {
        uncheckBs('#checkPatrimonyBS');
        uncheckBs('#checkAllBS');
    }
    else {
        checkBs('#checkPatrimonyBS');
        if (validateChekAllBS()) {
            checkBs('#checkAllBS');
        }
    }
});

$('#checkIncomeBS').click(function () {
    $("#alertBs").UifAlert('hide');
    if ($('#checkIncomeBS').hasClass('glyphicon glyphicon-check')) {
        uncheckBs('#checkIncomeBS');
        uncheckBs('#checkAllBS');
    }
    else {
        checkBs('#checkIncomeBS');
        if (validateChekAllBS()) {
            checkBs('#checkAllBS');
        }
    }
});

$('#checkExpensesBS').click(function () {
    $("#alertBs").UifAlert('hide');
    if ($('#checkExpensesBS').hasClass('glyphicon glyphicon-check')) {
        uncheckBs('#checkExpensesBS');
        uncheckBs('#checkAllBS');
    }
    else {
        checkBs('#checkExpensesBS');
        if (validateChekAllBS()) {
            checkBs('#checkAllBS');
        }
    }
});


function getTotalRecordBS() {
    var paramterModel = SetModelReportBs();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/GetTotalRecordsBalanceSheet",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsBS').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertBs").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertBs").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

function getMassiveReportBS() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.BalanceSheet.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunninReportBs = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunninReportBs != undefined) {
        if (validateProcessReport(processTableRunninReportBs, processTableRunninReportBs.length)) {
            clearInterval(timeReportBs);
            isRunningTBReport = false;
            $("#alertBs").UifAlert('hide');
        } else {
            isRunningTBReport = true;
        }
    }
};

function generateFileBS(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertBs").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportBS();
                    if (!isRunningTBReport) {
                        timeReportBs = window.setInterval(getMassiveReportBS, 6000);
                    }
                }
            } else {
                $("#alertBs").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}


function SetDownloadLinkBS(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertBs", "selectFileTypePartial");
}

function checkBs(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-unchecked");
    $(checkAccount).addClass("glyphicon glyphicon-check");
}

function uncheckBs(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-check");
    $(checkAccount).addClass("glyphicon glyphicon-unchecked");
}

function validateChekAllBS() {
    if ($('#checkAssetsBS').hasClass('glyphicon glyphicon-check') && $('#checkLiabilitiesBS').hasClass('glyphicon glyphicon-check') &&
        $('#checkPatrimonyBS').hasClass('glyphicon glyphicon-check') && $('#checkIncomeBS').hasClass('glyphicon glyphicon-check') &&
        $('#checkExpensesBS').hasClass('glyphicon glyphicon-check')) {
        return true;
    }
    else {
        return false
    }
}

function validateUnChekAllBS() {
    if ($('#checkAssetsBS').hasClass('glyphicon glyphicon-unchecked') && $('#checkLiabilitiesBS').hasClass('glyphicon glyphicon-unchecked') &&
        $('#checkPatrimonyBS').hasClass('glyphicon glyphicon-unchecked') && $('#checkIncomeBS').hasClass('glyphicon glyphicon-unchecked') &&
        $('#checkExpensesBS').hasClass('glyphicon glyphicon-unchecked')) {
        return true;
    }
    else {
        return false
    }
}

function SetModelReportBs() {
    return {
        Branch: $("#BSReportBranch").val(),
        Month: $("#BSReportMonthFrom").val(),
        Year: $("#BSReportYear").val(),
        MonthTo: $("#BSReportMonthTo").val(),
        Assets: ($('#checkAssetsBS').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagAssets").val() : 0,
        Liabilities: ($('#checkLiabilitiesBS').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagLiabilities").val() : 0,
        Patrimony: ($('#checkPatrimonyBS').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagPatrimony").val() : 0,
        Income: ($('#checkIncomeBS').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagIncome").val() : 0,
        Expenses: ($('#checkExpensesBS').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagExpenses").val() : 0,
        Operation: $('input:radio[name=options]:checked').val() == 'D' ? 0 : 1,
        Accumulated: ($('#checkAccumulatedBS').hasClass('glyphicon glyphicon-check')) ? 1 : 0,
        Name: Resources.BalanceSheet
    }
}
