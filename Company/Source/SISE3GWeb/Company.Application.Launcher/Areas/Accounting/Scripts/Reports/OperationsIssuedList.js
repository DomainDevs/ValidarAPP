/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
var status;
var reportNameOperationsIssued = Resources.OperationsIssued;
var typeProcessOperationsIssued = 2116;
var processTableRunning;
var isRunningOperationsIssued = false;
var timeOperationsIssued = window.setInterval(getMassiveReportOperationsIssued, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#Branch").attr("disabled", "disabled");
    }, 300);

}
else {
    $("#Branch").removeAttr("disabled");
}

getMassiveReportOperationsIssued();

// Controla que la fecha final sea mayor a la inicial
$('#StartDateOperationsIssued').blur(function () {
    if ($("#EndDateOperationsIssued").val() != "") {
        if (compare_dates($('#StartDateOperationsIssued').val(), $("#EndDateOperationsIssued").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateOperationsIssued").val('');
        } else {
            $("#StartDateOperationsIssued").val($('#StartDateOperationsIssued').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateOperationsIssued').blur(function () {
    if ($("#StartDateOperationsIssued").val() != "") {
        if (compare_dates($("#StartDateOperationsIssued").val(), $('#EndDateOperationsIssued').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateOperationsIssued").val('');
        } else {
            $("#EndDateOperationsIssued").val($('#EndDateOperationsIssued').val());
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

$("#MassiveReportOperationIssued").click(function () {
    $("#alertForm").UifAlert('hide');

    $("#ReportFormOperationsIssued").validate();
    if ($("#ReportFormOperationsIssued").valid()) {
        if ($("#StartDateOperationsIssued").val() != "" && $("#EndDateOperationsIssued").val() != "") {

            if (IsDate($("#StartDateOperationsIssued").val()) == false || IsDate($("#EndDateOperationsIssued").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateOperationsIssued").val(), $("#EndDateOperationsIssued").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsOperationsIssued();
                    }, 300);
                }
            }
        }
    }
});


//SI del modal
$("#btnModalOperationsIssued").click(function () {

    $('#modalMassiveTotRecords').modal('hide');


    if ($('#BranchOperationsIssued').val() != "") {
        branchId = $('#BranchOperationsIssued').val();
    }

    status = $('input:radio[name=options]:checked').val();
    
    $.ajax({
        url: ACC_ROOT + "Reports/OperationsIssuedReports",
        data: {
            "dateFrom": $("#StartDateOperationsIssued").val(), "dateTo": $("#EndDateOperationsIssued").val(),
            "branchId": branchId, "Operation": status,
            "reportType": typeProcessOperationsIssued
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportOperationsIssued();

                if (!isRunningOperationsIssued) {
                    timeOperationsIssued = window.setInterval(getMassiveReportOperationsIssued, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelOperationsIssued').click(function () {
    $('#BranchOperationsIssued').val("");
    $('#StartDateOperationsIssued').val("");
    $('#EndDateOperationsIssued').val("");
    $("#alertForm").UifAlert('hide');
    $("#selectFileTypePartial").val("");
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/
function getTotalRecordsOperationsIssued() {
    totRecords = 0;
    recordsProcessed = 0;


    var branchId = -1;
    var status = $('input:radio[name=options]:checked').val();


    if ($('#BranchOperationsIssued').val() != "") {
        branchId = $('#BranchOperationsIssued').val();
    }


    $.ajax({
        //async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsOperationsIssued",
        data: {
            "dateFrom": $("#StartDateOperationsIssued").val(), "dateTo": $("#EndDateOperationsIssued").val(),
            "branchId": branchId, "operation": status
        },
        success: function (data) {
            if (data.success) {
                totRecords = data.result.records;
                msj = Resources.MsgMassiveTotRecords + ': ' + totRecords + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (totRecords > 0) {
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
};

//Valida la fila seleccionada(lapiz edit)
function whenSelectRow(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");
    var status = $('input:radio[name=options]:checked').val();

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
          
            generateFileReport(data.ProcessId, data.RecordsNumber, data.Description);
                      
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkOperationsIssued(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateOperationsIssued").val("");
    $("#EndDateOperationsIssued").val("");
}; //ReportFormOperationsIssued

function getMassiveReportOperationsIssued() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameOperationsIssued;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeOperationsIssued);
            isRunningOperationsIssued = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningOperationsIssued = true;
        }
    }
};

function SetDownloadLinkOperationsIssued(fileName) {
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

                    getMassiveReportOperationsIssued();
                    if (!isRunningOperationsIssued) {
                        timeOperationsIssued = window.setInterval(getMassiveReportOperationsIssued, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
};
