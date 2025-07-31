/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
var timePBBLReport = window.setInterval(getMassiveReportPBBLReport, 6000);
var isRunningPBBLReport
var processTableRunningPBBLReport;

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  DEFINICIÓN DE CLASE                                                     */
/*--------------------------------------------------------------------------------------------------------------------------*/

getMassiveReportPBBLReport();


$("#PBBLReportDateFrom").blur(function () {
    $("#alertPBBLR").UifAlert('hide');
    if ($("#PBBLReportDateFrom").val() != '') {
        if (IsDate($("#PBBLReportDateFrom").val()) == true) {
            if ($("#PBBLReportDateTo").val() != '') {
                if (CompareDates($("#PBBLReportDateFrom").val(), $("#PBBLReportDateTo").val())) {
                    $("#PBBLReportDateFrom").val(getCurrentDate);
                }
            }
        } else {
            $("#alertPBBLR").UifAlert('show', Resources.InvalidDates, "warning");
            $("#PBBLReportDateFrom").val("");
        }
    }
});

$("#PBBLReportDateTo").blur(function () {
    $("#alertPBBLR").UifAlert('hide');
    if ($("#PBBLReportDateTo").val() != '') {
        if (IsDate($("#PBBLReportDateTo").val()) == true) {
            if ($("#PBBLReportDateFrom").val() != '') {
                if (CompareDates($("#PBBLReportDateFrom").val(), $("#PBBLReportDateTo").val())) {
                    $("#PBBLReportDateTo").val(getCurrentDate);
                }
            }
        } else {
            $("#alertPBBLR").UifAlert('show', Resources.InvalidDates, "warning");
            $("#PBBLReportDateTo").val("");
        }
    }
});

$("#PBBLReportGenerate").on("click", function () {
    $("#alertPBBLR").UifAlert('hide');

    $("#ReportFormatPBBLR").validate();
    if ($("#ReportFormatPBBLR").valid()) {

        lockScreen();

        setTimeout(function () {
            getTotalRecordPortfolioByBroker();
        }, 300);
    }
});

$("#PBBLReportClean").on("click", function () {
    $('#PBBLReportDateFrom').val("");
    $('#PBBLReportDateTo').val("");
    $("#alertPBBLR").UifAlert('hide');
});

$("#PBBLReportModal").on("click", function () {
    $('#modalMassiveTotRecords').modal('hide');

    processTableRunningPBBLReport = undefined;
    clearInterval(timePBBLReport);

    $.ajax({
        url: ACC_ROOT + "Reports/PortfolioByBroker",
        data: { "dateFrom": $("#PBBLReportDateFrom").val(), "dateTo": $("#PBBLReportDateTo").val() },
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportPBBLReport();

                if (!isRunningPBBLReport) {
                    timePBBLReport = window.setInterval(getMassiveReportPBBLReport, 6000);
                }
            } else {
                $("#alertPBBLR").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertPBBLR").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningPBBLReport = undefined;
            clearInterval(timePBBLReport);

            generateFilePortfolioByBroker(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkPortfolioByBroker(data.UrlFile);
    } else {
        $("#alertPBBLR").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});


function getTotalRecordPortfolioByBroker() {
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalPortfolioByBroker",
        data: {
            "dateFrom": $("#PBBLReportDateFrom").val(), "dateTo": $("#PBBLReportDateTo").val()
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertPBBLR").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertPBBLR").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

function getMassiveReportPBBLReport() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.PortfolioByBroker.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningPBBLReport = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningPBBLReport != undefined) {
        if (validateProcessReport(processTableRunningPBBLReport, processTableRunningPBBLReport.length)) {
            clearInterval(timePBBLReport);
            isRunningPBBLReport = false;
            $("#alertPBBLR").UifAlert('hide');
        } else {
            isRunningPBBLReport = true;
        }
    }
};

function generateFilePortfolioByBroker(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertPBBLR").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportPBBLReport();
                    if (!isRunningPBBLReport) {
                        timePBBLReport = window.setInterval(getMassiveReportPBBLReport, 6000);
                    }
                }
            } else {
                $("#alertPBBLR").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkPortfolioByBroker(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertPBBLR", "selectFileTypePartial");
}