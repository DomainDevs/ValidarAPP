/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameReceiptsAppliedList = Resources.ReceiptsApplied.toUpperCase();
var typeProcessReceiptsApplied = 2106;
var processTableRunningReceiptsApplied;
var isRunningReceiptsApplied = false;
var timeReceiptsApplied = window.setInterval(getMassiveReportReceiptApplied, 6000);
var isApplied = -1;


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchReceiptsApplied").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchReceiptsApplied").removeAttr("disabled");
}

getMassiveReportReceiptApplied();

$('#CancelReceiptAppliedList').click(function () {
    $('#BranchReceiptsApplied').val("");
    $('#StartDateReceiptsApplied').val("");
    $('#EndDateReceiptsApplied').val("");
    $("#selectFileTypePartial").val("");
    $("#alertForm").UifAlert('hide');
});

// Controla que la fecha final sea mayor a la inicial
$('#StartDateReceiptsApplied').blur(function() {
    if ($("#EndDateReceiptsApplied").val() != "") {
        if (compare_dates($('#StartDateReceiptsApplied').val(), $("#EndDateReceiptsApplied").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateReceiptsApplied").val('');
        } else {
            $("#StartDateReceiptsApplied").val($('#StartDateReceiptsApplied').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateReceiptsApplied').blur(function() {
    if ($("#StartDateReceiptsApplied").val() != "") {
        if (compare_dates($("#StartDateReceiptsApplied").val(), $('#EndDateReceiptsApplied').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateReceiptsApplied").val('');
        } else {
            $("#EndDateReceiptsApplied").val($('#EndDateReceiptsApplied').val());
        }
    }
});

$("#MassiveReportReceiptAppliedList").click(function () {

    $("#alertForm").UifAlert('hide');
    $("#ReportReceiptsAppliedListForm").validate();

    if ($("#ReportReceiptsAppliedListForm").valid()) {
        if ($("#StartDateReceiptsApplied").val() != "" && $("#EndDateReceiptsApplied").val() != "") {

            if (IsDate($("#StartDateReceiptsApplied").val()) == false || IsDate($("#EndDateReceiptsApplied").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateReceiptsApplied").val(), $("#EndDateReceiptsApplied").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveReceiptAppliedList();
                    }, 300);
                }
            }
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

$("#btnModalReceiptsAppliedList").click(function () {

    $('#modalMassiveTotRecords').modal('hide');

    if ($('#BranchReceiptsApplied').val() != "") {
        branchId = $('#BranchReceiptsApplied').val();
    }

    $.ajax({
        url: ACC_ROOT + "Reports/ReceiptsAppliedListReports",
        data: {
            "dateFrom": $("#StartDateReceiptsApplied").val(), "dateTo": $("#EndDateReceiptsApplied").val(),
            "branchId": branchId, "isApplied": isApplied, "reportType": typeProcessReceiptsApplied
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportReceiptApplied();

                if (!isRunningReceiptsApplied) {
                    timeReceiptsApplied = window.setInterval(getMassiveReportReceiptApplied, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/


function getMassiveReportReceiptApplied() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameReceiptsAppliedList;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReceiptsApplied = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReceiptsApplied != undefined) {
        if (validateProcessReport(processTableRunningReceiptsApplied, processTableRunningReceiptsApplied.length)) {
            clearInterval(timeReceiptsApplied);
            isRunningReceiptsApplied = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningReceiptsApplied = true;
        }
    }
};


//Valida la fila seleccionada(lapiz edit)
function whenSelectRow(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReport(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkReceiptsApplied(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateReceiptsApplied").val("");
    $("#EndDateReceiptsApplied").val("");
}


//Genera el archivo una vez terminado el proceso inicial
function generateFileReport(processId, records, reportDescription) {

    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
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
                    getMassiveReportReceiptApplied();
                    if (!isRunningReceiptsApplied) {
                        timeReceiptsApplied = window.setInterval(getMassiveReportReceiptApplied, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function getTotalRecordsMassiveReceiptAppliedList() {

    if ($('#BranchReceiptsApplied').val() != "") {
        branchId = $('#BranchReceiptsApplied').val();
    }

    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsReceiptsAppliedList",
        data: {
            "dateFrom": $("#StartDateReceiptsApplied").val(), "dateTo": $("#EndDateReceiptsApplied").val(),
            "branchId": branchId, "isApplied": isApplied
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

function SetDownloadLinkReceiptsApplied(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertForm", "selectFileTypePartial");
}