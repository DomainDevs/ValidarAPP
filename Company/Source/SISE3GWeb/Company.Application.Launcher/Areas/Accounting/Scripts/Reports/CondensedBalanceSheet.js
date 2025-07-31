var timeReportCobs = window.setInterval(getMassiveReportCobs, 6000);
var isRunningCobs;
var processTableRunninReportCobs;
var ReportTypeId = 0;

getMassiveReportCobs();

$("#COBSReportGenerate").on("click", function () {
    $("#alertCobs").UifAlert('hide');

    $("#COBSReportForm").validate();
    if ($("#COBSReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            getTotalRecordCobs();
        }, 300);
    }
});

$("#COBSReportCancel").on("click", function () {
    $('#COBSReportYear').val("");
    $('#COBSReportMonth').val("");
    $('#COBSReportBranch').val("");
    $("#alertCobs").UifAlert('hide');
});

$("#COBSReportModal").on("click", function () {
    $('#modalMassiveTotRecordsCOBS').modal('hide');

    processTableRunninReportCobs = undefined;
    clearInterval(timeReportCobs);

    $.ajax({
        url: ACC_ROOT + "Reports/CondensedBalanceSheetReports",
        data: {"month": $('#COBSReportMonth').val(), "year": $("#COBSReportYear").val(), "brancCd": $("#COBSReportBranch").val()},
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportCobs();

                if (!isRunningCobs) {
                    timeReportCobs = window.setInterval(getMassiveReportCobs, 6000);
                }
            } else {
                $("#alertCobs").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertCobs").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunninReportCobs = undefined;
            clearInterval(timeReportCobs);

            generateFileCobs(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkCobs(data.UrlFile);
    } else {
        $("#alertCobs").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

//FUNCIONES//
function getTotalRecordCobs() {
    
    $.ajax({
        url: ACC_ROOT + "Reports/GetTotalRecordsCondensedBalanceSheet",
        data: {"month": $('#COBSReportMonth').val(), "year": $("#COBSReportYear").val(), "brancCd": $("#COBSReportBranch").val()},
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsCOBS').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertCobs").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertCobs").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

function getMassiveReportCobs() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.CondensedBalanceSheet.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunninReportCobs = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunninReportCobs != undefined) {
        if (validateProcessReport(processTableRunninReportCobs, processTableRunninReportCobs.length)) {
            clearInterval(timeReportCobs);
            isRunningCobs = false;
            $("#alertCobs").UifAlert('hide');
        } else {
            isRunningCobs = true;
        }
    }
};

function generateFileCobs(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertCobs").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportCobs();
                    if (!isRunningCobs) {
                        timeReportCobs = window.setInterval(getMassiveReportCobs, 6000);
                    }
                }
            } else {
                $("#alertCobs").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkCobs(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertCobs", "selectFileTypePartial");
}