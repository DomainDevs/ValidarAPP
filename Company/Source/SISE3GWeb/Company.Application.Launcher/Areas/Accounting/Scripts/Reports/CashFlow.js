var timeReportCf = window.setInterval(getMassiveReportCF, 6000);
var isRunningCashFlow;
var processTableRunninReportCf;
var ReportTypeId = 0;

getMassiveReportCF();


$("#CFReportGenerate").on("click", function () {
    $("#alertCf").UifAlert('hide');

    $("#CFReportForm").validate();
    if ($("#CFReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            getTotalRecordCF();
        }, 300);
    }
});

$("#CFReportCancel").on("click", function () {
    $('#CFReportYear').val("");
    $('#CFReportMonthFrom').val("");
    $('#CFReportMonthTo').val("");
    $("#alertCf").UifAlert('hide');
});

$("#CFReportModal").on("click", function () {
    $('#modalMassiveTotRecordsCF').modal('hide');

    processTableRunninReportCf = undefined;
    clearInterval(timeReportCf);

    var paramterModel = SetModelReportCf();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/BalanceSheetReports",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportCF();

                if (!isRunningCashFlow) {
                    timeReportCf = window.setInterval(getMassiveReportCF, 6000);
                }
            } else {
                $("#alertCf").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#CFReportMonthFrom").on('itemSelected', function (event, selectedItem) {
    $("#alertCf").UifAlert('hide');
    if ($("#CFReportMonthTo").val() != '') {
        if (parseInt($("#CFReportMonthFrom").val()) != parseInt($("#CFReportMonthTo").val())) {
            if (parseInt($("#CFReportMonthFrom").val()) > parseInt($("#CFReportMonthTo").val())) {
                $("#alertCf").UifAlert('show', Resources.MessageValidateMonthFrom, "warning");
                $("#CFReportMonthFrom").val('');
            }
        }
    }
});

$("#CFReportMonthTo").on('itemSelected', function (event, selectedItem) {
    $("#alertCf").UifAlert('hide');
    if ($("#CFReportMonthFrom").val() != '') {
        if (parseInt($("#CFReportMonthTo").val()) != parseInt($("#CFReportMonthFrom").val())) {
            if (parseInt($("#CFReportMonthTo").val()) < parseInt($("#CFReportMonthFrom").val())) {
                $("#alertCf").UifAlert('show', Resources.MessageValidateMonthTo, "warning");
                $("#CFReportMonthTo").val('');
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertCf").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunninReportCf = undefined;
            clearInterval(timeReportCf);

            generateFileCF(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkCF(data.UrlFile);
    } else {
        $("#alertCf").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

$('#checkAccumulatedCF').click(function () {
    $("#alertCf").UifAlert('hide');
    if ($('#checkAccumulatedCF').hasClass('glyphicon glyphicon-check')) {
        uncheckedCf('#checkAccumulatedCF');
    }
    else {
        checkCf('#checkAccumulatedCF');
    }
});

function getTotalRecordCF() {
    var paramterModel = SetModelReportCf();
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
                    $('#modalMassiveTotRecordsCF').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertCf").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertCf").UifAlert('show', Resources.MessageInternalError, "danger");
            }

            unlockScreen();
        }
    });
}

function getMassiveReportCF() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.CashFlow.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunninReportCf = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunninReportCf != undefined) {
        if (validateProcessReport(processTableRunninReportCf, processTableRunninReportCf.length)) {
            clearInterval(timeReportCf);
            isRunningCashFlow = false;
            $("#alertCf").UifAlert('hide');
        } else {
            isRunningCashFlow = true;
        }
    }
};

function generateFileCF(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertCf").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportCF();
                    if (!isRunningCashFlow) {
                        timeReportCf = window.setInterval(getMassiveReportCF, 6000);
                    }
                }
            } else {
                $("#alertCf").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkCF(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertCf", "selectFileTypePartial");
}

function checkCf(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-unchecked");
    $(checkAccount).addClass("glyphicon glyphicon-check");
}

function uncheckedCf(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-check");
    $(checkAccount).addClass("glyphicon glyphicon-unchecked");
}

function SetModelReportCf() {
    return {
        Branch: $("#CFReportBranch").val(),
        Month: $("#CFReportMonthFrom").val(),
        Year: $("#CFReportYear").val(),
        MonthTo: $("#CFReportMonthTo").val(),
        Assets: $("#ViewBagAssets").val(),
        Liabilities: $("#ViewBagLiabilities").val(),
        Patrimony: 0,
        Income: $("#ViewBagIncome").val(),
        Expenses: $("#ViewBagExpenses").val(),
        Accumulated: ($('#checkAccumulatedCF').hasClass('glyphicon glyphicon-check')) ? 1 : 0,
        Name: Resources.CashFlow
    }
}
