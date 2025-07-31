var timeCPLReport = window.setInterval(getMassiveReportCPLReport, 6000);
var isRunningCPLReport;
var processTableRunningCPLReport;

getMassiveReportCPLReport();


$("#CPLReportGenerate").on("click", function () {
    $("#alertCPLForm").UifAlert('hide');

    $("#CPLReportForm").validate();
    if ($("#CPLReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            getTotalRecordCPL();
        }, 300);
    }
});

$("#CPLReportClean").on("click", function () {
    $('#CPLReportCurrency').val("");
    $('#CPLReportBranch').val("");
    $('#CPLReportPrefix').val("");
    $('#CPLReportDateTo').val("");
    $("#alertCPLForm").UifAlert('hide');
});

$("#CPLReportModal").on("click", function () {
    $('#modalMassiveTotRecords').modal('hide');

    processTableRunningCPLReport = undefined;
    clearInterval(timeCPLReport);

    $.ajax({
        url: ACC_ROOT + "Reports/CurrentPoliciesReports",
        data: {
            "branchId": $("#CPLReportBranch").val(), "prefixId": $("#CPLReportPrefix").val(),
            "dateTo": $("#CPLReportDateTo").val(), "currencyId": $("#CPLReportCurrency").val()
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportCPLReport();

                if (!isRunningCPLReport) {
                    timeCPLReport = window.setInterval(getMassiveReportCPLReport, 6000);
                }
            } else {
                $("#alertCPLForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertCPLForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningCPLReport = undefined;
            clearInterval(timeCPLReport);

            generateFileCPL(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkCPL(data.UrlFile);
    } else {
        $("#alertCPLForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

function getTotalRecordCPL() {
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsCurrentPolicies",
        data: {
            "branchId": $("#CPLReportBranch").val(), "prefixId": $("#CPLReportPrefix").val(),
            "dateTo": $("#CPLReportDateTo").val(), "currencyId": $("#CPLReportCurrency").val()
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertCPLForm").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertCPLForm").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

function getMassiveReportCPLReport() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.CurrentPolicies;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningCPLReport = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningCPLReport != undefined) {
        if (validateProcessReport(processTableRunningCPLReport, processTableRunningCPLReport.length)) {
            clearInterval(timeCPLReport);
            isRunningCPLReport = false;
            $("#alertCPLForm").UifAlert('hide');
        } else {
            isRunningCPLReport = true;
        }
    }
};

function generateFileCPL(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertCPLForm").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportCPLReport();
                    if (!isRunningCPLReport) {
                        timeCPLReport = window.setInterval(getMassiveReportCPLReport, 6000);
                    }
                }
            } else {
                $("#alertCPLForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkCPL(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertCPLForm", "selectFileTypePartial");
}