/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var billingDate;
var billingDateSplit;
var reportNameCRI = Resources.CancellationRecord.toUpperCase();
var typeProcessCRI = 2235;
var processTableRunning;
var isRunning = false;
var timeCRI = window.setInterval(getMassiveReportCRI, 6000);
/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  DEFINICIÓN DE CLASE                                                     */
/*--------------------------------------------------------------------------------------------------------------------------*/

getMassiveReportCRI();

$("#MassiveCRI").on("click", function () {
    
    $("#alertForm").UifAlert('hide');

    $("#CRIReportForm").validate();
    if ($("#CRIReportForm").valid()) {
        lockScreen();
        setTimeout(function () {
            getTotalRecordCRI();
        }, 400);

    }
});

$("#CRIReportCancel").on("click", function () {
    $('#CRIReportYear').val("");
    $('#CRIReportMonth').val("");
    $('#CRIReportDay').val("");
    $("#alertForm").UifAlert('hide');
});

//Botón (SI) del modal de confirmación
$("#btnModalCRI").on("click", function () {
    $('#modalMassiveTotRecords').modal('hide');

    processTableRunning = undefined;
    getMassiveReportCRI();

    $.ajax({
        url: ACL_ROOT + "MassiveReports/CancellationRecordIssuanceReports",
        data: {
            "year": $("#CRIReportYear").val(), "month": $("#CRIReportMonth").val(),
            "day": $("#CRIReportDay").val()
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportCRI();

                if (!isRunning) {
                    timeCRI = window.setInterval(getMassiveReportCRI, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#CRIReportMonth").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    var systemDate = new Date();
    var year = systemDate.getFullYear();

    if ($("#CRIReportYear").val() != "") {
        year = $("#CRIReportYear").val();
    }
    if (selectedItem.Id > 0) {
        $("#CRIReportDay").attr("disabled", false);
    }
    else {
        $("#CRIReportDay").attr("disabled", true);
        $("#CRIReportDay").val("");
    }
    if ($("#CRIReportMonth").val() != "") {
        if ($("#CRIReportDay").val() != "") {
            var month = parseInt($("#CRIReportMonth").val());

            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
                if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 31) {
                    $("#alertForm").UifAlert('show', Resources.MessageJanuaryValidate, "warning");
                    $("#CRIReportDay").val("");
                }
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11) {
                if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 30) {
                    $("#alertForm").UifAlert('show', Resources.MessageAprilValidate, "warning");
                    $("#CRIReportDay").val("");
                }
            }
            else if (month == 2) {
                if ((((year % 100) != 0) && ((year % 4) == 0)) || ((year % 400) == 0)) {
                    if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 29) {
                        $("#alertForm").UifAlert('show', Resources.MessageFebraryLeapValidate, "warning");
                        $("#CRIReportDay").val("");
                    }
                }
                else {
                    if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 28) {
                        $("#alertForm").UifAlert('show', Resources.MessageFebraryValidate, "warning");
                        $("#CRIReportDay").val("");
                    }
                }
            }
        }
    }
});

$("#CRIReportDay").blur(function () {
    var systemDate = new Date();
    var year = systemDate.getFullYear();

    $("#alertForm").UifAlert('hide');

    if ($("#CRIReportYear").val() != "") {
        year = $("#CRIReportYear").val();
    }


    if ($("#CRIReportDay").val() != "") {
        var month = parseInt($("#CRIReportMonth").val());

        if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
            if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 31) {
                $("#alertForm").UifAlert('show', Resources.MessageJanuaryValidate, "warning");
                $("#CRIReportDay").val("");
            }
        }
        else if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 30) {
                $("#alertForm").UifAlert('show', Resources.MessageAprilValidate, "warning");
                $("#CRIReportDay").val("");
            }
        }
        else if (month == 2) {
            if ((((year % 100) != 0) && ((year % 4) == 0)) || ((year % 400) == 0)) {
                if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 29) {
                    $("#alertForm").UifAlert('show', Resources.MessageFebraryLeapValidate, "warning");
                    $("#CRIReportDay").val("");
                }
            }
            else {
                if (parseInt($("#CRIReportDay").val()) < 1 || parseInt($("#CRIReportDay").val()) > 28) {
                    $("#alertForm").UifAlert('show', Resources.MessageFebraryValidate, "warning");
                    $("#CRIReportDay").val("");
                }
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunning = undefined;
            clearInterval(timeCRI);

            generateFileCRI(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkCRI(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

function getTotalRecordCRI() {
    $.ajax({
        async: false,
        url: ACL_ROOT + "MassiveReports/GetTotalRecordsCancellationRecordIssuance",
        data: {
            "year": $("#CRIReportYear").val(), "month": $("#CRIReportMonth").val(),
            "day": $("#CRIReportDay").val()
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertForm").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertForm").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

//Refresca o detiene la grilla de avance de proceso
function getMassiveReportCRI() {
    var controller = ACL_ROOT + "MassiveReports/GetReportProcess?reportName=" + reportNameCRI;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 2000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeCRI);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }
};

//Genera el archivo una vez terminado el proceso inicial
function generateFileCRI(processId, records, reportDescription) {
    $.ajax({
        url: ACL_ROOT + "MassiveReports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertForm").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportCRI();
                    if (!isRunning) {
                        timeCRI = window.setInterval(getMassiveReportCRI, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkCRI(fileName) {
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        window.open(url + fileName);
    }
    else {
        window.open(ACC_ROOT + fileName);
    }
}

