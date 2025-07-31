var timeTBReport = window.setInterval(getMassiveReportTBReport, 6000);
var isRunningTBReport;
var processTableRunningTBReport;
var reportName = Resources.TrialBalanceTotal;
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

getMassiveReportTBReport();

$("#TBReportGenerate").on("click", function () {
    $("#alertFormTB").UifAlert('hide');

    $("#TBReportForm").validate();
    if ($("#TBReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            getTotalRecordTB();
        }, 300);
    }
});

$("#TBReportCancel").on("click", function () {
    $('#TBReportYear').val("");
    $('#TBReportMonthFrom').val("");
    $('#TBReportMonthTo').val("");
    $("#alertFormTB").UifAlert('hide');
});

$("#TBReportModal").on("click", function () {
    $('#modalMassiveTotRecordsTb').modal('hide');

    processTableRunningTBReport = undefined;
    clearInterval(timeTBReport);

    var paramterModel = SetTBReportFilter();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/TrialBalanceReports",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportTBReport();

                if (!isRunningTBReport) {
                    timeTBReport = window.setInterval(getMassiveReportTBReport, 6000);
                }
            } else {
                $("#alertFormTB").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#TBReportMonthFrom").on('itemSelected', function (event, selectedItem) {
    $("#alertFormTB").UifAlert('hide');
    if ($("#TBReportMonthTo").val() != '') {
        if (parseInt($("#TBReportMonthFrom").val()) != parseInt($("#TBReportMonthTo").val())) {
            if (parseInt($("#TBReportMonthFrom").val()) > parseInt($("#TBReportMonthTo").val())) {
                $("#alertFormTB").UifAlert('show', Resources.MessageValidateMonthFrom, "warning");
                $("#TBReportMonthFrom").val('');
            }
        }
    }
});

$("#TBReportMonthTo").on('itemSelected', function (event, selectedItem) {
    $("#alertFormTB").UifAlert('hide');
    if ($("#TBReportMonthFrom").val() != '') {
        if (parseInt($("#TBReportMonthTo").val()) != parseInt($("#TBReportMonthFrom").val())) {
            if (parseInt($("#TBReportMonthTo").val()) < parseInt($("#TBReportMonthFrom").val())) {
                $("#alertFormTB").UifAlert('show', Resources.MessageValidateMonthTo, "warning");
                $("#TBReportMonthTo").val('');
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertFormTB").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningTBReport = undefined;
            clearInterval(timeTBReport);

            generateFileTB(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkTB(data.UrlFile);
    } else {
        $("#alertFormTB").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

$('#checkAllTB').click(function () {
    if ($('#checkAllTB').hasClass('glyphicon glyphicon-check')) {
        uncheck('#checkAllTB');
        uncheck('#checkAssetsTB');
        uncheck('#checkLiabilitiesTB');
        uncheck('#checkPatrimonyTB');
        uncheck('#checkIncomeTB');
        uncheck('#checkExpensesTB');
    }
    else {
        check('#checkAllTB');
        check('#checkAssetsTB');
        check('#checkLiabilitiesTB');
        check('#checkPatrimonyTB');
        check('#checkIncomeTB');
        check('#checkExpensesTB');
    }
});

$('#checkAssetsTB').click(function () {
    if ($('#checkAssetsTB').hasClass('glyphicon glyphicon-check')) {
        uncheck('#checkAssetsTB');
        uncheck('#checkAllTB');
    }
    else {
        check('#checkAssetsTB');
        if (validateChekAll()) {
            check('#checkAllTB');
        }
    }
});

$('#checkLiabilitiesTB').click(function () {
    if ($('#checkLiabilitiesTB').hasClass('glyphicon glyphicon-check')) {
        uncheck('#checkLiabilitiesTB');
        uncheck('#checkAllTB');
    }
    else {
        check('#checkLiabilitiesTB');
        if (validateChekAll()) {
            check('#checkAllTB');
        }
    }
});

$('#checkPatrimonyTB').click(function () {
    if ($('#checkPatrimonyTB').hasClass('glyphicon glyphicon-check')) {
        uncheck('#checkPatrimonyTB');
        uncheck('#checkAllTB');
    }
    else {
        check('#checkPatrimonyTB');
        if (validateChekAll()) {
            check('#checkAllTB');
        }
    }
});

$('#checkIncomeTB').click(function () {
    if ($('#checkIncomeTB').hasClass('glyphicon glyphicon-check')) {
        uncheck('#checkIncomeTB');
        uncheck('#checkAllTB');
    }
    else {
        check('#checkIncomeTB');
        if (validateChekAll()) {
            check('#checkAllTB');
        }
    }
});

$('#checkExpensesTB').click(function () {
    if ($('#checkExpensesTB').hasClass('glyphicon glyphicon-check')) {
        uncheck('#checkExpensesTB');
        uncheck('#checkAllTB');
    }
    else {
        check('#checkExpensesTB');
        if (validateChekAll()) {
            check('#checkAllTB');
        }
    }
});

$('#Detail').click(function () {
    reportName = Resources.TrialBalanceDetail;
    processTableRunningTBReport = undefined;
    clearInterval(timeTBReport);
    timeTBReport = window.setInterval(getMassiveReportTBReport, 6000);
    getMassiveReportTBReport();
});

$('#Total').click(function () {
    reportName = Resources.TrialBalanceTotal;
    processTableRunningTBReport = undefined;
    clearInterval(timeTBReport);
    timeTBReport = window.setInterval(getMassiveReportTBReport, 6000);
    getMassiveReportTBReport();
});


function getTotalRecordTB() {
    var paramterModel = SetTBReportFilter();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/GetTotalRecordsTrialBalance",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsTb').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertFormTB").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertFormTB").UifAlert('show', Resources.MessageInternalError, "danger");
            }

            unlockScreen();
        }
    });
}

function getMassiveReportTBReport() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportName;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningTBReport = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningTBReport != undefined) {
        if (validateProcessReport(processTableRunningTBReport, processTableRunningTBReport.length)) {
            clearInterval(timeTBReport);
            isRunningTBReport = false;
            $("#alertFormTB").UifAlert('hide');
        } else {
            isRunningTBReport = true;
        }
    }
};

function generateFileTB(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertFormTB").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportTBReport();
                    if (!isRunningTBReport) {
                        timeTBReport = window.setInterval(getMassiveReportTBReport, 6000);
                    }
                }
            } else {
                $("#alertFormTB").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkTB(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertFormTB", "selectFileTypePartial");
}


function check(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-unchecked");
    $(checkAccount).addClass("glyphicon glyphicon-check");
}

function uncheck(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-check");
    $(checkAccount).addClass("glyphicon glyphicon-unchecked");
}

function validateChekAll() {
    if ($('#checkAssetsTB').hasClass('glyphicon glyphicon-check') && $('#checkLiabilitiesTB').hasClass('glyphicon glyphicon-check') &&
        $('#checkPatrimonyTB').hasClass('glyphicon glyphicon-check') && $('#checkIncomeTB').hasClass('glyphicon glyphicon-check') &&
        $('#checkExpensesTB').hasClass('glyphicon glyphicon-check')) {
        return true;
    }
    else {
        return false
    }
}

function SetTBReportFilter() {
    return {
        Branch: $("#TBReportBranch").val(),
        Month: $("#TBReportMonthFrom").val(),
        Year: $("#TBReportYear").val(),
        MonthTo: $("#TBReportMonthTo").val(),
        All: ($('#checkAllTB').hasClass('glyphicon glyphicon-check')) ? -1 : 0,
        Assets: ($('#checkAssetsTB').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagAssets").val() : 0,
        Liabilities: ($('#checkLiabilitiesTB').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagLiabilities").val() : 0,
        Patrimony: ($('#checkPatrimonyTB').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagPatrimony").val() : 0,
        Income: ($('#checkIncomeTB').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagIncome").val() : 0,
        Expenses: ($('#checkExpensesTB').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagExpenses").val() : 0,
        Operation: $('input:radio[name=options]:checked').val() == 'D' ? 0 : 1
    }
}
