var timePRDLReport = window.setInterval(getMassiveReportPRDLReport, 6000);
var isRunningPRDLReport;
var processTableRunningPRDLReport;

getMassiveReportPRDLReport();

$("#PRDLReportGenerate").on("click", function () {
    $("#alertFormPRDL").UifAlert('hide');

    $("#PRDLReportForm").validate();
    if ($("#PRDLReportForm").valid()) {
        lockScreen();
        setTimeout(function () {
            getTotalRecordPRDL();
        }, 400);
    }
});

$("#PRDLReportClean").on("click", function () {
    $('#PRDLReportYear').val("");
    $('#PRDLReportMonth').val("");
    $('#PRDLReportDay').val("");
    $("#alertFormPRDL").UifAlert('hide');
});

$("#PRDLReportModal").on("click", function () {
    $('#modalMassiveTotRecordsPrdl').modal('hide');

    processTableRunningPRDLReport = undefined;
    getMassiveReportPRDLReport();

    $.ajax({
        url: ACL_ROOT + "MassiveReports/ProductionDetailReports",
        data: {
            "year": $("#PRDLReportYear").val(), "month": $("#PRDLReportMonth").val(),
            "day": $("#PRDLReportDay").val()
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportPRDLReport();

                if (!isRunningPRDLReport) {
                    timePRDLReport = window.setInterval(getMassiveReportPRDLReport, 6000);
                }
            } else {
                $("#alertFormPRDL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#PRDLReportMonth").on('itemSelected', function (event, selectedItem) {
    $("#alertFormPRDL").UifAlert('hide');
    var systemDate = new Date();
    var year = systemDate.getFullYear();

    if ($("#PRDLReportYear").val() != "") {
        year = $("#PRDLReportYear").val();
    }
    if (selectedItem.Id > 0) {
        $("#PRDLReportDay").attr("disabled", false);
    }
    else {
        $("#PRDLReportDay").attr("disabled", true);
        $("#PRDLReportDay").val("");
    }
        if ($("#PRDLReportMonth").val() != "") {
            if ($("#PRDLReportDay").val() != "") {
                var month = parseInt($("#PRDLReportMonth").val());

                if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
                    if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 31) {
                        $("#alertFormPRDL").UifAlert('show', Resources.MessageJanuaryValidate, "warning");
                        $("#PRDLReportDay").val("");
                    }
                }
                else if (month == 4 || month == 6 || month == 9 || month == 11) {
                    if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 30) {
                        $("#alertFormPRDL").UifAlert('show', Resources.MessageAprilValidate, "warning");
                        $("#PRDLReportDay").val("");
                    }
                }
                else if (month == 2) {
                    if ((((year % 100) != 0) && ((year % 4) == 0)) || ((year % 400) == 0)) {
                        if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 29) {
                            $("#alertFormPRDL").UifAlert('show', Resources.MessageFebraryLeapValidate, "warning");
                            $("#PRDLReportDay").val("");
                        }
                    }
                    else {
                        if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 28) {
                            $("#alertFormPRDL").UifAlert('show', Resources.MessageFebraryValidate, "warning");
                            $("#PRDLReportDay").val("");
                        }
                    }
                }
            }
        }
    });

$("#PRDLReportDay").blur(function () {
    var systemDate = new Date();
    var year = systemDate.getFullYear();

    $("#alertFormPRDL").UifAlert('hide');

    if ($("#PRDLReportYear").val() != "") {
        year = $("#PRDLReportYear").val();
    }


    if ($("#PRDLReportDay").val() != "") {
        var month = parseInt($("#PRDLReportMonth").val());

        if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
            if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 31) {
                $("#alertFormPRDL").UifAlert('show', Resources.MessageJanuaryValidate, "warning");
                $("#PRDLReportDay").val("");
            }
        }
        else if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 30) {
                $("#alertFormPRDL").UifAlert('show', Resources.MessageAprilValidate, "warning");
                $("#PRDLReportDay").val("");
            }
        }
        else if (month == 2) {
            if ((((year % 100) != 0) && ((year % 4) == 0)) || ((year % 400) == 0)) {
                if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 29) {
                    $("#alertFormPRDL").UifAlert('show', Resources.MessageFebraryLeapValidate, "warning");
                    $("#PRDLReportDay").val("");
                }
            }
            else {
                if (parseInt($("#PRDLReportDay").val()) < 1 || parseInt($("#PRDLReportDay").val()) > 28) {
                    $("#alertFormPRDL").UifAlert('show', Resources.MessageFebraryValidate, "warning");
                    $("#PRDLReportDay").val("");
                }
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertFormPRDL").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningPRDLReport = undefined;
            clearInterval(timePRDLReport);

            generateFilePRDL(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkPRDL(data.UrlFile);
    } else {
        $("#alertFormPRDL").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

function getTotalRecordPRDL() {
    $.ajax({
        async: false,
        url: ACL_ROOT + "MassiveReports/GetTotalRecordsProductionDetail",
        data: {
            "year": $("#PRDLReportYear").val(), "month": $("#PRDLReportMonth").val(),
            "day": $("#PRDLReportDay").val()
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsPrdl').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertFormPRDL").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertFormPRDL").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

function getMassiveReportPRDLReport() {
    var controller = ACL_ROOT + "MassiveReports/GetReportProcess?reportName=" + Resources.ProductionDetailList.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningPRDLReport = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 2000);

    if (processTableRunningPRDLReport != undefined) {
        if (validateProcessReport(processTableRunningPRDLReport, processTableRunningPRDLReport.length)) {
            clearInterval(timePRDLReport);
            isRunningPRDLReport = false;
            $("#alertFormPRDL").UifAlert('hide');
        } else {
            isRunningPRDLReport = true;
        }
    }
};

function generateFilePRDL(processId, records, reportDescription) {
    $.ajax({
        url: ACL_ROOT + "MassiveReports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertFormPRDL").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportPRDLReport();
                    if (!isRunningPRDLReport) {
                        timePRDLReport = window.setInterval(getMassiveReportPRDLReport, 6000);
                    }
                }
            } else {
                $("#alertFormPRDL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkPRDL(fileName) {
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        window.open(url + fileName);
    }
    else {
        window.open(ACC_ROOT + fileName);
    }
}